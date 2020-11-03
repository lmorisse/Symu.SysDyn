#region Licence

// Description: SymuSysDyn - SymuSysDynTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.SysDyn.Functions;

#endregion

namespace SymuSysDynTests.Functions
{
    [TestClass]
    public class DtTests : BaseClassTest
    {
        [DataRow(0.1F)]
        [DataRow(0.5F)]
        [DataRow(1F)]
        [TestMethod]
        public void DtTest1(float dt)
        {
            var dtFunction = new Dt(string.Empty, "DT");
            Machine.Simulation.DeltaTime = dt;
            Assert.AreEqual(dt, dtFunction.Evaluate(null, Machine.Models.GetVariables(), Machine.Simulation));
        }
    }
}