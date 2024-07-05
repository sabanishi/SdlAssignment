using System;
using System.Linq;
using System.Text.RegularExpressions;
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
            //改行コードを削除する
            var command = text.Replace("\n", "");
            
            //空白で分割する
            var args = Regex.Matches(command, @"("".*?""|[^ ""]+)+")
                .Select(x => x.Groups[0].Value)
                .ToArray();
            foreach (var arg in args)
            {
                _outputSubject.OnNext(arg);
            }
        }
    }
}