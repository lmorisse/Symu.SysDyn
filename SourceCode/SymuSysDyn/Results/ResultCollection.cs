#region Licence

// Description: SymuSysDyn - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Symu.SysDyn.Results
{
    public class ResultCollection
    {
        /// <summary>
        /// Key => iteration
        /// Value => Result
        /// </summary>
        private readonly Dictionary<int, Result> _result = new Dictionary<int, Result>();

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
    }
}