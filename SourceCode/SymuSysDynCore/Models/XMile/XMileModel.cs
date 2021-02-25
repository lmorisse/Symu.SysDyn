#region Licence

// Description: SymuSysDyn - SymuSysDynCore
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Linq;
using Symu.SysDyn.Core.Parser;

#endregion

namespace Symu.SysDyn.Core.Models.XMile
{
    /// <summary>
    ///     Define a XMile model with the list of all its variables, modules and groups
    /// </summary>
    public class XMileModel
    {
        /// <summary>
        ///     Name is mandatory for the subModels
        /// </summary>
        /// <param name="name"></param>
        public XMileModel(string name)
        {
            Name = name != null ? StringUtils.CleanName(name) : string.Empty;
        }

        /// <summary>
        ///     The name of the model
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     List of all the variables of the model
        /// </summary>
        public VariableCollection Variables { get; } = new VariableCollection();

        /// <summary>
        ///     List of all the groups of the model
        /// </summary>
        public GroupCollection Groups { get; } = new GroupCollection();

        public void Initialize()
        {
            Variables.Initialize();
        }

        /// <summary>
        ///     Add another model artefacts to the actual model
        /// </summary>
        /// <param name="model"></param>
        public void Add(XMileModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            Variables.AddRange(model.Variables);
            Groups.AddRange(model.Groups);
        }

        /// <summary>
        ///     Get the VariableCollection of a group
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public VariableCollection GetGroupVariables(string groupName)
        {
            return GetGroupVariables(Groups.Get(groupName));
        }

        /// <summary>
        ///     Get the list of variables of a group
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public VariableCollection GetGroupVariables(Group group)
        {
            if (group == null)
            {
                throw new ArgumentNullException(nameof(group));
            }

            var variables = new VariableCollection();
            foreach (var variable in group.Entities.Select(entity => Variables.Get(entity))
                .Where(variable => variable != null))
            {
                variables.Add(variable);
            }

            return variables;
        }
    }
}