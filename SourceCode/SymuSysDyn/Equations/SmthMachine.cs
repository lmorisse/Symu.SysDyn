﻿#region Licence

// Description: SymuSysDyn - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Symu.SysDyn.Model;
using Symu.SysDyn.Simulation;

namespace Symu.SysDyn.Equations
{
    /// <summary>
    /// The smth1, smth3 and smthn functions perform a first-, third- and nth-order respectively exponential smooth of input, using an exponential averaging time of averaging,
    /// and an optional initial value initial for the smooth.smth3 does this by setting up a cascade of three first-order exponential smooths, each with an averaging time of averaging/3.
    /// The other functions behave analogously.They return the value of the final smooth in the cascade.
    /// If you do not specify an initial value initial, they assume the value to be the initial value of input.
    /// </summary>
    public class SmthMachine
    {
        private readonly StateMachine _stateMachine = new StateMachine();

        public SmthMachine(string input, string averaging, byte order, string initial = "", float deltaTime= 0.5F)
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
            return new Auxiliary("Input", Input);
        }
        public Variable CreateInitial()
        {
            return new Auxiliary("Initial", Initial);
        }
        public Variable CreateAveraging()
        {
            return new Auxiliary("Averaging", Averaging);
        }
        public Variable CreateStock(byte order)
        {
            var eqn = Initial == string.Empty ? "Input" : "Initial";

            return new Stock("Comp"+order, eqn, "Flow"+order);
        }
        public Variable CreateFlow(byte order)
        {
            string eqn;
            if (order == 0)
            {
                eqn = "(Input - Comp0) * " + Order + " / Averaging";
            }
            else
            {
                eqn = "(Comp"+ (order-1)+" - Comp"+order+") * " + Order + " / Averaging";
            }
            return new Flow("Flow" + order, eqn);
        }

        public float Evaluate(ushort time)
        {
            _stateMachine.Initialize();
            _stateMachine.Simulation.Stop = time;
            _stateMachine.Clear();
            _stateMachine.Process();
            RemoveVariables();
            return _stateMachine.Variables.Get("Comp" + (Order - 1)).Value;
        }
        private readonly List<string> _variablesToRemove= new List<string>();
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
                _variablesToRemove.Add(variable.Name);
                _stateMachine.Variables.Add(variable);
            }
        }

        private void RemoveVariables()
        {
            foreach (var variable in _variablesToRemove)
            {
                _stateMachine.Variables.Remove(variable);
            }
        }
    }
}