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
    public class ActiveInitialTest : ActiveInitialClass
    {
        [TestInitialize]
        public async Task InitializeTest()
        {
            await Initialize();
        }

        [TestMethod]
        public void StateMachineTest()
        {
            Assert.IsNotNull(Machine.Simulation);
            Assert.IsNotNull(Machine.Results);
            Assert.IsNotNull(Machine.ReferenceVariables);
            Assert.IsNotNull(Machine.Variables);
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
            var variable = Machine.Variables.Get("_Value_a");
            Assert.IsNotNull(variable);
            Assert.AreEqual(0, variable.Value);
            variable = Machine.Variables.Get("_Stock_a");
            Assert.IsNotNull(variable);
            Assert.AreEqual(0, variable.Value);
            Machine.Process();
            variable = Machine.Variables.Get("_Value_a");
            Assert.IsNotNull(variable);
            Assert.AreEqual(9, variable.Value);
            variable = Machine.Variables.Get("_Stock_a");
            Assert.IsNotNull(variable);
            Assert.AreEqual(10, variable.Value);
        }
    }
}