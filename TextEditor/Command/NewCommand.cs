using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextEditor.Features.Macro;

namespace TextEditor
{
    public class NewCommand : ICommand
    {
        private MainForm _form;
        private MacroRecorder _recorder;

        public NewCommand(MainForm form, MacroRecorder recorder)
        {
            _form = form;
            _recorder = recorder;
        }

        public void Execute()
        {
            _form.CreateNewTab();

            if (_recorder != null && _recorder.IsRecording)
                _recorder.RecordCommand("New");
        }
    }
}
