using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;

namespace Sabanishi.SdiAssignment
{
    public class ConsolePresenter:IScopable
    {   
        private readonly ConsoleModel _model;
        private readonly ConsoleView _view;
        
        private readonly InputPresenter _inputPresenter;
        private readonly HistoryPresenter _historyPresenter;
        
        public ConsolePresenter(ConsoleModel model, ConsoleView view)
        {
            _model = model;
            _view = view;
            
            _inputPresenter = new InputPresenter(_model.InputModel,_view.InputView);
            _historyPresenter = new HistoryPresenter(_model.HistoryModel,_view.HistoryView);
        }
        
        public void Setup(CancellationToken token)
        {
            _inputPresenter.Setup(token);
            _historyPresenter.Setup(token);
            
            _model.IsOpen.Where(x => x).Subscribe(_ => _view.Open()).AddTo(token);
            _model.IsOpen.Where(x => !x).Subscribe(_ => _view.Close()).AddTo(token);
            _model.IsAcceptingInput.Subscribe(_view.InputView.SetSendButtonInteractable);

            _view.CloseButtonObservable.Subscribe(_ => _model.Close()).AddTo(token);
        }

        public void Cleanup()
        {
            _inputPresenter.Cleanup();
            _historyPresenter.Cleanup();
        }
    }
}