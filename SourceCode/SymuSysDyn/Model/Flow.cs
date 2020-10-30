#region Licence

// Description: SymuSysDyn - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

namespace Symu.SysDyn.Model
{
    /// <summary>
    ///     Flows represent rates of change of the stocks.
    /// Non negative : invokes an optional macro that prevents the flow from going negative(also called a unidirectional flow, or uniflow –
    /// a flow without this property is a bidirectional flow, or biflow, i.e., material can flow in either direction depending on whether the flow value is positive or negative)
    /// </summary>
    public class Flow : Variable
    {
        public Flow(string name, string eqn) : base(name, eqn)
        {
        }
        public Flow(string name, string eqn, GraphicalFunction graph, Range range, Range scale, NonNegative nonNegative) : base(name, eqn, graph,
            range, scale, nonNegative)
        {
        }
    }
}