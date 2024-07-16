using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Sabanishi.SdiAssignment
{
    [Command(name:"!test",description:"Test command")]
    public class TestUser
    {
        [Option(names:new[]{"-t","--test"},description:"Test option")]
        public static string test;

        [Option(names:new[]{"-h","--hoge"},description:"Hoge option")]
        public static bool isHoge;

        [Command(name:"run",description:"Run test command")]
        public async UniTask<bool> DoRun([Parameter(names:new []{"-p","--path"},description:"Run",defaultValue:"/hoge")]string path)
        {
            Debug.Log($"path:{path},test:{test},isHoge:{isHoge}");
            return true;
        }
    }
}