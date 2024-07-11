using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Sabanishi.SdiAssignment.Sandbox
{
    public class InterpreterTest:MonoBehaviour
    {
        private void Awake()
        {
            var interpreter = new Interpreter();
            var text = "Test Run --path \\hoge\\huga -t test";
            interpreter.Interpret(text).Forget();
        }
    }
}