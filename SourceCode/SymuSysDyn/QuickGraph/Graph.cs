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
    public class Graph : BidirectionalGraph<Variable, VariableEdge>
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
            graph.AddVertexRange(variables.ToList());
            foreach (var variable in variables)
            {
                foreach (var childName in variable.Children)
                {
                    var child = variables.Get(childName);
                    VariableEdge edge;
                    // particular case : stock outflow
                    if (variable is Stock stock)
                    {
                        if (stock.Outflow.Contains(childName))
                        {
                            edge = new CausalLink(variable, child);
                        }
                        else if (stock.Inflow.Contains(childName))
                        {
                            edge = new CausalLink(child, variable);
                        }
                        else
                        {
                            edge = new InformationFlow(child, variable);
                        }
                    }
                    else
                    {
                        edge = new InformationFlow(child, variable);
                    }

                    // In case of subGraph
                    if (edge.Source != null && edge.Target != null)
                    {
                        graph.AddEdge(edge);
                    }
                }
            }

            return graph;
        }
    }
}