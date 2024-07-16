using Cysharp.Threading.Tasks;

namespace Sabanishi.SdiAssignment
{
    [Command(name:"!setup_chat",description:"Setup ChatGPT Api")]
    public class SetupApiUser:IRunnable
    {
        [Option(names:new[]{"-k","--key"},description:"Api Key")]
        public static string apiKey;
        
        public async UniTask<bool> Run()
        {
            if (apiKey.Equals(default))
            {
                Outputter.Instance.Output("APIキーを入力してください！");
                return false;
            }

            var saveData = new ApiData()
            {
                apiKey = apiKey
            };

            SaveUtils.TrySave<ApiData>(saveData);
            
            //0.3秒待機
            await UniTask.Delay(500);
            Outputter.Instance.Output("ChatGPTの設定ができました！");
            await UniTask.Delay(300);
            Outputter.Instance.Output("これで一緒におしゃべりできますね！");
            
            return true;
        }
    }
}