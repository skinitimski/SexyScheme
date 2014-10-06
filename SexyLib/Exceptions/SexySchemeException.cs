using System;

namespace Atmosphere.SexyLib.Exceptions
{
    public class SexySchemeException : Exception
    {
        public SexySchemeException(string msg, params object[] parts)
            : base(String.Format(msg, parts))
        {
        }
        
        /// <summary>
        /// Creates a new <see cref="SexySchemeException" />.
        /// </summary>
        /// <param name="msg">Exception message</param>
        /// <param name="inner">Inner exception</param>
        public SexySchemeException(Exception inner, string msg) 
            : base(msg, inner)
        {
        }
    }
}

