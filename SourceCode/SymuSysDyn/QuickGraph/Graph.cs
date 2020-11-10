#region Licence

// Description: SymuBiz - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using QuickGraph;
using Symu.SysDyn.Models;
using Symu.SysDyn.Models.XMile;
using Symu.SysDyn.Parser;

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
                // Children
                foreach (var childName in variable.Children)
                {
                    var child = variables.Get(childName);
                    var edge = new InformationFlow(child, variable);
                    // In case of subGraph
                    if (edge.Source != null && edge.Target != null)
                    {
                        graph.AddEdge(edge);
                    }
                }

                switch (variable)
                {
                    // particular cases
                    case Stock stock:
                    {
                        foreach (var outflow in stock.Outflow)
                        {
                            var target = variables.Get(StringUtils.FullName(variable.Model, outflow));
                            if (target != null)
                            {
                                graph.AddEdge(new CausalLink(variable, target));
                            }
                        }

                        foreach (var inflow in stock.Inflow)
                        {
                            var source = variables.Get(StringUtils.FullName(variable.Model, inflow));
                            if (source != null)
                            {
                                graph.AddEdge(new CausalLink(source, variable));
                            }
                        }

                        break;
                    }
                    case Module module:
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

                        break;
                    }
                }
            }

            return graph;
        }
    }
}