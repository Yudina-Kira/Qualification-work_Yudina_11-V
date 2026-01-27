using ChartsLib;
using DvmProtocolConsole;
using LabDataLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml;
using static ChartsLib.ChartAnimator;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace PhysicsLabsComplex
{
    public partial class Lab_1 : Form
    {
        private SettingsChart chartManager;
        private LabInterfaceHelper interfaceHelper;
        private ChartAnimator chartAnimator;

        private const int labNumber = 1;
        private int tabPageIndex = 0;

        private Series chargeAndDischarge, uGenerator;

        private double r, c, u, T, D;
        private double tau, uCharged, pulseDuration;

        private ChartMode currentChartMode = ChartMode.None;
        private GraphMode currentGraphMode;

        private bool graphicsExisting = false;

        private byte type;
        private byte[] payload;
        private ushort[] ch1;
        private ushort[] ch2;

        private readonly string basePath = AppDomain.CurrentDomain.BaseDirectory;
        DataTable dataTable;

        public Lab_1()
        {
            InitializeComponent();

            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            chartManager = new SettingsChart(chart1);
            interfaceHelper = new LabInterfaceHelper(toolTip1, this);

            chart1.Series.Clear();
            chargeAndDischarge = new Series();
            uGenerator = new Series();

            chartAnimator = new ChartAnimator(chargeAndDischarge, uGenerator);

            chartManager.ConfigureAxes("Час", "Напруга", 0, 0, "мс", "В");

            chart1.MouseWheel += ScaleChartByMouseWheel;

            toolTip1.SetToolTip(pictureBox1, "Ознайомтеся з інструкцією до лабораторної роботи та порядком її виконання");
            toolTip1.SetToolTip(chart1, "Затисніть Ctrl для розтягування по вертикалі,\r\nЗатисніть Shift для розтягування по горизонталі\r\n");

            CursorSetting.SetHandCursor(panel3);
            CursorSetting.SetHandCursor(pictureBox1);

            dataGridView1.ContextMenuStrip = contextMenuStrip1;
        }

        private void Lab_1_Load(object sender, EventArgs e)
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

            {
                bool anyChecked = false;
                {
                    foreach (Control ctrl in groupBox1.Controls)
                    {
                        if (ctrl is RadioButton rb && rb.Checked)
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
            u = double.Parse(textBox3.Text); //В
            T = double.Parse(textBox4.Text); //мс
            D = double.Parse(textBox5.Text); //%

            var parameters = new Dictionary<string, double>
            {
                { "R", r },
                { "C", c },
                { "U", u },
                { "T", T },
                { "D", D }
            };

            {
                Console.WriteLine("r = " + r + " кОм");
                Console.WriteLine("c = " + c + " мкФ");
                Console.WriteLine("u = " + u + " В");
                Console.WriteLine("T = " + T + " мс");
                Console.WriteLine("D = " + D + "%");

                Console.WriteLine("After normalising");

                //Normalising
                r = UnitsToSI.KiloToBase(r); //Ом
                c = UnitsToSI.MicroToBase(c); //Ф
                T = UnitsToSI.MilliToBase(T); //с
                D = UnitsToSI.PercentToFraction(D); //1

                Console.WriteLine("r = " + r + " Ом");
                Console.WriteLine("c = " + c + " Ф");
                Console.WriteLine("u = " + u + " В");
                Console.WriteLine("T = " + T + " с");
                Console.WriteLine("D = " + D);
            }

            //Consts
            tau = DataCalculating.Tau(r, c); // константа часу, наскільки швидко йде зарядка та розрядка
            uCharged = DataCalculating.UCharged(u); //максимальна напруга на конденсаторі
            pulseDuration = DataCalculating.PulseDuration(D, T); //протягом якого часу з періоду імпульсу буде текти струм

            {
                Console.WriteLine("Normal consts");
                Console.WriteLine($"tau = R * C = {r} Om * {c} F = {tau} seconds");
                Console.WriteLine($"uCharged = 0.99 * u = 0.99 * {u} = {uCharged} V");
                Console.WriteLine($"pulseDuration = D * T = {D} * {T} sec = {pulseDuration} seconds");
            }

            currentChartMode = ChartMode.Model;
            chartManager.DrawLineAnnotations(SettingsChart.AnnotationsMode.ChargeDischargeModel);
            interfaceHelper.SetSeriesSelector(comboBox1);

            chargeAndDischarge.Points.Clear();
            chartManager.ConfigureSeries(chargeAndDischarge, SettingsChart.SeriesMode.ChargeDischargeModel);
            chart1.Series.Clear();

            if (radioButton1.Checked)
            {
                GraphicsBuilder.BuildCharge(chargeAndDischarge, u, tau, pulseDuration);
                currentGraphMode = GraphMode.Charge;
            }
            else if (radioButton2.Checked)
            {
                GraphicsBuilder.BuildDischarge(chargeAndDischarge, u, tau, T - pulseDuration);
                currentGraphMode = GraphMode.Discharge;
            }
            else if (radioButton3.Checked)
            {
                GraphicsBuilder.BuildChargeDischarge(chargeAndDischarge, u, tau, T, D, T);
                currentGraphMode = GraphMode.ChargeDischarge;
            }

            chart1.Series.Add(chargeAndDischarge);
            ResetZoom();

            chart1.Invalidate();
            chart1.Update();

            graphicsExisting = true;

            DataWorking.AppendData(labNumber, DataWorking.DataKind.Model, parameters);
            dataTable = DataWorking.LoadData(labNumber, DataWorking.DataKind.Model);
            GridHelper.AddExperimentNumberColumn(dataTable);
            dataGridView1.DataSource = dataTable;

            GridHelper.HideArrayColumns(dataGridView1);
            GridHelper.SetUpColumnHeaders(dataGridView1);

            string parametersValues = GridHelper.GetModelParametersToString(parameters);
            GridHelper.SelectCurrentExperiment(dataGridView1, parametersValues);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox1.Checked)
            {
                chartAnimator.StopAnimation();
                return;
            }

            double currentXMax = chart1.ChartAreas[0].AxisX.Maximum;
            chartAnimator.ConfigureChargeDischarge(0.0005, currentXMax, u, tau, D, T);

            if (radioButton1.Checked)
                chartAnimator.StartAnimation(LabMode.ChargeDischarge, AnimationMode.Charge);
            
            if (radioButton2.Checked)
                chartAnimator.StartAnimation(LabMode.ChargeDischarge, AnimationMode.Discharge);
            
            if (radioButton3.Checked)
                chartAnimator.StartAnimation(LabMode.ChargeDischarge, AnimationMode.ChargeDischarge);
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
            //////////////////////
            {
                ch1 = new ushort[64];
                ch2 = new ushort[64];

                for (int i = 0; i < 64; i++)
                {
                    ch1[i] = (ushort)(i);
                    ch2[i] = (ushort)(i + 1);
                }
            }

            graphicsExisting = false;

            {
                bool anyChecked = false;
                {
                    foreach (Control ctrl in groupBox5.Controls)
                    {
                        if (ctrl is RadioButton rb && rb.Checked)
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

            r = double.Parse(textBox10.Text); //кОм
            c = double.Parse(textBox11.Text); //мкФ
            u = double.Parse(textBox9.Text); //В
            T = double.Parse(textBox7.Text); //мс
            D = double.Parse(textBox6.Text); //%

            var parameters = new Dictionary<string, string>
            {
                { "R", r.ToString(CultureInfo.InvariantCulture) },
                { "C", c.ToString(CultureInfo.InvariantCulture) },
                { "U", u.ToString(CultureInfo.InvariantCulture) },
                { "T", T.ToString(CultureInfo.InvariantCulture) },
                { "D", D.ToString(CultureInfo.InvariantCulture) },
                { "ch1", string.Join(" ", ch1) },
                { "ch2", string.Join(" ", ch2) }
            };

            currentChartMode = ChartMode.Experiment;
            interfaceHelper.SetSeriesSelector(comboBox1);

            chargeAndDischarge.Points.Clear();
            uGenerator.Points.Clear();
            chartManager.ConfigureSeries(chargeAndDischarge, SettingsChart.SeriesMode.ChargeDischargeExp);
            chartManager.ConfigureSeries(uGenerator, SettingsChart.SeriesMode.UGenerator);
            chart1.Series.Clear();

            if (radioButton5.Checked)
            {
                GraphicsBuilder.BuildExperiment(chargeAndDischarge, ch1);
                chartManager.DrawLineAnnotations(SettingsChart.AnnotationsMode.ChargeDischargeExpSingle);
            }
            else if (radioButton6.Checked)
            {
                GraphicsBuilder.BuildExperiment(uGenerator, ch2);
                chartManager.DrawLineAnnotations(SettingsChart.AnnotationsMode.GeneratorSingle);
            }
            else if (radioButton4.Checked)
            {
                chartManager.ConfigureAxes("Час", "Напруга", 0, 0, "мс", "В", "В");
                GraphicsBuilder.BuildExperiment(chargeAndDischarge, ch1, uGenerator, ch2);
                chartManager.DrawLineAnnotations(SettingsChart.AnnotationsMode.ChargeDischargeGenerator);
            }

            chart1.Series.Add(chargeAndDischarge);
            chart1.Series.Add(uGenerator);

            chartManager.ResetAxes();
            chartManager.ApplyStaticGrid();

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

        #region --- Working with chart ---

        private enum ChartMode
        {
            None,
            Model,
            Experiment
        }

        private enum GraphMode
        {
            Charge,
            Discharge,
            ChargeDischarge
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
            double xMax = GetInitialXMax(currentGraphMode);

            UpdateSeriesForAxisX(xMax);
            chartManager.SetAxisLimits(0, xMax, 0, u);
            chartManager.ApplyNiceGrid();
        }

        private void ScaleChartByMouseWheel(object sender, MouseEventArgs e)
        {
            if (!IsScalingAllowed())
                return;

            var chartArea = chart1.ChartAreas[0];

            if (Control.ModifierKeys == Keys.Control)
            {
                chartManager.ZoomAxisY(e.Delta);
            }
            else if (Control.ModifierKeys == Keys.Shift)
            {
                if (!radioButton3.Checked)
                    if (e.Delta < 0)
                        if (chartArea.AxisX.Maximum >= pulseDuration) return;

                chartManager.ZoomAxisX(e.Delta);
                UpdateSeriesForAxisX(chartArea.AxisX.Maximum);
            }
        }

        private double GetInitialXMax(GraphMode mode)
        {
            switch (mode)
            {
                case GraphMode.Charge:
                    return pulseDuration;

                case GraphMode.Discharge:
                    return T - pulseDuration;

                case GraphMode.ChargeDischarge:
                    return T;

                default:
                    throw new InvalidOperationException("Невідомий режим графіка");
            }
        }

        private void UpdateSeriesForAxisX(double newXMax)
        {
            chargeAndDischarge.Points.Clear();

            if (radioButton1.Checked)
            {
                GraphicsBuilder.BuildCharge(chargeAndDischarge, u, tau, newXMax);
            }
            else if (radioButton2.Checked)
            {
                GraphicsBuilder.BuildDischarge(chargeAndDischarge, u, tau, newXMax);
            }
            else if (radioButton3.Checked)
            {
                GraphicsBuilder.BuildChargeDischarge(chargeAndDischarge, u, tau, T, D, newXMax);
            }
        }

        private void chart1_DoubleClick(object sender, EventArgs e)
        {
            if (!graphicsExisting)
                return;

            if (!IsScalingAllowed())
                return;

            ResetZoom();
        }

        #endregion

        #region --- Interface ---

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var dataRow = dataGridView1.Rows[e.RowIndex];

            textBox1.Text = dataRow.Cells["R"].Value.ToString();
            textBox2.Text = dataRow.Cells["C"].Value.ToString();
            textBox3.Text = dataRow.Cells["U"].Value.ToString();
            textBox4.Text = dataRow.Cells["T"].Value.ToString();
            textBox5.Text = dataRow.Cells["D"].Value.ToString();
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
            chartManager.ClearChartArea();
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
            var labInstructions = new LabsInstructions();
            labInstructions.Show();
        }

        #endregion

        private void Lab_1_FormClosing(object sender, FormClosingEventArgs e)
        {
            var labsExitForm = new LabsExit(this);
            labsExitForm.ShowDialog();
            if (!LabsExit.closing)
            {
                e.Cancel = true;
            }
        }
    }
}
