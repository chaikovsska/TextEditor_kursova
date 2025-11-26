using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TextEditor.Features.Macro;

namespace TextEditor.Command
{
    public class CopyCommand : ICommand
    {
        private TextBoxBase _editor;
        private MacroRecorder _recorder;

        public CopyCommand(TextBoxBase editor, MacroRecorder recorder)
        {
            _editor = editor;
            _recorder = recorder;
        }

        public void Execute()
        {
            if (!string.IsNullOrEmpty(_editor.SelectedText))
                Clipboard.SetText(_editor.SelectedText);

            if (_recorder != null && _recorder.IsRecording)
                _recorder.RecordCommand("Copy");
        }
    }
}
