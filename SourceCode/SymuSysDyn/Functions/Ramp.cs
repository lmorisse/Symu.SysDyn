#region Licence

// Description: SymuSysDyn - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Globalization;
using System.Linq;
using Symu.SysDyn.Model;
using Symu.SysDyn.Simulation;

#endregion

namespace Symu.SysDyn.Functions
{
    /// <summary>
    ///     Ramp built in function
    ///     Generates a ramp of slope slope, starting at time time and zero before that time.
    ///     ramp(time,slope)
    ///     Arguments: time at which to start ramping, slope (positive or negative) of ramp
    /// </summary>
    /// <example>ramp(20,-7) will have a return value of 0 at time 20 and -70 at time 30</example>
    public class Ramp : BuiltInFunction
    {
        public const string Value = "Ramp";

        public Ramp(string function) : base(function)
        {
        }

        public string Time => GetParam(0);
        public string Slope => GetParam(1);

        public override float Evaluate(Variables variables, SimSpecs sim)
        {
            if (sim == null)
            {
                throw new ArgumentNullException(nameof(sim));
            }
            // can be either a literal or a numeric
            var time = Convert.ToUInt16(GetValue(0,variables, sim));

            var slope = GetValue(1,variables, sim);

            return sim.Time >= time ? slope*(sim.Time - time) : 0;
        }
    }
}