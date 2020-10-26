#region Licence

// Description: SymuSysDyn - SymuSysDynApp
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Symu.SysDyn.QuickGraph;
using Symu.SysDyn.Simulation;
using SymuSysDynApp.Graph;
using Syncfusion.Windows.Forms.Chart;

#endregion

namespace SymuSysDynApp
{
    public partial class Home : Form
    {
        private StateMachine _stateMachine;

        public Home()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _stateMachine.Optimized = cbOptimized.Checked;
            _stateMachine.Process();
            cbResults.Items.Clear();
            if (_stateMachine.Variables.Names != null)
            {
                cbResults.Items.AddRange(_stateMachine.Variables.Names.ToArray());
            }
            cbResults.Enabled = true;
            btnClear.Enabled = true;
            lblTime.Text = _stateMachine.Simulation.Time.ToString(CultureInfo.InvariantCulture);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                InitializeStateMachine();
            }
        }

        private void InitializeStateMachine()
        {
            _stateMachine = new StateMachine(openFileDialog1.FileName, false);

            cbGroups.Items.Clear();
            if (_stateMachine.ReferenceVariables.Groups.Any())
            {
                cbGroups.Items.AddRange(_stateMachine.ReferenceVariables.Groups.ToArray());
            }

            cbVariables.Items.Clear();
            if (_stateMachine.ReferenceVariables.Any())
            {
                cbVariables.Items.AddRange(_stateMachine.ReferenceVariables.ToArray());
            }

            cbOptimized.Checked = _stateMachine.Optimized;
            tbStart.Text = _stateMachine.Simulation.Start.ToString(CultureInfo.InvariantCulture);
            tbStop.Text = _stateMachine.Simulation.Stop.ToString(CultureInfo.InvariantCulture);
            tbDt.Text = _stateMachine.Simulation.DeltaTime.ToString(CultureInfo.InvariantCulture);
            tbPause.Text = _stateMachine.Simulation.PauseInterval.ToString(CultureInfo.InvariantCulture);
            var dotString = GraphVizDot.GenerateDotString(_stateMachine.GetGraph());
            picImage.Image = GraphViz.RenderImage(dotString, "jpg");
        }

        private void cbVariables_SelectedIndexChanged(object sender, EventArgs e)
        {
            var variableName = cbResults.SelectedItem.ToString();
            var items = _stateMachine.Results.GetResults(variableName).ToArray();
            var chartSerie = new ChartSeries {Name = variableName};
            for (var i = 0; i < items.Length; i++)
            {
                chartSerie.Points.Add(i, items[i]);
            }

            chartControl1.Series.Clear();
            chartControl1.Series.Add(chartSerie);
            chartControl1.Text = variableName;
            //ChartAppearance.ApplyChartStyles(chartControl2);
        }

        private void tbPause_TextChanged(object sender, EventArgs e)
        {
            try
            {
                _stateMachine.Simulation.PauseInterval =
                    ushort.Parse(tbPause.Text, CultureInfo.InvariantCulture);
                tbPause.BackColor = SystemColors.Window;
            }
            catch (FormatException)
            {
                tbPause.BackColor = Color.Red;
            }
            catch (ArgumentOutOfRangeException)
            {
                tbPause.BackColor = Color.Red;
            }
        }

        private void tbStart_TextChanged(object sender, EventArgs e)
        {
            try
            {
                _stateMachine.Simulation.Start =
                    ushort.Parse(tbStart.Text, CultureInfo.InvariantCulture);
                tbStart.BackColor = SystemColors.Window;
            }
            catch (FormatException)
            {
                tbStart.BackColor = Color.Red;
            }
        }

        private void tbStop_TextChanged(object sender, EventArgs e)
        {
            try
            {
                _stateMachine.Simulation.Stop =
                    ushort.Parse(tbStop.Text, CultureInfo.InvariantCulture);
                tbStop.BackColor = SystemColors.Window;
            }
            catch (FormatException)
            {
                tbStop.BackColor = Color.Red;
            }
        }

        private void tbDt_TextChanged(object sender, EventArgs e)
        {
            try
            {
                _stateMachine.Simulation.DeltaTime =
                    float.Parse(tbDt.Text, CultureInfo.InvariantCulture);
                tbDt.BackColor = SystemColors.Window;
            }
            catch (FormatException)
            {
                tbDt.BackColor = Color.Red;
            }
        }

        private void cbGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            var groupName = cbGroups.SelectedItem.ToString();
            var dotString = GraphVizDot.GenerateDotString(_stateMachine.GetSubGraph(groupName));
            picImage.Image = GraphViz.RenderImage(dotString, "jpg");
        }

        private void cbVariables_SelectedIndexChanged_1(object sender, EventArgs e)
        {

            var variableName = cbVariables.SelectedItem.ToString();
            var variable = _stateMachine.ReferenceVariables.Get(variableName);
            tbEquation.Text = variable.Equation != null ? variable.Equation.ToString() : string.Empty;
            tbValue.Text = variable.Value.ToString(CultureInfo.InvariantCulture);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            _stateMachine.Clear();
            lblTime.Text = "0";
            cbResults.SelectedText = string.Empty;
            btnClear.Enabled = false;
        }
    }
}