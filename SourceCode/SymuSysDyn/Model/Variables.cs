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

namespace Symu.SysDyn.Model
{
    /// <summary>
    ///     List of the variables of the model
    /// </summary>
    public class Variables : IEnumerable<Variable>
    {
        private readonly List<Variable> _nodes = new List<Variable>();
        public IEnumerable<Variable> GetNotUpdated => _nodes.Where(x => !x.Updated);

        /// <summary>
        ///     Gets or sets the node with the specified name
        /// </summary>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        public Variable this[string nodeId]
        {
            get => GetNode(nodeId);
            //set => GetNode(nodeId).Value = value;
        }

        public void Add(Variable node)
        {
            _nodes.Add(node);
        }

        public void AddRange(IEnumerable<Variable> nodes)
        {
            _nodes.AddRange(nodes);
        }

        public Variable GetNode(string nodeId)
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

        #region IEnumerator members


        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<Variable> GetEnumerator()
        {
            return _nodes.GetEnumerator();
        }

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _nodes.GetEnumerator();
        }

        #endregion

    }
}