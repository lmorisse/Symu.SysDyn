#region Licence

// Description: SymuBiz - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System.Collections.Generic;
using NCalc2;
using Symu.SysDyn.Core.Engine;
using Symu.SysDyn.Core.Models.XMile;

#endregion

namespace Symu.SysDyn.Core.Equations
{
    /// <summary>
    ///     Interface for equation
    /// </summary>
    public interface IEquation
    {
        Expression Expression { get; set; }
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