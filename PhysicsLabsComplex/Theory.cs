using NotesLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhysicsLabsComplex
{
    public partial class Theory : Form
    {
        NoteTakingHelper noteHelper;
        DrawingModeHelper drawingModeHelper;

        private readonly string htmlFilePath;
        private string currentHtmlPageName = "";
        private string currentNode = "";

        private bool hasUnsavedChanges = false;

        private readonly string basePath = AppDomain.CurrentDomain.BaseDirectory;

        public Theory(string filePath)
        {
            InitializeComponent();

            noteHelper = new NoteTakingHelper(webBrowser1);
            NoteDataWorking.Initialize(basePath);

            Buttons[] toolButtons = new Buttons[] { penButton, markerButton, laserButton, colorButton };
            drawingModeHelper = new DrawingModeHelper(toolButtons, saveButton, resetButton, widthTextBox, toolColorPanel, saveStatus);

            htmlFilePath = filePath;
            currentHtmlPageName = "Загальне_визначення.html";
            currentNode = "1.1. Загальне визначення";
        }

        private void Theory_Load(object sender, EventArgs e)
        {
            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.Navigate(htmlFilePath);
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e) => noteHelper.LoadNotes(currentNode);

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            switch (e.Node.Text)
            {
                case "1.1. Загальне визначення змінного струму та його характеристики":
                    currentHtmlPageName = "Загальне_визначення.html";
                    currentNode = "1.1. Загальне визначення";
                    break;

                case "1.2. Діюче значення змінного струму":
                    currentHtmlPageName = "Діюче_значення_змінного_струму.html";
                    currentNode = "1.2. Діюче значення";
                    break;

                case "1.3. Багатофазний струм":
                    currentHtmlPageName = "Багатофазний_змінний_струм.html";
                    currentNode = "1.3. Багатофазний струм";
                    break;

                case "1.4. Переваги змінного струму":
                    currentHtmlPageName = "Переваги_змінного_струму.html";
                    currentNode = "1.4. Переваги змінного струму";
                    break;

                case "2.1. Імпульсний струм":
                    currentHtmlPageName = "Імпульсний_струм.html";
                    currentNode = "2.1. Імпульсний струм";
                    break;

                case "2.2. Конденсатор. Принцип роботи та характеристики":
                    currentHtmlPageName = "Конденсатор.html";
                    currentNode = "2.2. Конденсатор";
                    break;

                case "2.3. Процеси зарядки і розрядки конденсатора":
                    currentHtmlPageName = "Зарядка_і_розрядка_конденсатора.html";
                    currentNode = "2.3. Процеси зарядки і розрядки";
                    break;

                case "2.4. Види конденсаторів":
                    currentHtmlPageName = "Види_конденсаторів.html";
                    currentNode = "2.4. Види конденсаторів";
                    break;

                case "2.5. Способи збільшення та зменшення загальної ємності":
                    currentHtmlPageName = "Способи_зміни_ємності.html";
                    currentNode = "2.5. Способи зміни ємності";
                    break;

                case "3.1. Резистор. Принцип роботи та характеристики":
                    currentHtmlPageName = "Резистор.html";
                    currentNode = "3.1. Резистор";
                    break;

                case "3.2. Індуктивність. Принцип роботи та характеристики":
                    currentHtmlPageName = "Індуктивність.html";
                    currentNode = "3.2. Індуктивність";
                    break;

                case "3.3. Види опору в колах змінного струму":
                    currentHtmlPageName = "Види_опору.html";
                    currentNode = "3.3. Види опору";
                    break;

                case "3.4. Поведінка RC-ланки в колі змінного струму":
                    currentHtmlPageName = "Поведінка_RC-ланки.html";
                    currentNode = "3.4. Поведінка RC-ланки";
                    break;

                case "3.5. Поведінка RL-ланки в колі змінного струму":
                    currentHtmlPageName = "Поведінка_RL-ланки.html";
                    currentNode = "3.5. Поведінка RL-ланки";
                    break;

                case "3.6. Види резисторів":
                    currentHtmlPageName = "Види_резисторів.html";
                    currentNode = "3.6. Види резисторів";
                    break;

                case "3.7. Способи збільшення та зменшення загального опору":
                    currentHtmlPageName = "Способи_зміни_опору.html";
                    currentNode = "3.7. Способи зміни опору";
                    break;

                case "3.8. Види котушок індуктивності":
                    currentHtmlPageName = "Види_індуктивностей.html";
                    currentNode = "3.8. Види котушок";
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
                noteHelper.SetDrawingEnabled(false);
                drawingModeHelper.DisableDrawingMode();
            }
        }

        #region --- Tool and Save/Clear Buttons ---

        private void penButton_Click(object sender, EventArgs e) //Олівець
        {
            noteHelper.SetTool("pen", out var penParams);
            drawingModeHelper.SelectTool(penButton, penParams.color, penParams.width);
        }

        private void markerButton_Click(object sender, EventArgs e) //Маркер
        {
            noteHelper.SetTool("marker", out var markerParams);
            drawingModeHelper.SelectTool(markerButton, markerParams.color, markerParams.width);
        }

        private void laserButton_Click(object sender, EventArgs e) //Лазерна вказівка
        {
            noteHelper.SetTool("laser", out var laserParams);
            drawingModeHelper.SelectTool(laserButton, laserParams.color, laserParams.width);
        }

        private void colorButton_Click(object sender, EventArgs e) //Колір
        {
            if (colorDialog1.ShowDialog() != DialogResult.OK) return;

            noteHelper.SetLineColor(colorDialog1.Color);
            drawingModeHelper.UpdateColor(colorDialog1.Color);
        }

        private void widthTextBox_TextChanged(object sender, EventArgs e) //Ширина
        {
            if (string.IsNullOrWhiteSpace(widthTextBox.Text)) return;
        }

        private void widthTextBox_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(widthTextBox.Text))
            {
                widthTextBox.Text = noteHelper.lineWidth.ToString();
            }

            if (float.TryParse(widthTextBox.Text, out float width))
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

        private void saveButton_Click(object sender, EventArgs e) //Зберегти
        {
            try
            {
                noteHelper.SaveNotes(currentNode);

                //hasUnsavedChanges = false;
                //saveStatus.BackColor = Color.LimeGreen;

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

        private void resetButton_Click(object sender, EventArgs e) //Очистити
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

        private void Theory_FormClosing(object sender, FormClosingEventArgs e)
        {
            var menuForm = new Menu();
            menuForm.Show();
        }        
    }
}
