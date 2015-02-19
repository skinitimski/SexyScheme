using System;
using System.Numerics;

namespace Atmosphere.SexyLib
{
    public class Number
    {        

        #region Constructors

        private Number()
        {
            Mantissa = BigInteger.Zero;
            Denominator = BigInteger.One;
            Exponent = BigInteger.Zero;

            IsExact = true;
        }

        private Number(BigInteger value)
            : this()
        {
            Mantissa = value;
        }

        #endregion Constructors


        public static implicit operator Number(long value)
        {
            return new Number(value);
        }


        #region Member Variables

        public BigInteger Mantissa { get; private set; }
        
        public BigInteger Denominator { get; private set; }
        
        public BigInteger Exponent { get; private set; }

        public bool IsExact { get; internal set; }
        
        #endregion Member Variables



        #region Other Variables

        public bool IsComplex { get { return false; } }
        
        public bool IsReal { get { return false; } }
        
        public bool IsRational { get { return IsReal && Exponent == BigInteger.Zero; } }
        
        public bool IsInteger { get { return IsRational && Denominator == BigInteger.One; } }

        #endregion Other Variables
    }
}
