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
       
        public ResultCollection Results { get; } = new ResultCollection();
        public bool StoreResults { get; set; } = true;

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

            List<IVariable> waitingParents;
            do
            {
                waitingParents = new List<IVariable>();
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
        public List<IVariable> UpdateChildren(IVariable parent)
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
        public void UpdateVariable(IVariable variable)
        {
            if (variable == null)
            {
                throw new ArgumentNullException(nameof(variable));
            }

            variable.Update(Variables, Simulation);
        }
    }
}