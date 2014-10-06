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
    public partial class TestPrimitives
    {
        protected SexyEvaluator Evaluator = new SexyEvaluator();

        [SetUp]
        public void ResetEvaluator()
        {
            Evaluator = new SexyEvaluator();
        }
    }
}