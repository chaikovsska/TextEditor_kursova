using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TextEditor.Composite.Gutter
{
    public class GutterService
    {
        public List<IGutterLayer> Layers { get; } = new List<IGutterLayer>();

        private readonly PictureBox _gutter;
        private readonly RichTextBox _editor;

        public GutterService(PictureBox gutter, RichTextBox editor)
        {
            _gutter = gutter;
            _editor = editor;
        }

        public void AddLayer(IGutterLayer layer)
        {
            Layers.Add(layer);
        }

        public void Paint(Graphics g)
        {
            g.Clear(_gutter.BackColor);

            int firstCharIndex = _editor.GetCharIndexFromPosition(new Point(0, 0));
            int firstLine = _editor.GetLineFromCharIndex(firstCharIndex);

            int lastCharIndex = _editor.GetCharIndexFromPosition(new Point(0, _gutter.Height));
            int lastLine = _editor.GetLineFromCharIndex(lastCharIndex);

            if (lastLine < firstLine) lastLine = _editor.Lines.Length;

            foreach (var layer in Layers)
            {
                layer.Draw(g, _editor, firstLine, lastLine + 1, _gutter.Height);
            }
        }
    }
}