using System.Threading;

namespace Sabanishi.SdiAssignment
{
    public class HistoryElementModel:IScopable
    {
        public string Text { get; }

        public bool IsInput { get; }

        public HistoryElementModel(string text, bool isInput)
        {
            Text = text;
            IsInput = isInput;
        }
        
        public void Setup(CancellationToken token)
        {
        }

        public void Cleanup()
        {
        }
    }
}