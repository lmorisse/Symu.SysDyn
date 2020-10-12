#region Licence

// Description: SymuSysDyn - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

namespace Symu.SysDyn.Model
{
    public class Auxiliary : Node
    {
        public Auxiliary(string name, string eqn, GraphicalFunction graph) : base(name, eqn, graph)
        {
        }
    }
}