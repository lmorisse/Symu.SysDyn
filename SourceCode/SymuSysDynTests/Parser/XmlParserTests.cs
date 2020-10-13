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

namespace SymuSysDynTests.Parser
{
    [TestClass]
    public class XmlParserTests : BaseClassTest
    {
        [TestMethod]
        public void ParseVariablesTest()
        {
            var variables = Parser.ParseVariables();
            Assert.AreEqual(5, variables.Count());
        }

        [TestMethod]
        public void ParseSimSpecsTest()
        {
            var sim = Parser.ParseSimSpecs();
            Assert.AreEqual(0, sim.Start);
            Assert.AreEqual(10, sim.Stop);
            Assert.AreEqual(1, sim.DeltaTime);
        }

        [TestMethod]
        public void ParseAuxiliariesTest()
        {
            Parser.ParseAuxiliaries(XElement, Variables);
            Assert.AreEqual(1, Variables.Count());
            Assert.AreEqual("aux1", Variables[0].Name);
            Assert.AreEqual(2, Variables[0].Value);
        }

        [TestMethod]
        public void ParseFlowsTest()
        {
            Parser.ParseFlows(XElement, Variables);
            Assert.AreEqual(2, Variables.Count());
            Assert.IsTrue(Variables.Exists("inflow1"));
            Assert.IsTrue(Variables.Exists("outflow1"));
            Assert.AreEqual("stock2/2", Variables[0].Equation);
        }

        [TestMethod]
        public void ParseStocksTest()
        {
            Parser.ParseStocks(XElement, Variables);
            Assert.AreEqual(2, Variables.Count());
            Assert.IsTrue(Variables.Exists("stock1"));
            Assert.IsTrue(Variables.Exists("stock2"));
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
            Assert.AreEqual(0, gf.XMin);
            Assert.AreEqual(1, gf.XMax);
            Assert.AreEqual(0, gf.YMin);
            Assert.AreEqual(1, gf.YMax);
        }
    }
}