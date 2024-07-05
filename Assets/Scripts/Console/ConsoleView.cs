using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Sabanishi.SdiAssignment
{
    public class ConsoleView:MonoBehaviour
    {
        [SerializeField] private GameObject root;
        [SerializeField] private Button closeButton;

        [SerializeField] private InputView inputView;
        [SerializeField] private HistoryView historyView;
        
        public IObservable<Unit> CloseButtonObservable => closeButton.OnClickAsObservable();
        
        public InputView InputView => inputView;
        public HistoryView HistoryView => historyView;

        public void Open()
        {
            root.SetActive(true);
        }

        public void Close()
        {
            root.SetActive(false);
        }
    }
}