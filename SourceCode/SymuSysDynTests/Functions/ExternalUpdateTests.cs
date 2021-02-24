#region Licence

// Description: SymuBiz - SymuSysDynTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.SysDyn.Core.Functions;
using Symu.SysDyn.Core.Models.XMile;
using SymuSysDynTests.Classes;

#endregion

namespace SymuSysDynTests.Functions
{
    [TestClass]
    public class ExternalUpdateTests : BaseClassTest
    {
        [TestInitialize]
        public async Task InitializeTest()
        {
            await Initialize();
        }
        /// <summary>
        /// Without initial value
        /// </summary>
        [TestMethod]
        public async Task ExternalUpdateTest()
        {
            var function = await ExternalUpdate.CreateExternalUpdate(string.Empty, "ExternalUpdate");
            var tryReplace = await function.TryReplace(Machine.Simulation);
            Assert.IsFalse(tryReplace.Success);
            var variable = await Variable.CreateInstance<Variable>("variable", Machine.Models.RootModel, "1");
            Assert.AreEqual(1, await function.Evaluate(variable, Machine.Models.GetVariables(), Machine.Simulation));
        }
        /// <summary>
        /// With initial value
        /// </summary>
        [TestMethod]
        public async Task ExternalUpdateTest1()
        {
            var function = await ExternalUpdate.CreateExternalUpdate(string.Empty, "ExternalUpdate(2)");
            var tryReplace = await function.TryReplace(Machine.Simulation);
            Assert.IsFalse(tryReplace.Success);
            var variable = await Variable.CreateInstance<Variable>("variable", Machine.Models.RootModel, "1");
            //Initial value
            Assert.AreEqual(2, await function.Evaluate(variable, Machine.Models.GetVariables(), Machine.Simulation));
            Machine.Simulation.Step++;
            //Variable value
            Assert.AreEqual(1, await function.Evaluate(variable, Machine.Models.GetVariables(), Machine.Simulation));
        }
    }
}