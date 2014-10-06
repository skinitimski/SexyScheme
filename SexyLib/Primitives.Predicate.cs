using System;
using System.Linq;

using Atmosphere.SexyLib.Exceptions;

namespace Atmosphere.SexyLib
{
    public static partial class Primitives
    {        
        public static ISExp PairP(params ISExp[] parameters)
        {
            CheckArity("pair?", 1, parameters);
                    
            return new Atom(IsPair(parameters[0]), AtomType.BOOLEAN);
        }

        public static ISExp BooleanP(params ISExp[] parameters)
        {
            CheckArity("boolean?", 1, parameters);
            
            return new Atom(IsBoolean(parameters[0]), AtomType.BOOLEAN);
        }
        
        public static ISExp CharP(params ISExp[] parameters)
        {
            CheckArity("char?", 1, parameters);
            
            return new Atom(IsChar(parameters[0]), AtomType.BOOLEAN);
        }
        
        public static ISExp StringP(params ISExp[] parameters)
        {
            CheckArity("string?", 1, parameters);

            return new Atom(IsString(parameters[0]), AtomType.BOOLEAN);
        }
        
        public static ISExp SymbolP(params ISExp[] parameters)
        {
            CheckArity("symbol?", 1, parameters);
            
            return new Atom(IsSymbol(parameters[0]), AtomType.BOOLEAN);
        }
        
        public static ISExp NumberP(params ISExp[] parameters)
        {
            CheckArity("number?", 1, parameters);
            
            return new Atom(IsNumber(parameters[0]), AtomType.BOOLEAN);
        }
        
        public static ISExp IntegerP(params ISExp[] parameters)
        {
            CheckArity("integer?", 1, parameters);
            
            return new Atom(IsInteger(parameters[0]), AtomType.BOOLEAN);
        }

    }
}

