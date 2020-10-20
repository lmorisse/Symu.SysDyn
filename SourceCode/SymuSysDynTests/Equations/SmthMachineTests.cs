using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.SysDyn.Equations;

namespace SymuSysDynTests.Equations
{
    [TestClass()]
    public class SmthMachineTests
    {
        private SmthMachine _smth;
        [TestMethod()]
        public void SmthMachineTest()
        {
            _smth = new SmthMachine("1","2",3,"4");
            Assert.AreEqual("1", _smth.Input);
            Assert.AreEqual("2", _smth.Averaging);
            Assert.AreEqual(3, _smth.Order);
            Assert.AreEqual("4", _smth.Initial);
        }

        /// <summary>
        /// Test SMTH3.xmile template
        /// </summary>
        [TestMethod()]
        public void EvaluateTest()
        {
            _smth = new SmthMachine("5+Step(10,3)", "5", 3);
            Assert.AreEqual(5, _smth.Evaluate(0));
            Assert.IsTrue(5 < _smth.Evaluate(5));
            Assert.IsTrue(10 < _smth.Evaluate(10));
            Assert.IsTrue(14 < _smth.Evaluate(20));
        }
    }
}