using System;

namespace Atmosphere.SexyLib
{
    public static class Extensions
    {
        public static string GetTypeString(this ISExp sexp)
        {
            string type = "(null)";

            if (sexp != null)
            {
                if (sexp.IsAtom)
                {
                    Atom atom = sexp as Atom;

                    type = atom.Type.ToString();
                }
                else
                {

                    type = "pair";
                }
            }

            return type;
        }
    }
}

