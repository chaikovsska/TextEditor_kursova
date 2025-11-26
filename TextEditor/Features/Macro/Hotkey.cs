using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TextEditor.Features.Macro
{
    public class Hotkey
    {
        public bool Ctrl { get; set; }
        public bool Shift { get; set; }
        public bool Alt { get; set; }
        public Keys Key { get; set; }

        public override string ToString()
        {
            string combo = "";
            if (Ctrl) combo += "Ctrl+";
            if (Shift) combo += "Shift+";
            if (Alt) combo += "Alt+";
            combo += Key.ToString();
            return combo;
        }
    }
}
