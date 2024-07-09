using System;
using System.Threading;
using Live2D.Cubism.Framework.Raycasting;
using UniRx;
using UnityEngine;

namespace Sabanishi.SdiAssignment
{
    public class CharacterUi:MonoBehaviour,IScopable
    {
        [SerializeField] private Camera camera;
        [SerializeField] private CubismRaycaster cubismRaycaster;
        [SerializeField] private Transform modelRoot;

        private Subject<Unit> _openMenuSubject;

        public IObservable<Unit> OpenMenuObservable => _openMenuSubject;

        public void Setup(CancellationToken token)
        {
            _openMenuSubject = new Subject<Unit>();
        }

        public void Cleanup()
        {
            _openMenuSubject.Dispose();
        }

        private void Update()
        {
            if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
            {
                var ray = camera.ScreenPointToRay(Input.mousePosition);
                var results = new CubismRaycastHit[4];
                var hitCount = cubismRaycaster.Raycast(ray, results);
                if (hitCount <= 0) return;
                if (Input.GetMouseButtonDown(1))
                {
                    _openMenuSubject?.OnNext(Unit.Default);
                }

                if (Input.GetMouseButton(0))
                {
                    //モデルの位置をマウスの位置に合わせる
                    var mousePosition = Input.mousePosition;
                    var worldPosition = camera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 10));
                    modelRoot.position = worldPosition;
                }
            }
        }
    }
}