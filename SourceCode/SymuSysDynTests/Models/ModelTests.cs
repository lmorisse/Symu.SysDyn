#region Licence

// Description: SymuBiz - SymuSysDynTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.SysDyn.Models;

#endregion

namespace SymuSysDynTests.Models
{
    [TestClass]
    public class ModelTests
    {
        private readonly Model _model = new Model("1");
        private readonly Variable _variable = new Variable("0", string.Empty, "STEP(1,5)");
        private Group _group;

        [TestInitialize]
        public void Initialize()
        {
            _group = new Group("0", string.Empty, new List<string> {_variable.FullName});
        }

        [TestMethod]
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