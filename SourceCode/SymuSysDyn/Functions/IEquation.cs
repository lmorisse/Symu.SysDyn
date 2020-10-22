#region Licence

// Description: SymuSysDyn - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

using System.Collections.Generic;
using Symu.SysDyn.Model;
using Symu.SysDyn.Simulation;

namespace Symu.SysDyn.Functions
{
    /// <summary>
    /// Interface for equation
    /// </summary>
    public interface IEquation
    {
        string OriginalEquation { get; }
        string InitializedEquation { get; }
        List<string> Variables { get; }
        float Evaluate(Variables variables, SimSpecs sim);
        void Prepare(Variables variables, SimSpecs sim);
    }
}