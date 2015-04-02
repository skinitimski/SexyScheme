using System;
using System.IO;
using System.Linq;

using Atmosphere.SexyLib.Exceptions;

namespace Atmosphere.SexyLib
{
    public static partial class Primitives
    {        
        [PrimitiveMethod("file-content")]
        public static ISExp FileContent(string name, params ISExp[] parameters)
        {
            CheckArity(name, 1, parameters);

            CheckType(IsText, name, 0, "text", parameters);
            
            string filename = GetAsText(parameters[0]);

            string content = File.ReadAllText(filename);
            
            return Atom.CreateString(content);
        }
    }
}