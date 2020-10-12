#region Licence

// Description: SymuSysDyn - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

namespace Symu.SysDyn.Graph
{
    public class Flow : Node
    {
        public Flow(string name, string eqn) : base(name, eqn)
        {
            Eqn = eqn;
            Value = CheckInitialValue(eqn);
        }
    }
}