using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NotesLib
{
    public class NoteTakingHelper
    {
        private readonly WebBrowser _browser;

        public string tool { get; private set; } = "pen";
        public Color lineColor { get; private set; } = Color.Blue;
        public float lineWidth { get; private set; } = 6;

        public Dictionary<string, (Color color, float width)> toolSettings { get; private set; }
            = new Dictionary<string, (Color color, float width)>
        {
            { "pen", (Color.Blue, 6f) },
            { "marker", (Color.Yellow, 20f) },
            { "laser", (Color.Red, 4f) }
        };

        public (Color color, float width) GetCurrentToolSettings() => toolSettings[tool];

        public NoteTakingHelper(WebBrowser browser)
        {
            _browser = browser ?? throw new ArgumentNullException(nameof(browser));
        }

        // --- JS Invoking --- 
        private void InvokeJS(string functionName, params object[] args)
        {
            try
            {
                _browser.Document?.InvokeScript(functionName, args);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"JS error: {ex.Message}");
            }
        }

        #region --- Tools ---

        public void SetTool(string toolName, out (Color color, float width) settings)
        {
            tool = toolName;
            InvokeJS("setTool", toolName);

            settings = toolSettings[toolName];
            SetLineColor(settings.color);
            SetLineWidth(settings.width);
        }

        public void SetLineColor(Color color)
        {
            lineColor = color;
            InvokeJS("setColor", ColorTranslator.ToHtml(lineColor));

            var (_, width) = toolSettings[tool];
            toolSettings[tool] = (color, width);
        }

        public void SetLineWidth(float width)
        {
            lineWidth = width;
            InvokeJS("setLineWidth", width);

            var (color, _) = toolSettings[tool];
            toolSettings[tool] = (color, width);
        }

        public void SetDrawingEnabled(bool enabled) => InvokeJS("setDrawingEnabled", enabled);

        public void ClearCanvas() => InvokeJS("clearCanvas");

        #endregion

        #region --- Save/Load JSON-data ---

        public void SaveNotes(string nodeName)
        {
            try
            {
                var json = _browser.Document.InvokeScript("saveDrawing")?.ToString();
                
                if (string.IsNullOrEmpty(json)) return;

                NoteDataWorking.SaveData(nodeName, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка збереження: {ex.Message}");
            }
        }

        public void LoadNotes(string nodeName)
        {
            string json = NoteDataWorking.LoadData(nodeName);

            if (string.IsNullOrEmpty(json)) return;

            InvokeJS("loadDrawing", new object[] { json });
        }

        #endregion
    }
}
