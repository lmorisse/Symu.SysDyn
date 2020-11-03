#region Licence

// Description: SymuSysDyn - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Symu.SysDyn.Parser;
using ArgumentNullException = System.ArgumentNullException;

#endregion

namespace Symu.SysDyn.Models
{
    /// <summary>
    ///     Define a XMile module.
    ///     Modules are placeholders in the variables section, and in the stock-flow diagram, for submodels. 
    /// </summary>
    public class Connect
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="to"></param>
        /// <param name="from"></param>
        public Connect(string to, string from)
        {
            To = StringUtils.CleanFullName(to);
            From = StringUtils.CleanFullName(from);
        }
        public string To { get; set; }
        public string From { get; set; }

        public bool Equals(string to, string from)
        {
            return To == to && From == from;
        }
    }
}