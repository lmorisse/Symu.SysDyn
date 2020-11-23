using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Symu.SysDyn.Models.XMile;
using Symu.SysDyn.Parser;

namespace Symu.SysDyn.Models.Symu
{
    /// <summary>
    /// Graph of all the models and their outcomes of the organization
    /// </summary>
    public class ModelNetwork : IEnumerable<IEntity>
    {
        private readonly List<IEntity> _network = new List<IEntity>();
        public Model RootModel { get; }  = new Model(string.Empty);

        /// <summary>
        ///     Gets or sets the node with the specified fullName
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IEntity this[string name]
        {
            get => Get(name);
            set
            {
                var index = _network.FindIndex(x => x.Name==name);
                _network[index] = value;
            }
        }

        /// <summary>
        ///     Gets or sets the node with the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public IEntity this[int index] => _network[index];

        public void Add(IEntity entity)
        {
            _network.Add(entity);
        }

        public IEntity Get(string name)
        {
            return _network.Find(x => x.Name == name);
        }

        public ModelNetwork Clone()
        {
            var clone = new ModelNetwork();
            CopyTo(clone);
            return clone;
        }

        public virtual void CopyTo(ModelNetwork clone)
        {
            if (clone == null)
            {
                throw new ArgumentNullException(nameof(clone));
            }

            foreach (var entity in _network)
            {
                clone._network.Add(entity.Clone());
            }
        }

        /// <summary>
        /// Aggregate all the _models to process the entire model
        /// Each entity of the GraphMetaNetwork is a model, with its sub/inline models
        /// </summary>
        public ModelCollection GlobalModel()
        {
            var models = new ModelCollection {[0] = RootModel};
            // don't call entity.Model (via a Select(x => x.Model) directly to be sure to call BaseModelEntity.GetModelCollection
            foreach (var entity in _network)
            {
                models.Add(entity.GetModelGraph());
            }

            CreateConnects(models);
            return models;
        }
        /// <summary>
        /// For Model with subModels, we need to create the module and the ConnectCollection before trying to resolveConnects
        /// </summary>
        /// <remarks>XMileModel are not concerned by this method, Module are already created in XML file</remarks>
        public void CreateConnects(ModelCollection models)
        {
            // except RootModel
            foreach (var model in models.OfType<Model>().Where(x => !string.IsNullOrEmpty(x.Name)))
            {
                // Root connects
                var module = Module.CreateInstance(RootModel, model.Name);
                // Connect Outputs
                foreach (var fullNameOutput in RootModel.Variables.Outputs)
                {
                    var output = fullNameOutput.Remove(0, 1);
                    if (!model.Variables.ExistInput(output))
                    {
                        continue;
                    }

                    var variable = model.Variables.GetInput(output);
                    var to = StringUtils.ConnectName(variable.Model, output);
                    var from = StringUtils.ConnectName(RootModel.Name, output);
                    module.Add(new Connect(model.Name, to, @from));
                }
                // Entities connects
                CreateConnects(model);
            }
        }
        /// <summary>
        /// For Model with subModels, we need to create the module and the ConnectCollection before trying to resolveConnects
        /// Only the module are already created
        /// </summary>
        private static void CreateConnects(XMileModel model)
        {
            foreach (var module in model.Variables.Modules)
            {
                // Connect Inputs
                foreach (var input in model.Variables.Inputs)
                {
                    var variable = model.Variables.Get(input);
                    var name = variable.Name;
                    var output = model.Name + "_" + name;
                    if (model.Variables.Outputs.Contains(output))
                    {
                        module.Add(new Connect(module.Name, name, StringUtils.ConnectName(model.Name,name)));
                    }
                }
            }
        }

        /// <summary>
        /// Process only a subModel
        /// </summary>
        /// <param name="model">The subModel to process</param>
        public XMileModel LocalModel(Model model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return model.GetModelCollection(null).First();
        }
        #region IEnumerator members

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<IEntity> GetEnumerator()
        {
            return _network.GetEnumerator();
        }

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _network.GetEnumerator();
        }

        #endregion
    }
}
