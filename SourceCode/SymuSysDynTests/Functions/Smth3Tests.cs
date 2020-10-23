using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.SysDyn.Functions;
using Symu.SysDyn.Simulation;

namespace SymuSysDynTests.Functions
{
    [TestClass()]
    public class Smth3Tests
    {
        private readonly StateMachine _machine = new StateMachine();

        [TestInitialize]
        public void Initialize()
        {
            _machine.Simulation.Stop = 20;
        }
        [TestMethod()]
        public void Smth3Test()
        {
            var smth = new Smth3("SMTH3(5+Step(10,3),5)");
            Assert.AreEqual(string.Empty, smth.Initial);
            Assert.AreEqual("5+Step(10,3)", smth.Input);
            Assert.AreEqual("5", smth.Averaging);
        }
        [TestMethod()]
        public void Smth3Test1()
        {
            var smth = new Smth3("SMTH3(5+Step(10,3),5,2)");
            Assert.AreEqual("2", smth.Initial);
            Assert.AreEqual("5+Step(10,3)", smth.Input);
            Assert.AreEqual("5", smth.Averaging);
        }

        [TestMethod()]
        public void EvaluateTest()
        {
            var smth = new Smth3("SMTH3(5+Step(10,3),5)");
            _machine.Simulation.Time = 2;
            Assert.AreEqual(5, smth.Evaluate(_machine.Variables, _machine.Simulation));
            _machine.Simulation.Time = 4;
            Assert.AreEqual(5, smth.Evaluate(_machine.Variables, _machine.Simulation));
        }
    }
}