#region Licence

// Description: SymuSysDyn - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace Symu.SysDyn.Graph
{
    /// <summary>
    ///     Base class for nodes of the graph
    /// </summary>
    public class Nodes
    {
        private readonly List<Node> _nodes = new List<Node>();
        public IEnumerable<Node> GetNotUpdated => _nodes.Where(x => !x.Updated);

        public void Add(Node node)
        {
            _nodes.Add(node);
        }

        public void AddRange(IEnumerable<Node> nodes)
        {
            _nodes.AddRange(nodes);
        }

        public Node GetNode(string nodeId)
        {
            return _nodes.Find(x => x.Name == nodeId);
        }

        public Hashtable GetHashTable()
        {
            var hashtable = new Hashtable();
            foreach (var node in _nodes)
            {
                hashtable.Add(node.Name, node);
            }

            return hashtable;
        }

        public void Initialize()
        {
            foreach (var node in _nodes)
            {
                node.Updated = false;
                node.OldValue = node.Value;
            }
        }

        public List<Tuple<Stock, string>> GetInflows()
        {
            var nodeInflows = new List<Tuple<Stock, string>>();
            foreach (var node in _nodes.OfType<Stock>())
            {
                nodeInflows.AddRange(node.Inflow.Select(inflow => new Tuple<Stock, string>(node, inflow)));
            }

            return nodeInflows;
        }

        public List<Tuple<Stock, string>> GetOutflows()
        {
            var nodeOutflows = new List<Tuple<Stock, string>>();
            foreach (var node in GetStocks())
            {
                nodeOutflows.AddRange(node.Outflow.Select(outflow => new Tuple<Stock, string>(node, outflow)));
            }

            return nodeOutflows;
        }

        public IEnumerable<Stock> GetStocks()
        {
            return _nodes.OfType<Stock>();
        }
    }
}