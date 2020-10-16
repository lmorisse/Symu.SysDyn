#region Licence

// Description: SymuSysDyn - SymuSysDynTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            Assert.IsNotNull(Machine.Variables);
            Assert.AreEqual(0, Machine.Results.Count);
        }

        [TestMethod]
        public void ProcessTest()
        {
            Machine.Process();
            Assert.AreEqual(11, Machine.Results.Count);
            foreach (var result in Machine.Results)
            {
                Assert.AreEqual(5, result.Count);
            }
        }

        [TestMethod]
        public void UpdateVariableTest()
        {
            var stock = Machine.Variables["Stock1"];
            Machine.UpdateVariable(stock);
            Assert.AreEqual(0, stock.Value);
            Assert.IsTrue(stock.Updated);
        }

        [TestMethod]
        public void UpdateVariableTest1()
        {
            Machine.Variables.Initialize();
            var flow = Machine.Variables["Outflow1"];
            Machine.UpdateVariable(flow);
            Assert.AreEqual(4, flow.Value);
            Assert.IsTrue(flow.Updated);
        }

        [TestMethod]
        public void UpdateChildrenTest()
        {
            Machine.Variables.Initialize();
            var stock = Machine.Variables["Stock1"];
            var waiting = Machine.UpdateChildren(stock);
            Assert.AreEqual(0, waiting.Count);
            Assert.AreEqual(1, Machine.Variables.GetNotUpdated.Count());
        }

        [TestMethod]
        public void GetGraphTest()
        {
            var graph = Machine.GetGraph();
            Assert.AreEqual(5, graph.VertexCount);
            Assert.AreEqual(4, graph.EdgeCount);
        }
    }
}