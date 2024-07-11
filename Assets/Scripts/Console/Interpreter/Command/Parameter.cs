using System;

namespace Sabanishi.SdiAssignment
{
    [AttributeUsage(AttributeTargets.Parameter,AllowMultiple = true,Inherited = true)]
    public class Parameter:Attribute
    {
        public string[] Names { get; }
        public string Description { get; }
        public string DefaultValue { get; }
        
        public Parameter(string[] names, string description, string defaultValue = "")
        {
            Names = names;
            Description = description;
            DefaultValue = defaultValue;
        }
    }
}