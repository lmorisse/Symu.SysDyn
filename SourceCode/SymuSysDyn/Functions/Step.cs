#region Licence

// Description: SymuBiz - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using Symu.SysDyn.Engine;
using Symu.SysDyn.Models;
using Symu.SysDyn.Models.XMile;

#endregion

namespace Symu.SysDyn.Functions
{
    /// <summary>
    ///     STEP built in function
    ///     STEP: Generate a step increase(or decrease) at the given time
    ///     Parameters: 2: (height, start time); step up/ down at start time
    /// </summary>
    /// <example>STEP(6, 3) steps from 0 to 6 at time 3(and stays there)</example>
    public class Step : BuiltInFunction
    {
        public const string Value = "Step";

        public Step(string model, string function) : base(model, function)
        {
        }

        public string Height => GetParam(0);
        public string StartTime => GetParam(1);

        public override IBuiltInFunction Clone()
        {
            var clone = new Step(Model, OriginalFunction);
            CopyTo(clone);
            return clone;
        }

        public override float Evaluate(IVariable selfVariable, VariableCollection variables, SimSpecs sim)
        {
            if (sim == null)
            {
                throw new ArgumentNullException(nameof(sim));
            }

            //Height can be either a literal or a numeric
            var height = GetValue(0, selfVariable, variables, sim);

            var startTime = Convert.ToUInt16(GetValue(1, selfVariable, variables, sim));

            return sim.Time >= startTime ? height : 0;
        }

        public override bool TryReplace(SimSpecs sim, out float result)
        {
            result = 0;
            return false;
        }
    }
}