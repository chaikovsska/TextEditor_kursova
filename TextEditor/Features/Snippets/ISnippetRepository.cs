using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEditor.Features.Snippets
{
    public interface ISnippetRepository
    {
        void Add(Snippet snippet);
        List<Snippet> GetAll();
    }
}
