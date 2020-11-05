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
    public class PulseTests : BaseClassTest
    {
        [TestMethod]
        public void PulseTest()
        {
            var function = new Pulse(string.Empty, "Pulse(20, 12)");
            Assert.AreEqual("20", function.Magnitude);
            Assert.AreEqual("12", function.FirstTime);
            Assert.AreEqual(string.Empty, function.Interval);
        }

        [TestMethod]
        public void NormalTest1()
        {
            var function = new Pulse(string.Empty, "Pulse(20, 12,5)");
            Assert.AreEqual("20", function.Magnitude);
            Assert.AreEqual("12", function.FirstTime);
            Assert.AreEqual("5", function.Interval);
        }

        /// <summary>
        ///     DT = 1
        /// </summary>
        [TestMethod]
        public void EvaluateTest()
        {
            var function = new Pulse(string.Empty, "Pulse(20, 12)");
            Machine.Simulation.Time = 10;
            Assert.AreEqual(0, function.Evaluate(null, null, Machine.Simulation));
            Machine.Simulation.Time = 12;
            Assert.AreEqual(20, function.Evaluate(null,null, Machine.Simulation));
            Machine.Simulation.Time = 17;
            Assert.AreEqual(0, function.Evaluate(null, null, Machine.Simulation));
        }

        /// <summary>
        ///     DT = 1, with interval
        /// </summary>
        [TestMethod]
        public void EvaluateTest1()
        {
            var function = new Pulse(string.Empty, "Pulse(20, 12, 5)");
            Machine.Simulation.Time = 10;
            Assert.AreEqual(0, function.Evaluate(null, null, Machine.Simulation));
            Machine.Simulation.Time = 12;
            Assert.AreEqual(20, function.Evaluate(null, null, Machine.Simulation));
            Machine.Simulation.Time = 15;
            Assert.AreEqual(0, function.Evaluate(null, null, Machine.Simulation));
            Machine.Simulation.Time = 17;
            Assert.AreEqual(20, function.Evaluate(null, null, Machine.Simulation));
        }

        /// <summary>
        ///     DT = 0.5 , with interval
        /// </summary>
        [TestMethod]
        public void EvaluateTest2()
        {
            Machine.Simulation.DeltaTime = 0.5F;

            var function = new Pulse(string.Empty, "Pulse(20, 12, 5)");
            Machine.Simulation.Step = 23;
            Machine.Simulation.Time = 11;
            Assert.AreEqual(0, function.Evaluate(null, null, Machine.Simulation));
            Machine.Simulation.Step = 24;
            Machine.Simulation.Time = 12;
            Assert.AreEqual(10, function.Evaluate(null, null, Machine.Simulation));
            Machine.Simulation.Step = 25;
            Assert.AreEqual(10, function.Evaluate(null, null, Machine.Simulation));
            Machine.Simulation.Step = 27;
            Machine.Simulation.Time = 13;
            Assert.AreEqual(0, function.Evaluate(null, null, Machine.Simulation));
        }



        /// <summary>
        ///     With seed
        /// </summary>
        [TestMethod]
        public void EvaluateTest3()
        {
            var function = new Pulse(string.Empty, "Pulse(variable1+variable2, 0)");
            Variable.CreateInstance("variable1", Machine.Models.RootModel, "1");
            Variable.CreateInstance("variable2", Machine.Models.RootModel, "2");
            Assert.AreEqual(3, function.Evaluate(null, Machine.Models.GetVariables(), Machine.Simulation));
        }

        /// <summary>
        ///     With seed
        /// </summary>
        [TestMethod]
        public void EvaluateTest4()
        {
            var function = new Pulse(string.Empty, "Pulse(variable1, variable2, variable3)");
            Variable.CreateInstance("variable1", Machine.Models.RootModel, "1");
            Variable.CreateInstance("variable2", Machine.Models.RootModel, "2");
            Variable.CreateInstance("variable3", Machine.Models.RootModel, "3");
            Machine.Simulation.Time = 2;
            Assert.AreEqual(1, function.Evaluate(null, Machine.Models.GetVariables(), Machine.Simulation));
        }

        /// <summary>
        ///    with interval = 0
        /// </summary>
        [TestMethod]
        public void EvaluateTest5()
        {
            var function = new Pulse(string.Empty, "Pulse(20, 12, 0)");
            Machine.Simulation.Time = 10;
            Assert.AreEqual(0, function.Evaluate(null, null, Machine.Simulation));
            Machine.Simulation.Time = 12;
            Assert.AreEqual(20, function.Evaluate(null, null, Machine.Simulation));
            Machine.Simulation.Time = 15;
            Assert.AreEqual(0, function.Evaluate(null, null, Machine.Simulation));
            Machine.Simulation.Time = 17;
            Assert.AreEqual(0, function.Evaluate(null, null, Machine.Simulation));
        }
    }
}