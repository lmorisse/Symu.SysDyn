#region Licence

// Description: SymuSysDyn - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System.Linq;
using Symu.SysDyn.Model;
using Symu.SysDyn.Parser;
using Symu.SysDyn.QuickGraph;

#endregion

namespace Symu.SysDyn.Simulation
{
    public partial class StateMachine
    {
        /// <summary>
        ///     Immutable List of the optimized variables if optimized option is on
        /// </summary>
        private Variables _optimizedVariablesReference;

        /// <summary>
        ///     Create an instance of the state machine from an xml File
        ///     The stateMachine is Not Initialized - you have to call Initialize after having filled the variables
        /// </summary>
        public StateMachine()
        {
            Variables = new Variables();
            Simulation = new SimSpecs();
            Simulation.OnTimer += OnTimer;
        }

        /// <summary>
        ///     Create an instance of the state machine from an xml File
        ///     The stateMachine is Initialized
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

        #region Initialize

        public void Initialize()
        {
            Compute(); // Initialize the model / don't store the result
            SetStocksEquations();
            StoreReferenceVariables();
        }
        /// <summary>
        /// Clone the  Variables
        /// </summary>
        private void StoreReferenceVariables()
        {
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
    }
}