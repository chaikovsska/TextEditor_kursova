using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TextEditor
{
    public static class RichTextBoxExtensions
    {
        private const int WM_USER = 0x0400;
        private const int EM_SETEVENTMASK = WM_USER + 69;
        private const int WM_SETREDRAW = 0x0B;
        private const int EM_GETSCROLLPOS = WM_USER + 221;
        private const int EM_SETSCROLLPOS = WM_USER + 222;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, ref Point lParam);

        private static IntPtr _oldEventMask;

        public static void BeginUpdate(this RichTextBox rtb)
        {
            SendMessage(rtb.Handle, WM_SETREDRAW, IntPtr.Zero, IntPtr.Zero);

            _oldEventMask = SendMessage(rtb.Handle, EM_SETEVENTMASK, IntPtr.Zero, IntPtr.Zero);
        }

        public static void EndUpdate(this RichTextBox rtb)
        {
            SendMessage(rtb.Handle, EM_SETEVENTMASK, IntPtr.Zero, _oldEventMask);

            SendMessage(rtb.Handle, WM_SETREDRAW, (IntPtr)1, IntPtr.Zero);

            rtb.Invalidate();
        }

        public static Point GetScrollPos(this RichTextBox rtb)
        {
            Point pt = new Point();
            SendMessage(rtb.Handle, EM_GETSCROLLPOS, IntPtr.Zero, ref pt);
            return pt;
        }

        public static void SetScrollPos(this RichTextBox rtb, Point pt)
        {
            SendMessage(rtb.Handle, EM_SETSCROLLPOS, IntPtr.Zero, ref pt);
        }
    }
}
