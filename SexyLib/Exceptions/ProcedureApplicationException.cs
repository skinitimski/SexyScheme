using System;

namespace Atmosphere.SexyLib.Exceptions
{
    public class ProcedureApplicationException : SexySchemeException
    {
        public ProcedureApplicationException(ISExp given)
            : base(String.Format("Not a procedure: {0}" , given))
        {
        }

        protected ProcedureApplicationException(string msg)
            : base(msg)
        {
        }
    }
}

