using Dapper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using TextEditor.Command;
using TextEditor.Database;
using TextEditor.Features.Bookmarks;
using TextEditor.Composite.Gutter;
using TextEditor.Memento;

namespace TextEditor.Services
{
    public class TabManager
    {
        private readonly TabControl _tabControl;
        private readonly DatabaseService _db;
        private readonly IBookmarkRepository _bookmarkRepo;
        private readonly Dictionary<TextBoxBase, CommandInvoker> _invokers;

        public event Action<RichTextBox> OnEditorCreated;
        public Action<RichTextBox> OnSaveRequested; 

        public TabManager(TabControl tabControl, DatabaseService db, IBookmarkRepository bookmarkRepo, Dictionary<TextBoxBase, CommandInvoker> invokers)
        {
            _tabControl = tabControl;
            _db = db;
            _bookmarkRepo = bookmarkRepo;
            _invokers = invokers;
        }

        public RichTextBox GetActiveEditor()
        {
            if (_tabControl.SelectedTab == null || _tabControl.SelectedTab.Controls.Count == 0) return null;

            var mainControl = _tabControl.SelectedTab.Controls[0];
            if (mainControl is Panel panel)
            {
                foreach (Control c in panel.Controls)
                    if (c is RichTextBox rtb) return rtb;
            }
            else if (mainControl is TextBoxBase tb)
            {
                return tb as RichTextBox;
            }
            return null;
        }

        public CommandInvoker GetActiveInvoker()
        {
            var editor = GetActiveEditor();
            if (editor != null && _invokers.ContainsKey(editor))
                return _invokers[editor];
            return null;
        }

        public string GetActiveTabTitle() => _tabControl.SelectedTab?.Text ?? "";

        public RichTextBox CreateTab(string title, string content, string filePath, HashSet<int> currentBookmarks = null)
        {
            if (currentBookmarks == null) currentBookmarks = new HashSet<int>();

            if (string.IsNullOrEmpty(filePath))
            {
                using (var con = _db.GetConnection())
                {
                    con.Execute("DELETE FROM Bookmarks WHERE FilePath = 'Untitled'");
                }
            }

            if (!string.IsNullOrEmpty(filePath) && currentBookmarks.Count == 0)
            {
                var bookmarksFromDb = _bookmarkRepo.GetByFile(filePath);
                foreach (var b in bookmarksFromDb) currentBookmarks.Add(b.Line);
            }

            var (editor, gutter) = EditorFactory.CreateEditorComponents(content, currentBookmarks);

            if (editor.Tag is EditorMetadata meta)
            {
                meta.FilePath = filePath;
            }

            var tab = new TabPage(title);
            var container = new Panel { Dock = DockStyle.Fill };
            container.Controls.Add(editor);
            container.Controls.Add(gutter);
            tab.Controls.Add(container);

            var invoker = new CommandInvoker(new EditorOriginator(editor));
            _invokers[editor] = invoker;
            invoker.TrackState();

            _tabControl.TabPages.Add(tab);
            _tabControl.SelectedTab = tab;

            gutter.MouseClick += (s, e) =>
            {
                int charIndex = editor.GetCharIndexFromPosition(new Point(1, e.Y));
                int lineIndex = editor.GetLineFromCharIndex(charIndex);
                ToggleBookmarkAtLine(editor, lineIndex);
            };

            OnEditorCreated?.Invoke(editor);

            return editor;
        }

        public void RequestCloseTab(int index)
        {
            if (index < 0 || index >= _tabControl.TabPages.Count) return;

            var tab = _tabControl.TabPages[index];

            RichTextBox editor = null;
            if (tab.Controls[0] is Panel p) editor = p.Controls.OfType<RichTextBox>().FirstOrDefault();
            else if (tab.Controls[0] is RichTextBox rtb) editor = rtb;

            if (editor != null)
            {
                var meta = editor.Tag as EditorMetadata;

                if (meta != null && meta.IsPinned)
                {
                    MessageBox.Show(
                        "Ця вкладка закріплена.\nСпочатку відкріпіть її (Unpin), щоб закрити.",
                        "Закріплена вкладка",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    return;
                }

                if (meta != null && meta.IsModified)
                {
                    _tabControl.SelectedIndex = index;
                    var res = MessageBox.Show($"Зберегти зміни у файлі {tab.Text}?", "Увага", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

                    if (res == DialogResult.Cancel) return;

                    if (res == DialogResult.Yes)
                    {
                        OnSaveRequested?.Invoke(editor);
                    }
                }
                _invokers.Remove(editor);
            }

            _tabControl.TabPages.RemoveAt(index);

            if (_tabControl.TabCount > 0)
            {
                int newIndex = index - 1;
                if (newIndex < 0) newIndex = 0;
                if (newIndex >= _tabControl.TabCount) newIndex = _tabControl.TabCount - 1;
                _tabControl.SelectedIndex = newIndex;
            }
            else
            {
                CreateTab("New File", "", null, new HashSet<int>()); // Створюємо нову, якщо закрили останню
            }
        }

        public void HandleMouseDown(MouseEventArgs e)
        {
            for (int i = 0; i < _tabControl.TabPages.Count; i++)
            {
                var tabRect = _tabControl.GetTabRect(i);
                if (tabRect.Contains(e.Location))
                {
                    var tab = _tabControl.TabPages[i];
                    RichTextBox editor = null;
                    if (tab.Controls[0] is Panel p) editor = p.Controls.OfType<RichTextBox>().FirstOrDefault();
                    var meta = editor?.Tag as EditorMetadata;

                    if (e.Button == MouseButtons.Right)
                    {
                        _tabControl.SelectedIndex = i;
                        ContextMenuStrip ctxMenu = new ContextMenuStrip();

                        ToolStripMenuItem renameItem = new ToolStripMenuItem("Rename");
                        renameItem.Click += (s, args) => PerformTabRename(tab);
                        ctxMenu.Items.Add(renameItem);

                        string pinText = (meta != null && meta.IsPinned) ? "Unpin Tab" : "Pin Tab";
                        ToolStripMenuItem pinItem = new ToolStripMenuItem(pinText);
                        pinItem.Click += (s, args) =>
                        {
                            if (meta != null) meta.IsPinned = !meta.IsPinned;
                            SortTabs();
                            _tabControl.Invalidate();
                        };
                        ctxMenu.Items.Add(pinItem);

                        ToolStripMenuItem closeItem = new ToolStripMenuItem("Close");
                        closeItem.Click += (s, args) => RequestCloseTab(i);
                        ctxMenu.Items.Add(closeItem);

                        ctxMenu.Show(_tabControl, e.Location);
                        return;
                    }

                    var closeRect = new Rectangle(tabRect.Right - 20, tabRect.Top + 4, 16, 16);

                    if (closeRect.Contains(e.Location))
                    {
                        if (meta != null && meta.IsPinned)
                        {
                            MessageBox.Show(
                                "This tab is pinned.\nUnpin it first before closing.",
                                "Pinned Tab",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);

                            return; 
                        }

                        RequestCloseTab(i);
                        return;
                    }
                }
            }
        }

        private void PerformTabRename(TabPage tab)
        {
            RichTextBox editor = null;
            if (tab.Controls[0] is Panel p) editor = p.Controls.OfType<RichTextBox>().FirstOrDefault();
            if (editor == null) return;
            var meta = editor.Tag as EditorMetadata;

            using (var dlg = new RenameForm(tab.Text))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    string newName = dlg.NewName;

                    if (!string.IsNullOrEmpty(meta.FilePath))
                    {
                        try
                        {
                            string dir = Path.GetDirectoryName(meta.FilePath);
                            string ext = Path.GetExtension(meta.FilePath);
                            if (!Path.HasExtension(newName)) newName += ext;

                            string newPath = Path.Combine(dir, newName);

                            if (File.Exists(newPath))
                            {
                                MessageBox.Show("File already exists!"); return;
                            }

                            File.Move(meta.FilePath, newPath);
                            UpdateBookmarksPath(meta.FilePath, newPath);
                            meta.FilePath = newPath;
                        }
                        catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); return; }
                    }

                    tab.Text = newName + (meta.IsModified ? "*" : "");
                }
            }
        }

        private void SortTabs()
        {
            var pinned = new List<TabPage>();
            var normal = new List<TabPage>();

            foreach (TabPage tab in _tabControl.TabPages)
            {
                RichTextBox editor = null;
                if (tab.Controls[0] is Panel p) editor = p.Controls.OfType<RichTextBox>().FirstOrDefault();
                var meta = editor?.Tag as EditorMetadata;

                if (meta != null && meta.IsPinned) pinned.Add(tab);
                else normal.Add(tab);
            }

            var current = _tabControl.SelectedTab;
            _tabControl.TabPages.Clear();
            _tabControl.TabPages.AddRange(pinned.ToArray());
            _tabControl.TabPages.AddRange(normal.ToArray());

            if (current != null) _tabControl.SelectedTab = current;
        }

        public void DrawTab(Graphics g, int index, Rectangle bounds, Font font)
        {
            var tab = _tabControl.TabPages[index];
            var rect = bounds;
            rect.Inflate(-2, -2);

            bool isPinned = false;
            if (tab.Controls.Count > 0 && tab.Controls[0] is Panel panel)
            {
                var editor = panel.Controls.OfType<RichTextBox>().FirstOrDefault();
                if (editor?.Tag is EditorMetadata meta) isPinned = meta.IsPinned;
            }

            var titleFont = isPinned ? new Font(font, FontStyle.Bold) : font;
            TextRenderer.DrawText(g, tab.Text, titleFont, rect, tab.ForeColor, TextFormatFlags.Left | TextFormatFlags.VerticalCenter);

            var btnRect = new Rectangle(rect.Right - 18, rect.Top + 4, 15, 15);
            if (isPinned)
            {
                try
                {
                    using (Font pinFont = new Font("Segoe UI Symbol", 7))
                    {
                        g.DrawString("📌", pinFont, Brushes.DarkRed, btnRect.Location);
                    }
                }
                catch
                {
                    g.DrawString("P", new Font(font, FontStyle.Bold), Brushes.DarkRed, btnRect.Location);
                }
            }
            else
            {
                g.DrawString("×", new Font(font.FontFamily, 9, FontStyle.Bold), Brushes.Black, btnRect.Location);
            }

            if (index == _tabControl.SelectedIndex)
                g.DrawRectangle(Pens.Gray, rect);
        }


        public void UpdateBookmarksPath(string oldPath, string newPath)
        {
            string searchPath = string.IsNullOrEmpty(oldPath) ? "Untitled" : oldPath;
            using (var con = _db.GetConnection())
            {
                con.Execute("UPDATE Bookmarks SET FilePath = @NewPath WHERE FilePath = @OldPath",
                            new { NewPath = newPath, OldPath = searchPath });
            }
            LoadBookmarksForActiveTab();
        }

        public void LoadBookmarksForActiveTab()
        {
            var editor = GetActiveEditor();
            if (editor == null) return;

            var meta = editor.Tag as EditorMetadata;
            if (meta == null) return;

            string path = meta.FilePath ?? "Untitled";
            var fromDb = _bookmarkRepo.GetByFile(path);

            var bookmarkLayer = meta.GutterService.Layers.OfType<BookmarkLayer>().FirstOrDefault();
            if (bookmarkLayer != null)
            {
                bookmarkLayer.UpdateBookmarks(fromDb.Select(b => b.Line));
            }

            meta.Gutter.Invalidate();
        }

        public void ToggleBookmarkAtLine(RichTextBox editor, int lineIndex)
        {
            if (lineIndex < 0) return;
            var meta = editor.Tag as EditorMetadata;
            string path = meta?.FilePath ?? "Untitled";

            if (_bookmarkRepo.Exists(path, lineIndex))
            {
                _bookmarkRepo.Remove(path, lineIndex);
            }
            else
            {
                _bookmarkRepo.Add(new Bookmark { FilePath = path, Line = lineIndex, CreatedAt = DateTime.Now.ToString() });
            }
            LoadBookmarksForActiveTab();
        }

        public void ToggleBookmark(RichTextBox editor)
        {
            int line = editor.GetLineFromCharIndex(editor.SelectionStart);
            ToggleBookmarkAtLine(editor, line);
        }

        public void GoToNextBookmark(RichTextBox editor)
        {
            var meta = editor.Tag as EditorMetadata;
            string path = meta?.FilePath ?? "Untitled";
            var bookmarks = _bookmarkRepo.GetByFile(path).Select(b => b.Line).OrderBy(l => l).ToList();

            if (bookmarks.Count == 0) return;

            int currentLine = editor.GetLineFromCharIndex(editor.SelectionStart);
            int nextLine = bookmarks.FirstOrDefault(l => l > currentLine);

            if (nextLine == 0 && !bookmarks.Contains(0)) nextLine = bookmarks.First(); 

            int charIndex = editor.GetFirstCharIndexFromLine(nextLine);
            editor.SelectionStart = charIndex;
            editor.ScrollToCaret();
        }

        public void GoToPreviousBookmark(RichTextBox editor)
        {
            var meta = editor.Tag as EditorMetadata;
            string path = meta?.FilePath ?? "Untitled";
            var bookmarks = _bookmarkRepo.GetByFile(path).Select(b => b.Line).OrderByDescending(l => l).ToList();

            if (bookmarks.Count == 0) return;

            int currentLine = editor.GetLineFromCharIndex(editor.SelectionStart);
            int prevLine = bookmarks.FirstOrDefault(l => l < currentLine);

            if (prevLine == 0 && !bookmarks.Contains(0)) prevLine = bookmarks.First(); 

            int charIndex = editor.GetFirstCharIndexFromLine(prevLine);
            editor.SelectionStart = charIndex;
            editor.ScrollToCaret();
        }

        public void UpdateActiveTabTitle(bool isModified, string newName = null)
        {
            if (_tabControl.SelectedTab == null) return;
            string text = newName ?? _tabControl.SelectedTab.Text.TrimEnd('*');
            _tabControl.SelectedTab.Text = text + (isModified ? "*" : "");
        }

        public bool FocusTabIfOpen(string path)
        {
            foreach (TabPage tab in _tabControl.TabPages)
            {
                RichTextBox editor = null;
                if (tab.Controls[0] is Panel p) editor = p.Controls.OfType<RichTextBox>().FirstOrDefault();
                else if (tab.Controls[0] is RichTextBox rtb) editor = rtb;

                if (editor?.Tag is EditorMetadata meta &&
                    string.Equals(meta.FilePath, path, StringComparison.OrdinalIgnoreCase))
                {
                    _tabControl.SelectedTab = tab;
                    return true;
                }
            }
            return false;
        }

        public void RefreshBookmarks()
        {
            LoadBookmarksForActiveTab();
        }
    }
}