using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Sabanishi.SdiAssignment
{
    public class MenuUi:MonoBehaviour
    {
        [SerializeField] private RectTransform root;

        [SerializeField] private Button openConsoleButton;
        [SerializeField] private Button closeConsoleButton;
        [SerializeField] private Button quitEvent;

        private Subject<bool> _setConsoleActive;
        public IObservable<bool> SetConsoleActiveObservable => _setConsoleActive;

        public void Setup(CancellationToken token)
        {
            _setConsoleActive = new Subject<bool>();
            
            openConsoleButton.OnClickAsObservable().Subscribe(_=>OpenConsole()).AddTo(token);
            closeConsoleButton.OnClickAsObservable().Subscribe(_=>CloseConsole()).AddTo(token);
            quitEvent.OnPointerDownAsObservable().Subscribe(_=>QuitApp()).AddTo(token);
            
            SetActive(false);
            SetIsOpenConsole(false);
        }

        public void Cleanup()
        {
            _setConsoleActive.Dispose();
        }

        public void SetActive(bool isActive)
        {
            root.gameObject.SetActive(isActive);

            if (isActive)
            {
                //マウスの位置に自身を移動させる
                var mousePosition = Input.mousePosition;
                root.position = new Vector3(mousePosition.x, mousePosition.y, 0);
            }
        }

        public void SetIsOpenConsole(bool isOpen)
        {
            if (isOpen)
            {
                openConsoleButton.gameObject.SetActive(false);
                closeConsoleButton.gameObject.SetActive(true);
            }
            else
            {
                openConsoleButton.gameObject.SetActive(true);
                closeConsoleButton.gameObject.SetActive(false);
            }
        }

        private void OpenConsole()
        {
            _setConsoleActive.OnNext(true);
            SetActive(false);
        }

        private void CloseConsole()
        {
            _setConsoleActive.OnNext(false);
            SetActive(false);
        }

        private void QuitApp()
        {
            Application.Quit();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                //自身の子オブジェクト以外がクリックされた時、自身を閉じる
                var selectedObject = EventSystem.current.currentSelectedGameObject;
                if (selectedObject == null)
                {
                    SetActive(false);
                    return;
                }

                if (!selectedObject.transform.IsChildOf(transform))
                {
                    SetActive(false);
                }
            }
        }
        
        private void OnApplicationFocus(bool hasFocus)
        {
            //他のウィンドウに照準が映った時に自身を閉じる
            if (!hasFocus)
            {
                SetActive(false);
            }
        }
    }
}