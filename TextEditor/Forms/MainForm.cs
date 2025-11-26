using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using TextEditor.Command;
using TextEditor.Database;
using TextEditor.Features.Bookmarks;
using TextEditor.Features.Macro;
using TextEditor.Features.Snippets;
using TextEditor.Services;

namespace TextEditor
{
    public partial class MainForm : Form
    {
        private readonly TabManager _tabManager;
        private readonly SuggestionService _suggestionService;
        private readonly FileService _fileService;
        private readonly SnippetService _snippetService;
        private readonly MacroService _macroService;
        private readonly HighlighterService _highlighterService;
        private readonly IEditorMediator _mediator;

        private readonly MacroRecorder _recorder;
        private readonly HotkeyService _hotkeyService;
        private readonly System.Windows.Forms.Timer _textChangeTimer;
        private readonly System.Windows.Forms.Timer _scrollTimer;
        private bool _textChangedPending = false;

        private readonly ListBox suggestions;

        private const int WM_VSCROLL = 0x115;
        private const int WM_MOUSEWHEEL = 0x20A;

        private readonly Dictionary<TextBoxBase, CommandInvoker> _invokers =
            new Dictionary<TextBoxBase, CommandInvoker>();

        private readonly IMacroRepository _macroRepository;
        private Macro currentRecordedMacro;

        public MainForm()
        {
            InitializeComponent();

            suggestions = new ListBox
            {
                Visible = false,
                Font = new Font("Consolas", 11),
                Size = new Size(220, 150)
            };
            suggestions.Click += Suggestions_Click;
            this.Controls.Add(suggestions);
            suggestions.BringToFront();

            var db = new DatabaseService();
            var bookmarkRepo = new SqlBookmarkRepository(db);
            _macroRepository = new SqlMacroRepository(db);
            var snippetRepo = new SqlSnippetRepository(db);

            _recorder = new MacroRecorder();
            _macroService = new MacroService(_macroRepository);
            _hotkeyService = new HotkeyService(_macroService.GetAll());

            _snippetService = new SnippetService(snippetRepo);
            _suggestionService = new SuggestionService(this, suggestions);
            _highlighterService = new HighlighterService();

            _tabManager = new TabManager(tabControl, db, bookmarkRepo, _invokers);
            _fileService = new FileService(_tabManager, db);

            _mediator = new EditorMediator(this, _tabManager, _recorder, _fileService, _highlighterService);

            _tabManager.OnSaveRequested = (editor) => _fileService.SaveFile();
            _tabManager.OnEditorCreated += SubscribeToEditorEvents;

            tabControl.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControl.Padding = new Point(18, 4);
            toolStripStatusLabel1.Text = "UTF-8";

            tabControl.DrawItem += (s, e) =>
                _tabManager.DrawTab(e.Graphics, e.Index, e.Bounds, tabControl.Font);

            tabControl.MouseDown += (s, e) => _tabManager.HandleMouseDown(e);

            tabControl.SelectedIndexChanged += TabControl_SelectedIndexChanged;

            _textChangeTimer = new System.Windows.Forms.Timer { Interval = 50 };
            _textChangeTimer.Tick += TextChangeTimer_Tick;

            _scrollTimer = new System.Windows.Forms.Timer { Interval = 50 };
            _scrollTimer.Tick += ScrollTimer_Tick;

            CreateNewTab();
        }

        public void CreateNewTab(string title = "New File", string content = "", string filePath = null)
        {
            _tabManager.CreateTab(title, content, filePath);
        }

        private void SubscribeToEditorEvents(RichTextBox editor)
        {
            editor.KeyDown += Editor_KeyDown;
            editor.KeyPress += Editor_KeyPress;

            editor.TextChanged += (s, e) =>
            {
                var invoker = _tabManager.GetActiveInvoker();
                if (invoker != null && invoker.IsRestoring)
                    return;

                _textChangeTimer.Stop();
                _textChangedPending = true;
                _textChangeTimer.Start();

                _suggestionService.Show(editor);

                if (editor.Tag is EditorMetadata meta)
                {
                    meta.IsModified = true;
                    var tab = tabControl.SelectedTab;
                    if (tab != null && !tab.Text.EndsWith("*"))
                        tab.Text += "*";
                }
            };

            editor.VScroll += (s, e) =>
            {
                _scrollTimer.Stop();
                _scrollTimer.Start();

                if (editor.Tag is EditorMetadata meta)
                    meta.Gutter?.Invalidate();

                if (_suggestionService.IsVisible)
                    editor.Focus();
            };

            editor.MouseWheel += (s, e) =>
            {
                _scrollTimer.Stop();
                _scrollTimer.Start();

                if (editor.Tag is EditorMetadata meta)
                    meta.Gutter?.Invalidate();
            };

            editor.Resize += (s, e) =>
            {
                _scrollTimer.Stop();
                _scrollTimer.Start();

                if (editor.Tag is EditorMetadata meta)
                    meta.Gutter?.Invalidate();
            };

            editor.SelectionChanged += (s, e) =>
            {
                if (editor.Tag is EditorMetadata meta)
                    meta.Gutter?.Invalidate();
            };

            editor.GotFocus += (s, e) =>
            {
                if (_invokers.ContainsKey(editor))
                {
                    var invoker = _invokers[editor];
                }
            };

            _highlighterService.ForceRefresh(editor);
        }

        private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            var editor = _tabManager.GetActiveEditor();

            if (editor != null && editor.Tag is EditorMetadata meta && meta.FileEncoding != null)
                toolStripStatusLabel1.Text = meta.FileEncoding.EncodingName;
            else
                toolStripStatusLabel1.Text = "UTF-8";

            _tabManager.LoadBookmarksForActiveTab();

            if (editor != null)
                _highlighterService.ForceRefresh(editor);
        }

        private void TextChangeTimer_Tick(object sender, EventArgs e)
        {
            _textChangeTimer.Stop();

            var invoker = _tabManager.GetActiveInvoker();
            if (_textChangedPending && invoker != null)
                invoker.TrackState();

            _textChangedPending = false;

            var editor = _tabManager.GetActiveEditor();
            if (editor != null)
            {
                _highlighterService.ForceRefresh(editor);
            }
        }

        private void ScrollTimer_Tick(object sender, EventArgs e)
        {
            _scrollTimer.Stop();

            var editor = _tabManager.GetActiveEditor();
            if (editor != null)
            {
                _highlighterService.HighlightVisibleArea(editor);
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            var editor = _tabManager.GetActiveEditor();

            if (keyData == Keys.Tab && !_recorder.IsRecording)
            {
                if (editor != null && _snippetService.TryExpandSnippet(editor))
                {
                    _suggestionService.Hide();
                    return true;
                }
            }

            if (_suggestionService.IsVisible)
            {
                if (keyData == Keys.Enter || keyData == Keys.Tab)
                {
                    _suggestionService.Insert(editor);
                    return true;
                }
                if (keyData == Keys.Up)
                {
                    _suggestionService.MoveSelection(-1);
                    return true;
                }
                if (keyData == Keys.Down)
                {
                    _suggestionService.MoveSelection(1);
                    return true;
                }
                if (keyData == Keys.Escape)
                {
                    _suggestionService.Hide();
                    return true;
                }
            }

            if (editor != null)
            {
                if (keyData == (Keys.F2 | Keys.Shift))
                {
                    _tabManager.GoToPreviousBookmark(editor);
                    return true;
                }
                if (keyData == Keys.F2)
                {
                    _tabManager.GoToNextBookmark(editor);
                    return true;
                }
                if (keyData == (Keys.Control | Keys.B))
                {
                    _tabManager.ToggleBookmark(editor);
                    return true;
                }
                if (keyData == (Keys.Control | Keys.G))
                {
                    _mediator.Notify("GoTo");
                    return true;
                }
                if (keyData == (Keys.Control | Keys.F))
                {
                    _mediator.Notify("Find");
                    return true;
                }
                if (keyData == (Keys.Control | Keys.Oemplus) ||
                    keyData == (Keys.Control | Keys.Add))
                {
                    _mediator.Notify("ZoomIn");
                    return true;
                }
                if (keyData == (Keys.Control | Keys.OemMinus) ||
                    keyData == (Keys.Control | Keys.Subtract))
                {
                    _mediator.Notify("ZoomOut");
                    return true;
                }
            }

            if (_recorder.IsRecording && keyData == (Keys.Control | Keys.V))
            {
                if (editor != null && Clipboard.ContainsText())
                    _recorder.Insert(editor.SelectionStart, Clipboard.GetText());
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void Editor_KeyDown(object sender, KeyEventArgs e)
        {
            var rtb = (RichTextBox)sender;

            if (_recorder.IsRecording)
            {
                if (e.Control && e.KeyCode == Keys.X)
                {
                    if (rtb.SelectionLength > 0)
                        _recorder.Delete(rtb.SelectionStart, rtb.SelectionLength);
                }
                else if (e.KeyCode == Keys.Back)
                {
                    if (rtb.SelectionLength > 0)
                        _recorder.Delete(rtb.SelectionStart, rtb.SelectionLength);
                    else if (rtb.SelectionStart > 0)
                        _recorder.Delete(rtb.SelectionStart - 1, 1);
                }
                else if (e.KeyCode == Keys.Delete)
                {
                    if (rtb.SelectionLength > 0)
                        _recorder.Delete(rtb.SelectionStart, rtb.SelectionLength);
                    else if (rtb.SelectionStart < rtb.TextLength)
                        _recorder.Delete(rtb.SelectionStart, 1);
                }
                else if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Right ||
                         e.KeyCode == Keys.Up || e.KeyCode == Keys.Down ||
                         e.KeyCode == Keys.Home || e.KeyCode == Keys.End)
                {
                    this.BeginInvoke(new Action(() =>
                    {
                        _recorder.MoveCursor(rtb.SelectionStart);
                    }));
                }
            }

            if (!_recorder.IsRecording)
            {
                var macro = _hotkeyService.TryMatchHotkey(e);
                if (macro != null)
                {
                    e.SuppressKeyPress = true;
                    _macroService.RunMacro(macro, rtb,
                        cmd => _mediator.Notify(cmd));
                }
            }

            if (e.Control && e.KeyCode == Keys.Z)
            {
                e.SuppressKeyPress = true;
                _mediator.Notify("Undo");
            }
            else if (e.Control && e.KeyCode == Keys.Y)
            {
                e.SuppressKeyPress = true;
                _mediator.Notify("Redo");
            }
        }

        private void Editor_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (_recorder.IsRecording)
            {
                var rtb = (RichTextBox)sender;
                if (e.KeyChar == (char)Keys.Back) return;

                if (rtb.SelectionLength > 0)
                    _recorder.Delete(rtb.SelectionStart, rtb.SelectionLength);

                _recorder.Insert(rtb.SelectionStart, e.KeyChar.ToString());
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e) => _fileService.SaveFileAs();
        private void saveToolStripMenuItem_Click(object sender, EventArgs e) => _fileService.SaveFile();
        private void openToolStripMenuItem_Click(object sender, EventArgs e) => _fileService.OpenFile();
        private void newToolStripMenuItem_Click(object sender, EventArgs e) => CreateNewTab();
        private void exitToolStripMenuItem_Click(object sender, EventArgs e) => Close();

        private void undoToolStripMenuItem_Click(object sender, EventArgs e) => _mediator.Notify("Undo");
        private void redoToolStripMenuItem_Click(object sender, EventArgs e) => _mediator.Notify("Redo");
        private void cutToolStripMenuItem_Click(object sender, EventArgs e) => _mediator.Notify("Cut");
        private void copyToolStripMenuItem_Click(object sender, EventArgs e) => _mediator.Notify("Copy");
        private void pasteToolStripMenuItem_Click(object sender, EventArgs e) => _mediator.Notify("Paste");
        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e) => _mediator.Notify("SelectAll");
        private void findToolStripMenuItem_Click(object sender, EventArgs e) => _mediator.Notify("Find");
        private void goToToolStripMenuItem_Click(object sender, EventArgs e) => _mediator.Notify("GoTo");
        private void zoomInToolStripMenuItem_Click(object sender, EventArgs e) => _mediator.Notify("ZoomIn");
        private void zoomOutToolStripMenuItem_Click(object sender, EventArgs e) => _mediator.Notify("ZoomOut");

        private void Suggestions_Click(object sender, EventArgs e)
        {
            var editor = _tabManager.GetActiveEditor();
            if (editor != null)
                _suggestionService.Insert(editor);
        }

        private void addSnippetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var form = new SnippetAddForm(_snippetService))
            {
                if (form.ShowDialog() == DialogResult.OK)
                    MessageBox.Show("Новий сніппет готовий до використання!");
            }
        }

        private void startRecordingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var editor = _tabManager.GetActiveEditor();
            if (editor == null) return;

            _recorder.Start(editor.SelectionStart);
            currentRecordedMacro = null;
            MessageBox.Show("Macro recording started.");
        }

        private void stopRecordingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var actions = _recorder.Stop();
            if (actions.Count == 0)
            {
                MessageBox.Show("Запис порожній. Макрос не створено.");
                return;
            }

            currentRecordedMacro = new Macro { Actions = actions };

            using (var dlg = new MacroSaveForm(currentRecordedMacro, _macroRepository))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    _hotkeyService.UpdateMacros(_macroService.GetAll());
                    MessageBox.Show("Макрос успішно збережено!");
                }
            }
        }

        private void playLastMacroToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentRecordedMacro == null)
            {
                MessageBox.Show("No macro recorded.");
                return;
            }

            var editor = _tabManager.GetActiveEditor();
            if (editor == null) return;

            _macroService.RunMacro(currentRecordedMacro, editor,
                cmd => _mediator.Notify(cmd));
        }

        private void saveMacroToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentRecordedMacro == null)
            {
                MessageBox.Show("Nothing to save.");
                return;
            }

            using (var dlg = new MacroSaveForm(currentRecordedMacro, _macroRepository))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    _hotkeyService.UpdateMacros(_macroService.GetAll());
                }
            }
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == WM_VSCROLL || m.Msg == WM_MOUSEWHEEL)
            {
                _scrollTimer.Stop();
                _scrollTimer.Start();
            }
        }
    }
}
