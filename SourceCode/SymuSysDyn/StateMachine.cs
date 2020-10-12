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
using Symu.SysDyn.Results;

#endregion

namespace Symu.SysDyn
{
    public class StateMachine
    {
        private readonly Variables _variables;
        private readonly Parser _xmlParser;
        private readonly ResultCollection _results =new ResultCollection();

        public StateMachine(string xmlFile, bool validate = true)
        {
            _xmlParser = new Parser(xmlFile, validate);
            _variables = _xmlParser.Parse();
            Process(); // Initialize the model
        }

        /// <summary>
        /// returns current value of a node
        /// </summary>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        public float GetValue(string nodeId)
        {
            return _variables[nodeId].Value;
        }

        public void Process()
        {
            _variables.Initialize();

            List<Variable> waitingParents;
            do
            {
                waitingParents = new List<Variable>();
                foreach (var variable in _variables.GetNotUpdated)
                {
                    var withChildren = waitingParents;
                    withChildren.AddRange(UpdateChildren(variable));
                    waitingParents = withChildren.Distinct().ToList(); //no duplicates
                }
            } while (waitingParents.Count > 0);

            _results.Add(Result.CreateInstance(_variables));

        }
        /// <summary>
        /// Takes a variable and updates all variables listed as children. 
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public List<Variable> UpdateChildren(Variable parent)
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
            var waitingParents = new List<Variable>();
            var children = parent.Children.ToArray();
            for (var index = 0; index < parent.Children.Count; index++) //check to see if children are busy
            {
                if (_variables[children[index]] == null)
                {
                    continue;
                }

                var child = _variables[children[index]];

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
        public void UpdateNode(Variable node)
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
                var target = _variables[words[counter]];
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
            return _xmlParser.CreateGraph("[GLOBAL]", _variables);
        }

        public IEnumerable<string> GetVariables()
        {
            return _variables.GetStocks().Select(x => x.Name);
        }

        public IEnumerable<float> GetResults(string name)
        {
            return _results.GetResults(name);
        }
    }
}