
using Cysharp.Threading.Tasks;
using UnityEngine;
using Screen = UnityEngine.Device.Screen;

namespace Sabanishi.SdiAssignment
{
    [Command(name:"!console_size",description:"Update Console Size")]
    public class ConsoleSizeUser:IRunnable
    {
        [Option(names:new[]{"-w","--width"},description:"Width Size")]
        public string width = "";
        
        [Option(names:new[]{"-h","--height"},description:"Height Size")]
        public string height = "";
        
        public async UniTask<bool> Run()
        {
            await UniTask.Delay(500);
            
            int widthInt = 1160;
            int heightInt = 1180;

            if (!SaveUtils.TryLoad<ConsoleSizeData>(out var data))
            {
                data = new ConsoleSizeData();
                var screenSize = new Vector2(Screen.width, Screen.height);
                data.width = (int)screenSize.x/2;
                data.height = (int)screenSize.y/2;
            }
            
            if(!int.TryParse(width,out widthInt))
            {
                widthInt = data.width;
            }
            
            if(!int.TryParse(height,out heightInt))
            {
                heightInt = data.height;
            }
            
            ConsoleSizeUpdater.Instance.UpdateSize(widthInt, heightInt);
            Outputter.Instance.Output("コンソールのサイズを変更しました！");
            Outputter.Instance.Output($"横幅は{widthInt}縦幅は{heightInt}だよ");
            return true;
        }
    }
}