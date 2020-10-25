#region Licence

// Description: SymuSysDyn - SymuSysDynTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

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
            //Initialize
            Assert.AreEqual(1, Machine.Variables["Stock1"].Value);
            Assert.AreEqual(2, Machine.Variables["Stock2"].Value);
            Assert.AreEqual(1, Machine.Variables["Inflow1"].Value);
            Assert.AreEqual(5, Machine.Variables["Outflow1"].Value);
            Assert.AreEqual(1, Machine.Variables["Aux1"].Value);
            Assert.AreEqual(1, Machine.Variables["Aux2"].Value);
        }

        [TestMethod]
        public void ProcessTest()
        {
            Machine.Process();
            Assert.AreEqual(11, Machine.Results.Count);
            foreach (var result in Machine.Results)
            {
                Assert.AreEqual(6, result.Count);
            }
        }

        [TestMethod]
        public void UpdateVariableTest()
        {
            var stock = Machine.Variables["Stock1"];
            Machine.UpdateVariable(stock);
            Assert.AreEqual(1, stock.Value);
            Assert.IsTrue(stock.Updated);
        }

        [TestMethod]
        public void UpdateVariableTest1()
        {
            var flow = Machine.Variables["Outflow1"];
            Machine.UpdateVariable(flow);
            Assert.AreEqual(5, flow.Value);
            Assert.IsTrue(flow.Updated);
        }

        [TestMethod]
        public void UpdateVariableTest2()
        {
            var flow = Machine.Variables["Inflow1"];
            Machine.UpdateVariable(flow);
            Assert.AreEqual(1, flow.Value);
            Assert.IsTrue(flow.Updated);
        }

        [TestMethod]
        public void UpdateChildrenTest()
        {
            Machine.Variables.Initialize();
            var stock = Machine.Variables["Stock1"];
            var waiting = Machine.UpdateChildren(stock);
            Assert.AreEqual(0, waiting.Count);
            Assert.AreEqual(0, Machine.Variables.GetNotUpdated.Count());
        }

        [TestMethod]
        public void GetGraphTest()
        {
            var graph = Machine.GetGraph();
            Assert.AreEqual(6, graph.VertexCount);
            Assert.AreEqual(6, graph.EdgeCount);
        }

        [TestMethod()]
        public void OptimizeTest()
        {
            //Variables
            var variable = Machine.Variables.Get("Stock1");
            Assert.AreEqual(2, variable.Children.Count);
            variable = Machine.Variables.Get("Inflow1");
            Assert.AreEqual(1, variable.Children.Count);
            Assert.AreEqual(1, variable.Value);
            variable = Machine.Variables.Get("Outflow1");
            Assert.AreEqual(2, variable.Children.Count);
            Assert.AreEqual(5, variable.Value);
            variable = Machine.Variables.Get("Stock2");
            Assert.AreEqual(0, variable.Children.Count);
            Assert.AreEqual(2, variable.Value);
            variable = Machine.Variables.Get("Aux1");
            Assert.AreEqual(0, variable.Children.Count);
            Assert.AreEqual(1, variable.Value);
            variable = Machine.Variables.Get("Aux2");
            Assert.AreEqual(1, variable.Children.Count);
            Assert.AreEqual(1, variable.Value);
            // Optimized
            Assert.AreEqual(1, Machine.OptimizedVariables.Count());
            Assert.AreEqual(1, Machine.OptimizedVariables[0].Value);
            Assert.AreEqual(1, Machine.OptimizedVariables[0].Equation.Variables.Count);
            Assert.AreEqual("Stock1+Dt0*(1-5)", Machine.OptimizedVariables[0].Equation.InitializedEquation);
        }

    }
}