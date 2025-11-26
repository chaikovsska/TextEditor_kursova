using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TextEditor.Features.Macro;

namespace TextEditor.Command
{
    public class PasteCommand : ICommand
    {
        private readonly TextBoxBase _editor;
        private readonly MacroRecorder _recorder;

        public PasteCommand(TextBoxBase editor, MacroRecorder recorder)
        {
            _editor = editor;
            _recorder = recorder;
        }

        public void Execute()
        {
            int start = _editor.SelectionStart;
            string pasteText = Clipboard.GetText();
            _editor.Text = _editor.Text.Insert(start, pasteText);
            _editor.SelectionStart = start + pasteText.Length;

            if (_recorder != null && _recorder.IsRecording)
                _recorder.RecordCommand("Paste");
        }
    }
}
