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
    public class ActiveInitialTest : FunctionClassTest
    {
        [TestInitialize]
        public async Task InitializeTest()
        {
            await Initialize("Active_initial.xmile");
        }

        [TestMethod]
        public override void StateMachineTest()
        {
            base.StateMachineTest();
            Assert.AreEqual(7, Machine.Variables.Count());
        }

        /// <summary>
        /// Check if dynamic variables are in initial position
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task OptimizeTest()
        {
            Machine.Optimized = false;
            await Machine.Prepare();
            Assert.AreEqual(7, Machine.Variables.Count());
            Assert.IsNotNull(Machine.Variables);
            var initialValue = Machine.Variables.Get("_Value_a");
            Assert.IsNotNull(initialValue);
            Assert.AreEqual(0, initialValue.Value);
            var initialStock = Machine.Variables.Get("_Stock_a");
            Assert.IsNotNull(initialStock);
            Assert.AreEqual(0, initialStock.Value);
            Machine.Process();
            var variable = Machine.Variables.Get("_Value_a");
            Assert.IsNotNull(variable);
            Assert.AreNotEqual(initialValue, variable.Value);
            variable = Machine.Variables.Get("_Stock_a");
            Assert.IsNotNull(variable);
            Assert.AreNotEqual(initialStock, variable.Value);
        }
    }
}