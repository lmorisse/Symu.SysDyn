#region Licence

// Description: SymuSysDyn - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Globalization;
using System.Linq;
using Symu.SysDyn.Simulation;

#endregion

namespace Symu.SysDyn.Equations
{
    /// <summary>
    ///     STEP built in function
    ///     STEP: Generate a step increase(or decrease) at the given time
    ///     Parameters: 2: (height, start time); step up/ down at start time
    /// </summary>
    /// <example>STEP(6, 3) steps from 0 to 6 at time 3(and stays there)</example>
    public class Step : BuiltInFunction
    {
        public const string Value = "Step";

        public Step(string function) : base(function)
        {
            // Height could be stored in string , but we need to check that it is a float
            Height = Convert.ToSingle(Parameters.ElementAt(0));
            StartTime = Convert.ToUInt16(Parameters.ElementAt(1));
            //TOdo Manage exception 
        }

        public float Height { get; }
        public ushort StartTime { get; }

        public override string Prepare(string word, SimSpecs sim)
        {
            if (sim == null)
            {
                throw new ArgumentNullException(nameof(sim));
            }

            return sim.Time == StartTime ? Height.ToString(CultureInfo.InvariantCulture) : "0";
        }
    }
}