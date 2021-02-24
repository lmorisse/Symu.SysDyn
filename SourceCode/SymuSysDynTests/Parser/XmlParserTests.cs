#region Licence

// Description: SymuBiz - SymuSysDynTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

#region Licence

// Description: SymuSysDyn - SymuSysDynTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.SysDyn.Core.Models.XMile;
using SymuSysDynTests.Classes;

#endregion

#endregion

namespace SymuSysDynTests.Parser
{
    [TestClass]
    public class XmlParserTests : BaseClassTest
    {
        [TestInitialize]
        public async Task InitializeTest()
        {
            await Initialize();
        }
        [TestMethod]
        public async Task ParseVariablesTest()
        {
            var models = await Parser.ParseModels();
            Assert.AreEqual(2, models.Count());
            Assert.AreEqual(8, models.RootModel.Variables.Count());
            Assert.AreEqual(2, models[1].Variables.Count());
        }

        [TestMethod]
        public void ParseSimSpecsTest()
        {
            var sim = Parser.ParseSimSpecs();
            Assert.AreEqual(0, sim.Start);
            Assert.AreEqual(10, sim.Stop);
            Assert.AreEqual(1, sim.DeltaTime);
            Assert.IsTrue(sim.Pause);
            Assert.AreEqual(10, sim.PauseInterval);
        }

        [TestMethod]
        public async Task ParseAuxiliariesTest()
        {
            await Parser.ParseAuxiliaries(XElement, RootModel);
            var aux = RootModel.Variables.OfType<Auxiliary>().ToList();
            Assert.AreEqual(3, aux.Count);
            Assert.AreEqual("_Aux1", aux[0].FullName);
            Assert.AreEqual(1, aux[0].Value);
            Assert.AreEqual("_Aux2", aux[1].FullName);
            Assert.AreEqual(0, aux[1].Value);
            Assert.AreEqual("_Aux3", aux[2].FullName);
            Assert.AreEqual(3, aux[2].Value);
        }

        [TestMethod]
        public async Task ParseFlowsTest()
        {
            await Parser.ParseFlows(XElement, RootModel);
            var flows = RootModel.Variables.OfType<Flow>().ToList();
            Assert.AreEqual(2, flows.Count);
            Assert.AreEqual("_Inflow1", flows[0].FullName);
            Assert.AreEqual("_Stock2/2", flows[0].Equation.InitializedEquation);
            Assert.AreEqual("_Outflow1", flows[1].FullName);
            Assert.AreEqual("_Stock2*2+_Aux2", flows[1].Equation.InitializedEquation);
        }

        [TestMethod]
        public async Task ParseStocksTest()
        {
            await Parser.ParseStocks(XElement, RootModel);
            var stocks = RootModel.Variables.Stocks.ToList();
            Assert.AreEqual(2, stocks.Count);
            Assert.AreEqual("_Stock1", stocks[0].FullName);
            Assert.AreEqual(1, stocks[0].Value);
            Assert.AreEqual(1, stocks[0].Inflow.Count);
            Assert.AreEqual(1, stocks[0].Outflow.Count);
            Assert.AreEqual("_Stock2", stocks[1].FullName);
            Assert.AreEqual(2, stocks[1].Value);
            Assert.AreEqual(0, stocks[1].Inflow.Count);
            Assert.AreEqual(0, stocks[1].Outflow.Count);
        }

        [TestMethod]
        public void ParseModulesTest()
        {
            Parser.ParseModules(XElement, Model);
            Assert.AreEqual(1, Model.Variables.Count());
            Assert.IsTrue(Model.Variables.Exists("1_Hares"));
        }

        [TestMethod]
        public void ParseConnectsTest()
        {
            XElement = XElement.Descendants(Ns + "module").First();
            var connects = Parser.ParseConnects(XElement);
            Assert.AreEqual(2, connects.Count());
            Assert.AreEqual("Hares_Area", connects[0].To);
            Assert.AreEqual("_Aux3", connects[0].From);
            Assert.AreEqual("Hares_Lynxes", connects[1].To);
            Assert.AreEqual("_Aux2", connects[1].From);
        }

        [TestMethod]
        public void ParseGraphicalFunctionTest()
        {
            XElement = XElement.Descendants(Ns + "aux").First();
            var gf = Parser.ParseGraphicalFunction(XElement);
            Assert.AreEqual(0, gf.XScale.Min);
            Assert.AreEqual(1, gf.XScale.Max);
            Assert.AreEqual(0, gf.YScale.Min);
            Assert.AreEqual(1, gf.YScale.Max);
        }

        [TestMethod]
        public void ParseRangeTest()
        {
            XElement = XElement.Descendants(Ns + "stock").First();
            var range = Parser.ParseRange(XElement, "range");
            Assert.AreEqual(-1, range.Min);
            Assert.AreEqual(10, range.Max);
        }

        [TestMethod]
        public void ParseRangeTest1()
        {
            XElement = XElement.Descendants(Ns + "flow").ElementAt(1);
            var range = Parser.ParseRange(XElement, "range");
            Assert.AreEqual(float.NegativeInfinity, range.Min);
            Assert.AreEqual(10, range.Max);
        }

        [TestMethod]
        public void ParseRangeTest2()
        {
            XElement = XElement.Descendants(Ns + "stock").ElementAt(1);
            var range = Parser.ParseRange(XElement, "range");
            Assert.AreEqual(float.NegativeInfinity, range.Min);
            Assert.AreEqual(float.PositiveInfinity, range.Max);
        }

        [TestMethod]
        public void ParseScaleTest()
        {
            XElement = XElement.Descendants(Ns + "stock").First();
            var range = Parser.ParseRange(XElement, "scale");
            Assert.AreEqual(-1, range.Min);
            Assert.AreEqual(10, range.Max);
        }

        [TestMethod]
        public void ParseScaleTest1()
        {
            XElement = XElement.Descendants(Ns + "flow").ElementAt(1);
            var range = Parser.ParseRange(XElement, "scale");
            Assert.AreEqual(-1, range.Min);
            Assert.AreEqual(float.PositiveInfinity, range.Max);
        }

        [TestMethod]
        public void ParseScaleTest2()
        {
            XElement = XElement.Descendants(Ns + "stock").ElementAt(1);
            var range = Parser.ParseRange(XElement, "scale");
            Assert.AreEqual(float.NegativeInfinity, range.Min);
            Assert.AreEqual(float.PositiveInfinity, range.Max);
        }

        [TestMethod]
        public void ParseGroupsTest()
        {
            Assert.AreEqual(2, RootModel.Groups.Count());
            Assert.IsTrue(RootModel.Groups.Exists("Group1"));
            var group1 = RootModel.Groups.Get("Group1").Entities;
            Assert.AreEqual(2, group1.Count);
            Assert.AreEqual("_Stock1", group1[0]);
            Assert.AreEqual("_Aux1", group1[1]);
            Assert.IsTrue(RootModel.Groups.Exists("Group2"));
            var group2 = RootModel.Groups.Get("Group2").Entities;
            Assert.AreEqual("_Stock2", group2[0]);
        }
    }
}