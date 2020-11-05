#region Licence

// Description: SymuSysDyn - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using Symu.SysDyn.Engine;
using Symu.SysDyn.Models;

#endregion

namespace Symu.SysDyn.Functions
{
    /// <summary>
    ///     Normal built in function
    ///     Sample a value from a Normal distribution
    ///     Parameters: 2 or 3: (mean, standard deviation[, seed]);
    ///     Range [0;  232[
    ///     If seed is provided, the sequence of numbers will always be identical
    /// </summary>
    /// <example>NORMAL(100, 5) samples from N(100, 5)</example>
    public class Normal : BuiltInFunction
    {
        public const string Value = "Normal";

        public Normal(string model, string function) : base(model, function)
        {
        }

        public string Mean => GetParam(0);

        public string StandardDeviation => GetParam(1);
        public string Seed => Parameters.Count == 3 ? GetParam(2) : string.Empty;

        public override IBuiltInFunction Clone()
        {
            var clone = new Normal(Model, OriginalFunction);
            CopyTo(clone);
            return clone;
        }

        public override float Evaluate(IVariable selfVariable, VariableCollection variables, SimSpecs sim)
        {
            var mean = GetValue(0, selfVariable, variables, sim);

            var standardDeviation = GetValue(1, selfVariable, variables, sim);

            if (string.IsNullOrEmpty(Seed))
            {
                return Common.Math.ProbabilityDistributions.Normal.Sample(mean, standardDeviation);
            }

            // with seed parameter
            var seed = Convert.ToInt32(GetValue(2, selfVariable, variables, sim));

            return Common.Math.ProbabilityDistributions.Normal.Sample(mean, standardDeviation, seed);
        }

        public override float InitialValue(SimSpecs sim)
        {
            return Args.Count == 2
                ? Common.Math.ProbabilityDistributions.Normal.Sample(Args[0], Args[1])
                : Common.Math.ProbabilityDistributions.Normal.Sample(Args[0], Args[1], Convert.ToInt32(Args[2]));
        }
    }
}