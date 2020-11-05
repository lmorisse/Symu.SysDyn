#region Licence

// Description: SymuSysDyn - SymuSysDynTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
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
            Assert.IsNotNull(Machine.Variables);
            Assert.AreEqual(0, Machine.Results.Count);
            Assert.AreEqual(10, Machine.Variables.Count());
        }

        [TestMethod]
        public void ProcessTest()
        {
            Machine.Process();
            Assert.AreEqual(10, Machine.Results.Count);
            foreach (var result in Machine.Results)
            {
                Assert.AreEqual(9, result.Count);// Module is not processed
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
            Machine.Prepare();
            var flow = Machine.Variables["_Outflow1"];
            Machine.UpdateVariable(flow);
            Assert.AreEqual(5, flow.Value);
            Assert.IsTrue(flow.Updated);
        }

        [TestMethod]
        public void UpdateVariableTest2()
        {
            Machine.Prepare();
            var flow = Machine.Variables["_Inflow1"];
            Machine.UpdateVariable(flow);
            Assert.AreEqual(1, flow.Value);
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

        [TestMethod]
        public void GetGraphTest()
        {
            var graph = Machine.GetGraph();
            Assert.AreEqual(10, graph.VertexCount);
            Assert.AreEqual(8, graph.EdgeCount);
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
            Machine.OptimizeVariables();

            Assert.AreEqual(1, Machine.Variables.Count());
            Assert.AreEqual(1, Machine.Variables[0].Value);
            Assert.AreEqual(1, Machine.Variables[0].Equation.Variables.Count);
            Assert.AreEqual("_Stock1+1*(1-5)", Machine.Variables[0].Equation.InitializedEquation);
        }

        [TestMethod()]
        public void ResolveConnectsTest()
        {
            Assert.AreEqual(1, Machine.Variables.Get("Hares_Area").Value);
            Assert.AreEqual("TIME", Machine.Variables.Get("Hares_Lynxes").Equation.OriginalEquation);
            Machine.Prepare(); 
            Assert.AreEqual("ABS(aux1)", Machine.Variables.Get("_Aux2").Equation.OriginalEquation);
            Assert.AreEqual(3, Machine.Variables.Get("_Aux3").Value);
            //Area.Value has been replaced by Aux3.Value
            Assert.AreEqual(3, Machine.Variables.Get("Hares_Area").Value);
            //Lynxes.Equation has been replaced by Aux2
            var lynxes = Machine.Variables.Get("Hares_Lynxes");
            Assert.AreEqual("_Aux2", lynxes.Equation.OriginalEquation);
            Assert.AreEqual(1, lynxes.Value);
            Assert.AreEqual(1, lynxes.Children.Count);
            Assert.AreEqual("_Aux2", lynxes.Children[0]);

        }
    }
}