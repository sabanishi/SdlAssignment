using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace Sabanishi.SdiAssignment
{
    public class UiManager:MonoBehaviour
    {
        [SerializeField] private CharacterUi characterUi;
        [SerializeField] private CharacterMenu characterMenu;
        [SerializeField] private WindowUi windowUi;

        public void Setup(CancellationToken token)
        {
            characterUi.Setup(token);
            characterMenu.Setup(token);
            windowUi.Setup(token);
            
            characterUi.OpenMenuObservable.Subscribe(_=>characterMenu.SetActive(true)).AddTo(token);
            characterMenu.SetWindowActiveObservable.Subscribe(windowUi.SetActive).AddTo(token);
            windowUi.IsOpen.Subscribe(characterMenu.SetIsOpenWindow).AddTo(token);
        }
        
        public void Cleanup()
        {
            characterUi.Cleanup();
            characterMenu.Cleanup();
            windowUi.Cleanup();
        }
    }
}