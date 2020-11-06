#region Licence

// Description: SymuBiz - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;

#endregion

namespace Symu.SysDyn.Models
{
    /// <summary>
    ///     Prevents the stock from going negative
    /// </summary>
    public class NonNegative
    {
        /// <summary>
        ///     Constructor based on float values
        /// </summary>
        public NonNegative(bool nonNegative)
        {
            if (nonNegative)
            {
                // Value must be positive
                Min = Math.Max(Min, 0);
            }
        }

        public float Min { get; } = float.NegativeInfinity;

        public float GetOutputInsideRange(float input)
        {
            return input < Min ? Min : input;
        }
    }
}