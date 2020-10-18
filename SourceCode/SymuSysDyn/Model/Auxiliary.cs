#region Licence

// Description: SymuSysDyn - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

namespace Symu.SysDyn.Model
{
    /// <summary>
    ///     Auxiliaries allow the isolation of any algebraic function that is used. They can both clarify a model and
    ///     factor out important or repeated calculations
    /// </summary>
    public class Auxiliary : Variable
    {
        public Auxiliary(string name, string eqn, GraphicalFunction graph, Range range, Range scale) : base(name, eqn,
            graph, range, scale)
        {
        }
    }
}