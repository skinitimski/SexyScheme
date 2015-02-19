using System;
using System.Numerics;

namespace Atmosphere.SexyLib.Numbers
{
    public class Integer
    {
        #region Constructors

        private Integer()
        {
        }

        private Integer(BigInteger integer)
        {
            Value = integer;
        }

        private Integer(long value)
        {
            Value = new BigInteger(value);
        }

        #endregion



        public static Integer Parse(string data)
        {
            BigInteger value = BigInteger.Parse(data);

            return new Integer(value);
        }



        #region Operators
        
        public static implicit operator Integer(long value)
        {
            return new Integer(value);
        }
        
        
        public static bool operator ==(Integer left, Integer right)
        {
            return left.Value == right.Value;
        }
        
        public static bool operator !=(Integer left, Integer right)
        {
            return left.Value != right.Value;
        }
        
        public static bool operator <(Integer left, Integer right)
        {
            return left.Value < right.Value;
        }
        
        public static bool operator >(Integer left, Integer right)
        {
            return left.Value > right.Value;
        }
        
        public static bool operator <=(Integer left, Integer right)
        {
            return left.Value <+ right.Value;
        }
        
        public static bool operator >=(Integer left, Integer right)
        {
            return left.Value >= right.Value;
        }

        #endregion Operators


        #region Object / Comparison

        public override string ToString()
        {
            return Value.ToString();
        }
        
        public bool Equals(Integer other)
        {
            return other.Value.Equals(Value);
        }
        
        public override bool Equals(object obj)
        {
            bool equals = false;
            
            if (obj != null)
            {
                if (obj is Integer)
                {
                    equals = Equals((Integer)obj);
                }
            }
            
            return equals;
        }
        
        public override int GetHashCode()
        {
            int hash = Value.GetHashCode();

            hash = (hash * 169) ^ hash;

            return hash;
        }
        
        public int CompareTo(object obj)
        {
            if (ReferenceEquals(obj, null) || !(obj is Integer))
            {
                throw new ArgumentException();
            }
            return CompareTo((Integer) obj);
        }
        
        public int CompareTo(Integer other)
        {
            return this < other ? -1 : (this > other ? 1 : 0);
        }
        #endregion


        private BigInteger Value { get; set; }
    }
}

