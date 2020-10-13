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
    public class StockTests
    {
        private const string Equation1 = "1";
        private readonly List<string> _inflows = new List<string> {"inflow1", "inflow2"};
        private readonly List<string> _outflows = new List<string> {"outflow1", "outflow2"};
        private Stock _stock;

        [TestMethod]
        public void StockTest()
        {
            _stock = new Stock("name", Equation1, _inflows, _outflows);
            Assert.AreEqual("name", _stock.Name);
            CollectionAssert.AreEqual(_inflows, _stock.Inflow);
            CollectionAssert.AreEqual(_outflows, _stock.Outflow);
            Assert.AreNotEqual("1", _stock.Equation);
            Assert.AreEqual(1, _stock.Value);
            Assert.AreEqual(4, _stock.Children.Count);
        }

        /// <summary>
        ///     No inflow nor outflow
        /// </summary>
        [TestMethod]
        public void SetStockEquationTest()
        {
            _stock = new Stock("name", Equation1, new List<string>(), new List<string>());
            Assert.AreEqual(_stock.Name, _stock.Equation);
        }

        /// <summary>
        ///     With inflow and outflow
        /// </summary>
        [TestMethod]
        public void SetStockEquationTest1()
        {
            _stock = new Stock("name", Equation1, _inflows, _outflows);
            Assert.AreEqual("name + inflow1 + inflow2 - outflow1 - outflow2", _stock.Equation);
        }
    }
}