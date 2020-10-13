#region Licence

// Description: SymuSysDyn - SymuSysDynTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.SysDyn.Model;

#endregion

namespace SymuSysDynTests.Model
{
    [TestClass]
    public class VariablesTests
    {
        private readonly Variable _variable = new Variable("0");
        private readonly Variable _variable1 = new Variable("1");
        private readonly Variables _variables = new Variables();
        private List<Variable> _variableList;

        [TestInitialize]
        public void Initialize()
        {
            _variableList = new List<Variable> {_variable, _variable1};
        }

        [TestMethod]
        public void AddTest()
        {
            Assert.IsFalse(_variables.Contains(_variable));
            _variables.Add(_variable);
            Assert.IsTrue(_variables.Contains(_variable));
        }

        [TestMethod]
        public void AddRangeTest()
        {
            _variables.AddRange(_variableList);
            Assert.IsTrue(_variables.Contains(_variable));
            Assert.IsTrue(_variables.Contains(_variable1));
        }

        [TestMethod]
        public void GetTest()
        {
            Assert.IsNull(_variables.Get(_variable.Name));
            _variables.Add(_variable);
            Assert.AreEqual(_variable, _variables.Get(_variable.Name));
        }

        [TestMethod]
        public void InitializeTest()
        {
            foreach (var variable in _variableList)
            {
                variable.Value = 1;
                variable.Updated = true;
            }

            _variables.AddRange(_variableList);
            _variables.Initialize();
            foreach (var variable in _variables)
            {
                Assert.AreEqual(variable.Value, variable.OldValue);
                Assert.IsFalse(variable.Updated);
            }
        }

        [TestMethod]
        public void GetNotUpdatedTest()
        {
            foreach (var variable in _variableList)
            {
                variable.Value = 1;
                variable.Updated = true;
            }

            _variables.AddRange(_variableList);
            Assert.AreEqual(0, _variables.GetNotUpdated.Count());
        }

        [TestMethod]
        public void GetNotUpdatedTest1()
        {
            _variables.AddRange(_variableList);
            Assert.AreEqual(2, _variables.GetNotUpdated.Count());
        }

        [TestMethod]
        public void NamesTest()
        {
            _variables.AddRange(_variableList);
            Assert.AreEqual(2, _variables.Names.Count());
            Assert.AreEqual(_variable.Name, _variables.Names.ElementAt(0));
            Assert.AreEqual(_variable1.Name, _variables.Names.ElementAt(1));
        }
    }
}