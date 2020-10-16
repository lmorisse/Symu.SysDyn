#region Licence

// Description: SymuSysDyn - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System.Collections;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace Symu.SysDyn.Results
{
    public class ResultCollection : IEnumerable<Result>
    {
        /// <summary>
        ///     Key => iteration
        ///     Value => Result
        /// </summary>
        private readonly Dictionary<int, Result> _result = new Dictionary<int, Result>();

        /// <summary>
        ///     Gets or sets the node with the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Result this[int index] => _result[index];

        public int Count => _result.Count;

        public void Add(Result result)
        {
            Add(_result.Count, result);
        }

        public void Add(int iteration, Result result)
        {
            _result.Add(iteration, result);
        }

        public IEnumerable<float> GetResults(string name)
        {
            return _result.Values.ToList().Select(result => result.GetValue(name)).ToList();
        }

        #region IEnumerator members

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<Result> GetEnumerator()
        {
            return _result.Values.GetEnumerator();
        }

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        public void Clear()
        {
            _result.Clear();
        }
    }
}