#region Licence

// Description: SymuSysDyn - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System.Collections.Generic;
using System.Linq;
using QuickGraph;
using Symu.SysDyn.Model;

#endregion

namespace Symu.SysDyn.QuickGraph
{
    public class Graph : BidirectionalGraph<Node, FlowEdge>
    {
        private readonly Node _defaultNode;

        public Graph(bool allowParallelEdges, Node defaultNode) : base(allowParallelEdges)
        {
            _defaultNode = defaultNode;
        }

        public new IEnumerable<Node> Vertices => base.Vertices.Where(stock => !Equals(stock, _defaultNode));

        public IEnumerable<FlowEdge> InnerFlows => Edges.Where(flow => !flow.IsAdjacent(_defaultNode));

        public IEnumerable<FlowEdge> SelfFlows => InnerFlows.Where(flow => flow.IsSelfEdge<Node, FlowEdge>());

        public IEnumerable<FlowEdge> NonSelfFlows => InnerFlows.Where(flow => !flow.IsSelfEdge<Node, FlowEdge>());

        //public Graph Subgraph(string[] includedVertices)
        //{
        //    var vertices = Vertices.Where(v => includedVertices.Contains(v.Name)).ToArray();
        //    var edges = from edge in InnerFlows
        //        where vertices.Contains(edge.Source) &&
        //              vertices.Contains(edge.Target)
        //        select edge;

        //    var subgraph = new Graph(true, DefaultNode);
        //    subgraph.AddVertexRange(vertices);
        //    subgraph.AddEdgeRange(edges);

        //    return subgraph;
        //}
    }
}