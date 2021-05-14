namespace AutoAssembler
{
    partial class frmFuncRollingUI
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
            this.timerAxis = new System.Windows.Forms.Timer(this.components);
            this.btnJogPlus = new System.Windows.Forms.Button();
            this.btnJogMinus = new System.Windows.Forms.Button();
            this.txtRolling70Rate = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.txtMetalThick1 = new System.Windows.Forms.TextBox();
            this.txtMetalThick2 = new System.Windows.Forms.TextBox();
            this.txtFLValue = new System.Windows.Forms.TextBox();
            this.txtSLValue = new System.Windows.Forms.TextBox();
            this.txtWRValue = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtRolling80Rate = new System.Windows.Forms.TextBox();
            this.btnFLMove = new System.Windows.Forms.Button();
            this.btnRollingUp = new System.Windows.Forms.Button();
            this.btnSwing = new System.Windows.Forms.Button();
            this.btnRolling80 = new System.Windows.Forms.Button();
            this.btnRolling70 = new System.Windows.Forms.Button();
            this.btnRollingDown = new System.Windows.Forms.Button();
            this.btnUnswing = new System.Windows.Forms.Button();
            this.txtCapsule = new System.Windows.Forms.TextBox();
            this.btnStop = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.txtRollingRate = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnIndexRotate = new System.Windows.Forms.Button();
            this.txtRotateCount = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnAuto = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtRollingOffset = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtLestLength = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btn70Save = new System.Windows.Forms.Button();
            this.btn80Save = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.radioBtnFast = new System.Windows.Forms.RadioButton();
            this.radioBtnMidium = new System.Windows.Forms.RadioButton();
            this.radioBtnSlow = new System.Windows.Forms.RadioButton();
            this.label8 = new System.Windows.Forms.Label();
            this.radioBtnSSlow = new System.Windows.Forms.RadioButton();
            this.btnVBlockDown = new System.Windows.Forms.Button();
            this.btnVBlockUp = new System.Windows.Forms.Button();
            this.txtVBlockFL_Limit_Value = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txtVBlockFL_Offset_Value = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // timerAxis
            // 
            this.timerAxis.Interval = 500;
            this.timerAxis.Tick += new System.EventHandler(this.timerAxis_Tick);
            // 
            // btnJogPlus
            // 
            this.btnJogPlus.Location = new System.Drawing.Point(616, 104);
            this.btnJogPlus.Name = "btnJogPlus";
            this.btnJogPlus.Size = new System.Drawing.Size(74, 74);
            this.btnJogPlus.TabIndex = 157;
            this.btnJogPlus.Text = ">";
            this.btnJogPlus.UseVisualStyleBackColor = true;
            this.btnJogPlus.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnJogPlus_MouseDown);
            this.btnJogPlus.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnJogPlus_MouseUp);
            // 
            // btnJogMinus
            // 
            this.btnJogMinus.Location = new System.Drawing.Point(397, 104);
            this.btnJogMinus.Name = "btnJogMinus";
            this.btnJogMinus.Size = new System.Drawing.Size(74, 74);
            this.btnJogMinus.TabIndex = 158;
            this.btnJogMinus.Text = "<";
            this.btnJogMinus.UseVisualStyleBackColor = true;
            this.btnJogMinus.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnJogMinus_MouseDown);
            this.btnJogMinus.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnJogMinus_MouseUp);
            // 
            // txtRolling70Rate
            // 
            this.txtRolling70Rate.Location = new System.Drawing.Point(524, 8);
            this.txtRolling70Rate.Name = "txtRolling70Rate";
            this.txtRolling70Rate.Size = new System.Drawing.Size(59, 26);
            this.txtRolling70Rate.TabIndex = 156;
            this.txtRolling70Rate.Text = "70.0";
            this.txtRolling70Rate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtRolling70Rate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtRolling70Rate_KeyDown);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(841, 859);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(120, 40);
            this.btnCancel.TabIndex = 154;
            this.btnCancel.Text = "취소";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(715, 859);
            this.btnOk.Margin = new System.Windows.Forms.Padding(4);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(120, 40);
            this.btnOk.TabIndex = 153;
            this.btnOk.Text = "저장";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // txtMetalThick1
            // 
            this.txtMetalThick1.BackColor = System.Drawing.Color.LightGray;
            this.txtMetalThick1.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtMetalThick1.Location = new System.Drawing.Point(714, 475);
            this.txtMetalThick1.Name = "txtMetalThick1";
            this.txtMetalThick1.ShortcutsEnabled = false;
            this.txtMetalThick1.Size = new System.Drawing.Size(120, 26);
            this.txtMetalThick1.TabIndex = 160;
            this.txtMetalThick1.Text = "0.0";
            this.txtMetalThick1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtMetalThick2
            // 
            this.txtMetalThick2.BackColor = System.Drawing.Color.LightGray;
            this.txtMetalThick2.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtMetalThick2.Location = new System.Drawing.Point(47, 447);
            this.txtMetalThick2.Name = "txtMetalThick2";
            this.txtMetalThick2.ShortcutsEnabled = false;
            this.txtMetalThick2.Size = new System.Drawing.Size(120, 26);
            this.txtMetalThick2.TabIndex = 161;
            this.txtMetalThick2.Text = "0.0";
            this.txtMetalThick2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtFLValue
            // 
            this.txtFLValue.BackColor = System.Drawing.Color.LightGray;
            this.txtFLValue.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtFLValue.Location = new System.Drawing.Point(459, 318);
            this.txtFLValue.Name = "txtFLValue";
            this.txtFLValue.ShortcutsEnabled = false;
            this.txtFLValue.Size = new System.Drawing.Size(120, 26);
            this.txtFLValue.TabIndex = 167;
            this.txtFLValue.Text = "0.0";
            this.txtFLValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtSLValue
            // 
            this.txtSLValue.BackColor = System.Drawing.Color.LightGray;
            this.txtSLValue.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtSLValue.Location = new System.Drawing.Point(548, 454);
            this.txtSLValue.Name = "txtSLValue";
            this.txtSLValue.ShortcutsEnabled = false;
            this.txtSLValue.Size = new System.Drawing.Size(120, 26);
            this.txtSLValue.TabIndex = 168;
            this.txtSLValue.Text = "0.0";
            this.txtSLValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtWRValue
            // 
            this.txtWRValue.BackColor = System.Drawing.Color.LightGray;
            this.txtWRValue.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtWRValue.Location = new System.Drawing.Point(714, 575);
            this.txtWRValue.Name = "txtWRValue";
            this.txtWRValue.ShortcutsEnabled = false;
            this.txtWRValue.Size = new System.Drawing.Size(120, 26);
            this.txtWRValue.TabIndex = 170;
            this.txtWRValue.Text = "0.0";
            this.txtWRValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(588, 11);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(21, 16);
            this.label7.TabIndex = 173;
            this.label7.Text = "%";
            // 
            // txtRolling80Rate
            // 
            this.txtRolling80Rate.Location = new System.Drawing.Point(743, 8);
            this.txtRolling80Rate.Name = "txtRolling80Rate";
            this.txtRolling80Rate.Size = new System.Drawing.Size(59, 26);
            this.txtRolling80Rate.TabIndex = 175;
            this.txtRolling80Rate.Text = "80.0";
            this.txtRolling80Rate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtRolling80Rate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtRolling80Rate_KeyDown);
            // 
            // btnFLMove
            // 
            this.btnFLMove.Location = new System.Drawing.Point(13, 8);
            this.btnFLMove.Margin = new System.Windows.Forms.Padding(4);
            this.btnFLMove.Name = "btnFLMove";
            this.btnFLMove.Size = new System.Drawing.Size(120, 88);
            this.btnFLMove.TabIndex = 179;
            this.btnFLMove.Text = "F-L 이동";
            this.btnFLMove.UseVisualStyleBackColor = true;
            this.btnFLMove.Click += new System.EventHandler(this.btnFLMove_Click);
            // 
            // btnRollingUp
            // 
            this.btnRollingUp.Location = new System.Drawing.Point(141, 8);
            this.btnRollingUp.Margin = new System.Windows.Forms.Padding(4);
            this.btnRollingUp.Name = "btnRollingUp";
            this.btnRollingUp.Size = new System.Drawing.Size(120, 88);
            this.btnRollingUp.TabIndex = 180;
            this.btnRollingUp.Text = "Rolling Up";
            this.btnRollingUp.UseVisualStyleBackColor = true;
            this.btnRollingUp.Click += new System.EventHandler(this.btnRollingUp_Click);
            // 
            // btnSwing
            // 
            this.btnSwing.Location = new System.Drawing.Point(269, 8);
            this.btnSwing.Margin = new System.Windows.Forms.Padding(4);
            this.btnSwing.Name = "btnSwing";
            this.btnSwing.Size = new System.Drawing.Size(120, 88);
            this.btnSwing.TabIndex = 181;
            this.btnSwing.Text = "Swing Down";
            this.btnSwing.UseVisualStyleBackColor = true;
            this.btnSwing.Click += new System.EventHandler(this.btnSwing_Click);
            // 
            // btnRolling80
            // 
            this.btnRolling80.Location = new System.Drawing.Point(616, 8);
            this.btnRolling80.Margin = new System.Windows.Forms.Padding(4);
            this.btnRolling80.Name = "btnRolling80";
            this.btnRolling80.Size = new System.Drawing.Size(120, 89);
            this.btnRolling80.TabIndex = 184;
            this.btnRolling80.Text = "Rolling Finish :";
            this.btnRolling80.UseVisualStyleBackColor = true;
            this.btnRolling80.Click += new System.EventHandler(this.btnRolling80_Click);
            // 
            // btnRolling70
            // 
            this.btnRolling70.Location = new System.Drawing.Point(397, 8);
            this.btnRolling70.Margin = new System.Windows.Forms.Padding(4);
            this.btnRolling70.Name = "btnRolling70";
            this.btnRolling70.Size = new System.Drawing.Size(120, 88);
            this.btnRolling70.TabIndex = 183;
            this.btnRolling70.Text = "Rolling Start :";
            this.btnRolling70.UseVisualStyleBackColor = true;
            this.btnRolling70.Click += new System.EventHandler(this.btnRolling70_Click);
            // 
            // btnRollingDown
            // 
            this.btnRollingDown.Location = new System.Drawing.Point(141, 104);
            this.btnRollingDown.Margin = new System.Windows.Forms.Padding(4);
            this.btnRollingDown.Name = "btnRollingDown";
            this.btnRollingDown.Size = new System.Drawing.Size(120, 40);
            this.btnRollingDown.TabIndex = 187;
            this.btnRollingDown.Text = "Rolling Down";
            this.btnRollingDown.UseVisualStyleBackColor = true;
            this.btnRollingDown.Click += new System.EventHandler(this.btnRollingDown_Click);
            // 
            // btnUnswing
            // 
            this.btnUnswing.Location = new System.Drawing.Point(269, 104);
            this.btnUnswing.Margin = new System.Windows.Forms.Padding(4);
            this.btnUnswing.Name = "btnUnswing";
            this.btnUnswing.Size = new System.Drawing.Size(120, 40);
            this.btnUnswing.TabIndex = 188;
            this.btnUnswing.Text = "Swing Up";
            this.btnUnswing.UseVisualStyleBackColor = true;
            this.btnUnswing.Click += new System.EventHandler(this.btnUnswing_Click);
            // 
            // txtCapsule
            // 
            this.txtCapsule.BackColor = System.Drawing.Color.LightGray;
            this.txtCapsule.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtCapsule.Location = new System.Drawing.Point(280, 669);
            this.txtCapsule.Name = "txtCapsule";
            this.txtCapsule.ShortcutsEnabled = false;
            this.txtCapsule.Size = new System.Drawing.Size(120, 26);
            this.txtCapsule.TabIndex = 190;
            this.txtCapsule.Text = "0.0";
            this.txtCapsule.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnStop
            // 
            this.btnStop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btnStop.Location = new System.Drawing.Point(708, 193);
            this.btnStop.Margin = new System.Windows.Forms.Padding(4);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(120, 60);
            this.btnStop.TabIndex = 192;
            this.btnStop.Text = "일시 정지";
            this.btnStop.UseVisualStyleBackColor = false;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::AutoAssembler.Properties.Resources.롤링_마스터;
            this.pictureBox1.Location = new System.Drawing.Point(0, 310);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(974, 600);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 189;
            this.pictureBox1.TabStop = false;
            // 
            // txtRollingRate
            // 
            this.txtRollingRate.Enabled = false;
            this.txtRollingRate.Location = new System.Drawing.Point(477, 130);
            this.txtRollingRate.Name = "txtRollingRate";
            this.txtRollingRate.Size = new System.Drawing.Size(133, 26);
            this.txtRollingRate.TabIndex = 194;
            this.txtRollingRate.Text = "0.0";
            this.txtRollingRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(807, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(21, 16);
            this.label1.TabIndex = 197;
            this.label1.Text = "%";
            // 
            // btnIndexRotate
            // 
            this.btnIndexRotate.Location = new System.Drawing.Point(835, 8);
            this.btnIndexRotate.Margin = new System.Windows.Forms.Padding(4);
            this.btnIndexRotate.Name = "btnIndexRotate";
            this.btnIndexRotate.Size = new System.Drawing.Size(120, 88);
            this.btnIndexRotate.TabIndex = 198;
            this.btnIndexRotate.Text = "INDEX 회전";
            this.btnIndexRotate.UseVisualStyleBackColor = true;
            this.btnIndexRotate.Click += new System.EventHandler(this.btnIndexRotate_Click);
            // 
            // txtRotateCount
            // 
            this.txtRotateCount.Location = new System.Drawing.Point(902, 113);
            this.txtRotateCount.Name = "txtRotateCount";
            this.txtRotateCount.Size = new System.Drawing.Size(53, 26);
            this.txtRotateCount.TabIndex = 199;
            this.txtRotateCount.Text = "1";
            this.txtRotateCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(835, 116);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 16);
            this.label2.TabIndex = 200;
            this.label2.Text = "회전수 :";
            // 
            // btnAuto
            // 
            this.btnAuto.Location = new System.Drawing.Point(835, 193);
            this.btnAuto.Margin = new System.Windows.Forms.Padding(4);
            this.btnAuto.Name = "btnAuto";
            this.btnAuto.Size = new System.Drawing.Size(120, 60);
            this.btnAuto.TabIndex = 202;
            this.btnAuto.Text = "AUTO";
            this.btnAuto.UseVisualStyleBackColor = true;
            this.btnAuto.Click += new System.EventHandler(this.btnAuto_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(54, 154);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 16);
            this.label3.TabIndex = 205;
            this.label3.Text = "* Roll 옵셋 :";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // txtRollingOffset
            // 
            this.txtRollingOffset.Location = new System.Drawing.Point(141, 151);
            this.txtRollingOffset.Name = "txtRollingOffset";
            this.txtRollingOffset.Size = new System.Drawing.Size(68, 26);
            this.txtRollingOffset.TabIndex = 204;
            this.txtRollingOffset.Text = "-1.0";
            this.txtRollingOffset.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(835, 448);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(84, 16);
            this.label4.TabIndex = 206;
            this.label4.Text = "고정축(A)";
            // 
            // txtLestLength
            // 
            this.txtLestLength.Location = new System.Drawing.Point(477, 184);
            this.txtLestLength.Name = "txtLestLength";
            this.txtLestLength.Size = new System.Drawing.Size(133, 26);
            this.txtLestLength.TabIndex = 207;
            this.txtLestLength.Text = "0.0";
            this.txtLestLength.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(384, 187);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(87, 16);
            this.label5.TabIndex = 208;
            this.label5.Text = "남은 간격 :";
            // 
            // btn70Save
            // 
            this.btn70Save.Location = new System.Drawing.Point(524, 40);
            this.btn70Save.Name = "btn70Save";
            this.btn70Save.Size = new System.Drawing.Size(85, 56);
            this.btn70Save.TabIndex = 209;
            this.btn70Save.Text = "위치 저장";
            this.btn70Save.UseVisualStyleBackColor = true;
            this.btn70Save.Click += new System.EventHandler(this.btn70Save_Click);
            // 
            // btn80Save
            // 
            this.btn80Save.Location = new System.Drawing.Point(743, 40);
            this.btn80Save.Name = "btn80Save";
            this.btn80Save.Size = new System.Drawing.Size(85, 56);
            this.btn80Save.TabIndex = 210;
            this.btn80Save.Text = "위치 저장";
            this.btn80Save.UseVisualStyleBackColor = true;
            this.btn80Save.Click += new System.EventHandler(this.btn80Save_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(616, 187);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(34, 16);
            this.label6.TabIndex = 211;
            this.label6.Text = "mm";
            // 
            // radioBtnFast
            // 
            this.radioBtnFast.AutoSize = true;
            this.radioBtnFast.Location = new System.Drawing.Point(897, 285);
            this.radioBtnFast.Name = "radioBtnFast";
            this.radioBtnFast.Size = new System.Drawing.Size(58, 20);
            this.radioBtnFast.TabIndex = 232;
            this.radioBtnFast.TabStop = true;
            this.radioBtnFast.Tag = "3";
            this.radioBtnFast.Text = "Fast";
            this.radioBtnFast.UseVisualStyleBackColor = true;
            this.radioBtnFast.CheckedChanged += new System.EventHandler(this.radioBtn_CheckedChanged);
            // 
            // radioBtnMidium
            // 
            this.radioBtnMidium.AutoSize = true;
            this.radioBtnMidium.Location = new System.Drawing.Point(815, 285);
            this.radioBtnMidium.Name = "radioBtnMidium";
            this.radioBtnMidium.Size = new System.Drawing.Size(76, 20);
            this.radioBtnMidium.TabIndex = 231;
            this.radioBtnMidium.TabStop = true;
            this.radioBtnMidium.Tag = "2";
            this.radioBtnMidium.Text = "Midium";
            this.radioBtnMidium.UseVisualStyleBackColor = true;
            this.radioBtnMidium.CheckedChanged += new System.EventHandler(this.radioBtn_CheckedChanged);
            // 
            // radioBtnSlow
            // 
            this.radioBtnSlow.AutoSize = true;
            this.radioBtnSlow.Location = new System.Drawing.Point(748, 285);
            this.radioBtnSlow.Name = "radioBtnSlow";
            this.radioBtnSlow.Size = new System.Drawing.Size(61, 20);
            this.radioBtnSlow.TabIndex = 230;
            this.radioBtnSlow.TabStop = true;
            this.radioBtnSlow.Tag = "1";
            this.radioBtnSlow.Text = "Slow";
            this.radioBtnSlow.UseVisualStyleBackColor = true;
            this.radioBtnSlow.CheckedChanged += new System.EventHandler(this.radioBtn_CheckedChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label8.Location = new System.Drawing.Point(570, 287);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(87, 16);
            this.label8.TabIndex = 229;
            this.label8.Text = "속도 설정 :";
            // 
            // radioBtnSSlow
            // 
            this.radioBtnSSlow.AutoSize = true;
            this.radioBtnSSlow.Location = new System.Drawing.Point(663, 285);
            this.radioBtnSSlow.Name = "radioBtnSSlow";
            this.radioBtnSSlow.Size = new System.Drawing.Size(79, 20);
            this.radioBtnSSlow.TabIndex = 233;
            this.radioBtnSSlow.TabStop = true;
            this.radioBtnSSlow.Tag = "0";
            this.radioBtnSSlow.Text = "S-Slow";
            this.radioBtnSSlow.UseVisualStyleBackColor = true;
            this.radioBtnSSlow.CheckedChanged += new System.EventHandler(this.radioBtn_CheckedChanged);
            // 
            // btnVBlockDown
            // 
            this.btnVBlockDown.Location = new System.Drawing.Point(141, 243);
            this.btnVBlockDown.Margin = new System.Windows.Forms.Padding(4);
            this.btnVBlockDown.Name = "btnVBlockDown";
            this.btnVBlockDown.Size = new System.Drawing.Size(120, 60);
            this.btnVBlockDown.TabIndex = 235;
            this.btnVBlockDown.Text = "V-Block Down";
            this.btnVBlockDown.UseVisualStyleBackColor = true;
            this.btnVBlockDown.Click += new System.EventHandler(this.btnVBlockDown_Click);
            // 
            // btnVBlockUp
            // 
            this.btnVBlockUp.Location = new System.Drawing.Point(13, 242);
            this.btnVBlockUp.Margin = new System.Windows.Forms.Padding(4);
            this.btnVBlockUp.Name = "btnVBlockUp";
            this.btnVBlockUp.Size = new System.Drawing.Size(120, 60);
            this.btnVBlockUp.TabIndex = 234;
            this.btnVBlockUp.Text = "V-Block Up";
            this.btnVBlockUp.UseVisualStyleBackColor = true;
            this.btnVBlockUp.Click += new System.EventHandler(this.btnVBlockUp_Click);
            // 
            // txtVBlockFL_Limit_Value
            // 
            this.txtVBlockFL_Limit_Value.Location = new System.Drawing.Point(409, 279);
            this.txtVBlockFL_Limit_Value.Name = "txtVBlockFL_Limit_Value";
            this.txtVBlockFL_Limit_Value.Size = new System.Drawing.Size(68, 26);
            this.txtVBlockFL_Limit_Value.TabIndex = 236;
            this.txtVBlockFL_Limit_Value.Text = "200.0";
            this.txtVBlockFL_Limit_Value.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(262, 283);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(148, 16);
            this.label9.TabIndex = 237;
            this.label9.Text = "* V-Block FL 제한 :";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(285, 250);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(125, 16);
            this.label10.TabIndex = 239;
            this.label10.Text = "* V-Block 옵셋 :";
            // 
            // txtVBlockFL_Offset_Value
            // 
            this.txtVBlockFL_Offset_Value.Location = new System.Drawing.Point(409, 247);
            this.txtVBlockFL_Offset_Value.Name = "txtVBlockFL_Offset_Value";
            this.txtVBlockFL_Offset_Value.Size = new System.Drawing.Size(68, 26);
            this.txtVBlockFL_Offset_Value.TabIndex = 238;
            this.txtVBlockFL_Offset_Value.Text = "-1.0";
            this.txtVBlockFL_Offset_Value.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // frmFuncRollingUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(974, 912);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txtVBlockFL_Offset_Value);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtVBlockFL_Limit_Value);
            this.Controls.Add(this.btnVBlockDown);
            this.Controls.Add(this.btnVBlockUp);
            this.Controls.Add(this.radioBtnSSlow);
            this.Controls.Add(this.radioBtnFast);
            this.Controls.Add(this.radioBtnMidium);
            this.Controls.Add(this.radioBtnSlow);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btn80Save);
            this.Controls.Add(this.btn70Save);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtLestLength);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtRollingOffset);
            this.Controls.Add(this.btnAuto);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtRotateCount);
            this.Controls.Add(this.btnIndexRotate);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtRollingRate);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.txtCapsule);
            this.Controls.Add(this.btnUnswing);
            this.Controls.Add(this.btnRollingDown);
            this.Controls.Add(this.btnRolling80);
            this.Controls.Add(this.btnRolling70);
            this.Controls.Add(this.btnSwing);
            this.Controls.Add(this.btnRollingUp);
            this.Controls.Add(this.btnFLMove);
            this.Controls.Add(this.txtRolling80Rate);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtWRValue);
            this.Controls.Add(this.txtSLValue);
            this.Controls.Add(this.txtFLValue);
            this.Controls.Add(this.txtMetalThick2);
            this.Controls.Add(this.txtMetalThick1);
            this.Controls.Add(this.btnJogPlus);
            this.Controls.Add(this.btnJogMinus);
            this.Controls.Add(this.txtRolling70Rate);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.pictureBox1);
            this.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmFuncRollingUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Rolling 편집 UI";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmFuncRollingUI_FormClosing);
            this.Load += new System.EventHandler(this.frmFuncRollingUI_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timerAxis;
        private System.Windows.Forms.Button btnJogPlus;
        private System.Windows.Forms.Button btnJogMinus;
        private System.Windows.Forms.TextBox txtRolling70Rate;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.TextBox txtMetalThick1;
        private System.Windows.Forms.TextBox txtMetalThick2;
        private System.Windows.Forms.TextBox txtFLValue;
        private System.Windows.Forms.TextBox txtSLValue;
        private System.Windows.Forms.TextBox txtWRValue;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtRolling80Rate;
        private System.Windows.Forms.Button btnFLMove;
        private System.Windows.Forms.Button btnRollingUp;
        private System.Windows.Forms.Button btnSwing;
        private System.Windows.Forms.Button btnRolling80;
        private System.Windows.Forms.Button btnRolling70;
        private System.Windows.Forms.Button btnRollingDown;
        private System.Windows.Forms.Button btnUnswing;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox txtCapsule;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.TextBox txtRollingRate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnIndexRotate;
        private System.Windows.Forms.TextBox txtRotateCount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnAuto;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtRollingOffset;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtLestLength;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btn70Save;
        private System.Windows.Forms.Button btn80Save;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.RadioButton radioBtnFast;
        private System.Windows.Forms.RadioButton radioBtnMidium;
        private System.Windows.Forms.RadioButton radioBtnSlow;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.RadioButton radioBtnSSlow;
        private System.Windows.Forms.Button btnVBlockDown;
        private System.Windows.Forms.Button btnVBlockUp;
        private System.Windows.Forms.TextBox txtVBlockFL_Limit_Value;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtVBlockFL_Offset_Value;
    }
}