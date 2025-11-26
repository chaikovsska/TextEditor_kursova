using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TextEditor.Composite.Gutter
{
    public interface IGutterLayer
    {
        void Draw(Graphics g, RichTextBox editor, int firstLine, int lastLine, int visibleHeight);
    }
}
