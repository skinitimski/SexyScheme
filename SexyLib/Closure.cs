using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Atmosphere.SexyLib.Exceptions;

namespace Atmosphere.SexyLib
{
    public partial class Closure
    {       
        private Closure()
        {
            Map = new Dictionary<string, ISExp>();
            Parent = null;
        }

        public static Closure Create()
        {
            return new Closure();
        }

        public static Closure Create(Closure parent)
        {
            Closure closure = new Closure();
            closure.Parent = parent;

            return closure;
        }

        public ISExp Resolve(string symbol)
        {
            if (!ContainsSymbol(symbol))
            {
                if (Parent == null)
                {
                    throw new UndefinedSymbolException(symbol);
                }
                else
                {
                    return Parent.Resolve(symbol);
                }
            }

            return this[symbol];
        }

        public void AddSymbolDefinition(string symbol, ISExp sexp)
        {
            Map.Add(symbol, sexp);
        }

        public void Define(string symbol, ISExp sexp)
        {
            Map[symbol] = sexp;
        }

        public void Bind(string symbol, ISExp sexp)
        {
            if (!ContainsSymbol(symbol))
            {
                throw new BindException(symbol);
            }

            Map[symbol] = sexp;
        }

        public bool ContainsSymbol(string symbol)
        {
            return Map.ContainsKey(symbol);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine(new String('-', 78));

            if (Map.Count > 0)
            {
                string indent = new String(' ', Depth);

                int longest = LongestSymbol;

                foreach (String symbol in Map.Keys)
                {
                    builder.AppendFormat("{0}{1,-" + longest + "} => ", indent, symbol);
                    builder.AppendLine(Map[symbol].ToString());
                }

                if (Parent != null)
                {
                    builder.Append(Parent.ToString());
                }
            }

            return builder.ToString();
        }

        private ISExp this[string symbol]
        {
            get
            {
                return Map[symbol];
            }
        }

        private Dictionary<string, ISExp> Map { get; set; }

        private Closure Parent { get; set; }

        public int Count { get { return Map.Count; } }

        private int Depth
        {
            get
            {
                int depth = 0;

                Closure closure = Parent;

                while (closure != null)
                {
                    depth++;
                    closure = closure.Parent;
                }

                return depth;
            }
        }

        private int LongestSymbol
        {
            get
            {
                int longest = 0;

                if (Map.Count > 0)
                {
                    longest = Map.Keys.Max(x => x.Length);
                }
                if (Parent != null)
                {
                    longest = Math.Max(longest, Parent.LongestSymbol);
                }

                return longest;
            }
        }
    }
}

