#region Licence

// Description: SymuSysDyn - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Collections.Generic;
using System.Linq;
using Symu.SysDyn.Parser;

#endregion

namespace Symu.SysDyn.Model
{
    public class Stock : Variable, IComparable
    {
        public Stock(string name, string eqn, List<string> inflow, List<string> outflow) : base(name, eqn)
        {
            Eqn = eqn;
            Inflow = inflow;
            Outflow = outflow;
            Equation = SetStockEquation();
            FindChildren();
        }

        public string SetStockEquation()
        {
            var equation = Inflow.Aggregate(Name, (current, inflow) => current + XmlConstants.SpacePlus + inflow);
            return Outflow.Aggregate(equation, (current, outflow) => current + XmlConstants.SpaceMinus + outflow);
        }

        #region Xml attributes

        public List<string> Inflow { get; set; }
        public List<string> Outflow { get; set; }

        #endregion

        #region IComparable

        public override bool Equals(object that)
        {
            if (that is Variable stock)
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
            var that = obj as Variable;
            return string.Compare(Name, that?.Name, StringComparison.Ordinal);
        }

        #endregion
    }
}