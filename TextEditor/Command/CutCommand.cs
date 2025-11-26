using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TextEditor.Features.Macro;

namespace TextEditor.Command
{
    public class CutCommand : ICommand
    {
        private readonly TextBoxBase _editor;
        private readonly MacroRecorder _recorder;

        public CutCommand(TextBoxBase editor, MacroRecorder recorder)
        {
            _editor = editor;
            _recorder = recorder;
        }

        public void Execute()
        {
            if (!string.IsNullOrEmpty(_editor.SelectedText))
            {
                Clipboard.SetText(_editor.SelectedText);
                int start = _editor.SelectionStart;
                _editor.Text = _editor.Text.Remove(start, _editor.SelectionLength);
                _editor.SelectionStart = start;
            }

            if (_recorder != null && _recorder.IsRecording)
                _recorder.RecordCommand("Cut");
        }
    }
}
