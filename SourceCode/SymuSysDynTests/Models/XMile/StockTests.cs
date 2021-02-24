﻿#region Licence

// Description: SymuBiz - SymuSysDynTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.SysDyn.Core.Models.XMile;

#endregion

namespace SymuSysDynTests.Models.XMile
{
    [TestClass]
    public class StockTests
    {
        private const string Equation1 = "1";
        private readonly List<string> _inflows = new List<string> {"Inflow1", "Inflow2"};
        private readonly List<string> _outflows = new List<string> {"Outflow1", "Outflow2"};
        private Stock _stock;

        [TestMethod]
        public async Task StockTest()
        {
            _stock = await Stock.CreateStock("name", Equation1, _inflows, _outflows);
            Assert.AreEqual("_Name", _stock.FullName);
            CollectionAssert.AreEqual(_inflows, _stock.Inflow);
            CollectionAssert.AreEqual(_outflows, _stock.Outflow);
            Assert.IsNull(_stock.Equation);
            Assert.AreEqual(1, _stock.Value);
            Assert.AreEqual(0, _stock.Children.Count);
        }

        [TestMethod]
        public async Task CloneTest()
        {
            _stock = await Stock.CreateStock("name", "SET(param1, param2)", _inflows, _outflows);
            var clone = (Stock)await _stock.Clone();
            Assert.IsNotNull(clone);
            Assert.AreEqual(clone.FullName, _stock.FullName);
            CollectionAssert.AreEqual(_inflows, clone.Inflow);
            CollectionAssert.AreEqual(_outflows, clone.Outflow);
            Assert.IsNotNull(clone.Equation);
            Assert.AreEqual(2, clone.Children.Count);
        }

        /// <summary>
        ///     No inflow nor outflow
        /// </summary>
        [TestMethod]
        public async Task SetStockEquationTest()
        {
            _stock = await Stock.CreateStock("name", Equation1, new List<string>(), new List<string>());
            await _stock.SetStockEquation("1");
            Assert.AreEqual(_stock.FullName, _stock.Equation.InitializedEquation);
            Assert.AreEqual(0, _stock.Children.Count);
        }

        /// <summary>
        ///     With inflow and no outflow
        /// </summary>
        [TestMethod]
        public async Task SetStockEquationTest1()
        {
            _stock = await Stock.CreateStock("name", Equation1, _inflows, new List<string>());
            await _stock.SetStockEquation("1");
            Assert.AreEqual("_Name+1*(_Inflow1+_Inflow2)", _stock.Equation.InitializedEquation);
            Assert.AreEqual(2, _stock.Children.Count);
        }

        /// <summary>
        ///     With no inflow and outflow
        /// </summary>
        [TestMethod]
        public async Task SetStockEquationTest2()
        {
            _stock = await Stock.CreateStock("name", Equation1, new List<string>(), _outflows);
            await _stock.SetStockEquation("1");
            Assert.AreEqual("_Name+1*(-_Outflow1-_Outflow2)", _stock.Equation.InitializedEquation);
            Assert.AreEqual(2, _stock.Children.Count);
        }

        /// <summary>
        ///     With inflow and outflow
        /// </summary>
        [TestMethod]
        public async Task SetStockEquationTest3()
        {
            _stock = await Stock.CreateStock("name", Equation1, _inflows, _outflows);
            await _stock.SetStockEquation("1");
            Assert.AreEqual("_Name+1*(_Inflow1+_Inflow2-_Outflow1-_Outflow2)", _stock.Equation.InitializedEquation);
            Assert.AreEqual(4, _stock.Children.Count);
        }
    }
}