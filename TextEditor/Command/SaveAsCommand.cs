using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TextEditor.Features.Macro;
using TextEditor.Services;

namespace TextEditor.Command
{
    public class SaveAsCommand : ICommand
    {
        private readonly FileService _fileService;
        private readonly TextBoxBase _editor;
        private readonly MacroRecorder _recorder;

        public SaveAsCommand(FileService fileService, TextBoxBase editor, MacroRecorder recorder)
        {
            _fileService = fileService;
            _editor = editor;
            _recorder = recorder;
        }

        public void Execute()
        {
            _fileService.SaveFileAs();

            if (_recorder?.IsRecording == true)
                _recorder.RecordCommand("SaveAs");
        }
    }
}
