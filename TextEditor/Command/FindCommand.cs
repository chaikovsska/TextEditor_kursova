using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TextEditor.Command
{
    public class FindCommand : ICommand
    {
        private readonly MainForm _form; 
        private readonly TextBoxBase _editor;

        public FindCommand(MainForm form, TextBoxBase editor)
        {
            _form = form;
            _editor = editor;
        }

        public void Execute()
        {
            if (_editor is RichTextBox rtb)
            {
                FindForm findForm = new FindForm(rtb);
                findForm.Owner = _form; 
                findForm.Show();
            }
        }
    }
}
