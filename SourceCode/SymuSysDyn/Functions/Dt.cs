#region Licence

// Description: SymuSysDyn - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using Symu.SysDyn.Model;
using Symu.SysDyn.Simulation;

#endregion

namespace Symu.SysDyn.Functions
{
    /// <summary>
    ///     DT built in function
    /// </summary>
    public class Dt : BuiltInFunction
    {
        public const string Value = "Dt";

        public Dt(string function) : base(function)
        {
        }

        public override IBuiltInFunction Clone()
        {
            var clone = new Dt(OriginalFunction);
            CopyTo(clone);
            return clone;
        }

        public override float Evaluate(IVariable selfVariable, Variables variables, SimSpecs sim)
        {
            if (sim == null)
            {
                throw new ArgumentNullException(nameof(sim));
            }

            return sim.DeltaTime;
        }
    }
}