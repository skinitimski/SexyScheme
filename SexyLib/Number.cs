using System;
using System.Numerics;

using Atmosphere.Extensions;
using Atmosphere.SexyLib.Exceptions;

namespace Atmosphere.SexyLib
{
    public struct Number
    {        

        public static readonly Number Zero = new Number();
        public static readonly Number One = new Number(1);



        #region Constructors

        public Number(BigInteger value)
            : this()
        {
            Mantissa = value;
        }




        public static Number Create()
        {
            Number number = new Number
            {
                Mantissa = BigInteger.Zero,
                Denominator = BigInteger.One,
                Exponent = BigInteger.Zero,                
                IsExact = true
            };

            return number;
        }

        #endregion Constructors






        public static Number Parse(string data)
        {
            data = data.ToLower();

            if (data.Contains(".") || data.Contains("e"))
            {

            }
            else
            {
                 
            }

            return Number.Zero;
        }





        #region Arithmetic Operations

        public static Number Add(Number left, Number right)
        {
            Number result = new Number();

            int comparison = CompareAlignment(left, right);

            // 

//            if (IsInteger)
//            {
//            }
//            else if (IsRational)
//            {
//            }
//            else // real
//            {
//                if (left.Exponent < right.Exponent)
//                {
//                    Number tmp = left;
//                    left = right;
//                    right = tmp;
//                }
//
//                result.Mantissa = AlignExponent(left, right) + right.Mantissa;
//                result.Exponent = right.Exponent;
//            }

            return result;
        }
        
        public static Number Subtract(Number left, Number right)
        {
            return Number.Zero;
        }
        
        public static Number Multiply(Number left, Number right)
        {
            return Number.Zero;
        }
        
        public static Number Divide(Number left, Number right)
        {
            return Number.Zero;
        }

        #endregion Arithmetic Operations







        private static void Align(ref Number left, ref Number right)
        {
        }

        /// <summary>
        /// Compares the alignment.
        /// </summary>
        /// <returns>
        /// -1 if left must be aligned to right, +1 if right must be aligned to left, and 0 if they are aligned.
        /// </returns>        
        private static int CompareAlignment(Number left, Number right)
        {
            int comparison = 0;
            
            if (left.IsInteger ^ right.IsInteger)
            {
                // one of them is not an integer. must align.

                comparison = left.IsInteger ? 1 : -1;
            }
            else if (left.IsRational ^ right.IsRational)
            {
                // one of them is not rational. must align.

                comparison = left.IsRational ? 1 : -1;
            }
            else if (left.IsReal ^ right.IsReal)
            {
                comparison = left.IsReal ? 1 : -1;
            }

            return comparison;
        }

        
        /// <summary>
        /// Returns the mantissa of <paramref name="value" />, aligned to the exponent of <paramref name="reference" />.
        /// Assumes the exponent of <paramref name="value" /> is greater than or equal to that of <paramref name="reference" />.
        /// </summary>
        private static BigInteger AlignExponent(Number value, Number reference)
        {
#if DEBUG
            if (value.Exponent <= reference.Exponent) throw new SexySchemeImplementationException("Exponent of value is expected to be larger than that of reference.");
#endif
            return value.Mantissa * BigInteger.Pow(10, (int)(value.Exponent - reference.Exponent));
        }

        #region Operators

        #region Conversion Operators
        
        #region Implicit Conversion Operators
        
        /// <summary>
        /// int->Number conversion
        /// </summary>            
        public static implicit operator Number(int value)
        {
            return new Number(value);
        }
        
        /// <summary>
        /// long->Number conversion
        /// </summary>            
        public static implicit operator Number(long value)
        {
            return new Number(value);
        }
        
        #endregion Implicit Conversion Operators
        
        #region Explicit Conversion Operators
        
        /// <summary>
        /// int->Number conversion
        /// </summary>            
        public static explicit operator int(Number value)
        {
            return (int)value.Mantissa;
        }
        
        /// <summary>
        /// long->Number conversion
        /// </summary>            
        public static explicit operator long(Number value)
        {
            return (long)value.Mantissa;
        }
        
        #endregion Explicit Conversion Operators

        #endregion Conversion Operators

        #endregion Operators










        #region Member Variables

        public BigInteger Mantissa { get; private set; }
        
        public BigInteger Denominator { get; private set; }
        
        public BigInteger Exponent { get; private set; }

        public bool IsExact { get; internal set; }
        
        #endregion Member Variables



        #region Other Variables

        public bool IsComplex { get { return false; } }
        
        public bool IsReal { get { return true; } }
        
        public bool IsRational { get { return IsReal && Exponent == BigInteger.Zero; } }
        
        public bool IsInteger { get { return IsRational && Denominator == BigInteger.One; } }

        #endregion Other Variables
    }
}
