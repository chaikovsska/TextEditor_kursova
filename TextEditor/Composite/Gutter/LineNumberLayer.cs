using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TextEditor.Composite.Gutter
{
    public class LineNumberLayer : IGutterLayer
    {
        private readonly Font _baseFont;
        private readonly Brush _textBrush;

        public LineNumberLayer()
        {
            _baseFont = new Font("Consolas", 11);
            _textBrush = Brushes.Gray;
        }

        public void Draw(Graphics g, RichTextBox editor, int firstLine, int lastLine, int visibleHeight)
        {
            float zoom = editor.ZoomFactor;

            using (Font scaledFont = new Font(_baseFont.FontFamily, _baseFont.Size * zoom, _baseFont.Style))
            {
                for (int line = firstLine; line <= lastLine; line++)
                {
                    int charIndex = editor.GetFirstCharIndexFromLine(line);
                    if (charIndex == -1) continue;

                    Point pos = editor.GetPositionFromCharIndex(charIndex);
                    int y = pos.Y;

                    if (y < -50 || y > visibleHeight + 50) continue;

                    string text = (line + 1).ToString();
                    var size = g.MeasureString(text, scaledFont);

                    float x = (45 * zoom) - size.Width;
                    if (x < 2) x = 2;

                    g.DrawString(text, scaledFont, _textBrush, x, y);
                }
            }
        }
    }
}
