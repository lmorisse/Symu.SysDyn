#region Licence

// Description: SymuSysDyn - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Linq;
using QuickGraph;
using Symu.SysDyn.Model;

#endregion

namespace Symu.SysDyn.QuickGraph
{
    public class Graph : BidirectionalGraph<string, VariableEdge>
    {
        public Graph(bool allowParallelEdges) : base(allowParallelEdges)
        {
        }

        /// <summary>
        ///     Create Graph of variables using QuickGraph
        /// </summary>
        /// <param name="variables"></param>
        /// <returns></returns>
        public static Graph CreateInstance(Variables variables)
        {
            if (variables == null)
            {
                throw new ArgumentNullException(nameof(variables));
            }

            var graph = new Graph(true);
            graph.AddVertexRange(variables.Select(x => x.Name).ToList());
            foreach (var variable in variables)
            {
                foreach (var child in variable.Children)
                {
                    VariableEdge edge;
                    // particular case : stock outflow
                    if (variable is Stock stock && stock.Outflow.Contains(child))
                    {
                        edge = new VariableEdge(variable.Name, child);
                    }
                    else
                    {
                        edge = new VariableEdge(child, variable.Name);
                    }
                    graph.AddEdge(edge);
                }

            }

            return graph;
        }
    }
}