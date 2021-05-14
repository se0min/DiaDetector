namespace AutoAssembler
{
    partial class frmFuncLamp
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
            this.components = new System.ComponentModel.Container();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnLightingPlus = new System.Windows.Forms.Button();
            this.btnLightingMinus = new System.Windows.Forms.Button();
            this.txtLightingValue = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cboLightingName = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.timerLighting = new System.Windows.Forms.Timer(this.components);
            this.btnTest = new System.Windows.Forms.Button();
            this.btnLightingHome = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(285, 94);
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
            this.btnOk.Location = new System.Drawing.Point(197, 94);
            this.btnOk.Margin = new System.Windows.Forms.Padding(4);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(80, 40);
            this.btnOk.TabIndex = 70;
            this.btnOk.Text = "확인";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnLightingPlus
            // 
            this.btnLightingPlus.Location = new System.Drawing.Point(279, 47);
            this.btnLightingPlus.Name = "btnLightingPlus";
            this.btnLightingPlus.Size = new System.Drawing.Size(40, 40);
            this.btnLightingPlus.TabIndex = 77;
            this.btnLightingPlus.Text = ">";
            this.btnLightingPlus.UseVisualStyleBackColor = true;
            this.btnLightingPlus.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnLightingPlus_MouseDown);
            this.btnLightingPlus.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnLightingPlus_MouseUp);
            // 
            // btnLightingMinus
            // 
            this.btnLightingMinus.Location = new System.Drawing.Point(113, 47);
            this.btnLightingMinus.Name = "btnLightingMinus";
            this.btnLightingMinus.Size = new System.Drawing.Size(40, 40);
            this.btnLightingMinus.TabIndex = 76;
            this.btnLightingMinus.Text = "<";
            this.btnLightingMinus.UseVisualStyleBackColor = true;
            this.btnLightingMinus.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnLightingMinus_MouseDown);
            this.btnLightingMinus.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnLightingMinus_MouseUp);
            // 
            // txtLightingValue
            // 
            this.txtLightingValue.Location = new System.Drawing.Point(159, 56);
            this.txtLightingValue.Name = "txtLightingValue";
            this.txtLightingValue.Size = new System.Drawing.Size(114, 26);
            this.txtLightingValue.TabIndex = 75;
            this.txtLightingValue.Text = "0";
            this.txtLightingValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(20, 59);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(87, 16);
            this.label4.TabIndex = 74;
            this.label4.Text = "조명 밝기 :";
            // 
            // cboLightingName
            // 
            this.cboLightingName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLightingName.FormattingEnabled = true;
            this.cboLightingName.Location = new System.Drawing.Point(113, 17);
            this.cboLightingName.Name = "cboLightingName";
            this.cboLightingName.Size = new System.Drawing.Size(252, 24);
            this.cboLightingName.TabIndex = 73;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label6.Location = new System.Drawing.Point(20, 20);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(87, 16);
            this.label6.TabIndex = 72;
            this.label6.Text = "조명 선택 :";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(20, 75);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 16);
            this.label1.TabIndex = 78;
            this.label1.Text = "(0 ~ 1023)";
            // 
            // timerLighting
            // 
            this.timerLighting.Tick += new System.EventHandler(this.timerLighting_Tick);
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(109, 94);
            this.btnTest.Margin = new System.Windows.Forms.Padding(4);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(80, 40);
            this.btnTest.TabIndex = 79;
            this.btnTest.Text = "테스트";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // btnLightingHome
            // 
            this.btnLightingHome.Location = new System.Drawing.Point(325, 47);
            this.btnLightingHome.Name = "btnLightingHome";
            this.btnLightingHome.Size = new System.Drawing.Size(40, 40);
            this.btnLightingHome.TabIndex = 80;
            this.btnLightingHome.Text = "H";
            this.btnLightingHome.UseVisualStyleBackColor = true;
            this.btnLightingHome.Click += new System.EventHandler(this.btnLightingHome_Click);
            // 
            // frmFuncLamp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 152);
            this.Controls.Add(this.btnLightingHome);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnLightingPlus);
            this.Controls.Add(this.btnLightingMinus);
            this.Controls.Add(this.txtLightingValue);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cboLightingName);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmFuncLamp";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "조명 조정 기능";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmFuncLamp_FormClosing);
            this.Load += new System.EventHandler(this.frmFuncLamp_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnLightingPlus;
        private System.Windows.Forms.Button btnLightingMinus;
        private System.Windows.Forms.TextBox txtLightingValue;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cboLightingName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer timerLighting;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Button btnLightingHome;
    }
}