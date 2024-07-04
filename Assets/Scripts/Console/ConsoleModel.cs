using System.Threading;
using UniRx;

namespace Sabanishi.SdiAssignment
{
    public class ConsoleModel
    {
        private ReactiveProperty<bool> _isOpen;
        public IReadOnlyReactiveProperty<bool> IsOpen => _isOpen;

        public void Setup(CancellationToken token)
        {
            _isOpen = new ReactiveProperty<bool>();
        }
        
        public void Cleanup()
        {
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
    }
}