#region Licence

// Description: SymuSysDyn - SymuSysDynTests
// Website: https://symu.org
// Copyright: (c) 2021 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.SysDyn.Core.Engine;
using Symu.SysDyn.Core.Models.XMile;
using Symu.SysDyn.Tests.Classes;

#endregion

namespace Symu.SysDyn.Tests.NewTests
{
    [TestClass]
    public class InitialFunctionTest : FunctionClassTest
    {
        [TestInitialize]
        public async Task InitializeTest()
        {
            await Initialize("Initial_function.xmile");
        }

        [TestMethod]
        public override void StateMachineTest()
        {
            base.StateMachineTest();
            Assert.AreEqual(7, Machine.Variables.Count());
        }

        /// <summary>
        /// Check if dynamic variables are in initial position
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task OptimizeTest()
        {
            Machine.Optimized = false;
            await Machine.Prepare();
            Assert.AreEqual(7, Machine.Variables.Count());
            Assert.IsNotNull(Machine.Variables);
            var stockA = Machine.Variables.Get("_Stocka");
            Assert.IsNotNull(stockA);
            Assert.AreEqual(10, stockA.Value);
            var inflowA = Machine.Variables.Get("_Inflowa");
            Assert.IsNotNull(inflowA);
            Assert.AreEqual(stockA.Value, inflowA.Value);
            var StockAInitialValue = Machine.Variables.Get("_Stocka_initial_value");
            Assert.IsNotNull(StockAInitialValue);
            Assert.AreEqual(inflowA.Value, StockAInitialValue.Value);
            Assert.AreEqual(10, StockAInitialValue.Value);
            Machine.Process();
            stockA = Machine.Variables.Get("_Stocka");
            Assert.AreEqual(10240, stockA.Value);
            inflowA = Machine.Variables.Get("_Inflowa");
            Assert.AreEqual(10240, inflowA.Value);
            Assert.AreEqual(stockA.Value, inflowA.Value);
            StockAInitialValue = Machine.Variables.Get("_Stocka_initial_value");
            Assert.AreEqual(10, StockAInitialValue.Value);


        }
    }
}