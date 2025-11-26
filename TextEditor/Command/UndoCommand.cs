using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TextEditor.Features.Macro;

namespace TextEditor.Command
{
    public class UndoCommand : ICommand
    {
        private readonly CommandInvoker _invoker;
        private readonly MacroRecorder _recorder;

        public UndoCommand(CommandInvoker invoker, MacroRecorder recorder)
        {
            _invoker = invoker;
            _recorder = recorder;
        }

        public void Execute()
        {
            _invoker.Undo();

            if (_recorder != null && _recorder.IsRecording)
                _recorder.RecordCommand("Undo");
        }
    }
}
