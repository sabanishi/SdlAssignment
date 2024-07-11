using System.Runtime.InteropServices;
using UnityEngine;

namespace Sabanishi.SdiAssignment.Sandbox
{
    public class Test:MonoBehaviour
    {
#if UNITY_STANDALONE_OSX
        [DllImport("Transparent")]
        private static extern void transparent();
#endif

        // Use this for initialization
        [RuntimeInitializeOnLoadMethod]
        static void DoStuff()
        {
#if UNITY_STANDALONE_OSX
            //transparent();
#endif
            
            //PCの縦横の長さを取得する
            var width = Screen.currentResolution.width;
            var height = Screen.currentResolution.height;
            //Debug.Log($"width:{width},height:{height}");
            Screen.SetResolution(width, height, FullScreenMode.Windowed);
        }
    }
}