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
        #region Add
        
        [Test]
        public void TestAddFail()
        {
            Assert.Throws<UnexpectedTypeException>(() => Evaluator.Eval(SexyParser.Parse("(+ \"a\")")));
        }

        [Test]
        public void TestAddLong()
        {
            ISExp orig, evaluated;
            
            
            orig = SexyParser.Parse("(+)");
            evaluated = Evaluator.Eval(orig);
            
            Assert.IsTrue(evaluated.IsAtom);
            Assert.AreEqual(0L, (Integer)((Atom)evaluated).Value);
            
            
            orig = SexyParser.Parse("(+ 1)");
            evaluated = Evaluator.Eval(orig);
            
            Assert.IsTrue(evaluated.IsAtom);
            Assert.AreEqual(1L, (Integer)((Atom)evaluated).Value);
            
            
            orig = SexyParser.Parse("(+ 1 2)");
            evaluated = Evaluator.Eval(orig);
            
            Assert.IsTrue(evaluated.IsAtom);
            Assert.AreEqual(3L, (Integer)((Atom)evaluated).Value);
            
            
            orig = SexyParser.Parse("(+ 1 2 -4)");
            evaluated = Evaluator.Eval(orig);
            
            Assert.IsTrue(evaluated.IsAtom);
            Assert.AreEqual(-1L, (Integer)((Atom)evaluated).Value);
            
            
            orig = SexyParser.Parse("(+ 1 2 -4)");
            evaluated = Evaluator.Eval(orig);
            
            Assert.IsTrue(evaluated.IsAtom);
            Assert.AreEqual(-1L, (Integer)((Atom)evaluated).Value);
            
            
            orig = SexyParser.Parse("(+ (+ 1 2) 1 (+ -1) -2 (+ 2 2))");
            evaluated = Evaluator.Eval(orig);
            
            Assert.IsTrue(evaluated.IsAtom);
            Assert.AreEqual(5, (Integer)((Atom)evaluated).Value);
        }
        
        [Test]
        public void TestAddDouble()
        {
            ISExp orig, evaluated;
            
            
            orig = SexyParser.Parse("(+ 0.1)");
            evaluated = Evaluator.Eval(orig);
            
            Assert.IsTrue(evaluated.IsAtom);
            AssertCloseEnough(0.1D, (double)((Atom)evaluated).Value);
            
            
            orig = SexyParser.Parse("(+ 0.1 0.1)");
            evaluated = Evaluator.Eval(orig);
            
            Assert.IsTrue(evaluated.IsAtom);
            AssertCloseEnough(0.2D, (double)((Atom)evaluated).Value);
            
            
            orig = SexyParser.Parse("(+ 0.1 0.1 -0.2)");
            evaluated = Evaluator.Eval(orig);
            
            Assert.IsTrue(evaluated.IsAtom);
            AssertCloseEnough(0.0D, (double)((Atom)evaluated).Value);
        }
        
        [Test]
        public void TestAddMix()
        {
            ISExp orig, evaluated;
            
            
            orig = SexyParser.Parse("(+ 1 0.1 2 0.2)");
            evaluated = Evaluator.Eval(orig);
            
            Assert.IsTrue(evaluated.IsAtom);
            AssertCloseEnough(3.3D, (double)((Atom)evaluated).Value);
            
            
            orig = SexyParser.Parse("(+ 0.1 1 0.2 2)");
            evaluated = Evaluator.Eval(orig);
            
            Assert.IsTrue(evaluated.IsAtom);
            AssertCloseEnough(3.3D, (double)((Atom)evaluated).Value);
        }

        #endregion Add



        #region Subtract
        
        [Test]
        public void TestSubtractFail()
        {
            Assert.Throws<NotEnoughArgumentsException>(() => Evaluator.Eval(SexyParser.Parse("(-)")));
            Assert.Throws<UnexpectedTypeException>(() => Evaluator.Eval(SexyParser.Parse("(- \"a\")")));
        }

        [Test]
        public void TestSubtractLong()
        {
            ISExp orig, evaluated;        
            
            orig = SexyParser.Parse("(- -1)");
            evaluated = Evaluator.Eval(orig);
            
            Assert.IsTrue(evaluated.IsAtom);
            Assert.AreEqual(1, (Integer)((Atom)evaluated).Value);         

            
            orig = SexyParser.Parse("(- 1)");
            evaluated = Evaluator.Eval(orig);
            
            Assert.IsTrue(evaluated.IsAtom);
            Assert.AreEqual(-1, (Integer)((Atom)evaluated).Value);         
                        
            
            orig = SexyParser.Parse("(- 1 2)");
            evaluated = Evaluator.Eval(orig);
            
            Assert.IsTrue(evaluated.IsAtom);
            Assert.AreEqual(-1L, (Integer)((Atom)evaluated).Value);         
            
            
            orig = SexyParser.Parse("(- 1 2 -4)");
            evaluated = Evaluator.Eval(orig);
            
            Assert.IsTrue(evaluated.IsAtom);
            Assert.AreEqual(3L, (Integer)((Atom)evaluated).Value);     
            
            
            orig = SexyParser.Parse("(- 1 1)");
            evaluated = Evaluator.Eval(orig);
            
            Assert.IsTrue(evaluated.IsAtom);
            Assert.AreEqual(0L, (Integer)((Atom)evaluated).Value);
            
            
            orig = SexyParser.Parse("(- 1 1 -2)");
            evaluated = Evaluator.Eval(orig);

            Assert.IsTrue(evaluated.IsAtom);
            Assert.AreEqual(2L, (Integer)((Atom)evaluated).Value);




            string sexp = "1";
            long result = 1;

            for (int i = 0; i < 100; i++)
            {
                result = -result;

                sexp = String.Format("(- {0})", sexp);

                orig = SexyParser.Parse(sexp);
                evaluated = Evaluator.Eval(orig);

                Assert.IsTrue(evaluated.IsAtom);
                Assert.AreEqual(result, (Integer)((Atom)evaluated).Value);
            }
        }
        
        [Test]
        public void TestSubtractDouble()
        {
            ISExp orig, evaluated;
            
            
            orig = SexyParser.Parse("(- 0.1)");
            evaluated = Evaluator.Eval(orig);
            
            Assert.IsTrue(evaluated.IsAtom);
            AssertCloseEnough(-0.1D, (double)((Atom)evaluated).Value);
            
            
            orig = SexyParser.Parse("(- 0.1 0.1)");
            evaluated = Evaluator.Eval(orig);
            
            Assert.IsTrue(evaluated.IsAtom);
            AssertCloseEnough(0.0D, (double)((Atom)evaluated).Value);
            
            
            orig = SexyParser.Parse("(- 0.1 0.1 -0.2)");
            evaluated = Evaluator.Eval(orig);
            
            Assert.IsTrue(evaluated.IsAtom);
            AssertCloseEnough(0.2D, (double)((Atom)evaluated).Value);


            
            string sexp = "0.1";
            double result = 0.1;
            
            for (int i = 0; i < 100; i++)
            {
                result = -result;
                
                sexp = String.Format("(- {0})", sexp);
                
                orig = SexyParser.Parse(sexp);
                evaluated = Evaluator.Eval(orig);
                
                Assert.IsTrue(evaluated.IsAtom);
                Assert.AreEqual(result, (double)((Atom)evaluated).Value);
            }
        }
        
        [Test]
        public void TestSubtractMix()
        {
            ISExp orig, evaluated;
            
            
            orig = SexyParser.Parse("(- 1 0.1 2 0.2)");
            evaluated = Evaluator.Eval(orig);
            
            Assert.IsTrue(evaluated.IsAtom);
            AssertCloseEnough(-1.3D, (double)((Atom)evaluated).Value);
            
            
            orig = SexyParser.Parse("(- 0.1 1 0.2 2)");
            evaluated = Evaluator.Eval(orig);
            
            Assert.IsTrue(evaluated.IsAtom);
            AssertCloseEnough(-3.1D, (double)((Atom)evaluated).Value);
        }

        #endregion Subtract



        #region Multiply
        
        [Test]
        public void TestMultiplyFail()
        {
            Assert.Throws<UnexpectedTypeException>(() => Evaluator.Eval(SexyParser.Parse("(* \"a\")")));
        }
        
        [Test]
        public void TestMultiplyLong()
        {
            ISExp orig, evaluated;
            
            
            orig = SexyParser.Parse("(*)");
            evaluated = Evaluator.Eval(orig);
            
            Assert.IsTrue(evaluated.IsAtom);
            Assert.AreEqual(1L, (Integer)((Atom)evaluated).Value);
            
            
            orig = SexyParser.Parse("(* 2)");
            evaluated = Evaluator.Eval(orig);
            
            Assert.IsTrue(evaluated.IsAtom);
            Assert.AreEqual(2L, (Integer)((Atom)evaluated).Value);
            
            
            orig = SexyParser.Parse("(* 3 2)");
            evaluated = Evaluator.Eval(orig);
            
            Assert.IsTrue(evaluated.IsAtom);
            Assert.AreEqual(6L, (Integer)((Atom)evaluated).Value);
            
            
            orig = SexyParser.Parse("(* 1 2 -4)");
            evaluated = Evaluator.Eval(orig);
            
            Assert.IsTrue(evaluated.IsAtom);
            Assert.AreEqual(-8L, (Integer)((Atom)evaluated).Value);
            
            
            orig = SexyParser.Parse("(* (* 1 2) 1 (* -1) -2 (* 2 2))");
            evaluated = Evaluator.Eval(orig);
            
            Assert.IsTrue(evaluated.IsAtom);
            Assert.AreEqual(16L, (Integer)((Atom)evaluated).Value);
        }
        
        [Test]
        public void TestMultiplyDouble()
        {
            ISExp orig, evaluated;
            
            
            orig = SexyParser.Parse("(* 0.1)");
            evaluated = Evaluator.Eval(orig);
            
            Assert.IsTrue(evaluated.IsAtom);
            AssertCloseEnough(0.1D, (double)((Atom)evaluated).Value);
            
            
            orig = SexyParser.Parse("(* 0.1 0.1)");
            evaluated = Evaluator.Eval(orig);
            
            Assert.IsTrue(evaluated.IsAtom);
            AssertCloseEnough(0.01D, (double)((Atom)evaluated).Value);
            
            
            orig = SexyParser.Parse("(* 0.1 0.1 -0.2)");
            evaluated = Evaluator.Eval(orig);
            
            Assert.IsTrue(evaluated.IsAtom);
            AssertCloseEnough(-0.002D, (double)((Atom)evaluated).Value);
        }
        
        [Test]
        public void TestMultiplyMix()
        {
            ISExp orig, evaluated;
            
            
            orig = SexyParser.Parse("(* 1 0.1 2 0.2)");
            evaluated = Evaluator.Eval(orig);
            
            Assert.IsTrue(evaluated.IsAtom);
            AssertCloseEnough(0.04D, (double)((Atom)evaluated).Value);
            
            
            orig = SexyParser.Parse("(* 0.1 1 0.2 2)");
            evaluated = Evaluator.Eval(orig);
            
            Assert.IsTrue(evaluated.IsAtom);
            AssertCloseEnough(0.04D, (double)((Atom)evaluated).Value);
        }


        #endregion Multiply

        
        
        #region Divide
        
        [Test]
        public void TestDivideFail()
        {
            Assert.Throws<NotEnoughArgumentsException>(() => Evaluator.Eval(SexyParser.Parse("(/)")));
            Assert.Throws<UnexpectedTypeException>(() => Evaluator.Eval(SexyParser.Parse("(/ \"a\")")));
        }
        
        [Test]
        public void TestDivideLong()
        {
            ISExp orig, evaluated;        
            
            orig = SexyParser.Parse("(/ 1)");
            evaluated = Evaluator.Eval(orig);
            
            Assert.IsTrue(evaluated.IsAtom);
            Assert.AreEqual(1L, (Integer)((Atom)evaluated).Value);         
            
            
            orig = SexyParser.Parse("(/ -1)");
            evaluated = Evaluator.Eval(orig);
            
            Assert.IsTrue(evaluated.IsAtom);
            Assert.AreEqual(-1L, (Integer)((Atom)evaluated).Value);         
            
            
            orig = SexyParser.Parse("(/ 27 3 3)");
            evaluated = Evaluator.Eval(orig);
            
            Assert.IsTrue(evaluated.IsAtom);
            Assert.AreEqual(3L, (Integer)((Atom)evaluated).Value);         
            
            
            orig = SexyParser.Parse("(/ 4 -2)");
            evaluated = Evaluator.Eval(orig);
            
            Assert.IsTrue(evaluated.IsAtom);
            Assert.AreEqual(-2L, (Integer)((Atom)evaluated).Value);     
            
            
            
            
            string sexp = "1";
            long result = 1;
            
            for (int i = 0; i < 100; i++)
            {                
                sexp = String.Format("(/ {0})", sexp);
                
                orig = SexyParser.Parse(sexp);
                evaluated = Evaluator.Eval(orig);
                
                Assert.IsTrue(evaluated.IsAtom);
                Assert.AreEqual(result, (Integer)((Atom)evaluated).Value);
            }
        }
        
        [Test]
        public void TestDivideDouble()
        {
            ISExp orig, evaluated;
            
            
            orig = SexyParser.Parse("(/ 0.1)");
            evaluated = Evaluator.Eval(orig);
            
            Assert.IsTrue(evaluated.IsAtom);
            AssertCloseEnough(10D, (double)((Atom)evaluated).Value);
            
            
            orig = SexyParser.Parse("(/ 0.1 0.1)");
            evaluated = Evaluator.Eval(orig);
            
            Assert.IsTrue(evaluated.IsAtom);
            AssertCloseEnough(1D, (double)((Atom)evaluated).Value);
            
            
            orig = SexyParser.Parse("(/ 0.1 -0.1 0.1 -0.1)");
            evaluated = Evaluator.Eval(orig);
            
            Assert.IsTrue(evaluated.IsAtom);
            AssertCloseEnough(100, (double)((Atom)evaluated).Value);
            
            
            
            string sexp = "0.1";
            double result = 0.1;
            
            for (int i = 0; i < 100; i++)
            {
                result = 1.0D / result;
                
                sexp = String.Format("(/ {0})", sexp);
                
                orig = SexyParser.Parse(sexp);
                evaluated = Evaluator.Eval(orig);
                
                Assert.IsTrue(evaluated.IsAtom);
                AssertCloseEnough(result, (double)((Atom)evaluated).Value);
            }
        }
        
        [Test]
        public void TestDivideMix()
        {
            ISExp orig, evaluated;
            
            
            orig = SexyParser.Parse("(/ 2 0.2)");
            evaluated = Evaluator.Eval(orig);
            
            Assert.IsTrue(evaluated.IsAtom);
            AssertCloseEnough(10D, (double)((Atom)evaluated).Value);
            
            
            orig = SexyParser.Parse("(/ -0.2 2)");
            evaluated = Evaluator.Eval(orig);
            
            Assert.IsTrue(evaluated.IsAtom);
            AssertCloseEnough(-0.1, (double)((Atom)evaluated).Value);
        }
        
        #endregion Divide


        
        #region Modulo
        
        [Test]
        public void TestModuloFail()
        {
            Assert.Throws<ArityException>(() => Evaluator.Eval(SexyParser.Parse("(modulo)")));
            Assert.Throws<ArityException>(() => Evaluator.Eval(SexyParser.Parse("(modulo 1)")));
            Assert.Throws<ArityException>(() => Evaluator.Eval(SexyParser.Parse("(modulo 1 1 1)")));

            Assert.Throws<UnexpectedTypeException>(() => Evaluator.Eval(SexyParser.Parse("(modulo 1 1.5)")));
            Assert.Throws<UnexpectedTypeException>(() => Evaluator.Eval(SexyParser.Parse("(modulo 1.5 1)")));
            Assert.Throws<UnexpectedTypeException>(() => Evaluator.Eval(SexyParser.Parse("(modulo \"a\" \"a\")")));
        }
        
        [Test]
        public void TestModuloLong()
        {
            ISExp orig, evaluated;        
            
            orig = SexyParser.Parse("(modulo 1 1)");
            evaluated = Evaluator.Eval(orig);
            
            Assert.IsTrue(evaluated.IsAtom);
            Assert.AreEqual(0L, (Integer)((Atom)evaluated).Value);    
            
            orig = SexyParser.Parse("(modulo 12 4)");
            evaluated = Evaluator.Eval(orig);
            
            Assert.IsTrue(evaluated.IsAtom);
            Assert.AreEqual(0L, (Integer)((Atom)evaluated).Value);         
            
            
            orig = SexyParser.Parse("(modulo 13 4)");
            evaluated = Evaluator.Eval(orig);
            
            Assert.IsTrue(evaluated.IsAtom);
            Assert.AreEqual(1L, (Integer)((Atom)evaluated).Value);         
            
            
            orig = SexyParser.Parse("(modulo 14 4)");
            evaluated = Evaluator.Eval(orig);
            
            Assert.IsTrue(evaluated.IsAtom);
            Assert.AreEqual(2L, (Integer)((Atom)evaluated).Value);         
            
            
            orig = SexyParser.Parse("(modulo 15 4)");
            evaluated = Evaluator.Eval(orig);
            
            Assert.IsTrue(evaluated.IsAtom);
            Assert.AreEqual(3L, (Integer)((Atom)evaluated).Value);     


            // Negative divisor
            
            orig = SexyParser.Parse("(modulo 12 -4)");
            evaluated = Evaluator.Eval(orig);
            
            Assert.IsTrue(evaluated.IsAtom);
            Assert.AreEqual(0L, (Integer)((Atom)evaluated).Value);         
            
            
            orig = SexyParser.Parse("(modulo 13 -4)");
            evaluated = Evaluator.Eval(orig);
            
            Assert.IsTrue(evaluated.IsAtom);
            Assert.AreEqual(-3L, (Integer)((Atom)evaluated).Value);         
            
            
            orig = SexyParser.Parse("(modulo 14 -4)");
            evaluated = Evaluator.Eval(orig);
            
            Assert.IsTrue(evaluated.IsAtom);
            Assert.AreEqual(-2L, (Integer)((Atom)evaluated).Value);         
            
            
            orig = SexyParser.Parse("(modulo 15 -4)");
            evaluated = Evaluator.Eval(orig);
            
            Assert.IsTrue(evaluated.IsAtom);
            Assert.AreEqual(-1L, (Integer)((Atom)evaluated).Value);  
            
            
            // Negative dividend
            
            orig = SexyParser.Parse("(modulo -12 4)");
            evaluated = Evaluator.Eval(orig);
            
            Assert.IsTrue(evaluated.IsAtom);
            Assert.AreEqual(0L, (Integer)((Atom)evaluated).Value);         
            
            
            orig = SexyParser.Parse("(modulo -13 4)");
            evaluated = Evaluator.Eval(orig);
            
            Assert.IsTrue(evaluated.IsAtom);
            Assert.AreEqual(3L, (Integer)((Atom)evaluated).Value);         
            
            
            orig = SexyParser.Parse("(modulo -14 4)");
            evaluated = Evaluator.Eval(orig);
            
            Assert.IsTrue(evaluated.IsAtom);
            Assert.AreEqual(2L, (Integer)((Atom)evaluated).Value);         
            
            
            orig = SexyParser.Parse("(modulo -15 4)");
            evaluated = Evaluator.Eval(orig);
            
            Assert.IsTrue(evaluated.IsAtom);
            Assert.AreEqual(1L, (Integer)((Atom)evaluated).Value);  
            
            
            // Negative everything!
            
            orig = SexyParser.Parse("(modulo -12 -4)");
            evaluated = Evaluator.Eval(orig);
            
            Assert.IsTrue(evaluated.IsAtom);
            Assert.AreEqual(0L, (Integer)((Atom)evaluated).Value);         
            
            
            orig = SexyParser.Parse("(modulo -13 -4)");
            evaluated = Evaluator.Eval(orig);
            
            Assert.IsTrue(evaluated.IsAtom);
            Assert.AreEqual(-1L, (Integer)((Atom)evaluated).Value);         
            
            
            orig = SexyParser.Parse("(modulo -14 -4)");
            evaluated = Evaluator.Eval(orig);
            
            Assert.IsTrue(evaluated.IsAtom);
            Assert.AreEqual(-2L, (Integer)((Atom)evaluated).Value);         
            
            
            orig = SexyParser.Parse("(modulo -15 -4)");
            evaluated = Evaluator.Eval(orig);
            
            Assert.IsTrue(evaluated.IsAtom);
            Assert.AreEqual(-3L, (Integer)((Atom)evaluated).Value);        
        }
        
        #endregion Modulo



        #region Exponent
        
        [Test]
        public void TestExponentFail()
        {
            Assert.Throws<ArityException>(() => Evaluator.Eval(SexyParser.Parse("(expt)")));
            Assert.Throws<ArityException>(() => Evaluator.Eval(SexyParser.Parse("(expt 1)")));
            Assert.Throws<ArityException>(() => Evaluator.Eval(SexyParser.Parse("(expt 1 1 1)")));

            Assert.Throws<UnexpectedTypeException>(() => Evaluator.Eval(SexyParser.Parse("(expt \"a\" \"a\")")));
        }
        
        [Test]
        public void TestExponentLong()
        {
            ISExp orig, evaluated;

            
            orig = SexyParser.Parse("(expt 2 3)");
            evaluated = Evaluator.Eval(orig);
            
            Assert.IsTrue(evaluated.IsAtom);
            Assert.AreEqual(8L, (Integer)((Atom)evaluated).Value);


            
            orig = SexyParser.Parse("(expt 2 -3)");
            evaluated = Evaluator.Eval(orig);
            
            Assert.IsTrue(evaluated.IsAtom);
            Assert.AreEqual(0.125, (double)((Atom)evaluated).Value);
            
                        
            orig = SexyParser.Parse("(expt -2 3)");
            evaluated = Evaluator.Eval(orig);
            
            Assert.IsTrue(evaluated.IsAtom);
            Assert.AreEqual(-8L, (Integer)((Atom)evaluated).Value);

            
            orig = SexyParser.Parse("(expt -2 -3)");
            evaluated = Evaluator.Eval(orig);
            
            Assert.IsTrue(evaluated.IsAtom);
            Assert.AreEqual(-0.125, (double)((Atom)evaluated).Value);
        } 

        #endregion



        private static void AssertCloseEnough(double expected, double actual)
        {
            double diff = Math.Abs(expected - actual);

            Assert.IsTrue(expected == actual || diff < 0.000000000001);
        }
    }
}

