using System;

namespace Atmosphere.SexyLib.Exceptions
{
    /// <summary>
    /// Exception class used when an external file cannot be found.
    /// </summary>
    public class FileNotFoundException : SexySchemeException
    {
        /// <summary>
        /// Creates a new <see cref="SexpParserException" />. Treat the method signature just like you were passing
        /// parameters to String.Format(string, params string[]).
        /// </summary>
        /// <param name="msg">Message of the exception; may have formatting</param>
        /// <param name="parts">Parts to format into msg</param>
        public FileNotFoundException(string msg, params object[] parts) : base(String.Format(msg, parts))
        {
        }
        
        /// <summary>
        /// Creates a new <see cref="SexpParserException" />.
        /// </summary>
        /// <param name="msg">Exception message</param>
        /// <param name="inner">Inner exception</param>
        public FileNotFoundException(Exception inner, string msg) : base(msg, inner)
        {
        }
    }
}

