#region Licence

// Description: SymuSysDyn - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using QuickGraph;
using QuickGraph.Graphviz;
using QuickGraph.Graphviz.Dot;
using Symu.SysDyn.Model;

#endregion

namespace Symu.SysDyn.QuickGraph
{
    /// <summary>
    ///     Use to generate dotString a format used by GraphViz to generate graphics
    ///     GraphViz is implemented in SymuSysDynApp
    /// </summary>
    public static class GraphVizDot
    {
        public static string GenerateDotString(Graph graph)
        {
            var viz = new GraphvizAlgorithm<Variable, VariableEdge>(graph);

            viz.FormatVertex += FormatVertex;
            viz.FormatEdge += FormatEdge;

            return viz.Generate(new DotPrinter(), string.Empty);
        }

        private static void NoOpEdgeFormatter<TVertex, TEdge>(object sender, FormatEdgeEventArgs<TVertex, TEdge> e)
            where TEdge : IEdge<TVertex>
        {
            // noop
        }

        private static string ToDotNotation<TVertex, TEdge>(this IEdgeListGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            var viz = new GraphvizAlgorithm<TVertex, TEdge>(graph);
            viz.FormatVertex += FormatVertex;
            return viz.Generate(new DotPrinter(), "");
        }

        private static void FormatVertex<TVertex>(object sender, FormatVertexEventArgs<TVertex> e)
        {
            switch (e.Vertex)
            {
                case Stock _:
                    e.VertexFormatter.Shape = GraphvizVertexShape.Box;
                    break;
                case Flow _:
                    e.VertexFormatter.Shape = GraphvizVertexShape.House;
                    break;
                case Auxiliary _:
                    e.VertexFormatter.Shape = GraphvizVertexShape.Circle;
                    break;
            }

            e.VertexFormatter.Label = e.Vertex.ToString();
        }

        private static void FormatEdge<TVertex, TEdge>(object sender, FormatEdgeEventArgs<TVertex, TEdge> e)
            where TEdge : IEdge<TVertex>
        {
            e.EdgeFormatter.Style = e.Edge is InformationFlow ? GraphvizEdgeStyle.Dotted : GraphvizEdgeStyle.Solid;
        }
    }
}