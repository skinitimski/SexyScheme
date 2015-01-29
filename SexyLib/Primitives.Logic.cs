using System;
using System.Linq;

using Atmosphere.SexyLib.Exceptions;

namespace Atmosphere.SexyLib
{
    public static partial class Primitives
    {        
        public static ISExp If(string name, params ISExp[] parameters)
        {
            if (parameters.Length < 2 || parameters.Length > 3)
            {
                throw new ArityException(name, 2, 3, parameters.Length);
            }
            
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
        
        public static ISExp Not(string name, params ISExp[] parameters)
        {            
            if (parameters.Length != 1)
            {
                throw new ArityException(name, 1, parameters.Length);
            }

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

