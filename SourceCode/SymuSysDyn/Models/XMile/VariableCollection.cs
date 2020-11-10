#region Licence

// Description: SymuBiz - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace Symu.SysDyn.Models.XMile
{
    /// <summary>
    ///     List of the variables of the model
    /// </summary>
    public class VariableCollection : IEnumerable<IVariable>
    {
        private readonly List<IVariable> _variables = new List<IVariable>();
        public IEnumerable<IVariable> GetNotUpdated => _variables.Where(x => !x.Updated && !(x is Module));
        public IEnumerable<IVariable> GetUpdated => _variables.Where(x => x.Updated);

        public IEnumerable<string> FullNames => _variables.Select(x => x.FullName);
        public IEnumerable<string> Names => _variables.Select(x => x.Name);

        public IEnumerable<string> Inputs =>
            _variables.Where(x => x.Access == VariableAccess.Input).Select(x => x.FullName);

        public IEnumerable<string> Outputs =>
            _variables.Where(x => x.Access == VariableAccess.Output).Select(x => x.FullName);

        /// <summary>
        ///     Get the list of the stocks
        /// </summary>
        public IEnumerable<Stock> Stocks => _variables.OfType<Stock>();

        /// <summary>
        ///     Get the ist of the modules
        /// </summary>
        public IEnumerable<Module> Modules => _variables.OfType<Module>();

        /// <summary>
        ///     Gets or sets the node with the specified fullName
        /// </summary>
        /// <param name="fullName"></param>
        /// <returns></returns>
        public IVariable this[string fullName]
        {
            get => Get(fullName);
            set
            {
                var index = _variables.FindIndex(x => x.Equals(fullName));
                _variables[index] = value;
            }
        }

        /// <summary>
        ///     Gets or sets the node with the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public IVariable this[int index] => _variables[index];

        public void Add(IVariable variable)
        {
            if (!Contains(variable))
            {
                _variables.Add(variable);
            }
        }

        public void AddRange(IEnumerable<IVariable> variables)
        {
            _variables.AddRange(variables);
        }

        public bool Contains(IVariable variable)
        {
            return _variables.Contains(variable);
        }

        public bool Exists(string fullName)
        {
            return _variables.Exists(x => x.Equals(fullName));
        }

        public bool ExistInput(string name)
        {
            return _variables.Exists(x => x.Name == name && x.Access == VariableAccess.Input);
        }
        public IVariable GetInput(string name)
        {
            return _variables.Find(x => x.Name == name && x.Access == VariableAccess.Input);
        }

        public IVariable Get(string fullName)
        {
            return _variables.Find(x => x.Equals(fullName));
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
                variable.Initialize();
            }
        }

        public void Remove(string fullName)
        {
            _variables.RemoveAll(x => x.Equals(fullName));
        }

        public void Clear()
        {
            _variables.Clear();
        }

        public VariableCollection Clone()
        {
            var clone = new VariableCollection();
            foreach (var variable in _variables)
            {
                clone.Add(variable.Clone());
            }

            return clone;
        }

        #region IEnumerator members

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<IVariable> GetEnumerator()
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