using System;

namespace Atmosphere.SexyLib.Exceptions
{
    public class DefineException : BadSyntaxException
    {
        public DefineException(string msg, params object[] parts)
            : base(String.Format("define: {0}", String.Format(msg, parts)))
        {
        }
    }
}

