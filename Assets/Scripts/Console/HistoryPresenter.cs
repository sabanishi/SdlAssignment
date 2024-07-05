using System.Threading;

namespace Sabanishi.SdiAssignment
{
    public class HistoryPresenter:IScopable
    {
        private readonly HistoryModel _model;
        private readonly HistoryView _view;
        
        public HistoryPresenter(HistoryModel model, HistoryView view)
        {
            _model = model;
            _view = view;
        }
        
        public void Setup(CancellationToken token)
        {
            
        }

        public void Cleanup()
        {
        }
    }
}