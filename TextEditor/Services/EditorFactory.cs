using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TextEditor.Composite.Gutter;
using TextEditor.Features.Bookmarks;
using System.Collections.Generic; 

namespace TextEditor.Services
{
    public static class EditorFactory
    {
        public static (RichTextBox editor, PictureBox gutter) CreateEditorComponents(string content, HashSet<int> bookmarks)
        {
            var gutter = new PictureBox
            {
                Dock = DockStyle.Left,
                Width = 50,
                BackColor = Color.FromArgb(240, 240, 240)
            };

            var editor = new RichTextBox
            {
                Dock = DockStyle.Fill,
                Multiline = true,
                AcceptsTab = true,
                WordWrap = false,
                Font = new Font("Consolas", 11),
                Text = content,
                ScrollBars = RichTextBoxScrollBars.Both,
                DetectUrls = false,
                HideSelection = false,
                BorderStyle = BorderStyle.None
            };

            var gutterService = new GutterService(gutter, editor);
            gutterService.AddLayer(new LineNumberLayer());
            gutterService.AddLayer(new BookmarkLayer(bookmarks));

            editor.Tag = new EditorMetadata
            {
                IsModified = false,
                FileEncoding = Encoding.UTF8,
                Gutter = gutter,
                GutterService = gutterService
            };
            gutter.Tag = editor;

            gutter.Paint += (s, e) => gutterService.Paint(e.Graphics);

            return (editor, gutter);
        }
    }
}