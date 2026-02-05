using ChartsLib;
using DvmProtocolConsole;
using LabDataLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml;
using static ChartsLib.ChartAnimator;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

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
        private SettingsChart.AnnotationsMode annotationsMode;

        private bool graphicsExisting = false;
        private bool isAnimating = false;

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
            chartAnimator.AnimationFinished += ChartAnimator_AnimationFinished;

            chartManager.ConfigureAxes("Час", "Напруга", "мс", "В");
            chartManager.SetActiveAxisY(chart1.ChartAreas[0].AxisY);

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

            if (!interfaceHelper.TryGetPositiveDouble(textBox1, "R (кОм)", out r))
            {
                MessageBox.Show("Опір R повинен бути в межах (0; 1000] кОм");
                return;
            }
            if (!interfaceHelper.TryGetPositiveDouble(textBox2, "C (мкФ)", out c, 0, 10000))
            {
                MessageBox.Show("Ємність С повинна бути в межах (0; 10000] мкФ");
                return;
            }
            if (!interfaceHelper.TryGetPositiveDouble(textBox3, "U (В)", out u, 0, 100))
            {
                MessageBox.Show("Напруга U повинна бути в межах (0; 100] В");
                return;
            }
            if (!interfaceHelper.TryGetPositiveDouble(textBox4, "T (мс)", out T, 0, 10000))
            {
                MessageBox.Show("Період Т повинен бути в межах (0; 10000] мс");
                return;
            }
            if (!interfaceHelper.TryGetPositiveDouble(textBox5, "D (%)", out D, 0, 99))
            {
                MessageBox.Show("Коефіцієнт заповнення D повинен бути в межах (0; 99] %");
                return;
            }

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
            annotationsMode = SettingsChart.AnnotationsMode.ChargeDischargeModel;

            chartManager.DrawLineAnnotations(annotationsMode);
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
            checkBox1.Enabled = true;

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
                isAnimating = false;
                return;
            }

            isAnimating = true;
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
            graphicsExisting = false;

            {
                bool anyChecked = false;
                {
                    foreach (Control ctrl in groupBox5.Controls)
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

            if (!interfaceHelper.TryGetPositiveDouble(textBox10, "R (кОм)", out r))
            {
                MessageBox.Show("Опір R повинен бути в межах (0; 1000] кОм");
                return;
            }
            if (!interfaceHelper.TryGetPositiveDouble(textBox11, "C (мкФ)", out c, 0, 10000))
            {
                MessageBox.Show("Ємність С повинна бути в межах (0; 10000] мкФ");
                return;
            }
            if (!interfaceHelper.TryGetPositiveDouble(textBox9, "U (В)", out u, 0, 100))
            {
                MessageBox.Show("Напруга U повинна бути в межах (0; 100] В");
                return;
            }
            if (!interfaceHelper.TryGetPositiveDouble(textBox7, "T (мс)", out T, 0, 10000))
            {
                MessageBox.Show("Період Т повинен бути в межах (0; 10000] мс");
                return;
            }
            if (!interfaceHelper.TryGetPositiveDouble(textBox6, "D (%)", out D, 0, 99))
            {
                MessageBox.Show("Коефіцієнт заповнення D повинен бути в межах (0; 99] %");
                return;
            }

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
                annotationsMode = SettingsChart.AnnotationsMode.ChargeDischargeExpSingle;
            }
            else if (radioButton6.Checked)
            {
                GraphicsBuilder.BuildExperiment(uGenerator, ch2);
                annotationsMode = SettingsChart.AnnotationsMode.GeneratorSingle;
            }
            else if (radioButton4.Checked)
            {
                chartManager.ConfigureAxes("Час", "Напруга (вхід 1)", "мс", "В");
                chartManager.ConfigureAxisY2("Напруга (вхід 2)", "В");

                GraphicsBuilder.BuildExperiment(chargeAndDischarge, ch1, uGenerator, ch2);
                annotationsMode = SettingsChart.AnnotationsMode.ChargeDischargeGenerator;
            }

            chart1.Series.Add(chargeAndDischarge);
            chart1.Series.Add(uGenerator);

            chartManager.ResetAxes();

            chart1.ChartAreas[0].AxisY2.Maximum = 700;
            chart1.ChartAreas[0].AxisY2.Minimum = 0;
            chart1.ChartAreas[0].AxisY.Maximum = chart1.ChartAreas[0].AxisY2.Maximum;
            chart1.ChartAreas[0].AxisY.Minimum = chart1.ChartAreas[0].AxisY2.Minimum;

            chartManager.ApplyStaticGrid4x4Experiment();
            chartManager.SetStaticAnnotations();
            chartManager.DrawLineAnnotations(annotationsMode);

            graphicsExisting = true;

            DataWorking.AppendData(labNumber, DataWorking.DataKind.Experiment, parameters, "R", "C", "U", "T", "D");
            dataTable = DataWorking.LoadData(labNumber, DataWorking.DataKind.Experiment);
            GridHelper.AddExperimentNumberColumn(dataTable);
            dataGridView1.DataSource = dataTable;

            GridHelper.HideArrayColumns(dataGridView1);
            GridHelper.SetUpColumnHeaders(dataGridView1);

            string parametersValues = GridHelper.GetParametersToString(parameters, "R", "C", "U", "T", "D");
            GridHelper.SelectCurrentExperiment(dataGridView1, parametersValues);
        }

        #endregion

        #region --- Working with chart (scaling) ---

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
                if (isAnimating) return;

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

        #region --- Interface ---

        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            isAnimating = false;
            chartAnimator.StopAnimation();
            checkBox1.Checked = false;
            checkBox1.Enabled = false;
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var dataRow = dataGridView1.Rows[e.RowIndex];

            if (tabPageIndex == 0)
            {
                textBox1.Text = dataRow.Cells["R"].Value.ToString();
                textBox2.Text = dataRow.Cells["C"].Value.ToString();
                textBox3.Text = dataRow.Cells["U"].Value.ToString();
                textBox4.Text = dataRow.Cells["T"].Value.ToString();
                textBox5.Text = dataRow.Cells["D"].Value.ToString();
            }
            else if (tabPageIndex == 1)
            {
                textBox10.Text = dataRow.Cells["R"].Value.ToString();
                textBox11.Text = dataRow.Cells["C"].Value.ToString();
                textBox9.Text = dataRow.Cells["U"].Value.ToString();
                textBox7.Text = dataRow.Cells["T"].Value.ToString();
                textBox6.Text = dataRow.Cells["D"].Value.ToString();
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
            var instructionPath = Path.Combine(Application.StartupPath, "Labs instructions", "Інструкція до лабораторної роботи №1.pdf");
            Process.Start(new ProcessStartInfo
            {
                FileName = instructionPath,
                UseShellExecute = true
            });
        }
        
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            string sourcePath = Path.Combine(Application.StartupPath, "Labs instructions", "Інструкція до лабораторної роботи №1.docx");

            if (!File.Exists(sourcePath))
            {
                MessageBox.Show("Файл Word не знайдено", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Word document (*.docx)|*.docx";
                sfd.FileName = "Інструкція до лабораторної роботи №1.docx";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    File.Copy(sourcePath, sfd.FileName, true);
                    MessageBox.Show("Документ успішно збережено!", "Збережено", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        #endregion

        private void Lab_1_FormClosing(object sender, FormClosingEventArgs e)
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
