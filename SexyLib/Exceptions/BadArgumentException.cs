using System;

namespace Atmosphere.SexyLib.Exceptions
{
    public class BadArgumentException : SexySchemeException
    {
        public BadArgumentException(string expectation, string given)
            : base(String.Format("Expected {0}, given {1}", expectation, given))
        {
        }
    }
}

