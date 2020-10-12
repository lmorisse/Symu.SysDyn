#region Licence

// Description: SymuSysDyn - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Symu.SysDyn.XmlParser;

#endregion

namespace Symu.SysDyn.Graph
{
    /// <summary>
    ///     Base class for nodes of the graph
    /// </summary>
    public class Node
    {
        private readonly List<string> _operators = new List<string> { " ", "+", "-", "*", "/" };
        public Node(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            Name = name.Replace(' ', '_');
        }

        public Node(string name, string eqn) : this(name)
        {
            Value = CheckInitialValue(eqn);
            Equation = eqn;
            Children = FindChildren();
        }

        public Node(string name, string eqn, GraphicalFunction graph) : this(name, eqn)
        {
            Function = graph;
        }

        #region Xml attributes

        public string Eqn { get; set; }

        #endregion

        public string Name { get; }

        public float Value { get; set; }

        public string Equation { get; set; }

        public GraphicalFunction Function { get; set; }

        public float OldValue { get; set; }

        public bool Updated { get; set; }

        public bool InUse { get; set; }

        //public bool isStock { get; }

        public List<string> Children { get; protected set; }

        /// <summary>
        /// Children are items from Equation that are not numbers, operators, blanks nor itself
        /// </summary>
        /// <returns></returns>
        protected List<string> FindChildren()
        {
            var words = Equation?.Split(' ', '+', '-', '*', '/');

            return words?.Where(word => !_operators.Contains(word) && 
                                        !float.TryParse(word, NumberStyles.Any, CultureInfo.InvariantCulture, out _) && 
                                        word.Length > 0 
                                        && !word.Equals(Name)).ToList();
        }

        protected static float CheckInitialValue(string equation)
        {
            if (!float.TryParse(equation, out var n))
            {
                n = 0;
            }

            return n;
        }

        protected static string SetStockEquation(string name, IReadOnlyList<string> inflows,
            IReadOnlyList<string> outflows)
        {
            const string plus = " + ";
            const string minus = " - ";
            var equation = inflows.Aggregate(name, (current, inflow) => current + plus + inflow);
            return outflows.Aggregate(equation, (current, outflow) => current + minus + outflow);
        }
    }
}