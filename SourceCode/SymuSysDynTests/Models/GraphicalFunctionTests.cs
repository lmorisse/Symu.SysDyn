#region Licence

// Description: SymuSysDyn - SymuSysDynTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.SysDyn.Models;

#endregion

namespace SymuSysDynTests.Models
{
    [TestClass]
    public class GraphicalFunctionTests
    {
        private const string Xpts = "0.0, 1.0";
        private const string Ypts = "1.0, 2.0";
        private readonly List<string> _xScaleList = new List<string> {"0.0", "1.0"};
        private readonly List<string> _yScaleList = new List<string> {"1.0", "2.0"};
        private GraphicalFunction _gf;

        [TestMethod]
        public void ParseStringTableTest()
        {
            var result = GraphicalFunction.ParseStringTable(string.Empty);
            Assert.AreEqual(0, result.Length);
            result = GraphicalFunction.ParseStringTable("1.0");
            Assert.AreEqual(1, result.Length);
            Assert.AreEqual(1.0F, result[0]);
            result = GraphicalFunction.ParseStringTable(Ypts);
            Assert.AreEqual(2, result.Length);
            Assert.AreEqual(1.0F, result[0]);
            Assert.AreEqual(2.0F, result[1]);
        }

        [TestMethod]
        public void GraphicalFunctionTest()
        {
            _gf = new GraphicalFunction(null, Ypts, _xScaleList, _yScaleList);
            Assert.AreEqual(2, _gf.XPoints.Length);
            Assert.AreEqual(0, _gf.XScale.Min);
            Assert.AreEqual(1, _gf.XScale.Max);
            Assert.AreEqual(2, _gf.YPoints.Length);
            Assert.AreEqual(1, _gf.YScale.Min);
            Assert.AreEqual(2, _gf.YScale.Max);
        }

        [TestMethod]
        public void GraphicalFunctionTest1()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                new GraphicalFunction(null, Ypts, new List<string>(), _yScaleList));
        }

        [TestMethod]
        public void GraphicalFunctionTest2()
        {
            _gf = new GraphicalFunction(Xpts, Ypts, _xScaleList, _yScaleList);
            Assert.AreEqual(2, _gf.XPoints.Length);
            Assert.AreEqual(0, _gf.XScale.Min);
            Assert.AreEqual(1, _gf.XScale.Max);
            Assert.AreEqual(2, _gf.YPoints.Length);
            Assert.AreEqual(1, _gf.YScale.Min);
            Assert.AreEqual(2, _gf.YScale.Max);
        }
    }
}