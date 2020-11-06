#region Licence

// Description: SymuBiz - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Collections.Generic;
using Symu.SysDyn.Engine;
using Symu.SysDyn.Equations;
using Symu.SysDyn.Parser;

#endregion

namespace Symu.SysDyn.Models
{
    /// <summary>
    ///     Define a XMile module.
    ///     Modules are placeholders in the variables section, and in the stock-flow diagram, for submodels.
    ///     Module are used to support arbitrary re-use of the sub-models
    /// </summary>
    public class Module : IVariable
    {
        /// <summary>
        ///     Name is mandatory for Module
        /// </summary>
        /// <param name="name"></param>
        /// <param name="model"></param>
        public Module(string name, string model)
        {
            Name = StringUtils.CleanName(name);
            Model = model;
            FullName = StringUtils.FullName(Model, Name);
        }

        public Module(string name, ConnectCollection connects, string model) : this(name, model)
        {
            Connects = connects;
        }

        /// <summary>
        ///     List of all the connections of the module
        /// </summary>
        public ConnectCollection Connects { get; private set; } = new ConnectCollection();

        #region IVariable Members

        /// <summary>
        ///     The name of the subModel called by the module
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     Model name
        /// </summary>
        public string Model { get; }

        /// <summary>
        ///     Module and Connect are using FullName = ModelName.VariableName
        /// </summary>
        public string FullName { get; }

        public bool TryOptimize(bool setInitialValue, SimSpecs sim)
        {
            return true;
        }

        public void Initialize()
        {
            Updated = true;
        }

        public IVariable Clone()
        {
            var clone = new Module(Name, Model)
            {
                Connects = Connects
            };
            return clone;
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return FullName;
        }

        public bool Equals(string fullName)
        {
            return fullName == FullName;
        }

        #endregion

        public static Module CreateInstance(Model model, string name)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var variable = new Module(name, model.Name);
            model.Variables.Add(variable);
            return variable;
        }

        public static Module CreateInstance(Model model, string name, ConnectCollection connects)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var variable = new Module(name, connects, model.Name);
            model.Variables.Add(variable);
            return variable;
        }

        /// <summary>
        ///     Add a connect to the module
        /// </summary>
        /// <param name="connect"></param>
        public void Add(Connect connect)
        {
            Connects.Add(connect);
        }

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            return obj is Module module
                   && module.FullName == FullName;
        }

        #region IVariable members

        public float Value { get; set; }
        public IEquation Equation { get; set; }

        /// <summary>
        ///     The variable has been updated
        /// </summary>
        public bool Updated { get; set; }

        /// <summary>
        ///     The variable is being updating
        /// </summary>
        public bool Updating { get; set; }

        /// <summary>
        ///     Children are Equation's Variables except itself
        /// </summary>
        /// <remarks>Could be a computed property, but for performance, it is setted once</remarks>
        public List<string> Children { get; set; } = new List<string>();

        public GraphicalFunction GraphicalFunction { get; set; }


        public Units Units { get; set; }

        /// <summary>
        ///     Input range
        /// </summary>
        public Range Range { get; set; }

        /// <summary>
        ///     Output scale
        /// </summary>
        public Range Scale { get; set; }

        public NonNegative NonNegative { get; set; }
        public VariableAccess Access { get; set; }

        public void Update(VariableCollection variables, SimSpecs simulation)
        {
            //
        }

        #endregion
    }
}