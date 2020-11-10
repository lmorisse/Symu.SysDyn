#region Licence

// Description: SymuBiz - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System.Collections.Generic;
using Symu.SysDyn.Engine;
using Symu.SysDyn.Models;
using Symu.SysDyn.Models.XMile;

#endregion

namespace Symu.SysDyn.Equations
{
    /// <summary>
    ///     Interface for equation
    /// </summary>
    public interface IEquation
    {
        string OriginalEquation { get; }
        string InitializedEquation { get; set; }
        List<string> Variables { get; }
        float InitialValue();
        float Evaluate(IVariable variable, VariableCollection variables, SimSpecs sim);
        void Prepare(IVariable variable, VariableCollection variables, SimSpecs sim);
        void Replace(string child, string value, SimSpecs sim);
        bool CanBeOptimized(string variableName);
        IEquation Clone();
    }
}