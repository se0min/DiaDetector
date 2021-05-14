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
    public struct RGBDatInfo
    {
        public byte Red;
        public byte Green;
        public byte Blue;
        public bool ExistFlag;
    }

    public struct RecoStructInfo
    {
        public bool RecoExistTypeFlag;
        public string RecoDXFFileName;
        public string RecoCapFileName;
        public int FullShot_ZoomNum;
        public int FullShot_FocusNum;
        public int DetailShot_ZoomNum;
        public int DetailShot_FocusNum;

        // ----------

        public int ZoomFocus_PortNo;

        public int RecoCamIndex;
        public string RecoCamID;

        public int Rotation_Index;
        public double Rotation_R;
        
        public int Motion_Index;
        public double Motion_Move_X;
        public double Motion_Move_Y;
        public double Motion_Move_Z;

        public int Lamp_Index;
        public int Lamp_PortNo;
        public int Lamp_SetChannel;
        public int Lamp_SetValue;

        public double Move_R_FIX_Angle_Gap;
        public double Move_R_MOVE_Angle_Gap;
        public double Move_R_Move_X_Gap;
        public double Move_R_Move_Y_Gap;
        public double Move_R_Move_Z_Gap;

        public double Object_Weight;
        public double Object_Diameter;
        public double Object_Hole_Distance;
        public double Object_Pin_Height;
        public double Object_Pin_Diameter;

        public double Back_R_MOVE_Angle_Gap;
        public double Top_R_MOVE_Angle_Gap;

        public int Object_TopHoleType;
        public int DXF_RecoType;
        public int Object_Param3;
        public int Object_Param4;
        public int Object_Param5;

        public double Object_TopDirHoleDistance;
        public double Motion_Move_MR;
        public double Object_ShaftLength;
        public double Object_Param9;
        public double Object_Param10;
        public double Object_Param11;
        public double Object_Param12;
    }

    public struct ShotDetailInfo
    {
        public int ShotType;
        public double ShotGapDistance;
        public int ZoomNum;
        public int FocusNum;
    }

    public struct LevelAlignHoleInfo
    {
        public bool In_Level_ExistFlag;
        public double In_Level_X;
        public double In_Level_Y;
        public double In_Level_Angle;
        public double In_Level_Distance;
    }

    public struct LevelAlignInfo
    {
        public LevelAlignHoleInfo In_Level;
        public LevelAlignHoleInfo In_Level_B;
        public double In_Cur_GapAngle;
    }

    public struct DisplayAngleInfo
    {
        public bool FullShot_ExistFlag;
        public double FullShotAngle;
        public bool DetailShot_ExistFlag;
        public double DetailShotAngle;
        public bool AddGap_ExistFlag;
        public double AddGapAngle;
    }


    public partial class frmReco : Form
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
        public const string SetCalibPosDataFileName = "CalibPosValue.dat";
        public bool RunCamPlayFlag = false;

        public bool GuidLineFlag = false;
        public bool DXFDataViewFlag = true;

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
        private ShotDetailInfo[] DxfShotDataList_A = new ShotDetailInfo[19];
        private ShotDetailInfo[] DxfShotDataList_B = new ShotDetailInfo[19];
        private ShotDetailInfo[] DxfShotDataList_C = new ShotDetailInfo[19];
        private ShotDetailInfo[] DxfShotDataList_D = new ShotDetailInfo[19];
        private ShotDetailInfo[] DxfShotDataList_E = new ShotDetailInfo[19];
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
        public Point BaseDefault;
        float FS_W, FS_H, FS_W_Base, FS_H_Base;
        bool StartCalcFlag;
        bool LoadDXFCalcFlag = false;
        private string CurDXFFileNameStr = "";

        public const int SHAPE_NONE = 0;
        public const int SHAPE_CIRCLE = 1;
        public const int SHAPE_LINE = 2;
        DXFLayerInfo[] DXFObjList;
        private const int DXFOBJLIST_MAX = 100;
        double IndexFindRoAngle = 0;
        double IndexGapAddRoAngle = -0.8544;

        float CalcReScalePer = 92.0f;//77.0f;


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
        private const int INDEXROTATE_GAPADD = 14;
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
        DoubleDataPosInfo RealCalibrationData_C;
        DoubleDataPosInfo RealCalibrationData_D;
        DoubleDataPosInfo RealCalibrationData_E;
        DoubleDataPosInfo RealCalibrationData_F;
        DoubleDataPosInfo RealCalibrationData_G;
        DoubleDataPosInfo RealCalibrationData_H;

        DoubleDataXYZPosInfo Calibration_Origin;
        DoubleDataXYZPosInfo Calibration_Current;
        DoubleDataXYZPosInfo Calibration_GapNum;


        private int CamOpticalGap_Base_X, CamOpticalGap_Base_Y;
        private int[] CamOpticalGap_X;
        private int[] CamOpticalGap_Y;
        private int CamOpticalGapIndex;
        private bool DXFDrawOptAdjFlag = false;

        public const int SETTYPE_NONE = 700;
        public const int SETTYPE_FIX_FRONT = 710;
        public const int SETTYPE_FIX_BACK = 720;
        public const int SETTYPE_MOVE_FRONT = 730;
        public const int SETTYPE_MOVE_BACK = 740;
        public const int SETTYPE_MOVE_TOP = 750;

        public int SetTypeIndex = SETTYPE_NONE;

        private int SaveIncIndex;

        private const int RE_ALIGN_NONE = 0;
        private const int RE_ALIGN_TRANSLATION = 1;
        private const int RE_ALIGN_ANGLE_DIVIDE = 2;
        private int ReAlignIndex = RE_ALIGN_ANGLE_DIVIDE;

        private const int RECO_TYPE_NONE = 0;
        private const int RECO_TYPE_HOLE = 1;
        private const int RECO_TYPE_HOLE_ONELINE = 2;
        private const int RECO_TYPE_LINE = 3;

        private DisplayAngleInfo DisplayAngleDat;
        private const int DIS_TYPE_FULLSHOT = 0;
        private const int DIS_TYPE_DETAILSHOT = 1;
        private const int DIS_TYPE_ADDANGLE = 2;

        private const double OBJECT_WEIGHT_MAX= 50.0;
        private const double FIX_INDEX_POS = 870.0;
        private const double MOVE_INDEX_POS = 1190.0;
        private const double MOVE_BACK_INDEX_X_POS = 0.0;
        private const double MOVE_BACK_INDEX_INDEX_POS = 800.0;
        private const double MOVE_BACK_INDEX_INDEX_POS_CUTOFF = MOVE_BACK_INDEX_INDEX_POS + 1.0;
        private const double FIX_INDEX_POS_MIN = 820.0;
        private const double MOVE_INDEX_POS_MAX = 1240.0;

#endregion Data ...


#region 강성호 ...

        public frmReco()
        {
            InitializeComponent();

            SaveIncIndex = 0;
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

            CamOpticalGap_Base_X = 924;
            CamOpticalGap_Base_Y = 773;
            CamOpticalGap_X[0] = 460;
            CamOpticalGap_Y[0] = 399;
            CamOpticalGap_X[1] = 457;
            CamOpticalGap_Y[1] = 374;
            CamOpticalGap_X[2] = 430;// 484;// (CamOpticalGap_Base_X - 10) / 2;
            CamOpticalGap_Y[2] = 371;// 391;// (CamOpticalGap_Base_Y - 10) / 2;
            CamOpticalGap_X[3] = (CamOpticalGap_Base_X - 10) / 2;
            CamOpticalGap_Y[3] = (CamOpticalGap_Base_Y - 10) / 2;

            Base.X = -500;
            Base.Y = 500;
        }

        private void frmReco_Load(object sender, EventArgs e)
        {
            LampComm.Open(DeviceManager.LightingComPort);

            TopHoleTypeList();
            DXFTypeList();

            RealCalibrationData_A.ddp_X = 66.92;
            RealCalibrationData_A.ddp_Y = 543.66;
            RealCalibrationData_B.ddp_X = 59.24;
            RealCalibrationData_B.ddp_Y = 536.13;
            RealCalibrationData_C.ddp_X = 0.0;
            RealCalibrationData_C.ddp_Y = 0.0;
            RealCalibrationData_D.ddp_X = 0.0;
            RealCalibrationData_D.ddp_Y = 0.0;

            RealCalibrationData_E.ddp_X = 0.0;
            RealCalibrationData_E.ddp_Y = 0.0;
            RealCalibrationData_F.ddp_X = 0.0;
            RealCalibrationData_F.ddp_Y = 0.0;
            RealCalibrationData_G.ddp_X = 0.0;
            RealCalibrationData_G.ddp_Y = 0.0;
            RealCalibrationData_H.ddp_X = 0.0;
            RealCalibrationData_H.ddp_Y = 0.0;

            ViewDXFCadFlag = true;
            FullDetailShotFlag = FULLSHOTTYPE;
            StartCalcFlag = false;
            LoadDXFCalcFlag = false;

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

            SetDXFShotDataInit();

            LoadCalibPosSetFiles(SetCalibPosDataFileName);

            DXFObjList = new DXFLayerInfo[DXFOBJLIST_MAX];

            btDetailShot.ForeColor = Color.Black;
            btDetailShot.Font = new Font(btDetailShot.Font, FontStyle.Regular);

            btFullShot.ForeColor = Color.Red;
            btFullShot.Font = new Font(btFullShot.Font, FontStyle.Bold);

            cboZoomCamList.Items.Clear();

            for(int i=0;i<4;i++)
            {
                cboZoomCamList.Items.Add(DataManager.CameraSettingInfoList[i].Name.ToString());
            }


            if (SetTypeIndex == SETTYPE_NONE)
            {
                // 홍동성 => 상단 콤보 박스 ...
                // ----------
                Initialize();
                // ----------
            }
            else
            {
                // 홍동성 => 상단 콤보 박스 ...
                // ----------
                Initialize();
                // ----------
            }


            if (SetTypeIndex == SETTYPE_NONE)
            {
            }
            else
            {
                if (SetTypeIndex == SETTYPE_FIX_FRONT)
                {
                    RecoSetData.Motion_Move_X = 863;
                    RecoSetData.Motion_Move_Y = RealCalibrationData_A.ddp_X;
                    RecoSetData.Motion_Move_Z = RealCalibrationData_A.ddp_Y;

                    CurZoomFactNum = 4317;
                    CurFocusFactNum = 19584;

                    RecoSetData.FullShot_ZoomNum = CurZoomFactNum;
                    RecoSetData.FullShot_FocusNum = CurFocusFactNum;
                    RecoSetData.DetailShot_ZoomNum = 11755;
                    RecoSetData.DetailShot_FocusNum = 19584;

                    ShotData[FULLSHOTTYPE].ZoomNum = CurZoomFactNum;
                    ShotData[FULLSHOTTYPE].FocusNum = CurFocusFactNum;
                    ShotData[DETAILSHOTTYPE].ZoomNum = RecoSetData.DetailShot_ZoomNum;
                    ShotData[DETAILSHOTTYPE].FocusNum = RecoSetData.DetailShot_FocusNum;

                }
                else if (SetTypeIndex == SETTYPE_FIX_BACK)
                {
                }
                else if (SetTypeIndex == SETTYPE_MOVE_FRONT)
                {
                    RecoSetData.Motion_Move_X = 1193;
                    RecoSetData.Motion_Move_Y = RealCalibrationData_B.ddp_X;
                    RecoSetData.Motion_Move_Z = RealCalibrationData_B.ddp_Y;

                    CurZoomFactNum = 3692;
                    CurFocusFactNum = 19081;

                    RecoSetData.FullShot_ZoomNum = CurZoomFactNum;
                    RecoSetData.FullShot_FocusNum = CurFocusFactNum;
                    RecoSetData.DetailShot_ZoomNum = 11716;
                    RecoSetData.DetailShot_FocusNum = 19081;

                    ShotData[FULLSHOTTYPE].ZoomNum = CurZoomFactNum;
                    ShotData[FULLSHOTTYPE].FocusNum = CurFocusFactNum;
                    ShotData[DETAILSHOTTYPE].ZoomNum = RecoSetData.DetailShot_ZoomNum;
                    ShotData[DETAILSHOTTYPE].FocusNum = RecoSetData.DetailShot_FocusNum;

                }
                else if (SetTypeIndex == SETTYPE_MOVE_BACK)
                {
                }
                else if (SetTypeIndex == SETTYPE_MOVE_TOP)
                {
                }
            }

            // 설정 파일 읽기 ... => 읽은 후 대화상자 정보를 업데이트 해야 함.
            // ----------
            CreateMakeFolderFunc(CfgSaveFolderPath);

            if (CfgSaveHeadFileName != null)
            {
                if (CfgSaveHeadFileName.Length > 0)
                {
                    CfgSaveFolderPath_FileName = CfgSaveFolderPath + @"\" + CfgSaveHeadFileName + @".dat";

                    LoadCfgFiles(CfgSaveFolderPath_FileName);

                    DisplayGapAngleAndThickness();

                    // 설정치 화면 복원 ...
                    // ----------

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
            }
            else
            {
                CfgSaveFolderPath_FileName = "";
            }

            LoadCalibSetFiles(SetCalibrationDataFileName);
            

            // ...
            // ----------
            UInt64[] WorkUID = new UInt64[0];

            RGBListFlag=false;
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

            if (SetTypeIndex == SETTYPE_NONE)
            {
                if (CfgSaveHeadFileName != null)
                {
                    if (CfgSaveHeadFileName.Length > 0)
                    {
                        if (RecoSetData.RecoCamIndex > -1)
                        {
                            cboZoomCamList.SelectedIndex = RecoSetData.RecoCamIndex;
                        }
                    }
                }
            }
            else
            {
                if (SetTypeIndex == SETTYPE_FIX_FRONT)
                {
                    cboZoomCamList.SelectedIndex = 0;
                    cboIndexList.SelectedIndex = 0;
                    cboMoveList.SelectedIndex = 0;
                    cboLampList.SelectedIndex = 0;
                    //RecoSetData.Motion_Move_X = 860;
                    //RecoSetData.Motion_Move_Y = RealCalibrationData_A.ddp_X;
                    //RecoSetData.Motion_Move_Z = RealCalibrationData_A.ddp_Y;
                }
                else if (SetTypeIndex == SETTYPE_FIX_BACK)
                {
                    cboZoomCamList.SelectedIndex = 2;
                    cboIndexList.SelectedIndex = 0;
                    cboMoveList.SelectedIndex = 1;
                    cboLampList.SelectedIndex = 3;
                }
                else if (SetTypeIndex == SETTYPE_MOVE_FRONT)
                {
                    cboZoomCamList.SelectedIndex = 1;
                    cboIndexList.SelectedIndex = 1;
                    cboMoveList.SelectedIndex = 0;
                    cboLampList.SelectedIndex = 1;
                    //RecoSetData.Motion_Move_X = 1200;
                    //RecoSetData.Motion_Move_Y = RealCalibrationData_B.ddp_X;
                    //RecoSetData.Motion_Move_Z = RealCalibrationData_B.ddp_Y;
                }
                else if (SetTypeIndex == SETTYPE_MOVE_BACK)
                {
                    cboZoomCamList.SelectedIndex = 0;
                    cboIndexList.SelectedIndex = 1;
                    cboMoveList.SelectedIndex = 0;
                    cboLampList.SelectedIndex = 4;
                }
                else if (SetTypeIndex == SETTYPE_MOVE_TOP)
                {
                    cboZoomCamList.SelectedIndex = 3;
                    cboIndexList.SelectedIndex = 1;
                    cboMoveList.SelectedIndex = 0;
                    cboLampList.SelectedIndex = 2;
                }
                TopSelectViewLock();
            }

            timer1.Enabled = true;

            radioBtnMidium.Checked = true;

            // ZOOM & FOCUS ...
            // ----------
            //NavitarCtrl = new ControllerLegacy("COM1");
            //NavitarCtrl.Connect();

            MultiMotion.IndexRPosClear();
            MultiMotion.SetSpeed(MultiMotion.KSM_SPEED_MIDIUM);
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
            frmReco CameraListRef = (frmReco)param.Target;

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
                XCCam = null;
            }

            System.GC.Collect();
            System.GC.WaitForPendingFinalizers();
        }

        private static void ImageCallback(XCCAM XCCam, IntPtr pInBuf, UInt32 Length, UInt32 iWidth, UInt32 iHeight, XCCAM_IMAGEDATAINFO Info, IntPtr Context)
        {
            GCHandle param = GCHandle.FromIntPtr(Context);
            frmReco VRef = (frmReco)param.Target;
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
                if (ExitRecoFormFlag==false)
                {
                    SetDisplayImageAutoCallback d = new SetDisplayImageAutoCallback(SetDisplayImageAuto);
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
                    int calc_gap_x, calc_gap_y;
                    if (BaseDefault.X == 0 && BaseDefault.Y == 0)
                    {
                        calc_gap_x = 0;
                        calc_gap_y = 0;
                    }
                    else
                    {
                        calc_gap_x = CamOpticalGap_X[CamOpticalGapIndex] - BaseDefault.X;
                        calc_gap_y = BaseDefault.Y - CamOpticalGap_Y[CamOpticalGapIndex];
                    }
                    objShotBitmap = ReSizeCurBitmap2(objShotBitmap, (int)PanelPicture.Width - 10, (int)PanelPicture.Height - 10, calc_gap_x, calc_gap_y);//objShotBitmap;
                    RecoBlankImg = ReSizeCurBitmap(objShotBitmap, (int)PanelPicture.Width - 10, (int)PanelPicture.Height - 10);//objShotBitmap;
                    RealScaleBox.Image = objShotBitmap;
                    DrawDXFImageCalc(RealScaleBox.Width, RealScaleBox.Height, false);
                    if (ViewDXFCadFlag == true)
                    {
                        DrawDXFImage2((Bitmap)RealScaleBox.Image);
                    }
                    DrawGuideInfo((Bitmap)RealScaleBox.Image);
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
                FCADImage = new CADImage();
                Base.X = 1;
                Base.Y = 1;
                FScale = 1.0f;
                //timer1.Interval = 600;
                //timer1.Enabled = true;
                FCADImage.LoadFromFile(openFileDialog1.FileName);
                CurDXFFileNameStr = openFileDialog1.FileName;
                StartCalcFlag = true;
                LoadDXFCalcFlag = true;
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
                Bitmap newBitmap = new Bitmap(OriginalImg,width, height);
                return newBitmap;
            }
        }
        static Bitmap ReSizeCurBitmap2(Bitmap OriginalImg, int width, int height, int gap_x, int gap_y)
        {
            unsafe
            {
                Bitmap newBitmap = new Bitmap(width, height);
                Graphics thumbGraph = Graphics.FromImage(newBitmap);
                thumbGraph.FillRectangle(new SolidBrush(Color.Black), 0, 0, width, height);
                thumbGraph.DrawImage(OriginalImg, gap_x, gap_y, width, height);
                //thumbGraph.DrawImage(OriginalImg, 0,0, width, height);
                thumbGraph.Flush();
                return newBitmap;
            }
        }
        static Bitmap GetTotalImg2(Bitmap OriginalImg, int width, int height)
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



        private void frmReco_Paint(object sender, PaintEventArgs e)
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

        private void frmReco_FormClosing(object sender, FormClosingEventArgs e)
        {
            ExitRecoFormFlag = true;

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

            CamOpticalGap_X = null;
            CamOpticalGap_Y = null;
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
            int SetTrackValue1, SetTrackValue2;
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
                    SetTrackValue1 = NavitarCtrl.Read(REG_USER_CURRENT_1);
                    if (trackBar1.Maximum < SetTrackValue1)
                    {
                        SetTrackValue1 = trackBar1.Maximum;
                    }
                    trackBar1.Value = SetTrackValue1;
                }
                if (TrackValue2 < 0)
                {
                    trackBar2.Value = 0;
                }
                else
                {
                    SetTrackValue2 = NavitarCtrl.Read(REG_USER_CURRENT_2);
                    if (trackBar2.Maximum < SetTrackValue2)
                    {
                        SetTrackValue2 = trackBar2.Maximum;
                    }
                    trackBar2.Value = SetTrackValue2;
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
            PanelPicture.Visible = true;
            picRecoImg.Visible = false;

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
                W_FScale = ((float)DrImgW * CalcReScalePer / 100.0f) / FS_W;
                H_FScale = ((float)DrImgH * CalcReScalePer / 100.0f) / FS_H;

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
                    P1 = dxArc.Point1;

                    float sA = -dxArc.startAngle, eA = -dxArc.endAngle;
                    if (dxArc.endAngle < dxArc.startAngle) sA = Conversion_Angle(sA);
                    eA -= sA;

                    if (eA == 0)
                    {
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
                W_FScale = ((float)DrImgW * CalcReScalePer / 100.0f) / FS_W;
                H_FScale = ((float)DrImgH * CalcReScalePer / 100.0f) / FS_H;
                if (W_FScale < H_FScale)
                {
                    FScale = W_FScale;
                }
                else
                {
                    FScale = H_FScale;
                }

                //Base.X = DrImgW / 2;
                //Base.Y = DrImgH / 2;
                BaseDefault.X = DrImgW / 2;
                BaseDefault.Y = DrImgH / 2;
                if (DXFDrawOptAdjFlag==true)
                {
                    if (CamOpticalGapIndex > -1 && CamOpticalGapIndex < 4)
                    {
                        Base.X = CamOpticalGap_X[CamOpticalGapIndex];
                        Base.Y = CamOpticalGap_Y[CamOpticalGapIndex];
                    }
                    else
                    {
                        Base.X = BaseDefault.X;
                        Base.Y = BaseDefault.Y;
                    }
                }
                else
                {
                    Base.X = BaseDefault.X;
                    Base.Y = BaseDefault.Y;
                }


                StartCalcFlag = false;
                if(LoadDXFCalcFlag == true)
                {
                    LoadDXFCalcFlag = false;
                    DXFReFocus_Zoom();
                }
            }
            //bmp = tmp;
        }

        public void DXFSelectHoleClear()
        {
            string GetEntityName;
            DXFLine dxLine;
            DXFCircle dxCircle;
            DXFArc dxArc;

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
                    dxLine = (DXFLine)FCADImage.FEntities.Entities[i];
                    dxLine.SelectObjFlag = false;
                    dxLine.SelectHoleLevelObjFlag = false;
                    dxLine.SelectHoleLevelObjIndex = 0;
                }
                else if (GetEntityName == "DXFImportReco.DXFCircle")
                {
                    dxCircle = (DXFCircle)FCADImage.FEntities.Entities[i];
                    dxCircle.SelectObjFlag = false;
                    dxCircle.SelectHoleLevelObjFlag = false;
                    dxCircle.SelectHoleLevelObjIndex = 0;
                }
                else if (GetEntityName == "DXFImportReco.DXFArc")
                {
                    dxArc = (DXFArc)FCADImage.FEntities.Entities[i];
                    dxArc.SelectObjFlag = false;
                    dxArc.SelectHoleLevelObjFlag = false;
                    dxArc.SelectHoleLevelObjIndex = 0;
                }
                else
                {
                }
            }
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
                        W_FScale = ((float)ImgCur_W * CalcReScalePer / 100.0f) / Real_W;
                        H_FScale = ((float)ImgCur_H * CalcReScalePer / 100.0f) / Real_H;
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

            Pen SelectHolePen_2 = new Pen(Color.DarkBlue, 2);
            Pen SelectHolePen_1 = new Pen(Color.Blue, 4);

            using (var graphics = Graphics.FromImage(bmp))
            {
                for (int i = 0; i < FCADImage.FEntities.Entities.Count; i++)
                {
                    GetEntityName = FCADImage.FEntities.Entities[i].ToString();
                    if (GetEntityName == "DXFImportReco.DXFLine")
                    {
                        
                        dxLine = (DXFLine)FCADImage.FEntities.Entities[i];

                        //blackPen.Color = dxLine.FColor;

                        if (dxLine.FColor == Color.Red)
                        {
                            blackPen.Color = dxLine.FColor;
                        }
                        else
                        {
                            blackPen.Color = Color.GreenYellow;
                        }


                        P1 = GetPoint(dxLine.Point1);
                        P2 = GetPoint(dxLine.Point2);

                        graphics.DrawLine(blackPen, P1.X, P1.Y, P2.X, P2.Y);
                        
                    }
                    else if (GetEntityName == "DXFImportReco.DXFCircle")
                    {
                        dxCircle = (DXFCircle)FCADImage.FEntities.Entities[i];

                        if (dxCircle.FColor == Color.Red)
                        {
                            blackPen.Color = dxCircle.FColor;
                        }
                        else
                        {
                            blackPen.Color = Color.GreenYellow;
                        }

                        rd1 = dxCircle.radius;
                        P1 = GetPoint(dxCircle.Point1);
                        rd1 = rd1 * FScale;
                        P1.X = P1.X - rd1;
                        P1.Y = P1.Y - rd1;

                        if (dxCircle.SelectHoleLevelObjFlag == true)
                        {
                            if (dxCircle.SelectHoleLevelObjIndex == 1)
                            {
                                graphics.DrawEllipse(SelectHolePen_1, P1.X, P1.Y, rd1 * 2, rd1 * 2);
                            }
                            else if (dxCircle.SelectHoleLevelObjIndex == 2)
                            {
                                graphics.DrawEllipse(SelectHolePen_2, P1.X, P1.Y, rd1 * 2, rd1 * 2);
                            }
                        }
                        else
                        {
                            if (dxCircle.SelectObjFlag == true)
                            {
                                graphics.DrawEllipse(SelectPen, P1.X, P1.Y, rd1 * 2, rd1 * 2);
                            }
                            else
                            {
                                graphics.DrawEllipse(blackPen, P1.X, P1.Y, rd1 * 2, rd1 * 2);
                            }
                        }

                        
                    }
                    else if (GetEntityName == "DXFImportReco.DXFArc")
                    {
                        
                        dxArc = (DXFArc)FCADImage.FEntities.Entities[i];

                        //blackPen.Color = dxArc.FColor;

                        if (dxArc.FColor == Color.Red)
                        {
                            blackPen.Color = dxArc.FColor;
                        }
                        else
                        {
                            blackPen.Color = Color.GreenYellow;
                        }



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
                            graphics.DrawArc(blackPen, P1.X, P1.Y, rd1 * 2, rd2 * 2, sA, eA);//sA, eA);
                        }
                    }
                    else
                    {
                        //OutTxtStr += "\r\n" + GetEntityName;
                    }
                }
                //graphics.DrawLine(blackPen, 0, Dr_Cen_Y, Dr_Cen_X, Dr_Cen_Y);
                if(GuidLineFlag == true)
                {
                    P1.X = 0;
                    P1.Y = Dr_Cen_Y;
                    P2.X = Dr_Cen_X;
                    P2.Y = Dr_Cen_Y;

                    graphics.DrawLine(SelectPen, P1.X, P1.Y, P2.X, P2.Y);
                }

                string OutPutAngleStr = GetDisplayAngleDataStr();
                if(OutPutAngleStr.Length>0)
                {
                    RectangleF rectf = new RectangleF(5, 5, 350, 80);
                    graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                    graphics.DrawString(OutPutAngleStr, new Font("Arial", 13), Brushes.Red, rectf);
                }

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


        public void DrawGuideInfo(Bitmap bmp)
        {
            SFPoint P1, P2;
            int Dr_Cen_X, Dr_Cen_Y;

            Dr_Cen_X = bmp.Width;
            Dr_Cen_Y = bmp.Height / 2;

            Pen blackPen = new Pen(Color.LightGreen, 2);

            Pen SelectPen = new Pen(Color.Red, 2);

            using (var graphics = Graphics.FromImage(bmp))
            {
                if (GuidLineFlag == true)
                {
                    P1.X = 0;
                    P1.Y = Dr_Cen_Y;
                    P2.X = Dr_Cen_X;
                    P2.Y = Dr_Cen_Y;

                    graphics.DrawLine(SelectPen, P1.X, P1.Y, P2.X, P2.Y);
                }

            }
        }


        public float Conversion_Angle(float Val)
        {
            while (Val < 0) Val = Val + 360;
            return Val;
        }

        private void timWork_Tick(object sender, EventArgs e)
        {
            int Cur_Temp_Zoom_Status, Cur_Temp_Focus_Status;
            int ZeroLoopCountChkMax = 3;
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
            bool BackAlignProcFlag = false;
            bool MoveBackAlignProcFlag = false;
            double ObjectPlWeight, ObjPLAddMove_X;


            if (cboZoomCamList.SelectedIndex == 0 && cboIndexList.SelectedIndex == 1)
            {
                MoveBackAlignProcFlag = true;
            }

            if (cboZoomCamList.SelectedIndex == 3)
            {
                TopAlignProcFlag = true;
            }

            if (cboZoomCamList.SelectedIndex == 2)
            {
                BackAlignProcFlag = true;
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

                if (ExitRecoFormFlag == true)
                {
                    return;
                }

                if (LpWk_RUN_STATUS == WORKSTART)
                {
                    ClearDisplayAngleData();
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

                        if (BackAlignProcFlag == true)
                        {
                            MultiMotion.GetCurrentPos();
                            SetMoveValueAxis = RecoSetData.Motion_Move_Z;
                            TmpMoveAxis_Index = MultiMotion.BACK_CAM_Z;
                            TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];

                            if (RecoSetData.Motion_Move_Z > 80)
                            {
                                MessageBox.Show("Z축 위치가 너무 낮습니다. 테스트를 종료합니다.");
                                timerAxis.Enabled = true;
                                return;
                            }

                            RunMotionAxis_Back_Z(RecoSetData.Motion_Move_Z);

                            if (ExitRecoFormFlag == true)
                            {
                                //MultiMotion.SetSpeed(MultiMotion.KSM_SPEED_SLOW);
                                return;
                            }


                        }
                        else if (TopAlignProcFlag == true)
                        {

                            MultiMotion.GetCurrentPos();
                            SetMoveValueAxis = RecoSetData.Motion_Move_Z;
                            TmpMoveAxis_Index = MultiMotion.CAM_UNIT_Z;
                            TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];

                            if (RecoSetData.Motion_Move_Z > 80)
                            {
                                MessageBox.Show("Z축 위치가 너무 낮습니다. 테스트를 종료합니다.");
                                timerAxis.Enabled = true;
                                //MultiMotion.SetSpeed(MultiMotion.KSM_SPEED_SLOW);
                                return;
                            }

                            RunMotionAxis_Z(RecoSetData.Motion_Move_Z);
                            RunMotionAxis_X(RecoSetData.Motion_Move_X);
                            RunMotionAxis_Y(RecoSetData.Motion_Move_Y);

                            if (ExitRecoFormFlag == true)
                            {
                                return;
                            }


                            //MOTION이동부분
                            //=============================================
                            //MultiMotion.MoveAxis(MultiMotion.CAM_UNIT_X, RecoSetData.Motion_Move_X, true);
                            //MultiMotion.MoveAxis(MultiMotion.CAM_UNIT_Y, RecoSetData.Motion_Move_Y, true);
                            //MultiMotion.MoveAxis(MultiMotion.CAM_UNIT_Z, RecoSetData.Motion_Move_Z, true);
                            //=============================================

                        }
                        else if (MoveBackAlignProcFlag == true)
                        {
                            //RunMotionAxis_X(RecoSetData.Motion_Move_X);
                            //RunMotionAxis_Y(RecoSetData.Motion_Move_Y);
                            //RunMotionAxis_Z(RecoSetData.Motion_Move_Z);
                            if (RecoSetData.Motion_Move_MR < MOVE_BACK_INDEX_INDEX_POS_CUTOFF)
                            {
                                MessageBox.Show("이동축의 위치가 후방카메라와 겹칩니다.");
                                return;
                            }

                            MultiMotion.GetCurrentPos();
                            TmpMoveAxis_Index = MultiMotion.INDEX_MOVE_M;
                            TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];

                            if (TmpMoveValueAxis < 800.0)
                            {
                                RunMotionAxis_Z(1.0);
                            }

                            RunMotionAxis_MR(RecoSetData.Motion_Move_MR);
                            RunMotionAxis_X(RecoSetData.Motion_Move_X);
                            RunMotionAxis_Y(RecoSetData.Motion_Move_Y);
                            RunMotionAxis_Z(RecoSetData.Motion_Move_Z);

                            if (ExitRecoFormFlag == true)
                            {
                                return;
                            }

                        }
                        else
                        {
                            MultiMotion.GetCurrentPos();
                            TmpMoveAxis_Index = MultiMotion.INDEX_MOVE_M;
                            TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];

                            if (TmpMoveValueAxis > 10.0)
                            {
                                RunMotionAxis_Z(1.0);
                                RunMotionAxis_MR(1.0);
                            }

                            ObjectPlWeight = RecoSetData.Object_Weight;
                            if (ObjectPlWeight > OBJECT_WEIGHT_MAX)
                            {
                                ObjectPlWeight = OBJECT_WEIGHT_MAX;
                            }
                            else if (ObjectPlWeight < 0)
                            {
                                ObjectPlWeight = 0;
                            }

                            if (RotateIndex_Index == MultiMotion.INDEX_MOVE_R)
                            {
                                //ObjPLAddMove_X = RecoSetData.Motion_Move_X + ObjectPlWeight;
                                ObjPLAddMove_X = MOVE_INDEX_POS + ObjectPlWeight;
                            }
                            else
                            {
                                //ObjPLAddMove_X = RecoSetData.Motion_Move_X - ObjectPlWeight;
                                ObjPLAddMove_X = FIX_INDEX_POS - ObjectPlWeight;
                            }

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

                            //RunMotionAxis_X(RecoSetData.Motion_Move_X);
                            RunMotionAxis_X(ObjPLAddMove_X);

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

                            if (ExitRecoFormFlag == true)
                            {
                                return;
                            }


                            //=============================================
                        }

                    }
                    if (LpWk_ZeroCount > ZeroLoopCountChkMax)
                    {
                        UpdatePos();
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

                    if (ExitRecoFormFlag == true)
                    {
                        return;
                    }

                    if (LpWk_ZeroCount == 0)
                    {
                        StartCalcFlag = true;

                        //Base.X = RealScaleBox.Image.Width / 2;
                        //Base.Y = RealScaleBox.Image.Height / 2;

                        BaseDefault.X = RealScaleBox.Image.Width / 2;
                        BaseDefault.Y = RealScaleBox.Image.Height / 2;
                        if (DXFDrawOptAdjFlag == true)
                        {
                            if (CamOpticalGapIndex > -1 && CamOpticalGapIndex < 4)
                            {
                                Base.X = CamOpticalGap_X[CamOpticalGapIndex];
                                Base.Y = CamOpticalGap_Y[CamOpticalGapIndex];
                            }
                            else
                            {
                                Base.X = BaseDefault.X;
                                Base.Y = BaseDefault.Y;
                            }
                        }
                        else
                        {
                            Base.X = BaseDefault.X;
                            Base.Y = BaseDefault.Y;
                        }

                        DrawDXFImageCalc(RealScaleBox.Image.Width, RealScaleBox.Image.Height, false);


                        CurZoomFactNum = ShotData[FULLSHOTTYPE].ZoomNum;
                        CurFocusFactNum = ShotData[FULLSHOTTYPE].FocusNum;
                        LpWk_ZeroCount = 1;

                        if (ExitRecoFormFlag == true)
                        {
                            return;
                        }


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

                    if (ExitRecoFormFlag == true)
                    {
                        return;
                    }

                    if (LpWk_ZeroCount == 0)
                    {
                        RecoWorkImg = RecoBlankImg;
                        //RealScaleBox.Image.Save("001_1.jpg");

                        if (BackAlignProcFlag == true)
                        {
                            if (RecoWorkImg != null)
                            {
                                bool LineCheckFlag;
                                LineCheckFlag = false;
                                if (cboDXFType.SelectedIndex == RECO_TYPE_HOLE)
                                {
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
                                    //RecoCls.FindEdge2(1);
                                    RecoCls.FindEdge_BackShadow(1);
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
                                    RecoCls.FindObjectCenterAngle5();
                                }
                                else if (cboDXFType.SelectedIndex == RECO_TYPE_HOLE_ONELINE)
                                {
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
                                    //RecoCls.FindEdge2(1);
                                    RecoCls.FindEdge_BackShadow(1);
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
                                    RecoCls.FindObjectCenterAngle5();
                                }
                                else if (cboDXFType.SelectedIndex == RECO_TYPE_LINE)
                                {
                                    int DXFLineCountNum;
                                    DXFLineCountNum = 0;
                                    GetDXFObjList();
                                    for (int i = 0; i < DXFOBJLIST_MAX; i++)
                                    {
                                        if (DXFObjList[i].ExistLyFlag == true)
                                        {
                                            if (DXFObjList[i].LyType != SHAPE_LINE)
                                            {
                                                //LineCheckFlag = true;
                                                DXFObjList[i].ExistLyFlag = false;
                                            }
                                            else
                                            {
                                                DXFLineCountNum++;
                                            }
                                        }
                                    }
                                    RecoCls = new clsRecoProcess(ref RecoWorkImg, CamOpticalGap_X[CamOpticalGapIndex], CamOpticalGap_Y[CamOpticalGapIndex]);
                                    //RecoCls.FindEdge2(1);
                                    RecoCls.FindEdge_BackShadow(1);
                                    RecoCls.CutOffFindEdge2();
                                    RecoCls.FindObject();
                                    RecoCls.FindCircleObjSizeArc();
                                    //RecoCls.FindGetLineData2();
                                    //if (LineCheckFlag == true)
                                    {
                                        //picRecoImg.Image = RecoCls.GetLineArrDiplayImage();
                                        //picRecoImg.Image = RecoCls.GetLineArrDiplayImage(2);
                                        picRecoImg.Image = RecoCls.GetLineArrDiplayImage(DXFLineCountNum);
                                        
                                    }
                                    RecoCls.ClearDXFDataList();
                                    for (int i = 0; i < DXFOBJLIST_MAX; i++)
                                    {
                                        if (DXFObjList[i].ExistLyFlag == true)
                                        {
                                            RecoCls.AddDXFDataList(DXFObjList[i]);
                                        }
                                    }
                                    RecoCls.FindObjectLineCenterAngle();
                                }
                                else
                                {
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
                                    //RecoCls.FindEdge2(1);
                                    RecoCls.FindEdge_BackShadow(1);
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
                                    RecoCls.FindObjectCenterAngle5();
                                }


                                IndexFindRoAngle = RecoCls.GetFullAngle;
                                IndexFindRoAngle = 0 - IndexFindRoAngle;
                                picRecoImg.Image = RecoCls.GetArrObjectImage();
                                if (RecoCls.FindAngleWorkOKFlag == false)
                                {

                                }

                                if (ExitRecoFormFlag == true)
                                {
                                    return;
                                }


                            }
                        }
                        else if (TopAlignProcFlag == true)
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
                                //RecoCls.FindEdge2(1);
                                RecoCls.FindEdge_BackShadow(1);
                                RecoCls.CutOffFindEdge2();
                                RecoCls.FindObject();
                                RecoCls.FindCircleObjSizeArc();
                                RecoCls.FindCircleDetailShot();

                                IndexFindRoAngle = RecoCls.FindTopCenterAngle(RecoSetData.Object_Pin_Height + RecoSetData.Object_Diameter, RecoSetData.Object_Pin_Diameter * 1.36);
                                picRecoImg.Image = RecoCls.GetArrObjectImage();

                                if (ExitRecoFormFlag == true)
                                {
                                    return;
                                }


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
                                if (cboDXFType.SelectedIndex == RECO_TYPE_HOLE)
                                {
                                    GetDXFObjList();
                                    for (int i = 0; i < DXFOBJLIST_MAX; i++)
                                    {
                                        if (DXFObjList[i].ExistLyFlag == true)
                                        {
                                            if (DXFObjList[i].LyType == SHAPE_LINE)
                                            {
                                                //LineCheckFlag = true;
                                                DXFObjList[i].ExistLyFlag = false;
                                            }
                                        }
                                    }
                                    RecoCls = new clsRecoProcess(ref RecoWorkImg, CamOpticalGap_X[CamOpticalGapIndex], CamOpticalGap_Y[CamOpticalGapIndex]);
                                    //RecoCls.FindEdge2(1);
                                    RecoCls.FindEdge_BackShadow(1);
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
                                    //RecoCls.FindObjectCenterAngle3();
                                    RecoCls.FindObjectCenterAngle5();
                                }
                                else if (cboDXFType.SelectedIndex == RECO_TYPE_HOLE_ONELINE)
                                {
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
                                    //RecoCls.FindEdge2(1);
                                    RecoCls.FindEdge_BackShadow(1);
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
                                    //RecoCls.FindObjectCenterAngle3();
                                    RecoCls.FindObjectCenterAngle5();
                                }
                                else if (cboDXFType.SelectedIndex == RECO_TYPE_LINE)
                                {
                                    GetDXFObjList();
                                    for (int i = 0; i < DXFOBJLIST_MAX; i++)
                                    {
                                        if (DXFObjList[i].ExistLyFlag == true)
                                        {
                                            if (DXFObjList[i].LyType != SHAPE_LINE)
                                            {
                                                //LineCheckFlag = true;
                                                DXFObjList[i].ExistLyFlag = false;
                                            }
                                            else
                                            {
                                                if (DXFObjList[i].SelectLyFlag == false)
                                                {
                                                    DXFObjList[i].ExistLyFlag = false;
                                                }
                                            }
                                        }
                                    }
                                    RecoCls = new clsRecoProcess(ref RecoWorkImg, CamOpticalGap_X[CamOpticalGapIndex], CamOpticalGap_Y[CamOpticalGapIndex]);
                                    //RecoCls.FindEdge2(1);
                                    RecoCls.FindEdge_BackShadow(1);
                                    RecoCls.CutOffFindEdge2();
                                    RecoCls.FindObject();
                                    RecoCls.FindCircleObjSizeArc();
                                    //RecoCls.FindGetLineData2();
                                    //if (LineCheckFlag == true)
                                    {
                                        //picRecoImg.Image = RecoCls.GetLineArrDiplayImage();
                                        picRecoImg.Image = RecoCls.GetLineArrDiplayImage(2);
                                    }
                                    RecoCls.ClearDXFDataList();
                                    for (int i = 0; i < DXFOBJLIST_MAX; i++)
                                    {
                                        if (DXFObjList[i].ExistLyFlag == true)
                                        {
                                            RecoCls.AddDXFDataList(DXFObjList[i]);
                                        }
                                    }
                                    //RecoCls.FindObjectCenterAngle3();
                                    //RecoCls.FindObjectCenterAngle5();
                                    RecoCls.FindObjectLineCenterAngle();
                                }
                                else
                                {
                                    GetDXFObjList();
                                    for (int i = 0; i < DXFOBJLIST_MAX; i++)
                                    {
                                        if (DXFObjList[i].ExistLyFlag == true)
                                        {
                                            if (DXFObjList[i].LyType == SHAPE_LINE)
                                            {
                                                //LineCheckFlag = true;
                                                DXFObjList[i].ExistLyFlag = false;
                                            }
                                        }
                                    }
                                    RecoCls = new clsRecoProcess(ref RecoWorkImg, CamOpticalGap_X[CamOpticalGapIndex], CamOpticalGap_Y[CamOpticalGapIndex]);
                                    //RecoCls.FindEdge2(1);
                                    RecoCls.FindEdge_BackShadow(1);
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
                                    //RecoCls.FindObjectCenterAngle3();
                                    RecoCls.FindObjectCenterAngle5();
                                }




                                IndexFindRoAngle = RecoCls.GetFullAngle;
                                picRecoImg.Image = RecoCls.GetArrObjectImage();
                                if(RecoCls.FindAngleWorkOKFlag==false)
                                {

                                }

                                if (ExitRecoFormFlag == true)
                                {
                                    return;
                                }


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

                    if (ExitRecoFormFlag == true)
                    {
                        return;
                    }

                    if (LpWk_ZeroCount == 0)
                    {
                        LpWk_ZeroCount = 1;
                        IndexFindRoAngle = IndexRotationHalf(IndexFindRoAngle);
                        MultiMotion.GetCurrentPos();
                        IndexRoAngle = MultiMotion.AxisValue[RotateIndex_Index];
                        if (RotateIndex_Index == MultiMotion.INDEX_MOVE_R)
                        {
                            if (MoveBackAlignProcFlag == true)
                            {
                                IndexRoAngle = IndexRoAngle - IndexFindRoAngle;
                            }
                            else
                            {
                                IndexRoAngle = IndexRoAngle + IndexFindRoAngle;
                            }
                        }
                        else
                        {
                            IndexRoAngle = IndexRoAngle - IndexFindRoAngle;
                        }
                        PanelPicture.Visible = true;
                        picRecoImg.Visible = false;

                        SetAddDisplayAngle(DIS_TYPE_FULLSHOT, IndexFindRoAngle);

                        //INDEX회전부분
                        //=============================================
                        MultiMotion.MoveAxis(RotateIndex_Index, IndexRoAngle, true);

                        //MultiMotion.MoveAxis(0, 10.0, true);  // 완료 후 리턴

                        //MultiMotion.MoveAxis(0, -10.0, true); // 완료 후 리턴

                        //MultiMotion.RotateAxis(0, 10.0);
                        //=============================================
                        /*
                        MultiMotion.GetCurrentPos();
                        while (Math.Abs(MultiMotion.AxisValue[RotateIndex_Index] - IndexRoAngle) > 2.0)
                        {
                            MultiMotion.GetCurrentPos();
                            DelayWaitRun(1);
                            if (ExitRecoFormFlag == true)
                            {
                                return;
                            }
                        }
                        */
                        //DelayWaitRun(1);
                        if (ExitRecoFormFlag == true)
                        {
                            return;
                        }

                    }
                    if (LpWk_ZeroCount > ZeroLoopCountChkMax)
                    {
                        UpdatePos();
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

                    if (ExitRecoFormFlag == true)
                    {
                        return;
                    }

                    if (LpWk_ZeroCount == 0)
                    {
                        RecoWorkImg = RecoBlankImg;
                        RealScaleBox.Image.Save("001_2.jpg");
                        if (BackAlignProcFlag == true)
                        {
                            if (RecoWorkImg != null)
                            {
                                bool LineCheckFlag;
                                LineCheckFlag = false;

                                if (cboDXFType.SelectedIndex == RECO_TYPE_HOLE)
                                {
                                    GetDXFObjList();
                                    for (int i = 0; i < DXFOBJLIST_MAX; i++)
                                    {
                                        if (DXFObjList[i].ExistLyFlag == true)
                                        {
                                            if (DXFObjList[i].LyType == SHAPE_LINE)
                                            {
                                                LineCheckFlag = true;
                                                //DXFObjList[i].ExistLyFlag = false;
                                            }
                                        }
                                    }
                                    RecoCls = new clsRecoProcess(ref RecoWorkImg, CamOpticalGap_X[CamOpticalGapIndex], CamOpticalGap_Y[CamOpticalGapIndex]);
                                    //RecoCls.FindEdge2(1);
                                    RecoCls.FindEdge_BackShadow(1);
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
                                    RecoCls.FindObjectCenterAngle5();
                                }
                                else if (cboDXFType.SelectedIndex == RECO_TYPE_HOLE_ONELINE)
                                {
                                    GetDXFObjList();
                                    for (int i = 0; i < DXFOBJLIST_MAX; i++)
                                    {
                                        if (DXFObjList[i].ExistLyFlag == true)
                                        {
                                            if (DXFObjList[i].LyType == SHAPE_LINE)
                                            {
                                                LineCheckFlag = true;
                                                //DXFObjList[i].ExistLyFlag = false;
                                            }
                                        }
                                    }
                                    RecoCls = new clsRecoProcess(ref RecoWorkImg, CamOpticalGap_X[CamOpticalGapIndex], CamOpticalGap_Y[CamOpticalGapIndex]);
                                    //RecoCls.FindEdge2(1);
                                    RecoCls.FindEdge_BackShadow(1);
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
                                    RecoCls.FindObjectCenterAngle5();
                                }
                                else if (cboDXFType.SelectedIndex == RECO_TYPE_LINE)
                                {
                                    GetDXFObjList();
                                    for (int i = 0; i < DXFOBJLIST_MAX; i++)
                                    {
                                        if (DXFObjList[i].ExistLyFlag == true)
                                        {
                                            if (DXFObjList[i].LyType != SHAPE_LINE)
                                            {
                                                //LineCheckFlag = true;
                                                //DXFObjList[i].ExistLyFlag = false;
                                                DXFObjList[i].ExistLyFlag = false;
                                            }
                                        }
                                    }
                                    RecoCls = new clsRecoProcess(ref RecoWorkImg, CamOpticalGap_X[CamOpticalGapIndex], CamOpticalGap_Y[CamOpticalGapIndex]);
                                    //RecoCls.FindEdge2(1);
                                    RecoCls.FindEdge_BackShadow(1);
                                    RecoCls.CutOffFindEdge2();
                                    RecoCls.FindObject();
                                    RecoCls.FindCircleObjSizeArc();
                                    //RecoCls.FindGetLineData2();
                                    //if (LineCheckFlag == true)
                                    {
                                        //picRecoImg.Image = RecoCls.GetLineArrDiplayImage();
                                        picRecoImg.Image = RecoCls.GetLineArrDiplayImage(2);
                                    }
                                    RecoCls.ClearDXFDataList();
                                    for (int i = 0; i < DXFOBJLIST_MAX; i++)
                                    {
                                        if (DXFObjList[i].ExistLyFlag == true)
                                        {
                                            RecoCls.AddDXFDataList(DXFObjList[i]);
                                        }
                                    }
                                    //RecoCls.FindObjectCenterAngle5();
                                    RecoCls.FindObjectLineCenterAngle();
                                }
                                else
                                {
                                    GetDXFObjList();
                                    for (int i = 0; i < DXFOBJLIST_MAX; i++)
                                    {
                                        if (DXFObjList[i].ExistLyFlag == true)
                                        {
                                            if (DXFObjList[i].LyType == SHAPE_LINE)
                                            {
                                                LineCheckFlag = true;
                                                //DXFObjList[i].ExistLyFlag = false;
                                            }
                                        }
                                    }
                                    RecoCls = new clsRecoProcess(ref RecoWorkImg, CamOpticalGap_X[CamOpticalGapIndex], CamOpticalGap_Y[CamOpticalGapIndex]);
                                    //RecoCls.FindEdge2(1);
                                    RecoCls.FindEdge_BackShadow(1);
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
                                    RecoCls.FindObjectCenterAngle5();
                                }
                                



                                IndexFindRoAngle = RecoCls.GetFullAngle;
                                IndexFindRoAngle = 0 - IndexFindRoAngle;
                                picRecoImg.Image = RecoCls.GetArrObjectImage();

                                if (ExitRecoFormFlag == true)
                                {
                                    return;
                                }

                            }
                        }
                        else if (TopAlignProcFlag == true)
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
                                //RecoCls.FindEdge2(1);
                                RecoCls.FindEdge_BackShadow(1);
                                RecoCls.CutOffFindEdge2();
                                RecoCls.FindObject();
                                RecoCls.FindCircleObjSizeArc();
                                RecoCls.FindCircleDetailShot();

                                //IndexFindRoAngle = RecoCls.FindTopCenterAngle(18 + 6.35, 8);
                                IndexFindRoAngle = RecoCls.FindTopCenterAngle(RecoSetData.Object_Pin_Height + RecoSetData.Object_Diameter, RecoSetData.Object_Pin_Diameter * 1.36);
                                picRecoImg.Image = RecoCls.GetArrObjectImage();

                                if (ExitRecoFormFlag == true)
                                {
                                    return;
                                }

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
                                if (cboDXFType.SelectedIndex == RECO_TYPE_HOLE)
                                {
                                    GetDXFObjList();
                                    for (int i = 0; i < DXFOBJLIST_MAX; i++)
                                    {
                                        if (DXFObjList[i].ExistLyFlag == true)
                                        {
                                            if (DXFObjList[i].LyType == SHAPE_LINE)
                                            {
                                                //LineCheckFlag = true;
                                                DXFObjList[i].ExistLyFlag = false;
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
                                }
                                else if (cboDXFType.SelectedIndex == RECO_TYPE_HOLE_ONELINE)
                                {
                                    GetDXFObjList();
                                    for (int i = 0; i < DXFOBJLIST_MAX; i++)
                                    {
                                        if (DXFObjList[i].ExistLyFlag == true)
                                        {
                                            if (DXFObjList[i].LyType == SHAPE_LINE)
                                            {
                                                LineCheckFlag = true;
                                                //DXFObjList[i].ExistLyFlag = false;
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
                                }
                                else if (cboDXFType.SelectedIndex == RECO_TYPE_LINE)
                                {
                                    GetDXFObjList();
                                    for (int i = 0; i < DXFOBJLIST_MAX; i++)
                                    {
                                        if (DXFObjList[i].ExistLyFlag == true)
                                        {
                                            if (DXFObjList[i].LyType != SHAPE_LINE)
                                            {
                                                //LineCheckFlag = true;
                                                DXFObjList[i].ExistLyFlag = false;
                                            }
                                            else
                                            {
                                                if (DXFObjList[i].SelectLyFlag == false)
                                                {
                                                    DXFObjList[i].ExistLyFlag = false;
                                                }
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
                                        picRecoImg.Image = RecoCls.GetLineArrDiplayImage(2);
                                    }
                                    RecoCls.ClearDXFDataList();
                                    for (int i = 0; i < DXFOBJLIST_MAX; i++)
                                    {
                                        if (DXFObjList[i].ExistLyFlag == true)
                                        {
                                            RecoCls.AddDXFDataList(DXFObjList[i]);
                                        }
                                    }
                                    RecoCls.FindObjectLineCenterAngle();
                                }
                                else
                                {
                                    GetDXFObjList();
                                    for (int i = 0; i < DXFOBJLIST_MAX; i++)
                                    {
                                        if (DXFObjList[i].ExistLyFlag == true)
                                        {
                                            if (DXFObjList[i].LyType == SHAPE_LINE)
                                            {
                                                //LineCheckFlag = true;
                                                DXFObjList[i].ExistLyFlag = false;
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
                                }

                                
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

                                if (ExitRecoFormFlag == true)
                                {
                                    return;
                                }

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

                    if (ExitRecoFormFlag == true)
                    {
                        return;
                    }

                    if (LpWk_ZeroCount == 0)
                    {
                        LpWk_ZeroCount = 1;

                        IndexFindRoAngle = IndexRotationHalf(IndexFindRoAngle);

                        MultiMotion.GetCurrentPos();
                        IndexRoAngle = MultiMotion.AxisValue[RotateIndex_Index];
                        if (RotateIndex_Index == MultiMotion.INDEX_MOVE_R)
                        {
                            if (MoveBackAlignProcFlag == true)
                            {
                                IndexRoAngle = IndexRoAngle - IndexFindRoAngle;
                            }
                            else
                            {
                                IndexRoAngle = IndexRoAngle + IndexFindRoAngle;
                            }
                        }
                        else
                        {
                            IndexRoAngle = IndexRoAngle - IndexFindRoAngle;
                        }

                        SetAddDisplayAngle(DIS_TYPE_FULLSHOT, IndexFindRoAngle);
                        //INDEX회전부분
                        //=============================================
                        MultiMotion.MoveAxis(RotateIndex_Index, IndexRoAngle, true);

                        //MultiMotion.MoveAxis(0, 10.0, true);  // 완료 후 리턴

                        //MultiMotion.MoveAxis(0, -10.0, true); // 완료 후 리턴

                        //MultiMotion.RotateAxis(0, 10.0);
                        //=============================================
                        PanelPicture.Visible = true;
                        picRecoImg.Visible = false;
                        /*
                        while (Math.Abs(MultiMotion.AxisValue[RotateIndex_Index] - IndexRoAngle) > 1.0)
                        {
                            MultiMotion.GetCurrentPos();
                            DelayWaitRun(1);
                        }
                        */
                        //DelayWaitRun(1);
                        if (ExitRecoFormFlag == true)
                        {
                            return;
                        }

                    }
                    if (LpWk_ZeroCount > ZeroLoopCountChkMax)
                    {
                        UpdatePos();
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
                            if (RecoSetData.Object_TopHoleType == 0)
                            {
                                UpdatePos();
                                label5.Text = "테스트끝";
                                LpWk_RunFlag = false;
                                timWork.Enabled = false;
                                btLoopStart.Enabled = true;
                                btLoopStop.Enabled = false;
                                MessageBox.Show("테스트종료!");
                                timerAxis.Enabled = true;
                                return;
                            }
                        }


                        if (BackAlignProcFlag == true)
                        {
                            if (cboDXFType.SelectedIndex == RECO_TYPE_LINE)
                            {
                                UpdatePos();
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
                            //Detail_Move_Pos_Y = 0 - GetDetailSelObj.Y;
                            Detail_Move_Pos_Z = GetDetailSelObj.Z;

                            MultiMotion.GetCurrentPos();
                            TmpMoveAxis_Index = MultiMotion.BACK_CAM_Z;
                            TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
                            SetMoveValueAxis = TmpMoveValueAxis - Detail_Move_Pos_Z;
                            /*
                            MultiMotion.MoveAxis(TmpMoveAxis_Index, SetMoveValueAxis, false);
                            while (Math.Abs(MultiMotion.AxisValue[TmpMoveAxis_Index] - TmpMoveValueAxis) > 1.0)
                            {
                                DelayWaitRun(5);
                            }
                            */
                            RunMotionAxis_Back_Z(SetMoveValueAxis);

                            if (ExitRecoFormFlag == true)
                            {
                                UpdatePos();
                                return;
                            }

                        }
                        else if (TopAlignProcFlag == true)
                        {
                            Detail_Move_Pos_X = RecoSetData.Object_Hole_Distance;

                            MultiMotion.GetCurrentPos();
                            TmpMoveAxis_Index = MultiMotion.CAM_UNIT_X;
                            TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
                            SetMoveValueAxis = TmpMoveValueAxis - Detail_Move_Pos_X;

                            //SetMoveValueAxis = TmpMoveValueAxis - Detail_Move_Pos_Y;
                            MultiMotion.MoveAxis(TmpMoveAxis_Index, SetMoveValueAxis, true);
                            MultiMotion.GetCurrentPos();

                            if (ExitRecoFormFlag == true)
                            {
                                return;
                            }

                        }
                        else
                        {
                            if (cboDXFType.SelectedIndex == RECO_TYPE_LINE)
                            {
                                UpdatePos();
                                picRecoImg.Visible = false;
                                PanelPicture.Visible = true;
                                PanelPicture.Refresh();
                                LpWk_RUN_STATUS = INDEXROTATE_GAPADD;
                                LpWk_ZeroCount = 0;
                                timWork.Enabled = true;
                                return;
                            }

                            GetDetailSelObj = GetSelectDXFData();
                            Detail_Move_Pos_X = 0;
                            Detail_Move_Pos_Y = 0 - GetDetailSelObj.Y;
                            Detail_Move_Pos_Z = GetDetailSelObj.Z;

                            if(ReAlignIndex == RE_ALIGN_TRANSLATION)
                            {
                                DoubleDataPosInfo GetInputAnglePos, GetOutputAnglePos;

                                GetInputAnglePos.ddp_ExistFlag = true;
                                GetInputAnglePos.ddp_X = Detail_Move_Pos_Y;
                                GetInputAnglePos.ddp_Y = Detail_Move_Pos_Z;

                                if (RotateIndex_Index == MultiMotion.INDEX_MOVE_R)
                                {
                                    GetOutputAnglePos = GetDataMovePos2(GetInputAnglePos);
                                }
                                else
                                {
                                    GetOutputAnglePos = GetDataFixPos2(GetInputAnglePos);
                                }

                                //MessageBox.Show(GetOutputAnglePos.ToString());

                                Detail_Move_Pos_Y = GetOutputAnglePos.ddp_X;
                                Detail_Move_Pos_Z = GetOutputAnglePos.ddp_Y;
                            }

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

                            if (ExitRecoFormFlag == true)
                            {
                                return;
                            }


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

                            if (ExitRecoFormFlag == true)
                            {
                                return;
                            }

                            //MultiMotion.MoveAxis(0, 10.0, true);  // 완료 후 리턴
                            //MultiMotion.MoveAxis(0, -10.0, true); // 완료 후 리턴
                            //MultiMotion.RotateAxis(0, 10.0);
                            //=============================================
                        }
                    }
                    if (LpWk_ZeroCount > ZeroLoopCountChkMax)
                    {
                        UpdatePos();
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
                    if (ExitRecoFormFlag == true)
                    {
                        return;
                    }

                    if (LpWk_ZeroCount == 0)
                    {
                        //SelectDXFReScale(RealScaleBox.Image.Width, RealScaleBox.Image.Height);

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
                    if (ExitRecoFormFlag == true)
                    {
                        return;
                    }

                    if (LpWk_ZeroCount == 0)
                    {
                        RecoWorkImg = RecoBlankImg;
                        RealScaleBox.Image.Save("001_3.jpg");

                        if (TopAlignProcFlag == true)
                        {
                            if (RecoWorkImg != null)
                            {
                                RecoCls = new clsRecoProcess(ref RecoWorkImg, CamOpticalGap_X[CamOpticalGapIndex], CamOpticalGap_Y[CamOpticalGapIndex]);
                                int LeftRightType = RecoCls.FindCenterLR(0);

                                if (RecoSetData.Object_TopHoleType == 1)
                                {
                                    if(LeftRightType == -1)
                                    {
                                        MessageBox.Show("방향이 맞습니다!");
                                    }
                                    else
                                    {
                                        MessageBox.Show("방향이 틀립니다!");
                                    }
                                }
                                else if (RecoSetData.Object_TopHoleType == 2)
                                {
                                    if (LeftRightType == 1)
                                    {
                                        MessageBox.Show("방향이 맞습니다!");
                                    }
                                    else
                                    {
                                        MessageBox.Show("방향이 틀립니다!");
                                    }
                                }

                                UpdatePos();
                                label5.Text = "테스트끝";
                                LpWk_RunFlag = false;
                                timWork.Enabled = false;
                                btLoopStart.Enabled = true;
                                btLoopStop.Enabled = false;
                                MessageBox.Show("테스트종료!");
                                timerAxis.Enabled = true;
                                return;
                            }
                        }
                        else if (BackAlignProcFlag == true)
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
                                    IndexFindRoAngle = 0 - IndexFindRoAngle;
                                    picRecoImg.Image = RecoCls.GetArrDetailObjectImage(IndexFindRoAngle);
                                }
                                if (ExitRecoFormFlag == true)
                                {
                                    return;
                                }

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
                                if (ExitRecoFormFlag == true)
                                {
                                    return;
                                }

                            }
                        }



                        PanelPicture.Visible = false;
                        picRecoImg.Visible = true;


                        LpWk_ZeroCount = 1;
                    }
                    if (LpWk_ZeroCount > ZeroLoopCountChkMax)
                    {
                        UpdatePos();
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
                    if (ExitRecoFormFlag == true)
                    {
                        return;
                    }

                    if (LpWk_ZeroCount == 0)
                    {
                        LpWk_ZeroCount = 1;

                        MultiMotion.GetCurrentPos();
                        IndexRoAngle = MultiMotion.AxisValue[RotateIndex_Index];
                        if (RotateIndex_Index == MultiMotion.INDEX_MOVE_R)
                        {
                            if (MoveBackAlignProcFlag == true)
                            {
                                IndexRoAngle = IndexRoAngle - IndexFindRoAngle;
                            }
                            else
                            {
                                IndexRoAngle = IndexRoAngle + IndexFindRoAngle;
                            }
                        }
                        else
                        {
                            IndexRoAngle = IndexRoAngle - IndexFindRoAngle;
                        }
                        
                        PanelPicture.Visible = true;
                        picRecoImg.Visible = false;

                        SetAddDisplayAngle(DIS_TYPE_DETAILSHOT, IndexFindRoAngle);

                        //INDEX회전부분
                        //=============================================
                        MultiMotion.MoveAxis(RotateIndex_Index, IndexRoAngle, true);

                        //MultiMotion.MoveAxis(0, 10.0, true);  // 완료 후 리턴

                        //MultiMotion.MoveAxis(0, -10.0, true); // 완료 후 리턴

                        //MultiMotion.RotateAxis(0, 10.0);
                        //=============================================
                        /*
                        while (Math.Abs(MultiMotion.AxisValue[RotateIndex_Index] - IndexRoAngle) > 1.0)
                        {
                            MultiMotion.GetCurrentPos();
                            DelayWaitRun(1);
                        }
                        */
                        //DelayWaitRun(5);
                        if (ExitRecoFormFlag == true)
                        {
                            return;
                        }

                        
                    }
                    if (LpWk_ZeroCount > ZeroLoopCountChkMax)
                    {
                        UpdatePos();
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
                        RecoWorkImg = RecoBlankImg;
                        //RealScaleBox.Image.Save("001_4.jpg");

                        if (BackAlignProcFlag == true)
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
                                    IndexFindRoAngle = 0 - IndexFindRoAngle;
                                    picRecoImg.Image = RecoCls.GetArrDetailObjectImage(IndexFindRoAngle);
                                }
                            }
                            if (ExitRecoFormFlag == true)
                            {
                                return;
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
                            if (ExitRecoFormFlag == true)
                            {
                                return;
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
                    if (ExitRecoFormFlag == true)
                    {
                        return;
                    }

                    if (LpWk_ZeroCount == 0)
                    {
                        LpWk_ZeroCount = 1;

                        MultiMotion.GetCurrentPos();
                        IndexRoAngle = MultiMotion.AxisValue[RotateIndex_Index];
                        if (RotateIndex_Index == MultiMotion.INDEX_MOVE_R)
                        {
                            if (MoveBackAlignProcFlag == true)
                            {
                                IndexRoAngle = IndexRoAngle - IndexFindRoAngle;
                            }
                            else
                            {
                                IndexRoAngle = IndexRoAngle + IndexFindRoAngle;
                            }
                        }
                        else
                        {
                            IndexRoAngle = IndexRoAngle - IndexFindRoAngle;
                        }

                        PanelPicture.Visible = true;
                        picRecoImg.Visible = false;

                        SetAddDisplayAngle(DIS_TYPE_DETAILSHOT, IndexFindRoAngle);

                        //INDEX회전부분
                        //=============================================
                        MultiMotion.MoveAxis(RotateIndex_Index, IndexRoAngle, true);
                        if (ExitRecoFormFlag == true)
                        {
                            return;
                        }


                        //MultiMotion.MoveAxis(0, 10.0, true);  // 완료 후 리턴

                        //MultiMotion.MoveAxis(0, -10.0, true); // 완료 후 리턴

                        //MultiMotion.RotateAxis(0, 10.0);
                        //=============================================
                        /*
                        while (Math.Abs(MultiMotion.AxisValue[RotateIndex_Index] - IndexRoAngle) > 1.0)
                        {
                            MultiMotion.GetCurrentPos();
                            DelayWaitRun(1);
                        }
                        */
                        //DelayWaitRun(5);
                        
                    }
                    if (LpWk_ZeroCount > ZeroLoopCountChkMax)
                    {
                        UpdatePos();
                        picRecoImg.Visible = false;
                        PanelPicture.Visible = true;
                        PanelPicture.Refresh();
                        LpWk_RUN_STATUS = INDEXROTATE_GAPADD;
                        LpWk_ZeroCount = 0;
                    }
                    else
                    {

                        LpWk_ZeroCount++;
                    }
                }
                else if (LpWk_RUN_STATUS == INDEXROTATE_GAPADD)
                {
                    label5.Text = "INDEX보정회전";
                    if (ExitRecoFormFlag == true)
                    {
                        return;
                    }

                    if (LpWk_ZeroCount == 0)
                    {
                        LpWk_ZeroCount = 1;
                        double DisGapAddAngle = 0.0;
                        MultiMotion.GetCurrentPos();
                        IndexRoAngle = MultiMotion.AxisValue[RotateIndex_Index];
                        if (RecoSetData.RecoCamIndex == 0)
                        {
                            DisGapAddAngle = RecoSetData.Move_R_FIX_Angle_Gap;
                            //IndexRoAngle = IndexRoAngle + IndexGapAddRoAngle;
                            IndexRoAngle = IndexRoAngle + RecoSetData.Move_R_FIX_Angle_Gap;
                            if(ReAlignIndex == RE_ALIGN_ANGLE_DIVIDE)
                            {
                                GetDetailSelObj = GetSelectDXFData();
                                Detail_Move_Pos_X = 0;
                                Detail_Move_Pos_Y = GetDetailSelObj.Y;
                                Detail_Move_Pos_Z = GetDetailSelObj.Z;
                                IndexRoAngle = IndexRoAngle + Axis4_Fix_GetGap(Detail_Move_Pos_Y, Detail_Move_Pos_Z);
                                DisGapAddAngle += Axis4_Fix_GetGap(Detail_Move_Pos_Y, Detail_Move_Pos_Z);
                            }
                        }
                        else if (RecoSetData.RecoCamIndex == 1)
                        {
                            DisGapAddAngle = RecoSetData.Move_R_MOVE_Angle_Gap;
                            //IndexRoAngle = IndexRoAngle + IndexGapAddRoAngle;
                            IndexRoAngle = IndexRoAngle + RecoSetData.Move_R_MOVE_Angle_Gap;
                            if (ReAlignIndex == RE_ALIGN_ANGLE_DIVIDE)
                            {
                                GetDetailSelObj = GetSelectDXFData();
                                Detail_Move_Pos_X = 0;
                                Detail_Move_Pos_Y = GetDetailSelObj.Y;
                                Detail_Move_Pos_Z = GetDetailSelObj.Z;
                                IndexRoAngle = IndexRoAngle + Axis4_Move_GetGap(Detail_Move_Pos_Y, Detail_Move_Pos_Z);
                                DisGapAddAngle += Axis4_Move_GetGap(Detail_Move_Pos_Y, Detail_Move_Pos_Z);
                            }
                        }
                        else if (RecoSetData.RecoCamIndex == 2)
                        {
                            //IndexRoAngle = IndexRoAngle + IndexGapAddRoAngle;
                            IndexRoAngle = IndexRoAngle + RecoSetData.Back_R_MOVE_Angle_Gap;
                            DisGapAddAngle = RecoSetData.Back_R_MOVE_Angle_Gap;
                        }
                        else
                        {
                            //IndexRoAngle = IndexRoAngle - IndexGapAddRoAngle;
                        }

                        PanelPicture.Visible = true;
                        picRecoImg.Visible = false;

                        SetAddDisplayAngle(DIS_TYPE_ADDANGLE, DisGapAddAngle);

                        //INDEX회전부분
                        //=============================================
                        MultiMotion.MoveAxis(RotateIndex_Index, IndexRoAngle, true);

                        if (ExitRecoFormFlag == true)
                        {
                            return;
                        }

                        //MultiMotion.MoveAxis(0, 10.0, true);  // 완료 후 리턴

                        //MultiMotion.MoveAxis(0, -10.0, true); // 완료 후 리턴

                        //MultiMotion.RotateAxis(0, 10.0);
                        //=============================================
                        /*
                        while (Math.Abs(MultiMotion.AxisValue[RotateIndex_Index] - IndexRoAngle) > 1.0)
                        {
                            MultiMotion.GetCurrentPos();
                            DelayWaitRun(1);
                            if (ExitRecoFormFlag == true)
                            {
                                return;
                            }

                        }
                        DelayWaitRun(5);
                        */
                        if (ExitRecoFormFlag == true)
                        {
                            return;
                        }

                        
                    }
                    if (LpWk_ZeroCount > ZeroLoopCountChkMax)
                    {
                        UpdatePos();
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
                    if (ExitRecoFormFlag == true)
                    {
                        return;
                    }

                    if (LpWk_ZeroCount == ZeroLoopCountChkMax_Shot)
                    {
                        //RealScaleBox.Image.Save("shot" + LpWk_LoopCount.ToString() + ".jpg");
                        LpWk_LoopCount++;
                    }
                    if (LpWk_ZeroCount > ZeroLoopCountChkMax)
                    {
                        //LpWk_RUN_STATUS = WORKSTART;
                        //LpWk_ZeroCount = 0;
                    }
                    else
                    {
                        LpWk_ZeroCount++;
                    }
                }
                //if (LpWk_LoopCount >= LpWk_LoopCountMax)
                if (LpWk_RUN_STATUS == WORKEND)
                {
                    if (ExitRecoFormFlag == true)
                    {
                        return;
                    }

                    label5.Text = "";
                    LpWk_RunFlag = false;
                    timWork.Enabled = false;
                    btLoopStart.Enabled = true;
                    btLoopStop.Enabled = false;
                    MessageBox.Show("테스트종료!");
                }
                else
                {
                    if (ExitRecoFormFlag == true)
                    {
                        return;
                    }

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
            //Base.X = RealScaleBox.Image.Width / 2;
            //Base.Y = RealScaleBox.Image.Height / 2;
            BaseDefault.X = RealScaleBox.Image.Width / 2;
            BaseDefault.Y = RealScaleBox.Image.Height / 2;
            if (DXFDrawOptAdjFlag == true)
            {
                if (CamOpticalGapIndex > -1 && CamOpticalGapIndex < 4)
                {
                    Base.X = CamOpticalGap_X[CamOpticalGapIndex];
                    Base.Y = CamOpticalGap_Y[CamOpticalGapIndex];
                }
                else
                {
                    Base.X = BaseDefault.X;
                    Base.Y = BaseDefault.Y;
                }
            }
            else
            {
                Base.X = BaseDefault.X;
                Base.Y = BaseDefault.Y;
            }

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
            //SelectDXFReScale(RealScaleBox.Image.Width, RealScaleBox.Image.Height);

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
            Base.Y = Base.Y - 1;
        }

        private void DXFMoveDown()
        {
            Base.Y = Base.Y + 1;
        }

        private void DXFMoveLeft()
        {
            Base.X = Base.X - 1;
        }

        private void DXFMoveRight()
        {
            Base.X = Base.X + 1;
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
            SelectDXFObj(e.X, e.Y, RealScaleBox.Width, RealScaleBox.Height, RealScaleBox.Image.Width, RealScaleBox.Image.Height);
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
                        Lbl_SelectL_Hole_Flag = true;
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
            bool SelObjRedFlag;

            ResPointData.X = 0;
            ResPointData.Y = 0;
            ResPointData.Z = 0;

            if (FCADImage == null)
                return ResPointData;

            if (FCADImage.FEntities == null)
                return ResPointData;

            if (FCADImage.FEntities.Entities == null)
                return ResPointData;

            SelObjRedFlag = false;

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
                        SelObjRedFlag = true;
                    }
                }
            }

            if (SelObjRedFlag == false)
            {
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
                            SFPoint p1 = dxCircle.Point1;
                            ResPointData.Y = p1.X - FS_W_Base - FS_W / 2;
                            ResPointData.Z = p1.Y - FS_H_Base - FS_H / 2;
                            SelObjRedFlag = true;
                        }
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

                RecoSetData.Move_R_FIX_Angle_Gap = 0.0;
                RecoSetData.Move_R_MOVE_Angle_Gap = 0.0;
                RecoSetData.Move_R_Move_X_Gap = 0.0;
                RecoSetData.Move_R_Move_Y_Gap = 0.0;
                RecoSetData.Move_R_Move_Z_Gap = 0.0;
                RecoSetData.Object_Weight = 0.0;
                RecoSetData.Object_Diameter = 0.0;
                RecoSetData.Object_Pin_Height = 45.0;
                RecoSetData.Object_Pin_Diameter = 16.0;
                RecoSetData.Back_R_MOVE_Angle_Gap = 0.0;
                RecoSetData.Top_R_MOVE_Angle_Gap = 0.0;
                RecoSetData.Object_Hole_Distance = 0.0;

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
                    if (m_WordStr.Length > 19)
                    {
                        RecoSetData.Move_R_FIX_Angle_Gap = Convert.ToDouble(m_WordStr[19]);
                        RecoSetData.Move_R_MOVE_Angle_Gap = Convert.ToDouble(m_WordStr[20]);
                        RecoSetData.Move_R_Move_X_Gap = Convert.ToDouble(m_WordStr[21]);
                        RecoSetData.Move_R_Move_Y_Gap = Convert.ToDouble(m_WordStr[22]);
                        RecoSetData.Move_R_Move_Z_Gap = Convert.ToDouble(m_WordStr[23]);
                        RecoSetData.Object_Weight = Convert.ToDouble(m_WordStr[24]);
                        RecoSetData.Object_Diameter = Convert.ToDouble(m_WordStr[25]);
                        RecoSetData.Object_Pin_Height = Convert.ToDouble(m_WordStr[26]);
                        RecoSetData.Object_Pin_Diameter = Convert.ToDouble(m_WordStr[27]);
                        RecoSetData.Back_R_MOVE_Angle_Gap = Convert.ToDouble(m_WordStr[28]);
                        RecoSetData.Top_R_MOVE_Angle_Gap = Convert.ToDouble(m_WordStr[29]);
                        RecoSetData.Object_Hole_Distance = Convert.ToDouble(m_WordStr[30]);
                        if (m_WordStr.Length > 31)
                        {
                            RecoSetData.Object_TopHoleType = Convert.ToInt32(m_WordStr[31]);
                            RecoSetData.DXF_RecoType = Convert.ToInt32(m_WordStr[32]);
                            RecoSetData.Object_Param3 = Convert.ToInt32(m_WordStr[33]);
                            RecoSetData.Object_Param4 = Convert.ToInt32(m_WordStr[34]);
                            RecoSetData.Object_Param5 = Convert.ToInt32(m_WordStr[35]);

                            RecoSetData.Object_TopDirHoleDistance = Convert.ToDouble(m_WordStr[36]);
                            RecoSetData.Motion_Move_MR = Convert.ToDouble(m_WordStr[37]);
                            RecoSetData.Object_ShaftLength = Convert.ToDouble(m_WordStr[38]);
                            RecoSetData.Object_Param9 = Convert.ToDouble(m_WordStr[39]);
                            RecoSetData.Object_Param10 = Convert.ToDouble(m_WordStr[40]);
                            RecoSetData.Object_Param11 = Convert.ToDouble(m_WordStr[41]);
                            RecoSetData.Object_Param12 = Convert.ToDouble(m_WordStr[42]);
                        }
                    }
                    
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
            WritePresetDataStr = WritePresetDataStr + "|" + RecoSetData.Move_R_FIX_Angle_Gap.ToString() + "|" + RecoSetData.Move_R_MOVE_Angle_Gap.ToString();
            WritePresetDataStr = WritePresetDataStr + "|" + RecoSetData.Move_R_Move_X_Gap.ToString() + "|" + RecoSetData.Move_R_Move_Y_Gap.ToString() + "|" + RecoSetData.Move_R_Move_Z_Gap.ToString();
            WritePresetDataStr = WritePresetDataStr + "|" + RecoSetData.Object_Weight.ToString() + "|" + RecoSetData.Object_Diameter.ToString() + "|" + RecoSetData.Object_Pin_Height.ToString() + "|" + RecoSetData.Object_Pin_Diameter.ToString();
            WritePresetDataStr = WritePresetDataStr + "|" + RecoSetData.Back_R_MOVE_Angle_Gap.ToString() + "|" + RecoSetData.Top_R_MOVE_Angle_Gap.ToString() + "|" + RecoSetData.Object_Hole_Distance.ToString();

            WritePresetDataStr = WritePresetDataStr + "|" + RecoSetData.Object_TopHoleType.ToString() + "|" + RecoSetData.DXF_RecoType.ToString() + "|" + RecoSetData.Object_Param3.ToString() + "|" + RecoSetData.Object_Param4.ToString() + "|" + RecoSetData.Object_Param5.ToString();
            WritePresetDataStr = WritePresetDataStr + "|" + RecoSetData.Object_TopDirHoleDistance.ToString() + "|" + RecoSetData.Motion_Move_MR.ToString() + "|" + RecoSetData.Object_ShaftLength.ToString() + "|" + RecoSetData.Object_Param9.ToString() + "|" + RecoSetData.Object_Param10.ToString() + "|" + RecoSetData.Object_Param11.ToString() + "|" + RecoSetData.Object_Param12.ToString();
            sw.Write(WritePresetDataStr);

            sw.Close();

        }

        private void SetCalibCenter(int CurSelCamIndex)
        {
            bool TopAlignProcFlag = false;
            bool BackAlignProcFlag = false;
            bool MoveBackAlignProcFlag = false;

            if (cboZoomCamList.SelectedIndex == 0 && cboIndexList.SelectedIndex == 1)
            {
                MoveBackAlignProcFlag = true;
            }

            if (cboZoomCamList.SelectedIndex == 3)
            {
                TopAlignProcFlag = true;
            }

            if (cboZoomCamList.SelectedIndex == 2)
            {
                BackAlignProcFlag = true;
            }

            if (MoveBackAlignProcFlag == true)
            {
                RecoSetData.Motion_Move_Y = RealCalibrationData_E.ddp_X;
                RecoSetData.Motion_Move_Z = RealCalibrationData_E.ddp_Y;
            }
            else if (CurSelCamIndex == 0)
            {
                RecoSetData.Motion_Move_Y = RealCalibrationData_A.ddp_X;
                RecoSetData.Motion_Move_Z = RealCalibrationData_A.ddp_Y;
            }
            if (CurSelCamIndex == 1)
            {
                RecoSetData.Motion_Move_Y = RealCalibrationData_B.ddp_X;
                RecoSetData.Motion_Move_Z = RealCalibrationData_B.ddp_Y;
            }
            if (CurSelCamIndex == 2)
            {
                RecoSetData.Motion_Move_Z = RealCalibrationData_C.ddp_X;
                //RecoSetData.Motion_Move_Z = RealCalibrationData_C.ddp_Y;
            }
            if (CurSelCamIndex == 3)
            {
                RecoSetData.Motion_Move_X = RealCalibrationData_D.ddp_X;
                RecoSetData.Motion_Move_Y = RealCalibrationData_D.ddp_Y;
            }
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
                    RealCalibrationData_C.ddp_X = Convert.ToDouble(m_WordStr[4]);
                    RealCalibrationData_C.ddp_Y = Convert.ToDouble(m_WordStr[5]);
                    RealCalibrationData_D.ddp_X = Convert.ToDouble(m_WordStr[6]);
                    RealCalibrationData_D.ddp_Y = Convert.ToDouble(m_WordStr[7]);
                    if (m_WordStr.Length > 8)
                    {
                        RealCalibrationData_E.ddp_X = Convert.ToDouble(m_WordStr[8]);
                        RealCalibrationData_E.ddp_Y = Convert.ToDouble(m_WordStr[9]);
                        RealCalibrationData_F.ddp_X = Convert.ToDouble(m_WordStr[10]);
                        RealCalibrationData_F.ddp_Y = Convert.ToDouble(m_WordStr[11]);
                        RealCalibrationData_G.ddp_X = Convert.ToDouble(m_WordStr[12]);
                        RealCalibrationData_G.ddp_Y = Convert.ToDouble(m_WordStr[13]);
                        RealCalibrationData_H.ddp_X = Convert.ToDouble(m_WordStr[14]);
                        RealCalibrationData_H.ddp_Y = Convert.ToDouble(m_WordStr[15]);
                    }
                }
            }
        }

        private void SaveCalibSetFiles(string SaveCalibFilePath)
        {
            string WritePresetDataStr;

            StreamWriter sw = new StreamWriter(SaveCalibFilePath, false, Encoding.Default);

            WritePresetDataStr = RealCalibrationData_A.ddp_X.ToString() + "|" + RealCalibrationData_A.ddp_Y.ToString() + "|" + RealCalibrationData_B.ddp_X.ToString() + "|" + RealCalibrationData_B.ddp_Y.ToString();
            WritePresetDataStr = WritePresetDataStr + "|" + RealCalibrationData_C.ddp_X.ToString() + "|" + RealCalibrationData_C.ddp_Y.ToString() + "|" + RealCalibrationData_D.ddp_X.ToString() + "|" + RealCalibrationData_D.ddp_Y.ToString();
            WritePresetDataStr = WritePresetDataStr + "|" + RealCalibrationData_E.ddp_X.ToString() + "|" + RealCalibrationData_E.ddp_Y.ToString() + "|" + RealCalibrationData_F.ddp_X.ToString() + "|" + RealCalibrationData_F.ddp_Y.ToString();
            WritePresetDataStr = WritePresetDataStr + "|" + RealCalibrationData_G.ddp_X.ToString() + "|" + RealCalibrationData_G.ddp_Y.ToString() + "|" + RealCalibrationData_H.ddp_X.ToString() + "|" + RealCalibrationData_H.ddp_Y.ToString();
            sw.Write(WritePresetDataStr);

            sw.Close();

        }


        private void btnSave_Click(object sender, EventArgs e)
        {

            RecoSetData.Move_R_FIX_Angle_Gap = Convert.ToDouble(txtFixAngleAddGap.Text);
            RecoSetData.Move_R_MOVE_Angle_Gap = Convert.ToDouble(txtMoveAngleAddGap.Text);
            RecoSetData.Back_R_MOVE_Angle_Gap = Convert.ToDouble(txtBackAngleAddGap.Text);
            RecoSetData.Top_R_MOVE_Angle_Gap = Convert.ToDouble(txtTopAngleAddGap.Text);
            RecoSetData.Object_Weight = Convert.ToDouble(txtPlangeThickness.Text);
            if (RecoSetData.Object_Weight<0)
            {
                RecoSetData.Object_Weight = 0;
            }

            RecoSetData.Object_Hole_Distance = Convert.ToDouble(txtPinHoleDistance.Text);
            RecoSetData.Object_Diameter = Convert.ToDouble(txtPinHoleDiameter.Text);
            RecoSetData.Object_Pin_Diameter = Convert.ToDouble(txtCalcPinDiameter.Text);
            RecoSetData.Object_Pin_Height = Convert.ToDouble(txtCalcPinHeight.Text);

            RecoSetData.Object_TopHoleType = cboTopHolePos.SelectedIndex;

            RecoSetData.Object_TopDirHoleDistance = Convert.ToDouble(txtTopDirHoleDistance.Text);
            RecoSetData.Object_ShaftLength = Convert.ToDouble(txtShaftLength.Text);

            RecoSetData.DXF_RecoType = cboDXFType.SelectedIndex;
            
            

            //cboTopHolePos

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
                //NewDXFFileNameStr = CfgSaveHeadFileName + @".dxf";
                NewDXFFileNameStr = GetOutOnlyFileName(OriSrcPath);
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

#endregion 강성호 ...


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
                RecoSetData.Motion_Move_MR = double.Parse(txtMoveRValue.Text);


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

                IndexEnableSetting(cboZoomCamList.SelectedIndex, cboIndexList.SelectedIndex);

                CamOpticalGapIndex = cboZoomCamList.SelectedIndex;

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
            int SetValueNum = int.Parse(txtLampValue.Text);
            SetValueNum = SetValueNum - LAMP_INC_GAP;
            if (SetValueNum<0)
            {
                SetValueNum = 0;
            }
            txtLampValue.Text = SetValueNum.ToString();
            SetValueLamp();
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
                if (SelectCamIndex == 0 || SelectCamIndex == 1)// || SelectCamIndex == 2)
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
            // 속도 설정 ...
            // --------------------------------------------------
            AxisSpeed = _WorkFuncInfo.AxisSpeed;

            

            SelectSpeed(AxisSpeed);

            MultiMotion.SetSpeed(AxisSpeed);
            // --------------------------------------------------



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
            //txtMoveRValue.Text = _WorkFuncInfo.WFMoveZ.ToString();
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


            if (SelectedRB != null)
            {
                _WorkFuncInfo.AxisSpeed = Convert.ToInt32(SelectedRB.Tag);
            }
        }

        private void cboIndexList_SelectedIndexChanged(object sender, EventArgs e)
        {
            _WorkFuncInfo.Rotation_Index = cboIndexList.SelectedIndex;
            IndexEnableSetting(cboZoomCamList.SelectedIndex, cboIndexList.SelectedIndex);
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

            if (MultiMotion.CheckDefense() == MultiMotion.KSM_OK)
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
                        txtXAxisValue.Text = MultiMotion.AxisValue[MultiMotion.CAM_UNIT_X].ToString("##.0000");
                        txtYAxisValue.Text = MultiMotion.AxisValue[MultiMotion.CAM_UNIT_Y].ToString("##.0000");
                        txtZAxisValue.Text = MultiMotion.AxisValue[MultiMotion.CAM_UNIT_Z].ToString("##.0000");
                    }
                    break;
                case 1: // 후방 카메라 이동(Z축) 기능
                    {
                        txtXAxisValue.Text = "0.0";
                        txtYAxisValue.Text = "0.0";
                        txtZAxisValue.Text = MultiMotion.AxisValue[MultiMotion.BACK_CAM_Z].ToString("##.000");
                    }
                    break;
            }

            switch (_WorkFuncInfo.Rotation_Index)
            {
                case 0: // 고정축 INDEX 회전(R)
                    {
                        txtRAxisValue.Text = MultiMotion.AxisValue[MultiMotion.INDEX_FIX_R].ToString("##.000");
                    }
                    break;
                case 1: // 이동축 INDEX 회전(R)
                    {
                        txtRAxisValue.Text = MultiMotion.AxisValue[MultiMotion.INDEX_MOVE_R].ToString("##.000");
                    }                    
                    break;
            }


            txtMoveRValue.Text = MultiMotion.AxisValue[MultiMotion.INDEX_MOVE_M].ToString("##.0000");

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

                    if (MultiMotion.GantryAxisEnable(1, true) == MultiMotion.KSM_OK)
                    {
                        MultiMotion.HomeMove(MultiMotion.CAM_UNIT_X, true);
                    }
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
            //timerAxis.Enabled = false;

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
            //timerAxis.Enabled = false;

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
                            if (dTempValue > -1.0 && dTempValue < 1300.0)
                            {
                                MultiMotion.MoveAxis(MultiMotion.CAM_UNIT_X, dTempValue, false);
                            }
                            
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
                            

                            switch (_WorkFuncInfo.Motion_Index)
                            {
                                case 0: // 카메라 유닛 이동(X/Y/Z)  기능
                                    if (dTempValue > -1.0 && dTempValue < 590.0)
                                    {
                                        MultiMotion.MoveAxis(MultiMotion.CAM_UNIT_Z, dTempValue, false);
                                    }
                                    
                                    //MultiMotion.JogStop(MultiMotion.CAM_UNIT_Z);
                                    break;
                                case 1: // 후방 카메라 이동(Z축) 기능
                                    if (dTempValue > -23.0 && dTempValue < 30.0)
                                    {
                                        MultiMotion.MoveAxis(MultiMotion.BACK_CAM_Z, dTempValue, false);
                                    }
                                    break;
                            }
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
                        MultiMotion.JogMove(MultiMotion.INDEX_FIX_R, 1);
                        break;
                    case 1: // 이동축 INDEX 회전(R)
                        MultiMotion.JogMove(MultiMotion.INDEX_MOVE_R, 1);
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
                        MultiMotion.JogStop(MultiMotion.INDEX_FIX_R);
                        break;
                    case 1: // 이동축 INDEX 회전(R)
                        MultiMotion.JogStop(MultiMotion.INDEX_MOVE_R);
                        break;
                }
            }
        }

        private void btnJogRPlus_MouseDown(object sender, MouseEventArgs e)
        {
            timerAxis.Enabled = true;

            if (MultiMotion.GantryAxisEnable(0, false) == MultiMotion.KSM_OK) // INDEX(R) 갠트리 비활성화
            {
                switch (_WorkFuncInfo.Rotation_Index)
                {
                    case 0: // 고정축 INDEX 회전(R)
                        MultiMotion.JogMove(MultiMotion.INDEX_FIX_R, 0);
                        break;
                    case 1: // 이동축 INDEX 회전(R)
                        MultiMotion.JogMove(MultiMotion.INDEX_MOVE_R, 0);
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
                        MultiMotion.JogStop(MultiMotion.INDEX_FIX_R);
                        break;
                    case 1: // 이동축 INDEX 회전(R)
                        MultiMotion.JogStop(MultiMotion.INDEX_MOVE_R);
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



        // INDEX MOVE X ...
        // --------------------------------------------------


        private void btnJogMRMinus_MouseDown(object sender, MouseEventArgs e)
        {
            timerAxis.Enabled = true;

            if (MultiMotion.GantryAxisEnable(1, true) == MultiMotion.KSM_OK) // INDEX(X) 갠트리 활성화
            {
                MultiMotion.StepMove(MultiMotion.INDEX_MOVE_M, 1, false);
            }
        }

        private void btnJogMRMinus_MouseUp(object sender, MouseEventArgs e)
        {
        }

        private void btnJogMRPlus_MouseDown(object sender, MouseEventArgs e)
        {
            timerAxis.Enabled = true;

            if (MultiMotion.GantryAxisEnable(1, true) == MultiMotion.KSM_OK) // INDEX(X) 갠트리 활성화
            {
                MultiMotion.StepMove(MultiMotion.INDEX_MOVE_M, 0, false);
            }
        }

        private void btnJogMRPlus_MouseUp(object sender, MouseEventArgs e)
        {
        }

        private void btnJogMRHome_Click(object sender, EventArgs e)
        {
            timerAxis.Enabled = true;

            if (MultiMotion.GantryAxisEnable(1, true) == MultiMotion.KSM_OK) // INDEX(X) 갠트리 활성화
            {
                MultiMotion.HomeMove(MultiMotion.INDEX_MOVE_M, true);

                MessageBox.Show("Home완료!");
            }
        }

        private void txtMoveRValue_KeyDown(object sender, KeyEventArgs e)
        {
            double dTempValue = 0.0;

            switch (e.KeyCode)
            {
                case Keys.Enter:
                    {
                        if (double.TryParse(this.txtMoveRValue.Text, out dTempValue))
                        {
                            if (MultiMotion.GantryAxisEnable(1, true) == MultiMotion.KSM_OK) // INDEX(X) 갠트리 활성화
                            {
                                if (dTempValue > -1.0 && dTempValue < 1500.0)
                                {
                                    MultiMotion.MoveAxis(MultiMotion.INDEX_MOVE_M, dTempValue, false);
                                }                                
                            }
                            
                        }

                        timerAxis.Enabled = true;
                    }
                    break;
                default:
                    timerAxis.Enabled = false;
                    break;
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
                DXFObjList[i].SelectLyFlag =false;
            }

            for (int i = 0; i < FCADImage.FEntities.Entities.Count; i++)
            {
                GetEntityName = FCADImage.FEntities.Entities[i].ToString();
                if (GetEntityName == "DXFImportReco.DXFLine")
                {

                    dxLine = (DXFLine)FCADImage.FEntities.Entities[i];

                    if (dxLine.FColor == Color.Red)
                    {
                        DXFObjList[LoopDXFCount].SelectLyFlag = true;
                    }

                    P1 = GetPoint(dxLine.Point1);
                    P2 = GetPoint(dxLine.Point2);

                    DXFObjList[LoopDXFCount].Obj_X1 = (int)P1.X;
                    DXFObjList[LoopDXFCount].Obj_Y1 = (int)P1.Y;
                    DXFObjList[LoopDXFCount].Obj_X2 = (int)P2.X;
                    DXFObjList[LoopDXFCount].Obj_Y2 = (int)P2.Y;
                    Calc_Gap_X = (int)(DXFObjList[LoopDXFCount].Obj_X1 - DXFObjList[LoopDXFCount].Obj_X2);
                    Calc_Gap_Y = (int)(DXFObjList[LoopDXFCount].Obj_Y1 - DXFObjList[LoopDXFCount].Obj_Y2);

                    if (Math.Sqrt(Calc_Gap_X * Calc_Gap_X + Calc_Gap_Y * Calc_Gap_Y) > 100)
                    {

                        DXFObjList[LoopDXFCount].ExistLyFlag = true;
                        DXFObjList[LoopDXFCount].LyType = SHAPE_LINE;

                        //Calc_Gap_X = (int)(DXFObjList[LoopDXFCount].Obj_X1 + DXFObjList[LoopDXFCount].Obj_X2) / 2 - Base.X;
                        //Calc_Gap_Y = Base.Y - (int)(DXFObjList[LoopDXFCount].Obj_Y1 + DXFObjList[LoopDXFCount].Obj_Y2) / 2;
                        ObjectPointInfo GetCenterDat = Get3PointCurPos((int)DXFObjList[LoopDXFCount].Obj_X1, (int)DXFObjList[LoopDXFCount].Obj_Y1, (int)DXFObjList[LoopDXFCount].Obj_X2, (int)DXFObjList[LoopDXFCount].Obj_Y2, Base.X, Base.Y);
                        Calc_Gap_X = GetCenterDat.Obj_X - Base.X;
                        Calc_Gap_Y = Base.Y - GetCenterDat.Obj_Y;
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


        private ObjectPointInfo Get3PointCurPos(int p3_x1, int p3_y1, int p3_x2, int p3_y2, int p3_cn_x1, int p3_cn_y1)
        {
            double a, b;
            double calc_x, calc_y;
            ObjectPointInfo ResPntData;
            ResPntData.Obj_X = 0;
            ResPntData.Obj_Y = 0;

            a = p3_x2 - p3_x1;
            b = p3_y2 - p3_y1;

            calc_x = (b * b * (double)p3_x2 - a * b * (double)p3_y2 + a * a * (double)p3_cn_x1 + a * b * (double)p3_cn_y1) / (b * b + a * a);
            calc_y = -1 * (a / b * (calc_x - (double)p3_cn_x1)) + (double)p3_cn_y1;

            ResPntData.Obj_X = (int)calc_x;
            ResPntData.Obj_Y = (int)calc_y;

            if (p3_x1 == p3_x2)
            {
                ResPntData.Obj_X = p3_x1;
            }

            if (p3_y1 == p3_y2)
            {
                ResPntData.Obj_Y = p3_y1;
            }

            return ResPntData;
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
            double CmpMoveGapPos = 0;
            double TmpMoveValueAxis = 0;
            short TmpMoveAxis_Index;

            if (SetMoveValueAxis > FIX_INDEX_POS_MIN && SetMoveValueAxis < MOVE_INDEX_POS_MAX)
            {
                MultiMotion.GetCurrentPos();
                TmpMoveAxis_Index = MultiMotion.CAM_UNIT_X;

                TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
                MultiMotion.MoveAxis(TmpMoveAxis_Index, SetMoveValueAxis, true);
                MultiMotion.GetCurrentPos();
                TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
                CmpMoveGapPos = TmpMoveValueAxis - SetMoveValueAxis;
                CmpMoveGapPos = Math_Abs(CmpMoveGapPos);
                while (CmpMoveGapPos > 1.0)
                {
                    MultiMotion.GetCurrentPos();
                    TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
                    CmpMoveGapPos = TmpMoveValueAxis - SetMoveValueAxis;
                    CmpMoveGapPos = Math_Abs(CmpMoveGapPos);
                    Application.DoEvents();
                    //DelayWaitRun(1);
                    if (ExitRecoFormFlag == true)
                    {
                        break;
                    }

                }
            }
            else
            {
                MultiMotion.GetCurrentPos();
                TmpMoveAxis_Index = MultiMotion.CAM_UNIT_Z;
                TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
                if (TmpMoveValueAxis>80)
                {
                    TmpMoveAxis_Index = MultiMotion.INDEX_MOVE_M;
                    TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
                    if (TmpMoveValueAxis<800)
                    {
                        MessageBox.Show("X축이동위치에 Index이동축이 있어 부딪힐 수 있습니다.");
                        return;
                    }
                }
                
                {
                    MultiMotion.GetCurrentPos();
                    TmpMoveAxis_Index = MultiMotion.CAM_UNIT_X;

                    TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
                    MultiMotion.MoveAxis(TmpMoveAxis_Index, SetMoveValueAxis, true);
                    MultiMotion.GetCurrentPos();
                    TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
                    CmpMoveGapPos = TmpMoveValueAxis - SetMoveValueAxis;
                    CmpMoveGapPos = Math_Abs(CmpMoveGapPos);
                    while (CmpMoveGapPos > 1.0)
                    {
                        MultiMotion.GetCurrentPos();
                        TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
                        CmpMoveGapPos = TmpMoveValueAxis - SetMoveValueAxis;
                        CmpMoveGapPos = Math_Abs(CmpMoveGapPos);
                        Application.DoEvents();
                        //DelayWaitRun(1);
                        if (ExitRecoFormFlag == true)
                        {
                            break;
                        }

                    }
                    //TmpMoveAxis_Index = MultiMotion.CAM_UNIT_X;
                }
            }
        }

        private void RunMotionAxis_Y(double SetMoveValueAxis)
        {
            double CmpMoveGapPos = 0;
            double TmpMoveValueAxis = 0;
            short TmpMoveAxis_Index;

            MultiMotion.GetCurrentPos();
            TmpMoveAxis_Index = MultiMotion.CAM_UNIT_Y;

            TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
            MultiMotion.MoveAxis(TmpMoveAxis_Index, SetMoveValueAxis, true);
            return;
            MultiMotion.GetCurrentPos();
            TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
            CmpMoveGapPos = TmpMoveValueAxis - SetMoveValueAxis;
            CmpMoveGapPos = Math_Abs(CmpMoveGapPos);
            while (CmpMoveGapPos > 1.0)
            {
                MultiMotion.GetCurrentPos();
                TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
                CmpMoveGapPos = TmpMoveValueAxis - SetMoveValueAxis;
                CmpMoveGapPos = Math_Abs(CmpMoveGapPos);
                Application.DoEvents();
                //DelayWaitRun(1);
                if (ExitRecoFormFlag == true)
                {
                    break;
                }
            }
            TmpMoveAxis_Index = 0;
        }

        private void RunMotionAxis_Z(double SetMoveValueAxis)
        {
            double CmpMoveGapPos = 0;
            double TmpMoveValueAxis = 0;
            short TmpMoveAxis_Index;

            MultiMotion.GetCurrentPos();
            TmpMoveAxis_Index = MultiMotion.CAM_UNIT_Z;
            TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
            MultiMotion.MoveAxis(TmpMoveAxis_Index, SetMoveValueAxis, true);
            MultiMotion.GetCurrentPos();
            return;
            TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
            CmpMoveGapPos = TmpMoveValueAxis - SetMoveValueAxis;
            CmpMoveGapPos = Math_Abs(CmpMoveGapPos);
            while (CmpMoveGapPos > 1.0)
            {
                MultiMotion.GetCurrentPos();
                TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
                CmpMoveGapPos = TmpMoveValueAxis - SetMoveValueAxis;
                CmpMoveGapPos = Math_Abs(CmpMoveGapPos);
                Application.DoEvents();
                //DelayWaitRun(1);
                if(ExitRecoFormFlag == true)
                {
                    break;
                }
            }
        }

        private void RunMotionAxis_Back_Z(double SetMoveValueAxis)
        {
            double CmpMoveGapPos = 0;
            double TmpMoveValueAxis = 0;
            short TmpMoveAxis_Index;

            MultiMotion.GetCurrentPos();
            TmpMoveAxis_Index = MultiMotion.BACK_CAM_Z;
            TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
            MultiMotion.MoveAxis(TmpMoveAxis_Index, SetMoveValueAxis, true);
            MultiMotion.GetCurrentPos();
            TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
            CmpMoveGapPos = TmpMoveValueAxis - SetMoveValueAxis;
            CmpMoveGapPos = Math_Abs(CmpMoveGapPos);
            while (CmpMoveGapPos > 1.0)
            {
                MultiMotion.GetCurrentPos();
                TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
                CmpMoveGapPos = TmpMoveValueAxis - SetMoveValueAxis;
                CmpMoveGapPos = Math_Abs(CmpMoveGapPos);
                Application.DoEvents();
                //DelayWaitRun(1);
                if (ExitRecoFormFlag == true)
                {
                    break;
                }
            }
        }

        private void RunMotionAxis_MR(double SetMoveValueAxis)
        {
            double CmpMoveGapPos = 0;
            double TmpMoveValueAxis = 0;
            short TmpMoveAxis_Index;

            MultiMotion.GetCurrentPos();
            TmpMoveAxis_Index = MultiMotion.INDEX_MOVE_M;
            TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
            if (MultiMotion.GantryAxisEnable(1, true) == MultiMotion.KSM_OK) // INDEX(X) 갠트리 활성화
            {
                MultiMotion.MoveAxis(TmpMoveAxis_Index, SetMoveValueAxis, true);
            }
            MultiMotion.GetCurrentPos();
            TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
            CmpMoveGapPos = TmpMoveValueAxis - SetMoveValueAxis;
            CmpMoveGapPos = Math_Abs(CmpMoveGapPos);
            while (CmpMoveGapPos > 1.0)
            {
                MultiMotion.GetCurrentPos();
                TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
                CmpMoveGapPos = TmpMoveValueAxis - SetMoveValueAxis;
                CmpMoveGapPos = Math_Abs(CmpMoveGapPos);
                Application.DoEvents();
                //DelayWaitRun(1);
                if (ExitRecoFormFlag == true)
                {
                    break;
                }
            }
        }


        private void RotationIndex_Axis(short RotationIndexAxis, double SetMoveValueAxis)
        {
            double CmpMoveGapPos = 0;
            double TmpMoveValueAxis = 0;
            short TmpMoveAxis_Index;

            MultiMotion.GetCurrentPos();
            TmpMoveAxis_Index = RotationIndexAxis;
            TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
            MultiMotion.MoveAxis(TmpMoveAxis_Index, SetMoveValueAxis, false);
            CmpMoveGapPos = TmpMoveValueAxis - SetMoveValueAxis;
            CmpMoveGapPos = Math_Abs(CmpMoveGapPos);
            while (CmpMoveGapPos > 1.0)
            {
                MultiMotion.GetCurrentPos();
                TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
                CmpMoveGapPos = TmpMoveValueAxis - SetMoveValueAxis;
                CmpMoveGapPos = Math_Abs(CmpMoveGapPos);
                DelayWaitRun(1);
                if (ExitRecoFormFlag == true)
                {
                    break;
                }
            }
        }


        private double Math_Abs(double RecvPosNum)
        {
            double TmpResNum;
            if(RecvPosNum<0)
            {
                TmpResNum = 0 - RecvPosNum;
            }
            else
            {
                TmpResNum = RecvPosNum;
            }
            return TmpResNum;
        }

        private void FullShotMove()
        {
            bool TopAlignProcFlag = false;
            bool BackAlignProcFlag = false;
            bool MoveBackAlignProcFlag = false;
            double SetMoveValueAxis = 0;
            double TmpMoveValueAxis = 0;
            short TmpMoveAxis_Index;
            short RotateIndex_Index;
            double ObjectPlWeight, ObjPLAddMove_X;

            timerAxis.Enabled = false;

            if (cboIndexList.SelectedIndex == 0)
            {
                RotateIndex_Index = MultiMotion.INDEX_FIX_R;
            }
            else
            {
                RotateIndex_Index = MultiMotion.INDEX_MOVE_R;
            }
             
            if (cboZoomCamList.SelectedIndex == 0 && cboIndexList.SelectedIndex == 1)
            {
                MoveBackAlignProcFlag = true;
            }

            if (cboZoomCamList.SelectedIndex == 3)
            {
                TopAlignProcFlag = true;
            }

            if (cboZoomCamList.SelectedIndex == 2)
            {
                BackAlignProcFlag = true;
            }

            if (BackAlignProcFlag == true)
            {
                RunMotionAxis_Back_Z(RecoSetData.Motion_Move_Z);
            }
            else if (MoveBackAlignProcFlag == true)
            {
                if (RecoSetData.Motion_Move_MR<800)
                {
                    MessageBox.Show("이동축의 위치가 후방카메라와 겹칩니다.");
                    return;
                }

                MultiMotion.GetCurrentPos();
                TmpMoveAxis_Index = MultiMotion.INDEX_MOVE_M;
                TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];

                if (TmpMoveValueAxis<800.0)
                {
                    RunMotionAxis_Z(1.0);
                }

                RunMotionAxis_MR(RecoSetData.Motion_Move_MR);

                RunMotionAxis_X(RecoSetData.Motion_Move_X);

                RunMotionAxis_Y(RecoSetData.Motion_Move_Y);

                RunMotionAxis_Z(RecoSetData.Motion_Move_Z);
            }
            else
            {
                MultiMotion.GetCurrentPos();
                TmpMoveAxis_Index = MultiMotion.INDEX_MOVE_M;
                TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];

                if (TmpMoveValueAxis > 10.0)
                {
                    RunMotionAxis_Z(1.0);
                    RunMotionAxis_MR(1.0);
                }

                ObjectPlWeight = RecoSetData.Object_Weight;
                if (ObjectPlWeight > OBJECT_WEIGHT_MAX)
                {
                    ObjectPlWeight = OBJECT_WEIGHT_MAX;
                }
                else if (ObjectPlWeight < 0)
                {
                    ObjectPlWeight = 0;
                }

                if (RotateIndex_Index == MultiMotion.INDEX_MOVE_R)
                {
                    //ObjPLAddMove_X = RecoSetData.Motion_Move_X + ObjectPlWeight;
                    ObjPLAddMove_X = MOVE_INDEX_POS + ObjectPlWeight;
                }
                else
                {
                    //ObjPLAddMove_X = RecoSetData.Motion_Move_X - ObjectPlWeight;
                    ObjPLAddMove_X = FIX_INDEX_POS - ObjectPlWeight;
                }

                //double SetMoveValueAxis = 0;
                //double TmpMoveValueAxis = 0;
                //short TmpMoveAxis_Index;

                /*
                SetMoveValueAxis = RecoSetData.Motion_Move_X;
                TmpMoveAxis_Index = MultiMotion.CAM_UNIT_X;

                TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
                MultiMotion.MoveAxis(TmpMoveAxis_Index, SetMoveValueAxis, true);
                while (Math.Abs(MultiMotion.AxisValue[TmpMoveAxis_Index] - TmpMoveValueAxis) > 1.0)
                {
                    DelayWaitRun(5);
                }
                */
                RunMotionAxis_X(ObjPLAddMove_X);

                /*
                SetMoveValueAxis = RecoSetData.Motion_Move_Y;
                TmpMoveAxis_Index = MultiMotion.CAM_UNIT_Y;

                TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
                MultiMotion.MoveAxis(TmpMoveAxis_Index, SetMoveValueAxis, true);
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
                MultiMotion.MoveAxis(TmpMoveAxis_Index, SetMoveValueAxis, true);
                while (Math.Abs(MultiMotion.AxisValue[TmpMoveAxis_Index] - TmpMoveValueAxis) > 1.0)
                {
                    DelayWaitRun(5);
                }
                */
                RunMotionAxis_Z(RecoSetData.Motion_Move_Z);
            }


            timerAxis.Enabled = true;
        }

        private void DetailShotSelectObjMove()
        {
            double CmpMoveGapPos = 0;
            SFPoint GetDetailSelObj;

            double Detail_Move_Pos_X, Detail_Move_Pos_Y, Detail_Move_Pos_Z;
            double SetMoveValueAxis = 0;
            double TmpMoveValueAxis = 0;
            short TmpMoveAxis_Index;
            short RotateIndex_Index;

            bool TopAlignProcFlag = false;
            bool BackAlignProcFlag = false;
            bool MoveBackAlignProcFlag = false;

            timerAxis.Enabled = false;

            if (cboZoomCamList.SelectedIndex == 0 && cboIndexList.SelectedIndex == 1)
            {
                MoveBackAlignProcFlag = true;
            }

            if (cboZoomCamList.SelectedIndex == 3)
            {
                TopAlignProcFlag = true;
            }

            if (cboZoomCamList.SelectedIndex == 2)
            {
                BackAlignProcFlag = true;
            }

            if (cboIndexList.SelectedIndex == 0)
            {
                RotateIndex_Index = MultiMotion.INDEX_FIX_R;
            }
            else
            {
                RotateIndex_Index = MultiMotion.INDEX_MOVE_R;
            }

            if (BackAlignProcFlag == true)
            {
                GetDetailSelObj = GetSelectDXFData();
                Detail_Move_Pos_X = 0;
                Detail_Move_Pos_Y = 0 - GetDetailSelObj.Y;

                Detail_Move_Pos_Z = GetDetailSelObj.Z;

                MultiMotion.GetCurrentPos();
                TmpMoveAxis_Index = MultiMotion.BACK_CAM_Z;
                TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
                SetMoveValueAxis = RecoSetData.Motion_Move_Z - Detail_Move_Pos_Z;
                MultiMotion.MoveAxis(TmpMoveAxis_Index, SetMoveValueAxis, false);
                MultiMotion.GetCurrentPos();
                while (Math.Abs(MultiMotion.AxisValue[TmpMoveAxis_Index] - TmpMoveValueAxis) > 1.0)
                {
                    MultiMotion.GetCurrentPos();
                    DelayWaitRun(5);
                }
            }
            else if (TopAlignProcFlag == true)
            {
                Detail_Move_Pos_X = RecoSetData.Object_Hole_Distance;

                MultiMotion.GetCurrentPos();
                TmpMoveAxis_Index = MultiMotion.CAM_UNIT_X;
                TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
                SetMoveValueAxis = TmpMoveValueAxis - Detail_Move_Pos_X;

                //SetMoveValueAxis = TmpMoveValueAxis - Detail_Move_Pos_Y;
                MultiMotion.MoveAxis(TmpMoveAxis_Index, SetMoveValueAxis, true);
                MultiMotion.GetCurrentPos();
                /*
                while (Math.Abs(MultiMotion.AxisValue[TmpMoveAxis_Index] - TmpMoveValueAxis) > 1.0)
                {
                    MultiMotion.GetCurrentPos();
                    DelayWaitRun(5);
                }
                */
            }
            else
            {
                GetDetailSelObj = GetSelectDXFData();

                DoubleDataPosInfo GetInputAnglePos, GetOutputAnglePos;


                Detail_Move_Pos_X = 0;
                Detail_Move_Pos_Y = 0 - GetDetailSelObj.Y;
                Detail_Move_Pos_Z = GetDetailSelObj.Z;

                if (ReAlignIndex == RE_ALIGN_TRANSLATION)
                {
                    if (RotateIndex_Index == MultiMotion.INDEX_MOVE_R)
                    {
                        GetInputAnglePos.ddp_ExistFlag = true;
                        GetInputAnglePos.ddp_X = Detail_Move_Pos_Y;
                        GetInputAnglePos.ddp_Y = Detail_Move_Pos_Z;

                        GetOutputAnglePos = GetDataMovePos2(GetInputAnglePos);

                        Detail_Move_Pos_Y = GetOutputAnglePos.ddp_X;
                        Detail_Move_Pos_Z = GetOutputAnglePos.ddp_Y;

                    }
                    else
                    {
                        GetInputAnglePos.ddp_ExistFlag = true;
                        GetInputAnglePos.ddp_X = Detail_Move_Pos_Y;
                        GetInputAnglePos.ddp_Y = Detail_Move_Pos_Z;

                        GetOutputAnglePos = GetDataFixPos2(GetInputAnglePos);

                        Detail_Move_Pos_Y = GetOutputAnglePos.ddp_X;
                        Detail_Move_Pos_Z = GetOutputAnglePos.ddp_Y;

                        //Detail_Move_Pos_Y = 0 - Detail_Move_Pos_Y;
                    }
                }




                MultiMotion.GetCurrentPos();
                TmpMoveAxis_Index = MultiMotion.CAM_UNIT_Y;
                TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
                if (RotateIndex_Index == MultiMotion.INDEX_MOVE_R)
                {
                    SetMoveValueAxis = RecoSetData.Motion_Move_Y + Detail_Move_Pos_Y;
                }
                else
                {
                    SetMoveValueAxis = RecoSetData.Motion_Move_Y - Detail_Move_Pos_Y;
                }

                MultiMotion.GetCurrentPos();
                //SetMoveValueAxis = TmpMoveValueAxis - Detail_Move_Pos_Y;
                MultiMotion.MoveAxis(TmpMoveAxis_Index, SetMoveValueAxis, false);
                MultiMotion.GetCurrentPos();
                while (Math.Abs(MultiMotion.AxisValue[TmpMoveAxis_Index] - TmpMoveValueAxis) > 1.0)
                {
                    MultiMotion.GetCurrentPos();
                    DelayWaitRun(5);
                }

                MultiMotion.GetCurrentPos();
                TmpMoveAxis_Index = MultiMotion.CAM_UNIT_Z;
                TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
                SetMoveValueAxis = RecoSetData.Motion_Move_Z - Detail_Move_Pos_Z;
                MultiMotion.MoveAxis(TmpMoveAxis_Index, SetMoveValueAxis, false);
                MultiMotion.GetCurrentPos();
                while (Math.Abs(MultiMotion.AxisValue[TmpMoveAxis_Index] - TmpMoveValueAxis) > 1.0)
                {
                    MultiMotion.GetCurrentPos();
                    DelayWaitRun(5);
                }
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
            bool TopAlignProcFlag = false;
            bool BackAlignProcFlag = false;
            bool MoveBackAlignProcFlag = false;
            double DXF_Center_Hole_Diameter = 20;


            if (cboZoomCamList.SelectedIndex == 0 && cboIndexList.SelectedIndex == 1)
            {
                MoveBackAlignProcFlag = true;
            }

            if (cboZoomCamList.SelectedIndex == 3)
            {
                TopAlignProcFlag = true;
                DXF_Center_Hole_Diameter = 16;
            }

            if (cboZoomCamList.SelectedIndex == 2)
            {
                BackAlignProcFlag = true;
            }

            if (TopAlignProcFlag == true)
            {
                //tLampValue.Text = "410";
                //SetValueLamp();
            }
            else
            {
                //txtLampValue.Text = "410";
                //SetValueLamp();
            }

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
                UpdatePos();
                label5.Text = "CALIBRATION START";
                LpWk_ZeroCount = 0;
                Calib_Run_Sts = CALIB_RECOPROCESS;
            }
            else if (Calib_Run_Sts == CALIB_ZOOMFOCUS)
            {
                if (LpWk_ZeroCount == 0)
                {
                    label5.Text = "CALIBRATION ZOOMFOCUS";
                    if (TopAlignProcFlag == true || MoveBackAlignProcFlag == true)
                    {
                        //CurZoomFactNum = ShotData[FULLSHOTTYPE].ZoomNum;
                        //CurFocusFactNum = ShotData[FULLSHOTTYPE].FocusNum;
                    }
                    else
                    {
                        if (RotateIndex_Index == MultiMotion.INDEX_MOVE_R)
                        {
                            //CurZoomFactNum = 11733;
                            //CurFocusFactNum = 19013;
                            CurZoomFactNum = 11642;
                            CurFocusFactNum = 21280;
                        }
                        else
                        {
                            //CurZoomFactNum = 11733;
                            //CurFocusFactNum = 19514;
                            CurZoomFactNum = 11642;
                            CurFocusFactNum = 21280;
                        }
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

                    if (TopAlignProcFlag == true)
                    {
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
                            CameraMoveXYPos = RecoCls.FindCalibrationCenterAlign(DXF_Center_Hole_Diameter);
                            picRecoImg.Image = RecoCls.GetArrCalibrationCenterObjectImage(CameraMoveXYPos);
                            //DrawDXFImage((Bitmap)picRecoImg.Image);

                        }
                    }
                    else
                    {
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
                            CameraMoveXYPos = RecoCls.FindCalibrationCenterAlign(11);
                            picRecoImg.Image = RecoCls.GetArrCalibrationCenterObjectImage(CameraMoveXYPos);
                            //DrawDXFImage((Bitmap)picRecoImg.Image);

                        }
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

                    UpdatePos();

                    if (TopAlignProcFlag == true)
                    {
                        if (CameraMoveXYPos.ddp_ExistFlag == true)
                        {
                            TmpMoveAxis_Index = MultiMotion.CAM_UNIT_Y;
                            TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
                            SetMoveValueAxis = TmpMoveValueAxis - CameraMoveXYPos.ddp_X;
                            RunMotionAxis_Y(SetMoveValueAxis);


                            TmpMoveAxis_Index = MultiMotion.CAM_UNIT_X;
                            TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
                            SetMoveValueAxis = TmpMoveValueAxis - CameraMoveXYPos.ddp_Y;
                            RunMotionAxis_X(SetMoveValueAxis);
                        }
                    }
                    else if (MoveBackAlignProcFlag == true)
                    {
                        if (CameraMoveXYPos.ddp_ExistFlag == true)
                        {
                            TmpMoveAxis_Index = MultiMotion.CAM_UNIT_Y;
                            TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];

                            SetMoveValueAxis = TmpMoveValueAxis - CameraMoveXYPos.ddp_X;

                            RunMotionAxis_Y(SetMoveValueAxis);

                            TmpMoveAxis_Index = MultiMotion.CAM_UNIT_Z;
                            TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
                            SetMoveValueAxis = TmpMoveValueAxis - CameraMoveXYPos.ddp_Y;
                            RunMotionAxis_Z(SetMoveValueAxis);
                        }
                    }
                    else
                    {
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


                            TmpMoveAxis_Index = MultiMotion.CAM_UNIT_Z;
                            TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
                            SetMoveValueAxis = TmpMoveValueAxis - CameraMoveXYPos.ddp_Y;
                            RunMotionAxis_Z(SetMoveValueAxis);
                        }
                    }

                    LpWk_ZeroCount = 1;
                }
                if (LpWk_ZeroCount > ZeroLoopCountChkMax)
                {
                    UpdatePos();
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

                    if (TopAlignProcFlag == true)
                    {
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
                            CameraMoveXYPos = RecoCls.FindCalibrationCenterAlign(DXF_Center_Hole_Diameter);
                            picRecoImg.Image = RecoCls.GetArrCalibrationCenterObjectImage(CameraMoveXYPos);
                            //DrawDXFImage((Bitmap)picRecoImg.Image);

                        }
                    }
                    else
                    {
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

                    UpdatePos();

                    if (TopAlignProcFlag == true)
                    {
                        if (CameraMoveXYPos.ddp_ExistFlag == true)
                        {
                            TmpMoveAxis_Index = MultiMotion.CAM_UNIT_Y;
                            TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
                            SetMoveValueAxis = TmpMoveValueAxis + CameraMoveXYPos.ddp_X;
                            RunMotionAxis_Y(SetMoveValueAxis);
                            RecoSetData.Motion_Move_Y = SetMoveValueAxis;


                            TmpMoveAxis_Index = MultiMotion.CAM_UNIT_X;
                            TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
                            SetMoveValueAxis = TmpMoveValueAxis - CameraMoveXYPos.ddp_Y;
                            RunMotionAxis_X(SetMoveValueAxis);
                            RecoSetData.Motion_Move_X = SetMoveValueAxis;

                            RealCalibrationData_D.ddp_X = RecoSetData.Motion_Move_Y;
                            RealCalibrationData_D.ddp_Y = RecoSetData.Motion_Move_X;
                        }
                    }
                    else if (MoveBackAlignProcFlag == true)
                    {
                        if (CameraMoveXYPos.ddp_ExistFlag == true)
                        {
                            TmpMoveAxis_Index = MultiMotion.CAM_UNIT_Y;
                            TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
                            
                            SetMoveValueAxis = TmpMoveValueAxis - CameraMoveXYPos.ddp_X;

                            RunMotionAxis_Y(SetMoveValueAxis);
                            RecoSetData.Motion_Move_Y = SetMoveValueAxis;


                            TmpMoveAxis_Index = MultiMotion.CAM_UNIT_Z;
                            TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
                            SetMoveValueAxis = TmpMoveValueAxis - CameraMoveXYPos.ddp_Y;
                            RunMotionAxis_Z(SetMoveValueAxis);
                            RecoSetData.Motion_Move_Z = SetMoveValueAxis;

                            RealCalibrationData_E.ddp_X = RecoSetData.Motion_Move_Y;
                            RealCalibrationData_E.ddp_Y = RecoSetData.Motion_Move_Z;
                        }
                    }
                    else
                    {
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
                    }


                    LpWk_ZeroCount = 1;
                }
                if (LpWk_ZeroCount > ZeroLoopCountChkMax)
                {
                    UpdatePos();
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
                UpdatePos();
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

            bool TopAlignProcFlag = false;
            bool BackAlignProcFlag = false;
            bool MoveBackAlignProcFlag = false;

            if (cboZoomCamList.SelectedIndex == 0 && cboIndexList.SelectedIndex == 1)
            {
                MoveBackAlignProcFlag = true;
            }

            if (cboZoomCamList.SelectedIndex == 3)
            {
                TopAlignProcFlag = true;
            }

            if (cboZoomCamList.SelectedIndex == 2)
            {
                BackAlignProcFlag = true;
            }

            if (MoveBackAlignProcFlag == true)
            {
                SetMoveValueAxis = RealCalibrationData_E.ddp_X;
                RunMotionAxis_Y(SetMoveValueAxis);
                RecoSetData.Motion_Move_Y = SetMoveValueAxis;
                SetMoveValueAxis = RealCalibrationData_E.ddp_Y;
                RunMotionAxis_Z(SetMoveValueAxis);
                RecoSetData.Motion_Move_Z = SetMoveValueAxis;

            }
            else if (cboZoomCamList.SelectedIndex == 0)
            {
                SetMoveValueAxis = RealCalibrationData_A.ddp_X;
                RunMotionAxis_Y(SetMoveValueAxis);
                RecoSetData.Motion_Move_Y = SetMoveValueAxis;
                SetMoveValueAxis = RealCalibrationData_A.ddp_Y;
                RunMotionAxis_Z(SetMoveValueAxis);
                RecoSetData.Motion_Move_Z = SetMoveValueAxis;
                
            }
            else if (cboZoomCamList.SelectedIndex == 1)
            {
                SetMoveValueAxis = RealCalibrationData_B.ddp_X;
                RunMotionAxis_Y(SetMoveValueAxis);
                RecoSetData.Motion_Move_Y = SetMoveValueAxis;
                SetMoveValueAxis = RealCalibrationData_B.ddp_Y;
                RunMotionAxis_Z(SetMoveValueAxis);
                RecoSetData.Motion_Move_Z = SetMoveValueAxis;

            }
            else if (cboZoomCamList.SelectedIndex == 2)
            {
                SetMoveValueAxis = RealCalibrationData_C.ddp_X;
                RunMotionAxis_Back_Z(SetMoveValueAxis);
                RecoSetData.Motion_Move_Z = SetMoveValueAxis;
            }
            else if (cboZoomCamList.SelectedIndex == 3)
            {
                SetMoveValueAxis = RealCalibrationData_D.ddp_X;
                RunMotionAxis_Y(SetMoveValueAxis);
                RecoSetData.Motion_Move_Y = SetMoveValueAxis;
                SetMoveValueAxis = RealCalibrationData_D.ddp_Y;
                RunMotionAxis_X(SetMoveValueAxis);
                RecoSetData.Motion_Move_X = SetMoveValueAxis;

            }

            MessageBox.Show("저장된 Center위치로 이동완료!");
            /*
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
            */
        }

        private void LoadCalibPosSetFiles(string SaveCalibFilePath)
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
                    Calibration_Origin.ddp_X = Convert.ToDouble(m_WordStr[0]);
                    Calibration_Origin.ddp_Y = Convert.ToDouble(m_WordStr[1]);
                    Calibration_Origin.ddp_Z = Convert.ToDouble(m_WordStr[2]);

                    Calibration_Current.ddp_X = Convert.ToDouble(m_WordStr[3]);
                    Calibration_Current.ddp_Y = Convert.ToDouble(m_WordStr[4]);
                    Calibration_Current.ddp_Z = Convert.ToDouble(m_WordStr[5]);

                    Calibration_GapNum.ddp_X = Calibration_Origin.ddp_X - Calibration_Current.ddp_X;
                    Calibration_GapNum.ddp_Y = Calibration_Origin.ddp_Y - Calibration_Current.ddp_Y;
                    Calibration_GapNum.ddp_Z = Calibration_Origin.ddp_Z - Calibration_Current.ddp_Z;
                }
            }
        }

        private void SaveCalibPosSetFiles(string SaveCalibFilePath)
        {
            string WritePresetDataStr;

            StreamWriter sw = new StreamWriter(SaveCalibFilePath, false, Encoding.Default);

            WritePresetDataStr = Calibration_Origin.ddp_X.ToString() + "|" + Calibration_Origin.ddp_Y.ToString() + "|" + Calibration_Origin.ddp_Z.ToString() + "|" + Calibration_Current.ddp_X.ToString() + "|" + Calibration_Current.ddp_Y.ToString() + "|" + Calibration_Current.ddp_Z.ToString();
            sw.Write(WritePresetDataStr);

            sw.Close();
        }

        private void IndexEnableSetting(int SelSettingIndex, int SelRotateIndex)
        {
            if(SelSettingIndex == 0)
            {
                if (SelRotateIndex == 1)
                {
                    txtFixAngleAddGap.Enabled = false;
                    txtMoveAngleAddGap.Enabled = true;
                    txtBackAngleAddGap.Enabled = false;
                    txtTopAngleAddGap.Enabled = false;
                    txtPlangeThickness.Enabled = true;

                    txtPinHoleDistance.Enabled = false;
                    txtPinHoleDiameter.Enabled = false;
                    txtCalcPinDiameter.Enabled = false;
                    txtCalcPinHeight.Enabled = false;
                    cboTopHolePos.Enabled = false;
                    txtTopDirHoleDistance.Enabled = false;
                }
                else
                {
                    txtFixAngleAddGap.Enabled = true;
                    txtMoveAngleAddGap.Enabled = false;
                    txtBackAngleAddGap.Enabled = false;
                    txtTopAngleAddGap.Enabled = false;
                    txtPlangeThickness.Enabled = true;

                    txtPinHoleDistance.Enabled = false;
                    txtPinHoleDiameter.Enabled = false;
                    txtCalcPinDiameter.Enabled = false;
                    txtCalcPinHeight.Enabled = false;
                    cboTopHolePos.Enabled = false;
                    txtTopDirHoleDistance.Enabled = false;
                }
            }
            else if (SelSettingIndex == 1)
            {
                txtFixAngleAddGap.Enabled = false;
                txtMoveAngleAddGap.Enabled = true;
                txtBackAngleAddGap.Enabled = false;
                txtTopAngleAddGap.Enabled = false;
                txtPlangeThickness.Enabled = true;

                txtPinHoleDistance.Enabled = false;
                txtPinHoleDiameter.Enabled = false;
                txtCalcPinDiameter.Enabled = false;
                txtCalcPinHeight.Enabled = false;
                cboTopHolePos.Enabled = false;
                txtTopDirHoleDistance.Enabled = false;
            }
            else if (SelSettingIndex == 2)
            {
                txtFixAngleAddGap.Enabled = false;
                txtMoveAngleAddGap.Enabled = false;
                txtBackAngleAddGap.Enabled = true;
                txtTopAngleAddGap.Enabled = false;
                txtPlangeThickness.Enabled = true;

                txtPinHoleDistance.Enabled = false;
                txtPinHoleDiameter.Enabled = false;
                txtCalcPinDiameter.Enabled = false;
                txtCalcPinHeight.Enabled = false;
                cboTopHolePos.Enabled = false;
                txtTopDirHoleDistance.Enabled = false;
            }
            else if (SelSettingIndex == 3)
            {
                txtFixAngleAddGap.Enabled = false;
                txtMoveAngleAddGap.Enabled = false;
                txtBackAngleAddGap.Enabled = false;
                txtTopAngleAddGap.Enabled = true;
                txtPlangeThickness.Enabled = true;

                txtPinHoleDistance.Enabled = true;
                txtPinHoleDiameter.Enabled = true;
                txtCalcPinDiameter.Enabled = true;
                txtCalcPinHeight.Enabled = true;
                cboTopHolePos.Enabled = true;
                txtTopDirHoleDistance.Enabled = true;
            }
            else
            {
                txtFixAngleAddGap.Enabled = false;
                txtMoveAngleAddGap.Enabled = false;
                txtBackAngleAddGap.Enabled = false;
                txtTopAngleAddGap.Enabled = false;
                txtPlangeThickness.Enabled = true;

                txtPinHoleDistance.Enabled = false;
                txtPinHoleDiameter.Enabled = false;
                txtCalcPinDiameter.Enabled = false;
                txtCalcPinHeight.Enabled = false;
                cboTopHolePos.Enabled = false;
                txtTopDirHoleDistance.Enabled = false;
            }
        }

        private void DisplayGapAngleAndThickness()
        {
            txtFixAngleAddGap.Text = RecoSetData.Move_R_FIX_Angle_Gap.ToString("##.0000");
            txtMoveAngleAddGap.Text = RecoSetData.Move_R_MOVE_Angle_Gap.ToString("##.0000");
            txtBackAngleAddGap.Text = RecoSetData.Back_R_MOVE_Angle_Gap.ToString("##.0000");
            txtTopAngleAddGap.Text = RecoSetData.Top_R_MOVE_Angle_Gap.ToString("##.0000");
            txtPlangeThickness.Text = RecoSetData.Object_Weight.ToString("##.0000");

            txtPinHoleDistance.Text = RecoSetData.Object_Hole_Distance.ToString("##.0000");
            txtPinHoleDiameter.Text = RecoSetData.Object_Diameter.ToString("##.0000");
            txtCalcPinDiameter.Text = RecoSetData.Object_Pin_Diameter.ToString("##.0000");
            txtCalcPinHeight.Text = RecoSetData.Object_Pin_Height.ToString("##.0000");

            if(RecoSetData.Object_TopHoleType>-1)
            {
                cboTopHolePos.SelectedIndex = RecoSetData.Object_TopHoleType;
            }

            txtTopDirHoleDistance.Text = RecoSetData.Object_TopDirHoleDistance.ToString("##.0000");
            txtShaftLength.Text = RecoSetData.Object_ShaftLength.ToString("##.0000");

            if (RecoSetData.DXF_RecoType > -1)
            {
                cboDXFType.SelectedIndex = RecoSetData.DXF_RecoType;
            }

        }

        private void btUp_Click(object sender, EventArgs e)
        {
            
        }



        private ShotDetailInfo GetDXFShotZoomFocus(int GetCamShotIndex, double DXFObjLength)
        {
            ShotDetailInfo ResDXFZoomFocus;
            int CmpObjLength;
            int CmpObjStartNum, CmpObjOtherNum;

            ResDXFZoomFocus.ShotType = 0;
            ResDXFZoomFocus.ShotGapDistance = 0;
            ResDXFZoomFocus.ZoomNum = 0;
            ResDXFZoomFocus.FocusNum = 0;

            CmpObjLength = (int)(DXFObjLength * 10.0);

            CmpObjStartNum = CmpObjLength / 100;
            CmpObjOtherNum = CmpObjLength % 100;

            if (GetCamShotIndex == 0)
            {
                ResDXFZoomFocus.ShotType = DxfShotDataList_A[CmpObjStartNum].ShotType;
                ResDXFZoomFocus.ShotGapDistance = DxfShotDataList_A[CmpObjStartNum].ShotGapDistance;
                ResDXFZoomFocus.ZoomNum = DxfShotDataList_A[CmpObjStartNum].ZoomNum - CmpObjOtherNum * (DxfShotDataList_A[CmpObjStartNum].ZoomNum - DxfShotDataList_A[CmpObjStartNum + 1].ZoomNum) / 100;
                ResDXFZoomFocus.FocusNum = DxfShotDataList_A[CmpObjStartNum].FocusNum;
            }
            else if (GetCamShotIndex == 1)
            {
                ResDXFZoomFocus.ShotType = DxfShotDataList_B[CmpObjStartNum].ShotType;
                ResDXFZoomFocus.ShotGapDistance = DxfShotDataList_B[CmpObjStartNum].ShotGapDistance;
                ResDXFZoomFocus.ZoomNum = DxfShotDataList_B[CmpObjStartNum].ZoomNum - CmpObjOtherNum * (DxfShotDataList_B[CmpObjStartNum].ZoomNum - DxfShotDataList_B[CmpObjStartNum + 1].ZoomNum) / 100;
                ResDXFZoomFocus.FocusNum = DxfShotDataList_B[CmpObjStartNum].FocusNum;
            }


            return ResDXFZoomFocus;
        }

        private void SetDXFShotDataInit()
        {
            int i;
            int i_gap1, i_gap2, i_gap;

            //==================================
            // A
            //==================================

            int i_gap_Center;

            for (int j = 0; j < 19; j++)
            {
                i_gap_Center = (130 - 40) / 2 + 40;
                i_gap1 = (i_gap_Center - j * 10);
                i_gap2 = i_gap_Center - 40;
                if (i_gap1 < 0)
                {
                    i_gap1 = 0 - i_gap1;
                }
                i_gap = i_gap1 - i_gap2;
                DxfShotDataList_A[j].ZoomNum = 10939 - (j * 10 - 40) * (10939 - 4907) / (130 - 40);// +i_gap * 328 / 10;
                DxfShotDataList_A[j].FocusNum = 19584;
            }

            /*
            i = 0;
            DxfShotDataList_A[i].ShotType = 0;
            DxfShotDataList_A[i].ShotGapDistance = 0.0;
            DxfShotDataList_A[i].ZoomNum = 0;
            DxfShotDataList_A[i].FocusNum = 19678;
            i = 1;
            DxfShotDataList_A[i].ShotType = 0;
            DxfShotDataList_A[i].ShotGapDistance = 0.0;
            DxfShotDataList_A[i].ZoomNum = 0;
            DxfShotDataList_A[i].FocusNum = 19678;
            i = 2;
            DxfShotDataList_A[i].ShotType = 0;
            DxfShotDataList_A[i].ShotGapDistance = 0.0;
            DxfShotDataList_A[i].ZoomNum = 0;
            DxfShotDataList_A[i].FocusNum = 19678;
            i = 3;
            DxfShotDataList_A[i].ShotType = 0;
            DxfShotDataList_A[i].ShotGapDistance = 0.0;
            DxfShotDataList_A[i].ZoomNum = 0;
            DxfShotDataList_A[i].FocusNum = 19678;
            */
            i = 4;
            DxfShotDataList_A[i].ShotType = 0;
            DxfShotDataList_A[i].ShotGapDistance = 0.0;
            DxfShotDataList_A[i].ZoomNum = 10939;
            //DxfShotDataList_A[i].FocusNum = 19678;
            i = 5;
            DxfShotDataList_A[i].ShotType = 0;
            DxfShotDataList_A[i].ShotGapDistance = 0.0;
            DxfShotDataList_A[i].ZoomNum = 9797;
            //DxfShotDataList_A[i].FocusNum = 19678;
            i = 6;
            DxfShotDataList_A[i].ShotType = 0;
            DxfShotDataList_A[i].ShotGapDistance = 0.0;
            DxfShotDataList_A[i].ZoomNum = 8958;
            //DxfShotDataList_A[i].FocusNum = 19678;
            i = 7;
            DxfShotDataList_A[i].ShotType = 0;
            DxfShotDataList_A[i].ShotGapDistance = 0.0;
            DxfShotDataList_A[i].ZoomNum = 8208;
            //DxfShotDataList_A[i].FocusNum = 19678;
            i = 8;
            DxfShotDataList_A[i].ShotType = 0;
            DxfShotDataList_A[i].ShotGapDistance = 0.0;
            DxfShotDataList_A[i].ZoomNum = 7602;
            //DxfShotDataList_A[i].FocusNum = 19678;
            i = 9;
            DxfShotDataList_A[i].ShotType = 0;
            DxfShotDataList_A[i].ShotGapDistance = 0.0;
            DxfShotDataList_A[i].ZoomNum = 7013;
            //DxfShotDataList_A[i].FocusNum = 19678;
            i = 10;
            DxfShotDataList_A[i].ShotType = 0;
            DxfShotDataList_A[i].ShotGapDistance = 0.0;
            DxfShotDataList_A[i].ZoomNum = 6460;
            //DxfShotDataList_A[i].FocusNum = 19678;
            i = 11;
            DxfShotDataList_A[i].ShotType = 0;
            DxfShotDataList_A[i].ShotGapDistance = 0.0;
            DxfShotDataList_A[i].ZoomNum = 5924;
            //DxfShotDataList_A[i].FocusNum = 19678;
            i = 12;
            DxfShotDataList_A[i].ShotType = 0;
            DxfShotDataList_A[i].ShotGapDistance = 0.0;
            DxfShotDataList_A[i].ZoomNum = 5425;
            //DxfShotDataList_A[i].FocusNum = 19678;
            i = 13;
            DxfShotDataList_A[i].ShotType = 0;
            DxfShotDataList_A[i].ShotGapDistance = 0.0;
            DxfShotDataList_A[i].ZoomNum = 4907;
            //DxfShotDataList_A[i].FocusNum = 19678;
            i = 14;
            DxfShotDataList_A[i].ShotType = 0;
            DxfShotDataList_A[i].ShotGapDistance = 0.0;
            DxfShotDataList_A[i].ZoomNum = 4386;
            //DxfShotDataList_A[i].FocusNum = 19678;
            i = 15;
            DxfShotDataList_A[i].ShotType = 0;
            DxfShotDataList_A[i].ShotGapDistance = 0.0;
            DxfShotDataList_A[i].ZoomNum = 3852;
            //DxfShotDataList_A[i].FocusNum = 19678;
            i = 16;
            DxfShotDataList_A[i].ShotType = 0;
            DxfShotDataList_A[i].ShotGapDistance = 0.0;
            DxfShotDataList_A[i].ZoomNum = 0;
            //DxfShotDataList_A[i].FocusNum = 19678;
            i = 17;
            DxfShotDataList_A[i].ShotType = 0;
            DxfShotDataList_A[i].ShotGapDistance = 0.0;
            DxfShotDataList_A[i].ZoomNum = 0;
            //DxfShotDataList_A[i].FocusNum = 19678;
            i = 18;
            DxfShotDataList_A[i].ShotType = 0;
            DxfShotDataList_A[i].ShotGapDistance = 0.0;
            DxfShotDataList_A[i].ZoomNum = 0;
            //DxfShotDataList_A[i].FocusNum = 19678;

            //==================================
            // B
            //==================================
            for (int j = 0; j < 19; j++)
            {
                DxfShotDataList_B[j].ZoomNum = 10403 - (j * 10 - 40) * (10403 - 3692) / (130 - 40);
                DxfShotDataList_B[j].FocusNum = 19081;
            }

            /*
            i = 0;
            DxfShotDataList_B[i].ShotType = 0;
            DxfShotDataList_B[i].ShotGapDistance = 0.0;
            DxfShotDataList_B[i].ZoomNum = 0;
            DxfShotDataList_B[i].FocusNum = 0;
            i = 1;
            DxfShotDataList_B[i].ShotType = 0;
            DxfShotDataList_B[i].ShotGapDistance = 0.0;
            DxfShotDataList_B[i].ZoomNum = 0;
            DxfShotDataList_B[i].FocusNum = 0;
            i = 2;
            DxfShotDataList_B[i].ShotType = 0;
            DxfShotDataList_B[i].ShotGapDistance = 0.0;
            DxfShotDataList_B[i].ZoomNum = 0;
            DxfShotDataList_B[i].FocusNum = 0;
            i = 3;
            DxfShotDataList_B[i].ShotType = 0;
            DxfShotDataList_B[i].ShotGapDistance = 0.0;
            DxfShotDataList_B[i].ZoomNum = 0;
            DxfShotDataList_B[i].FocusNum = 0;
            */
            i = 4;
            DxfShotDataList_B[i].ShotType = 0;
            DxfShotDataList_B[i].ShotGapDistance = 0.0;
            DxfShotDataList_B[i].ZoomNum = 10405;
            DxfShotDataList_B[i].FocusNum = 18995;
            i = 5;
            DxfShotDataList_B[i].ShotType = 0;
            DxfShotDataList_B[i].ShotGapDistance = 0.0;
            DxfShotDataList_B[i].ZoomNum = 9322;
            DxfShotDataList_B[i].FocusNum = 18995;
            i = 6;
            DxfShotDataList_B[i].ShotType = 0;
            DxfShotDataList_B[i].ShotGapDistance = 0.0;
            DxfShotDataList_B[i].ZoomNum = 8452;
            DxfShotDataList_B[i].FocusNum = 18995;
            i = 7;
            DxfShotDataList_B[i].ShotType = 0;
            DxfShotDataList_B[i].ShotGapDistance = 0.0;
            DxfShotDataList_B[i].ZoomNum = 7706;
            DxfShotDataList_B[i].FocusNum = 18995;
            i = 8;
            DxfShotDataList_B[i].ShotType = 0;
            DxfShotDataList_B[i].ShotGapDistance = 0.0;
            DxfShotDataList_B[i].ZoomNum = 7049;
            DxfShotDataList_B[i].FocusNum = 18995;
            i = 9;
            DxfShotDataList_B[i].ShotType = 0;
            DxfShotDataList_B[i].ShotGapDistance = 0.0;
            DxfShotDataList_B[i].ZoomNum = 6446;
            DxfShotDataList_B[i].FocusNum = 18995;
            i = 10;
            DxfShotDataList_B[i].ShotType = 0;
            DxfShotDataList_B[i].ShotGapDistance = 0.0;
            DxfShotDataList_B[i].ZoomNum = 5860;
            DxfShotDataList_B[i].FocusNum = 18995;
            i = 11;
            DxfShotDataList_B[i].ShotType = 0;
            DxfShotDataList_B[i].ShotGapDistance = 0.0;
            DxfShotDataList_B[i].ZoomNum = 5292;
            DxfShotDataList_B[i].FocusNum = 18995;
            i = 12;
            DxfShotDataList_B[i].ShotType = 0;
            DxfShotDataList_B[i].ShotGapDistance = 0.0;
            DxfShotDataList_B[i].ZoomNum = 4741;
            DxfShotDataList_B[i].FocusNum = 18995;
            i = 13;
            DxfShotDataList_B[i].ShotType = 0;
            DxfShotDataList_B[i].ShotGapDistance = 0.0;
            DxfShotDataList_B[i].ZoomNum = 4191;
            DxfShotDataList_B[i].FocusNum = 18995;
            /*
            i = 14;
            DxfShotDataList_B[i].ShotType = 0;
            DxfShotDataList_B[i].ShotGapDistance = 0.0;
            DxfShotDataList_B[i].ZoomNum = 0;
            DxfShotDataList_B[i].FocusNum = 18995;
            i = 15;
            DxfShotDataList_B[i].ShotType = 0;
            DxfShotDataList_B[i].ShotGapDistance = 0.0;
            DxfShotDataList_B[i].ZoomNum = 0;
            DxfShotDataList_B[i].FocusNum = 18995;
            i = 16;
            DxfShotDataList_B[i].ShotType = 0;
            DxfShotDataList_B[i].ShotGapDistance = 0.0;
            DxfShotDataList_B[i].ZoomNum = 0;
            DxfShotDataList_B[i].FocusNum = 18995;
            i = 17;
            DxfShotDataList_B[i].ShotType = 0;
            DxfShotDataList_B[i].ShotGapDistance = 0.0;
            DxfShotDataList_B[i].ZoomNum = 0;
            DxfShotDataList_B[i].FocusNum = 18995;
            i = 18;
            DxfShotDataList_B[i].ShotType = 0;
            DxfShotDataList_B[i].ShotGapDistance = 0.0;
            DxfShotDataList_B[i].ZoomNum = 0;
            DxfShotDataList_B[i].FocusNum = 18995;
            */

            //==================================
            // C
            //==================================
            i = 0;
            DxfShotDataList_C[i].ShotType = 0;
            DxfShotDataList_C[i].ShotGapDistance = 0.0;
            DxfShotDataList_C[i].ZoomNum = 0;
            DxfShotDataList_C[i].FocusNum = 0;
            i = 1;
            DxfShotDataList_C[i].ShotType = 0;
            DxfShotDataList_C[i].ShotGapDistance = 0.0;
            DxfShotDataList_C[i].ZoomNum = 0;
            DxfShotDataList_C[i].FocusNum = 0;
            i = 2;
            DxfShotDataList_C[i].ShotType = 0;
            DxfShotDataList_C[i].ShotGapDistance = 0.0;
            DxfShotDataList_C[i].ZoomNum = 0;
            DxfShotDataList_C[i].FocusNum = 0;
            i = 3;
            DxfShotDataList_C[i].ShotType = 0;
            DxfShotDataList_C[i].ShotGapDistance = 0.0;
            DxfShotDataList_C[i].ZoomNum = 0;
            DxfShotDataList_C[i].FocusNum = 0;
            i = 4;
            DxfShotDataList_C[i].ShotType = 0;
            DxfShotDataList_C[i].ShotGapDistance = 0.0;
            DxfShotDataList_C[i].ZoomNum = 0;
            DxfShotDataList_C[i].FocusNum = 0;
            i = 5;
            DxfShotDataList_C[i].ShotType = 0;
            DxfShotDataList_C[i].ShotGapDistance = 0.0;
            DxfShotDataList_C[i].ZoomNum = 0;
            DxfShotDataList_C[i].FocusNum = 0;
            i = 6;
            DxfShotDataList_C[i].ShotType = 0;
            DxfShotDataList_C[i].ShotGapDistance = 0.0;
            DxfShotDataList_C[i].ZoomNum = 0;
            DxfShotDataList_C[i].FocusNum = 0;
            i = 7;
            DxfShotDataList_C[i].ShotType = 0;
            DxfShotDataList_C[i].ShotGapDistance = 0.0;
            DxfShotDataList_C[i].ZoomNum = 0;
            DxfShotDataList_C[i].FocusNum = 0;
            i = 8;
            DxfShotDataList_C[i].ShotType = 0;
            DxfShotDataList_C[i].ShotGapDistance = 0.0;
            DxfShotDataList_C[i].ZoomNum = 0;
            DxfShotDataList_C[i].FocusNum = 0;
            i = 9;
            DxfShotDataList_C[i].ShotType = 0;
            DxfShotDataList_C[i].ShotGapDistance = 0.0;
            DxfShotDataList_C[i].ZoomNum = 0;
            DxfShotDataList_C[i].FocusNum = 0;
            i = 10;
            DxfShotDataList_C[i].ShotType = 0;
            DxfShotDataList_C[i].ShotGapDistance = 0.0;
            DxfShotDataList_C[i].ZoomNum = 0;
            DxfShotDataList_C[i].FocusNum = 0;
            i = 11;
            DxfShotDataList_C[i].ShotType = 0;
            DxfShotDataList_C[i].ShotGapDistance = 0.0;
            DxfShotDataList_C[i].ZoomNum = 0;
            DxfShotDataList_C[i].FocusNum = 0;
            i = 12;
            DxfShotDataList_C[i].ShotType = 0;
            DxfShotDataList_C[i].ShotGapDistance = 0.0;
            DxfShotDataList_C[i].ZoomNum = 0;
            DxfShotDataList_C[i].FocusNum = 0;
            i = 13;
            DxfShotDataList_C[i].ShotType = 0;
            DxfShotDataList_C[i].ShotGapDistance = 0.0;
            DxfShotDataList_C[i].ZoomNum = 0;
            DxfShotDataList_C[i].FocusNum = 0;
            i = 14;
            DxfShotDataList_C[i].ShotType = 0;
            DxfShotDataList_C[i].ShotGapDistance = 0.0;
            DxfShotDataList_C[i].ZoomNum = 0;
            DxfShotDataList_C[i].FocusNum = 0;
            i = 15;
            DxfShotDataList_C[i].ShotType = 0;
            DxfShotDataList_C[i].ShotGapDistance = 0.0;
            DxfShotDataList_C[i].ZoomNum = 0;
            DxfShotDataList_C[i].FocusNum = 0;
            i = 16;
            DxfShotDataList_C[i].ShotType = 0;
            DxfShotDataList_C[i].ShotGapDistance = 0.0;
            DxfShotDataList_C[i].ZoomNum = 0;
            DxfShotDataList_C[i].FocusNum = 0;
            i = 17;
            DxfShotDataList_C[i].ShotType = 0;
            DxfShotDataList_C[i].ShotGapDistance = 0.0;
            DxfShotDataList_C[i].ZoomNum = 0;
            DxfShotDataList_C[i].FocusNum = 0;
            i = 18;
            DxfShotDataList_C[i].ShotType = 0;
            DxfShotDataList_C[i].ShotGapDistance = 0.0;
            DxfShotDataList_C[i].ZoomNum = 0;
            DxfShotDataList_C[i].FocusNum = 0;


            //==================================
            // D
            //==================================
            i = 0;
            DxfShotDataList_D[i].ShotType = 0;
            DxfShotDataList_D[i].ShotGapDistance = 0.0;
            DxfShotDataList_D[i].ZoomNum = 0;
            DxfShotDataList_D[i].FocusNum = 0;
            i = 1;
            DxfShotDataList_D[i].ShotType = 0;
            DxfShotDataList_D[i].ShotGapDistance = 0.0;
            DxfShotDataList_D[i].ZoomNum = 0;
            DxfShotDataList_D[i].FocusNum = 0;
            i = 2;
            DxfShotDataList_D[i].ShotType = 0;
            DxfShotDataList_D[i].ShotGapDistance = 0.0;
            DxfShotDataList_D[i].ZoomNum = 0;
            DxfShotDataList_D[i].FocusNum = 0;
            i = 3;
            DxfShotDataList_D[i].ShotType = 0;
            DxfShotDataList_D[i].ShotGapDistance = 0.0;
            DxfShotDataList_D[i].ZoomNum = 0;
            DxfShotDataList_D[i].FocusNum = 0;
            i = 4;
            DxfShotDataList_D[i].ShotType = 0;
            DxfShotDataList_D[i].ShotGapDistance = 0.0;
            DxfShotDataList_D[i].ZoomNum = 0;
            DxfShotDataList_D[i].FocusNum = 0;
            i = 5;
            DxfShotDataList_D[i].ShotType = 0;
            DxfShotDataList_D[i].ShotGapDistance = 0.0;
            DxfShotDataList_D[i].ZoomNum = 0;
            DxfShotDataList_D[i].FocusNum = 0;
            i = 6;
            DxfShotDataList_D[i].ShotType = 0;
            DxfShotDataList_D[i].ShotGapDistance = 0.0;
            DxfShotDataList_D[i].ZoomNum = 0;
            DxfShotDataList_D[i].FocusNum = 0;
            i = 7;
            DxfShotDataList_D[i].ShotType = 0;
            DxfShotDataList_D[i].ShotGapDistance = 0.0;
            DxfShotDataList_D[i].ZoomNum = 0;
            DxfShotDataList_D[i].FocusNum = 0;
            i = 8;
            DxfShotDataList_D[i].ShotType = 0;
            DxfShotDataList_D[i].ShotGapDistance = 0.0;
            DxfShotDataList_D[i].ZoomNum = 0;
            DxfShotDataList_D[i].FocusNum = 0;
            i = 9;
            DxfShotDataList_D[i].ShotType = 0;
            DxfShotDataList_D[i].ShotGapDistance = 0.0;
            DxfShotDataList_D[i].ZoomNum = 0;
            DxfShotDataList_D[i].FocusNum = 0;
            i = 10;
            DxfShotDataList_D[i].ShotType = 0;
            DxfShotDataList_D[i].ShotGapDistance = 0.0;
            DxfShotDataList_D[i].ZoomNum = 0;
            DxfShotDataList_D[i].FocusNum = 0;
            i = 11;
            DxfShotDataList_D[i].ShotType = 0;
            DxfShotDataList_D[i].ShotGapDistance = 0.0;
            DxfShotDataList_D[i].ZoomNum = 0;
            DxfShotDataList_D[i].FocusNum = 0;
            i = 12;
            DxfShotDataList_D[i].ShotType = 0;
            DxfShotDataList_D[i].ShotGapDistance = 0.0;
            DxfShotDataList_D[i].ZoomNum = 0;
            DxfShotDataList_D[i].FocusNum = 0;
            i = 13;
            DxfShotDataList_D[i].ShotType = 0;
            DxfShotDataList_D[i].ShotGapDistance = 0.0;
            DxfShotDataList_D[i].ZoomNum = 0;
            DxfShotDataList_D[i].FocusNum = 0;
            i = 14;
            DxfShotDataList_D[i].ShotType = 0;
            DxfShotDataList_D[i].ShotGapDistance = 0.0;
            DxfShotDataList_D[i].ZoomNum = 0;
            DxfShotDataList_D[i].FocusNum = 0;
            i = 15;
            DxfShotDataList_D[i].ShotType = 0;
            DxfShotDataList_D[i].ShotGapDistance = 0.0;
            DxfShotDataList_D[i].ZoomNum = 0;
            DxfShotDataList_D[i].FocusNum = 0;
            i = 16;
            DxfShotDataList_D[i].ShotType = 0;
            DxfShotDataList_D[i].ShotGapDistance = 0.0;
            DxfShotDataList_D[i].ZoomNum = 0;
            DxfShotDataList_D[i].FocusNum = 0;
            i = 17;
            DxfShotDataList_D[i].ShotType = 0;
            DxfShotDataList_D[i].ShotGapDistance = 0.0;
            DxfShotDataList_D[i].ZoomNum = 0;
            DxfShotDataList_D[i].FocusNum = 0;
            i = 18;
            DxfShotDataList_D[i].ShotType = 0;
            DxfShotDataList_D[i].ShotGapDistance = 0.0;
            DxfShotDataList_D[i].ZoomNum = 0;
            DxfShotDataList_D[i].FocusNum = 0;

            //==================================
            // E
            //==================================
            i = 0;
            DxfShotDataList_E[i].ShotType = 0;
            DxfShotDataList_E[i].ShotGapDistance = 0.0;
            DxfShotDataList_E[i].ZoomNum = 0;
            DxfShotDataList_E[i].FocusNum = 0;
            i = 1;
            DxfShotDataList_E[i].ShotType = 0;
            DxfShotDataList_E[i].ShotGapDistance = 0.0;
            DxfShotDataList_E[i].ZoomNum = 0;
            DxfShotDataList_E[i].FocusNum = 0;
            i = 2;
            DxfShotDataList_E[i].ShotType = 0;
            DxfShotDataList_E[i].ShotGapDistance = 0.0;
            DxfShotDataList_E[i].ZoomNum = 0;
            DxfShotDataList_E[i].FocusNum = 0;
            i = 3;
            DxfShotDataList_E[i].ShotType = 0;
            DxfShotDataList_E[i].ShotGapDistance = 0.0;
            DxfShotDataList_E[i].ZoomNum = 0;
            DxfShotDataList_E[i].FocusNum = 0;
            i = 4;
            DxfShotDataList_E[i].ShotType = 0;
            DxfShotDataList_E[i].ShotGapDistance = 0.0;
            DxfShotDataList_E[i].ZoomNum = 0;
            DxfShotDataList_E[i].FocusNum = 0;
            i = 5;
            DxfShotDataList_E[i].ShotType = 0;
            DxfShotDataList_E[i].ShotGapDistance = 0.0;
            DxfShotDataList_E[i].ZoomNum = 0;
            DxfShotDataList_E[i].FocusNum = 0;
            i = 6;
            DxfShotDataList_E[i].ShotType = 0;
            DxfShotDataList_E[i].ShotGapDistance = 0.0;
            DxfShotDataList_E[i].ZoomNum = 0;
            DxfShotDataList_E[i].FocusNum = 0;
            i = 7;
            DxfShotDataList_E[i].ShotType = 0;
            DxfShotDataList_E[i].ShotGapDistance = 0.0;
            DxfShotDataList_E[i].ZoomNum = 0;
            DxfShotDataList_E[i].FocusNum = 0;
            i = 8;
            DxfShotDataList_E[i].ShotType = 0;
            DxfShotDataList_E[i].ShotGapDistance = 0.0;
            DxfShotDataList_E[i].ZoomNum = 0;
            DxfShotDataList_E[i].FocusNum = 0;
            i = 9;
            DxfShotDataList_E[i].ShotType = 0;
            DxfShotDataList_E[i].ShotGapDistance = 0.0;
            DxfShotDataList_E[i].ZoomNum = 0;
            DxfShotDataList_E[i].FocusNum = 0;
            i = 10;
            DxfShotDataList_E[i].ShotType = 0;
            DxfShotDataList_E[i].ShotGapDistance = 0.0;
            DxfShotDataList_E[i].ZoomNum = 0;
            DxfShotDataList_E[i].FocusNum = 0;
            i = 11;
            DxfShotDataList_E[i].ShotType = 0;
            DxfShotDataList_E[i].ShotGapDistance = 0.0;
            DxfShotDataList_E[i].ZoomNum = 0;
            DxfShotDataList_E[i].FocusNum = 0;
            i = 12;
            DxfShotDataList_E[i].ShotType = 0;
            DxfShotDataList_E[i].ShotGapDistance = 0.0;
            DxfShotDataList_E[i].ZoomNum = 0;
            DxfShotDataList_E[i].FocusNum = 0;
            i = 13;
            DxfShotDataList_E[i].ShotType = 0;
            DxfShotDataList_E[i].ShotGapDistance = 0.0;
            DxfShotDataList_E[i].ZoomNum = 0;
            DxfShotDataList_E[i].FocusNum = 0;
            i = 14;
            DxfShotDataList_E[i].ShotType = 0;
            DxfShotDataList_E[i].ShotGapDistance = 0.0;
            DxfShotDataList_E[i].ZoomNum = 0;
            DxfShotDataList_E[i].FocusNum = 0;
            i = 15;
            DxfShotDataList_E[i].ShotType = 0;
            DxfShotDataList_E[i].ShotGapDistance = 0.0;
            DxfShotDataList_E[i].ZoomNum = 0;
            DxfShotDataList_E[i].FocusNum = 0;
            i = 16;
            DxfShotDataList_E[i].ShotType = 0;
            DxfShotDataList_E[i].ShotGapDistance = 0.0;
            DxfShotDataList_E[i].ZoomNum = 0;
            DxfShotDataList_E[i].FocusNum = 0;
            i = 17;
            DxfShotDataList_E[i].ShotType = 0;
            DxfShotDataList_E[i].ShotGapDistance = 0.0;
            DxfShotDataList_E[i].ZoomNum = 0;
            DxfShotDataList_E[i].FocusNum = 0;
            i = 18;
            DxfShotDataList_E[i].ShotType = 0;
            DxfShotDataList_E[i].ShotGapDistance = 0.0;
            DxfShotDataList_E[i].ZoomNum = 0;
            DxfShotDataList_E[i].FocusNum = 0;

        }

        /*
        private void SetDXFShotDataInit()
        {
            int i;

            //==================================
            // A
            //==================================
            i = 0;
            DxfShotDataList_A[i].ShotType = 0;
            DxfShotDataList_A[i].ShotGapDistance = 0.0;
            DxfShotDataList_A[i].ZoomNum = 0;
            DxfShotDataList_A[i].FocusNum = 19678;
            i = 1;
            DxfShotDataList_A[i].ShotType = 0;
            DxfShotDataList_A[i].ShotGapDistance = 0.0;
            DxfShotDataList_A[i].ZoomNum = 0;
            DxfShotDataList_A[i].FocusNum = 19678;
            i = 2;
            DxfShotDataList_A[i].ShotType = 0;
            DxfShotDataList_A[i].ShotGapDistance = 0.0;
            DxfShotDataList_A[i].ZoomNum = 0;
            DxfShotDataList_A[i].FocusNum = 19678;
            i = 3;
            DxfShotDataList_A[i].ShotType = 0;
            DxfShotDataList_A[i].ShotGapDistance = 0.0;
            DxfShotDataList_A[i].ZoomNum = 0;
            DxfShotDataList_A[i].FocusNum = 19678;
            i = 4;
            DxfShotDataList_A[i].ShotType = 0;
            DxfShotDataList_A[i].ShotGapDistance = 0.0;
            DxfShotDataList_A[i].ZoomNum = 10928;
            DxfShotDataList_A[i].FocusNum = 19678;
            i = 5;
            DxfShotDataList_A[i].ShotType = 0;
            DxfShotDataList_A[i].ShotGapDistance = 0.0;
            DxfShotDataList_A[i].ZoomNum = 9841;
            DxfShotDataList_A[i].FocusNum = 19678;
            i = 6;
            DxfShotDataList_A[i].ShotType = 0;
            DxfShotDataList_A[i].ShotGapDistance = 0.0;
            DxfShotDataList_A[i].ZoomNum = 8975;
            DxfShotDataList_A[i].FocusNum = 19678;
            i = 7;
            DxfShotDataList_A[i].ShotType = 0;
            DxfShotDataList_A[i].ShotGapDistance = 0.0;
            DxfShotDataList_A[i].ZoomNum = 8219;
            DxfShotDataList_A[i].FocusNum = 19678;
            i = 8;
            DxfShotDataList_A[i].ShotType = 0;
            DxfShotDataList_A[i].ShotGapDistance = 0.0;
            DxfShotDataList_A[i].ZoomNum = 7574;
            DxfShotDataList_A[i].FocusNum = 19678;
            i = 9;
            DxfShotDataList_A[i].ShotType = 0;
            DxfShotDataList_A[i].ShotGapDistance = 0.0;
            DxfShotDataList_A[i].ZoomNum = 7003;
            DxfShotDataList_A[i].FocusNum = 19678;
            i = 10;
            DxfShotDataList_A[i].ShotType = 0;
            DxfShotDataList_A[i].ShotGapDistance = 0.0;
            DxfShotDataList_A[i].ZoomNum = 6450;
            DxfShotDataList_A[i].FocusNum = 19678;
            i = 11;
            DxfShotDataList_A[i].ShotType = 0;
            DxfShotDataList_A[i].ShotGapDistance = 0.0;
            DxfShotDataList_A[i].ZoomNum = 5934;
            DxfShotDataList_A[i].FocusNum = 19678;
            i = 12;
            DxfShotDataList_A[i].ShotType = 0;
            DxfShotDataList_A[i].ShotGapDistance = 0.0;
            DxfShotDataList_A[i].ZoomNum = 5400;
            DxfShotDataList_A[i].FocusNum = 19678;
            i = 13;
            DxfShotDataList_A[i].ShotType = 0;
            DxfShotDataList_A[i].ShotGapDistance = 0.0;
            DxfShotDataList_A[i].ZoomNum = 4902;
            DxfShotDataList_A[i].FocusNum = 19678;
            i = 14;
            DxfShotDataList_A[i].ShotType = 0;
            DxfShotDataList_A[i].ShotGapDistance = 0.0;
            DxfShotDataList_A[i].ZoomNum = 4386;
            DxfShotDataList_A[i].FocusNum = 19678;
            i = 15;
            DxfShotDataList_A[i].ShotType = 0;
            DxfShotDataList_A[i].ShotGapDistance = 0.0;
            DxfShotDataList_A[i].ZoomNum = 3852;
            DxfShotDataList_A[i].FocusNum = 19678;
            i = 16;
            DxfShotDataList_A[i].ShotType = 0;
            DxfShotDataList_A[i].ShotGapDistance = 0.0;
            DxfShotDataList_A[i].ZoomNum = 0;
            DxfShotDataList_A[i].FocusNum = 19678;
            i = 17;
            DxfShotDataList_A[i].ShotType = 0;
            DxfShotDataList_A[i].ShotGapDistance = 0.0;
            DxfShotDataList_A[i].ZoomNum = 0;
            DxfShotDataList_A[i].FocusNum = 19678;
            i = 18;
            DxfShotDataList_A[i].ShotType = 0;
            DxfShotDataList_A[i].ShotGapDistance = 0.0;
            DxfShotDataList_A[i].ZoomNum = 0;
            DxfShotDataList_A[i].FocusNum = 19678;

            //==================================
            // B
            //==================================
            i = 0;
            DxfShotDataList_B[i].ShotType = 0;
            DxfShotDataList_B[i].ShotGapDistance = 0.0;
            DxfShotDataList_B[i].ZoomNum = 0;
            DxfShotDataList_B[i].FocusNum = 0;
            i = 1;
            DxfShotDataList_B[i].ShotType = 0;
            DxfShotDataList_B[i].ShotGapDistance = 0.0;
            DxfShotDataList_B[i].ZoomNum = 0;
            DxfShotDataList_B[i].FocusNum = 0;
            i = 2;
            DxfShotDataList_B[i].ShotType = 0;
            DxfShotDataList_B[i].ShotGapDistance = 0.0;
            DxfShotDataList_B[i].ZoomNum = 0;
            DxfShotDataList_B[i].FocusNum = 0;
            i = 3;
            DxfShotDataList_B[i].ShotType = 0;
            DxfShotDataList_B[i].ShotGapDistance = 0.0;
            DxfShotDataList_B[i].ZoomNum = 0;
            DxfShotDataList_B[i].FocusNum = 0;
            i = 4;
            DxfShotDataList_B[i].ShotType = 0;
            DxfShotDataList_B[i].ShotGapDistance = 0.0;
            DxfShotDataList_B[i].ZoomNum = 0;
            DxfShotDataList_B[i].FocusNum = 0;
            i = 5;
            DxfShotDataList_B[i].ShotType = 0;
            DxfShotDataList_B[i].ShotGapDistance = 0.0;
            DxfShotDataList_B[i].ZoomNum = 0;
            DxfShotDataList_B[i].FocusNum = 0;
            i = 6;
            DxfShotDataList_B[i].ShotType = 0;
            DxfShotDataList_B[i].ShotGapDistance = 0.0;
            DxfShotDataList_B[i].ZoomNum = 0;
            DxfShotDataList_B[i].FocusNum = 0;
            i = 7;
            DxfShotDataList_B[i].ShotType = 0;
            DxfShotDataList_B[i].ShotGapDistance = 0.0;
            DxfShotDataList_B[i].ZoomNum = 0;
            DxfShotDataList_B[i].FocusNum = 0;
            i = 8;
            DxfShotDataList_B[i].ShotType = 0;
            DxfShotDataList_B[i].ShotGapDistance = 0.0;
            DxfShotDataList_B[i].ZoomNum = 0;
            DxfShotDataList_B[i].FocusNum = 0;
            i = 9;
            DxfShotDataList_B[i].ShotType = 0;
            DxfShotDataList_B[i].ShotGapDistance = 0.0;
            DxfShotDataList_B[i].ZoomNum = 0;
            DxfShotDataList_B[i].FocusNum = 0;
            i = 10;
            DxfShotDataList_B[i].ShotType = 0;
            DxfShotDataList_B[i].ShotGapDistance = 0.0;
            DxfShotDataList_B[i].ZoomNum = 0;
            DxfShotDataList_B[i].FocusNum = 0;
            i = 11;
            DxfShotDataList_B[i].ShotType = 0;
            DxfShotDataList_B[i].ShotGapDistance = 0.0;
            DxfShotDataList_B[i].ZoomNum = 0;
            DxfShotDataList_B[i].FocusNum = 0;
            i = 12;
            DxfShotDataList_B[i].ShotType = 0;
            DxfShotDataList_B[i].ShotGapDistance = 0.0;
            DxfShotDataList_B[i].ZoomNum = 0;
            DxfShotDataList_B[i].FocusNum = 0;
            i = 13;
            DxfShotDataList_B[i].ShotType = 0;
            DxfShotDataList_B[i].ShotGapDistance = 0.0;
            DxfShotDataList_B[i].ZoomNum = 0;
            DxfShotDataList_B[i].FocusNum = 0;
            i = 14;
            DxfShotDataList_B[i].ShotType = 0;
            DxfShotDataList_B[i].ShotGapDistance = 0.0;
            DxfShotDataList_B[i].ZoomNum = 0;
            DxfShotDataList_B[i].FocusNum = 0;
            i = 15;
            DxfShotDataList_B[i].ShotType = 0;
            DxfShotDataList_B[i].ShotGapDistance = 0.0;
            DxfShotDataList_B[i].ZoomNum = 0;
            DxfShotDataList_B[i].FocusNum = 0;
            i = 16;
            DxfShotDataList_B[i].ShotType = 0;
            DxfShotDataList_B[i].ShotGapDistance = 0.0;
            DxfShotDataList_B[i].ZoomNum = 0;
            DxfShotDataList_B[i].FocusNum = 0;
            i = 17;
            DxfShotDataList_B[i].ShotType = 0;
            DxfShotDataList_B[i].ShotGapDistance = 0.0;
            DxfShotDataList_B[i].ZoomNum = 0;
            DxfShotDataList_B[i].FocusNum = 0;
            i = 18;
            DxfShotDataList_B[i].ShotType = 0;
            DxfShotDataList_B[i].ShotGapDistance = 0.0;
            DxfShotDataList_B[i].ZoomNum = 0;
            DxfShotDataList_B[i].FocusNum = 0;

            //==================================
            // C
            //==================================
            i = 0;
            DxfShotDataList_C[i].ShotType = 0;
            DxfShotDataList_C[i].ShotGapDistance = 0.0;
            DxfShotDataList_C[i].ZoomNum = 0;
            DxfShotDataList_C[i].FocusNum = 0;
            i = 1;
            DxfShotDataList_C[i].ShotType = 0;
            DxfShotDataList_C[i].ShotGapDistance = 0.0;
            DxfShotDataList_C[i].ZoomNum = 0;
            DxfShotDataList_C[i].FocusNum = 0;
            i = 2;
            DxfShotDataList_C[i].ShotType = 0;
            DxfShotDataList_C[i].ShotGapDistance = 0.0;
            DxfShotDataList_C[i].ZoomNum = 0;
            DxfShotDataList_C[i].FocusNum = 0;
            i = 3;
            DxfShotDataList_C[i].ShotType = 0;
            DxfShotDataList_C[i].ShotGapDistance = 0.0;
            DxfShotDataList_C[i].ZoomNum = 0;
            DxfShotDataList_C[i].FocusNum = 0;
            i = 4;
            DxfShotDataList_C[i].ShotType = 0;
            DxfShotDataList_C[i].ShotGapDistance = 0.0;
            DxfShotDataList_C[i].ZoomNum = 0;
            DxfShotDataList_C[i].FocusNum = 0;
            i = 5;
            DxfShotDataList_C[i].ShotType = 0;
            DxfShotDataList_C[i].ShotGapDistance = 0.0;
            DxfShotDataList_C[i].ZoomNum = 0;
            DxfShotDataList_C[i].FocusNum = 0;
            i = 6;
            DxfShotDataList_C[i].ShotType = 0;
            DxfShotDataList_C[i].ShotGapDistance = 0.0;
            DxfShotDataList_C[i].ZoomNum = 0;
            DxfShotDataList_C[i].FocusNum = 0;
            i = 7;
            DxfShotDataList_C[i].ShotType = 0;
            DxfShotDataList_C[i].ShotGapDistance = 0.0;
            DxfShotDataList_C[i].ZoomNum = 0;
            DxfShotDataList_C[i].FocusNum = 0;
            i = 8;
            DxfShotDataList_C[i].ShotType = 0;
            DxfShotDataList_C[i].ShotGapDistance = 0.0;
            DxfShotDataList_C[i].ZoomNum = 0;
            DxfShotDataList_C[i].FocusNum = 0;
            i = 9;
            DxfShotDataList_C[i].ShotType = 0;
            DxfShotDataList_C[i].ShotGapDistance = 0.0;
            DxfShotDataList_C[i].ZoomNum = 0;
            DxfShotDataList_C[i].FocusNum = 0;
            i = 10;
            DxfShotDataList_C[i].ShotType = 0;
            DxfShotDataList_C[i].ShotGapDistance = 0.0;
            DxfShotDataList_C[i].ZoomNum = 0;
            DxfShotDataList_C[i].FocusNum = 0;
            i = 11;
            DxfShotDataList_C[i].ShotType = 0;
            DxfShotDataList_C[i].ShotGapDistance = 0.0;
            DxfShotDataList_C[i].ZoomNum = 0;
            DxfShotDataList_C[i].FocusNum = 0;
            i = 12;
            DxfShotDataList_C[i].ShotType = 0;
            DxfShotDataList_C[i].ShotGapDistance = 0.0;
            DxfShotDataList_C[i].ZoomNum = 0;
            DxfShotDataList_C[i].FocusNum = 0;
            i = 13;
            DxfShotDataList_C[i].ShotType = 0;
            DxfShotDataList_C[i].ShotGapDistance = 0.0;
            DxfShotDataList_C[i].ZoomNum = 0;
            DxfShotDataList_C[i].FocusNum = 0;
            i = 14;
            DxfShotDataList_C[i].ShotType = 0;
            DxfShotDataList_C[i].ShotGapDistance = 0.0;
            DxfShotDataList_C[i].ZoomNum = 0;
            DxfShotDataList_C[i].FocusNum = 0;
            i = 15;
            DxfShotDataList_C[i].ShotType = 0;
            DxfShotDataList_C[i].ShotGapDistance = 0.0;
            DxfShotDataList_C[i].ZoomNum = 0;
            DxfShotDataList_C[i].FocusNum = 0;
            i = 16;
            DxfShotDataList_C[i].ShotType = 0;
            DxfShotDataList_C[i].ShotGapDistance = 0.0;
            DxfShotDataList_C[i].ZoomNum = 0;
            DxfShotDataList_C[i].FocusNum = 0;
            i = 17;
            DxfShotDataList_C[i].ShotType = 0;
            DxfShotDataList_C[i].ShotGapDistance = 0.0;
            DxfShotDataList_C[i].ZoomNum = 0;
            DxfShotDataList_C[i].FocusNum = 0;
            i = 18;
            DxfShotDataList_C[i].ShotType = 0;
            DxfShotDataList_C[i].ShotGapDistance = 0.0;
            DxfShotDataList_C[i].ZoomNum = 0;
            DxfShotDataList_C[i].FocusNum = 0;


            //==================================
            // D
            //==================================
            i = 0;
            DxfShotDataList_D[i].ShotType = 0;
            DxfShotDataList_D[i].ShotGapDistance = 0.0;
            DxfShotDataList_D[i].ZoomNum = 0;
            DxfShotDataList_D[i].FocusNum = 0;
            i = 1;
            DxfShotDataList_D[i].ShotType = 0;
            DxfShotDataList_D[i].ShotGapDistance = 0.0;
            DxfShotDataList_D[i].ZoomNum = 0;
            DxfShotDataList_D[i].FocusNum = 0;
            i = 2;
            DxfShotDataList_D[i].ShotType = 0;
            DxfShotDataList_D[i].ShotGapDistance = 0.0;
            DxfShotDataList_D[i].ZoomNum = 0;
            DxfShotDataList_D[i].FocusNum = 0;
            i = 3;
            DxfShotDataList_D[i].ShotType = 0;
            DxfShotDataList_D[i].ShotGapDistance = 0.0;
            DxfShotDataList_D[i].ZoomNum = 0;
            DxfShotDataList_D[i].FocusNum = 0;
            i = 4;
            DxfShotDataList_D[i].ShotType = 0;
            DxfShotDataList_D[i].ShotGapDistance = 0.0;
            DxfShotDataList_D[i].ZoomNum = 0;
            DxfShotDataList_D[i].FocusNum = 0;
            i = 5;
            DxfShotDataList_D[i].ShotType = 0;
            DxfShotDataList_D[i].ShotGapDistance = 0.0;
            DxfShotDataList_D[i].ZoomNum = 0;
            DxfShotDataList_D[i].FocusNum = 0;
            i = 6;
            DxfShotDataList_D[i].ShotType = 0;
            DxfShotDataList_D[i].ShotGapDistance = 0.0;
            DxfShotDataList_D[i].ZoomNum = 0;
            DxfShotDataList_D[i].FocusNum = 0;
            i = 7;
            DxfShotDataList_D[i].ShotType = 0;
            DxfShotDataList_D[i].ShotGapDistance = 0.0;
            DxfShotDataList_D[i].ZoomNum = 0;
            DxfShotDataList_D[i].FocusNum = 0;
            i = 8;
            DxfShotDataList_D[i].ShotType = 0;
            DxfShotDataList_D[i].ShotGapDistance = 0.0;
            DxfShotDataList_D[i].ZoomNum = 0;
            DxfShotDataList_D[i].FocusNum = 0;
            i = 9;
            DxfShotDataList_D[i].ShotType = 0;
            DxfShotDataList_D[i].ShotGapDistance = 0.0;
            DxfShotDataList_D[i].ZoomNum = 0;
            DxfShotDataList_D[i].FocusNum = 0;
            i = 10;
            DxfShotDataList_D[i].ShotType = 0;
            DxfShotDataList_D[i].ShotGapDistance = 0.0;
            DxfShotDataList_D[i].ZoomNum = 0;
            DxfShotDataList_D[i].FocusNum = 0;
            i = 11;
            DxfShotDataList_D[i].ShotType = 0;
            DxfShotDataList_D[i].ShotGapDistance = 0.0;
            DxfShotDataList_D[i].ZoomNum = 0;
            DxfShotDataList_D[i].FocusNum = 0;
            i = 12;
            DxfShotDataList_D[i].ShotType = 0;
            DxfShotDataList_D[i].ShotGapDistance = 0.0;
            DxfShotDataList_D[i].ZoomNum = 0;
            DxfShotDataList_D[i].FocusNum = 0;
            i = 13;
            DxfShotDataList_D[i].ShotType = 0;
            DxfShotDataList_D[i].ShotGapDistance = 0.0;
            DxfShotDataList_D[i].ZoomNum = 0;
            DxfShotDataList_D[i].FocusNum = 0;
            i = 14;
            DxfShotDataList_D[i].ShotType = 0;
            DxfShotDataList_D[i].ShotGapDistance = 0.0;
            DxfShotDataList_D[i].ZoomNum = 0;
            DxfShotDataList_D[i].FocusNum = 0;
            i = 15;
            DxfShotDataList_D[i].ShotType = 0;
            DxfShotDataList_D[i].ShotGapDistance = 0.0;
            DxfShotDataList_D[i].ZoomNum = 0;
            DxfShotDataList_D[i].FocusNum = 0;
            i = 16;
            DxfShotDataList_D[i].ShotType = 0;
            DxfShotDataList_D[i].ShotGapDistance = 0.0;
            DxfShotDataList_D[i].ZoomNum = 0;
            DxfShotDataList_D[i].FocusNum = 0;
            i = 17;
            DxfShotDataList_D[i].ShotType = 0;
            DxfShotDataList_D[i].ShotGapDistance = 0.0;
            DxfShotDataList_D[i].ZoomNum = 0;
            DxfShotDataList_D[i].FocusNum = 0;
            i = 18;
            DxfShotDataList_D[i].ShotType = 0;
            DxfShotDataList_D[i].ShotGapDistance = 0.0;
            DxfShotDataList_D[i].ZoomNum = 0;
            DxfShotDataList_D[i].FocusNum = 0;

        }
        */


        private void btSetCenterPos_Click(object sender, EventArgs e)
        {
            short TmpMoveAxis_Index;
            double TmpMoveValueAxis;

            bool TopAlignProcFlag = false;
            bool BackAlignProcFlag = false;
            bool MoveBackAlignProcFlag = false;


            if (MessageBox.Show("본 정보를 저장할 경우 향후 모든 모델의 중심점값이 변경됩니다. 저장하시겠습니까?", "센터저장", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            if (cboZoomCamList.SelectedIndex == 0 && cboIndexList.SelectedIndex == 1)
            {
                MoveBackAlignProcFlag = true;
            }

            if (cboZoomCamList.SelectedIndex == 3)
            {
                TopAlignProcFlag = true;
            }

            if (cboZoomCamList.SelectedIndex == 2)
            {
                BackAlignProcFlag = true;
            }

            MultiMotion.GetCurrentPos();
            if (MoveBackAlignProcFlag == true)
            {
                TmpMoveAxis_Index = MultiMotion.CAM_UNIT_Y;
                TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
                RealCalibrationData_E.ddp_X = TmpMoveValueAxis;
                TmpMoveAxis_Index = MultiMotion.CAM_UNIT_Z;
                TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
                RealCalibrationData_E.ddp_Y = TmpMoveValueAxis;
            }
            else if (cboZoomCamList.SelectedIndex == 0)
            {
                TmpMoveAxis_Index = MultiMotion.CAM_UNIT_Y;
                TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
                RealCalibrationData_A.ddp_X = TmpMoveValueAxis;
                TmpMoveAxis_Index = MultiMotion.CAM_UNIT_Z;
                TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
                RealCalibrationData_A.ddp_Y = TmpMoveValueAxis;
            }
            if (cboZoomCamList.SelectedIndex == 1)
            {
                TmpMoveAxis_Index = MultiMotion.CAM_UNIT_Y;
                TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
                RealCalibrationData_B.ddp_X = TmpMoveValueAxis;
                TmpMoveAxis_Index = MultiMotion.CAM_UNIT_Z;
                TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
                RealCalibrationData_B.ddp_Y = TmpMoveValueAxis;
            }
            if (cboZoomCamList.SelectedIndex == 2)
            {
                TmpMoveAxis_Index = MultiMotion.BACK_CAM_Z;
                TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
                RealCalibrationData_C.ddp_X = TmpMoveValueAxis;
                TmpMoveAxis_Index = MultiMotion.BACK_CAM_Z;
                TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
                RealCalibrationData_C.ddp_Y = TmpMoveValueAxis;
            }
            if (cboZoomCamList.SelectedIndex == 3)
            {
                TmpMoveAxis_Index = MultiMotion.CAM_UNIT_Y;
                TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
                RealCalibrationData_D.ddp_X = TmpMoveValueAxis;
                TmpMoveAxis_Index = MultiMotion.CAM_UNIT_X;
                TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
                RealCalibrationData_D.ddp_Y = TmpMoveValueAxis;
            }
            SaveCalibSetFiles(SetCalibrationDataFileName);
            MessageBox.Show("센터값을 저장하였습니다.");
        }

        private void TopHoleTypeList()
        {
            cboTopHolePos.Items.Clear();
            cboTopHolePos.Items.Add("NONE");
            cboTopHolePos.Items.Add("LEFT");
            cboTopHolePos.Items.Add("RIGHT");
        }

        private void DXFTypeList()
        {
            cboDXFType.Items.Clear();
            cboDXFType.Items.Add("NONE");
            cboDXFType.Items.Add("홀 Type");
            cboDXFType.Items.Add("홀 & 단일직선 Type");
            cboDXFType.Items.Add("직선 Type");
            cboDXFType.SelectedIndex = 1;
        }

        private void txtXAxisValue_TextChanged(object sender, EventArgs e)
        {

        }

#region 속도 ...

        public RadioButton SelectedRB;
        public int AxisSpeed = 1;

        public void SelectSpeed(int speed)
        {
            radioBtnSSlow.Checked = false;
            radioBtnSlow.Checked = false;
            radioBtnMidium.Checked = false;
            radioBtnFast.Checked = false;


            // ----------
            switch (speed)
            {
                case MultiMotion.KSM_SPEED_SSLOW:
                    SelectedRB = radioBtnSSlow;
                    break;
                case MultiMotion.KSM_SPEED_SLOW:
                    SelectedRB = radioBtnSlow;
                    break;
                case MultiMotion.KSM_SPEED_MIDIUM:
                    SelectedRB = radioBtnMidium;
                    break;
                case MultiMotion.KSM_SPEED_FAST:
                    SelectedRB = radioBtnFast;
                    break;
            }
            // ----------


            SelectedRB.Checked = true;
        }

        private void radioBtn_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb == null)
            {
                return;
            }

            if (rb.Checked)
            {
                SelectedRB = rb;

                AxisSpeed = Convert.ToInt32(SelectedRB.Tag);

                MultiMotion.SetSpeed(AxisSpeed);
            }
        }

#endregion 속도 ...

        private void chkGuideLine_CheckedChanged(object sender, EventArgs e)
        {
            if(chkGuideLine.Checked == true)
            {
                GuidLineFlag = true;
            }
            else
            {
                GuidLineFlag = false;
            }
        }

        private void TopSelectViewLock()
        {
            cboZoomCamList.Enabled = false;
            cboIndexList.Enabled = false;
            cboMoveList.Enabled = false;
            cboLampList.Enabled = false;
        }

        private void chkDXFView_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDXFView.Checked == true)
            {
                ViewDXFCadFlag = true;
            }
            else
            {
                ViewDXFCadFlag = false;
            }
            
        }

        private DoubleDataPosInfo GetFixDataPos(DoubleDataPosInfo GetFixRefDataPos)
        {
            DoubleDataPosInfo CalcResYZPos;
            double Calc_X1, Calc_Y1;
            double GetCmpAngle;
            CalcResYZPos.ddp_ExistFlag = true;
            CalcResYZPos.ddp_X = 0.0;
            CalcResYZPos.ddp_Y = 0.0;
            // 수직 z = -85.706 y - 9.443
            // 수평 z = 0.1006 y + 0.3502
            Calc_X1 = GetFixRefDataPos.ddp_X;
            Calc_Y1 = GetFixRefDataPos.ddp_Y;
            GetCmpAngle = Math.Atan2(Calc_Y1, Calc_X1) * 180.0 / 3.1415926535;

            return CalcResYZPos;
        }

        private DoubleDataPosInfo GetMoveDataPos(DoubleDataPosInfo GetMoveRefDataPos)
        {
            DoubleDataPosInfo CalcResYZPos;
            CalcResYZPos.ddp_ExistFlag = true;
            CalcResYZPos.ddp_X = 0.0;
            CalcResYZPos.ddp_Y = 0.0;
            // 수직 z = 155.679 y - 13.809
            // 수평 z = 0.000668 y - 0.032

            return CalcResYZPos;
        }

        private DoubleDataPosInfo GetDataFixPos2(DoubleDataPosInfo GetSrcDataPos)
        {
            double Diag_Z, Diag_Y;
            double Theta_Z, Theta_Y;
            DoubleDataPosInfo ResDataPos;
            double Real_Calc_X1, Real_Calc_Y1;
            double Input_X1, Input_Y1;
            Diag_Z = -85.076;
            Diag_Y = 0.00668;
            //Diag_Z = 155.679;
            //Diag_Y = 0.000668;
            Input_X1 = GetSrcDataPos.ddp_X;
            Input_Y1 = GetSrcDataPos.ddp_Y;
            ResDataPos.ddp_ExistFlag = true;
            ResDataPos.ddp_X = 0;
            ResDataPos.ddp_Y = 0;
            if (Diag_Z < 0)
            {
                Theta_Z = 3.141592 + Math.Atan(Diag_Z);
            }
            else
            {
                Theta_Z = Math.Atan(Diag_Z);
            }
            Theta_Y = Math.Atan(Diag_Y);
            Real_Calc_Y1 = (Input_Y1 * Math.Cos(Theta_Y) - Input_X1 * Math.Sin(Theta_Y)) / (Math.Cos(Theta_Y) * Math.Sin(Theta_Z) - Math.Sin(Theta_Y) * Math.Cos(Theta_Z));
            Real_Calc_X1 = (Input_Y1 * Math.Cos(Theta_Z) - Input_X1 * Math.Sin(Theta_Z)) / (Math.Cos(Theta_Z) * Math.Sin(Theta_Y) - Math.Sin(Theta_Z) * Math.Cos(Theta_Y));
            ResDataPos.ddp_X = Real_Calc_X1;
            ResDataPos.ddp_Y = Real_Calc_Y1;
            return ResDataPos;
        }

        private DoubleDataPosInfo GetDataMovePos2(DoubleDataPosInfo GetSrcDataPos)
        {
            double Diag_Z, Diag_Y;
            double Theta_Z, Theta_Y;
            DoubleDataPosInfo ResDataPos;
            double Real_Calc_X1, Real_Calc_Y1;
            double Input_X1, Input_Y1;
            //Diag_Z = -85.076;
            //Diag_Y = 0.00668;
            Diag_Z = -155.679;
            Diag_Y = 0.000668;
            Input_X1 = GetSrcDataPos.ddp_X;
            Input_Y1 = GetSrcDataPos.ddp_Y;
            ResDataPos.ddp_ExistFlag = true;
            ResDataPos.ddp_X = 0;
            ResDataPos.ddp_Y = 0;
            if (Diag_Z < 0)
            {
                Theta_Z = 3.141592 + Math.Atan(Diag_Z);
            }
            else
            {
                Theta_Z = Math.Atan(Diag_Z);
            }
            Theta_Y = Math.Atan(Diag_Y);
            Real_Calc_Y1 = (Input_Y1 * Math.Cos(Theta_Y) - Input_X1 * Math.Sin(Theta_Y)) / (Math.Cos(Theta_Y) * Math.Sin(Theta_Z) - Math.Sin(Theta_Y) * Math.Cos(Theta_Z));
            Real_Calc_X1 = (Input_Y1 * Math.Cos(Theta_Z) - Input_X1 * Math.Sin(Theta_Z)) / (Math.Cos(Theta_Z) * Math.Sin(Theta_Y) - Math.Sin(Theta_Z) * Math.Cos(Theta_Y));
            ResDataPos.ddp_X = Real_Calc_X1;
            ResDataPos.ddp_Y = Real_Calc_Y1;
            return ResDataPos;
        }

        private double Axis4_Fix_GetGap(double CurDetailPos_X, double CurDetailPos_Y)
        {
            double CalcCurDetail_X, CalcCurDetail_Y, CalcAxis4ResAngle;
            CalcCurDetail_X = CurDetailPos_X;
            CalcCurDetail_Y = CurDetailPos_Y;
            CalcAxis4ResAngle = Axis4TransAddGap(CurDetailPos_X, CurDetailPos_Y, 0.789, 0.548, 0.328, 0.438);
            return CalcAxis4ResAngle;
        }

        private double Axis4_Move_GetGap(double CurDetailPos_X, double CurDetailPos_Y)
        {
            double CalcCurDetail_X, CalcCurDetail_Y, CalcAxis4ResAngle;
            CalcCurDetail_X = CurDetailPos_X;
            CalcCurDetail_Y = CurDetailPos_Y;
            CalcAxis4ResAngle = Axis4TransAddGap(CurDetailPos_X, CurDetailPos_Y, -0.465, -0.271, 0.0, 0.07);
            return CalcAxis4ResAngle;
        }

        private double Axis4TransAddGap(double CurDetailPos_X, double CurDetailPos_Y, double AxitTopGap, double AxitBottomGap, double AxitLeftGap, double AxitRightGap)
        {
            double ResAxis4GapData;
            double StartAngleGap, EndAngleGap;
            double CurXYAngle;
            int DivIndexPlan;
            ResAxis4GapData = 0;

            if (CurDetailPos_X < 0)
            {
                if (CurDetailPos_Y < 0)
                {
                    DivIndexPlan = 3;
                }
                else
                {
                    DivIndexPlan = 2;
                }
            }
            else
            {
                if (CurDetailPos_Y < 0)
                {
                    DivIndexPlan = 4;
                }
                else
                {
                    DivIndexPlan = 1;
                }
            }
            if (DivIndexPlan == 1)
            {
                StartAngleGap = AxitRightGap;
                EndAngleGap = AxitTopGap;
                CurXYAngle = Math.Atan2(CurDetailPos_Y, CurDetailPos_X);
                if (CurXYAngle < 0)
                {
                    CurXYAngle = 2.0 * Math.PI + CurXYAngle;
                }
                ResAxis4GapData = StartAngleGap + (EndAngleGap - StartAngleGap) * CurXYAngle / (0.5 * Math.PI);
            }
            else if (DivIndexPlan == 2)
            {
                StartAngleGap = AxitTopGap;
                EndAngleGap = AxitLeftGap;
                CurXYAngle = Math.Atan2(CurDetailPos_Y, CurDetailPos_X);
                if (CurXYAngle < 0)
                {
                    CurXYAngle = 2.0 * Math.PI + CurXYAngle;
                }
                ResAxis4GapData = StartAngleGap + (EndAngleGap - StartAngleGap) * (CurXYAngle - (0.5 * Math.PI)) / (0.5 * Math.PI);
            }
            else if (DivIndexPlan == 3)
            {
                StartAngleGap = AxitLeftGap;
                EndAngleGap = AxitBottomGap;
                CurXYAngle = Math.Atan2(CurDetailPos_Y, CurDetailPos_X);
                if (CurXYAngle < 0)
                {
                    CurXYAngle = 2.0 * Math.PI + CurXYAngle;
                }
                ResAxis4GapData = StartAngleGap + (EndAngleGap - StartAngleGap) * (CurXYAngle - (1.0 * Math.PI)) / (0.5 * Math.PI);
            }
            else if (DivIndexPlan == 4)
            {
                StartAngleGap = AxitBottomGap;
                EndAngleGap = AxitRightGap;
                CurXYAngle = Math.Atan2(CurDetailPos_Y, CurDetailPos_X);
                if (CurXYAngle < 0)
                {
                    CurXYAngle = 2.0 * Math.PI + CurXYAngle;
                }
                ResAxis4GapData = StartAngleGap + (EndAngleGap - StartAngleGap) * (CurXYAngle - (1.5 * Math.PI)) / (0.5 * Math.PI);
            }
            return ResAxis4GapData;
        }



        private void btDXFCamFitting_Click(object sender, EventArgs e)
        {
            DXFReFocus_Zoom();
        }

        private void DXFReFocus_Zoom()
        {
            if (SetTypeIndex == SETTYPE_FIX_FRONT)
            {
                //double TestNum = Convert.ToDouble(txtFixAngleAddGap.Text);
                ShotDetailInfo TestZoom = GetDXFShotZoomFocus(0, FS_H);
                //ShotDetailInfo TestZoom = GetDXFShotZoomFocus(0, TestNum);
                CurZoomFactNum = TestZoom.ZoomNum;
                CurFocusFactNum = TestZoom.FocusNum;

                RecoSetData.FullShot_ZoomNum = CurZoomFactNum;
                RecoSetData.FullShot_FocusNum = CurFocusFactNum;
                RecoSetData.DetailShot_ZoomNum = 11755;
                RecoSetData.DetailShot_FocusNum = 19584;

                ShotData[FULLSHOTTYPE].ZoomNum = CurZoomFactNum;
                ShotData[FULLSHOTTYPE].FocusNum = CurFocusFactNum;
                ShotData[DETAILSHOTTYPE].ZoomNum = RecoSetData.DetailShot_ZoomNum;
                ShotData[DETAILSHOTTYPE].FocusNum = RecoSetData.DetailShot_FocusNum;

                trackBar1.Value = CurZoomFactNum;
                trackBar2.Value = CurFocusFactNum;

            }
            else if (SetTypeIndex == SETTYPE_MOVE_FRONT)
            {
                ShotDetailInfo TestZoom = GetDXFShotZoomFocus(1, FS_H);
                CurZoomFactNum = TestZoom.ZoomNum;
                CurFocusFactNum = TestZoom.FocusNum;

                RecoSetData.FullShot_ZoomNum = CurZoomFactNum;
                RecoSetData.FullShot_FocusNum = CurFocusFactNum;
                RecoSetData.DetailShot_ZoomNum = 11716;
                RecoSetData.DetailShot_FocusNum = 19081;

                ShotData[FULLSHOTTYPE].ZoomNum = CurZoomFactNum;
                ShotData[FULLSHOTTYPE].FocusNum = CurFocusFactNum;
                ShotData[DETAILSHOTTYPE].ZoomNum = RecoSetData.DetailShot_ZoomNum;
                ShotData[DETAILSHOTTYPE].FocusNum = RecoSetData.DetailShot_FocusNum;


                trackBar1.Value = CurZoomFactNum;
                trackBar2.Value = CurFocusFactNum;

            }
        }

        private void timLevelAlign_Tick(object sender, EventArgs e)
        {
            timLevelAlign.Enabled = false;

            if (Lvl_Align_Index == LVL_ALIGN_START)
            {
                Lvl_Align_Index = LVL_ALIGN_SEL_FIRST;
                Lbl_SelectL_Hole_Flag = false;
                DXFSelectHoleClear();
                MessageBox.Show("측정기준홀을 선택하세요.");
            }
            else if (Lvl_Align_Index == LVL_ALIGN_SEL_FIRST)
            {
                if (Lbl_SelectL_Hole_Flag == true)
                {
                    LvlAlignData.In_Level = GetSelectDXFHole(1);
                    Lvl_Align_Index = LVL_ALIGN_SEL_SECOND;
                    Lbl_SelectL_Hole_Flag = false;
                    MessageBox.Show("반대편 측정홀을 선택하세요.");
                }
            }
            else if (Lvl_Align_Index == LVL_ALIGN_SEL_SECOND)
            {
                if (Lbl_SelectL_Hole_Flag == true)
                {
                    LvlAlignData.In_Level_B = GetSelectDXFHole(2);
                    if (cboZoomCamList.SelectedIndex == 0)
                    {
                        LvlAlignData.In_Cur_GapAngle = GetDoubleStrNum(txtFixAngleAddGap.Text);
                    }
                    else if (cboZoomCamList.SelectedIndex == 1)
                    {
                        LvlAlignData.In_Cur_GapAngle = GetDoubleStrNum(txtMoveAngleAddGap.Text);
                    }
                    Lvl_Align_Index = LVL_ALIGN_SEL_CALC_VIEW;
                }
            }
            else if (Lvl_Align_Index == LVL_ALIGN_SEL_CALC_VIEW)
            {
                Lbl_SelectL_Hole_Flag = false;
                Lvl_Align_Index = LVL_ALIGN_NONE;

                frmLevelAlign frmDlg;
                frmDlg = new frmLevelAlign();

                frmDlg.In_Level_X = LvlAlignData.In_Level.In_Level_X;
                frmDlg.In_Level_Y = LvlAlignData.In_Level.In_Level_Y;
                frmDlg.In_Level_Angle = LvlAlignData.In_Level.In_Level_Angle;
                frmDlg.In_Level_Distance = LvlAlignData.In_Level.In_Level_Distance;
                frmDlg.In_Level_X_B = LvlAlignData.In_Level_B.In_Level_X;
                frmDlg.In_Level_Y_B = LvlAlignData.In_Level_B.In_Level_Y;
                frmDlg.In_Level_Angle_B = LvlAlignData.In_Level_B.In_Level_Angle;
                frmDlg.In_Level_Distance_B = LvlAlignData.In_Level_B.In_Level_Distance;
                frmDlg.In_Cur_GapAngle = LvlAlignData.In_Cur_GapAngle;
                frmDlg.SetTypeIndex = SetTypeIndex;

                if (frmDlg.ShowDialog() == DialogResult.OK)
                {
                    short RotateIndex_Index;
                    double IndexRoAngle;

                    if (cboZoomCamList.SelectedIndex == 0 )
                    {
                        txtFixAngleAddGap.Text = frmDlg.Out_FinalGapAngle.ToString("##.0000");
                        if (frmDlg.AngleActionFlag == true)
                        {
                            RotateIndex_Index = MultiMotion.INDEX_FIX_R;
                            MultiMotion.GetCurrentPos();
                            IndexRoAngle = MultiMotion.AxisValue[RotateIndex_Index];
                            IndexRoAngle = IndexRoAngle + frmDlg.Out_Level_Gap_Angle;
                            MultiMotion.MoveAxis(RotateIndex_Index, IndexRoAngle, true);
                            UpdatePos();
                        }
                    }
                    else if (cboZoomCamList.SelectedIndex == 1)
                    {
                        txtMoveAngleAddGap.Text = frmDlg.Out_FinalGapAngle.ToString("##.0000");
                        if (frmDlg.AngleActionFlag == true)
                        {
                            RotateIndex_Index = MultiMotion.INDEX_MOVE_R;
                            MultiMotion.GetCurrentPos();
                            IndexRoAngle = MultiMotion.AxisValue[RotateIndex_Index];
                            IndexRoAngle = IndexRoAngle + frmDlg.Out_Level_Gap_Angle;
                            MultiMotion.MoveAxis(RotateIndex_Index, IndexRoAngle, true);
                            UpdatePos();
                        }
                    }
                }
                else
                {
                }
                DXFSelectHoleClear();
                return;
            }

            timLevelAlign.Enabled = true;
        }

        public const int LVL_ALIGN_NONE = 0;
        public const int LVL_ALIGN_START = 1;
        public const int LVL_ALIGN_SEL_FIRST = 2;
        public const int LVL_ALIGN_SEL_SECOND = 3;
        public const int LVL_ALIGN_SEL_CALC_VIEW = 4;
        public int Lvl_Align_Index = LVL_ALIGN_NONE;
        public bool Lbl_SelectL_Hole_Flag;

        private LevelAlignInfo LvlAlignData;

        private void btLevelCalc_Click(object sender, EventArgs e)
        {
            if (FCADImage == null)
            {
                MessageBox.Show("DXF파일이 없습니다.");
                return;
            }

            Lvl_Align_Index = LVL_ALIGN_START;
            timLevelAlign.Enabled = true;

        }

        private double GetDoubleStrNum(string DoubleStr)
        {
            double ResDblNum = 0;
            bool ResCheckNum;
            ResCheckNum = double.TryParse(DoubleStr, out ResDblNum);
            if(ResCheckNum == true)
            {
                return ResDblNum;
            }
            else
            {
                return 0;
            }
        }

        public LevelAlignHoleInfo GetSelectDXFHole(int SelLevelIndex)
        {
            LevelAlignHoleInfo SelHoleData;
            string GetEntityName;
            DXFLine dxLine;
            DXFCircle dxCircle;
            DXFArc dxArc;

            SelHoleData.In_Level_X = 0;
            SelHoleData.In_Level_Y = 0;
            SelHoleData.In_Level_Angle = 0;
            SelHoleData.In_Level_Distance = 0;
            SelHoleData.In_Level_ExistFlag = false;

            if (FCADImage == null)
                return SelHoleData;

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
                        dxCircle.SelectHoleLevelObjFlag = true;
                        dxCircle.SelectHoleLevelObjIndex = SelLevelIndex;
                        SelHoleData.In_Level_X = dxCircle.Point1.X - (FS_W_Base + FS_W / 2.0);
                        SelHoleData.In_Level_Y = dxCircle.Point1.Y - (FS_H_Base + FS_H / 2.0);
                        SelHoleData.In_Level_Angle = Math.Atan2(SelHoleData.In_Level_Y, SelHoleData.In_Level_X) * 180.0 / 3.1415926535;
                        if (SelHoleData.In_Level_Angle < 0)
                        {
                            SelHoleData.In_Level_Angle = 360 + SelHoleData.In_Level_Angle;
                        }
                        SelHoleData.In_Level_Distance = Math.Sqrt(SelHoleData.In_Level_X * SelHoleData.In_Level_X + SelHoleData.In_Level_Y * SelHoleData.In_Level_Y);
                        SelHoleData.In_Level_ExistFlag = false;
                        return SelHoleData;
                    }
                }
            }
            return SelHoleData;
        }

        private string GetOutOnlyFileName(string FindFullFilePath)
        {
            string OutOnlyFileNameStr;
            OutOnlyFileNameStr = "";
            OutOnlyFileNameStr = FindFullFilePath.Substring(FindFullFilePath.LastIndexOf(@"\") + 1);
            return OutOnlyFileNameStr;
        }

        private void ClearDisplayAngleData()
        {
            DisplayAngleDat.FullShot_ExistFlag = false;
            DisplayAngleDat.FullShotAngle = 0.0;
            DisplayAngleDat.DetailShot_ExistFlag = false;
            DisplayAngleDat.DetailShotAngle = 0.0;
            DisplayAngleDat.AddGap_ExistFlag = false;
            DisplayAngleDat.AddGapAngle = 0.0;
        }
        private string GetDisplayAngleDataStr()
        {
            string ResDisplayStr;
            ResDisplayStr = "";
            if (DisplayAngleDat.FullShot_ExistFlag == true)
            {
                ResDisplayStr += "FullShot:" + DisplayAngleDat.FullShotAngle.ToString("#.0000") + "\n";
            }
            if (DisplayAngleDat.DetailShot_ExistFlag == true)
            {
                ResDisplayStr += "DetailShot:" + DisplayAngleDat.DetailShotAngle.ToString("#.0000") + "\n";
            }
            if (DisplayAngleDat.AddGap_ExistFlag == true)
            {
                ResDisplayStr += "AddAngle:" + DisplayAngleDat.AddGapAngle.ToString("#.0000") + "\n";
            }
            return ResDisplayStr;
        }

        private void SetAddDisplayAngle(int DisplayTypeIndex, double AddDisAngle)
        {
            if(DisplayTypeIndex==DIS_TYPE_FULLSHOT)
            {
                DisplayAngleDat.FullShot_ExistFlag = true;
                DisplayAngleDat.FullShotAngle += AddDisAngle;
            }
            else if (DisplayTypeIndex == DIS_TYPE_DETAILSHOT)
            {
                DisplayAngleDat.DetailShot_ExistFlag = true;
                DisplayAngleDat.DetailShotAngle += AddDisAngle;
            }
            else if (DisplayTypeIndex == DIS_TYPE_ADDANGLE)
            {
                DisplayAngleDat.AddGap_ExistFlag = true;
                DisplayAngleDat.AddGapAngle += AddDisAngle;
            }
        }
    }
}
