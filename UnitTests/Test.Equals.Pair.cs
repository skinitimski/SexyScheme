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
    public partial class TestEquals
    {
        [Test]
        public void TestEqualsPair()
        {
            Pair pair1, pair2, emptyList;

            emptyList = (Pair)SexyParser.Parse("()");

            pair1 = (Pair)SexyParser.Parse("()");
            
            Assert.IsTrue(pair1.Equals(pair1));    
            Assert.IsFalse(pair1.Equals(null));

            pair2 = (Pair)SexyParser.Parse("( )");
            
            Assert.IsTrue(pair1.Equals(pair2));
            Assert.IsTrue(pair2.Equals(pair1));

            pair2 = (Pair)SexyParser.Parse("(1)");

            Assert.IsTrue(pair2.Equals(pair2));
            
            Assert.IsFalse(pair1.Equals(pair2));
            Assert.IsFalse(pair2.Equals(pair1));

            pair1 = (Pair)SexyParser.Parse("(1)");
            
            Assert.IsTrue(pair1.Equals(pair2));
            Assert.IsTrue(pair2.Equals(pair1));

            pair1 = (Pair)SexyParser.Parse("(1 ())");
            
            Assert.IsTrue(pair1.Equals(pair1));
            
            Assert.IsFalse(pair1.Equals(pair2));
            Assert.IsFalse(pair2.Equals(pair1));


        }
    }
}

