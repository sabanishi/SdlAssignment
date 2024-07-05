using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace Sabanishi.SdiAssignment
{
    public class ConsoleModel:IScopable
    {
        private ReactiveProperty<bool> _isOpen;
        public IReadOnlyReactiveProperty<bool> IsOpen => _isOpen;

        private readonly Interpreter _interpreter;
        public readonly HistoryModel HistoryModel;
        public readonly InputModel InputModel;
        
        public ConsoleModel()
        {
            _interpreter = new Interpreter();
            HistoryModel = new HistoryModel();
            InputModel = new InputModel();
        }

        public void Setup(CancellationToken token)
        {
            HistoryModel.Setup(token);
            InputModel.Setup(token);
            _isOpen = new ReactiveProperty<bool>();

            InputModel.SendInputTextObservable.Subscribe(ReceiveInput).AddTo(token);
        }
        
        public void Cleanup()
        {
            HistoryModel.Cleanup();
            InputModel.Cleanup();
            _isOpen.Dispose();
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
            var output = _interpreter.Interpret(input);
            Debug.Log($"input: {input}, output: {output}");
        }
    }
}