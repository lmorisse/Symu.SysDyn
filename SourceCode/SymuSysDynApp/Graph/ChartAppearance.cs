#region Licence

// Description: SymuSysDyn - SymuSysDynApp
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Syncfusion.Drawing;
using Syncfusion.Windows.Forms.Chart;

#endregion

namespace SymuSysDynApp.Graph
{
    public static class ChartAppearance
    {
        public static void ApplyChartStyles(ChartControl chart)
        {
            if (chart == null)
            {
                throw new ArgumentNullException(nameof(chart));
            }

            #region Chart Appearance Customization

            chart.Skins = Skins.Metro;
            chart.BorderAppearance.SkinStyle = ChartBorderSkinStyle.None;
            chart.BorderAppearance.FrameThickness = new ChartThickness(-2, -2, 2, 2);
            chart.SmoothingMode = SmoothingMode.AntiAlias;
            chart.ChartArea.PrimaryXAxis.HidePartialLabels = true;
            chart.ElementsSpacing = 5;

            #endregion

            #region Axes Customization

            chart.PrimaryYAxis.RangeType = ChartAxisRangeType.Set;
            chart.PrimaryXAxis.RangeType = ChartAxisRangeType.Set;
            if (chart.Series.Count == 0 || chart.Series[0].Points.Count == 0)
            {
                return;
            }

            var minY = chart.Series[0].Points[0].YValues[0];
            var maxY = chart.Series[0].Points[0].YValues[0];
            for (var i = 0; i < chart.Series.Count; i++)
            {
                for (var j = 0; j < chart.Series[i].Points.Count; j++)
                {
                    minY = Math.Min(minY, chart.Series[i].Points[j].YValues[0]);
                }
            }

            for (var i = 0; i < chart.Series.Count; i++)
            {
                for (var j = 0; j < chart.Series[i].Points.Count; j++)
                {
                    if (chart.Series[i].Points[j].YValues.Length == 2)
                    {
                        maxY = Math.Max(maxY,
                            chart.Series[i].Points[j].YValues[0] + chart.Series[i].Points[j].YValues[1]);
                    }
                    else
                    {
                        maxY = Math.Max(maxY,
                            chart.Series[i].Points[j].YValues[0]);
                    }
                }
            }

            chart.PrimaryYAxis.Range = new MinMaxInfo(minY * 0.9, maxY * 1.1, Math.Round(maxY / 10));
            var min = chart.Series[0].Points[0].X;
            maxY = min;
            for (var i = 0; i < chart.Series.Count; i++)
            {
                for (var j = 0; j < chart.Series[i].Points.Count; j++)
                {
                    min = Math.Min(min, chart.Series[i].Points[j].X);
                    maxY = Math.Max(maxY, chart.Series[i].Points[j].X);
                }
            }

            chart.PrimaryXAxis.Range = new MinMaxInfo(min, maxY, Math.Round((maxY - min) / 10));

            chart.PrimaryXAxis.LabelRotate = true;
            chart.PrimaryXAxis.LabelRotateAngle = 270;

            chart.Series[0].Style.Border.Color = Color.Transparent;

            chart.Series[0].Style.Interior = new BrushInfo(Color.FromArgb(0xFF, 0x1B, 0xA1, 0xE2));
            if (chart.Series.Count > 1)
            {
                chart.Series[1].Style.Interior = new BrushInfo(Color.FromArgb(0xFF, 0xA0, 0x50, 0x00));
            }

            #endregion
        }
    }
}