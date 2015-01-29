using System;
using System.Linq;

using Atmosphere.SexyLib.Exceptions;

namespace Atmosphere.SexyLib
{
    public static partial class Primitives
    {        
        public static ISExp SymbolToString(params ISExp[] parameters)
        {
            CheckArity("symbol->string", 1, parameters);
            
            CheckType(IsSymbol, "symbol->string", 0, "symbol", parameters[0]);
            
            Atom symbol = parameters[0] as Atom;
            
            ISExp @string = Atom.CreateString((string)symbol.Value);
            
            return @string;
        }
        
        public static ISExp LongToString(params ISExp[] parameters)
        {
            CheckArity("long->string", 1, parameters);
            
            CheckType(IsLong, "long->string", 0, "long", parameters[0]);
            
            Atom @long = parameters[0] as Atom;
            
            ISExp @string = Atom.CreateString((long)@long.Value.ToString());
            
            return @string;
        }
        
        public static ISExp DoubleToString(params ISExp[] parameters)
        {
            CheckArity("double->string", 1, parameters);
            
            CheckType(IsDouble, "double->string", 0, "double", parameters[0]);
            
            Atom @double = parameters[0] as Atom;
            
            ISExp @string = Atom.CreateString((double)@double.Value.ToString());
            
            return @string;
        }
    }
}

