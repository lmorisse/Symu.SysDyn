#region Licence

// Description: SymuSysDyn - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Symu.SysDyn.Model;
using Symu.SysDyn.Simulation;

namespace Symu.SysDyn.Functions
{
    /// <summary>
    /// The smth1, smth3 and smthn functions perform a first-, third- and nth-order respectively exponential smooth of input, using an exponential averaging time of averaging,
    /// and an optional initial value initial for the smooth.smth3 does this by setting up a cascade of three first-order exponential smooths, each with an averaging time of averaging/3.
    /// The other functions behave analogously.They return the value of the final smooth in the cascade.
    /// If you do not specify an initial value initial, they assume the value to be the initial value of input.
    /// </summary>
    public class SmthMachine
    {
        private const string SmthInput = "Smthinput";
        private const string SmthInitial = "Smthinitial";
        private const string SmthAveraging = "Smthaveraging";
        private const string SmthComp = "Smthcomp";
        private const string SmthFlow = "Smthflow";
        private readonly StateMachine _stateMachine = new StateMachine();

        public SmthMachine(string input, string averaging, byte order, string initial, float deltaTime= 0.5F)
        {
            Input = input;
            Averaging = averaging;
            Order = order;
            Initial = initial;

            _stateMachine.StoreResults = false;
            _stateMachine.Simulation.DeltaTime  = deltaTime;
            _stateMachine.Variables.Add(CreateInput());
            _stateMachine.Variables.Add(CreateInitial());
            _stateMachine.Variables.Add(CreateAveraging());
            for (byte i = 0; i < order; i++)
            {
                _stateMachine.Variables.Add(CreateStock(i));
                _stateMachine.Variables.Add(CreateFlow(i));
            }

            _stateMachine.Optimized = true;
        }
        public string Input { get; }
        public string Averaging { get; }
        public string Initial { get; }
        /// <summary>
        /// Nth order smooth
        /// </summary>
        public byte Order { get; }

        public Variable CreateInput()
        {
            return new Auxiliary(SmthInput, Input);
        }
        public Variable CreateInitial()
        {
            return new Auxiliary(SmthInitial, Initial);
        }
        public Variable CreateAveraging()
        {
            return new Auxiliary(SmthAveraging, Averaging);
        }
        public Variable CreateStock(byte order)
        {
            //var eqn = Initial == string.Empty ? SmthInput : SmthInitial;
            return new Stock(SmthComp+order, SmthInitial, SmthFlow + order);
        }
        public Variable CreateFlow(byte order)
        {
            string eqn;
            if (order == 0)
            {
                eqn = "("+SmthInput+"-"+SmthComp+"0)*" + Order + "/"+SmthAveraging;
            }
            else
            {
                eqn = "("+SmthComp+ (order-1)+ "-"+ SmthComp + order+")*" + Order + "/"+SmthAveraging;
            }
            return new Flow(SmthFlow + order, eqn);
        }

        public float Evaluate(ushort time)
        {
            _stateMachine.Simulation.Stop = time;
            // Initialize intentionally in Evaluate and not in Constructor because of external parameters
            _stateMachine.Initialize();
            _stateMachine.Process();
            return _stateMachine.Variables.Get(SmthComp + (Order - 1)).Value;
        }
        /// <summary>
        /// If SMTH has variables as parameters, we need to had them in the state machine so that can be initialized
        /// </summary>
        /// <param name="variables"></param>
        public void AddVariables(Variables variables)
        {
            var children = new List<string>();
            foreach (var variable in _stateMachine.Variables.Select(x => x.Children).Distinct())
            {
                children.AddRange(variable);
            }
            children = children.Distinct().ToList();


            foreach (var variable in from child in children where variables.Exists(child) select variables.Get(child))
            {
                // Adding a constant variable, not the real one
                // Because teh variable is already updated, and should not be updated once again
                var constant = new Variable(variable.Name, variable.Value.ToString(CultureInfo.InvariantCulture));
                _stateMachine.Variables.Add(constant);
            }
        }
    }
}