#region Licence

// Description: SymuBiz - SymuSysDynTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.SysDyn.Core.Equations;
using Symu.SysDyn.Core.Functions;
using SymuSysDynTests.Classes;

#endregion

namespace SymuSysDynTests.Functions
{
    [TestClass]
    public class IfThenElseTests : BaseClassTest
    {
        [TestInitialize]
        public async Task InitializeTest()
        {
            await Initialize();
        }
        [DataRow("IF x1 THEN x2 ELSE x3")]
        [DataRow("If x1 Then x2 Else x3")]
        [TestMethod]
        public async Task ParseTestOk(string function)
        {
            var ifThenElse = await IfThenElse.CreateIfThenElse(string.Empty, function);
            CheckOkTest(ifThenElse.Expression.OriginalExpression, ifThenElse.Parameters, ifThenElse.Args);
        }

        [DataRow("IF x1 ELSE x3")]
        [DataRow("IF x1 THEN x2")]
        [DataRow("")]
        [TestMethod]
        public async Task ParseTestKo()
        {
            const string function = "";
            var ifThenElse = await IfThenElse.CreateIfThenElse(string.Empty, function);
            CheckKoTest(ifThenElse.Parameters, ifThenElse.Args);
        }

        private static void CheckOkTest(string function, IReadOnlyList<IEquation> parameters, IReadOnlyList<float> args)
        {
            Assert.AreEqual(3, parameters.Count);
            Assert.AreEqual("_X1", parameters[0].Variables.First());
            Assert.AreEqual("_X2", parameters[1].Variables.First());
            Assert.AreEqual("_X3", parameters[2].Variables.First());
            Assert.AreEqual(3, args.Count);
            Assert.AreEqual(0, args[0]);
            Assert.AreEqual(0, args[1]);
            Assert.AreEqual(0, args[2]);
            Assert.AreEqual("if(_X1,_X2,_X3)", function);
        }

        private static void CheckKoTest(IReadOnlyCollection<IEquation> parameters, IReadOnlyCollection<float> args)
        {
            Assert.AreEqual(0, parameters.Count);
            Assert.AreEqual(0, args.Count);
        }

        [DataRow("IF (1) THEN (2) ELSE (3)", "if(1,2,3)")]
        [DataRow("IF(1)THEN(2)ELSE(3)", "if(1,2,3)")]
        [DataRow("IF (1+1) THEN (2+2) ELSE (3+3)", "if(2,4,6)")]
        [TestMethod]
        public async Task ParseTest(string function, string expected)
        {
            var ifThenElse = await IfThenElse.CreateIfThenElse(string.Empty, function);
            Assert.AreEqual(expected, ifThenElse.Expression.OriginalExpression);
        }

        [DataRow("IF (1+1) THEN (2+2) ELSE (3+3)", 4)]
        [DataRow("IF (1>2) THEN (2+2) ELSE (3+3)", 6)]
        [TestMethod]
        public async Task EvaluateTest(string function, float expected)
        {
            var ifThenElse = await IfThenElse.CreateIfThenElse(string.Empty, function);
            Assert.AreEqual(expected, await ifThenElse.Evaluate(null, Machine.Models.GetVariables(), Machine.Simulation));
        }

        /// <summary>
        /// nested Function with no ()
        /// </summary>
        [DataRow("If(Time < 3) then 0 else (1)", 0)]
        [DataRow("If(1) then (Pow(1,2)) else (1)", 1)]
        [DataRow("If((Time < 3) or (2==2)) then (0) else (1)", 0)]
        [DataRow("If(1==2) then (0) else (Time*10+10)", 10)]
        [TestMethod]
        public async Task EvaluateTest2(string function, float expected)
        {
            var ifThenElse = await IfThenElse.CreateIfThenElse(string.Empty, function);
            await ifThenElse.Prepare(Machine.Models.GetVariables().First(), Machine.Models.GetVariables(), Machine.Simulation);
            Assert.AreEqual(expected, await ifThenElse.Evaluate(null, Machine.Models.GetVariables(), Machine.Simulation));
        }
    }
}