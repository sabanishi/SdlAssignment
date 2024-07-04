using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sabanishi.SdiAssignment
{
    public class UiManager:MonoBehaviour
    {
        [SerializeField] private CharacterUi characterUi;
        [SerializeField] private MenuUi menuUi;
        [SerializeField] private WindowUi windowUi;

        public void Setup(CancellationToken token)
        {
            characterUi.Setup(token);
            menuUi.Setup(token);
            windowUi.Setup(token);
            
            characterUi.OpenMenuObservable.Subscribe(_=>menuUi.SetActive(true)).AddTo(token);
            menuUi.SetWindowActiveObservable.Subscribe(windowUi.SetActive).AddTo(token);
            windowUi.IsOpen.Subscribe(menuUi.SetIsOpenWindow).AddTo(token);
        }
        
        public void Cleanup()
        {
            characterUi.Cleanup();
            menuUi.Cleanup();
            windowUi.Cleanup();
        }
    }
}