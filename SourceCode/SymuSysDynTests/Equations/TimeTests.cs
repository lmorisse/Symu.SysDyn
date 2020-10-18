#region Licence

// Description: SymuSysDyn - SymuSysDynTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.SysDyn.Equations;

#endregion

namespace SymuSysDynTests.Equations
{
    [TestClass]
    public class TimeTests
    {
        [TestMethod]
        public void IsContainedInTest()
        {
            Assert.IsTrue(Time.IsContainedIn("TIME"));
            Assert.IsTrue(Time.IsContainedIn(" TIME "));
            Assert.IsTrue(Time.IsContainedIn("-TIME+"));
            Assert.IsFalse(Time.IsContainedIn("TIME_XXX"));
            Assert.IsFalse(Time.IsContainedIn("XXX_TIME"));
        }
    }
}