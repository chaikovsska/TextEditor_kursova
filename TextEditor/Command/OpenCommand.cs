using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextEditor.Features.Macro;
using TextEditor.Services;

namespace TextEditor.Command
{
    public class OpenCommand : ICommand
    {
        private readonly FileService _fileService;
        private readonly MacroRecorder _recorder;

        public OpenCommand(FileService fileService, MacroRecorder recorder)
        {
            _fileService = fileService;
            _recorder = recorder;
        }

        public void Execute()
        {
            _fileService.OpenFile();

            if (_recorder?.IsRecording == true)
                _recorder.RecordCommand("Open");
        }
    }
}
