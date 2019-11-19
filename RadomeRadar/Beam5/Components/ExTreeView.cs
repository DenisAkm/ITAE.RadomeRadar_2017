using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apparat
{
    public class ExTreeView : TreeView
    {
        private const int WM_LBUTTONDBLCLK = 0x0203;
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_LBUTTONDBLCLK)
            {
                var info = this.HitTest(PointToClient(Cursor.Position));
                if (info.Location == TreeViewHitTestLocations.StateImage)
                {
                    m.Result = IntPtr.Zero;
                    return;
                }
            }
            base.WndProc(ref m);
        }
        protected override void DefWndProc(ref Message m)
        {
            if (m.Msg == 515)
            { /* WM_LBUTTONDBLCLK */
            }
            else
            {
                base.DefWndProc(ref m);
            }
        }
    }
}
