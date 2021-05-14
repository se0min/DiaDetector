namespace AutoAssembler
{
    partial class frmDIOSetting
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
            this.txtInputName = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.lstInputView = new System.Windows.Forms.ListView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lstOutputView = new System.Windows.Forms.ListView();
            this.txtOutputName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnOffTest = new System.Windows.Forms.Button();
            this.btnOnTest = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(581, 562);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 40);
            this.btnCancel.TabIndex = 73;
            this.btnCancel.Text = "취소";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(493, 562);
            this.btnOk.Margin = new System.Windows.Forms.Padding(4);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(80, 40);
            this.btnOk.TabIndex = 72;
            this.btnOk.Text = "확인";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // txtInputName
            // 
            this.txtInputName.Location = new System.Drawing.Point(173, 528);
            this.txtInputName.Margin = new System.Windows.Forms.Padding(4);
            this.txtInputName.Name = "txtInputName";
            this.txtInputName.Size = new System.Drawing.Size(160, 26);
            this.txtInputName.TabIndex = 110;
            this.txtInputName.TextChanged += new System.EventHandler(this.txtInputName_TextChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(13, 531);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(127, 16);
            this.label14.TabIndex = 109;
            this.label14.Text = "Input 접점 이름 :";
            // 
            // lstInputView
            // 
            this.lstInputView.FullRowSelect = true;
            this.lstInputView.Location = new System.Drawing.Point(13, 40);
            this.lstInputView.Margin = new System.Windows.Forms.Padding(4);
            this.lstInputView.Name = "lstInputView";
            this.lstInputView.Size = new System.Drawing.Size(320, 480);
            this.lstInputView.TabIndex = 106;
            this.lstInputView.UseCompatibleStateImageBehavior = false;
            this.lstInputView.View = System.Windows.Forms.View.Details;
            this.lstInputView.SelectedIndexChanged += new System.EventHandler(this.lstInputView_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 20);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(153, 16);
            this.label1.TabIndex = 111;
            this.label1.Text = "Digital Input 리스트 :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(338, 20);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(167, 16);
            this.label2.TabIndex = 112;
            this.label2.Text = "Digital Output 리스트 :";
            // 
            // lstOutputView
            // 
            this.lstOutputView.FullRowSelect = true;
            this.lstOutputView.Location = new System.Drawing.Point(341, 40);
            this.lstOutputView.Margin = new System.Windows.Forms.Padding(4);
            this.lstOutputView.Name = "lstOutputView";
            this.lstOutputView.Size = new System.Drawing.Size(320, 480);
            this.lstOutputView.TabIndex = 113;
            this.lstOutputView.UseCompatibleStateImageBehavior = false;
            this.lstOutputView.View = System.Windows.Forms.View.Details;
            this.lstOutputView.SelectedIndexChanged += new System.EventHandler(this.lstOutputView_SelectedIndexChanged);
            // 
            // txtOutputName
            // 
            this.txtOutputName.Location = new System.Drawing.Point(501, 528);
            this.txtOutputName.Margin = new System.Windows.Forms.Padding(4);
            this.txtOutputName.Name = "txtOutputName";
            this.txtOutputName.Size = new System.Drawing.Size(160, 26);
            this.txtOutputName.TabIndex = 115;
            this.txtOutputName.TextChanged += new System.EventHandler(this.txtOutputName_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(341, 531);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(141, 16);
            this.label3.TabIndex = 114;
            this.label3.Text = "Output 접점 이름 :";
            // 
            // btnOffTest
            // 
            this.btnOffTest.Location = new System.Drawing.Point(405, 562);
            this.btnOffTest.Margin = new System.Windows.Forms.Padding(4);
            this.btnOffTest.Name = "btnOffTest";
            this.btnOffTest.Size = new System.Drawing.Size(80, 40);
            this.btnOffTest.TabIndex = 116;
            this.btnOffTest.Text = "OFF";
            this.btnOffTest.UseVisualStyleBackColor = true;
            this.btnOffTest.Click += new System.EventHandler(this.btnOffTest_Click);
            // 
            // btnOnTest
            // 
            this.btnOnTest.Location = new System.Drawing.Point(317, 562);
            this.btnOnTest.Margin = new System.Windows.Forms.Padding(4);
            this.btnOnTest.Name = "btnOnTest";
            this.btnOnTest.Size = new System.Drawing.Size(80, 40);
            this.btnOnTest.TabIndex = 117;
            this.btnOnTest.Text = "ON";
            this.btnOnTest.UseVisualStyleBackColor = true;
            this.btnOnTest.Click += new System.EventHandler(this.btnOnTest_Click);
            // 
            // frmDIOSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(679, 622);
            this.Controls.Add(this.btnOnTest);
            this.Controls.Add(this.btnOffTest);
            this.Controls.Add(this.txtOutputName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lstOutputView);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtInputName);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.lstInputView);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmDIOSetting";
            this.Text = "Digital I/O 설정";
            this.Load += new System.EventHandler(this.frmDIOSetting_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.TextBox txtInputName;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ListView lstInputView;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListView lstOutputView;
        private System.Windows.Forms.TextBox txtOutputName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnOffTest;
        private System.Windows.Forms.Button btnOnTest;
    }
}