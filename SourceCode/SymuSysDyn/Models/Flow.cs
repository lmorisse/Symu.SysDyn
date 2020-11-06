#region Licence

// Description: SymuBiz - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;

#endregion

namespace Symu.SysDyn.Models
{
    /// <summary>
    ///     Flows represent rates of change of the stocks.
    ///     Non negative : invokes an optional macro that prevents the flow from going negative(also called a unidirectional
    ///     flow, or uniflow –
    ///     a flow without this property is a bidirectional flow, or biflow, i.e., material can flow in either direction
    ///     depending on whether the flow value is positive or negative)
    /// </summary>
    public class Flow : Variable
    {
        private Flow(string name, string model) : base(name, model)
        {
        }

        public Flow(string name, string model, string eqn, GraphicalFunction graph, Range range, Range scale,
            NonNegative nonNegative, VariableAccess access) : base(name, model, eqn, graph, range, scale, nonNegative,
            access)
        {
        }

        public static Flow CreateInstance(string name, Model model, string eqn, GraphicalFunction graph, Range range,
            Range scale,
            NonNegative nonNegative, VariableAccess access)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var variable = new Flow(name, model.Name, eqn, graph, range, scale, nonNegative, access);
            model.Variables.Add(variable);
            return variable;
        }

        public override IVariable Clone()
        {
            var clone = new Flow(Name, Model);
            CopyTo(clone);
            return clone;
        }
    }
}