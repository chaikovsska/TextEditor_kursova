using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEditor.Command
{
    public class ExitCommand : ICommand
    {
        private MainForm _form;
        public ExitCommand(MainForm form) { _form = form; }
        public void Execute() => _form.Close();
    }
}
