using System.Collections.Generic;
using System.Threading.Tasks;
using Symu.SysDyn.Models.XMile;

namespace Symu.SysDyn.Models.Symu
{
    /// <summary>
    /// Model for system dynamics, to be used to hand code a model
    /// Inherited from XMileModel (to be used to load an XMile file)
    /// It is a recursive object as a model can has SubModels and inline models
    /// </summary>
    /// <seealso cref="XMileModel"/>
    /// <remarks>Output variables should be a public property, others variables maybe inlined</remarks>
    public class Model: XMileModel
    {
        /// <summary>
        /// SubModels are treated with Modules
        /// </summary>
        public List<Model> SubModels { get; } = new List<Model>();
        /// <summary>
        /// InlineModels are subModels that are inlined in the model parent
        /// because they don't need visibility as Model
        /// and don't output variables
        /// </summary>
        public List<Model> InlineModels { get; } = new List<Model>();

        public Model(string name):base(name){}
        /// <summary>
        /// Add a subModel
        /// </summary>
        /// <param name="model"></param>
        public void AddSubModel(Model model)
        {
            SubModels.Add(model);
        }
        /// <summary>
        /// Add an inlineModel
        /// </summary>
        public void AddInlineModel(Model model)
        {
            InlineModels.Add(model);
        }
        /// <summary>
        /// Get the model collection :
        ///     SubModel are transformed as Module
        ///     InlineModel are transformed as Group
        /// </summary>
        /// <param name="parentModel"></param>
        /// <returns></returns>
        //TODO change name SubModel into Module - InlineModel into Group
        public virtual ModelCollection GetModelCollection(Model parentModel)
        {
            var models = new ModelCollection();
            foreach (var subModel in SubModels)
            {
                var modelCollection = subModel.GetModelCollection(null);
                Variables.AddRange(modelCollection.GetVariables());
                Groups.AddRange(modelCollection.GetGroups());
                // Add module
                Module.CreateInstance(this, subModel.Name);
                models.AddRange(modelCollection);
            }
            foreach (var subModel in InlineModels)
            {
                var modelCollection = subModel.GetModelCollection(this);
                var variables = modelCollection.GetVariables();
                Variables.AddRange(variables);
                // Add group for each inline model
                // InlineModel.Name are the same as their parentModel, therefore we use the className
                var group = new Group(subModel.GetType().Name, Name, variables.Names);
                Groups.Add(group);
            }
            models[0] = this;
            return models;
        }

    }
}
