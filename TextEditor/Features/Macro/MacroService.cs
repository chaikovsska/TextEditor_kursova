using System;
using System.Collections.Generic;
using System.Windows.Forms; 
using TextEditor.Database;

namespace TextEditor.Features.Macro
{
    public class MacroService
    {
        private readonly IMacroRepository _repo;

        public MacroService(IMacroRepository repo)
        {
            _repo = repo;
        }

        public void SaveMacro(Macro macro) => _repo.Save(macro);
        public List<Macro> GetAll() => _repo.GetAll();
        public void DeleteMacro(int id) => _repo.Delete(id);

        public void RunMacro(Macro macro, RichTextBox editor, Action<string> executeCommand)
        {
            if (macro == null || editor == null) return;

            int baseline = editor.SelectionStart;
            foreach (var act in macro.Actions)
            {
                int targetPos = baseline + act.Position;
                if (targetPos < 0) targetPos = 0;
                if (targetPos > editor.TextLength) targetPos = editor.TextLength;

                switch (act.Type)
                {
                    case MacroActionType.InsertText:
                        editor.SelectionStart = targetPos;
                        editor.SelectedText = act.Text;
                        break;

                    case MacroActionType.DeleteText:
                        editor.SelectionStart = targetPos;
                        editor.SelectionLength = act.Length;
                        editor.SelectedText = "";
                        break;

                    case MacroActionType.MoveCursor:
                        editor.SelectionStart = targetPos;
                        editor.ScrollToCaret();
                        break;

                    case MacroActionType.ExecuteCommand:
                        executeCommand?.Invoke(act.CommandName);

                        if (act.CommandName == "InsertDate")
                        {
                            editor.SelectedText = DateTime.Now.ToString();
                        }
                        break;
                }
            }
        }
    }
}