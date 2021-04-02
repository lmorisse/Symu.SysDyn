#region Licence

// Description: SymuSysDyn - SymuSysDynTests
// Website: https://symu.org
// Copyright: (c) 2021 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.SysDyn.Core.Engine;
using Symu.SysDyn.Core.Models.XMile;
using Symu.SysDyn.Tests.Classes;

#endregion

namespace Symu.SysDyn.Tests.NewTests
{
    [TestClass]
    public class AbsTest : FunctionClassTest
    {
        [TestInitialize]
        public async Task InitializeTest()
        {
            await Initialize("Abs.xmile");
        }

        [TestMethod]
        public override void StateMachineTest()
        {
            base.StateMachineTest();
            Assert.AreEqual(7, Machine.Variables.Count());
        }

        /// <summary>
        /// Tests the absolute value function
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task OptimizeTest()
        {
            Machine.Optimized = false;
            await Machine.Prepare();
            Assert.AreEqual(7, Machine.Variables.Count());
            Assert.IsNotNull(Machine.Variables);
            var stockA = Machine.Variables.Get("_Stocka");
            Assert.IsNotNull(stockA);
            Assert.AreEqual(-10, stockA.Value);
            var testAbs = Machine.Variables.Get("_Test_abs");
            Assert.IsNotNull(testAbs);
            Assert.AreEqual(10, testAbs.Value);
            var flowA = Machine.Variables.Get("_Flowa");
            Assert.IsNotNull(flowA);
            Assert.AreEqual(1, flowA.Value);
            Machine.Process();
            flowA = Machine.Variables.Get("_Flowa");
            Assert.AreEqual(1, flowA.Value);
            stockA = Machine.Variables.Get("_Stocka");
            Assert.AreEqual(10, stockA.Value);
            testAbs = Machine.Variables.Get("_Test_abs");
            Assert.AreEqual(10, testAbs.Value);
        }
    }
}