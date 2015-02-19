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
        public void TestEvalSymbol()
        {
            Atom orig, evaluated;

            Evaluator.Closure.AddSymbolDefinition("true", Atom.True);
            Evaluator.Closure.AddSymbolDefinition("false", Atom.False);
            
            
            orig = Atom.CreateSymbol("true");
            evaluated = (Atom)Evaluator.Eval(orig);
            
            Assert.AreNotSame(orig, evaluated);
            Assert.AreNotEqual(orig, evaluated);
            Assert.AreEqual(true, (bool)evaluated.Value);
            
            
            orig = Atom.CreateSymbol("false");
            evaluated = (Atom)Evaluator.Eval(orig);
            
            Assert.AreNotSame(orig, evaluated);
            Assert.AreNotEqual(orig, evaluated);
            Assert.AreEqual(false, (bool)evaluated.Value);
        }
    }
}

