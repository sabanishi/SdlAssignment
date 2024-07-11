using System.Collections.Generic;
using UnityEngine;

namespace Sabanishi.SdiAssignment.User
{
    [Command(name:"Test",description:"Test command")]
    public class TestUser
    {
        [Option(names:new[]{"-t","--test"},description:"Test option")]
        public static string test;

        [Option(names:new[]{"-h","--hoge"},description:"Hoge option")]
        public static bool isHoge;

        [Command(name:"Run",description:"Run test command")]
        public void DoRun([Parameter(names:new []{"-p","--path"},description:"Run",defaultValue:"/hoge")]string path)
        {
            Debug.Log($"path:{path},test:{test},isHoge:{isHoge}");
        }
    }
}