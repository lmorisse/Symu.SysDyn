#region Licence

// Description: SymuSysDyn - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Collections.Generic;
using System.Linq;
using Symu.SysDyn.Parser;

#endregion

namespace Symu.SysDyn.Models
{
    /// <summary>
    ///     Groups, aka sectors, collect related model structure together
    ///     Different from Module
    /// </summary>
    public class Group
    {
        public Group(string name, string model, IEnumerable<string> entities)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            Name = StringUtils.CleanName(name);
            foreach (var entity in entities.ToList())
            {
                Entities.Add(StringUtils.FullName(model,StringUtils.CleanName(entity)));
            }
        }


        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return Name;
        }

        #region Xml attributes

        public string Name { get; }

        /// <summary>
        ///     The group includes the names of all entities in that group using the entity tag with a name attribute.
        /// </summary>
        public List<string> Entities { get; } = new List<string>();

        #endregion
    }
}