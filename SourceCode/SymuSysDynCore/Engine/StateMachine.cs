#region Licence

// Description: SymuSysDyn - SymuSysDynCore
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Linq;
using System.Threading.Tasks;
using Symu.Common.Core.Classes;
using Symu.SysDyn.Core.Models.XMile;
using Symu.SysDyn.Core.Parser;

#endregion

namespace Symu.SysDyn.Core.Engine
{
    public partial class StateMachine
    {
        public const string FrequencyFactor = "Frequencyfactor";
        private string _xmlFile;
        private bool _xmlValidate;

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

        public SimSpecs Simulation { get; private set; }
        public ModelCollection Models { get; private set; }

        /// <summary>
        ///     Create an instance of the state machine from an xml File
        ///     The stateMachine is Initialized
        /// </summary>
        /// <param name="xmlFile"></param>
        /// <param name="xmlValidate"></param>
        public static async Task<StateMachine> CreateStateMachine(string xmlFile, bool xmlValidate = true)
        {
            var stateMachine = new StateMachine {_xmlFile = xmlFile, _xmlValidate = xmlValidate};
            await stateMachine.Initialize();
            return stateMachine;
        }

        #region Initialize

        /// <summary>
        ///     Initialize the model
        ///     Don't store the result
        ///     Store the reference variables
        /// </summary>
        public async Task Initialize()
        {
            if (!string.IsNullOrEmpty(_xmlFile))
            {
                var xmlParser = new XmlParser(_xmlFile, _xmlValidate);
                Models = await xmlParser.ParseModels();
                Simulation = xmlParser.ParseSimSpecs();
                Simulation.OnTimer += OnTimer;
            }

            Variables = Models.GetVariables(); // see full model
        }

        public void Clear()
        {
            Simulation.Clear();
            Results.Clear();
            ReferenceVariables.Clear();
            _isPrepared = false;
        }

        /// <summary>
        ///     Set the simulation
        /// </summary>
        /// <param name="pauseInterval"></param>
        /// <param name="fidelity"></param>
        /// <param name="timeUnits"></param>
        public void SetSimulation(Fidelity fidelity, ushort pauseInterval, TimeStepType timeUnits)
        {
            Simulation.DeltaTime = fidelity switch
            {
                Fidelity.Low => 0.5F,
                Fidelity.Medium => 0.25F,
                Fidelity.High => 0.125F,
                _ => throw new ArgumentOutOfRangeException(nameof(fidelity), fidelity, null)
            };

            Simulation.PauseInterval = pauseInterval;
            Simulation.TimeUnits = timeUnits;
        }

        public float GetFrequency()
        {
            return Schedule.FrequencyFactor(Simulation.TimeUnits);
        }

        /// <summary>
        ///     Add a model
        /// </summary>
        /// <param name="model"></param>
        public void Add(XMileModel model)
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