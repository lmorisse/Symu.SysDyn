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

#endregion

namespace Symu.SysDyn.Model
{
    public class GraphicalFunction
    {
        private readonly float[,] _points;
        private readonly float[] _xBounds = new float [2];
        private readonly float[] _yBounds = new float [2];

        public GraphicalFunction(string ypts, IReadOnlyList<string> bounds)
        {
            if (ypts == null)
            {
                throw new ArgumentNullException(nameof(ypts));
            }

            if (bounds == null)
            {
                throw new ArgumentNullException(nameof(bounds));
            }

            _xBounds[0] = float.Parse(bounds[0], CultureInfo.InvariantCulture);
            _xBounds[1] = float.Parse(bounds[1], CultureInfo.InvariantCulture);
            _yBounds[0] = float.Parse(bounds[2], CultureInfo.InvariantCulture);
            _yBounds[1] = float.Parse(bounds[3], CultureInfo.InvariantCulture);

            var yTable = ParseStringTable(ypts);
            var xTable = CreateXTable(_xBounds, yTable.Length);
            _points = new float[yTable.Length, 2];

            for (var counter = 0; counter < yTable.Length; counter++)
            {
                _points[counter, 0] = xTable[counter];
                _points[counter, 1] = yTable[counter];
            }
        }

        public float XMin => _xBounds[0];

        public float XMax => _xBounds[1];

        public float YMin => _yBounds[0];

        public float YMax => _yBounds[1];

        private static float[] ParseStringTable(string inputTable)
        {
            var words = inputTable.Split(',');

            var outputTable = new float[words.Length];

            for (var counter = 0; counter < words.Length; counter++)
            {
                outputTable[counter] = float.Parse(words[counter], CultureInfo.InvariantCulture);
            }

            return outputTable;
        }

        private static float[] CreateXTable(IReadOnlyList<float> bounds, int divisions)
        {
            var difference = bounds[1] - bounds[0];

            var increment = difference / (divisions - 1);

            var xTable = new float[divisions];

            for (var counter = 0; counter < divisions; counter++)
            {
                xTable[counter] = bounds[0] + increment * counter;
            }

            return xTable;
        }

        private float GetOutput(float x)
        {
            var target = 0;

            //find which line segment our x fits into 
            //TODO: currently n time, need faster implementation to protect against very high resolution graphical functions. 
            for (var counter = 0; counter < _points.GetLongLength(0); counter++)
            {
                if (x < _points[counter, 0])
                {
                    target = counter;
                    break;
                }

                //todo use Symu.Common Tolerance
                if (Math.Abs(x - _points[counter, 0]) < 0.0001)
                {
                    return _points[counter, 1];
                }
            }

            var a = _points[target, 1] - _points[target - 1, 1]; //y2-y1
            var b = _points[target, 0] - _points[target - 1, 0]; //x2-x1
            var m = a / b; //slope
            var intercept = _points[target, 1] - m * _points[target, 0]; //b

            return m * x + intercept;
        }

        public float GetOutputWithBounds(float input)
        {
            var output = input;
            if (input > XMax)
            {
                output = XMax;
            }
            else if (input < XMin)
            {
                output = XMin;
            }

            return GetOutput(output);
        }
    }
}