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
    ///     STEP built in function
    ///     STEP: Generate a step increase(or decrease) at the given time
    ///     Parameters: 2: (height, start time); step up/ down at start time
    /// </summary>
    /// <example>STEP(6, 3) steps from 0 to 6 at time 3(and stays there)</example>
    public class Step : BuiltInFunction
    {
        public const string Value = "Step";

        public Step(string function) : base(function)
        {
        }

        public string Height => Parameters[0].InitializedEquation;
        public string StartTime => Parameters[1].InitializedEquation;

        public override float Evaluate(Variables variables, SimSpecs sim)
        {
            if (sim == null)
            {
                throw new ArgumentNullException(nameof(sim));
            }
            //Height can be either a literal or a numeric
            var height = Convert.ToSingle(Parameters[0].Evaluate(variables, sim));

            var startTime = Convert.ToUInt16(Parameters[1].Evaluate(variables, sim));

            return sim.Time >= startTime ? height : 0;
        }
    }
}