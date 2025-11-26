using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TextEditor.Features.Macro;

namespace TextEditor
{
    public partial class MacroSaveForm : Form
    {
        private readonly IMacroRepository _repo;
        private readonly Macro _macro;

        public MacroSaveForm(Macro macro, IMacroRepository repo)
        {
            InitializeComponent();
            _macro = macro;
            _repo = repo;

            cmbKey.Items.AddRange(Enum.GetNames(typeof(Keys)));
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveMacroToDatabase();
        }

        private void SaveMacroToDatabase()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Будь ласка, введіть назву макроса.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbKey.SelectedItem == null)
            {
                MessageBox.Show("Будь ласка, оберіть клавішу.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var tempHotkey = new Hotkey
            {
                Ctrl = chkCtrl.Checked,
                Shift = chkShift.Checked,
                Alt = chkAlt.Checked,
                Key = (Keys)Enum.Parse(typeof(Keys), cmbKey.SelectedItem.ToString())
            };

            if (IsHotkeyReserved(tempHotkey))
            {
                MessageBox.Show($"Комбінація {tempHotkey} зарезервована системою.\nОберіть іншу.",
                                "Зайнято", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var allMacros = _repo.GetAll();

            foreach (var m in allMacros)
            {
                if (m.Id == _macro.Id) continue;

                if (string.Equals(m.Name, txtName.Text.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show($"Макрос з назвою '{m.Name}' вже існує.\nБудь ласка, введіть унікальну назву.",
                                    "Дублікат назви", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (IsSameHotkey(m.Hotkey, tempHotkey))
                {
                    MessageBox.Show($"Ця комбінація клавіш вже використовується макросом '{m.Name}'.",
                                    "Дублікат клавіш", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            _macro.Name = txtName.Text.Trim(); 
            _macro.Hotkey = tempHotkey;

            _repo.Save(_macro);

            this.DialogResult = DialogResult.OK;
            Close();
        }

        private bool IsSameHotkey(Hotkey a, Hotkey b)
        {
            if (a == null || b == null) return false;
            return a.Ctrl == b.Ctrl &&
                   a.Shift == b.Shift &&
                   a.Alt == b.Alt &&
                   a.Key == b.Key;
        }

        private bool IsHotkeyReserved(Hotkey h)
        {
            if (h.Ctrl && !h.Shift && !h.Alt)
            {
                switch (h.Key)
                {
                    case Keys.C:
                    case Keys.V:
                    case Keys.X: 
                    case Keys.Z:
                    case Keys.Y:              
                    case Keys.A:                          
                    case Keys.S:
                    case Keys.N:
                    case Keys.O: 
                        return true;
                }
            }
            return false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void chkCtrl_CheckedChanged(object sender, EventArgs e) { }
        private void chkShift_CheckedChanged(object sender, EventArgs e) { }
        private void chkAlt_CheckedChanged(object sender, EventArgs e) { }
        private void cmbKey_SelectedIndexChanged(object sender, EventArgs e) { }
        private void txtName_TextChanged(object sender, EventArgs e) { }
        private void MacroSaveForm_Load(object sender, EventArgs e) { }
    }
}
