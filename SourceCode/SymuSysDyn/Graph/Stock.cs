#region Licence

// Description: SymuSysDyn - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Collections.Generic;

#endregion

namespace Symu.SysDyn.Graph
{
    public class Stock : Node, IComparable
    {
        public Stock(string name, string eqn, List<string> inflow, List<string> outflow) : base(name, eqn)
        {
            Eqn = eqn;
            Inflow = inflow;
            Outflow = outflow;
            Equation = SetStockEquation(Name, inflow, outflow);
            Children = FindChildren();
        }

        #region Xml attributes

        public List<string> Inflow { get; set; }
        public List<string> Outflow { get; set; }

        #endregion

        #region IComparable

        public override bool Equals(object that)
        {
            if (that is Node stock)
            {
                return Name.Equals(stock.Name);
            }

            return ReferenceEquals(this, that);
        }

        public override int GetHashCode()
        {
            return Name != null ? Name.GetHashCode() : 0;
        }

        public int CompareTo(object obj)
        {
            var that = obj as Node;
            return string.Compare(Name, that?.Name, StringComparison.Ordinal);
        }

        #endregion
    }
}