#region Licence

// Description: SymuBiz - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Symu.SysDyn.Core.Models.XMile;

#endregion

namespace Symu.SysDyn.Core.Results
{
    public class ResultCollection : IEnumerable<Result>
    {
        /// <summary>
        ///     Key => iteration
        ///     Value => Result
        /// </summary>
        private readonly Dictionary<int, Result> _result = new Dictionary<int, Result>();
        /// <summary>
        /// Constant results are variables with store results but with a fixed value
        /// After optimization, those variables are not stored in _result
        /// </summary>
        public Dictionary<string, float> ConstantResults { get; set; }= new Dictionary<string, float>();

        /// <summary>
        ///     Gets or sets the node with the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Result this[int index] => _result[index];

        public int Count => _result.Count;

        public void SetConstantResults(VariableCollection optimized, VariableCollection reference)
        {
            if (optimized == null || reference == null)
            {
                throw new ArgumentNullException();
            }

            ConstantResults.Clear();
            foreach (var variable in reference.Where(variable => variable.StoreResult && !optimized.Contains(variable)))
            {
                ConstantResults.Add(variable.FullName, variable.Value);
            }
        }

        public void Add(Result result)
        {
            Add(_result.Count, result);
        }

        public void Add(int iteration, Result result)
        {
            _result.Add(iteration, result);
        }
        /// <summary>
        /// Get the values for the variable name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IEnumerable<float> GetResults(string name)
        {
            return _result.Values.ToList().Select(result => result.GetValue(name)).ToList();
        }

        public static List<float> GetSteps(int start, int stepCount)
        {
            var steps = new List<float>();
            var index = start;
            for (var i = 0; i < stepCount; i++)
            {
                steps.Add(index);
                index++;
            }

            return steps;
        }

        public void Clear()
        {
            _result.Clear();
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
    }
}