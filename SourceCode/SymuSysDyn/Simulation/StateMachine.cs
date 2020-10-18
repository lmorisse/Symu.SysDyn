#region Licence

// Description: SymuSysDyn - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Collections.Generic;
using System.Linq;
using Symu.SysDyn.Model;
using Symu.SysDyn.Parser;
using Symu.SysDyn.QuickGraph;
using Symu.SysDyn.Results;

#endregion

namespace Symu.SysDyn.Simulation
{
    public class StateMachine
    {
        public StateMachine()
        {
            Variables = new Variables();
            Simulation = new SimSpecs();
            Simulation.OnTimer += OnTimer;
        }

        public StateMachine(string xmlFile, bool validate = true)
        {
            var xmlParser = new XmlParser(xmlFile, validate);
            Variables = xmlParser.ParseVariables();
            Simulation = xmlParser.ParseSimSpecs();
            Compute(); // Initialize the model / don't store the result
            SetStocksEquations();
            Simulation.OnTimer += OnTimer;
        }

        public SimSpecs Simulation { get; }
        public Variables Variables { get; }
        public ResultCollection Results { get; } = new ResultCollection();

        /// <summary>
        ///     Process compute all iterations from Simulation.Start to Simulation.Stop
        /// </summary>
        public void Process()
        {
            while (Simulation.Run())
            {
                Compute();
            }
        }

        /// <summary>
        ///     Once stock value is evaluated with initial equation, the real equation based on inflows and outflows is setted
        /// </summary>
        private void SetStocksEquations()
        {
            foreach (var stock in Variables.Stocks)
            {
                stock.SetStockEquation();
            }
        }

        /// <summary>
        ///     Compute one iteration
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
        }

        /// <summary>
        ///     Timer has a new value, we store the results
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnTimer(object sender, EventArgs e)
        {
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
        ///     Take a variable and update the value of that node
        /// </summary>
        /// <param name="variable"></param>
        public void UpdateVariable(Variable variable)
        {
            if (variable == null)
            {
                throw new ArgumentNullException(nameof(variable));
            }

            var value = variable.Equation.Compute(Variables, Simulation);

            variable.Value = variable.Function?.GetOutputWithBounds(value) ?? value;

            variable.Updated = true;
        }

        /// <summary>
        ///     Create Graph of variables using QuickGraph
        /// </summary>
        public Graph GetGraph()
        {
            return Graph.CreateInstance(Variables);
        }

        /// <summary>
        ///     Create a SubGraph of variables via a group name using QuickGraph
        /// </summary>
        public Graph GetSubGraph(string groupName)
        {
            return Graph.CreateInstance(Variables.GetGroupVariables(groupName));
        }
    }
}