using System;
using UnityEngine;

namespace Sabanishi.SdiAssignment
{
    public class Pointer:MonoBehaviour
    {
        [SerializeField] private GameObject target;
        
        public void OnPointerDown()
        {
            //右クリックの時のみ
            if (Input.GetMouseButton(1))
            {
                //targetの位置をマウスの位置にする
                target.transform.position = Input.mousePosition;
                target.SetActive(true);
            }
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (!hasFocus)
            {
                target.SetActive(false);
            }
        }
    }
}