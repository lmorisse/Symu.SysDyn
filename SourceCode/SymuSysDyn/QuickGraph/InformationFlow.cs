#region Licence

// Description: SymuBiz - SymuSysDyn
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
    public sealed class InformationFlow : VariableEdge
    {
        public InformationFlow(IVariable source, IVariable target) : base(source, target)
        {
        }
    }
}