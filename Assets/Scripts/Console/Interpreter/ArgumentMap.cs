using System.Collections.Generic;

namespace Sabanishi.SdiAssignment
{
    /// <summary>
    /// コマンドライン引数を解析し保存するクラス<br />
    /// ex) 「hoge -option value -switch」の場合、<br />
    /// _mainArgs: ["hoge"] <br />
    /// _optionMap: {"-option": "value"} <br />
    /// _optionSwitches: ["-switch"] となる
    /// </summary>
    public class ArgumentMap
    {
        private readonly List<string> _mainArgs;
        private readonly Dictionary<string, string> _optionMap;
        private readonly List<string> _optionSwitches;
        
        public ArgumentMap(string[] args)
        {
            _mainArgs = new List<string>();
            _optionMap = new Dictionary<string, string>();
            _optionSwitches = new List<string>();
            
            int i = 0;
            while (i < args.Length)
            {
                string arg = args[i];
                if (arg.StartsWith("-"))
                {
                    if (i + 1 < args.Length)
                    {
                        if (args[i + 1].StartsWith("-"))
                        {
                            _optionSwitches.Add(args[i]);
                        }
                        else
                        {
                            _optionMap[arg] = args[i + 1];
                            i++;
                        }
                    }
                }
                else
                {
                    _mainArgs.Add(arg);
                }

                i++;
            }
        }
        
        public List<string> GetMainArgs()
        {
            return _mainArgs;
        }

        public bool HasSwitch(string key)
        {
            return _optionSwitches.Contains(key);
        }

        public bool TryGetOption(string key, out string result)
        {
            result = "";
            if (_optionMap.TryGetValue(key, out var option))
            {
                result = option;
                return true;
            }

            return false;
        }
    }
}