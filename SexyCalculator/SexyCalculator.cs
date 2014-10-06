using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using Atmosphere.Extensions;

using Atmosphere.SexyLib;

namespace Atmosphere.SexyCalculator
{
    public class SexyCalculator
    {



        public static void Main(string[] args)
        {   
            foreach (string arg in args)
            {
                Console.WriteLine("> " + arg);

                ISExp sexp = SexyParser.Parse(arg);

                ISExp eval = new SexyEvaluator().Eval(sexp);

                Console.WriteLine(eval);
            }
        }
    }
}
