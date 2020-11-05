#region Licence

// Description: SymuSysDyn - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using Symu.SysDyn.Models;
using Symu.SysDyn.Parser;
using Symu.SysDyn.QuickGraph;

#endregion

namespace Symu.SysDyn.Engine
{
    public partial class StateMachine
    {

        /// <summary>
        ///     Create an instance of the state machine from an xml File
        ///     The stateMachine is Not Initialized - you have to call Initialize after having filled the variables
        /// </summary>
        public StateMachine()
        {
            Models = new ModelCollection();
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
            Models = xmlParser.ParseModels();
            Simulation = xmlParser.ParseSimSpecs();
            Simulation.OnTimer += OnTimer;
            Initialize();
        }

        public SimSpecs Simulation { get; }
        public ModelCollection Models { get; }

        #region Graph
        /// <summary>
        ///     Create Graph of variables using QuickGraph
        /// </summary>
        public Graph GetGraph()
        {
            return Graph.CreateInstance(Models.GetVariables());
        }

        /// <summary>
        ///     Create a SubGraph of variables of the root model via a group name using QuickGraph
        /// </summary>
        public Graph GetGroupGraph(string groupName)
        {
            return Graph.CreateInstance(Models.RootModel.GetGroupVariables(groupName));
        }

        /// <summary>
        ///     Create a SubGraph of variables of a subModel via a group name using QuickGraph
        /// </summary>
        public Graph GetRootModelGraph()
        {

            return Graph.CreateInstance(Models.RootModel.Variables);
        }

        /// <summary>
        ///     Create a SubGraph of variables of a subModel via a group name using QuickGraph
        /// </summary>
        public Graph GetSubModelGraph(string modelName)
        {

            return Graph.CreateInstance(Models.Get(modelName).Variables);
        }
        #endregion

        #region Initialize
        /// <summary>
        /// Initialize the model
        /// Don't store the result
        /// Store the reference variables
        /// </summary>
        public void Initialize()
        {
            Variables = Models.GetVariables(); // see full model
        }

        public void Clear()
        {
            Simulation.Clear();
            Results.Clear();
            ReferenceVariables.Clear();
        }

        #endregion
    }
}