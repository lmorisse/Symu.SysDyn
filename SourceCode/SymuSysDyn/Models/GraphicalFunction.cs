#region Licence

// Description: SymuSysDyn - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using static Symu.Common.Constants;

#endregion

namespace Symu.SysDyn.Models
{
    /// <summary>
    ///     Graphical functions are alternately called lookup functions and table functions. They are used to describe
    ///     an arbitrary relationship between one input variable and one output variable.The domain of these
    ///     functions is consistently referred to as x and the range is consistently referred to as y.
    ///     A graphical function MUST be defined either with an x-axis scale and a set of y-values(evenly spaced
    ///     across the given x-axis scale) or with a set of x-y pairs.
    /// </summary>
    public class GraphicalFunction
    {
        /// <summary>
        /// </summary>
        /// <param name="xPts">Optional</param>
        /// <param name="yPts"></param>
        /// <param name="xScale">Optional</param>
        /// <param name="yScale"></param>
        public GraphicalFunction(string xPts, string yPts, IReadOnlyList<string> xScale, IReadOnlyList<string> yScale)
        {
            if (yPts == null)
            {
                throw new ArgumentNullException(nameof(yPts));
            }

            if (yScale == null)
            {
                throw new ArgumentNullException(nameof(yScale));
            }

            XScale = new Range(xScale);
            YScale = new Range(yScale);

            var yTable = ParseStringTable(yPts);
            var xTable = xPts == null ? CreateXTable(yTable.Length) : ParseStringTable(xPts);

            XPoints = new float[xTable.Length];
            for (var counter = 0; counter < xTable.Length; counter++)
            {
                XPoints[counter] = xTable[counter];
            }

            YPoints = new float[yTable.Length];
            for (var counter = 0; counter < yTable.Length; counter++)
            {
                YPoints[counter] = yTable[counter];
            }

            ChecksRange();
        }

        /// <summary>
        ///     specifies the exact x-values corresponding to the y-values given in the YPoints
        ///     default: the x-axis XScale evenly divided into the number of points in YPoints creating a(fixed) increment along
        ///     the x - axis).
        ///     Note that when this is specified, the number of points given MUST exactly match the number of points in YPoints,
        ///     one x - value for each and every y - value, corresponding positionally,
        ///     and the values must be sorted in ascending order(from smallest to largest).
        ///     By default, these values are comma - separated.
        ///     REQUIRED when no x - axis scale.
        /// </summary>
        public float[] XPoints { get; }

        /// <summary>
        ///     specifies the y-values for the graphical function, starting with the y-value for the smallest x-value
        ///     and continuing as x increases until ending with the y-value corresponding to the largest x-value.
        ///     By default, these values are comma-separated.
        ///     REQUIRED
        /// </summary>
        public float[] YPoints { get; }

        /// <summary>
        ///     defines the scale of the x-axis
        ///     default: smallest and largest values in XPoints
        ///     REQUIRED when no x-axis points.
        /// </summary>
        public Range XScale { get; }

        /// <summary>
        ///     defines the scale of the y-axis
        ///     default: smallest and largest values in YPoints.
        ///     This only affects the scale of the graph as shown in a user interface;
        ///     it has no impact on the behavior or interpretation of the graphical function.
        /// </summary>
        public Range YScale { get; }

        /// <summary>
        ///     Check that Range and Points are
        /// </summary>
        public void ChecksRange()
        {
            if (!XScale.Check(XPoints) || !YScale.Check(YPoints))
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        public static float[] ParseStringTable(string inputTable)
        {
            if (inputTable == null)
            {
                throw new ArgumentNullException(nameof(inputTable));
            }

            if (inputTable.Length == 0)
            {
                return new float[0];
            }

            var words = inputTable.Split(',');

            var outputTable = new float[words.Length];

            for (var counter = 0; counter < words.Length; counter++)
            {
                outputTable[counter] = float.Parse(words[counter], CultureInfo.InvariantCulture);
            }

            return outputTable;
        }

        public float[] CreateXTable(int divisions)
        {
            var difference = XScale.Max - XScale.Min;
            if (float.IsInfinity(difference))
            {
                throw new ArgumentOutOfRangeException();
            }

            var xTable = new float[divisions];
            if (divisions > 1)
            {
                var increment = difference / (divisions - 1);

                for (var counter = 0; counter < divisions; counter++)
                {
                    xTable[counter] = XScale.Min + increment * counter;
                }
            }
            else
            {
                xTable[0] = XScale.Min;
                xTable[1] = XScale.Max;
            }

            return xTable;
        }

        /// <summary>
        ///     find which line segment our x fits into
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public float GetOutput(float x)
        {
            var target = 0;

            if (x < XPoints[0])
            {
                return YPoints[0];
            }

            if (x > XPoints.Last())
            {
                return YPoints.Last();
            }

            for (var counter = 0; counter < XPoints.Length; counter++)
            {
                if (x < XPoints[counter])
                {
                    target = counter;
                    break;
                }

                if (Math.Abs(x - XPoints[counter]) < Tolerance)
                {
                    return YPoints[counter];
                }
            }

            var a = YPoints[target] - YPoints[target - 1]; //y2-y1
            var b = XPoints[target] - XPoints[target - 1]; //x2-x1
            var m = a / b; //slope
            var intercept = YPoints[target] - m * XPoints[target]; //b

            return m * x + intercept;
        }
    }
}