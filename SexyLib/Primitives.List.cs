using System;
using System.Linq;

using Atmosphere.SexyLib.Exceptions;

namespace Atmosphere.SexyLib
{
    public static partial class Primitives
    {        
//        public static ISExp List(params ISExp[] parameters)
//        {
//            List list = new List();
//
//            foreach (ISExp parameter in parameters)
//            {
//                list.AppendMember(parameter);
//            }
//            
//            return list;
//        }

        [PrimitiveMethod("length")]
        public static ISExp Length(string name, params ISExp[] parameters)
        {
            Pair pair = parameters[0] as Pair;

            if (pair == null)
            {
                throw new UnexpectedTypeException(name, 0, "proper list", parameters[0]);
            }

            int length = 0;

            while (!pair.IsEmpty)
            {
                length++;

                pair = pair.Cdr as Pair;

                if (pair == null)
                {
                    throw new UnexpectedTypeException(name, 0, "proper list", parameters[0]);
                }
            }

            return Atom.CreateLong(length);
        }

        [PrimitiveMethod("car")]
        public static ISExp Car(string name, params ISExp[] parameters)
        {
            CheckArity(name, 1, parameters);

            // Note that IsPair returns false for the empty list
            CheckType(IsPair, name, 0, "pair", parameters);

            Pair pair = parameters[0] as Pair;
            
            return pair.Car;
        }
        
        [PrimitiveMethod("cdr")]
        public static ISExp Cdr(string name, params ISExp[] parameters)
        {
            CheckArity(name, 1, parameters);

            // Note that IsPair returns false for the empty list
            CheckType(IsPair, name, 0, "pair", parameters);
            
            Pair list = parameters[0] as Pair;
            
            return list.Cdr;
        }
        
        [PrimitiveMethod("cons")]
        public static ISExp Cons(string name, params ISExp[] parameters)
        {
            CheckArity(name, 2, parameters);

            return Pair.Cons(parameters[0], parameters[1]);
        }
    }
}

