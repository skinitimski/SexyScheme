using System;

namespace Atmosphere.SexyLib.Exceptions
{
    public class SexySchemeImplementationException : SexySchemeException
    {
        public SexySchemeImplementationException(string msg, params object[] parts)
            : base(msg, parts)
        {
        }
        
        /// <summary>
        /// Creates a new <see cref="SexySchemeException" />.
        /// </summary>
        /// <param name="msg">Exception message</param>
        /// <param name="inner">Inner exception</param>
        public SexySchemeImplementationException(Exception inner, string msg) 
            : base(msg, inner)
        {
        }
    }
}

