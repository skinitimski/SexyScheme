using System;

namespace Atmosphere.SexyLib.Exceptions
{
    public class UnsupportedOperationException : SexySchemeException
    {
        /// <summary>
        /// Creates a new <see cref="UndefinedOperationException" />. 
        /// </summary>
        public UnsupportedOperationException(string msg, params object[] parts) : base(String.Format(msg, parts))
        {
        }
    }
}

