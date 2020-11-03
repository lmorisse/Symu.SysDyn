using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.SysDyn.Models;

namespace SymuSysDynTests.Models
{
    [TestClass()]
    public class ModelTests
    {
        private readonly Variable _variable = new Variable("0", string.Empty, "STEP(1,5)");
        private readonly Model _model = new Model("1");
        private Group _group;

        [TestInitialize]
        public void Initialize()
        {
            _group = new Group("0", string.Empty, new List<string> { _variable.FullName});
        }
        [TestMethod()]
        public void GetVariablesTest()
        {
        }
        [TestMethod]
        public void GetGroupVariablesTest()
        {
            var groupVariables = _model.GetGroupVariables(_group);
            Assert.AreEqual(1, groupVariables.Count());
        }
    }
}