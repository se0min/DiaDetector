namespace AutoAssembler
{
    partial class frmFuncIndex
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
            this.btnClose = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnJogHome = new System.Windows.Forms.Button();
            this.btnJogPlus = new System.Windows.Forms.Button();
            this.btnJogMinus = new System.Windows.Forms.Button();
            this.txtAxisValue = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.timerAxis = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(344, 59);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 40);
            this.btnClose.TabIndex = 71;
            this.btnClose.Text = "닫기";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(256, 59);
            this.btnOk.Margin = new System.Windows.Forms.Padding(4);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(80, 40);
            this.btnOk.TabIndex = 70;
            this.btnOk.Text = "확인";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnJogHome
            // 
            this.btnJogHome.Location = new System.Drawing.Point(384, 12);
            this.btnJogHome.Name = "btnJogHome";
            this.btnJogHome.Size = new System.Drawing.Size(40, 40);
            this.btnJogHome.TabIndex = 129;
            this.btnJogHome.Text = "H";
            this.btnJogHome.UseVisualStyleBackColor = true;
            this.btnJogHome.Click += new System.EventHandler(this.btnJogHome_Click);
            // 
            // btnJogPlus
            // 
            this.btnJogPlus.Location = new System.Drawing.Point(338, 12);
            this.btnJogPlus.Name = "btnJogPlus";
            this.btnJogPlus.Size = new System.Drawing.Size(40, 40);
            this.btnJogPlus.TabIndex = 120;
            this.btnJogPlus.Text = ">";
            this.btnJogPlus.UseVisualStyleBackColor = true;
            this.btnJogPlus.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnJogPlus_MouseDown);
            this.btnJogPlus.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnJogPlus_MouseUp);
            // 
            // btnJogMinus
            // 
            this.btnJogMinus.Location = new System.Drawing.Point(126, 12);
            this.btnJogMinus.Name = "btnJogMinus";
            this.btnJogMinus.Size = new System.Drawing.Size(40, 40);
            this.btnJogMinus.TabIndex = 119;
            this.btnJogMinus.Text = "<";
            this.btnJogMinus.UseVisualStyleBackColor = true;
            this.btnJogMinus.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnJogMinus_MouseDown);
            this.btnJogMinus.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnJogMinus_MouseUp);
            // 
            // txtAxisValue
            // 
            this.txtAxisValue.Location = new System.Drawing.Point(172, 21);
            this.txtAxisValue.Name = "txtAxisValue";
            this.txtAxisValue.Size = new System.Drawing.Size(160, 26);
            this.txtAxisValue.TabIndex = 118;
            this.txtAxisValue.Text = "0.0";
            this.txtAxisValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtAxisValue.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtAxisValue_KeyDown);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(33, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(87, 16);
            this.label4.TabIndex = 117;
            this.label4.Text = "회전 각도 :";
            // 
            // timerAxis
            // 
            this.timerAxis.Tick += new System.EventHandler(this.timerAxis_Tick);
            // 
            // frmFuncIndex
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(439, 112);
            this.Controls.Add(this.btnJogHome);
            this.Controls.Add(this.btnJogPlus);
            this.Controls.Add(this.btnJogMinus);
            this.Controls.Add(this.txtAxisValue);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnOk);
            this.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmFuncIndex";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Index 회전 기능";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmFuncIndex_FormClosing);
            this.Load += new System.EventHandler(this.frmFuncIndex_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnJogHome;
        private System.Windows.Forms.Button btnJogPlus;
        private System.Windows.Forms.Button btnJogMinus;
        private System.Windows.Forms.TextBox txtAxisValue;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Timer timerAxis;
    }
}