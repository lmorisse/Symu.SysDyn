#region Licence

// Description: SymuSysDyn - SymuSysDynTests
// Website: https://symu.org
// Copyright: (c) 2021 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.SysDyn.Tests.Classes;

#endregion

namespace Symu.SysDyn.Tests.Simulation
{
    [TestClass]
    public class LimitTest : FunctionClassTest
    {
        [TestInitialize]
        public async Task InitializeTest()
        {
            await Initialize("LimitTest.xmile");
        }

        [TestMethod]
        public override void StateMachineTest()
        {
            base.StateMachineTest();
            Assert.AreEqual(6, Machine.Variables.Count());
        }

        /// <summary>
        /// tests the ability to analyse the limits on the variables in the chain of units
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task OptimizeTest()
        {
            Machine.Optimized = false;
            await Machine.Prepare();
            Assert.AreEqual(6, Machine.Variables.Count());
            Assert.IsNotNull(Machine.Variables);
            var variable = Machine.Variables.Get("_Stock");
            Assert.IsNotNull(variable);
            Assert.AreEqual(1, variable.Value);
            Machine.Process();
            variable = Machine.Variables.Get("_Stock");
            Assert.IsNotNull(variable);
            Assert.AreEqual(51, variable.Value);
            Machine.Process();
            variable = Machine.Variables.Get("_Flow");
            Assert.IsNotNull(variable);
            Assert.AreEqual(1, variable.Value);


        }
    }
}