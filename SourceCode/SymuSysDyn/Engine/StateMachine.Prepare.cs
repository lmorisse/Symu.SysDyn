#region Licence

// Description: SymuBiz - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using Symu.SysDyn.Equations;
using Symu.SysDyn.Models;
using Symu.SysDyn.Models.XMile;
using Symu.SysDyn.Parser;

#endregion

namespace Symu.SysDyn.Engine
{
    public partial class StateMachine
    {
        /// <summary>
        ///     The actual name of the model (Root or subModel) to process
        /// </summary>
        private string _processModel;

        /// <summary>
        ///     Working list of the variables of the simulation
        ///     Can be all the variables or only a subModel's variables
        /// </summary>
        public VariableCollection Variables { get; set; }

        /// <summary>
        ///     The reference of the variables of the simulation
        /// </summary>
        public Dictionary<string, float> ReferenceVariables { get; } = new Dictionary<string, float>();

        /// <summary>
        ///     Prepare to process
        /// </summary>
        /// <returns>True if the variables need to be optimized</returns>
        public void Prepare()
        {
            Clear(); // Simulation Start or DeltaTime may have change since the initialization
            Variables = string.IsNullOrEmpty(_processModel)
                ? Models.GetVariables().Clone()
                : Models.Get(_processModel).Variables.Clone();

            ResolveConnects();
            foreach (var variable in Variables)
            {
                ReferenceVariables.Add(variable.FullName, variable.Value);
            }

            Compute(); // Initial value
            SetStocksEquations();
            foreach (var variable in Variables)
            {
                ReferenceVariables[variable.FullName] = variable.Value;
            }

            var allVariables = Variables.Clone();
            OptimizeVariables();
            Results.SetConstantResults(Variables, allVariables);
            _isPrepared = true;
        }


        /// <summary>
        ///     Once stock value is evaluated with initial equation, the real equation based on inflows and outflows is setted
        /// </summary>
        private void SetStocksEquations()
        {
            var dt = Simulation.DeltaTime.ToString(CultureInfo.InvariantCulture);
            foreach (var stock in Variables.Stocks)
            {
                stock.SetStockEquation(dt);
            }
        }
        /// <summary>
        ///     Modules connect variables together
        ///     This method make the connection and prepare the connected variables
        /// </summary>
        public void ResolveConnects()
        {
            foreach (var module in Variables.Modules.ToImmutableList())
            {
                var connects = module.Connects;
                foreach (var connect in connects.ToImmutableList())
                {
                    var to = Variables.Get(connect.To);
                    var from = Variables.Get(connect.From);
                    if (to.Access != VariableAccess.Input || from.Access != VariableAccess.Output)
                    {
                        continue;
                    }

                    if (to is Stock)
                    {
                        // to prevent the computation of toVariable as a stock, it is cast as an auxiliary
                        Variables[to.FullName] = new Auxiliary(to.Name, to.Model);
                        to = Variables[to.FullName];
                    }

                    // variables and words must be two different lists
                    to.Equation = new SimpleEquation(from.FullName, from.FullName, new List<string> {from.FullName},
                        new List<string> {from.FullName}, null);
                    to.Value = from.Value;
                    to.Children = new List<string> {from.FullName};
                }

                Variables.Remove(module.FullName);
            }
        }
    }
}