using ChartsLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace PhysicsLabsComplex
{
    public partial class Lab_3 : Form
    {
        private SettingsChart chartManager;
        private LabInterfaceHelper interfaceHelper;

        private const int labNumber = 3;
        private int tabPageIndex = 0;

        private Series Ut, It;
        //private Axis activeAxisY;

        private double r, c, uMax, f;
        private double omega, iMax, x0Max;

        private double stepGr;

        private bool graphicsExisting = false;

        private byte type;
        private byte[] payload;
        private ushort[] ch1;
        private ushort[] ch2;

        private readonly string basePath = AppDomain.CurrentDomain.BaseDirectory;
        DataTable dataTable;

        #region -- Graphics Animations --

        Timer animTimer = new Timer();

        int utIndex = 0;
        int itIndex = 0;
        int uitIndex = 0;
        double dt = 0.0025;

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                if (radioButton1.Checked) StartUtAnimation();
                if (radioButton2.Checked) StartItAnimation();
                if (radioButton3.Checked) StartUItAnimation();
            }
            else StopAnyAnimation();
        }

        private void StartUtAnimation()
        {
            Ut.Points.Clear();
            utIndex = 0;
            animTimer.Start();
        }

        private void StartItAnimation()
        {
            It.Points.Clear();
            itIndex = 0;
            animTimer.Start();
        }

        private void StartUItAnimation()
        {
            Ut.Points.Clear();
            It.Points.Clear();
            uitIndex = 0;
            animTimer.Start();
        }

        private void StopAnyAnimation()
        {
            animTimer.Stop();
        }

        private void AnimateGraphics(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                double t = utIndex * dt;
                double U = uMax * Math.Sin(omega * t);

                Ut.Points.AddXY(t, U);

                utIndex++;

                if (utIndex > 1000) animTimer.Stop();
            }
            if (radioButton2.Checked)
            {
                stepGr = Math.Abs(uMax * Math.Sin(-(Math.PI / 2)) - uMax * Math.Sin(omega * dt - (Math.PI / 2)));

                chart1.ChartAreas[0].AxisX.Interval = stepGr;

                double t = itIndex * dt;
                double I = iMax * Math.Sin(omega * t - (Math.PI / 2));

                It.Points.AddXY(t, I);

                itIndex++;

                if (itIndex > 1000) animTimer.Stop();
            }
            if (radioButton3.Checked)
            {
                double t = uitIndex * dt;
                double U = uMax * Math.Sin(omega * t);
                double I = iMax * Math.Sin(omega * t - (Math.PI / 2));

                Ut.Points.AddXY(t, U);
                It.Points.AddXY(t, I);

                uitIndex++;

                if (uitIndex > 1000) animTimer.Stop();
            }
        }

        #endregion

        public Lab_3()
        {
            InitializeComponent();
            
            chartManager = new SettingsChart(chart1);
            interfaceHelper = new LabInterfaceHelper(toolTip1, this);

            chart1.Series.Clear();
            Ut = new Series();
            It = new Series();

            chart1.MouseWheel += ScaleChartByMouseWheel;

            toolTip1.SetToolTip(pictureBox1, "Ознайомтеся з інструкцією до лабораторної роботи та порядком її виконання");

            animTimer.Interval = 20;
            animTimer.Tick += AnimateGraphics;

            //CursorSetting.SetHandCursor(panel3);
            CursorSetting.SetHandCursor(pictureBox1);

            //dataGridView1.ContextMenuStrip = contextMenuStrip1;
        }

        private void Lab_3_Load(object sender, EventArgs e)
        {

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

            r = double.Parse(textBox1.Text); //Ом
            c = double.Parse(textBox2.Text); //мкФ
            uMax = double.Parse(textBox3.Text); //В
            f = double.Parse(textBox4.Text); //Гц

            {
                Console.WriteLine("r = " + r + " Ом");
                Console.WriteLine("c = " + c + " мкФ");
                Console.WriteLine("u = " + uMax + " В");
                Console.WriteLine("f = " + f + " Гц");

                Console.WriteLine("After normalising");

                //Normalising
                c = UnitsToSI.MicroToBase(c); //Ф
                Console.WriteLine("c = " + c + " Ф");
            }

            omega = 2 * Math.PI * f; //кутова частота
            iMax = uMax / r; //за законом ома
            x0Max = 1 / f * 5; //кліькість періодів для початкової побудови

            {
                Console.WriteLine("Normal consts");
                Console.WriteLine($"omega = 2 * pi * f = {omega} радіан");
                Console.WriteLine($"iMax = uMax / r = {iMax}");
            }

            Ut.Points.Clear();
            It.Points.Clear();
            chartManager.ConfigureSeries(Ut, SettingsChart.SeriesMode.UtModel);
            chartManager.ConfigureSeries(It, SettingsChart.SeriesMode.ItModel);
            chart1.Series.Clear();

            if (radioButton1.Checked)
            {
                chartManager.ConfigureAxes("Час", "Напруга", 0, -uMax, "мс", "В");
                chartManager.DrawLineAnnotations(SettingsChart.AnnotationsMode.SingleUt);
                //interfaceHelper.SetOneOption(comboBox1, label24, "Y:");
                GraphicsBuilder.BuildUtRL(Ut, uMax, omega, x0Max);
                Console.WriteLine(string.Join(" ", Ut.Points));
            }
            else if (radioButton2.Checked)
            {
                chartManager.ConfigureAxes("Час", "Струм", 0, -uMax, "мс", "В");
                chartManager.DrawLineAnnotations(SettingsChart.AnnotationsMode.SingleIt);
                //interfaceHelper.SetOneOption(comboBox1, label24, "Y:");
                GraphicsBuilder.BuildItRL(It, iMax, omega, x0Max);
            }
            else if (radioButton3.Checked)
            {
                var area = chart1.ChartAreas[0];
                area.AxisY.Minimum = -uMax;
                area.AxisY.Maximum = uMax;

                chartManager.ConfigureAxes("Час", "Струм", 0, -uMax, "мс", "В", "А");
                chartManager.DrawLineAnnotations(SettingsChart.AnnotationsMode.UtIt);
                //interfaceHelper.SetTwoOptions(comboBox1, label24, new string[] { "U(t)", "I(t)" });
                GraphicsBuilder.BuildUtRL(Ut, uMax, omega, x0Max);
                GraphicsBuilder.BuildItRL(It, iMax, omega, x0Max);
            }

            chart1.Series.Add(Ut);
            chart1.Series.Add(It);
            ResetZoom();
            chartManager.CenterAxisX();

            chart1.Invalidate();
            chart1.Update();

            graphicsExisting = true;


        }

        #endregion

        #region --- Experiment page ---

        #endregion

        #region --- Work with data base ---

        #endregion

        #region --- Work with chart --- 

        private void ResetZoom()
        {
            double cycles = 5;
            double xMax = (1 / f) * cycles;

            chartManager.SetAxisLimits(0, xMax, -uMax, uMax);
            chartManager.ApplyNiceGrid();
            chartManager.CenterAxisX();
            UpdateSeriesForAxisX(xMax);

            //ApplySeriesMode();
        }

        private void ScaleChartByMouseWheel(object sender, MouseEventArgs e)
        {
            var chartArea = chart1.ChartAreas[0];
            double factor = 0.1;

            if (Control.ModifierKeys == Keys.Control)
            {
                if (e.Delta < 0) chartArea.AxisY.Maximum += chartArea.AxisY.Maximum * factor;
                else chartArea.AxisY.Maximum -= chartArea.AxisY.Maximum * factor;
            }
            else if (Control.ModifierKeys == Keys.Shift)
            {
                if (e.Delta < 0) chartArea.AxisX.Maximum += chartArea.AxisX.Maximum * factor;
                else chartArea.AxisX.Maximum -= chartArea.AxisX.Maximum * factor;

                UpdateSeriesForAxisX(chartArea.AxisX.Maximum);
            }
        }

        public void UpdateSeriesForAxisX(double newXMax)
        {
            Ut.Points.Clear();
            It.Points.Clear();

            if (radioButton1.Checked)
            {
                GraphicsBuilder.BuildUtRC(Ut, uMax, omega, newXMax);
            }
            else if (radioButton2.Checked)
            {
                GraphicsBuilder.BuildItRC(It, iMax, omega, newXMax);
            }
            else if (radioButton3.Checked)
            {
                GraphicsBuilder.BuildUtRC(Ut, uMax, omega, newXMax);
                GraphicsBuilder.BuildItRC(It, iMax, omega, newXMax);
            }
        }

        private void chart1_DoubleClick(object sender, EventArgs e)
        {
            if (!graphicsExisting)
                return;

            ResetZoom();
        }

        #endregion

        #region --- Interface ---

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            chartManager.ClearChartArea();
            graphicsExisting = false;
            if (tabControl1.SelectedTab == tabPage1)
            {
                toolTip1.SetToolTip(pictureBox1, "Ознайомтеся з інструкцією до лабораторної роботи та порядком її виконання");
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            var labInstructions = new LabsInstructions();
            labInstructions.Show();
        }

        #endregion

        private void Lab_3_FormClosing(object sender, FormClosingEventArgs e)
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
