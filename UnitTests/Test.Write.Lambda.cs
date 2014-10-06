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
        public void TestWriteFixedLambda()
        {
            ISExp sexp;
            string rep;
            
            rep = "(lambda (x) x)";
            sexp = Evaluator.Eval(SexyParser.Parse(rep));
            Assert.AreEqual(rep, sexp.ToString());
            
            rep = "(lambda (x y) x)";
            sexp = Evaluator.Eval(SexyParser.Parse(rep));
            Assert.AreEqual(rep, sexp.ToString());
            
            rep = "(lambda (x why z) why)";
            sexp = Evaluator.Eval(SexyParser.Parse(rep));
            Assert.AreEqual(rep, sexp.ToString());
        }

        [Test]
        public void TestWriteVariableLambda()
        {
            ISExp sexp;
            string rep;
            
            rep = "(lambda x x)";
            sexp = Evaluator.Eval(SexyParser.Parse(rep));
            Assert.AreEqual(rep, sexp.ToString());
            
            rep = "(lambda x (+ (car x) 1))";
            sexp = Evaluator.Eval(SexyParser.Parse(rep));
            Assert.AreEqual(rep, sexp.ToString());
            
            rep = "(lambda x x x)";
            sexp = Evaluator.Eval(SexyParser.Parse(rep));
            Assert.AreEqual(rep, sexp.ToString());
        }

         [Test]
        public void TestWriteRestLambda()
        {
            ISExp sexp;
            string rep;
            
            rep = "(lambda (x . y) x)";
            sexp = Evaluator.Eval(SexyParser.Parse(rep));
            Assert.AreEqual(rep, sexp.ToString());
            
            rep = "(lambda (x y . z) x)";
            sexp = Evaluator.Eval(SexyParser.Parse(rep));
            Assert.AreEqual(rep, sexp.ToString());
        }
    }
}

