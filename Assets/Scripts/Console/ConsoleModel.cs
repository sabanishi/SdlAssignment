using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;

namespace Sabanishi.SdiAssignment
{
    public class ConsoleModel:IScopable
    {
        private ReactiveProperty<bool> _isOpen;
        private ReactiveProperty<bool> _isAcceptingInput;

        private readonly Interpreter _interpreter;
        public readonly HistoryModel HistoryModel;
        public readonly InputModel InputModel;
        
        public IReadOnlyReactiveProperty<bool> IsOpen => _isOpen;
        public IReadOnlyReactiveProperty<bool> IsAcceptingInput => _isAcceptingInput;
        
        public ConsoleModel()
        {
            _interpreter = new Interpreter();
            HistoryModel = new HistoryModel();
            InputModel = new InputModel();
            
            Outputter.Boot(_interpreter);
        }

        public void Setup(CancellationToken token)
        {
            _interpreter.Setup(token);
            HistoryModel.Setup(token);
            InputModel.Setup(token);
            _isOpen = new ReactiveProperty<bool>();
            _isAcceptingInput = new ReactiveProperty<bool>(true);

            InputModel.SendInputTextObservable.Subscribe(ReceiveInput).AddTo(token);
            _interpreter.OutputObservable.Subscribe(x => AddHistoryElement(x, false)).AddTo(token);
        }
        
        public void Cleanup()
        {
            _interpreter.Cleanup();
            HistoryModel.Cleanup();
            InputModel.Cleanup();
            _isOpen.Dispose();
            _isAcceptingInput.Dispose();
        }

        public void Open()
        {
            _isOpen.Value = true;
        }
        
        public void Close()
        {
            _isOpen.Value = false;
        }

        /// <summary>
        /// InputFieldからの入力を受け取った時の処理
        /// </summary>
        private void ReceiveInput(string input)
        {
            UniTask.Void(async () =>
            {
                if (!_isAcceptingInput.Value) return;
                
                //入力を履歴に追加
                AddHistoryElement(input,true);
                
                _isAcceptingInput.Value = false;
                await _interpreter.Interpret(input);
                _isAcceptingInput.Value = true;
            });
        }
        
        private void AddHistoryElement(string text,bool isInput)
        {
            HistoryModel.AddElement(text,isInput);
        }
    }
}