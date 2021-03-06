﻿#region Licence

// Description: SymuSysDyn - SymuSysDynCore
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Threading.Tasks;
using Symu.SysDyn.Core.Equations;
using Symu.SysDyn.Core.Models.XMile;

#endregion

namespace Symu.SysDyn.Core.Engine
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
        public async Task Prepare()
        {
            Clear(); // Simulation Start or DeltaTime may have change since the initialization
            Variables = string.IsNullOrEmpty(_processModel)
                ? await Models.GetVariables().Clone()
                : await Models.Get(_processModel).Variables.Clone();

            await ResolveConnects();
            foreach (var variable in Variables)
            {
                ReferenceVariables.Add(variable.FullName, variable.Value);
            }

            await Compute();
            await SetStocksEquations();
            foreach (var variable in Variables)
            {
                ReferenceVariables[variable.FullName] = variable.Value;
            }

            var allVariables = await Variables.Clone();
            await OptimizeVariables();
            Results.SetConstantResults(Variables, allVariables);
            _isPrepared = true;
        }


        /// <summary>
        ///     Once stock value is evaluated with initial equation, the real equation based on inflows and outflows is setted
        /// </summary>
        private async Task SetStocksEquations()
        {
            var dt = Simulation.DeltaTime.ToString(CultureInfo.InvariantCulture);
            foreach (var stock in Variables.Stocks)
            {
                await stock.SetStockEquation(dt);
            }
        }

        /// <summary>
        ///     Modules connect variables together
        ///     This method make the connection and prepare the connected variables
        /// </summary>
        public async Task ResolveConnects()
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

                    var factory = await EquationFactory.CreateInstance(from.Model, from.Name);
                    to.Equation = factory.Equation;
                    to.Value = factory.Value;
                    //to.Equation = new Equation(from.FullName) {Variables = new List<string> {from.FullName}};
                    //to.Value = from.Value;
                    to.Children = new List<string> {from.FullName};
                }

                Variables.Remove(module.FullName);
            }
        }
    }
}