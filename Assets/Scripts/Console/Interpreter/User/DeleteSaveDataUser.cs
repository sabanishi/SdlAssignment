using Cysharp.Threading.Tasks;

namespace Sabanishi.SdiAssignment
{
    /// <summary>
    /// セーブデータを削除する機能
    /// </summary>
    [Command(name:"!delete_data",description:"Delete ALl Save Data")]
    public class DeleteSaveDataUser:IRunnable
    {
        public async UniTask<bool> Run()
        {
            await UniTask.Delay(500);
            
            //セーブデータの全削除
            SaveUtils.DeleteAll();
            
            Outputter.Instance.Output("セーブデータを削除しました！");
            return true;
        }
    }
}