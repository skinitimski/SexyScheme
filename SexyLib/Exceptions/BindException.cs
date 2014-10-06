using System;

namespace Atmosphere.SexyLib.Exceptions
{
    public class BindException : SexySchemeException
    {
        /// <summary>
        /// Creates a new <see cref="BindException" />. 
        /// </summary>
        /// <param name="symbolName">Name of the undefined symbol which threw the exception</param>
        public BindException(string symbolName) 
            : base(String.Format("set!: cannot bind undefined identifier: {0}", symbolName))
        {
        }
    }
}

