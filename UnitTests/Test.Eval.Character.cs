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
        public void TestEvalCharacter()
        {
            Atom orig, evaluated;


            orig = new Atom('t', AtomType.CHAR);
            evaluated = (Atom)Evaluator.Eval(orig);
            
            Assert.AreSame(orig, evaluated);
            Assert.AreEqual(orig, evaluated);
            Assert.AreEqual('t', (char)evaluated.Value);


            
            orig = new Atom(' ', AtomType.CHAR);
            evaluated = (Atom)Evaluator.Eval(orig);
            
            Assert.AreSame(orig, evaluated);
            Assert.AreEqual(orig, evaluated);
            Assert.AreEqual(' ', (char)evaluated.Value);
        }
    }
}

