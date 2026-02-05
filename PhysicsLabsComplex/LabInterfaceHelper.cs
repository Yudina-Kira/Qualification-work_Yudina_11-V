using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhysicsLabsComplex
{
    internal class LabInterfaceHelper
    {
        private ToolTip toolTip;
        private Control invoker;

        public LabInterfaceHelper(ToolTip toolTip, Control invoker)
        {
            this.toolTip = toolTip;
            this.invoker = invoker;
        }

        public void ShowTimedTooltip(Control ctrl, string txt, int duration = 2500)
        {
            if (invoker.InvokeRequired)
            {
                invoker.Invoke(new Action(() => ShowTimedTooltip(ctrl, txt, duration)));
                return;
            }

            toolTip.Show(txt, ctrl, ctrl.Width / 2, ctrl.Height / 2, duration);
        }

        public void SetIndicator(Cards indicator, Color color, string tooltipText)
        {
            if (invoker.InvokeRequired)
            {
                invoker.Invoke(new Action(() => SetIndicator(indicator, color, tooltipText)));
                return;
            }

            indicator.BackColor = color;
            toolTip.SetToolTip(indicator, tooltipText);
        }

        public void SetSeriesSelector(ComboBox cmb, params string[] comboItems)
        {
            if (invoker.InvokeRequired)
            {
                invoker.Invoke(new Action(() => SetSeriesSelector(cmb, comboItems)));
                return;
            }

            cmb.Items.Clear();

            if (comboItems == null || comboItems.Length < 2)
            {
                cmb.Visible = false;
                return;
            }

            cmb.Items.AddRange(comboItems);
            cmb.SelectedIndex = 0;
            cmb.Visible = true;
        }

        public bool TryGetPositiveDouble(TextBox textBox, string name, out double value, double min = 0, double max = 1000)
        {
            if (!double.TryParse(textBox.Text, NumberStyles.Float, CultureInfo.CurrentCulture, out value) || value <= min || value > max)
            {
                MessageBox.Show($"Некоректне значення параметра «{name}».","Помилка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                textBox.Focus();
                textBox.SelectAll();
                return false;
            }

            return true;
        }
    }
}
