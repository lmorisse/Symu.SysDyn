using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Symu.SysDyn;
using Symu.SysDyn.QuickGraph;
using SymuSysDynApp.Graph;
using Syncfusion.Windows.Forms.Chart;

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
            // implement _stateMachine.Simulation.DeltaTime
            for (var i = _stateMachine.Simulation.Start; i < _stateMachine.Simulation.Stop; i++)
            {
                _stateMachine.Process();
            }

            cbVariables.Enabled = true;
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
            cbVariables.Items.Clear();
            cbVariables.Items.AddRange(_stateMachine.GetVariables().ToArray());

            tbStart.Text = _stateMachine.Simulation.Start.ToString(CultureInfo.InvariantCulture);
            tbStop.Text = _stateMachine.Simulation.Stop.ToString(CultureInfo.InvariantCulture);
            tbDt.Text = _stateMachine.Simulation.DeltaTime.ToString(CultureInfo.InvariantCulture);
            var dotString = GraphVizDot.GenerateDotString(_stateMachine.GetGraph());
            pictureBox1.Image = GraphViz.RenderImage(dotString, "jpg");
        }

        private void cbVariables_SelectedIndexChanged(object sender, EventArgs e)
        {
            var variableName = cbVariables.SelectedItem.ToString();
            var items = _stateMachine.GetResults(variableName).ToArray();
            var chartSerie = new ChartSeries { Name = variableName };
            for (var i = 0; i < items.Count(); i++)
            {
                chartSerie.Points.Add(i, items[i]);
            }

            chartControl1.Series.Clear();
            chartControl1.Series.Add(chartSerie);

            //ChartAppearance.ApplyChartStyles(chartControl2);
        }

        private void tbStart_TextChanged(object sender, EventArgs e)
        {
            try
            {
                _stateMachine.Simulation.Start =
                    float.Parse(tbStart.Text, CultureInfo.InvariantCulture);
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
                    float.Parse(tbStop.Text, CultureInfo.InvariantCulture);
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

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
