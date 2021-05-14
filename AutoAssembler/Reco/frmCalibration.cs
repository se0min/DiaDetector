using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using XCCamDotNet;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Drawing.Imaging;

using DXFImportReco;

//using Navitar;
using System.IO;

using AutoAssembler.Data;
using AutoAssembler.Drivers;
using AutoAssembler.VisionLibrary;
using AutoAssembler.Utilities;

using System.Threading;


namespace AutoAssembler
{
    public partial class frmCalibration : Form
    {



        #region Data ...

        private static char[] SaveDiv = new char[] { '|' };
        private static char[] SaveDataDiv = new char[] { '#' };
        private static char[] SaveDataDtlDiv = new char[] { ',' };

        private const int LAMP_INC_GAP = 10;

        clsLamp LampComm = new clsLamp();

        bool ScreenFlipFlag = false;

        bool ExitRecoFormFlag = false;
        bool DelayLoopExitFlag;

        public RecoStructInfo RecoSetData;
        public string CfgSaveFolderPath;
        public string CfgSaveHeadFileName;
        public string CfgSaveFolderPath_FileName;
        public const string CfgSaveDataFileName = "RecoCfg.dat";
        public const string SetCalibrationDataFileName = "CalibValue.dat";
        public bool RunCamPlayFlag = false;
        /*
        [DllImport("navserAPI.dll")]
        public static extern  int SerConnectionConnect(int port,ushort ProductID);
        [DllImport("navserAPI.dll")]
        public static extern int SerConnectionDisconnectAll();

        private const ushort DEF_BRIGHTLIGHT = 0x4000;
        private const ushort DEF_MOTOR_CONTROLLER = 0x4001;
        */

        private const int FULLSHOTTYPE = 0;
        private const int DETAILSHOTTYPE = 1;
        private int FullDetailShotFlag;
        private ShotDetailInfo[] ShotData = new ShotDetailInfo[2];
        private const string RecoDXFFileName = "RecoDXF.dxf";

        private const uint REG_USER_TARGET_1 = 0x10;//       '	motor 1 current position
        private const uint REG_USER_TARGET_2 = 0x20;//     '   motor 2 current position

        private const uint REG_USER_INCREMENT_1 = 0x11;//     '   motor 2 current position
        private const uint REG_USER_INCREMENT_2 = 0x21;//     '   motor 2 current position

        private const uint REG_USER_CURRENT_1 = 0x12;//       '	motor 1 current position
        private const uint REG_USER_CURRENT_2 = 0x22;//     '   motor 2 current position

        private const uint REG_SETUP_LIMIT_1 = 0x1C;//       '	motor 1 current position
        private const uint REG_SETUP_LIMIT_2 = 0x2C;//     '   motor 2 current position

        private const uint REG_USER_STATUS_1 = 0x14;
        private const uint REG_USER_STATUS_2 = 0x24;

        private GCHandle ParamCB;
        private XCCAM.SystemFunc SystemCB = new XCCAM.SystemFunc(SystemCallback);
        private UInt64[] CUID;
        private int CamTotalNum;


        private XCCAM XCCam;
        private XCCAM_IMAGEINFO ImageInfo;
        //private XCCAM_FEATUREINFO NowFeature = new XCCAM_FEATUREINFO();
        private XCCAM.ImageFunc ImageCB = new XCCAM.ImageFunc(ImageCallback);
        //private GCHandle ParamCB;
        private IntPtr RGBData;
        private Stopwatch DispExec = new Stopwatch();
        private Stopwatch Frame = new Stopwatch();
        private Int64 FrameCount;
        private double Fps;
        private Bitmap RGBImage;
        private Int32 Dislay_FPS;
        private Boolean Scale_Flag, Disp_Flag;

        protected CADImage FCADImage;// = new CADImage();
        float FScale = 4.0f;
        public Point Base;
        float FS_W, FS_H, FS_W_Base, FS_H_Base;
        bool StartCalcFlag;
        private string CurDXFFileNameStr = "";

        public const int SHAPE_NONE = 0;
        public const int SHAPE_CIRCLE = 1;
        public const int SHAPE_LINE = 2;
        DXFLayerInfo[] DXFObjList;
        private const int DXFOBJLIST_MAX = 100;
        double IndexFindRoAngle = 0;


        Bitmap canvas;
        static int Pic_Width, Pic_Height;

        private static RGBDatInfo[,] RGBDXFData;
        private static bool RGBListFlag;
        private static bool RGBListActionFlag;

        private static bool GetProcRunFlag;
        Image TempImg;

        private ControllerLegacy NavitarCtrl;

        private int ZoomFactNum, FocusFactNum;
        private int CurZoomFactNum, CurFocusFactNum;
        private int CurRealZoomFactNum, CurRealFocusFactNum;

        private int ZoomIn_Min, ZoomIn_Max;
        private int Focus_Min, Focus_Max;
        private int FocusPlusMinus = 0;

        private int DXFForm_Width, DXFForm_Height;

        private int LpWk_LoopCount, LpWk_LoopCountMax;
        private int LpWk_Zoom, LpWk_Focus;
        private int LpWk_RUN_STATUS;
        private int LpWk_ZeroCount;
        private int LpWk_RunTimerCount;
        private bool LpWk_RunFlag = false;

        private const int WORKSTART = 1;
        private const int MOTIONMOVE = 2;
        private const int GOFULLSHOT = 3;
        private const int FULLRECOPROC = 4;
        private const int INDEXROTATE_FULL = 5;
        private const int FULLRECOPROC2 = 6;
        private const int INDEXROTATE_FULL2 = 7;
        private const int DETAILMOTIONMOVE = 8;
        private const int GODETAILSHOT = 9;
        private const int DETAILRECOPROC = 10;
        private const int INDEXROTATE_DETAIL = 11;
        private const int DETAILRECOPROC2 = 12;
        private const int INDEXROTATE_DETAIL2 = 13;
        private const int WORKEND = 99;

        private const int DXF_EDIT_NONE = 0;
        private const int DXF_EDIT_UP = 1;
        private const int DXF_EDIT_DOWN = 2;
        private const int DXF_EDIT_LEFT = 3;
        private const int DXF_EDIT_RIGHT = 4;
        private const int DXF_EDIT_ZOOMIN = 5;
        private const int DXF_EDIT_ZOOMOUT = 6;

        private bool ViewDXFCadFlag = true;

        private bool WaitDelayRunFlag = false;

        private int DXF_Edit_Cur_Sts = DXF_EDIT_NONE;

        Image RecoBlankImg;
        Image RecoWorkImg;
        public clsRecoProcess RecoCls;

        private const int CALIB_NONE = 0;
        private const int CALIB_START = 1;
        private const int CALIB_ZOOMFOCUS = 2;
        private const int CALIB_RECOPROCESS = 3;
        private const int CALIB_MOV_POSITION = 4;
        private const int CALIB_RECOPROCESS2 = 5;
        private const int CALIB_MOV_POSITION2 = 6;

        private const int CALIB_END = 99;

        private int Calib_Run_Sts = CALIB_NONE;
        DoubleDataPosInfo CameraMoveXYPos;
        DoubleDataPosInfo RealCalibrationData_A;
        DoubleDataPosInfo RealCalibrationData_B;

        private int[] CamOpticalGap_X;
        private int[] CamOpticalGap_Y;
        private int CamOpticalGapIndex;

        #endregion Data ...


        public frmCalibration()
        {
            InitializeComponent();

            CamTotalNum = 0;
            XCCAM.SetStructVersion(XCCamDotNet.Constants.LIBRARY_STRUCT_VERSION);
            ParamCB = GCHandle.Alloc(this);
            XCCAM.SetCallBack(GCHandle.ToIntPtr(ParamCB), SystemCB);
            SetBounds(10, 10, 0, 0, BoundsSpecified.X | BoundsSpecified.Y);

            ZoomFactNum = 0;
            FocusFactNum = 0;
            CurZoomFactNum = 0;
            CurFocusFactNum = 0;
            CurRealZoomFactNum = 0;
            CurRealFocusFactNum = 0;

            DXFForm_Width = 640;
            DXFForm_Height = 480;


            CamOpticalGapIndex = 0;
            CamOpticalGap_X = new int[4];
            CamOpticalGap_Y = new int[4];


            CamOpticalGap_X[0] = 455;
            CamOpticalGap_Y[0] = 396;
            CamOpticalGap_X[1] = 455;
            CamOpticalGap_Y[1] = 374;
            CamOpticalGap_X[2] = 457;
            CamOpticalGap_Y[2] = 381;
            CamOpticalGap_X[3] = 457;
            CamOpticalGap_Y[3] = 381;


            Base.X = -500;
            Base.Y = 500;

        }

        private void frmCalibration_Load(object sender, EventArgs e)
        {
            LampComm.Open(DeviceManager.LightingComPort);

            RealCalibrationData_A.ddp_X = 66.92;
            RealCalibrationData_A.ddp_Y = 543.66;
            RealCalibrationData_B.ddp_X = 59.24;
            RealCalibrationData_B.ddp_Y = 536.13;

            ViewDXFCadFlag = true;
            FullDetailShotFlag = FULLSHOTTYPE;
            StartCalcFlag = false;

            picRecoImg.Top = PanelPicture.Top;
            picRecoImg.Left = PanelPicture.Left;
            picRecoImg.Width = PanelPicture.Width;
            picRecoImg.Height = PanelPicture.Height;

            ShotData[FULLSHOTTYPE].ShotType = FULLSHOTTYPE;
            ShotData[FULLSHOTTYPE].ZoomNum = -1;
            ShotData[FULLSHOTTYPE].FocusNum = -1;
            ShotData[DETAILSHOTTYPE].ShotType = DETAILSHOTTYPE;
            ShotData[DETAILSHOTTYPE].ZoomNum = -1;
            ShotData[DETAILSHOTTYPE].FocusNum = -1;

            DXFObjList = new DXFLayerInfo[DXFOBJLIST_MAX];

            btDetailShot.ForeColor = Color.Black;
            btDetailShot.Font = new Font(btDetailShot.Font, FontStyle.Regular);

            btFullShot.ForeColor = Color.Red;
            btFullShot.Font = new Font(btFullShot.Font, FontStyle.Bold);

            cboZoomCamList.Items.Clear();

            for (int i = 0; i < 4; i++)
            {
                cboZoomCamList.Items.Add(DataManager.CameraSettingInfoList[i].Name.ToString());
            }



            // 홍동성 => 상단 콤보 박스 ...
            // ----------
            Initialize();
            // ----------



            // 설정 파일 읽기 ... => 읽은 후 대화상자 정보를 업데이트 해야 함.
            // ----------
            CreateMakeFolderFunc(CfgSaveFolderPath);

            if (CfgSaveHeadFileName.Length > 0)
            {
                CfgSaveFolderPath_FileName = CfgSaveFolderPath + @"\" + CfgSaveHeadFileName + @".dat";

                LoadCfgFiles(CfgSaveFolderPath_FileName);


                // 설정치 화면 복원 ...
                // ----------

                if (RecoSetData.RecoCamIndex > -1)
                {
                    cboZoomCamList.SelectedIndex = RecoSetData.RecoCamIndex;
                }
                ShotData[FULLSHOTTYPE].ZoomNum = RecoSetData.FullShot_ZoomNum;
                ShotData[FULLSHOTTYPE].FocusNum = RecoSetData.FullShot_FocusNum;
                ShotData[DETAILSHOTTYPE].ZoomNum = RecoSetData.DetailShot_ZoomNum;
                ShotData[DETAILSHOTTYPE].FocusNum = RecoSetData.DetailShot_FocusNum;
                // ...
                // ----------

            }
            else
            {
                CfgSaveFolderPath_FileName = "";
            }

            LoadCalibSetFiles(SetCalibrationDataFileName);


            // ...
            // ----------
            UInt64[] WorkUID = new UInt64[0];

            RGBListFlag = false;
            RGBListActionFlag = false;
            GetProcRunFlag = false;

            CameraList_Relist(ref WorkUID);

            CUID = (UInt64[])WorkUID.Clone();
            //Array.Resize(ref CView, WorkUID.Length);
            CamTotalNum = WorkUID.Length;

            UIDList.SelectedIndexChanged -= new System.EventHandler(UIDList_SelectedIndexChanged);
            UIDList.SelectedIndex = 0;
            UIDList.SelectedIndexChanged += new System.EventHandler(UIDList_SelectedIndexChanged);
            CameraClose.Enabled = false;

            DXFForm_Width = 1024;
            DXFForm_Height = 768;

            pictureBox1.Width = DXFForm_Width;
            pictureBox1.Height = DXFForm_Height;

            Pic_Width = pictureBox1.Width;
            Pic_Height = pictureBox1.Height;

            canvas = new Bitmap(Pic_Width, Pic_Height);

            timer1.Enabled = true;

            // ZOOM & FOCUS ...
            // ----------
            //NavitarCtrl = new ControllerLegacy("COM1");
            //NavitarCtrl.Connect();
        }

        private void frmCalibration_FormClosing(object sender, FormClosingEventArgs e)
        {
                        // 홍동성 ...
            // ----------
            MultiMotion.StopAll();

            timerAxis.Enabled = false;
            // ----------


            for (int i = 0; i < 8; i++)
            {
                LampComm.OFFLamp(i);
            }


            LampComm.Close();
            LampComm = null;

            ExitRecoFormFlag = true;
            if (BStop.Enabled == true)
            {
                if (!XCCam.ImageStop())
                {
                    //MessageBox.Show("Image Stop Error");
                }

                XCCam.SetImageCallBack();

                if (!XCCam.ResourceRelease())
                {
                    //MessageBox.Show("Resource Release Error");
                }

                DispExec.Stop();
                Frame.Stop();
                CameraOneClose();

                RunCamPlayFlag = false;
            }


            if (NavitarCtrl != null)
            {
                //if (NavitarCtrl.ProductID > 0)
                if (NavitarCtrl.Connected == true)
                {
                    NavitarCtrl.Stop();
                    NavitarCtrl.Disconnect();
                    NavitarCtrl.Dispose();
                    NavitarCtrl = null;
                }
            }
        }



        private void CreateMakeFolderFunc(string CrMakeFolderStr)
        {
            DirectoryInfo ChkDirFld = new DirectoryInfo(CrMakeFolderStr);
            if (ChkDirFld.Exists == false)
            {
                ChkDirFld.Create();
            }
        }


        private void CameraList_Relist(ref UInt64[] WorkUID)
        {
            XCCAM_CAMERAINFO[] CameraInfo;
            int idx;
            String str;


            XCCAM.GetList(out CameraInfo);
            Array.Resize(ref WorkUID, CameraInfo.Length);

            UIDList.Items.Clear();

            if (CameraInfo.Length != 0)
            {
                for (idx = 0; idx < CameraInfo.Length; idx++)
                {
                    WorkUID[idx] = CameraInfo[idx].UID;
                    str = String.Format("0x{0:X}", WorkUID[idx]);
                    UIDList.Items.Add(str);
                }
            }
            else
            {
                UIDList.Items.Add("Not found Camera");
                UIDList.Enabled = false;
                CameraOpen.Enabled = false;
            }
        }

        private static void SystemCallback(STATUS_SYSTEMCODE SystemStatus, IntPtr Context)
        {
            GCHandle param = GCHandle.FromIntPtr(Context);
            frmCalibration CameraListRef = (frmCalibration)param.Target;

            switch (SystemStatus)
            {
                case STATUS_SYSTEMCODE.STATUSXCCAM_BUSRESET: // Processing of bus reset
                    if (!CameraListRef.BusResetWorker.IsBusy)
                        CameraListRef.BusResetWorker.RunWorkerAsync();
                    break;

                case STATUS_SYSTEMCODE.STATUSXCCAM_POWERUP: // Processing of PowerUP
                    break;
            }
        }

        private void BusResetWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //SetBusResetText();
        }

        /*
        delegate void BusResetTextCallback();

        private void SetBusResetText()
        {
            if (BusReset.InvokeRequired)
            {
                BusResetTextCallback d = new BusResetTextCallback(SetBusResetText);
                Invoke(d, new object[] { });
            }
            else
                BusReset.Enabled = !BusReset.Enabled;
        }
        */

        private void UIDList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idx;

            idx = UIDList.SelectedIndex;
            if (CamTotalNum > idx)
            {
                CameraOpen.Enabled = true;
                CameraClose.Enabled = false;

            }
            else
            {
                CameraOpen.Enabled = false;
                CameraClose.Enabled = true;
            }

            CameraOpen_Click(this,new EventArgs());
        }

        private void CameraOpen_Click(object sender, EventArgs e)
        {
            /*
            CView[UIDList.SelectedIndex] = new CameraView(CUID[UIDList.SelectedIndex]);
            if (!CView[UIDList.SelectedIndex].IsDisposed)
            {
                CView[UIDList.SelectedIndex].Show();
                CameraOpen.Enabled = false;
                CameraClose.Enabled = true;
            }
            else
                CView[UIDList.SelectedIndex] = null;
            */

            if (CameraOneOpen(CUID[UIDList.SelectedIndex]) == true)
            {
                CameraOpen.Enabled = false;
                CameraClose.Enabled = true;
                BStart.Enabled = true;
                BStop.Enabled = false;
                btShotSave.Enabled = false;
                btLoopStart.Enabled = false;
                //btLoopStop.Enabled = false;
                BStart_Click(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("카메라를 정상적으로 열지 못하였습니다.");
            }
        }

        private void CameraClose_Click(object sender, EventArgs e)
        {
            /*
            CView[UIDList.SelectedIndex].CameraDispose();
            CView[UIDList.SelectedIndex] = null;
            CameraOpen.Enabled = true;
            CameraClose.Enabled = false;
            */
            CameraOneClose();
            CameraOpen.Enabled = true;
            CameraClose.Enabled = false;
            BStart.Enabled = false;
            BStop.Enabled = false;
            btShotSave.Enabled = false;
            btLoopStart.Enabled = false;
            //btLoopStop.Enabled = false;

        }

        private bool CameraOneOpen(UInt64 UID)
        {
            bool SucCamOpenFlag = true;
            XCCAM_CAMERAINFO CameraInfo;

            XCCam = new XCCAM(UID);

            if (!XCCam.IsXCCam_Ready())
            {
                MessageBox.Show("Open Camera Error");
                XCCam.Dispose();
                SucCamOpenFlag = false;
                return SucCamOpenFlag;
            }
            XCCam.CameraInfo(out CameraInfo);

            //ParamCB = GCHandle.Alloc(this);
            return SucCamOpenFlag;
        }

        private void CameraOneClose()
        {
            if (XCCam.IsXCCam_Ready())
            {
                XCCam.Dispose();
            }
        }

        private static void ImageCallback(XCCAM XCCam, IntPtr pInBuf, UInt32 Length, UInt32 iWidth, UInt32 iHeight, XCCAM_IMAGEDATAINFO Info, IntPtr Context)
        {
            GCHandle param = GCHandle.FromIntPtr(Context);
            frmCalibration VRef = (frmCalibration)param.Target;
            Int64 TickCount;

            VRef.FrameCount++;
            TickCount = VRef.Frame.ElapsedTicks;
            if (TickCount >= Stopwatch.Frequency)
            {
                VRef.Fps = VRef.FrameCount;
                VRef.FrameCount = 0;
                VRef.Frame.Restart();
                VRef.Fps *= Stopwatch.Frequency;
                VRef.Fps /= TickCount;
                if (!VRef.FpsUpdate.IsBusy)
                    VRef.FpsUpdate.RunWorkerAsync();
            }

            TickCount = VRef.DispExec.ElapsedTicks;
            if (TickCount >= (Stopwatch.Frequency / VRef.Dislay_FPS) || !VRef.Disp_Flag)
            {
                VRef.Disp_Flag = true;
                VRef.DispExec.Restart();
                XCCam.BufferConvExec(pInBuf, VRef.RGBData);
                if (!VRef.DisplayUpdate.IsBusy)
                    VRef.DisplayUpdate.RunWorkerAsync();
            }
        }

        private void DisplayUpdate_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            RGBImage = new Bitmap((int)ImageInfo.Width, (int)ImageInfo.Height, (int)ImageInfo.Width * 4, PixelFormat.Format32bppRgb, RGBData);
            if (DispSize.Checked)
                SetDisplayImageReal();
            else
                SetDisplayImageAuto();

        }

        delegate void SetDisplayImageAutoCallback();

        private void SetDisplayImageAuto()
        {
            if (AutoScaleBox.InvokeRequired)
            {
                SetDisplayImageAutoCallback d = new SetDisplayImageAutoCallback(SetDisplayImageAuto);
                if (ExitRecoFormFlag==false)
                {
                    try
                    {
                        Invoke(d, new object[] { });

                    }
                    catch(Exception e)
                    {

                    }
                }
                
            }
            else
            {
                //AutoScaleBox.Image = RGBImage;
                //AutoScaleBox.Image = GetTotalImg((int)ImageInfo.Width, (int)ImageInfo.Height);
                //Image RefCalcImg = RGBImage;
                if (GetProcRunFlag == false)
                {
                    GetProcRunFlag = true;
                    Bitmap objShotBitmap = new Bitmap((Image)RGBImage, (int)PanelPicture.Width - 10, (int)PanelPicture.Height - 10);
                    if (ScreenFlipFlag==true)
                    {
                        objShotBitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    }
                    RealScaleBox.Image = objShotBitmap;
                    //RecoBlankImg = ReSizeCurBitmap(RGBImage, (int)PanelPicture.Width - 10, (int)PanelPicture.Height - 10);//objShotBitmap;
                    RecoBlankImg = ReSizeCurBitmap(objShotBitmap, (int)PanelPicture.Width - 10, (int)PanelPicture.Height - 10);//objShotBitmap;
                    DrawDXFImageCalc(RealScaleBox.Width, RealScaleBox.Height, false);
                    if (ViewDXFCadFlag == true)
                    {
                        DrawDXFImage2((Bitmap)RealScaleBox.Image);
                    }
                    //RealScaleBox.Image = GetTotalImg3((Bitmap)RGBImage, (int)PanelPicture.Width - 10, (int)PanelPicture.Height - 10);
                    //RealScaleBox.Image = GetTotalImg2((Bitmap)RGBImage, (int)ImageInfo.Width, (int)ImageInfo.Height);
                    if (Scale_Flag)
                    {
                        Scale_Flag = false;
                        if (DispSize.Checked)
                        {
                            PanelPicture.Visible = true;
                            RealScaleBox.Visible = false;
                        }
                        else
                        {
                            PanelPicture.Visible = true;
                            RealScaleBox.Visible = false;
                        }
                    }
                    GetProcRunFlag = false;
                }
            }
        }

        delegate void SetDisplayImageRealCallback();

        private void SetDisplayImageReal()
        {
            if (RealScaleBox.InvokeRequired)
            {
                SetDisplayImageRealCallback d = new SetDisplayImageRealCallback(SetDisplayImageReal);

                Invoke(d, new object[] { });
            }
            else
            {
                //RealScaleBox.Image = RGBImage;
                //RealScaleBox.Image = GetTotalImg((int)ImageInfo.Width, (int)ImageInfo.Height);
                //Image RefCalcImg = RGBImage;
                if (GetProcRunFlag == false)
                {
                    GetProcRunFlag = true;
                    Bitmap objShotBitmap = new Bitmap((Image)RGBImage, (int)ImageInfo.Width, (int)ImageInfo.Height);
                    if (ScreenFlipFlag == true)
                    {
                        objShotBitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    }
                    RealScaleBox.Image = objShotBitmap;
                    ////RealScaleBox.Image = GetTotalImg3((Bitmap)RGBImage, (int)PanelPicture.Width - 10, (int)PanelPicture.Height - 10);
                    //RealScaleBox.Image = GetTotalImg2((Bitmap)RGBImage, (int)ImageInfo.Width, (int)ImageInfo.Height);
                    RecoBlankImg = RealScaleBox.Image;
                    DrawDXFImageCalc(RealScaleBox.Width, RealScaleBox.Height,false);
                    if (ViewDXFCadFlag==true)
                    {
                        DrawDXFImage2((Bitmap)RealScaleBox.Image);
                    }
                    if (Scale_Flag)
                    {
                        Scale_Flag = false;
                        if (DispSize.Checked)
                        {
                            PanelPicture.Visible = true;
                            RealScaleBox.Visible = false;
                            //RealScaleBox.SizeMode = PictureBoxSizeMode.AutoSize;
                        }
                        else
                        {
                            /*
                            PanelPicture.Visible = false;
                            RealScaleBox.Visible = true;
                            */
                            PanelPicture.Visible = true;
                            RealScaleBox.Visible = false;
                            //RealScaleBox.SizeMode = PictureBoxSizeMode.Zoom;
                            RealScaleBox.Width = PanelPicture.Width;
                            RealScaleBox.Height = PanelPicture.Height;
                        }
                    }
                    GetProcRunFlag = false;
                }
            }
        }

        private void BStart_Click(object sender, EventArgs e)
        {
            Dislay_FPS = 10;//30;//10-100;

            //XCCam.SetValue("width", "640");
            //XCCam.SetValue("height", "480");

            if (!XCCam.ResourceAlloc())
            {
                MessageBox.Show("Resource Alloc Error");
                return;
            }
            
            if (!XCCam.GetImageInfo(out ImageInfo))
            {
                XCCam.ResourceRelease();
                MessageBox.Show("Get ImageInfo Error");
                return;
            }
            
            if (DispSize.Checked)
            {
                PanelPicture.Visible = true;
                AutoScaleBox.Visible = false;
            }
            else
            {
                /*
                PanelPicture.Visible = false;
                AutoScaleBox.Visible = true;
                */
                PanelPicture.Visible = true;
                AutoScaleBox.Visible = false;

            }
            
            RGBImage = new Bitmap((int)ImageInfo.Width, (int)ImageInfo.Height, PixelFormat.Format32bppRgb);
            
            if (RGBData != null)
                Marshal.FreeCoTaskMem(RGBData);
            RGBData = Marshal.AllocCoTaskMem((int)ImageInfo.RGBLength);

            DispExec.Reset();
            Frame.Reset();

            XCCam.SetImageCallBack(GCHandle.ToIntPtr(ParamCB), ImageCB, 5, false);

            FrameCount = 0;
            DispExec.Start();
            Frame.Start();

            if (!XCCam.ImageStart())
            {
                MessageBox.Show("Image Start Error");
                XCCam.SetImageCallBack();
                DispExec.Stop();
                Frame.Stop();
                XCCam.ResourceRelease();
                return;
            }

            Disp_Flag = false;
            BStart.Enabled = false;
            BStop.Enabled = true;
            btShotSave.Enabled = true;
            btLoopStart.Enabled = true;
            //btLoopStop.Enabled = true;
            //OnFeatureUpdate();
            RunCamPlayFlag = true;
        }

        private void btDXFOpen_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "CAD Files|*.dxf";
            openFileDialog1.Title = "Select a CAD File";
            if (openFileDialog1.ShowDialog(this) != DialogResult.OK) return;
            if (openFileDialog1.FileName != null)
            {
                StartCalcFlag = true;
                FCADImage = new CADImage();
                Base.X = 1;
                Base.Y = 1;
                FScale = 1.0f;
                //timer1.Interval = 600;
                //timer1.Enabled = true;
                FCADImage.LoadFromFile(openFileDialog1.FileName);
                CurDXFFileNameStr = openFileDialog1.FileName;
            }
        }

        private void BStop_Click(object sender, EventArgs e)
        {
            if (!XCCam.ImageStop())
                MessageBox.Show("Image Stop Error");

            XCCam.SetImageCallBack();

            if (!XCCam.ResourceRelease())
                MessageBox.Show("Resource Release Error");

            DispExec.Stop();
            Frame.Stop();

            BStart.Enabled = true;
            BStop.Enabled = false;
            btShotSave.Enabled = false;
            btLoopStart.Enabled = false;
            //btLoopStop.Enabled = false;
            //OnFeatureUpdate();
        }

        static Bitmap GetTotalImg(int width, int height)
        {
            unsafe
            {
                Bitmap newBitmap = new Bitmap(width, height);

                BitmapData newData = newBitmap.LockBits(
                   new Rectangle(0, 0, width, height),
                   ImageLockMode.WriteOnly, PixelFormat.Format32bppRgb);

                int pixelSize = 4;
                int x_pixelSize, x_pixelSize2, x_pixelSize3, x_pixelSize4;
                int y_AddNum;



                for (int y = 0; y < height; y++)
                {
                    byte* nRow = (byte*)newData.Scan0 + (y * newData.Stride);
                    y_AddNum = y * width * 4;

                    for (int x = 0; x < width/2; x++)
                    {

                        x_pixelSize = x * pixelSize;
                        x_pixelSize2 = x_pixelSize + 1;
                        x_pixelSize3 = x_pixelSize + 2;
                        x_pixelSize4 = x_pixelSize + 3;

                        nRow[x_pixelSize] = 255;
                        nRow[x_pixelSize2] = 0;
                        nRow[x_pixelSize3] = 255;
                        nRow[x_pixelSize3] = 255;
                    }
                }

                newBitmap.UnlockBits(newData);

                return newBitmap;
            }
        }


        static Bitmap ReSizeCurBitmap(Bitmap OriginalImg, int width, int height)
        {
            unsafe
            {
                Bitmap newBitmap = new Bitmap(OriginalImg, width, height);

                return newBitmap;
            }
        }
        static Bitmap GetTotalImg2(Bitmap OriginalImg,int width, int height)
        {
            unsafe
            {
                //int width, height;

                //width = OriginalImg.Width;
                //height = OriginalImg.Height;

                bool RunRGBTempFlag = false;

                Bitmap newBitmap = new Bitmap(width, height);

                //Bitmap objShotBitmap = new Bitmap(CamShotImgSrc, BaseImg_W, BaseImg_H);

                BitmapData newData2 = OriginalImg.LockBits(
                   new Rectangle(0, 0, width, height),
                   ImageLockMode.WriteOnly, PixelFormat.Format32bppRgb);

                BitmapData newData = newBitmap.LockBits(
                   new Rectangle(0, 0, width, height),
                   ImageLockMode.WriteOnly, PixelFormat.Format32bppRgb);

                int pixelSize = 4;
                int x_pixelSize, x_pixelSize2, x_pixelSize3, x_pixelSize4;
                int y_AddNum;

                int width_2 = width / 2;

                if (RGBListFlag == true && RGBListActionFlag == false)
                {
                    RunRGBTempFlag = true;
                }
                else
                {
                    RunRGBTempFlag = false;
                }


                for (int y = 0; y < height; y++)
                {
                    byte* nRow = (byte*)newData.Scan0 + (y * newData.Stride);
                    byte* nRow2 = (byte*)newData2.Scan0 + (y * newData2.Stride);
                    y_AddNum = y * width * 4;

                    /*
                    for (int x = 0; x < width_2; x++)
                    {

                        x_pixelSize = x * pixelSize;
                        x_pixelSize2 = x_pixelSize + 1;
                        x_pixelSize3 = x_pixelSize + 2;
                        //x_pixelSize4 = x_pixelSize + 3;

                        nRow[x_pixelSize] = 255;
                        nRow[x_pixelSize2] = 255;
                        nRow[x_pixelSize3] = 255;
                        //nRow[x_pixelSize3] = 0;
                    }
                    */
                    for (int x = 0; x < width; x++)
                    {

                        x_pixelSize = x * pixelSize;
                        x_pixelSize2 = x_pixelSize + 1;
                        x_pixelSize3 = x_pixelSize + 2;
                        x_pixelSize4 = x_pixelSize + 3;

                        if (RunRGBTempFlag == true)
                        {
                            if (Pic_Width > x && Pic_Height > y)
                            {
                                if (RGBDXFData[y, x].ExistFlag == true)
                                {

                                    nRow[x_pixelSize] = RGBDXFData[y, x].Blue;
                                    nRow[x_pixelSize2] = RGBDXFData[y, x].Green;
                                    nRow[x_pixelSize3] = RGBDXFData[y, x].Red;
                                    nRow[x_pixelSize4] = 255;
                                    
                                    
                                    /*
                                    nRow[x_pixelSize] = 255;
                                    nRow[x_pixelSize2] = 0;
                                    nRow[x_pixelSize3] = 255;
                                    nRow[x_pixelSize4] = 255;
                                    */
                                }
                                else
                                {
                                    
                                    nRow[x_pixelSize] = nRow2[x_pixelSize];
                                    nRow[x_pixelSize2] = nRow2[x_pixelSize2];
                                    nRow[x_pixelSize3] = nRow2[x_pixelSize3];
                                    nRow[x_pixelSize4] = nRow2[x_pixelSize4];
                                    
                                    /*
                                    nRow[x_pixelSize] = 0;
                                    nRow[x_pixelSize2] = 255;
                                    nRow[x_pixelSize3] = 255;
                                    nRow[x_pixelSize4] = 255;
                                    */
                                }
                            }
                            else
                            {
                                nRow[x_pixelSize] = nRow2[x_pixelSize];
                                nRow[x_pixelSize2] = nRow2[x_pixelSize2];
                                nRow[x_pixelSize3] = nRow2[x_pixelSize3];
                                nRow[x_pixelSize4] = nRow2[x_pixelSize4];
                            }
                        }
                        else
                        {

                            nRow[x_pixelSize] = nRow2[x_pixelSize];
                            nRow[x_pixelSize2] = nRow2[x_pixelSize2];
                            nRow[x_pixelSize3] = nRow2[x_pixelSize3];
                            nRow[x_pixelSize4] = nRow2[x_pixelSize4];
                        }

                    }
                }

                newBitmap.UnlockBits(newData);
                OriginalImg.UnlockBits(newData2);


                return newBitmap;
            }
        }

        static Bitmap GetTotalImg3(Bitmap OriginalImg, int width, int height)
        {
            unsafe
            {
                //int width, height;
                int Ori_W, Ori_H;

                //width = OriginalImg.Width;
                //height = OriginalImg.Height;
                Ori_W = OriginalImg.Width;
                Ori_H = OriginalImg.Height;

                bool RunRGBTempFlag = false;

                Bitmap newBitmap = new Bitmap(width, height);

                Bitmap objShotBitmap = new Bitmap((Image)OriginalImg, width, height);
                
                BitmapData newData2 = objShotBitmap.LockBits(
                   new Rectangle(0, 0, width, height),
                   ImageLockMode.WriteOnly, PixelFormat.Format32bppRgb);

                BitmapData newData = newBitmap.LockBits(
                   new Rectangle(0, 0, width, height),
                   ImageLockMode.WriteOnly, PixelFormat.Format32bppRgb);

                int pixelSize = 4;
                int x_pixelSize, x_pixelSize2, x_pixelSize3, x_pixelSize4;
                //int y_AddNum;

                int width_2 = width / 2;

                if (RGBListFlag == true && RGBListActionFlag == false)
                {
                    RunRGBTempFlag = true;
                }
                else
                {
                    RunRGBTempFlag = false;
                }


                for (int y = 0; y < height; y++)
                {
                    byte* nRow = (byte*)newData.Scan0 + (y * newData.Stride);
                    byte* nRow2 = (byte*)newData2.Scan0 + (y * newData2.Stride);
                    //y_AddNum = y * width * 4;

                    /*
                    for (int x = 0; x < width_2; x++)
                    {

                        x_pixelSize = x * pixelSize;
                        x_pixelSize2 = x_pixelSize + 1;
                        x_pixelSize3 = x_pixelSize + 2;
                        //x_pixelSize4 = x_pixelSize + 3;

                        nRow[x_pixelSize] = 255;
                        nRow[x_pixelSize2] = 255;
                        nRow[x_pixelSize3] = 255;
                        //nRow[x_pixelSize3] = 0;
                    }
                    */
                    for (int x = 0; x < width; x++)
                    {
                        x_pixelSize = x * pixelSize;
                        x_pixelSize2 = x_pixelSize + 1;
                        x_pixelSize3 = x_pixelSize + 2;
                        x_pixelSize4 = x_pixelSize + 3;
                        
                        RunRGBTempFlag = false;
                        if (RunRGBTempFlag == true)
                        {
                            if (Pic_Width > x && Pic_Height > y)
                            {
                                if (RGBDXFData[y, x].ExistFlag == true)
                                {

                                    nRow[x_pixelSize] = RGBDXFData[y, x].Blue;
                                    nRow[x_pixelSize2] = RGBDXFData[y, x].Green;
                                    nRow[x_pixelSize3] = RGBDXFData[y, x].Red;
                                    nRow[x_pixelSize4] = 255;


                                    /*
                                    nRow[x_pixelSize] = 255;
                                    nRow[x_pixelSize2] = 0;
                                    nRow[x_pixelSize3] = 255;
                                    nRow[x_pixelSize4] = 255;
                                    */
                                }
                                else
                                {

                                    nRow[x_pixelSize] = nRow2[x_pixelSize];
                                    nRow[x_pixelSize2] = nRow2[x_pixelSize2];
                                    nRow[x_pixelSize3] = nRow2[x_pixelSize3];
                                    nRow[x_pixelSize4] = nRow2[x_pixelSize4];

                                    /*
                                    nRow[x_pixelSize] = 0;
                                    nRow[x_pixelSize2] = 255;
                                    nRow[x_pixelSize3] = 255;
                                    nRow[x_pixelSize4] = 255;
                                    */
                                }
                            }
                            else
                            {
                                nRow[x_pixelSize] = nRow2[x_pixelSize];
                                nRow[x_pixelSize2] = nRow2[x_pixelSize2];
                                nRow[x_pixelSize3] = nRow2[x_pixelSize3];
                                nRow[x_pixelSize4] = nRow2[x_pixelSize4];
                            }
                        }
                        else
                        {

                            nRow[x_pixelSize] = nRow2[x_pixelSize];
                            nRow[x_pixelSize2] = nRow2[x_pixelSize2];
                            nRow[x_pixelSize3] = nRow2[x_pixelSize3];
                            nRow[x_pixelSize4] = nRow2[x_pixelSize4];

                        }

                    }
                }

                newBitmap.UnlockBits(newData);
                objShotBitmap.UnlockBits(newData2);

                return newBitmap;
            }
        }




        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;

            cboZoomCamList_SelectedIndexChanged(cboZoomCamList, new EventArgs());

            SetValueLamp();

            timLoadStart.Enabled = true;

            /*
            //pictureBox1.Image = canvas;

            canvas.Save(@"D:\test.jpg");

            TempImg = (Image)canvas.Clone();

            TempImg.Save(@"D:\test2.jpg");

            ReDefRGBData();
            */
        }

        private void ReDefRGBData()
        {
            if (RGBListFlag == false)
            {
                //RGBDXFData = new RGBDatInfo[Pic_Height, Pic_Width];
                //RGBListFlag = true;
            }
            if (RGBListFlag == true)
            {
                RGBListActionFlag = true;


                unsafe
                {
                    int y, x;
                    int pixelSize = 3;
                    int x_pixelSize, x_pixelSize2, x_pixelSize3, x_pixelSize4;
                    //int y_AddNum;

                    //Bitmap objBlkBitmap = new Bitmap(TempImg, Pic_Width, Pic_Height);

                    //////////////////////////////////////////////////////////////////////////////
                    // 이미지 배열화 작업.

                    Bitmap objBlkBitmap = new Bitmap(TempImg, Pic_Width, Pic_Height);

                    RGBDatInfo[,] BlkImgRGBData = new RGBDatInfo[objBlkBitmap.Height, objBlkBitmap.Width];

                    //lock the original bitmap in memory
                    BitmapData BlkoriginalData = objBlkBitmap.LockBits(
                       new Rectangle(0, 0, objBlkBitmap.Width, objBlkBitmap.Height),
                       ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

                    for (y = 0; y < objBlkBitmap.Height; y++)
                    {
                        //get the data from the original image
                        byte* oRow = (byte*)BlkoriginalData.Scan0 + (y * BlkoriginalData.Stride);

                        for (x = 0; x < objBlkBitmap.Width; x++)
                        {
                            x_pixelSize = x * pixelSize;
                            x_pixelSize2 = x_pixelSize + 1;
                            x_pixelSize3 = x_pixelSize + 2;

                            /*
                            BlkImgRGBData[y, x].Blue = oRow[x_pixelSize];
                            BlkImgRGBData[y, x].Green = oRow[x_pixelSize2];
                            BlkImgRGBData[y, x].Red = oRow[x_pixelSize3];

                            RGBDXFData[y, x].Blue = BlkImgRGBData[y, x].Blue;
                            RGBDXFData[y, x].Green = BlkImgRGBData[y, x].Green;
                            RGBDXFData[y, x].Red = BlkImgRGBData[y, x].Red;
                            */

                            RGBDXFData[y, x].Blue = oRow[x_pixelSize];
                            RGBDXFData[y, x].Green = oRow[x_pixelSize2];
                            RGBDXFData[y, x].Red = oRow[x_pixelSize3];

                            /*
                            if (RGBDXFData[y, x].Blue > 0 || RGBDXFData[y, x].Green > 0 || RGBDXFData[y, x].Red > 0)
                            {
                                RGBDXFData[y, x].ExistFlag = true;
                            }
                            */

                            if (RGBDXFData[y, x].Red == 0 && RGBDXFData[y, x].Green == 0 && RGBDXFData[y, x].Blue == 0)
                            {
                                RGBDXFData[y, x].ExistFlag = false;
                            }
                            else
                            {
                                RGBDXFData[y, x].ExistFlag = true;
                            }

                            /*
                            ImgRGBData[y, x].Blue = 0;
                            ImgRGBData[y, x].Green = 255;
                            ImgRGBData[y, x].Red = 0;
                            */
                        }
                    }

                    objBlkBitmap.UnlockBits(BlkoriginalData);


                    /*
                    //int width, height;
                    BitmapData newData2 = objBlkBitmap.LockBits(
                       new Rectangle(0, 0, Pic_Width, Pic_Height),
                       ImageLockMode.WriteOnly, PixelFormat.Format32bppRgb);


                    for (y = 0; y < Pic_Height; y++)
                    {
                        byte* nRow2 = (byte*)newData2.Scan0 + (y * newData2.Stride);
                        //y_AddNum = y * Pic_Width * 3;

                        for (x = 0; x < Pic_Width; x++)
                        {

                            x_pixelSize = x * pixelSize;
                            x_pixelSize2 = x_pixelSize + 1;
                            x_pixelSize3 = x_pixelSize + 2;
                            x_pixelSize4 = x_pixelSize + 3;

                            
                            RGBDXFData[y, x].Red = nRow2[x_pixelSize];
                            RGBDXFData[y, x].Green = nRow2[x_pixelSize2];
                            RGBDXFData[y, x].Blue = nRow2[x_pixelSize3];
                            

                            if (RGBDXFData[y, x].Blue > 0 || RGBDXFData[y, x].Green > 0 || RGBDXFData[y, x].Red > 0)
                            {
                                RGBDXFData[y, x].ExistFlag = true;
                            }
                            else if (RGBDXFData[y, x].Red == 0 && RGBDXFData[y, x].Green == 0 && RGBDXFData[y, x].Blue == 0)
                            {
                                RGBDXFData[y, x].ExistFlag = true;
                            }
                            else
                            {
                                RGBDXFData[y, x].ExistFlag = true;
                            }

                        }
                    }

                    objBlkBitmap.UnlockBits(newData2);
                    */

                }

                RGBListActionFlag = false;
            }
        }

        private void button19_Click(object sender, EventArgs e)
        {
            trackBar1.Value = trackBar1.Maximum;
            //if (NavitarCtrl.ProductID > 0)
            if (NavitarCtrl.Connected == true)
            {
                NavitarCtrl.Write(REG_USER_TARGET_1, trackBar1.Value);
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            trackBar1.Value = 0;
            //if (NavitarCtrl.ProductID > 0)
            if (NavitarCtrl.Connected == true)
            {
                NavitarCtrl.Write(REG_USER_TARGET_1, trackBar1.Value);
            }
        }

        private void button20_Click(object sender, EventArgs e)
        {
            trackBar2.Value = trackBar2.Maximum;
            //if (NavitarCtrl.ProductID > 0)
            if (NavitarCtrl.Connected == true)
            {
                NavitarCtrl.Write(REG_USER_TARGET_2, trackBar2.Value);
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            trackBar2.Value = 0;
            //if (NavitarCtrl.ProductID > 0)
            if (NavitarCtrl.Connected == true)
            {
                NavitarCtrl.Write(REG_USER_TARGET_2, trackBar2.Value);
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            textBox1.Text = trackBar1.Value.ToString();
            CurZoomFactNum = trackBar1.Value;
            /*
            if (NavitarCtrl.ProductID > 0)
            {
                NavitarCtrl.Write(REG_USER_TARGET_1, trackBar1.Value);
            }
            */
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            textBox2.Text = trackBar2.Value.ToString();
            CurFocusFactNum = trackBar2.Value;
            /*
            if (NavitarCtrl.ProductID > 0)
            {
                NavitarCtrl.Write(REG_USER_TARGET_2, trackBar2.Value);
            }
            */
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            textBox1.Text = trackBar1.Value.ToString();
            CurZoomFactNum = trackBar1.Value;
        }

        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
            textBox2.Text = trackBar2.Value.ToString();
            CurFocusFactNum = trackBar2.Value;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            timer2.Enabled = false;
            int TrackValue1, TrackValue2;
            //int GetResValue = SerConnectionConnect(8, DEF_MOTOR_CONTROLLER);
            //MessageBox.Show(GetResValue.ToString());
            //if (NavitarCtrl.ProductID > 0)
            if (NavitarCtrl.Connected == true)
            {
                trackBar1.Maximum = NavitarCtrl.Read(REG_SETUP_LIMIT_1);
                trackBar2.Maximum = NavitarCtrl.Read(REG_SETUP_LIMIT_2);

                TrackValue1 = NavitarCtrl.Read(REG_USER_CURRENT_1);
                TrackValue2 = NavitarCtrl.Read(REG_USER_CURRENT_2);

                if (TrackValue1<0)
                {
                    trackBar1.Value = 0;
                }
                else
                {
                    trackBar1.Value = NavitarCtrl.Read(REG_USER_CURRENT_1);
                }
                if (TrackValue2 < 0)
                {
                    trackBar2.Value = 0;
                }
                else
                {
                    trackBar2.Value = NavitarCtrl.Read(REG_USER_CURRENT_2);
                }

                ZoomIn_Max = NavitarCtrl.Read(REG_SETUP_LIMIT_1);
                Focus_Max = NavitarCtrl.Read(REG_SETUP_LIMIT_2);
                ZoomIn_Min = 0;
                Focus_Min = 0;



                ZoomFactNum = trackBar1.Value;
                FocusFactNum = trackBar2.Value;

                if (ShotData[FULLSHOTTYPE].ZoomNum > -1 && ShotData[FULLSHOTTYPE].FocusNum > -1)
                {
                    CurZoomFactNum = ShotData[FULLSHOTTYPE].ZoomNum;
                    CurFocusFactNum = ShotData[FULLSHOTTYPE].FocusNum;
                }


                timer3.Enabled = true;

            }

            //label2.Text = RealScaleBox.Width.ToString() + "," + RealScaleBox.Height.ToString() + "," + RealScaleBox.SizeMode.ToString();

        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            /*
            if (ZoomFactNum != CurZoomFactNum)
            {
                if (NavitarCtrl.ProductID > 0)
                {
                    ZoomFactNum = CurZoomFactNum;
                    NavitarCtrl.Write(REG_USER_TARGET_1, ZoomFactNum);
                }
            }
            if (FocusFactNum != CurFocusFactNum)
            {
                if (NavitarCtrl.ProductID > 0)
                {
                    if (FocusFactNum > CurFocusFactNum)
                    {
                        FocusFactNum = CurFocusFactNum;
                        NavitarCtrl.Write(REG_USER_INCREMENT_2, - 100);
                        //NavitarCtrl.Write(REG_USER_INCREMENT_2, FocusFactNum);
                    }
                    else
                    {
                        FocusFactNum = CurFocusFactNum;
                        NavitarCtrl.Write(REG_USER_TARGET_2, FocusFactNum);
                    }
                }
            }
            */

            int Cur_Temp_ZoomNum, Cur_Temp_FocusNum, Cur_Temp_Zoom_Status, Cur_Temp_Focus_Status;

            //if (NavitarCtrl.ProductID > 0)
            if (NavitarCtrl != null)
            {
                if (NavitarCtrl.Connected == true)
                {
                    Cur_Temp_ZoomNum = NavitarCtrl.Read(REG_USER_CURRENT_1);
                    Cur_Temp_FocusNum = NavitarCtrl.Read(REG_USER_CURRENT_2);
                    Cur_Temp_Zoom_Status = NavitarCtrl.Read(REG_USER_STATUS_1);
                    Cur_Temp_Focus_Status = NavitarCtrl.Read(REG_USER_STATUS_2);
                    label6.Text = Cur_Temp_Zoom_Status.ToString() + "," + Cur_Temp_Focus_Status.ToString();
                    if (Cur_Temp_Zoom_Status >= 512)
                    {
                        Cur_Temp_Zoom_Status = Cur_Temp_Zoom_Status - 512;
                    }
                    if (Cur_Temp_Focus_Status >= 512)
                    {
                        Cur_Temp_Focus_Status = Cur_Temp_Focus_Status - 512;
                    }
                    if (ZoomFactNum != CurZoomFactNum && Cur_Temp_Zoom_Status == 0)
                    {
                        ZoomFactNum = CurZoomFactNum;
                        Cur_Temp_ZoomNum = ZoomFactNum - Cur_Temp_ZoomNum;
                        NavitarCtrl.Write(REG_USER_INCREMENT_1, Cur_Temp_ZoomNum);
                    }
                    else if (ZoomFactNum != Cur_Temp_ZoomNum && Cur_Temp_Zoom_Status == 0)
                    {
                        Cur_Temp_ZoomNum = ZoomFactNum - Cur_Temp_ZoomNum;
                        NavitarCtrl.Write(REG_USER_INCREMENT_1, Cur_Temp_ZoomNum);
                    }
                    /*
                    if (FocusFactNum != CurFocusFactNum)
                    {
                        if (FocusFactNum > CurFocusFactNum)
                        {
                            FocusFactNum = CurFocusFactNum;
                            NavitarCtrl.Write(REG_USER_INCREMENT_2, -100);
                            //NavitarCtrl.Write(REG_USER_INCREMENT_2, FocusFactNum);
                        }
                        else
                        {
                            FocusFactNum = CurFocusFactNum;
                            NavitarCtrl.Write(REG_USER_TARGET_2, FocusFactNum);
                        }
                    }
                    */
                    if (FocusFactNum != CurFocusFactNum && Cur_Temp_Focus_Status == 0)
                    {
                        FocusFactNum = CurFocusFactNum;
                        Cur_Temp_FocusNum = FocusFactNum - Cur_Temp_FocusNum;
                        NavitarCtrl.Write(REG_USER_INCREMENT_2, Cur_Temp_FocusNum);
                    }
                    else if (FocusFactNum != Cur_Temp_FocusNum && Cur_Temp_Focus_Status == 0)
                    {
                        Cur_Temp_FocusNum = FocusFactNum - Cur_Temp_FocusNum;
                        NavitarCtrl.Write(REG_USER_INCREMENT_2, Cur_Temp_FocusNum);
                    }

                    if (textBox1.Focused == false)
                    {
                        CurRealZoomFactNum = Cur_Temp_ZoomNum;
                        textBox1.Text = Cur_Temp_ZoomNum.ToString();
                    }
                    if (textBox2.Focused == false)
                    {
                        CurRealFocusFactNum = Cur_Temp_FocusNum;
                        textBox2.Text = Cur_Temp_FocusNum.ToString();
                    }
                }
            }

        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                textBox1.Focus();
                CurFocusFactNum = Int32.Parse(textBox2.Text);
                trackBar2.Value = CurFocusFactNum;
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                textBox2.Focus();
                CurZoomFactNum = Int32.Parse(textBox1.Text);
                trackBar1.Value = CurZoomFactNum;
            }
        }

        private void btUp_MouseDown(object sender, MouseEventArgs e)
        {
            FocusPlusMinus = 10;
            timer4.Enabled = true;
        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            //if (NavitarCtrl.ProductID > 0)
            if (NavitarCtrl.Connected == true)
            {
                CurFocusFactNum = CurFocusFactNum + FocusPlusMinus;
                if (CurFocusFactNum < Focus_Min)
                {
                    CurFocusFactNum = Focus_Min;
                }
                if (CurFocusFactNum > Focus_Max)
                {
                    CurFocusFactNum = Focus_Max;
                }
                textBox2.Text = CurFocusFactNum.ToString();
            }
        }

        private void btUp_MouseUp(object sender, MouseEventArgs e)
        {
            FocusPlusMinus = 0;
            timer4.Enabled = false;
        }

        private void btDown_MouseDown(object sender, MouseEventArgs e)
        {
            FocusPlusMinus = -10;
            timer4.Enabled = true;
        }

        private void btDown_MouseUp(object sender, MouseEventArgs e)
        {
            FocusPlusMinus = 0;
            timer4.Enabled = false;
        }

        private void btShotSave_Click(object sender, EventArgs e)
        {
            if (RealScaleBox.Image != null)
            {
                RealScaleBox.Image.Save("shot.jpg");
                MessageBox.Show("저장완료!");
            }
        }

        private void btLoopStart_Click(object sender, EventArgs e)
        {
            //string LoopWorkStr;

            if (FCADImage == null)
            {
                MessageBox.Show("도면정보가 없습니다.");
                return;
            }


            //RecoSetData.Motion_Move_X = double.Parse(txtXAxisValue.Text);
            //RecoSetData.Motion_Move_Y = double.Parse(txtYAxisValue.Text);
            //RecoSetData.Motion_Move_Z = double.Parse(txtZAxisValue.Text);


            LpWk_LoopCount = 0;
            LpWk_LoopCountMax = 1;
            LpWk_Zoom = Int32.Parse(textBox1.Text);
            LpWk_Focus = Int32.Parse(textBox2.Text);


            /*
            LoopWorkStr = "Zoom:" + LpWk_Zoom.ToString() + ", Focus:" + LpWk_Focus.ToString() + ", Loop:" + LpWk_LoopCountMax.ToString() + @" 값들로 진행하겠습니까?";

            if (MessageBox.Show(LoopWorkStr, "반복작업여부", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }
            */

            LpWk_RunTimerCount = 0;
            LpWk_RunFlag = true;
            LpWk_RUN_STATUS = WORKSTART;
            timWork.Enabled = true;
            btLoopStart.Enabled = false;
            btLoopStop.Enabled = true;
            timerAxis.Enabled = false;
        }

        private void btLoopStop_Click(object sender, EventArgs e)
        {
            LpWk_RunFlag = false;
            timWork.Enabled = false;
            btLoopStart.Enabled = true;
            btLoopStop.Enabled = false;
            MessageBox.Show("테스트중지!");
        }

        public void DrawDXFImage(Bitmap bmp)
        {
            string GetEntityName;
            DXFLine dxLine;
            DXFCircle dxCircle;
            DXFArc dxArc;
            SFPoint P1, P2;
            float rd1, rd2;
            float MinX, MinY, MaxX, MaxY;

            MinX = 99999.0f;
            MinY = 99999.0f;
            MaxX = -99999.0f;
            MaxY = -99999.0f;
            if (FCADImage == null)
                return;

            Pen blackPen = new Pen(Color.LightGreen, 2);

            // Draw line to screen.
            using (var graphics = Graphics.FromImage(bmp))
            {
                for (int i = 0; i < FCADImage.FEntities.Entities.Count; i++)
                {
                    GetEntityName = FCADImage.FEntities.Entities[i].ToString();
                    if (GetEntityName == "DXFImportReco.DXFLine")
                    {
                        dxLine = (DXFLine)FCADImage.FEntities.Entities[i];
                        P1 = GetPoint(dxLine.Point1);
                        P2 = GetPoint(dxLine.Point2);

                        blackPen.Color = dxLine.FColor;

                        graphics.DrawLine(blackPen, P1.X, P1.Y, P2.X, P2.Y);
                        if (MinX > P1.X)
                        {
                            MinX = P1.X;
                        }
                        if (MaxX < P1.X)
                        {
                            MaxX = P1.X;
                        }
                        if (MinY > P1.Y)
                        {
                            MinY = P1.Y;
                        }
                        if (MaxY < P1.Y)
                        {
                            MaxY = P1.Y;
                        }

                        if (MinX > P2.X)
                        {
                            MinX = P2.X;
                        }
                        if (MaxX < P2.X)
                        {
                            MaxX = P2.X;
                        }
                        if (MinY > P2.Y)
                        {
                            MinY = P2.Y;
                        }
                        if (MaxY < P2.Y)
                        {
                            MaxY = P2.Y;
                        }
                    }
                    else if (GetEntityName == "DXFImportReco.DXFCircle")
                    {
                        dxCircle = (DXFCircle)FCADImage.FEntities.Entities[i];
                        rd1 = dxCircle.radius;
                        P1 = GetPoint(dxCircle.Point1);
                        rd1 = rd1 * FScale;
                        P1.X = P1.X - rd1;
                        P1.Y = P1.Y - rd1;

                        blackPen.Color = dxCircle.FColor;

                        graphics.DrawEllipse(blackPen, P1.X, P1.Y, rd1 * 2, rd1 * 2);
                        if (MinX > P1.X - rd1)
                        {
                            MinX = P1.X - rd1;
                        }
                        if (MaxX < P1.X + rd1)
                        {
                            MaxX = P1.X + rd1;
                        }
                        if (MinY > P1.Y - rd1)
                        {
                            MinY = P1.Y - rd1;
                        }
                        if (MaxY < P1.Y + rd1)
                        {
                            MaxY = P1.Y + rd1;
                        }


                    }
                    else if (GetEntityName == "DXFImportReco.DXFArc")
                    {
                        dxArc = (DXFArc)FCADImage.FEntities.Entities[i];

                        blackPen.Color = dxArc.FColor;

                        if (dxArc.pt1.X == 0)
                        {
                            rd1 = dxArc.radius; //Math.Abs(dxArc.radius * dxArc.ratio);
                            rd2 = dxArc.radius;
                        }
                        else
                        {
                            rd1 = dxArc.radius;
                            rd2 = dxArc.radius; //Math.Abs(dxArc.radius * ratio);
                        }

                        rd1 = dxArc.radius;
                        P1 = GetPoint(dxArc.Point1);
                        rd1 = rd1 * FScale;
                        rd2 = rd2 * FScale;
                        P1.X = P1.X - rd1;
                        P1.Y = P1.Y - rd1;
                        float sA = -dxArc.startAngle, eA = -dxArc.endAngle;
                        if (dxArc.endAngle < dxArc.startAngle) sA = Conversion_Angle(sA);
                        eA -= sA;

                        if (eA == 0)
                        {
                            graphics.DrawEllipse(blackPen, P1.X, P1.Y, rd1 * 2, rd2 * 2);
                        }
                        else
                        {
                            graphics.DrawArc(blackPen, P1.X, P1.Y, rd1 * 2, rd2 * 2, 0, 360);//sA, eA);
                        }
                        if (MinX > P1.X - rd1)
                        {
                            MinX = P1.X - rd1;
                        }
                        if (MaxX < P1.X + rd1)
                        {
                            MaxX = P1.X + rd1;
                        }
                        if (MinY > P1.Y - rd1)
                        {
                            MinY = P1.Y - rd1;
                        }
                        if (MaxY < P1.Y + rd1)
                        {
                            MaxY = P1.Y + rd1;
                        }

                    }
                    else
                    {
                        //OutTxtStr += "\r\n" + GetEntityName;
                    }
                }
            }
        }


        public void DrawDXFImageCalc_Old(int DrImgW, int DrImgH)
        {
            string GetEntityName;
            DXFLine dxLine;
            DXFCircle dxCircle;
            DXFArc dxArc;
            SFPoint P1, P2;
            float rd1, rd2;
            float MinX, MinY, MaxX, MaxY;

            MinX = 99999.0f;
            MinY = 99999.0f;
            MaxX = -99999.0f;
            MaxY = -99999.0f;
            if (FCADImage == null)
                return;

            //Bitmap tmp = new Bitmap(bmp, bmp.Width, bmp.Height);

            for (int i = 0; i < FCADImage.FEntities.Entities.Count; i++)
            {
                GetEntityName = FCADImage.FEntities.Entities[i].ToString();
                if (GetEntityName == "DXFImportReco.DXFLine")
                {
                    /*
                    dxLine = (DXFLine)FCADImage.FEntities.Entities[i];

                    blackPen.Color = dxLine.FColor;

                    P1 = GetPoint(dxLine.Point1);
                    P2 = GetPoint(dxLine.Point2);

                    graphics.DrawLine(blackPen, P1.X, P1.Y, P2.X, P2.Y);
                    if (MinX > P1.X)
                    {
                        MinX = P1.X;
                    }
                    if (MaxX < P1.X)
                    {
                        MaxX = P1.X;
                    }
                    if (MinY > P1.Y)
                    {
                        MinY = P1.Y;
                    }
                    if (MaxY < P1.Y)
                    {
                        MaxY = P1.Y;
                    }

                    if (MinX > P2.X)
                    {
                        MinX = P2.X;
                    }
                    if (MaxX < P2.X)
                    {
                        MaxX = P2.X;
                    }
                    if (MinY > P2.Y)
                    {
                        MinY = P2.Y;
                    }
                    if (MaxY < P2.Y)
                    {
                        MaxY = P2.Y;
                    }
                    */
                }
                else if (GetEntityName == "DXFImportReco.DXFCircle")
                {
                    dxCircle = (DXFCircle)FCADImage.FEntities.Entities[i];

                    rd1 = dxCircle.radius;
                    P1 = dxCircle.Point1;
                    rd1 = rd1 * FScale;

                    if (MinX > P1.X - rd1)
                    {
                        MinX = P1.X - rd1;
                    }
                    if (MaxX < P1.X + rd1)
                    {
                        MaxX = P1.X + rd1;
                    }
                    if (MinY > P1.Y - rd1)
                    {
                        MinY = P1.Y - rd1;
                    }
                    if (MaxY < P1.Y + rd1)
                    {
                        MaxY = P1.Y + rd1;
                    }


                }
                else if (GetEntityName == "DXFImportReco.DXFArc")
                {
                    /*
                    dxArc = (DXFArc)FCADImage.FEntities.Entities[i];

                    blackPen.Color = dxArc.FColor;

                    if (dxArc.pt1.X == 0)
                    {
                        rd1 = dxArc.radius; //Math.Abs(dxArc.radius * dxArc.ratio);
                        rd2 = dxArc.radius;
                    }
                    else
                    {
                        rd1 = dxArc.radius;
                        rd2 = dxArc.radius; //Math.Abs(dxArc.radius * ratio);
                    }

                    rd1 = dxArc.radius;
                    P1 = GetPoint(dxArc.Point1);
                    rd1 = rd1 * FScale;
                    rd2 = rd2 * FScale;
                    P1.X = P1.X - rd1;
                    P1.Y = P1.Y - rd1;
                    float sA = -dxArc.startAngle, eA = -dxArc.endAngle;
                    if (dxArc.endAngle < dxArc.startAngle) sA = Conversion_Angle(sA);
                    eA -= sA;

                    if (eA == 0)
                    {
                        graphics.DrawEllipse(blackPen, P1.X, P1.Y, rd1 * 2, rd2 * 2);
                    }
                    else
                    {
                        graphics.DrawArc(blackPen, P1.X, P1.Y, rd1 * 2, rd2 * 2, 0, 360);//sA, eA);
                    }
                    if (MinX > P1.X - rd1)
                    {
                        MinX = P1.X - rd1;
                    }
                    if (MaxX < P1.X + rd1)
                    {
                        MaxX = P1.X + rd1;
                    }
                    if (MinY > P1.Y - rd1)
                    {
                        MinY = P1.Y - rd1;
                    }
                    if (MaxY < P1.Y + rd1)
                    {
                        MaxY = P1.Y + rd1;
                    }
                    */
                }
                else
                {
                    //OutTxtStr += "\r\n" + GetEntityName;
                }
            }
            if (StartCalcFlag == true)
            {
                FS_W = MaxX - MinX;
                FS_H = MaxY - MinY;
                FS_W_Base = MinX;
                FS_H_Base = MinY;

                float W_FScale, H_FScale;
                W_FScale = ((float)DrImgW * 80.0f / 100.0f) / FS_W;
                H_FScale = ((float)DrImgH * 80.0f / 100.0f) / FS_H;

                if (W_FScale < H_FScale)
                {
                    FScale = W_FScale;
                }
                else
                {
                    FScale = H_FScale;
                }

                Base.X = DrImgW/2;
                Base.Y = DrImgH/2;

                StartCalcFlag = false;
            }
            //bmp = tmp;
        }


        public void DrawDXFImageCalc(int DrImgW, int DrImgH, bool ChangeSelFlag)
        {
            string GetEntityName;
            DXFLine dxLine;
            DXFCircle dxCircle;
            DXFArc dxArc;
            SFPoint P1, P2;
            float rd1, rd2;
            float MinX, MinY, MaxX, MaxY;

            MinX = 99999.0f;
            MinY = 99999.0f;
            MaxX = -99999.0f;
            MaxY = -99999.0f;
            if (FCADImage == null)
                return;
            if (FCADImage.FEntities == null)
                return;

            //Bitmap tmp = new Bitmap(bmp, bmp.Width, bmp.Height);

            for (int i = 0; i < FCADImage.FEntities.Entities.Count; i++)
            {
                GetEntityName = FCADImage.FEntities.Entities[i].ToString();
                if (GetEntityName == "DXFImportReco.DXFLine")
                {
                    /*
                    dxLine = (DXFLine)FCADImage.FEntities.Entities[i];

                    blackPen.Color = dxLine.FColor;

                    P1 = GetPoint(dxLine.Point1);
                    P2 = GetPoint(dxLine.Point2);

                    graphics.DrawLine(blackPen, P1.X, P1.Y, P2.X, P2.Y);
                    if (MinX > P1.X)
                    {
                        MinX = P1.X;
                    }
                    if (MaxX < P1.X)
                    {
                        MaxX = P1.X;
                    }
                    if (MinY > P1.Y)
                    {
                        MinY = P1.Y;
                    }
                    if (MaxY < P1.Y)
                    {
                        MaxY = P1.Y;
                    }

                    if (MinX > P2.X)
                    {
                        MinX = P2.X;
                    }
                    if (MaxX < P2.X)
                    {
                        MaxX = P2.X;
                    }
                    if (MinY > P2.Y)
                    {
                        MinY = P2.Y;
                    }
                    if (MaxY < P2.Y)
                    {
                        MaxY = P2.Y;
                    }
                    */
                }
                else if (GetEntityName == "DXFImportReco.DXFCircle")
                {
                    dxCircle = (DXFCircle)FCADImage.FEntities.Entities[i];

                    if (StartCalcFlag == true && ChangeSelFlag == true)
                    {
                        dxCircle.SelectObjFlag = false;
                    }

                    rd1 = dxCircle.radius;
                    P1 = dxCircle.Point1;
                    rd1 = rd1;// *FScale;

                    if (MinX > P1.X - rd1)
                    {
                        MinX = P1.X - rd1;
                    }
                    if (MaxX < P1.X + rd1)
                    {
                        MaxX = P1.X + rd1;
                    }
                    if (MinY > P1.Y - rd1)
                    {
                        MinY = P1.Y - rd1;
                    }
                    if (MaxY < P1.Y + rd1)
                    {
                        MaxY = P1.Y + rd1;
                    }


                }
                else if (GetEntityName == "DXFImportReco.DXFArc")
                {
                    /*
                    dxArc = (DXFArc)FCADImage.FEntities.Entities[i];

                    blackPen.Color = dxArc.FColor;

                    if (dxArc.pt1.X == 0)
                    {
                        rd1 = dxArc.radius; //Math.Abs(dxArc.radius * dxArc.ratio);
                        rd2 = dxArc.radius;
                    }
                    else
                    {
                        rd1 = dxArc.radius;
                        rd2 = dxArc.radius; //Math.Abs(dxArc.radius * ratio);
                    }

                    rd1 = dxArc.radius;
                    P1 = GetPoint(dxArc.Point1);
                    rd1 = rd1 * FScale;
                    rd2 = rd2 * FScale;
                    P1.X = P1.X - rd1;
                    P1.Y = P1.Y - rd1;
                    float sA = -dxArc.startAngle, eA = -dxArc.endAngle;
                    if (dxArc.endAngle < dxArc.startAngle) sA = Conversion_Angle(sA);
                    eA -= sA;

                    if (eA == 0)
                    {
                        graphics.DrawEllipse(blackPen, P1.X, P1.Y, rd1 * 2, rd2 * 2);
                    }
                    else
                    {
                        graphics.DrawArc(blackPen, P1.X, P1.Y, rd1 * 2, rd2 * 2, 0, 360);//sA, eA);
                    }
                    if (MinX > P1.X - rd1)
                    {
                        MinX = P1.X - rd1;
                    }
                    if (MaxX < P1.X + rd1)
                    {
                        MaxX = P1.X + rd1;
                    }
                    if (MinY > P1.Y - rd1)
                    {
                        MinY = P1.Y - rd1;
                    }
                    if (MaxY < P1.Y + rd1)
                    {
                        MaxY = P1.Y + rd1;
                    }
                    */
                }
                else
                {
                    //OutTxtStr += "\r\n" + GetEntityName;
                }
            }
            if (StartCalcFlag == true)
            {
                FS_W = MaxX - MinX;
                FS_H = MaxY - MinY;
                FS_W_Base = MinX;
                FS_H_Base = MinY;

                float W_FScale, H_FScale;
                W_FScale = ((float)DrImgW * 80.0f / 100.0f) / FS_W;
                H_FScale = ((float)DrImgH * 80.0f / 100.0f) / FS_H;
                if (W_FScale < H_FScale)
                {
                    FScale = W_FScale;
                }
                else
                {
                    FScale = H_FScale;
                }

                Base.X = DrImgW / 2;
                Base.Y = DrImgH / 2;

                StartCalcFlag = false;
            }
            //bmp = tmp;
        }

        public void SelectDXFReScale(int ImgCur_W, int ImgCur_H)
        {
            string GetEntityName;
            DXFLine dxLine;
            DXFCircle dxCircle;
            DXFArc dxArc;
            SFPoint P1, P2;
            float rd1, rd2;
            //float MinX, MinY, MaxX, MaxY;
            float MinSelDisCalc;
            float MinSelDisNum = 999999.0f;
            int MinSelIndex;
            float Real_W, Real_H;

            MinSelIndex = -1;

            if (FCADImage == null)
                return;
            if (FCADImage.FEntities == null)
                return;

            for (int i = 0; i < FCADImage.FEntities.Entities.Count; i++)
            {
                GetEntityName = FCADImage.FEntities.Entities[i].ToString();
                if (GetEntityName == "DXFImportReco.DXFLine")
                {
                    /*
                    dxLine = (DXFLine)FCADImage.FEntities.Entities[i];

                    blackPen.Color = dxLine.FColor;

                    P1 = GetPoint(dxLine.Point1);
                    P2 = GetPoint(dxLine.Point2);

                    graphics.DrawLine(blackPen, P1.X, P1.Y, P2.X, P2.Y);
                    */
                }
                else if (GetEntityName == "DXFImportReco.DXFCircle")
                {
                    dxCircle = (DXFCircle)FCADImage.FEntities.Entities[i];

                    if (dxCircle.SelectObjFlag == true)
                    {
                        rd1 = dxCircle.radius;
                        P1 = dxCircle.Point1;
                        //rd1 = rd1 * FScale;

                        Real_W = rd1 * 2;
                        Real_H = rd1 * 2;

                        float W_FScale, H_FScale;
                        W_FScale = ((float)ImgCur_W * 80.0f / 100.0f) / Real_W;
                        H_FScale = ((float)ImgCur_H * 80.0f / 100.0f) / Real_H;
                        FS_W = Real_W;
                        FS_H = Real_W;
                        FS_W_Base = P1.X - rd1;
                        FS_H_Base = P1.Y - rd1;

                        if (W_FScale < H_FScale)
                        {
                            FScale = W_FScale;
                        }
                        else
                        {
                            FScale = H_FScale;
                        }

                        Base.X = (int)(P1.X - FScale * (P1.X - FS_W / 2 - FS_W_Base)) + ImgCur_W * 80 / 200;
                        Base.Y = (int)(P1.Y + FScale * (P1.Y - FS_H / 2 - FS_H_Base)) + ImgCur_W * 80 / 200;
                        //Base.X = (int)(P1.X * FScale);
                        //Base.Y = (int)(P1.Y * FScale);
                    }

                }
                else if (GetEntityName == "DXFImportReco.DXFArc")
                {
                    /*
                    dxArc = (DXFArc)FCADImage.FEntities.Entities[i];

                    blackPen.Color = dxArc.FColor;

                    if (dxArc.pt1.X == 0)
                    {
                        rd1 = dxArc.radius; //Math.Abs(dxArc.radius * dxArc.ratio);
                        rd2 = dxArc.radius;
                    }
                    else
                    {
                        rd1 = dxArc.radius;
                        rd2 = dxArc.radius; //Math.Abs(dxArc.radius * ratio);
                    }

                    rd1 = dxArc.radius;
                    P1 = GetPoint(dxArc.Point1);
                    rd1 = rd1 * FScale;
                    rd2 = rd2 * FScale;
                    P1.X = P1.X - rd1;
                    P1.Y = P1.Y - rd1;
                    float sA = -dxArc.startAngle, eA = -dxArc.endAngle;
                    if (dxArc.endAngle < dxArc.startAngle) sA = Conversion_Angle(sA);
                    eA -= sA;

                    if (eA == 0)
                    {
                        graphics.DrawEllipse(blackPen, P1.X, P1.Y, rd1 * 2, rd2 * 2);
                    }
                    else
                    {
                        graphics.DrawArc(blackPen, P1.X, P1.Y, rd1 * 2, rd2 * 2, 0, 360);//sA, eA);
                    }
                    */
                }
                else
                {
                    //OutTxtStr += "\r\n" + GetEntityName;
                }
            }
        }

        public void DrawDXFImage2(Bitmap bmp)
        {
            string GetEntityName;
            DXFLine dxLine;
            DXFCircle dxCircle;
            DXFArc dxArc;
            SFPoint P1, P2;
            float rd1, rd2;
            float MinX, MinY, MaxX, MaxY;
            int Dr_Cen_X, Dr_Cen_Y;

            MinX = 99999.0f;
            MinY = 99999.0f;
            MaxX = -99999.0f;
            MaxY = -99999.0f;
            Dr_Cen_X = bmp.Width;
            Dr_Cen_Y = bmp.Height/2;
            if (FCADImage == null)
                return;
            if (FCADImage.FEntities == null)
                return;

            Pen blackPen = new Pen(Color.LightGreen, 2);

            Pen SelectPen = new Pen(Color.Red, 2);

            using (var graphics = Graphics.FromImage(bmp))
            {
                for (int i = 0; i < FCADImage.FEntities.Entities.Count; i++)
                {
                    GetEntityName = FCADImage.FEntities.Entities[i].ToString();
                    if (GetEntityName == "DXFImportReco.DXFLine")
                    {
                        
                        dxLine = (DXFLine)FCADImage.FEntities.Entities[i];

                        blackPen.Color = dxLine.FColor;

                        P1 = GetPoint(dxLine.Point1);
                        P2 = GetPoint(dxLine.Point2);

                        graphics.DrawLine(blackPen, P1.X, P1.Y, P2.X, P2.Y);
                        
                    }
                    else if (GetEntityName == "DXFImportReco.DXFCircle")
                    {
                        dxCircle = (DXFCircle)FCADImage.FEntities.Entities[i];

                        blackPen.Color = dxCircle.FColor;

                        rd1 = dxCircle.radius;
                        P1 = GetPoint(dxCircle.Point1);
                        rd1 = rd1 * FScale;
                        P1.X = P1.X - rd1;
                        P1.Y = P1.Y - rd1;

                        if (dxCircle.SelectObjFlag == true)
                        {
                            graphics.DrawEllipse(SelectPen, P1.X, P1.Y, rd1 * 2, rd1 * 2);
                        }
                        else
                        {
                            graphics.DrawEllipse(blackPen, P1.X, P1.Y, rd1 * 2, rd1 * 2);
                        }
                    }
                    else if (GetEntityName == "DXFImportReco.DXFArc")
                    {
                        
                        dxArc = (DXFArc)FCADImage.FEntities.Entities[i];

                        blackPen.Color = dxArc.FColor;

                        if (dxArc.pt1.X == 0)
                        {
                            rd1 = dxArc.radius; //Math.Abs(dxArc.radius * dxArc.ratio);
                            rd2 = dxArc.radius;
                        }
                        else
                        {
                            rd1 = dxArc.radius;
                            rd2 = dxArc.radius; //Math.Abs(dxArc.radius * ratio);
                        }

                        rd1 = dxArc.radius;
                        P1 = GetPoint(dxArc.Point1);
                        rd1 = rd1 * FScale;
                        rd2 = rd2 * FScale;
                        P1.X = P1.X - rd1;
                        P1.Y = P1.Y - rd1;
                        float sA = -dxArc.startAngle, eA = -dxArc.endAngle;
                        if (dxArc.endAngle < dxArc.startAngle) sA = Conversion_Angle(sA);
                        eA -= sA;

                        if (eA == 0)
                        {
                            graphics.DrawEllipse(blackPen, P1.X, P1.Y, rd1 * 2, rd2 * 2);
                        }
                        else
                        {
                            graphics.DrawArc(blackPen, P1.X, P1.Y, rd1 * 2, rd2 * 2, 0, 360);//sA, eA);
                        }
                    }
                    else
                    {
                        //OutTxtStr += "\r\n" + GetEntityName;
                    }
                }
                //graphics.DrawLine(blackPen, 0, Dr_Cen_Y, Dr_Cen_X, Dr_Cen_Y);

                
            }
        }

        public SFPoint GetPoint(SFPoint Point)
        {
            SFPoint P;
            P.X = Base.X + FScale * (Point.X - FS_W / 2 - FS_W_Base);// * FParams.Scale.X);
            P.Y = Base.Y - FScale * (Point.Y - FS_H / 2 - FS_H_Base);// * FParams.Scale.Y);
            P.Z = Point.Z * FScale;
            return P;
        }

        /*
        public SFPoint GetPoint(SFPoint Point)
        {
            SFPoint P;
            P.X = Base.X + FScale * (Point.X);// * FParams.Scale.X);
            P.Y = Base.Y - FScale * (Point.Y);// * FParams.Scale.Y);
            P.Z = Point.Z * FScale;
            return P;
        }
        */

        public float Conversion_Angle(float Val)
        {
            while (Val < 0) Val = Val + 360;
            return Val;
        }

        private void timWork_Tick(object sender, EventArgs e)
        {
            int Cur_Temp_Zoom_Status, Cur_Temp_Focus_Status;
            int ZeroLoopCountChkMax = 10;
            int ZeroLoopCountChkMax_Shot = 5;
            short RotateIndex_Index;
            double IndexRoAngle = 0;
            //double TmpMoveXAxis = 0;
            //double TmpMoveYAxis = 0;
            //double TmpMoveZAxis = 0;
            double SetMoveValueAxis = 0;
            double TmpMoveValueAxis = 0;
            double Detail_Move_Pos_X, Detail_Move_Pos_Y, Detail_Move_Pos_Z;
            SFPoint GetDetailSelObj;
            short TmpMoveAxis_Index;
            bool TopAlignProcFlag = false;

            if (cboZoomCamList.SelectedIndex == 3)
            {
                TopAlignProcFlag = true;
            }

            if (cboIndexList.SelectedIndex == 0)
            {
                RotateIndex_Index = MultiMotion.INDEX_FIX_R;
            }
            else
            {
                RotateIndex_Index = MultiMotion.INDEX_MOVE_R;
            }
             
            if (LpWk_RunFlag == true)
            {
                timWork.Enabled = false;

                if (LpWk_RUN_STATUS == WORKSTART)
                {
                    LpWk_RUN_STATUS = MOTIONMOVE;
                    LpWk_ZeroCount = 0;
                    label5.Text = "WORKSTART";
                }
                else if (LpWk_RUN_STATUS == MOTIONMOVE)
                {
                    label5.Text = "MOTION이동";
                    if (LpWk_ZeroCount == 0)
                    {
                        LpWk_ZeroCount = 1;

                        if (TopAlignProcFlag == true)
                        {

                            MultiMotion.GetCurrentPos();
                            SetMoveValueAxis = RecoSetData.Motion_Move_Z;
                            TmpMoveAxis_Index = MultiMotion.CAM_UNIT_Z;
                            TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];

                            if (RecoSetData.Motion_Move_Z > 80)
                            {
                                MessageBox.Show("Z축 위치가 너무 낮습니다. 테스트를 종료합니다.");
                                timerAxis.Enabled = true;
                                return;
                            }

                            RunMotionAxis_Z(RecoSetData.Motion_Move_Z);
                            RunMotionAxis_X(RecoSetData.Motion_Move_X);
                            RunMotionAxis_Y(RecoSetData.Motion_Move_Y);


                            //MOTION이동부분
                            //=============================================
                            //MultiMotion.MoveAxis(MultiMotion.CAM_UNIT_X, RecoSetData.Motion_Move_X, true);
                            //MultiMotion.MoveAxis(MultiMotion.CAM_UNIT_Y, RecoSetData.Motion_Move_Y, true);
                            //MultiMotion.MoveAxis(MultiMotion.CAM_UNIT_Z, RecoSetData.Motion_Move_Z, true);

                            /*
                            SetMoveValueAxis = RecoSetData.Motion_Move_X;
                            TmpMoveAxis_Index = MultiMotion.CAM_UNIT_X;

                            TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
                            MultiMotion.MoveAxis(TmpMoveAxis_Index, SetMoveValueAxis, false);
                            while (Math.Abs(MultiMotion.AxisValue[TmpMoveAxis_Index] - TmpMoveValueAxis) > 1.0)
                            {
                                DelayWaitRun(5);
                            }
                            */

                            /*
                            SetMoveValueAxis = RecoSetData.Motion_Move_Y;
                            TmpMoveAxis_Index = MultiMotion.CAM_UNIT_Y;

                            TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
                            MultiMotion.MoveAxis(TmpMoveAxis_Index, SetMoveValueAxis, false);
                            while (Math.Abs(MultiMotion.AxisValue[TmpMoveAxis_Index] - TmpMoveValueAxis) > 1.0)
                            {
                                DelayWaitRun(5);
                            }
                            */

                            /*
                            SetMoveValueAxis = RecoSetData.Motion_Move_Z;
                            TmpMoveAxis_Index = MultiMotion.CAM_UNIT_Z;

                            TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
                            MultiMotion.MoveAxis(TmpMoveAxis_Index, SetMoveValueAxis, false);
                            while (Math.Abs(MultiMotion.AxisValue[TmpMoveAxis_Index] - TmpMoveValueAxis) > 1.0)
                            {
                                DelayWaitRun(5);
                            }
                            */

                            //=============================================

                        }
                        else
                        {
                            //MOTION이동부분
                            //=============================================
                            //MultiMotion.MoveAxis(MultiMotion.CAM_UNIT_X, RecoSetData.Motion_Move_X, true);
                            //MultiMotion.MoveAxis(MultiMotion.CAM_UNIT_Y, RecoSetData.Motion_Move_Y, true);
                            //MultiMotion.MoveAxis(MultiMotion.CAM_UNIT_Z, RecoSetData.Motion_Move_Z, true);

                            /*
                            SetMoveValueAxis = RecoSetData.Motion_Move_X;
                            TmpMoveAxis_Index = MultiMotion.CAM_UNIT_X;

                            TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
                            MultiMotion.MoveAxis(TmpMoveAxis_Index, SetMoveValueAxis, false);
                            while (Math.Abs(MultiMotion.AxisValue[TmpMoveAxis_Index] - TmpMoveValueAxis) > 1.0)
                            {
                                DelayWaitRun(5);
                            }
                            */
                            RunMotionAxis_X(RecoSetData.Motion_Move_X);

                            /*
                            SetMoveValueAxis = RecoSetData.Motion_Move_Y;
                            TmpMoveAxis_Index = MultiMotion.CAM_UNIT_Y;

                            TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
                            MultiMotion.MoveAxis(TmpMoveAxis_Index, SetMoveValueAxis, false);
                            while (Math.Abs(MultiMotion.AxisValue[TmpMoveAxis_Index] - TmpMoveValueAxis) > 1.0)
                            {
                                DelayWaitRun(5);
                            }
                            */
                            RunMotionAxis_Y(RecoSetData.Motion_Move_Y);

                            /*
                            SetMoveValueAxis = RecoSetData.Motion_Move_Z;
                            TmpMoveAxis_Index = MultiMotion.CAM_UNIT_Z;

                            TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
                            MultiMotion.MoveAxis(TmpMoveAxis_Index, SetMoveValueAxis, false);
                            while (Math.Abs(MultiMotion.AxisValue[TmpMoveAxis_Index] - TmpMoveValueAxis) > 1.0)
                            {
                                DelayWaitRun(5);
                            }
                            */
                            RunMotionAxis_Z(RecoSetData.Motion_Move_Z);

                            //=============================================
                        }


                    }
                    if (LpWk_ZeroCount > ZeroLoopCountChkMax)
                    {
                        LpWk_RUN_STATUS = GOFULLSHOT;
                        LpWk_ZeroCount = 0;
                    }
                    else
                    {
                        LpWk_ZeroCount++;

                    }
                }
                else if (LpWk_RUN_STATUS == GOFULLSHOT)
                {
                    label5.Text = "FULLSHOT보기";
                    if (LpWk_ZeroCount == 0)
                    {
                        StartCalcFlag = true;

                        Base.X = RealScaleBox.Image.Width / 2;
                        Base.Y = RealScaleBox.Image.Height / 2;

                        DrawDXFImageCalc(RealScaleBox.Image.Width, RealScaleBox.Image.Height, false);


                        CurZoomFactNum = ShotData[FULLSHOTTYPE].ZoomNum;
                        CurFocusFactNum = ShotData[FULLSHOTTYPE].FocusNum;
                        LpWk_ZeroCount = 1;
                    }
                    if (LpWk_ZeroCount > ZeroLoopCountChkMax)
                    {
                        LpWk_RUN_STATUS = FULLRECOPROC;
                        LpWk_ZeroCount = 0;
                    }
                    else
                    {
                        //if (NavitarCtrl.ProductID > 0)
                        if (NavitarCtrl.Connected == true)
                        {
                            Cur_Temp_Zoom_Status = NavitarCtrl.Read(REG_USER_STATUS_1);
                            if (Cur_Temp_Zoom_Status >= 512)
                            {
                                Cur_Temp_Zoom_Status = Cur_Temp_Zoom_Status - 512;
                            }

                            if (Cur_Temp_Zoom_Status == 0)
                            {
                                LpWk_ZeroCount++;
                            }
                            else
                            {
                                LpWk_ZeroCount = 1;
                            }
                        }
                    }
                }
                else if (LpWk_RUN_STATUS == FULLRECOPROC)
                {
                    label5.Text = "인식";
                    if (LpWk_ZeroCount == 0)
                    {
                        //picRecoImg.Image = RealScaleBox.Image;
                        //RecoBlankImg = RealScaleBox.Image;
                        RecoWorkImg = RecoBlankImg;
                        //RealScaleBox.Image.Save("001_1.jpg");

                        if (TopAlignProcFlag == true)
                        {
                            if (RecoWorkImg != null)
                            {
                                /*
                                RecoCls = new clsRecoProcess(ref RecoWorkImg);
                                RecoCls.FindEdge(0);
                                RecoCls.CutOffFindEdge();
                                RecoCls.FindObject();
                                RecoCls.FindCircleArc();
                                RecoCls.FindObjectCenterAngle();
                                picRecoImg.Image = RecoCls.GetArrObjectImage();
                                */

                                RecoCls = new clsRecoProcess(ref RecoWorkImg, CamOpticalGap_X[CamOpticalGapIndex], CamOpticalGap_Y[CamOpticalGapIndex]);
                                RecoCls.FindEdge2(1);
                                RecoCls.CutOffFindEdge2();
                                RecoCls.FindObject();
                                RecoCls.FindCircleObjSizeArc();
                                RecoCls.FindCircleDetailShot();

                                IndexFindRoAngle = RecoCls.FindTopCenterAngle(18+6.35, 8*2);
                                picRecoImg.Image = RecoCls.GetArrObjectImage();
                            }
                        }
                        else
                        {
                            if (RecoWorkImg != null)
                            {
                                /*
                                RecoCls = new clsRecoProcess(ref RecoWorkImg);
                                RecoCls.FindEdge(0);
                                RecoCls.CutOffFindEdge();
                                RecoCls.FindObject();
                                RecoCls.FindCircleArc();
                                RecoCls.FindObjectCenterAngle();
                                picRecoImg.Image = RecoCls.GetArrObjectImage();
                                */
                                bool LineCheckFlag;
                                LineCheckFlag = false;
                                GetDXFObjList();
                                for (int i = 0; i < DXFOBJLIST_MAX; i++)
                                {
                                    if (DXFObjList[i].ExistLyFlag == true)
                                    {
                                        if (DXFObjList[i].LyType == SHAPE_LINE)
                                        {
                                            LineCheckFlag = true;
                                        }
                                    }
                                }
                                RecoCls = new clsRecoProcess(ref RecoWorkImg, CamOpticalGap_X[CamOpticalGapIndex], CamOpticalGap_Y[CamOpticalGapIndex]);
                                RecoCls.FindEdge2(1);
                                RecoCls.CutOffFindEdge2();
                                RecoCls.FindObject();
                                RecoCls.FindCircleObjSizeArc();
                                //RecoCls.FindGetLineData2();
                                if (LineCheckFlag == true)
                                {
                                    picRecoImg.Image = RecoCls.GetLineArrDiplayImage();
                                }
                                RecoCls.ClearDXFDataList();
                                for (int i = 0; i < DXFOBJLIST_MAX; i++)
                                {
                                    if (DXFObjList[i].ExistLyFlag == true)
                                    {
                                        RecoCls.AddDXFDataList(DXFObjList[i]);
                                    }
                                }
                                RecoCls.FindObjectCenterAngle3();
                                IndexFindRoAngle = RecoCls.GetFullAngle;
                                picRecoImg.Image = RecoCls.GetArrObjectImage();
                            }
                        }



                        PanelPicture.Visible = false;
                        picRecoImg.Visible = true;
                        LpWk_ZeroCount = 1;

                        /*
                        DXFLayerInfo FindDXFSelObj;
                        FindDXFSelObj = FindSelectDXFObjList();
                        if (FindDXFSelObj.ExistLyFlag == true)
                        {
                            RecoCls = new clsRecoProcess(ref RecoBlankImg);
                            RecoCls.FindEdge2(1);
                            RecoCls.CutOffFindEdge2();
                            //picRecoImg.Image = RecoCls.GetArrDiplayImage();
                            //picRecoImg.Image = RecoCls.GetArrGrayDiplayImage();

                            RecoCls.FindObject();
                            RecoCls.FindCircleObjSizeArc();
                            RecoCls.FindCircleDetailShot();
                            //RecoCls.HoughTransform();
                            //RecoCls.FindGetLineData2();
                            //picRecoImg.Image = RecoCls.GetLineArrDiplayImage();
                            //picRecoImg.Image = RecoCls.GetArrDiplayImage();
                            //picRecoImg.Image = RecoCls.GetArrObjectListDiplayImage();
                            double IndexRoAngle111 = RecoCls.FindTopCenterAngle(FindDXFSelObj.Obj_Dia);
                            picRecoImg.Image = RecoCls.GetArrDetailObjectImage(IndexRoAngle111);
                            //DrawDXFImage((Bitmap)picRecoImg.Image);
                        }
                        */
                    }
                    if (LpWk_ZeroCount > ZeroLoopCountChkMax)
                    {
                        LpWk_RUN_STATUS = INDEXROTATE_FULL;
                        LpWk_ZeroCount = 0;
                    }
                    else
                    {
                        LpWk_ZeroCount++;
                    }
                }
                else if (LpWk_RUN_STATUS == INDEXROTATE_FULL)
                {
                    label5.Text = "INDEX회전";
                    if (LpWk_ZeroCount == 0)
                    {
                        LpWk_ZeroCount = 1;
                        IndexFindRoAngle = IndexRotationHalf(IndexFindRoAngle);
                        MultiMotion.GetCurrentPos();
                        IndexRoAngle = MultiMotion.AxisValue[RotateIndex_Index];
                        if (RotateIndex_Index == MultiMotion.INDEX_MOVE_R)
                        {
                            IndexRoAngle = IndexRoAngle + IndexFindRoAngle;
                        }
                        else
                        {
                            IndexRoAngle = IndexRoAngle - IndexFindRoAngle;
                        }
                        
                        //INDEX회전부분
                        //=============================================
                        MultiMotion.MoveAxis(RotateIndex_Index, IndexRoAngle, false);

                        //MultiMotion.MoveAxis(0, 10.0, true);  // 완료 후 리턴

                        //MultiMotion.MoveAxis(0, -10.0, true); // 완료 후 리턴

                        //MultiMotion.RotateAxis(0, 10.0);
                        //=============================================
                        PanelPicture.Visible = true;
                        picRecoImg.Visible = false;
                        MultiMotion.GetCurrentPos();
                        while (Math.Abs(MultiMotion.AxisValue[RotateIndex_Index] - IndexRoAngle) > 1.0)
                        {
                            MultiMotion.GetCurrentPos();
                            DelayWaitRun(5);
                        }
                        DelayWaitRun(10);
                    }
                    if (LpWk_ZeroCount > ZeroLoopCountChkMax)
                    {
                        LpWk_RUN_STATUS = FULLRECOPROC2;
                        LpWk_ZeroCount = 0;
                    }
                    else
                    {
                        LpWk_ZeroCount++;

                    }
                }
                else if (LpWk_RUN_STATUS == FULLRECOPROC2)
                {
                    label5.Text = "인식2";
                    if (LpWk_ZeroCount == 0)
                    {
                        //picRecoImg.Image = RealScaleBox.Image;
                        //RecoBlankImg = RealScaleBox.Image;
                        RecoWorkImg = RecoBlankImg;
                        //RealScaleBox.Image.Save("001_2.jpg");
                        if (TopAlignProcFlag == true)
                        {
                            if (RecoWorkImg != null)
                            {
                                /*
                                RecoCls = new clsRecoProcess(ref RecoWorkImg);
                                RecoCls.FindEdge(0);
                                RecoCls.CutOffFindEdge();
                                RecoCls.FindObject();
                                RecoCls.FindCircleArc();
                                RecoCls.FindObjectCenterAngle();
                                picRecoImg.Image = RecoCls.GetArrObjectImage();
                                */

                                RecoCls = new clsRecoProcess(ref RecoWorkImg, CamOpticalGap_X[CamOpticalGapIndex], CamOpticalGap_Y[CamOpticalGapIndex]);
                                RecoCls.FindEdge2(1);
                                RecoCls.CutOffFindEdge2();
                                RecoCls.FindObject();
                                RecoCls.FindCircleObjSizeArc();
                                RecoCls.FindCircleDetailShot();

                                IndexFindRoAngle = RecoCls.FindTopCenterAngle(18 + 6.35, 8);
                                picRecoImg.Image = RecoCls.GetArrObjectImage();
                            }
                        }
                        else
                        {
                            if (RecoWorkImg != null)
                            {
                                /*
                                RecoCls = new clsRecoProcess(ref RecoWorkImg);
                                RecoCls.FindEdge(0);
                                RecoCls.CutOffFindEdge();
                                RecoCls.FindObject();
                                RecoCls.FindCircleArc();
                                RecoCls.FindObjectCenterAngle();
                                picRecoImg.Image = RecoCls.GetArrObjectImage();
                                */
                                bool LineCheckFlag;
                                LineCheckFlag = false;
                                GetDXFObjList();
                                for (int i = 0; i < DXFOBJLIST_MAX; i++)
                                {
                                    if (DXFObjList[i].ExistLyFlag == true)
                                    {
                                        if (DXFObjList[i].LyType == SHAPE_LINE)
                                        {
                                            LineCheckFlag = true;
                                        }
                                    }
                                }
                                RecoCls = new clsRecoProcess(ref RecoWorkImg, CamOpticalGap_X[CamOpticalGapIndex], CamOpticalGap_Y[CamOpticalGapIndex]);
                                RecoCls.FindEdge2(1);
                                RecoCls.CutOffFindEdge2();
                                RecoCls.FindObject();
                                RecoCls.FindCircleObjSizeArc();
                                //RecoCls.FindGetLineData2();
                                if (LineCheckFlag == true)
                                {
                                    picRecoImg.Image = RecoCls.GetLineArrDiplayImage();
                                }
                                RecoCls.ClearDXFDataList();
                                for (int i = 0; i < DXFOBJLIST_MAX; i++)
                                {
                                    if (DXFObjList[i].ExistLyFlag == true)
                                    {
                                        RecoCls.AddDXFDataList(DXFObjList[i]);
                                    }
                                }
                                RecoCls.FindObjectCenterAngle3();
                                IndexFindRoAngle = RecoCls.GetFullAngle;
                                picRecoImg.Image = RecoCls.GetArrObjectImage();

                                /*
                                GetDXFObjList();
                                RecoCls = new clsRecoProcess(ref RecoWorkImg);
                                RecoCls.FindEdge2(1);
                                RecoCls.CutOffFindEdge2();
                                RecoCls.FindObject();
                                RecoCls.FindCircleObjSizeArc();
                                //RecoCls.FindGetLineData2();
                                picRecoImg.Image = RecoCls.GetLineArrDiplayImage();
                                RecoCls.ClearDXFDataList();
                                for (int i = 0; i < DXFOBJLIST_MAX; i++)
                                {
                                    if (DXFObjList[i].ExistLyFlag == true)
                                    {
                                        RecoCls.AddDXFDataList(DXFObjList[i]);
                                    }
                                }
                                RecoCls.FindObjectCenterAngle2();
                                IndexFindRoAngle = RecoCls.GetFullAngle;
                                picRecoImg.Image = RecoCls.GetArrObjectImage();
                                */
                            }
                        }


                        PanelPicture.Visible = false;
                        picRecoImg.Visible = true;

                        LpWk_ZeroCount = 1;
                    }
                    if (LpWk_ZeroCount > ZeroLoopCountChkMax)
                    {
                        LpWk_RUN_STATUS = INDEXROTATE_FULL2;
                        LpWk_ZeroCount = 0;
                    }
                    else
                    {
                        LpWk_ZeroCount++;

                    }
                }
                else if (LpWk_RUN_STATUS == INDEXROTATE_FULL2)
                {
                    label5.Text = "INDEX회전2";
                    if (LpWk_ZeroCount == 0)
                    {
                        LpWk_ZeroCount = 1;

                        IndexFindRoAngle = IndexRotationHalf(IndexFindRoAngle);

                        MultiMotion.GetCurrentPos();
                        IndexRoAngle = MultiMotion.AxisValue[RotateIndex_Index];
                        if (RotateIndex_Index == MultiMotion.INDEX_MOVE_R)
                        {
                            IndexRoAngle = IndexRoAngle + IndexFindRoAngle;
                        }
                        else
                        {
                            IndexRoAngle = IndexRoAngle - IndexFindRoAngle;
                        }

                        //INDEX회전부분
                        //=============================================
                        MultiMotion.MoveAxis(RotateIndex_Index, IndexRoAngle, false);

                        //MultiMotion.MoveAxis(0, 10.0, true);  // 완료 후 리턴

                        //MultiMotion.MoveAxis(0, -10.0, true); // 완료 후 리턴

                        //MultiMotion.RotateAxis(0, 10.0);
                        //=============================================
                        PanelPicture.Visible = true;
                        picRecoImg.Visible = false;
                        while (Math.Abs(MultiMotion.AxisValue[RotateIndex_Index] - IndexRoAngle) > 1.0)
                        {
                            MultiMotion.GetCurrentPos();
                            DelayWaitRun(5);
                        }
                        DelayWaitRun(10);

                    }
                    if (LpWk_ZeroCount > ZeroLoopCountChkMax)
                    {
                        picRecoImg.Visible = false;
                        PanelPicture.Visible = true;
                        PanelPicture.Refresh();
                        LpWk_RUN_STATUS = DETAILMOTIONMOVE;
                        LpWk_ZeroCount = 0;
                    }
                    else
                    {
                        LpWk_ZeroCount++;
                    }
                }
                else if (LpWk_RUN_STATUS == DETAILMOTIONMOVE)
                {
                    label5.Text = "MOTION이동";
                    if (LpWk_ZeroCount == 0)
                    {
                        LpWk_ZeroCount = 1;

                        if (TopAlignProcFlag == true)
                        {
                            label5.Text = "테스트끝";
                            LpWk_RunFlag = false;
                            timWork.Enabled = false;
                            btLoopStart.Enabled = true;
                            btLoopStop.Enabled = false;
                            MessageBox.Show("테스트종료!");
                            timerAxis.Enabled = true;
                            return;
                        }

                        GetDetailSelObj = GetSelectDXFData();
                        Detail_Move_Pos_X = 0;
                        Detail_Move_Pos_Y = 0 - GetDetailSelObj.Y;
                        Detail_Move_Pos_Z = GetDetailSelObj.Z;


                        //DETAIL_MOTION이동부분
                        //=============================================
                        //MultiMotion.MoveAxis(MultiMotion.CAM_UNIT_X, RecoSetData.Motion_Move_X, true);
                        //MultiMotion.MoveAxis(MultiMotion.CAM_UNIT_Y, RecoSetData.Motion_Move_Y, true);
                        //MultiMotion.MoveAxis(MultiMotion.CAM_UNIT_Z, RecoSetData.Motion_Move_Z, true);

                        /*
                        SetMoveValueAxis = RecoSetData.Motion_Move_X;
                        TmpMoveAxis_Index = MultiMotion.CAM_UNIT_X;

                        TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
                        MultiMotion.MoveAxis(TmpMoveAxis_Index, SetMoveValueAxis, false);
                        while (Math.Abs(MultiMotion.AxisValue[TmpMoveAxis_Index] - TmpMoveValueAxis) > 1.0)
                        {
                            DelayWaitRun(100);
                        }
                        */

                        MultiMotion.GetCurrentPos();
                        TmpMoveAxis_Index = MultiMotion.CAM_UNIT_Y;
                        TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
                        if (RotateIndex_Index == MultiMotion.INDEX_MOVE_R)
                        {
                            SetMoveValueAxis = TmpMoveValueAxis + Detail_Move_Pos_Y;
                        }
                        else
                        {
                            SetMoveValueAxis = TmpMoveValueAxis - Detail_Move_Pos_Y;
                        }
                        /*
                        MultiMotion.MoveAxis(TmpMoveAxis_Index, SetMoveValueAxis, false);
                        while (Math.Abs(MultiMotion.AxisValue[TmpMoveAxis_Index] - TmpMoveValueAxis) > 1.0)
                        {
                            DelayWaitRun(5);
                        }
                        */
                        RunMotionAxis_Y(SetMoveValueAxis);


                        MultiMotion.GetCurrentPos();
                        TmpMoveAxis_Index = MultiMotion.CAM_UNIT_Z;
                        TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
                        SetMoveValueAxis = TmpMoveValueAxis - Detail_Move_Pos_Z;
                        /*
                        MultiMotion.MoveAxis(TmpMoveAxis_Index, SetMoveValueAxis, false);
                        while (Math.Abs(MultiMotion.AxisValue[TmpMoveAxis_Index] - TmpMoveValueAxis) > 1.0)
                        {
                            DelayWaitRun(5);
                        }
                        */
                        RunMotionAxis_Z(SetMoveValueAxis);

                        //MultiMotion.MoveAxis(0, 10.0, true);  // 완료 후 리턴
                        //MultiMotion.MoveAxis(0, -10.0, true); // 완료 후 리턴
                        //MultiMotion.RotateAxis(0, 10.0);
                        //=============================================

                    }
                    if (LpWk_ZeroCount > ZeroLoopCountChkMax)
                    {
                        LpWk_RUN_STATUS = GODETAILSHOT;
                        LpWk_ZeroCount = 0;
                    }
                    else
                    {
                        LpWk_ZeroCount++;

                    }
                }
                else if (LpWk_RUN_STATUS == GODETAILSHOT)
                {
                    label5.Text = "DETAILSHOT보기";
                    if (LpWk_ZeroCount == 0)
                    {
                        SelectDXFReScale(RealScaleBox.Image.Width, RealScaleBox.Image.Height);

                        CurZoomFactNum = ShotData[DETAILSHOTTYPE].ZoomNum;
                        CurFocusFactNum = ShotData[DETAILSHOTTYPE].FocusNum;
                        LpWk_ZeroCount = 1;
                    }
                    if (LpWk_ZeroCount > ZeroLoopCountChkMax)
                    {
                        LpWk_RUN_STATUS = DETAILRECOPROC;
                        LpWk_ZeroCount = 0;
                    }
                    else
                    {
                        //if (NavitarCtrl.ProductID > 0)
                        if (NavitarCtrl.Connected == true)
                        {
                            Cur_Temp_Zoom_Status = NavitarCtrl.Read(REG_USER_STATUS_1);
                            if (Cur_Temp_Zoom_Status >= 512)
                            {
                                Cur_Temp_Zoom_Status = Cur_Temp_Zoom_Status - 512;
                            }
                            if (Cur_Temp_Zoom_Status == 0)
                            {
                                LpWk_ZeroCount++;
                            }
                            else
                            {
                                LpWk_ZeroCount = 1;
                            }
                        }
                    }
                }
                else if (LpWk_RUN_STATUS == DETAILRECOPROC)
                {
                    label5.Text = "세부인식";
                    if (LpWk_ZeroCount == 0)
                    {
                        //picRecoImg.Image = RealScaleBox.Image;
                        //RecoBlankImg = RealScaleBox.Image;
                        RecoWorkImg = RecoBlankImg;
                        RealScaleBox.Image.Save("001_3.jpg");
                        if (RecoWorkImg != null)
                        {
                            /*
                            RecoCls = new clsRecoProcess(ref RecoWorkImg);
                            RecoCls.FindEdge(0);
                            RecoCls.CutOffFindEdge();
                            RecoCls.FindObject();
                            RecoCls.FindCircleArc();
                            RecoCls.FindObjectCenterAngle();
                            picRecoImg.Image = RecoCls.GetArrObjectImage();
                            */
                            DXFLayerInfo FindDXFSelObj;
                            FindDXFSelObj = FindSelectDXFObjList();
                            if (FindDXFSelObj.ExistLyFlag == true)
                            {
                                RecoCls = new clsRecoProcess(ref RecoWorkImg, CamOpticalGap_X[CamOpticalGapIndex], CamOpticalGap_Y[CamOpticalGapIndex]);
                                RecoCls.FindEdge2(1);
                                RecoCls.CutOffFindEdge2();
                                RecoCls.FindObject();
                                RecoCls.FindCircleObjSizeArc();
                                RecoCls.FindCircleDetailShot();
                                IndexFindRoAngle = RecoCls.FindDetailCenterAngle(FindDXFSelObj.Obj_X1, FindDXFSelObj.Obj_Y1, FindDXFSelObj.Obj_Dia);
                                picRecoImg.Image = RecoCls.GetArrDetailObjectImage(IndexFindRoAngle);
                            }
                        }

                        PanelPicture.Visible = false;
                        picRecoImg.Visible = true;


                        LpWk_ZeroCount = 1;
                    }
                    if (LpWk_ZeroCount > ZeroLoopCountChkMax)
                    {
                        LpWk_RUN_STATUS = INDEXROTATE_DETAIL;
                        LpWk_ZeroCount = 0;
                    }
                    else
                    {

                        LpWk_ZeroCount++;
                    }
                }
                else if (LpWk_RUN_STATUS == INDEXROTATE_DETAIL)
                {
                    label5.Text = "INDEX회전";
                    if (LpWk_ZeroCount == 0)
                    {
                        LpWk_ZeroCount = 1;

                        MultiMotion.GetCurrentPos();
                        IndexRoAngle = MultiMotion.AxisValue[RotateIndex_Index];
                        if (RotateIndex_Index == MultiMotion.INDEX_MOVE_R)
                        {
                            IndexRoAngle = IndexRoAngle + IndexFindRoAngle;
                        }
                        else
                        {
                            IndexRoAngle = IndexRoAngle - IndexFindRoAngle;
                        }

                        //INDEX회전부분
                        //=============================================
                        MultiMotion.MoveAxis(RotateIndex_Index, IndexRoAngle, false);

                        //MultiMotion.MoveAxis(0, 10.0, true);  // 완료 후 리턴

                        //MultiMotion.MoveAxis(0, -10.0, true); // 완료 후 리턴

                        //MultiMotion.RotateAxis(0, 10.0);
                        //=============================================
                        PanelPicture.Visible = true;
                        picRecoImg.Visible = false;
                        while (Math.Abs(MultiMotion.AxisValue[RotateIndex_Index] - IndexRoAngle) > 1.0)
                        {
                            MultiMotion.GetCurrentPos();
                            DelayWaitRun(5);
                        }
                        DelayWaitRun(10);

                    }
                    if (LpWk_ZeroCount > ZeroLoopCountChkMax)
                    {
                        LpWk_RUN_STATUS = DETAILRECOPROC2;
                        LpWk_ZeroCount = 0;
                    }
                    else
                    {

                        LpWk_ZeroCount++;
                    }
                }
                else if (LpWk_RUN_STATUS == DETAILRECOPROC2)
                {
                    label5.Text = "세부인식2";
                    if (LpWk_ZeroCount == 0)
                    {
                        //picRecoImg.Image = RealScaleBox.Image;
                        //RecoBlankImg = RealScaleBox.Image;
                        RecoWorkImg = RecoBlankImg;
                        RealScaleBox.Image.Save("001_4.jpg");
                        if (RecoWorkImg != null)
                        {
                            /*
                            RecoCls = new clsRecoProcess(ref RecoWorkImg);
                            RecoCls.FindEdge(0);
                            RecoCls.CutOffFindEdge();
                            RecoCls.FindObject();
                            RecoCls.FindCircleArc();
                            RecoCls.FindObjectCenterAngle();
                            picRecoImg.Image = RecoCls.GetArrObjectImage();
                            */
                            DXFLayerInfo FindDXFSelObj;
                            FindDXFSelObj = FindSelectDXFObjList();
                            if (FindDXFSelObj.ExistLyFlag == true)
                            {
                                RecoCls = new clsRecoProcess(ref RecoWorkImg, CamOpticalGap_X[CamOpticalGapIndex], CamOpticalGap_Y[CamOpticalGapIndex]);
                                RecoCls.FindEdge2(1);
                                RecoCls.CutOffFindEdge2();
                                RecoCls.FindObject();
                                RecoCls.FindCircleObjSizeArc();
                                RecoCls.FindCircleDetailShot();
                                IndexFindRoAngle = RecoCls.FindDetailCenterAngle(FindDXFSelObj.Obj_X1, FindDXFSelObj.Obj_Y1, FindDXFSelObj.Obj_Dia);
                                picRecoImg.Image = RecoCls.GetArrDetailObjectImage(IndexFindRoAngle);
                            }
                        }
                        PanelPicture.Visible = false;
                        picRecoImg.Visible = true;

                        LpWk_ZeroCount = 1;
                    }
                    if (LpWk_ZeroCount > ZeroLoopCountChkMax)
                    {
                        LpWk_RUN_STATUS = INDEXROTATE_DETAIL2;
                        LpWk_ZeroCount = 0;
                    }
                    else
                    {

                        LpWk_ZeroCount++;
                    }
                }
                else if (LpWk_RUN_STATUS == INDEXROTATE_DETAIL2)
                {
                    label5.Text = "INDEX회전2";
                    if (LpWk_ZeroCount == 0)
                    {
                        LpWk_ZeroCount = 1;

                        MultiMotion.GetCurrentPos();
                        IndexRoAngle = MultiMotion.AxisValue[RotateIndex_Index];
                        if (RotateIndex_Index == MultiMotion.INDEX_MOVE_R)
                        {
                            IndexRoAngle = IndexRoAngle + IndexFindRoAngle;
                        }
                        else
                        {
                            IndexRoAngle = IndexRoAngle - IndexFindRoAngle;
                        }

                        //INDEX회전부분
                        //=============================================
                        MultiMotion.MoveAxis(RotateIndex_Index, IndexRoAngle, false);

                        //MultiMotion.MoveAxis(0, 10.0, true);  // 완료 후 리턴

                        //MultiMotion.MoveAxis(0, -10.0, true); // 완료 후 리턴

                        //MultiMotion.RotateAxis(0, 10.0);
                        //=============================================
                        PanelPicture.Visible = true;
                        picRecoImg.Visible = false;
                        while (Math.Abs(MultiMotion.AxisValue[RotateIndex_Index] - IndexRoAngle) > 1.0)
                        {
                            MultiMotion.GetCurrentPos();
                            DelayWaitRun(5);
                        }
                        DelayWaitRun(10);

                    }
                    if (LpWk_ZeroCount > ZeroLoopCountChkMax)
                    {
                        picRecoImg.Visible = false;
                        PanelPicture.Visible = true;
                        PanelPicture.Refresh();
                        LpWk_RUN_STATUS = WORKEND;
                        LpWk_ZeroCount = 0;
                    }
                    else
                    {

                        LpWk_ZeroCount++;
                    }
                }
                else if (LpWk_RUN_STATUS == WORKEND)
                {
                    label5.Text = "테스트끝";
                    if (LpWk_ZeroCount == ZeroLoopCountChkMax_Shot)
                    {
                        //RealScaleBox.Image.Save("shot" + LpWk_LoopCount.ToString() + ".jpg");
                        LpWk_LoopCount++;
                    }
                    if (LpWk_ZeroCount > ZeroLoopCountChkMax)
                    {
                        LpWk_RUN_STATUS = WORKSTART;
                        LpWk_ZeroCount = 0;
                    }
                    else
                    {
                        LpWk_ZeroCount++;
                    }
                }
                if (LpWk_LoopCount >= LpWk_LoopCountMax)
                {
                    label5.Text = "";
                    LpWk_RunFlag = false;
                    timWork.Enabled = false;
                    btLoopStart.Enabled = true;
                    btLoopStop.Enabled = false;
                    MessageBox.Show("테스트종료!");
                }
                else
                {
                    timWork.Enabled = true;
                }
            }
            LpWk_RunTimerCount++;
        }

        private string GetNewDXFFileName(string FindSubFolderStr)
        {
            string NewDXFFileStr;
            int NewIncNum;

            NewIncNum = 0;

            do
            {
                NewIncNum++;
                NewDXFFileStr =FindSubFolderStr + NewIncNum.ToString() + ".dxf";
            } while (File.Exists(NewDXFFileStr));
            NewDXFFileStr = NewIncNum.ToString() + ".dxf";
            return NewDXFFileStr;
        }

        private string GetNewRecoDataFileName(string FindSubFolderStr)
        {
            string NewRecoDatFileStr;
            int NewIncNum;

            NewIncNum = 0;

            do
            {
                NewIncNum++;
                NewRecoDatFileStr = FindSubFolderStr + @"\" + NewIncNum.ToString() + ".dat";
            } while (File.Exists(NewRecoDatFileStr));
            NewRecoDatFileStr = NewIncNum.ToString();
            return NewRecoDatFileStr;
        }

        private void btFullShot_Click(object sender, EventArgs e)
        {
            if (RunCamPlayFlag == false)
            {
                return;
            }
            Base.X = RealScaleBox.Image.Width / 2;
            Base.Y = RealScaleBox.Image.Height / 2;
            DrawDXFImageCalc(RealScaleBox.Image.Width, RealScaleBox.Image.Height, false);

            if (FullDetailShotFlag == DETAILSHOTTYPE)
            {
                StartCalcFlag = true;

                btDetailShot.ForeColor = Color.Black;
                btDetailShot.Font = new Font(btDetailShot.Font, FontStyle.Regular);

                btFullShot.ForeColor = Color.Red;
                btFullShot.Font = new Font(btFullShot.Font, FontStyle.Bold);

                FullDetailShotFlag = FULLSHOTTYPE;
            }
            if (ShotData[FULLSHOTTYPE].ZoomNum > -1 && ShotData[FULLSHOTTYPE].FocusNum > -1)
            {
                /*
                CurZoomFactNum = MinusCalcValue(ShotData[FULLSHOTTYPE].ZoomNum);
                CurFocusFactNum = MinusCalcValue(ShotData[FULLSHOTTYPE].FocusNum);

                DelayWaitRun(10);
                while (Math.Abs(CurZoomFactNum - CurRealZoomFactNum) > 5)
                {
                    DelayWaitRun(5);
                }
                DelayWaitRun(10);
                */

                CurZoomFactNum = ShotData[FULLSHOTTYPE].ZoomNum;
                CurFocusFactNum = ShotData[FULLSHOTTYPE].FocusNum;

                trackBar1.Value = CurZoomFactNum;
                trackBar2.Value = CurFocusFactNum;


            }
            FullShotMove();
        }

        private void btDetailShot_Click(object sender, EventArgs e)
        {
            if (RunCamPlayFlag == false)
            {
                return;
            }
            SelectDXFReScale(RealScaleBox.Image.Width, RealScaleBox.Image.Height);

            if (FullDetailShotFlag == FULLSHOTTYPE)
            {
                btFullShot.ForeColor = Color.Black;
                btFullShot.Font = new Font(btFullShot.Font, FontStyle.Regular);

                btDetailShot.ForeColor = Color.Red;
                btDetailShot.Font = new Font(btDetailShot.Font, FontStyle.Bold);

                FullDetailShotFlag = DETAILSHOTTYPE;
            }
            if (ShotData[DETAILSHOTTYPE].ZoomNum > -1 && ShotData[DETAILSHOTTYPE].FocusNum > -1)
            {
                CurZoomFactNum = ShotData[DETAILSHOTTYPE].ZoomNum;
                CurFocusFactNum = ShotData[DETAILSHOTTYPE].FocusNum;

                trackBar1.Value = CurZoomFactNum;
                trackBar2.Value = CurFocusFactNum;
            }

            DetailShotSelectObjMove();
        }

        private void DXFMoveUp()
        {
            Base.Y = Base.Y - 2;
        }

        private void DXFMoveDown()
        {
            Base.Y = Base.Y + 2;
        }

        private void DXFMoveLeft()
        {
            Base.X = Base.X - 2;
        }

        private void DXFMoveRight()
        {
            Base.X = Base.X + 2;
        }

        private void timDXFWork_Tick(object sender, EventArgs e)
        {
            if (DXF_Edit_Cur_Sts == DXF_EDIT_UP)
            {
                DXFMoveUp();
            }
            else if (DXF_Edit_Cur_Sts == DXF_EDIT_DOWN)
            {
                DXFMoveDown();
            }
            else if (DXF_Edit_Cur_Sts == DXF_EDIT_LEFT)
            {
                DXFMoveLeft();
            }
            else if (DXF_Edit_Cur_Sts == DXF_EDIT_RIGHT)
            {
                DXFMoveRight();
            }
            else if (DXF_Edit_Cur_Sts == DXF_EDIT_ZOOMIN)
            {
                DXFZoomIn();
            }
            else if (DXF_Edit_Cur_Sts == DXF_EDIT_ZOOMOUT)
            {
                DXFZoomOut();
            }
        }

        private void btDXFUp_MouseDown(object sender, MouseEventArgs e)
        {
            DXF_Edit_Cur_Sts = DXF_EDIT_UP;
        }

        private void btDXFUp_MouseUp(object sender, MouseEventArgs e)
        {
            DXF_Edit_Cur_Sts = DXF_EDIT_NONE;
        }

        private void btDXFLeft_MouseDown(object sender, MouseEventArgs e)
        {
            DXF_Edit_Cur_Sts = DXF_EDIT_LEFT;
        }

        private void btDXFLeft_MouseUp(object sender, MouseEventArgs e)
        {
            DXF_Edit_Cur_Sts = DXF_EDIT_NONE;
        }

        private void btDXFDown_MouseDown(object sender, MouseEventArgs e)
        {
            DXF_Edit_Cur_Sts = DXF_EDIT_DOWN;
        }

        private void btDXFDown_MouseUp(object sender, MouseEventArgs e)
        {
            DXF_Edit_Cur_Sts = DXF_EDIT_NONE;
        }

        private void btDXFRight_MouseDown(object sender, MouseEventArgs e)
        {
            DXF_Edit_Cur_Sts = DXF_EDIT_RIGHT;
        }

        private void btDXFRight_MouseUp(object sender, MouseEventArgs e)
        {
            DXF_Edit_Cur_Sts = DXF_EDIT_NONE;
        }

        private void DXFZoomOut()
        {
            if (FCADImage != null)
            {
                FScale = FScale / 1.005f;
                /*
                float Tmp_FS_W, Tmp_FS_H;

                Tmp_FS_W = FS_W * FScale;
                Tmp_FS_H = FS_H * FScale;

                FScale = FScale / 1.1f;
                */
                //Base.X = Base.X + (int)(Tmp_FS_W - FS_W * FScale) / 2;
                //Base.Y = Base.Y - (int)(Tmp_FS_H - FS_H * FScale) / 2;
                /*
                FCADImage.FScale = FCADImage.FScale / (float)1.2;
                FCADImage.Base.Y = Bottom - 400;
                FCADImage.Base.X = 100 - (int)((FCADImage.FScale - 1.0f) * 140.0f);
                timer1.Interval = 300;
                timer1.Enabled = true;
                this.Invalidate();
                */
            }
        }

        private void DXFZoomIn()
        {
            if (FCADImage != null)
            {
                FScale = FScale * 1.005f;
                /*
                float Tmp_FS_W, Tmp_FS_H;

                Tmp_FS_W = FS_W * FScale;
                Tmp_FS_H = FS_H * FScale;

                label5.Text = Base.X.ToString() + "," + Base.Y.ToString() + "|" + FS_W.ToString() + "," + FS_H.ToString() + "," + FScale + "," + Base.X + "," + Base.Y;

                FScale = FScale * 1.1f;

                //Base.X = Base.X - (int)(Tmp_FS_W - FS_W * FScale) / 2;
                //Base.Y = Base.Y + (int)(Tmp_FS_H - FS_H * FScale) / 2;

                label2.Text = Base.X.ToString() + "," + Base.Y.ToString() + "|" + Tmp_FS_W.ToString() + "," + Tmp_FS_H.ToString() + "," + FScale + "," + Base.X + "," + Base.Y;

                //label6.Text = Base.X.ToString() + "," + Base.Y.ToString();
                */

                /*
                FCADImage.FScale = FCADImage.FScale * (float)1.2;
                FCADImage.Base.Y = Bottom - 400;
                FCADImage.Base.X = 100 - (int)((FCADImage.FScale - 1.0f) * 140.0f);
                timer1.Interval = 300;
                timer1.Enabled = true;
                this.Invalidate();
                */
            }
        }

        private void btZoomOut_MouseDown(object sender, MouseEventArgs e)
        {
            DXF_Edit_Cur_Sts = DXF_EDIT_ZOOMOUT;
        }

        private void btZoomOut_MouseUp(object sender, MouseEventArgs e)
        {
            DXF_Edit_Cur_Sts = DXF_EDIT_NONE;
        }

        private void btZoomIn_MouseDown(object sender, MouseEventArgs e)
        {
            DXF_Edit_Cur_Sts = DXF_EDIT_ZOOMIN;
        }

        private void btZoomIn_MouseUp(object sender, MouseEventArgs e)
        {
            DXF_Edit_Cur_Sts = DXF_EDIT_NONE;
        }

        private void btZoomIn_Click(object sender, EventArgs e)
        {
            DXF_Edit_Cur_Sts = DXF_EDIT_ZOOMIN;
        }

        private void btZoomOut_Click(object sender, EventArgs e)
        {
            DXF_Edit_Cur_Sts = DXF_EDIT_ZOOMOUT;
        }

        private void AutoScaleBox_MouseDown(object sender, MouseEventArgs e)
        {
            //MessageBox.Show("A" + e.X.ToString() + "," + e.Y.ToString());
        }

        private void RealScaleBox_MouseDown(object sender, MouseEventArgs e)
        {
            //MessageBox.Show("R" + e.X.ToString() + "," + e.Y.ToString());
            //SelectDXFObj(e.X, e.Y, RealScaleBox.Width, RealScaleBox.Height, RealScaleBox.Image.Width, RealScaleBox.Image.Height);
        }

        public void SelectDXFObj(int Sel_X, int Sel_Y, int Obj_W, int Obj_H, int Data_W, int Data_H)
        {
            string GetEntityName;
            DXFLine dxLine;
            DXFCircle dxCircle;
            DXFArc dxArc;
            SFPoint P1, P2;
            float rd1, rd2;
            //float MinX, MinY, MaxX, MaxY;
            float MinSelDisCalc;
            float MinSelDisNum = 999999.0f;
            int MinSelIndex;
            float Real_X, Real_Y;

            Real_X = Sel_X * Data_W / Obj_W;
            Real_Y = Sel_Y * Data_H / Obj_H;

            MinSelIndex = -1;

            if (FCADImage == null)
                return;

            for (int i = 0; i < FCADImage.FEntities.Entities.Count; i++)
            {
                GetEntityName = FCADImage.FEntities.Entities[i].ToString();
                if (GetEntityName == "DXFImportReco.DXFLine")
                {
                    /*
                    dxLine = (DXFLine)FCADImage.FEntities.Entities[i];

                    blackPen.Color = dxLine.FColor;

                    P1 = GetPoint(dxLine.Point1);
                    P2 = GetPoint(dxLine.Point2);

                    graphics.DrawLine(blackPen, P1.X, P1.Y, P2.X, P2.Y);
                    */
                }
                else if (GetEntityName == "DXFImportReco.DXFCircle")
                {
                    dxCircle = (DXFCircle)FCADImage.FEntities.Entities[i];

                    rd1 = dxCircle.radius;
                    P1 = GetPoint(dxCircle.Point1);
                    rd1 = rd1 * FScale;

                    MinSelDisCalc = (float)Math.Sqrt((Real_X - P1.X) * (Real_X - P1.X) + (Real_Y - P1.Y) * (Real_Y - P1.Y)) - rd1;

                    if (MinSelDisCalc < 0)
                    {
                        MinSelDisCalc = 0 - MinSelDisCalc;
                    }

                    if (MinSelDisCalc < MinSelDisNum && MinSelDisCalc < 80)
                    {
                        MinSelDisNum = MinSelDisCalc;
                        MinSelIndex = i;
                    }
                    else
                    {
                        //dxCircle.SelectObjFlag = false;
                    }

                }
                else if (GetEntityName == "DXFImportReco.DXFArc")
                {
                    /*
                    dxArc = (DXFArc)FCADImage.FEntities.Entities[i];

                    blackPen.Color = dxArc.FColor;

                    if (dxArc.pt1.X == 0)
                    {
                        rd1 = dxArc.radius; //Math.Abs(dxArc.radius * dxArc.ratio);
                        rd2 = dxArc.radius;
                    }
                    else
                    {
                        rd1 = dxArc.radius;
                        rd2 = dxArc.radius; //Math.Abs(dxArc.radius * ratio);
                    }

                    rd1 = dxArc.radius;
                    P1 = GetPoint(dxArc.Point1);
                    rd1 = rd1 * FScale;
                    rd2 = rd2 * FScale;
                    P1.X = P1.X - rd1;
                    P1.Y = P1.Y - rd1;
                    float sA = -dxArc.startAngle, eA = -dxArc.endAngle;
                    if (dxArc.endAngle < dxArc.startAngle) sA = Conversion_Angle(sA);
                    eA -= sA;

                    if (eA == 0)
                    {
                        graphics.DrawEllipse(blackPen, P1.X, P1.Y, rd1 * 2, rd2 * 2);
                    }
                    else
                    {
                        graphics.DrawArc(blackPen, P1.X, P1.Y, rd1 * 2, rd2 * 2, 0, 360);//sA, eA);
                    }
                    */
                }
                else
                {
                    //OutTxtStr += "\r\n" + GetEntityName;
                }
            }

            for (int i = 0; i < FCADImage.FEntities.Entities.Count; i++)
            {
                GetEntityName = FCADImage.FEntities.Entities[i].ToString();
                if (GetEntityName == "DXFImportReco.DXFLine")
                {
                    /*
                    dxLine = (DXFLine)FCADImage.FEntities.Entities[i];

                    blackPen.Color = dxLine.FColor;

                    P1 = GetPoint(dxLine.Point1);
                    P2 = GetPoint(dxLine.Point2);

                    graphics.DrawLine(blackPen, P1.X, P1.Y, P2.X, P2.Y);
                    */
                }
                else if (GetEntityName == "DXFImportReco.DXFCircle")
                {
                    dxCircle = (DXFCircle)FCADImage.FEntities.Entities[i];
                    if (MinSelIndex == i)
                    {
                        dxCircle.SelectObjFlag = true;
                    }
                    else
                    {
                        dxCircle.SelectObjFlag = false;
                    }
                }
            }
        }


        public SFPoint GetSelectDXFData()
        {
            string GetEntityName;
            DXFLine dxLine;
            DXFCircle dxCircle;
            DXFArc dxArc;
            //float MinX, MinY, MaxX, MaxY;
            SFPoint ResPointData;

            ResPointData.X = 0;
            ResPointData.Y = 0;
            ResPointData.Z = 0;

            if (FCADImage == null)
                return ResPointData;

            for (int i = 0; i < FCADImage.FEntities.Entities.Count; i++)
            {
                GetEntityName = FCADImage.FEntities.Entities[i].ToString();
                if (GetEntityName == "DXFImportReco.DXFLine")
                {
                    /*
                    dxLine = (DXFLine)FCADImage.FEntities.Entities[i];

                    blackPen.Color = dxLine.FColor;

                    P1 = GetPoint(dxLine.Point1);
                    P2 = GetPoint(dxLine.Point2);

                    graphics.DrawLine(blackPen, P1.X, P1.Y, P2.X, P2.Y);
                    */
                }
                else if (GetEntityName == "DXFImportReco.DXFCircle")
                {
                    dxCircle = (DXFCircle)FCADImage.FEntities.Entities[i];
                    if (dxCircle.FColor == Color.Red)
                    {
                        SFPoint p1= dxCircle.Point1;
                        ResPointData.Y = p1.X - FS_W_Base - FS_W / 2;
                        ResPointData.Z = p1.Y - FS_H_Base - FS_H / 2;
                    }
                }
            }
            return ResPointData;
        }


        private void LoadCfgFiles(string LoadCfgFilePath)
        {
            if (System.IO.File.Exists(LoadCfgFilePath))
            {
                string[] m_WordStr;

                StreamReader sr = new StreamReader(LoadCfgFilePath, Encoding.Default);
                string m_GetConfigDataStr = sr.ReadToEnd();
                sr.Close();

                m_WordStr = m_GetConfigDataStr.Split(SaveDiv);
                if (m_WordStr.Length > 0)
                {
                    RecoSetData.RecoDXFFileName = m_WordStr[0];
                    RecoSetData.RecoCapFileName = m_WordStr[1];
                    RecoSetData.RecoCamIndex = Convert.ToInt32(m_WordStr[2]);
                    RecoSetData.RecoCamID = m_WordStr[3];
                    RecoSetData.ZoomFocus_PortNo = Convert.ToInt32(m_WordStr[4]);
                    RecoSetData.FullShot_ZoomNum = Convert.ToInt32(m_WordStr[5]);
                    RecoSetData.FullShot_FocusNum = Convert.ToInt32(m_WordStr[6]);
                    RecoSetData.DetailShot_ZoomNum = Convert.ToInt32(m_WordStr[7]);
                    RecoSetData.DetailShot_FocusNum = Convert.ToInt32(m_WordStr[8]);
                    RecoSetData.Rotation_Index = Convert.ToInt32(m_WordStr[9]);
                    RecoSetData.Rotation_R = Convert.ToDouble(m_WordStr[10]);
                    RecoSetData.Motion_Index = Convert.ToInt32(m_WordStr[11]);
                    RecoSetData.Motion_Move_X = Convert.ToDouble(m_WordStr[12]);
                    RecoSetData.Motion_Move_Y = Convert.ToDouble(m_WordStr[13]);
                    RecoSetData.Motion_Move_Z = Convert.ToDouble(m_WordStr[14]);
                    RecoSetData.Lamp_Index = Convert.ToInt32(m_WordStr[15]);
                    RecoSetData.Lamp_PortNo = Convert.ToInt32(m_WordStr[16]);
                    RecoSetData.Lamp_SetChannel = Convert.ToInt32(m_WordStr[17]);
                    RecoSetData.Lamp_SetValue = Convert.ToInt32(m_WordStr[18]);
                    
                }
            }
        }

        private void SaveCfgFiles(string SaveCfgFilePath)
        {
            string WritePresetDataStr;

            StreamWriter sw = new StreamWriter(SaveCfgFilePath, false, Encoding.Default);

            WritePresetDataStr = RecoSetData.RecoDXFFileName + "|" + RecoSetData.RecoCapFileName + "|" + cboZoomCamList.SelectedIndex.ToString();
            WritePresetDataStr = WritePresetDataStr + "|" + RecoSetData.RecoCamID + "|" + RecoSetData.ZoomFocus_PortNo.ToString() + "|" + RecoSetData.FullShot_ZoomNum.ToString();
            //WritePresetDataStr = RecoSetData.RecoDXFFileName + "|" + RecoSetData.RecoCapFileName + "|" + RecoSetData.RecoCamIndex.ToString();
            //WritePresetDataStr = WritePresetDataStr + "|" + RecoSetData.RecoCamID + "|" + RecoSetData.ZoomFocus_PortNo.ToString() + "|" + RecoSetData.FullShot_ZoomNum.ToString();
            WritePresetDataStr = WritePresetDataStr + "|" + RecoSetData.FullShot_FocusNum.ToString() + "|" + RecoSetData.DetailShot_ZoomNum.ToString() + "|" + RecoSetData.DetailShot_FocusNum.ToString();
            WritePresetDataStr = WritePresetDataStr + "|" + RecoSetData.Rotation_Index.ToString() + "|" + RecoSetData.Rotation_R.ToString() + "|" + RecoSetData.Motion_Index.ToString() + "|" + RecoSetData.Motion_Move_X.ToString();
            WritePresetDataStr = WritePresetDataStr + "|" + RecoSetData.Motion_Move_Y.ToString() + "|" + RecoSetData.Motion_Move_Z.ToString() + "|" + RecoSetData.Lamp_Index.ToString() + "|" + RecoSetData.Lamp_PortNo.ToString();
            WritePresetDataStr = WritePresetDataStr + "|" + RecoSetData.Lamp_SetChannel.ToString() + "|" + RecoSetData.Lamp_SetValue.ToString();
            sw.Write(WritePresetDataStr);

            sw.Close();

        }

        private void LoadCalibSetFiles(string SaveCalibFilePath)
        {
            if (System.IO.File.Exists(SaveCalibFilePath))
            {
                string[] m_WordStr;

                StreamReader sr = new StreamReader(SaveCalibFilePath, Encoding.Default);
                string m_GetConfigDataStr = sr.ReadToEnd();
                sr.Close();

                m_WordStr = m_GetConfigDataStr.Split(SaveDiv);
                if (m_WordStr.Length > 0)
                {
                    RealCalibrationData_A.ddp_X = Convert.ToDouble(m_WordStr[0]);
                    RealCalibrationData_A.ddp_Y = Convert.ToDouble(m_WordStr[1]);
                    RealCalibrationData_B.ddp_X = Convert.ToDouble(m_WordStr[2]);
                    RealCalibrationData_B.ddp_Y = Convert.ToDouble(m_WordStr[3]);
                }
            }
        }

        private void SaveCalibSetFiles(string SaveCalibFilePath)
        {
            string WritePresetDataStr;

            StreamWriter sw = new StreamWriter(SaveCalibFilePath, false, Encoding.Default);

            WritePresetDataStr = RealCalibrationData_A.ddp_X.ToString() + "|" + RealCalibrationData_A.ddp_Y.ToString() + "|" + RealCalibrationData_B.ddp_X.ToString() + "|" + RealCalibrationData_B.ddp_Y.ToString();
            sw.Write(WritePresetDataStr);

            sw.Close();

        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            
            //if (CfgSaveFolderPath_FileName.Length == 0)
            if (CfgSaveHeadFileName == null)
            {
                CfgSaveHeadFileName = GetNewRecoDataFileName(CfgSaveFolderPath);
                CfgSaveFolderPath_FileName = CfgSaveFolderPath + @"\" + CfgSaveHeadFileName + @".dat";
            }
            else if (CfgSaveHeadFileName.Length == 0)
            {
                CfgSaveHeadFileName = GetNewRecoDataFileName(CfgSaveFolderPath);
                CfgSaveFolderPath_FileName = CfgSaveFolderPath + @"\" + CfgSaveHeadFileName + @".dat";
            }
            if (File.Exists(CurDXFFileNameStr) == true)
            {
                string OriSrcPath, OriTarPath, NewDXFFileNameStr;

                if (FCADImage != null)
                {
                    FCADImage = null;
                }

                CreateMakeFolderFunc(CfgSaveFolderPath);
                OriSrcPath = CurDXFFileNameStr;
                //NewDXFFileNameStr = GetNewDXFFileName(CfgSaveFolderPath);
                NewDXFFileNameStr = CfgSaveHeadFileName + @".dxf";
                OriTarPath = CfgSaveFolderPath + @"\" + NewDXFFileNameStr;
                if (File.Exists(OriTarPath) == true)
                {
                    File.Delete(OriTarPath);
                }
                File.Copy(OriSrcPath, OriTarPath);
                RecoSetData.RecoDXFFileName = NewDXFFileNameStr;
                CurDXFFileNameStr = "";

                /*
                string OriSrcPath, OriTarPath, NewDXFFileNameStr;
                CreateMakeFolderFunc(CfgSaveFolderPath);
                OriSrcPath = CurDXFFileNameStr;
                NewDXFFileNameStr = GetNewDXFFileName(CfgSaveFolderPath);
                OriTarPath = CfgSaveFolderPath + NewDXFFileNameStr;
                File.Copy(OriSrcPath, OriTarPath);
                RecoSetData.RecoDXFFileName = NewDXFFileNameStr;
                */
            }
            SaveCfgFiles(CfgSaveFolderPath_FileName);


            // 홍동성 ...
            // ----------
            SaveWorkFuncInfo();
            // ----------



            this.DialogResult = DialogResult.OK;
            this.Dispose();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Dispose();
        }



        private void btnJogLampPlus_Click(object sender, EventArgs e)
        {
            int SetValueNum = int.Parse(txtLampValue.Text);
            SetValueNum = SetValueNum + LAMP_INC_GAP;
            if (SetValueNum > 1024)
            {
                SetValueNum = 1024;
            }
            txtLampValue.Text = SetValueNum.ToString();
            SetValueLamp();

        }

        private void btnJogLampHome_Click(object sender, EventArgs e)
        {
            txtLampValue.Text = "0.0";
        }

        private void btShotSetSave_Click(object sender, EventArgs e)
        {
            if (FullDetailShotFlag == FULLSHOTTYPE)
            {
                ShotData[FULLSHOTTYPE].ZoomNum = Int32.Parse(textBox1.Text);
                ShotData[FULLSHOTTYPE].FocusNum = Int32.Parse(textBox2.Text);

                RecoSetData.FullShot_ZoomNum = ShotData[FULLSHOTTYPE].ZoomNum;
                RecoSetData.FullShot_FocusNum = ShotData[FULLSHOTTYPE].FocusNum;
                RecoSetData.DetailShot_ZoomNum = ShotData[DETAILSHOTTYPE].ZoomNum;
                RecoSetData.DetailShot_FocusNum = ShotData[DETAILSHOTTYPE].FocusNum;



                RecoSetData.Motion_Move_X = double.Parse(txtXAxisValue.Text);
                RecoSetData.Motion_Move_Y = double.Parse(txtYAxisValue.Text);
                RecoSetData.Motion_Move_Z = double.Parse(txtZAxisValue.Text);



                MessageBox.Show("FullShot 저장완료!");
                //MessageBox.Show(DataManager.GetModelSelectFileNameFolder());
            }
            else
            {
                ShotData[DETAILSHOTTYPE].ZoomNum = Int32.Parse(textBox1.Text);
                ShotData[DETAILSHOTTYPE].FocusNum = Int32.Parse(textBox2.Text);

                RecoSetData.FullShot_ZoomNum = ShotData[FULLSHOTTYPE].ZoomNum;
                RecoSetData.FullShot_FocusNum = ShotData[FULLSHOTTYPE].FocusNum;
                RecoSetData.DetailShot_ZoomNum = ShotData[DETAILSHOTTYPE].ZoomNum;
                RecoSetData.DetailShot_FocusNum = ShotData[DETAILSHOTTYPE].FocusNum;

                MessageBox.Show("DetailShot 저장완료!");
            }
        }

        private void cboZoomCamList_SelectedIndexChanged(object sender, EventArgs e)
        {
            string ComPortStr;

            if(cboZoomCamList.SelectedIndex>-1)
            {
                timer3.Enabled = false;
                timer2.Enabled = false;

                ScreenFlipFlag = GetScreenFlip(cboZoomCamList.SelectedIndex);
                if (RunCamPlayFlag == true)
                {
                    if (!XCCam.ImageStop())
                        MessageBox.Show("Image Stop Error");

                    XCCam.SetImageCallBack();

                    if (!XCCam.ResourceRelease())
                        MessageBox.Show("Resource Release Error");

                    DispExec.Stop();
                    Frame.Stop();
                    CameraOneClose();

                    RunCamPlayFlag = false;
                }

                ComPortStr = "COM" + DataManager.CameraSettingInfoList[cboZoomCamList.SelectedIndex].ZoomFocusPort.ToString();

                Application.DoEvents();
                Application.DoEvents();

                if(true)
                {
                    if (NavitarCtrl != null)
                    {

                        Application.DoEvents();
                        NavitarCtrl.Stop();
                        Application.DoEvents();

                        NavitarCtrl.Disconnect();
                        Application.DoEvents();

                        NavitarCtrl.Dispose();
                        Application.DoEvents();

                        NavitarCtrl = null;
                        Application.DoEvents();
                    }

                    NavitarCtrl = new ControllerLegacy(ComPortStr);
                    NavitarCtrl.Connect();

                    timer2.Enabled = true;

                }

                int FindCamIndex = FindSelectUIDList(DataManager.CameraSettingInfoList[cboZoomCamList.SelectedIndex].IP);
                if(FindCamIndex>-1)
                {
                    /*
                    if (RunCamPlayFlag == true)
                    {
                        if (XCCam.ImageStart())
                        {
                            if (!XCCam.ImageStop())
                                MessageBox.Show("Image Stop Error");
                            XCCam.SetImageCallBack();
                            if (!XCCam.ResourceRelease())
                                MessageBox.Show("Resource Release Error");
                            DispExec.Stop();
                            Frame.Stop();
                        }

                        CameraOneClose();

                        RunCamPlayFlag = false;
                    }
                    */

                    if (UIDList.SelectedIndex != FindCamIndex)
                    {
                        UIDList.SelectedIndex = FindCamIndex;
                    }
                    else
                    {
                        //UIDList.SelectedIndex = FindCamIndex;
                        UIDList_SelectedIndexChanged(UIDList, new EventArgs());
                    }
                }
            }
        }

        private int FindSelectUIDList(string UIDFindStr)
        {
            int FindSelIndex;
            FindSelIndex = -1;
            for (int i = 0; i < UIDList.Items.Count; i++)
            {
                if (UIDFindStr == UIDList.Items[i].ToString())
                {
                    FindSelIndex = i;
                    break;
                }
            }
            return FindSelIndex;
        }

        private void timLoadStart_Tick(object sender, EventArgs e)
        {
            string LoadDXFLocalFileStr;
            timLoadStart.Enabled = false;

            if (CfgSaveFolderPath_FileName.Length>0)
            {
                StartCalcFlag = true;
                FCADImage = new CADImage();
                Base.X = 1;
                Base.Y = 1;
                FScale = 1.0f;
                //timer1.Interval = 600;
                //timer1.Enabled = true;
                LoadDXFLocalFileStr = CfgSaveFolderPath + @"\" + RecoSetData.RecoDXFFileName;
                if (File.Exists(LoadDXFLocalFileStr)==true)
                {
                    FCADImage.LoadFromFile(LoadDXFLocalFileStr);
                }
            }

        }

        private void btnJogLampMinus_Click(object sender, EventArgs e)
        {

        }

        private void SetValueLamp()
        {
            if(cboLampList.SelectedIndex>-1)
            {
                int SetChnlNum = DataManager.LightingSettingInfoList[cboLampList.SelectedIndex].Channel-1;
                int SetValueNum = int.Parse(txtLampValue.Text);
                LampComm.SetLamp(SetChnlNum, SetValueNum);
            }
        }

        private bool GetScreenFlip(int SelectCamIndex)
        {
            bool ResScreenFlipFlag;
            ResScreenFlipFlag = false;
            if (SelectCamIndex > -1)
            {
                if (SelectCamIndex == 0 || SelectCamIndex == 1 || SelectCamIndex == 2)
                {
                    ResScreenFlipFlag = true;
                }
            }
            return ResScreenFlipFlag;
        }


#region 홍동성 ...

        public WorkFuncInfo _WorkFuncInfo;  // 홍동성 => 시나리오 정보 저장

        string[] MoveNameList = { @"카메라 유닛 이동(X,Y,Z) 기능", 
                                  @"후방 카메라 이동(Z) 기능" };

        /*
        private int AxisXIndex = -1;
        private int AxisX2Index = -1;
        private int AxisYIndex = -1;
        private int AxisZIndex = -1;
        private int AxisRIndex = -1;
        */

        // 조명 ...
        // ----------
        private LightingSettingInfo CurLampSettingInfo;


        private void Initialize()
        {
            // 인덱스 회전
            // ----------
            for (int i = 0; i < 2; i++)
            {
                cboIndexList.Items.Add(DataManager.WorkNameList[i]);
            }

            // 좌표 이동
            // ----------
            for (int i = 0; i < MoveNameList.Length; i++)
            {
                cboMoveList.Items.Add(MoveNameList[i]);
            }

            // 조명
            // ----------
            for (int i = 0; i < 7; i++)
            {
                cboLampList.Items.Add(DataManager.LightingSettingInfoList[i].Name);
            }



            // 동작 기능 선택
            // ----------
            cboIndexList.SelectedIndex  = _WorkFuncInfo.Rotation_Index;
            cboMoveList.SelectedIndex   = _WorkFuncInfo.Motion_Index;
            cboLampList.SelectedIndex   = _WorkFuncInfo.Lamp_Index;
            //UIDList.SelectedIndex     = _WorkFuncInfo.RecoCamIndex;


            // 축 정보 불러오기
            // ----------
            /*
            AxisXIndex = _WorkFuncInfo.AxisXIndex;
            AxisYIndex = _WorkFuncInfo.AxisYIndex;
            AxisZIndex = _WorkFuncInfo.AxisZIndex;
            AxisRIndex = _WorkFuncInfo.AxisRIndex;
            */

            // 상태값 정보 불러오기
            // ----------
            txtRAxisValue.Text = _WorkFuncInfo.WFRotationAngle.ToString();
            txtXAxisValue.Text = _WorkFuncInfo.WFMoveX.ToString();
            txtYAxisValue.Text = _WorkFuncInfo.WFMoveY.ToString();
            txtZAxisValue.Text = _WorkFuncInfo.WFMoveZ.ToString();
            txtLampValue.Text = _WorkFuncInfo.WFLampValue.ToString();


            // 축 현재 위치값 갱신용 타이머 ...
            // ----------
            timerAxis.Enabled = true;
            // ----------
        }

        public void SaveWorkFuncInfo()
        {
            _WorkFuncInfo.Rotation_Index    = cboIndexList.SelectedIndex;
            _WorkFuncInfo.Motion_Index      = cboMoveList.SelectedIndex;
            _WorkFuncInfo.Lamp_Index        = cboLampList.SelectedIndex;
            //_WorkFuncInfo.RecoCamIndex      = UIDList.SelectedIndex;


            // 축 정보 불러오기
            // ----------
            /*
            _WorkFuncInfo.AxisXIndex        = AxisXIndex;
            _WorkFuncInfo.AxisYIndex        = AxisYIndex;
            _WorkFuncInfo.AxisZIndex        = AxisZIndex;
            _WorkFuncInfo.AxisRIndex        = AxisRIndex;
            */

            // 상태값 정보 불러오기
            // ----------
            _WorkFuncInfo.WFRotationAngle   = Convert.ToDouble(txtRAxisValue.Text);
            _WorkFuncInfo.WFMoveX           = Convert.ToDouble(txtXAxisValue.Text);
            _WorkFuncInfo.WFMoveY           = Convert.ToDouble(txtYAxisValue.Text);
            _WorkFuncInfo.WFMoveZ           = Convert.ToDouble(txtZAxisValue.Text);
            _WorkFuncInfo.WFLampValue       = Convert.ToInt32(txtLampValue.Text);
        }

        private void cboIndexList_SelectedIndexChanged(object sender, EventArgs e)
        {
            _WorkFuncInfo.Rotation_Index = cboIndexList.SelectedIndex;
        }

        private void cboMoveList_SelectedIndexChanged(object sender, EventArgs e)
        {            
            _WorkFuncInfo.Motion_Index = cboMoveList.SelectedIndex;
        }

        private void cboLampList_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurLampSettingInfo = DataManager.LightingSettingInfoList[cboLampList.SelectedIndex];
        }


#endregion 홍동성 ...


#region JOG ...

        private void timerAxis_Tick(object sender, EventArgs e)
        {
            UpdatePos();

            if (MultiMotion.CheckDefense() != MultiMotion.KSM_OK)
            {
                // ...
            }            
        }

        private void UpdatePos()
        {
            MultiMotion.GetCurrentPos();

            switch (_WorkFuncInfo.Motion_Index)
            {
                case 0: // 카메라 유닛 이동(X/Y/Z)  기능
                    {
                        txtXAxisValue.Text = MultiMotion.AxisValue[MultiMotion.CAM_UNIT_X].ToString("##.00");
                        txtYAxisValue.Text = MultiMotion.AxisValue[MultiMotion.CAM_UNIT_Y].ToString("##.00");
                        txtZAxisValue.Text = MultiMotion.AxisValue[MultiMotion.CAM_UNIT_Z].ToString("##.00");
                    }
                    break;
                case 1: // 후방 카메라 이동(Z축) 기능
                    {
                        txtXAxisValue.Text = "0.0";
                        txtYAxisValue.Text = "0.0";
                        txtZAxisValue.Text = MultiMotion.AxisValue[MultiMotion.BACK_CAM_Z].ToString("##.00");
                    }
                    break;
            }

            switch (_WorkFuncInfo.Rotation_Index)
            {
                case 0: // 고정축 INDEX 회전(R)
                    {
                        txtRAxisValue.Text = MultiMotion.AxisValue[MultiMotion.INDEX_FIX_R].ToString("##.00");
                    }
                    break;
                case 1: // 이동축 INDEX 회전(R)
                    {
                        txtRAxisValue.Text = MultiMotion.AxisValue[MultiMotion.INDEX_MOVE_R].ToString("##.00");
                    }                    
                    break;
            }

        }

        private void btEmergency_Click(object sender, EventArgs e)
        {
            MultiMotion.StopAll();
        }

        private void btnJogRHome_Click(object sender, EventArgs e)
        {
            timerAxis.Enabled = true;

            if (MultiMotion.GantryAxisEnable(0, false) == MultiMotion.KSM_OK) // INDEX(R) 갠트리 비활성화
            {
                switch (_WorkFuncInfo.Rotation_Index)
                {
                    case 0: // 고정축 INDEX 회전(R)                        
                        MultiMotion.HomeMove(MultiMotion.INDEX_FIX_R, false);
                        break;
                    case 1: // 이동축 INDEX 회전(R)
                        MultiMotion.HomeMove(MultiMotion.INDEX_MOVE_R, false);
                        break;
                }
            }
        }

        private void btnJogXHome_Click(object sender, EventArgs e)
        {
            timerAxis.Enabled = true;

            switch (_WorkFuncInfo.Motion_Index)
            {
                case 0: // 카메라 유닛 이동(X/Y/Z)  기능
                    //MultiMotion.HomeMove(MultiMotion.CAM_UNIT_X, false);
                    MultiMotion.HomeMove(MultiMotion.CAM_UNIT_X, true);
                    break;
            }
            MessageBox.Show("Home완료!");
        }

        private void btnJogYHome_Click(object sender, EventArgs e)
        {
            timerAxis.Enabled = true;

            switch (_WorkFuncInfo.Motion_Index)
            {
                case 0: // 카메라 유닛 이동(X/Y/Z)  기능
                    {
                        //MultiMotion.HomeMove(MultiMotion.CAM_UNIT_Y, false);
                        MultiMotion.HomeMove(MultiMotion.CAM_UNIT_Y, true);
                    }
                    break;
                case 1: // 후방 카메라 이동(Z축) 기능
                    break;
            }
            MessageBox.Show("Home완료!");
        }

        private void btnJogZHome_Click(object sender, EventArgs e)
        {
            timerAxis.Enabled = true;

            switch (_WorkFuncInfo.Motion_Index)
            {
                case 0: // 카메라 유닛 이동(X/Y/Z)  기능
                    MultiMotion.HomeMove(MultiMotion.CAM_UNIT_Z, true);
                    break;
                case 1: // 후방 카메라 이동(Z축) 기능
                    MultiMotion.HomeMove(MultiMotion.BACK_CAM_Z, true);
                    break;
            }
            MessageBox.Show("Home완료!");
        }

        // X ...
        // --------------------------------------------------

        private void btnJogXMinus_MouseDown(object sender, MouseEventArgs e)
        {
            timerAxis.Enabled = true;

            switch (_WorkFuncInfo.Motion_Index)
            {
                case 0: // 카메라 유닛 이동(X/Y/Z)  기능
                    MultiMotion.StepMove(MultiMotion.CAM_UNIT_X, 1, false);
                    break;
                case 1: // 후방 카메라 이동(Z축) 기능
                    break;
            }
        }

        private void btnJogXMinus_MouseUp(object sender, MouseEventArgs e)
        {
            switch (_WorkFuncInfo.Motion_Index)
            {
                case 0: // 카메라 유닛 이동(X/Y/Z)  기능
                    //MultiMotion.JogStop(MultiMotion.CAM_UNIT_X);
                    break;
                case 1: // 후방 카메라 이동(Z축) 기능
                    break;
            }
        }

        private void btnJogXPlus_MouseDown(object sender, MouseEventArgs e)
        {
            timerAxis.Enabled = true;

            switch (_WorkFuncInfo.Motion_Index)
            {
                case 0: // 카메라 유닛 이동(X/Y/Z)  기능
                    MultiMotion.StepMove(MultiMotion.CAM_UNIT_X, 0, false);
                    break;
                case 1: // 후방 카메라 이동(Z축) 기능
                    break;
            }
        }

        private void btnJogXPlus_MouseUp(object sender, MouseEventArgs e)
        {
            switch (_WorkFuncInfo.Motion_Index)
            {
                case 0: // 카메라 유닛 이동(X/Y/Z)  기능
                    //MultiMotion.JogStop(MultiMotion.CAM_UNIT_X);
                    break;
                case 1: // 후방 카메라 이동(Z축) 기능
                    break;
            }
        }

        private void txtXAxisValue_KeyDown(object sender, KeyEventArgs e)
        {
            double dTempValue = 0.0;

            switch (e.KeyCode)
            {
                case Keys.Enter:
                    {
                        if (double.TryParse(this.txtXAxisValue.Text, out dTempValue))
                        {
                            MultiMotion.MoveAxis(MultiMotion.CAM_UNIT_X, dTempValue, false);
                        }

                        timerAxis.Enabled = true;
                    }
                    break;
                default:
                    timerAxis.Enabled = false;
                    break;
            }
        }


        // Y ...
        // --------------------------------------------------

        private void btnJogYMinus_MouseDown(object sender, MouseEventArgs e)
        {
            timerAxis.Enabled = true;

            switch (_WorkFuncInfo.Motion_Index)
            {
                case 0: // 카메라 유닛 이동(X/Y/Z)  기능
                    MultiMotion.StepMove(MultiMotion.CAM_UNIT_Y, 1, false);
                    break;
                case 1: // 후방 카메라 이동(Z축) 기능
                    break;
            }
        }

        private void btnJogYMinus_MouseUp(object sender, MouseEventArgs e)
        {
            switch (_WorkFuncInfo.Motion_Index)
            {
                case 0: // 카메라 유닛 이동(X/Y/Z)  기능
                    {
                        //MultiMotion.JogStop(MultiMotion.CAM_UNIT_Y);
                    }
                    break;
                case 1: // 후방 카메라 이동(Z축) 기능
                    break;
            }
        }

        private void btnJogYPlus_MouseDown(object sender, MouseEventArgs e)
        {
            timerAxis.Enabled = true;

            switch (_WorkFuncInfo.Motion_Index)
            {
                case 0: // 카메라 유닛 이동(X/Y/Z)  기능
                    MultiMotion.StepMove(MultiMotion.CAM_UNIT_Y, 0, false);
                    break;
                case 1: // 후방 카메라 이동(Z축) 기능
                    break;
            }
        }

        private void btnJogYPlus_MouseUp(object sender, MouseEventArgs e)
        {
            switch (_WorkFuncInfo.Motion_Index)
            {
                case 0: // 카메라 유닛 이동(X/Y/Z)  기능
                    {
                        //MultiMotion.JogStop(MultiMotion.CAM_UNIT_Y);
                    }
                    break;
                case 1: // 후방 카메라 이동(Z축) 기능
                    break;
            }
        }

        private void txtYAxisValue_KeyDown(object sender, KeyEventArgs e)
        {
            double dTempValue = 0.0;

            switch (e.KeyCode)
            {
                case Keys.Enter:
                    {
                        if (double.TryParse(this.txtYAxisValue.Text, out dTempValue))
                        {
                            MultiMotion.MoveAxis(MultiMotion.CAM_UNIT_Y, dTempValue, false);
                        }

                        timerAxis.Enabled = true;
                    }
                    break;
                default:
                    timerAxis.Enabled = false;
                    break;
            }
        }


        // Z ...
        // --------------------------------------------------

        private void btnJogZMinus_MouseDown(object sender, MouseEventArgs e)
        {
            timerAxis.Enabled = true;

            switch (_WorkFuncInfo.Motion_Index)
            {
                case 0: // 카메라 유닛 이동(X/Y/Z)  기능
                    MultiMotion.StepMove(MultiMotion.CAM_UNIT_Z, 1, false);
                    break;
                case 1: // 후방 카메라 이동(Z축) 기능
                    MultiMotion.StepMove(MultiMotion.BACK_CAM_Z, 1, false);
                    break;
            }
        }

        private void btnJogZMinus_MouseUp(object sender, MouseEventArgs e)
        {
            switch (_WorkFuncInfo.Motion_Index)
            {
                case 0: // 카메라 유닛 이동(X/Y/Z)  기능
                    //MultiMotion.JogStop(MultiMotion.CAM_UNIT_Z);
                    break;
                case 1: // 후방 카메라 이동(Z축) 기능
                    //MultiMotion.JogStop(MultiMotion.BACK_CAM_Z);
                    break;
            }
        }

        private void btnJogZPlus_MouseDown(object sender, MouseEventArgs e)
        {
            timerAxis.Enabled = true;

            switch (_WorkFuncInfo.Motion_Index)
            {
                case 0: // 카메라 유닛 이동(X/Y/Z)  기능
                    MultiMotion.StepMove(MultiMotion.CAM_UNIT_Z, 0, false);
                    break;
                case 1: // 후방 카메라 이동(Z축) 기능
                    MultiMotion.StepMove(MultiMotion.BACK_CAM_Z, 0, false);
                    break;
            }
        }

        private void btnJogZPlus_MouseUp(object sender, MouseEventArgs e)
        {
            switch (_WorkFuncInfo.Motion_Index)
            {
                case 0: // 카메라 유닛 이동(X/Y/Z)  기능
                    //MultiMotion.JogStop(MultiMotion.CAM_UNIT_Z);
                    break;
                case 1: // 후방 카메라 이동(Z축) 기능
                    //MultiMotion.JogStop(MultiMotion.BACK_CAM_Z);
                    break;
            }
        }

        private void txtZAxisValue_KeyDown(object sender, KeyEventArgs e)
        {
            double dTempValue = 0.0;

            switch (e.KeyCode)
            {
                case Keys.Enter:
                    {
                        if (double.TryParse(this.txtZAxisValue.Text, out dTempValue))
                        {
                            MultiMotion.MoveAxis(MultiMotion.CAM_UNIT_Z, dTempValue, false);
                        }

                        timerAxis.Enabled = true;
                    }
                    break;
                default:
                    timerAxis.Enabled = false;
                    break;
            }
        }


        // R ...
        // --------------------------------------------------

        private void btnJogRMinus_MouseDown(object sender, MouseEventArgs e)
        {
            timerAxis.Enabled = true;

            if (MultiMotion.GantryAxisEnable(0, false) == MultiMotion.KSM_OK) // INDEX(X) 갠트리 비활성화
            {
                switch (_WorkFuncInfo.Rotation_Index)
                {
                    case 0: // 고정축 INDEX 회전(R)
                        MultiMotion.StepMove(MultiMotion.INDEX_FIX_R, 1, false);
                        break;
                    case 1: // 이동축 INDEX 회전(R)
                        MultiMotion.StepMove(MultiMotion.INDEX_MOVE_R, 1, false);
                        break;
                }
            }
        }

        private void btnJogRMinus_MouseUp(object sender, MouseEventArgs e)
        {
            if (MultiMotion.GantryAxisEnable(0, false) == MultiMotion.KSM_OK) // INDEX(X) 갠트리 비활성화
            {
                switch (_WorkFuncInfo.Rotation_Index)
                {
                    case 0: // 고정축 INDEX 회전(R)
                        //MultiMotion.JogStop(MultiMotion.INDEX_FIX_R);
                        break;
                    case 1: // 이동축 INDEX 회전(R)
                        //MultiMotion.JogStop(MultiMotion.INDEX_MOVE_R);
                        break;
                }
            }
        }

        private void btnJogRPlus_MouseDown(object sender, MouseEventArgs e)
        {
            timerAxis.Enabled = true;

            if (MultiMotion.GantryAxisEnable(0, false) == MultiMotion.KSM_OK) // INDEX(X) 갠트리 비활성화
            {
                switch (_WorkFuncInfo.Rotation_Index)
                {
                    case 0: // 고정축 INDEX 회전(R)
                        MultiMotion.StepMove(MultiMotion.INDEX_FIX_R, 0, false);
                        break;
                    case 1: // 이동축 INDEX 회전(R)
                        MultiMotion.StepMove(MultiMotion.INDEX_MOVE_R, 0, false);
                        break;
                }
            }
        }

        private void btnJogRPlus_MouseUp(object sender, MouseEventArgs e)
        {
            if (MultiMotion.GantryAxisEnable(0, false) == MultiMotion.KSM_OK) // 갠트리 비활성화
            {
                switch (_WorkFuncInfo.Rotation_Index)
                {
                    case 0: // 고정축 INDEX 회전(R)
                        //MultiMotion.JogStop(MultiMotion.INDEX_FIX_R);
                        break;
                    case 1: // 이동축 INDEX 회전(R)
                        //MultiMotion.JogStop(MultiMotion.INDEX_MOVE_R);
                        break;
                }
            }            
        }


        private void txtRAxisValue_KeyDown(object sender, KeyEventArgs e)
        {
            double dTempValue = 0.0;


            if (MultiMotion.GantryAxisEnable(0, false) == MultiMotion.KSM_OK) // 갠트리 비활성화
            {
                switch (_WorkFuncInfo.Rotation_Index)
                {
                    case 0: // 고정축 INDEX 회전(R)
                        {
                            switch (e.KeyCode)
                            {
                                case Keys.Enter:
                                    {
                                        if (double.TryParse(this.txtRAxisValue.Text, out dTempValue))
                                        {
                                            MultiMotion.MoveAxis(MultiMotion.INDEX_FIX_R, dTempValue, false);
                                        }

                                        timerAxis.Enabled = true;
                                    }
                                    break;
                                default:
                                    timerAxis.Enabled = false;
                                    break;
                            }

                        }
                        
                        break;
                    case 1: // 이동축 INDEX 회전(R)
                        {
                            switch (e.KeyCode)
                            {
                                case Keys.Enter:
                                    {
                                        if (double.TryParse(this.txtRAxisValue.Text, out dTempValue))
                                        {
                                            MultiMotion.MoveAxis(MultiMotion.INDEX_MOVE_R, dTempValue, false);
                                        }

                                        timerAxis.Enabled = true;
                                    }
                                    break;
                                default:
                                    timerAxis.Enabled = false;
                                    break;
                            }

                        }
                        
                        break;
                }
            }            

        }



#endregion JOG ...


        private void Test()
        {
            /*MultiMotion.MoveAxis(0, 30.0);  // 완료 후 리턴

            MultiMotion.MoveAxis(0, -30.0); // 완료 후 리턴

            //MultiMotion.RotateAxis(0, 10.0);*/
        }

        public void GetDXFObjList()
        {
            string GetEntityName;
            DXFLine dxLine;
            DXFCircle dxCircle;
            DXFArc dxArc;
            SFPoint P1, P2;
            double LineLenNum;
            float rd1, rd2;
            float MinX, MinY, MaxX, MaxY;
            int LoopDXFCount;
            int Calc_Gap_X, Calc_Gap_Y;

            if (FCADImage == null)
                return;

            LoopDXFCount = 0;
            //Bitmap tmp = new Bitmap(bmp, bmp.Width, bmp.Height);

            for (int i = 0; i < 1; i++)
            {
                DXFObjList[i].ExistLyFlag = false;
                DXFObjList[i].LyType = SHAPE_NONE;
            }

            for (int i = 0; i < FCADImage.FEntities.Entities.Count; i++)
            {
                GetEntityName = FCADImage.FEntities.Entities[i].ToString();
                if (GetEntityName == "DXFImportReco.DXFLine")
                {

                    dxLine = (DXFLine)FCADImage.FEntities.Entities[i];

                    P1 = GetPoint(dxLine.Point1);
                    P2 = GetPoint(dxLine.Point2);

                    DXFObjList[LoopDXFCount].Obj_X1 = (int)P1.X;
                    DXFObjList[LoopDXFCount].Obj_Y1 = (int)P1.Y;
                    DXFObjList[LoopDXFCount].Obj_X2 = (int)P2.X;
                    DXFObjList[LoopDXFCount].Obj_Y2 = (int)P2.Y;
                    Calc_Gap_X = (int)(DXFObjList[LoopDXFCount].Obj_X1 - DXFObjList[LoopDXFCount].Obj_X2);
                    Calc_Gap_Y = (int)(DXFObjList[LoopDXFCount].Obj_Y1 - DXFObjList[LoopDXFCount].Obj_Y2);

                    if (Math.Sqrt(Calc_Gap_X * Calc_Gap_X + Calc_Gap_Y * Calc_Gap_Y) > 50)
                    {

                        DXFObjList[LoopDXFCount].ExistLyFlag = true;
                        DXFObjList[LoopDXFCount].LyType = SHAPE_LINE;

                        Calc_Gap_X = (int)(DXFObjList[LoopDXFCount].Obj_X1 + DXFObjList[LoopDXFCount].Obj_X2) / 2 - Base.X;
                        Calc_Gap_Y = Base.Y - (int)(DXFObjList[LoopDXFCount].Obj_Y1 + DXFObjList[LoopDXFCount].Obj_Y2) / 2;
                        DXFObjList[LoopDXFCount].Obj_CenterDistance = Math.Sqrt(Calc_Gap_X * Calc_Gap_X + Calc_Gap_Y * Calc_Gap_Y);
                        DXFObjList[LoopDXFCount].Obj_CenterAngle = Math.Atan2(Calc_Gap_Y, Calc_Gap_X) * 180.0 / 3.1415926535;
                        DXFObjList[LoopDXFCount].Obj_Dia = -99;
                        DXFObjList[LoopDXFCount].Obj_CenterDistance = DXFObjList[LoopDXFCount].Obj_CenterDistance * 110 / 100;

                        LoopDXFCount++;
                    }

                    /*
                    LineLenNum = GetLineDistance(P1, P2);
                    if (LineLenNum > 1.1)
                    {
                        if (MinX > P1.X)
                        {
                            MinX = P1.X;
                        }
                        if (MaxX < P1.X)
                        {
                            MaxX = P1.X;
                        }
                        if (MinY > P1.Y)
                        {
                            MinY = P1.Y;
                        }
                        if (MaxY < P1.Y)
                        {
                            MaxY = P1.Y;
                        }

                        if (MinX > P2.X)
                        {
                            MinX = P2.X;
                        }
                        if (MaxX < P2.X)
                        {
                            MaxX = P2.X;
                        }
                        if (MinY > P2.Y)
                        {
                            MinY = P2.Y;
                        }
                        if (MaxY < P2.Y)
                        {
                            MaxY = P2.Y;
                        }
                    }
                    */
                }
                else if (GetEntityName == "DXFImportReco.DXFCircle")
                {
                    dxCircle = (DXFCircle)FCADImage.FEntities.Entities[i];

                    rd1 = dxCircle.radius * FScale;
                    P1 = GetPoint(dxCircle.Point1);

                    DXFObjList[LoopDXFCount].ExistLyFlag = true;
                    DXFObjList[LoopDXFCount].LyType = SHAPE_CIRCLE;
                    DXFObjList[LoopDXFCount].Obj_Center.Obj_X = (int)P1.X;
                    DXFObjList[LoopDXFCount].Obj_Center.Obj_Y = (int)P1.Y;
                    DXFObjList[LoopDXFCount].Obj_Dia = (int)(rd1 * 2);
                    Calc_Gap_X = DXFObjList[LoopDXFCount].Obj_Center.Obj_X - Base.X;
                    Calc_Gap_Y = Base.Y - DXFObjList[LoopDXFCount].Obj_Center.Obj_Y;
                    DXFObjList[LoopDXFCount].Obj_CenterDistance = Math.Sqrt(Calc_Gap_X * Calc_Gap_X + Calc_Gap_Y * Calc_Gap_Y);
                    DXFObjList[LoopDXFCount].Obj_CenterAngle = Math.Atan2(Calc_Gap_Y, Calc_Gap_X) * 180.0 / 3.1415926535;
                    DXFObjList[LoopDXFCount].Obj_Dia = DXFObjList[LoopDXFCount].Obj_Dia * 110 / 100;
                    DXFObjList[LoopDXFCount].Obj_CenterDistance = DXFObjList[LoopDXFCount].Obj_CenterDistance * 110 / 100;

                    LoopDXFCount++;
                }
                else if (GetEntityName == "DXFImportReco.DXFArc")
                {
                    /*
                    dxArc = (DXFArc)FCADImage.FEntities.Entities[i];

                    if (dxArc.pt1.X == 0)
                    {
                        rd1 = dxArc.radius; //Math.Abs(dxArc.radius * dxArc.ratio);
                        rd2 = dxArc.radius;
                    }
                    else
                    {
                        rd1 = dxArc.radius;
                        rd2 = dxArc.radius; //Math.Abs(dxArc.radius * ratio);
                    }

                    rd1 = dxArc.radius;
                    P1 = GetPoint(dxArc.Point1);
                    rd1 = rd1 * FScale;
                    rd2 = rd2 * FScale;
                    P1.X = P1.X - rd1;
                    P1.Y = P1.Y - rd1;
                    float sA = -dxArc.startAngle, eA = -dxArc.endAngle;
                    if (dxArc.endAngle < dxArc.startAngle) sA = Conversion_Angle(sA);
                    eA -= sA;

                    if (MinX > P1.X - rd1)
                    {
                        MinX = P1.X - rd1;
                    }
                    if (MaxX < P1.X + rd1)
                    {
                        MaxX = P1.X + rd1;
                    }
                    if (MinY > P1.Y - rd1)
                    {
                        MinY = P1.Y - rd1;
                    }
                    if (MaxY < P1.Y + rd1)
                    {
                        MaxY = P1.Y + rd1;
                    }
                    */
                }
                else
                {
                    //OutTxtStr += "\r\n" + GetEntityName;
                }
            }
        }


        public DXFLayerInfo FindSelectDXFObjList()
        {
            DXFLayerInfo ResDXFLayer;
            string GetEntityName;
            DXFLine dxLine;
            DXFCircle dxCircle;
            DXFArc dxArc;
            SFPoint P1, P2;
            double LineLenNum;
            float rd1, rd2;
            float MinX, MinY, MaxX, MaxY;
            int LoopDXFCount;
            double Calc_Gap_X, Calc_Gap_Y;

            ResDXFLayer.ExistLyFlag = false;
            ResDXFLayer.CalcType = 0;
            ResDXFLayer.FindLyFlag = false;
            ResDXFLayer.LyType = SHAPE_CIRCLE;
            ResDXFLayer.Obj_Center.Obj_X = 0;
            ResDXFLayer.Obj_Center.Obj_Y = 0;
            ResDXFLayer.Obj_CenterAngle = 0;
            ResDXFLayer.Obj_CenterDistance = 0;
            ResDXFLayer.Obj_Dia = 0;
            ResDXFLayer.Obj_X1 = 0;
            ResDXFLayer.Obj_X2 = 0;
            ResDXFLayer.Obj_Y1 = 0;
            ResDXFLayer.Obj_Y2 = 0;
            ResDXFLayer.SelectLyFlag = false;

            if (FCADImage == null)
                return ResDXFLayer;

            LoopDXFCount = 0;
            //Bitmap tmp = new Bitmap(bmp, bmp.Width, bmp.Height);

            for (int i = 0; i < 1; i++)
            {
                DXFObjList[i].ExistLyFlag = false;
                DXFObjList[i].LyType = SHAPE_NONE;
            }

            for (int i = 0; i < FCADImage.FEntities.Entities.Count; i++)
            {
                GetEntityName = FCADImage.FEntities.Entities[i].ToString();
                if (GetEntityName == "DXFImportReco.DXFLine")
                {
                    /*

                    dxLine = (DXFLine)FCADImage.FEntities.Entities[i];

                    P1 = GetPoint(dxLine.Point1);
                    P2 = GetPoint(dxLine.Point2);

                    LineLenNum = GetLineDistance(P1, P2);
                    if (LineLenNum > 1.1)
                    {
                        if (MinX > P1.X)
                        {
                            MinX = P1.X;
                        }
                        if (MaxX < P1.X)
                        {
                            MaxX = P1.X;
                        }
                        if (MinY > P1.Y)
                        {
                            MinY = P1.Y;
                        }
                        if (MaxY < P1.Y)
                        {
                            MaxY = P1.Y;
                        }

                        if (MinX > P2.X)
                        {
                            MinX = P2.X;
                        }
                        if (MaxX < P2.X)
                        {
                            MaxX = P2.X;
                        }
                        if (MinY > P2.Y)
                        {
                            MinY = P2.Y;
                        }
                        if (MaxY < P2.Y)
                        {
                            MaxY = P2.Y;
                        }
                    }
                    */
                }
                else if (GetEntityName == "DXFImportReco.DXFCircle")
                {
                    dxCircle = (DXFCircle)FCADImage.FEntities.Entities[i];

                    if (dxCircle.FColor == Color.Red)
                    {

                        rd1 = dxCircle.radius;
                        P1 = dxCircle.Point1;

                        ResDXFLayer.ExistLyFlag = true;
                        ResDXFLayer.LyType = SHAPE_CIRCLE;
                        ResDXFLayer.Obj_Dia = rd1 * 2.0;
                        ResDXFLayer.Obj_X1 = P1.X - (FS_W / 2 + FS_W_Base);
                        ResDXFLayer.Obj_Y1 = (FS_H / 2 + FS_H_Base) - P1.Y;
                        Calc_Gap_X = ResDXFLayer.Obj_X1;
                        Calc_Gap_Y = ResDXFLayer.Obj_Y1;
                        ResDXFLayer.Obj_CenterAngle = Math.Atan2(Calc_Gap_Y, Calc_Gap_X) * 180.0 / 3.1415926535;
                        ResDXFLayer.Obj_CenterDistance = Math.Sqrt(Calc_Gap_X * Calc_Gap_X + Calc_Gap_Y * Calc_Gap_Y);

                        break;

                    }
                }
                else if (GetEntityName == "DXFImportReco.DXFArc")
                {
                    /*
                    dxArc = (DXFArc)FCADImage.FEntities.Entities[i];

                    if (dxArc.pt1.X == 0)
                    {
                        rd1 = dxArc.radius; //Math.Abs(dxArc.radius * dxArc.ratio);
                        rd2 = dxArc.radius;
                    }
                    else
                    {
                        rd1 = dxArc.radius;
                        rd2 = dxArc.radius; //Math.Abs(dxArc.radius * ratio);
                    }

                    rd1 = dxArc.radius;
                    P1 = GetPoint(dxArc.Point1);
                    rd1 = rd1 * FScale;
                    rd2 = rd2 * FScale;
                    P1.X = P1.X - rd1;
                    P1.Y = P1.Y - rd1;
                    float sA = -dxArc.startAngle, eA = -dxArc.endAngle;
                    if (dxArc.endAngle < dxArc.startAngle) sA = Conversion_Angle(sA);
                    eA -= sA;

                    if (MinX > P1.X - rd1)
                    {
                        MinX = P1.X - rd1;
                    }
                    if (MaxX < P1.X + rd1)
                    {
                        MaxX = P1.X + rd1;
                    }
                    if (MinY > P1.Y - rd1)
                    {
                        MinY = P1.Y - rd1;
                    }
                    if (MaxY < P1.Y + rd1)
                    {
                        MaxY = P1.Y + rd1;
                    }
                    */
                }
                else
                {
                    //OutTxtStr += "\r\n" + GetEntityName;
                }
            }

            return ResDXFLayer;
        }


        private void DelayWaitRun(int SetIntvalDelayTime)
        {
            int IntvalDelayTime, CurDelayTime, IntvalDelayTimeValue;

            DelayLoopExitFlag = false;
            IntvalDelayTimeValue = SetIntvalDelayTime; // 10분의 1초단위 0.1초단위임.
            IntvalDelayTime = DelayCurrentSecond() + IntvalDelayTimeValue;
            CurDelayTime = DelayCurrentSecond();
            while (IntvalDelayTime > CurDelayTime)
            {
                Application.DoEvents();
                CurDelayTime = GetCurrent24OverSecond(IntvalDelayTime);
                //this.Text = CurDelayTime.ToString();
                if (DelayLoopExitFlag == true)
                {
                    break;
                }
            }
        }

        private int GetCurrent24OverSecond(int GetIntvalDelayTime)
        {
            int CurDelayTime;
            CurDelayTime = DelayCurrentSecond();
            if (GetIntvalDelayTime > CurDelayTime + 100000) // 10분의 1초단위 0.1초단위임.
            {
                CurDelayTime = CurDelayTime + 864000; // 10분의 1초단위 0.1초단위임.
            }
            return CurDelayTime;
        }

        private int DelayCurrentSecond()
        {
            int ResBackTimeSec;

            DateTime dt = DateTime.Now;

            ResBackTimeSec = (dt.Hour * 3600 + dt.Minute * 60 + dt.Second) * 10 + dt.Millisecond / 100; // 10분의 1초단위 0.1초단위임.
            return ResBackTimeSec;
        }

        private void RunMotionAxis_X(double SetMoveValueAxis)
        {
            double TmpMoveValueAxis = 0;
            short TmpMoveAxis_Index;

            if (SetMoveValueAxis > 780 && SetMoveValueAxis < 1210)
            {
                MultiMotion.GetCurrentPos();
                TmpMoveAxis_Index = MultiMotion.CAM_UNIT_X;

                TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
                MultiMotion.MoveAxis(TmpMoveAxis_Index, SetMoveValueAxis, false);
                while (Math.Abs(MultiMotion.AxisValue[TmpMoveAxis_Index] - SetMoveValueAxis) > 1.0)
                {
                    MultiMotion.GetCurrentPos();
                    //DelayWaitRun(5);
                    CommonUtility.WaitTime(50, true);
                }
            }
            else
            {
                MultiMotion.GetCurrentPos();
                TmpMoveAxis_Index = MultiMotion.CAM_UNIT_Z;

                TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
                if (TmpMoveValueAxis>80)
                {
                    MessageBox.Show("X축이 허용범위를 넘었습니다.");
                }
                else
                {
                    MultiMotion.GetCurrentPos();
                    TmpMoveAxis_Index = MultiMotion.CAM_UNIT_X;

                    TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
                    MultiMotion.MoveAxis(TmpMoveAxis_Index, SetMoveValueAxis, false);
                    while (Math.Abs(MultiMotion.AxisValue[TmpMoveAxis_Index] - SetMoveValueAxis) > 1.0)
                    {
                        MultiMotion.GetCurrentPos();
                        //DelayWaitRun(5);
                        CommonUtility.WaitTime(50, true);
                    }
                    //TmpMoveAxis_Index = MultiMotion.CAM_UNIT_X;
                }
            }
        }

        private void RunMotionAxis_Y(double SetMoveValueAxis)
        {
            double TmpMoveValueAxis = 0;
            short TmpMoveAxis_Index;

            MultiMotion.GetCurrentPos();
            TmpMoveAxis_Index = MultiMotion.CAM_UNIT_Y;

            TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
            MultiMotion.MoveAxis(TmpMoveAxis_Index, SetMoveValueAxis, false);
            while (Math.Abs(MultiMotion.AxisValue[TmpMoveAxis_Index] - SetMoveValueAxis) > 1.0)
            {
                MultiMotion.GetCurrentPos();
                //DelayWaitRun(5);
                CommonUtility.WaitTime(50, true);
            }
            TmpMoveAxis_Index = 0;
        }

        private void RunMotionAxis_Z(double SetMoveValueAxis)
        {
            double TmpMoveValueAxis = 0;
            short TmpMoveAxis_Index;

            MultiMotion.GetCurrentPos();
            TmpMoveAxis_Index = MultiMotion.CAM_UNIT_Z;
            TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
            MultiMotion.MoveAxis(TmpMoveAxis_Index, SetMoveValueAxis, false);
            while (Math.Abs(MultiMotion.AxisValue[TmpMoveAxis_Index] - SetMoveValueAxis) > 1.0)
            {
                MultiMotion.GetCurrentPos();
                //DelayWaitRun(5);
                CommonUtility.WaitTime(50, true);
            }
        }


        private void FullShotMove()
        {
            timerAxis.Enabled = false;
            //double SetMoveValueAxis = 0;
            //double TmpMoveValueAxis = 0;
            //short TmpMoveAxis_Index;

            /*
            SetMoveValueAxis = RecoSetData.Motion_Move_X;
            TmpMoveAxis_Index = MultiMotion.CAM_UNIT_X;

            TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
            MultiMotion.MoveAxis(TmpMoveAxis_Index, SetMoveValueAxis, false);
            while (Math.Abs(MultiMotion.AxisValue[TmpMoveAxis_Index] - TmpMoveValueAxis) > 1.0)
            {
                DelayWaitRun(5);
            }
            */
            RunMotionAxis_X(RecoSetData.Motion_Move_X);

            /*
            SetMoveValueAxis = RecoSetData.Motion_Move_Y;
            TmpMoveAxis_Index = MultiMotion.CAM_UNIT_Y;

            TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
            MultiMotion.MoveAxis(TmpMoveAxis_Index, SetMoveValueAxis, false);
            while (Math.Abs(MultiMotion.AxisValue[TmpMoveAxis_Index] - TmpMoveValueAxis) > 1.0)
            {
                DelayWaitRun(5);
            }
            */
            RunMotionAxis_Y(RecoSetData.Motion_Move_Y);

            /*
            SetMoveValueAxis = RecoSetData.Motion_Move_Z;
            TmpMoveAxis_Index = MultiMotion.CAM_UNIT_Z;

            TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
            MultiMotion.MoveAxis(TmpMoveAxis_Index, SetMoveValueAxis, false);
            while (Math.Abs(MultiMotion.AxisValue[TmpMoveAxis_Index] - TmpMoveValueAxis) > 1.0)
            {
                DelayWaitRun(5);
            }
            */
            RunMotionAxis_Z(RecoSetData.Motion_Move_Z);
            timerAxis.Enabled = true;
        }

        private void DetailShotSelectObjMove()
        {
            SFPoint GetDetailSelObj;

            double Detail_Move_Pos_X, Detail_Move_Pos_Y, Detail_Move_Pos_Z;
            double SetMoveValueAxis = 0;
            double TmpMoveValueAxis = 0;
            short TmpMoveAxis_Index;
            short RotateIndex_Index;

            if (cboIndexList.SelectedIndex == 0)
            {
                RotateIndex_Index = MultiMotion.INDEX_FIX_R;
            }
            else
            {
                RotateIndex_Index = MultiMotion.INDEX_MOVE_R;
            }
             

            GetDetailSelObj = GetSelectDXFData();
            Detail_Move_Pos_X = 0;
            Detail_Move_Pos_Y = 0 - GetDetailSelObj.Y;

            Detail_Move_Pos_Z = GetDetailSelObj.Z;

            TmpMoveAxis_Index = MultiMotion.CAM_UNIT_Y;
            TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
            if (RotateIndex_Index == MultiMotion.INDEX_MOVE_R)
            {
                SetMoveValueAxis = TmpMoveValueAxis + Detail_Move_Pos_Y;
            }
            else
            {
                SetMoveValueAxis = TmpMoveValueAxis - Detail_Move_Pos_Y;
            }

            //SetMoveValueAxis = TmpMoveValueAxis - Detail_Move_Pos_Y;
            MultiMotion.MoveAxis(TmpMoveAxis_Index, SetMoveValueAxis, false);
            while (Math.Abs(MultiMotion.AxisValue[TmpMoveAxis_Index] - TmpMoveValueAxis) > 1.0)
            {
                DelayWaitRun(5);
            }

            TmpMoveAxis_Index = MultiMotion.CAM_UNIT_Z;
            TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
            SetMoveValueAxis = TmpMoveValueAxis - Detail_Move_Pos_Z;
            MultiMotion.MoveAxis(TmpMoveAxis_Index, SetMoveValueAxis, false);
            while (Math.Abs(MultiMotion.AxisValue[TmpMoveAxis_Index] - TmpMoveValueAxis) > 1.0)
            {
                DelayWaitRun(5);
            }
        }

        private int MinusCalcValue(int CurOriginValue)
        {
            int ResMinValue;
            ResMinValue = CurOriginValue - 100;
            if (ResMinValue<0)
            {
                ResMinValue = 0;
            }
            return ResMinValue;
        }

        private double IndexRotationHalf(double IndexCurRotAngle)
        {
            double ResIndexRotAngle;
            ResIndexRotAngle = IndexCurRotAngle;
            if (IndexCurRotAngle<-180.0)
            {
                ResIndexRotAngle = IndexCurRotAngle + 360;
            }
            if (IndexCurRotAngle > 180.0)
            {
                //ResIndexRotAngle = -1 * (360 - IndexCurRotAngle);
                ResIndexRotAngle = IndexCurRotAngle - 360;
            }
            return ResIndexRotAngle;
        }

        private void timerJog_Tick(object sender, EventArgs e)
        {

        }

        private void timWaitDelay_Tick(object sender, EventArgs e)
        {
            timWaitDelay.Enabled = false;
            WaitDelayRunFlag = false;
        }

        private void DelayWaitRun2(int DlyWaitNum)
        {
            WaitDelayRunFlag = false;
            timWaitDelay.Enabled = true;
        }

        private void btCalibrationRun_Click(object sender, EventArgs e)
        {
            Calib_Run_Sts = CALIB_START;
            timCalibration.Enabled = true;
        }

        private void timCalibration_Tick(object sender, EventArgs e)
        {
            int Cur_Temp_Zoom_Status, Cur_Temp_Focus_Status;
            int ZeroLoopCountChkMax = 10;
            short TmpMoveAxis_Index;
            short RotateIndex_Index;
            double SetMoveValueAxis = 0;
            double TmpMoveValueAxis = 0;

            timCalibration.Enabled = false;

            if (cboIndexList.SelectedIndex == 0)
            {
                RotateIndex_Index = MultiMotion.INDEX_FIX_R;
            }
            else
            {
                RotateIndex_Index = MultiMotion.INDEX_MOVE_R;
            }

           if (Calib_Run_Sts == CALIB_START)
            {
                label5.Text = "CALIBRATION START";
                LpWk_ZeroCount = 0;
                Calib_Run_Sts = CALIB_ZOOMFOCUS;
            }
            else if (Calib_Run_Sts == CALIB_ZOOMFOCUS)
            {
                if (LpWk_ZeroCount == 0)
                {
                    label5.Text = "CALIBRATION ZOOMFOCUS";
                    if (RotateIndex_Index == MultiMotion.INDEX_MOVE_R)
                    {
                        CurZoomFactNum = 11733;
                        CurFocusFactNum = 19013;
                    }
                    else
                    {
                        CurZoomFactNum = 11733;
                        CurFocusFactNum = 19514;
                    }
                    LpWk_ZeroCount = 1;
                }

                if (LpWk_ZeroCount > ZeroLoopCountChkMax)
                {
                    Calib_Run_Sts = CALIB_RECOPROCESS;
                    LpWk_ZeroCount = 0;
                }
                else
                {
                    //if (NavitarCtrl.ProductID > 0)
                    if (NavitarCtrl.Connected == true)
                    {
                        Cur_Temp_Zoom_Status = NavitarCtrl.Read(REG_USER_STATUS_1);
                        if (Cur_Temp_Zoom_Status >= 512)
                        {
                            Cur_Temp_Zoom_Status = Cur_Temp_Zoom_Status - 512;
                        }

                        if (Cur_Temp_Zoom_Status == 0)
                        {
                            LpWk_ZeroCount++;
                        }
                        else
                        {
                            LpWk_ZeroCount = 1;
                        }
                    }
                }
                
            }
            else if (Calib_Run_Sts == CALIB_RECOPROCESS)
            {
                if (LpWk_ZeroCount == 0)
                {
                    label5.Text = "CALIBRATION RECOPROCESS";

                    RecoWorkImg = RecoBlankImg;
                    //RealScaleBox.Image.Save("001_1.jpg");

                    if (RecoWorkImg != null)
                    {
                        RecoCls = new clsRecoProcess(ref RecoWorkImg, CamOpticalGap_X[CamOpticalGapIndex], CamOpticalGap_Y[CamOpticalGapIndex]);
                        RecoCls.FindEdge2(1);
                        RecoCls.CutOffFindEdge2();
                        //picRecoImg.Image = RecoCls.GetArrDiplayImage();
                        //picRecoImg.Image = RecoCls.GetArrGrayDiplayImage();

                        RecoCls.FindObject();
                        RecoCls.FindCircleObjSizeArc();
                        DoubleDataPosInfo FindCalbCenterPos = RecoCls.FindCalibrationCenter();
                        //RecoCls.HoughTransform();
                        //RecoCls.FindGetLineData2();
                        //picRecoImg.Image = RecoCls.GetLineArrDiplayImage();
                        //picRecoImg.Image = RecoCls.GetArrDiplayImage();
                        //picRecoImg.Image = RecoCls.GetArrObjectListDiplayImage();
                        CameraMoveXYPos = RecoCls.FindCalibrationCenterAlign(20);
                        picRecoImg.Image = RecoCls.GetArrCalibrationCenterObjectImage(CameraMoveXYPos);
                        //DrawDXFImage((Bitmap)picRecoImg.Image);

                    }
                    PanelPicture.Visible = false;
                    picRecoImg.Visible = true;
                    LpWk_ZeroCount = 1;
                }
                if (LpWk_ZeroCount > ZeroLoopCountChkMax)
                {
                    PanelPicture.Visible = true;
                    picRecoImg.Visible = false;
                    Calib_Run_Sts = CALIB_MOV_POSITION;
                    LpWk_ZeroCount = 0;
                }
                else
                {
                    LpWk_ZeroCount++;
                }
            }
            else if (Calib_Run_Sts == CALIB_MOV_POSITION)
            {
                if (LpWk_ZeroCount == 0)
                {
                    label5.Text = "CALIBRATION MOVE POSITION";

                    if (CameraMoveXYPos.ddp_ExistFlag==true)
                    {
                        TmpMoveAxis_Index = MultiMotion.CAM_UNIT_Y;
                        TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
                        if (RotateIndex_Index == MultiMotion.INDEX_MOVE_R)
                        {
                            SetMoveValueAxis = TmpMoveValueAxis + CameraMoveXYPos.ddp_X;
                        }
                        else
                        {
                            SetMoveValueAxis = TmpMoveValueAxis - CameraMoveXYPos.ddp_X;
                        }
                        RunMotionAxis_Y(SetMoveValueAxis);

                        TmpMoveAxis_Index = MultiMotion.CAM_UNIT_Z;
                        TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
                        SetMoveValueAxis = TmpMoveValueAxis - CameraMoveXYPos.ddp_Y;
                        RunMotionAxis_Z(SetMoveValueAxis);
                    }

                    LpWk_ZeroCount = 1;
                }
                if (LpWk_ZeroCount > ZeroLoopCountChkMax)
                {
                    Calib_Run_Sts = CALIB_RECOPROCESS2;
                    LpWk_ZeroCount = 0;
                }
                else
                {
                    LpWk_ZeroCount++;
                }
            }
            else if (Calib_Run_Sts == CALIB_RECOPROCESS2)
            {
                if (LpWk_ZeroCount == 0)
                {
                    label5.Text = "CALIBRATION RECOPROCESS2";

                    RecoWorkImg = RecoBlankImg;
                    //RealScaleBox.Image.Save("001_1.jpg");

                    if (RecoWorkImg != null)
                    {
                        RecoCls = new clsRecoProcess(ref RecoWorkImg, CamOpticalGap_X[CamOpticalGapIndex], CamOpticalGap_Y[CamOpticalGapIndex]);
                        RecoCls.FindEdge2(1);
                        RecoCls.CutOffFindEdge2();
                        //picRecoImg.Image = RecoCls.GetArrDiplayImage();
                        //picRecoImg.Image = RecoCls.GetArrGrayDiplayImage();

                        RecoCls.FindObject();
                        RecoCls.FindCircleObjSizeArc();
                        DoubleDataPosInfo FindCalbCenterPos = RecoCls.FindCalibrationCenter();
                        //RecoCls.HoughTransform();
                        //RecoCls.FindGetLineData2();
                        //picRecoImg.Image = RecoCls.GetLineArrDiplayImage();
                        //picRecoImg.Image = RecoCls.GetArrDiplayImage();
                        //picRecoImg.Image = RecoCls.GetArrObjectListDiplayImage();
                        CameraMoveXYPos = RecoCls.FindCalibrationCenterAlign(20);
                        picRecoImg.Image = RecoCls.GetArrCalibrationCenterObjectImage(CameraMoveXYPos);
                        //DrawDXFImage((Bitmap)picRecoImg.Image);

                    }
                    PanelPicture.Visible = false;
                    picRecoImg.Visible = true;
                    LpWk_ZeroCount = 1;
                }
                if (LpWk_ZeroCount > ZeroLoopCountChkMax)
                {
                    PanelPicture.Visible = true;
                    picRecoImg.Visible = false;
                    Calib_Run_Sts = CALIB_MOV_POSITION2;
                    LpWk_ZeroCount = 0;
                }
                else
                {
                    LpWk_ZeroCount++;
                }
            }
            else if (Calib_Run_Sts == CALIB_MOV_POSITION2)
            {
                if (LpWk_ZeroCount == 0)
                {
                    label5.Text = "CALIBRATION MOVE POSITION2";

                    if (CameraMoveXYPos.ddp_ExistFlag == true)
                    {
                        TmpMoveAxis_Index = MultiMotion.CAM_UNIT_Y;
                        TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
                        if (RotateIndex_Index == MultiMotion.INDEX_MOVE_R)
                        {
                            SetMoveValueAxis = TmpMoveValueAxis + CameraMoveXYPos.ddp_X;
                        }
                        else
                        {
                            SetMoveValueAxis = TmpMoveValueAxis - CameraMoveXYPos.ddp_X;
                        }
                        RunMotionAxis_Y(SetMoveValueAxis);
                        RecoSetData.Motion_Move_Y = SetMoveValueAxis;


                        TmpMoveAxis_Index = MultiMotion.CAM_UNIT_Z;
                        TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
                        SetMoveValueAxis = TmpMoveValueAxis - CameraMoveXYPos.ddp_Y;
                        RunMotionAxis_Z(SetMoveValueAxis);
                        RecoSetData.Motion_Move_Z = SetMoveValueAxis;

                        if (RotateIndex_Index == MultiMotion.INDEX_MOVE_R)
                        {
                            RealCalibrationData_B.ddp_X = RecoSetData.Motion_Move_Y;
                            RealCalibrationData_B.ddp_Y = RecoSetData.Motion_Move_Z;
                        }
                        else
                        {
                            RealCalibrationData_A.ddp_X = RecoSetData.Motion_Move_Y;
                            RealCalibrationData_A.ddp_Y = RecoSetData.Motion_Move_Z;
                        }

                    }

                    LpWk_ZeroCount = 1;
                }
                if (LpWk_ZeroCount > ZeroLoopCountChkMax)
                {
                    Calib_Run_Sts = CALIB_END;
                    LpWk_ZeroCount = 0;
                }
                else
                {
                    LpWk_ZeroCount++;
                }
            }
            else if (Calib_Run_Sts == CALIB_END)
            {
                SaveCalibSetFiles(SetCalibrationDataFileName);
                label5.Text = "CALIBRATION END";
                MessageBox.Show("Calibration완료!");
                return;
            }
            LpWk_RunTimerCount++;
            timCalibration.Enabled = true;
        }

        private void btSetCalibrationValue_Click(object sender, EventArgs e)
        {
            short TmpMoveAxis_Index;
            short RotateIndex_Index;
            double SetMoveValueAxis = 0;
            double TmpMoveValueAxis = 0;

            if (cboIndexList.SelectedIndex == 0)
            {
                RotateIndex_Index = MultiMotion.INDEX_FIX_R;
            }
            else
            {
                RotateIndex_Index = MultiMotion.INDEX_MOVE_R;
            }
            if(RotateIndex_Index == MultiMotion.INDEX_MOVE_R)
            {
                //TmpMoveAxis_Index = MultiMotion.CAM_UNIT_Y;
                //TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
                SetMoveValueAxis = RealCalibrationData_B.ddp_X;
                RunMotionAxis_Y(SetMoveValueAxis);
                RecoSetData.Motion_Move_Y = SetMoveValueAxis;


                //TmpMoveAxis_Index = MultiMotion.CAM_UNIT_Z;
                //TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
                //SetMoveValueAxis = TmpMoveValueAxis - CameraMoveXYPos.ddp_Y;
                SetMoveValueAxis = RealCalibrationData_B.ddp_Y;
                RunMotionAxis_Z(SetMoveValueAxis);
                RecoSetData.Motion_Move_Z = SetMoveValueAxis;

            }
            else
            {
                //TmpMoveAxis_Index = MultiMotion.CAM_UNIT_Y;
                //TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
                SetMoveValueAxis = RealCalibrationData_A.ddp_X;
                RunMotionAxis_Y(SetMoveValueAxis);
                RecoSetData.Motion_Move_Y = SetMoveValueAxis;


                //TmpMoveAxis_Index = MultiMotion.CAM_UNIT_Z;
                //TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
                //SetMoveValueAxis = TmpMoveValueAxis - CameraMoveXYPos.ddp_Y;
                SetMoveValueAxis = RealCalibrationData_A.ddp_Y;
                RunMotionAxis_Z(SetMoveValueAxis);
                RecoSetData.Motion_Move_Z = SetMoveValueAxis;

            }

        }

        private void frmCalibration_Paint(object sender, PaintEventArgs e)
        {
            /*
            if (FCADImage == null)
                return;
            FCADImage.Draw(e.Graphics);
            */

            if (FCADImage == null)
                return;
            Graphics g = Graphics.FromImage(canvas);

            g.Clear(Color.Black);

            FCADImage.Draw(g);

        }
    }
}
