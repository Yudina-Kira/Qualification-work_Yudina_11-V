using ChartsLib;
using DvmProtocolConsole;
using LabDataLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static ChartsLib.ChartAnimator;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;

namespace PhysicsLabsComplex
{
    public partial class Lab_2 : Form
    {
        private SettingsChart chartManager;
        private LabInterfaceHelper interfaceHelper;
        private ChartAnimator chartAnimator;

        private const int labNumber = 2;
        private int tabPageIndex = 0;

        private Series Ut, It;

        private double r, c, uMax, f;
        private double omega, iMax, x0Max, phi;

        private ChartMode currentChartMode = ChartMode.None;
        private SettingsChart.GraphicsMode currentGraphMode = SettingsChart.GraphicsMode.SingleUt;
        private SettingsChart.AnnotationsMode annotationsMode;

        private bool graphicsExisting = false;
        private bool isAnimating = false;

        private byte type;
        private byte[] payload;
        private ushort[] ch1;
        private ushort[] ch2;

        private readonly string basePath = AppDomain.CurrentDomain.BaseDirectory;
        DataTable dataTable;

        public Lab_2()
        {
            InitializeComponent();

            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            chartManager = new SettingsChart(chart1);
            interfaceHelper = new LabInterfaceHelper(toolTip1, this);

            chart1.Series.Clear();
            Ut = new Series();
            It = new Series();

            chartAnimator = new ChartAnimator(Ut, It);
            chartAnimator.AnimationFinished += ChartAnimator_AnimationFinished;

            chartManager.ConfigureAxes("Час", "Напруга", "мс", "В");
            chartManager.ConfigureAxisY2("Струм", "А");
            chartManager.ApplyAxisMode(currentGraphMode);

            Ut.YAxisType = AxisType.Primary;
            It.YAxisType = AxisType.Secondary;
            chart1.ChartAreas[0].AxisY.Enabled = AxisEnabled.False;

            chart1.MouseWheel += ScaleChartByMouseWheel;

            toolTip1.SetToolTip(pictureBox1, "Ознайомтеся з інструкцією до лабораторної роботи та порядком її виконання");
            toolTip1.SetToolTip(chart1, "Затисніть Ctrl для розтягування по вертикалі,\r\nЗатисніть Shift для розтягування по горизонталі\r\n");

            dataGridView1.ContextMenuStrip = contextMenuStrip1;

            //setting cursor on active elements
            {
                CursorSetting.SetHandCursor(panel3);
                CursorSetting.SetHandCursor(pictureBox1);
                CursorSetting.SetHandCursor(pictureBox2);
                CursorSetting.SetHandCursor(button1);
                CursorSetting.SetHandCursor(button2);
                CursorSetting.SetHandCursor(button3);
                CursorSetting.SetHandCursor(button4);
                CursorSetting.SetHandCursor(radioButton1);
                CursorSetting.SetHandCursor(radioButton2);
                CursorSetting.SetHandCursor(radioButton3);
                CursorSetting.SetHandCursor(radioButton4);
                CursorSetting.SetHandCursor(radioButton5);
                CursorSetting.SetHandCursor(radioButton6);
                CursorSetting.SetHandCursor(checkBox1);
                CursorSetting.SetHandCursor(comboBox1);
            }
        }

        private void Lab_2_Load(object sender, EventArgs e)
        {
            DataWorking.Initialize(basePath);

            var kind = GridHelper.GetTabKind(tabPageIndex);
            if (kind == DataWorking.DataKind.None) return;

            dataTable = DataWorking.LoadData(labNumber, kind);
            GridHelper.AddExperimentNumberColumn(dataTable);
            dataGridView1.DataSource = dataTable;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            GridHelper.SetUpColumnHeaders(dataGridView1);
            dataGridView1.ClearSelection();

            comboBox1.Visible = false;
        }

        #region --- Modeling page ---

        private void button1_Click(object sender, EventArgs e)
        {
            graphicsExisting = false;
            checkBox1.Enabled = true;

            {
                bool anyChecked = false;
                { 
                    foreach (Control ctrl in groupBox1.Controls)
                    {
                        if (ctrl is System.Windows.Forms.RadioButton rb && rb.Checked)
                        {
                            anyChecked = true;
                            break;
                        }
                    }

                    if (!anyChecked)
                    {
                        MessageBox.Show("Оберіть графік для побудови", "Помилка");
                        return;
                    }
                }
            }

            r = double.Parse(textBox1.Text); //кОм
            c = double.Parse(textBox2.Text); //мкФ
            uMax = double.Parse(textBox3.Text); //В
            f = double.Parse(textBox4.Text); //Гц

            var parameters = new Dictionary<string, double>
            {
                { "R", r },
                { "C", c },
                { "Umax", uMax },
                { "f", f },
            };

            {
                Console.WriteLine("r = " + r + " кОм");
                Console.WriteLine("c = " + c + " мкФ");
                Console.WriteLine("u = " + uMax + " В");
                Console.WriteLine("f = " + f + " Гц");

                Console.WriteLine("After normalising");

                //Normalising
                r = UnitsToSI.KiloToBase(r); //Ом
                c = UnitsToSI.MicroToBase(c); //Ф
                Console.WriteLine("r = " + r + " Ом");
                Console.WriteLine("c = " + c + " Ф");
            }
            
            //Consts
            omega = 2 * Math.PI * f; //кутова частота
            iMax = uMax / r; //за законом ома
            x0Max = 1 / f * 5; //кліькість періодів для початкової побудови
            phi = -(Math.Atan(omega * r * c));

            {
                Console.WriteLine("Normal consts");
                Console.WriteLine($"omega = 2 * pi * f = {omega} радіан");
                Console.WriteLine($"iMax = uMax / r = {iMax}");
                Console.WriteLine($"phi = {phi} рад = {phi * 180 / Math.PI}°");
            }

            currentChartMode = ChartMode.Model;

            Ut.Points.Clear();
            It.Points.Clear();
            chartManager.ConfigureSeries(Ut, SettingsChart.SeriesMode.UtModel);
            chartManager.ConfigureSeries(It, SettingsChart.SeriesMode.ItModel);
            chart1.Series.Clear();

            if (radioButton1.Checked)
            {
                GraphicsBuilder.BuildUtRC(Ut, uMax, omega, phi, x0Max);
                currentGraphMode = SettingsChart.GraphicsMode.SingleUt;
                chartManager.DrawLineAnnotations(SettingsChart.AnnotationsMode.SingleUt);
                interfaceHelper.SetSeriesSelector(comboBox1);
            }
            else if (radioButton2.Checked)
            {
                GraphicsBuilder.BuildItRC(It, iMax, omega, x0Max);
                currentGraphMode = SettingsChart.GraphicsMode.SingleIt;
                chartManager.DrawLineAnnotations(SettingsChart.AnnotationsMode.SingleIt);
                interfaceHelper.SetSeriesSelector(comboBox1);
            }
            else if (radioButton3.Checked)
            {
                GraphicsBuilder.BuildUtRC(Ut, uMax, omega, phi, x0Max);
                GraphicsBuilder.BuildItRC(It, iMax, omega, x0Max);

                currentGraphMode = SettingsChart.GraphicsMode.UtIt;

                chartManager.DrawLineAnnotations(SettingsChart.AnnotationsMode.UtIt);
                interfaceHelper.SetSeriesSelector(comboBox1, new string[] { "IN1", "IN2" });
                comboBox1.SelectedIndex = -1;
            }

            chart1.Series.Add(Ut);
            chart1.Series.Add(It);

            chartManager.ApplyAxisMode(currentGraphMode);
            ResetZoom();

            chart1.Invalidate();
            chart1.Update();

            graphicsExisting = true;

            DataWorking.AppendData(labNumber, DataWorking.DataKind.Model, parameters);
            dataTable = DataWorking.LoadData(labNumber, DataWorking.DataKind.Model);
            GridHelper.AddExperimentNumberColumn(dataTable);
            dataGridView1.DataSource = dataTable;

            GridHelper.SetUpColumnHeaders(dataGridView1);

            string parametersValues = GridHelper.GetModelParametersToString(parameters);
            GridHelper.SelectCurrentExperiment(dataGridView1, parametersValues);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox1.Checked)
            {
                chartAnimator.StopAnimation();
                isAnimating = false;
                return;
            }

            isAnimating = true;
            double currentXMax = chart1.ChartAreas[0].AxisX.Maximum;
            chartAnimator.Configure(0.00025, currentXMax, uMax, iMax, omega, phi);

            if (radioButton1.Checked)
                chartAnimator.StartAnimation(LabMode.RC, AnimationMode.Ut);

            if (radioButton2.Checked)
                chartAnimator.StartAnimation(LabMode.RC, AnimationMode.It);

            if (radioButton3.Checked)
                chartAnimator.StartAnimation(LabMode.RC, AnimationMode.UtIt);
        }

        #endregion

        #region --- Experiment page ---

        // Checking connection 
        private void button2_Click(object sender, EventArgs e)
        {
            var portName = "COM5";

            Console.WriteLine($"Відкриваю порт {portName}...");

            interfaceHelper.SetIndicator(connectStatus, Color.Gold, "Очікування з'єднання...");

            using (var dvm = new DvmProtocol())
            {
                try
                {
                    dvm.Open(portName);
                    Console.WriteLine("Порт відкрито.");

                    interfaceHelper.SetIndicator(connectStatus, Color.LimeGreen, "З'єднання встановлено!");

                    bool sent = dvm.SendSimpleCmd(0x84);
                    if (!sent)
                    {
                        Console.WriteLine("Не вдалося відправити команду.");

                        interfaceHelper.SetIndicator(connectStatus, Color.Red, "Пристрій не відповідає");
                        interfaceHelper.ShowTimedTooltip(connectStatus, "Не вдалося відправити команду");

                        button3.Enabled = false;

                        return;
                    }

                    Console.WriteLine("Команда відправлена. Очікую відповіді...");

                    bool run = true;
                    while (run)
                    {
                        bool ok = dvm.ReadFrame(out type, out payload);

                        if (!ok)
                        {
                            Thread.Sleep(100);
                            continue;
                        }

                        Console.WriteLine("\n=== Відповідь отримано ===");
                        Console.WriteLine("Тип: 0x" + type.ToString("X2"));
                        Console.WriteLine("Розмір payload: " + (payload != null ? payload.Length : 0));
                        if (payload != null && payload.Length > 0)
                        {
                            Console.WriteLine("Дані (hex): " + BitConverter.ToString(payload));
                        }
                        else
                        {
                            Console.WriteLine("Payload порожній.");
                        }

                        Console.WriteLine("====================\n");

                        interfaceHelper.SetIndicator(connectStatus, Color.LimeGreen, "Пристрій відповідає нормально ✔️");
                        interfaceHelper.ShowTimedTooltip(connectStatus, "Давай розпочинати працювати!");

                        run = false;

                        button3.Enabled = true;

                        dvm.Close();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Помилка: " + ex.Message);

                    interfaceHelper.SetIndicator(connectStatus, Color.Red, "Помилка з'єднання ❌");
                    interfaceHelper.ShowTimedTooltip(connectStatus, "Сталась неочікувана помилка");

                    button3.Enabled = false;
                }
            }
        }

        // Data receiving and processing
        private void button3_Click(object sender, EventArgs e)
        {
            using (var dvm = new DvmProtocol("COM5"))
            {
                if (dvm.StartSingleAndWaitForData(dvm, out var payload))
                {
                    Console.WriteLine("Отримав DATA!");
                    Console.WriteLine($"Payload size: {payload.Length}");
                    dvm.SeparateChannels(payload, out ch1, out ch2);
                    button4.Enabled = true;
                    interfaceHelper.SetIndicator(dataStatus, Color.LimeGreen, "Дані отримані, готові до обробки ✔️");
                    interfaceHelper.ShowTimedTooltip(dataStatus, "Дані отримані, готові до обробки ✔️");

                }
                else
                {
                    Console.WriteLine("Не вдалось отримати дані 0x10.");
                    button4.Enabled = false;
                    interfaceHelper.SetIndicator(dataStatus, Color.Red, "Помилка отримання даних ❌");
                    interfaceHelper.ShowTimedTooltip(dataStatus, "Не вдалось отримати дані ❌");
                }

                dvm.Close();
            }
        }

        // Graph building
        private void button4_Click(object sender, EventArgs e)
        {
            graphicsExisting = false;

            {
                bool anyChecked = false;
                {
                    foreach (Control ctrl in groupBox4.Controls)
                    {
                        if (ctrl is System.Windows.Forms.RadioButton rb && rb.Checked)
                        {
                            anyChecked = true;
                            break;
                        }
                    }

                    if (!anyChecked)
                    {
                        MessageBox.Show("Оберіть графік для побудови", "Помилка");
                        return;
                    }
                }
            }

            r = double.Parse(textBox8.Text); //кОм
            c = double.Parse(textBox9.Text); //мкФ
            uMax = double.Parse(textBox7.Text); //В
            f = double.Parse(textBox6.Text); //Гц

            var parameters = new Dictionary<string, string>
            {
                { "R", r.ToString(CultureInfo.InvariantCulture) },
                { "C", c.ToString(CultureInfo.InvariantCulture) },
                { "Umax", uMax.ToString(CultureInfo.InvariantCulture) },
                { "f", f.ToString(CultureInfo.InvariantCulture) },
                { "ch1", string.Join(" ", ch1) },
                { "ch2", string.Join(" ", ch2) }
            };

            currentChartMode = ChartMode.Experiment;
            interfaceHelper.SetSeriesSelector(comboBox1);

            Ut.Points.Clear();
            It.Points.Clear();
            chartManager.ConfigureSeries(Ut, SettingsChart.SeriesMode.UtExp);
            chartManager.ConfigureSeries(It, SettingsChart.SeriesMode.ItExp);
            chartManager.ApplyAxisMode(SettingsChart.GraphicsMode.UtIt);
            chart1.Series.Clear();

            if (radioButton5.Checked)
            {
                GraphicsBuilder.BuildExperiment(Ut, ch1);
                annotationsMode = SettingsChart.AnnotationsMode.SingleUt;
            }
            else if (radioButton6.Checked)
            {
                GraphicsBuilder.BuildExperiment(It, ch2);
                annotationsMode = SettingsChart.AnnotationsMode.SingleIt;
                chartManager.ApplyAxisMode(SettingsChart.GraphicsMode.SingleIt);
            }
            else if (radioButton4.Checked)
            {
                chartManager.ConfigureAxes("Час", "Напруга (вхід 1)", "мс", "В");
                chartManager.ConfigureAxisY2("Напруга (вхід 2)", "В");

                GraphicsBuilder.BuildExperiment(Ut, ch1, It, ch2);
                annotationsMode = SettingsChart.AnnotationsMode.UtIt;
            }

            chart1.Series.Add(Ut);
            chart1.Series.Add(It);

            chartManager.ResetAxes();
            chartManager.ApplyStaticGrid4x4Experiment();
            chartManager.DrawLineAnnotations(annotationsMode);

            graphicsExisting = true;

            DataWorking.AppendData(labNumber, DataWorking.DataKind.Experiment, parameters);
            dataTable = DataWorking.LoadData(labNumber, DataWorking.DataKind.Experiment);
            GridHelper.AddExperimentNumberColumn(dataTable);
            dataGridView1.DataSource = dataTable;

            GridHelper.HideArrayColumns(dataGridView1);
            GridHelper.SetUpColumnHeaders(dataGridView1);

            string parametersValues = GridHelper.GetExperimentParametersToString(parameters);
            GridHelper.SelectCurrentExperiment(dataGridView1, parametersValues);
        }

        #endregion

        #region --- Work with chart --- 

        private enum ChartMode
        {
            None,
            Model,
            Experiment
        }

        private bool IsScalingAllowed()
        {
            if (!graphicsExisting)
                return false;

            if (currentChartMode == ChartMode.Experiment)
                return false;

            bool allEmpty = true;
            foreach (Series series in chart1.Series)
            {
                if (series.Points.Count != 0)
                {
                    allEmpty = false;
                    break;
                }
            }

            if (allEmpty)
                return false;

            return true;
        }

        private void ResetZoom()
        {
            double cycles = 5;
            double xMax = (1 / f) * cycles;

            double y = uMax + 10;
            double y2 = iMax + 0.002;

            chartManager.SetAxisLimits(0, xMax, -y, y, -y2, y2);

            if (currentGraphMode == SettingsChart.GraphicsMode.UtIt)
                chartManager.ApplyStaticGridY(5);
            else
                chartManager.ApplyNiceGrid();

            UpdateSeriesForAxisX(xMax);
        }

        private void ScaleChartByMouseWheel(object sender, MouseEventArgs e)
        {
            if (!IsScalingAllowed())
                return;

            var chartArea = chart1.ChartAreas[0];

            if (Control.ModifierKeys == Keys.Control)
            {
                if (currentGraphMode == SettingsChart.GraphicsMode.UtIt)
                {
                    if (comboBox1.SelectedItem == null) return;

                    if (comboBox1.SelectedItem.ToString() == "IN1")
                        chartManager.SetActiveAxisY(chartArea.AxisY);

                    if (comboBox1.SelectedItem.ToString() == "IN2")
                        chartManager.SetActiveAxisY(chartArea.AxisY2);

                    chartManager.ZoomActiveAxisYSharp(e.Delta);
                }
                else
                {
                    chartManager.ZoomActiveAxisY(e.Delta);
                }
            }
            else if (Control.ModifierKeys == Keys.Shift)
            {
                if (isAnimating) return;

                chartManager.ZoomAxisX(e.Delta);
                UpdateSeriesForAxisX(chartArea.AxisX.Maximum);
            }
        }

        private void UpdateSeriesForAxisX(double newXMax)
        {
            Ut.Points.Clear();
            It.Points.Clear();

            if (radioButton1.Checked)
            {
                GraphicsBuilder.BuildUtRC(Ut, uMax, omega, phi, newXMax);
            }
            else if (radioButton2.Checked)
            {
                GraphicsBuilder.BuildItRC(It, iMax, omega, newXMax);
            }
            else if (radioButton3.Checked)
            {
                GraphicsBuilder.BuildUtRC(Ut, uMax, omega, phi, newXMax);
                GraphicsBuilder.BuildItRC(It, iMax, omega, newXMax);
            }
        }

        private void ChartAnimator_AnimationFinished(object sender, EventArgs e)
        {
            isAnimating = false;
            checkBox1.Checked = false;
        }

        private void chart1_DoubleClick(object sender, EventArgs e)
        {
            if (!IsScalingAllowed())
                return;

            if (isAnimating)
                return;

            ResetZoom();
        }

        #endregion

        #region --- Form's elements' events ---

        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            isAnimating = false;
            chartAnimator.StopAnimation();
            checkBox1.Checked = false;
            checkBox1.Enabled = false;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var area = chart1.ChartAreas[0];

            if (comboBox1.SelectedItem == null)
            {
                chartManager.SetActiveAxisY(null);
                return;
            }

            switch (comboBox1.SelectedItem.ToString())
            {
                case "IN1":
                    chartManager.SetActiveAxisY(area.AxisY);
                    break;

                case "IN2":
                    chartManager.SetActiveAxisY(area.AxisY2);
                    break;

                default:
                    chartManager.SetActiveAxisY(null);
                    break;
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var dataRow = dataGridView1.Rows[e.RowIndex];

            if (tabPageIndex == 0)
            {
                textBox1.Text = dataRow.Cells["R"].Value.ToString();
                textBox2.Text = dataRow.Cells["C"].Value.ToString();
                textBox3.Text = dataRow.Cells["Umax"].Value.ToString();
                textBox4.Text = dataRow.Cells["f"].Value.ToString();
            }
            else if (tabPageIndex == 1)
            {
                textBox8.Text = dataRow.Cells["R"].Value.ToString();
                textBox9.Text = dataRow.Cells["C"].Value.ToString();
                textBox7.Text = dataRow.Cells["Umax"].Value.ToString();
                textBox6.Text = dataRow.Cells["f"].Value.ToString();
            }
        }

        private void видалитиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null) return;

            var result = MessageBox.Show("Ви точно хочете видалити обрані параметри моделювання?\n Цю дію не можна буде відмінити.", "Видалення параметрів", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                int rowIndex = dataGridView1.CurrentRow.Index;
                dataTable.Rows[rowIndex].Delete();
                dataTable.AcceptChanges();

                if (dataTable.Rows.Count == 0)
                {
                    DataWorking.DataKind currentKind = GridHelper.GetTabKind(tabPageIndex);
                    DataWorking.DeleteFile(labNumber, currentKind);

                    dataGridView1.DataSource = null;
                    dataGridView1.Rows.Clear();
                    dataGridView1.Columns.Clear();

                    MessageBox.Show(
                        "Всі записи про параметри моделювання видалено",
                        "Видалення успішне",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    return;
                }
                else
                {
                    List<string> newLines = GridHelper.ToFileLines(dataTable);

                    DataWorking.DataKind currentKind = GridHelper.GetTabKind(tabPageIndex);
                    DataWorking.SaveFile(labNumber, currentKind, newLines);

                    MessageBox.Show("Інформацію про параметри моделювання видалено", "Видалення успішне", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void видалитиВсіЗаписиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Ви точно хочете видалити всі записи про параметри моделювань?\n Цю дію не можна буде відмінити.", "Видалення всіх записів", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                DataWorking.DataKind currentKind = GridHelper.GetTabKind(tabPageIndex);
                DataWorking.DeleteFile(labNumber, currentKind);

                dataTable.Clear();
                dataGridView1.DataSource = null;
                dataGridView1.Rows.Clear();
                dataGridView1.Columns.Clear();

                MessageBox.Show("Всі записи про параметри моделювань видалено", "Видалення успішне", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            chartAnimator.StopAnimation();
            isAnimating = false;

            chartManager.ClearChartArea();
            chart1.ChartAreas[0].AxisY.Enabled = AxisEnabled.False;
            chart1.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            chart1.ChartAreas[0].AxisY.MinorGrid.Enabled = false;
            chart1.ChartAreas[0].AxisY2.MajorGrid.Enabled = false;
            chart1.ChartAreas[0].AxisY2.MinorGrid.Enabled = false;
            comboBox1.Visible = false;

            graphicsExisting = false;

            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();

            tabPageIndex = tabControl1.SelectedIndex;

            var kind = GridHelper.GetTabKind(tabPageIndex);
            if (kind == DataWorking.DataKind.None) return;

            dataTable = DataWorking.LoadData(labNumber, kind);
            GridHelper.AddExperimentNumberColumn(dataTable);
            dataGridView1.DataSource = dataTable;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            if (tabPageIndex == 0)
            {
                toolTip1.SetToolTip(pictureBox1, "Ознайомтеся з інструкцією до лабораторної роботи та порядком її виконання");
                toolTip1.SetToolTip(chart1, "Затисніть Ctrl для розтягування по вертикалі,\r\nЗатисніть Shift для розтягування по горизонталі\r\n");
            }

            if (tabPageIndex == 1)
            {
                GridHelper.HideArrayColumns(dataGridView1);
                toolTip1.SetToolTip(chart1, null);
            }

            GridHelper.SetUpColumnHeaders(dataGridView1);
        }

        private void panel3_Click(object sender, EventArgs e)
        {
            var bigImg = new ImageView(panel2.BackgroundImage);
            bigImg.Show();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            var instructionPath = Path.Combine(Application.StartupPath, "Labs instructions", "Інструкція до лабораторної роботи №2.pdf");
            Process.Start(new ProcessStartInfo
            {
                FileName = instructionPath,
                UseShellExecute = true
            });
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            string sourcePath = Path.Combine(Application.StartupPath, "Labs instructions", "Інструкція до лабораторної роботи №2.docx");

            if (!File.Exists(sourcePath))
            {
                MessageBox.Show("Файл Word не знайдено", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Word document (*.docx)|*.docx";
                sfd.FileName = "Інструкція до лабораторної роботи №2.docx";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    File.Copy(sourcePath, sfd.FileName, true);
                    MessageBox.Show("Документ успішно збережено!", "Збережено", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        #endregion

        private void Lab_2_FormClosing(object sender, FormClosingEventArgs e)
        {
            chartAnimator.StopAnimation();
            var labsExitForm = new LabsExit(this);
            labsExitForm.ShowDialog();
            if (!LabsExit.closing)
            {
                e.Cancel = true;
            }
        }
    }
}
