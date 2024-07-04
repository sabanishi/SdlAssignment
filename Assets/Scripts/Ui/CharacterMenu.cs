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
    public class CharacterMenu:MonoBehaviour
    {
        [SerializeField] private RectTransform root;

        [SerializeField] private Button openWindowButton;
        [SerializeField] private Button closeWindowButton;
        [SerializeField] private Button quitEvent;

        private Subject<bool> _setWindowActive;
        public IObservable<bool> SetWindowActiveObservable => _setWindowActive;

        public void Setup(CancellationToken token)
        {
            _setWindowActive = new Subject<bool>();
            
            openWindowButton.OnClickAsObservable().Subscribe(_=>OpenWindow()).AddTo(token);
            closeWindowButton.OnClickAsObservable().Subscribe(_=>CloseWindow()).AddTo(token);
            quitEvent.OnPointerDownAsObservable().Subscribe(_=>QuitApp()).AddTo(token);
            
            SetActive(false);
            SetIsOpenWindow(false);
        }

        public void Cleanup()
        {
            _setWindowActive.Dispose();
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

        public void SetIsOpenWindow(bool isOpen)
        {
            if (isOpen)
            {
                openWindowButton.gameObject.SetActive(false);
                closeWindowButton.gameObject.SetActive(true);
            }
            else
            {
                openWindowButton.gameObject.SetActive(true);
                closeWindowButton.gameObject.SetActive(false);
            }
        }

        private void OpenWindow()
        {
            _setWindowActive.OnNext(true);
            SetActive(false);
        }

        private void CloseWindow()
        {
            _setWindowActive.OnNext(false);
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