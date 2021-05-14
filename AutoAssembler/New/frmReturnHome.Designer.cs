namespace AutoAssembler
{
    partial class frmReturnHome
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
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.button1 = new System.Windows.Forms.Button();
            this.btnCamZ = new System.Windows.Forms.Button();
            this.timerHomeReturn = new System.Windows.Forms.Timer(this.components);
            this.button2 = new System.Windows.Forms.Button();
            this.btnCamY = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.btnCamX = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.btnBackCamZ = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.btnIndexX = new System.Windows.Forms.Button();
            this.btntttt = new System.Windows.Forms.Button();
            this.btnVBlockZ = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.btnRollingFixA = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.btnRollingFixB = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.btnRollingMoveA = new System.Windows.Forms.Button();
            this.button13 = new System.Windows.Forms.Button();
            this.btnRollingMoveB = new System.Windows.Forms.Button();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.button1);
            this.flowLayoutPanel1.Controls.Add(this.btnCamZ);
            this.flowLayoutPanel1.Controls.Add(this.button2);
            this.flowLayoutPanel1.Controls.Add(this.btnCamY);
            this.flowLayoutPanel1.Controls.Add(this.button4);
            this.flowLayoutPanel1.Controls.Add(this.btnCamX);
            this.flowLayoutPanel1.Controls.Add(this.button3);
            this.flowLayoutPanel1.Controls.Add(this.btnBackCamZ);
            this.flowLayoutPanel1.Controls.Add(this.button6);
            this.flowLayoutPanel1.Controls.Add(this.btnIndexX);
            this.flowLayoutPanel1.Controls.Add(this.btntttt);
            this.flowLayoutPanel1.Controls.Add(this.btnVBlockZ);
            this.flowLayoutPanel1.Controls.Add(this.button5);
            this.flowLayoutPanel1.Controls.Add(this.btnRollingFixA);
            this.flowLayoutPanel1.Controls.Add(this.button9);
            this.flowLayoutPanel1.Controls.Add(this.btnRollingFixB);
            this.flowLayoutPanel1.Controls.Add(this.button11);
            this.flowLayoutPanel1.Controls.Add(this.btnRollingMoveA);
            this.flowLayoutPanel1.Controls.Add(this.button13);
            this.flowLayoutPanel1.Controls.Add(this.btnRollingMoveB);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(624, 401);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(0, 0);
            this.button1.Margin = new System.Windows.Forms.Padding(0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(500, 40);
            this.button1.TabIndex = 0;
            this.button1.Text = "카메라 유닛 Z축 홈 복귀";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // btnCamZ
            // 
            this.btnCamZ.Location = new System.Drawing.Point(500, 0);
            this.btnCamZ.Margin = new System.Windows.Forms.Padding(0);
            this.btnCamZ.Name = "btnCamZ";
            this.btnCamZ.Size = new System.Drawing.Size(120, 40);
            this.btnCamZ.TabIndex = 1;
            this.btnCamZ.Text = "Stop";
            this.btnCamZ.UseVisualStyleBackColor = true;
            // 
            // timerHomeReturn
            // 
            this.timerHomeReturn.Enabled = true;
            this.timerHomeReturn.Interval = 500;
            this.timerHomeReturn.Tick += new System.EventHandler(this.timerHomeReturn_Tick);
            // 
            // button2
            // 
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Location = new System.Drawing.Point(0, 40);
            this.button2.Margin = new System.Windows.Forms.Padding(0);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(500, 40);
            this.button2.TabIndex = 2;
            this.button2.Text = "카메라 유닛 Y축 홈 복귀";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // btnCamY
            // 
            this.btnCamY.Location = new System.Drawing.Point(500, 40);
            this.btnCamY.Margin = new System.Windows.Forms.Padding(0);
            this.btnCamY.Name = "btnCamY";
            this.btnCamY.Size = new System.Drawing.Size(120, 40);
            this.btnCamY.TabIndex = 3;
            this.btnCamY.Text = "Stop";
            this.btnCamY.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.Location = new System.Drawing.Point(0, 80);
            this.button4.Margin = new System.Windows.Forms.Padding(0);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(500, 40);
            this.button4.TabIndex = 4;
            this.button4.Text = "카메라 유닛 X축 홈 복귀";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // btnCamX
            // 
            this.btnCamX.Location = new System.Drawing.Point(500, 80);
            this.btnCamX.Margin = new System.Windows.Forms.Padding(0);
            this.btnCamX.Name = "btnCamX";
            this.btnCamX.Size = new System.Drawing.Size(120, 40);
            this.btnCamX.TabIndex = 5;
            this.btnCamX.Text = "Stop";
            this.btnCamX.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Location = new System.Drawing.Point(0, 120);
            this.button3.Margin = new System.Windows.Forms.Padding(0);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(500, 40);
            this.button3.TabIndex = 6;
            this.button3.Text = "Back Camera Z축 홈 복귀";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // btnBackCamZ
            // 
            this.btnBackCamZ.Location = new System.Drawing.Point(500, 120);
            this.btnBackCamZ.Margin = new System.Windows.Forms.Padding(0);
            this.btnBackCamZ.Name = "btnBackCamZ";
            this.btnBackCamZ.Size = new System.Drawing.Size(120, 40);
            this.btnBackCamZ.TabIndex = 7;
            this.btnBackCamZ.Text = "Stop";
            this.btnBackCamZ.UseVisualStyleBackColor = true;
            // 
            // button6
            // 
            this.button6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button6.Location = new System.Drawing.Point(0, 160);
            this.button6.Margin = new System.Windows.Forms.Padding(0);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(500, 40);
            this.button6.TabIndex = 8;
            this.button6.Text = "Index 주행 X축 홈 복귀";
            this.button6.UseVisualStyleBackColor = true;
            // 
            // btnIndexX
            // 
            this.btnIndexX.Location = new System.Drawing.Point(500, 160);
            this.btnIndexX.Margin = new System.Windows.Forms.Padding(0);
            this.btnIndexX.Name = "btnIndexX";
            this.btnIndexX.Size = new System.Drawing.Size(120, 40);
            this.btnIndexX.TabIndex = 9;
            this.btnIndexX.Text = "Stop";
            this.btnIndexX.UseVisualStyleBackColor = true;
            // 
            // btntttt
            // 
            this.btntttt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btntttt.Location = new System.Drawing.Point(0, 200);
            this.btntttt.Margin = new System.Windows.Forms.Padding(0);
            this.btntttt.Name = "btntttt";
            this.btntttt.Size = new System.Drawing.Size(500, 40);
            this.btntttt.TabIndex = 10;
            this.btntttt.Text = "V 블럭 Z축 홈 복귀";
            this.btntttt.UseVisualStyleBackColor = true;
            // 
            // btnVBlockZ
            // 
            this.btnVBlockZ.Location = new System.Drawing.Point(500, 200);
            this.btnVBlockZ.Margin = new System.Windows.Forms.Padding(0);
            this.btnVBlockZ.Name = "btnVBlockZ";
            this.btnVBlockZ.Size = new System.Drawing.Size(120, 40);
            this.btnVBlockZ.TabIndex = 11;
            this.btnVBlockZ.Text = "Stop";
            this.btnVBlockZ.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button5.Location = new System.Drawing.Point(0, 240);
            this.button5.Margin = new System.Windows.Forms.Padding(0);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(500, 40);
            this.button5.TabIndex = 12;
            this.button5.Text = "고정축 롤링 A 홈 복귀";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // btnRollingFixA
            // 
            this.btnRollingFixA.Location = new System.Drawing.Point(500, 240);
            this.btnRollingFixA.Margin = new System.Windows.Forms.Padding(0);
            this.btnRollingFixA.Name = "btnRollingFixA";
            this.btnRollingFixA.Size = new System.Drawing.Size(120, 40);
            this.btnRollingFixA.TabIndex = 13;
            this.btnRollingFixA.Text = "Stop";
            this.btnRollingFixA.UseVisualStyleBackColor = true;
            // 
            // button9
            // 
            this.button9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button9.Location = new System.Drawing.Point(0, 280);
            this.button9.Margin = new System.Windows.Forms.Padding(0);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(500, 40);
            this.button9.TabIndex = 14;
            this.button9.Text = "고정축 롤링 B 홈 복귀";
            this.button9.UseVisualStyleBackColor = true;
            // 
            // btnRollingFixB
            // 
            this.btnRollingFixB.Location = new System.Drawing.Point(500, 280);
            this.btnRollingFixB.Margin = new System.Windows.Forms.Padding(0);
            this.btnRollingFixB.Name = "btnRollingFixB";
            this.btnRollingFixB.Size = new System.Drawing.Size(120, 40);
            this.btnRollingFixB.TabIndex = 15;
            this.btnRollingFixB.Text = "Stop";
            this.btnRollingFixB.UseVisualStyleBackColor = true;
            // 
            // button11
            // 
            this.button11.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button11.Location = new System.Drawing.Point(0, 320);
            this.button11.Margin = new System.Windows.Forms.Padding(0);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(500, 40);
            this.button11.TabIndex = 16;
            this.button11.Text = "이동축 롤링 A 홈 복귀";
            this.button11.UseVisualStyleBackColor = true;
            // 
            // btnRollingMoveA
            // 
            this.btnRollingMoveA.Location = new System.Drawing.Point(500, 320);
            this.btnRollingMoveA.Margin = new System.Windows.Forms.Padding(0);
            this.btnRollingMoveA.Name = "btnRollingMoveA";
            this.btnRollingMoveA.Size = new System.Drawing.Size(120, 40);
            this.btnRollingMoveA.TabIndex = 17;
            this.btnRollingMoveA.Text = "Stop";
            this.btnRollingMoveA.UseVisualStyleBackColor = true;
            // 
            // button13
            // 
            this.button13.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button13.Location = new System.Drawing.Point(0, 360);
            this.button13.Margin = new System.Windows.Forms.Padding(0);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(500, 40);
            this.button13.TabIndex = 18;
            this.button13.Text = "이동축 롤링 B 홈 복귀";
            this.button13.UseVisualStyleBackColor = true;
            // 
            // btnRollingMoveB
            // 
            this.btnRollingMoveB.Location = new System.Drawing.Point(500, 360);
            this.btnRollingMoveB.Margin = new System.Windows.Forms.Padding(0);
            this.btnRollingMoveB.Name = "btnRollingMoveB";
            this.btnRollingMoveB.Size = new System.Drawing.Size(120, 40);
            this.btnRollingMoveB.TabIndex = 19;
            this.btnRollingMoveB.Text = "Stop";
            this.btnRollingMoveB.UseVisualStyleBackColor = true;
            // 
            // frmReturnHome
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 401);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmReturnHome";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "원점 복귀";
            this.Load += new System.EventHandler(this.frmReturnHome_Load);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnCamZ;
        private System.Windows.Forms.Timer timerHomeReturn;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btnCamY;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button btnCamX;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button btnBackCamZ;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button btnIndexX;
        private System.Windows.Forms.Button btntttt;
        private System.Windows.Forms.Button btnVBlockZ;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button btnRollingFixA;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button btnRollingFixB;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.Button btnRollingMoveA;
        private System.Windows.Forms.Button button13;
        private System.Windows.Forms.Button btnRollingMoveB;

    }
}