namespace AutoAssembler
{
    partial class frmCameraSetting
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
            this.cboCamScreenSize = new System.Windows.Forms.ComboBox();
            this.cboZoomPortNum = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtFrameRate = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnEnd = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label7 = new System.Windows.Forms.Label();
            this.lstCameraList = new System.Windows.Forms.ListView();
            this.UIDList = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.BusResetWorker = new System.ComponentModel.BackgroundWorker();
            this.FpsUpdate = new System.ComponentModel.BackgroundWorker();
            this.DisplayUpdate = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(796, 467);
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
            this.btnOk.Location = new System.Drawing.Point(708, 467);
            this.btnOk.Margin = new System.Windows.Forms.Padding(4);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(80, 40);
            this.btnOk.TabIndex = 70;
            this.btnOk.Text = "확인";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // cboCamScreenSize
            // 
            this.cboCamScreenSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCamScreenSize.FormattingEnabled = true;
            this.cboCamScreenSize.Items.AddRange(new object[] {
            "640 * 480",
            "800 * 600",
            "1024 * 768",
            "1280 * 1024",
            "1920 * 1080"});
            this.cboCamScreenSize.Location = new System.Drawing.Point(263, 325);
            this.cboCamScreenSize.Name = "cboCamScreenSize";
            this.cboCamScreenSize.Size = new System.Drawing.Size(160, 24);
            this.cboCamScreenSize.TabIndex = 89;
            this.cboCamScreenSize.SelectedIndexChanged += new System.EventHandler(this.cboCamScreenSize_SelectedIndexChanged);
            // 
            // cboZoomPortNum
            // 
            this.cboZoomPortNum.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboZoomPortNum.FormattingEnabled = true;
            this.cboZoomPortNum.Items.AddRange(new object[] {
            "COM01",
            "COM02",
            "COM03",
            "COM04",
            "COM05",
            "COM06",
            "COM07",
            "COM08",
            "COM09",
            "COM10"});
            this.cboZoomPortNum.Location = new System.Drawing.Point(263, 387);
            this.cboZoomPortNum.Name = "cboZoomPortNum";
            this.cboZoomPortNum.Size = new System.Drawing.Size(160, 24);
            this.cboZoomPortNum.TabIndex = 87;
            this.cboZoomPortNum.SelectedIndexChanged += new System.EventHandler(this.cboZoomPortNum_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(91, 390);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(166, 16);
            this.label5.TabIndex = 85;
            this.label5.Text = "Zoom && Focus 포트 :";
            // 
            // txtFrameRate
            // 
            this.txtFrameRate.Location = new System.Drawing.Point(263, 355);
            this.txtFrameRate.Name = "txtFrameRate";
            this.txtFrameRate.Size = new System.Drawing.Size(160, 26);
            this.txtFrameRate.TabIndex = 84;
            this.txtFrameRate.TextChanged += new System.EventHandler(this.txtFrameRate_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(161, 358);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 16);
            this.label4.TabIndex = 83;
            this.label4.Text = "FrameRate :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(170, 328);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 16);
            this.label3.TabIndex = 82;
            this.label3.Text = "화면 크기 :";
            // 
            // txtIP
            // 
            this.txtIP.Location = new System.Drawing.Point(263, 293);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(160, 26);
            this.txtIP.TabIndex = 81;
            this.txtIP.TextChanged += new System.EventHandler(this.txtIP_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(189, 296);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 16);
            this.label2.TabIndex = 80;
            this.label2.Text = "연결 IP :";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(263, 231);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(160, 26);
            this.txtName.TabIndex = 79;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(154, 234);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 16);
            this.label1.TabIndex = 78;
            this.label1.Text = "카메라 이름 :";
            // 
            // btnEnd
            // 
            this.btnEnd.Location = new System.Drawing.Point(756, 420);
            this.btnEnd.Name = "btnEnd";
            this.btnEnd.Size = new System.Drawing.Size(120, 40);
            this.btnEnd.TabIndex = 75;
            this.btnEnd.Text = "카메라 중지";
            this.btnEnd.UseVisualStyleBackColor = true;
            this.btnEnd.Click += new System.EventHandler(this.btnEnd_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(630, 420);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(120, 40);
            this.btnStart.TabIndex = 74;
            this.btnStart.Text = "카메라 시작";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Gray;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(429, 39);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(447, 372);
            this.pictureBox1.TabIndex = 73;
            this.pictureBox1.TabStop = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(20, 20);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(119, 16);
            this.label7.TabIndex = 122;
            this.label7.Text = "카메라 리스트 :";
            // 
            // lstCameraList
            // 
            this.lstCameraList.FullRowSelect = true;
            this.lstCameraList.Location = new System.Drawing.Point(23, 39);
            this.lstCameraList.Name = "lstCameraList";
            this.lstCameraList.Size = new System.Drawing.Size(400, 186);
            this.lstCameraList.TabIndex = 121;
            this.lstCameraList.UseCompatibleStateImageBehavior = false;
            this.lstCameraList.View = System.Windows.Forms.View.Details;
            this.lstCameraList.SelectedIndexChanged += new System.EventHandler(this.lstCameraList_SelectedIndexChanged);
            // 
            // UIDList
            // 
            this.UIDList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.UIDList.FormattingEnabled = true;
            this.UIDList.Location = new System.Drawing.Point(263, 263);
            this.UIDList.Name = "UIDList";
            this.UIDList.Size = new System.Drawing.Size(160, 24);
            this.UIDList.TabIndex = 124;
            this.UIDList.SelectedIndexChanged += new System.EventHandler(this.UIDList_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(173, 266);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(84, 16);
            this.label6.TabIndex = 123;
            this.label6.Text = "카메라 ID :";
            // 
            // frmCameraSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(889, 520);
            this.Controls.Add(this.UIDList);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lstCameraList);
            this.Controls.Add(this.cboCamScreenSize);
            this.Controls.Add(this.cboZoomPortNum);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtFrameRate);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtIP);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnEnd);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmCameraSetting";
            this.Text = "카메라 설정";
            this.Load += new System.EventHandler(this.frmCamSetting_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.ComboBox cboCamScreenSize;
        private System.Windows.Forms.ComboBox cboZoomPortNum;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtFrameRate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnEnd;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ListView lstCameraList;
        private System.Windows.Forms.ComboBox UIDList;
        private System.Windows.Forms.Label label6;
        private System.ComponentModel.BackgroundWorker BusResetWorker;
        private System.ComponentModel.BackgroundWorker FpsUpdate;
        private System.ComponentModel.BackgroundWorker DisplayUpdate;
    }
}