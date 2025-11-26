using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace TextEditor.Features.Snippets
{
    public class SnippetService
    {
        private readonly ISnippetRepository _repo;

        public SnippetService(ISnippetRepository repo)
        {
            _repo = repo;
        }

        public void AddSnippet(string title, string shortcut, string content)
        {
            _repo.Add(new Snippet
            {
                Title = title,
                Shortcut = shortcut,
                Content = content
            });
        }

        public List<Snippet> GetAll() => _repo.GetAll();

        public bool TryExpandSnippet(RichTextBox rtb)
        {
            int pos = rtb.SelectionStart;
            if (pos == 0) return false;

            int start = pos - 1;
            while (start >= 0 && !char.IsWhiteSpace(rtb.Text[start]))
            {
                start--;
            }
            start++;

            string shortcut = rtb.Text.Substring(start, pos - start);
            if (string.IsNullOrEmpty(shortcut)) return false;

            var allSnippets = GetAll();
            var match = allSnippets.FirstOrDefault(s => s.Shortcut == shortcut);

            if (match != null)
            {
                rtb.Select(start, shortcut.Length);
                rtb.SelectedText = match.Content;
                return true;
            }

            return false;
        }
    }
}