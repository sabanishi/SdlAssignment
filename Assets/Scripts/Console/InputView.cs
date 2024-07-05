using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Unit = UniRx.Unit;

namespace Sabanishi.SdiAssignment
{
    public class InputView:MonoBehaviour,IScopable
    {
        [SerializeField] private RectTransform root;
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private Button sendButton;

        public IObservable<string> OnInputValueChanged => inputField.onValueChanged.AsObservable();
        public IObservable<Unit> OnSendButtonClicked => sendButton.OnClickAsObservable();

        private const int LineLimit = 8;

        public void Setup(CancellationToken token)
        {
            OnInputValueChanged.Subscribe(x =>
            {
                //文字列の行数が制限に達していた時、末尾の改行を無くす
                if (x.Split("\n").Length > LineLimit)
                {
                    SetText(x.Substring(0, x.Length - 1));
                }
            });
        }

        public void Cleanup()
        {
        }

        private void Update()
        {
            if (EventSystem.current.currentSelectedGameObject == inputField.gameObject)
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        //改行する
                        UniTask.Void(async () =>
                        {
                            inputField.ActivateInputField();
                            await UniTask.Yield();
                            inputField.text += "\n";
                            inputField.MoveTextEnd(false);
                        });
                    }
                    else
                    {
                        sendButton.onClick.Invoke();
                    }
                }
            }
        }
        
        public void SetSendButtonActive(bool isActive)
        {
            sendButton.gameObject.SetActive(isActive);
        }
        
        public void SetSendButtonInteractable(bool isInteractable)
        {
            sendButton.interactable = isInteractable;
        }

        public void SetInputLine(int line)
        {
            var realLine = Math.Clamp(line, 1, inputField.lineLimit);
            var height = 26 + 46 * realLine;
            root.sizeDelta = new Vector2(root.sizeDelta.x, height);
        }

        public void SetText(string text)
        {
            inputField.text = text;
        }
    }
}