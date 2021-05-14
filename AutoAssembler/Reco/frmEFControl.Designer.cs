namespace AutoAssembler
{
    partial class frmEFControl
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
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.picDXFIndexA = new System.Windows.Forms.PictureBox();
            this.picDXFIndexB = new System.Windows.Forms.PictureBox();
            this.btDXFOpenA = new System.Windows.Forms.Button();
            this.btDXFOpenB = new System.Windows.Forms.Button();
            this.btRPosUp = new System.Windows.Forms.Button();
            this.btRPosDown = new System.Windows.Forms.Button();
            this.txtRPos = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btEndPosUp = new System.Windows.Forms.Button();
            this.btEndPosDown = new System.Windows.Forms.Button();
            this.txtEndPos = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btStartPosUp = new System.Windows.Forms.Button();
            this.btStartPosDown = new System.Windows.Forms.Button();
            this.txtStartPos = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.btEFTestStop = new System.Windows.Forms.Button();
            this.btEFTestStart = new System.Windows.Forms.Button();
            this.btClose = new System.Windows.Forms.Button();
            this.btSave = new System.Windows.Forms.Button();
            this.txtDXFPress80Pos = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtDXFEndPos = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtDXFStartPos = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtDXFPress100Pos = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picDXFIndexA)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picDXFIndexB)).BeginInit();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // picDXFIndexA
            // 
            this.picDXFIndexA.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picDXFIndexA.Location = new System.Drawing.Point(12, 12);
            this.picDXFIndexA.Name = "picDXFIndexA";
            this.picDXFIndexA.Size = new System.Drawing.Size(569, 434);
            this.picDXFIndexA.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picDXFIndexA.TabIndex = 1;
            this.picDXFIndexA.TabStop = false;
            this.picDXFIndexA.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picDXFIndexA_MouseDown);
            // 
            // picDXFIndexB
            // 
            this.picDXFIndexB.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picDXFIndexB.Location = new System.Drawing.Point(587, 387);
            this.picDXFIndexB.Name = "picDXFIndexB";
            this.picDXFIndexB.Size = new System.Drawing.Size(213, 61);
            this.picDXFIndexB.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picDXFIndexB.TabIndex = 2;
            this.picDXFIndexB.TabStop = false;
            this.picDXFIndexB.Visible = false;
            this.picDXFIndexB.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picDXFIndexB_MouseDown);
            // 
            // btDXFOpenA
            // 
            this.btDXFOpenA.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btDXFOpenA.Location = new System.Drawing.Point(12, 452);
            this.btDXFOpenA.Name = "btDXFOpenA";
            this.btDXFOpenA.Size = new System.Drawing.Size(182, 40);
            this.btDXFOpenA.TabIndex = 4;
            this.btDXFOpenA.Text = "IndexA-DXFFileLoad";
            this.btDXFOpenA.UseVisualStyleBackColor = true;
            this.btDXFOpenA.Click += new System.EventHandler(this.btDXFOpenA_Click);
            // 
            // btDXFOpenB
            // 
            this.btDXFOpenB.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btDXFOpenB.Location = new System.Drawing.Point(587, 341);
            this.btDXFOpenB.Name = "btDXFOpenB";
            this.btDXFOpenB.Size = new System.Drawing.Size(213, 40);
            this.btDXFOpenB.TabIndex = 5;
            this.btDXFOpenB.Text = "IndexB-DXFFileLoad";
            this.btDXFOpenB.UseVisualStyleBackColor = true;
            this.btDXFOpenB.Visible = false;
            this.btDXFOpenB.Click += new System.EventHandler(this.btDXFOpenB_Click);
            // 
            // btRPosUp
            // 
            this.btRPosUp.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btRPosUp.Location = new System.Drawing.Point(958, 67);
            this.btRPosUp.Name = "btRPosUp";
            this.btRPosUp.Size = new System.Drawing.Size(27, 23);
            this.btRPosUp.TabIndex = 120;
            this.btRPosUp.Text = ">";
            this.btRPosUp.UseVisualStyleBackColor = true;
            // 
            // btRPosDown
            // 
            this.btRPosDown.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btRPosDown.Location = new System.Drawing.Point(733, 67);
            this.btRPosDown.Name = "btRPosDown";
            this.btRPosDown.Size = new System.Drawing.Size(27, 23);
            this.btRPosDown.TabIndex = 119;
            this.btRPosDown.Text = "<";
            this.btRPosDown.UseVisualStyleBackColor = true;
            // 
            // txtRPos
            // 
            this.txtRPos.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtRPos.Location = new System.Drawing.Point(766, 67);
            this.txtRPos.Name = "txtRPos";
            this.txtRPos.Size = new System.Drawing.Size(186, 26);
            this.txtRPos.TabIndex = 118;
            this.txtRPos.Text = "0.0";
            this.txtRPos.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(667, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 16);
            this.label2.TabIndex = 117;
            this.label2.Text = "R위치 :";
            // 
            // btEndPosUp
            // 
            this.btEndPosUp.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btEndPosUp.Location = new System.Drawing.Point(958, 38);
            this.btEndPosUp.Name = "btEndPosUp";
            this.btEndPosUp.Size = new System.Drawing.Size(27, 23);
            this.btEndPosUp.TabIndex = 116;
            this.btEndPosUp.Text = ">";
            this.btEndPosUp.UseVisualStyleBackColor = true;
            // 
            // btEndPosDown
            // 
            this.btEndPosDown.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btEndPosDown.Location = new System.Drawing.Point(733, 38);
            this.btEndPosDown.Name = "btEndPosDown";
            this.btEndPosDown.Size = new System.Drawing.Size(27, 23);
            this.btEndPosDown.TabIndex = 115;
            this.btEndPosDown.Text = "<";
            this.btEndPosDown.UseVisualStyleBackColor = true;
            // 
            // txtEndPos
            // 
            this.txtEndPos.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtEndPos.Location = new System.Drawing.Point(766, 38);
            this.txtEndPos.Name = "txtEndPos";
            this.txtEndPos.Size = new System.Drawing.Size(186, 26);
            this.txtEndPos.TabIndex = 114;
            this.txtEndPos.Text = "0.0";
            this.txtEndPos.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(664, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 16);
            this.label1.TabIndex = 113;
            this.label1.Text = "끝위치 :";
            // 
            // btStartPosUp
            // 
            this.btStartPosUp.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btStartPosUp.Location = new System.Drawing.Point(958, 9);
            this.btStartPosUp.Name = "btStartPosUp";
            this.btStartPosUp.Size = new System.Drawing.Size(27, 23);
            this.btStartPosUp.TabIndex = 112;
            this.btStartPosUp.Text = ">";
            this.btStartPosUp.UseVisualStyleBackColor = true;
            // 
            // btStartPosDown
            // 
            this.btStartPosDown.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btStartPosDown.Location = new System.Drawing.Point(733, 9);
            this.btStartPosDown.Name = "btStartPosDown";
            this.btStartPosDown.Size = new System.Drawing.Size(27, 23);
            this.btStartPosDown.TabIndex = 111;
            this.btStartPosDown.Text = "<";
            this.btStartPosDown.UseVisualStyleBackColor = true;
            // 
            // txtStartPos
            // 
            this.txtStartPos.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtStartPos.Location = new System.Drawing.Point(766, 9);
            this.txtStartPos.Name = "txtStartPos";
            this.txtStartPos.Size = new System.Drawing.Size(186, 26);
            this.txtStartPos.TabIndex = 110;
            this.txtStartPos.Text = "0.0";
            this.txtStartPos.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label8.Location = new System.Drawing.Point(647, 12);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(82, 16);
            this.label8.TabIndex = 109;
            this.label8.Text = "시작위치 :";
            // 
            // btEFTestStop
            // 
            this.btEFTestStop.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btEFTestStop.Location = new System.Drawing.Point(706, 451);
            this.btEFTestStop.Name = "btEFTestStop";
            this.btEFTestStop.Size = new System.Drawing.Size(94, 41);
            this.btEFTestStop.TabIndex = 124;
            this.btEFTestStop.Text = "동작중지";
            this.btEFTestStop.UseVisualStyleBackColor = true;
            // 
            // btEFTestStart
            // 
            this.btEFTestStart.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btEFTestStart.Location = new System.Drawing.Point(587, 452);
            this.btEFTestStart.Name = "btEFTestStart";
            this.btEFTestStart.Size = new System.Drawing.Size(113, 41);
            this.btEFTestStart.TabIndex = 123;
            this.btEFTestStart.Text = "동작테스트";
            this.btEFTestStart.UseVisualStyleBackColor = true;
            // 
            // btClose
            // 
            this.btClose.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btClose.Location = new System.Drawing.Point(906, 452);
            this.btClose.Name = "btClose";
            this.btClose.Size = new System.Drawing.Size(79, 41);
            this.btClose.TabIndex = 122;
            this.btClose.Text = "닫기";
            this.btClose.UseVisualStyleBackColor = true;
            this.btClose.Click += new System.EventHandler(this.btClose_Click);
            // 
            // btSave
            // 
            this.btSave.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btSave.Location = new System.Drawing.Point(806, 452);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(94, 41);
            this.btSave.TabIndex = 121;
            this.btSave.Text = "내용저장";
            this.btSave.UseVisualStyleBackColor = true;
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // txtDXFPress80Pos
            // 
            this.txtDXFPress80Pos.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtDXFPress80Pos.Location = new System.Drawing.Point(766, 178);
            this.txtDXFPress80Pos.Name = "txtDXFPress80Pos";
            this.txtDXFPress80Pos.Size = new System.Drawing.Size(186, 26);
            this.txtDXFPress80Pos.TabIndex = 130;
            this.txtDXFPress80Pos.Text = "0.0";
            this.txtDXFPress80Pos.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(637, 181);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(111, 16);
            this.label3.TabIndex = 129;
            this.label3.Text = "압축위치80% :";
            // 
            // txtDXFEndPos
            // 
            this.txtDXFEndPos.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtDXFEndPos.Location = new System.Drawing.Point(766, 149);
            this.txtDXFEndPos.Name = "txtDXFEndPos";
            this.txtDXFEndPos.Size = new System.Drawing.Size(186, 26);
            this.txtDXFEndPos.TabIndex = 128;
            this.txtDXFEndPos.Text = "0.0";
            this.txtDXFEndPos.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(650, 152);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(98, 16);
            this.label4.TabIndex = 127;
            this.label4.Text = "도면끝위치 :";
            // 
            // txtDXFStartPos
            // 
            this.txtDXFStartPos.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtDXFStartPos.Location = new System.Drawing.Point(766, 120);
            this.txtDXFStartPos.Name = "txtDXFStartPos";
            this.txtDXFStartPos.Size = new System.Drawing.Size(186, 26);
            this.txtDXFStartPos.TabIndex = 126;
            this.txtDXFStartPos.Text = "0.0";
            this.txtDXFStartPos.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.Location = new System.Drawing.Point(634, 123);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(114, 16);
            this.label5.TabIndex = 125;
            this.label5.Text = "도면시작위치 :";
            // 
            // txtDXFPress100Pos
            // 
            this.txtDXFPress100Pos.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtDXFPress100Pos.Location = new System.Drawing.Point(766, 205);
            this.txtDXFPress100Pos.Name = "txtDXFPress100Pos";
            this.txtDXFPress100Pos.Size = new System.Drawing.Size(186, 26);
            this.txtDXFPress100Pos.TabIndex = 132;
            this.txtDXFPress100Pos.Text = "0.0";
            this.txtDXFPress100Pos.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label6.Location = new System.Drawing.Point(628, 208);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(119, 16);
            this.label6.TabIndex = 131;
            this.label6.Text = "압축위치100% :";
            // 
            // frmEFControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(997, 499);
            this.Controls.Add(this.txtDXFPress100Pos);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtDXFPress80Pos);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtDXFEndPos);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtDXFStartPos);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btEFTestStop);
            this.Controls.Add(this.btEFTestStart);
            this.Controls.Add(this.btClose);
            this.Controls.Add(this.btSave);
            this.Controls.Add(this.btRPosUp);
            this.Controls.Add(this.btRPosDown);
            this.Controls.Add(this.txtRPos);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btEndPosUp);
            this.Controls.Add(this.btEndPosDown);
            this.Controls.Add(this.txtEndPos);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btStartPosUp);
            this.Controls.Add(this.btStartPosDown);
            this.Controls.Add(this.txtStartPos);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.btDXFOpenB);
            this.Controls.Add(this.btDXFOpenA);
            this.Controls.Add(this.picDXFIndexB);
            this.Controls.Add(this.picDXFIndexA);
            this.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmEFControl";
            this.Text = "EF제어";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmEFControl_FormClosing);
            this.Load += new System.EventHandler(this.frmEFControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picDXFIndexA)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picDXFIndexB)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.PictureBox picDXFIndexA;
        private System.Windows.Forms.PictureBox picDXFIndexB;
        private System.Windows.Forms.Button btDXFOpenA;
        private System.Windows.Forms.Button btDXFOpenB;
        private System.Windows.Forms.Button btRPosUp;
        private System.Windows.Forms.Button btRPosDown;
        private System.Windows.Forms.TextBox txtRPos;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btEndPosUp;
        private System.Windows.Forms.Button btEndPosDown;
        private System.Windows.Forms.TextBox txtEndPos;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btStartPosUp;
        private System.Windows.Forms.Button btStartPosDown;
        private System.Windows.Forms.TextBox txtStartPos;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btEFTestStop;
        private System.Windows.Forms.Button btEFTestStart;
        private System.Windows.Forms.Button btClose;
        private System.Windows.Forms.Button btSave;
        private System.Windows.Forms.TextBox txtDXFPress80Pos;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtDXFEndPos;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtDXFStartPos;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtDXFPress100Pos;
        private System.Windows.Forms.Label label6;
    }
}