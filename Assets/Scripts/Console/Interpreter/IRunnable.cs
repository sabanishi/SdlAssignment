using Cysharp.Threading.Tasks;

namespace Sabanishi.SdiAssignment
{
    public interface IRunnable
    {
        public UniTask<bool> Run();
    }
}