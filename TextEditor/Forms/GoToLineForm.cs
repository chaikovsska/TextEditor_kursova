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

            lblCurrentInfo.Text = $"You are currently on line: {currentLine}";

            lblLine.Text = $"Enter line number:";

            txtLineNumber.Text = currentLine.ToString();
            txtLineNumber.SelectAll(); 
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtLineNumber.Text, out int line))
            {
                if (line < 1 || line > _maxLines)
                {
                    MessageBox.Show($"Line number out of range.\nPlease enter a number between 1 and {_maxLines}.",
                                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("Please enter a valid number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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