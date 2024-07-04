using UnityEngine;

namespace Sabanishi.SdiAssignment
{
    public class WindowHider:MonoBehaviour
    {
        [SerializeField] private GameObject window;
        
        public void OnPointerDown()
        {
            window.SetActive(false);
        }
    }
}