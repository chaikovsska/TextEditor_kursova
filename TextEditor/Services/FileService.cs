using System;
using System.IO;
using System.Windows.Forms;
using TextEditor.Database;
using Dapper;
using TextEditor.Strategy.EncodingStrategy;
using System.Linq;

namespace TextEditor.Services
{
    public class FileService
    {
        private readonly TabManager _tabManager;
        private readonly DatabaseService _db;

        public FileService(TabManager tabManager, DatabaseService db)
        {
            _tabManager = tabManager;
            _db = db;
        }

        public void OpenFile()
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Text Files|*.txt;*.cs;*.cpp;*.h;*.java;*.py;*.html;*.css;*.json;*.xml|All Files|*.*";

                if (ofd.ShowDialog() != DialogResult.OK) return;

                string selectedPath = ofd.FileName;

                if (_tabManager.FocusTabIfOpen(selectedPath))
                {
                    MessageBox.Show("Цей файл вже відкрито!", "Інформація", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var bytes = File.ReadAllBytes(selectedPath);
                var detector = new EncodingDetector();
                var encoding = detector.DetectEncoding(bytes);
                string text = encoding.GetString(bytes);

                var editor = _tabManager.CreateTab(Path.GetFileName(selectedPath), text, selectedPath);

                if (editor.Tag is EditorMetadata metaNew)
                {
                    metaNew.FileEncoding = encoding;
                }

                _tabManager.LoadBookmarksForActiveTab();
            }
        }

        public void SaveFile()
        {
            var editor = _tabManager.GetActiveEditor();
            if (editor == null) return;

            var meta = editor.Tag as EditorMetadata;
            if (meta == null) return;

            if (string.IsNullOrEmpty(meta.FilePath))
            {
                SaveFileAs();
                return;
            }

            try
            {
                File.WriteAllText(meta.FilePath, editor.Text, meta.FileEncoding);
                meta.IsModified = false;

                _tabManager.UpdateActiveTabTitle(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка при збереженні: " + ex.Message);
            }
        }

        public void SaveFileAs()
        {
            var editor = _tabManager.GetActiveEditor();
            if (editor == null) return;

            var meta = editor.Tag as EditorMetadata;
            if (meta == null) return;

            string oldPath = meta.FilePath;

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Text Files (*.txt)|*.txt|C# Files (*.cs)|*.cs|All Files (*.*)|*.*";

                string currentTitle = _tabManager.GetActiveTabTitle();
                if (string.IsNullOrEmpty(meta.FilePath))
                    sfd.FileName = currentTitle.TrimEnd('*');
                else
                    sfd.FileName = Path.GetFileName(meta.FilePath);

                if (sfd.ShowDialog() != DialogResult.OK) return;

                try
                {
                    File.WriteAllText(sfd.FileName, editor.Text, meta.FileEncoding);

                    meta.FilePath = sfd.FileName;
                    meta.IsModified = false;

                    _tabManager.UpdateActiveTabTitle(false, Path.GetFileName(sfd.FileName));

                    UpdateBookmarksPath(oldPath, sfd.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Помилка при збереженні: " + ex.Message);
                }
            }
        }

        private void UpdateBookmarksPath(string oldPath, string newPath)
        {
            string searchPath = string.IsNullOrEmpty(oldPath) ? "Untitled" : oldPath;

            using (var con = _db.GetConnection())
            {
                con.Execute("UPDATE Bookmarks SET FilePath = @NewPath WHERE FilePath = @OldPath",
                            new { NewPath = newPath, OldPath = searchPath });
            }

            _tabManager.LoadBookmarksForActiveTab();
        }
    }
}