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
    public class TestParseComment
    {        
        private struct ParseCommentTestCase
        {
            public string Input { get; set; }

            public int StartIndex { get; set; }

            public ISExp ExpectedValue { get; set; }

            public int ExpectedEndIndex { get; set; }
        }
                
        [Test]
        public void TestParseCommentFail()
        {
            Assert.Throws<SexyParserException>(() => SexyParser.Parse("; this is a comment"));
        }
        
        [Test]
        public void TestParseCommentSuccess()
        {
            List<ParseCommentTestCase> testCases = new List<ParseCommentTestCase>
            {
                new ParseCommentTestCase
                {
                    Input = "; this is a comment\n1",
                    ExpectedValue = Atom.CreateLong(1),
                    ExpectedEndIndex = 21
                },

                new ParseCommentTestCase
                {
                    Input = "\"String; not comment\"",
                    ExpectedValue = Atom.CreateString("String; not comment"),
                    ExpectedEndIndex = 21
                },
            };
            
            foreach (ParseCommentTestCase testCase in testCases)
            {
                int index = testCase.StartIndex;
                
                ISExp expected = testCase.ExpectedValue;
                ISExp actual = SexyParser.Parse(testCase.Input, ref index);

                Assert.AreEqual(expected, actual);
                
                int expectedIndex = testCase.ExpectedEndIndex;
                
                Assert.AreEqual(expectedIndex, index);
            }
        }
    }
}

