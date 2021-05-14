namespace AutoAssembler
{
    partial class frmFuncAxisMove
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
            this.txtAxisValue = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.cboSelectHome = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cboSelectAxis = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.radioBtnSlow = new System.Windows.Forms.RadioButton();
            this.radioBtnMidium = new System.Windows.Forms.RadioButton();
            this.radioBtnFast = new System.Windows.Forms.RadioButton();
            this.cboEndWait = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtAxisValue
            // 
            this.txtAxisValue.Location = new System.Drawing.Point(200, 102);
            this.txtAxisValue.Name = "txtAxisValue";
            this.txtAxisValue.Size = new System.Drawing.Size(207, 26);
            this.txtAxisValue.TabIndex = 162;
            this.txtAxisValue.Text = "0.0";
            this.txtAxisValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtAxisValue.TextChanged += new System.EventHandler(this.txtAxisValue_TextChanged);
            this.txtAxisValue.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtAxisValue_KeyDown);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(327, 172);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 40);
            this.btnCancel.TabIndex = 160;
            this.btnCancel.Text = "취소";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(239, 172);
            this.btnOk.Margin = new System.Windows.Forms.Padding(4);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(80, 40);
            this.btnOk.TabIndex = 159;
            this.btnOk.Text = "확인";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // cboSelectHome
            // 
            this.cboSelectHome.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSelectHome.FormattingEnabled = true;
            this.cboSelectHome.Items.AddRange(new object[] {
            "아니오",
            "예"});
            this.cboSelectHome.Location = new System.Drawing.Point(200, 72);
            this.cboSelectHome.Name = "cboSelectHome";
            this.cboSelectHome.Size = new System.Drawing.Size(207, 24);
            this.cboSelectHome.TabIndex = 166;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(33, 75);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(161, 16);
            this.label4.TabIndex = 165;
            this.label4.Text = "이동 후 원점 클리어 :";
            // 
            // cboSelectAxis
            // 
            this.cboSelectAxis.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSelectAxis.FormattingEnabled = true;
            this.cboSelectAxis.Location = new System.Drawing.Point(200, 12);
            this.cboSelectAxis.Name = "cboSelectAxis";
            this.cboSelectAxis.Size = new System.Drawing.Size(207, 24);
            this.cboSelectAxis.TabIndex = 164;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label6.Location = new System.Drawing.Point(86, 15);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(108, 16);
            this.label6.TabIndex = 163;
            this.label6.Text = "이동 축 선택 :";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(128, 105);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 16);
            this.label1.TabIndex = 167;
            this.label1.Text = "좌표값 :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(106, 147);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 16);
            this.label2.TabIndex = 171;
            this.label2.Text = "속도 설정 :";
            // 
            // radioBtnSlow
            // 
            this.radioBtnSlow.AutoSize = true;
            this.radioBtnSlow.Location = new System.Drawing.Point(200, 145);
            this.radioBtnSlow.Name = "radioBtnSlow";
            this.radioBtnSlow.Size = new System.Drawing.Size(61, 20);
            this.radioBtnSlow.TabIndex = 172;
            this.radioBtnSlow.TabStop = true;
            this.radioBtnSlow.Tag = "1";
            this.radioBtnSlow.Text = "Slow";
            this.radioBtnSlow.UseVisualStyleBackColor = true;
            this.radioBtnSlow.CheckedChanged += new System.EventHandler(this.radioBtn_CheckedChanged);
            // 
            // radioBtnMidium
            // 
            this.radioBtnMidium.AutoSize = true;
            this.radioBtnMidium.Location = new System.Drawing.Point(267, 145);
            this.radioBtnMidium.Name = "radioBtnMidium";
            this.radioBtnMidium.Size = new System.Drawing.Size(76, 20);
            this.radioBtnMidium.TabIndex = 173;
            this.radioBtnMidium.TabStop = true;
            this.radioBtnMidium.Tag = "2";
            this.radioBtnMidium.Text = "Midium";
            this.radioBtnMidium.UseVisualStyleBackColor = true;
            this.radioBtnMidium.CheckedChanged += new System.EventHandler(this.radioBtn_CheckedChanged);
            // 
            // radioBtnFast
            // 
            this.radioBtnFast.AutoSize = true;
            this.radioBtnFast.Location = new System.Drawing.Point(349, 145);
            this.radioBtnFast.Name = "radioBtnFast";
            this.radioBtnFast.Size = new System.Drawing.Size(58, 20);
            this.radioBtnFast.TabIndex = 174;
            this.radioBtnFast.TabStop = true;
            this.radioBtnFast.Tag = "3";
            this.radioBtnFast.Text = "Fast";
            this.radioBtnFast.UseVisualStyleBackColor = true;
            this.radioBtnFast.CheckedChanged += new System.EventHandler(this.radioBtn_CheckedChanged);
            // 
            // cboEndWait
            // 
            this.cboEndWait.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboEndWait.FormattingEnabled = true;
            this.cboEndWait.Items.AddRange(new object[] {
            "예",
            "아니오"});
            this.cboEndWait.Location = new System.Drawing.Point(200, 42);
            this.cboEndWait.Name = "cboEndWait";
            this.cboEndWait.Size = new System.Drawing.Size(207, 24);
            this.cboEndWait.TabIndex = 176;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(86, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(124, 16);
            this.label3.TabIndex = 175;
            this.label3.Text = "이동 완료 대기 :";
            // 
            // frmFuncAxisMove
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(424, 227);
            this.Controls.Add(this.cboEndWait);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.radioBtnFast);
            this.Controls.Add(this.radioBtnMidium);
            this.Controls.Add(this.radioBtnSlow);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboSelectHome);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cboSelectAxis);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtAxisValue);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmFuncAxisMove";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "개별 축 이동";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmFuncAxisMove_FormClosing);
            this.Load += new System.EventHandler(this.frmFuncAxisMove_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtAxisValue;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.ComboBox cboSelectHome;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cboSelectAxis;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton radioBtnSlow;
        private System.Windows.Forms.RadioButton radioBtnMidium;
        private System.Windows.Forms.RadioButton radioBtnFast;
        private System.Windows.Forms.ComboBox cboEndWait;
        private System.Windows.Forms.Label label3;
    }
}