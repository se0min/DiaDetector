namespace AutoAssembler
{
    partial class frmMotionSetting
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
            this.txtVelocityStart = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cboEmergency = new System.Windows.Forms.ComboBox();
            this.lstMotionList = new System.Windows.Forms.ListView();
            this.label1 = new System.Windows.Forms.Label();
            this.txtVelocityAcc = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtBallScrewLead = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtVelocityMax = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtVelocityDec = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtUnitPerPulse = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cboLimitPlus = new System.Windows.Forms.ComboBox();
            this.cboLimitMinus = new System.Windows.Forms.ComboBox();
            this.cboNear = new System.Windows.Forms.ComboBox();
            this.cboPulseMode = new System.Windows.Forms.ComboBox();
            this.cboHomeMode = new System.Windows.Forms.ComboBox();
            this.cboAlarm = new System.Windows.Forms.ComboBox();
            this.cboEncInput = new System.Windows.Forms.ComboBox();
            this.cboEncZ = new System.Windows.Forms.ComboBox();
            this.cboEnc = new System.Windows.Forms.ComboBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label17 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.cboAxisFunc = new System.Windows.Forms.ComboBox();
            this.label18 = new System.Windows.Forms.Label();
            this.txtMinValue = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.txtMaxValue = new System.Windows.Forms.TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label23 = new System.Windows.Forms.Label();
            this.txtVelocityHomeOffset = new System.Windows.Forms.TextBox();
            this.label24 = new System.Windows.Forms.Label();
            this.txtVelocityHome3 = new System.Windows.Forms.TextBox();
            this.label25 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.txtVelocityHome1 = new System.Windows.Forms.TextBox();
            this.txtHomeOffset = new System.Windows.Forms.TextBox();
            this.label27 = new System.Windows.Forms.Label();
            this.txtVelocityHome2 = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(702, 669);
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
            this.btnOk.Location = new System.Drawing.Point(614, 669);
            this.btnOk.Margin = new System.Windows.Forms.Padding(4);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(80, 40);
            this.btnOk.TabIndex = 70;
            this.btnOk.Text = "확인";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // txtVelocityStart
            // 
            this.txtVelocityStart.Location = new System.Drawing.Point(160, 27);
            this.txtVelocityStart.Margin = new System.Windows.Forms.Padding(4);
            this.txtVelocityStart.Name = "txtVelocityStart";
            this.txtVelocityStart.Size = new System.Drawing.Size(250, 26);
            this.txtVelocityStart.TabIndex = 86;
            this.txtVelocityStart.Text = "0.0";
            this.txtVelocityStart.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtVelocityStart.TextChanged += new System.EventHandler(this.txtVelocityStart_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(100, 30);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 16);
            this.label4.TabIndex = 81;
            this.label4.Text = "Start :";
            // 
            // cboEmergency
            // 
            this.cboEmergency.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboEmergency.FormattingEnabled = true;
            this.cboEmergency.Items.AddRange(new object[] {
            "NC",
            "NO"});
            this.cboEmergency.Location = new System.Drawing.Point(160, 26);
            this.cboEmergency.Margin = new System.Windows.Forms.Padding(4);
            this.cboEmergency.Name = "cboEmergency";
            this.cboEmergency.Size = new System.Drawing.Size(160, 24);
            this.cboEmergency.TabIndex = 74;
            this.cboEmergency.SelectedIndexChanged += new System.EventHandler(this.cboEmergency_SelectedIndexChanged);
            // 
            // lstMotionList
            // 
            this.lstMotionList.FullRowSelect = true;
            this.lstMotionList.Location = new System.Drawing.Point(23, 40);
            this.lstMotionList.Margin = new System.Windows.Forms.Padding(4);
            this.lstMotionList.Name = "lstMotionList";
            this.lstMotionList.Size = new System.Drawing.Size(320, 622);
            this.lstMotionList.TabIndex = 72;
            this.lstMotionList.UseCompatibleStateImageBehavior = false;
            this.lstMotionList.View = System.Windows.Forms.View.Details;
            this.lstMotionList.SelectedIndexChanged += new System.EventHandler(this.lstMotionList_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 20);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(124, 16);
            this.label1.TabIndex = 89;
            this.label1.Text = "제어 축 리스트 :";
            // 
            // txtVelocityAcc
            // 
            this.txtVelocityAcc.Location = new System.Drawing.Point(160, 61);
            this.txtVelocityAcc.Margin = new System.Windows.Forms.Padding(4);
            this.txtVelocityAcc.Name = "txtVelocityAcc";
            this.txtVelocityAcc.Size = new System.Drawing.Size(250, 26);
            this.txtVelocityAcc.TabIndex = 90;
            this.txtVelocityAcc.Text = "0.0";
            this.txtVelocityAcc.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtVelocityAcc.TextChanged += new System.EventHandler(this.txtVelocityAcc_TextChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtBallScrewLead);
            this.groupBox1.Controls.Add(this.label21);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.txtVelocityMax);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtVelocityDec);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtVelocityStart);
            this.groupBox1.Controls.Add(this.txtUnitPerPulse);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txtVelocityAcc);
            this.groupBox1.Location = new System.Drawing.Point(350, 30);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(432, 240);
            this.groupBox1.TabIndex = 92;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "속도 및 단위 설정";
            // 
            // txtBallScrewLead
            // 
            this.txtBallScrewLead.Location = new System.Drawing.Point(160, 197);
            this.txtBallScrewLead.Margin = new System.Windows.Forms.Padding(4);
            this.txtBallScrewLead.Name = "txtBallScrewLead";
            this.txtBallScrewLead.Size = new System.Drawing.Size(250, 26);
            this.txtBallScrewLead.TabIndex = 99;
            this.txtBallScrewLead.Text = "0.0";
            this.txtBallScrewLead.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtBallScrewLead.TextChanged += new System.EventHandler(this.txtBallScrewLead_TextChanged);
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(18, 200);
            this.label21.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(134, 16);
            this.label21.TabIndex = 100;
            this.label21.Text = "Ball Screw Lead :";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(103, 132);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(49, 16);
            this.label8.TabIndex = 95;
            this.label8.Text = "Max :";
            // 
            // txtVelocityMax
            // 
            this.txtVelocityMax.Location = new System.Drawing.Point(160, 129);
            this.txtVelocityMax.Margin = new System.Windows.Forms.Padding(4);
            this.txtVelocityMax.Name = "txtVelocityMax";
            this.txtVelocityMax.Size = new System.Drawing.Size(250, 26);
            this.txtVelocityMax.TabIndex = 94;
            this.txtVelocityMax.Text = "0.0";
            this.txtVelocityMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtVelocityMax.TextChanged += new System.EventHandler(this.txtVelocityMax_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(106, 98);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 16);
            this.label3.TabIndex = 93;
            this.label3.Text = "Dec :";
            // 
            // txtVelocityDec
            // 
            this.txtVelocityDec.Location = new System.Drawing.Point(160, 95);
            this.txtVelocityDec.Margin = new System.Windows.Forms.Padding(4);
            this.txtVelocityDec.Name = "txtVelocityDec";
            this.txtVelocityDec.Size = new System.Drawing.Size(250, 26);
            this.txtVelocityDec.TabIndex = 92;
            this.txtVelocityDec.Text = "0.0";
            this.txtVelocityDec.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtVelocityDec.TextChanged += new System.EventHandler(this.txtVelocityDec_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(106, 64);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 16);
            this.label2.TabIndex = 91;
            this.label2.Text = "Acc :";
            // 
            // txtUnitPerPulse
            // 
            this.txtUnitPerPulse.Location = new System.Drawing.Point(160, 163);
            this.txtUnitPerPulse.Margin = new System.Windows.Forms.Padding(4);
            this.txtUnitPerPulse.Name = "txtUnitPerPulse";
            this.txtUnitPerPulse.Size = new System.Drawing.Size(250, 26);
            this.txtUnitPerPulse.TabIndex = 96;
            this.txtUnitPerPulse.Text = "0.0";
            this.txtUnitPerPulse.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtUnitPerPulse.TextChanged += new System.EventHandler(this.txtUnitPerPulse_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(42, 166);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(110, 16);
            this.label6.TabIndex = 98;
            this.label6.Text = "UnitPerPulse :";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cboLimitPlus);
            this.groupBox2.Controls.Add(this.cboLimitMinus);
            this.groupBox2.Controls.Add(this.cboNear);
            this.groupBox2.Controls.Add(this.cboPulseMode);
            this.groupBox2.Controls.Add(this.cboHomeMode);
            this.groupBox2.Controls.Add(this.cboAlarm);
            this.groupBox2.Controls.Add(this.cboEncInput);
            this.groupBox2.Controls.Add(this.cboEncZ);
            this.groupBox2.Controls.Add(this.cboEnc);
            this.groupBox2.Controls.Add(this.label16);
            this.groupBox2.Controls.Add(this.label15);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.cboEmergency);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Enabled = false;
            this.groupBox2.Location = new System.Drawing.Point(23, 669);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(93, 26);
            this.groupBox2.TabIndex = 93;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "로직 설정";
            this.groupBox2.Visible = false;
            // 
            // cboLimitPlus
            // 
            this.cboLimitPlus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLimitPlus.FormattingEnabled = true;
            this.cboLimitPlus.Items.AddRange(new object[] {
            "NC",
            "NO"});
            this.cboLimitPlus.Location = new System.Drawing.Point(450, 128);
            this.cboLimitPlus.Margin = new System.Windows.Forms.Padding(4);
            this.cboLimitPlus.Name = "cboLimitPlus";
            this.cboLimitPlus.Size = new System.Drawing.Size(160, 24);
            this.cboLimitPlus.TabIndex = 116;
            this.cboLimitPlus.SelectedIndexChanged += new System.EventHandler(this.cboLimitPlus_SelectedIndexChanged);
            // 
            // cboLimitMinus
            // 
            this.cboLimitMinus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLimitMinus.FormattingEnabled = true;
            this.cboLimitMinus.Items.AddRange(new object[] {
            "NC",
            "NO"});
            this.cboLimitMinus.Location = new System.Drawing.Point(450, 94);
            this.cboLimitMinus.Margin = new System.Windows.Forms.Padding(4);
            this.cboLimitMinus.Name = "cboLimitMinus";
            this.cboLimitMinus.Size = new System.Drawing.Size(160, 24);
            this.cboLimitMinus.TabIndex = 115;
            this.cboLimitMinus.SelectedIndexChanged += new System.EventHandler(this.cboLimitMinus_SelectedIndexChanged);
            // 
            // cboNear
            // 
            this.cboNear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboNear.FormattingEnabled = true;
            this.cboNear.Items.AddRange(new object[] {
            "NC",
            "NO"});
            this.cboNear.Location = new System.Drawing.Point(450, 60);
            this.cboNear.Margin = new System.Windows.Forms.Padding(4);
            this.cboNear.Name = "cboNear";
            this.cboNear.Size = new System.Drawing.Size(160, 24);
            this.cboNear.TabIndex = 114;
            this.cboNear.SelectedIndexChanged += new System.EventHandler(this.cboNear_SelectedIndexChanged);
            // 
            // cboPulseMode
            // 
            this.cboPulseMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPulseMode.FormattingEnabled = true;
            this.cboPulseMode.Items.AddRange(new object[] {
            "2P,Low,CW/CCW",
            "2P,Low,CCW/CW",
            "2P,High,CW/CCW",
            "2P,High,CCW/CW",
            "1P,Low,CW/CCW",
            "1P,Low,CCW/CCW",
            "1P,High,CW/CCW",
            "1P,High,CCW/CW"});
            this.cboPulseMode.Location = new System.Drawing.Point(160, 230);
            this.cboPulseMode.Margin = new System.Windows.Forms.Padding(4);
            this.cboPulseMode.Name = "cboPulseMode";
            this.cboPulseMode.Size = new System.Drawing.Size(160, 24);
            this.cboPulseMode.TabIndex = 113;
            this.cboPulseMode.SelectedIndexChanged += new System.EventHandler(this.cboPulseMode_SelectedIndexChanged);
            // 
            // cboHomeMode
            // 
            this.cboHomeMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboHomeMode.FormattingEnabled = true;
            this.cboHomeMode.Items.AddRange(new object[] {
            "+Limit",
            "-Limit",
            "+Near",
            "-Near",
            "+Limit + Z",
            "-Limit + Z",
            "+Near + Z",
            "-Near + Z",
            "Z-",
            "Z+"});
            this.cboHomeMode.Location = new System.Drawing.Point(160, 194);
            this.cboHomeMode.Margin = new System.Windows.Forms.Padding(4);
            this.cboHomeMode.Name = "cboHomeMode";
            this.cboHomeMode.Size = new System.Drawing.Size(160, 24);
            this.cboHomeMode.TabIndex = 112;
            this.cboHomeMode.SelectedIndexChanged += new System.EventHandler(this.cboHomeMode_SelectedIndexChanged);
            // 
            // cboAlarm
            // 
            this.cboAlarm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAlarm.FormattingEnabled = true;
            this.cboAlarm.Items.AddRange(new object[] {
            "NC",
            "NO"});
            this.cboAlarm.Location = new System.Drawing.Point(160, 162);
            this.cboAlarm.Margin = new System.Windows.Forms.Padding(4);
            this.cboAlarm.Name = "cboAlarm";
            this.cboAlarm.Size = new System.Drawing.Size(160, 24);
            this.cboAlarm.TabIndex = 111;
            this.cboAlarm.SelectedIndexChanged += new System.EventHandler(this.cboAlarm_SelectedIndexChanged);
            // 
            // cboEncInput
            // 
            this.cboEncInput.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboEncInput.FormattingEnabled = true;
            this.cboEncInput.Items.AddRange(new object[] {
            "EA/EB",
            "EB/EA"});
            this.cboEncInput.Location = new System.Drawing.Point(160, 128);
            this.cboEncInput.Margin = new System.Windows.Forms.Padding(4);
            this.cboEncInput.Name = "cboEncInput";
            this.cboEncInput.Size = new System.Drawing.Size(160, 24);
            this.cboEncInput.TabIndex = 110;
            this.cboEncInput.SelectedIndexChanged += new System.EventHandler(this.cboEncInput_SelectedIndexChanged);
            // 
            // cboEncZ
            // 
            this.cboEncZ.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboEncZ.FormattingEnabled = true;
            this.cboEncZ.Items.AddRange(new object[] {
            "NC",
            "NO"});
            this.cboEncZ.Location = new System.Drawing.Point(160, 94);
            this.cboEncZ.Margin = new System.Windows.Forms.Padding(4);
            this.cboEncZ.Name = "cboEncZ";
            this.cboEncZ.Size = new System.Drawing.Size(160, 24);
            this.cboEncZ.TabIndex = 109;
            this.cboEncZ.SelectedIndexChanged += new System.EventHandler(this.cboEncZ_SelectedIndexChanged);
            // 
            // cboEnc
            // 
            this.cboEnc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboEnc.FormattingEnabled = true;
            this.cboEnc.Items.AddRange(new object[] {
            "4채배",
            "2채배",
            "1채배"});
            this.cboEnc.Location = new System.Drawing.Point(160, 60);
            this.cboEnc.Margin = new System.Windows.Forms.Padding(4);
            this.cboEnc.Name = "cboEnc";
            this.cboEnc.Size = new System.Drawing.Size(160, 24);
            this.cboEnc.TabIndex = 108;
            this.cboEnc.SelectedIndexChanged += new System.EventHandler(this.cboEnc_SelectedIndexChanged);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(48, 233);
            this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(104, 16);
            this.label16.TabIndex = 107;
            this.label16.Text = "Pulse Mode :";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(46, 199);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(106, 16);
            this.label15.TabIndex = 106;
            this.label15.Text = "Home Mode :";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(94, 165);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(58, 16);
            this.label14.TabIndex = 105;
            this.label14.Text = "Alarm :";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(378, 131);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(64, 16);
            this.label13.TabIndex = 104;
            this.label13.Text = "+ Limit :";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(378, 97);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(64, 16);
            this.label12.TabIndex = 103;
            this.label12.Text = "- Limit :";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(390, 63);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(52, 16);
            this.label11.TabIndex = 102;
            this.label11.Text = "Near :";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(66, 131);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(86, 16);
            this.label10.TabIndex = 101;
            this.label10.Text = "Enc Input :";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(53, 29);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(99, 16);
            this.label5.TabIndex = 97;
            this.label5.Text = "Emergency :";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(84, 97);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(68, 16);
            this.label9.TabIndex = 100;
            this.label9.Text = "Enc(Z) :";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(106, 63);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(46, 16);
            this.label7.TabIndex = 99;
            this.label7.Text = "Enc :";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label17);
            this.groupBox3.Controls.Add(this.txtName);
            this.groupBox3.Controls.Add(this.cboAxisFunc);
            this.groupBox3.Controls.Add(this.label18);
            this.groupBox3.Controls.Add(this.txtMinValue);
            this.groupBox3.Controls.Add(this.label19);
            this.groupBox3.Controls.Add(this.label20);
            this.groupBox3.Controls.Add(this.txtMaxValue);
            this.groupBox3.Location = new System.Drawing.Point(350, 276);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(432, 170);
            this.groupBox3.TabIndex = 96;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "제어 설정";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(44, 30);
            this.label17.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(108, 16);
            this.label17.TabIndex = 119;
            this.label17.Text = "제어 축 이름 :";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(160, 27);
            this.txtName.Margin = new System.Windows.Forms.Padding(4);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(250, 26);
            this.txtName.TabIndex = 118;
            this.txtName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // cboAxisFunc
            // 
            this.cboAxisFunc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAxisFunc.FormattingEnabled = true;
            this.cboAxisFunc.Location = new System.Drawing.Point(160, 61);
            this.cboAxisFunc.Margin = new System.Windows.Forms.Padding(4);
            this.cboAxisFunc.Name = "cboAxisFunc";
            this.cboAxisFunc.Size = new System.Drawing.Size(250, 24);
            this.cboAxisFunc.TabIndex = 117;
            this.cboAxisFunc.SelectedIndexChanged += new System.EventHandler(this.cboAxisFunc_SelectedIndexChanged);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(42, 96);
            this.label18.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(110, 16);
            this.label18.TabIndex = 93;
            this.label18.Text = "SW Limit Min :";
            // 
            // txtMinValue
            // 
            this.txtMinValue.Location = new System.Drawing.Point(160, 93);
            this.txtMinValue.Margin = new System.Windows.Forms.Padding(4);
            this.txtMinValue.Name = "txtMinValue";
            this.txtMinValue.Size = new System.Drawing.Size(250, 26);
            this.txtMinValue.TabIndex = 92;
            this.txtMinValue.Text = "0.0";
            this.txtMinValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtMinValue.TextChanged += new System.EventHandler(this.txtMinValue_TextChanged);
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(36, 130);
            this.label19.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(116, 16);
            this.label19.TabIndex = 91;
            this.label19.Text = "SW Limit Max :";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(81, 64);
            this.label20.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(71, 16);
            this.label20.TabIndex = 81;
            this.label20.Text = "축 기능 :";
            // 
            // txtMaxValue
            // 
            this.txtMaxValue.Location = new System.Drawing.Point(160, 127);
            this.txtMaxValue.Margin = new System.Windows.Forms.Padding(4);
            this.txtMaxValue.Name = "txtMaxValue";
            this.txtMaxValue.Size = new System.Drawing.Size(250, 26);
            this.txtMaxValue.TabIndex = 90;
            this.txtMaxValue.Text = "0.0";
            this.txtMaxValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtMaxValue.TextChanged += new System.EventHandler(this.txtMaxValue_TextChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label23);
            this.groupBox4.Controls.Add(this.txtVelocityHomeOffset);
            this.groupBox4.Controls.Add(this.label24);
            this.groupBox4.Controls.Add(this.txtVelocityHome3);
            this.groupBox4.Controls.Add(this.label25);
            this.groupBox4.Controls.Add(this.label26);
            this.groupBox4.Controls.Add(this.txtVelocityHome1);
            this.groupBox4.Controls.Add(this.txtHomeOffset);
            this.groupBox4.Controls.Add(this.label27);
            this.groupBox4.Controls.Add(this.txtVelocityHome2);
            this.groupBox4.Location = new System.Drawing.Point(350, 452);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(432, 210);
            this.groupBox4.TabIndex = 101;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "원점 설정";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(52, 132);
            this.label23.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(100, 16);
            this.label23.TabIndex = 95;
            this.label23.Text = "Offset 속도 :";
            // 
            // txtVelocityHomeOffset
            // 
            this.txtVelocityHomeOffset.Location = new System.Drawing.Point(160, 129);
            this.txtVelocityHomeOffset.Margin = new System.Windows.Forms.Padding(4);
            this.txtVelocityHomeOffset.Name = "txtVelocityHomeOffset";
            this.txtVelocityHomeOffset.Size = new System.Drawing.Size(250, 26);
            this.txtVelocityHomeOffset.TabIndex = 94;
            this.txtVelocityHomeOffset.Text = "0.0";
            this.txtVelocityHomeOffset.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtVelocityHomeOffset.TextChanged += new System.EventHandler(this.txtVelocityHomeOffset_TextChanged);
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(89, 98);
            this.label24.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(63, 16);
            this.label24.TabIndex = 93;
            this.label24.Text = "속도 3 :";
            // 
            // txtVelocityHome3
            // 
            this.txtVelocityHome3.Location = new System.Drawing.Point(160, 95);
            this.txtVelocityHome3.Margin = new System.Windows.Forms.Padding(4);
            this.txtVelocityHome3.Name = "txtVelocityHome3";
            this.txtVelocityHome3.Size = new System.Drawing.Size(250, 26);
            this.txtVelocityHome3.TabIndex = 92;
            this.txtVelocityHome3.Text = "0.0";
            this.txtVelocityHome3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtVelocityHome3.TextChanged += new System.EventHandler(this.txtVelocityHome3_TextChanged);
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(89, 64);
            this.label25.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(63, 16);
            this.label25.TabIndex = 91;
            this.label25.Text = "속도 2 :";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(89, 30);
            this.label26.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(63, 16);
            this.label26.TabIndex = 81;
            this.label26.Text = "속도 1 :";
            // 
            // txtVelocityHome1
            // 
            this.txtVelocityHome1.Location = new System.Drawing.Point(160, 27);
            this.txtVelocityHome1.Margin = new System.Windows.Forms.Padding(4);
            this.txtVelocityHome1.Name = "txtVelocityHome1";
            this.txtVelocityHome1.Size = new System.Drawing.Size(250, 26);
            this.txtVelocityHome1.TabIndex = 86;
            this.txtVelocityHome1.Text = "0.0";
            this.txtVelocityHome1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtVelocityHome1.TextChanged += new System.EventHandler(this.txtVelocityHome1_TextChanged);
            // 
            // txtHomeOffset
            // 
            this.txtHomeOffset.Location = new System.Drawing.Point(160, 163);
            this.txtHomeOffset.Margin = new System.Windows.Forms.Padding(4);
            this.txtHomeOffset.Name = "txtHomeOffset";
            this.txtHomeOffset.Size = new System.Drawing.Size(250, 26);
            this.txtHomeOffset.TabIndex = 96;
            this.txtHomeOffset.Text = "0.0";
            this.txtHomeOffset.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtHomeOffset.TextChanged += new System.EventHandler(this.txtHomeOffset_TextChanged);
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(52, 166);
            this.label27.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(100, 16);
            this.label27.TabIndex = 98;
            this.label27.Text = "Offset 위치 :";
            // 
            // txtVelocityHome2
            // 
            this.txtVelocityHome2.Location = new System.Drawing.Point(160, 61);
            this.txtVelocityHome2.Margin = new System.Windows.Forms.Padding(4);
            this.txtVelocityHome2.Name = "txtVelocityHome2";
            this.txtVelocityHome2.Size = new System.Drawing.Size(250, 26);
            this.txtVelocityHome2.TabIndex = 90;
            this.txtVelocityHome2.Text = "0.0";
            this.txtVelocityHome2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtVelocityHome2.TextChanged += new System.EventHandler(this.txtVelocityHome2_TextChanged);
            // 
            // frmMotionSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(794, 727);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lstMotionList);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmMotionSetting";
            this.Text = "모션 설정";
            this.Load += new System.EventHandler(this.frmControlSetting_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.TextBox txtVelocityStart;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cboEmergency;
        private System.Windows.Forms.ListView lstMotionList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtVelocityAcc;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtVelocityMax;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtVelocityDec;
        private System.Windows.Forms.TextBox txtUnitPerPulse;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.ComboBox cboLimitPlus;
        private System.Windows.Forms.ComboBox cboLimitMinus;
        private System.Windows.Forms.ComboBox cboNear;
        private System.Windows.Forms.ComboBox cboPulseMode;
        private System.Windows.Forms.ComboBox cboHomeMode;
        private System.Windows.Forms.ComboBox cboAlarm;
        private System.Windows.Forms.ComboBox cboEncInput;
        private System.Windows.Forms.ComboBox cboEncZ;
        private System.Windows.Forms.ComboBox cboEnc;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox cboAxisFunc;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox txtMinValue;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox txtMaxValue;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtBallScrewLead;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.TextBox txtVelocityHomeOffset;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.TextBox txtVelocityHome3;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.TextBox txtVelocityHome1;
        private System.Windows.Forms.TextBox txtHomeOffset;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.TextBox txtVelocityHome2;
    }
}