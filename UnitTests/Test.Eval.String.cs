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
        public void TestEvalString()
        {
            Atom orig, evaluated;


            orig = Atom.CreateString("string");
            evaluated = (Atom)Evaluator.Eval(orig);
            
            Assert.AreSame(orig, evaluated);
            Assert.AreEqual(orig, evaluated);
            Assert.AreEqual("string", (string)evaluated.Value);
        }
    }
}

