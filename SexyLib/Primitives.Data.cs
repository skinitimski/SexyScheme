using System;
using System.Linq;

using Atmosphere.SexyLib.Exceptions;

namespace Atmosphere.SexyLib
{
    public static partial class Primitives
    {        
        public static ISExp SymbolToString(string name, params ISExp[] parameters)
        {
            CheckArity(name, 1, parameters);
            
            CheckType(IsSymbol, name, 0, "symbol", parameters[0]);
            
            Atom symbol = parameters[0] as Atom;
            
            ISExp @string = Atom.CreateString((string)symbol.Value);
            
            return @string;
        }
        
        public static ISExp LongToString(string name, params ISExp[] parameters)
        {
            CheckArity(name, 1, parameters);
            
            CheckType(IsLong, name, 0, "long", parameters[0]);
            
            Atom @long = parameters[0] as Atom;
            
            ISExp @string = Atom.CreateString(@long.Value.ToString());
            
            return @string;
        }
        
        public static ISExp DoubleToString(string name, params ISExp[] parameters)
        {
            CheckArity(name, 1, parameters);
            
            CheckType(IsDouble, name, 0, "double", parameters[0]);
            
            Atom @double = parameters[0] as Atom;
            
            ISExp @string = Atom.CreateString(@double.Value.ToString());
            
            return @string;
        }
    }
}

