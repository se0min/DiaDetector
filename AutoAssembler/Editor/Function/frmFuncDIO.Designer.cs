namespace AutoAssembler
{
    partial class frmFuncDIO
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.cboDIOFunc = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cboOnOff = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(227, 78);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 40);
            this.btnCancel.TabIndex = 71;
            this.btnCancel.Text = "취소";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(141, 78);
            this.btnOk.Margin = new System.Windows.Forms.Padding(4);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(80, 40);
            this.btnOk.TabIndex = 70;
            this.btnOk.Text = "확인";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(20, 50);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(115, 16);
            this.label4.TabIndex = 74;
            this.label4.Text = "ON/OFF 선택 :";
            // 
            // cboDIOFunc
            // 
            this.cboDIOFunc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDIOFunc.FormattingEnabled = true;
            this.cboDIOFunc.Location = new System.Drawing.Point(141, 17);
            this.cboDIOFunc.Name = "cboDIOFunc";
            this.cboDIOFunc.Size = new System.Drawing.Size(166, 24);
            this.cboDIOFunc.TabIndex = 73;
            this.cboDIOFunc.SelectedIndexChanged += new System.EventHandler(this.cboDIOFunc_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label6.Location = new System.Drawing.Point(48, 20);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(87, 16);
            this.label6.TabIndex = 72;
            this.label6.Text = "동작 선택 :";
            // 
            // cboOnOff
            // 
            this.cboOnOff.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboOnOff.FormattingEnabled = true;
            this.cboOnOff.Items.AddRange(new object[] {
            "OFF",
            "ON"});
            this.cboOnOff.Location = new System.Drawing.Point(141, 47);
            this.cboOnOff.Name = "cboOnOff";
            this.cboOnOff.Size = new System.Drawing.Size(166, 24);
            this.cboOnOff.TabIndex = 77;
            this.cboOnOff.SelectedIndexChanged += new System.EventHandler(this.cboOnOff_SelectedIndexChanged);
            // 
            // frmFuncDIO
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(324, 132);
            this.Controls.Add(this.cboOnOff);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cboDIOFunc);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmFuncDIO";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Digital I/O 동작 기능";
            this.Load += new System.EventHandler(this.frmFuncDIO_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cboDIOFunc;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cboOnOff;
    }
}