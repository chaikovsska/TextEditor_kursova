using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TextEditor.Command
{
    public class ZoomInCommand : ICommand
    {
        private readonly TextBoxBase _editor;
        private const int BaseGutterWidth = 50; 

        public ZoomInCommand(TextBoxBase editor) => _editor = editor;

        public void Execute()
        {
            if (_editor is RichTextBox rtb)
            {
                if (rtb.ZoomFactor < 5.0f)
                {
                    rtb.ZoomFactor += 0.2f;

                    if (rtb.Tag is EditorMetadata meta && meta.Gutter != null)
                    {
                        meta.Gutter.Width = (int)(BaseGutterWidth * rtb.ZoomFactor);
                        meta.Gutter.Invalidate();
                    }
                }
            }
        }
    }
}
