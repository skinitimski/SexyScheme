using System;
using System.Linq;

using Atmosphere.SexyLib.Exceptions;

namespace Atmosphere.SexyLib
{
    public static partial class Primitives
    {   
        private delegate bool TypeChecker(ISExp sexp);


        private static void CheckEnoughArguements(string functionName, int atLeastCount, params ISExp[] parameters)
        {
            if (parameters.Length < atLeastCount)
            {
                throw new NotEnoughArgumentsException(functionName, atLeastCount, parameters.Length);
            }
        }

        private static void CheckArity(string functionName, int expectedCount, params ISExp[] parameters)
        {
            if (parameters.Length != expectedCount)
            {
                throw new ArityException(functionName, expectedCount, parameters.Length);
            }
        }

        private static void CheckArity(string functionName, int minArgumentCount, int maxArgumentCount, params ISExp[] parameters)
        {
            if (parameters.Length < minArgumentCount || parameters.Length > maxArgumentCount)
            {
                throw new ArityException(functionName, minArgumentCount, maxArgumentCount, parameters.Length);
            }
        }

        private static void CheckType(Predicate<ISExp> checker, string functionName, int index, string expectedType, ISExp parameter)
        {
            if (!checker(parameter))
            {
                throw new UnexpectedTypeException(functionName, index, expectedType, parameter);
            }
        }







        
//        public static bool IsList(ISExp sexp)
//        {
//        }

        public static bool IsPair(ISExp sexp)
        {
            bool isPair = false;

            Pair pair = sexp as Pair;

            if (pair != null)
            {
                // The empty pair is not a pair!
                isPair = !pair.IsEmpty;
            }

            return isPair;
        }
        
        public static bool IsNumber(ISExp sexp)
        {
            return IsOneOf(sexp, AtomType.DOUBLE, AtomType.LONG);
        }
        
        public static bool IsLong(ISExp sexp)
        {
            return IsOneOf(sexp, AtomType.LONG);
        }
        
        public static bool IsDouble(ISExp sexp)
        {
            return IsOneOf(sexp, AtomType.DOUBLE);
        }
        
        public static bool IsBoolean(ISExp sexp)
        {
            return IsOneOf(sexp, AtomType.BOOLEAN);
        }
        
        public static bool IsChar(ISExp sexp)
        {
            return IsOneOf(sexp, AtomType.CHAR);
        }
        
        public static bool IsString(ISExp sexp)
        {
            return IsOneOf(sexp, AtomType.STRING);
        }
        
        public static bool IsSymbol(ISExp sexp)
        {
            return IsOneOf(sexp, AtomType.SYMBOL);
        }
        
        public static bool IsOneOf(ISExp sexp, params AtomType[] types)
        {
            bool isOneOf = false;
            
            Atom atom = sexp as Atom;
            
            if (atom != null)
            {
                if (types.Any(x => x.Equals(atom.Type)))
                {
                    isOneOf = true;
                }
            }
            
            return isOneOf;
        }
        
        public static bool IsTrue(ISExp sexp)
        {
            return !IsFalse(sexp);
        }
        
        public static bool IsFalse(ISExp sexp)
        {
            // Everything is true except the boolean value false (#f)
            
            bool isFalse = false;
            
            Atom atom = sexp as Atom;
            
            if (atom != null)
            {
                if (atom.Type == AtomType.BOOLEAN)
                {
                    isFalse = !(bool)atom.Value;
                }
            }
            
            return isFalse;
        }
    }
}

