using System.Threading;
using UniRx;

namespace Sabanishi.SdiAssignment
{
    public class HistoryModel:IScopable
    {
        private ReactiveCollection<HistoryElementModel> _elements;
        
        public IReadOnlyReactiveCollection<HistoryElementModel> Elements => _elements;
        
        public void Setup(CancellationToken token)
        {
            _elements = new ReactiveCollection<HistoryElementModel>();
        }

        public void Cleanup()
        {
            _elements.Clear();
            _elements.Dispose();
        }
        
        public void AddElement(string text,bool isInput)
        {
            _elements.Add(new HistoryElementModel(text,isInput));
        }
    }
}