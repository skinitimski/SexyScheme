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
    public class TestParseCharacter
    {
        private struct ParseCharacterTestCase
        {
            public string Input { get; set; }
            
            public int StartIndex { get; set; }
            
            public char ExpectedValue { get; set; }
            
            public int ExpectedEndIndex { get; set; }
        }
        
        [Test]
        public void TestParseCharacterFail()
        {
            Assert.Throws<NotSupportedException>(() => SexyParser.Parse("#\\u0020"));
            Assert.Throws<SexyParserException>(() => SexyParser.Parse("#e"));
            Assert.Throws<SexyParserException>(() => SexyParser.Parse("#\\ef"));
        }
        
        [Test]
        public void TestParseCharacterSuccess()
        {
            List<ParseCharacterTestCase> testCases = new List<ParseCharacterTestCase>
            {
                new ParseCharacterTestCase
                {
                    Input = "#\\c",
                    ExpectedValue = 'c',
                    ExpectedEndIndex = 3
                },
                new ParseCharacterTestCase
                {
                    Input = "#\\r",
                    ExpectedValue = 'r',
                    ExpectedEndIndex = 3
                },
                new ParseCharacterTestCase
                {
                    Input = "'(#\\c #\\a)",
                    StartIndex = 2,
                    ExpectedValue = 'c',
                    ExpectedEndIndex = 5
                },
                new ParseCharacterTestCase
                {
                    Input = "'(#\\c #\\a)",
                    StartIndex = 6,
                    ExpectedValue = 'a',
                    ExpectedEndIndex = 9
                },
            };
            
            foreach (ParseCharacterTestCase testCase in testCases)
            {
                int index = testCase.StartIndex;
                
                Atom atom = (Atom)SexyParser.Parse(testCase.Input, ref index);
                
                char expected = testCase.ExpectedValue;
                char actual = (char)(atom.Value);
                
                Assert.AreEqual(expected, actual);
                
                int expectedIndex = testCase.ExpectedEndIndex;

                Assert.AreEqual(expectedIndex, index);
            }
        }
        
    }
}

