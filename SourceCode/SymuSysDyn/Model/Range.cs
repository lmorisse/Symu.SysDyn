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
        private readonly bool _ranged;
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
                _ranged = true;
            }
            else
            {
                Min = 0;
                Max = 1;
                _ranged = false;
            }
            Check();
        }
        /// <summary>
        /// Constructor based on float values
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public Range(float min, float max)
        {
            Min = min;
            Max = max;
            _ranged = true;
            Check();
        }
        private void Check()
        {
            if (Min > Max)
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        public float GetOutputInsideRange(string input)
        {
            var floatInput = float.Parse(input, CultureInfo.InvariantCulture);
            return GetOutputInsideRange(floatInput);
        }

        public float GetOutputInsideRange(float input)
        {
            if (!_ranged)
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