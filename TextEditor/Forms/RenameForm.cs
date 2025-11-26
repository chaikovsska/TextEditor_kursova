using System;
using System.IO;
using System.Windows.Forms;

namespace TextEditor
{
    public partial class RenameForm : Form
    {
        public string NewName { get; private set; }

        public RenameForm(string currentName)
        {
            InitializeComponent();

            this.Text = "Rename Tab";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.AcceptButton = btnOK;
            this.CancelButton = btnCancel;

            txtNewName.Text = currentName.TrimEnd('*');
            txtNewName.SelectAll();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string name = txtNewName.Text.Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Name cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (name.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
            {
                MessageBox.Show("Name contains invalid characters.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            NewName = name;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}