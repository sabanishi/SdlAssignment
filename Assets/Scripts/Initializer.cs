using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Sabanishi.SdiAssignment
{
    public class Initializer:MonoBehaviour
    {
        [SerializeField] private UiManager uiManager;
        
        private void Start()
        {
            uiManager.Setup(this.GetCancellationTokenOnDestroy());
        }

        private void OnDestroy()
        {
            uiManager.Cleanup();
        }
    }
}