﻿#region Licence

// Description: SymuSysDyn - SymuSysDynCore
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Symu.SysDyn.Core.Models.XMile;

#endregion

namespace Symu.SysDyn.Core.Engine
{
    public partial class StateMachine
    {
        public bool Optimized { get; set; } = true;

        /// <summary>
        ///     Optimize variables
        /// </summary>
        public async Task OptimizeVariables()
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
                    withChildren.AddRange(await OptimizeChildren(variable));
                    waitingParents = withChildren.Distinct().ToList(); //no duplicates
                }
            } while (waitingParents.Any());
        }

        /// <summary>
        ///     Takes a variable and updates all variables listed as children.
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public async Task<List<IVariable>> OptimizeChildren(IVariable parent)
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
                        waitingParents.AddRange(await OptimizeChildren(child));
                        break;
                }
            }

            // Update other variables
            if (readyToUpdate && await TryOptimizeVariable(parent, Simulation))
            {
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
        public async Task<bool> TryOptimizeVariable(IVariable variable, SimSpecs sim)
        {
            if (variable == null)
            {
                throw new ArgumentNullException(nameof(variable));
            }


            if (await variable.TryOptimize(false, sim) || variable.Equation.Expression?.ParsedExpression == null)
            {
                return true;
            }

            foreach (var childName in variable.Children.Where(x => !Variables.Exists(x)).ToImmutableList())
            {
                if (!ReferenceVariables.ContainsKey(childName))
                {
                    // For Time & Dt
                    continue;
                }

                var childValue = ReferenceVariables[childName].ToString(CultureInfo.InvariantCulture);
                variable.Equation.Replace(childName, childValue);
                variable.Children.Remove(childName);
            }

            variable.Updated = true;
            return await variable.TryOptimize(true, sim);
        }
    }
}