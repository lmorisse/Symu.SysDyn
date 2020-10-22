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
    public class DtTests
    {
        [TestMethod]
        public void IsContainedInTest()
        {
            Assert.IsTrue(Dt.IsContainedIn("DT", out var dt));
            Assert.AreEqual("DT", dt);
            Assert.IsTrue(Dt.IsContainedIn(" Dt ", out dt));
            Assert.AreEqual("Dt", dt);
            Assert.IsTrue(Dt.IsContainedIn("-dt+", out dt));
            Assert.AreEqual("dt", dt);
            Assert.IsFalse(Dt.IsContainedIn("DTXXX", out _));
            Assert.IsFalse(Dt.IsContainedIn("DT_XXX", out _));
            Assert.IsFalse(Dt.IsContainedIn("XXX_DT", out _));
        }
    }
}