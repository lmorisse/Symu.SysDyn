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
    public class LogicalsTest : FunctionClassTest
    {



        [TestInitialize]
        public async Task InitializeTest()
        {
            await Initialize("Logicals.xmile");
        }

        [TestMethod]
        public override void StateMachineTest()
        {
            base.StateMachineTest();
            Assert.AreEqual(9, Machine.Variables.Count());
        }

        /// <summary>
        /// Tests the AND, OR and NOT logicals
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task OptimizeTest()
        {
            Machine.Optimized = false;
            await Machine.Prepare();
            Assert.AreEqual(9, Machine.Variables.Count());
            Assert.IsNotNull(Machine.Variables);
            var variable = Machine.Variables.Get("_False_input");
            Assert.IsNotNull(variable);
            Assert.AreEqual(0, variable.Value);
            Machine.Process();
            variable = Machine.Variables.Get("_True_input");
            Assert.IsNotNull(variable);
            Assert.AreEqual(1, variable.Value);
            Machine.Process();
            variable = Machine.Variables.Get("_Not_output");
            Assert.IsNotNull(variable);
            Assert.AreEqual(1, variable.Value);
            Machine.Process();
            variable = Machine.Variables.Get("_And_output");
            Assert.IsNotNull(variable);
            Assert.AreEqual(0, variable.Value);
            Machine.Process();
            variable = Machine.Variables.Get("_Or_output");
            Assert.IsNotNull(variable);
            Assert.AreEqual(1, variable.Value);
        }
    }

}
