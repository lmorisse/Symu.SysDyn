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
    ///  a variable that is not a flow and is capable of changing its value instantaneously.
    /// </summary>
    public class Auxiliary : Variable
    {
        public Auxiliary(string name, string eqn) : base(name, eqn)
        {
        }
        public Auxiliary(string name, string eqn, GraphicalFunction graph, Range range, Range scale) : base(name, eqn,
            graph, range, scale)
        {
        }
        //Auxiliaries have one OPTIONAL attribute:
        // Flow concept: flow_concept="…" with true/false, which is true if the auxiliary represents a
        //flow concept(default: false). Besides documenting that the variable is conceptually a flow, this
        //affects how values are reported under certain integration methods and in tables.
    }
}