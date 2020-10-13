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
        private readonly StateMachine _stateMachine;
        public Home()
        {
            InitializeComponent();
            const string xml = @"C:\Users\laure\Dropbox\Symu\SourceCode\Symu.SysDyn\Github\SourceCode\SymuSysDyn\Templates\Fishmodel.stmx";
            //string xml = @"C:\Users\laure\Dropbox\Symu\SourceCode\Symu.SysDyn\Github\SourceCode\SymuSysDyn\Templates\Borneo.xmile";

            //Model object using location
            _stateMachine = new StateMachine(xml, false);

            cbVariables.Items.AddRange(_stateMachine.GetVariables().ToArray());
        }

        private void button1_Click(object sender, EventArgs e)
        {

            //run 30 timesteps and print the number of fish at each time step.
            for (var counter = 0; counter < 25; counter++)
            {
                _stateMachine.Process();
            }

            cbVariables.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
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
    }
}
