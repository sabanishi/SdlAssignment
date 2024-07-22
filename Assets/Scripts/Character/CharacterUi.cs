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
        [SerializeField] private NadeNadeController nadeNadeController;

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
            var ray = camera.ScreenPointToRay(Input.mousePosition);
            var results = new CubismRaycastHit[4];
            var hitCount = 0;
            if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
            {
                hitCount = cubismRaycaster.Raycast(ray, results);
            }
            
            characterMover.UpdateRaycaster(hitCount,results);
            
            if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
            {
                if (hitCount > 0)
                {
                    if (Input.GetMouseButtonDown(1))
                    {
                        _openMenuSubject?.OnNext(Unit.Default);
                    }
                }
            }
        }
    }
}