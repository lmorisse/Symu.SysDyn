#region Licence

// Description: SymuSysDyn - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Symu.SysDyn.Equations;
using Symu.SysDyn.Models;
using Symu.SysDyn.Parser;
using Symu.SysDyn.QuickGraph;

#endregion

namespace Symu.SysDyn.Simulation
{
    public partial class StateMachine
    {
        /// <summary>
        /// The actual name of the model (Root or subModel) to process
        /// </summary>
        private string _processModel;
        /// <summary>
        ///     Immutable List of the optimized variables if optimized option is on
        /// </summary>
        private VariableCollection _referenceOptimizedVariables;
        /// <summary>
        ///     Working list of the variables of the simulation
        ///     Can be all the variables or only a subModel's variables
        /// </summary>
        public VariableCollection Variables { get; private set; }
        /// <summary>
        ///     The reference of the variables of the simulation
        /// </summary>
        public VariableCollection ReferenceVariables { get; private set; }

        /// <summary>
        /// Clone the  Variables
        /// </summary>
        private void StoreReferenceVariables()
        {
            //ReferenceVariables = Models.GetVariables().Clone();
            ReferenceVariables = string.IsNullOrEmpty(_processModel) 
                ? Models.GetVariables().Clone() 
                : Models.Get(_processModel).Variables.Clone();
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
        /// Reset the working Variables
        /// </summary>
        /// <returns>True if the variables need to be optimized</returns>
        private bool SetVariables(string model)
        {
            Variables.Clear();
            if (_processModel != model)
            {
                _processModel = model;
                StoreReferenceVariables();
                ResolveConnects();
            }
            if (!Optimized)
            {
                RetrieveFromReferenceVariables();

                return false;
            }

            if (Simulation.State != SimState.NotStarted)
            {
                return false;
            }

            if (_referenceOptimizedVariables != null)
            {
                RetrieveFromOptimizedReferenceVariables();
                return false;
            }

            RetrieveFromReferenceVariables();
            return true;
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
        ///     Modules connect variables together
        ///     This method make the connection and prepare the connected variables
        /// </summary>
        public void ResolveConnects()
        {
            foreach (var connects in ReferenceVariables.Modules.Select(x => x.Connects).ToImmutableList())
            {
                foreach (var connect in connects)
                {
                    var to = ReferenceVariables.Get(connect.To);
                    var from = ReferenceVariables.Get(connect.From);
                    if (to.Access != VariableAccess.Input || from.Access != VariableAccess.Output)
                    {
                        continue;
                    }


                    if (to is Stock)
                    {
                        // to prevent the computation of to as a stock, it is cast as an auxiliary
                        ReferenceVariables[to.FullName] = new Auxiliary(to.Name, to.Model);
                        to = ReferenceVariables[to.FullName];
                    }

                    var words = new List<string> { from.FullName };
                    to.Equation = new SimpleEquation(from.FullName, from.FullName, words, words, null);//EquationFactory.CreateInstance(from.Model, from.Name, out var value);
                    to.Value = from.Value;
                    to.Children = new List<string> {from.FullName};
                }
            }
        }
    }
}