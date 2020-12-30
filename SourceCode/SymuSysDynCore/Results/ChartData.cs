#region Licence

// Description: SymuBiz - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

namespace Symu.SysDyn.Core.Results
{
    /// <summary>
    /// Struct to store a data for the chart with Xvalue (Step) and YValue (the result of the simulation)
    /// </summary>
    public struct ChartData
    {
        public ChartData(float xValue, float yValue)
        {
            XValue = xValue;
            YValue = yValue;
        }
        public float XValue { get; set; }
        public float YValue { get; set; }
    }
}