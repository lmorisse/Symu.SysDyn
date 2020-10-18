#region Licence

// Description: SymuSysDyn - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace Symu.SysDyn.Model
{
    /// <summary>
    ///     List of the variables of the model
    /// </summary>
    public class Variables : IEnumerable<Variable>
    {
        private readonly List<Variable> _variables = new List<Variable>();
        public Groups Groups { get; } = new Groups();
        public IEnumerable<Variable> GetNotUpdated => _variables.Where(x => !x.Updated);

        public IEnumerable<string> Names => _variables.Select(x => x.Name);
        public IEnumerable<Stock> Stocks => _variables.OfType<Stock>();

        /// <summary>
        ///     Gets or sets the node with the specified name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Variable this[string name] => Get(name);

        /// <summary>
        ///     Gets or sets the node with the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Variable this[int index] => _variables[index];

        public void Add(Variable variable)
        {
            if (!Contains(variable))
            {
                _variables.Add(variable);
            }
        }

        public void AddRange(IEnumerable<Variable> variables)
        {
            _variables.AddRange(variables);
        }

        public bool Contains(Variable variable)
        {
            return _variables.Contains(variable);
        }

        public bool Exists(string name)
        {
            return _variables.Exists(x => x.Name == name);
        }

        public Variable Get(string name)
        {
            return _variables.Find(x => x.Name == name);
        }

        /// <summary>
        ///     returns current value of a node
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public float GetValue(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (!Exists(name))
            {
                throw new NullReferenceException(nameof(name));
            }

            return Get(name).Value;
        }

        public void SetValue(string name, float value)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (!Exists(name))
            {
                throw new NullReferenceException(nameof(name));
            }

            Get(name).Value = value;
        }

        public void Initialize()
        {
            foreach (var variable in _variables)
            {
                variable.Updated = false;
                variable.OldValue = variable.Value;
            }
        }

        /// <summary>
        ///     Get the list of variables of a group
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public Variables GetGroupVariables(string groupName)
        {
            return GetGroupVariables(Groups.Get(groupName));
        }

        /// <summary>
        ///     Get the list of variables of a group
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public Variables GetGroupVariables(Group group)
        {
            if (group == null)
            {
                throw new ArgumentNullException(nameof(group));
            }

            var variables = new Variables();
            foreach (var entity in group.Entities)
            {
                variables.Add(Get(entity));
            }

            return variables;
        }

        #region IEnumerator members

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<Variable> GetEnumerator()
        {
            return _variables.GetEnumerator();
        }

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _variables.GetEnumerator();
        }

        #endregion
    }
}