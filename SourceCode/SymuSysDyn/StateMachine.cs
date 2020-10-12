#region Licence

// Description: SymuSysDyn - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using NCalc2;
using Symu.SysDyn.Model;
using Symu.SysDyn.QuickGraph;

#endregion

namespace Symu.SysDyn
{
    public class StateMachine
    {
        private readonly Nodes _nodes;
        private readonly Parser xmlParser;

        public StateMachine(string xmlFile, bool validate = true)
        {
            xmlParser = new Parser(xmlFile, validate);
            _nodes = xmlParser.Parse();
            Process(); // Initialize the model
        }

        /// <summary>
        /// returns current value of a node
        /// </summary>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        public float GetVariable(string nodeId)
        {
            return _nodes[nodeId].Value;
        }

        public void Process()
        {
            _nodes.Initialize();

            List<Node> waitingParents;
            do
            {
                waitingParents = new List<Node>();
                foreach (var variable in _nodes.GetNotUpdated)
                {
                    var withChildren = waitingParents;
                    withChildren.AddRange(UpdateChildren(variable));
                    waitingParents = withChildren.Distinct().ToList(); //no duplicates
                }
            } while (waitingParents.Count > 0);
        }
        /// <summary>
        /// Takes a variable and updates all variables listed as children. 
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public List<Node> UpdateChildren(Node parent)
        {
            if (parent == null)
            {
                throw new ArgumentNullException(nameof(parent));
            }

            parent.InUse = true;
            if (parent is Stock)
            {
                UpdateNode(parent);
                parent.InUse = false;
            }

            var readyToUpdate = true;
            var waitingParents = new List<Node>();
            var children = parent.Children.ToArray();
            for (var index = 0; index < parent.Children.Count; index++) //check to see if children are busy
            {
                if (_nodes[children[index]] == null)
                {
                    continue;
                }

                var child = _nodes[children[index]];

                switch (child.Updated)
                {
                    //parent who needs to wait for children 
                    case false when child.InUse:
                        waitingParents.Add(parent);
                        readyToUpdate = false;
                        break;
                    case false:
                        waitingParents.AddRange(UpdateChildren(child));
                        break;
                }
            }

            if (readyToUpdate && !(parent is Stock))
            {
                UpdateNode(parent);
            }

            parent.InUse = false;
            return waitingParents;
        }

        /// <summary>
        /// Take a node and update the value of that node
        /// </summary>
        /// <param name="node"></param>
        public void UpdateNode(Node node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            var value = CalculateEquation(node.Equation);

            node.Value = node.Function?.GraphicalOutput(value) ?? value;

            node.Updated = true;
        }

        /// <summary>
        /// Takes equation and hashtable of current variable values returns the result of the equation as the float
        /// </summary>
        /// <param name="equation"></param>
        /// <returns></returns>
        private float CalculateEquation(string equation)
        {
            try
            {
                var explicitEquation = MakeExplicitEquation(equation);
                var e = new Expression(explicitEquation);
                return float.Parse(e.Evaluate().ToString(), CultureInfo.InvariantCulture);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(nameof(equation) +  " : the internal details for this exception are as follows: \r\n" + ex.Message);
            }
            //todo Trace.WriteLineIf(World.LoggingSwitch.TraceVerbose, equation + "was calculate");
        }

        private string MakeExplicitEquation(string equation)
        {
            var operators = new List<string> {"+", "-", "*", "/"};
            equation = AddSpacesToString(equation);
            //break equation into pieces
            var words = equation.Split(' ');

            //for each piece
            for (var counter = 0; counter < words.Length; counter++)
            {
                //that isn't a math operator
                if (operators.Contains(words[counter]))
                {
                    continue;
                }

                //convert it to the value in table
                var target = _nodes[words[counter]];
                if (target != null)
                {
                    words[counter] = target.Value.ToString(CultureInfo.InvariantCulture);
                }
            }

            //remake the string and return it
            return string.Join(" ", words);
        }

        private static string AddSpacesToString(string input)
        {
            var output = input;
            output = AddSPaceToStringBySeparator(input, '*', " * ", output);
            output = AddSPaceToStringBySeparator(input, '/', " / ", output);
            output = AddSPaceToStringBySeparator(input, '-', " - ", output);
            output = AddSPaceToStringBySeparator(input, '+', " + ", output);
            return output;
        }

        private static string AddSPaceToStringBySeparator(string input, char separator, string replace, string output)
        {
            var words = input.Split(separator);

            if (words.Length <= 1)
            {
                return output;
            }

            output = words[0];

            for (var counter = 1; counter < words.Length; counter++)
            {
                output += replace + words[counter];
            }

            return output;
        }

        public Graph GetGraph()
        {
            //todo => parse header.Name
            return xmlParser.CreateGraph("[GLOBAL]", _nodes);
        }
    }
}