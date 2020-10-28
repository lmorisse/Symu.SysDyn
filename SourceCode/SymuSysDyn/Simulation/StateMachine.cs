#region Licence

// Description: SymuSysDyn - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using NCalc2;
using Symu.SysDyn.Equations;
using Symu.SysDyn.Functions;
using Symu.SysDyn.Model;
using Symu.SysDyn.Parser;
using Symu.SysDyn.QuickGraph;
using Symu.SysDyn.Results;

#endregion

namespace Symu.SysDyn.Simulation
{
    public class StateMachine
    {
        /// <summary>
        /// Immutable List of the optimized variables if optimized option is on
        /// </summary>
        private Variables _optimizedVariablesReference;
        /// <summary>
        /// Create an instance of the state machine from an xml File
        /// The stateMachine is Not Initialized - you have to call Initialize after having filled the variables
        /// </summary>
        public StateMachine()
        {
            Variables = new Variables();
            Simulation = new SimSpecs();
            Simulation.OnTimer += OnTimer;
        }
        /// <summary>
        /// Create an instance of the state machine from an xml File
        /// The stateMachine is Initialized
        /// </summary>
        /// <param name="xmlFile"></param>
        /// <param name="validate"></param>
        public StateMachine(string xmlFile, bool validate = true)
        {
            var xmlParser = new XmlParser(xmlFile, validate);
            Variables = xmlParser.ParseVariables();
            Simulation = xmlParser.ParseSimSpecs();
            Simulation.OnTimer += OnTimer;
            Initialize();
        }

        public SimSpecs Simulation { get; }
        public Variables ReferenceVariables { get; private set; } 
        public Variables Variables { get; private set; } 
        public ResultCollection Results { get; } = new ResultCollection();
        public bool StoreResults { get; set; } = true;
        public bool Optimized { get; set; }

        #region Initialize


        public void Initialize()
        {
            Compute(); // Initialize the model / don't store the result
            SetStocksEquations();
            StoreReferenceVariables();
            Simulation.Clear();
        }

        private void StoreReferenceVariables()
        {
            // Clone the  Variables
            ReferenceVariables = Variables.Clone();

        }
        private void RetrieveFromReferenceVariables()
        {
            if (Variables.Any())
            {
                return;
            }

            Variables = ReferenceVariables.Clone();
        }
        /// <summary>
        /// Store the optimized variables
        /// </summary>
        private void StoreOptimizedReferenceVariables()
        {
            _optimizedVariablesReference = Variables.Clone();
        }
        private void RetrieveFromOptimizedReferenceVariables()
        {
            foreach (var variable in _optimizedVariablesReference)
            {
                Variables.Add(variable.Clone());
            }
        }

        /// <summary>
        ///     Optimize variables
        /// </summary>
        public void OptimizeVariables()
        {
            if (!Optimized)
            {
                RetrieveFromReferenceVariables();
                return;
            }

            if (Simulation.State != SimState.NotStarted)
            {
                return;
            }

            if (_optimizedVariablesReference != null)
            {
               RetrieveFromOptimizedReferenceVariables();
                return;
            }

            RetrieveFromReferenceVariables();

            Variables.Initialize();
            List<Variable> waitingParents;
            //First remove all constant variables
            foreach (var variable in Variables.GetUpdated.Select(x => x.Name).ToImmutableList())
            {
                Variables.Remove(variable);
            }
            do
            {
                waitingParents = new List<Variable>();
                foreach (var variable in Variables.GetNotUpdated.ToImmutableList())
                {
                    var withChildren = waitingParents;
                    withChildren.AddRange(OptimizeChildren(variable));
                    waitingParents = withChildren.Distinct().ToList(); //no duplicates
                }
            } while (waitingParents.Any());
            StoreOptimizedReferenceVariables();
        }

        /// <summary>
        ///     Takes a variable and updates all variables listed as children.
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public List<Variable> OptimizeChildren(Variable parent)
        {
            if (parent == null)
            {
                throw new ArgumentNullException(nameof(parent));
            }

            parent.Updating = true;
            var readyToUpdate = true;
            var waitingParents = new List<Variable>();
            foreach (var child in parent.Children.Select(childName => Variables[childName]).Where(x => x != null && !x.Updated)) 
            {
                switch (child.Updating)
                {
                    //parent who needs to wait for children 
                    case true:
                        waitingParents.Add(parent);
                        readyToUpdate = false;
                        break;
                    case false:
                        waitingParents.AddRange(OptimizeChildren(child));
                        break;
                }
            }

            // Update other variables
            if (readyToUpdate && TryOptimizeVariable(parent))
            {
                ReferenceVariables.SetValue(parent.Name, parent.Value);
                Variables.Remove(parent.Name);
            }

            parent.Updating = false;
            return waitingParents;
        }

        /// <summary>
        ///     Take a variable and update the value of that node
        /// </summary>
        /// <param name="variable"></param>
        public bool TryOptimizeVariable(Variable variable)
        {
            if (variable == null)
            {
                throw new ArgumentNullException(nameof(variable));
            }

            if (variable.TryOptimize(false))
            {
                return true;
            }
            // Replace function Dt
            if (variable.Equation is ComplexEquation complexEquation)
            {
                complexEquation.Replace(Dt.Value, Simulation.DeltaTime.ToString(CultureInfo.InvariantCulture));
            }
            // Replace variable per its value
            foreach (var child in variable.Children.Select(childName => ReferenceVariables[childName]).ToImmutableList().
                Where(child => !Variables.Exists(child.Name)))
            {
                variable.Equation.Replace(child.Name, child.Value.ToString(CultureInfo.InvariantCulture));
                variable.Children.Remove(child.Name);
            }
            variable.Updated = true;
            return variable.TryOptimize(true);
        }

        public void Clear()
        {
            Simulation.Clear();
            Results.Clear();
            Variables.Clear();
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
        #endregion

        #region Process
        /// <summary>
        ///     Process compute all iterations from Simulation.Start to Simulation.Stop
        /// </summary>
        public void Process()
        {
            OptimizeVariables();
            while (Simulation.Run())
            {
                Compute();
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
            } while (waitingParents.Any());
        }

        /// <summary>
        ///     Timer has a new value, we store the results
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnTimer(object sender, EventArgs e)
        {
            if (StoreResults)
            {
                Results.Add(Result.CreateInstance(Variables));
            }
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
            foreach (var child in parent.Children.Select(childName => Variables[childName]).Where(x => x != null && !x.Updated))
            {
                switch (child.Updating)
                {
                    //parent who needs to wait for children 
                    case true:
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

            variable.Update(Variables, Simulation);
        }
        #endregion

        /// <summary>
        ///     Create Graph of variables using QuickGraph
        /// </summary>
        public Graph GetGraph()
        {
            return Graph.CreateInstance(ReferenceVariables);
        }

        /// <summary>
        ///     Create a SubGraph of variables via a group name using QuickGraph
        /// </summary>
        public Graph GetSubGraph(string groupName)
        {
            return Graph.CreateInstance(ReferenceVariables.GetGroupVariables(groupName));
        }
    }
}