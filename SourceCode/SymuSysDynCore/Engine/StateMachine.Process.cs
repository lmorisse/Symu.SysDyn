#region Licence

// Description: SymuBiz - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Symu.SysDyn.Core.Models.XMile;
using Symu.SysDyn.Core.Results;

#endregion

namespace Symu.SysDyn.Core.Engine
{
    public partial class StateMachine
    {
        private bool _isPrepared;
        public ResultCollection Results { get; } = new ResultCollection();
        public bool StoreResults { get; set; } = true;

        /// <summary>
        ///     Process compute all iterations from Simulation.Start to Simulation.Stop
        /// </summary>
        /// <param name="model">The name of the subModel or empty string for a global process</param>
        /// <remarks>true if the process was successful</remarks>
        public bool Process(string model = "")
        {
            if (!_isPrepared || _processModel != model)
            {
                _processModel = model;
                Prepare().Wait();
            }

            while (Simulation.Run())
            {
                Compute().Wait();
                //Intentionally after Compute
                Simulation.OnTimerEvent();
            }
            return true;
        }
        
        /// <summary>
        ///     Process compute all iterations from Simulation.Start to Simulation.Stop
        /// </summary>
        /// <param name="model">The name of the subModel or empty string for a global process</param>
        /// <remarks>true if the process was successful</remarks>
        public async Task<bool> ProcessAsync(string model = "")
        {
            if (!_isPrepared || _processModel != model)
            {
                _processModel = model;
                await Prepare();
            }

            while (Simulation.Run())
            {
                await Compute();
                //Intentionally after Compute
                Simulation.OnTimerEvent();
            }
            return true;
        }

        /// <summary>
        ///     Compute one iteration
        /// </summary>
        public async Task Compute()
        {
            Variables.Initialize();

            List<IVariable> waitingParents;
            do
            {
                waitingParents = new List<IVariable>();
                foreach (var variable in Variables.GetNotUpdated)
                {
                    var withChildren = waitingParents;
                    withChildren.AddRange(await UpdateChildren(variable));
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
        public async Task<List<IVariable>> UpdateChildren(IVariable parent)
        {
            if (parent == null)
            {
                throw new ArgumentNullException(nameof(parent));
            }

            parent.Updating = true;
            // Update stocks before other variables
            var stockFirst = _isPrepared && parent is Stock;
            if (stockFirst)
            {
                await UpdateVariable(parent);
                parent.Updating = false;
            }

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
                        waitingParents.AddRange(await UpdateChildren(child));
                        break;
                }
            }

            // Update other variables
            if (readyToUpdate && !stockFirst)
            {
                await UpdateVariable(parent);
            }

            parent.Updating = false;
            return waitingParents;
        }

        /// <summary>
        ///     Take a variable and update the value of that node
        /// </summary>
        /// <param name="variable"></param>
        public async Task UpdateVariable(IVariable variable)
        {
            if (variable == null)
            {
                throw new ArgumentNullException(nameof(variable));
            }

            await variable.Update(Variables, Simulation);
        }
    }
}