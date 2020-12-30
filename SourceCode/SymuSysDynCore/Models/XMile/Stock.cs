#region Licence

// Description: SymuBiz - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Collections.Generic;
using Symu.SysDyn.Core.Equations;
using Symu.SysDyn.Core.Parser;

#endregion

namespace Symu.SysDyn.Core.Models.XMile
{
    /// <summary>
    ///     Core building block of a model, also called level or state. Stocks accumulate change.
    ///     Use Stock when past event influence present event. Stocks are a kind of memory, storing the results of past
    ///     actions.
    ///     Their value at the start of the simulation must be set as either a constant or with an initial equation.
    ///     The initial equation is evaluated only once, at the beginning of the simulation.
    /// </summary>
    public class Stock : Variable
    {
        private Stock(string name, string model) : base(name, model)
        {
        }
        /// <summary>
        ///     Constructor for root model
        /// </summary>
        /// <param name="name"></param>
        /// <param name="eqn"></param>
        /// <param name="inflow"></param>
        /// <param name="outflow"></param>
        public Stock(string name, string eqn, List<string> inflow, List<string> outflow) : this(name, string.Empty, eqn,
            inflow, outflow)
        {
        }

        public Stock(string name, string model, string eqn, List<string> inflow, List<string> outflow) : base(name,
            model, eqn)
        {
            Inflow = StringUtils.CleanNames(inflow);
            Outflow = StringUtils.CleanNames(outflow);
            SetChildren();
        }

        private Stock(string name, string model, string eqn, List<string> inflow, List<string> outflow,
            GraphicalFunction graph,
            Range range, Range scale, NonNegative nonNegative, VariableAccess access) : base(name, model, eqn, graph,
            range, scale, nonNegative, access)
        {
            Inflow = StringUtils.CleanNames(inflow);
            Outflow = StringUtils.CleanNames(outflow);
            SetChildren();
        }

        public static Stock CreateInstance(string name, XMileModel model, string eqn, List<string> inflow,
            List<string> outflow, GraphicalFunction graph,
            Range range, Range scale, NonNegative nonNegative, VariableAccess access)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var variable = new Stock(name, model.Name, eqn, inflow, outflow, graph, range, scale, nonNegative, access);
            model.Variables.Add(variable);
            return variable;
        }

        public override IVariable Clone()
        {
            var clone = new Stock(Name, Model);
            CopyTo(clone);
            clone.Inflow = Inflow;
            clone.Outflow = Outflow;
            return clone;
        }

        /// <summary>
        ///     stock(t) = stock(t - dt) + dt*(inflows(t - dt) – outflows(t - dt))
        ///     Re compute SetChildren with the new equation
        /// </summary>
        public void SetStockEquation(string dt)
        {
            var equation = Name;
            var inflows = AggregateFlows(Inflow, "+");
            var outflows = AggregateFlows(Outflow, "-");
            if (inflows.Length > 0 || outflows.Length > 0)
            {
                equation += "+" + dt + "*(" +
                            inflows;
                if (outflows.Length > 0)
                {
                    equation += "-" + outflows;
                }

                equation += ")";
            }

            Equation = EquationFactory.CreateInstance(Model, equation, out _);

            SetChildren();
        }

        private static string AggregateFlows(IReadOnlyList<string> list, string @operator)
        {
            var flow = string.Empty;
            if (list == null)
            {
                return flow;
            }

            for (var i = 0; i < list.Count; i++)
            {
                if (i == 0)
                {
                    flow += list[i];
                }
                else
                {
                    flow += @operator + list[i];
                }
            }

            return flow;
        }


        #region Xml attributes

        public List<string> Inflow { get; private set; }
        public List<string> Outflow { get; private set; }

        #endregion
    }
}