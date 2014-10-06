using System;

namespace Atmosphere.SexyLib.Exceptions
{
    public class UndefinedOperationException : ProcedureApplicationException
    {
        /// <summary>
        /// Creates a new <see cref="UndefinedOperationException" />. 
        /// </summary>
        public UndefinedOperationException(string msg, params object[] parts) : base(String.Format(msg, parts))
        {
        }
    }
}

