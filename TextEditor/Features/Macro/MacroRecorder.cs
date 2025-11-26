using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEditor.Features.Macro
{
    public class MacroRecorder
    {
        public bool IsRecording { get; private set; }
        private List<MacroAction> _actions = new List<MacroAction>();

        private int _startOffset = 0;

        public void Start(int currentPosition)
        {
            IsRecording = true;
            _actions.Clear();
            _startOffset = currentPosition; 
        }

        public List<MacroAction> Stop()
        {
            IsRecording = false;
            return new List<MacroAction>(_actions);
        }

        public void Insert(int pos, string text)
        {
            if (!IsRecording) return;

            _actions.Add(new MacroAction
            {
                Type = MacroActionType.InsertText,
                Position = pos - _startOffset,
                Text = text
            });
        }

        public void Delete(int pos, int len)
        {
            if (!IsRecording) return;

            _actions.Add(new MacroAction
            {
                Type = MacroActionType.DeleteText,
                Position = pos - _startOffset,
                Length = len
            });
        }

        public void MoveCursor(int pos)
        {
            if (!IsRecording) return;

            _actions.Add(new MacroAction
            {
                Type = MacroActionType.MoveCursor,
                Position = pos - _startOffset
            });
        }

        public void RecordCommand(string commandName)
        {
            if (!IsRecording) return;

            _actions.Add(new MacroAction
            {
                Type = MacroActionType.ExecuteCommand,
                CommandName = commandName,
                Position = 0 
            });
        }
    }
}
