#region Licence

// Description: SymuSysDyn - SymuSysDynTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.SysDyn.Engine;
using Symu.SysDyn.Models;

#endregion

namespace SymuSysDynTests.Models
{
    [TestClass]
    public class VariablesTests
    {
        private readonly Variable _variable = new Variable("0", string.Empty,"STEP(1,5)");
        private readonly Variable _variable1 = new Variable("1", string.Empty, "STEP(1,5)");
        private readonly Model _model = new Model("1");
        private VariableCollection Variables => _model.Variables;
        private List<Variable> _variableList;

        [TestInitialize]
        public void Initialize()
        {
            _variableList = new List<Variable> {_variable, _variable1};
        }

        [TestMethod]
        public void AddTest()
        {
            Assert.IsFalse(Variables.Contains(_variable));
            Variables.Add(_variable);
            Assert.IsTrue(Variables.Contains(_variable));
        }

        [TestMethod]
        public void AddRangeTest()
        {
            Variables.AddRange(_variableList);
            Assert.IsTrue(Variables.Contains(_variable));
            Assert.IsTrue(Variables.Contains(_variable1));
        }

        [TestMethod]
        public void GetTest()
        {
            Assert.IsNull(Variables.Get(_variable.FullName));
            Variables.Add(_variable);
            Assert.AreEqual(_variable, Variables.Get(_variable.FullName));
        }

        [TestMethod]
        public void InitializeTest()
        {
            foreach (var variable in _variableList)
            {
                variable.Update(new VariableCollection(), new SimSpecs());
            }

            Variables.AddRange(_variableList);
            Variables.Initialize();
            foreach (var variable in Variables)
            {
                Assert.IsFalse(variable.Updated);
            }
        }

        [TestMethod]
        public void GetNotUpdatedTest()
        {
            foreach (var variable in _variableList)
            {
                variable.Update(new VariableCollection(), new SimSpecs());
            }

            Variables.AddRange(_variableList);
            Assert.AreEqual(0, Variables.GetNotUpdated.Count());
        }

        [TestMethod]
        public void GetNotUpdatedTest1()
        {
            Variables.AddRange(_variableList);
            Assert.AreEqual(2, Variables.GetNotUpdated.Count());
        }

        [TestMethod]
        public void NamesTest()
        {
            Variables.AddRange(_variableList);
            Assert.AreEqual(2, Variables.FullNames.Count());
            Assert.AreEqual(_variable.FullName, Variables.FullNames.ElementAt(0));
            Assert.AreEqual(_variable1.FullName, Variables.FullNames.ElementAt(1));
        }

        /// <summary>
        ///     Passing test
        /// </summary>
        [TestMethod]
        public void GetValueTest()
        {
            Variables.AddRange(_variableList);
            Assert.AreEqual(_variable.Value, Variables.GetValue(_variable.FullName));
        }

        /// <summary>
        ///     Non passing tests
        /// </summary>
        [TestMethod]
        public void GetValueTest1()
        {
            Assert.ThrowsException<ArgumentNullException>(() => Variables.GetValue(null));
            Assert.ThrowsException<ArgumentNullException>(() => Variables.GetValue(string.Empty));
            Assert.ThrowsException<NullReferenceException>(() => Variables.GetValue("noVariableName"));
        }

        [TestMethod]
        public void SetValueTest()
        {
            Variables.Add(_variable);
            Variables.SetValue(_variable.FullName, 2);
            Assert.AreEqual(2, _variable.Value);
        }

        [TestMethod]
        public void SetValueTest1()
        {
            Assert.ThrowsException<ArgumentNullException>(() => Variables.SetValue(null, 0));
            Assert.ThrowsException<ArgumentNullException>(() => Variables.SetValue(string.Empty, 0));
            Assert.ThrowsException<NullReferenceException>(() => Variables.SetValue("noVariableName", 0));
        }


    }
}