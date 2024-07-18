using System;
using UniRx;

namespace Sabanishi.SdiAssignment
{
    public class SaveSubject
    {
        private static SaveSubject _instance;
        public static SaveSubject Instance => _instance;

        private Subject<Type> _saveSubject;
        public IObservable<Type> SaveObservable => _saveSubject;

        public static void Boot()
        {
            if(_instance != null) return;
            _instance = new SaveSubject();
            _instance._saveSubject = new Subject<Type>();
        }
        
        public static void Dispose()
        {
            _instance._saveSubject.Dispose();
            _instance = null;
        }
        
        public void OnNext<T>()
        {
            _saveSubject.OnNext(typeof(T));
        }
    }
}