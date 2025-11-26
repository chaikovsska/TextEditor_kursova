using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEditor.Features.Macro
{
    public interface IMacroRepository
    {
        void Save(Macro macro);
        List<Macro> GetAll();
        void Delete(int id);
    }
}
