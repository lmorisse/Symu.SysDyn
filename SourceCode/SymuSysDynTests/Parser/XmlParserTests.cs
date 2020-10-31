#region Licence

// Description: SymuSysDyn - SymuSysDynTests
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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.SysDyn.Model;

#endregion

#endregion

namespace SymuSysDynTests.Parser
{
    [TestClass]
    public class XmlParserTests : BaseClassTest
    {
        [TestMethod]
        public void ParseVariablesTest()
        {
            var variables = Parser.ParseVariables();
            Assert.AreEqual(6, variables.Count());
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
        public void ParseAuxiliariesTest()
        {
            Parser.ParseAuxiliaries(XElement, Variables);
            Assert.AreEqual(2, Variables.Count());
            Assert.AreEqual("Aux1", Variables[0].Name);
            Assert.AreEqual(1, Variables[0].Value);
            Assert.AreEqual("Aux2", Variables[1].Name);
            Assert.AreEqual(0, Variables[1].Value);
        }

        [TestMethod]
        public void ParseFlowsTest()
        {
            Parser.ParseFlows(XElement, Variables);
            Assert.AreEqual(2, Variables.Count());
            Assert.IsTrue(Variables.Exists("Inflow1"));
            Assert.IsTrue(Variables.Exists("Outflow1"));
            Assert.AreEqual("Stock2/2", Variables[0].Equation.InitializedEquation);
        }

        [TestMethod]
        public void ParseStocksTest()
        {
            Parser.ParseStocks(XElement, Variables);
            Assert.AreEqual(2, Variables.Count());
            Assert.IsTrue(Variables.Exists("Stock1"));
            Assert.IsTrue(Variables.Exists("Stock2"));
            var stock = (Stock) Variables[0];
            Assert.AreEqual(1, stock.Value);
            Assert.AreEqual(1, stock.Inflow.Count);
            Assert.AreEqual(1, stock.Outflow.Count);
            var stock2 = (Stock) Variables[1];
            Assert.AreEqual(2, stock2.Value);
            Assert.AreEqual(0, stock2.Inflow.Count);
            Assert.AreEqual(0, stock2.Outflow.Count);
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
            Parser.ParseGroups(XElement, Variables);
            Assert.AreEqual(2, Variables.Groups.Count());
            Assert.IsTrue(Variables.Groups.Exists("Group1"));
            var group1 = Variables.Groups.Get("Group1").Entities;
            Assert.AreEqual(2, group1.Count);
            Assert.AreEqual("Stock1", group1[0]);
            Assert.AreEqual("Aux1", group1[1]);
            Assert.IsTrue(Variables.Groups.Exists("Group2"));
            var group2 = Variables.Groups.Get("Group2").Entities;
            Assert.AreEqual("Stock2", group2[0]);
        }
    }
}