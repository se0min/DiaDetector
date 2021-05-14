namespace AutoAssembler
{
    partial class frmFuncWelding
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
            this.txtWeldSolidValue = new System.Windows.Forms.TextBox();
            this.btn70JogPlus = new System.Windows.Forms.Button();
            this.btn70JogMinus = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.txtWeldSolidRate = new System.Windows.Forms.TextBox();
            this.btnRollingDown = new System.Windows.Forms.Button();
            this.btnUnswing = new System.Windows.Forms.Button();
            this.btnAuto = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnWelding = new System.Windows.Forms.Button();
            this.timerAxis = new System.Windows.Forms.Timer(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.txtMetalThick1 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtMetalThick2 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtCapsulePi = new System.Windows.Forms.TextBox();
            this.serialPort_PCToPC = new System.IO.Ports.SerialPort(this.components);
            this.btnMJog_Index = new System.Windows.Forms.Button();
            this.btnPJog_Index = new System.Windows.Forms.Button();
            this.btnFLMove = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.timerParsePacket = new System.Windows.Forms.Timer(this.components);
            this.radioBtnSlow = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.btnHome = new System.Windows.Forms.Button();
            this.radioBtnSSlow = new System.Windows.Forms.RadioButton();
            this.timerCommand = new System.Windows.Forms.Timer(this.components);
            this.btnVBlockDown = new System.Windows.Forms.Button();
            this.btn5StepMove = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(653, 389);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(120, 40);
            this.btnCancel.TabIndex = 162;
            this.btnCancel.Text = "취소";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(525, 389);
            this.btnOk.Margin = new System.Windows.Forms.Padding(4);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(120, 40);
            this.btnOk.TabIndex = 161;
            this.btnOk.Text = "저장";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // txtWeldSolidValue
            // 
            this.txtWeldSolidValue.Location = new System.Drawing.Point(186, 279);
            this.txtWeldSolidValue.Name = "txtWeldSolidValue";
            this.txtWeldSolidValue.Size = new System.Drawing.Size(75, 26);
            this.txtWeldSolidValue.TabIndex = 198;
            this.txtWeldSolidValue.Text = "0.0";
            this.txtWeldSolidValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btn70JogPlus
            // 
            this.btn70JogPlus.Location = new System.Drawing.Point(267, 250);
            this.btn70JogPlus.Name = "btn70JogPlus";
            this.btn70JogPlus.Size = new System.Drawing.Size(80, 80);
            this.btn70JogPlus.TabIndex = 195;
            this.btn70JogPlus.Text = ">";
            this.btn70JogPlus.UseVisualStyleBackColor = true;
            this.btn70JogPlus.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn70JogPlus_MouseDown);
            this.btn70JogPlus.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn70JogPlus_MouseUp);
            // 
            // btn70JogMinus
            // 
            this.btn70JogMinus.Location = new System.Drawing.Point(100, 250);
            this.btn70JogMinus.Name = "btn70JogMinus";
            this.btn70JogMinus.Size = new System.Drawing.Size(80, 80);
            this.btn70JogMinus.TabIndex = 196;
            this.btn70JogMinus.Text = "<";
            this.btn70JogMinus.UseVisualStyleBackColor = true;
            this.btn70JogMinus.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn70JogMinus_MouseDown);
            this.btn70JogMinus.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn70JogMinus_MouseUp);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("돋움", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label8.Location = new System.Drawing.Point(11, 380);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(164, 15);
            this.label8.TabIndex = 201;
            this.label8.Text = "Welding Solid Rate(%) :";
            // 
            // txtWeldSolidRate
            // 
            this.txtWeldSolidRate.Enabled = false;
            this.txtWeldSolidRate.Location = new System.Drawing.Point(181, 376);
            this.txtWeldSolidRate.Name = "txtWeldSolidRate";
            this.txtWeldSolidRate.Size = new System.Drawing.Size(80, 26);
            this.txtWeldSolidRate.TabIndex = 199;
            this.txtWeldSolidRate.Text = "0.0";
            this.txtWeldSolidRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnRollingDown
            // 
            this.btnRollingDown.Location = new System.Drawing.Point(526, 13);
            this.btnRollingDown.Margin = new System.Windows.Forms.Padding(4);
            this.btnRollingDown.Name = "btnRollingDown";
            this.btnRollingDown.Size = new System.Drawing.Size(247, 80);
            this.btnRollingDown.TabIndex = 208;
            this.btnRollingDown.Text = "롤러 다운";
            this.btnRollingDown.UseVisualStyleBackColor = true;
            this.btnRollingDown.Click += new System.EventHandler(this.btnRollingDown_Click);
            // 
            // btnUnswing
            // 
            this.btnUnswing.Location = new System.Drawing.Point(13, 13);
            this.btnUnswing.Margin = new System.Windows.Forms.Padding(4);
            this.btnUnswing.Name = "btnUnswing";
            this.btnUnswing.Size = new System.Drawing.Size(248, 80);
            this.btnUnswing.TabIndex = 209;
            this.btnUnswing.Text = "Swing Up";
            this.btnUnswing.UseVisualStyleBackColor = true;
            this.btnUnswing.Click += new System.EventHandler(this.btnUnswing_Click);
            // 
            // btnAuto
            // 
            this.btnAuto.Location = new System.Drawing.Point(525, 341);
            this.btnAuto.Margin = new System.Windows.Forms.Padding(4);
            this.btnAuto.Name = "btnAuto";
            this.btnAuto.Size = new System.Drawing.Size(120, 40);
            this.btnAuto.TabIndex = 210;
            this.btnAuto.Text = "AUTO";
            this.btnAuto.UseVisualStyleBackColor = true;
            this.btnAuto.Click += new System.EventHandler(this.btnAuto_Click);
            // 
            // btnStop
            // 
            this.btnStop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btnStop.Location = new System.Drawing.Point(653, 341);
            this.btnStop.Margin = new System.Windows.Forms.Padding(4);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(120, 40);
            this.btnStop.TabIndex = 212;
            this.btnStop.Text = "일시 정지";
            this.btnStop.UseVisualStyleBackColor = false;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnWelding
            // 
            this.btnWelding.Location = new System.Drawing.Point(398, 13);
            this.btnWelding.Margin = new System.Windows.Forms.Padding(4);
            this.btnWelding.Name = "btnWelding";
            this.btnWelding.Size = new System.Drawing.Size(120, 80);
            this.btnWelding.TabIndex = 164;
            this.btnWelding.Text = "초기 좌표 전송";
            this.btnWelding.UseVisualStyleBackColor = true;
            this.btnWelding.Click += new System.EventHandler(this.btnWelding_Click);
            // 
            // timerAxis
            // 
            this.timerAxis.Interval = 500;
            this.timerAxis.Tick += new System.EventHandler(this.timerAxis_Tick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Enabled = false;
            this.label2.Location = new System.Drawing.Point(75, 124);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 16);
            this.label2.TabIndex = 215;
            this.label2.Text = "메탈 두께 1 :";
            // 
            // txtMetalThick1
            // 
            this.txtMetalThick1.Enabled = false;
            this.txtMetalThick1.Location = new System.Drawing.Point(181, 121);
            this.txtMetalThick1.Name = "txtMetalThick1";
            this.txtMetalThick1.Size = new System.Drawing.Size(80, 26);
            this.txtMetalThick1.TabIndex = 214;
            this.txtMetalThick1.Text = "0.0";
            this.txtMetalThick1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Enabled = false;
            this.label4.Location = new System.Drawing.Point(75, 156);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 16);
            this.label4.TabIndex = 217;
            this.label4.Text = "메탈 두께 2 :";
            // 
            // txtMetalThick2
            // 
            this.txtMetalThick2.Enabled = false;
            this.txtMetalThick2.Location = new System.Drawing.Point(181, 153);
            this.txtMetalThick2.Name = "txtMetalThick2";
            this.txtMetalThick2.Size = new System.Drawing.Size(80, 26);
            this.txtMetalThick2.TabIndex = 216;
            this.txtMetalThick2.Text = "0.0";
            this.txtMetalThick2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Enabled = false;
            this.label5.Location = new System.Drawing.Point(107, 188);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 16);
            this.label5.TabIndex = 219;
            this.label5.Text = "캡슐 Pi :";
            // 
            // txtCapsulePi
            // 
            this.txtCapsulePi.Enabled = false;
            this.txtCapsulePi.Location = new System.Drawing.Point(181, 185);
            this.txtCapsulePi.Name = "txtCapsulePi";
            this.txtCapsulePi.Size = new System.Drawing.Size(80, 26);
            this.txtCapsulePi.TabIndex = 218;
            this.txtCapsulePi.Text = "0.0";
            this.txtCapsulePi.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // serialPort_PCToPC
            // 
            this.serialPort_PCToPC.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort_PCToPC_DataReceived);
            // 
            // btnMJog_Index
            // 
            this.btnMJog_Index.Location = new System.Drawing.Point(270, 121);
            this.btnMJog_Index.Name = "btnMJog_Index";
            this.btnMJog_Index.Size = new System.Drawing.Size(120, 120);
            this.btnMJog_Index.TabIndex = 220;
            this.btnMJog_Index.Text = "<";
            this.btnMJog_Index.UseVisualStyleBackColor = true;
            this.btnMJog_Index.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnMJog_Index_MouseDown);
            this.btnMJog_Index.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnMJog_Index_MouseUp);
            // 
            // btnPJog_Index
            // 
            this.btnPJog_Index.Location = new System.Drawing.Point(398, 121);
            this.btnPJog_Index.Name = "btnPJog_Index";
            this.btnPJog_Index.Size = new System.Drawing.Size(120, 120);
            this.btnPJog_Index.TabIndex = 221;
            this.btnPJog_Index.Text = ">";
            this.btnPJog_Index.UseVisualStyleBackColor = true;
            this.btnPJog_Index.Click += new System.EventHandler(this.btnPJog_Index_Click);
            this.btnPJog_Index.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnPJog_Index_MouseDown);
            this.btnPJog_Index.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnPJog_Index_MouseUp);
            // 
            // btnFLMove
            // 
            this.btnFLMove.Location = new System.Drawing.Point(525, 121);
            this.btnFLMove.Margin = new System.Windows.Forms.Padding(4);
            this.btnFLMove.Name = "btnFLMove";
            this.btnFLMove.Size = new System.Drawing.Size(248, 55);
            this.btnFLMove.TabIndex = 222;
            this.btnFLMove.Text = "F-L 이동";
            this.btnFLMove.UseVisualStyleBackColor = true;
            this.btnFLMove.Click += new System.EventHandler(this.btnFLMove_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("돋움", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label6.Location = new System.Drawing.Point(332, 103);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(120, 15);
            this.label6.TabIndex = 224;
            this.label6.Text = "INDEX 정/역회전";
            // 
            // timerParsePacket
            // 
            this.timerParsePacket.Interval = 500;
            this.timerParsePacket.Tick += new System.EventHandler(this.timerParsePacket_Tick);
            // 
            // radioBtnSlow
            // 
            this.radioBtnSlow.AutoSize = true;
            this.radioBtnSlow.Location = new System.Drawing.Point(193, 399);
            this.radioBtnSlow.Name = "radioBtnSlow";
            this.radioBtnSlow.Size = new System.Drawing.Size(61, 20);
            this.radioBtnSlow.TabIndex = 226;
            this.radioBtnSlow.TabStop = true;
            this.radioBtnSlow.Tag = "1";
            this.radioBtnSlow.Text = "Slow";
            this.radioBtnSlow.UseVisualStyleBackColor = true;
            this.radioBtnSlow.CheckedChanged += new System.EventHandler(this.radioBtn_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(12, 401);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 16);
            this.label1.TabIndex = 225;
            this.label1.Text = "속도 설정 :";
            // 
            // btnHome
            // 
            this.btnHome.Location = new System.Drawing.Point(524, 250);
            this.btnHome.Margin = new System.Windows.Forms.Padding(4);
            this.btnHome.Name = "btnHome";
            this.btnHome.Size = new System.Drawing.Size(249, 55);
            this.btnHome.TabIndex = 229;
            this.btnHome.Text = "인덱스 홈 복귀";
            this.btnHome.UseVisualStyleBackColor = true;
            this.btnHome.Click += new System.EventHandler(this.btnHome_Click);
            // 
            // radioBtnSSlow
            // 
            this.radioBtnSSlow.AutoSize = true;
            this.radioBtnSSlow.Location = new System.Drawing.Point(107, 399);
            this.radioBtnSSlow.Name = "radioBtnSSlow";
            this.radioBtnSSlow.Size = new System.Drawing.Size(79, 20);
            this.radioBtnSSlow.TabIndex = 230;
            this.radioBtnSSlow.TabStop = true;
            this.radioBtnSSlow.Tag = "0";
            this.radioBtnSSlow.Text = "S-Slow";
            this.radioBtnSSlow.UseVisualStyleBackColor = true;
            this.radioBtnSSlow.CheckedChanged += new System.EventHandler(this.radioBtn_CheckedChanged);
            // 
            // timerCommand
            // 
            this.timerCommand.Interval = 300;
            this.timerCommand.Tick += new System.EventHandler(this.timerCommand_Tick);
            // 
            // btnVBlockDown
            // 
            this.btnVBlockDown.Location = new System.Drawing.Point(270, 13);
            this.btnVBlockDown.Margin = new System.Windows.Forms.Padding(4);
            this.btnVBlockDown.Name = "btnVBlockDown";
            this.btnVBlockDown.Size = new System.Drawing.Size(120, 80);
            this.btnVBlockDown.TabIndex = 236;
            this.btnVBlockDown.Text = "V-Block Down";
            this.btnVBlockDown.UseVisualStyleBackColor = true;
            this.btnVBlockDown.Click += new System.EventHandler(this.btnVBlockDown_Click);
            // 
            // btn5StepMove
            // 
            this.btn5StepMove.Location = new System.Drawing.Point(13, 250);
            this.btn5StepMove.Margin = new System.Windows.Forms.Padding(4);
            this.btn5StepMove.Name = "btn5StepMove";
            this.btn5StepMove.Size = new System.Drawing.Size(80, 80);
            this.btn5StepMove.TabIndex = 237;
            this.btn5StepMove.Text = "10mm Step 이동";
            this.btn5StepMove.UseVisualStyleBackColor = true;
            this.btn5StepMove.Click += new System.EventHandler(this.btn5StepMove_Click);
            // 
            // frmFuncWelding
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 442);
            this.Controls.Add(this.btn5StepMove);
            this.Controls.Add(this.btnVBlockDown);
            this.Controls.Add(this.radioBtnSSlow);
            this.Controls.Add(this.btnHome);
            this.Controls.Add(this.radioBtnSlow);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnUnswing);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnFLMove);
            this.Controls.Add(this.btnPJog_Index);
            this.Controls.Add(this.btnMJog_Index);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtCapsulePi);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtMetalThick2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtMetalThick1);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnAuto);
            this.Controls.Add(this.btnRollingDown);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtWeldSolidRate);
            this.Controls.Add(this.txtWeldSolidValue);
            this.Controls.Add(this.btn70JogPlus);
            this.Controls.Add(this.btn70JogMinus);
            this.Controls.Add(this.btnWelding);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmFuncWelding";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "용접 위치 이동 기능";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmFuncWelding_FormClosing);
            this.Load += new System.EventHandler(this.frmFuncWelding_Load);
            this.Shown += new System.EventHandler(this.frmFuncWelding_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.TextBox txtWeldSolidValue;
        private System.Windows.Forms.Button btn70JogPlus;
        private System.Windows.Forms.Button btn70JogMinus;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtWeldSolidRate;
        private System.Windows.Forms.Button btnRollingDown;
        private System.Windows.Forms.Button btnUnswing;
        private System.Windows.Forms.Button btnAuto;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnWelding;
        private System.Windows.Forms.Timer timerAxis;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtMetalThick1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtMetalThick2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtCapsulePi;
        public System.IO.Ports.SerialPort serialPort_PCToPC;
        private System.Windows.Forms.Button btnMJog_Index;
        private System.Windows.Forms.Button btnPJog_Index;
        private System.Windows.Forms.Button btnFLMove;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Timer timerParsePacket;
        private System.Windows.Forms.RadioButton radioBtnSlow;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnHome;
        private System.Windows.Forms.RadioButton radioBtnSSlow;
        private System.Windows.Forms.Timer timerCommand;
        private System.Windows.Forms.Button btnVBlockDown;
        private System.Windows.Forms.Button btn5StepMove;

    }
}