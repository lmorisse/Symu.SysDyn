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
    public class Function_capitalization : FunctionClassTest
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
            Assert.AreEqual(9, Machine.Variables.Count());
        }

        //Check if non-dynamic variable consider capitalization on the xmile file
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