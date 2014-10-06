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
        public void TestEqualsSymbol()
        {
            Atom atom1, atom2;



            atom1 = new Atom("symbol", AtomType.SYMBOL);            

            Assert.IsFalse(atom1.Equals(null));
            Assert.IsFalse(atom1.Equals("symbol"));
            Assert.IsTrue(atom1.Equals(atom1));

            atom2 = new Atom("string", AtomType.STRING);

            Assert.IsFalse(atom1.Equals(atom2));
            
            atom2 = new Atom("string", AtomType.SYMBOL);
            
            Assert.IsFalse(atom1.Equals(atom2));
            
            atom2 = new Atom("symbol", AtomType.SYMBOL); 
            
            Assert.IsTrue(atom1.Equals(atom2));
            
            atom2 = new Atom("SYMBOL", AtomType.SYMBOL); 
            
            Assert.IsTrue(atom1.Equals(atom2));

        }
    }
}

