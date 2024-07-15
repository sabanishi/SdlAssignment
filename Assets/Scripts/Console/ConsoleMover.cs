using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Sabanishi.SdiAssignment
{
    public class ConsoleMover : MonoBehaviour, IDragHandler, IBeginDragHandler
    {
        [SerializeField] private Transform root;

        private Vector3 _catchPos;
        private Vector2 _catchMousePos;


        // ドラッグ開始時の処理
        public void OnBeginDrag(PointerEventData eventData)
        {
            _catchPos = root.position;
            _catchMousePos = eventData.position;
        }

        // ドラッグ中の処理
        public void OnDrag(PointerEventData eventData)
        {
            var mousePos = eventData.position;
            var diff = mousePos - _catchMousePos;
            root.position = _catchPos + new Vector3(diff.x, diff.y, 0);
        }
    }
}