using System;
using System.Linq;

using Atmosphere.SexyLib.Exceptions;

namespace Atmosphere.SexyLib
{
    public static partial class Primitives
    {        
        [PrimitiveMethod("pair?")]
        public static ISExp PairP(string name, params ISExp[] parameters)
        {
            CheckArity(name, 1, parameters);
            
            return Atom.CreateBoolean(IsPair(parameters[0]));
        }  

        [PrimitiveMethod("list?")]
        public static ISExp ListP(string name, params ISExp[] parameters)
        {
            CheckArity(name, 1, parameters);
            
            return Atom.CreateBoolean(IsList(parameters[0]));
        }
        
        [PrimitiveMethod("boolean?")]
        public static ISExp BooleanP(string name, params ISExp[] parameters)
        {
            CheckArity(name, 1, parameters);
            
            return Atom.CreateBoolean(IsBoolean(parameters[0]));
        }
        
        [PrimitiveMethod("char?")]
        public static ISExp CharP(string name, params ISExp[] parameters)
        {
            CheckArity(name, 1, parameters);

            return Atom.CreateBoolean(IsChar(parameters[0]));
        }
        
        [PrimitiveMethod("string?")]
        public static ISExp StringP(string name, params ISExp[] parameters)
        {
            CheckArity(name, 1, parameters);

            return Atom.CreateBoolean(IsString(parameters[0]));
        }
        
        [PrimitiveMethod("symbol?")]
        public static ISExp SymbolP(string name, params ISExp[] parameters)
        {
            CheckArity(name, 1, parameters);

            return Atom.CreateBoolean(IsSymbol(parameters[0]));
        }
        
        [PrimitiveMethod("number?")]
        public static ISExp NumberP(string name, params ISExp[] parameters)
        {
            CheckArity(name, 1, parameters);
            
            return Atom.CreateBoolean(IsNumber(parameters[0]));
        }
        
        [PrimitiveMethod("integer?")]
        public static ISExp IntegerP(string name, params ISExp[] parameters)
        {
            CheckArity(name, 1, parameters);
            
            return Atom.CreateBoolean(IsLong(parameters[0]));
        }

    }
}

