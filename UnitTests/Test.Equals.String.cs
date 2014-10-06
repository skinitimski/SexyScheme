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
        public void TestEqualsString()
        {
            Atom atom1, atom2;



            atom1 = Atom.CreateString("string");            

            Assert.IsFalse(atom1.Equals(null));
            Assert.IsFalse(atom1.Equals(10L));
            Assert.IsTrue(atom1.Equals(atom1));

            atom2 = Atom.CreateLong(6L);

            Assert.IsFalse(atom1.Equals(atom2));
            
            atom2 = Atom.CreateString("STRING");
            
            Assert.IsFalse(atom1.Equals(atom2));
            
            atom2 = Atom.CreateString("string");
            
            Assert.IsTrue(atom1.Equals(atom2));

        }
    }
}

