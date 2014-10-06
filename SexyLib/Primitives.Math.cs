using System;
using System.Linq;

using Atmosphere.SexyLib.Exceptions;

namespace Atmosphere.SexyLib
{
    public static partial class Primitives
    {
        #region Helper Routines

        private static double Addition(double a, double b)
        {
            return a + b;
        }

        private static double Subtraction(double a, double b)
        {
            return a - b;
        }

        private static double Multiplication(double a, double b)
        {
            return a * b;
        }

        private static double Division(double a, double b)
        {
            return a / b;
        }

        #endregion Helper Routines



        #region Abstractions

        private static ISExp AdditiveMath(Func<double, double, double> arithmetic, string functionName, double baseValue, params ISExp[] parameters)
        {
            double value = baseValue;
            
            bool floating = false;
            
            for (int i = 0; i < parameters.Length; i++)
            {      
                CheckType(IsNumber, functionName, i, "number", parameters[i]);
                
                Atom atom = parameters[i] as Atom;
                
                if (!floating && atom.Type == AtomType.DOUBLE)
                {
                    floating = true;
                }

                value = arithmetic(value, atom.GetValueAsDouble());
            }

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
                
        public static ISExp SubtractiveMath(Func<double, double, double> arithmetic, string functionName, double baseValue, params ISExp[] parameters)
        {
            CheckEnoughArguements(functionName, 1, parameters);
            
            double value = baseValue;
            
            bool floating = false;
            
            for (int i = 0; i < parameters.Length; i++)
            {
                CheckType(IsNumber, functionName, i, "number", parameters[i]);
                
                Atom atom = parameters[i] as Atom;
                
                if (!floating && atom.Type == AtomType.DOUBLE)
                {
                    floating = true;
                }
                
                if (i == 0 && parameters.Length != 1)
                {
                    // If we have multiple values, use the first value (this one) 
                    // as the base without flipping its sign
                    
                    value = atom.GetValueAsDouble();
                }
                else
                {
                    value = arithmetic(value, atom.GetValueAsDouble());
                }
            }
            
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


        #endregion Abstractions






        public static ISExp Add(params ISExp[] parameters)
        {
            return AdditiveMath(Addition, "+", 0, parameters);
        }
        
        public static ISExp Multiply(params ISExp[] parameters)
        {
            return AdditiveMath(Multiplication, "*", 1, parameters);
        }
        
        public static ISExp Subtract(params ISExp[] parameters)
        {
            return SubtractiveMath(Subtraction, "-", 0, parameters);
        }
        
        public static ISExp Divide(params ISExp[] parameters)
        {
            return SubtractiveMath(Division, "/", 1, parameters);
        }







        public static ISExp Modulo(params ISExp[] parameters)
        {
            // We could in theory support non-integer modulo, since C# supports it.
            // But it's horribly confusing, especially for negative decimals, so f**k it.
            
            CheckArity("modulo", 2, parameters);
                                               
            for (int i = 0; i < parameters.Length; i++)
            {
                CheckType(IsInteger, "modulo", i, "integer", parameters[i]);               
            }
                
            long dividend = ((Atom)parameters[0]).GetValueAsLong();
            long divisor = ((Atom)parameters[1]).GetValueAsLong();

            if (divisor == 0)
            {
                throw new UndefinedOperationException("modulo: Operation not defined for a divisor of 0.");
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



        public static ISExp Exponent(params ISExp[] parameters)
        {
            CheckArity("expt", 2, parameters);
                       
            for (int i = 0; i < parameters.Length; i++)
            {
                CheckType(IsNumber, "expt", i, "number", parameters[i]);
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



    }
}

