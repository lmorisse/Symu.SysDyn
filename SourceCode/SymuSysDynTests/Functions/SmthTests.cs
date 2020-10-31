using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.SysDyn.Functions;
using Symu.SysDyn.Model;
using Symu.SysDyn.Simulation;

namespace SymuSysDynTests.Functions
{
    [TestClass()]
    public class SmthTests
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
            Assert.AreEqual("5+Step(10,3)", smth.Initial);
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
        /// <summary>
        /// With Initial value
        /// </summary>
        [TestMethod()]
        public void EvaluateTest()
        {
            var smth = new Smth1("SMTH1(5+Step(10,3),5,5)");
            _machine.Simulation.Time = 1;
            Assert.AreEqual(5, smth.Evaluate(null, _machine.Variables, _machine.Simulation));
            _machine.Simulation.Time = 2;
            Assert.AreEqual(5, smth.Evaluate(null, _machine.Variables, _machine.Simulation));
            _machine.Simulation.Time = 4;
            Assert.AreEqual(7, smth.Evaluate(null, _machine.Variables, _machine.Simulation));
        }
        /// <summary>
        /// Without initial value
        /// </summary>
        [TestMethod()]
        public void EvaluateTest1()
        {
            var smth = new Smth1("SMTH1(5+Step(10,3),5)");
            _machine.Simulation.Time = 1;
            Assert.AreEqual(5, smth.Evaluate(null, _machine.Variables, _machine.Simulation));
            _machine.Simulation.Time = 2;
            Assert.AreEqual(5, smth.Evaluate(null, _machine.Variables, _machine.Simulation));
            _machine.Simulation.Time = 4;
            Assert.AreEqual(7, smth.Evaluate(null, _machine.Variables, _machine.Simulation));
        }

        /// <summary>
        /// With external parameter
        /// </summary>
        [TestMethod()]
        public void EvaluateTest2()
        {
            var aux = new Auxiliary("aux", "5+Step(10,3)+aux2");
            _machine.Variables.Add(aux);
            var aux1 = new Auxiliary("aux1", "SMTH1(aux, 5)");
            _machine.Variables.Add(aux1);
            var aux2 = new Auxiliary("aux2", "0");
            _machine.Variables.Add(aux2);
            _machine.Initialize();
            _machine.Simulation.Time = 4;
            _machine.Compute();
            //At step 4, Aux = 15 => Aux1 = SMTH1(15,5)
            Assert.AreEqual(7, _machine.Variables[1].Value);
        }
    }
}