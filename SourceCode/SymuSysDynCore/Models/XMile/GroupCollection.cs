#region Licence

// Description: SymuSysDyn - SymuSysDynCore
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System.Collections;
using System.Collections.Generic;

#endregion

namespace Symu.SysDyn.Core.Models.XMile
{
    /// <summary>
    ///     List of all the groups of a model
    /// </summary>
    public class GroupCollection : IEnumerable<Group>
    {
        private readonly List<Group> _groups = new List<Group>();

        /// <summary>
        ///     Gets or sets the node with the specified name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Group this[string name] => Get(name);

        /// <summary>
        ///     Gets or sets the node with the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Group this[int index] => _groups[index];

        public void Add(Group group)
        {
            if (!Contains(group))
            {
                _groups.Add(group);
            }
        }

        public void AddRange(IEnumerable<Group> groups)
        {
            _groups.AddRange(groups);
        }

        public bool Contains(Group group)
        {
            return _groups.Contains(group);
        }

        public bool Exists(string name)
        {
            return _groups.Exists(x => x.Name == name);
        }

        public Group Get(string name)
        {
            return _groups.Find(x => x.Name == name);
        }

        public void Clear()
        {
            _groups.Clear();
        }

        #region IEnumerator members

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<Group> GetEnumerator()
        {
            return _groups.GetEnumerator();
        }

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _groups.GetEnumerator();
        }

        #endregion
    }
}