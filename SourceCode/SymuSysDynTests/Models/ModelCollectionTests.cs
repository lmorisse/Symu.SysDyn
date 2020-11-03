using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.SysDyn.Models;

namespace SymuSysDynTests.Models
{
    [TestClass()]
    public class ModelCollectionTests
    {
        private readonly Variable _variable = new Variable("0", string.Empty, "STEP(1,5)");
        private readonly Variable _variable1 = new Variable("1", string.Empty, "STEP(1,5)");
        private readonly Model _model = new Model("1");
        private readonly Model _model1 = new Model("2");
        private readonly ModelCollection _models = new ModelCollection();

        [TestInitialize]
        public void Initialize()
        {
            _model.Variables.Add(_variable);
            _model1.Variables.Add(_variable1);
            _models.Add(_model);
            _models.Add(_model1);
        }

        [TestMethod()]
        public void ModelCollectionTest()
        {
            Assert.AreEqual(3, _models.Count());
        }

        [TestMethod()]
        public void GetVariablesTest()
        {
            var variables = _models.GetVariables();
            Assert.AreEqual(2, variables.Count());
        }

    }
}