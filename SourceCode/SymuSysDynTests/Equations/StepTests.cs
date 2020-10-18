#region Licence

// Description: SymuSysDyn - SymuSysDynTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.SysDyn.Equations;
using Symu.SysDyn.Simulation;

#endregion

namespace SymuSysDynTests.Equations
{
    [TestClass]
    public class StepTests
    {
        private readonly Step _function = new Step("STEP(5, 10)");
        private readonly SimSpecs _sim = new SimSpecs(0, 10);

        [TestMethod]
        public void StepBuiltInFunctionTest()
        {
            Assert.AreEqual(5, _function.Height);
            Assert.AreEqual(10, _function.StartTime);
        }

        [TestMethod]
        public void PrepareTest()
        {
            _sim.Time = 0;
            Assert.AreEqual("0", _function.Prepare("Variable1", _sim));
            _sim.Time = 10;
            Assert.AreEqual("5", _function.Prepare("Variable1", _sim));
        }
    }
}