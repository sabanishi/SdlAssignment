using System.Runtime.InteropServices;
using UnityEngine;

namespace Sabanishi.SdiAssignment
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
        }
    }
}