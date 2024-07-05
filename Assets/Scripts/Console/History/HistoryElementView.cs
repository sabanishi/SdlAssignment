using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sabanishi.SdiAssignment
{
    public class HistoryElementView:MonoBehaviour
    {
        [SerializeField] private RectTransform root;
        [SerializeField] private RectTransform content;
        [SerializeField] private TMP_Text text;
        [SerializeField] private RectTransform textTransform;
        [SerializeField] private ContentSizeFitter textFitter;
        [SerializeField] private Image backGround;
        [SerializeField] private Color inputColor;
        [SerializeField] private Color outputColor;
        
        public void SetText(string value)
        {
            text.text = value;
            
            //行数を計算
            var lineTexts = value.Split('\n');
            var lineCount = lineTexts.Length;
            
            //高さを設定する
            var height = 18 + 46 * lineCount;
            root.sizeDelta = new Vector2(root.sizeDelta.x, height);
            
            //contentのwidthを決める
            textFitter.SetLayoutHorizontal();
            var textWidth = textTransform.sizeDelta.x;
            content.sizeDelta = new Vector2(20 + textWidth, content.sizeDelta.y);
            content.offsetMin = new Vector2(content.offsetMin.x, 0);
            content.offsetMax = new Vector2(content.offsetMax.x, 0);
        }

        public void SetIsLeft(bool isLeft)
        {
            if (isLeft)
            {
                content.anchorMin = new Vector2(0, 0);
                content.anchorMax = new Vector2(0, 1);
                content.pivot = new Vector2(0, 0.5f);
                content.anchoredPosition = new Vector2(25, 0);
                
                backGround.color = inputColor;
            }
            else
            {
                content.anchorMin = new Vector2(1, 0);
                content.anchorMax = new Vector2(1, 1);
                content.pivot = new Vector2(1, 0.5f);
                content.anchoredPosition = new Vector2(-25, 0);
                
                backGround.color = outputColor;
            }
        }
    }
}