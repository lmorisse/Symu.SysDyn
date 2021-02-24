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
using Symu.SysDyn.Core.Models.XMile;

#endregion

namespace SymuSysDynTests.Models.XMile
{
    [TestClass]
    public class ModelCollectionTests
    {
        private readonly XMileModel _model = new XMileModel("1");
        private readonly XMileModel _model1 = new XMileModel("2");
        private readonly ModelCollection _models = new ModelCollection();
        private Variable _variable ;
        private Variable _variable1 ;

        [TestInitialize]
        public async Task Initialize()
        {
            _variable = await Variable.CreateVariable<Variable>("0", string.Empty, "STEP(1,5)");
            _variable1 = await Variable.CreateVariable<Variable>("1", string.Empty, "STEP(1,5)");
            _model.Variables.Add(_variable);
            _model1.Variables.Add(_variable1);
            _models.Add(_model);
            _models.Add(_model1);
        }

        [TestMethod]
        public void ModelCollectionTest()
        {
            Assert.AreEqual(3, _models.Count());
        }

        [TestMethod]
        public void GetVariablesTest()
        {
            var variables = _models.GetVariables();
            Assert.AreEqual(2, variables.Count());
        }
    }
}