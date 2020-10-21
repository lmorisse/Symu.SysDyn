using Microsoft.VisualStudio.TestTools.UnitTesting;

using Symu.SysDyn.Equations;
using Symu.SysDyn.Model;
using Symu.SysDyn.Simulation;

namespace SymuSysDynTests.Equations
{
    [TestClass()]
    public class Smth1Tests
    {
        private readonly StateMachine _machine = new StateMachine();

        [TestInitialize]
        public void Initialize()
        {
            _machine.Simulation.Stop = 20;
        }

        [TestMethod()]
        public void Smth1Test()
        {
            var smth = new Smth1("SMTH1(5+Step(10,3),5)");
            Assert.AreEqual(string.Empty, smth.Initial);
            Assert.AreEqual("5+Step(10,3)", smth.Input);
            Assert.AreEqual("5", smth.Averaging);
        }

        [TestMethod()]
        public void Smth1Test1()
        {
            var smth = new Smth1("SMTH1(5+Step(10,3),5,2)");
            Assert.AreEqual("2", smth.Initial);
            Assert.AreEqual("5+Step(10,3)", smth.Input);
            Assert.AreEqual("5", smth.Averaging);
        }

        [TestMethod()]
        public void EvaluateTest()
        {
            var smth = new Smth1("SMTH1(5+Step(10,3),5)");
            _machine.Simulation.Time = 4;
            Assert.AreEqual(6, smth.Evaluate(_machine.Variables, _machine.Simulation));
        }

        /// <summary>
        /// With external parameter
        /// </summary>
        [TestMethod()]
        public void EvaluateTest1()
        {
            _machine.Simulation.Time = 4;
            var aux = new Auxiliary("aux", "5+Step(10,3)");
            _machine.Variables.Add(aux);
            var aux1 = new Auxiliary("aux1", "SMTH1(aux, 5)");
            _machine.Variables.Add(aux1);
            _machine.Compute();
            Assert.AreEqual(6, _machine.Variables[1].Value);
        }
    }
}