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
    public class TestParseDouble
    {
        private struct ParseDoubleTestCase
        {
            public string Input { get; set; }
            
            public int StartIndex { get; set; }
            
            public double ExpectedValue { get; set; }
            
            public int ExpectedEndIndex { get; set; }
        }
        
        [Test]
        public void TestParseDoubleFail()
        {
            // TODO?
        }
        
        [Test]
        public void TestParseDoubleSuccess()
        {
            List<ParseDoubleTestCase> testCases = new List<ParseDoubleTestCase>
            {
                new ParseDoubleTestCase
                {
                    Input = "0.0",
                    ExpectedValue = 0.0,
                    ExpectedEndIndex = 3
                },
                new ParseDoubleTestCase
                {
                    Input = "0.0",
                    ExpectedValue = 0.0,
                    ExpectedEndIndex = 3
                },
                new ParseDoubleTestCase
                {
                    Input = "(+ 0.13 .12 100. -0.1 1e7)",
                    StartIndex = 3,
                    ExpectedValue = 0.13,
                    ExpectedEndIndex = 7
                },
                new ParseDoubleTestCase
                {
                    Input = "(+ 0.13 .12 100. -0.1 1e7)",
                    StartIndex = 8,
                    ExpectedValue = 0.12,
                    ExpectedEndIndex = 11
                },
                new ParseDoubleTestCase
                {
                    Input = "(+ 0.13 .12 100. -0.1 1e7)",
                    StartIndex = 12,
                    ExpectedValue = 100D,
                    ExpectedEndIndex = 16
                },
                new ParseDoubleTestCase
                {
                    Input = "(+ 0.13 .12 100. -0.1 1e7)",
                    StartIndex = 17,
                    ExpectedValue = -0.1,
                    ExpectedEndIndex = 21
                },
                new ParseDoubleTestCase
                {
                    Input = "(+ 0.13 .12 100. -0.1 1e7)",
                    StartIndex = 22,
                    ExpectedValue = 1e7D,
                    ExpectedEndIndex = 25
                },
            };
            
            foreach (ParseDoubleTestCase testCase in testCases)
            {
                int index = testCase.StartIndex;
                
                Atom atom = (Atom)SexyParser.Parse(testCase.Input, ref index);
                
                double expected = testCase.ExpectedValue;
                double actual = (double)(atom.Value);
                
                Assert.AreEqual(expected, actual);
                
                int expectedIndex = testCase.ExpectedEndIndex;
                
                Assert.AreEqual(expectedIndex, index);
            }
        }

    }
}

