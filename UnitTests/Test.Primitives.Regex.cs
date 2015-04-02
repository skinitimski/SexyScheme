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
    public partial class TestPrimitives
    {
        [Test]
        public void TestRegexMatch()
        {
            ISExp orig, evaluated;   
            
            
            orig = SexyParser.Parse("(regex-match \"is\" \"This is a string\")");
            evaluated = Evaluator.Eval(orig);
            Assert.AreEqual("is", (String)((Atom)evaluated).Value); 
        }  

        [Test]
        public void TestRegexMatches()
        {
            ISExp orig, evaluated;   
            
            
            orig = SexyParser.Parse("(regex-matches \"is\" \"This is a string\")");
            evaluated = Evaluator.Eval(orig);
            Assert.AreEqual("is", (String)((Atom)((Pair)evaluated).Car).Value); 
            Assert.AreEqual("is", (String)((Atom)((Pair)((Pair)evaluated).Cdr).Car).Value);  
        }     
    }
}