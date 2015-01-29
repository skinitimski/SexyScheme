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

        public static ISExp Car(string name, params ISExp[] parameters)
        {
            CheckArity(name, 1, parameters);

            // Note that IsPair returns false for the empty list
            CheckType(IsPair, name, 0, "pair", parameters[0]);

            Pair pair = parameters[0] as Pair;
            
            return pair.Car;
        }

        public static ISExp Cdr(string name, params ISExp[] parameters)
        {
            CheckArity(name, 1, parameters);

            // Note that IsPair returns false for the empty list
            CheckType(IsPair, name, 0, "pair", parameters[0]);
            
            Pair list = parameters[0] as Pair;
            
            return list.Cdr;
        }

        public static ISExp Cons(string name, params ISExp[] parameters)
        {
            CheckArity(name, 2, parameters);

            return Pair.Cons(parameters[0], parameters[1]);
        }
    }
}

