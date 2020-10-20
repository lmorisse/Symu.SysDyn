using Microsoft.VisualStudio.TestTools.UnitTesting;

using Symu.SysDyn.Equations;
using Symu.SysDyn.Simulation;

namespace SymuSysDynTests.Equations
{
    [TestClass()]
    public class Smth1Tests
    {
        private readonly Smth1 _smth = new Smth1("SMTH1(5+Step(10,3),5)");
        private readonly SimSpecs _sim = new SimSpecs(0, 20);
        [TestMethod()]
        public void Smth1Test()
        {
            Assert.AreEqual("0", _smth.Initial);
            Assert.AreEqual("5+Step(10,3)", _smth.Input);
            Assert.AreEqual("5", _smth.Averaging);
        }

        [TestMethod()]
        public void EvaluateTest()
        {
            _sim.Time = 4;
            Assert.AreEqual(6, _smth.Evaluate(_sim));
        }
    }
}