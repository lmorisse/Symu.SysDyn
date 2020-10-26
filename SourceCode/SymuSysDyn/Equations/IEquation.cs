#region Licence

// Description: SymuSysDyn - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

using System.Collections.Generic;
using Symu.SysDyn.Model;
using Symu.SysDyn.Simulation;

namespace Symu.SysDyn.Equations
{
    /// <summary>
    /// Interface for equation
    /// </summary>
    public interface IEquation
    {
        string OriginalEquation { get; }
        string InitializedEquation { get; set; }
        List<string> Variables { get; }
        float InitialValue();
        float Evaluate(Variables variables, SimSpecs sim);
        void Prepare(Variables variables, SimSpecs sim);
        void Replace(string child, string value);
        bool CanBeOptimized(string variableName);
    }
}