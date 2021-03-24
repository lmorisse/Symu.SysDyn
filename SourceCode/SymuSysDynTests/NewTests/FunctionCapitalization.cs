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
    public class Function_capitalization : FunctionClassTest
    {
        [TestInitialize]
        public async Task InitializeTest()
        {
            await Initialize("Function_capitalization.xmile");
        }

        [TestMethod]
        public override void StateMachineTest()
        {
            base.StateMachineTest();
            Assert.AreEqual(9, Machine.Variables.Count());
        }

        /// <summary>
        /// Check if non-dynamic variable consider capitalization on the xmile file and test the abs(int) function
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task OptimizeTest()
        {
            Machine.Optimized = false;
            await Machine.Prepare();
            Assert.IsNotNull(Machine.Variables);
            var variable = Machine.Variables.Get("_Test1");
            Assert.IsNotNull(variable);
            Assert.AreEqual(5, variable.Value);
            variable = Machine.Variables.Get("_Test2");
            Assert.IsNotNull(variable);
            Assert.AreEqual(5, variable.Value);
            variable = Machine.Variables.Get("_Test3");
            Assert.IsNotNull(variable);
            Assert.AreEqual(5, variable.Value);
        }
    }
}