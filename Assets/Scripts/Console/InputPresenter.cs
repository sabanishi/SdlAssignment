using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;

namespace Sabanishi.SdiAssignment
{
    public class InputPresenter:IScopable
    {
        private readonly InputModel _model;
        private readonly InputView _view;
        
        public InputPresenter(InputModel model, InputView view)
        {
            _model = model;
            _view = view;
        }
        
        public void Setup(CancellationToken token)
        {
            _view.Setup(token);
            
            _model.LineCount.Subscribe(x => _view.SetInputLine(x)).AddTo(token);
            _model.ResetInputObservable.Subscribe(_ => _view.SetText("")).AddTo(token);
            
            _view.OnInputValueChanged.Subscribe(_model.OnValueChanged).AddTo(token);
            _view.OnInputValueChanged.Subscribe(x=>_view.SetSendButtonActive(!string.IsNullOrEmpty(x))).AddTo(token);
            _view.OnSendButtonClicked.Subscribe(_=>_model.OnButtonClicked()).AddTo(token);
        }

        public void Cleanup()
        {
            _view.Cleanup();
        }
    }
}