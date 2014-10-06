using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using NUnit.Framework;

using Atmosphere.Extensions;
using Atmosphere.SexyLib;
using Atmosphere.SexyLib.Exceptions;

namespace Atmosphere.UnitTests
{
    [TestFixture]
    public partial class TestWrite : SexyLibTestFixture
    {
        [Test]
        public void TestWritePair()
        {
            ISExp sexp;
            string rep;
            
            
            rep = "()";
            sexp = SexyParser.Parse(rep);
            Assert.AreEqual(rep, sexp.ToString());
            
            
            rep = "(1 2 3 4)";
            sexp = SexyParser.Parse(rep);
            Assert.AreEqual(rep, sexp.ToString());
            
            
            rep = "(cons 1 ())";
            sexp = Evaluator.Eval(SexyParser.Parse(rep));
            Assert.AreEqual("(1)", sexp.ToString());
            
            
            rep = "(cons 1 (cons 2 ()))";
            sexp = Evaluator.Eval(SexyParser.Parse(rep));
            Assert.AreEqual("(1 2)", sexp.ToString());
            
            
            rep = "(cons 1 2)";
            sexp = Evaluator.Eval(SexyParser.Parse(rep));
            Assert.AreEqual("(1 . 2)", sexp.ToString());
            
            
            rep = "(1 (2 (3 (4))))";
            sexp = Evaluator.Eval(SexyParser.Parse(String.Format("(quote {0})", rep)));
            Assert.AreEqual(rep, sexp.ToString());
        }
    }
}

