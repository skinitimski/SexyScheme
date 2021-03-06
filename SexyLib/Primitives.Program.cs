using System;
using System.Linq;

using Atmosphere.SexyLib.Exceptions;

namespace Atmosphere.SexyLib
{
    public static partial class Primitives
    {        
        [PrimitiveMethod("write")]
        public static ISExp Write(string name, params ISExp[] parameters)
        {
            CheckArity(name, 1, parameters);
            
            Console.WriteLine(parameters[0].ToString());
            
            return Atom.Null;
        }
        
        [PrimitiveMethod("display")]
        public static ISExp Display(string name, params ISExp[] parameters)
        {
            CheckArity(name, 1, parameters);
            
            Console.WriteLine(parameters[0].ToDisplay());
            
            return Atom.Null;
        }
        
        [PrimitiveMethod("newline")]
        public static ISExp Newline(string name, params ISExp[] parameters)
        {
            CheckArity(name, 0, parameters);

            Console.WriteLine();

            return Atom.Null;
        }
        
        [PrimitiveMethod("exit")]
        public static ISExp Exit(string name, params ISExp[] parameters)
        {
            CheckArity(name, 0, 1, parameters);

            if (parameters.Length == 0)
            {
                Environment.Exit(0);
                return Atom.Null;
            }
            else
            {
                if (!IsLong(parameters[0]))
                {
                    throw new UnexpectedTypeException(name, 0, "integer", parameters[0]);
                }

                Environment.Exit((int)(long)((Atom)parameters[0]).Value);
                return Atom.Null;
            }
        }
    }
}

