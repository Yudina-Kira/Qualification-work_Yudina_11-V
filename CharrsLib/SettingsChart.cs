using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization;
using System.Windows.Forms.DataVisualization.Charting;
using System.Security.Cryptography.X509Certificates;

namespace ChartsLib
{
    public class SettingsChart
    {
        private Chart chart;
        private ChartArea chartArea;

        private TextAnnotation xAnnotation;
        private TextAnnotation yAnnotation;
        private TextAnnotation addyAnnotation;
        private LineAnnotation xLine, supXLine, subXLine;
        private LineAnnotation yLine, supYLine, subYLine;
        private LineAnnotation addYLine, addSupYLine, addSubYLine;
        private string xUnit = "";
        private string yUnit = "";
        private string addyUnit = "";

        private double xUnitFactor = 1;

        public SettingsChart(Chart chart)
        {
            this.chart = chart;
            chartArea = new ChartArea();

            xAnnotation = new TextAnnotation();
            yAnnotation = new TextAnnotation();
            addyAnnotation = new TextAnnotation();

            xLine = new LineAnnotation();
            supXLine = new LineAnnotation();
            subXLine = new LineAnnotation();
            yLine = new LineAnnotation();
            supYLine = new LineAnnotation();
            subYLine = new LineAnnotation();
            addYLine = new LineAnnotation();
            addSupYLine = new LineAnnotation();
            addSubYLine = new LineAnnotation();

            PrepareChart(); //підготовка чарту та додавання елементів
            SetChartArea(); //налаштування області
            SetAnnotations(); //налаштування анотацій
        }

        public void PrepareChart()
        {
            chart.BackColor = Color.Black;

            chart.ChartAreas.Clear();
            chart.ChartAreas.Add(chartArea);

            chart.Annotations.Clear();
            chart.Annotations.Add(xAnnotation);
            chart.Annotations.Add(yAnnotation);
            chart.Annotations.Add(addyAnnotation);
            chart.Annotations.Add(xLine);
            chart.Annotations.Add(yLine);
            chart.Annotations.Add(addYLine);
            chart.Annotations.Add(supXLine);
            chart.Annotations.Add(subXLine);
            chart.Annotations.Add(supYLine);
            chart.Annotations.Add(subYLine);
            chart.Annotations.Add(addSupYLine);
            chart.Annotations.Add(addSubYLine);

            chart.Legends.Clear();
        }

        public void SetChartArea()
        {
            chartArea.BackColor = Color.Black;

            // Main axises
            chartArea.AxisX.LineColor = Color.LimeGreen;
            chartArea.AxisY.LineColor = Color.LimeGreen;

            chartArea.AxisX.LineDashStyle = ChartDashStyle.Solid;
            chartArea.AxisY.LineDashStyle = ChartDashStyle.Solid;

            chartArea.AxisX.TitleForeColor = Color.LimeGreen;
            chartArea.AxisY.TitleForeColor = Color.LimeGreen;

            // Major grid
            chartArea.AxisX.MajorGrid.LineColor = Color.LimeGreen;
            chartArea.AxisY.MajorGrid.LineColor = Color.LimeGreen;

            chartArea.AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Solid;
            chartArea.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Solid;

            chartArea.AxisX.MajorGrid.LineWidth = 1;
            chartArea.AxisY.MajorGrid.LineWidth = 1;

            chartArea.AxisX.MajorTickMark.LineColor = Color.LimeGreen;
            chartArea.AxisY.MajorTickMark.LineColor = Color.LimeGreen;

            // Minor grid
            chartArea.AxisX.MinorGrid.Enabled = true;
            chartArea.AxisY.MinorGrid.Enabled = true;

            chartArea.AxisX.MinorGrid.LineColor = Color.LimeGreen;
            chartArea.AxisY.MinorGrid.LineColor = Color.LimeGreen;

            chartArea.AxisX.MinorGrid.LineDashStyle = ChartDashStyle.Dot;
            chartArea.AxisY.MinorGrid.LineDashStyle = ChartDashStyle.Dot;

            chartArea.AxisX.MinorGrid.LineWidth = 1;
            chartArea.AxisY.MinorGrid.LineWidth = 1;

            // Scrollbars
            chartArea.AxisX.ScrollBar.Enabled = false;
            chartArea.AxisY.ScrollBar.Enabled = false;

            // Placement
            chartArea.Position.X = 0;
            chartArea.Position.Y = 0;
            chartArea.Position.Width = 100;
            chartArea.Position.Height = 100;

            chartArea.InnerPlotPosition.X = 0;
            chartArea.InnerPlotPosition.Y = 0;
            chartArea.InnerPlotPosition.Width = 100;
            chartArea.InnerPlotPosition.Height = 100;
        }

        public void SetAnnotations()
        {
            xAnnotation.Font = new Font("Consolas", 12);
            yAnnotation.Font = new Font("Consolas", 12);
            addyAnnotation.Font = new Font("Consolas", 12);

            xAnnotation.BackColor = Color.Transparent;
            yAnnotation.BackColor = Color.Transparent;
            addyAnnotation.BackColor = Color.Transparent;

            xAnnotation.X = 81;
            xAnnotation.Y = 5;

            yAnnotation.X = 9;
            yAnnotation.Y = 7;

            addyAnnotation.X = 9;
            addyAnnotation.Y = 87;

            xAnnotation.ClipToChartArea = chartArea.Name;
            yAnnotation.ClipToChartArea = chartArea.Name;
            addyAnnotation.ClipToChartArea = chartArea.Name;
        }

        public enum AnnotationsMode
        {
            ChargeDischargeSingle,
            GeneratorSingle,
            ChargeDischargeGenerator,
            SingleUt,
            SingleIt,
            UtIt
        }

        public void DrawLineAnnotations(AnnotationsMode mode)
        {
            // Lines' width
            xLine.LineWidth = 2;
            supXLine.LineWidth = 2;
            subXLine.LineWidth = 2;
            yLine.LineWidth = 2;
            supYLine.LineWidth = 2;
            subYLine.LineWidth = 2;
            addYLine.LineWidth = 2;
            addSupYLine.LineWidth = 2;
            addSubYLine.LineWidth = 2;

            // Annotations' coordinates
            xLine.X = 81;
            xLine.Y = 12;
            xLine.Width = 18;
            xLine.Height = 0;

            supXLine.X = 81;
            supXLine.Y = 10;
            supXLine.Width = 0;
            supXLine.Height = 4;

            subXLine.X = 99;
            subXLine.Y = 10;
            subXLine.Width = 0;
            subXLine.Height = 4;

            yLine.X = 7;
            yLine.Y = 1;
            yLine.Width = 0;
            yLine.Height = 18;

            supYLine.X = 5;
            supYLine.Y = 1;
            supYLine.Width = 4;
            supYLine.Height = 0;

            subYLine.X = 5;
            subYLine.Y = 19;
            subYLine.Width = 4;
            subYLine.Height = 0;

            addYLine.X = 7;
            addYLine.Y = 81;
            addYLine.Width = 0;
            addYLine.Height = 18;

            addSupYLine.X = 5;
            addSupYLine.Y = 81;
            addSupYLine.Width = 4;
            addSupYLine.Height = 0;

            addSubYLine.X = 5;
            addSubYLine.Y = 99;
            addSubYLine.Width = 4;
            addSubYLine.Height = 0;

            // Colors and additional annotations
            xAnnotation.ForeColor = Color.GreenYellow;
            SetLinesColor(Color.GreenYellow, xLine, supXLine, subXLine);

            switch (mode)
            {
                case AnnotationsMode.ChargeDischargeSingle:

                    yAnnotation.ForeColor = Color.GreenYellow;
                    SetLinesColor(Color.GreenYellow, yLine, supYLine, subYLine);

                    addyAnnotation.ForeColor = Color.Transparent;
                    SetLinesColor(Color.Transparent, addYLine, addSupYLine, addSubYLine);

                    break;

                case AnnotationsMode.GeneratorSingle:

                    yAnnotation.ForeColor = Color.Red;
                    SetLinesColor(Color.Red, yLine, supYLine, subYLine);

                    addyAnnotation.ForeColor = Color.Transparent;
                    SetLinesColor(Color.Transparent, addYLine, addSupYLine, addSubYLine);

                    break;

                case AnnotationsMode.ChargeDischargeGenerator:

                    yAnnotation.ForeColor = Color.LimeGreen;
                    SetLinesColor(Color.LimeGreen, yLine, supYLine, subYLine);

                    addyAnnotation.ForeColor = Color.Red;
                    SetLinesColor(Color.Red, addYLine, addSupYLine, addSubYLine);

                    break;

                case AnnotationsMode.SingleUt:

                    yAnnotation.ForeColor = Color.Aquamarine;
                    SetLinesColor(Color.Aquamarine, yLine, supYLine, subYLine);

                    addyAnnotation.ForeColor = Color.Transparent;
                    SetLinesColor(Color.Transparent, addYLine, addSupYLine, addSubYLine);

                    break;

                case AnnotationsMode.SingleIt:

                    yAnnotation.ForeColor = Color.Yellow;
                    SetLinesColor(Color.Yellow, yLine, supYLine, subYLine);

                    addyAnnotation.ForeColor = Color.Transparent;
                    SetLinesColor(Color.Transparent, addYLine, addSupYLine, addSubYLine);

                    break;

                case AnnotationsMode.UtIt:

                    yAnnotation.ForeColor = Color.Aquamarine;
                    SetLinesColor(Color.Aquamarine, yLine, supYLine, subYLine);

                    addyAnnotation.ForeColor = Color.Yellow;
                    SetLinesColor(Color.Yellow, addYLine, addSupYLine, addSubYLine);

                    break;
            }
        }

        public void SetLinesColor(Color color, params LineAnnotation[] ln)
        {
            foreach (LineAnnotation l in ln)
            {
                l.LineColor = color;
            }
        }

        public enum SeriesMode
        {
            ChargeDischargeModel,
            ChargeDischargeExp,
            UGenerator,
            UtModel,
            UtExp,
            ItModel,
            ItExp
        }

        public void ConfigureSeries(Series series, SeriesMode mode)
        {
            switch (mode)
            {
                case SeriesMode.ChargeDischargeModel:
                    SetSeries(series, Color.LimeGreen, SeriesChartType.Line);
                    break;

                case SeriesMode.ChargeDischargeExp:
                    SetSeries(series, Color.SkyBlue, SeriesChartType.Point);
                    break;

                case SeriesMode.UGenerator:
                    SetSeries(series, Color.Red, SeriesChartType.Line);
                    break;

                case SeriesMode.UtModel:
                    SetSeries(series, Color.Aquamarine, SeriesChartType.Line);
                    break;

                case SeriesMode.UtExp:
                    SetSeries(series, Color.Aquamarine, SeriesChartType.Point);
                    break;

                case SeriesMode.ItModel:
                    SetSeries(series, Color.Yellow, SeriesChartType.Line);
                    break;

                case SeriesMode.ItExp:
                    SetSeries(series, Color.Yellow, SeriesChartType.Point);
                    break;
            }
        }

        public void SetSeries(Series series, Color color, SeriesChartType type)
        {
            series.Points.Clear();
            series.ChartType = type;
            series.Color = color;
            series.BorderWidth = 4;
            series.ChartArea = chartArea.Name;
        }

        public void ConfigureAxes(string xTitle, string yTitle, double xMin, double yMin, string xUnit, string yUnit, string addyUnit = "")
        {
            chartArea.AxisX.Title = xTitle;
            chartArea.AxisY.Title = yTitle;

            chartArea.AxisX.Minimum = xMin;
            chartArea.AxisY.Minimum = yMin;

            this.xUnit = xUnit;
            this.yUnit = yUnit;
            this.addyUnit = addyUnit;
        }

        public void SetAxisY2()
        {
            chartArea.AxisY2.Enabled = AxisEnabled.True;
            chartArea.AxisY2.MajorGrid.Enabled = false;
            chartArea.AxisY2.MinorGrid.Enabled = false;
            chartArea.AxisY2.LabelStyle.Enabled = false;
            chartArea.AxisY2.ScrollBar.Enabled = false;

            chartArea.AxisY.IsStartedFromZero = false;
            chartArea.AxisY2.IsStartedFromZero = false;

            chartArea.AxisY2.Minimum = chartArea.AxisY.Minimum;
            chartArea.AxisY2.Maximum = chartArea.AxisY.Maximum;
        }

        public void CenterAxisX()
        {
            chartArea.AxisX.IsStartedFromZero = false;
            chartArea.AxisX.Crossing = (chartArea.AxisY.Minimum + chartArea.AxisY.Maximum) / 2;
        }

        public void SetAxisLimits(double xMin, double xMax, double yMin, double yMax)
        {
            chartArea.AxisX.Minimum = xMin;
            chartArea.AxisX.Maximum = xMax;
            chartArea.AxisY.Minimum = yMin;
            chartArea.AxisY.Maximum = yMax;
        }

        public void ApplyNiceGrid(int divisions = 5)
        {
            double xRange = chartArea.AxisX.Maximum - chartArea.AxisX.Minimum;
            double yRange = chartArea.AxisY.Maximum - chartArea.AxisY.Minimum;

            if (xRange < 1e-3) { xUnit = "мкс"; xUnitFactor = 1e6; }
            else if (xRange < 1) { xUnit = "мс"; xUnitFactor = 1e3; }
            else { xUnit = "с"; xUnitFactor = 1; }

            chartArea.AxisX.Interval = GetNiceInterval(xRange, divisions);
            chartArea.AxisY.Interval = GetNiceInterval(yRange, divisions);

            UpdateAnnotations();
        }

        public void ZoomAxisX(double delta, double factor = 0.1)
        {
            if (delta < 0) chartArea.AxisX.Maximum += chartArea.AxisX.Maximum * factor;
            else if (delta > 0) chartArea.AxisX.Maximum -= chartArea.AxisX.Maximum * factor;
        
            ApplyNiceGrid();
            UpdateAnnotations();

            chart.Invalidate();
        }

        public void ZoomAxisY(double delta, double factor = 0.1)
        {
            if (delta < 0) chartArea.AxisY.Maximum += chartArea.AxisY.Maximum * factor;
            else if (delta > 0) chartArea.AxisY.Maximum -= chartArea.AxisY.Maximum * factor;

            ApplyNiceGrid();
            UpdateAnnotations();

            chart.Invalidate();
        }

        public void ZoomAxisYTwoSides(double delta, double factor = 0.1)
        {
            if (delta < 0)
            {
                chartArea.AxisY.Maximum += chartArea.AxisY.Maximum * factor;
                chartArea.AxisY.Minimum += chartArea.AxisY.Minimum * factor;
            }
            else if (delta > 0)
            {
                chartArea.AxisY.Maximum -= chartArea.AxisY.Maximum * factor;
                chartArea.AxisY.Minimum -= chartArea.AxisY.Minimum * factor;
            }

            ApplyNiceGrid();
            UpdateAnnotations();

            chart.Invalidate();
        }

        public void ZoomActiveAxisY(double delta, Axis activeAxisY, double factor = 0.1)
        {
            if (activeAxisY == null)
                return;

            double min = activeAxisY.Minimum;
            double max = activeAxisY.Maximum;

            double center = (min + max) / 2;
            double halfRange = (max - min) / 2;

            if (delta < 0)
                halfRange *= (1 + factor);
            else
                halfRange *= (1 - factor);

            activeAxisY.Minimum = center - halfRange;
            activeAxisY.Maximum = center + halfRange;

            ApplyNiceGrid();
            //UpdateAnnotations();

            chart.Invalidate();
        }

        public void ClearChartArea()
        {
            chart.Series.Clear();

            xAnnotation.Text = "";
            yAnnotation.Text = "";

            xLine.Width = 0;
            xLine.Height = 0;
            supXLine.Width = 0;
            supXLine.Height = 0;
            subXLine.Width = 0;
            subXLine.Height = 0;

            yLine.Width = 0;
            yLine.Height = 0;
            supYLine.Width = 0;
            supYLine.Height = 0;
            subYLine.Width = 0;
            subYLine.Height = 0;

            addYLine.Width = 0;
            addYLine.Height = 0;
            addSupYLine.Width = 0;
            addSupYLine.Height = 0;
            addSubYLine.Width = 0;
            addSubYLine.Height = 0;
        }

        public void ResetAxes()
        {
            chartArea.AxisX.Minimum = double.NaN;
            chartArea.AxisX.Maximum = double.NaN;
            chartArea.AxisY.Minimum = double.NaN;
            chartArea.AxisY.Maximum = double.NaN;

            chartArea.AxisX.Interval = double.NaN;
            chartArea.AxisY.Interval = double.NaN;

            chartArea.AxisX.ScaleView.ZoomReset();
            chartArea.AxisY.ScaleView.ZoomReset();
        }

        #region --- Private utilities ---
        
        private double GetNiceInterval(double range, int divisions)
        {
            double rough = range / divisions;

            double[] niceSteps =
            {
                1e-6, 2e-6, 5e-6, // мкс
                1e-5, 2e-5, 5e-5,
                1e-4, 2e-4, 5e-4,
                1e-3, 2e-3, 5e-3, // мс
                1e-2, 2e-2, 5e-2,
                1e-1, 2e-1, 5e-1,
                1, 2, 5,          // сек
                10, 20, 50,
                100, 200, 500,
                1000
            };

            foreach (var step in niceSteps)
                if (step >= rough) 
                    return step;

            return niceSteps[niceSteps.Length - 1];
        }

        private void UpdateAnnotations()
        {
            if (chartArea.AxisX.Interval > 0)
                xAnnotation.Text = $"{chartArea.AxisX.Interval * xUnitFactor:0.###} {xUnit} / 1 под.";

            if (chartArea.AxisY.Interval > 0)
            {
                yAnnotation.Text = $"{chartArea.AxisY.Interval} {yUnit} / 1 под.";
                addyAnnotation.Text = $"{chartArea.AxisY.Interval} {addyUnit} / 1 под.";
            }
        }

        #endregion
    }
}
