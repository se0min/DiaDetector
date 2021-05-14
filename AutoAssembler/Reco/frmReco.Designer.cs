namespace AutoAssembler
{
    partial class frmReco
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.BusResetWorker = new System.ComponentModel.BackgroundWorker();
            this.UIDList = new System.Windows.Forms.ComboBox();
            this.CameraClose = new System.Windows.Forms.Button();
            this.CameraOpen = new System.Windows.Forms.Button();
            this.FpsUpdate = new System.ComponentModel.BackgroundWorker();
            this.DisplayUpdate = new System.ComponentModel.BackgroundWorker();
            this.PanelPicture = new System.Windows.Forms.Panel();
            this.AutoScaleBox = new System.Windows.Forms.PictureBox();
            this.RealScaleBox = new System.Windows.Forms.PictureBox();
            this.DispSize = new System.Windows.Forms.CheckBox();
            this.BStop = new System.Windows.Forms.Button();
            this.BStart = new System.Windows.Forms.Button();
            this.btDXFOpen = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btZoomIn = new System.Windows.Forms.Button();
            this.btZoomOut = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.button20 = new System.Windows.Forms.Button();
            this.button19 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.trackBar2 = new System.Windows.Forms.TrackBar();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.button18 = new System.Windows.Forms.Button();
            this.button17 = new System.Windows.Forms.Button();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.timer3 = new System.Windows.Forms.Timer(this.components);
            this.btUp = new System.Windows.Forms.Button();
            this.btDown = new System.Windows.Forms.Button();
            this.timer4 = new System.Windows.Forms.Timer(this.components);
            this.btShotSave = new System.Windows.Forms.Button();
            this.timWork = new System.Windows.Forms.Timer(this.components);
            this.btLoopStart = new System.Windows.Forms.Button();
            this.btLoopStop = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.cboLampList = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.cboMoveList = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cboIndexList = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.BusReset = new System.Windows.Forms.Label();
            this.btFullShot = new System.Windows.Forms.Button();
            this.btDetailShot = new System.Windows.Forms.Button();
            this.btnJogLampHome = new System.Windows.Forms.Button();
            this.btnJogZHome = new System.Windows.Forms.Button();
            this.btnJogYHome = new System.Windows.Forms.Button();
            this.btnJogXHome = new System.Windows.Forms.Button();
            this.btnJogRHome = new System.Windows.Forms.Button();
            this.btnJogLampPlus = new System.Windows.Forms.Button();
            this.btnJogLampMinus = new System.Windows.Forms.Button();
            this.txtLampValue = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.btnJogRPlus = new System.Windows.Forms.Button();
            this.btnJogRMinus = new System.Windows.Forms.Button();
            this.txtRAxisValue = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.btnJogZPlus = new System.Windows.Forms.Button();
            this.btnJogZMinus = new System.Windows.Forms.Button();
            this.txtZAxisValue = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.btnJogYPlus = new System.Windows.Forms.Button();
            this.btnJogYMinus = new System.Windows.Forms.Button();
            this.txtYAxisValue = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.btnJogXPlus = new System.Windows.Forms.Button();
            this.btnJogXMinus = new System.Windows.Forms.Button();
            this.txtXAxisValue = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.btDXFUp = new System.Windows.Forms.Button();
            this.btDXFLeft = new System.Windows.Forms.Button();
            this.btDXFRight = new System.Windows.Forms.Button();
            this.btDXFDown = new System.Windows.Forms.Button();
            this.timDXFWork = new System.Windows.Forms.Timer(this.components);
            this.btShotSetSave = new System.Windows.Forms.Button();
            this.cboZoomCamList = new System.Windows.Forms.ComboBox();
            this.timLoadStart = new System.Windows.Forms.Timer(this.components);
            this.btEmergency = new System.Windows.Forms.Button();
            this.timerAxis = new System.Windows.Forms.Timer(this.components);
            this.picRecoImg = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.timWaitDelay = new System.Windows.Forms.Timer(this.components);
            this.btCalibrationRun = new System.Windows.Forms.Button();
            this.timCalibration = new System.Windows.Forms.Timer(this.components);
            this.btSetCalibrationValue = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.txtFixAngleAddGap = new System.Windows.Forms.TextBox();
            this.txtMoveAngleAddGap = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.txtBackAngleAddGap = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.txtPlangeThickness = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.btSetCenterPos = new System.Windows.Forms.Button();
            this.txtCalcPinDiameter = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.txtPinHoleDiameter = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.txtPinHoleDistance = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.txtTopAngleAddGap = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.txtCalcPinHeight = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.cboTopHolePos = new System.Windows.Forms.ComboBox();
            this.radioBtnSSlow = new System.Windows.Forms.RadioButton();
            this.radioBtnFast = new System.Windows.Forms.RadioButton();
            this.radioBtnMidium = new System.Windows.Forms.RadioButton();
            this.radioBtnSlow = new System.Windows.Forms.RadioButton();
            this.label25 = new System.Windows.Forms.Label();
            this.btnJogMRHome = new System.Windows.Forms.Button();
            this.btnJogMRPlus = new System.Windows.Forms.Button();
            this.btnJogMRMinus = new System.Windows.Forms.Button();
            this.txtMoveRValue = new System.Windows.Forms.TextBox();
            this.label26 = new System.Windows.Forms.Label();
            this.chkGuideLine = new System.Windows.Forms.CheckBox();
            this.label27 = new System.Windows.Forms.Label();
            this.txtTopDirHoleDistance = new System.Windows.Forms.TextBox();
            this.chkDXFView = new System.Windows.Forms.CheckBox();
            this.btDXFCamFitting = new System.Windows.Forms.Button();
            this.label28 = new System.Windows.Forms.Label();
            this.txtShaftLength = new System.Windows.Forms.TextBox();
            this.timLevelAlign = new System.Windows.Forms.Timer(this.components);
            this.btLevelCalc = new System.Windows.Forms.Button();
            this.cboDXFType = new System.Windows.Forms.ComboBox();
            this.label29 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.PanelPicture.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AutoScaleBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RealScaleBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picRecoImg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // UIDList
            // 
            this.UIDList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.UIDList.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.UIDList.FormattingEnabled = true;
            this.UIDList.Location = new System.Drawing.Point(268, -8);
            this.UIDList.Name = "UIDList";
            this.UIDList.Size = new System.Drawing.Size(190, 24);
            this.UIDList.TabIndex = 7;
            this.UIDList.Visible = false;
            this.UIDList.SelectedIndexChanged += new System.EventHandler(this.UIDList_SelectedIndexChanged);
            // 
            // CameraClose
            // 
            this.CameraClose.Location = new System.Drawing.Point(1221, 667);
            this.CameraClose.Name = "CameraClose";
            this.CameraClose.Size = new System.Drawing.Size(87, 23);
            this.CameraClose.TabIndex = 10;
            this.CameraClose.Text = "Close";
            this.CameraClose.UseVisualStyleBackColor = true;
            this.CameraClose.Visible = false;
            this.CameraClose.Click += new System.EventHandler(this.CameraClose_Click);
            // 
            // CameraOpen
            // 
            this.CameraOpen.Location = new System.Drawing.Point(1221, 646);
            this.CameraOpen.Name = "CameraOpen";
            this.CameraOpen.Size = new System.Drawing.Size(87, 23);
            this.CameraOpen.TabIndex = 9;
            this.CameraOpen.Text = "Open";
            this.CameraOpen.UseVisualStyleBackColor = true;
            this.CameraOpen.Visible = false;
            this.CameraOpen.Click += new System.EventHandler(this.CameraOpen_Click);
            // 
            // DisplayUpdate
            // 
            this.DisplayUpdate.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.DisplayUpdate_RunWorkerCompleted);
            // 
            // PanelPicture
            // 
            this.PanelPicture.AutoScroll = true;
            this.PanelPicture.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PanelPicture.Controls.Add(this.label30);
            this.PanelPicture.Controls.Add(this.AutoScaleBox);
            this.PanelPicture.Controls.Add(this.RealScaleBox);
            this.PanelPicture.Location = new System.Drawing.Point(12, 38);
            this.PanelPicture.Name = "PanelPicture";
            this.PanelPicture.Size = new System.Drawing.Size(924, 773);
            this.PanelPicture.TabIndex = 16;
            // 
            // AutoScaleBox
            // 
            this.AutoScaleBox.Location = new System.Drawing.Point(-3, -2);
            this.AutoScaleBox.Name = "AutoScaleBox";
            this.AutoScaleBox.Size = new System.Drawing.Size(924, 773);
            this.AutoScaleBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.AutoScaleBox.TabIndex = 0;
            this.AutoScaleBox.TabStop = false;
            this.AutoScaleBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.AutoScaleBox_MouseDown);
            // 
            // RealScaleBox
            // 
            this.RealScaleBox.Location = new System.Drawing.Point(-2, 0);
            this.RealScaleBox.Margin = new System.Windows.Forms.Padding(1);
            this.RealScaleBox.Name = "RealScaleBox";
            this.RealScaleBox.Size = new System.Drawing.Size(924, 773);
            this.RealScaleBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.RealScaleBox.TabIndex = 23;
            this.RealScaleBox.TabStop = false;
            this.RealScaleBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.RealScaleBox_MouseDown);
            // 
            // DispSize
            // 
            this.DispSize.AutoSize = true;
            this.DispSize.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.DispSize.Location = new System.Drawing.Point(271, 11);
            this.DispSize.Name = "DispSize";
            this.DispSize.Size = new System.Drawing.Size(103, 20);
            this.DispSize.TabIndex = 19;
            this.DispSize.Text = "Real Scale";
            this.DispSize.UseVisualStyleBackColor = true;
            // 
            // BStop
            // 
            this.BStop.Enabled = false;
            this.BStop.Location = new System.Drawing.Point(1220, 700);
            this.BStop.Name = "BStop";
            this.BStop.Size = new System.Drawing.Size(87, 23);
            this.BStop.TabIndex = 18;
            this.BStop.Text = "Stop";
            this.BStop.UseVisualStyleBackColor = true;
            this.BStop.Visible = false;
            this.BStop.Click += new System.EventHandler(this.BStop_Click);
            // 
            // BStart
            // 
            this.BStart.Enabled = false;
            this.BStart.Location = new System.Drawing.Point(1220, 685);
            this.BStart.Name = "BStart";
            this.BStart.Size = new System.Drawing.Size(87, 23);
            this.BStart.TabIndex = 17;
            this.BStart.Text = "Start";
            this.BStart.UseVisualStyleBackColor = true;
            this.BStart.Visible = false;
            this.BStart.Click += new System.EventHandler(this.BStart_Click);
            // 
            // btDXFOpen
            // 
            this.btDXFOpen.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btDXFOpen.Location = new System.Drawing.Point(1159, 71);
            this.btDXFOpen.Name = "btDXFOpen";
            this.btDXFOpen.Size = new System.Drawing.Size(96, 45);
            this.btDXFOpen.TabIndex = 20;
            this.btDXFOpen.Text = "DXF_File Open";
            this.btDXFOpen.UseVisualStyleBackColor = true;
            this.btDXFOpen.Click += new System.EventHandler(this.btDXFOpen_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // btZoomIn
            // 
            this.btZoomIn.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btZoomIn.Location = new System.Drawing.Point(1227, 149);
            this.btZoomIn.Name = "btZoomIn";
            this.btZoomIn.Size = new System.Drawing.Size(27, 23);
            this.btZoomIn.TabIndex = 21;
            this.btZoomIn.Text = "+";
            this.btZoomIn.UseVisualStyleBackColor = true;
            this.btZoomIn.Click += new System.EventHandler(this.btZoomIn_Click);
            this.btZoomIn.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btZoomIn_MouseDown);
            this.btZoomIn.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btZoomIn_MouseUp);
            // 
            // btZoomOut
            // 
            this.btZoomOut.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btZoomOut.Location = new System.Drawing.Point(1227, 120);
            this.btZoomOut.Name = "btZoomOut";
            this.btZoomOut.Size = new System.Drawing.Size(27, 23);
            this.btZoomOut.TabIndex = 22;
            this.btZoomOut.Text = "-";
            this.btZoomOut.UseVisualStyleBackColor = true;
            this.btZoomOut.Click += new System.EventHandler(this.btZoomOut_Click);
            this.btZoomOut.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btZoomOut_MouseDown);
            this.btZoomOut.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btZoomOut_MouseUp);
            // 
            // timer1
            // 
            this.timer1.Interval = 300;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // button20
            // 
            this.button20.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button20.Location = new System.Drawing.Point(1016, 120);
            this.button20.Name = "button20";
            this.button20.Size = new System.Drawing.Size(60, 23);
            this.button20.TabIndex = 109;
            this.button20.Text = "Limit";
            this.button20.UseVisualStyleBackColor = true;
            this.button20.Click += new System.EventHandler(this.button20_Click);
            // 
            // button19
            // 
            this.button19.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button19.Location = new System.Drawing.Point(944, 120);
            this.button19.Name = "button19";
            this.button19.Size = new System.Drawing.Size(60, 23);
            this.button19.TabIndex = 108;
            this.button19.Text = "Limit";
            this.button19.UseVisualStyleBackColor = true;
            this.button19.Click += new System.EventHandler(this.button19_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(1021, 69);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 16);
            this.label4.TabIndex = 107;
            this.label4.Text = "Focus";
            // 
            // textBox2
            // 
            this.textBox2.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox2.Location = new System.Drawing.Point(1018, 89);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(59, 26);
            this.textBox2.TabIndex = 106;
            this.textBox2.Text = "0";
            this.textBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox2_KeyDown);
            // 
            // trackBar2
            // 
            this.trackBar2.LargeChange = 1;
            this.trackBar2.Location = new System.Drawing.Point(1030, 236);
            this.trackBar2.Maximum = 25000;
            this.trackBar2.Name = "trackBar2";
            this.trackBar2.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBar2.Size = new System.Drawing.Size(45, 631);
            this.trackBar2.TabIndex = 105;
            this.trackBar2.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBar2.Scroll += new System.EventHandler(this.trackBar2_Scroll);
            this.trackBar2.ValueChanged += new System.EventHandler(this.trackBar2_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(950, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 16);
            this.label3.TabIndex = 104;
            this.label3.Text = "Zoom";
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox1.Location = new System.Drawing.Point(944, 89);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(59, 26);
            this.textBox1.TabIndex = 103;
            this.textBox1.Text = "0";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
            // 
            // trackBar1
            // 
            this.trackBar1.LargeChange = 1;
            this.trackBar1.Location = new System.Drawing.Point(958, 178);
            this.trackBar1.Maximum = 9999;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBar1.Size = new System.Drawing.Size(45, 689);
            this.trackBar1.TabIndex = 102;
            this.trackBar1.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            this.trackBar1.ValueChanged += new System.EventHandler(this.trackBar1_ValueChanged);
            // 
            // button18
            // 
            this.button18.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button18.Location = new System.Drawing.Point(1015, 149);
            this.button18.Name = "button18";
            this.button18.Size = new System.Drawing.Size(60, 23);
            this.button18.TabIndex = 111;
            this.button18.Text = "Home";
            this.button18.UseVisualStyleBackColor = true;
            this.button18.Click += new System.EventHandler(this.button18_Click);
            // 
            // button17
            // 
            this.button17.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button17.Location = new System.Drawing.Point(943, 149);
            this.button17.Name = "button17";
            this.button17.Size = new System.Drawing.Size(60, 23);
            this.button17.TabIndex = 110;
            this.button17.Text = "Home";
            this.button17.UseVisualStyleBackColor = true;
            this.button17.Click += new System.EventHandler(this.button17_Click);
            // 
            // timer2
            // 
            this.timer2.Interval = 500;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // timer3
            // 
            this.timer3.Tick += new System.EventHandler(this.timer3_Tick);
            // 
            // btUp
            // 
            this.btUp.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btUp.Location = new System.Drawing.Point(1016, 178);
            this.btUp.Name = "btUp";
            this.btUp.Size = new System.Drawing.Size(60, 23);
            this.btUp.TabIndex = 112;
            this.btUp.Text = "Up";
            this.btUp.UseVisualStyleBackColor = true;
            this.btUp.Click += new System.EventHandler(this.btUp_Click);
            this.btUp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btUp_MouseDown);
            this.btUp.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btUp_MouseUp);
            // 
            // btDown
            // 
            this.btDown.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btDown.Location = new System.Drawing.Point(1016, 207);
            this.btDown.Name = "btDown";
            this.btDown.Size = new System.Drawing.Size(60, 23);
            this.btDown.TabIndex = 113;
            this.btDown.Text = "Down";
            this.btDown.UseVisualStyleBackColor = true;
            this.btDown.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btDown_MouseDown);
            this.btDown.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btDown_MouseUp);
            // 
            // timer4
            // 
            this.timer4.Interval = 50;
            this.timer4.Tick += new System.EventHandler(this.timer4_Tick);
            // 
            // btShotSave
            // 
            this.btShotSave.Enabled = false;
            this.btShotSave.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btShotSave.Location = new System.Drawing.Point(1094, 71);
            this.btShotSave.Name = "btShotSave";
            this.btShotSave.Size = new System.Drawing.Size(59, 45);
            this.btShotSave.TabIndex = 114;
            this.btShotSave.Text = "Shot Save";
            this.btShotSave.UseVisualStyleBackColor = true;
            this.btShotSave.Click += new System.EventHandler(this.btShotSave_Click);
            // 
            // timWork
            // 
            this.timWork.Tick += new System.EventHandler(this.timWork_Tick);
            // 
            // btLoopStart
            // 
            this.btLoopStart.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btLoopStart.Location = new System.Drawing.Point(756, 873);
            this.btLoopStart.Name = "btLoopStart";
            this.btLoopStart.Size = new System.Drawing.Size(87, 43);
            this.btLoopStart.TabIndex = 115;
            this.btLoopStart.Text = "TestStart";
            this.btLoopStart.UseVisualStyleBackColor = true;
            this.btLoopStart.Click += new System.EventHandler(this.btLoopStart_Click);
            // 
            // btLoopStop
            // 
            this.btLoopStop.Enabled = false;
            this.btLoopStop.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btLoopStop.Location = new System.Drawing.Point(849, 873);
            this.btLoopStop.Name = "btLoopStop";
            this.btLoopStop.Size = new System.Drawing.Size(87, 43);
            this.btLoopStop.TabIndex = 116;
            this.btLoopStop.Text = "TestStop";
            this.btLoopStop.UseVisualStyleBackColor = true;
            this.btLoopStop.Click += new System.EventHandler(this.btLoopStop_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1240, 670);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 12);
            this.label2.TabIndex = 119;
            this.label2.Text = "label2";
            this.label2.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.Location = new System.Drawing.Point(14, 844);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(156, 16);
            this.label5.TabIndex = 120;
            this.label5.Text = "Recognizing........";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(1165, 702);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(38, 12);
            this.label6.TabIndex = 121;
            this.label6.Text = "label6";
            this.label6.Visible = false;
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnClose.Location = new System.Drawing.Point(1168, 874);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(87, 43);
            this.btnClose.TabIndex = 122;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSave.Location = new System.Drawing.Point(1075, 874);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(87, 43);
            this.btnSave.TabIndex = 123;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // cboLampList
            // 
            this.cboLampList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLampList.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cboLampList.FormattingEnabled = true;
            this.cboLampList.Location = new System.Drawing.Point(1077, 7);
            this.cboLampList.Name = "cboLampList";
            this.cboLampList.Size = new System.Drawing.Size(178, 24);
            this.cboLampList.TabIndex = 130;
            this.cboLampList.SelectedIndexChanged += new System.EventHandler(this.cboLampList_SelectedIndexChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label11.Location = new System.Drawing.Point(963, 11);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(114, 16);
            this.label11.TabIndex = 129;
            this.label11.Text = "조명제어항목 :";
            // 
            // cboMoveList
            // 
            this.cboMoveList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMoveList.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cboMoveList.FormattingEnabled = true;
            this.cboMoveList.Location = new System.Drawing.Point(764, 7);
            this.cboMoveList.Name = "cboMoveList";
            this.cboMoveList.Size = new System.Drawing.Size(193, 24);
            this.cboMoveList.TabIndex = 128;
            this.cboMoveList.SelectedIndexChanged += new System.EventHandler(this.cboMoveList_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(638, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(127, 16);
            this.label1.TabIndex = 127;
            this.label1.Text = "제어MOVE항목 :";
            // 
            // cboIndexList
            // 
            this.cboIndexList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboIndexList.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cboIndexList.FormattingEnabled = true;
            this.cboIndexList.Location = new System.Drawing.Point(464, 8);
            this.cboIndexList.Name = "cboIndexList";
            this.cboIndexList.Size = new System.Drawing.Size(164, 24);
            this.cboIndexList.TabIndex = 126;
            this.cboIndexList.SelectedIndexChanged += new System.EventHandler(this.cboIndexList_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label7.Location = new System.Drawing.Point(372, 12);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(95, 16);
            this.label7.TabIndex = 125;
            this.label7.Text = "동작INDEX :";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label8.Location = new System.Drawing.Point(10, 12);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(66, 16);
            this.label8.TabIndex = 124;
            this.label8.Text = "카메라 :";
            // 
            // BusReset
            // 
            this.BusReset.AutoSize = true;
            this.BusReset.Location = new System.Drawing.Point(10, -1);
            this.BusReset.Name = "BusReset";
            this.BusReset.Size = new System.Drawing.Size(59, 12);
            this.BusReset.TabIndex = 131;
            this.BusReset.Text = "BusReset";
            this.BusReset.Visible = false;
            // 
            // btFullShot
            // 
            this.btFullShot.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btFullShot.Location = new System.Drawing.Point(945, 38);
            this.btFullShot.Name = "btFullShot";
            this.btFullShot.Size = new System.Drawing.Size(136, 28);
            this.btFullShot.TabIndex = 132;
            this.btFullShot.Text = "Full Shot";
            this.btFullShot.UseVisualStyleBackColor = true;
            this.btFullShot.Click += new System.EventHandler(this.btFullShot_Click);
            // 
            // btDetailShot
            // 
            this.btDetailShot.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btDetailShot.Location = new System.Drawing.Point(1094, 39);
            this.btDetailShot.Name = "btDetailShot";
            this.btDetailShot.Size = new System.Drawing.Size(161, 28);
            this.btDetailShot.TabIndex = 133;
            this.btDetailShot.Text = "Detail Shot";
            this.btDetailShot.UseVisualStyleBackColor = true;
            this.btDetailShot.Click += new System.EventHandler(this.btDetailShot_Click);
            // 
            // btnJogLampHome
            // 
            this.btnJogLampHome.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnJogLampHome.Location = new System.Drawing.Point(1227, 432);
            this.btnJogLampHome.Name = "btnJogLampHome";
            this.btnJogLampHome.Size = new System.Drawing.Size(27, 25);
            this.btnJogLampHome.TabIndex = 158;
            this.btnJogLampHome.Text = "H";
            this.btnJogLampHome.UseVisualStyleBackColor = true;
            this.btnJogLampHome.Click += new System.EventHandler(this.btnJogLampHome_Click);
            // 
            // btnJogZHome
            // 
            this.btnJogZHome.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnJogZHome.Location = new System.Drawing.Point(1227, 340);
            this.btnJogZHome.Name = "btnJogZHome";
            this.btnJogZHome.Size = new System.Drawing.Size(27, 25);
            this.btnJogZHome.TabIndex = 157;
            this.btnJogZHome.Text = "H";
            this.btnJogZHome.UseVisualStyleBackColor = true;
            this.btnJogZHome.Click += new System.EventHandler(this.btnJogZHome_Click);
            // 
            // btnJogYHome
            // 
            this.btnJogYHome.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnJogYHome.Location = new System.Drawing.Point(1227, 294);
            this.btnJogYHome.Name = "btnJogYHome";
            this.btnJogYHome.Size = new System.Drawing.Size(27, 25);
            this.btnJogYHome.TabIndex = 156;
            this.btnJogYHome.Text = "H";
            this.btnJogYHome.UseVisualStyleBackColor = true;
            this.btnJogYHome.Click += new System.EventHandler(this.btnJogYHome_Click);
            // 
            // btnJogXHome
            // 
            this.btnJogXHome.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnJogXHome.Location = new System.Drawing.Point(1227, 246);
            this.btnJogXHome.Name = "btnJogXHome";
            this.btnJogXHome.Size = new System.Drawing.Size(27, 25);
            this.btnJogXHome.TabIndex = 155;
            this.btnJogXHome.Text = "H";
            this.btnJogXHome.UseVisualStyleBackColor = true;
            this.btnJogXHome.Click += new System.EventHandler(this.btnJogXHome_Click);
            // 
            // btnJogRHome
            // 
            this.btnJogRHome.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnJogRHome.Location = new System.Drawing.Point(1227, 194);
            this.btnJogRHome.Name = "btnJogRHome";
            this.btnJogRHome.Size = new System.Drawing.Size(27, 25);
            this.btnJogRHome.TabIndex = 154;
            this.btnJogRHome.Text = "H";
            this.btnJogRHome.UseVisualStyleBackColor = true;
            this.btnJogRHome.Click += new System.EventHandler(this.btnJogRHome_Click);
            // 
            // btnJogLampPlus
            // 
            this.btnJogLampPlus.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnJogLampPlus.Location = new System.Drawing.Point(1195, 433);
            this.btnJogLampPlus.Name = "btnJogLampPlus";
            this.btnJogLampPlus.Size = new System.Drawing.Size(27, 25);
            this.btnJogLampPlus.TabIndex = 153;
            this.btnJogLampPlus.Text = ">";
            this.btnJogLampPlus.UseVisualStyleBackColor = true;
            this.btnJogLampPlus.Click += new System.EventHandler(this.btnJogLampPlus_Click);
            // 
            // btnJogLampMinus
            // 
            this.btnJogLampMinus.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnJogLampMinus.Location = new System.Drawing.Point(1086, 435);
            this.btnJogLampMinus.Name = "btnJogLampMinus";
            this.btnJogLampMinus.Size = new System.Drawing.Size(27, 25);
            this.btnJogLampMinus.TabIndex = 152;
            this.btnJogLampMinus.Text = "<";
            this.btnJogLampMinus.UseVisualStyleBackColor = true;
            this.btnJogLampMinus.Click += new System.EventHandler(this.btnJogLampMinus_Click);
            // 
            // txtLampValue
            // 
            this.txtLampValue.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtLampValue.Location = new System.Drawing.Point(1119, 434);
            this.txtLampValue.Name = "txtLampValue";
            this.txtLampValue.Size = new System.Drawing.Size(74, 26);
            this.txtLampValue.TabIndex = 151;
            this.txtLampValue.Text = "50";
            this.txtLampValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.label10.Location = new System.Drawing.Point(1082, 415);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(66, 16);
            this.label10.TabIndex = 150;
            this.label10.Text = "조명값 :";
            // 
            // btnJogRPlus
            // 
            this.btnJogRPlus.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnJogRPlus.Location = new System.Drawing.Point(1197, 194);
            this.btnJogRPlus.Name = "btnJogRPlus";
            this.btnJogRPlus.Size = new System.Drawing.Size(27, 25);
            this.btnJogRPlus.TabIndex = 149;
            this.btnJogRPlus.Text = ">";
            this.btnJogRPlus.UseVisualStyleBackColor = true;
            this.btnJogRPlus.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnJogRPlus_MouseDown);
            this.btnJogRPlus.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnJogRPlus_MouseUp);
            // 
            // btnJogRMinus
            // 
            this.btnJogRMinus.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnJogRMinus.Location = new System.Drawing.Point(1088, 194);
            this.btnJogRMinus.Name = "btnJogRMinus";
            this.btnJogRMinus.Size = new System.Drawing.Size(27, 25);
            this.btnJogRMinus.TabIndex = 148;
            this.btnJogRMinus.Text = "<";
            this.btnJogRMinus.UseVisualStyleBackColor = true;
            this.btnJogRMinus.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnJogRMinus_MouseDown);
            this.btnJogRMinus.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnJogRMinus_MouseUp);
            // 
            // txtRAxisValue
            // 
            this.txtRAxisValue.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtRAxisValue.Location = new System.Drawing.Point(1121, 195);
            this.txtRAxisValue.Name = "txtRAxisValue";
            this.txtRAxisValue.Size = new System.Drawing.Size(74, 26);
            this.txtRAxisValue.TabIndex = 147;
            this.txtRAxisValue.Text = "0.0";
            this.txtRAxisValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtRAxisValue.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtRAxisValue_KeyDown);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label9.ForeColor = System.Drawing.Color.Maroon;
            this.label9.Location = new System.Drawing.Point(1084, 176);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(89, 16);
            this.label9.TabIndex = 146;
            this.label9.Text = "Index회전 :";
            // 
            // btnJogZPlus
            // 
            this.btnJogZPlus.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnJogZPlus.Location = new System.Drawing.Point(1195, 339);
            this.btnJogZPlus.Name = "btnJogZPlus";
            this.btnJogZPlus.Size = new System.Drawing.Size(27, 25);
            this.btnJogZPlus.TabIndex = 145;
            this.btnJogZPlus.Text = ">";
            this.btnJogZPlus.UseVisualStyleBackColor = true;
            this.btnJogZPlus.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnJogZPlus_MouseDown);
            this.btnJogZPlus.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnJogZPlus_MouseUp);
            // 
            // btnJogZMinus
            // 
            this.btnJogZMinus.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnJogZMinus.Location = new System.Drawing.Point(1086, 339);
            this.btnJogZMinus.Name = "btnJogZMinus";
            this.btnJogZMinus.Size = new System.Drawing.Size(27, 25);
            this.btnJogZMinus.TabIndex = 144;
            this.btnJogZMinus.Text = "<";
            this.btnJogZMinus.UseVisualStyleBackColor = true;
            this.btnJogZMinus.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnJogZMinus_MouseDown);
            this.btnJogZMinus.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnJogZMinus_MouseUp);
            // 
            // txtZAxisValue
            // 
            this.txtZAxisValue.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtZAxisValue.Location = new System.Drawing.Point(1119, 340);
            this.txtZAxisValue.Name = "txtZAxisValue";
            this.txtZAxisValue.Size = new System.Drawing.Size(74, 26);
            this.txtZAxisValue.TabIndex = 143;
            this.txtZAxisValue.Text = "0.0";
            this.txtZAxisValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtZAxisValue.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtZAxisValue_KeyDown);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label12.ForeColor = System.Drawing.Color.Blue;
            this.label12.Location = new System.Drawing.Point(1082, 321);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(68, 16);
            this.label12.TabIndex = 142;
            this.label12.Text = "Z-이동 :";
            // 
            // btnJogYPlus
            // 
            this.btnJogYPlus.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnJogYPlus.Location = new System.Drawing.Point(1195, 293);
            this.btnJogYPlus.Name = "btnJogYPlus";
            this.btnJogYPlus.Size = new System.Drawing.Size(27, 25);
            this.btnJogYPlus.TabIndex = 141;
            this.btnJogYPlus.Text = ">";
            this.btnJogYPlus.UseVisualStyleBackColor = true;
            this.btnJogYPlus.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnJogYPlus_MouseDown);
            this.btnJogYPlus.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnJogYPlus_MouseUp);
            // 
            // btnJogYMinus
            // 
            this.btnJogYMinus.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnJogYMinus.Location = new System.Drawing.Point(1086, 293);
            this.btnJogYMinus.Name = "btnJogYMinus";
            this.btnJogYMinus.Size = new System.Drawing.Size(27, 25);
            this.btnJogYMinus.TabIndex = 140;
            this.btnJogYMinus.Text = "<";
            this.btnJogYMinus.UseVisualStyleBackColor = true;
            this.btnJogYMinus.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnJogYMinus_MouseDown);
            this.btnJogYMinus.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnJogYMinus_MouseUp);
            // 
            // txtYAxisValue
            // 
            this.txtYAxisValue.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtYAxisValue.Location = new System.Drawing.Point(1119, 294);
            this.txtYAxisValue.Name = "txtYAxisValue";
            this.txtYAxisValue.Size = new System.Drawing.Size(74, 26);
            this.txtYAxisValue.TabIndex = 139;
            this.txtYAxisValue.Text = "0.0";
            this.txtYAxisValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtYAxisValue.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtYAxisValue_KeyDown);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label13.ForeColor = System.Drawing.Color.Blue;
            this.label13.Location = new System.Drawing.Point(1083, 275);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(68, 16);
            this.label13.TabIndex = 138;
            this.label13.Text = "Y-이동 :";
            // 
            // btnJogXPlus
            // 
            this.btnJogXPlus.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnJogXPlus.Location = new System.Drawing.Point(1196, 247);
            this.btnJogXPlus.Name = "btnJogXPlus";
            this.btnJogXPlus.Size = new System.Drawing.Size(27, 25);
            this.btnJogXPlus.TabIndex = 137;
            this.btnJogXPlus.Text = ">";
            this.btnJogXPlus.UseVisualStyleBackColor = true;
            this.btnJogXPlus.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnJogXPlus_MouseDown);
            this.btnJogXPlus.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnJogXPlus_MouseUp);
            // 
            // btnJogXMinus
            // 
            this.btnJogXMinus.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnJogXMinus.Location = new System.Drawing.Point(1087, 247);
            this.btnJogXMinus.Name = "btnJogXMinus";
            this.btnJogXMinus.Size = new System.Drawing.Size(27, 25);
            this.btnJogXMinus.TabIndex = 136;
            this.btnJogXMinus.Text = "<";
            this.btnJogXMinus.UseVisualStyleBackColor = true;
            this.btnJogXMinus.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnJogXMinus_MouseDown);
            this.btnJogXMinus.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnJogXMinus_MouseUp);
            // 
            // txtXAxisValue
            // 
            this.txtXAxisValue.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtXAxisValue.Location = new System.Drawing.Point(1120, 248);
            this.txtXAxisValue.Name = "txtXAxisValue";
            this.txtXAxisValue.Size = new System.Drawing.Size(74, 26);
            this.txtXAxisValue.TabIndex = 135;
            this.txtXAxisValue.Text = "0.0";
            this.txtXAxisValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtXAxisValue.TextChanged += new System.EventHandler(this.txtXAxisValue_TextChanged);
            this.txtXAxisValue.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtXAxisValue_KeyDown);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label14.ForeColor = System.Drawing.Color.Blue;
            this.label14.Location = new System.Drawing.Point(1084, 229);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(69, 16);
            this.label14.TabIndex = 134;
            this.label14.Text = "X-이동 :";
            // 
            // btDXFUp
            // 
            this.btDXFUp.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btDXFUp.Location = new System.Drawing.Point(1145, 120);
            this.btDXFUp.Name = "btDXFUp";
            this.btDXFUp.Size = new System.Drawing.Size(27, 23);
            this.btDXFUp.TabIndex = 159;
            this.btDXFUp.Text = "^";
            this.btDXFUp.UseVisualStyleBackColor = true;
            this.btDXFUp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btDXFUp_MouseDown);
            this.btDXFUp.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btDXFUp_MouseUp);
            // 
            // btDXFLeft
            // 
            this.btDXFLeft.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btDXFLeft.Location = new System.Drawing.Point(1114, 149);
            this.btDXFLeft.Name = "btDXFLeft";
            this.btDXFLeft.Size = new System.Drawing.Size(27, 23);
            this.btDXFLeft.TabIndex = 160;
            this.btDXFLeft.Text = "<";
            this.btDXFLeft.UseVisualStyleBackColor = true;
            this.btDXFLeft.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btDXFLeft_MouseDown);
            this.btDXFLeft.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btDXFLeft_MouseUp);
            // 
            // btDXFRight
            // 
            this.btDXFRight.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btDXFRight.Location = new System.Drawing.Point(1178, 149);
            this.btDXFRight.Name = "btDXFRight";
            this.btDXFRight.Size = new System.Drawing.Size(27, 23);
            this.btDXFRight.TabIndex = 161;
            this.btDXFRight.Text = ">";
            this.btDXFRight.UseVisualStyleBackColor = true;
            this.btDXFRight.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btDXFRight_MouseDown);
            this.btDXFRight.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btDXFRight_MouseUp);
            // 
            // btDXFDown
            // 
            this.btDXFDown.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btDXFDown.Location = new System.Drawing.Point(1145, 149);
            this.btDXFDown.Name = "btDXFDown";
            this.btDXFDown.Size = new System.Drawing.Size(27, 23);
            this.btDXFDown.TabIndex = 162;
            this.btDXFDown.Text = "v";
            this.btDXFDown.UseVisualStyleBackColor = true;
            this.btDXFDown.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btDXFDown_MouseDown);
            this.btDXFDown.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btDXFDown_MouseUp);
            // 
            // timDXFWork
            // 
            this.timDXFWork.Enabled = true;
            this.timDXFWork.Tick += new System.EventHandler(this.timDXFWork_Tick);
            // 
            // btShotSetSave
            // 
            this.btShotSetSave.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btShotSetSave.Location = new System.Drawing.Point(1075, 824);
            this.btShotSetSave.Name = "btShotSetSave";
            this.btShotSetSave.Size = new System.Drawing.Size(180, 43);
            this.btShotSetSave.TabIndex = 171;
            this.btShotSetSave.Text = "ShotSave";
            this.btShotSetSave.UseVisualStyleBackColor = true;
            this.btShotSetSave.Click += new System.EventHandler(this.btShotSetSave_Click);
            // 
            // cboZoomCamList
            // 
            this.cboZoomCamList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboZoomCamList.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cboZoomCamList.FormattingEnabled = true;
            this.cboZoomCamList.Location = new System.Drawing.Point(75, 7);
            this.cboZoomCamList.Name = "cboZoomCamList";
            this.cboZoomCamList.Size = new System.Drawing.Size(187, 24);
            this.cboZoomCamList.TabIndex = 172;
            this.cboZoomCamList.SelectedIndexChanged += new System.EventHandler(this.cboZoomCamList_SelectedIndexChanged);
            // 
            // timLoadStart
            // 
            this.timLoadStart.Tick += new System.EventHandler(this.timLoadStart_Tick);
            // 
            // btEmergency
            // 
            this.btEmergency.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btEmergency.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btEmergency.Location = new System.Drawing.Point(12, 873);
            this.btEmergency.Name = "btEmergency";
            this.btEmergency.Size = new System.Drawing.Size(125, 43);
            this.btEmergency.TabIndex = 173;
            this.btEmergency.Text = "비상 정지";
            this.btEmergency.UseVisualStyleBackColor = false;
            this.btEmergency.Click += new System.EventHandler(this.btEmergency_Click);
            // 
            // timerAxis
            // 
            this.timerAxis.Interval = 300;
            this.timerAxis.Tick += new System.EventHandler(this.timerAxis_Tick);
            // 
            // picRecoImg
            // 
            this.picRecoImg.BackColor = System.Drawing.Color.Silver;
            this.picRecoImg.Location = new System.Drawing.Point(1227, 563);
            this.picRecoImg.Name = "picRecoImg";
            this.picRecoImg.Size = new System.Drawing.Size(100, 73);
            this.picRecoImg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picRecoImg.TabIndex = 163;
            this.picRecoImg.TabStop = false;
            this.picRecoImg.Visible = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Gray;
            this.pictureBox1.Location = new System.Drawing.Point(1363, 206);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(640, 480);
            this.pictureBox1.TabIndex = 27;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Visible = false;
            // 
            // timWaitDelay
            // 
            this.timWaitDelay.Tick += new System.EventHandler(this.timWaitDelay_Tick);
            // 
            // btCalibrationRun
            // 
            this.btCalibrationRun.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btCalibrationRun.Location = new System.Drawing.Point(247, 873);
            this.btCalibrationRun.Name = "btCalibrationRun";
            this.btCalibrationRun.Size = new System.Drawing.Size(108, 43);
            this.btCalibrationRun.TabIndex = 174;
            this.btCalibrationRun.Text = "Calibration";
            this.btCalibrationRun.UseVisualStyleBackColor = true;
            this.btCalibrationRun.Click += new System.EventHandler(this.btCalibrationRun_Click);
            // 
            // timCalibration
            // 
            this.timCalibration.Tick += new System.EventHandler(this.timCalibration_Tick);
            // 
            // btSetCalibrationValue
            // 
            this.btSetCalibrationValue.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btSetCalibrationValue.Location = new System.Drawing.Point(361, 873);
            this.btSetCalibrationValue.Name = "btSetCalibrationValue";
            this.btSetCalibrationValue.Size = new System.Drawing.Size(180, 43);
            this.btSetCalibrationValue.TabIndex = 175;
            this.btSetCalibrationValue.Text = "Set Center Value";
            this.btSetCalibrationValue.UseVisualStyleBackColor = true;
            this.btSetCalibrationValue.Click += new System.EventHandler(this.btSetCalibrationValue_Click);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label15.Location = new System.Drawing.Point(1078, 467);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(98, 16);
            this.label15.TabIndex = 176;
            this.label15.Text = "고정축보정 :";
            // 
            // txtFixAngleAddGap
            // 
            this.txtFixAngleAddGap.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtFixAngleAddGap.Location = new System.Drawing.Point(1178, 463);
            this.txtFixAngleAddGap.Name = "txtFixAngleAddGap";
            this.txtFixAngleAddGap.Size = new System.Drawing.Size(74, 26);
            this.txtFixAngleAddGap.TabIndex = 177;
            this.txtFixAngleAddGap.Text = "0.0";
            this.txtFixAngleAddGap.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtMoveAngleAddGap
            // 
            this.txtMoveAngleAddGap.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtMoveAngleAddGap.Location = new System.Drawing.Point(1178, 492);
            this.txtMoveAngleAddGap.Name = "txtMoveAngleAddGap";
            this.txtMoveAngleAddGap.Size = new System.Drawing.Size(74, 26);
            this.txtMoveAngleAddGap.TabIndex = 179;
            this.txtMoveAngleAddGap.Text = "0.0";
            this.txtMoveAngleAddGap.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label16.Location = new System.Drawing.Point(1078, 496);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(98, 16);
            this.label16.TabIndex = 178;
            this.label16.Text = "이동축보정 :";
            // 
            // txtBackAngleAddGap
            // 
            this.txtBackAngleAddGap.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtBackAngleAddGap.Location = new System.Drawing.Point(1178, 522);
            this.txtBackAngleAddGap.Name = "txtBackAngleAddGap";
            this.txtBackAngleAddGap.Size = new System.Drawing.Size(74, 26);
            this.txtBackAngleAddGap.TabIndex = 181;
            this.txtBackAngleAddGap.Text = "0.0";
            this.txtBackAngleAddGap.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label17.Location = new System.Drawing.Point(1078, 526);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(98, 16);
            this.label17.TabIndex = 180;
            this.label17.Text = "후방축보정 :";
            // 
            // txtPlangeThickness
            // 
            this.txtPlangeThickness.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtPlangeThickness.Location = new System.Drawing.Point(1178, 584);
            this.txtPlangeThickness.Name = "txtPlangeThickness";
            this.txtPlangeThickness.Size = new System.Drawing.Size(74, 26);
            this.txtPlangeThickness.TabIndex = 183;
            this.txtPlangeThickness.Text = "0.0";
            this.txtPlangeThickness.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label18.Location = new System.Drawing.Point(1078, 588);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(98, 16);
            this.label18.TabIndex = 182;
            this.label18.Text = "플렌지두께 :";
            // 
            // btSetCenterPos
            // 
            this.btSetCenterPos.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btSetCenterPos.Location = new System.Drawing.Point(547, 873);
            this.btSetCenterPos.Name = "btSetCenterPos";
            this.btSetCenterPos.Size = new System.Drawing.Size(180, 43);
            this.btSetCenterPos.TabIndex = 184;
            this.btSetCenterPos.Text = "Save Center Position";
            this.btSetCenterPos.UseVisualStyleBackColor = true;
            this.btSetCenterPos.Click += new System.EventHandler(this.btSetCenterPos_Click);
            // 
            // txtCalcPinDiameter
            // 
            this.txtCalcPinDiameter.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtCalcPinDiameter.Location = new System.Drawing.Point(1178, 674);
            this.txtCalcPinDiameter.Name = "txtCalcPinDiameter";
            this.txtCalcPinDiameter.Size = new System.Drawing.Size(74, 26);
            this.txtCalcPinDiameter.TabIndex = 190;
            this.txtCalcPinDiameter.Text = "0.0";
            this.txtCalcPinDiameter.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label19.Location = new System.Drawing.Point(1078, 678);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(98, 16);
            this.label19.TabIndex = 189;
            this.label19.Text = "측정핀지름 :";
            // 
            // txtPinHoleDiameter
            // 
            this.txtPinHoleDiameter.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtPinHoleDiameter.Location = new System.Drawing.Point(1178, 644);
            this.txtPinHoleDiameter.Name = "txtPinHoleDiameter";
            this.txtPinHoleDiameter.Size = new System.Drawing.Size(74, 26);
            this.txtPinHoleDiameter.TabIndex = 188;
            this.txtPinHoleDiameter.Text = "0.0";
            this.txtPinHoleDiameter.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label20.Location = new System.Drawing.Point(1094, 649);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(82, 16);
            this.label20.TabIndex = 187;
            this.label20.Text = "핀반지름 :";
            // 
            // txtPinHoleDistance
            // 
            this.txtPinHoleDistance.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtPinHoleDistance.Location = new System.Drawing.Point(1178, 614);
            this.txtPinHoleDistance.Name = "txtPinHoleDistance";
            this.txtPinHoleDistance.Size = new System.Drawing.Size(74, 26);
            this.txtPinHoleDistance.TabIndex = 186;
            this.txtPinHoleDistance.Text = "0.0";
            this.txtPinHoleDistance.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label21.Location = new System.Drawing.Point(1094, 618);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(82, 16);
            this.label21.TabIndex = 185;
            this.label21.Text = "핀홀거리 :";
            // 
            // txtTopAngleAddGap
            // 
            this.txtTopAngleAddGap.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtTopAngleAddGap.Location = new System.Drawing.Point(1178, 554);
            this.txtTopAngleAddGap.Name = "txtTopAngleAddGap";
            this.txtTopAngleAddGap.Size = new System.Drawing.Size(74, 26);
            this.txtTopAngleAddGap.TabIndex = 192;
            this.txtTopAngleAddGap.Text = "0.0";
            this.txtTopAngleAddGap.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label22.Location = new System.Drawing.Point(1078, 558);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(98, 16);
            this.label22.TabIndex = 191;
            this.label22.Text = "상방축보정 :";
            // 
            // txtCalcPinHeight
            // 
            this.txtCalcPinHeight.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtCalcPinHeight.Location = new System.Drawing.Point(1178, 705);
            this.txtCalcPinHeight.Name = "txtCalcPinHeight";
            this.txtCalcPinHeight.Size = new System.Drawing.Size(74, 26);
            this.txtCalcPinHeight.TabIndex = 194;
            this.txtCalcPinHeight.Text = "0.0";
            this.txtCalcPinHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label23.Location = new System.Drawing.Point(1078, 709);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(98, 16);
            this.label23.TabIndex = 193;
            this.label23.Text = "측정핀높이 :";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label24.Location = new System.Drawing.Point(1078, 738);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(98, 16);
            this.label24.TabIndex = 195;
            this.label24.Text = "상방홀방향 :";
            // 
            // cboTopHolePos
            // 
            this.cboTopHolePos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTopHolePos.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cboTopHolePos.FormattingEnabled = true;
            this.cboTopHolePos.Location = new System.Drawing.Point(1178, 735);
            this.cboTopHolePos.Name = "cboTopHolePos";
            this.cboTopHolePos.Size = new System.Drawing.Size(74, 24);
            this.cboTopHolePos.TabIndex = 196;
            // 
            // radioBtnSSlow
            // 
            this.radioBtnSSlow.AutoSize = true;
            this.radioBtnSSlow.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.radioBtnSSlow.Location = new System.Drawing.Point(644, 848);
            this.radioBtnSSlow.Name = "radioBtnSSlow";
            this.radioBtnSSlow.Size = new System.Drawing.Size(79, 20);
            this.radioBtnSSlow.TabIndex = 238;
            this.radioBtnSSlow.TabStop = true;
            this.radioBtnSSlow.Tag = "0";
            this.radioBtnSSlow.Text = "S-Slow";
            this.radioBtnSSlow.UseVisualStyleBackColor = true;
            this.radioBtnSSlow.CheckedChanged += new System.EventHandler(this.radioBtn_CheckedChanged);
            // 
            // radioBtnFast
            // 
            this.radioBtnFast.AutoSize = true;
            this.radioBtnFast.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.radioBtnFast.Location = new System.Drawing.Point(878, 848);
            this.radioBtnFast.Name = "radioBtnFast";
            this.radioBtnFast.Size = new System.Drawing.Size(58, 20);
            this.radioBtnFast.TabIndex = 237;
            this.radioBtnFast.TabStop = true;
            this.radioBtnFast.Tag = "3";
            this.radioBtnFast.Text = "Fast";
            this.radioBtnFast.UseVisualStyleBackColor = true;
            this.radioBtnFast.CheckedChanged += new System.EventHandler(this.radioBtn_CheckedChanged);
            // 
            // radioBtnMidium
            // 
            this.radioBtnMidium.AutoSize = true;
            this.radioBtnMidium.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.radioBtnMidium.Location = new System.Drawing.Point(796, 848);
            this.radioBtnMidium.Name = "radioBtnMidium";
            this.radioBtnMidium.Size = new System.Drawing.Size(76, 20);
            this.radioBtnMidium.TabIndex = 236;
            this.radioBtnMidium.TabStop = true;
            this.radioBtnMidium.Tag = "2";
            this.radioBtnMidium.Text = "Midium";
            this.radioBtnMidium.UseVisualStyleBackColor = true;
            this.radioBtnMidium.CheckedChanged += new System.EventHandler(this.radioBtn_CheckedChanged);
            // 
            // radioBtnSlow
            // 
            this.radioBtnSlow.AutoSize = true;
            this.radioBtnSlow.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.radioBtnSlow.Location = new System.Drawing.Point(729, 848);
            this.radioBtnSlow.Name = "radioBtnSlow";
            this.radioBtnSlow.Size = new System.Drawing.Size(61, 20);
            this.radioBtnSlow.TabIndex = 235;
            this.radioBtnSlow.TabStop = true;
            this.radioBtnSlow.Tag = "1";
            this.radioBtnSlow.Text = "Slow";
            this.radioBtnSlow.UseVisualStyleBackColor = true;
            this.radioBtnSlow.CheckedChanged += new System.EventHandler(this.radioBtn_CheckedChanged);
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label25.Location = new System.Drawing.Point(551, 850);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(87, 16);
            this.label25.TabIndex = 234;
            this.label25.Text = "속도 설정 :";
            // 
            // btnJogMRHome
            // 
            this.btnJogMRHome.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnJogMRHome.Location = new System.Drawing.Point(1227, 387);
            this.btnJogMRHome.Name = "btnJogMRHome";
            this.btnJogMRHome.Size = new System.Drawing.Size(27, 25);
            this.btnJogMRHome.TabIndex = 243;
            this.btnJogMRHome.Text = "H";
            this.btnJogMRHome.UseVisualStyleBackColor = true;
            this.btnJogMRHome.Click += new System.EventHandler(this.btnJogMRHome_Click);
            // 
            // btnJogMRPlus
            // 
            this.btnJogMRPlus.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnJogMRPlus.Location = new System.Drawing.Point(1195, 386);
            this.btnJogMRPlus.Name = "btnJogMRPlus";
            this.btnJogMRPlus.Size = new System.Drawing.Size(27, 25);
            this.btnJogMRPlus.TabIndex = 242;
            this.btnJogMRPlus.Text = ">";
            this.btnJogMRPlus.UseVisualStyleBackColor = true;
            this.btnJogMRPlus.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnJogMRPlus_MouseDown);
            this.btnJogMRPlus.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnJogMRPlus_MouseUp);
            // 
            // btnJogMRMinus
            // 
            this.btnJogMRMinus.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnJogMRMinus.Location = new System.Drawing.Point(1086, 386);
            this.btnJogMRMinus.Name = "btnJogMRMinus";
            this.btnJogMRMinus.Size = new System.Drawing.Size(27, 25);
            this.btnJogMRMinus.TabIndex = 241;
            this.btnJogMRMinus.Text = "<";
            this.btnJogMRMinus.UseVisualStyleBackColor = true;
            this.btnJogMRMinus.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnJogMRMinus_MouseDown);
            this.btnJogMRMinus.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnJogMRMinus_MouseUp);
            // 
            // txtMoveRValue
            // 
            this.txtMoveRValue.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtMoveRValue.Location = new System.Drawing.Point(1119, 387);
            this.txtMoveRValue.Name = "txtMoveRValue";
            this.txtMoveRValue.Size = new System.Drawing.Size(74, 26);
            this.txtMoveRValue.TabIndex = 240;
            this.txtMoveRValue.Text = "0.0";
            this.txtMoveRValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtMoveRValue.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtMoveRValue_KeyDown);
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label26.ForeColor = System.Drawing.Color.Fuchsia;
            this.label26.Location = new System.Drawing.Point(1082, 368);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(106, 16);
            this.label26.TabIndex = 239;
            this.label26.Text = "이동축-이동 :";
            // 
            // chkGuideLine
            // 
            this.chkGuideLine.AutoSize = true;
            this.chkGuideLine.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.chkGuideLine.Location = new System.Drawing.Point(297, 849);
            this.chkGuideLine.Name = "chkGuideLine";
            this.chkGuideLine.Size = new System.Drawing.Size(107, 20);
            this.chkGuideLine.TabIndex = 244;
            this.chkGuideLine.Text = "가이드라인";
            this.chkGuideLine.UseVisualStyleBackColor = true;
            this.chkGuideLine.CheckedChanged += new System.EventHandler(this.chkGuideLine_CheckedChanged);
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label27.Location = new System.Drawing.Point(1078, 768);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(98, 16);
            this.label27.TabIndex = 246;
            this.label27.Text = "상방홀위치 :";
            // 
            // txtTopDirHoleDistance
            // 
            this.txtTopDirHoleDistance.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtTopDirHoleDistance.Location = new System.Drawing.Point(1178, 764);
            this.txtTopDirHoleDistance.Name = "txtTopDirHoleDistance";
            this.txtTopDirHoleDistance.Size = new System.Drawing.Size(74, 26);
            this.txtTopDirHoleDistance.TabIndex = 245;
            this.txtTopDirHoleDistance.Text = "0.0";
            this.txtTopDirHoleDistance.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // chkDXFView
            // 
            this.chkDXFView.AutoSize = true;
            this.chkDXFView.Checked = true;
            this.chkDXFView.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDXFView.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.chkDXFView.Location = new System.Drawing.Point(411, 849);
            this.chkDXFView.Name = "chkDXFView";
            this.chkDXFView.Size = new System.Drawing.Size(121, 20);
            this.chkDXFView.TabIndex = 247;
            this.chkDXFView.Text = "DXF도면보기";
            this.chkDXFView.UseVisualStyleBackColor = true;
            this.chkDXFView.CheckedChanged += new System.EventHandler(this.chkDXFView_CheckedChanged);
            // 
            // btDXFCamFitting
            // 
            this.btDXFCamFitting.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btDXFCamFitting.Location = new System.Drawing.Point(943, 873);
            this.btDXFCamFitting.Name = "btDXFCamFitting";
            this.btDXFCamFitting.Size = new System.Drawing.Size(126, 43);
            this.btDXFCamFitting.TabIndex = 248;
            this.btDXFCamFitting.Text = "DXF Cam Fitting";
            this.btDXFCamFitting.UseVisualStyleBackColor = true;
            this.btDXFCamFitting.Click += new System.EventHandler(this.btDXFCamFitting_Click);
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label28.Location = new System.Drawing.Point(1078, 799);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(98, 16);
            this.label28.TabIndex = 250;
            this.label28.Text = "샤프트길이 :";
            // 
            // txtShaftLength
            // 
            this.txtShaftLength.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtShaftLength.Location = new System.Drawing.Point(1178, 795);
            this.txtShaftLength.Name = "txtShaftLength";
            this.txtShaftLength.Size = new System.Drawing.Size(74, 26);
            this.txtShaftLength.TabIndex = 249;
            this.txtShaftLength.Text = "0.0";
            this.txtShaftLength.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // timLevelAlign
            // 
            this.timLevelAlign.Tick += new System.EventHandler(this.timLevelAlign_Tick);
            // 
            // btLevelCalc
            // 
            this.btLevelCalc.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btLevelCalc.Location = new System.Drawing.Point(143, 874);
            this.btLevelCalc.Name = "btLevelCalc";
            this.btLevelCalc.Size = new System.Drawing.Size(98, 43);
            this.btLevelCalc.TabIndex = 251;
            this.btLevelCalc.Text = "단차계산";
            this.btLevelCalc.UseVisualStyleBackColor = true;
            this.btLevelCalc.Click += new System.EventHandler(this.btLevelCalc_Click);
            // 
            // cboDXFType
            // 
            this.cboDXFType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDXFType.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cboDXFType.FormattingEnabled = true;
            this.cboDXFType.Location = new System.Drawing.Point(757, 817);
            this.cboDXFType.Name = "cboDXFType";
            this.cboDXFType.Size = new System.Drawing.Size(178, 24);
            this.cboDXFType.TabIndex = 253;
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label29.Location = new System.Drawing.Point(666, 820);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(85, 16);
            this.label29.TabIndex = 252;
            this.label29.Text = "도면Type :";
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.BackColor = System.Drawing.Color.Transparent;
            this.label30.ForeColor = System.Drawing.Color.Red;
            this.label30.Location = new System.Drawing.Point(852, 15);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(44, 12);
            this.label30.TabIndex = 24;
            this.label30.Text = "label30";
            // 
            // frmReco
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 926);
            this.Controls.Add(this.cboDXFType);
            this.Controls.Add(this.label29);
            this.Controls.Add(this.btLevelCalc);
            this.Controls.Add(this.label28);
            this.Controls.Add(this.txtShaftLength);
            this.Controls.Add(this.btDXFCamFitting);
            this.Controls.Add(this.chkDXFView);
            this.Controls.Add(this.label27);
            this.Controls.Add(this.txtTopDirHoleDistance);
            this.Controls.Add(this.chkGuideLine);
            this.Controls.Add(this.btnJogMRHome);
            this.Controls.Add(this.btnJogMRPlus);
            this.Controls.Add(this.btnJogMRMinus);
            this.Controls.Add(this.txtMoveRValue);
            this.Controls.Add(this.label26);
            this.Controls.Add(this.radioBtnSSlow);
            this.Controls.Add(this.radioBtnFast);
            this.Controls.Add(this.radioBtnMidium);
            this.Controls.Add(this.radioBtnSlow);
            this.Controls.Add(this.label25);
            this.Controls.Add(this.cboTopHolePos);
            this.Controls.Add(this.label24);
            this.Controls.Add(this.txtCalcPinHeight);
            this.Controls.Add(this.label23);
            this.Controls.Add(this.txtTopAngleAddGap);
            this.Controls.Add(this.label22);
            this.Controls.Add(this.txtCalcPinDiameter);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.txtPinHoleDiameter);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.txtPinHoleDistance);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.btSetCenterPos);
            this.Controls.Add(this.txtPlangeThickness);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.txtBackAngleAddGap);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.txtMoveAngleAddGap);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.txtFixAngleAddGap);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.btSetCalibrationValue);
            this.Controls.Add(this.btCalibrationRun);
            this.Controls.Add(this.btEmergency);
            this.Controls.Add(this.cboZoomCamList);
            this.Controls.Add(this.btShotSetSave);
            this.Controls.Add(this.UIDList);
            this.Controls.Add(this.picRecoImg);
            this.Controls.Add(this.btDXFDown);
            this.Controls.Add(this.btDXFRight);
            this.Controls.Add(this.btDXFLeft);
            this.Controls.Add(this.btDXFUp);
            this.Controls.Add(this.btnJogLampHome);
            this.Controls.Add(this.btnJogZHome);
            this.Controls.Add(this.btnJogYHome);
            this.Controls.Add(this.btnJogXHome);
            this.Controls.Add(this.btnJogRHome);
            this.Controls.Add(this.btnJogLampPlus);
            this.Controls.Add(this.btnJogLampMinus);
            this.Controls.Add(this.txtLampValue);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.btnJogRPlus);
            this.Controls.Add(this.btnJogRMinus);
            this.Controls.Add(this.txtRAxisValue);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.btnJogZPlus);
            this.Controls.Add(this.btnJogZMinus);
            this.Controls.Add(this.txtZAxisValue);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.btnJogYPlus);
            this.Controls.Add(this.btnJogYMinus);
            this.Controls.Add(this.txtYAxisValue);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.btnJogXPlus);
            this.Controls.Add(this.btnJogXMinus);
            this.Controls.Add(this.txtXAxisValue);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.btDetailShot);
            this.Controls.Add(this.btFullShot);
            this.Controls.Add(this.BusReset);
            this.Controls.Add(this.cboLampList);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.cboMoveList);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboIndexList);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btLoopStop);
            this.Controls.Add(this.btLoopStart);
            this.Controls.Add(this.btShotSave);
            this.Controls.Add(this.btDown);
            this.Controls.Add(this.btUp);
            this.Controls.Add(this.button18);
            this.Controls.Add(this.button17);
            this.Controls.Add(this.button20);
            this.Controls.Add(this.button19);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.trackBar2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btZoomOut);
            this.Controls.Add(this.btZoomIn);
            this.Controls.Add(this.btDXFOpen);
            this.Controls.Add(this.DispSize);
            this.Controls.Add(this.BStop);
            this.Controls.Add(this.BStart);
            this.Controls.Add(this.PanelPicture);
            this.Controls.Add(this.CameraClose);
            this.Controls.Add(this.CameraOpen);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmReco";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "인식편집화면";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmReco_FormClosing);
            this.Load += new System.EventHandler(this.frmReco_Load);
            this.PanelPicture.ResumeLayout(false);
            this.PanelPicture.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AutoScaleBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RealScaleBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picRecoImg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.ComponentModel.BackgroundWorker BusResetWorker;
        private System.Windows.Forms.ComboBox UIDList;
        private System.Windows.Forms.Button CameraClose;
        private System.Windows.Forms.Button CameraOpen;
        private System.ComponentModel.BackgroundWorker FpsUpdate;
        private System.ComponentModel.BackgroundWorker DisplayUpdate;
        private System.Windows.Forms.Panel PanelPicture;
        private System.Windows.Forms.PictureBox AutoScaleBox;
        private System.Windows.Forms.CheckBox DispSize;
        private System.Windows.Forms.Button BStop;
        private System.Windows.Forms.Button BStart;
        private System.Windows.Forms.Button btDXFOpen;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btZoomIn;
        private System.Windows.Forms.Button btZoomOut;
        private System.Windows.Forms.PictureBox RealScaleBox;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button button20;
        private System.Windows.Forms.Button button19;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TrackBar trackBar2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Button button18;
        private System.Windows.Forms.Button button17;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Timer timer3;
        private System.Windows.Forms.Button btUp;
        private System.Windows.Forms.Button btDown;
        private System.Windows.Forms.Timer timer4;
        private System.Windows.Forms.Button btShotSave;
        private System.Windows.Forms.Timer timWork;
        private System.Windows.Forms.Button btLoopStart;
        private System.Windows.Forms.Button btLoopStop;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ComboBox cboLampList;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cboMoveList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboIndexList;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label BusReset;
        private System.Windows.Forms.Button btFullShot;
        private System.Windows.Forms.Button btDetailShot;
        private System.Windows.Forms.Button btnJogLampHome;
        private System.Windows.Forms.Button btnJogZHome;
        private System.Windows.Forms.Button btnJogYHome;
        private System.Windows.Forms.Button btnJogXHome;
        private System.Windows.Forms.Button btnJogRHome;
        private System.Windows.Forms.Button btnJogLampPlus;
        private System.Windows.Forms.Button btnJogLampMinus;
        private System.Windows.Forms.TextBox txtLampValue;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btnJogRPlus;
        private System.Windows.Forms.Button btnJogRMinus;
        private System.Windows.Forms.TextBox txtRAxisValue;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnJogZPlus;
        private System.Windows.Forms.Button btnJogZMinus;
        private System.Windows.Forms.TextBox txtZAxisValue;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button btnJogYPlus;
        private System.Windows.Forms.Button btnJogYMinus;
        private System.Windows.Forms.TextBox txtYAxisValue;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button btnJogXPlus;
        private System.Windows.Forms.Button btnJogXMinus;
        private System.Windows.Forms.TextBox txtXAxisValue;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button btDXFUp;
        private System.Windows.Forms.Button btDXFLeft;
        private System.Windows.Forms.Button btDXFRight;
        private System.Windows.Forms.Button btDXFDown;
        private System.Windows.Forms.PictureBox picRecoImg;
        private System.Windows.Forms.Timer timDXFWork;
        private System.Windows.Forms.Button btShotSetSave;
        private System.Windows.Forms.ComboBox cboZoomCamList;
        private System.Windows.Forms.Timer timLoadStart;
        private System.Windows.Forms.Button btEmergency;
        private System.Windows.Forms.Timer timerAxis;
        private System.Windows.Forms.Timer timWaitDelay;
        private System.Windows.Forms.Button btCalibrationRun;
        private System.Windows.Forms.Timer timCalibration;
        private System.Windows.Forms.Button btSetCalibrationValue;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtFixAngleAddGap;
        private System.Windows.Forms.TextBox txtMoveAngleAddGap;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox txtBackAngleAddGap;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox txtPlangeThickness;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Button btSetCenterPos;
        private System.Windows.Forms.TextBox txtCalcPinDiameter;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox txtPinHoleDiameter;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox txtPinHoleDistance;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.TextBox txtTopAngleAddGap;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.TextBox txtCalcPinHeight;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.ComboBox cboTopHolePos;
        private System.Windows.Forms.RadioButton radioBtnSSlow;
        private System.Windows.Forms.RadioButton radioBtnFast;
        private System.Windows.Forms.RadioButton radioBtnMidium;
        private System.Windows.Forms.RadioButton radioBtnSlow;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Button btnJogMRHome;
        private System.Windows.Forms.Button btnJogMRPlus;
        private System.Windows.Forms.Button btnJogMRMinus;
        private System.Windows.Forms.TextBox txtMoveRValue;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.CheckBox chkGuideLine;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.TextBox txtTopDirHoleDistance;
        private System.Windows.Forms.CheckBox chkDXFView;
        private System.Windows.Forms.Button btDXFCamFitting;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.TextBox txtShaftLength;
        private System.Windows.Forms.Timer timLevelAlign;
        private System.Windows.Forms.Button btLevelCalc;
        private System.Windows.Forms.ComboBox cboDXFType;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.Label label30;
    }
}

