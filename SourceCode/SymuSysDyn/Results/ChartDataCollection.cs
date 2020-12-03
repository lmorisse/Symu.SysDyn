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

namespace Symu.SysDyn.Results
{
    /// <summary>
    /// Store a collection of data for the chart 
    /// </summary>
    public class ChartDataCollection : IEnumerable<ChartData>
    {
        private readonly List<ChartData> _chartData = new List<ChartData>();

        public ChartDataCollection(List<float> xValues, List<float> yValues)
        {
            if (xValues == null)
            {
                throw new ArgumentNullException(nameof(xValues));
            }

            if (yValues == null)
            {
                throw new ArgumentNullException(nameof(yValues));
            }

            for (var i = 0; i < xValues.Count; i++)
            {
                _chartData.Add(new ChartData(xValues[i], yValues[i]));
            }
        }
        /// <summary>
        ///     Gets or sets the ChartDataCollection with the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ChartData this[int index] => _chartData[index];
        #region IEnumerator members

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<ChartData> GetEnumerator()
        {
            return _chartData.GetEnumerator();
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