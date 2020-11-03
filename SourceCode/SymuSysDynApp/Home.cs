#region Licence

// Description: SymuSysDyn - SymuSysDynApp
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Windows.Forms;
using Symu.SysDyn.Models;
using Symu.SysDyn.QuickGraph;
using Symu.SysDyn.Simulation;
using SymuSysDynApp.Graph;
using Syncfusion.Windows.Forms.Chart;

#endregion

namespace SymuSysDynApp
{
    public partial class Home : Form
    {
        private const string All = "-All";
        private const string Root = "--Root";
        private StateMachine _stateMachine;

        public Home()
        {
            InitializeComponent();
        }

        private void btnGlobalProcess_Click(object sender, EventArgs e)
        {
            Process(string.Empty);
        }

        private void btnSubModelProcess_Click(object sender, EventArgs e)
        {
            var modelName = cbModels.SelectedItem.ToString();
            switch (modelName)
            {
                case All:
                case Root:
                    modelName = string.Empty;
                    break;
            }
            Process(modelName);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model">the name of the subModel or emptyString</param>
        private void Process(string model)
        {
            _stateMachine.Optimized = cbOptimized.Checked;
            _stateMachine.Process(model);
            cbResults.Items.Clear();
            if (_stateMachine.Variables.FullNames != null)
            {
                cbResults.Items.AddRange(_stateMachine.Variables.FullNames.OrderBy(x => x).ToArray());
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

            cbModels.Items.Clear();
            if (_stateMachine.Models.Count()>1)
            {
                cbModels.Items.Add(All);
                cbModels.Items.Add(Root);
                cbModels.Items.AddRange(_stateMachine.Models.Where(x => !string.IsNullOrEmpty(x.Name)).Select(x => x.Name).OrderBy(x => x).ToArray());
                cbModels.Enabled = true;
                btnSubModelProcess.Enabled = true;
            }
            else
            {
                cbModels.Enabled = false;
                btnSubModelProcess.Enabled = false;
            }

            SetGlobalModels();

            lblTime.Text = "0";
            cbResults.SelectedText = string.Empty;
            btnClear.Enabled = false;
            cbOptimized.Checked = _stateMachine.Optimized;
            tbStart.Text = _stateMachine.Simulation.Start.ToString(CultureInfo.InvariantCulture);
            tbStop.Text = _stateMachine.Simulation.Stop.ToString(CultureInfo.InvariantCulture);
            tbDt.Text = _stateMachine.Simulation.DeltaTime.ToString(CultureInfo.InvariantCulture);
            tbPause.Text = _stateMachine.Simulation.PauseInterval.ToString(CultureInfo.InvariantCulture);
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
            switch (groupName)
            {
                case All:
                    SetGroup(null);
                    break;
                default:
                    var group = _stateMachine.Models.GetGroups().First(x => x.Name == groupName);
                    SetGroup(group);
                    break;
            }

            var dotString = GraphVizDot.GenerateDotString(groupName == All
                ? _stateMachine.GetGraph()
                : _stateMachine.GetGroupGraph(groupName));
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

        private void cbModels_SelectedIndexChanged(object sender, EventArgs e)
        {
            var modelName = cbModels.SelectedItem.ToString();
            switch (modelName)
            {
                case All:
                    SetGlobalModels();
                    break;
                case Root:
                    SetRootModel();
                    break;
                default:
                    SetSubModel(modelName);
                    break;
            }
        }

        private void SetGlobalModels()
        {
            SetModel(null);
            var dotString = GraphVizDot.GenerateDotString(_stateMachine.GetGraph());
            picImage.Image = GraphViz.RenderImage(dotString, "jpg");
        }

        private void SetRootModel()
        {
            SetModel(_stateMachine.Models.RootModel);
            var dotString = GraphVizDot.GenerateDotString(_stateMachine.GetRootModelGraph());
            picImage.Image = GraphViz.RenderImage(dotString, "jpg");
        }

        private void SetSubModel(string modelName)
        {
            SetModel(_stateMachine.Models.Get(modelName));
            var dotString = GraphVizDot.GenerateDotString(_stateMachine.GetSubModelGraph(modelName));
            picImage.Image = GraphViz.RenderImage(dotString, "jpg");
        }

        private void SetModel(Model model)
        {
            cbGroups.Items.Clear();
            cbVariables.Items.Clear();
            var groups = model == null ? _stateMachine.Models.GetGroups() : model.Groups;
            if (groups.Any())
            {
                cbGroups.Items.Add(All);
                cbGroups.Items.AddRange(groups.OrderBy(x => x.Name).ToArray());
                foreach (var group in groups)
                {
                    cbVariables.Items.AddRange(group.Entities.ToArray());
                }

                cbGroups.Enabled = true;
            }
            else
            {
                cbGroups.Enabled = false;
                if (model == null)
                {
                    cbVariables.Items.AddRange(_stateMachine.Models.GetVariables().ToArray());
                }
                else
                {
                    cbVariables.Items.AddRange(model.Variables.ToArray());
                }
            }

            btnSubModelProcess.Enabled = model != null;
        }

        private void SetGroup(Group group)
        {
            cbVariables.Items.Clear();
            if (group != null)
            {
                cbVariables.Items.AddRange(group.Entities.ToArray());
            }
            else
            {
                var modelName = cbModels.SelectedItem?.ToString();
                switch (modelName)
                {
                    case All:
                        cbVariables.Items.AddRange(_stateMachine.Models.GetVariables().ToArray());
                        break;
                    case null:
                    case Root:
                        cbVariables.Items.AddRange(_stateMachine.Models.RootModel.Variables.ToArray());
                        break;
                    default:
                        cbVariables.Items.AddRange(_stateMachine.Models[modelName].Variables.ToArray());
                        break;
                }
            }
        }
    }
}