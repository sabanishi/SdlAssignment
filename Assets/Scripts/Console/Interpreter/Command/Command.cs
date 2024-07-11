using System;

namespace Sabanishi.SdiAssignment
{
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Method,AllowMultiple = false,Inherited = true )]
    public class Command :Attribute
    {
        public string Name { get; }
        public string Description { get; }
        
        public Command(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}