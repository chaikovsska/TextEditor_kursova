using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEditor.Features.Bookmarks
{
    public class Bookmark
    {
        public int Id { get; set; }
        public string FilePath { get; set; }
        public int Line { get; set; } 
        public string CreatedAt { get; set; }
    }
}
