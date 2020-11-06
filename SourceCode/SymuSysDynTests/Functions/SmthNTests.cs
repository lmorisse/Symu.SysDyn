#region Licence

// Description: SymuBiz - SymuSysDynTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.SysDyn.Engine;
using Symu.SysDyn.Functions;

#endregion

namespace SymuSysDynTests.Functions
{
    [TestClass]
    public class SmthNTests
    {
        private readonly StateMachine _machine = new StateMachine();

        [TestInitialize]
        public void Initialize()
        {
            _machine.Simulation.Stop = 20;
        }

        [TestMethod]
        public void SmthNTest()
        {
            var smth = new SmthN(string.Empty, "SMTHN(5+Step(10,3),5,3)");
            Assert.AreEqual("5+Step(10,3)", smth.Initial);
            Assert.AreEqual("5+Step(10,3)", smth.Input);
            Assert.AreEqual("5", smth.Averaging);
            Assert.AreEqual(3, smth.Order);
        }

        [TestMethod]
        public void SmthNTest1()
        {
            var smth = new SmthN(string.Empty, "SMTHN(5+Step(10,3),5,3,2)");
            Assert.AreEqual("2", smth.Initial);
            Assert.AreEqual("5+Step(10,3)", smth.Input);
            Assert.AreEqual("5", smth.Averaging);
            Assert.AreEqual(3, smth.Order);
        }

        [TestMethod]
        public void EvaluateTest()
        {
            var smth = new SmthN(string.Empty, "SMTHN(5+Step(10,3),5,3)");
            _machine.Simulation.Time = 2;
            Assert.AreEqual(5, smth.Evaluate(null, _machine.Models.GetVariables(), _machine.Simulation));
            _machine.Simulation.Time = 4;
            Assert.IsTrue(5 < smth.Evaluate(null, _machine.Models.GetVariables(), _machine.Simulation));
        }
    }
}