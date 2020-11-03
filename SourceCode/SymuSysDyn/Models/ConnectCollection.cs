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
    /// List of all the modules of a Variable
    /// </summary>
    public class ConnectCollection : IEnumerable<Connect>
    {
        private readonly List<Connect> _connects = new List<Connect>();

        /// <summary>
        ///     Gets or sets the node with the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Connect this[int index] => _connects[index];

        public void Add(Connect connect)
        {
            _connects.Add(connect);
        }
        /// <summary>
        /// Get a connect 
        /// </summary>
        /// <param name="to"></param>
        /// <param name="from"></param>
        /// <returns></returns>
        public Connect Get(string to, string from)
        {
            return _connects.Find(x => x.Equals(to, from));
        }

        #region IEnumerator members

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<Connect> GetEnumerator()
        {
            return _connects.GetEnumerator();
        }

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _connects.GetEnumerator();
        }

        #endregion

    }
}