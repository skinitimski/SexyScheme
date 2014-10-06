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
    public class TestParsePair
    {

        [Test]
        public void TestParsePairSimpleFail()
        {
            Assert.Throws<SexyParserException>(() => SexyParser.Parse("("));
            Assert.Throws<SexyParserException>(() => SexyParser.Parse("(((()))"));            
        }

        [Test]
        public void TestParsePairSimple()
        {
            string rep;
            Pair pair;
            
            rep = "(symbol \"string\" 1 5.0 #t #f #\\t ())";
            pair = (Pair)SexyParser.Parse(rep);

            List<ISExp> list = pair.ToList();

            
            Assert.AreEqual(8, list.Count);
            Assert.AreEqual("symbol", ((Atom)list[0]).Value);
            Assert.AreEqual("string", ((Atom)list[1]).Value);
            Assert.AreEqual(1, ((Atom)list[2]).Value);
            Assert.AreEqual(5.0, ((Atom)list[3]).Value);
            Assert.AreEqual(true, ((Atom)list[4]).Value);
            Assert.AreEqual(false, ((Atom)list[5]).Value);
            Assert.AreEqual('t', ((Atom)list[6]).Value);
            Assert.AreEqual(Pair.Empty, list[7]);
            
            
            
            rep = "((((()))))";
            pair = (Pair)SexyParser.Parse(rep);
            list = pair.ToList();

            for (int i = 0; i < 4; i++)
            {
                Assert.AreEqual(1, list.Count);
                pair = (Pair)list[0];
                list = pair.ToList();
            }



            rep = "(+ 1 (- 3 2) (- 4 2))";
            pair = (Pair)SexyParser.Parse(rep);
            list = pair.ToList();

            Assert.AreEqual(4, list.Count);
            Assert.AreEqual("+", ((Atom)list[0]).Value);
            Assert.AreEqual(1, ((Atom)list[1]).Value);

            Pair pair1 = (Pair)list[2];
            List<ISExp> list1 = pair1.ToList();

            Assert.AreEqual(3, list1.Count);
            Assert.AreEqual("-", ((Atom)list1[0]).Value);
            Assert.AreEqual(3, ((Atom)list1[1]).Value);
            Assert.AreEqual(2, ((Atom)list1[2]).Value);

            pair1 = (Pair)list[3];
            list1 = pair1.ToList();
            
            Assert.AreEqual(3, list1.Count);
            Assert.AreEqual("-", ((Atom)list1[0]).Value);
            Assert.AreEqual(4, ((Atom)list1[1]).Value);
            Assert.AreEqual(2, ((Atom)list1[2]).Value);
        }
    }
}

