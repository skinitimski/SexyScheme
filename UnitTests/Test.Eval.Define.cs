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
        public void TestEvalDefine()
        {
            ISExp orig, evaluated;


            int count = Evaluator.Closure.Count;


            orig = SexyParser.Parse("(define x (quote (1 2)))");
            evaluated = Evaluator.Eval(orig);

            Assert.AreEqual(Atom.Null, evaluated);           
            Assert.AreEqual(++count, Evaluator.Closure.Count);

            
            
            
            orig = SexyParser.Parse("(define y x)");
            evaluated = Evaluator.Eval(orig);
            
            Assert.AreEqual(Atom.Null, evaluated);           
            Assert.AreEqual(++count, Evaluator.Closure.Count);




            Assert.AreSame(Evaluator.Closure.Resolve("x"), Evaluator.Closure.Resolve("y"));
        }
    }
}
