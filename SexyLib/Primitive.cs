using System;

namespace Atmosphere.SexyLib
{
    public delegate ISExp SexyFunction(string name, params ISExp[] parameters);

    public class Primitive
    {
        private Primitive() { }


        internal static Primitive Create(string name, SexyFunction function)
        {
            Primitive primitive = new Primitive
            {
                Name = name,
                Function = function
            };

            return primitive;
        }



        public ISExp Invoke(params ISExp[] parameters)
        {
            return Function.Invoke(Name, parameters);
        }

        public string Name { get; private set; }

        public SexyFunction Function { get; private set; }
    }
}

