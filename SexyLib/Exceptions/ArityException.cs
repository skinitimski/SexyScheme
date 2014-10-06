using System;

namespace Atmosphere.SexyLib.Exceptions
{
    public class ArityException : ProcedureApplicationException
    {
        public ArityException(string functionName, int expectedArgumentCount, int actualArgumentCount)
            : base(String.Format("{0}: expects <{1}> argument{2}, given {3}", functionName, expectedArgumentCount, expectedArgumentCount == 1 ? "" : "s", actualArgumentCount))
        {
        }

        public ArityException(string functionName, int minArgumentCount, int maxArgumentCount, int actualArgumentCount)
            : base(String.Format("{0}: expects {1} to {2} arguments, given {3}", functionName, minArgumentCount, maxArgumentCount, actualArgumentCount))
        {
        }
    }
}

