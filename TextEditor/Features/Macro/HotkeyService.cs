using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace TextEditor.Features.Macro
{
    public class HotkeyService
    {
        private List<Macro> _macros;

        public HotkeyService(List<Macro> macros)
        {
            _macros = macros;
        }

        public Macro TryMatchHotkey(KeyEventArgs e)
        {
            foreach (var macro in _macros)
            {
                if (macro.Hotkey == null) continue;

                if (macro.Hotkey.Ctrl == e.Control &&
                    macro.Hotkey.Shift == e.Shift &&
                    macro.Hotkey.Alt == e.Alt &&
                    macro.Hotkey.Key == e.KeyCode)
                {
                    return macro;
                }
            }

            return null;
        }

        public void UpdateMacros(List<Macro> macros)
        {
            _macros = macros;
        }
    }
}
