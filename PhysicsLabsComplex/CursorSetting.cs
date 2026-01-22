using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhysicsLabsComplex
{
    public class CursorSetting
    {
        public static void SetHandCursor(Control control)
        {
            if (control == null) return;

            control.MouseEnter += (s, e) => control.Cursor = Cursors.Hand;

            control.MouseLeave += (s, e) => control.Cursor = Cursors.Default;
        }
    }
}
