using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Reflection;

using Atmosphere.Extensions;

using Atmosphere.SexyLib;
using Atmosphere.SexyLib.Exceptions;

namespace Atmosphere.SexyInterpreter
{
    public class SexyInterpreter
    {
        public SexyInterpreter()
        {
        }

        public void Init()
        {
            Evaluator = new SexyEvaluator();

            foreach (string resourceFilename in new[] { "Sexy.Utilities.scm" })
            {
                string code = null;

                using (Stream resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceFilename))
                using (StreamReader reader = new StreamReader(resourceStream))
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

        public void Run()
        {
            Init();

            while (true)
            {
                Console.Write("> ");
                
                string sexpString = Console.ReadLine();

                int index = 0;

                while (index < sexpString.Length && !SexyParser.IsAllWhitespaceAndComments(sexpString, index))
                {
                    ISExp orig, evaluated;
                
                    try
                    {
                        orig = SexyParser.Parse(sexpString, ref index);
                
                        evaluated = Evaluator.Eval(orig);
                    }
                    catch (SexySchemeException e)
                    {
                        Console.WriteLine(e.ToString());
                        continue;
                    }

                    if (!evaluated.Equals(Atom.Null))
                    {
                        Console.WriteLine(evaluated.ToString());
                    }
                }
            }
        }

        private SexyEvaluator Evaluator { get; set; }

        public static void Main(string[] args)
        {   
            SexyInterpreter interpreter = new SexyInterpreter();

            try
            {
                interpreter.Run();
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} : {1}{2}{3}", e.GetType().Name, e.Message, Environment.NewLine, e.StackTrace);
                Environment.Exit(1);
            }
        }
    }
}
