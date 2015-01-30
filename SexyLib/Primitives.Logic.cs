using System;
using System.Linq;

using Atmosphere.SexyLib.Exceptions;

namespace Atmosphere.SexyLib
{
    public static partial class Primitives
    {        
        [PrimitiveMethod("if")]
        public static ISExp If(string name, params ISExp[] parameters)
        {
            CheckArity(name, 2, 3, parameters);
            
            ISExp result;
            
            if (IsTrue(parameters[0]))
            {
                result = parameters[1];
            }
            else
            {
                if (parameters.Length == 3)
                {
                    result = parameters[2];
                }
                else
                {
                    result = Atom.Null;
                }
            }
            
            return result;
        }
        
        [PrimitiveMethod("and")]
        public static ISExp And(string name, params ISExp[] parameters)
        {            
            ISExp result;
            
            if (parameters.Any(x => IsFalse(x)))
            {
                result = Atom.False;
            }
            else
            {
                result = Atom.True;
            }
            
            return result;
        }
        
        [PrimitiveMethod("or")]
        public static ISExp Or(string name, params ISExp[] parameters)
        {            
            ISExp result;
            
            if (parameters.Any(x => IsTrue(x)))
            {
                result = Atom.True;
            }
            else
            {
                result = Atom.False;
            }
            
            return result;
        }
        
        [PrimitiveMethod("not")]
        public static ISExp Not(string name, params ISExp[] parameters)
        {            
            CheckArity(name, 1, parameters);

            ISExp result;
            
            if (IsFalse(parameters[0]))
            {
                result = Atom.True;
            }
            else
            {
                result = Atom.False;
            }
            
            return result;
        }
    }
}

