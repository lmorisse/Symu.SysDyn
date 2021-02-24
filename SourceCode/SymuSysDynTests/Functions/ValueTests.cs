#region Licence

// Description: SymuBiz - SymuSysDynTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.SysDyn.Core.Functions;
using SymuSysDynTests.Classes;

#endregion

namespace SymuSysDynTests.Functions
{
    [TestClass]
    public class ValueTests : BaseClassTest
    {
        [TestInitialize]
        public async Task InitializeTest()
        {
            await Initialize();
        }
        [TestMethod]
        public async Task ValueTest()
        {
            var function = await Value.CreateValue(string.Empty, "Value(Id1)");
            Assert.AreEqual("_Id1", function.VariableId);
        }

        /// <summary>
        ///     Non passing test
        /// </summary>
        [TestMethod]
        public async Task EvaluateTest()
        {
            var function = await Value.CreateValue(string.Empty, "Value(Aux1)");
            await function.Prepare(null, Machine.Models.GetVariables(), Machine.Simulation);
            Assert.AreEqual(1F, function.Expression.Parameters["_Aux1"]);
            Assert.AreEqual(1, await function.Evaluate(null, Machine.Models.GetVariables(), Machine.Simulation));

            // Change Aux1 value
            var aux = Machine.Models[0].Variables.First(x => x.Name == "Aux1");
            aux.Value = 2;
            //Assert.AreEqual(2F, function.Expression.Parameters["_Aux1"]);
            Assert.AreEqual(1, await function.Evaluate(null, Machine.Models.GetVariables(), Machine.Simulation));
        }

      
    }
}