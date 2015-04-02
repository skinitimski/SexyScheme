using System;
using System.Text.RegularExpressions;

namespace Atmosphere.SexyLib
{
    public static partial class Primitives
    {        
        [PrimitiveMethod("regex-match")]
        public static ISExp Match(string name, params ISExp[] parameters)
        {
            CheckArity(name, 2, parameters);
            
            CheckType(IsString, name, 0, "text", parameters);
            CheckType(IsString, name, 1, "text", parameters);
            
            Regex regex = new Regex((String)((Atom)parameters[0]).Value);
            string content = (String)((Atom)parameters[0]).Value;

            Match m = regex.Match(content);
            
            ISExp result = Atom.Null;
            
            if (m.Success)
            {
                result = Atom.CreateString(m.Value);
            }
            
            return result;
        }

        [PrimitiveMethod("regex-matches")]
        public static ISExp Matches(string name, params ISExp[] parameters)
        {
            CheckArity(name, 2, parameters);
            
            CheckType(IsString, name, 0, "text", parameters);
            CheckType(IsString, name, 1, "text", parameters);
            
            Regex regex = new Regex((String)((Atom)parameters[0]).Value);
            string content = (String)((Atom)parameters[1]).Value;

            int start = 0;
            Match m;

            ISExp result = Pair.Empty;

            while (true)
            {
                m = regex.Match(content, start);

                if (m.Success)
                {
                    result = Pair.Cons(Atom.CreateString(m.Value), result);
                    start = m.Index + m.Length;
                }
                else
                {
                    break;
                }
            }
            
            return result;
        }
    }
}

