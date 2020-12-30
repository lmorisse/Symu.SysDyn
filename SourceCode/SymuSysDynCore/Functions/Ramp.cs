#region Licence

// Description: SymuBiz - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using Symu.SysDyn.Core.Engine;
using Symu.SysDyn.Core.Models.XMile;

#endregion

namespace Symu.SysDyn.Core.Functions
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

        public Ramp(string model, string function) : base(model, function)
        {
        }

        public string Time => GetParam(0);
        public string Slope => GetParam(1);

        public override IBuiltInFunction Clone()
        {
            var clone = new Ramp(Model, OriginalFunction);
            CopyTo(clone);
            return clone;
        }

        public override float Evaluate(IVariable selfVariable, VariableCollection variables, SimSpecs sim)
        {
            if (sim == null)
            {
                throw new ArgumentNullException(nameof(sim));
            }

            // can be either a literal or a numeric
            var time = Convert.ToUInt16(GetValue(0, selfVariable, variables, sim));

            var slope = GetValue(1, selfVariable, variables, sim);

            return sim.Time >= time ? slope * (sim.Time - time) : 0;
        }

        public override bool TryReplace(SimSpecs sim, out float result)
        {
            result = 0;
            return false;
            //return 0;
        }
    }
}