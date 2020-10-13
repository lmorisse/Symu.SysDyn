#region Licence

// Description: SymuSysDyn - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using QuickGraph;
using QuickGraph.Graphviz;
using Symu.SysDyn.Model;

#endregion

namespace Symu.SysDyn.QuickGraph
{
    /// <summary>
    /// Use to generate dotString a format used by GraphViz to generate graphics
    /// GraphViz is implemented in SymuSysDynApp
    /// </summary>
    public static class GraphVizDot
    {
        public static string GenerateDotString(Graph graph)
        {
            var viz = new GraphvizAlgorithm<string, VariableEdge>(graph);

            viz.FormatVertex += VizFormatVertex;
            // TODO no edge viz.FormatEdge += edgeFormatter; param FormatEdgeAction<Node, FlowEdge> edgeFormatter

            return viz.Generate(new DotPrinter(), string.Empty);
        }

        private static void NoOpEdgeFormatter<TVertex, TEdge>(object sender, FormatEdgeEventArgs<TVertex, TEdge> e)
            where TEdge : IEdge<TVertex>
        {
            // noop
        }

        private static string ToDotNotation<TVertex, TEdge>(this IVertexAndEdgeListGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            var viz = new GraphvizAlgorithm<TVertex, TEdge>(graph);
            viz.FormatVertex += VizFormatVertex;
            return viz.Generate(new DotPrinter(), "");
        }

        private static void VizFormatVertex<TVertex>(object sender, FormatVertexEventArgs<TVertex> e)
        {
            e.VertexFormatter.Label = e.Vertex.ToString();
        }
    }
}