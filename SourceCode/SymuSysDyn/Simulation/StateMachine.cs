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
using NCalc2;
using Symu.SysDyn.Model;
using Symu.SysDyn.Parser;
using Symu.SysDyn.QuickGraph;
using Symu.SysDyn.Results;

#endregion

namespace Symu.SysDyn.Simulation
{
    public class StateMachine
    {
        public StateMachine(string xmlFile, bool validate = true)
        {
            var xmlParser = new XmlParser(xmlFile, validate);
            Variables = xmlParser.ParseVariables();
            Simulation = xmlParser.ParseSimSpecs();
            Compute(); // Initialize the model
        }

        public SimSpecs Simulation { get; }
        public Variables Variables { get; }
        public ResultCollection Results { get; } = new ResultCollection();

        /// <summary>
        /// Process compute all iterations from Simulation.Start to Simulation.Stop
        /// </summary>
        public void Process()
        {
            // TODO : implement _stateMachine.Simulation.DeltaTime
            for (var i = Simulation.Start; i < Simulation.Stop; i++)
            {
                Compute();
            }
        }

        /// <summary>
        /// Compute one iteration
        /// </summary>
        public void Compute()
        {
            Variables.Initialize();

            List<Variable> waitingParents;
            do
            {
                waitingParents = new List<Variable>();
                foreach (var variable in Variables.GetNotUpdated)
                {
                    var withChildren = waitingParents;
                    withChildren.AddRange(UpdateChildren(variable));
                    waitingParents = withChildren.Distinct().ToList(); //no duplicates
                }
            } while (waitingParents.Count > 0);

            Results.Add(Result.CreateInstance(Variables));
        }

        /// <summary>
        ///     Takes a variable and updates all variables listed as children.
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public List<Variable> UpdateChildren(Variable parent)
        {
            if (parent == null)
            {
                throw new ArgumentNullException(nameof(parent));
            }

            parent.Updating = true;
            // Update stocks before other variables
            if (parent is Stock)
            {
                UpdateVariable(parent);
                parent.Updating = false;
            }

            var readyToUpdate = true;
            var waitingParents = new List<Variable>();
            foreach (var child in parent.Children.Select(childName => Variables[childName]))
            {
                switch (child.Updated)
                {
                    //parent who needs to wait for children 
                    case false when child.Updating:
                        waitingParents.Add(parent);
                        readyToUpdate = false;
                        break;
                    case false:
                        waitingParents.AddRange(UpdateChildren(child));
                        break;
                }
            }

            // Update other variables
            if (readyToUpdate && !(parent is Stock))
            {
                UpdateVariable(parent);
            }

            parent.Updating = false;
            return waitingParents;
        }

        /// <summary>
        ///     Take a node and update the value of that node
        /// </summary>
        /// <param name="variable"></param>
        public void UpdateVariable(Variable variable)
        {
            if (variable == null)
            {
                throw new ArgumentNullException(nameof(variable));
            }

            var value = CalculateEquation(variable.Equation);

            variable.Value = variable.Function?.GetOutputWithBounds(value) ?? value;

            variable.Updated = true;
        }

        /// <summary>
        ///     Takes equation and hashtable of current variable values returns the result of the equation as the float
        /// </summary>
        /// <param name="equation"></param>
        /// <returns></returns>
        private float CalculateEquation(string equation)
        {
            try
            {
                var explicitEquation = MakeExplicitEquation(equation);
                var e = new Expression(explicitEquation);
                return Convert.ToSingle(e.Evaluate());
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(nameof(equation) +
                                            " : the internal details for this exception are as follows: \r\n" +
                                            ex.Message);
            }

            //todo Trace.WriteLineIf(World.LoggingSwitch.TraceVerbose, equation + "was calculate");
        }

        private string MakeExplicitEquation(string equation)
        {
            var operators = new List<string>
                {XmlConstants.Multiplication, XmlConstants.Division, XmlConstants.Plus, XmlConstants.Minus};
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
                var target = Variables[words[counter]];
                if (target != null)
                {
                    words[counter] = target.Value.ToString(CultureInfo.InvariantCulture);
                }
            }

            //remake the string and return it
            return string.Join(XmlConstants.Blank, words);
        }

        private static string AddSpacesToString(string input)
        {
            var output = input;
            output = AddSPaceToStringBySeparator(input, '*', XmlConstants.SpaceMultiplication, output);
            output = AddSPaceToStringBySeparator(input, '/', XmlConstants.SpaceDivision, output);
            output = AddSPaceToStringBySeparator(input, '-', XmlConstants.SpaceMinus, output);
            output = AddSPaceToStringBySeparator(input, '+', XmlConstants.SpacePlus, output);
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

        /// <summary>
        ///     Create Graph of variables using QuickGraph
        /// </summary>
        public Graph GetGraph()
        {
            return Graph.CreateInstance(Variables);
        }
    }
}