using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace Sabanishi.SdiAssignment
{
    public class UiManager:MonoBehaviour,IScopable
    {
        [SerializeField] private CharacterUi characterUi;
        [SerializeField] private MenuUi menuUi;
        [SerializeField] private ConsoleView consoleView;

        private ConsoleModel _consoleModel;
        private ConsolePresenter _consolePresenter;

        public void Setup(CancellationToken token)
        {
            characterUi.Setup(token);
            menuUi.Setup(token);

            _consoleModel = new ConsoleModel();
            _consoleModel.Setup(token);
            _consolePresenter = new ConsolePresenter(_consoleModel,consoleView);
            _consolePresenter.Setup(token);
            
            characterUi.OpenMenuObservable.Subscribe(_=>menuUi.SetActive(true)).AddTo(token);
            menuUi.SetConsoleActiveObservable.Where(x=>x).Subscribe(_=>_consoleModel.Open()).AddTo(token);
            menuUi.SetConsoleActiveObservable.Where(x=>!x).Subscribe(_=>_consoleModel.Close()).AddTo(token);
            _consoleModel.IsOpen.Subscribe(menuUi.SetIsOpenConsole).AddTo(token);
        }
        
        public void Cleanup()
        {
            characterUi.Cleanup();
            menuUi.Cleanup();
            _consoleModel.Cleanup();
            _consolePresenter.Cleanup();
        }
    }
}