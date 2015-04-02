using System;

namespace Atmosphere.SexyLib
{
    public class FileNotFoundException : Exception
    {
        public FileNotFoundException(string filename)
            : base(String.Format("File does not exist: {0}", filename))
        {
        }
    }
}

