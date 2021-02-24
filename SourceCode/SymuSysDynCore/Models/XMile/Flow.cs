#region Licence

// Description: SymuBiz - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

#endregion

namespace Symu.SysDyn.Core.Models.XMile
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
        public Flow() : base()
        {
        }
        private Flow(string name, string model) : base(name, model)
        {
        }

        public static async Task<Flow> CreateFlow(string name, string model, string eqn, GraphicalFunction graph, Range range, Range scale,
            NonNegative nonNegative, VariableAccess access) 
        {
            return await CreateVariable<Flow>(name, model, eqn, graph, range, scale, nonNegative, access) ;
        }

        public static async Task<Flow> CreateInstance(string name, XMileModel model, string eqn, GraphicalFunction graph, Range range,
            Range scale,
            NonNegative nonNegative, VariableAccess access)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var variable = await CreateFlow(name, model.Name, eqn, graph, range, scale, nonNegative, access);
            model.Variables.Add(variable);
            return variable;
        }

        public override async Task<IVariable> Clone()
        {
            var clone = new Flow(Name, Model);
            await CopyTo(clone);
            return clone;
        }
    }
}