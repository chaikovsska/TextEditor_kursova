using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TextEditor.Features.Snippets;

namespace TextEditor
{
    public partial class SnippetAddForm : Form
    {
        private readonly SnippetService _service;

        public SnippetAddForm(SnippetService service)
        {
            InitializeComponent();
            _service = service;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("Введіть назву (Title)!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtShortcut.Text))
            {
                MessageBox.Show("Введіть шорткат (наприклад: for, cw)!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtContent.Text))
            {
                MessageBox.Show("Введіть код сніппета!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                _service.AddSnippet(txtTitle.Text.Trim(), txtShortcut.Text.Trim(), txtContent.Text);

                MessageBox.Show("Сніппет успішно додано!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK; 
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка збереження. Можливо, такий шорткат вже існує.\n\nДеталі: {ex.Message}",
                                "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void SnippetAddForm_Load(object sender, EventArgs e) { }

    }
}
