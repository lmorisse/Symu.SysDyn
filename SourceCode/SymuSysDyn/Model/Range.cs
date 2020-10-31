#region Licence

// Description: SymuSysDyn - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

#endregion

namespace Symu.SysDyn.Model
{
    /// <summary>
    ///     Class for Range and scale with a min and a max
    ///     It is not use to compute a min and a max value, but of input and output device
    ///     It is informative for visualization
    /// </summary>
    public class Range
    {
        /// <summary>
        ///     Constructor based on float values
        /// </summary>
        public Range()
        {
        }

        /// <summary>
        ///     Constructor based on float values
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public Range(float min, float max)
        {
            Min = min;
            Max = max;
            Check();
        }

        /// <summary>
        ///     Constructor based on string scale
        /// </summary>
        /// <param name="stringScale">Optional</param>
        public Range(IReadOnlyList<string> stringScale)
        {
            if (stringScale != null && stringScale.Count == 2)
            {
                if (!string.IsNullOrEmpty(stringScale[0]))
                {
                    Min = float.Parse(stringScale[0], CultureInfo.InvariantCulture);
                }

                if (!string.IsNullOrEmpty(stringScale[1]))
                {
                    Max = float.Parse(stringScale[1], CultureInfo.InvariantCulture);
                }
            }

            Check();
        }

        /// <summary>
        ///     Constructor based on string values
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public Range(string min, string max)
        {
            if (!string.IsNullOrEmpty(min))
            {
                Min = float.Parse(min, CultureInfo.InvariantCulture);
            }

            if (!string.IsNullOrEmpty(max))
            {
                Max = float.Parse(max, CultureInfo.InvariantCulture);
            }

            Check();
        }

        public float Min { get; } = float.NegativeInfinity;

        public float Max { get; } = float.PositiveInfinity;

        private void Check()
        {
            if (Min > Max)
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        ///     Check if a serie of points is in the range
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public bool Check(float[] points)
        {
            return Min <= points.Min() && Max >= points.Max();
        }
    }
}