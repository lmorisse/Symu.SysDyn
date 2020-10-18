#region Licence

// Description: SymuSysDyn - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using QuickGraph;
using Symu.SysDyn.Model;

#endregion

namespace Symu.SysDyn.QuickGraph
{
    /// <summary>
    ///     Flow Represents the flow of the system being analyzed.
    ///     Flow is defined as an edge of stocks
    /// </summary>
    public class VariableEdge : IEdge<Variable>
    {
        public VariableEdge(Variable source, Variable target)
        {
            Source = source;
            Target = target;
        }

        #region IEdge<Variable> Members

        /// <summary>
        ///     Source stock is the outflow property of a stock
        /// </summary>
        public Variable Source { get; }

        /// <summary>
        ///     Target stock is the inflow property of a stock
        /// </summary>
        public Variable Target { get; }

        #endregion

        public override string ToString()
        {
            return string.Concat(Source.ToString(), " -> ", Target.ToString());
        }
    }
}