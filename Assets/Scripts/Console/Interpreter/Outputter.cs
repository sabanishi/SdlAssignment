namespace Sabanishi.SdiAssignment
{
    public class Outputter
    {
        private static Outputter _instance;
        public static Outputter Instance => _instance;

        public static void Boot(Interpreter interpreter)
        {
            _instance = new Outputter(interpreter);
        }

        private readonly Interpreter _interpreter;
        private Outputter(Interpreter interpreter)
        {
            _interpreter = interpreter;
        }
        
        public void Output(string message)
        {
            _interpreter.Output(message);
        }
    }
}