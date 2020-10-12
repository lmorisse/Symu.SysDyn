using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Symu.SysDyn;
using Symu.SysDyn.QuickGraph;
using SymuSysDynApp.Graph;

namespace SymuSysDynApp
{
    public partial class Form1 : Form
    {
        private StateMachine stateMachine;
        public Form1()
        {
            InitializeComponent();
            string xml = @"C:\Users\laure\Dropbox\Symu\SourceCode\Symu.SysDyn\Github\SourceCode\SymuSysDyn\Templates\Fishmodel.stmx";
            //string xml = @"C:\Users\laure\Dropbox\Symu\SourceCode\Symu.SysDyn\Github\SourceCode\SymuSysDyn\Templates\Borneo.xmile";

            //Model object using location
            stateMachine = new StateMachine(xml, false);
        }

        private void button1_Click(object sender, EventArgs e)
        {

            //run 30 timesteps and print the number of fish at each time step.
            for (int counter = 0; counter < 25; counter++)
            {
                Variables.Items.Add("Iteration " + counter + "Fish = " + stateMachine.GetVariable("Fish"));
                stateMachine.Process();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var dotString = GraphVizDot.GenerateDotString(stateMachine.GetGraph());
            pictureBox1.Image = GraphViz.RenderImage(dotString, "jpg");
        }

    }
}
