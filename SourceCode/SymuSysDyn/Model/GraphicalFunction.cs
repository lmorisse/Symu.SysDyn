#region Licence

// Description: SymuSysDyn - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Collections.Generic;
using System.Globalization;
using static Symu.Common.Constants;
#endregion

namespace Symu.SysDyn.Model
{
    /// <summary>
    /// Graphical functions are alternately called lookup functions and table functions. They are used to describe
    /// an arbitrary relationship between one input variable and one output variable.The domain of these
    /// functions is consistently referred to as x and the range is consistently referred to as y.
    /// A graphical function MUST be defined either with an x-axis scale and a set of y-values(evenly spaced
    /// across the given x-axis scale) or with a set of x-y pairs.
    /// </summary>
    public class GraphicalFunction
    {

        public float[] XPoints { get; }
        public float[] YPoints { get; }

        public Range XRange { get; }
        public Range YRange { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xPts">Optional</param>
        /// <param name="yPts"></param>
        /// <param name="xRange">Optional</param>
        /// <param name="yRange"></param>
        public GraphicalFunction(string xPts, string yPts, IReadOnlyList<string> xRange, IReadOnlyList<string> yRange)
        {
            if (yPts == null)
            {
                throw new ArgumentNullException(nameof(yPts));
            }

            if (yRange == null)
            {
                throw new ArgumentNullException(nameof(yRange));
            }

            XRange = new Range(xRange);
            YRange = new Range(yRange);

            var yTable = ParseStringTable(yPts);
            var xTable = xPts == null? CreateXTable(yTable.Length) : ParseStringTable(xPts);

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
        /// Check that Range and Points are 
        /// </summary>
        public void ChecksRange()
        {
            if (!XRange.Check(XPoints) || !YRange.Check(YPoints))
            {
                throw new ArgumentOutOfRangeException();
            }
            if (!XRange.Check(XPoints) || !YRange.Check(YPoints))
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
            var difference = XRange.Max - XRange.Min;
            var xTable = new float[divisions];
            if (divisions > 1)
            {

                var increment = difference / (divisions - 1);

                for (var counter = 0; counter < divisions; counter++)
                {
                    xTable[counter] = XRange.Min + increment * counter;
                }
            }
            else
            {
                xTable[0] = XRange.Min;
                xTable[1] = XRange.Max;
            }
            return xTable;
        }

        private float GetOutput(float x)
        {
            var target = 0;

            //find which line segment our x fits into 
            //TODO: currently n time, need faster implementation to protect against very high resolution graphical functions. 
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

        public float GetOutputWithBounds(float input)
        {
            return GetOutput(XRange.GetOutputInsideRange(input));
        }
    }
}