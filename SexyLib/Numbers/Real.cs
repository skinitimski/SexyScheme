using System;
using System.Numerics;

using Atmosphere.SexyLib.Exceptions;

namespace Atmosphere.SexyLib.Numbers
{
    /// <summary>
    /// Arbitrary precision decimal.
    /// All operations are exact, except for division. Division never determines more digits than the given precision.
    /// Based on http://stackoverflow.com/a/4524254
    /// Author: Jan Christoph Bernack (contact: jc.bernack at googlemail.com)
    /// </summary>
    public struct Real : IComparable<Real>
    {
        /// <summary>
        /// Specifies whether the significant digits should be truncated to the given precision after each operation.
        /// </summary>
        public static bool AlwaysTruncate = false;
        
        /// <summary>
        /// Sets the maximum precision of division operations.
        /// If AlwaysTruncate is set to true all operations are affected.
        /// </summary>
        public static int Precision = 50;




        #region Constructor

        public Real(BigInteger mantissa, int exponent)
            : this()
        {
            Mantissa = mantissa;
            Exponent = exponent;

            Normalize();

            if (AlwaysTruncate)
            {
                Truncate();
            }
        }

        #endregion


        
        /// <summary>
        /// Removes trailing zeros on the mantissa
        /// </summary>
        public void Normalize()
        {
            if (Mantissa.IsZero)
            {
                Exponent = 0;
            }
            else
            {
                BigInteger remainder = 0;

                while (true)
                {
                    BigInteger shortened = BigInteger.DivRem(Mantissa, 10, out remainder);

                    if (remainder == 0)
                    {
                        Mantissa = shortened;
                        Exponent++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
        
        /// <summary>
        /// Truncate the number to the given precision by removing the least significant digits.
        /// </summary>
        /// <returns>The truncated number</returns>
        public Real Truncate(int precision)
        {
            // copy this instance (remember its a struct)
            var shortened = this;
            // save some time because the number of digits is not needed to remove trailing zeros
            shortened.Normalize();
            // remove the least significant digits, as long as the number of digits is higher than the given Precision
            while (NumberOfDigits(shortened.Mantissa) > precision)
            {
                shortened.Mantissa /= 10;
                shortened.Exponent++;
            }
            return shortened;
        }
        
        public Real Truncate()
        {
            return Truncate(Precision);
        }
        
        private static int NumberOfDigits(BigInteger value)
        {
            // do not count the sign
            return (value * value.Sign).ToString().Length;
        }



        #region Conversions
        
        public static implicit operator Real(int value)
        {
            return new Real(value, 0);
        }
        
        public static implicit operator Real(double value)
        {
            var mantissa = (BigInteger) value;
            var exponent = 0;
            double scaleFactor = 1;
            while (Math.Abs(value * scaleFactor - (double)mantissa) > 0)
            {
                exponent -= 1;
                scaleFactor *= 10;
                mantissa = (BigInteger)(value * scaleFactor);
            }
            return new Real(mantissa, exponent);
        }
        
        public static implicit operator Real(decimal value)
        {
            var mantissa = (BigInteger)value;
            var exponent = 0;
            decimal scaleFactor = 1;
            while ((decimal)mantissa != value * scaleFactor)
            {
                exponent -= 1;
                scaleFactor *= 10;
                mantissa = (BigInteger)(value * scaleFactor);
            }
            return new Real(mantissa, exponent);
        }
        
        public static explicit operator double(Real value)
        {
            return (double)value.Mantissa * Math.Pow(10, value.Exponent);
        }
        
        public static explicit operator float(Real value)
        {
            return Convert.ToSingle((double)value);
        }
        
        public static explicit operator decimal(Real value)
        {
            return (decimal)value.Mantissa * (decimal)Math.Pow(10, value.Exponent);
        }
        
        public static explicit operator int(Real value)
        {
            return (int)(value.Mantissa * BigInteger.Pow(10, value.Exponent));
        }
        
        public static explicit operator uint(Real value)
        {
            return (uint)(value.Mantissa * BigInteger.Pow(10, value.Exponent));
        }
        
        #endregion



        #region Operators
        
        public static Real operator +(Real value)
        {
            return value;
        }
        
        public static Real operator -(Real value)
        {
            value.Mantissa *= -1;
            return value;
        }
        
        public static Real operator ++(Real value)
        {
            return value + 1;
        }
        
        public static Real operator --(Real value)
        {
            return value - 1;
        }
        
        public static Real operator +(Real left, Real right)
        {
            return Add(left, right);
        }
        
        public static Real operator -(Real left, Real right)
        {
            return Add(left, -right);
        }
        
        private static Real Add(Real left, Real right)
        {
            return left.Exponent > right.Exponent
                ? new Real(AlignExponent(left, right) + right.Mantissa, right.Exponent)
                    : new Real(AlignExponent(right, left) + left.Mantissa, left.Exponent);
        }
        
        public static Real operator *(Real left, Real right)
        {
            return new Real(left.Mantissa * right.Mantissa, left.Exponent + right.Exponent);
        }
        
        public static Real operator /(Real dividend, Real divisor)
        {
            var exponentChange = Precision - (NumberOfDigits(dividend.Mantissa) - NumberOfDigits(divisor.Mantissa));

            if (exponentChange < 0) exponentChange = 0;

            dividend.Mantissa *= BigInteger.Pow(10, exponentChange);

            return new Real(dividend.Mantissa / divisor.Mantissa, dividend.Exponent - divisor.Exponent - exponentChange);
        }
        
        public static bool operator ==(Real left, Real right)
        {
            return left.Exponent == right.Exponent && left.Mantissa == right.Mantissa;
        }
        
        public static bool operator !=(Real left, Real right)
        {
            return left.Exponent != right.Exponent || left.Mantissa != right.Mantissa;
        }
        
        public static bool operator <(Real left, Real right)
        {
            return left.Exponent > right.Exponent ? AlignExponent(left, right) < right.Mantissa : left.Mantissa < AlignExponent(right, left);
        }
        
        public static bool operator >(Real left, Real right)
        {
            return left.Exponent > right.Exponent ? AlignExponent(left, right) > right.Mantissa : left.Mantissa > AlignExponent(right, left);
        }
        
        public static bool operator <=(Real left, Real right)
        {
            return left.Exponent > right.Exponent ? AlignExponent(left, right) <= right.Mantissa : left.Mantissa <= AlignExponent(right, left);
        }
        
        public static bool operator >=(Real left, Real right)
        {
            return left.Exponent > right.Exponent ? AlignExponent(left, right) >= right.Mantissa : left.Mantissa >= AlignExponent(right, left);
        }
        
        /// <summary>
        /// Returns the mantissa of value, aligned to the exponent of reference.
        /// Assumes the exponent of value is larger than of reference.
        /// </summary>
        private static BigInteger AlignExponent(Real value, Real reference)
        {
#if DEBUG
            if (value.Exponent <= reference.Exponent) throw new SexySchemeImplementationException("Exponent of value is expected to be larger than that of reference.");
#endif
            return value.Mantissa * BigInteger.Pow(10, value.Exponent - reference.Exponent);
        }
        
        #endregion



        #region Additional mathematical functions
        
        public static Real Exp(double exponent)
        {
            var tmp = (Real)1;
            while (Math.Abs(exponent) > 100)
            {
                var diff = exponent > 0 ? 100 : -100;
                tmp *= Math.Exp(diff);
                exponent -= diff;
            }
            return tmp * Math.Exp(exponent);
        }
        
        public static Real Pow(double basis, double exponent)
        {
            var tmp = (Real)1;
            while (Math.Abs(exponent) > 100)
            {
                var diff = exponent > 0 ? 100 : -100;
                tmp *= Math.Pow(basis, diff);
                exponent -= diff;
            }
            return tmp * Math.Pow(basis, exponent);
        }
        
        #endregion



        #region Object/Comparison

        public override string ToString()
        {
            return string.Concat(Mantissa.ToString(), "E", Exponent);
        }
        
        public bool Equals(Real other)
        {
            return other.Mantissa.Equals(Mantissa) && other.Exponent == Exponent;
        }
        
        public override bool Equals(object obj)
        {
            bool equals = false;

            if (obj != null)
            {
                if (obj is Real)
                {
                    equals = Equals((Real)obj);
                }
            }

            return equals;
        }
        
        public override int GetHashCode()
        {
            unchecked
            {
                return (Mantissa.GetHashCode()*397) ^ Exponent;
            }
        }
        
        public int CompareTo(object obj)
        {
            if (ReferenceEquals(obj, null) || !(obj is Real))
            {
                throw new ArgumentException();
            }
            return CompareTo((Real) obj);
        }
        
        public int CompareTo(Real other)
        {
            return this < other ? -1 : (this > other ? 1 : 0);
        }

        #endregion



        
        public BigInteger Mantissa { get; private set; }
        
        public int Exponent { get; private set; }
    }
}