using System;

namespace Atmosphere.SexyLib.Exceptions
{
    public class WrongTypeException : SexySchemeException
    {
        public WrongTypeException(Type givenType, Type actualType)
            : base(String.Format("Cannot cast object of type {0} to type {1}", actualType.Name, givenType.Name))
        {
        }
    }
}

