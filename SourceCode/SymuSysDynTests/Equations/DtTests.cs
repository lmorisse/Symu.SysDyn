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
    public class DtTests
    {
        [TestMethod]
        public void IsContainedInTest()
        {
            Assert.IsTrue(Dt.IsContainedIn("DT"));
            Assert.IsTrue(Dt.IsContainedIn(" DT "));
            Assert.IsTrue(Dt.IsContainedIn("-DT+"));
            Assert.IsFalse(Dt.IsContainedIn("DT_XXX"));
            Assert.IsFalse(Dt.IsContainedIn("XXX_DT"));
        }
    }
}