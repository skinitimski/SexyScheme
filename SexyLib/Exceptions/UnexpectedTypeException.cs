using System;

namespace Atmosphere.SexyLib.Exceptions
{
    public class UnexpectedTypeException : SexySchemeException
    {
        public UnexpectedTypeException(string functionName, int index, string expectedType, ISExp value)
            : base(String.Format("{0}: expects type <{1}> for parameter {2}, given: {3}", functionName, expectedType, index, value))
        {
        }
    }
}

