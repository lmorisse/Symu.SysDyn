#region Licence

// Description: SymuSysDyn - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using QuickGraph;

#endregion

namespace Symu.SysDyn.QuickGraph
{
    /// <summary>
    ///     Flow Represents the flow of the system being analyzed.
    ///     Flow is defined as an edge of stocks
    /// </summary>
    public sealed class VariableEdge : IEdge<string>
    {
        public VariableEdge(string source, string target)
        {
            Source = source;
            Target = target;
        }

        #region IEdge<string> Members

        /// <summary>
        ///     Source stock is the outflow property of a stock
        /// </summary>
        public string Source { get; }

        /// <summary>
        ///     Target stock is the inflow property of a stock
        /// </summary>
        public string Target { get; }

        #endregion

        public override string ToString()
        {
            return string.Concat(Source, " -> ", Target);
        }
    }
}