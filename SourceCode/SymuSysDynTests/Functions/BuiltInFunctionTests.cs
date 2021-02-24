#region Licence

// Description: SymuBiz - SymuSysDynTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NCalcAsync;
using Symu.SysDyn.Core.Engine;
using Symu.SysDyn.Core.Functions;
using Symu.SysDyn.Core.Models.XMile;

#endregion

namespace SymuSysDynTests.Functions
{
    [TestClass]
    public class BuiltInFunctionTests
    {
        private readonly StateMachine _machine = new StateMachine();

        #region Dt
        /// <summary>
        /// Dt : DeltaTime is treated like a parameter
        /// Attention, it is case sensitive!
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        [DataRow(0.1F)]
        [DataRow(0.5F)]
        [DataRow(1F)]
        [TestMethod]
        public async Task DtTest(float dt)
        {
            var e = new Expression("Dt");
            e.Parameters.Add("Dt", dt);
            Assert.AreEqual(dt, await e.EvaluateAsync());
        }
        #endregion

        #region Time
        /// <summary>
        /// Time : is treated like a parameter
        /// Attention, it is case sensitive!
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        [DataRow(1)]
        [TestMethod]
        public async Task TimeTest(int time)
        {
            var e = new Expression("Time");
            e.Parameters.Add("Time", time);
            Assert.AreEqual(time, await e.EvaluateAsync());
        }
        #endregion

        [TestMethod]
        public void BuiltInFunctionTest()
        {
            Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await BuiltInFunction.CreateBuiltInFunction<BuiltInFunction>(string.Empty, null));
            Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await BuiltInFunction.CreateBuiltInFunction<BuiltInFunction>(null, string.Empty));
        }

        [TestMethod]
        public async Task BuiltInFunctionTest1()
        {
            var function = await BuiltInFunction.CreateBuiltInFunction<BuiltInFunction>(string.Empty, "Func");
            Assert.AreEqual("Func", function.Name);
            Assert.AreEqual(0, function.Parameters.Count);
        }

        /// <summary>
        ///     No parameter
        /// </summary>
        [TestMethod]
        public async Task BuiltInFunctionTest2()
        {
            var function = await BuiltInFunction.CreateBuiltInFunction<BuiltInFunction>(string.Empty, "Func()");
            Assert.AreEqual("Func", function.Name);
            Assert.AreEqual(0, function.Parameters.Count);
        }

        /// <summary>
        ///     One literal parameter
        /// </summary>
        [TestMethod]
        public async Task BuiltInFunctionTest3()
        {
            var function = await BuiltInFunction.CreateBuiltInFunction<BuiltInFunction>(string.Empty, "Func(param1)");
            Assert.AreEqual("Func", function.Name);
            Assert.AreEqual(1, function.Parameters.Count);
            Assert.AreEqual("_Param1", function.Parameters[0].Variables.First());
        }

        /// <summary>
        ///     Two literal parameters
        /// </summary>
        [TestMethod]
        public async Task BuiltInFunctionTest4()
        {
            var function = await BuiltInFunction.CreateBuiltInFunction<BuiltInFunction>(string.Empty, "Func(param1, param2)");
            Assert.AreEqual("Func", function.Name);
            Assert.AreEqual(2, function.Parameters.Count);
            Assert.AreEqual("_Param1", function.Parameters[0].Variables.First());
            Assert.AreEqual("_Param2", function.Parameters[1].Variables.First());
        }

        /// <summary>
        ///     Mix parameters
        /// </summary>
        [TestMethod]
        public async Task BuiltInFunctionTest5()
        {
            var function = await BuiltInFunction.CreateBuiltInFunction<BuiltInFunction>(string.Empty, "func(param1,5)");
            Assert.AreEqual("Func", function.Name);
            Assert.AreEqual(2, function.Parameters.Count);
            Assert.AreEqual("_Param1", function.Parameters[0].Variables.First());
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
        public async Task EvaluateTest()
        {
            var function = await BuiltInFunction.CreateBuiltInFunction<BuiltInFunction>(string.Empty, "abs(-5)");
            await function.Prepare(null, _machine.Models.GetVariables(), _machine.Simulation);
            Assert.AreEqual(5F, await function.Evaluate(null, _machine.Models.GetVariables(), _machine.Simulation));
        }

        /// <summary>
        ///     Prepare with a literal parameter
        /// </summary>
        [TestMethod]
        public async Task EvaluateTest1()
        {
            var function = await BuiltInFunction.CreateBuiltInFunction<BuiltInFunction>(string.Empty, "abs(aux1)");
            await Variable.CreateInstance<Auxiliary>("Aux1", _machine.Models.RootModel, "1");
            await function.Prepare(null, _machine.Models.GetVariables(), _machine.Simulation);
            Assert.AreEqual(1F, function.Expression.Parameters["_Aux1"]);
            Assert.AreEqual(1F, await function.Evaluate(null, _machine.Models.GetVariables(), _machine.Simulation));
        }

        #endregion

        #region Replace

        [TestMethod]
        public async Task ReplaceTest()
        {
            var function = await BuiltInFunction.CreateBuiltInFunction<BuiltInFunction>(string.Empty, "abs(param1)");
            await function.Replace("_Param1", "1", _machine.Simulation);
            var tryReplace = await function.TryReplace(_machine.Simulation);
            Assert.IsTrue(tryReplace.Success);
            Assert.AreEqual(1, tryReplace.Value);
        }

        [TestMethod]
        public async Task ReplaceTest1()
        {
            var function = await BuiltInFunction.CreateBuiltInFunction<BuiltInFunction>(string.Empty, "Min(param1, param2)");
            await function.Replace("_Param1", "1", _machine.Simulation);
            await function.Replace("_Param2", "2", _machine.Simulation);
            var tryReplace = await function.TryReplace(_machine.Simulation);
            Assert.IsTrue(tryReplace.Success);
            Assert.AreEqual(1, tryReplace.Value);
        }

        [TestMethod]
        public async Task ReplaceTest2()
        {
            var function = await BuiltInFunction.CreateBuiltInFunction<BuiltInFunction>(string.Empty, "Min(abs(param1), param1+param2)");
            await function.Replace("_Param1", "1", _machine.Simulation);
            await function.Replace("_Param2", "2", _machine.Simulation);
            var tryReplace = await function.TryReplace(_machine.Simulation);
            Assert.IsTrue(tryReplace.Success);
            Assert.AreEqual(1, tryReplace.Value);
        }

        #endregion
    }
}