#region Licence

// Description: SymuSysDyn - SymuSysDynTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.SysDyn.Functions;
using Symu.SysDyn.Model;
using Symu.SysDyn.Simulation;

#endregion

namespace SymuSysDynTests.Functions
{
    [TestClass]
    public class BuiltInFunctionTests
    {
        private readonly StateMachine _machine = new StateMachine();

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
        ///     No parameter
        /// </summary>
        [TestMethod]
        public void BuiltInFunctionTest2()
        {
            var function = new BuiltInFunction("Func()");
            Assert.AreEqual("Func", function.Name);
            Assert.AreEqual(0, function.Parameters.Count);
        }

        /// <summary>
        ///     One literal parameter
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
        ///     Two literal parameters
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
        ///     Mix parameters
        /// </summary>
        [TestMethod]
        public void BuiltInFunctionTest5()
        {
            var function = new BuiltInFunction("func(param1,5)");
            Assert.AreEqual("Func", function.Name);
            Assert.AreEqual(2, function.Parameters.Count);
            Assert.AreEqual("Param1", function.Parameters[0].Variables.First());
            Assert.IsNull(function.Parameters[1]);
            Assert.AreEqual(2, function.Args.Count);
            Assert.AreEqual(0, function.Args[0]);
            Assert.AreEqual(5, function.Args[1]);
        }

        #region Evaluate

        /// <summary>
        ///     Prepare with no literal parameters
        /// </summary>
        [TestMethod]
        public void EvaluateTest()
        {
            var function = new BuiltInFunction("abs(-5)");
            function.Prepare(null, _machine.Variables, _machine.Simulation);
            Assert.AreEqual(5F, function.Evaluate(null, _machine.Variables, _machine.Simulation));
        }

        /// <summary>
        ///     Prepare with a literal parameter
        /// </summary>
        [TestMethod]
        public void EvaluateTest1()
        {
            var function = new BuiltInFunction("abs(aux1)");
            Variable.CreateInstance(_machine.Variables, "Aux1", "1");
            function.Prepare(null, _machine.Variables, _machine.Simulation);
            Assert.AreEqual(1F, function.Expression.Parameters["Aux1"]);
            Assert.AreEqual(1F, function.Evaluate(null, _machine.Variables, _machine.Simulation));
        }

        #endregion

        #region

        [TestMethod]
        public void ReplaceTest()
        {
            var function = new BuiltInFunction("abs(param1)");
            function.Replace("Param1", "1");
            Assert.AreEqual(1, function.InitialValue());
        }

        [TestMethod]
        public void ReplaceTest1()
        {
            var function = new BuiltInFunction("Min(param1, param2)");
            function.Replace("Param1", "1");
            function.Replace("Param2", "2");
            Assert.AreEqual(1, function.InitialValue());
        }

        [TestMethod]
        public void ReplaceTest2()
        {
            var function = new BuiltInFunction("Min(abs(param1), param1+param2)");
            function.Replace("Param1", "1");
            function.Replace("Param2", "2");
            Assert.AreEqual(1, function.InitialValue());
        }

        #endregion
    }
}