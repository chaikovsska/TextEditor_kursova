using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEditor.Features.Bookmarks
{
    public interface IBookmarkRepository
    {
        void Add(Bookmark bookmark);
        void Remove(string filePath, int line);
        List<Bookmark> GetByFile(string filePath);
        bool Exists(string filePath, int line);
    }
}
