using Symu.SysDyn.Simulation;
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
            Assert.AreEqual(0, Machine.Results.Count);
            //Initialize
            Assert.AreEqual(1, Machine.ReferenceVariables["_Stock1"].Value);
            Assert.AreEqual(2, Machine.ReferenceVariables["_Stock2"].Value);
            Assert.AreEqual(1, Machine.ReferenceVariables["_Inflow1"].Value);
            Assert.AreEqual(5, Machine.ReferenceVariables["_Outflow1"].Value);
            Assert.AreEqual(1, Machine.ReferenceVariables["_Aux1"].Value);
            Assert.AreEqual(1, Machine.ReferenceVariables["_Aux2"].Value);
        }

        [TestMethod]
        public void ProcessTest()
        {
            Machine.Process();
            Assert.AreEqual(11, Machine.Results.Count);
            foreach (var result in Machine.Results)
            {
                Assert.AreEqual(10, result.Count);
            }
        }

        [TestMethod]
        public void UpdateVariableTest()
        {
            var stock = Machine.Models.RootModel.Variables["_Stock1"];
            Machine.UpdateVariable(stock);
            Assert.AreEqual(1, stock.Value);
            Assert.IsTrue(stock.Updated);
        }

        [TestMethod]
        public void UpdateVariableTest1()
        {
            var flow = Machine.Models.RootModel.Variables["_Outflow1"];
            Machine.UpdateVariable(flow);
            Assert.AreEqual(5, flow.Value);
            Assert.IsTrue(flow.Updated);
        }

        [TestMethod]
        public void UpdateVariableTest2()
        {
            var flow = Machine.Models.RootModel.Variables["_Inflow1"];
            Machine.UpdateVariable(flow);
            Assert.AreEqual(1, flow.Value);
            Assert.IsTrue(flow.Updated);
        }

        [TestMethod]
        public void UpdateChildrenTest()
        {
            Machine.Models.RootModel.Initialize();
            var stock = Machine.Models.RootModel.Variables["_Stock1"];
            var waiting = Machine.UpdateChildren(stock);
            Assert.AreEqual(0, waiting.Count);
            Assert.AreEqual(0, Machine.Models.GetVariables().GetNotUpdated.Count());
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
            Machine.OptimizeVariables();
            //Variables
            var variable = Machine.ReferenceVariables.Get("_Stock1");
            Assert.AreEqual(1, variable.Value);
            Assert.AreEqual(2, variable.Children.Count);
            variable = Machine.ReferenceVariables.Get("_Inflow1");
            Assert.AreEqual(1, variable.Children.Count);
            Assert.AreEqual(1, variable.Value);
            variable = Machine.ReferenceVariables.Get("_Outflow1");
            Assert.AreEqual(2, variable.Children.Count);
            Assert.AreEqual(5, variable.Value);
            variable = Machine.ReferenceVariables.Get("_Stock2");
            Assert.AreEqual(0, variable.Children.Count);
            Assert.AreEqual(2, variable.Value);
            variable = Machine.ReferenceVariables.Get("_Aux1");
            Assert.AreEqual(0, variable.Children.Count);
            Assert.AreEqual(1, variable.Value);
            variable = Machine.ReferenceVariables.Get("_Aux2");
            Assert.AreEqual(1, variable.Children.Count);
            Assert.AreEqual(1, variable.Value);
            // Optimized
            Assert.AreEqual(2, Machine.Models.Count());
            Assert.AreEqual(1, Machine.Models.RootModel.Variables[0].Value);
            Assert.AreEqual(1, Machine.Models.RootModel.Variables[0].Equation.Variables.Count);
            Assert.AreEqual("_Stock1+1*(1-5)", Machine.Models.RootModel.Variables[0].Equation.InitializedEquation);
        }

        [TestMethod()]
        public void ResolveConnectsTest()
        {
            Assert.AreEqual(1, Machine.ReferenceVariables.Get("Hares_Area").Value);
            Assert.AreEqual("TIME", Machine.ReferenceVariables.Get("Hares_Lynxes").Equation.OriginalEquation);
            Machine.ResolveConnects();
            Assert.AreEqual("ABS(aux1)", Machine.ReferenceVariables.Get("_Aux2").Equation.OriginalEquation);
            Assert.AreEqual(3, Machine.ReferenceVariables.Get("_Aux3").Value);
            //Area.Value has been replaced by Aux3.Value
            Assert.AreEqual(3, Machine.ReferenceVariables.Get("Hares_Area").Value);
            //Lynxes.Equation has been replaced by Aux2
            var lynxes = Machine.ReferenceVariables.Get("Hares_Lynxes");
            Assert.AreEqual("_Aux2", lynxes.Equation.OriginalEquation);
            Assert.AreEqual(1, lynxes.Value);
            Assert.AreEqual(1, lynxes.Children.Count);
            Assert.AreEqual("_Aux2", lynxes.Children[0]);

        }
    }
}