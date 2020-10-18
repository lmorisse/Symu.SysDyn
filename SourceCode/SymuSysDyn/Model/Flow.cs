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
    /// </summary>
    public class Flow : Variable
    {
        public Flow(string name, string eqn, GraphicalFunction graph, Range range, Range scale) : base(name, eqn, graph,
            range, scale)
        {
            Eqn = eqn;
            Value = CheckInitialValue(eqn);
        }
    }
}