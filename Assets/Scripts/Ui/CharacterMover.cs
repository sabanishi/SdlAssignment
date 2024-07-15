using UnityEngine;

namespace Sabanishi.SdiAssignment
{
    public class CharacterMover:MonoBehaviour
    {
        [SerializeField] private Transform modelRoot;
        [SerializeField] private Camera camera;
        
        private bool _isDragging;
        private Vector3 _catchCharacterPos;
        private Vector3 _catchMousePos;
        public void SetIsDragging(bool isDragging)
        {
            var catchDrag = _isDragging;
            _isDragging = isDragging;
            if (!catchDrag && isDragging)
            {
                _catchCharacterPos = modelRoot.position;
                _catchMousePos = Input.mousePosition;
            }
        }

        private void Update()
        {
            if (_isDragging)
            {
                var mousePos = Input.mousePosition;
                var diff = mousePos - _catchMousePos;
                var worldDiff = camera.ScreenToWorldPoint(diff) - camera.ScreenToWorldPoint(Vector3.zero);
                modelRoot.position = _catchCharacterPos + worldDiff;
            }
        }
    }
}