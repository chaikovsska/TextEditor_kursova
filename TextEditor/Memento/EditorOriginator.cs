using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TextEditor.Memento
{
    public class EditorOriginator
    {
        private readonly RichTextBox _editor;

        public EditorOriginator(TextBoxBase editor)
        {
            _editor = (RichTextBox)editor;
        }

        public TextEditorMemento SaveState()
        {
            var text = _editor.Text ?? string.Empty;
            var selStart = _editor.SelectionStart;
            var selLength = _editor.SelectionLength;

            Point scroll = _editor.GetScrollPos();

            return new TextEditorMemento(text, selStart, selLength, scroll.X, scroll.Y);
        }

        public void RestoreState(TextEditorMemento m)
        {
            if (m == null) return;

            _editor.Text = m.Text ?? string.Empty;

            _editor.SelectionStart = Math.Max(0, Math.Min(m.SelectionStart, _editor.Text.Length));
            _editor.SelectionLength = Math.Max(0, Math.Min(m.SelectionLength, _editor.Text.Length - _editor.SelectionStart));
            _editor.SetScrollPos(new Point(m.ScrollX, m.ScrollY));
        }
    }
}
