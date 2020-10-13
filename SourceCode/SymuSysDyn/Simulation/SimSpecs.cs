#region Licence

// Description: SymuSysDyn - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

namespace Symu.SysDyn.Simulation
{
    /// <summary>
    ///     SimSpecs is the structure to define and store information about the simulation
    ///     It is based on sim_specs element of the xmile schema (schema.xsd)
    /// </summary>
    public class SimSpecs
    {
        public SimSpecs(float start, float stop, float deltaTime)
        {
            Stop = stop;
            Start = start;
            DeltaTime = deltaTime;
        }

        public float Stop { get; set; }
        public float Start { get; set; }
        public float DeltaTime { get; set; }
    }
}