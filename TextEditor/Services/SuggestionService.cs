using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace TextEditor.Services
{
    public class SuggestionService
    {
        private readonly ListBox _view;
        private readonly Form _mainForm;
        private readonly Dictionary<string, List<string>> _keywordsByExtension = new Dictionary<string, List<string>>();

        public SuggestionService(Form mainForm, ListBox listBox)
        {
            _mainForm = mainForm;
            _view = listBox;
            InitializeDictionaries();
        }

        public bool IsVisible => _view.Visible;
        public int SelectedIndex
        {
            get => _view.SelectedIndex;
            set => _view.SelectedIndex = value;
        }
        public int ItemsCount => _view.Items.Count;

        public void Show(RichTextBox editor)
        {
            string ext = ".txt";
            if (editor.Tag is EditorMetadata meta && !string.IsNullOrEmpty(meta.FilePath))
            {
                ext = Path.GetExtension(meta.FilePath).ToLower();
            }

            List<string> targetList;
            if (_keywordsByExtension.ContainsKey(ext))
                targetList = _keywordsByExtension[ext];
            else if (_keywordsByExtension.ContainsKey(".cs"))
                targetList = _keywordsByExtension[".cs"];
            else
                return;

            string word = GetCurrentWord(editor);
            if (word.Length < 1)
            {
                _view.Visible = false;
                return;
            }

            var filtered = targetList
                .Where(x => x.StartsWith(word, StringComparison.OrdinalIgnoreCase))
                .OrderBy(x => x)
                .ToArray();

            if (filtered.Length == 0)
            {
                _view.Visible = false;
                return;
            }

            _view.Items.Clear();
            _view.Items.AddRange(filtered);
            PositionSuggestions(editor);
            _view.Visible = true;
        }

        public void Insert(RichTextBox editor)
        {
            if (_view.SelectedItem == null) return;

            string word = GetCurrentWord(editor);
            string selectedText = _view.SelectedItem.ToString();

            int pos = editor.SelectionStart;
            int start = pos - word.Length;

            editor.Select(start, word.Length);
            editor.SelectedText = selectedText;

            _view.Visible = false;
            editor.Focus();
        }

        public void MoveSelection(int direction)
        {
            if (!_view.Visible) return;
            int newIndex = _view.SelectedIndex + direction;
            if (newIndex >= 0 && newIndex < _view.Items.Count)
                _view.SelectedIndex = newIndex;
        }

        public void Hide() => _view.Visible = false;

        public void HandleClick(object sender, EventArgs e)
        {
        }

        private string GetCurrentWord(RichTextBox editor)
        {
            int pos = editor.SelectionStart;
            int start = pos;
            while (start > 0 && (Char.IsLetterOrDigit(editor.Text[start - 1]) || editor.Text[start - 1] == '_'))
                start--;

            return editor.Text.Substring(start, pos - start);
        }

        private void PositionSuggestions(RichTextBox editor)
        {
            int index = editor.SelectionStart;
            Point local = editor.GetPositionFromCharIndex(index);
            local.Y += (int)editor.Font.Height + 2;

            Point screen = editor.PointToScreen(local);
            Point formPoint = _mainForm.PointToClient(screen);

            if (formPoint.X + _view.Width > _mainForm.ClientSize.Width)
                formPoint.X = _mainForm.ClientSize.Width - _view.Width - 10;

            if (formPoint.Y + _view.Height > _mainForm.ClientSize.Height)
                formPoint.Y = formPoint.Y - _view.Height - editor.Font.Height - 4;

            if (formPoint.Y < 0) formPoint.Y = 0;

            _view.Location = formPoint;
            _view.BringToFront();
        }

        private void InitializeDictionaries()
        {
            // C#
            var csharp = new List<string> {
                "abstract", "as", "base", "bool", "break", "byte", "case", "catch", "char", "checked",
                "class", "const", "continue", "decimal", "default", "delegate", "do", "double", "else",
                "enum", "event", "explicit", "extern", "false", "finally", "fixed", "float", "for",
                "foreach", "goto", "if", "implicit", "in", "int", "interface", "internal", "is", "lock",
                "long", "namespace", "new", "null", "object", "operator", "out", "override", "params",
                "private", "protected", "public", "readonly", "ref", "return", "sbyte", "sealed",
                "short", "sizeof", "stackalloc", "static", "string", "struct", "switch", "this", "throw",
                "true", "try", "typeof", "uint", "ulong", "unchecked", "unsafe", "ushort", "using",
                "virtual", "void", "volatile", "while", "var", "dynamic", "async", "await", "Console",
                "WriteLine", "ReadLine", "List", "Dictionary", "System", "Task", "Thread", "Exception"
            };
            _keywordsByExtension[".cs"] = csharp;

            // Python
            var python = new List<string> {
                "def", "class", "if", "elif", "else", "while", "for", "try", "except", "finally",
                "with", "as", "import", "from", "return", "yield", "break", "continue", "pass",
                "raise", "global", "nonlocal", "lambda", "True", "False", "None", "and", "or",
                "not", "is", "in", "print", "len", "range", "open", "str", "int", "float", "list",
                "dict", "set", "tuple", "super", "self", "__init__"
            };
            _keywordsByExtension[".py"] = python;

            // Java
            var java = new List<string> {
                "public", "private", "protected", "class", "interface", "enum", "extends", "implements",
                "static", "final", "void", "int", "double", "boolean", "char", "new", "return",
                "if", "else", "switch", "case", "default", "while", "do", "for", "break", "continue",
                "try", "catch", "finally", "throw", "throws", "package", "import", "this", "super",
                "System", "out", "println", "String", "List", "ArrayList", "Map", "HashMap"
            };
            _keywordsByExtension[".java"] = java;

            // JavaScript / TypeScript
            var js = new List<string> {
                "function", "var", "let", "const", "if", "else", "switch", "case", "default",
                "for", "while", "do", "break", "continue", "return", "try", "catch", "finally",
                "throw", "new", "this", "class", "extends", "super", "import", "export", "default",
                "async", "await", "null", "undefined", "true", "false", "NaN", "console", "log",
                "document", "window", "alert", "prompt", "JSON", "parseInt", "parseFloat", "map", "filter"
            };
            _keywordsByExtension[".js"] = js;
            _keywordsByExtension[".ts"] = js;

            // HTML
            var html = new List<string> {
                "html", "head", "body", "title", "div", "span", "p", "h1", "h2", "h3", "h4", "h5", "h6",
                "a", "img", "ul", "ol", "li", "table", "tr", "td", "th", "form", "input", "button",
                "label", "select", "option", "textarea", "style", "script", "link", "meta", "br", "hr",
                "class", "id", "src", "href", "style", "type", "value", "placeholder"
            };
            _keywordsByExtension[".html"] = html;
            _keywordsByExtension[".htm"] = html;

            // C++
            var cpp = new List<string> {
                "auto", "break", "case", "char", "const", "continue", "default", "do", "double",
                "else", "enum", "extern", "float", "for", "goto", "if", "int", "long", "register",
                "return", "short", "signed", "sizeof", "static", "struct", "switch", "typedef",
                "union", "unsigned", "void", "volatile", "while", "asm", "bool", "catch", "class",
                "const_cast", "delete", "dynamic_cast", "explicit", "export", "false", "friend",
                "inline", "mutable", "namespace", "new", "operator", "private", "protected",
                "public", "reinterpret_cast", "static_cast", "template", "this", "throw", "true",
                "try", "typeid", "typename", "using", "virtual", "wchar_t", "std", "cout", "cin", "endl"
            };
            _keywordsByExtension[".cpp"] = cpp;
            _keywordsByExtension[".h"] = cpp;

            // SQL
            var sql = new List<string> {
                "SELECT", "FROM", "WHERE", "INSERT", "INTO", "VALUES", "UPDATE", "SET", "DELETE",
                "CREATE", "TABLE", "DROP", "ALTER", "INDEX", "JOIN", "INNER", "LEFT", "RIGHT",
                "OUTER", "ON", "GROUP", "BY", "ORDER", "HAVING", "LIMIT", "DISTINCT", "AS",
                "AND", "OR", "NOT", "NULL", "PRIMARY", "KEY", "FOREIGN", "DEFAULT", "CHECK"
            };
            _keywordsByExtension[".sql"] = sql;
        }
    }
}