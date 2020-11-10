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

#endregion

namespace Symu.SysDyn.Models.XMile
{
    /// <summary>
    ///     List of all the models of the system :
    ///     The root model and subModels
    /// </summary>
    public class ModelCollection : IEnumerable<XMileModel>
    {
        private readonly List<XMileModel> _models = new List<XMileModel>();

        public ModelCollection()
        {
            var rootModel = new XMileModel(null);
            _models.Add(rootModel);
        }

        /// <summary>
        ///     The root model of the system
        ///     Initialized with the modelCollection
        /// </summary>
        public XMileModel RootModel => _models[0];


        /// <summary>
        ///     Gets or sets the node with the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public XMileModel this[int index]
        {
            get => _models[index];
            set => _models[index] = value;
        }

        /// <summary>
        ///     Gets or sets the node with the specified name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public XMileModel this[string name] => Get(name);

        public void Add(XMileModel model)
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
        ///     Get a model via its name
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public XMileModel Get(string model)
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
        public IEnumerator<XMileModel> GetEnumerator()
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