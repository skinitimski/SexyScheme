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
    public class TestParseString
    {        
        private struct ParseStringTestCase
        {
            public string Input { get; set; }

            public int StartIndex { get; set; }

            public string ExpectedValue { get; set; }

            public int ExpectedEndIndex { get; set; }
        }
                
        [Test]
        public void TestParseStringFail()
        {
            Assert.Throws<SexyParserException>(() => SexyParser.Parse("\"unending"));
            Assert.Throws<NotSupportedException>(() => SexyParser.Parse("\"Th\\u0069s\""));
            Assert.Throws<FormatException>(() => SexyParser.Parse("\"\\a\""));
        }
        
        [Test]
        public void TestParseStringSuccess()
        {
            List<ParseStringTestCase> testCases = new List<ParseStringTestCase>
            {
                new ParseStringTestCase
                {
                    Input = "\"\"",
                    ExpectedValue = "",
                    ExpectedEndIndex = 2
                },
                new ParseStringTestCase
                {
                    Input = "\"Hi\"",
                    ExpectedValue = "Hi",
                    ExpectedEndIndex = 4
                },
                new ParseStringTestCase
                {
                    Input = "(string-concat \"left\" \"-\" \"right\"",
                    StartIndex = 15,
                    ExpectedValue = "left",
                    ExpectedEndIndex = 21
                },
                new ParseStringTestCase
                {
                    Input = "(string-concat \"left\" \"-\" \"right\"",
                    StartIndex = 22,
                    ExpectedValue = "-",
                    ExpectedEndIndex = 25
                },
                new ParseStringTestCase
                {
                    Input = "(string-concat \"left\" \"-\" \"right\"",
                    StartIndex = 26,
                    ExpectedValue = "right",
                    ExpectedEndIndex = 33
                },
            };
            
            foreach (ParseStringTestCase testCase in testCases)
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

