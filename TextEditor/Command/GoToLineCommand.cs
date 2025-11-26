using System.Windows.Forms;
using TextEditor.Features.Macro;

namespace TextEditor.Command
{
    public class GoToLineCommand : ICommand
    {
        private readonly MainForm _form;
        private readonly TextBoxBase _editor;

        public GoToLineCommand(MainForm form, TextBoxBase editor)
        {
            _form = form;
            _editor = editor;
        }

        public void Execute()
        {
            if (_editor == null) return;

            int totalLines = _editor.GetLineFromCharIndex(_editor.TextLength) + 1;

            int currentLine = _editor.GetLineFromCharIndex(_editor.SelectionStart) + 1;

            using (var dlg = new GoToLineForm(currentLine, totalLines))
            {
                if (dlg.ShowDialog(_form) == DialogResult.OK)
                {
                    int targetLine = dlg.SelectedLine - 1; 

                    if (targetLine >= 0 && targetLine < _editor.Lines.Length)
                    {
                        int charIndex = _editor.GetFirstCharIndexFromLine(targetLine);

                        if (charIndex == -1)
                        {
                            charIndex = _editor.TextLength;
                        }

                        _editor.SelectionStart = charIndex;
                        _editor.SelectionLength = 0;
                        _editor.ScrollToCaret();
                        _editor.Focus();
                    }
                }
            }
        }
    }
}