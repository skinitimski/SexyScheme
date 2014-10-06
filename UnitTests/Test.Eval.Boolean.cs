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
        public void TestEvalBoolean()
        {
            Atom orig, evaluated;


            orig = new Atom(true, AtomType.BOOLEAN);
            evaluated = (Atom)Evaluator.Eval(orig);
            
            Assert.AreSame(orig, evaluated);
            Assert.AreEqual(orig, evaluated);
            Assert.IsTrue((bool)(evaluated.Value));
            
            
            orig = new Atom(false, AtomType.BOOLEAN);
            evaluated = (Atom)Evaluator.Eval(orig);
            
            Assert.AreSame(orig, evaluated);
            Assert.AreEqual(orig, evaluated);
            Assert.IsFalse((bool)(evaluated.Value));
        }
    }
}

