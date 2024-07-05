using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;

namespace Sabanishi.SdiAssignment
{
    public class Interpreter:IScopable
    {
        private Subject<string> _outputSubject;
        public IObservable<string> OutputObservable => _outputSubject;
        
        public void Setup(CancellationToken token)
        {
            _outputSubject = new Subject<string>();
        }
        
        public void Cleanup()
        {
            _outputSubject.Dispose();
        }
        
        /// <summary>
        /// 命令を解釈、実行し、出力結果を返す
        /// </summary>
        public async UniTask Interpret(string text)
        {
            //TODO: 命令の解釈、実行処理
            await UniTask.Delay(TimeSpan.FromSeconds(1));
            _outputSubject.OnNext(text + "ぽよ");
        }
    }
}