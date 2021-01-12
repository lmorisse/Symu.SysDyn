#region Licence

// Description: SymuBiz - SymuSysDynTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SymuSysDynTests.Classes;

#endregion

namespace SymuSysDynTests.Simulation
{
    [TestClass]
    public class StateMachineTests : BaseClassTest
    {
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
        public void UpdateVariableTest()
        {
            Machine.Prepare();
            var stock = Machine.Variables["_Stock1"];
            Machine.UpdateVariable(stock);
            Assert.AreEqual(1, stock.Value);
            Assert.IsTrue(stock.Updated);
        }

        [TestMethod]
        public void UpdateVariableTest1()
        {
            Machine.Optimized = false;
            Machine.Prepare();
            var flow = Machine.Variables["_Outflow1"];
            Machine.UpdateVariable(flow);
            Assert.AreEqual(5, flow.Value);
            Assert.IsTrue(flow.Updated);
        }

        [TestMethod]
        public void UpdateChildrenTest()
        {
            Machine.Prepare();
            var stock = Machine.Variables["_Stock1"];
            var waiting = Machine.UpdateChildren(stock);
            Assert.AreEqual(0, waiting.Count);
            Assert.AreEqual(0, Machine.Variables.GetNotUpdated.Count());
        }

        /// <summary>
        ///     Optimize with constants
        /// </summary>
        /// <remarks>See StateMachineSMTH3Tests for non constant variables</remarks>
        [TestMethod]
        public void OptimizeTest()
        {
            Machine.Optimized = true;
            Machine.Prepare();
            Assert.AreEqual(1, Machine.Variables.Count());
            Assert.AreEqual(1, Machine.Variables[0].Value);
            Assert.AreEqual(1, Machine.Variables[0].Equation.Variables.Count);
            Assert.AreEqual("_Stock1+1*(1-5)", Machine.Variables[0].Equation.InitializedEquation);
        }

        [TestMethod]
        public void ResolveConnectsTest()
        {
            Assert.AreEqual(1, Machine.Variables.Get("Hares_Area").Value);
            Assert.AreEqual("TIME", Machine.Variables.Get("Hares_Lynxes").Equation.OriginalEquation);
            Machine.Optimized = false;
            Machine.Prepare();
            Assert.AreEqual("_Aux3", Machine.Variables.Get("Hares_Area").Equation.OriginalEquation);
            Assert.AreEqual("_Aux2", Machine.Variables.Get("Hares_Lynxes").Equation.OriginalEquation);
        }
    }
}