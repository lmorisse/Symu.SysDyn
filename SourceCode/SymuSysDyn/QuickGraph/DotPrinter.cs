#region Licence

// Description: SymuBiz - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using QuickGraph.Graphviz;
using QuickGraph.Graphviz.Dot;

#endregion

namespace Symu.SysDyn.QuickGraph
{
    /// <summary>
    ///     Implementation of IDotEngine to use QuickGraph.GraphViz
    ///     Generate a dotString
    /// </summary>
    public sealed class DotPrinter : IDotEngine
    {
        #region IDotEngine Members

        public string Run(GraphvizImageType imageType, string dot, string outputFileName)
        {
            return dot;
        }

        #endregion
    }
}