using System;

namespace Atmosphere.SexyLib.Exceptions
{
    public class DotException : BadSyntaxException
    {
        public DotException()
            : base("Illegal use of '.'")
        {
        }
    }
}

