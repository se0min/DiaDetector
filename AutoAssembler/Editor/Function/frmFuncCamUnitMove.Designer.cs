namespace AutoAssembler
{
    partial class frmFuncCamUnitMove
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
            this.btnJogZHome = new System.Windows.Forms.Button();
            this.btnJogYHome = new System.Windows.Forms.Button();
            this.btnJogXHome = new System.Windows.Forms.Button();
            this.btnJogZMinus = new System.Windows.Forms.Button();
            this.btnJogZPlus = new System.Windows.Forms.Button();
            this.txtZAxisValue = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnJogYMinus = new System.Windows.Forms.Button();
            this.btnJogYPlus = new System.Windows.Forms.Button();
            this.txtYAxisValue = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnJogXMinus = new System.Windows.Forms.Button();
            this.btnJogXPlus = new System.Windows.Forms.Button();
            this.txtXAxisValue = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.timerAxis = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(344, 151);
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
            this.btnOk.Location = new System.Drawing.Point(256, 151);
            this.btnOk.Margin = new System.Windows.Forms.Padding(4);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(80, 40);
            this.btnOk.TabIndex = 70;
            this.btnOk.Text = "확인";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnJogZHome
            // 
            this.btnJogZHome.Location = new System.Drawing.Point(384, 104);
            this.btnJogZHome.Name = "btnJogZHome";
            this.btnJogZHome.Size = new System.Drawing.Size(40, 40);
            this.btnJogZHome.TabIndex = 146;
            this.btnJogZHome.Text = "H";
            this.btnJogZHome.UseVisualStyleBackColor = true;
            this.btnJogZHome.Click += new System.EventHandler(this.btnJogZHome_Click);
            // 
            // btnJogYHome
            // 
            this.btnJogYHome.Location = new System.Drawing.Point(384, 58);
            this.btnJogYHome.Name = "btnJogYHome";
            this.btnJogYHome.Size = new System.Drawing.Size(40, 40);
            this.btnJogYHome.TabIndex = 145;
            this.btnJogYHome.Text = "H";
            this.btnJogYHome.UseVisualStyleBackColor = true;
            this.btnJogYHome.Click += new System.EventHandler(this.btnJogYHome_Click);
            // 
            // btnJogXHome
            // 
            this.btnJogXHome.Location = new System.Drawing.Point(384, 12);
            this.btnJogXHome.Name = "btnJogXHome";
            this.btnJogXHome.Size = new System.Drawing.Size(40, 40);
            this.btnJogXHome.TabIndex = 144;
            this.btnJogXHome.Text = "H";
            this.btnJogXHome.UseVisualStyleBackColor = true;
            this.btnJogXHome.Click += new System.EventHandler(this.btnJogXHome_Click);
            // 
            // btnJogZMinus
            // 
            this.btnJogZMinus.Location = new System.Drawing.Point(126, 104);
            this.btnJogZMinus.Name = "btnJogZMinus";
            this.btnJogZMinus.Size = new System.Drawing.Size(40, 40);
            this.btnJogZMinus.TabIndex = 143;
            this.btnJogZMinus.Text = "<";
            this.btnJogZMinus.UseVisualStyleBackColor = true;
            this.btnJogZMinus.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnJogZMinus_MouseDown);
            this.btnJogZMinus.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnJogZMinus_MouseUp);
            // 
            // btnJogZPlus
            // 
            this.btnJogZPlus.Location = new System.Drawing.Point(338, 104);
            this.btnJogZPlus.Name = "btnJogZPlus";
            this.btnJogZPlus.Size = new System.Drawing.Size(40, 40);
            this.btnJogZPlus.TabIndex = 142;
            this.btnJogZPlus.Text = ">";
            this.btnJogZPlus.UseVisualStyleBackColor = true;
            this.btnJogZPlus.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnJogZPlus_MouseDown);
            this.btnJogZPlus.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnJogZPlus_MouseUp);
            // 
            // txtZAxisValue
            // 
            this.txtZAxisValue.Location = new System.Drawing.Point(172, 113);
            this.txtZAxisValue.Name = "txtZAxisValue";
            this.txtZAxisValue.Size = new System.Drawing.Size(160, 26);
            this.txtZAxisValue.TabIndex = 141;
            this.txtZAxisValue.Text = "0.0";
            this.txtZAxisValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtZAxisValue.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtZAxisValue_KeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(39, 116);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 16);
            this.label2.TabIndex = 140;
            this.label2.Text = "Z축 이동 :";
            // 
            // btnJogYMinus
            // 
            this.btnJogYMinus.Location = new System.Drawing.Point(126, 58);
            this.btnJogYMinus.Name = "btnJogYMinus";
            this.btnJogYMinus.Size = new System.Drawing.Size(40, 40);
            this.btnJogYMinus.TabIndex = 139;
            this.btnJogYMinus.Text = "<";
            this.btnJogYMinus.UseVisualStyleBackColor = true;
            this.btnJogYMinus.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnJogYMinus_MouseDown);
            this.btnJogYMinus.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnJogYMinus_MouseUp);
            // 
            // btnJogYPlus
            // 
            this.btnJogYPlus.Location = new System.Drawing.Point(338, 58);
            this.btnJogYPlus.Name = "btnJogYPlus";
            this.btnJogYPlus.Size = new System.Drawing.Size(40, 40);
            this.btnJogYPlus.TabIndex = 138;
            this.btnJogYPlus.Text = ">";
            this.btnJogYPlus.UseVisualStyleBackColor = true;
            this.btnJogYPlus.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnJogYPlus_MouseDown);
            this.btnJogYPlus.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnJogYPlus_MouseUp);
            // 
            // txtYAxisValue
            // 
            this.txtYAxisValue.Location = new System.Drawing.Point(172, 67);
            this.txtYAxisValue.Name = "txtYAxisValue";
            this.txtYAxisValue.Size = new System.Drawing.Size(160, 26);
            this.txtYAxisValue.TabIndex = 137;
            this.txtYAxisValue.Text = "0.0";
            this.txtYAxisValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtYAxisValue.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtYAxisValue_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(39, 70);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 16);
            this.label1.TabIndex = 136;
            this.label1.Text = "Y축 이동 :";
            // 
            // btnJogXMinus
            // 
            this.btnJogXMinus.Location = new System.Drawing.Point(126, 12);
            this.btnJogXMinus.Name = "btnJogXMinus";
            this.btnJogXMinus.Size = new System.Drawing.Size(40, 40);
            this.btnJogXMinus.TabIndex = 135;
            this.btnJogXMinus.Text = "<";
            this.btnJogXMinus.UseVisualStyleBackColor = true;
            this.btnJogXMinus.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnJogXMinus_MouseDown);
            this.btnJogXMinus.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnJogXMinus_MouseUp);
            // 
            // btnJogXPlus
            // 
            this.btnJogXPlus.Location = new System.Drawing.Point(338, 12);
            this.btnJogXPlus.Name = "btnJogXPlus";
            this.btnJogXPlus.Size = new System.Drawing.Size(40, 40);
            this.btnJogXPlus.TabIndex = 134;
            this.btnJogXPlus.Text = ">";
            this.btnJogXPlus.UseVisualStyleBackColor = true;
            this.btnJogXPlus.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnJogXPlus_MouseDown);
            this.btnJogXPlus.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnJogXPlus_MouseUp);
            // 
            // txtXAxisValue
            // 
            this.txtXAxisValue.Location = new System.Drawing.Point(172, 21);
            this.txtXAxisValue.Name = "txtXAxisValue";
            this.txtXAxisValue.Size = new System.Drawing.Size(160, 26);
            this.txtXAxisValue.TabIndex = 133;
            this.txtXAxisValue.Text = "0.0";
            this.txtXAxisValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtXAxisValue.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtXAxisValue_KeyDown);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(38, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(82, 16);
            this.label4.TabIndex = 132;
            this.label4.Text = "X축 이동 :";
            // 
            // timerAxis
            // 
            this.timerAxis.Tick += new System.EventHandler(this.timerAxis_Tick);
            // 
            // frmFuncCamUnitMove
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(439, 207);
            this.Controls.Add(this.btnJogXPlus);
            this.Controls.Add(this.btnJogZHome);
            this.Controls.Add(this.btnJogYHome);
            this.Controls.Add(this.btnJogXHome);
            this.Controls.Add(this.btnJogZMinus);
            this.Controls.Add(this.btnJogZPlus);
            this.Controls.Add(this.txtZAxisValue);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnJogYMinus);
            this.Controls.Add(this.btnJogYPlus);
            this.Controls.Add(this.txtYAxisValue);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnJogXMinus);
            this.Controls.Add(this.txtXAxisValue);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmFuncCamUnitMove";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "축 이동 기능";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmFuncMove_FormClosing);
            this.Load += new System.EventHandler(this.frmFuncMove_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnJogZHome;
        private System.Windows.Forms.Button btnJogYHome;
        private System.Windows.Forms.Button btnJogXHome;
        private System.Windows.Forms.Button btnJogZMinus;
        private System.Windows.Forms.Button btnJogZPlus;
        private System.Windows.Forms.TextBox txtZAxisValue;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnJogYMinus;
        private System.Windows.Forms.Button btnJogYPlus;
        private System.Windows.Forms.TextBox txtYAxisValue;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnJogXMinus;
        private System.Windows.Forms.Button btnJogXPlus;
        private System.Windows.Forms.TextBox txtXAxisValue;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Timer timerAxis;
    }
}