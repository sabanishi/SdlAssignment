using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Sabanishi.SdiAssignment
{
    public class HistoryView:MonoBehaviour
    {
        [SerializeField] private HistoryElementView elementPrefab;
        [SerializeField] private RectTransform elementParent;
        [SerializeField] private ScrollRect scrollRect;
        
        public HistoryElementView CreateElement()
        {
            var element =Instantiate(elementPrefab, elementParent);
            //scrollRectを下までスクロール
            UniTask.Void(async () =>
            {
                await UniTask.Yield();
                scrollRect.verticalNormalizedPosition = 0;
            });
            return element;
        }
    }
}