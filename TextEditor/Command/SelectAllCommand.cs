using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TextEditor.Features.Macro;

namespace TextEditor.Command
{
    public class SelectAllCommand : ICommand
    {
        private readonly TextBoxBase _editor;
        private readonly MacroRecorder _recorder;

        public SelectAllCommand(TextBoxBase editor, MacroRecorder recorder)
        {
            _editor = editor;
            _recorder = recorder;
        }

        public void Execute()
        {
            _editor.SelectAll();

            if (_recorder != null && _recorder.IsRecording)
                _recorder.RecordCommand("SelectAll");
        }
    }
}
