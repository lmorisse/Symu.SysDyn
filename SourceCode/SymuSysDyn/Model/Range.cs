#region Licence

// Description: SymuSysDyn - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Globalization;
using System.Linq;

using static Symu.Common.Constants;

#endregion

namespace Symu.SysDyn.Model
{
    /// <summary>
    /// Class for Range, scale, bounds, with a min and a max
    /// </summary>
    public readonly struct Range
    {
        /// <summary>
        /// True if the range is specified
        /// </summary>
        public bool Ranged { get; }
        public float Min { get; } 

        public float Max { get; } 
        /// <summary>
        /// Constructor based on string scale
        /// </summary>
        /// <param name="stringScale">Optional</param>
        public Range(IReadOnlyList<string> stringScale)
        {
            if (stringScale != null && stringScale.Count == 2)
            {
                Min = float.Parse(stringScale[0], CultureInfo.InvariantCulture);
                Max = float.Parse(stringScale[1], CultureInfo.InvariantCulture);
                Ranged = true;
            }
            else
            {
                Min = 0;
                Max = 1;
                Ranged = false;
            }
            Check();
        }

        /// <summary>
        /// Constructor based on string values
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public Range(string min, string max)
        {
            Min = float.Parse(min, CultureInfo.InvariantCulture);
            Max = float.Parse(max, CultureInfo.InvariantCulture);
            Ranged = true;
            Check();
        }
        /// <summary>
        /// Constructor based on float values
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="ranged"></param>
        public Range(float min, float max, bool ranged = true)
        {
            Min = min;
            Max = max;
            Ranged = ranged;
            Check();
        }
        private void Check()
        {
            if (Min > Max)
            {
                throw new ArgumentOutOfRangeException();
            }
        }
        /// <summary>
        /// Check if a serie of points is in the range
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public bool Check(float[] points)
        {
            return !Ranged || (Min <= points.Min() && Max >= points.Max());
        }

        public float GetOutputInsideRange(string input)
        {
            var floatInput = float.Parse(input, CultureInfo.InvariantCulture);
            return GetOutputInsideRange(floatInput);
        }

        public float GetOutputInsideRange(float input)
        {
            if (!Ranged)
            {
                return input;
            }
            if (input > Max)
            {
                return Max;
            }
            if (input < Min)
            {
                return Min;
            }

            return input;
        }
    }
}