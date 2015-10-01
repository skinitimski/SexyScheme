using System;
using File = System.IO.File;
using StreamReader = System.IO.StreamReader;
using System.Linq;

using Atmosphere.SexyLib.Exceptions;

namespace Atmosphere.SexyLib
{
    public static partial class Primitives
    {        
        [PrimitiveMethod("write")]
        public static ISExp Write(string name, params ISExp[] parameters)
        {
            CheckArity(name, 1, parameters);
            
            Console.WriteLine(parameters[0].ToString());
            
            return Atom.Null;
        }
        
        [PrimitiveMethod("display")]
        public static ISExp Display(string name, params ISExp[] parameters)
        {
            CheckArity(name, 1, parameters);
            
            Console.WriteLine(parameters[0].ToDisplay());
            
            return Atom.Null;
        }
        
        [PrimitiveMethod("newline")]
        public static ISExp Newline(string name, params ISExp[] parameters)
        {
            CheckArity(name, 0, parameters);

            Console.WriteLine();

            return Atom.Null;
        }
        
        [PrimitiveMethod("exit")]
        public static ISExp Exit(string name, params ISExp[] parameters)
        {
            CheckArity(name, 0, 1, parameters);

            if (parameters.Length == 0)
            {
                Environment.Exit(0);
                return Atom.Null;
            }
            else
            {
                if (!IsLong(parameters[0]))
                {
                    throw new UnexpectedTypeException(name, 0, "integer", parameters[0]);
                }

                Environment.Exit((int)(long)((Atom)parameters[0]).Value);
                return Atom.Null;
            }
        }
        
        
                public static ISExp LoadFile(string name, params ISExp[] parameters)
                {     
                    CheckEnoughArguements(name, 1, parameters);
        
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        ISExp arg = parameters[i];
        
                        if (!(IsString(arg) || IsSymbol(arg)))
                        {
                            string filename = (String)((Atom)arg).Value;
        
                            if (!File.Exists(filename))
                            {
                                throw new FileNotFoundException(filename);
                            }
        
                            string code;
        
                            using (StreamReader reader = new StreamReader(filename))
                            {
                                code = reader.ReadToEnd();
                            }
                            
                            int index = 0;
                            
                            while (index < code.Length && !SexyParser.IsAllWhitespaceAndComments(code, index))
                            {
                                ISExp sexp = SexyParser.Parse(code, ref index);
                                
                                sexp = Evaluator.Eval(sexp);
                                
                                if (!sexp.Equals(Atom.Null))
                                {
                                    Console.WriteLine(sexp.ToString());
                                }
                            }
        
                        }
                    }
                }

    }
}

