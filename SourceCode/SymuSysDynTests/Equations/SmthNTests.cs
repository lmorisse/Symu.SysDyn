using Microsoft.VisualStudio.TestTools.UnitTesting;

using Symu.SysDyn.Equations;
using Symu.SysDyn.Simulation;

namespace SymuSysDynTests.Equations
{
    [TestClass()]
    public class SmthNTests
    {
        private readonly SmthN _smth = new SmthN("SMTHN(5+Step(10,3),5,3)");
        private readonly SimSpecs _sim = new SimSpecs(0, 20);
        [TestMethod()]
        public void SmthNTest()
        {
            Assert.AreEqual("0", _smth.Initial);
            Assert.AreEqual("5+Step(10,3)", _smth.Input);
            Assert.AreEqual("5", _smth.Averaging);
            Assert.AreEqual(3, _smth.Order);
        }

        [TestMethod()]
        public void EvaluateTest()
        {
            _sim.Time = 4;
            Assert.AreEqual(5,_smth.Evaluate(_sim));
        }
    }
}