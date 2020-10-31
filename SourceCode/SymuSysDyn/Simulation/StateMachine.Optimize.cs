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
    public partial class StateMachine
    {
       
        public bool Optimized { get; set; }

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
            List<IVariable> waitingParents;
            //First remove all constant variables
            foreach (var variable in Variables.GetUpdated.Select(x => x.Name).ToImmutableList())
            {
                Variables.Remove(variable);
            }
            do
            {
                waitingParents = new List<IVariable>();
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
        public List<IVariable> OptimizeChildren(IVariable parent)
        {
            if (parent == null)
            {
                throw new ArgumentNullException(nameof(parent));
            }

            parent.Updating = true;
            var readyToUpdate = true;
            var waitingParents = new List<IVariable>();
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
        public bool TryOptimizeVariable(IVariable variable)
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

    }
}