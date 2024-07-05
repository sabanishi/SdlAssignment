using System.Threading;

namespace Sabanishi.SdiAssignment
{
    public interface IScopable
    {
        public void Setup(CancellationToken token);

        public void Cleanup();
    }
}