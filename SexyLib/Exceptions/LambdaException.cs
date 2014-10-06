using System;

namespace Atmosphere.SexyLib.Exceptions
{
    public class LambdaException : BadSyntaxException
    {
        public LambdaException(string msg, params object[] parts)
            : base(String.Format("lambda: {0}", String.Format(msg, parts)))
        {
        }
    }
}

