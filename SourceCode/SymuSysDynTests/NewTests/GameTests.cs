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

namespace Symu.SysDyn.Tests.NewTests
{
    [TestClass]
    public class GameTests : FunctionClassTest
    {
        [TestInitialize]
        public async Task InitializeTest()
        {
            await Initialize("Game.xmile");
        }

        [TestMethod]
        public override void StateMachineTest()
        {
            base.StateMachineTest();
            Assert.AreEqual(8, Machine.Variables.Count());
        }

        /// <summary>
        /// Test the parser ability to handle the GAME function
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task OptimizeTest()
        {
            Machine.Optimized = false;
            await Machine.Prepare();
            Assert.AreEqual(8, Machine.Variables.Count());
            Assert.IsNotNull(Machine.Variables);
            var variable = Machine.Variables.Get("_Gamed_equation");
            Assert.IsNotNull(variable);
            Assert.AreEqual(3, variable.Value);
            variable = Machine.Variables.Get("_Stock");
            Assert.IsNotNull(variable);
            Assert.AreEqual(0, variable.Value);

            Machine.Process();

            variable = Machine.Variables.Get("_Gamed_equation");
            Assert.IsNotNull(variable);
            Assert.AreEqual(1203, variable.Value);
            variable = Machine.Variables.Get("_Stock");
            Assert.IsNotNull(variable);
            Assert.AreEqual(1200, variable.Value);

        }
    }

}
