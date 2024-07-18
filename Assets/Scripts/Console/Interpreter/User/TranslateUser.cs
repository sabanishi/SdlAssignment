using Cysharp.Threading.Tasks;
using UniLang;

namespace Sabanishi.SdiAssignment
{
    [Command(name:"!trans",description:"Translate Sentences")]
    public class TranslateUser:IRunnable
    {
        [Option(names:new[]{"-s","--sentence"},description:"Target Sentence")]
        public static string sentence;
        
        [Option(names:new[]{"-i","--input"},description:"Input Language")]
        public static string input="";
        
        [Option(names:new[]{"-o","--output"},description:"Output Language")]
        public static string output="";
        
        public async UniTask<bool> Run()
        {
            //翻訳の実装
            var inputLang = input.Equals("") ? Language.Auto : input;
            var outputLang = output.Equals("") ? Language.Japanese : output;
            
            var translator = Translator.Create(inputLang, outputLang);
            var isComplete = false;
            translator.Run(sentence, results =>
            {
                foreach (var result in results)
                {
                    Outputter.Instance.Output(result.translated);
                }
                isComplete = true;
            });
            
            await UniTask.WaitUntil(() => isComplete);
            return true;
        }
    }
}