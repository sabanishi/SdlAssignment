using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

namespace Sabanishi.SdiAssignment
{
    public class CommandExecutor
    {
        private IEnumerable<Command> _mainCommands;

        public IEnumerable<Command> MainCommands
        {
            get
            {
                if (_mainCommands != null) return _mainCommands;
                return null;
            }
        }
        
        public bool Execute(ArgumentMap map)
        {
            //mapの_mainArgsの最初の要素を取得
            var maps = map.GetMainArgs();
            if (maps.Count == 0)
            {
                return false;
            }

            var mainCommandName = maps[0];
            
            //CommandのNameがmainCommandと一致するクラスを探す
            if (TrySearchMainCommand(mainCommandName, out var mainCommand,out var type))
            {
                //クラス中のOption属性を持つフィールドを全て取得
                var fields = type.GetFields();
                foreach (var field in fields)
                {
                    Debug.Log(field.Name);
                    var option = field.GetCustomAttribute<Option>();
                    if (option != null)
                    {
                        foreach (var name in option.Names)
                        {
                            //mapの_optionMapにnameがある場合、fieldに値を代入
                            if (map.TryGetOption(name,out var result))
                            {
                                field.SetValue(null,result);
                            }
                            
                            //mapの_optionSwitchesにnameがある場合、fieldに値を設定
                            if (map.HasSwitch(name))
                            {
                                field.SetValue(null,true);
                            }
                        }
                    }
                }
                
                
                if (maps.Count == 1)
                {
                    //サブコマンドがない場合
                }
                else
                {
                    var subCommandName = maps[1];
                    //CommandのNameがsubCommandと一致するメソッドを探す
                    Debug.Log(type);
                    if (TrySearchSubCommand(type, subCommandName, out var subCommand,out var memberInfo))
                    {
                        //サブコマンドがある場合
                        
                        //引数に含まれるParameter属性を持つフィールドを全て取得
                        var parameters = memberInfo.GetParameters();
                        var objects = parameters.Select(p =>
                        {
                            var parameterAttribute = p.GetCustomAttribute<Parameter>();
                            if (parameterAttribute != null)
                            {
                                foreach (var name in parameterAttribute.Names)
                                {
                                    if (map.TryGetOption(name, out var result))
                                    {
                                        Debug.Log(result);
                                        return (object)result;
                                    }

                                    if (map.HasSwitch(name))
                                    {
                                        Debug.Log(true);
                                        return (object)true;
                                    }
                                }
                                //デフォルトの値を返す
                                Debug.Log(parameterAttribute.DefaultValue);
                                return parameterAttribute.DefaultValue;
                            }

                            return false;
                        }).ToArray();
                        
                        //関数を実行する
                        var instance = Activator.CreateInstance(type);
                        memberInfo.Invoke(instance,objects);
                    }
                }
            }

            return false;
        }

        private bool TrySearchSubCommand(Type classType,string commandName, out Command command,out MethodInfo methodInfo)
        {
            command = null;
            methodInfo = null;
            var methods = classType.GetMethods();
            foreach (var method in methods)
            {
                var attributes = method.GetCustomAttributes<Command>();
                foreach (var attribute in attributes)
                {
                    if (attribute.Name.Equals(commandName))
                    {
                        command = attribute;
                        methodInfo = method;
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 引数commandNameとNameが一致するClass Commandを探す
        /// </summary>
        private bool TrySearchMainCommand(string commandName,out Command command,out Type type)
        {
            command = null;
            type = null;
            var types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (var t in types)
            {
                var attributes = t.GetCustomAttributes<Command>();
                foreach (var attribute in attributes)
                {
                    if (attribute.Name.Equals(commandName))
                    {
                        command = attribute;
                        type = t;
                        return true;
                    }
                }
            }

            return false;
        }
        
        private IEnumerable<MemberInfo> GetAllCommandLineMember()
        {
            var allMembers = new List<MemberInfo>();
            var types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (var type in types)
            {
                var attributes = type.GetCustomAttributes<Command>();
                foreach (var attribute in attributes)
                {
                    Debug.Log(attribute.Name);
                    allMembers.Add(type);
                }
                
                var members = type.GetMembers();
                foreach (var member in members)
                {
                    if (member.GetCustomAttribute<Command>() != null)
                    {
                        allMembers.Add(member);
                    }
                }
            }
            
            return allMembers;
        }
    }
}