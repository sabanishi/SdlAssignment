using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Unit = UniRx.Unit;

namespace Sabanishi.SdiAssignment
{
    public class InputView:MonoBehaviour,IScopable
    {
        [SerializeField] private RectTransform root;
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private Button sendButton;
        [SerializeField] private GameObject explainShiftKeyLabel;

        public IObservable<string> OnInputValueChanged => inputField.onValueChanged.AsObservable();
        public IObservable<Unit> OnSendButtonClicked => sendButton.OnClickAsObservable();

        private const int LineLimit = 8;

        public void Setup(CancellationToken token)
        {
            //文字列の行数が制限に達していた時、末尾の改行を無くす
            OnInputValueChanged.Subscribe(x =>
            {
                if (x.Split("\n").Length > LineLimit)
                {
                    SetText(x.Substring(0, x.Length - 1));
                }
            });
            
            //Enterキーで改行、Shift+Enterで送信
            inputField.onEndEdit.AsObservable().Subscribe(x =>
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        sendButton.onClick.Invoke();
                    }
                    else
                    {
                        UniTask.Void(async () =>
                        {
                            inputField.ActivateInputField();
                            await UniTask.Yield();
                            inputField.text += "\n";
                            inputField.MoveTextEnd(false);
                        });
                    }
                }
            }).AddTo(token);
        }

        private void Update()
        {
            //explainShiftKeyLabel.SetActive(Input.GetKey(KeyCode.LeftShift));
        }

        public void Cleanup()
        {
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