#region Licence

// Description: SymuBiz - SymuSysDynTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SymuSysDynTests.Classes;

#endregion

namespace SymuSysDynTests.Simulation
{
    [TestClass]
    public class StateMachineTests : BaseClassTest
    {
        [TestInitialize]
        public async Task InitializeTest()
        {
            await Initialize();
        }
        [TestMethod]
        public void StateMachineTest()
        {
            Assert.IsNotNull(Machine.Simulation);
            Assert.IsNotNull(Machine.Results);
            Assert.IsNotNull(Machine.ReferenceVariables);
            Assert.IsNotNull(Machine.Variables);
            Assert.AreEqual(0, Machine.Results.Count);
            Assert.AreEqual(10, Machine.Variables.Count());
        }

        /// <summary>
        ///     Optimized by default
        /// </summary>
        [TestMethod]
        public void ProcessTest()
        {
            Machine.Process();
            Assert.AreEqual(10, Machine.Results.Count);
            foreach (var result in Machine.Results)
            {
                Assert.AreEqual(1, result.Count);
            }
        }

        /// <summary>
        ///     Non optimized
        /// </summary>
        [TestMethod]
        public void ProcessTest1()
        {
            Machine.Optimized = false;
            Machine.Process();
            Assert.AreEqual(10, Machine.Results.Count);
            foreach (var result in Machine.Results)
            {
                Assert.AreEqual(9, result.Count); // Module is not computes
            }
        }

        [TestMethod]
        public async Task UpdateVariableTest()
        {
            await Machine.Prepare();
            var stock = Machine.Variables["_Stock1"];
            await Machine.UpdateVariable(stock);
            Assert.AreEqual(1, stock.Value);
            Assert.IsTrue(stock.Updated);
        }

        [TestMethod]
        public async Task UpdateVariableTest1()
        {
            Machine.Optimized = false;
            await Machine.Prepare();
            var flow = Machine.Variables["_Outflow1"];
            await Machine.UpdateVariable(flow);
            Assert.AreEqual(5, flow.Value);
            Assert.IsTrue(flow.Updated);
        }

        [TestMethod]
        public async Task UpdateChildrenTest()
        {
            await Machine.Prepare();
            var stock = Machine.Variables["_Stock1"];
            var waiting = await Machine.UpdateChildren(stock);
            Assert.AreEqual(0, waiting.Count);
            Assert.AreEqual(0, Machine.Variables.GetNotUpdated.Count());
        }

        /// <summary>
        ///     Optimize with constants
        /// </summary>
        /// <remarks>See StateMachineSMTH3Tests for non constant variables</remarks>
        [TestMethod]
        public async Task OptimizeTest()
        {
            Machine.Optimized = true;
            await Machine.Prepare();
            Assert.AreEqual(1, Machine.Variables.Count());
            Assert.AreEqual(1, Machine.Variables[0].Value);
            Assert.AreEqual(1, Machine.Variables[0].Equation.Variables.Count);
            Assert.AreEqual("_Stock1+1*(1-5)", Machine.Variables[0].Equation.InitializedEquation);
        }

        [TestMethod]
        public async Task ResolveConnectsTest()
        {
            Assert.AreEqual(1, Machine.Variables.Get("Hares_Area").Value);
            Assert.AreEqual("TIME", Machine.Variables.Get("Hares_Lynxes").Equation.OriginalEquation);
            Machine.Optimized = false;
            await Machine.Prepare();
            Assert.AreEqual("_Aux3", Machine.Variables.Get("Hares_Area").Equation.OriginalEquation);
            Assert.AreEqual("_Aux2", Machine.Variables.Get("Hares_Lynxes").Equation.OriginalEquation);
        }
    }
}