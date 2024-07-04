using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;

namespace Sabanishi.SdiAssignment
{
    public class ConsolePresenter
    {   
        private ConsoleModel _model;
        private ConsoleView _view;
        
        public ConsolePresenter(ConsoleModel model, ConsoleView view)
        {
            _model = model;
            _view = view;
        }
        
        public void Setup(CancellationToken token)
        {
            _model.IsOpen.Where(x => x).Subscribe(_ => _view.Open()).AddTo(token);
            _model.IsOpen.Where(x => !x).Subscribe(_ => _view.Close()).AddTo(token);

            _view.CloseButtonObservable.Subscribe(_ => _model.Close()).AddTo(token);
        }

        public void Cleanup()
        {
        }
    }
}