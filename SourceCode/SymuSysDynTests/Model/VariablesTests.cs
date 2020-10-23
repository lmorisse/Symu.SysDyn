#region Licence

// Description: SymuSysDyn - SymuSysDynTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.SysDyn.Model;
using Symu.SysDyn.Simulation;

#endregion

namespace SymuSysDynTests.Model
{
    [TestClass]
    public class VariablesTests
    {
        private readonly Variable _variable = new Variable("0", "STEP(1,5)");
        private readonly Variable _variable1 = new Variable("1", "STEP(1,5)");
        private readonly Variables _variables = new Variables();
        private Group _group;
        private List<Variable> _variableList;

        [TestInitialize]
        public void Initialize()
        {
            _variableList = new List<Variable> {_variable, _variable1};
            _group = new Group("0", new List<string> {_variable.Name});
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
                variable.Update(new Variables(), new SimSpecs());
            }

            _variables.AddRange(_variableList);
            _variables.Initialize();
            foreach (var variable in _variables)
            {
                Assert.IsFalse(variable.Updated);
            }
        }

        [TestMethod]
        public void GetNotUpdatedTest()
        {
            foreach (var variable in _variableList)
            {
                variable.Update(new Variables(), new SimSpecs());
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

        /// <summary>
        ///     Passing test
        /// </summary>
        [TestMethod]
        public void GetValueTest()
        {
            _variables.AddRange(_variableList);
            Assert.AreEqual(_variable.Value, _variables.GetValue(_variable.Name));
        }

        /// <summary>
        ///     Non passing tests
        /// </summary>
        [TestMethod]
        public void GetValueTest1()
        {
            Assert.ThrowsException<ArgumentNullException>(() => _variables.GetValue(null));
            Assert.ThrowsException<ArgumentNullException>(() => _variables.GetValue(string.Empty));
            Assert.ThrowsException<NullReferenceException>(() => _variables.GetValue("noVariableName"));
        }

        [TestMethod]
        public void SetValueTest()
        {
            _variables.Add(_variable);
            _variables.SetValue(_variable.Name, 2);
            Assert.AreEqual(2, _variable.Value);
        }

        [TestMethod]
        public void SetValueTest1()
        {
            Assert.ThrowsException<ArgumentNullException>(() => _variables.SetValue(null, 0));
            Assert.ThrowsException<ArgumentNullException>(() => _variables.SetValue(string.Empty, 0));
            Assert.ThrowsException<NullReferenceException>(() => _variables.SetValue("noVariableName", 0));
        }

        [TestMethod]
        public void GetGroupVariablesTest()
        {
            var groupVariables = _variables.GetGroupVariables(_group);
            Assert.AreEqual(1, groupVariables.Count());
        }
    }
}