using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Sabanishi.SdiAssignment
{
    public class WindowUi:MonoBehaviour
    {
        [SerializeField] private GameObject root;
        [SerializeField] private Button closeButton;

        private ReactiveProperty<bool> _isOpen;
        public IReadOnlyReactiveProperty<bool> IsOpen => _isOpen;
        
        public void Setup(CancellationToken token)
        {
            _isOpen = new ReactiveProperty<bool>();
            
            closeButton.OnClickAsObservable().Subscribe(_=>SetActive(false)).AddTo(token);
            
            SetActive(false);
        }

        public void Cleanup()
        {
            _isOpen.Dispose();
        }

        public void SetActive(bool isActive)
        {
            _isOpen.Value = isActive;
            root.SetActive(isActive);
        }
    }
}