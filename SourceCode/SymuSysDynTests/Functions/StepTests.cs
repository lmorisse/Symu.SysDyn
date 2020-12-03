#region Licence

// Description: SymuBiz - SymuSysDynTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.SysDyn.Functions;
using SymuSysDynTests.Classes;

#endregion

namespace SymuSysDynTests.Functions
{
    [TestClass]
    public class StepTests : BaseClassTest
    {
        [TestMethod]
        public void StepTest()
        {
            var function = new Step(string.Empty, "STEP(5, 10)");
            Assert.AreEqual("5", function.Height);
            Assert.AreEqual("10", function.StartTime);
        }

        /// <summary>
        ///     Non passing test
        /// </summary>
        [TestMethod]
        public void EvaluateTest()
        {
            var function = new Step(string.Empty, "STEP(5, 10)");
            Machine.Simulation.Time = 0;
            function.Prepare(null, Machine.Models.GetVariables(), Machine.Simulation);
            Assert.AreEqual(0, function.Evaluate(null, Machine.Models.GetVariables(), Machine.Simulation));
            Machine.Simulation.Time = 10;
            function.Prepare(null, Machine.Models.GetVariables(), Machine.Simulation);
            Assert.AreEqual(5, function.Evaluate(null, Machine.Models.GetVariables(), Machine.Simulation));
            Machine.Simulation.Time = 20;
            function.Prepare(null, Machine.Models.GetVariables(), Machine.Simulation);
            Assert.AreEqual(5, function.Evaluate(null, Machine.Models.GetVariables(), Machine.Simulation));
        }

        [TestMethod]
        public void EvaluateTest2()
        {
            var function = new Step(string.Empty, "STEP(aux1, 10)");
            Machine.Simulation.Time = 10;
            function.Prepare(null, Machine.Models.GetVariables(), Machine.Simulation);
            Assert.AreEqual(1F, function.Expression.Parameters["_Aux1"]);
            Assert.AreEqual(1, function.Evaluate(null, Machine.Models.GetVariables(), Machine.Simulation));
        }

        [TestMethod]
        public void EvaluateTest3()
        {
            var function = new Step(string.Empty, "STEP(5, aux1)");
            Machine.Simulation.Time = 1;
            function.Prepare(null, Machine.Models.GetVariables(), Machine.Simulation);
            Assert.AreEqual(1F, function.Expression.Parameters["_Aux1"]);
            Assert.AreEqual(5, function.Evaluate(null, Machine.Models.GetVariables(), Machine.Simulation));
        }

        [TestMethod]
        public void EvaluateTest4()
        {
            var function = new Step(string.Empty, "STEP(aux1, aux1)");
            Machine.Simulation.Time = 1;
            function.Prepare(null, Machine.Models.GetVariables(), Machine.Simulation);
            Assert.AreEqual(1F, function.Expression.Parameters["_Aux1"]);
            Assert.AreEqual(1, function.Evaluate(null, Machine.Models.GetVariables(), Machine.Simulation));
        }
    }
}