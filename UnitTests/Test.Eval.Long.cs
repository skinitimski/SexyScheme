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
        public void TestEvalLong()
        {
            Atom orig, evaluated;

            
            orig = new Atom(1056234987L, AtomType.LONG);
            evaluated = (Atom)Evaluator.Eval(orig);
            
            Assert.AreSame(orig, evaluated);
            Assert.AreEqual(orig, evaluated);
            Assert.AreEqual(1056234987L, (long)evaluated.Value);
        }
    }
}

