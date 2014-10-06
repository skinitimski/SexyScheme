using System;

namespace Atmosphere.SexyLib.Exceptions
{
    public class BadSyntaxException : SexySchemeException
    {
        public BadSyntaxException(string syntaxKeyword, ISExp sexp)
            : base(String.Format("{0}: bad syntax in {1}",syntaxKeyword, sexp))
        {
        }

        public BadSyntaxException(string syntaxKeyword, ISExp sexp, string msg)
            : base(String.Format("{0}: bad syntax in {1} ({2})",syntaxKeyword, sexp, msg))
        {
        }

        protected BadSyntaxException(string msg, params object[] parts)
           : base(msg, parts)
        {
        }
    }
}

