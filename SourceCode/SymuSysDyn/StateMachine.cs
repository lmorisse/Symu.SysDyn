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
using System.Globalization;
using System.Linq;
using NCalc2;
using Symu.SysDyn.Graph;
using Symu.SysDyn.XmlParser;

#endregion

namespace Symu.SysDyn
{
    public class StateMachine
    {
        private readonly Hashtable _hashtable;
        private readonly Nodes _nodes;

        public StateMachine(string xmlFile, bool validate = true)
        {
            var xmlParser = new Parser(xmlFile, validate);
            _nodes = xmlParser.Parse();
            _hashtable = _nodes.GetHashTable();

            Simulate(); //prime the model with zeroth values
        }

        //public float GetDifference(string variable)
        //{
        //    var target = (Node) _hashtable[variable];
        //    return target.Value - target.OldValue;
        //}

        //returns current value
        public float GetVariable(string variable)
        {
            return ((Node) _hashtable[variable]).Value;
        }

        //sets next value
        public int SetVariable(string variable, float value)
        {
            if (_hashtable[variable] == null)
            {
                return 0;
            }

            _hashtable[variable] = value;
            return 1;

        }

        public void Simulate()
        {
            _nodes.Initialize();

            List<Node> waitingParents;
            do
            {
                waitingParents = new List<Node>();
                foreach (var variable in _nodes.GetNotUpdated)
                {
                    var withDupes = waitingParents;
                    withDupes.AddRange(UpdateChildren(variable, _hashtable));
                    waitingParents = withDupes.Distinct().ToList(); //no duplicates
                }
            } while (waitingParents.Count > 0);
        }

        //takes a variable and updates all variables listed as children. 
        public List<Node> UpdateChildren(Node parent, Hashtable table)
        {
            if (parent == null)
            {
                throw new ArgumentNullException(nameof(parent));
            }

            if (table == null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            parent.InUse = true;
            if (parent is Stock)
            {
                UpdateVariable(parent, table);
                parent.InUse = false;
            }

            var readyToUpdate = true;
            var waitingParents = new List<Node>();
            var children = parent.Children.ToArray();
            for (var index = 0; index < parent.Children.Count; index++) //check to see if children are busy
            {
                if (table[children[index]] == null)
                {
                    continue;
                }

                var child = (Node) table[children[index]];

                switch (child.Updated)
                {
                    //parent who needs to wait for children 
                    case false when child.InUse:
                        waitingParents.Add(parent);
                        readyToUpdate = false;
                        break;
                    case false:
                        waitingParents.AddRange(UpdateChildren(child, table));
                        break;
                }
            }

            if (readyToUpdate && !(parent is Stock))
            {
                UpdateVariable(parent, table);
            }

            parent.InUse = false;
            return waitingParents;
        }
        /// <summary>
        /// Take a variable and a hashtable of current variable results and update the value of that variable
        /// </summary>
        /// <param name="variable"></param>
        /// <param name="table"></param>
        public void UpdateVariable(Node variable, Hashtable table)
        {
            if (variable == null)
            {
                throw new ArgumentNullException(nameof(variable));
            }

            var value = CalculateEquation(variable.Equation, table);

            variable.Value = variable.Function == null ? value : GraphicalOutput(value, variable.Function);

            variable.Updated = true;
        }
        /// <summary>
        /// Takes equation and hashtable of current variable values returns the result of the equation as the float
        /// </summary>
        /// <param name="equation"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        private float CalculateEquation(string equation, Hashtable table)
        {
            var explicitEquation = MakeExplicitEquation(equation, table);
            var e = new Expression(explicitEquation);
            var value = float.Parse(e.Evaluate().ToString(), CultureInfo.InvariantCulture);

            return value;
        }

        private static string MakeExplicitEquation(string equation, IDictionary table)
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
                var target = (Node) table[words[counter]];
                if (target != null)
                {
                    words[counter] = target.Value.ToString(CultureInfo.InvariantCulture);
                }
            }

            //remake the string and return it
            var concat = string.Join(" ", words);
            return concat;
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

            if (words.Length > 1)
            {
                output = words[0];

                for (var counter = 1; counter < words.Length; counter++)
                {
                    output = output + replace + words[counter];
                }
            }

            return output;
        }


        private static float GraphicalOutput(float input, GraphicalFunction function)
        {
            var output = input;
            if (input > function.XMax)
            {
                output = function.XMax;
            }
            else if (input < function.XMin)
            {
                output = function.XMin;
            }

            output = function.GetOutput(output);
            return output;
        }
    }
}