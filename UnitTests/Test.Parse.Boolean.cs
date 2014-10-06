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
    public class TestParseBoolean
    {
        private struct ParseBooleanTestCase
        {
            public string Input { get; set; }

            public int StartIndex { get; set; }
            
            public bool ExpectedValue { get; set; }
            
            public int ExpectedEndIndex { get; set; }
        }
        
        [Test]
        public void TestParseBooleanFail()
        {
            Assert.Throws<OverflowException>(() => SexyParser.Parse("1E500"));
        }
        
        [Test]
        public void TestParseDoubleSuccess()
        {
            List<ParseBooleanTestCase> testCases = new List<ParseBooleanTestCase>
            {
                new ParseBooleanTestCase
                {
                    Input = "#t",
                    ExpectedValue = true,
                    ExpectedEndIndex = 2
                },
                new ParseBooleanTestCase
                {
                    Input = "#f",
                    ExpectedValue = false,
                    ExpectedEndIndex = 2
                },
                new ParseBooleanTestCase
                {
                    Input = "#tidentifier",
                    ExpectedValue = true,
                    ExpectedEndIndex = 2
                },
                new ParseBooleanTestCase
                {
                    Input = "(and #t #f)",
                    StartIndex = 5,
                    ExpectedValue = true,
                    ExpectedEndIndex = 7
                },
                new ParseBooleanTestCase
                {
                    Input = "(and #t #f)",
                    StartIndex = 8,
                    ExpectedValue = false,
                    ExpectedEndIndex = 10
                },
            };
            
            foreach (ParseBooleanTestCase testCase in testCases)
            {
                int index = testCase.StartIndex;
                
                Atom atom = (Atom)SexyParser.Parse(testCase.Input, ref index);
                
                bool expected = testCase.ExpectedValue;
                bool actual = (bool)(atom.Value);
                
                Assert.AreEqual(expected, actual);
                
                int expectedIndex = testCase.ExpectedEndIndex;
                
                Assert.AreEqual(expectedIndex, index);
            }
        }

    }
}

