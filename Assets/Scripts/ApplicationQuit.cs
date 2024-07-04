using UnityEngine;

namespace Sabanishi.SdiAssignment
{
    public class ApplicationQuit:MonoBehaviour
    {
        public void OnPointerDown()
        {
            Application.Quit();
        }
    }
}