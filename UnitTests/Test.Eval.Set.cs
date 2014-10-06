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
    public partial class TestEval
    {
        [Test]
        public void TestEvalSetFail()
        {
            Assert.Throws<BindException>(() => Evaluator.Eval(SexyParser.Parse("(set! x 1)")));
        }

        [Test]
        public void TestEvalSet()
        {
            ISExp sexp;

            Evaluator.Closure.AddSymbolDefinition("x", new Atom(1L, AtomType.LONG));    
            
            
            sexp = Evaluator.Eval(SexyParser.Parse("(set! x 2)"));
            
            Assert.AreEqual(Atom.Null, sexp);           
            Assert.AreEqual(2L, (long)((Atom)Evaluator.Closure.Resolve("x")).Value);
            
            
            sexp = Evaluator.Eval(SexyParser.Parse("(set! x \"string\")"));            
            
            Assert.AreEqual(Atom.Null, sexp);           
            Assert.AreEqual("string", (string)((Atom)Evaluator.Closure.Resolve("x")).Value);
        }
    }
}
