﻿using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.SysDyn.Functions;

namespace SymuSysDynTests.Functions
{
    [TestClass()]
    public class BuiltInFunctionTests: BaseClassTest
    {
        [TestMethod]
        public void BuiltInFunctionTest()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new BuiltInFunction(null));
        }

        [TestMethod]
        public void BuiltInFunctionTest1()
        {
            var function = new BuiltInFunction("Func");
            Assert.AreEqual("Func", function.Name);
            Assert.AreEqual(0, function.Parameters.Count);
        }
        /// <summary>
        /// No parameter
        /// </summary>
        [TestMethod]
        public void BuiltInFunctionTest2()
        {
            var function = new BuiltInFunction("Func()");
            Assert.AreEqual("Func", function.Name);
            Assert.AreEqual(0, function.Parameters.Count);
        }
        /// <summary>
        /// One literal parameter
        /// </summary>
        [TestMethod]
        public void BuiltInFunctionTest3()
        {
            var function = new BuiltInFunction("Func(param1)");
            Assert.AreEqual("Func", function.Name);
            Assert.AreEqual(1, function.Parameters.Count);
            Assert.AreEqual("Param1", function.Parameters[0].Variables.First());
        }
        /// <summary>
        /// Two literal parameters
        /// </summary>
        [TestMethod]
        public void BuiltInFunctionTest4()
        {
            var function = new BuiltInFunction("Func(param1, param2)");
            Assert.AreEqual("Func", function.Name);
            Assert.AreEqual(2, function.Parameters.Count);
            Assert.AreEqual("Param1", function.Parameters[0].Variables.First());
            Assert.AreEqual("Param2", function.Parameters[1].Variables.First());
        }
        /// <summary>
        /// Mix parameters
        /// </summary>
        [TestMethod()]
        public void BuiltInFunctionTest5()
        {
            var function = new BuiltInFunction("func(param1,5)");
            Assert.AreEqual("Func", function.Name);
            Assert.AreEqual(2, function.Parameters.Count);
            Assert.AreEqual("Param1", function.Parameters[0].Variables.First());
            Assert.AreEqual("5", function.Parameters[1].OriginalEquation);
        }
        /// <summary>
        /// No parameter
        /// </summary>
        [TestMethod]
        public void SetCleanedFunctionTest()
        {
            var function = new BuiltInFunction("Func()");
            Assert.AreEqual("Func()", function.SetCleanedFunction());
        }
        /// <summary>
        /// One literal parameter
        /// </summary>
        [TestMethod]
        public void SetCleanedFunctionTest1()
        {
            var function = new BuiltInFunction("Func(param)");
            Assert.AreEqual("Func(Param)", function.SetCleanedFunction());
        }
        /// <summary>
        /// Two literal parameters
        /// </summary>
        [TestMethod]
        public void SetCleanedFunctionTest2()
        {
            var function = new BuiltInFunction("Func( param1 , Param2)");
            Assert.AreEqual("Func(Param1,Param2)", function.SetCleanedFunction());
        }
        /// <summary>
        /// Mix parameters
        /// </summary>
        [TestMethod]
        public void SetCleanedFunctionTest3()
        {
            var function = new BuiltInFunction("Func( 1 , Param2)");
            Assert.AreEqual("Func(1,Param2)", function.SetCleanedFunction());
        }
        /// <summary>
        /// Prepare with no literal parameters
        /// </summary>
        [TestMethod()]
        public void EvaluateTest()
        {
            var function = new BuiltInFunction("abs(-5)");
            function.Prepare(Machine.Variables, Machine.Simulation);
            Assert.AreEqual(5F, function.Evaluate(Machine.Variables, Machine.Simulation));
        }

        /// <summary>
        /// Prepare with a literal parameter
        /// </summary>
        [TestMethod()]
        public void EvaluateTest1()
        {
            var function = new BuiltInFunction("abs(aux1)");
            function.Prepare(Machine.Variables, Machine.Simulation);
            Assert.AreEqual(1F, function.Expression.Parameters["Aux1"]);
            Assert.AreEqual(1F, function.Evaluate(Machine.Variables, Machine.Simulation));
        }

    }
}