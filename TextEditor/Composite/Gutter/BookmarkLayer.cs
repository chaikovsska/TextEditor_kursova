using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace TextEditor.Composite.Gutter 
{
    public class BookmarkLayer : IGutterLayer
    {
        private HashSet<int> _bookmarks;

        public BookmarkLayer(HashSet<int> bookmarks)
        {
            _bookmarks = bookmarks;
        }

        public void UpdateBookmarks(IEnumerable<int> newBookmarks)
        {
            _bookmarks = new HashSet<int>(newBookmarks);
        }

        public void Draw(Graphics g, RichTextBox editor, int firstLine, int lastLine, int visibleHeight)
        {
            float zoom = editor.ZoomFactor;

            int xOffset = (int)(2 * zoom);
            int width = (int)(12 * zoom);
            int height = (int)(14 * zoom);
            int cutout = (int)(4 * zoom);

            for (int line = firstLine; line <= lastLine; line++)
            {
                if (_bookmarks.Contains(line))
                {
                    int charIndex = editor.GetFirstCharIndexFromLine(line);
                    if (charIndex == -1) continue;

                    Point pos = editor.GetPositionFromCharIndex(charIndex);
                    int y = pos.Y;

                    if (y < -50 || y > visibleHeight + 50) continue;

                    int drawY = y + (int)(1 * zoom);

                    Point[] bookmarkShape = {
                        new Point(xOffset, drawY),
                        new Point(xOffset + width, drawY),
                        new Point(xOffset + width, drawY + height),
                        new Point(xOffset + width / 2, drawY + height - cutout),
                        new Point(xOffset, drawY + height)
                    };

                    g.FillPolygon(Brushes.DodgerBlue, bookmarkShape);
                }
            }
        }
    }
}