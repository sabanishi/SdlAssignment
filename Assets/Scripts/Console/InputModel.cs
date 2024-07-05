using System;
using System.Threading;
using UniRx;

namespace Sabanishi.SdiAssignment
{
    public class InputModel:IScopable
    {
        private string _inputValue;
        private ReactiveProperty<int> _lineCount;
        private Subject<Unit> _resetInputSubject;
        private Subject<string> _sendInputTextSubject;
        
        public IReadOnlyReactiveProperty<int> LineCount => _lineCount;
        public IObservable<Unit> ResetInputObservable => _resetInputSubject;
        public IObservable<string> SendInputTextObservable => _sendInputTextSubject;
        
        public void Setup(CancellationToken token)
        {
            _lineCount = new ReactiveProperty<int>();
            _resetInputSubject = new Subject<Unit>();
            _sendInputTextSubject = new Subject<string>();
        }

        public void Cleanup()
        {
            _lineCount.Dispose();
            _resetInputSubject.Dispose();
            _sendInputTextSubject.Dispose();
        }
        
        public void OnValueChanged(string value)
        {
            _inputValue = value;
            _lineCount.Value = value.Split('\n').Length;
        }

        public void OnButtonClicked()
        {
            _sendInputTextSubject.OnNext(_inputValue);
            _resetInputSubject.OnNext(Unit.Default);
        }
    }
}