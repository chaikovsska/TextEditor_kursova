using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEditor.Features.Macro
{
    public class Macro
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Hotkey Hotkey { get; set; }

        public List<MacroAction> Actions { get; set; } = new List<MacroAction>();
    }
}
