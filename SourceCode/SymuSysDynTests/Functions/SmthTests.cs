#region Licence

// Description: SymuBiz - SymuSysDynTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.SysDyn.Core.Engine;
using Symu.SysDyn.Core.Functions;
using Symu.SysDyn.Core.Models.XMile;

#endregion

namespace SymuSysDynTests.Functions
{
    [TestClass]
    public class SmthTests
    {
        private readonly StateMachine _machine = new StateMachine();

        [TestInitialize]
        public void Initialize()
        {
            _machine.Simulation.Stop = 20;
        }

        [TestMethod]
        public void Smth1Test()
        {
            var smth = new Smth1(string.Empty, "SMTH1(5+Step(10,3),5)");
            Assert.AreEqual("5+Step(10,3)", smth.Initial);
            Assert.AreEqual("5+Step(10,3)", smth.Input);
            Assert.AreEqual("5", smth.Averaging);
        }

        [TestMethod]
        public void Smth1Test1()
        {
            var smth = new Smth1(string.Empty, "SMTH1(5+Step(10,3),5,2)");
            Assert.AreEqual("2", smth.Initial);
            Assert.AreEqual("5+Step(10,3)", smth.Input);
            Assert.AreEqual("5", smth.Averaging);
        }

        /// <summary>
        ///     With Initial value
        /// </summary>
        [TestMethod]
        public void EvaluateTest()
        {
            var smth = new Smth1(string.Empty, "SMTH1(5+Step(10,3),5,5)");
            _machine.Simulation.Time = 1;
            Assert.AreEqual(5, smth.Evaluate(null, _machine.Models.GetVariables(), _machine.Simulation));
            _machine.Simulation.Time = 2;
            Assert.AreEqual(5, smth.Evaluate(null, _machine.Models.GetVariables(), _machine.Simulation));
            _machine.Simulation.Time = 4;
            Assert.AreEqual(7, smth.Evaluate(null, _machine.Models.GetVariables(), _machine.Simulation));
        }

        /// <summary>
        ///     Without initial value
        /// </summary>
        [TestMethod]
        public void EvaluateTest1()
        {
            var smth = new Smth1(string.Empty, "SMTH1(5+Step(10,3),5)");
            _machine.Simulation.Time = 1;
            Assert.AreEqual(5, smth.Evaluate(null, _machine.Models.GetVariables(), _machine.Simulation));
            _machine.Simulation.Time = 2;
            Assert.AreEqual(5, smth.Evaluate(null, _machine.Models.GetVariables(), _machine.Simulation));
            _machine.Simulation.Time = 4;
            Assert.AreEqual(7, smth.Evaluate(null, _machine.Models.GetVariables(), _machine.Simulation));
        }

        /// <summary>
        ///     With external parameter
        /// </summary>
        [TestMethod]
        public void EvaluateTest2()
        {
            Auxiliary.CreateInstance("aux", _machine.Models.RootModel, "5+Step(10,3)+aux2");
            Auxiliary.CreateInstance("aux1", _machine.Models.RootModel, "SMTH1(aux, 5)");
            Auxiliary.CreateInstance("aux2", _machine.Models.RootModel, "0");
            _machine.Initialize();
            _machine.Simulation.Time = 4;
            _machine.Compute();
            Assert.AreEqual(15, _machine.Models.RootModel.Variables[1].Value);
        }
    }
}