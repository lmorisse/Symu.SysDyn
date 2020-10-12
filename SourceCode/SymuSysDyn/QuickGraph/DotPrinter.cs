#region Licence

// Description: SymuSysDyn - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

using QuickGraph.Graphviz;
using QuickGraph.Graphviz.Dot;

namespace Symu.SysDyn.QuickGraph
{
    /// <summary>
    /// Implementation of IDotEngine to use QuickGraph.GraphViz
    /// Generate a dotString
    /// </summary>
    public sealed class DotPrinter : IDotEngine
    {
        public string Run(GraphvizImageType imageType, string dot, string outputFileName)
        {
            return dot;
        }
    }
}