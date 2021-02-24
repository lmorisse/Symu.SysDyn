#region Licence

// Description: SymuBiz - SymuSysDynTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.SysDyn.Core.Models.XMile;

#endregion

namespace SymuSysDynTests.Models.XMile
{
    [TestClass]
    public class XMileModelTests
    {
        private readonly XMileModel _model = new XMileModel("1");
        private Variable _variable;
        private Group _group;

        [TestInitialize]
        public async Task Initialize()
        {
            _variable = await Variable.CreateVariable<Variable>("0", string.Empty, "STEP(1,5)");
            //_model.Variables.Add(_variable);
            _group = new Group("0", string.Empty, new List<string> {_variable.FullName});
        }

        [TestMethod]
        public void GetVariablesTest()
        {
            //todo
        }

        [TestMethod]
        public void GetGroupVariablesTest()
        {
            var groupVariables = _model.GetGroupVariables(_group);
            Assert.AreEqual(1, groupVariables.Count());
        }
    }
}