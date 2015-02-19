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
    }
}

