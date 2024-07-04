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
        
        public IObservable<Unit> CloseButtonObservable => closeButton.OnClickAsObservable();

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