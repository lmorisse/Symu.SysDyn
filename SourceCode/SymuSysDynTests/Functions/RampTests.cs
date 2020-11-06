#region Licence

// Description: SymuBiz - SymuSysDynTests
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
    public class RampTests : BaseClassTest
    {
        [TestMethod]
        public void RampTest()
        {
            var function = new Ramp(string.Empty, "RAMP(20, -7)");
            Assert.AreEqual("20", function.Time);
            Assert.AreEqual("-7", function.Slope);
        }

        /// <summary>
        ///     Non passing test
        /// </summary>
        [TestMethod]
        public void EvaluateTest()
        {
            var function = new Ramp(string.Empty, "RAMP(20, -7)");
            Machine.Simulation.Time = 20;
            Assert.AreEqual(0, function.Evaluate(null, Machine.Models.GetVariables(), Machine.Simulation));
            Machine.Simulation.Time = 30;
            Assert.AreEqual(-70, function.Evaluate(null, Machine.Models.GetVariables(), Machine.Simulation));
        }

        [TestMethod]
        public void EvaluateTest2()
        {
            var function = new Ramp(string.Empty, "RAMP(20, aux1)");
            Machine.Simulation.Time = 30;
            function.Prepare(null, Machine.Models.GetVariables(), Machine.Simulation);
            Assert.AreEqual(1F, function.Expression.Parameters["_Aux1"]);
            Assert.AreEqual(10F, function.Evaluate(null, Machine.Models.GetVariables(), Machine.Simulation));
        }

        [TestMethod]
        public void EvaluateTest3()
        {
            var function = new Ramp(string.Empty, "RAMP(aux1, -7)");
            Machine.Simulation.Time = 1;
            function.Prepare(null, Machine.Models.GetVariables(), Machine.Simulation);
            Assert.AreEqual(1F, function.Expression.Parameters["_Aux1"]);
            Assert.AreEqual(0F, function.Evaluate(null, Machine.Models.GetVariables(), Machine.Simulation));
        }

        [TestMethod]
        public void EvaluateTest4()
        {
            var function = new Ramp(string.Empty, "RAMP(aux1, aux1)");
            Machine.Simulation.Time = 10;
            function.Prepare(null, Machine.Models.GetVariables(), Machine.Simulation);
            Assert.AreEqual(1F, function.Expression.Parameters["_Aux1"]);
            Assert.AreEqual(9F, function.Evaluate(null, Machine.Models.GetVariables(), Machine.Simulation));
        }
    }
}