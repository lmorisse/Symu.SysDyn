#region Licence

// Description: SymuBiz - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Symu.SysDyn.Results
{
    /// <summary>
    /// Struct to store a dictionary of CHartDataCollection where the key is the name of the variable
    /// </summary>
    public class ChartDataDictionary : IEnumerable<KeyValuePair<string, ChartDataCollection>>
    {
        private readonly ushort _start;
        private readonly ResultCollection _results;
        private readonly string[] _outputs;
        private readonly Dictionary<string, ChartDataCollection> _dictionary = new Dictionary<string, ChartDataCollection>();

        public ChartDataDictionary()
        {

        }
        public ChartDataDictionary(ushort start, ResultCollection results, IEnumerable<string> outputs)
        {
            _start = start;
            _results = results;
            _outputs = outputs.OrderBy(x => x).ToArray();
        }

        public int Count => _dictionary.Count;

        /// <summary>
        ///     Gets or sets the ChartDataCollection with the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public KeyValuePair<string, ChartDataCollection> this[int index] => _dictionary.ElementAt(index);

        public void PrepareData()
        {
            var steps = new List<float>();
            var index = _start;
            for (var i = 0; i < _results.Count; i++)
            {
                steps.Add(index);
                index++;
            }
            foreach (var output in _outputs)
            {
                Add(output, new ChartDataCollection(steps, _results.GetResults(output).ToList()));
            }
        }

        public void Add(string name, ChartDataCollection chartData)
        {
            if (_dictionary.ContainsKey(name))
            {
                _dictionary[name] = chartData;
            }
            else
            {
                _dictionary.Add(name, chartData);
            }
        }

        public List<string> Outputs => new List<string>(_dictionary.Keys);
        #region IEnumerator members

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<KeyValuePair<string, ChartDataCollection>> GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}