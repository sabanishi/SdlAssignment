using System.Threading;

namespace Sabanishi.SdiAssignment
{
    public class HistoryElementPresenter:IScopable
    {
        private readonly HistoryElementModel _model;
        private readonly HistoryElementView _view;
        
        public HistoryElementView View => _view;
        
        public HistoryElementPresenter(HistoryElementModel model, HistoryElementView view)
        {
            _model = model;
            _view = view;
        }
        
        public void Setup(CancellationToken token)
        {
            _view.SetIsLeft(_model.IsInput);
            _view.SetText(_model.Text);
        }

        public void Cleanup()
        {
        }
    }
}