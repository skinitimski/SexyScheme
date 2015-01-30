using System;

namespace Atmosphere.SexyLib
{
    [AttributeUsage(AttributeTargets.Method)]
    public class PrimitiveMethod : Attribute
    {
        private string name;

        public PrimitiveMethod(string name)
        {
            this.name = name;
        }

        public string Name { get { return name; } }
    }
}

