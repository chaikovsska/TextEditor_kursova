namespace TextEditor
{
    partial class GoToLineForm
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
            this.lblLine = new System.Windows.Forms.Label();
            this.txtLineNumber = new System.Windows.Forms.TextBox();
            this.btnGo = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblCurrentInfo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblLine
            // 
            this.lblLine.AutoSize = true;
            this.lblLine.Location = new System.Drawing.Point(12, 59);
            this.lblLine.Name = "lblLine";
            this.lblLine.Size = new System.Drawing.Size(0, 16);
            this.lblLine.TabIndex = 0;
            // 
            // txtLineNumber
            // 
            this.txtLineNumber.Location = new System.Drawing.Point(141, 56);
            this.txtLineNumber.Name = "txtLineNumber";
            this.txtLineNumber.Size = new System.Drawing.Size(100, 22);
            this.txtLineNumber.TabIndex = 1;
            // 
            // btnGo
            // 
            this.btnGo.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnGo.Location = new System.Drawing.Point(45, 98);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(79, 23);
            this.btnGo.TabIndex = 2;
            this.btnGo.Text = "Перейти";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(130, 98);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Скасувати";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblCurrentInfo
            // 
            this.lblCurrentInfo.AutoSize = true;
            this.lblCurrentInfo.Location = new System.Drawing.Point(12, 25);
            this.lblCurrentInfo.Name = "lblCurrentInfo";
            this.lblCurrentInfo.Size = new System.Drawing.Size(0, 16);
            this.lblCurrentInfo.TabIndex = 4;
            // 
            // GoToLineForm
            // 
            this.AcceptButton = this.btnGo;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(267, 143);
            this.Controls.Add(this.lblCurrentInfo);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnGo);
            this.Controls.Add(this.txtLineNumber);
            this.Controls.Add(this.lblLine);
            this.Name = "GoToLineForm";
            this.Text = "GoToLineForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblLine;
        private System.Windows.Forms.TextBox txtLineNumber;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblCurrentInfo;
    }
}