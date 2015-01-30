using System;
using System.Linq;

using Atmosphere.SexyLib.Exceptions;

namespace Atmosphere.SexyLib
{
    public static partial class Primitives
    {        
        [PrimitiveMethod("symbol->string")]
        public static ISExp SymbolToString(string name, params ISExp[] parameters)
        {
            CheckArity(name, 1, parameters);
            
            CheckType(IsSymbol, name, 0, "symbol", parameters);
            
            Atom symbol = parameters[0] as Atom;
            
            ISExp @string = Atom.CreateString((string)symbol.Value);
            
            return @string;
        }
        
        [PrimitiveMethod("long->string")]
        public static ISExp LongToString(string name, params ISExp[] parameters)
        {
            CheckArity(name, 1, parameters);
            
            CheckType(IsLong, name, 0, "long", parameters);
            
            Atom @long = parameters[0] as Atom;
            
            ISExp @string = Atom.CreateString(@long.Value.ToString());
            
            return @string;
        }

        [PrimitiveMethod("double->string")]
        public static ISExp DoubleToString(string name, params ISExp[] parameters)
        {
            CheckArity(name, 1, parameters);
            
            CheckType(IsDouble, name, 0, "double", parameters);
            
            Atom @double = parameters[0] as Atom;
            
            ISExp @string = Atom.CreateString(@double.Value.ToString());
            
            return @string;
        }
    }
}

