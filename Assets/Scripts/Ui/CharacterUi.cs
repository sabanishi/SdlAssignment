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
        [SerializeField] private CharacterMover characterMover;

        private Subject<Unit> _openMenuSubject;

        public IObservable<Unit> OpenMenuObservable => _openMenuSubject;

        public void Setup(CancellationToken token)
        {
            _openMenuSubject = new Subject<Unit>();
            characterMover.Setup();
        }

        public void Cleanup()
        {
            _openMenuSubject.Dispose();
            characterMover.Cleanup();
        }

        private void Update()
        {
            if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
            {
                var ray = camera.ScreenPointToRay(Input.mousePosition);
                var results = new CubismRaycastHit[4];
                var hitCount = cubismRaycaster.Raycast(ray, results);
                if (hitCount > 0)
                {
                    if (Input.GetMouseButtonDown(1))
                    {
                        _openMenuSubject?.OnNext(Unit.Default);
                    }
                    characterMover.SetIsDragging(true);
                }
            }
            else
            {
                characterMover.SetIsDragging(false);
            }
        }
    }
}