#region Licence

// Description: SymuSysDyn - SymuSysDynTests
// Website: https://symu.org
// Copyright: (c) 2021 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.SysDyn.Core.Equations;
using Symu.SysDyn.Tests.Classes;

#endregion

namespace Symu.SysDyn.Tests.Equations
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
        [DataRow("if x1 then x2 else x3")]
        [TestMethod]
        public void ParseTestOk(string function)
        {
            var factory = IfThenElse.Parse(function);
            Assert.AreEqual("if(x1,x2,x3)", factory);
        }

        [DataRow("IF x1 ELSE x3")]
        [DataRow("IF x1 THEN x2")]
        [DataRow("")]
        [TestMethod]
        public void ParseTestKo(string function)
        {
            var factory = IfThenElse.Parse(function);
            Assert.IsTrue(string.IsNullOrEmpty(factory));
        }

        [DataRow("IF (1) THEN (2) ELSE (3)", 2)]
        [DataRow("IF(1)THEN(2)ELSE(3)", 2)]
        [DataRow("IF (1+1) THEN (2+2) ELSE (3+3)", 4)]
        [DataRow("IF (1>2) THEN (2+2) ELSE (3+3)", 6)]
        [DataRow("If(Time < 3) then 0 else (1)", 0)]
        [DataRow("If 1 then Pow(1,2) else 1", 1)]
        [DataRow("If((Time < 3) or (2==2)) then (0) else (1)", 0)]
        [DataRow("If(1==2) then (0) else (Time*10+10)", 10)]
        [TestMethod]
        public async Task EvaluateTest(string function, float expected)
        {
            var factory = await EquationFactory.CreateInstanceAsync(string.Empty, function, null);
            Assert.AreEqual(expected, factory.Value);


            //var ifThenElse = await IfThenElse.CreateIfThenElse(string.Empty, function);
            //await ifThenElse.Prepare(Machine.Models.GetVariables().First(), Machine.Models.GetVariables(), Machine.Simulation);
            //Assert.AreEqual(expected, await ifThenElse.Evaluate(null, Machine.Models.GetVariables(), Machine.Simulation));
        }
    }
}