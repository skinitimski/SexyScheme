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
    public class TestParseLong
    {
        private struct ParseLongTestCase
        {
            public string Input { get; set; }

            public int StartIndex { get; set; }

            public long ExpectedValue { get; set; }

            public int ExpectedEndIndex { get; set; }
        }
        
        [Test]
        public void TestParseLongFail()
        {
            Assert.Throws<OverflowException>(() => SexyParser.Parse("100000000000000000000000000000000000000000000000000000000000000000000000000000000000"));
        }
            
        [Test]
        public void TestParseLongSuccess()
        {
            List<ParseLongTestCase> testCases = new List<ParseLongTestCase>
            {
                new ParseLongTestCase
                {
                    Input = "0",
                    ExpectedValue = 0,
                    ExpectedEndIndex = 1
                },
                new ParseLongTestCase
                {
                    Input = "-100",
                    ExpectedValue = -100,
                    ExpectedEndIndex = 4
                },
                new ParseLongTestCase
                {
                    Input = "+100",
                    ExpectedValue = 100,
                    ExpectedEndIndex = 4
                },
                new ParseLongTestCase
                {
                    Input = "12345678",
                    ExpectedValue = 12345678,
                    ExpectedEndIndex = 8
                },
                new ParseLongTestCase
                {
                    Input = "(+ 1 22 333 4444)",
                    StartIndex = 3,
                    ExpectedValue = 1,
                    ExpectedEndIndex = 4
                },
                new ParseLongTestCase
                {
                    Input = "(+ 1 22 333 4444)",
                    StartIndex = 5,
                    ExpectedValue = 22,
                    ExpectedEndIndex = 7
                },
                new ParseLongTestCase
                {
                    Input = "(+ 1 22 333 4444)",
                    StartIndex = 8,
                    ExpectedValue = 333,
                    ExpectedEndIndex = 11
                },
                new ParseLongTestCase
                {
                    Input = "(+ 1 22 333 4444)",
                    StartIndex = 12,
                    ExpectedValue = 4444,
                    ExpectedEndIndex = 16
                },
            };
            
            foreach (ParseLongTestCase testCase in testCases)
            {
                int index = testCase.StartIndex;
                
                Atom atom = (Atom)SexyParser.Parse(testCase.Input, ref index);
                
                long expected = testCase.ExpectedValue;
                long actual = (long)(atom.Value);
                
                Assert.AreEqual(expected, actual);
                
                int expectedIndex = testCase.ExpectedEndIndex;
                
                Assert.AreEqual(expectedIndex, index);
            }
        }
    }
}

