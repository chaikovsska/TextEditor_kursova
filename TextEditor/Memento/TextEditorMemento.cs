using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEditor.Memento
{
    public class TextEditorMemento
    {
        public string Text { get; }
        public int SelectionStart { get; }
        public int SelectionLength { get; }
        public int ScrollX { get; }
        public int ScrollY { get; }

        public TextEditorMemento(string text, int selectionStart, int selectionLength, int scrollX, int scrollY)
        {
            Text = text ?? string.Empty;
            SelectionStart = selectionStart;
            SelectionLength = selectionLength;
            ScrollX = scrollX;
            ScrollY = scrollY;
        }

        public override bool Equals(object obj)
        {
            if (obj is TextEditorMemento other)
            {
                return this.SelectionStart == other.SelectionStart &&
                       string.Equals(this.Text, other.Text, StringComparison.Ordinal);
            }
            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + (Text?.GetHashCode() ?? 0);
                hash = hash * 23 + SelectionStart.GetHashCode();
                return hash;
            }
        }
    }
}
