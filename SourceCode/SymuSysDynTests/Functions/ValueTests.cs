#region Licence

// Description: SymuBiz - SymuSysDynTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.SysDyn.Core.Functions;
using SymuSysDynTests.Classes;

#endregion

namespace SymuSysDynTests.Functions
{
    [TestClass]
    public class ValueTests : BaseClassTest
    {
        [TestMethod]
        public void ValueTest()
        {
            var function = new Value(string.Empty, "Value(Id1)");
            Assert.AreEqual("_Id1", function.VariableId);
        }

        /// <summary>
        ///     Non passing test
        /// </summary>
        [TestMethod]
        public void EvaluateTest()
        {
            var function = new Value(string.Empty, "Value(Aux1)");
            function.Prepare(null, Machine.Models.GetVariables(), Machine.Simulation);
            Assert.AreEqual(1F, function.Expression.Parameters["_Aux1"]);
            Assert.AreEqual(1, function.Evaluate(null, Machine.Models.GetVariables(), Machine.Simulation));

            // Change Aux1 value
            var aux = Machine.Models[0].Variables.First(x => x.Name == "Aux1");
            aux.Value = 2;
            //Assert.AreEqual(2F, function.Expression.Parameters["_Aux1"]);
            Assert.AreEqual(1, function.Evaluate(null, Machine.Models.GetVariables(), Machine.Simulation));
        }

      
    }
}