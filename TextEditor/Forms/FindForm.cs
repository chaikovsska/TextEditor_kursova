using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TextEditor
{
    public partial class FindForm : Form
    {
        private readonly RichTextBox _editor;

        public FindForm(RichTextBox editor)
        {
            InitializeComponent();
            _editor = editor;

            if (!string.IsNullOrEmpty(_editor.SelectedText))
            {
                txtSearch.Text = _editor.SelectedText;
            }
        }

        private void btnFindNext_Click(object sender, EventArgs e)
        {
            string searchText = txtSearch.Text;
            if (string.IsNullOrEmpty(searchText)) return;

            RichTextBoxFinds options = RichTextBoxFinds.None;
            if (chkMatchCase.Checked) options |= RichTextBoxFinds.MatchCase;

            int startPos = _editor.SelectionStart + _editor.SelectionLength;

            try
            {
                int foundPos = _editor.Find(searchText, startPos, options);

                if (foundPos != -1)
                {
                    _editor.Select(foundPos, searchText.Length);
                    _editor.ScrollToCaret();
                    _editor.Focus();
                }
                else
                {
                    var res = MessageBox.Show("Reached end of document. Search from start?", "Find", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (res == DialogResult.Yes)
                    {
                        foundPos = _editor.Find(searchText, 0, options);
                        if (foundPos != -1)
                        {
                            _editor.Select(foundPos, searchText.Length);
                            _editor.ScrollToCaret();
                            _editor.Focus();
                        }
                        else
                        {
                            MessageBox.Show("Text not found.", "Find", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
