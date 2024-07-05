using System;
using System.Threading;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Sabanishi.SdiAssignment
{
    public class CharacterUi:MonoBehaviour,IScopable
    {
        [SerializeField] private ObservableEventTrigger playerEvent;

        private Subject<Unit> _openMenuSubject;

        public IObservable<Unit> OpenMenuObservable => _openMenuSubject;

        public void Setup(CancellationToken token)
        {
            _openMenuSubject = new Subject<Unit>();

            playerEvent.OnPointerDownAsObservable().Subscribe(_ =>
            {
                //右クリックしていた時のみSubjectを発火させる
                if (Input.GetMouseButton(1))
                {
                    _openMenuSubject.OnNext(Unit.Default);
                }
            }).AddTo(gameObject);
        }

        public void Cleanup()
        {
            _openMenuSubject.Dispose();
        }
    }
}