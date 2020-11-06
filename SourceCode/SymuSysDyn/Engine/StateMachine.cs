#region Licence

// Description: SymuBiz - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Linq;
using Symu.Common.Classes;
using Symu.SysDyn.Models;
using Symu.SysDyn.Parser;
using Symu.SysDyn.QuickGraph;

#endregion

namespace Symu.SysDyn.Engine
{
    public partial class StateMachine
    {
        public const string FrequencyFactor = "Frequencyfactor";

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
        ///     Initialize the model
        ///     Don't store the result
        ///     Store the reference variables
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

        /// <summary>
        ///     Set the simulation
        /// </summary>
        /// <param name="pauseInterval"></param>
        /// <param name="fidelity"></param>
        /// <param name="timeUnits"></param>
        public void SetSimulation(Fidelity fidelity, ushort pauseInterval, TimeStepType timeUnits)
        {
            switch (fidelity)
            {
                case Fidelity.Low:
                    Simulation.DeltaTime = 0.5F;
                    break;
                case Fidelity.Medium:
                    Simulation.DeltaTime = 0.25F;
                    break;
                case Fidelity.High:
                    Simulation.DeltaTime = 0.125F;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(fidelity), fidelity, null);
            }

            Simulation.PauseInterval = pauseInterval;
            Simulation.TimeUnits = timeUnits;
        }

        public void InitializeRootModel(Model model)
        {
            var frequency = Schedule.FrequencyFactor(Simulation.TimeUnits).ToString();
            Auxiliary.CreateInstance(FrequencyFactor, model, frequency, null, null, null, new NonNegative(true),
                VariableAccess.Output);
        }

        /// <summary>
        ///     Add a model
        /// </summary>
        /// <param name="model"></param>
        public void Add(Model model)
        {
            Models.Add(model);
        }

        /// <summary>
        ///     Add a model collection
        /// </summary>
        /// <param name="models">List of the variables of the model</param>
        public void Add(ModelCollection models)
        {
            if (models == null)
            {
                throw new ArgumentNullException(nameof(models));
            }

            // Root Model
            Models.RootModel.Add(models.RootModel);
            if (models.Count() == 1)
            {
                return;
            }

            // SubModels
            for (var i = 1; i < models.Count(); i++)
            {
                Models.Add(models[i]);
            }
        }

        #endregion
    }
}