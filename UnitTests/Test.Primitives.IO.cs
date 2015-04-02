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
        public void TestFileContent()
        {
            ISExp orig, evaluated;   
            
            
            orig = SexyParser.Parse("(file-content \"../../TestData/Primitives.IO/content.txt\")");
            evaluated = Evaluator.Eval(orig);
            Assert.AreEqual("This is some content.", (String)((Atom)evaluated).Value); 
        }      
    }
}