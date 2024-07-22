using System;
using UniRx;
using UnityEngine;

namespace Sabanishi.SdiAssignment
{
    public class CharacterMover:MonoBehaviour
    {
        [SerializeField] private Transform modelRoot;
        [SerializeField] private Camera camera;
        [SerializeField] private AnimatorController animatorController;
        
        private ReactiveProperty<bool> _isDragging;
        public IObservable<bool> IsDraggingObserver => _isDragging;
        
        private Vector3 _catchCharacterPos;
        private Vector3 _catchMousePos;

        public void Setup()
        {
            _isDragging = new ReactiveProperty<bool>(false);
            IsDraggingObserver.Where(x => !x).Subscribe(x =>
            {
                animatorController.SetIsDragging(false);
                CharacterBoringController.Instance.ResetCounter();
            }).AddTo(gameObject);

            //_isDraggingが0.2秒以上続けてtrueになった時の処理
            IsDraggingObserver.Throttle(TimeSpan.FromSeconds(0.2f)).Where(x => x).Subscribe(x =>
            {
                animatorController.SetIsDragging(true);
                CharacterBoringController.Instance.ResetCounter();
            }).AddTo(gameObject);

        }

        public void Cleanup()
        {
            _isDragging.Dispose();
        }
        
        public void SetIsDragging(bool isDragging)
        {
            var catchDrag = _isDragging.Value;
            _isDragging.Value = isDragging;
            if (!catchDrag && isDragging)
            {
                _catchCharacterPos = modelRoot.position;
                _catchMousePos = Input.mousePosition;
            }
        }

        private void Update()
        {
            if (_isDragging == null) return;
            
            if (_isDragging.Value)
            {
                var mousePos = Input.mousePosition;
                var diff = mousePos - _catchMousePos;
                var worldDiff = camera.ScreenToWorldPoint(diff) - camera.ScreenToWorldPoint(Vector3.zero);
                modelRoot.position = _catchCharacterPos + worldDiff;
            }
        }
    }
}