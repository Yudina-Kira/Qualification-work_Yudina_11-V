using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabDataLib
{
    public static class DataWorking
    {
        private static string _basePath;

        public static void Initialize(string basePath)
        {
            _basePath = basePath;
        }

        private static string BasePath =>
            _basePath ?? throw new InvalidOperationException("GraphicsSaving is not initialized");

        public enum DataKind
        {
            Model,
            Experiment,
            None
        }

        // --- Base folder ---
        public static string GetSaveFolder()
        {
            string folder = Path.Combine(BasePath, "SavedGraphics");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            return folder;
        }

        // --- Lab folder ---
        public static string GetLabFolder(int labNumber)
        {
            string folder = Path.Combine(GetSaveFolder(), $"Lab-{labNumber}");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            return folder;
        }

        // --- Lab file name ---
        private static string GetLabFileName(int labNumber, DataKind kind)
        {
            switch (kind)
            {
                case DataKind.Model:
                    return $"Lab-{labNumber}_model.txt";

                case DataKind.Experiment:
                    return $"Lab-{labNumber}_experiment.txt";

                default:
                    throw new ArgumentOutOfRangeException(nameof(kind));
            }
        }
        
        // --- Get path to labfile ---
        public static string GetLabFilePath(int labNumber, DataKind kind)
        {
            return Path.Combine(GetLabFolder(labNumber), GetLabFileName(labNumber, kind));
        }

        // --- Saving graphics data to the file ---
        public static void AppendData(int labNumber, DataKind kind, IReadOnlyDictionary<string, double> parameters)
        {
            string path = GetLabFilePath(labNumber, kind);

            EnsureFileExists(path, parameters.Keys);

            var values = new List<string>();
            foreach (var value in parameters.Values)
            {
                values.Add(value.ToString(CultureInfo.InvariantCulture));
            }
            string newLine = string.Join("|", values);

            var existingLines = File.ReadAllLines(path);
            if (!existingLines.Contains(newLine))
            {
                using (StreamWriter sw = new StreamWriter(path, true))
                {
                    sw.WriteLine(newLine);
                }
            }
        }

        public static void AppendData(int labNumber, DataKind kind, IReadOnlyDictionary<string, string> parameters, params string[] identityKeys)
        {
            string path = GetLabFilePath(labNumber, kind);

            EnsureFileExists(path, identityKeys);

            var values = new List<string>();
            foreach (var key in identityKeys)
            {
                values.Add(parameters[key] ?? "");
            }

            string newLine = string.Join("|", values);

            var existingLines = File.ReadAllLines(path);
            if (!existingLines.Contains(newLine))
            {
                using (StreamWriter sw = new StreamWriter(path, true))
                {
                    sw.WriteLine(newLine);
                }
            }
        }

        // --- Reading existing data ---
        public static DataTable LoadData(int labNumber, DataKind kind)
        {
            DataTable table = new DataTable();

            string path = GetLabFilePath(labNumber, kind);
            if (!File.Exists(path)) return table;

            string[] lines = File.ReadAllLines(path);
            string[] headers = null;
            bool headersFound = false;

            foreach (var l in lines)
            {
                if (string.IsNullOrWhiteSpace(l) || l.StartsWith("#")) continue;

                if (!headersFound)
                {
                    headers = l.Split('|');

                    foreach (var h in headers)
                    {
                        if (kind == DataKind.Experiment && (h == "ch1" || h == "ch2"))
                            table.Columns.Add(h, typeof(string));
                        else
                            table.Columns.Add(h, typeof(double));
                    }

                    headersFound = true;
                    continue;
                }

                string[] values = l.Split('|');
                DataRow row = table.NewRow();

                for (int i = 0; i < headers.Length && i < values.Length; i++)
                {
                    if (table.Columns[i].DataType == typeof(double))
                        row[i] = double.Parse(values[i], CultureInfo.InvariantCulture);
                    else
                        row[i] = values[i];
                }

                table.Rows.Add(row);
            }

            return table;
        }

        // --- Save updates to file (rewriting file) ---
        public static void SaveFile(int labNumber, DataKind kind, IEnumerable<string> lines)
        {
            string path = GetLabFilePath(labNumber, kind);

            var allLines = new List<string>();
            allLines.Add($"# Last updated {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            allLines.AddRange(lines);

            File.WriteAllLines(path, allLines);
        }

        // --- Delete whole file ---
        public static void DeleteFile(int labNumber, DataKind kind)
        {
            string path = GetLabFilePath(labNumber, kind);

            if (File.Exists(path)) File.Delete(path);
        }

        #region --- Private utilities ---

        // --- Creating file with basic table structure ---
        private static void EnsureFileExists(string path, IEnumerable<string> headers)
        {
            if (File.Exists(path)) return;

            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.WriteLine($"# Created {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                sw.WriteLine(string.Join("|", headers));
            }
        }

        #endregion
    }
}
