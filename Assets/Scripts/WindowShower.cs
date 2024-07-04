using UnityEngine;

namespace Sabanishi.SdiAssignment
{
    public class WindowShower:MonoBehaviour
    {
        [SerializeField] private GameObject window;
        public void OnPointerDown()
        {
            window.SetActive(true);
        }
    }
}