using System;
using System.Collections.Generic;

namespace Sabanishi.SdiAssignment
{
    [AttributeUsage(AttributeTargets.Field,AllowMultiple = false,Inherited = false)]
    public class Option:Attribute
    {
        public string[] Names { get; }
        public string Description { get; }
        
        public Option(string[] names, string description)
        {
            Names = names;
            Description = description;
        }
    }
}