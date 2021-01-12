﻿#region Licence

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
    ///     TIME built in function
    /// </summary>
    public class Time : BuiltInFunction
    {
        public const string Label = "Time";

        public Time(string model, string function) : base(model, function)
        {
        }

        public override IBuiltInFunction Clone()
        {
            var clone = new Time(Model, OriginalFunction);
            CopyTo(clone);
            return clone;
        }

        public override float Evaluate(IVariable selfVariable, VariableCollection variables, SimSpecs sim)
        {
            if (sim == null)
            {
                throw new ArgumentNullException(nameof(sim));
            }

            return sim.Time;
        }
        public override bool TryReplace(SimSpecs sim, out float result)
        {
            result = 0;
            return false;
        }
    }
}