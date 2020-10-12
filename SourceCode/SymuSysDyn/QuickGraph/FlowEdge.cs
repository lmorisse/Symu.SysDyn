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
    public sealed class FlowEdge : IEdge<Node>
    {
        public FlowEdge(string name, Node source, Node target)
        {
            Name = name;
            Source = source;
            Target = target;
        }

        public string Name { get; }

        #region IEdge<Node> Members

        /// <summary>
        ///     Source stock is the outflow property of a stock
        /// </summary>
        public Node Source { get; }

        /// <summary>
        ///     Target stock is the inflow property of a stock
        /// </summary>
        public Node Target { get; }

        #endregion

        public override string ToString()
        {
            return string.Concat(Name, ": ", Source.Name, " -> ", Target.Name);
        }

        public override bool Equals(object that)
        {
            if (that is FlowEdge flow)
            {
                return Name.Equals(flow.Name) &&
                       Source.Equals(flow.Source) &&
                       Target.Equals(flow.Target);
            }

            return ReferenceEquals(this, that);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Name != null ? Name.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (Source != null ? Source.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Target != null ? Target.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}