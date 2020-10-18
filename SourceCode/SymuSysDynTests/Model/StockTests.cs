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
        private readonly List<string> _inflows = new List<string> {"Inflow1", "Inflow2"};
        private readonly List<string> _outflows = new List<string> {"Outflow1", "Outflow2"};
        private Stock _stock;

        [TestMethod]
        public void StockTest()
        {
            _stock = new Stock("name", Equation1, _inflows, _outflows);
            Assert.AreEqual("Name", _stock.Name);
            CollectionAssert.AreEqual(_inflows, _stock.Inflow);
            CollectionAssert.AreEqual(_outflows, _stock.Outflow);
            Assert.AreEqual("1", _stock.Equation.ToString());
            Assert.AreEqual(1, _stock.Value);
            Assert.AreEqual(0, _stock.Children.Count);
        }

        /// <summary>
        ///     No inflow nor outflow
        /// </summary>
        [TestMethod]
        public void SetStockEquationTest()
        {
            _stock = new Stock("name", Equation1, new List<string>(), new List<string>());
            _stock.SetStockEquation();
            Assert.AreEqual(_stock.Name, _stock.Equation.ToString());
            Assert.AreEqual(0, _stock.Children.Count);
        }

        /// <summary>
        ///     With inflow and no outflow
        /// </summary>
        [TestMethod]
        public void SetStockEquationTest1()
        {
            _stock = new Stock("name", Equation1, _inflows, new List<string>());
            _stock.SetStockEquation();
            Assert.AreEqual("Name + Dt0 * ( Inflow1 + Inflow2 )", _stock.Equation.ToString());
            Assert.AreEqual(2, _stock.Children.Count);
        }

        /// <summary>
        ///     With no inflow and outflow
        /// </summary>
        [TestMethod]
        public void SetStockEquationTest2()
        {
            _stock = new Stock("name", Equation1, new List<string>(), _outflows);
            _stock.SetStockEquation();
            Assert.AreEqual("Name + Dt0 * ( - Outflow1 - Outflow2 )", _stock.Equation.ToString());
            Assert.AreEqual(2, _stock.Children.Count);
        }

        /// <summary>
        ///     With inflow and outflow
        /// </summary>
        [TestMethod]
        public void SetStockEquationTest3()
        {
            _stock = new Stock("name", Equation1, _inflows, _outflows);
            _stock.SetStockEquation();
            Assert.AreEqual("Name + Dt0 * ( Inflow1 + Inflow2 - Outflow1 - Outflow2 )", _stock.Equation.ToString());
            Assert.AreEqual(4, _stock.Children.Count);
        }
    }
}