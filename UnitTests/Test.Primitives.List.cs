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
        public void TestLengthFail()
        {
            Assert.Throws<UnexpectedTypeException>(() => Evaluator.Eval(SexyParser.Parse("(length 1)")));
            Assert.Throws<UnexpectedTypeException>(() => Evaluator.Eval(SexyParser.Parse("(length (cons 1 2))")));
        }   
        
        [Test]
        public void TestLength()
        {
            ISExp orig, evaluated;   
            
            
            orig = SexyParser.Parse("(length ())");
            evaluated = Evaluator.Eval(orig);
            Assert.AreEqual(0, (long)((Atom)evaluated).Value); 
            
            
            orig = SexyParser.Parse("(length (cons 1 ()))");
            evaluated = Evaluator.Eval(orig);
            Assert.AreEqual(1, (long)((Atom)evaluated).Value);      
            
            
            orig = SexyParser.Parse("(length (cons 1 (cons 2 ())))");
            evaluated = Evaluator.Eval(orig);
            Assert.AreEqual(2, (long)((Atom)evaluated).Value);      
        }   

        [Test]
        public void TestCarFail()
        {
            Assert.Throws<ArityException>(() => Evaluator.Eval(SexyParser.Parse("(car)")));
            Assert.Throws<ArityException>(() => Evaluator.Eval(SexyParser.Parse("(car 1 2)")));
            Assert.Throws<UnexpectedTypeException>(() => Evaluator.Eval(SexyParser.Parse("(car 1)")));
            Assert.Throws<UnexpectedTypeException>(() => Evaluator.Eval(SexyParser.Parse("(car ())")));
        }   
        
        [Test]
        public void TestCar()
        {
            ISExp orig, evaluated;   
            
            
            orig = SexyParser.Parse("(car (cons 1 ()))");
            evaluated = Evaluator.Eval(orig);
            Assert.AreEqual(1, (long)((Atom)evaluated).Value);      
        }     
        
        
        [Test]
        public void TestCdrFail()
        {
            Assert.Throws<ArityException>(() => Evaluator.Eval(SexyParser.Parse("(cdr)")));
            Assert.Throws<ArityException>(() => Evaluator.Eval(SexyParser.Parse("(cdr 1 2)")));
            Assert.Throws<UnexpectedTypeException>(() => Evaluator.Eval(SexyParser.Parse("(cdr 1)")));
            Assert.Throws<UnexpectedTypeException>(() => Evaluator.Eval(SexyParser.Parse("(cdr ())")));
        }   
        
        [Test]
        public void TestCdr()
        {
            ISExp orig, evaluated;   
            
            
            orig = SexyParser.Parse("(cdr (cons 1 ()))");
            evaluated = Evaluator.Eval(orig);
            Assert.AreEqual(Pair.Empty, (Pair)evaluated);  
        }     
        
        
        [Test]
        public void TestConsFail()
        {        
            Assert.Throws<ArityException>(() => Evaluator.Eval(SexyParser.Parse("(cons)")));
            Assert.Throws<ArityException>(() => Evaluator.Eval(SexyParser.Parse("(cons 1)")));
            Assert.Throws<ArityException>(() => Evaluator.Eval(SexyParser.Parse("(cons 1 2 3)")));
        }   
        
        [Test]
        public void TestCons()
        {
            ISExp orig, evaluated;   
            
            
            orig = SexyParser.Parse("(cons 1 ())");
            evaluated = Evaluator.Eval(orig);
            Assert.AreEqual(1, (long)((Atom)((Pair)evaluated).Car).Value);
            Assert.AreEqual(Pair.Empty, ((Pair)evaluated).Cdr);
            
            
            orig = SexyParser.Parse("(cons 1 2)");
            evaluated = Evaluator.Eval(orig);
            Assert.AreEqual(1, (long)((Atom)((Pair)evaluated).Car).Value);
            Assert.AreEqual(2, (long)((Atom)((Pair)evaluated).Cdr).Value);
        }   
        
//        //[Test]
//        public void TestList()
//        {
//            ISExp orig, evaluated;   
//            
//            
//            orig = SexyParser.Parse("(list 4 5 6)");
//            evaluated = Evaluator.Eval(orig);
//            Assert.AreEqual(3, ((SexyList)evaluated).Count);  
//            Assert.AreEqual(4, (long)((Atom)((SexyList)evaluated).Car).Value);
//            Assert.AreEqual(4, (long)((Atom)((SexyList)evaluated).GetAt(0)).Value);
//            Assert.AreEqual(5, (long)((Atom)((SexyList)evaluated).GetAt(1)).Value);
//            Assert.AreEqual(6, (long)((Atom)((SexyList)evaluated).GetAt(2)).Value);
//        }     
    }
}