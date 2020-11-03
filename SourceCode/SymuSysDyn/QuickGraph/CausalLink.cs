#region Licence

// Description: SymuSysDyn - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using Symu.SysDyn.Models;

#endregion

namespace Symu.SysDyn.QuickGraph
{
    /// <summary>
    ///     Specific implementation of VariableEdge
    /// </summary>
    public sealed class CausalLink : VariableEdge
    {
        public CausalLink(IVariable source, IVariable target) : base(source, target)
        {
        }
    }
}