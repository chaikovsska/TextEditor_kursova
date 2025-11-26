using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TextEditor
{
    public class EditorMetadata
    {
        public string FilePath { get; set; }
        public bool IsModified { get; set; }
        public Encoding FileEncoding { get; set; } = Encoding.UTF8;
        public PictureBox Gutter { get; set; }
        public TextEditor.Composite.Gutter.GutterService GutterService { get; set; }

        public bool IsPinned { get; set; } = false;
    }
}
