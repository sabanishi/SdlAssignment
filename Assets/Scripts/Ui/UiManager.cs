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
        [SerializeField] private ConsoleUi consoleUi;

        public void Setup(CancellationToken token)
        {
            characterUi.Setup(token);
            menuUi.Setup(token);
            consoleUi.Setup(token);
            
            characterUi.OpenMenuObservable.Subscribe(_=>menuUi.SetActive(true)).AddTo(token);
            menuUi.SetConsoleActiveObservable.Subscribe(consoleUi.SetActive).AddTo(token);
            consoleUi.IsOpen.Subscribe(menuUi.SetIsOpenConsole).AddTo(token);
        }
        
        public void Cleanup()
        {
            characterUi.Cleanup();
            menuUi.Cleanup();
            consoleUi.Cleanup();
        }
    }
}