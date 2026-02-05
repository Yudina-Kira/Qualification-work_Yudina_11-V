using NotesLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace PhysicsLabsComplex
{
    public partial class History : Form
    {
        NoteTakingHelper noteHelper;
        DrawingModeHelper drawingModeHelper;

        private readonly string htmlFilePath;
        private string currentHtmlPageName = "";
        private string currentNode = "";

        private readonly string basePath = AppDomain.CurrentDomain.BaseDirectory;

        public History(string filePath)
        {
            InitializeComponent();

            noteHelper = new NoteTakingHelper(webBrowser1);
            NoteDataWorking.Initialize(basePath);

            Buttons[] toolButtons = new Buttons[] { penButton, markerButton, laserButton, colorButton };
            drawingModeHelper = new DrawingModeHelper(toolButtons, saveButton, resetButton, widthTextBox, toolColorPanel);

            htmlFilePath = filePath;
            currentHtmlPageName = "Перші_уявлення.html";
            currentNode = "1.1. Перші уявлення про електрику";

            //setting cursor on active elements
            {
                CursorSetting.SetHandCursor(toggleSwitch1);
                CursorSetting.SetHandCursor(penButton);
                CursorSetting.SetHandCursor(markerButton);
                CursorSetting.SetHandCursor(laserButton);
                CursorSetting.SetHandCursor(colorButton);
                CursorSetting.SetHandCursor(saveButton);
                CursorSetting.SetHandCursor(resetButton);
            }
        }

        private void History_Load(object sender, EventArgs e)
        {
            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.Navigate(htmlFilePath);
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e) => noteHelper.LoadNotes(currentNode);

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                noteHelper.SaveNotes(currentNode);
            }
            catch { }

            if (toggleSwitch1.Checked)
            {
                toggleSwitch1.SetChecked(false);
                noteHelper.SetDrawingEnabled(false);
                drawingModeHelper.DisableDrawingMode();
            }

            switch (e.Node.Text)
            {
                case "1.1. Перші уявлення про електрику":
                    currentHtmlPageName = "Перші_уявлення.html";
                    currentNode = "1.1. Перші уявлення про електрику";
                    break;

                case "1.2. Розвиток техніки, перші генератори заряду":
                    currentHtmlPageName = "Розвиток_техніки.html";
                    currentNode = "1.2. Розвиток техніки";
                    break;

                case "1.3. Відкриття різних властивостей заряду":
                    currentHtmlPageName = "Властивості_заряду.html";
                    currentNode = "1.3. Відкриття властивостей заряду";
                    break;

                case "1.4. Лейденська банка":
                    currentHtmlPageName = "Лейденська_банка.html";
                    currentNode = "1.4. Лейденська банка";
                    break;

                case "1.5. Від статичної електрики до електричного струму":
                    currentHtmlPageName = "Поява_струму.html";
                    currentNode = "1.5. Від статики до струму";
                    break;

                default:
                    return;
            }

            webBrowser1.Navigate(Path.Combine(Application.StartupPath, "HTML-theory", currentHtmlPageName));
        }

        private void toggleSwitch1_Click(object sender, EventArgs e)
        {
            if (toggleSwitch1.Checked)
            {
                noteHelper.SetDrawingEnabled(true);
                drawingModeHelper.EnableDrawingMode();
            }
            else
            {
                try
                {
                    noteHelper.SaveNotes(currentNode);
                }
                catch { }

                noteHelper.SetDrawingEnabled(false);
                drawingModeHelper.DisableDrawingMode();
            }
        }

        #region --- Tool and Save/Clear Buttons ---

        private void penButton_Click(object sender, EventArgs e)
        {
            noteHelper.SetTool("pen", out var penParams);
            drawingModeHelper.SelectTool(penButton, penParams.color, penParams.width);
        }

        private void markerButton_Click(object sender, EventArgs e)
        {
            noteHelper.SetTool("marker", out var markerParams);
            drawingModeHelper.SelectTool(markerButton, markerParams.color, markerParams.width);
        }

        private void laserButton_Click(object sender, EventArgs e)
        {
            noteHelper.SetTool("laser", out var laserParams);
            drawingModeHelper.SelectTool(laserButton, laserParams.color, laserParams.width);
        }

        private void colorButton_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() != DialogResult.OK) return;

            noteHelper.SetLineColor(colorDialog1.Color);
            drawingModeHelper.UpdateColor(colorDialog1.Color);
        }

        private void widthTextBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(widthTextBox.Text)) return;
        }

        private void widthTextBox_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(widthTextBox.Text))
            {
                widthTextBox.Text = noteHelper.lineWidth.ToString();
            }

            if (float.TryParse(widthTextBox.Text, NumberStyles.Float, CultureInfo.CurrentCulture, out float width) && width > 0 && width <= 100)
            {
                drawingModeHelper.UpdateWidth(width); // спочатку перевірка на нуль
                noteHelper.SetLineWidth(width); // потім запис
            }
            else
            {
                MessageBox.Show("Значення товщини лінії введено некоректно. \n"
                    + "Спробуйте, будь ласка, ще раз.", "Помилка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                widthTextBox.Text = noteHelper.lineWidth.ToString();
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            try
            {
                noteHelper.SaveNotes(currentNode);

                string nodeFolder = NoteDataWorking.GetNodeFolder(currentNode);
                var files = Directory.GetFiles(nodeFolder).OrderByDescending(f => File.GetLastWriteTime(f)).ToArray();

                string latestFile = files.Length > 0 ? Path.GetFileName(files[0]) : "файл не знайдено";
                string fullPath = Path.Combine(nodeFolder, latestFile);

                MessageBox.Show($"Конспект збережено!\n\nФайл: {fullPath}",
                    "Успішно", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка збереження: {ex.Message}");
            }
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "Ви точно хочете очистити конспект?\n" + "Цю дію неможливо скасувати — він буде видалений назавжди.",
                "Підтвердження очищення",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                noteHelper.ClearCanvas();
                NoteDataWorking.ClearFolder(currentNode);
            }
        }

        #endregion

        private void History_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                noteHelper.SaveNotes(currentNode);
            }
            catch { }

            var menuForm = new Menu();
            menuForm.Show();
        }
    }
}
