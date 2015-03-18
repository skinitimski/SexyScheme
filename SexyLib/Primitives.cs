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

        /// <summary>
        /// Asserts that the number of parameters exactly equals the <paramref name="expectedCount" />.
        /// </summary>
        private static void CheckArity(string functionName, int expectedCount, params ISExp[] parameters)
        {
            if (parameters.Length != expectedCount)
            {
                throw new ArityException(functionName, expectedCount, parameters.Length);
            }
        }

        /// <summary>
        /// Asserts that the number of parameters falls between the <paramref name="minArgumentCount" /> and <paramref name="maxArgumentCount" />, inclusively.
        /// </summary>
        private static void CheckArity(string functionName, int minArgumentCount, int maxArgumentCount, params ISExp[] parameters)
        {
            if (parameters.Length < minArgumentCount || parameters.Length > maxArgumentCount)
            {
                throw new ArityException(functionName, minArgumentCount, maxArgumentCount, parameters.Length);
            }
        }
        
        /// <summary>
        /// Asserts that the type of the given <paramref name="parameter" /> matches the type asserted by the given <paramref name="checker" /> function.
        /// </summary>
        private static void CheckType(Predicate<ISExp> checker, string functionName, int index, string expectedType, ISExp[] parameters)
        {
            if (!checker(parameters[index]))
            {
                throw new UnexpectedTypeException(functionName, index, expectedType, parameters[index]);
            }
        }
        
        /// <summary>
        /// Asserts that the type of the given <paramref name="parameter" /> matches the type asserted by the given <paramref name="checker" /> function.
        /// </summary>
        private static void CheckAllTypes(Predicate<ISExp> checker, string functionName, string expectedType, ISExp[] parameters)
        {
            for (int i = 0; i < parameters.Length; i++)
            {
                if (!checker(parameters[i]))
                {
                    throw new UnexpectedTypeException(functionName, i, expectedType, parameters[i]);
                }
            }
        }









        public static bool IsPair(this ISExp sexp)
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

        public static bool IsList(this ISExp sexp)
        {
            bool isList = false;

            Pair pair = sexp as Pair;

            if (pair != null)
            {
                isList = pair.IsProper;
            }

            return isList;
        }
        
        public static bool IsNumber(this ISExp sexp)
        {
            return IsOneOf(sexp, AtomType.NUMBER);
        }
        
        public static bool IsBoolean(this ISExp sexp)
        {
            return IsOneOf(sexp, AtomType.BOOLEAN);
        }
        
        public static bool IsChar(this ISExp sexp)
        {
            return IsOneOf(sexp, AtomType.CHAR);
        }
        
        public static bool IsString(this ISExp sexp)
        {
            return IsOneOf(sexp, AtomType.STRING);
        }
        
        public static bool IsSymbol(this ISExp sexp)
        {
            return IsOneOf(sexp, AtomType.SYMBOL);
        }
        
        public static bool IsText(this ISExp sexp)
        {
            return IsOneOf(sexp, AtomType.STRING, AtomType.SYMBOL, AtomType.NUMBER);
        }

        public static bool IsOneOf(this ISExp sexp, params AtomType[] types)
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
        
        public static bool IsTrue(this ISExp sexp)
        {
            return !IsFalse(sexp);
        }
        
        public static bool IsFalse(this ISExp sexp)
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

