#region Licence

// Description: SymuSysDyn - SymuSysDynTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.SysDyn.Simulation;

#endregion

namespace SymuSysDynTests.Simulation
{
    [TestClass]
    public class SimSpecsTests
    {
        private SimSpecs _sim = new SimSpecs(0, 10);
        private bool _triggered;

        [TestMethod]
        public void PauseTest()
        {
            Assert.IsFalse(_sim.Pause);
            _sim.PauseInterval = 0;
            Assert.IsFalse(_sim.Pause);
            _sim.PauseInterval = 10;
            Assert.IsTrue(_sim.Pause);
        }


        /// <summary>
        ///     DT = 1, no pause
        /// </summary>
        [TestMethod]
        public void RunTest()
        {
            while (_sim.Run())
            {
            }

            Assert.AreEqual(10, _sim.Time);
        }

        /// <summary>
        ///     DT = 1, pause 5
        /// </summary>
        [TestMethod]
        public void RunTest1()
        {
            _sim.Pause = true;
            _sim.PauseInterval = 5;
            while (_sim.Run())
            {
            }

            Assert.AreEqual(5, _sim.Time);
            Assert.AreEqual(5, _sim.Step);
            while (_sim.Run())
            {
            }

            Assert.AreEqual(10, _sim.Time);
            Assert.AreEqual(10, _sim.Step);
        }

        /// <summary>
        ///     DT > 1
        /// </summary>
        [TestMethod]
        public void RunTest2()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _sim.DeltaTime = 2);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _sim.DeltaTime = 0);
        }

        /// <summary>
        ///     DT < 1, no pause
        /// </summary>
        [TestMethod]
        public void RunTest3()
        {
            _sim.DeltaTime = 0.25F;
            while (_sim.Run())
            {
            }

            Assert.AreEqual(10, _sim.Time);
            Assert.AreEqual(40, _sim.Step);
        }

        /// <summary>
        ///     DT < 1, pause 5
        /// </summary>
        [TestMethod]
        public void RunTest4()
        {
            _sim.Pause = true;
            _sim.PauseInterval = 5;
            _sim.DeltaTime = 0.25F;
            while (_sim.Run())
            {
            }

            Assert.AreEqual(5, _sim.Time);
            Assert.AreEqual(20, _sim.Step);
            while (_sim.Run())
            {
            }

            Assert.AreEqual(10, _sim.Time);
            Assert.AreEqual(40, _sim.Step);
        }

        /// <summary>
        ///     Start = stop
        /// </summary>
        [TestMethod]
        public void RunTest5()
        {
            _sim = new SimSpecs(0, 0);
            while (_sim.Run())
            {
            }

            Assert.AreEqual(0, _sim.Time);
            Assert.AreEqual(0, _sim.Step);
        }

        /// <summary>
        ///     Start = stop , DT < 1
        /// </summary>
        [TestMethod]
        public void RunTest6()
        {
            _sim = new SimSpecs(0, 0) {DeltaTime = 0.25F};
            while (_sim.Run())
            {
            }

            Assert.AreEqual(0, _sim.Time);
            Assert.AreEqual(0, _sim.Step);
        }

        /// <summary>
        ///     DT == 1
        /// </summary>
        [TestMethod]
        public void TimeManagementTest()
        {
            _sim.OnTimer += OnTimer;
            _sim.Step = 5;
            _sim.TimeManagement();
            Assert.IsTrue(_triggered);
        }

        /// <summary>
        ///     DT < 1
        /// </summary>
        [TestMethod]
        public void TimeManagementTest1()
        {
            _sim.OnTimer += OnTimer;
            _sim.DeltaTime = 0.5F;
            _sim.Step = 5;
            _sim.TimeManagement();
            Assert.IsFalse(_triggered);
            _sim.Step = 6;
            _sim.TimeManagement();
            Assert.IsTrue(_triggered);
        }

        /// <summary>
        ///     Timer has a new value, we store the results
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTimer(object sender, EventArgs e)
        {
            _triggered = true;
        }
    }
}