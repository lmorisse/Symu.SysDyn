#region Licence

// Description: SymuSysDyn - SymuSysDynTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.SysDyn.Model;

#endregion

namespace SymuSysDynTests.Model
{
    [TestClass]
    public class GraphicalFunctionTests
    {
        private const string Ypts = "0.0, 1.0";
        private readonly List<string> _bounds = new List<string> {"0.0", "1.0", "0.0", "1.0"};
        private GraphicalFunction _gf;

        [TestInitialize]
        public void Initialize()
        {
            _gf = new GraphicalFunction(Ypts, _bounds);
        }

        [TestMethod]
        public void GraphicalFunctionTest()
        {
            Assert.AreEqual(0, _gf.XMin);
            Assert.AreEqual(1, _gf.XMax);
            Assert.AreEqual(0, _gf.YMin);
            Assert.AreEqual(1, _gf.YMax);
        }

        [TestMethod]
        public void GetOutputWithBoundsTest()
        {
            Assert.AreEqual(0, _gf.GetOutputWithBounds(-1));
            Assert.AreEqual(0, _gf.GetOutputWithBounds(0));
            Assert.AreEqual(0.5F, _gf.GetOutputWithBounds(0.5F));
            Assert.AreEqual(1, _gf.GetOutputWithBounds(1));
            Assert.AreEqual(1, _gf.GetOutputWithBounds(2));
        }
    }
}