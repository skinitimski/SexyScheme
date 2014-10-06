using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Atmosphere.SexyLib.Exceptions;

namespace Atmosphere.SexyLib
{
    public class Atom : ISExp
    {
        public static readonly Atom True = new Atom(true, AtomType.BOOLEAN);
        public static readonly Atom False = new Atom(false, AtomType.BOOLEAN);

        public static readonly Atom Null = new Atom(null, AtomType.NONE);
                
        public static readonly Atom KeywordLambda = new Atom("lambda", AtomType.SYMBOL);
        public static readonly Atom KeywordQuote = new Atom("quote", AtomType.SYMBOL);
        public static readonly Atom KeywordDefine = new Atom("define", AtomType.SYMBOL);
        public static readonly Atom KeywordDefineSyntax = new Atom("define-syntax", AtomType.SYMBOL);
        public static readonly Atom KeywordSet = new Atom("set!", AtomType.SYMBOL);






        private Atom()
        {

        }

        public Atom(object value, AtomType type)
        {
            Value = value;
            Type = type;
        }

        public Atom(Primitive lambda)
        {
            Value = lambda;
            Type = AtomType.PRIMITIVE;
        }


        
        
        /// <summary>
        /// Creates a lambda object with fixed OR variable arity (using dot notation). 
        /// The first N-1 arguments to the function will be bound to the first N-1 formals,
        /// and the remaining arguments will be converted to a list and bound to the Nth formal
        /// (where N is the number of formals). For fixed arity (no dot notation), the first
        /// N arguments will be bound to the first N formals.
        /// </summary>
        /// <returns>
        /// An Atom object representing the given lambda expression.
        /// </returns>
        /// <param name='formal'>Formals of the lambda expression</param>
        /// <param name='body'>Body of the lambda expression</param>
        public static Atom CreateLambda(List<Atom> formals, List<ISExp> body)
        {
            Atom atom = new Atom();
            atom.Type = AtomType.LAMBDA;

            if (formals.Count >= 3 && (string)formals[formals.Count - 2].Value == ".")
            {
                atom.Value = new RestLambda(formals.GetRange(0, formals.Count - 2), formals[formals.Count - 1], body);
            }
            else
            {
                atom.Value = new FixedLambda(formals, body);
            }

            return atom;
        }

        public static Atom CreateLong(long value)
        {
            return new Atom(value, AtomType.LONG);
        }

        public static Atom CreateDouble(double value)
        {
            return new Atom(value, AtomType.DOUBLE);
        }

        /// <summary>
        /// Creates a lambda object with variable arity. Arguments to the function will
        /// be converted to a list and bound to the given formal.
        /// </summary>
        /// <returns>
        /// An Atom object representing the given lambda expression.
        /// </returns>
        /// <param name='formal'>Formal of the lambda expression</param>
        /// <param name='body'>Body of the lambda expression</param>
        public static Atom CreateLambda(Atom formal, List<ISExp> body)
        {
            Atom atom = new Atom();
            atom.Type = AtomType.LAMBDA;
            atom.Value = new VariableLambda(formal, body);

            return atom;
        }


        public ISExp Clone()
        {
            Atom clone = new Atom();

            clone.Value = Value;
            clone.Type = Type;

            return clone;
        }



        public override bool Equals(object obj)
        {
            bool equals = false;
            
            if (obj != null)
            {
                Atom that = obj as Atom;
                
                if (that != null)
                {
                    if (this.Type == that.Type)
                    {
                        if (this.Type.Equals(AtomType.SYMBOL))
                        {
                            string thisSymbol = (string)this.Value;
                            string thatSymbol = (string)that.Value;

                            equals = thisSymbol.ToLower().Equals(thatSymbol.ToLower());
                        }
                        else
                        {
                            object thisValue = this.Value;
                            object thatValue = that.Value;
                            
                            if ((thisValue == null && thatValue == null) || (thisValue != null && thatValue != null && thisValue.Equals(thatValue)))
                            {
                                equals = true;
                            }
                        }
                    }
                }
            }
            
            return equals;
        }

        public override string ToString()
        {
            string rep = null;

            switch (Type)
            {                
                case AtomType.NONE:
                    rep = "#<void>";
                    break;

                case AtomType.BOOLEAN:
                    if ((bool)Value)
                    {
                        rep = "#t";
                    }
                    else
                    {
                        rep = "#f";
                    }
                    break;

                case AtomType.CHAR:
                    rep = "#\\";

                    if (Value.Equals(' '))
                    {
                        rep += "space";
                    }
                    else if (Value.Equals('\n'))
                    {
                        rep += "newline";
                    }
                    else
                    {
                        rep += Value;
                    }

                    break;

                case AtomType.STRING:
                    rep = String.Format("\"{0}\"", Value);
                    break;
                    
                case AtomType.LAMBDA:
                case AtomType.DOUBLE:
                case AtomType.LONG:
                case AtomType.SYMBOL:
                    rep = Value.ToString().ToLower();
                    break;

                case AtomType.PRIMITIVE:
                    Primitive primitive = (Primitive)Value;
                    rep = "#<primitive:" + primitive.Method.Name.ToLower() + ">";
                    break;

                default:
                    throw new Exception(String.Format("No support yet for type {0}", Type));
            }

            return rep;
        }

        public string ToDisplay()
        {
            string rep = null;
            
            switch (Type)
            {                
                case AtomType.NONE:
                    rep = "#<void>";
                    break;

                case AtomType.CHAR:
                case AtomType.STRING:
                    rep =  Value.ToString();                                        
                    break;

                case AtomType.LAMBDA:
                case AtomType.DOUBLE:
                case AtomType.LONG:
                case AtomType.SYMBOL:
                    rep = Value.ToString().ToLower();
                    break;
                    
                case AtomType.PRIMITIVE:
                    Primitive primitive = (Primitive)Value;
                    rep = "#<primitive:" + primitive.Method.Name.ToLower() + ">";
                    break;
                    
                default:
                    throw new Exception(String.Format("No support yet for type {0}", Type));
            }
            
            return rep;
        }

        
        public long GetValueAsLong()
        {
            long ret = 0L;
            
            if (Type == AtomType.LONG)
            {
                ret = (long)Value;
            }
            else
            {
                throw new WrongTypeException(typeof(long), Value.GetType());
            }
            
            return ret;
        }
        
        public double GetValueAsDouble()
        {
            double ret = 0.0D;
            
            if (Type == AtomType.LONG)
            {
                ret = (long)Value;
            }
            else if (Type == AtomType.DOUBLE)
            {
                ret = (double)Value;
            }
            else
            {
                throw new WrongTypeException(typeof(double), Value.GetType());
            }
            
            return ret;
        }

        public bool IsAtom { get { return true; } }

        public object Value { get; private set; }

        public AtomType Type { get; private set; }
    }
}

