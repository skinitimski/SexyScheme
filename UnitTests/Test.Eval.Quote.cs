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
    public partial class TestEval : SexyLibTestFixture
    {
        [Test]
        public void TestEvalQuoteFail()
        {
            Assert.Throws<BadSyntaxException>(() => Evaluator.Eval(Pair.Cons(new Atom("quote", AtomType.SYMBOL), new Atom(1, AtomType.LONG))));
            Assert.Throws<BadSyntaxException>(() => Evaluator.Eval(SexyParser.Parse("(quote)")));
            Assert.Throws<BadSyntaxException>(() => Evaluator.Eval(SexyParser.Parse("(quote 1 2)")));
        }

        [Test]
        public void TestEvalQuote()
        {
            ISExp orig, evaluated;
            
            
            orig = SexyParser.Parse("(quote x)");
            evaluated = Evaluator.Eval(orig);
            
            Assert.AreEqual("x", (string)((Atom)evaluated).Value);
            
            
            orig = SexyParser.Parse("'x");
            evaluated = Evaluator.Eval(orig);
            
            Assert.AreEqual("x", (string)((Atom)evaluated).Value);
        }
    }
}
