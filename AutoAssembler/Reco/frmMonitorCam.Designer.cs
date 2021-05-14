namespace AutoAssembler
{
    partial class frmMonitorCam
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
            this.m_DisplayPanel = new System.Windows.Forms.Panel();
            this.m_PictureBox = new System.Windows.Forms.PictureBox();
            this.m_CameraList = new System.Windows.Forms.ComboBox();
            this.btCamStop = new System.Windows.Forms.Button();
            this.btCamStart = new System.Windows.Forms.Button();
            this.btClose = new System.Windows.Forms.Button();
            this.btSave = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnJogLampHome = new System.Windows.Forms.Button();
            this.btnJogLampPlus = new System.Windows.Forms.Button();
            this.btnJogLampMinus = new System.Windows.Forms.Button();
            this.txtLampValue = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.cboLampList = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.m_DisplayPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_PictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // m_DisplayPanel
            // 
            this.m_DisplayPanel.AutoScroll = true;
            this.m_DisplayPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.m_DisplayPanel.Controls.Add(this.m_PictureBox);
            this.m_DisplayPanel.Location = new System.Drawing.Point(8, 12);
            this.m_DisplayPanel.Name = "m_DisplayPanel";
            this.m_DisplayPanel.Size = new System.Drawing.Size(663, 500);
            this.m_DisplayPanel.TabIndex = 4;
            // 
            // m_PictureBox
            // 
            this.m_PictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_PictureBox.Location = new System.Drawing.Point(0, 0);
            this.m_PictureBox.Margin = new System.Windows.Forms.Padding(0);
            this.m_PictureBox.Name = "m_PictureBox";
            this.m_PictureBox.Size = new System.Drawing.Size(659, 496);
            this.m_PictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.m_PictureBox.TabIndex = 2;
            this.m_PictureBox.TabStop = false;
            this.m_PictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.m_PictureBox_Paint);
            // 
            // m_CameraList
            // 
            this.m_CameraList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_CameraList.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.m_CameraList.FormattingEnabled = true;
            this.m_CameraList.Location = new System.Drawing.Point(677, 12);
            this.m_CameraList.Name = "m_CameraList";
            this.m_CameraList.Size = new System.Drawing.Size(374, 24);
            this.m_CameraList.TabIndex = 5;
            this.m_CameraList.SelectedIndexChanged += new System.EventHandler(this.m_CameraList_SelectedIndexChanged);
            this.m_CameraList.Click += new System.EventHandler(this.m_CameraList_Click);
            // 
            // btCamStop
            // 
            this.btCamStop.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btCamStop.Location = new System.Drawing.Point(772, 470);
            this.btCamStop.Name = "btCamStop";
            this.btCamStop.Size = new System.Drawing.Size(88, 41);
            this.btCamStop.TabIndex = 128;
            this.btCamStop.Text = "CamStop";
            this.btCamStop.UseVisualStyleBackColor = true;
            this.btCamStop.Click += new System.EventHandler(this.btCamStop_Click);
            // 
            // btCamStart
            // 
            this.btCamStart.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btCamStart.Location = new System.Drawing.Point(678, 470);
            this.btCamStart.Name = "btCamStart";
            this.btCamStart.Size = new System.Drawing.Size(88, 41);
            this.btCamStart.TabIndex = 127;
            this.btCamStart.Text = "CamStart";
            this.btCamStart.UseVisualStyleBackColor = true;
            this.btCamStart.Click += new System.EventHandler(this.btCamStart_Click);
            // 
            // btClose
            // 
            this.btClose.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btClose.Location = new System.Drawing.Point(963, 468);
            this.btClose.Name = "btClose";
            this.btClose.Size = new System.Drawing.Size(88, 41);
            this.btClose.TabIndex = 126;
            this.btClose.Text = "닫기";
            this.btClose.UseVisualStyleBackColor = true;
            this.btClose.Click += new System.EventHandler(this.btClose_Click);
            // 
            // btSave
            // 
            this.btSave.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btSave.Location = new System.Drawing.Point(869, 469);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(88, 41);
            this.btSave.TabIndex = 125;
            this.btSave.Text = "내용저장";
            this.btSave.UseVisualStyleBackColor = true;
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(678, 193);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(373, 271);
            this.textBox1.TabIndex = 129;
            this.textBox1.Visible = false;
            // 
            // btnJogLampHome
            // 
            this.btnJogLampHome.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnJogLampHome.Location = new System.Drawing.Point(953, 128);
            this.btnJogLampHome.Name = "btnJogLampHome";
            this.btnJogLampHome.Size = new System.Drawing.Size(27, 25);
            this.btnJogLampHome.TabIndex = 163;
            this.btnJogLampHome.Text = "H";
            this.btnJogLampHome.UseVisualStyleBackColor = true;
            // 
            // btnJogLampPlus
            // 
            this.btnJogLampPlus.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnJogLampPlus.Location = new System.Drawing.Point(921, 129);
            this.btnJogLampPlus.Name = "btnJogLampPlus";
            this.btnJogLampPlus.Size = new System.Drawing.Size(27, 25);
            this.btnJogLampPlus.TabIndex = 162;
            this.btnJogLampPlus.Text = ">";
            this.btnJogLampPlus.UseVisualStyleBackColor = true;
            this.btnJogLampPlus.Click += new System.EventHandler(this.btnJogLampPlus_Click);
            // 
            // btnJogLampMinus
            // 
            this.btnJogLampMinus.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnJogLampMinus.Location = new System.Drawing.Point(812, 129);
            this.btnJogLampMinus.Name = "btnJogLampMinus";
            this.btnJogLampMinus.Size = new System.Drawing.Size(27, 25);
            this.btnJogLampMinus.TabIndex = 161;
            this.btnJogLampMinus.Text = "<";
            this.btnJogLampMinus.UseVisualStyleBackColor = true;
            this.btnJogLampMinus.Click += new System.EventHandler(this.btnJogLampMinus_Click);
            // 
            // txtLampValue
            // 
            this.txtLampValue.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtLampValue.Location = new System.Drawing.Point(845, 130);
            this.txtLampValue.Name = "txtLampValue";
            this.txtLampValue.Size = new System.Drawing.Size(74, 26);
            this.txtLampValue.TabIndex = 160;
            this.txtLampValue.Text = "50";
            this.txtLampValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtLampValue.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtLampValue_KeyPress);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label10.Location = new System.Drawing.Point(740, 133);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(66, 16);
            this.label10.TabIndex = 159;
            this.label10.Text = "조명값 :";
            // 
            // cboLampList
            // 
            this.cboLampList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLampList.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cboLampList.FormattingEnabled = true;
            this.cboLampList.Location = new System.Drawing.Point(802, 98);
            this.cboLampList.Name = "cboLampList";
            this.cboLampList.Size = new System.Drawing.Size(178, 24);
            this.cboLampList.TabIndex = 165;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label11.Location = new System.Drawing.Point(688, 102);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(114, 16);
            this.label11.TabIndex = 164;
            this.label11.Text = "조명제어항목 :";
            // 
            // frmMonitorCam
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1063, 521);
            this.Controls.Add(this.cboLampList);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.btnJogLampHome);
            this.Controls.Add(this.btnJogLampPlus);
            this.Controls.Add(this.btnJogLampMinus);
            this.Controls.Add(this.txtLampValue);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btCamStop);
            this.Controls.Add(this.btCamStart);
            this.Controls.Add(this.btClose);
            this.Controls.Add(this.btSave);
            this.Controls.Add(this.m_CameraList);
            this.Controls.Add(this.m_DisplayPanel);
            this.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMonitorCam";
            this.Text = "모니터링카메라";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMonitorCam_FormClosing);
            this.Load += new System.EventHandler(this.frmMonitorCam_Load);
            this.m_DisplayPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.m_PictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel m_DisplayPanel;
        private System.Windows.Forms.PictureBox m_PictureBox;
        private System.Windows.Forms.ComboBox m_CameraList;
        private System.Windows.Forms.Button btCamStop;
        private System.Windows.Forms.Button btCamStart;
        private System.Windows.Forms.Button btClose;
        private System.Windows.Forms.Button btSave;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnJogLampHome;
        private System.Windows.Forms.Button btnJogLampPlus;
        private System.Windows.Forms.Button btnJogLampMinus;
        private System.Windows.Forms.TextBox txtLampValue;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cboLampList;
        private System.Windows.Forms.Label label11;
    }
}