namespace TextEditor
{
    partial class MacroSaveForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.chkCtrl = new System.Windows.Forms.CheckBox();
            this.chkShift = new System.Windows.Forms.CheckBox();
            this.chkAlt = new System.Windows.Forms.CheckBox();
            this.cmbKey = new System.Windows.Forms.ComboBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Назва макросу:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(126, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "Комбінація клавіш:";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(129, 23);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(122, 22);
            this.txtName.TabIndex = 2;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // chkCtrl
            // 
            this.chkCtrl.AutoSize = true;
            this.chkCtrl.Location = new System.Drawing.Point(16, 87);
            this.chkCtrl.Name = "chkCtrl";
            this.chkCtrl.Size = new System.Drawing.Size(48, 20);
            this.chkCtrl.TabIndex = 3;
            this.chkCtrl.Text = "Ctrl";
            this.chkCtrl.UseVisualStyleBackColor = true;
            this.chkCtrl.CheckedChanged += new System.EventHandler(this.chkCtrl_CheckedChanged);
            // 
            // chkShift
            // 
            this.chkShift.AutoSize = true;
            this.chkShift.Location = new System.Drawing.Point(70, 87);
            this.chkShift.Name = "chkShift";
            this.chkShift.Size = new System.Drawing.Size(54, 20);
            this.chkShift.TabIndex = 4;
            this.chkShift.Text = "Shift";
            this.chkShift.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkShift.UseVisualStyleBackColor = true;
            this.chkShift.CheckedChanged += new System.EventHandler(this.chkShift_CheckedChanged);
            // 
            // chkAlt
            // 
            this.chkAlt.AutoSize = true;
            this.chkAlt.Location = new System.Drawing.Point(130, 87);
            this.chkAlt.Name = "chkAlt";
            this.chkAlt.Size = new System.Drawing.Size(44, 20);
            this.chkAlt.TabIndex = 5;
            this.chkAlt.Text = "Alt";
            this.chkAlt.UseVisualStyleBackColor = true;
            this.chkAlt.CheckedChanged += new System.EventHandler(this.chkAlt_CheckedChanged);
            // 
            // cmbKey
            // 
            this.cmbKey.FormattingEnabled = true;
            this.cmbKey.Location = new System.Drawing.Point(180, 83);
            this.cmbKey.Name = "cmbKey";
            this.cmbKey.Size = new System.Drawing.Size(121, 24);
            this.cmbKey.TabIndex = 6;
            this.cmbKey.SelectedIndexChanged += new System.EventHandler(this.cmbKey_SelectedIndexChanged);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnSave.Location = new System.Drawing.Point(53, 141);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(98, 34);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "Зберегти";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(168, 141);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(98, 32);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Скасувати";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // MacroSaveForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(316, 184);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.cmbKey);
            this.Controls.Add(this.chkAlt);
            this.Controls.Add(this.chkShift);
            this.Controls.Add(this.chkCtrl);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "MacroSaveForm";
            this.Text = "MacroSaveForm";
            this.Load += new System.EventHandler(this.MacroSaveForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.CheckBox chkCtrl;
        private System.Windows.Forms.CheckBox chkShift;
        private System.Windows.Forms.CheckBox chkAlt;
        private System.Windows.Forms.ComboBox cmbKey;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
    }
}
