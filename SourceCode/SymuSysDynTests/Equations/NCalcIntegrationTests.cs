#region Licence

// Description: SymuSysDyn - SymuSysDynTests
// Website: https://symu.org
// Copyright: (c) 2021 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NCalcAsync;
using Symu.SysDyn.Core.Equations;

#endregion

namespace Symu.SysDyn.Tests.Equations
{
    [TestClass]
    public class NCalcIntegrationTests
    {
        [DataRow("Step(Height,StartTime)", 2)]
        [DataRow("Pulse(Magnitude,FirstTime, Interval)", 3)]
        [DataRow("Ramp(StartTime,Slope)", 2)]
        [DataRow("Normal(Mean,StandardDeviation)", 2)]
        [DataRow("Normal(Mean,StandardDeviation,Seed)", 3)]
        [DataRow("If(condition,expression1,expression2)", 3)]
        [DataRow("Smth1(Input, Averaging)", 2)]
        [DataRow("Smth3(Input, Averaging)", 2)]
        [DataRow("SmthN(Input, Averaging, 3)", 2)]
        [DataRow("Smth1(Input, Averaging, Initial)", 3)]
        [DataRow("Smth3(Input, Averaging, Initial)", 3)]
        [DataRow("SmthN(Input, Averaging, 3, Initial)", 3)]
        [DataRow("Abs(variable)", 1)]
        [DataRow("Min(variable1, variable2)", 2)]
        [DataRow("Pow(variable1, variable2)", 2)]
        [DataRow("Max(variable1, variable2)", 2)]
        [TestMethod]
        public void IntegrationTest(string function, int parameters)
        {
            var expression = new Expression(function);
            Assert.IsFalse(expression.HasErrors());
            var compiled = CompiledExpression.Get(function, string.Empty);
            Assert.AreEqual(parameters, compiled.Parameters.Count);
        }

        #region Dt

        /// <summary>
        ///     Dt : DeltaTime is treated like a parameter
        ///     Attention, it is case sensitive!
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
        ///     Time : is treated like a parameter
        ///     Attention, it is case sensitive!
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
    }
}