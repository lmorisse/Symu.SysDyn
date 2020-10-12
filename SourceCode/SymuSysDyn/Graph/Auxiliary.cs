#region Licence

// Description: SymuSysDyn - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

using Symu.SysDyn.XmlParser;

namespace Symu.SysDyn.Graph
{
    public class Auxiliary : Node
    {
        public Auxiliary(string name, string eqn, GraphicalFunction graph) : base(name, eqn, graph)
        {
        }
    }
}