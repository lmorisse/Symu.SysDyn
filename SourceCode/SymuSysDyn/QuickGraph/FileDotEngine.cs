#region Licence

// Description: SymuSysDyn - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System.IO;
using QuickGraph.Graphviz;
using QuickGraph.Graphviz.Dot;

#endregion

namespace Symu.SysDyn.QuickGraph
{
    /// <summary>
    ///     Implementation of IDotEngine to use QuickGraph.GraphViz
    ///     Generate a dotString in a a file
    /// </summary>
    public class FileDotEngine : IDotEngine
    {
        #region IDotEngine Members

        public string Run(GraphvizImageType imageType, string dot, string outputFileName)
        {
            using (var writer = new StreamWriter(outputFileName))
            {
                writer.Write(dot);
            }

            return Path.GetFileName(outputFileName);
        }

        #endregion
    }
}