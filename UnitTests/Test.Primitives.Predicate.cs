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
        public void TestBooleanP()
        {
            ISExp orig, evaluated;   
            
            
            orig = SexyParser.Parse("(boolean? #f)");
            evaluated = Evaluator.Eval(orig);
            Assert.IsTrue((bool)((Atom)evaluated).Value);    
            
            
            orig = SexyParser.Parse("(boolean? #t)");
            evaluated = Evaluator.Eval(orig);
            Assert.IsTrue((bool)((Atom)evaluated).Value);    
            
            
            orig = SexyParser.Parse("(boolean? #\\t)");
            evaluated = Evaluator.Eval(orig);
            Assert.IsFalse((bool)((Atom)evaluated).Value);    
            
            
            orig = SexyParser.Parse("(boolean? #\\f)");
            evaluated = Evaluator.Eval(orig);
            Assert.IsFalse((bool)((Atom)evaluated).Value);    
            
            
            orig = SexyParser.Parse("(boolean? 1)");
            evaluated = Evaluator.Eval(orig);
            Assert.IsFalse((bool)((Atom)evaluated).Value);    
            
            
            orig = SexyParser.Parse("(boolean? ())");
            evaluated = Evaluator.Eval(orig);
            Assert.IsFalse((bool)((Atom)evaluated).Value);    
            
            
            orig = SexyParser.Parse("(boolean? \"a\")");
            evaluated = Evaluator.Eval(orig);
            Assert.IsFalse((bool)((Atom)evaluated).Value);    
        }     
        
        
        [Test]
        public void TestCharP()
        {
            ISExp orig, evaluated;       
            
            
            orig = SexyParser.Parse("(char? #\\f)");
            evaluated = Evaluator.Eval(orig);
            Assert.IsTrue((bool)((Atom)evaluated).Value);    
            
            
            orig = SexyParser.Parse("(char? #\\t)");
            evaluated = Evaluator.Eval(orig);
            Assert.IsTrue((bool)((Atom)evaluated).Value);   
            
            
            orig = SexyParser.Parse("(char? #f)");
            evaluated = Evaluator.Eval(orig);
            Assert.IsFalse((bool)((Atom)evaluated).Value); 
            
            
            orig = SexyParser.Parse("(char? 1)");
            evaluated = Evaluator.Eval(orig);
            Assert.IsFalse((bool)((Atom)evaluated).Value);    
            
            
            orig = SexyParser.Parse("(char? ())");
            evaluated = Evaluator.Eval(orig);
            Assert.IsFalse((bool)((Atom)evaluated).Value);    
            
            
            orig = SexyParser.Parse("(char? \"a\")");
            evaluated = Evaluator.Eval(orig);
            Assert.IsFalse((bool)((Atom)evaluated).Value);    
        }     
        
        
        [Test]
        public void TestSymbolP()
        {
            ISExp orig, evaluated;       
            
            
            orig = SexyParser.Parse("(symbol? #\\f)");
            evaluated = Evaluator.Eval(orig);
            Assert.IsFalse((bool)((Atom)evaluated).Value);  
            
            
            orig = SexyParser.Parse("(symbol? #f)");
            evaluated = Evaluator.Eval(orig);
            Assert.IsFalse((bool)((Atom)evaluated).Value);    
            
            
            orig = SexyParser.Parse("(symbol? #\\t)");
            evaluated = Evaluator.Eval(orig);
            Assert.IsFalse((bool)((Atom)evaluated).Value);  
            
            
            orig = SexyParser.Parse("(symbol? 1)");
            evaluated = Evaluator.Eval(orig);
            Assert.IsFalse((bool)((Atom)evaluated).Value);    
            
            
            orig = SexyParser.Parse("(symbol? ())");
            evaluated = Evaluator.Eval(orig);
            Assert.IsFalse((bool)((Atom)evaluated).Value);    
            
            
            orig = SexyParser.Parse("(symbol? \"a\")");
            evaluated = Evaluator.Eval(orig);
            Assert.IsFalse((bool)((Atom)evaluated).Value);    
        }  
        
        
        [Test]
        public void TestStringP()
        {
            ISExp orig, evaluated;       
            
            
            orig = SexyParser.Parse("(string? \"a\")");
            evaluated = Evaluator.Eval(orig);
            Assert.IsTrue((bool)((Atom)evaluated).Value);    
            
            
            orig = SexyParser.Parse("(string? #\\f)");
            evaluated = Evaluator.Eval(orig);
            Assert.IsFalse((bool)((Atom)evaluated).Value);  
            
            
            orig = SexyParser.Parse("(string? #f)");
            evaluated = Evaluator.Eval(orig);
            Assert.IsFalse((bool)((Atom)evaluated).Value);    
            
            
            orig = SexyParser.Parse("(string? #\\t)");
            evaluated = Evaluator.Eval(orig);
            Assert.IsFalse((bool)((Atom)evaluated).Value);  
            
            
            orig = SexyParser.Parse("(string? 1)");
            evaluated = Evaluator.Eval(orig);
            Assert.IsFalse((bool)((Atom)evaluated).Value);    
            
            
            orig = SexyParser.Parse("(string? ())");
            evaluated = Evaluator.Eval(orig);
            Assert.IsFalse((bool)((Atom)evaluated).Value);    
        }   
        
        
        [Test]
        public void TestNumberP()
        {
            ISExp orig, evaluated; 
            
            
            orig = SexyParser.Parse("(number? 1)");
            evaluated = Evaluator.Eval(orig);
            Assert.IsTrue((bool)((Atom)evaluated).Value);         
            
            
            orig = SexyParser.Parse("(number? \"a\")");
            evaluated = Evaluator.Eval(orig);
            Assert.IsFalse((bool)((Atom)evaluated).Value);    
            
            
            orig = SexyParser.Parse("(number? #\\f)");
            evaluated = Evaluator.Eval(orig);
            Assert.IsFalse((bool)((Atom)evaluated).Value);  
            
            
            orig = SexyParser.Parse("(number? #f)");
            evaluated = Evaluator.Eval(orig);
            Assert.IsFalse((bool)((Atom)evaluated).Value);    
            
            
            orig = SexyParser.Parse("(number? #\\t)");
            evaluated = Evaluator.Eval(orig);
            Assert.IsFalse((bool)((Atom)evaluated).Value);   
            
            
            orig = SexyParser.Parse("(number? ())");
            evaluated = Evaluator.Eval(orig);
            Assert.IsFalse((bool)((Atom)evaluated).Value);    
        }   
        
        
        [Test]
        public void TestIntegerP()
        {
            ISExp orig, evaluated;  
            
            
            orig = SexyParser.Parse("(integer? 1)");
            evaluated = Evaluator.Eval(orig);
            Assert.IsTrue((bool)((Atom)evaluated).Value);      
            
            
            orig = SexyParser.Parse("(integer? 0.1)");
            evaluated = Evaluator.Eval(orig);
            Assert.IsFalse((bool)((Atom)evaluated).Value);     
            
            
            orig = SexyParser.Parse("(integer? \"a\")");
            evaluated = Evaluator.Eval(orig);
            Assert.IsFalse((bool)((Atom)evaluated).Value);    
            
            
            orig = SexyParser.Parse("(integer? #\\f)");
            evaluated = Evaluator.Eval(orig);
            Assert.IsFalse((bool)((Atom)evaluated).Value);  
            
            
            orig = SexyParser.Parse("(integer? #f)");
            evaluated = Evaluator.Eval(orig);
            Assert.IsFalse((bool)((Atom)evaluated).Value);    
            
            
            orig = SexyParser.Parse("(integer? #\\t)");
            evaluated = Evaluator.Eval(orig);
            Assert.IsFalse((bool)((Atom)evaluated).Value);     
            
            
            orig = SexyParser.Parse("(integer? ())");
            evaluated = Evaluator.Eval(orig);
            Assert.IsFalse((bool)((Atom)evaluated).Value);    
        }   
        
        
        [Test]
        public void TestPairP()
        {
            ISExp orig, evaluated;    
            

            // The empty list is NOT a pair!!!
            orig = SexyParser.Parse("(pair? ())");
            evaluated = Evaluator.Eval(orig);
            Assert.IsFalse((bool)((Atom)evaluated).Value); 
            
            
            orig = SexyParser.Parse("(pair? (cons 1 2))");
            evaluated = Evaluator.Eval(orig);
            Assert.IsTrue((bool)((Atom)evaluated).Value);    
            
            
            orig = SexyParser.Parse("(pair? (quote (1 2)))");
            evaluated = Evaluator.Eval(orig);
            Assert.IsTrue((bool)((Atom)evaluated).Value);    
            
            
            orig = SexyParser.Parse("(pair? 1)");
            evaluated = Evaluator.Eval(orig);
            Assert.IsFalse((bool)((Atom)evaluated).Value);      
            
            
            orig = SexyParser.Parse("(pair? 0.1)");
            evaluated = Evaluator.Eval(orig);
            Assert.IsFalse((bool)((Atom)evaluated).Value);     
            
            
            orig = SexyParser.Parse("(pair? \"a\")");
            evaluated = Evaluator.Eval(orig);
            Assert.IsFalse((bool)((Atom)evaluated).Value);    
            
            
            orig = SexyParser.Parse("(pair? #\\f)");
            evaluated = Evaluator.Eval(orig);
            Assert.IsFalse((bool)((Atom)evaluated).Value);  
            
            
            orig = SexyParser.Parse("(pair? #f)");
            evaluated = Evaluator.Eval(orig);
            Assert.IsFalse((bool)((Atom)evaluated).Value);    
            
            
            orig = SexyParser.Parse("(pair? #\\t)");
            evaluated = Evaluator.Eval(orig);
            Assert.IsFalse((bool)((Atom)evaluated).Value);   
        }    

        [Test]
        public void TestListP()
        {
            ISExp orig, evaluated;


            
            orig = SexyParser.Parse("(list? ())");
            evaluated = Evaluator.Eval(orig);
            Assert.IsTrue((bool)((Atom)evaluated).Value);


            orig = SexyParser.Parse("(list? (cons 1 2))");
            evaluated = Evaluator.Eval(orig);
            Assert.IsFalse((bool)((Atom)evaluated).Value);   


            orig = SexyParser.Parse("(list? (quote (1 2)))");
            evaluated = Evaluator.Eval(orig);
            Assert.IsTrue((bool)((Atom)evaluated).Value);    


            orig = SexyParser.Parse("(list? 1)");
            evaluated = Evaluator.Eval(orig);
            Assert.IsFalse((bool)((Atom)evaluated).Value);      
            
            
            orig = SexyParser.Parse("(list? 0.1)");
            evaluated = Evaluator.Eval(orig);
            Assert.IsFalse((bool)((Atom)evaluated).Value);     
            
            
            orig = SexyParser.Parse("(list? \"a\")");
            evaluated = Evaluator.Eval(orig);
            Assert.IsFalse((bool)((Atom)evaluated).Value);    
            
            
            orig = SexyParser.Parse("(list? #\\f)");
            evaluated = Evaluator.Eval(orig);
            Assert.IsFalse((bool)((Atom)evaluated).Value);  
            
            
            orig = SexyParser.Parse("(list? #f)");
            evaluated = Evaluator.Eval(orig);
            Assert.IsFalse((bool)((Atom)evaluated).Value);    
            
            
            orig = SexyParser.Parse("(list? #\\t)");
            evaluated = Evaluator.Eval(orig);
            Assert.IsFalse((bool)((Atom)evaluated).Value);  
        }
    }
}