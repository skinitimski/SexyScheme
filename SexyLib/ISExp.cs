using System;

namespace Atmosphere.SexyLib
{
    public interface ISExp
    {
        bool IsAtom { get; }

        string ToString();
        string ToDisplay();

        ISExp Clone();
    }
}

