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
using Symu.SysDyn.Equations;

#endregion

namespace SymuSysDynTests.Parser
{
    [TestClass]
    public class StringFunctionTests
    {
        [TestMethod]
        public void StringFunctionTest0()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new BuiltInFunction(null));
        }

        [TestMethod]
        public void StringFunctionTest()
        {
            var function = new BuiltInFunction("Func1");
            Assert.AreEqual("Func1", function.Name);
            Assert.AreEqual(0, function.Parameters.Count());
        }

        [TestMethod]
        public void StringFunctionTest1()
        {
            var function = new BuiltInFunction("Func1()");
            Assert.AreEqual("Func1", function.Name);
            Assert.AreEqual(0, function.Parameters.Count());
        }

        [TestMethod]
        public void StringFunctionTest2()
        {
            var function = new BuiltInFunction("Func1(param1)");
            Assert.AreEqual("Func1", function.Name);
            Assert.AreEqual(1, function.Parameters.Count());
            Assert.AreEqual("param1", function.Parameters.ToList()[0]);
        }

        [TestMethod]
        public void StringFunctionTest3()
        {
            var function = new BuiltInFunction("Func1(param1, param2)");
            Assert.AreEqual("Func1", function.Name);
            Assert.AreEqual(2, function.Parameters.Count());
            Assert.AreEqual("param1", function.Parameters.ToList()[0]);
            Assert.AreEqual("param2", function.Parameters.ToList()[1]);
        }

        [TestMethod]
        public void GetParametersOfFunctionTest()
        {
            var parameters = BuiltInFunction.GetParametersOfFunction("Func1");
            Assert.AreEqual(0, parameters.Count);
        }

        [TestMethod]
        public void GetParametersOfFunctionTest1()
        {
            var parameters = BuiltInFunction.GetParametersOfFunction("Func1()");
            Assert.AreEqual(0, parameters.Count);
        }

        [TestMethod]
        public void GetParametersOfFunctionTest2()
        {
            var parameters = BuiltInFunction.GetParametersOfFunction("Func1(param1)");
            Assert.AreEqual(1, parameters.Count);
            Assert.AreEqual("param1", parameters[0]);
        }

        [TestMethod]
        public void GetParametersOfFunctionTest3()
        {
            var parameters = BuiltInFunction.GetParametersOfFunction("Func1(param1, param2)");
            Assert.AreEqual(2, parameters.Count);
            Assert.AreEqual("param1", parameters[0]);
            Assert.AreEqual("param2", parameters[1]);
        }
    }
}