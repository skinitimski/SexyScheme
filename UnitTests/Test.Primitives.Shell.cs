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
        public void TestExec()
        {
            ISExp orig, evaluated;   
            
            
            orig = SexyParser.Parse("(exec 'ls \"../../TestData/Primitives.Shell/ls\")");
            evaluated = Evaluator.Eval(orig);
            Assert.AreEqual("dir0\nfile0\n", (String)((Atom)evaluated).Value); 
            
            
            orig = SexyParser.Parse("((lambda x (exec (cons 'ls x))) \"../../TestData/Primitives.Shell/ls\")");
            evaluated = Evaluator.Eval(orig);
            Assert.AreEqual("dir0\nfile0\n", (String)((Atom)evaluated).Value); 

        }      
    }
}