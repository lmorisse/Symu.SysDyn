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
            Assert.IsNotNull(Machine.ReferenceVariables);
            Assert.AreEqual(0, Machine.Results.Count);
            //Initialize
            Assert.AreEqual(1, Machine.ReferenceVariables["Stock1"].Value);
            Assert.AreEqual(2, Machine.ReferenceVariables["Stock2"].Value);
            Assert.AreEqual(1, Machine.ReferenceVariables["Inflow1"].Value);
            Assert.AreEqual(5, Machine.ReferenceVariables["Outflow1"].Value);
            Assert.AreEqual(1, Machine.ReferenceVariables["Aux1"].Value);
            Assert.AreEqual(1, Machine.ReferenceVariables["Aux2"].Value);
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
        /// <summary>
        /// Optimize with constants
        /// </summary>
        /// <remarks>See StateMachineSMTH3Tests for non constant variables</remarks>
        [TestMethod()]
        public void OptimizeTest()
        {
            Machine.Optimized = true;
            Machine.OptimizeVariables();
            //Variables
            var variable = Machine.ReferenceVariables.Get("Stock1");
            Assert.AreEqual(1, variable.Value);
            Assert.AreEqual(2, variable.Children.Count);
            variable = Machine.ReferenceVariables.Get("Inflow1");
            Assert.AreEqual(1, variable.Children.Count);
            Assert.AreEqual(1, variable.Value);
            variable = Machine.ReferenceVariables.Get("Outflow1");
            Assert.AreEqual(2, variable.Children.Count);
            Assert.AreEqual(5, variable.Value);
            variable = Machine.ReferenceVariables.Get("Stock2");
            Assert.AreEqual(0, variable.Children.Count);
            Assert.AreEqual(2, variable.Value);
            variable = Machine.ReferenceVariables.Get("Aux1");
            Assert.AreEqual(0, variable.Children.Count);
            Assert.AreEqual(1, variable.Value);
            variable = Machine.ReferenceVariables.Get("Aux2");
            Assert.AreEqual(1, variable.Children.Count);
            Assert.AreEqual(1, variable.Value);
            // Optimized
            Assert.AreEqual(1, Machine.Variables.Count());
            Assert.AreEqual(1, Machine.Variables[0].Value);
            Assert.AreEqual(1, Machine.Variables[0].Equation.Variables.Count);
            Assert.AreEqual("Stock1+1*(1-5)", Machine.Variables[0].Equation.InitializedEquation);
        }

    }
}