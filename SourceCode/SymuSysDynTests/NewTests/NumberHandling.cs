#region Licence

// Description: SymuSysDyn - SymuSysDynTests
// Website: https://symu.org
// Copyright: (c) 2021 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives
using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.SysDyn.Tests.Classes;
#endregion

namespace Symu.SysDyn.Tests.NewTests
{
    [TestClass]
    public class NumberHandling : FunctionClassTest
    {
        [TestInitialize]
        public async Task InitializeTest()
        {
            await Initialize("NumberHandling.xmile");
        }

        [TestMethod]
        public override void StateMachineTest()
        {
            base.StateMachineTest();
            Assert.AreEqual(9, Machine.Variables.Count());
        }

        /// <summary>
        /// Test the integer division
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task OptimizeTest()
        {
            Machine.Optimized = false;
            await Machine.Prepare();
            Assert.AreEqual(9, Machine.Variables.Count());
            Assert.IsNotNull(Machine.Variables);
            var quotientTarget = Machine.Variables.Get("_Quotient_target");
            Assert.IsNotNull(quotientTarget);
            Assert.AreEqual(0.75, quotientTarget.Value);
            var numerator = Machine.Variables.Get("_Numerator");
            Assert.IsNotNull(numerator);
            Assert.AreEqual(3, numerator.Value);
            var denominator = Machine.Variables.Get("_Denominator");
            Assert.IsNotNull(denominator);
            Assert.AreEqual(4, denominator.Value);
            var quotient = Machine.Variables.Get("_Quotient");
            Assert.IsNotNull(quotient);
            Assert.AreEqual(0.75, quotient.Value);
            var equality = Machine.Variables.Get("_Equality");
            Assert.IsNotNull(equality);
            Assert.AreEqual(1, equality.Value);
            Machine.Process();
            numerator = Machine.Variables.Get("_Numerator");
            Assert.AreEqual(3, numerator.Value);
            denominator = Machine.Variables.Get("_Denominator");
            Assert.AreEqual(4, denominator.Value);
            equality = Machine.Variables.Get("_Equality");
            Assert.AreEqual(1, equality.Value);
            quotientTarget = Machine.Variables.Get("_Quotient_target");
            Assert.AreEqual(0.75, quotientTarget.Value);
            quotient = Machine.Variables.Get("_Quotient");
            Assert.AreEqual(0.75, quotient.Value);

        }
    }
}
