using System;
using System.Linq;

using Atmosphere.SexyLib.Exceptions;

namespace Atmosphere.SexyLib
{
    public static partial class Primitives
    {        
        public static ISExp PairP(string name, params ISExp[] parameters)
        {
            CheckArity("pair?", 1, parameters);
                    
            return Atom.CreateBoolean(IsPair(parameters[0]));
        }

        public static ISExp BooleanP(string name, params ISExp[] parameters)
        {
            CheckArity("boolean?", 1, parameters);
            
            return Atom.CreateBoolean(IsBoolean(parameters[0]));
        }
        
        public static ISExp CharP(string name, params ISExp[] parameters)
        {
            CheckArity("char?", 1, parameters);
            
            return Atom.CreateBoolean(IsChar(parameters[0]));
        }
        
        public static ISExp StringP(string name, params ISExp[] parameters)
        {
            CheckArity("string?", 1, parameters);

            return Atom.CreateBoolean(IsString(parameters[0]));
        }
        
        public static ISExp SymbolP(string name, params ISExp[] parameters)
        {
            CheckArity("symbol?", 1, parameters);
            
            return Atom.CreateBoolean(IsSymbol(parameters[0]));
        }
        
        public static ISExp NumberP(string name, params ISExp[] parameters)
        {
            CheckArity("number?", 1, parameters);
            
            return Atom.CreateBoolean(IsNumber(parameters[0]));
        }
        
        public static ISExp IntegerP(string name, params ISExp[] parameters)
        {
            CheckArity("integer?", 1, parameters);
            
            return Atom.CreateBoolean(IsLong(parameters[0]));
        }

    }
}

