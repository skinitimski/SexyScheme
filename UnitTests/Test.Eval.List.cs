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
    public partial class TestEval : SexyLibTestFixture
    {
        [Test]
        public void TestEvalListAdd()
        {
            ISExp orig, evaluated;


            orig = SexyParser.Parse("(+ 1 2)");
            evaluated = Evaluator.Eval(orig);
            
            Assert.IsTrue(evaluated.IsAtom);
            Assert.AreEqual(3L, (Integer)((Atom)evaluated).Value);
            
            
            
            orig = SexyParser.Parse("(+ 1 (+ -2 3) (+ -1 2))");
            evaluated = Evaluator.Eval(orig);
            
            Assert.IsTrue(evaluated.IsAtom);
            Assert.AreEqual(3L, (Integer)((Atom)evaluated).Value);
        }
        
        [Test]
        public void TestEvalListSubtract()
        {       
            ISExp orig, evaluated;
            
            orig = SexyParser.Parse("(- 1 2)");
            evaluated = Evaluator.Eval(orig);

            Assert.AreEqual(-1L, (Integer)((Atom)evaluated).Value);
        }
        
        [Test]
        public void TestEvalListLambdaFail()
        {       
            Assert.Throws<BadSyntaxException>(() => Evaluator.Eval(Pair.Cons(Atom.KeywordLambda, Atom.CreateLong(1))));
            Assert.Throws<BadSyntaxException>(() => Evaluator.Eval(SexyParser.Parse("(lambda)")));
            Assert.Throws<BadSyntaxException>(() => Evaluator.Eval(SexyParser.Parse("(lambda x)")));
            Assert.Throws<BadSyntaxException>(() => Evaluator.Eval(SexyParser.Parse("(lambda (x))")));
            Assert.Throws<BadSyntaxException>(() => Evaluator.Eval(SexyParser.Parse("(lambda (x . y))")));
            Assert.Throws<LambdaException>(() => Evaluator.Eval(SexyParser.Parse("(lambda (1) 1)")));
            Assert.Throws<LambdaException>(() => Evaluator.Eval(SexyParser.Parse("(lambda 1 1)")));
        }
        
        [Test]
        public void TestEvalListLambdaFixed()
        {       
            ISExp orig, evaluated;
            
            
            orig = SexyParser.Parse("((lambda (x) x) 1)");
            evaluated = Evaluator.Eval(orig);
           
            Assert.AreEqual(1, (Integer)((Atom)evaluated).Value);
            
            
            orig = SexyParser.Parse("((lambda (x y) (+ x y)) 1 2)");
            evaluated = Evaluator.Eval(orig);
            
            Assert.AreEqual(3, (Integer)((Atom)evaluated).Value);
        }
        
        [Test]
        public void TestEvalListLambdaVariable()
        {       
            ISExp orig, evaluated;

            
            orig = SexyParser.Parse("((lambda x (car x)) 1)");
            evaluated = Evaluator.Eval(orig);
            
            Assert.AreEqual(1, (Integer)((Atom)evaluated).Value);
            
            
            orig = SexyParser.Parse("((lambda x (car (cdr x))) 1 2)");
            evaluated = Evaluator.Eval(orig);
            
            Assert.AreEqual(2, (Integer)((Atom)evaluated).Value);
        }
        
        [Test]
        public void TestEvalListLambdaRest()
        {       
            ISExp orig, evaluated;
            
            
            orig = SexyParser.Parse("((lambda (x . y) (car y)) 1 2)");
            evaluated = Evaluator.Eval(orig);
            
            Assert.AreEqual(2, (Integer)((Atom)evaluated).Value);
            
            
            orig = SexyParser.Parse("((lambda (x y . z) (car z)) 1 2 3)");
            evaluated = Evaluator.Eval(orig);
            
            Assert.AreEqual(3, (Integer)((Atom)evaluated).Value);
        }

        [Test]
        public void TestEvalListLambdaWithSymbols()
        {
            ISExp orig, evaluated;

            Evaluator.Closure.AddSymbolDefinition("x", Pair.List(Atom.CreateSymbol("a"), Atom.CreateLong(2), Atom.CreateString("c")));
                        
            orig = SexyParser.Parse("((lambda (y) (car y)) x)");
            evaluated = Evaluator.Eval(orig);

            Evaluator.Closure.RemoveSymbolDefinition("x");

            Assert.AreEqual("a", (string)((Atom)evaluated).Value);
        }
    }
}
