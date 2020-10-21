#region Licence

// Description: SymuSysDyn - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Globalization;
using System.Text.RegularExpressions;
using Symu.SysDyn.Model;
using Symu.SysDyn.Simulation;

#endregion

namespace Symu.SysDyn.Equations
{
    /// <summary>
    ///     TIME built in function
    /// </summary>
    public class Time : BuiltInFunction
    {
        public const string Value = "Time";

        public Time(string function) : base(function)
        {
        }

        public override float Evaluate(Variables variables, SimSpecs sim)
        {
            if (sim == null)
            {
                throw new ArgumentNullException(nameof(sim));
            }

            return sim.Time;
        }

        /// <summary>
        ///     Check if it is a Time function
        /// </summary>
        /// <param name="input"></param>
        /// <param name="word"></param>
        /// <returns></returns>
        public static bool IsContainedIn(string input, out string word)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            var regex = new Regex(@"\w*(?<!_)time(?!_)", RegexOptions.IgnoreCase);
            var match = regex.Match(input);
            word = match.Value;
            return match.Success;
        }
    }
}