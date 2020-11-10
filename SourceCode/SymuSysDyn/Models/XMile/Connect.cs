#region Licence

// Description: SymuBiz - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Linq;
using Symu.SysDyn.Parser;

#endregion

namespace Symu.SysDyn.Models.XMile
{
    /// <summary>
    ///     Define a XMile module.
    ///     Modules are placeholders in the variables section, and in the stock-flow diagram, for subModels.
    /// </summary>
    public class Connect
    {
        /// <summary>
        /// </summary>
        /// <param name="model"></param>
        /// <param name="to"></param>
        /// <param name="from"></param>
        public Connect(string model, string to, string from)
        {
            if (!to.Contains('.'))
            {
                to = StringUtils.ConnectName(model, to);
            }

            if (!from.Contains('.'))
            {
                from = StringUtils.ConnectName(model, from);
            }

            To = StringUtils.CleanFullName(to);
            From = StringUtils.CleanFullName(from);
        }

        public string To { get; set; }
        public string From { get; set; }

        public static Connect CreateInstance(Module module, string to, string from)
        {
            if (module == null)
            {
                throw new ArgumentNullException(nameof(module));
            }

            var connect = new Connect(module.Model, to, from);
            module.Add(connect);
            return connect;
        }

        public bool Equals(string to, string from)
        {
            return To == to && From == from;
        }
    }
}