using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextEditor.Memento;

namespace TextEditor.Command
{
    public class CommandInvoker
    {
        private readonly Stack<TextEditorMemento> _undoStack = new Stack<TextEditorMemento>();
        private readonly Stack<TextEditorMemento> _redoStack = new Stack<TextEditorMemento>();
        private EditorOriginator _originator;

        public bool IsRestoring { get; private set; } 

        public CommandInvoker(EditorOriginator originator)
        {
            _originator = originator;
        }

        public bool CanUndo => _undoStack.Count > 0;
        public bool CanRedo => _redoStack.Count > 0;

        public void TrackState()
        {
            if (IsRestoring) return;

            var snap = _originator.SaveState();
            if (_undoStack.Count == 0 || !_undoStack.Peek().Equals(snap))
            {
                _undoStack.Push(snap);
                _redoStack.Clear(); 
            }
        }

        public void Execute(ICommand command, bool trackState = true)
        {
            if (trackState)
            {
                var snap = _originator.SaveState();
                if (_undoStack.Count == 0 || !_undoStack.Peek().Equals(snap))
                    _undoStack.Push(snap);
                _redoStack.Clear();
            }

            command.Execute();
        }

        public void Undo()
        {
            if (_undoStack.Count > 0)
            {
                IsRestoring = true;
                var current = _originator.SaveState();
                var previous = _undoStack.Pop();
                _redoStack.Push(current);
                _originator.RestoreState(previous);
                IsRestoring = false;
            }
        }

        public void Redo()
        {
            if (_redoStack.Count > 0)
            {
                IsRestoring = true;
                var current = _originator.SaveState();
                var next = _redoStack.Pop();
                _undoStack.Push(current);
                _originator.RestoreState(next);
                IsRestoring = false;
            }
        }

        public void UpdateOriginator(EditorOriginator originator)
        {
            _originator = originator;
        }
    }
}
