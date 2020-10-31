#region Licence

// Description: SymuSysDyn - SymuSysDynTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.SysDyn.Functions;

#endregion

namespace SymuSysDynTests.Functions
{
    [TestClass]
    public class TimeTests : BaseClassTest
    {
        [DataRow(0)]
        [DataRow(10)]
        [DataRow(100)]
        [TestMethod]
        public void TimeTest(int time)
        {
            var timeFunction = new Time("TIME");
            Machine.Simulation.Time = (ushort) time;
            Assert.AreEqual(time, timeFunction.Evaluate(null, Machine.Variables, Machine.Simulation));
        }
    }
}