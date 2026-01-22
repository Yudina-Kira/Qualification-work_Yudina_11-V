using LabDataLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhysicsLabsComplex
{
    public static class GridHelper
    {
        public static DataWorking.DataKind GetTabKind(int tabIndex)
        {
            switch (tabIndex)
            {
                case 0:
                    return DataWorking.DataKind.Model;

                case 1:
                    return DataWorking.DataKind.Experiment;

                default: 
                    return DataWorking.DataKind.None;
            }
        }

        public static void SetUpColumnHeaders(DataGridView dgv)
        {
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                switch (col.Name)
                {
                    case "R":
                        col.HeaderText = "Опір, Ом";
                        break;

                    case "C":
                        col.HeaderText = "Ємність, мкФ";
                        break;

                    case "L":
                        col.HeaderText = "Індуктивність, Гн";
                        break;

                    case "U":
                    case "Umax":
                        col.HeaderText = "Напруга, В";
                        break;

                    case "T":
                        col.HeaderText = "Період, мс";
                        break;

                    case "D":
                        col.HeaderText = "Коеф. заповн., %";
                        break;

                    case "f":
                        col.HeaderText = "Частота, Гц";
                        break;
                }
            }
        }

        public static void HideArrayColumns(DataGridView dgv)
        {
            if (dgv == null) return;

            if (dgv.Columns.Contains("ch1"))
                dgv.Columns["ch1"].Visible = false;

            if (dgv.Columns.Contains("ch2"))
                dgv.Columns["ch2"].Visible = false;
        }

        public static List<string> ToFileLines(DataTable dt)
        {
            var lines = new List<string>();
            if (dt == null) return lines;

            var headers = new List<string>();
            for (int i = 1; i < dt.Columns.Count; i++)
            {
                headers.Add(dt.Columns[i].ColumnName);
            }

            lines.Add(string.Join("|", headers));

            foreach (DataRow row in dt.Rows)
            {
                var values = new List<string>();
                for (int i = 1; i < row.ItemArray.Length; i++)
                {
                    var item = row[i];
                    values.Add(item != null ? item.ToString() : "");
                }

                lines.Add(string.Join("|", values));
            }

            return lines;
        }

        public static void AddExperimentNumberColumn(DataTable dt)
        {
            if (dt == null) return;
            if (dt.Rows.Count == 0 || dt.Columns.Count == 0) return;
            if (dt.Columns.Contains("Номер")) return;

            dt.Columns.Add("Номер", typeof(int));
            dt.Columns["Номер"].SetOrdinal(0);

            int num = 1;
            foreach (DataRow row in dt.Rows)
            {
                row["Номер"] = num++;
            }
        }

        public static void SelectCurrentExperiment(DataGridView dgv, IReadOnlyDictionary<string, double> parameters)
        {
            var values = new List<string>();
            foreach (var value in parameters.Values)
            {
                values.Add(value.ToString(CultureInfo.InvariantCulture));
            }
            string newLine = string.Join("|", values);

            dgv.ClearSelection();
            foreach (DataGridViewRow row in dgv.Rows)
            {
                var rowValues = new List<string>();
                string[] keys = parameters.Keys.ToArray();
                for (int i = 0; i < keys.Length; i++)
                {
                    rowValues.Add(Convert.ToString(row.Cells[i + 1].Value, CultureInfo.InvariantCulture));
                }
                string currentRow = string.Join("|", rowValues);

                if (currentRow == newLine)
                {
                    row.Selected = true;
                    dgv.CurrentCell = row.Cells[0];
                    break;
                }
            }
        }

        public static string GetModelParametersToString(IReadOnlyDictionary<string, double> parameters)
        {
            var values = new List<string>();
            foreach (var p in parameters)
            {
                values.Add(p.Value.ToString(CultureInfo.InvariantCulture));
            }
            return string.Join("|", values);
        }

        public static string GetExperimentParametersToString(IReadOnlyDictionary<string, string> parameters)
        {
            var values = new List<string>();
            foreach (var p in parameters)
            {
                if (p.Key != "ch1" && p.Key != "ch2")
                    values.Add(p.Value);
            }
            return string.Join("|", values);
        }

        public static void SelectCurrentExperiment(DataGridView dgv, string parameters)
        {
            dgv.ClearSelection();

            foreach (DataGridViewRow row in dgv.Rows)
            {
                var rowValues = new List<string>();

                for (int i = 1; i <= parameters.Split('|').Length; i++)
                {
                    rowValues.Add(Convert.ToString(row.Cells[i].Value, CultureInfo.InvariantCulture));
                }

                string currentRow = string.Join("|", rowValues);

                if (currentRow == parameters)
                {
                    row.Selected = true;
                    dgv.CurrentCell = row.Cells[0];
                    break;
                }
            }
        }
    }
}
