using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEditor
{
    public interface IEditorMediator
{
    void Notify(string eventCode, object sender = null);
}
}
