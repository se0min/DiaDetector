namespace AutoAssembler
{
    partial class MainFrame
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainFrame));
            this.AssembleSteppictureBox = new System.Windows.Forms.PictureBox();
            this.AssembleProgressBar = new System.Windows.Forms.ProgressBar();
            this.btCountReset = new System.Windows.Forms.Button();
            this.btnSetting = new System.Windows.Forms.Button();
            this.btnEditor = new System.Windows.Forms.Button();
            this.btnTestEnd = new System.Windows.Forms.Button();
            this.btnTestStart = new System.Windows.Forms.Button();
            this.txtTestTime = new System.Windows.Forms.TextBox();
            this.txtSerialNumber = new System.Windows.Forms.TextBox();
            this.txtModelName = new System.Windows.Forms.TextBox();
            this.lstViewTestList = new System.Windows.Forms.ListView();
            this.ModelPictureBox = new System.Windows.Forms.PictureBox();
            this.lstViewResList = new System.Windows.Forms.ListView();
            this.timeTestProc = new System.Windows.Forms.Timer(this.components);
            this.timLoopCam = new System.Windows.Forms.Timer(this.components);
            this.timCommPort = new System.Windows.Forms.Timer(this.components);
            this.timDelayWait = new System.Windows.Forms.Timer(this.components);
            this.timTestRunCheck = new System.Windows.Forms.Timer(this.components);
            this.BusResetWorker = new System.ComponentModel.BackgroundWorker();
            this.FpsUpdate = new System.ComponentModel.BackgroundWorker();
            this.DisplayUpdate = new System.ComponentModel.BackgroundWorker();
            this.txtOKCount = new System.Windows.Forms.Label();
            this.txtNGCount = new System.Windows.Forms.Label();
            this.txtTotalCount = new System.Windows.Forms.Label();
            this.NgOkPictureBox = new System.Windows.Forms.PictureBox();
            this.btnModelOpen = new System.Windows.Forms.Button();
            this.AutoScaleBox = new System.Windows.Forms.PictureBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.txtCoordinates = new System.Windows.Forms.TextBox();
            this.picRecoImg = new System.Windows.Forms.PictureBox();
            this.Status_pictureBox = new System.Windows.Forms.PictureBox();
            this.btnCalibration = new System.Windows.Forms.Button();
            this.btnTest = new System.Windows.Forms.Button();
            this.btnJog = new System.Windows.Forms.Button();
            this.timerAlarm = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.AssembleSteppictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ModelPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NgOkPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AutoScaleBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picRecoImg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Status_pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // AssembleSteppictureBox
            // 
            this.AssembleSteppictureBox.BackColor = System.Drawing.Color.White;
            this.AssembleSteppictureBox.Location = new System.Drawing.Point(25, 873);
            this.AssembleSteppictureBox.Name = "AssembleSteppictureBox";
            this.AssembleSteppictureBox.Size = new System.Drawing.Size(543, 105);
            this.AssembleSteppictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.AssembleSteppictureBox.TabIndex = 1;
            this.AssembleSteppictureBox.TabStop = false;
            // 
            // AssembleProgressBar
            // 
            this.AssembleProgressBar.Location = new System.Drawing.Point(26, 781);
            this.AssembleProgressBar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.AssembleProgressBar.Name = "AssembleProgressBar";
            this.AssembleProgressBar.Size = new System.Drawing.Size(812, 48);
            this.AssembleProgressBar.TabIndex = 0;
            // 
            // btCountReset
            // 
            this.btCountReset.BackColor = System.Drawing.Color.Transparent;
            this.btCountReset.BackgroundImage = global::AutoAssembler.Properties.Resources.btnCountReset1;
            this.btCountReset.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btCountReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btCountReset.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btCountReset.Location = new System.Drawing.Point(1067, 190);
            this.btCountReset.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btCountReset.Name = "btCountReset";
            this.btCountReset.Size = new System.Drawing.Size(130, 33);
            this.btCountReset.TabIndex = 31;
            this.btCountReset.UseVisualStyleBackColor = false;
            this.btCountReset.Click += new System.EventHandler(this.btCountReset_Click);
            // 
            // btnSetting
            // 
            this.btnSetting.BackColor = System.Drawing.Color.Transparent;
            this.btnSetting.BackgroundImage = global::AutoAssembler.Properties.Resources.btnTestSetting;
            this.btnSetting.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnSetting.FlatAppearance.BorderSize = 0;
            this.btnSetting.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSetting.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.btnSetting.Location = new System.Drawing.Point(1102, 256);
            this.btnSetting.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSetting.Name = "btnSetting";
            this.btnSetting.Size = new System.Drawing.Size(68, 33);
            this.btnSetting.TabIndex = 27;
            this.btnSetting.UseVisualStyleBackColor = false;
            this.btnSetting.Click += new System.EventHandler(this.btnSetting_Click);
            // 
            // btnEditor
            // 
            this.btnEditor.BackColor = System.Drawing.Color.Transparent;
            this.btnEditor.BackgroundImage = global::AutoAssembler.Properties.Resources.btnTestEditor;
            this.btnEditor.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnEditor.FlatAppearance.BorderSize = 0;
            this.btnEditor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEditor.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.btnEditor.Location = new System.Drawing.Point(1034, 256);
            this.btnEditor.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnEditor.Name = "btnEditor";
            this.btnEditor.Size = new System.Drawing.Size(69, 33);
            this.btnEditor.TabIndex = 26;
            this.btnEditor.UseVisualStyleBackColor = false;
            this.btnEditor.Click += new System.EventHandler(this.btnEditor_Click);
            // 
            // btnTestEnd
            // 
            this.btnTestEnd.BackgroundImage = global::AutoAssembler.Properties.Resources.btnTestStop;
            this.btnTestEnd.FlatAppearance.BorderSize = 0;
            this.btnTestEnd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTestEnd.Location = new System.Drawing.Point(965, 256);
            this.btnTestEnd.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnTestEnd.Name = "btnTestEnd";
            this.btnTestEnd.Size = new System.Drawing.Size(69, 33);
            this.btnTestEnd.TabIndex = 16;
            this.btnTestEnd.UseVisualStyleBackColor = true;
            this.btnTestEnd.Click += new System.EventHandler(this.btnTestEnd_Click);
            // 
            // btnTestStart
            // 
            this.btnTestStart.BackgroundImage = global::AutoAssembler.Properties.Resources.btnTestStart;
            this.btnTestStart.FlatAppearance.BorderSize = 0;
            this.btnTestStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTestStart.Location = new System.Drawing.Point(894, 256);
            this.btnTestStart.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnTestStart.Name = "btnTestStart";
            this.btnTestStart.Size = new System.Drawing.Size(71, 33);
            this.btnTestStart.TabIndex = 15;
            this.btnTestStart.UseVisualStyleBackColor = true;
            this.btnTestStart.Click += new System.EventHandler(this.btnTestStart_Click);
            // 
            // txtTestTime
            // 
            this.txtTestTime.BackColor = System.Drawing.Color.White;
            this.txtTestTime.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtTestTime.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtTestTime.ForeColor = System.Drawing.Color.Black;
            this.txtTestTime.Location = new System.Drawing.Point(684, 57);
            this.txtTestTime.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtTestTime.Name = "txtTestTime";
            this.txtTestTime.ReadOnly = true;
            this.txtTestTime.Size = new System.Drawing.Size(157, 19);
            this.txtTestTime.TabIndex = 33;
            this.txtTestTime.Text = "00:00:00";
            this.txtTestTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtSerialNumber
            // 
            this.txtSerialNumber.BackColor = System.Drawing.Color.White;
            this.txtSerialNumber.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSerialNumber.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtSerialNumber.Location = new System.Drawing.Point(436, 57);
            this.txtSerialNumber.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtSerialNumber.Name = "txtSerialNumber";
            this.txtSerialNumber.ReadOnly = true;
            this.txtSerialNumber.Size = new System.Drawing.Size(159, 19);
            this.txtSerialNumber.TabIndex = 32;
            this.txtSerialNumber.Text = "0001160101000000";
            this.txtSerialNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtModelName
            // 
            this.txtModelName.BackColor = System.Drawing.Color.White;
            this.txtModelName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtModelName.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtModelName.ForeColor = System.Drawing.Color.Black;
            this.txtModelName.Location = new System.Drawing.Point(997, 102);
            this.txtModelName.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtModelName.Name = "txtModelName";
            this.txtModelName.ReadOnly = true;
            this.txtModelName.Size = new System.Drawing.Size(196, 19);
            this.txtModelName.TabIndex = 13;
            this.txtModelName.Text = "Model Name";
            this.txtModelName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lstViewTestList
            // 
            this.lstViewTestList.FullRowSelect = true;
            this.lstViewTestList.Location = new System.Drawing.Point(868, 399);
            this.lstViewTestList.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lstViewTestList.MultiSelect = false;
            this.lstViewTestList.Name = "lstViewTestList";
            this.lstViewTestList.Size = new System.Drawing.Size(399, 301);
            this.lstViewTestList.TabIndex = 25;
            this.lstViewTestList.UseCompatibleStateImageBehavior = false;
            this.lstViewTestList.View = System.Windows.Forms.View.Details;
            this.lstViewTestList.Leave += new System.EventHandler(this.lstViewTestList_Leave);
            // 
            // ModelPictureBox
            // 
            this.ModelPictureBox.BackColor = System.Drawing.Color.White;
            this.ModelPictureBox.Location = new System.Drawing.Point(583, 874);
            this.ModelPictureBox.Name = "ModelPictureBox";
            this.ModelPictureBox.Size = new System.Drawing.Size(256, 103);
            this.ModelPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ModelPictureBox.TabIndex = 0;
            this.ModelPictureBox.TabStop = false;
            // 
            // lstViewResList
            // 
            this.lstViewResList.FullRowSelect = true;
            this.lstViewResList.Location = new System.Drawing.Point(868, 735);
            this.lstViewResList.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lstViewResList.MultiSelect = false;
            this.lstViewResList.Name = "lstViewResList";
            this.lstViewResList.Size = new System.Drawing.Size(399, 102);
            this.lstViewResList.TabIndex = 25;
            this.lstViewResList.UseCompatibleStateImageBehavior = false;
            this.lstViewResList.View = System.Windows.Forms.View.Details;
            // 
            // timeTestProc
            // 
            this.timeTestProc.Interval = 500;
            this.timeTestProc.Tick += new System.EventHandler(this.timeTestProc_Tick);
            // 
            // timLoopCam
            // 
            this.timLoopCam.Enabled = true;
            this.timLoopCam.Interval = 500;
            this.timLoopCam.Tick += new System.EventHandler(this.timLoopCam_Tick);
            // 
            // timCommPort
            // 
            this.timCommPort.Enabled = true;
            this.timCommPort.Interval = 300;
            this.timCommPort.Tick += new System.EventHandler(this.timCommPort_Tick);
            // 
            // timDelayWait
            // 
            this.timDelayWait.Tick += new System.EventHandler(this.timDelayWait_Tick);
            // 
            // timTestRunCheck
            // 
            this.timTestRunCheck.Tick += new System.EventHandler(this.timTestRunCheck_Tick);
            // 
            // DisplayUpdate
            // 
            this.DisplayUpdate.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.DisplayUpdate_RunWorkerCompleted);
            // 
            // txtOKCount
            // 
            this.txtOKCount.BackColor = System.Drawing.Color.Transparent;
            this.txtOKCount.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtOKCount.ForeColor = System.Drawing.Color.White;
            this.txtOKCount.Location = new System.Drawing.Point(966, 142);
            this.txtOKCount.Name = "txtOKCount";
            this.txtOKCount.Size = new System.Drawing.Size(84, 27);
            this.txtOKCount.TabIndex = 34;
            this.txtOKCount.Text = "0";
            this.txtOKCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtNGCount
            // 
            this.txtNGCount.BackColor = System.Drawing.Color.Transparent;
            this.txtNGCount.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtNGCount.ForeColor = System.Drawing.Color.White;
            this.txtNGCount.Location = new System.Drawing.Point(966, 174);
            this.txtNGCount.Name = "txtNGCount";
            this.txtNGCount.Size = new System.Drawing.Size(84, 27);
            this.txtNGCount.TabIndex = 35;
            this.txtNGCount.Text = "0";
            this.txtNGCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtTotalCount
            // 
            this.txtTotalCount.BackColor = System.Drawing.Color.Transparent;
            this.txtTotalCount.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtTotalCount.ForeColor = System.Drawing.Color.White;
            this.txtTotalCount.Location = new System.Drawing.Point(966, 207);
            this.txtTotalCount.Name = "txtTotalCount";
            this.txtTotalCount.Size = new System.Drawing.Size(84, 27);
            this.txtTotalCount.TabIndex = 36;
            this.txtTotalCount.Text = "0";
            this.txtTotalCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // NgOkPictureBox
            // 
            this.NgOkPictureBox.BackColor = System.Drawing.Color.White;
            this.NgOkPictureBox.Location = new System.Drawing.Point(851, 861);
            this.NgOkPictureBox.Name = "NgOkPictureBox";
            this.NgOkPictureBox.Size = new System.Drawing.Size(424, 115);
            this.NgOkPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.NgOkPictureBox.TabIndex = 18;
            this.NgOkPictureBox.TabStop = false;
            // 
            // btnModelOpen
            // 
            this.btnModelOpen.BackgroundImage = global::AutoAssembler.Properties.Resources.btnModelSelect;
            this.btnModelOpen.FlatAppearance.BorderSize = 0;
            this.btnModelOpen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnModelOpen.Location = new System.Drawing.Point(1202, 95);
            this.btnModelOpen.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnModelOpen.Name = "btnModelOpen";
            this.btnModelOpen.Size = new System.Drawing.Size(34, 34);
            this.btnModelOpen.TabIndex = 14;
            this.btnModelOpen.UseVisualStyleBackColor = true;
            this.btnModelOpen.Click += new System.EventHandler(this.btnModelOpen_Click);
            // 
            // AutoScaleBox
            // 
            this.AutoScaleBox.Location = new System.Drawing.Point(23, 85);
            this.AutoScaleBox.Name = "AutoScaleBox";
            this.AutoScaleBox.Size = new System.Drawing.Size(816, 683);
            this.AutoScaleBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.AutoScaleBox.TabIndex = 0;
            this.AutoScaleBox.TabStop = false;
            // 
            // btnClose
            // 
            this.btnClose.BackgroundImage = global::AutoAssembler.Properties.Resources.btnClose;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Location = new System.Drawing.Point(1234, 1);
            this.btnClose.Margin = new System.Windows.Forms.Padding(0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(46, 38);
            this.btnClose.TabIndex = 3;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // txtCoordinates
            // 
            this.txtCoordinates.BackColor = System.Drawing.Color.White;
            this.txtCoordinates.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtCoordinates.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtCoordinates.ForeColor = System.Drawing.Color.Black;
            this.txtCoordinates.Location = new System.Drawing.Point(1003, 342);
            this.txtCoordinates.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtCoordinates.Name = "txtCoordinates";
            this.txtCoordinates.ReadOnly = true;
            this.txtCoordinates.Size = new System.Drawing.Size(243, 19);
            this.txtCoordinates.TabIndex = 37;
            this.txtCoordinates.Text = "X=0.00, Y=0.00, Z=0.00";
            this.txtCoordinates.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtCoordinates.Visible = false;
            // 
            // picRecoImg
            // 
            this.picRecoImg.Location = new System.Drawing.Point(215, 16);
            this.picRecoImg.Name = "picRecoImg";
            this.picRecoImg.Size = new System.Drawing.Size(114, 60);
            this.picRecoImg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picRecoImg.TabIndex = 39;
            this.picRecoImg.TabStop = false;
            this.picRecoImg.Visible = false;
            // 
            // Status_pictureBox
            // 
            this.Status_pictureBox.Location = new System.Drawing.Point(23, 85);
            this.Status_pictureBox.Name = "Status_pictureBox";
            this.Status_pictureBox.Size = new System.Drawing.Size(816, 683);
            this.Status_pictureBox.TabIndex = 40;
            this.Status_pictureBox.TabStop = false;
            // 
            // btnCalibration
            // 
            this.btnCalibration.BackColor = System.Drawing.Color.Transparent;
            this.btnCalibration.BackgroundImage = global::AutoAssembler.Properties.Resources.Calibration_btn;
            this.btnCalibration.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnCalibration.FlatAppearance.BorderSize = 0;
            this.btnCalibration.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCalibration.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.btnCalibration.Location = new System.Drawing.Point(1169, 256);
            this.btnCalibration.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCalibration.Name = "btnCalibration";
            this.btnCalibration.Size = new System.Drawing.Size(68, 33);
            this.btnCalibration.TabIndex = 41;
            this.btnCalibration.UseVisualStyleBackColor = false;
            this.btnCalibration.Click += new System.EventHandler(this.btnCalibration_Click);
            // 
            // btnTest
            // 
            this.btnTest.Enabled = false;
            this.btnTest.Location = new System.Drawing.Point(1202, 142);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(72, 81);
            this.btnTest.TabIndex = 42;
            this.btnTest.Text = "Test";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Visible = false;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // btnJog
            // 
            this.btnJog.BackColor = System.Drawing.Color.Transparent;
            this.btnJog.BackgroundImage = global::AutoAssembler.Properties.Resources.btnJog;
            this.btnJog.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnJog.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnJog.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnJog.Location = new System.Drawing.Point(1067, 143);
            this.btnJog.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnJog.Name = "btnJog";
            this.btnJog.Size = new System.Drawing.Size(130, 33);
            this.btnJog.TabIndex = 43;
            this.btnJog.UseVisualStyleBackColor = false;
            this.btnJog.Click += new System.EventHandler(this.btnJog_Click);
            // 
            // timerAlarm
            // 
            this.timerAlarm.Enabled = true;
            this.timerAlarm.Interval = 200;
            this.timerAlarm.Tick += new System.EventHandler(this.timerAlarm_Tick);
            // 
            // MainFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.BackgroundImage = global::AutoAssembler.Properties.Resources.MainFrame7;
            this.ClientSize = new System.Drawing.Size(1276, 984);
            this.Controls.Add(this.btCountReset);
            this.Controls.Add(this.btnJog);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.btnCalibration);
            this.Controls.Add(this.Status_pictureBox);
            this.Controls.Add(this.picRecoImg);
            this.Controls.Add(this.txtCoordinates);
            this.Controls.Add(this.btnSetting);
            this.Controls.Add(this.btnEditor);
            this.Controls.Add(this.AssembleProgressBar);
            this.Controls.Add(this.btnTestEnd);
            this.Controls.Add(this.AssembleSteppictureBox);
            this.Controls.Add(this.btnTestStart);
            this.Controls.Add(this.ModelPictureBox);
            this.Controls.Add(this.NgOkPictureBox);
            this.Controls.Add(this.txtTotalCount);
            this.Controls.Add(this.txtNGCount);
            this.Controls.Add(this.txtOKCount);
            this.Controls.Add(this.lstViewResList);
            this.Controls.Add(this.lstViewTestList);
            this.Controls.Add(this.btnModelOpen);
            this.Controls.Add(this.txtTestTime);
            this.Controls.Add(this.AutoScaleBox);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.txtSerialNumber);
            this.Controls.Add(this.txtModelName);
            this.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainFrame";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Auto Assembler";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainFrame_FormClosing);
            this.Load += new System.EventHandler(this.MainFrame_Load);
            ((System.ComponentModel.ISupportInitialize)(this.AssembleSteppictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ModelPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NgOkPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AutoScaleBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picRecoImg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Status_pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ProgressBar AssembleProgressBar;
        private System.Windows.Forms.ListView lstViewTestList;
        private System.Windows.Forms.Button btnModelOpen;
        private System.Windows.Forms.TextBox txtModelName;
        private System.Windows.Forms.Button btnTestEnd;
        private System.Windows.Forms.Button btnTestStart;
        private System.Windows.Forms.Button btnSetting;
        private System.Windows.Forms.Button btnEditor;
        private System.Windows.Forms.Button btCountReset;
        private System.Windows.Forms.ListView lstViewResList;
        private System.Windows.Forms.TextBox txtTestTime;
        private System.Windows.Forms.TextBox txtSerialNumber;
        private System.Windows.Forms.Timer timeTestProc;
        private System.Windows.Forms.Timer timLoopCam;
        private System.Windows.Forms.Timer timCommPort;
        private System.Windows.Forms.Timer timDelayWait;
        private System.Windows.Forms.Timer timTestRunCheck;
        private System.Windows.Forms.PictureBox AutoScaleBox;
        private System.ComponentModel.BackgroundWorker BusResetWorker;
        private System.ComponentModel.BackgroundWorker FpsUpdate;
        private System.ComponentModel.BackgroundWorker DisplayUpdate;
        private System.Windows.Forms.PictureBox ModelPictureBox;
        private System.Windows.Forms.PictureBox NgOkPictureBox;
        private System.Windows.Forms.PictureBox AssembleSteppictureBox;
        private System.Windows.Forms.Label txtOKCount;
        private System.Windows.Forms.Label txtNGCount;
        private System.Windows.Forms.Label txtTotalCount;
        private System.Windows.Forms.TextBox txtCoordinates;
        private System.Windows.Forms.PictureBox picRecoImg;
        private System.Windows.Forms.PictureBox Status_pictureBox;
        private System.Windows.Forms.Button btnCalibration;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Button btnJog;
        private System.Windows.Forms.Timer timerAlarm;
    }
}

