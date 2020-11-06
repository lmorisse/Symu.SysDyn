#region Licence

// Description: SymuSysDyn - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using Symu.SysDyn.Equations;
using Symu.SysDyn.Functions;
using Symu.SysDyn.Models;

#endregion

namespace Symu.SysDyn.Engine
{
    public partial class StateMachine
    {
        public bool Optimized { get; set; } = true;

        /// <summary>
        ///     Optimize variables
        /// </summary>
        public void OptimizeVariables()
        {
            if (!Optimized)
            {
                return;
            }

            Variables.Initialize();
            List<IVariable> waitingParents;
            //First remove all constant variables
            foreach (var variable in Variables.GetUpdated.Select(x => x.FullName).ToImmutableList())
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
            foreach (var child in parent.Children.Select(childName => Variables[childName])
                .Where(x => x != null && !x.Updated))
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
            if (readyToUpdate && TryOptimizeVariable(parent, Simulation))
            {
                //ReferenceVariables.SetValue(parent.FullName, parent.Value);
                ReferenceVariables[parent.FullName] = parent.Value;
                Variables.Remove(parent.FullName);
            }

            parent.Updating = false;
            return waitingParents;
        }

        /// <summary>
        ///     Take a variable and update the value of that node
        /// </summary>
        /// <param name="variable"></param>
        /// <param name="sim"></param>
        public bool TryOptimizeVariable(IVariable variable, SimSpecs sim)
        {
            if (variable == null)
            {
                throw new ArgumentNullException(nameof(variable));
            }

            if (variable.TryOptimize(false, sim))
            {
                return true;
            }

            // Replace function Dt
            if (variable.Equation is ComplexEquation complexEquation)
            {
                complexEquation.Replace(Dt.Value, Simulation.DeltaTime.ToString(CultureInfo.InvariantCulture), Simulation);
            }

            // Replace variable per its value
            //foreach (var child in variable.Children.Select(childName => ReferenceVariables[childName]).ToImmutableList()
            //    .Where(child => !Variables.Exists(child.Key)))
            //{
            //    variable.Equation.Replace(child.Key.ToString(), child.Value.ToString(), Simulation);
            //    variable.Children.Remove(child.Key.ToString());
            //}
            foreach (var childName in variable.Children.Where(x => !Variables.Exists(x)).ToImmutableList())
            {
                var childValue = ReferenceVariables[childName].ToString(CultureInfo.InvariantCulture);
                variable.Equation.Replace(childName, childValue, Simulation);
                variable.Children.Remove(childName);
            }

            variable.Updated = true;
            return variable.TryOptimize(true, sim);
        }
    }
}