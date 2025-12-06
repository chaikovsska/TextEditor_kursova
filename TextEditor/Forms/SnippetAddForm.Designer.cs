namespace TextEditor
{
    partial class SnippetAddForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.title = new System.Windows.Forms.Label();
            this.shortcut = new System.Windows.Forms.Label();
            this.txtShortcut = new System.Windows.Forms.TextBox();
            this.content = new System.Windows.Forms.Label();
            this.txtContent = new System.Windows.Forms.RichTextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtTitle
            // 
            this.txtTitle.Location = new System.Drawing.Point(98, 22);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(202, 22);
            this.txtTitle.TabIndex = 0;
            // 
            // title
            // 
            this.title.AutoSize = true;
            this.title.Location = new System.Drawing.Point(34, 25);
            this.title.Name = "title";
            this.title.Size = new System.Drawing.Size(52, 16);
            this.title.TabIndex = 1;
            this.title.Text = "Назва:";
            // 
            // shortcut
            // 
            this.shortcut.AutoSize = true;
            this.shortcut.Location = new System.Drawing.Point(34, 65);
            this.shortcut.Name = "shortcut";
            this.shortcut.Size = new System.Drawing.Size(89, 16);
            this.shortcut.TabIndex = 2;
            this.shortcut.Text = "Скорочення:";
            // 
            // txtShortcut
            // 
            this.txtShortcut.Location = new System.Drawing.Point(129, 62);
            this.txtShortcut.Name = "txtShortcut";
            this.txtShortcut.Size = new System.Drawing.Size(171, 22);
            this.txtShortcut.TabIndex = 3;
            // 
            // content
            // 
            this.content.AutoSize = true;
            this.content.Location = new System.Drawing.Point(34, 105);
            this.content.Name = "content";
            this.content.Size = new System.Drawing.Size(45, 16);
            this.content.TabIndex = 4;
            this.content.Text = "Вміст:";
            // 
            // txtContent
            // 
            this.txtContent.Location = new System.Drawing.Point(37, 138);
            this.txtContent.Name = "txtContent";
            this.txtContent.Size = new System.Drawing.Size(232, 96);
            this.txtContent.TabIndex = 5;
            this.txtContent.Text = "";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(53, 269);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(83, 23);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "Зберегти";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(154, 269);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(103, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Скасувати";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // SnippetAddForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(307, 310);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtContent);
            this.Controls.Add(this.content);
            this.Controls.Add(this.txtShortcut);
            this.Controls.Add(this.shortcut);
            this.Controls.Add(this.title);
            this.Controls.Add(this.txtTitle);
            this.Name = "SnippetAddForm";
            this.Text = "SnippetAddForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.Label title;
        private System.Windows.Forms.Label shortcut;
        private System.Windows.Forms.TextBox txtShortcut;
        private System.Windows.Forms.Label content;
        private System.Windows.Forms.RichTextBox txtContent;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
    }
}