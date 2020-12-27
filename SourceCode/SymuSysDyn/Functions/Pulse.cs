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
using static Symu.Common.Constants;

#endregion

namespace Symu.SysDyn.Functions
{
    /// <summary>
    ///     Pulse built in function
    ///     Generate a one-DT wide pulse at the given time
    ///     Parameters: 2 or 3: (magnitude, first time[, interval])
    ///     Without interval or when interval = 0, the PULSE is generated only once
    /// </summary>
    /// <example>PULSE(20, 12, 5) generates a pulse value of 20/DT at time 12, 17, 22, etc</example>
    public class Pulse : BuiltInFunction
    {
        public const string Value = "Pulse";

        public Pulse(string model, string function) : base(model, function)
        {
        }

        public string Magnitude => GetParam(0);
        public string FirstTime => GetParam(1);
        public string Interval => Parameters.Count == 3 ? GetParam(2) : string.Empty;

        public override IBuiltInFunction Clone()
        {
            var clone = new Pulse(Model, OriginalFunction);
            CopyTo(clone);
            return clone;
        }

        public override float Evaluate(IVariable selfVariable, VariableCollection variables, SimSpecs sim)
        {
            if (sim == null)
            {
                throw new ArgumentNullException(nameof(sim));
            }

            var magnitude = GetValue(0, selfVariable, variables, sim);

            var firstTime = Convert.ToUInt16(GetValue(1, selfVariable, variables, sim));
            var interval = string.IsNullOrEmpty(Interval)
                ? 0
                : Convert.ToInt32(GetValue(2, selfVariable, variables, sim));
            if (interval == 0)
            {
                return Math.Abs(firstTime - sim.Step * sim.DeltaTime) < Tolerance//firstTime == sim.Time
                    ? magnitude //? magnitude * sim.DeltaTime
                    : 0;
            }

            return Math.Abs((firstTime - sim.Step * sim.DeltaTime) % interval) < Tolerance//(firstTime - sim.Time) % interval == 0
                ? magnitude //? magnitude * sim.DeltaTime
                : 0;
        }

        public override bool TryReplace(SimSpecs sim, out float result)
        {
            result = 0;
            return false;
            //if (sim == null)
            //{
            //    throw new ArgumentNullException(nameof(sim));
            //}

            ////return Evaluate(null, null, sim);
            //return Convert.ToUInt16(Args[1]) == 0
            //    ? Args[0] * sim.DeltaTime
            //    : 0;
        }
    }
}