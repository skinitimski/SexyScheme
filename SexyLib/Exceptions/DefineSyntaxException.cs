using System;

namespace Atmosphere.SexyLib.Exceptions
{
    public class DefineSyntaxException : BadSyntaxException
    {
        public DefineSyntaxException(string msg, params object[] parts)
            : base(String.Format("define-syntax: {0}", String.Format(msg, parts)))
        {
        }
    }
}

