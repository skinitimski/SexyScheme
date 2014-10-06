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
    public class TestParseSymbol
    {        
        private struct ParseSymbolTestCase
        {
            public string Input { get; set; }

            public int StartIndex { get; set; }

            public string ExpectedValue { get; set; }

            public int ExpectedEndIndex { get; set; }
        }
                
        [Test]
        public void TestParseSymbolFail()
        {
            Assert.Throws<SexyParserException>(() => SexyParser.Parse(""));
        }
        
        [Test]
        public void TestParseSymbolSuccess()
        {
            List<ParseSymbolTestCase> testCases = new List<ParseSymbolTestCase>
            {
                new ParseSymbolTestCase
                {
                    Input = "a",
                    ExpectedValue = "a",
                    ExpectedEndIndex = 1
                },
                new ParseSymbolTestCase
                {
                    Input = "ab_c",
                    ExpectedValue = "ab_c",
                    ExpectedEndIndex = 4
                },
                new ParseSymbolTestCase
                {
                    Input = "(a bunch of symbols)",
                    StartIndex  = 1,
                    ExpectedValue = "a",
                    ExpectedEndIndex = 2
                },
                new ParseSymbolTestCase
                {
                    Input = "(a bunch of symbols)",
                    StartIndex  = 3,
                    ExpectedValue = "bunch",
                    ExpectedEndIndex = 8
                },
                new ParseSymbolTestCase
                {
                    Input = "(a bunch of symbols)",
                    StartIndex  = 9,
                    ExpectedValue = "of",
                    ExpectedEndIndex = 11
                },
                new ParseSymbolTestCase
                {
                    Input = "(a bunch of symbols)",
                    StartIndex  = 12,
                    ExpectedValue = "symbols",
                    ExpectedEndIndex = 19
                },
            };
            
            foreach (ParseSymbolTestCase testCase in testCases)
            {
                int index = testCase.StartIndex;
                
                Atom atom = (Atom)SexyParser.Parse(testCase.Input, ref index);
                
                string expected = testCase.ExpectedValue;
                string actual = (string)(atom.Value);
                
                Assert.AreEqual(expected, actual);
                
                int expectedIndex = testCase.ExpectedEndIndex;
                
                Assert.AreEqual(expectedIndex, index);
            }
        }
    }
}

