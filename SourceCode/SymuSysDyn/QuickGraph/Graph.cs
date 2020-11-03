#region Licence

// Description: SymuSysDyn - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Collections.Generic;
using System.Linq;
using QuickGraph;
using Symu.SysDyn.Models;

#endregion

namespace Symu.SysDyn.QuickGraph
{
    public class Graph : BidirectionalGraph<IVariable, VariableEdge>
    {
        public Graph(bool allowParallelEdges) : base(allowParallelEdges)
        {
        }

        /// <summary>
        ///     Create Graph of variables using QuickGraph
        /// </summary>
        /// <param name="variables"></param>
        /// <returns></returns>
        public static Graph CreateInstance(VariableCollection variables)
        {
            if (variables == null)
            {
                throw new ArgumentNullException(nameof(variables));
            }

            var graph = new Graph(true);
            graph.AddVertexRange(variables);
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

                if (variable is Module module)
                {
                    foreach (var connect in module.Connects)
                    {
                        var from = variables.Get(connect.From);
                        // In case of subGraph
                        if (from == null)
                        {
                            continue;
                        }

                        var edge = new InformationFlow(from, variable);
                        graph.AddEdge(edge);
                    }
                }
            }

            return graph;
        }
    }
}