#region Licence

// Description: SymuBiz - SymuSysDynTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.SysDyn.Functions;
using Symu.SysDyn.Models;
using Symu.SysDyn.Models.XMile;

#endregion

namespace SymuSysDynTests.Functions
{
    [TestClass]
    public class ExternalUpdateTests : BaseClassTest
    {
        /// <summary>
        /// Without initial value
        /// </summary>
        [TestMethod]
        public void ExternalUpdateTest()
        {
            var function = new ExternalUpdate(string.Empty, "ExternalUpdate");
            Assert.IsFalse(function.TryReplace(Machine.Simulation, out var result));
            var variable = Variable.CreateInstance("variable", Machine.Models.RootModel, "1");
            Assert.AreEqual(1, function.Evaluate(variable, Machine.Models.GetVariables(), Machine.Simulation));
        }
        /// <summary>
        /// With initial value
        /// </summary>
        [TestMethod]
        public void ExternalUpdateTest1()
        {
            var function = new ExternalUpdate(string.Empty, "ExternalUpdate(2)");
            Assert.IsFalse(function.TryReplace(Machine.Simulation, out var result));
            var variable = Variable.CreateInstance("variable", Machine.Models.RootModel, "1");
            //Initial value
            Assert.AreEqual(2, function.Evaluate(variable, Machine.Models.GetVariables(), Machine.Simulation));
            Machine.Simulation.Step++;
            //Variable value
            Assert.AreEqual(1, function.Evaluate(variable, Machine.Models.GetVariables(), Machine.Simulation));
        }
    }
}