using System;

namespace Atmosphere.SexyLib.Exceptions
{
    public class UndefinedSymbolException : SexySchemeException
    {
        /// <summary>
        /// Creates a new <see cref="UndefinedSymbolException" />. 
        /// </summary>
        /// <param name="symbolName">Name of the undefined symbol which threw the exception</param>
        public UndefinedSymbolException(string symbolName) 
            : base(String.Format("Reference to an undefined identifier: {0}", symbolName))
        {
        }
    }
}

