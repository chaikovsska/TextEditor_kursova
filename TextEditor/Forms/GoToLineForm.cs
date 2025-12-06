using System;
using System.Windows.Forms;

namespace TextEditor
{
    public partial class GoToLineForm : Form
    {
        public int SelectedLine { get; private set; } = -1;
        private int _maxLines;

        public GoToLineForm(int currentLine, int maxLines)
        {
            InitializeComponent();
            _maxLines = maxLines;

            lblCurrentInfo.Text = $"Ви знаходитесь на рядку: {currentLine}";

            lblLine.Text = $"Введіть номер рядка:";

            txtLineNumber.Text = currentLine.ToString();
            txtLineNumber.SelectAll(); 
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtLineNumber.Text, out int line))
            {
                if (line < 1 || line > _maxLines)
                {
                    MessageBox.Show($"Номер рядка поза діапазоном.\nБудь ласка, введіть число від 1 до {_maxLines}.",
                                    "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtLineNumber.SelectAll();
                    txtLineNumber.Focus();
                    return;
                }

                SelectedLine = line;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Будь ласка, введіть коректне число.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLineNumber.SelectAll();
                txtLineNumber.Focus();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}