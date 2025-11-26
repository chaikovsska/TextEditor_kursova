using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TextEditor.Command;
using TextEditor.Features.Macro;
using TextEditor.Services;

namespace TextEditor
{
    public class EditorMediator : IEditorMediator
    {
        private readonly MainForm _form;
        private readonly TabManager _tabManager;
        private readonly MacroRecorder _recorder;
        private readonly FileService _fileService;
        private readonly HighlighterService _highlighterService;

        private readonly Dictionary<string, Func<ICommand>> _commands;

        public EditorMediator(MainForm form, TabManager tabManager, MacroRecorder recorder, FileService fileService, HighlighterService highlighterService)
        {
            _form = form;
            _tabManager = tabManager;
            _recorder = recorder;
            _fileService = fileService;
            _highlighterService = highlighterService;

            _commands = new Dictionary<string, Func<ICommand>>
            {
                { "New",    () => new NewCommand(_form, _recorder) },
                { "Open",   () => new OpenCommand(_fileService, _recorder) },
                { "Save",   () => new SaveCommand(_fileService, _tabManager.GetActiveEditor(), _recorder) },
                { "SaveAs", () => new SaveAsCommand(_fileService, _tabManager.GetActiveEditor(), _recorder) },
                { "Exit",   () => new ExitCommand(_form) },

                { "Cut",       () => new CutCommand(_tabManager.GetActiveEditor(), _recorder) },
                { "Copy",      () => new CopyCommand(_tabManager.GetActiveEditor(), _recorder) },
                { "Paste",     () => new PasteCommand(_tabManager.GetActiveEditor(), _recorder) },
                { "SelectAll", () => new SelectAllCommand(_tabManager.GetActiveEditor(), _recorder) },

                { "GoTo",    () => new GoToLineCommand(_form, _tabManager.GetActiveEditor()) },
                { "Find",    () => new FindCommand(_form, _tabManager.GetActiveEditor()) },
                { "ZoomIn",  () => new ZoomInCommand(_tabManager.GetActiveEditor()) },
                { "ZoomOut", () => new ZoomOutCommand(_tabManager.GetActiveEditor()) }
            };
        }

        public void Notify(string eventCode, object sender = null)
        {
            var invoker = _tabManager.GetActiveInvoker();

            if (eventCode == "Undo")
            {
                if (invoker?.CanUndo == true)
                {
                    invoker.Undo();

                    var editor = _tabManager.GetActiveEditor();
                    if (editor != null)
                    {
                        _highlighterService.ForceRefresh(editor);
                    }
                }
                return;
            }

            if (eventCode == "Redo")
            {
                if (invoker?.CanRedo == true)
                {
                    invoker.Redo();

                    var editor = _tabManager.GetActiveEditor();
                    if (editor != null)
                        _highlighterService.RecalculateTokens(editor);
                        _highlighterService.HighlightVisibleArea(editor);
                }
                return;
            }

            if (_commands.TryGetValue(eventCode, out var factory))
            {
                var command = factory();
                if (command == null) return;

                bool modifiesText = eventCode == "Cut" || eventCode == "Paste";

                if (invoker != null && modifiesText)
                    invoker.Execute(command);
                else
                    command.Execute();
            }
        }
    }
}
