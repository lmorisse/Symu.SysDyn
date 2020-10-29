#region Licence

// Description: SymuSysDyn - SymuSysDynTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.SysDyn.Model;

#endregion

namespace SymuSysDynTests.Model
{
    [TestClass]
    public class RangeTests
    {
        private readonly float[] _koPoints = {0, 1, -1, 3, 12};
        private readonly float[] _okPoints = {0, 1, 2, 3};
        private readonly Range _range = new Range(0, 10);

        [TestMethod]
        public void CheckTest()
        {
            Assert.IsTrue(_range.Check(_okPoints));
            Assert.IsFalse(_range.Check(_koPoints));
        }
    }
}