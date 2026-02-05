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

        private GraphicsMode currentMode = GraphicsMode.SingleUt;

        private Axis activeAxisY;

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
        private double yUnitFactor = 1;
        private double y2UnitFactor = 1;

        private double[] niceSteps =
        {
            1e-6, 2e-6, 5e-6,
            1e-5, 2e-5, 5e-5,
            1e-4, 2e-4, 5e-4,
            1e-3, 2e-3, 5e-3,
            1e-2, 2e-2, 5e-2,
            1e-1, 2e-1, 5e-1,
            1, 2, 5,
            10, 20, 50,
            100, 200, 500,
            1000
        };

        private double[] sharpSteps =
        {
            1e-6, 2e-6, 3e-6, 4e-6, 5e-6, 6e-6, 7e-6, 8e-6, 9e-6,
            1e-5, 2e-5, 3e-5, 4e-5, 5e-5, 6e-5, 7e-5, 8e-5, 9e-5,
            1e-4, 2e-4, 3e-4, 4e-4, 5e-4, 6e-4, 7e-4, 8e-4, 9e-4,
            1e-3, 2e-3, 3e-3, 4e-3, 5e-3, 6e-3, 7e-3, 8e-3, 9e-3,
            1e-2, 2e-2, 3e-2, 4e-2, 5e-2, 6e-2, 7e-2, 8e-2, 9e-2,
            1e-1, 2e-1, 3e-1, 4e-1, 5e-1, 6e-1, 7e-1, 8e-1, 9e-1,
            1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 
            12, 15, 20, 
            25, 30, 35, 40,
            45, 50, 60, 70, 80, 90,
            100, 125, 150, 200, 250, 500,
            1000
        };

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
            SetAnnotations(); //налаштування текстових анотацій
        }

        #region --- General customization of element styles and preparing ---

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

            // Main axes
            chartArea.AxisX.LineColor = Color.LimeGreen;
            chartArea.AxisY.LineColor = Color.LimeGreen;

            chartArea.AxisX.LineDashStyle = ChartDashStyle.Solid;
            chartArea.AxisY.LineDashStyle = ChartDashStyle.Solid;

            chartArea.AxisX.TitleForeColor = Color.LimeGreen;
            chartArea.AxisY.TitleForeColor = Color.LimeGreen;

            // Major grids
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

            // Additional axis Y2
            chartArea.AxisY2.LineColor = Color.LimeGreen;
            chartArea.AxisY2.LineDashStyle = ChartDashStyle.Solid;
            chartArea.AxisY2.TitleForeColor = Color.LimeGreen;

            chartArea.AxisY2.MajorGrid.LineColor = Color.LimeGreen;
            chartArea.AxisY2.MajorGrid.LineDashStyle = ChartDashStyle.Solid;
            chartArea.AxisY2.MajorGrid.LineWidth = 1;

            chartArea.AxisY2.MinorGrid.LineColor = Color.LimeGreen;
            chartArea.AxisY2.MinorGrid.LineDashStyle = ChartDashStyle.Dot;
            chartArea.AxisY2.MinorGrid.LineWidth = 1;
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

        public void ClearChartArea()
        {
            chart.Series.Clear();

            xAnnotation.Text = "";
            yAnnotation.Text = "";
            addyAnnotation.Text = "";

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

        #endregion

        #region --- Setting the color for annotations in different modes ---

        public enum AnnotationsMode
        {
            ChargeDischargeModel,
            ChargeDischargeExpSingle,
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
                case AnnotationsMode.ChargeDischargeModel:

                    yAnnotation.ForeColor = Color.GreenYellow;
                    SetLinesColor(Color.GreenYellow, yLine, supYLine, subYLine);

                    addyAnnotation.ForeColor = Color.Transparent;
                    SetLinesColor(Color.Transparent, addYLine, addSupYLine, addSubYLine);

                    break;

                case AnnotationsMode.ChargeDischargeExpSingle:

                    yAnnotation.ForeColor = Color.SkyBlue;
                    SetLinesColor(Color.SkyBlue, yLine, supYLine, subYLine);

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

                    yAnnotation.ForeColor = Color.SkyBlue;
                    SetLinesColor(Color.SkyBlue, yLine, supYLine, subYLine);

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

        private void SetLinesColor(Color color, params LineAnnotation[] ln)
        {
            foreach (LineAnnotation l in ln)
            {
                l.LineColor = color;
            }
        }

        #endregion

        #region --- Clearing and setting up series in different modes ---

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

        private void SetSeries(Series series, Color color, SeriesChartType type)
        {
            series.Points.Clear();
            series.ChartType = type;
            series.Color = color;
            series.BorderWidth = 4;
            series.ChartArea = chartArea.Name;
        }

        #endregion

        #region --- Configuration of axes, setting their limits ---

        public void ConfigureAxes(string xTitle, string yTitle, string xUnit, string yUnit)
        {
            chartArea.AxisX.Title = xTitle;
            chartArea.AxisY.Title = yTitle;

            this.xUnit = xUnit;
            this.yUnit = yUnit;
        }

        public void ConfigureAxisY2(string y2Title, string addyUnit)
        {
            chartArea.AxisY2.Title = y2Title;
            this.addyUnit = addyUnit;

            chartArea.AxisY2.LabelStyle.Enabled = false;
            chartArea.AxisY2.IsStartedFromZero = false;
        }

        public void SetAxisLimits(double xMin, double xMax, double yMin, double yMax, double? y2Min = null, double? y2Max = null)
        {
            chartArea.AxisX.Minimum = xMin;
            chartArea.AxisX.Maximum = xMax;
            chartArea.AxisY.Minimum = yMin;
            chartArea.AxisY.Maximum = yMax;

            if (y2Min.HasValue && y2Max.HasValue)
            {
                chartArea.AxisY2.Minimum = y2Min.Value;
                chartArea.AxisY2.Maximum = y2Max.Value;
            }
        }

        public void ResetAxes()
        {
            chartArea.AxisX.Minimum = double.NaN;
            chartArea.AxisX.Maximum = double.NaN;
            chartArea.AxisY.Minimum = double.NaN;
            chartArea.AxisY.Maximum = double.NaN;
            chartArea.AxisY2.Minimum = double.NaN;
            chartArea.AxisY2.Maximum = double.NaN;

            chartArea.AxisX.Interval = double.NaN;
            chartArea.AxisY.Interval = double.NaN;
            chartArea.AxisY2.Interval = double.NaN;

            chartArea.AxisX.ScaleView.ZoomReset();
            chartArea.AxisY.ScaleView.ZoomReset();
            chartArea.AxisY2.ScaleView.ZoomReset();
        }

        #endregion

        #region --- Active axis and axes modes by GraphicsMode ---

        public enum GraphicsMode
        {
            SingleUt,
            SingleIt,
            UtIt
        }

        public void ApplyAxisMode(GraphicsMode mode)
        {
            currentMode = mode;

            chartArea.AxisY.Enabled = AxisEnabled.False;
            chartArea.AxisY2.Enabled = AxisEnabled.False;

            chartArea.AxisY.MajorGrid.Enabled = false;
            chartArea.AxisY.MinorGrid.Enabled = false;
            chartArea.AxisY2.MajorGrid.Enabled = false;
            chartArea.AxisY2.MinorGrid.Enabled = false;

            switch (currentMode)
            {
                case GraphicsMode.SingleUt:
                    chartArea.AxisY.Enabled = AxisEnabled.True;
                    chartArea.AxisY.MajorGrid.Enabled = true;
                    chartArea.AxisY.MinorGrid.Enabled = true;
                    activeAxisY = chartArea.AxisY;
                    break;

                case GraphicsMode.SingleIt:
                    chartArea.AxisY2.Enabled = AxisEnabled.True;
                    chartArea.AxisY2.MajorGrid.Enabled = true;
                    chartArea.AxisY2.MinorGrid.Enabled = true;
                    activeAxisY = chartArea.AxisY2;
                    break;

                case GraphicsMode.UtIt:
                    chartArea.AxisY.Enabled = AxisEnabled.True;
                    chartArea.AxisY2.Enabled = AxisEnabled.True;
                    
                    chartArea.AxisY.MajorGrid.Enabled = true;
                    chartArea.AxisY.MinorGrid.Enabled = true;
                    
                    activeAxisY = chartArea.AxisY;
                    break;
            }
        }

        public void SetActiveAxisY(Axis axis) => activeAxisY = axis;

        //private Axis GetEffectiveYAxis()
        //{
        //    switch (currentMode)
        //    {
        //        case GraphicsMode.SingleUt:
        //        case GraphicsMode.UtIt:
        //            return 
        //    }
        //}

        #endregion

        #region --- Zoom of axes ---

        public void ZoomAxisX(double delta, double factor = 0.1)
        {
            if (delta < 0) chartArea.AxisX.Maximum += chartArea.AxisX.Maximum * factor;
            else if (delta > 0) chartArea.AxisX.Maximum -= chartArea.AxisX.Maximum * factor;

            if (currentMode == GraphicsMode.UtIt)
                ApplyStaticGridY(5);
            else
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

        public void ZoomActiveAxisY(double delta, double factor = 0.1)
        {
            Axis axis = activeAxisY;
            if (axis == null || axis.Interval <= 0) return;

            double min = axis.Minimum;
            double max = axis.Maximum;

            double center = (min + max) / 2;
            double halfRange = (max - min) / 2;

            if (delta < 0)
                halfRange *= (1 + factor);
            else
                halfRange *= (1 - factor);

            axis.Minimum = center - halfRange;
            axis.Maximum = center + halfRange;

            if (currentMode == GraphicsMode.UtIt)
                ApplyStaticGridY(5);
            else
                ApplyNiceGrid();

            UpdateAnnotations();

            chart.Invalidate();
        }

        public void ZoomActiveAxisYSharp(double delta)
        {
            if (activeAxisY == null) return;

            double currentInterval = activeAxisY.Interval;
            if (currentInterval <= 0) return;

            int idx = 0;
            for (int i = 0; i < sharpSteps.Length; i++)
            {
                if (sharpSteps[i] >= currentInterval)
                {
                    idx = i;
                    break;
                }
            }

            if (delta > 0 && idx > 0) idx--;        // zoom in – менший інтервал
            else if (delta < 0 && idx < sharpSteps.Length - 1) idx++;

            double newInterval = sharpSteps[idx];

            // центр осі залишаємо
            double center = (activeAxisY.Maximum + activeAxisY.Minimum) / 2;
            double halfRange = newInterval * 5 / 2; // 5 клітинок

            activeAxisY.Minimum = center - halfRange;
            activeAxisY.Maximum = center + halfRange;
            activeAxisY.Interval = newInterval;

            UpdateAnnotations();
            chart.Invalidate();
        }

        #endregion

        #region --- Grids for different modes ---

        public void ApplyNiceGrid(int divisions = 5)
        {
            chartArea.AxisX.IntervalAutoMode = IntervalAutoMode.FixedCount;
            chartArea.AxisX.IsMarginVisible = false;
            chartArea.AxisY.IntervalAutoMode = IntervalAutoMode.FixedCount;
            chartArea.AxisY.IsMarginVisible = false;
            chartArea.AxisY2.IntervalAutoMode = IntervalAutoMode.FixedCount;
            chartArea.AxisY2.IsMarginVisible = false;

            // Axis X
            double xRange = chartArea.AxisX.Maximum - chartArea.AxisX.Minimum;

            if (xRange < 1e-3) { xUnit = "мкс"; xUnitFactor = 1e6; }
            else if (xRange < 1) { xUnit = "мс"; xUnitFactor = 1e3; }
            else { xUnit = "с"; xUnitFactor = 1; }

            chartArea.AxisX.Interval = GetNiceInterval(xRange, divisions);

            // Axis Y
            double yRange = chartArea.AxisY.Maximum - chartArea.AxisY.Minimum;
            chartArea.AxisY.Interval = GetNiceInterval(yRange, divisions);

            // Axis Y2
            double y2Range = chartArea.AxisY2.Maximum - chartArea.AxisY2.Minimum;

            if (y2Range < 1e-3) { addyUnit = "мкА"; y2UnitFactor = 1e6; }
            else if (y2Range < 1) { addyUnit = "мА"; y2UnitFactor = 1e3; }
            else { addyUnit = "А"; y2UnitFactor = 1; }

            chartArea.AxisY2.Interval = GetNiceInterval(y2Range, divisions);

            UpdateAnnotations();
        }

        public void ApplyStaticGrid4x4Experiment()
        {
            double xMax = chartArea.AxisX.Maximum;
            double xMin = chartArea.AxisX.Minimum;

            chartArea.AxisX.Interval = (xMax - xMin) / 4;

            double yMax = chartArea.AxisY.Maximum;
            double yMin = chartArea.AxisY.Minimum;

            chartArea.AxisY.Interval = (yMax - yMin) / 4;

            double y2Max = chartArea.AxisY2.Maximum;
            double y2Min = chartArea.AxisY2.Minimum;

            chartArea.AxisY2.Interval = (y2Max - y2Min) / 4;

            //SetStaticAnnotations();
        }

        public void ApplyStaticGridY(int divisions = 5)
        {
            // Χ
            double xRange = chartArea.AxisX.Maximum - chartArea.AxisX.Minimum;

            if (xRange < 1e-3) { xUnit = "мкс"; xUnitFactor = 1e6; }
            else if (xRange < 1) { xUnit = "мс"; xUnitFactor = 1e3; }
            else { xUnit = "с"; xUnitFactor = 1; }

            chartArea.AxisX.Interval = GetNiceInterval(xRange, divisions);

            // Y
            if (chartArea.AxisY.Enabled == AxisEnabled.True)
            {
                chartArea.AxisY.IntervalAutoMode = IntervalAutoMode.FixedCount;
                chartArea.AxisY.IsMarginVisible = false;

                double yRange = chartArea.AxisY.Maximum - chartArea.AxisY.Minimum;
                chartArea.AxisY.Interval = yRange / divisions;
            }

            // Y2
            if (chartArea.AxisY2.Enabled == AxisEnabled.True)
            {
                chartArea.AxisY2.IntervalAutoMode = IntervalAutoMode.FixedCount;
                chartArea.AxisY2.IsMarginVisible = false;

                double y2Range = chartArea.AxisY2.Maximum - chartArea.AxisY2.Minimum;

                if (y2Range < 1e-3) { addyUnit = "мкА"; y2UnitFactor = 1e6; }
                else if (y2Range < 1) { addyUnit = "мА"; y2UnitFactor = 1e3; }
                else { addyUnit = "А"; y2UnitFactor = 1; }

                chartArea.AxisY2.Interval = y2Range / divisions;
            }

            UpdateAnnotations();
        }

        #endregion

        #region --- Annotations ---

        private void UpdateAnnotations()
        {
            // X-annotation
            if (chartArea.AxisX.Interval > 0)
                xAnnotation.Text = $"{chartArea.AxisX.Interval * xUnitFactor:0.###} {xUnit} / 1 под.";

            // Y-annotation and Y2-annotation
            if (activeAxisY != null && activeAxisY.Interval > 0)
            {
                (string mainUnit, double mainFactor, string secondUnit, double secondFactor) = GetAxisYUnits();

                if (activeAxisY == chartArea.AxisY)
                {
                    yAnnotation.Text = $"{chartArea.AxisY.Interval * mainFactor:0.###} {mainUnit} / 1 под.";
                    addyAnnotation.Text = $"{chartArea.AxisY2.Interval * secondFactor:0.###} {secondUnit} / 1 под.";
                }

                if (activeAxisY == chartArea.AxisY2)
                {
                    if (currentMode == GraphicsMode.SingleIt)
                    {
                        yAnnotation.Text = $"{activeAxisY.Interval * mainFactor:0.###} {mainUnit} / 1 под.";
                    }
                    else if (currentMode == GraphicsMode.UtIt)
                    {
                        yAnnotation.Text = $"{chartArea.AxisY.Interval * mainFactor:0.###} {mainUnit} / 1 под.";
                        addyAnnotation.Text = $"{activeAxisY.Interval * secondFactor:0.###} {secondUnit} / 1 под.";
                    }
                }

            }
        }

        private double GetNiceInterval(double range, int divisions)
        {
            double rough = range / divisions;

            foreach (var step in niceSteps)
                if (step >= rough) 
                    return step;

            return niceSteps[niceSteps.Length - 1];
        }

        private (string mainUnit, double mainFactor, string secondUnit, double secondFactor) GetAxisYUnits()
        {
            switch (currentMode)
            {
                case GraphicsMode.SingleIt:
                    return (addyUnit, y2UnitFactor, yUnit, yUnitFactor);

                case GraphicsMode.SingleUt:
                case GraphicsMode.UtIt:
                    return (yUnit, yUnitFactor, addyUnit, y2UnitFactor);

                default:
                    return (yUnit, yUnitFactor, addyUnit, y2UnitFactor);
            }
        }

        public void SetStaticAnnotations()
        {
            xAnnotation.Text = $"320 мкс / 1 под.";
            yAnnotation.Text = $"1 В / 1 под.";
            addyAnnotation.Text = $"1 В / 1 под.";
        }

        public void SetStaticAnnotationsRCRL()
        {
            xAnnotation.Text = $"320 мкс / 1 под.";
            yAnnotation.Text = $"2 В / 1 под.";
            addyAnnotation.Text = $"2 В / 1 под.";
        }

        #endregion
    }
}