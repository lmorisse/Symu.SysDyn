#region Licence

// Description: SymuBiz - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Symu.SysDyn.Models
{
    /// <summary>
    /// List of all the models of the system : 
    /// The root model and subModels
    /// </summary>
    public class ModelCollection : IEnumerable<Model>
    {
        private readonly List<Model> _models = new List<Model>();

        public ModelCollection()
        {
            var rootModel = new Model(null);
            _models.Add(rootModel);
        }

        /// <summary>
        /// The root model of the system
        /// Initialized with the modelCollection
        /// </summary>
        public Model RootModel => _models[0];


        /// <summary>
        ///     Gets or sets the node with the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Model this[int index] => _models[index];

        /// <summary>
        ///     Gets or sets the node with the specified name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Model this[string name] => Get(name);

        public void Add(Model model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (string.IsNullOrEmpty(model.Name))
            {
                _models[0] = model;
            }
            else
            {
                _models.Add(model);
            }
        }

        public void AddRange(ModelCollection models)
        {
            if (models == null)
            {
                throw new ArgumentNullException(nameof(models));
            }

            _models.AddRange(models._models);
        }
        public void Remove(string name)
        {
            _models.RemoveAll(x => x.Name == name);
        }
        /// <summary>
        /// Get a model via its name
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model Get(string model)
        {
            return _models.Find(x => x.Name == model);
        }

        public VariableCollection GetVariables()
        {
            var list = new VariableCollection();
            foreach (var model in _models)
            {
                list.AddRange(model.Variables);
            }
            return list;
        }

        public GroupCollection GetGroups()
        {
            var list = new GroupCollection();
            foreach (var model in _models)
            {
                list.AddRange(model.Groups);
            }
            return list;
        }

        #region IEnumerator members

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<Model> GetEnumerator()
        {
            return _models.GetEnumerator();
        }

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _models.GetEnumerator();
        }

        #endregion

    }
}