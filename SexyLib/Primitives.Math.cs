using System;
using System.Collections.Generic;
using System.Linq;

using Atmosphere.SexyLib.Exceptions;

namespace Atmosphere.SexyLib
{
    public static partial class Primitives
    {
        #region Abstractions


        private static ISExp AdditiveMath(Func<Number, Number, Number> arithmetic, string functionName, Number baseValue, params ISExp[] parameters)
        {
            Number value = baseValue;

            for (int i = 0; i < parameters.Length; i++)
            {      
                Number number = (Number)((Atom)parameters[i]).Value;

                value = arithmetic(value, number);
            }
            
            return Atom.CreateNumber(value);
        }
            
        public static ISExp SubtractiveMath(Func<Number, Number, Number> arithmetic, string functionName, Number baseValue, params ISExp[] parameters)
        {
            Number value = baseValue;
            
            for (int i = 0; i < parameters.Length; i++)
            {      
                Number number = (Number)((Atom)parameters[i]).Value;

                if (i == 0 && parameters.Length != 1)
                {
                    // If we have multiple values, use the first value (this one) 
                    // as the base without flipping its sign
                    
                    value = number;
                }
                else
                {
                    value = arithmetic(value, number);
                }
            }
            
            return Atom.CreateNumber(value);
        }


        #endregion Abstractions





        
        [PrimitiveMethod("+")]
        public static ISExp Add(string name, params ISExp[] parameters)
        {
            CheckAllTypes(IsNumber, name, "number", parameters);

            return AdditiveMath(Number.Add, name, Number.Zero, parameters);
        }
        
        [PrimitiveMethod("*")]
        public static ISExp Multiply(string name, params ISExp[] parameters)
        {
            CheckAllTypes(IsNumber, name, "number", parameters);

            return AdditiveMath(Number.Multiply, name, Number.One, parameters);
        }
        
        [PrimitiveMethod("-")]
        public static ISExp Subtract(string name, params ISExp[] parameters)
        {
            CheckEnoughArguements(name, 1, parameters);

            CheckAllTypes(IsNumber, name, "number", parameters);

            return SubtractiveMath(Number.Subtract, name, Number.Zero, parameters);
        }
        
        [PrimitiveMethod("/")]
        public static ISExp Divide(string name, params ISExp[] parameters)
        {
            CheckEnoughArguements(name, 1, parameters);
            
            CheckAllTypes(IsNumber, name, "number", parameters);

            return SubtractiveMath(Number.Divide, name, Number.One, parameters);
        }





        /*
        
        [PrimitiveMethod("modulo")]
        public static ISExp Modulo(string name, params ISExp[] parameters)
        {
            // We could in theory support non-integer modulo, since C# supports it.
            // But it's horribly confusing, especially for negative decimals, so f**k it.
            
            CheckArity(name, 2, parameters);
                                               
            for (int i = 0; i < parameters.Length; i++)
            {
                CheckType(IsLong, name, i, "integer", parameters);               
            }
                
            long dividend = ((Atom)parameters[0]).GetValueAsLong();
            long divisor = ((Atom)parameters[1]).GetValueAsLong();

            if (divisor == 0)
            {
                throw new UndefinedOperationException(String.Format("{0}: Operation not defined for a divisor of 0.", name));
            }

            long value;

            if (dividend > 0 && divisor > 0)
            {
                value = dividend % divisor;
            }
            else if (dividend > 0)
            {
                // Negative divisor. Take the normal modulo as if it were positive,
                // then add the divisor (which is negative).

                long temp = dividend % -divisor;

                value = temp == 0 ? temp : temp + divisor; // remember, divisor is negative
            }
            else if (divisor > 0)
            {
                // Negative dividend. Take the normal modulo as if it were positive, invert
                // it, then add the divisor.

                long temp = -(-dividend % divisor);

                    value = temp == 0 ? temp : temp + divisor;
            }
            else
            {
                // Both negative. Take the normal modulo as if they were 
                // positive and invert the result.

                value = -(-dividend % -divisor);
            }


            
            return Atom.CreateLong(value);
        }


        
        [PrimitiveMethod("expt")]
        public static ISExp Exponent(string name, params ISExp[] parameters)
        {
            CheckArity(name, 2, parameters);
                       
            for (int i = 0; i < parameters.Length; i++)
            {
                CheckType(IsNumber, name, i, "number", parameters);
            }

            bool floating = IsDouble(parameters[0]) || IsDouble(parameters[1]);

            double @base = ((Atom)parameters[0]).GetValueAsDouble();
            double exponent = ((Atom)parameters[1]).GetValueAsDouble();

            floating |= exponent < 0;

            double value = Math.Pow(@base, exponent);

            Atom result;
            
            if (floating)
            {
                result = Atom.CreateDouble(value);
            }
            else
            {
                result = Atom.CreateLong((long)value);
            }
            
            return result;
        }

        */


    }
}

