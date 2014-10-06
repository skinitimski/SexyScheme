using System;

namespace Atmosphere.SexyLib.Exceptions
{
    public class NotEnoughArgumentsException : ProcedureApplicationException
    {
        public NotEnoughArgumentsException(string functionName, int expectedArgumentCount, int actualArgumentCount)
            : base(String.Format("{0}: expects at least <{1}> argument{2}, given {3}", functionName, expectedArgumentCount, expectedArgumentCount == 1 ? "" : "s", actualArgumentCount))
        {
        }
    }
}

