using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using NUnit.Framework;

using Atmosphere.Extensions;
using Atmosphere.SexyLib;
using Atmosphere.SexyLib.Exceptions;
using Atmosphere.SexyLib.Numbers;

namespace Atmosphere.UnitTests
{
    [TestFixture]
    public partial class TestPrimitives
    {
        #region If
        
        [Test]
        public void TestIf()
        {
            ISExp orig, evaluated;   
            
            
            orig = SexyParser.Parse("(if #f 1 0)");
            evaluated = Evaluator.Eval(orig);
            Assert.AreEqual(0L, (Integer)((Atom)evaluated).Value);    
            
            
            orig = SexyParser.Parse("(if #t 1 0)");
            evaluated = Evaluator.Eval(orig);
            Assert.AreEqual(1L, (Integer)((Atom)evaluated).Value);  
            
            
            orig = SexyParser.Parse("(if 50 1 0)");
            evaluated = Evaluator.Eval(orig);
            Assert.AreEqual(1L, (Integer)((Atom)evaluated).Value); 
            
            
            orig = SexyParser.Parse("(if \"a\" 1 0)");
            evaluated = Evaluator.Eval(orig);
            Assert.AreEqual(1L, (Integer)((Atom)evaluated).Value);    
            
            
            orig = SexyParser.Parse("(if (if #t #t #f) 1 0)");
            evaluated = Evaluator.Eval(orig);
            Assert.AreEqual(1L, (Integer)((Atom)evaluated).Value);    
        }     
        
        #endregion If
        
        
        
        #region And
        
        [Test]
        public void TestAnd()
        {
            ISExp orig, evaluated;   
            
            
            orig = SexyParser.Parse("(and)");
            evaluated = Evaluator.Eval(orig);
            Assert.IsTrue((bool)((Atom)evaluated).Value);
            
            
            orig = SexyParser.Parse("(and #t)");
            evaluated = Evaluator.Eval(orig);
            Assert.IsTrue((bool)((Atom)evaluated).Value);
            
            
            orig = SexyParser.Parse("(and #t 1 \"a\" ())");
            evaluated = Evaluator.Eval(orig);
            Assert.IsTrue((bool)((Atom)evaluated).Value);
            
            
            orig = SexyParser.Parse("(and #f)");
            evaluated = Evaluator.Eval(orig);
            Assert.IsFalse((bool)((Atom)evaluated).Value);
            
            
            orig = SexyParser.Parse("(and #t 1 \"a\" () #f)");
            evaluated = Evaluator.Eval(orig);
            Assert.IsFalse((bool)((Atom)evaluated).Value);
        }     

        #endregion And
        
        
        
        #region Or
        
        [Test]
        public void TestOr()
        {
            ISExp orig, evaluated;   
            
            
            orig = SexyParser.Parse("(or #t)");
            evaluated = Evaluator.Eval(orig);
            Assert.IsTrue((bool)((Atom)evaluated).Value);
            
            
            orig = SexyParser.Parse("(or #t 1 \"a\" ())");
            evaluated = Evaluator.Eval(orig);
            Assert.IsTrue((bool)((Atom)evaluated).Value);
            
            
            orig = SexyParser.Parse("(or #f #f #f #f #f 0 #f)");
            evaluated = Evaluator.Eval(orig);
            Assert.IsTrue((bool)((Atom)evaluated).Value);
            
            
            orig = SexyParser.Parse("(or #f)");
            evaluated = Evaluator.Eval(orig);
            Assert.IsFalse((bool)((Atom)evaluated).Value);
            
            
            orig = SexyParser.Parse("(or)");
            evaluated = Evaluator.Eval(orig);
            Assert.IsFalse((bool)((Atom)evaluated).Value);
        }     
        
        #endregion Or
        
        
        
        #region Not
        
        [Test]
        public void TestNotFail()
        {
            Assert.Throws<ArityException>(() => Evaluator.Eval(SexyParser.Parse("(not)")));
            Assert.Throws<ArityException>(() => Evaluator.Eval(SexyParser.Parse("(not 1 2)")));
        }

        [Test]
        public void TestNot()
        {
            ISExp orig, evaluated;   
            
            
            orig = SexyParser.Parse("(not #t)");
            evaluated = Evaluator.Eval(orig);
            Assert.IsFalse((bool)((Atom)evaluated).Value);
            
            
            orig = SexyParser.Parse("(not 1)");
            evaluated = Evaluator.Eval(orig);
            Assert.IsFalse((bool)((Atom)evaluated).Value);
            
            
            orig = SexyParser.Parse("(not \"a\")");
            evaluated = Evaluator.Eval(orig);
            Assert.IsFalse((bool)((Atom)evaluated).Value);
            
            
            orig = SexyParser.Parse("(not #\\a)");
            evaluated = Evaluator.Eval(orig);
            Assert.IsFalse((bool)((Atom)evaluated).Value);
            
            
            orig = SexyParser.Parse("(not #f)");
            evaluated = Evaluator.Eval(orig);
            Assert.IsTrue((bool)((Atom)evaluated).Value);
        }     
        
        #endregion Not
    }
}

