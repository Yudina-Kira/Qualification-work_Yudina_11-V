using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhysicsLabsComplex
{
    public class DrawingModeHelper
    {
        private Buttons[] toolButtons;
        private Buttons saveButton;
        private Buttons resetButton;

        private TextBox widthTextBox;
        private Panel lineColorPanel;
        private Panel saveStatusPanel;

        private Color basicBackColor;
        private Color selectedBackColor;
        private Color saveButtonColor;
        private Color resetButtonColor;

        private Buttons lastSelectedTool;
        private Color toolColor;
        private float toolWidth;

        public DrawingModeHelper(Buttons[] tools, Buttons save, Buttons reset, TextBox width, Panel toolColor, Panel status)
        {
            this.toolButtons = tools;
            this.saveButton = save;
            this.resetButton = reset;

            this.widthTextBox = width;
            this.lineColorPanel = toolColor;
            this.saveStatusPanel = status;

            Initialize();
        }

        // --- Turning the drawing mode on to note ---
        public void EnableDrawingMode()
        {
            EnableDrawingPanel();

            if (lastSelectedTool != null)
            {
                SelectTool(lastSelectedTool, toolColor, toolWidth);
            }
            else
            {
                lastSelectedTool = toolButtons[0];
                SelectTool(lastSelectedTool, toolColor, toolWidth);
                //lineColorPanel.BackColor = Color.Blue;
            }
        }

        // --- Turning the drawing mode off ---
        public void DisableDrawingMode() => DisableDrawingPanel();

        // --- Select toolbutton and remember as last selected tool ---
        public void SelectTool(Buttons tool, Color color, float width)
        {
            if (tool == null) return;

            ClearSelection();

            lastSelectedTool = tool;

            tool.BackColor = selectedBackColor;
            tool.ForeColor = Color.Black;

            UpdateColor(color);
            UpdateWidth(width);
        }

        // --- Show current toolcolor on specific panel
        public void UpdateColor(Color color)
        {
            toolColor = color;
            lineColorPanel.BackColor = color;
        }

        // --- Show current toolwidth on specific textbox
        public void UpdateWidth(float width)
        {
            if (width <= 0)
            {
                widthTextBox.Text = toolWidth.ToString();
                MessageBox.Show("Товщина лінії не може бути менше або дорівнювати 0. \n" 
                    + "Спробуйте, будь ласка, ще раз.", "Помилка", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            toolWidth = width;
            widthTextBox.Text = width.ToString();
        }

        #region --- Private utilities ---

        private void Initialize()
        {
            basicBackColor = Color.FromArgb(87, 181, 195);
            selectedBackColor = Color.FromArgb(60, 210, 230);
            saveButtonColor = Color.LimeGreen;
            resetButtonColor = Color.FromArgb(255, 128, 128);

            UpdateColor(Color.Blue);
            UpdateWidth(6f);

            saveStatusPanel.BackColor = Color.LimeGreen;

            DisableDrawingPanel();
        }

        private void Enable(Color back, params Buttons[] buttons)
        {
            foreach (var btn in buttons)
            {
                btn.Enabled = true;
                btn.BackColor = back;
                btn.Invalidate();
            }
        }

        private void Disable(params Buttons[] buttons)
        {
            foreach (var btn in buttons)
            {
                btn.Enabled = false;
                btn.BackColor = ControlPaint.Dark(btn.BackColor, 0.3f);
                btn.Invalidate();
            }
        }

        private void EnableDrawingPanel()
        {
            Enable(basicBackColor, toolButtons);
            Enable(saveButtonColor, saveButton);
            Enable(resetButtonColor, resetButton);

            widthTextBox.Enabled = true;
        }

        private void DisableDrawingPanel()
        {
            Disable(toolButtons);
            Disable(saveButton);
            Disable(resetButton);

            widthTextBox.Enabled = false;
        }

        private void ClearSelection()
        {
            foreach (var btn in toolButtons)
            {
                btn.BackColor = basicBackColor;
                btn.ForeColor = Color.White;
                btn.Invalidate();
            }

            saveButton.BackColor = saveButtonColor;
            resetButton.BackColor = resetButtonColor;
        }

        #endregion
    }
}
