#region Licence

// Description: SymuSysDyn - SymuSysDynTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.SysDyn.Functions;

#endregion

namespace SymuSysDynTests.Functions
{
    [TestClass]
    public class TimeTests
    {
        [TestMethod]
        public void IsContainedInTest()
        {
            Assert.IsTrue(Time.IsContainedIn("TIME", out var time));
            Assert.AreEqual("TIME", time);
            Assert.IsTrue(Time.IsContainedIn(" Time ", out time));
            Assert.AreEqual("Time", time);
            Assert.IsTrue(Time.IsContainedIn("-time+", out time));
            Assert.AreEqual("time", time);
            Assert.IsFalse(Time.IsContainedIn("TIMEXXX", out _));
            Assert.IsFalse(Time.IsContainedIn("TIME_XXX", out _));
            Assert.IsFalse(Time.IsContainedIn("XXX_TIME", out _));
        }
    }
}