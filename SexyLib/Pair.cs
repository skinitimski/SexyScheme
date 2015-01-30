using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Atmosphere.Extensions;
using Atmosphere.SexyLib.Exceptions;

namespace Atmosphere.SexyLib
{
    public class Pair : ISExp
    {
        public static readonly Pair Empty = new Pair();

        private Pair()
        {
        }

        private Pair(ISExp car, ISExp cdr)
        {
            Car = car;
            Cdr = cdr;
        }

        public static Pair Cons(ISExp car, ISExp cdr)
        {
            return new Pair(car, cdr);
        }

        public static Pair List(params ISExp[] elements)
        {
            Pair pair = Empty;

            if (elements.Length > 0)
            {
                for (int i = elements.Length - 1; i >= 0; i--)
                {
                    pair = Pair.Cons(elements[i], pair);
                }
            }

            return pair;
        }

        public ISExp Clone()
        {
            Pair clone = Cons(Car.Clone(), Cdr.Clone());
            
            return clone;
        }
        
        public override bool Equals(object obj)
        {
            bool equals = false;
            
            if (obj != null)
            {
                Pair that = obj as Pair;
                
                if (that != null)
                {
                    ISExp thisCar = this.Car;
                    ISExp thatCar = that.Car;

                    // Either both cars are null or they're both not null AND are equal
                    if ((thisCar == null && thatCar == null) || (thisCar != null && thatCar != null && thisCar.Equals(thatCar)))
                    {
                        ISExp thisCdr = this.Cdr;
                        ISExp thatCdr = that.Cdr;

                        if ((thisCdr == null && thatCdr == null) || (thisCdr != null && thatCdr != null && thisCdr.Equals(thatCdr)))
                        {
                            equals = true;
                        }
                    }
                }
            }
            
            return equals;
        }

        public List<ISExp> ToList()
        {
            List<ISExp> elements = new List<ISExp>();

            Pair pair = this;

            while (pair.Cdr != null)
            {
                elements.Add(pair.Car);

                pair = pair.Cdr as Pair;

                if (pair == null)
                {
                    throw new InvalidOperationException("Cannot ToList() an improper list.");
                }
            }

            return elements;
        }

        public ISExp[] ToArray()
        {
            return ToList().ToArray();
        }
        
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
                        
            sb.Append("(");

            Pair pair = this;

            while (true)
            {
                if (!pair.IsEmpty)
                {
                    if (sb.Length > 1)
                    {
                        sb.Append(" ");
                    }

                    sb.Append(pair.Car.ToString());

                    if (pair.Cdr.IsAtom)
                    {
                        // Improper list -- add dot then quit
                        sb.Append(" . ");
                        sb.Append(pair.Cdr.ToString());

                        break;
                    }
                }
                else
                {
                    // Empty list -- nothing to represent
                    break;
                }
                    
                pair = (Pair)pair.Cdr;
            }
            
            sb.Append(")");
            
            return sb.ToString();
        }

        public string ToDisplay()
        {
            return ToString();
        }

        public ISExp Car { get; set; }

        public ISExp Cdr { get; set; }

        public bool IsAtom { get { return false; } }

        public bool IsEmpty { get { return this.Equals(Pair.Empty); } }
    }
}
