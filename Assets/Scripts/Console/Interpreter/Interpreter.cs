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
        private ChatUser _chatUser;
        
        private Subject<string> _outputSubject;
        public IObservable<string> OutputObservable => _outputSubject;
        
        public void Setup(CancellationToken token)
        {
            _chatUser = new ChatUser();
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
            CharacterBoringController.Instance.ResetCounter();
            
            //改行コードを削除する
            var command = text.Replace("\n", "");
            
            //空白で分割する
            var args = Regex.Matches(command, @"("".*?""|[^ ""]+)+")
                .Select(x => x.Groups[0].Value)
                .ToArray();
            
            //コマンドを解析する
            var argMap = new ArgumentMap(args);
            
            //MainArgsの第一要素を取得
            var mainArgs = argMap.GetMainArgs();
            if (mainArgs.Count > 0)
            {
                var mainCommand = mainArgs[0];
                //mainCommandが「！」で始まる場合、Executorを起動する
                if (mainCommand.StartsWith("!"))
                {
                    var executor = new CommandExecutor();
                    var canExecute = await executor.Execute(argMap);
                    if (canExecute) return;
                    
                    await UniTask.Delay(500);
                    Output("命令の形式が間違ってるみたい...");
                    return;
                }
            }
            
            //おしゃべり機能を起動する
            await _chatUser.TryRequest(text);
        }
        
        public void Output(string message)
        {
            _outputSubject.OnNext(message);
        }
    }
}