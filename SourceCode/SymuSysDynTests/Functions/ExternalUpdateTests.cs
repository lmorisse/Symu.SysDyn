#region Licence

// Description: SymuSysDyn - SymuSysDynTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.SysDyn.Functions;
using Symu.SysDyn.Models;

#endregion

namespace SymuSysDynTests.Functions
{
    [TestClass]
    public class ExternalUpdateTests : BaseClassTest
    {
        [TestMethod]
        public void ExternalUpdateTest1()
        {
            var function = new ExternalUpdate(string.Empty, "ExternalUpdate");
            var variable = Variable.CreateInstance("variable", Machine.Models.RootModel, "1");
            Assert.AreEqual(1, function.Evaluate(variable, Machine.Models.GetVariables(), Machine.Simulation));
        }
    }
}