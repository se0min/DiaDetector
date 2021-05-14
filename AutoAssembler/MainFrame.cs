using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//////////

using System.IO;
using System.Collections;
using System.Globalization;
using System.Threading;
using System.Drawing.Imaging;
using System.Net;
using System.Net.NetworkInformation;

using System.Runtime.InteropServices;
using System.Diagnostics;


//////////

using AutoAssembler.Data;
using AutoAssembler.Drivers;
using AutoAssembler.VisionLibrary;
using AutoAssembler.Utilities;

using XCCamDotNet;
using DXFImportReco;

//////////

using MediaPlayer;

using System.Media;

namespace AutoAssembler
{
    public partial class MainFrame : Form
    {

#region Data

        public bool LocalTestProcFlag = false;
        public int CurTestProcIndex = -1;

        public const int TESTPROCMAX = 1000;

        private TotalTestResultInfo TestCurrentResData;

        public int SaveTimeLoopIndexCount;        

        public int SelectModelDatIndex;
        public string ModelNameStr;
        public int NGLoopCountNum;


        // 시리얼 번호, 성공/실패 여부
        // ----------
        public bool AssembleOk = false;
        public string SerialNumber = "";
        // ----------


        // ----------
        Thread TestTimeThread;
        public int TestRunCountTime = 0;
        bool TestTimeThreadFlag = false;

        delegate void SetTextCallBack(string text);
        // ----------


        // 작업 진행 상태 업데이트 ...
        // ----------
        public const int NONE_RESULT = 0;
        public const int OK_RESULT = 210;
        public const int NG_RESULT = 220;
        public const int ING_RESULT = 230;        

        private ArrayList ProcResStatusInfoList = new ArrayList(new string[] { "대기중", "작업 OK", "작업 NG", "작업중" });
        private ArrayList ProcResStatusIndexList = new ArrayList(new int[] { NONE_RESULT, OK_RESULT, NG_RESULT, ING_RESULT });
        // ----------


        // Data(old) ...
        // ----------
        public bool PassWdOKFlag;

        public MediaPlayer.MediaPlayerClass _player = new MediaPlayer.MediaPlayerClass();



#endregion Data
        

#region Data(Vision) ...

        private int CurCamUID = -1;

        // 홍동성 => 아래 코드는 복사한 코드임.
        // ----------

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
        private Boolean Disp_Flag;

        protected CADImage FCADImage;// = new CADImage();

        Bitmap canvas;
        static int Pic_Width, Pic_Height;


        private static bool GetProcRunFlag;
        Image TempImg;

        //private ControllerLegacy NavitarCtrl;

        private int ZoomFactNum, FocusFactNum;
        private int CurZoomFactNum, CurFocusFactNum;

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
        private const int ZOOMHOME = 2;
        private const int FOCUSHOME = 3;
        private const int ZOOMMOVE = 4;
        private const int FOCUSMOVE = 5;
        private const int CAMSHOT = 6;

        
        
#endregion Data(Vision) ...


#region 강팀장 소스 통합 ...


        public string BaseCurPath;
        public string BaseCurSubPath;

        clsLamp LampComm = new clsLamp();

        public void PortConnect()
        {
            // 홍동성 => 장치 포트를 열고, 초기화를 수행함.

            // 조명 ...
            // ----------
            LampComm.Open(DeviceManager.LightingComPort);
        }

        public void PortDisconnect()
        {
            //CommonUtility.WaitTime(DataManager.TestProcList[CurTestProcIndex].WFDelayTime * 1000, false);


            // 홍동성 => 장치 포트를 닫고, 정리 작업을 수행함.

            // 조명 ...
            // ----------            
            for (int i = 0; i < 8; i++)
            {
                LampComm.OFFLamp(i);
            }

            LampComm.Close();
        }

#endregion 강팀장 소스 통합 ...


#region Vision Logic ...

        /*
        private GCHandle ParamCB;
        private XCCAM.SystemFunc SystemCB = new XCCAM.SystemFunc(SystemCallback);
        private UInt64[] CUID;
        private int CamTotalNum;

        private XCCAM XCCam;
        private XCCAM_IMAGEINFO ImageInfo;
        //private XCCAM_FEATUREINFO NowFeature = new XCCAM_FEATUREINFO();
        private XCCAM.ImageFunc ImageCB = new XCCAM.ImageFunc(ImageCallback);
        private IntPtr RGBData;
        private Stopwatch DispExec = new Stopwatch();
        private Stopwatch Frame = new Stopwatch();
        private Int64 FrameCount;
        private double Fps;
        private Bitmap RGBImage;
        private Int32 Dislay_FPS;
        private Boolean Disp_Flag;

        private static bool GetProcRunFlag;
        private static int Pic_Width, Pic_Height;
        protected CADImage FCADImage;// = new CADImage();
        */
        private static char[] SaveDiv = new char[] { '|' };
        public RecoStructInfo RecoSetData;
        public const string CfgSaveDataFileName = "RecoCfg.dat";
        private const string RecoDXFFileName = "RecoDXF.dxf";

        bool ScreenFlipFlag = false;

        private const int RECOWORKSTART = 1;
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
        private const int RECOWORKEND = 99;

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


        private Boolean Scale_Flag;
        private bool DispSize_Checked = false;
        private Image RecoBlankImg;
        private static RGBDatInfo[,] RGBDXFData;
        private static bool RGBListFlag;
        private static bool RGBListActionFlag;

        private const int FULLSHOTTYPE = 0;
        private const int DETAILSHOTTYPE = 1;
        private int FullDetailShotFlag;
        private ShotDetailInfo[] ShotData = new ShotDetailInfo[2];

        Image RecoWorkImg;
        clsRecoProcess RecoCls;

        private ControllerLegacy NavitarCtrl;

        float FScale = 4.0f;
        public Point Base;
        public Point BaseDefault;
        float FS_W, FS_H, FS_W_Base, FS_H_Base;
        bool StartCalcFlag;

        public const int SHAPE_NONE = 0;
        public const int SHAPE_CIRCLE = 1;
        public const int SHAPE_LINE = 2;
        DXFLayerInfo[] DXFObjList;
        private const int DXFOBJLIST_MAX = 100;
        double IndexFindRoAngle = 0;

        float CalcReScalePer = 92.0f;//77.0f;

        private int CamOpticalGap_Base_X, CamOpticalGap_Base_Y;
        private int[] CamOpticalGap_X;
        private int[] CamOpticalGap_Y;
        private int CamOpticalGapIndex;
        private bool DXFDrawOptAdjFlag = false;

        bool DelayLoopExitFlag;

        private const int CALIB_NONE = 100;
        private const int CALIB_START = 101;
        private const int CALIB_ZOOMFOCUS = 102;
        private const int CALIB_RECOPROCESS = 103;
        private const int CALIB_MOV_POSITION = 104;
        private const int CALIB_RECOPROCESS2 = 105;
        private const int CALIB_MOV_POSITION2 = 106;

        private const int CALIB_END = 99;

        private int Calib_Run_Sts = CALIB_NONE;
        private int Calib_Run_Count = 5;
        DoubleDataPosInfo CameraMoveXYPos;
        DoubleDataPosInfo RealCalibrationData_A;
        DoubleDataPosInfo RealCalibrationData_B;
        DoubleDataXYZPosInfo Calibration_Origin;
        DoubleDataXYZPosInfo Calibration_Current;
        DoubleDataXYZPosInfo Calibration_GapNum;

        public bool CalibrationRunFlag = false;
        public bool CalibrationRuninngFlag = false;
        
        public const string SetCalibPosDataFileName = "CalibPosValue.dat";

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

        private string SaveErrorAlignPath;
        private int SaveErrorAlignIndex = 1;
        private const bool SAVE_ERROR_ALIGN_FLAG = true;

        private const double OBJECT_WEIGHT_MAX = 50.0;
        private const double FIX_INDEX_POS = 870.0;
        private const double MOVE_INDEX_POS = 1190.0;
        private const double MOVE_BACK_INDEX_X_POS = 0.0;
        private const double MOVE_BACK_INDEX_INDEX_POS = 800.0;
        private const double FIX_INDEX_POS_MIN = 820.0;
        private const double MOVE_INDEX_POS_MAX = 1240.0;


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

        private static void SystemCallback(STATUS_SYSTEMCODE SystemStatus, IntPtr Context)
        {
            GCHandle param = GCHandle.FromIntPtr(Context);
            MainFrame CameraListRef = (MainFrame)param.Target;

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

        private void CameraList_Relist(ref UInt64[] WorkUID)
        {
            XCCAM_CAMERAINFO[] CameraInfo;
            int idx;
            String str;


            XCCAM.GetList(out CameraInfo);
            Array.Resize(ref WorkUID, CameraInfo.Length);

            //UIDList.Items.Clear();

            if (CameraInfo.Length != 0)
            {
                for (idx = 0; idx < CameraInfo.Length; idx++)
                {
                    WorkUID[idx] = CameraInfo[idx].UID;
                    str = String.Format("0x{0:X}", WorkUID[idx]);
                    //UIDList.Items.Add(str);
                }
            }
            else
            {
                //UIDList.Items.Add("Not found Camera");
                //UIDList.Enabled = false;
                //CameraOpen.Enabled = false;
            }
        }

        private void InitCamSetting()
        {
            CamTotalNum = 0;
            XCCAM.SetStructVersion(XCCamDotNet.Constants.LIBRARY_STRUCT_VERSION);
            ParamCB = GCHandle.Alloc(this);
            XCCAM.SetCallBack(GCHandle.ToIntPtr(ParamCB), SystemCB);
            SetBounds(10, 10, 0, 0, BoundsSpecified.X | BoundsSpecified.Y);
        }

        private void InitializingCam()
        {
            UInt64[] WorkUID = new UInt64[0];

            CameraList_Relist(ref WorkUID);

            CUID = (UInt64[])WorkUID.Clone();
            //Array.Resize(ref CView, WorkUID.Length);
            CamTotalNum = WorkUID.Length;

            Pic_Width = AutoScaleBox.Width;
            Pic_Height = AutoScaleBox.Height;

            canvas = new Bitmap(Pic_Width, Pic_Height);

            /*
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
            */

            CamOpticalGapIndex = 0;
            CamOpticalGap_X = new int[4];
            CamOpticalGap_Y = new int[4];

            int Calc_Gap_Ori_X, Calc_Gap_Ori_Y;
            int Calc_Gap_X2, Calc_Gap_Y2;
            int Calc_Gap_W, Calc_Gap_H;

            CamOpticalGap_Base_X = 924;
            CamOpticalGap_Base_Y = 773;
            CamOpticalGap_Base_X = 924;
            CamOpticalGap_Base_Y = 773;
            Calc_Gap_W = AutoScaleBox.Width;
            Calc_Gap_H = AutoScaleBox.Height;
            Calc_Gap_Ori_X = (CamOpticalGap_Base_X - 10) / 2;
            Calc_Gap_Ori_Y = (CamOpticalGap_Base_Y - 10) / 2;
            Calc_Gap_X2 = (Calc_Gap_W - 10) / 2;
            Calc_Gap_Y2 = (Calc_Gap_H - 10) / 2;
            /*
            CamOpticalGap_X[0] = Calc_Gap_X2 + (460 - Calc_Gap_Ori_X) * Calc_Gap_X2 / Calc_Gap_Ori_X;// AutoScaleBox.Width / 2 + (455 - Calc_Gap_Ori_X);
            CamOpticalGap_Y[0] = Calc_Gap_Y2 + (399 - Calc_Gap_Ori_Y) * Calc_Gap_Y2 / Calc_Gap_Ori_Y; //396;
            CamOpticalGap_X[1] = Calc_Gap_X2 + (457 - Calc_Gap_Ori_X) * Calc_Gap_X2 / Calc_Gap_Ori_X; //455;
            CamOpticalGap_Y[1] = Calc_Gap_Y2 + (374 - Calc_Gap_Ori_Y) * Calc_Gap_Y2 / Calc_Gap_Ori_Y; //374;
            CamOpticalGap_X[2] = Calc_Gap_X2 + (430 - Calc_Gap_Ori_X) * Calc_Gap_X2 / Calc_Gap_Ori_X; //455;
            CamOpticalGap_Y[2] = Calc_Gap_Y2 + (371 - Calc_Gap_Ori_Y) * Calc_Gap_Y2 / Calc_Gap_Ori_Y; //374;
            CamOpticalGap_X[3] = (CamOpticalGap_Base_X - 10) / 2;
            CamOpticalGap_Y[3] = (CamOpticalGap_Base_Y - 10) / 2;
            */

            /*
            CamOpticalGap_X[0] = Calc_Gap_X2 + ((460 - Calc_Gap_Ori_X) * Calc_Gap_W) / CamOpticalGap_Base_X;// AutoScaleBox.Width / 2 + (455 - Calc_Gap_Ori_X);
            CamOpticalGap_Y[0] = Calc_Gap_Y2 + ((399 - Calc_Gap_Ori_Y) * Calc_Gap_H) / CamOpticalGap_Base_Y; //396;
            CamOpticalGap_X[1] = Calc_Gap_X2 + ((457 - Calc_Gap_Ori_X) * Calc_Gap_W) / CamOpticalGap_Base_X; //455;
            CamOpticalGap_Y[1] = Calc_Gap_Y2 + ((374 - Calc_Gap_Ori_Y) * Calc_Gap_H) / CamOpticalGap_Base_Y; //374;
            CamOpticalGap_X[2] = Calc_Gap_X2 + ((430 - Calc_Gap_Ori_X) * Calc_Gap_W) / CamOpticalGap_Base_X; //455;
            CamOpticalGap_Y[2] = Calc_Gap_Y2 + ((371 - Calc_Gap_Ori_Y) * Calc_Gap_H) / CamOpticalGap_Base_Y; //374;
            CamOpticalGap_X[3] = Calc_Gap_X2;
            CamOpticalGap_Y[3] = Calc_Gap_Y2;
            */

            CamOpticalGap_X[0] = Calc_Gap_X2 + CalcRoundNum(((460 - Calc_Gap_Ori_X) * Calc_Gap_W) , CamOpticalGap_Base_X);// AutoScaleBox.Width / 2 + (455 - Calc_Gap_Ori_X);
            CamOpticalGap_Y[0] = Calc_Gap_Y2 + CalcRoundNum(((399 - Calc_Gap_Ori_Y) * Calc_Gap_H) , CamOpticalGap_Base_Y); //396;
            CamOpticalGap_X[1] = Calc_Gap_X2 + CalcRoundNum(((457 - Calc_Gap_Ori_X) * Calc_Gap_W) , CamOpticalGap_Base_X); //455;
            CamOpticalGap_Y[1] = Calc_Gap_Y2 + CalcRoundNum(((374 - Calc_Gap_Ori_Y) * Calc_Gap_H) , CamOpticalGap_Base_Y); //374;
            CamOpticalGap_X[2] = Calc_Gap_X2 + CalcRoundNum(((430 - Calc_Gap_Ori_X) * Calc_Gap_W) , CamOpticalGap_Base_X); //455;
            CamOpticalGap_Y[2] = Calc_Gap_Y2 + CalcRoundNum(((371 - Calc_Gap_Ori_Y) * Calc_Gap_H) , CamOpticalGap_Base_Y); //374;
            CamOpticalGap_X[3] = Calc_Gap_X2;
            CamOpticalGap_Y[3] = Calc_Gap_Y2;

            /*
            CamOpticalGap_X[0] = Calc_Gap_X2 + (460 - Calc_Gap_Ori_X);// AutoScaleBox.Width / 2 + (455 - Calc_Gap_Ori_X);
            CamOpticalGap_Y[0] = Calc_Gap_Y2 + (399 - Calc_Gap_Ori_Y); //396;
            CamOpticalGap_X[1] = Calc_Gap_X2 + (457 - Calc_Gap_Ori_X); //455;
            CamOpticalGap_Y[1] = Calc_Gap_Y2 + (374 - Calc_Gap_Ori_Y); //374;
            CamOpticalGap_X[2] = Calc_Gap_X2 + (430 - Calc_Gap_Ori_X); //455;
            CamOpticalGap_Y[2] = Calc_Gap_Y2 + (371 - Calc_Gap_Ori_Y); //374;
            CamOpticalGap_X[3] = Calc_Gap_X2;
            CamOpticalGap_Y[3] = Calc_Gap_Y2;
            */
        }

        private int CalcRoundNum(int RoundBaseNum, int RoundDivNum)
        {
            int CalcRoundResNum;
            double dbl_BaseNum, dbl_DivNum, dbl_DivResNum;
            CalcRoundResNum = 0;
            dbl_BaseNum = RoundBaseNum;
            dbl_DivNum = RoundDivNum;
            dbl_DivResNum = dbl_BaseNum / dbl_DivNum;
            CalcRoundResNum = Convert.ToInt32(Math.Round(dbl_DivResNum));
            return CalcRoundResNum;
        }

        private bool CheckUIDList(string GetUIDChkStr)
        {
            bool CheckUIDFlag;
            string GetUIDStr;
            CheckUIDFlag = false;
            for (int i = 0; i < CUID.Length; i++)
            {
                GetUIDStr = CUID[i].ToString();
                if (GetUIDStr == GetUIDChkStr)
                {
                    CheckUIDFlag = true;
                }
            }
            return CheckUIDFlag;
        }

        private int FindSelectUIDList(string UIDFindStr)
        {
            int FindSelIndex;
            UInt64 cmpCUID;
            cmpCUID = (UInt64)Convert.ToInt64(UIDFindStr, 16);
            FindSelIndex = -1;
            for (int i = 0; i < CUID.Length; i++)
            {
                if (cmpCUID == CUID[i])
                {
                    FindSelIndex = i;
                    break;
                }
            }
            return FindSelIndex;
        }

        private void CheckUID_ABCD() // 홍동성 => 카메라 온/오프 상태 확인
        {
            string strCUIDStr = "";
            bool ACamExistFlag, BCamExistFlag, CCamExistFlag, DCamExistFlag;
            for (int i = 0; i < CUID.Length; i++)
            {
                if (i == 0)
                {
                    ACamExistFlag = CheckUIDList(strCUIDStr);
                }
                else if (i == 1)
                {
                    BCamExistFlag = CheckUIDList(strCUIDStr);
                }
                else if (i == 2)
                {
                    CCamExistFlag = CheckUIDList(strCUIDStr);
                }
                else if (i == 3)
                {
                    DCamExistFlag = CheckUIDList(strCUIDStr);
                }
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
                    //ResScreenFlipFlag = false;
                }
            }
            return ResScreenFlipFlag;
        }

        private bool ZoomFocusRunFunc()
        {
            bool ResZoomFocusFlag;
            ResZoomFocusFlag = false;
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
            if (NavitarCtrl.Connected == true)
            {
                Cur_Temp_ZoomNum = NavitarCtrl.Read(REG_USER_CURRENT_1);
                Cur_Temp_FocusNum = NavitarCtrl.Read(REG_USER_CURRENT_2);
                Cur_Temp_Zoom_Status = NavitarCtrl.Read(REG_USER_STATUS_1);
                Cur_Temp_Focus_Status = NavitarCtrl.Read(REG_USER_STATUS_2);
                
                //label6.Text = Cur_Temp_Zoom_Status.ToString() + "," + Cur_Temp_Focus_Status.ToString();
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

                /*
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
                */
                if(Cur_Temp_Zoom_Status==0 && Cur_Temp_Focus_Status==0)
                {
                    ResZoomFocusFlag = true;
                }
                
            }
            return ResZoomFocusFlag;
        }

        private bool CameraOneOpen(UInt64 UID)
        {
            bool SucCamOpenFlag = true;
            XCCAM_CAMERAINFO CameraInfo;

            XCCam = new XCCAM(UID);

            if (!XCCam.IsXCCam_Ready())
            {
                //MessageBox.Show("Open Camera Error");
                XCCam.Dispose();
                SucCamOpenFlag = false;
                return SucCamOpenFlag;
            }
            XCCam.CameraInfo(out CameraInfo);

            //ParamCB = GCHandle.Alloc(this);
            return SucCamOpenFlag;
        }

        private void CameraPlayerStop()
        {
            if (XCCam.ImageStart())
            {
                if (!XCCam.ImageStop())
                {
                    //MessageBox.Show("Image Stop Error");
                }
                XCCam.SetImageCallBack();
                if (!XCCam.ResourceRelease())
                {
                    MessageBox.Show("Resource Release Error");
                }
                DispExec.Stop();
                Frame.Stop();
            }
        }

        private void CameraOneClose()
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

                /*
                NavitarCtrl.Disconnect();
                NavitarCtrl.Dispose();
                NavitarCtrl = null;
                */
            }
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
            MainFrame VRef = (MainFrame)param.Target;
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
            if (DispSize_Checked)
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
                Invoke(d, new object[] { });
            }
            else
            {
                //AutoScaleBox.Image = RGBImage;
                //AutoScaleBox.Image = GetTotalImg((int)ImageInfo.Width, (int)ImageInfo.Height);
                //Image RefCalcImg = RGBImage;
                if (GetProcRunFlag == false)
                {
                    GetProcRunFlag = true;
                    Bitmap objShotBitmap = new Bitmap((Image)RGBImage, (int)AutoScaleBox.Width - 10, (int)AutoScaleBox.Height - 10);
                    if (ScreenFlipFlag == true)
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
                    objShotBitmap = ReSizeCurBitmap2(objShotBitmap, (int)AutoScaleBox.Width - 10, (int)AutoScaleBox.Height - 10, calc_gap_x, calc_gap_y);//objShotBitmap;

                    RecoBlankImg = ReSizeCurBitmap(objShotBitmap, (int)AutoScaleBox.Width - 10, (int)AutoScaleBox.Height - 10);//objShotBitmap;
                    AutoScaleBox.Image = objShotBitmap;
                    //RecoBlankImg = objShotBitmap;
                    //AutoScaleBox.Image = objShotBitmap;

                    //RecoBlankImg = ReSizeCurBitmap(objShotBitmap, (int)AutoScaleBox.Width - 10, (int)AutoScaleBox.Height - 10);//objShotBitmap;
                    DrawDXFImageCalc(AutoScaleBox.Width, AutoScaleBox.Height, false);
                    DrawDXFImage2((Bitmap)AutoScaleBox.Image);
                    //RealScaleBox.Image = GetTotalImg3((Bitmap)RGBImage, (int)PanelPicture.Width - 10, (int)PanelPicture.Height - 10);
                    //RealScaleBox.Image = GetTotalImg2((Bitmap)RGBImage, (int)ImageInfo.Width, (int)ImageInfo.Height);
                    /*
                    if (Scale_Flag)
                    {
                        Scale_Flag = false;
                        if (DispSize_Checked)
                        {
                            AutoScaleBox.Visible = true;
                            RealScaleBox.Visible = false;
                        }
                        else
                        {
                            AutoScaleBox.Visible = true;
                            RealScaleBox.Visible = false;
                        }
                    }
                    */
                    GetProcRunFlag = false;
                }
            }
        }

        delegate void SetDisplayImageRealCallback();

        private void SetDisplayImageReal()
        {
            if (AutoScaleBox.InvokeRequired)
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
                    //RealScaleBox.Image = GetTotalImg3((Bitmap)RGBImage, (int)PanelPicture.Width - 10, (int)PanelPicture.Height - 10);
                    AutoScaleBox.Image = GetTotalImg2((Bitmap)RGBImage, (int)ImageInfo.Width, (int)ImageInfo.Height);
                    RecoBlankImg = AutoScaleBox.Image;
                    DrawDXFImageCalc(AutoScaleBox.Width, AutoScaleBox.Height, false);
                    DrawDXFImage2((Bitmap)AutoScaleBox.Image);
                    /*
                    if (Scale_Flag)
                    {
                        Scale_Flag = false;
                        if (DispSize_Checked)
                        {
                            AutoScaleBox.Visible = true;
                            AutoScaleBox.Visible = false;
                            //RealScaleBox.SizeMode = PictureBoxSizeMode.AutoSize;
                        }
                        else
                        {
                            AutoScaleBox.Visible = true;
                            AutoScaleBox.Visible = false;
                            //RealScaleBox.SizeMode = PictureBoxSizeMode.Zoom;
                            AutoScaleBox.Width = AutoScaleBox.Width;
                            AutoScaleBox.Height = AutoScaleBox.Height;
                        }
                    }
                    */
                    GetProcRunFlag = false;
                }
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
                    //rd1 = rd1;// *FScale;

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

                BaseDefault.X = DrImgW / 2;
                BaseDefault.Y = DrImgH / 2;

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



                StartCalcFlag = false;
            }
            //bmp = tmp;
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
            string OutPutAngleStr;

            MinX = 99999.0f;
            MinY = 99999.0f;
            MaxX = -99999.0f;
            MaxY = -99999.0f;
            if (FCADImage == null)
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

                        //blackPen.Color = dxCircle.FColor;

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

                OutPutAngleStr = GetDisplayAngleDataStr();
                if (OutPutAngleStr.Length > 0)
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

        public float Conversion_Angle(float Val)
        {
            while (Val < 0) Val = Val + 360;
            return Val;
        }

        /*
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
                    RecoSetData.Rotation_R = Convert.ToInt32(m_WordStr[10]);
                    RecoSetData.Motion_Index = Convert.ToInt32(m_WordStr[11]);
                    RecoSetData.Motion_Move_X = Convert.ToInt32(m_WordStr[12]);
                    RecoSetData.Motion_Move_Y = Convert.ToInt32(m_WordStr[13]);
                    RecoSetData.Motion_Move_Z = Convert.ToInt32(m_WordStr[14]);
                    RecoSetData.Lamp_Index = Convert.ToInt32(m_WordStr[15]);
                    RecoSetData.Lamp_PortNo = Convert.ToInt32(m_WordStr[16]);
                    RecoSetData.Lamp_SetChannel = Convert.ToInt32(m_WordStr[17]);
                    RecoSetData.Lamp_SetValue = Convert.ToInt32(m_WordStr[18]);

                }
            }
        }
        */

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

        private void SetValueLamp(int _WorkFuncInfo_Lamp_Index, int _WorkFuncInfo_WFLampValue)
        {
            if (_WorkFuncInfo_Lamp_Index > -1)
            {
                int SetChnlNum = DataManager.LightingSettingInfoList[_WorkFuncInfo_Lamp_Index].Channel - 1;
                int SetValueNum = _WorkFuncInfo_WFLampValue;
                LampComm.SetLamp(SetChnlNum, SetValueNum);
            }
        }

        private void RunMotionAxis_X(double SetMoveValueAxis, bool RunMotionWaitFlag)
        {
            double TmpMoveValueAxis = 0;
            short TmpMoveAxis_Index;

            if (LocalTestProcFlag == false)
            {
                return;
            }
            MultiMotion.GetCurrentPos();
            //if (SetMoveValueAxis > 780 && SetMoveValueAxis < 1210)
            //if (SetMoveValueAxis > 800 - OBJECT_WEIGHT_MAX && SetMoveValueAxis < 1210 + OBJECT_WEIGHT_MAX)
            if (SetMoveValueAxis > FIX_INDEX_POS_MIN && SetMoveValueAxis < MOVE_INDEX_POS_MAX)
            {
                TmpMoveAxis_Index = MultiMotion.CAM_UNIT_X;

                TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
                MultiMotion.MoveAxis(TmpMoveAxis_Index, SetMoveValueAxis, false);
                if (RunMotionWaitFlag == true)
                {
                    MultiMotion.GetCurrentPos();
                    while (Math.Abs(MultiMotion.AxisValue[TmpMoveAxis_Index] - SetMoveValueAxis) > 1.0)
                    {
                        if (LocalTestProcFlag == false)
                        {
                            return;
                        }
                        MultiMotion.GetCurrentPos();
                        DelayWaitRun(5);
                    }
                }
            }
            else
            {
                TmpMoveAxis_Index = MultiMotion.CAM_UNIT_Z;

                TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
                if (TmpMoveValueAxis > 80)
                {
                    TmpMoveAxis_Index = MultiMotion.INDEX_MOVE_M;
                    TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
                    if (TmpMoveValueAxis < 800)
                    {
                        MessageBox.Show("X축이동위치에 Index이동축이 있어 부딪힐 수 있습니다.");
                        return;
                    }
                }
                

                {
                    TmpMoveAxis_Index = MultiMotion.CAM_UNIT_X;

                    TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
                    MultiMotion.MoveAxis(TmpMoveAxis_Index, SetMoveValueAxis, false);
                    if (RunMotionWaitFlag == true)
                    {
                        MultiMotion.GetCurrentPos();
                        while (Math.Abs(MultiMotion.AxisValue[TmpMoveAxis_Index] - SetMoveValueAxis) > 1.0)
                        {
                            if (LocalTestProcFlag == false)
                            {
                                return;
                            }
                            MultiMotion.GetCurrentPos();
                            DelayWaitRun(5);
                        }
                    }
                }
            }
        }

        private void RunMotionAxis_Y(double SetMoveValueAxis, bool RunMotionWaitFlag)
        {
            double TmpMoveValueAxis = 0;
            short TmpMoveAxis_Index;

            if (LocalTestProcFlag == false)
            {
                return;
            }

            MultiMotion.GetCurrentPos();

            TmpMoveAxis_Index = MultiMotion.CAM_UNIT_Y;

            TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
            MultiMotion.MoveAxis(TmpMoveAxis_Index, SetMoveValueAxis, false);
            if (RunMotionWaitFlag == true)
            {
                MultiMotion.GetCurrentPos();
                while (Math.Abs(MultiMotion.AxisValue[TmpMoveAxis_Index] - SetMoveValueAxis) > 1.0)
                {
                    if (LocalTestProcFlag == false)
                    {
                        return;
                    }
                    MultiMotion.GetCurrentPos();
                    DelayWaitRun(5);
                }
            }
        }

        private void RunMotionAxis_Z(double SetMoveValueAxis, bool RunMotionWaitFlag)
        {
            double TmpMoveValueAxis = 0;
            short TmpMoveAxis_Index;

            if (LocalTestProcFlag == false)
            {
                return;
            }

            MultiMotion.GetCurrentPos();

            TmpMoveAxis_Index = MultiMotion.CAM_UNIT_Z;

            TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
            MultiMotion.MoveAxis(TmpMoveAxis_Index, SetMoveValueAxis, false);
            if (RunMotionWaitFlag == true)
            {
                MultiMotion.GetCurrentPos();
                while (Math.Abs(MultiMotion.AxisValue[TmpMoveAxis_Index] - SetMoveValueAxis) > 1.0)
                {
                    if (LocalTestProcFlag == false)
                    {
                        return;
                    }

                    MultiMotion.GetCurrentPos();
                    DelayWaitRun(5);
                }
            }
        }

        private void RunMotionAxis_MR(double SetMoveValueAxis, bool RunMotionWaitFlag)
        {
            double TmpMoveValueAxis = 0;
            short TmpMoveAxis_Index;

            if (LocalTestProcFlag == false)
            {
                return;
            }

            MultiMotion.GetCurrentPos();

            TmpMoveAxis_Index = MultiMotion.INDEX_MOVE_M;

            TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
            if (MultiMotion.GantryAxisEnable(1, true) == MultiMotion.KSM_OK) // INDEX(X) 갠트리 활성화
            {
                MultiMotion.MoveAxis(TmpMoveAxis_Index, SetMoveValueAxis, false);
            }
            if (RunMotionWaitFlag == true)
            {
                MultiMotion.GetCurrentPos();
                while (Math.Abs(MultiMotion.AxisValue[TmpMoveAxis_Index] - SetMoveValueAxis) > 1.0)
                {
                    if (LocalTestProcFlag == false)
                    {
                        return;
                    }

                    MultiMotion.GetCurrentPos();
                    DelayWaitRun(5);
                }
            }
        }


        private void RunMotionAxis_Back_Z(double SetMoveValueAxis)
        {
            double TmpMoveValueAxis = 0;
            short TmpMoveAxis_Index;

            if (LocalTestProcFlag == false)
            {
                return;
            }

            MultiMotion.GetCurrentPos();

            TmpMoveAxis_Index = MultiMotion.BACK_CAM_Z;

            TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
            MultiMotion.MoveAxis(TmpMoveAxis_Index, SetMoveValueAxis, false);
            MultiMotion.GetCurrentPos();
            while (Math.Abs(MultiMotion.AxisValue[TmpMoveAxis_Index] - SetMoveValueAxis) > 1.0)
            {
                if (LocalTestProcFlag == false)
                {
                    return;
                }

                MultiMotion.GetCurrentPos();
                DelayWaitRun(5);
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
                        SFPoint p1 = dxCircle.Point1;
                        ResPointData.Y = p1.X - FS_W_Base - FS_W / 2;
                        ResPointData.Z = p1.Y - FS_H_Base - FS_H / 2;
                    }
                }
            }
            return ResPointData;
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

        private double IndexRotationHalf(double IndexCurRotAngle)
        {
            double ResIndexRotAngle;
            ResIndexRotAngle = IndexCurRotAngle;
            if (IndexCurRotAngle < -180.0)
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


        private void ClearRecoViewObject()
        {
            CalibrationRuninngFlag = false;
            AutoScaleBox.Visible = false;
            picRecoImg.Visible = false;
        }

        private bool RecoProcessRun()
        {
            WorkFuncInfo _WorkFuncInfo;
            bool ResSucessFlag;
            string LoadRecoCfgFileStr;
            string LoadMainFolder;
            bool Disp_Chcek;
            string ZF_ComPortStr;
            string CfgSaveFolderPath;
            string CfgSaveHeadFileName;
            bool TopAlignProcFlag = false;
            bool BackAlignProcFlag = false;
            bool RunMultiMoveFlag = true;
            bool MoveBackAlignProcFlag = false;

            double SetMoveValueAxis = 0;
            double TmpMoveValueAxis = 0;
            short TmpMoveAxis_Index;

            ClearDisplayAngleData();

            AutoScaleBox.Visible = true;
            picRecoImg.Visible = false;

            _WorkFuncInfo = DataManager.TestProcList[CurTestProcIndex];

            picRecoImg.Top = AutoScaleBox.Top;
            picRecoImg.Left = AutoScaleBox.Left;
            picRecoImg.Width = AutoScaleBox.Width;
            picRecoImg.Height = AutoScaleBox.Height;


            CfgSaveFolderPath = ConfigManager.GetModelFilePath + DataManager.GetModelSelectFileNameFolder();
            CfgSaveHeadFileName = _WorkFuncInfo.WFRecoDatHeadFileName;

            int idx;

            ResSucessFlag = false;

            //LoadMainFolder = ConfigManager.GetModelFilePath + DataManager.GetModelSelectFileNameFolder() + @"\";
            LoadMainFolder = CfgSaveFolderPath;

            LoadRecoCfgFileStr = CfgSaveFolderPath + @"\" + CfgSaveHeadFileName + @".dat";

            if (File.Exists(LoadRecoCfgFileStr)==false)
            {
                ClearRecoViewObject();
                return false;
            }

            LoadCfgFiles(LoadRecoCfgFileStr);


            if (RecoSetData.RecoCamIndex == 3)
            {
                TopAlignProcFlag = true;
            }
            else
            {
                TopAlignProcFlag = false;
            }

            if (RecoSetData.RecoCamIndex == 2)
            {
                BackAlignProcFlag = true;
            }
            else
            {
                BackAlignProcFlag = false;
            }

            if (RecoSetData.RecoCamIndex == 0 && _WorkFuncInfo.Rotation_Index == 1)
            {
                MoveBackAlignProcFlag = true;
            }

            short RotateIndex_Index;
            if (_WorkFuncInfo.Rotation_Index == 0)
            {
                RotateIndex_Index = MultiMotion.INDEX_FIX_R;
            }
            else
            {
                RotateIndex_Index = MultiMotion.INDEX_MOVE_R;
            }

            double ObjectPlWeight, ObjPLAddMove_X;


            if (RunMultiMoveFlag)
            {
                MultiMotion.GetCurrentPos();

                if (BackAlignProcFlag == true)
                {
                    MultiMotion.GetCurrentPos();
                    SetMoveValueAxis = RecoSetData.Motion_Move_Z;
                    TmpMoveAxis_Index = MultiMotion.BACK_CAM_Z;
                    TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];

                    /*
                    if (RecoSetData.Motion_Move_Z > 80)
                    {
                        MessageBox.Show("Z축 위치가 너무 낮습니다. 테스트를 종료합니다.");
                        return false;
                    }
                    */

                    RunMotionAxis_Back_Z(RecoSetData.Motion_Move_Z);

                }
                else if (TopAlignProcFlag == true)
                {
                    MultiMotion.GetCurrentPos();
                    TmpMoveAxis_Index = MultiMotion.INDEX_MOVE_M;
                    TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];

                    if (TmpMoveValueAxis > 10.0)
                    {
                        RunMotionAxis_Z(1.0, true);
                        RunMotionAxis_MR(1.0, true);
                    }

                    MultiMotion.GetCurrentPos();
                    SetMoveValueAxis = RecoSetData.Motion_Move_Z;
                    TmpMoveAxis_Index = MultiMotion.CAM_UNIT_Z;

                    TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];

                    if (RecoSetData.Motion_Move_Z > 80)
                    {
                        MessageBox.Show("Z축 위치가 너무 낮습니다. 테스트를 종료합니다.");
                        return false;
                    }

                    RunMotionAxis_Z(RecoSetData.Motion_Move_Z, false);
                    RunMotionAxis_X(RecoSetData.Motion_Move_X, false);
                    RunMotionAxis_Y(RecoSetData.Motion_Move_Y, false);

                }
                else if (MoveBackAlignProcFlag == true)
                {
                    if (RecoSetData.Motion_Move_MR < 800)
                    {
                        MessageBox.Show("이동축의 위치가 후방카메라와 겹칩니다.");
                        return false;
                    }

                    MultiMotion.GetCurrentPos();
                    TmpMoveAxis_Index = MultiMotion.INDEX_MOVE_M;
                    TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];

                    if (TmpMoveValueAxis < 800.0)
                    {
                        RunMotionAxis_Z(1.0, true);
                    }


                    //MOTION이동부분
                    //=============================================
                    RunMotionAxis_MR(RecoSetData.Motion_Move_MR, true);
                    RunMotionAxis_X(RecoSetData.Motion_Move_X, true);
                    RunMotionAxis_Y(RecoSetData.Motion_Move_Y, false);
                    RunMotionAxis_Z(RecoSetData.Motion_Move_Z, false);

                    //MultiMotion.RotateAxis(0, 10.0);
                    //=============================================

                }
                else
                {
                    MultiMotion.GetCurrentPos();
                    TmpMoveAxis_Index = MultiMotion.INDEX_MOVE_M;
                    TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];

                    if (TmpMoveValueAxis > 10.0)
                    {
                        RunMotionAxis_MR(1.0, false);
                        RunMotionAxis_Z(1.0, true);
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
                    //RunMotionAxis_X(RecoSetData.Motion_Move_X, true);
                    RunMotionAxis_X(ObjPLAddMove_X, true);
                    RunMotionAxis_Y(RecoSetData.Motion_Move_Y, false);
                    RunMotionAxis_Z(RecoSetData.Motion_Move_Z, false);

                    //MultiMotion.RotateAxis(0, 10.0);
                    //=============================================

                }
            }

            idx = FindSelectUIDList(DataManager.CameraSettingInfoList[RecoSetData.RecoCamIndex].IP);// RecoSetData.RecoCamIndex;
            Disp_Chcek = false;

            if (CamTotalNum > idx)
            {
            }
            else
            {
                ClearRecoViewObject();
                return false;
            }

            if (CameraOneOpen(CUID[idx]) == true)
            {
            }
            else
            {
                ClearRecoViewObject();
                return false;
            }



            CamOpticalGapIndex = RecoSetData.RecoCamIndex;

            ScreenFlipFlag = GetScreenFlip(RecoSetData.RecoCamIndex);

            Dislay_FPS = 10;//30;//10-100;

            //XCCam.SetValue("width", "640");
            //XCCam.SetValue("height", "480");

            if (!XCCam.ResourceAlloc())
            {
                //MessageBox.Show("Resource Alloc Error");
                ClearRecoViewObject();
                return false;
            }

            if (!XCCam.GetImageInfo(out ImageInfo))
            {
                XCCam.ResourceRelease();
                //MessageBox.Show("Get ImageInfo Error");
                ClearRecoViewObject();
                return false;
            }

            if (Disp_Chcek)
            {
                //PanelPicture.Visible = true;
                //AutoScaleBox.Visible = false;
            }
            else
            {
                //PanelPicture.Visible = true;
                //AutoScaleBox.Visible = false;

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
                //MessageBox.Show("Image Start Error");
                XCCam.SetImageCallBack();
                DispExec.Stop();
                Frame.Stop();
                XCCam.ResourceRelease();
                ClearRecoViewObject();
                return false;
            }

            Disp_Flag = false;

            /*
            //DXF도면 로드

            StartCalcFlag = true;
            FCADImage = new CADImage();
            Base.X = 1;
            Base.Y = 1;
            FScale = 1.0f;
            FCADImage.LoadFromFile(RecoSetData.RecoDXFFileName);
            */

            string LoadDXFLocalFileStr;
            if (TopAlignProcFlag == false)
            {
                if (CfgSaveFolderPath.Length > 0)
                {
                    StartCalcFlag = true;
                    FCADImage = new CADImage();
                    Base.X = 1;
                    Base.Y = 1;
                    FScale = 1.0f;
                    //timer1.Interval = 600;
                    //timer1.Enabled = true;
                    LoadDXFLocalFileStr = CfgSaveFolderPath + @"\" + RecoSetData.RecoDXFFileName;
                    if (File.Exists(LoadDXFLocalFileStr) == true)
                    {
                        FCADImage.LoadFromFile(LoadDXFLocalFileStr);
                    }
                    else
                    {
                        ClearRecoViewObject();
                        CameraPlayerStop();
                        CameraOneClose();
                        return false;
                    }
                }
            }

            SetValueLamp(_WorkFuncInfo.Lamp_Index, _WorkFuncInfo.WFLampValue);


            //Navitar초기화

            ZF_ComPortStr = "COM" + DataManager.CameraSettingInfoList[RecoSetData.RecoCamIndex].ZoomFocusPort.ToString();

            ShotData[FULLSHOTTYPE].ZoomNum = RecoSetData.FullShot_ZoomNum;
            ShotData[FULLSHOTTYPE].FocusNum = RecoSetData.FullShot_FocusNum;
            ShotData[DETAILSHOTTYPE].ZoomNum = RecoSetData.DetailShot_ZoomNum;
            ShotData[DETAILSHOTTYPE].FocusNum = RecoSetData.DetailShot_FocusNum;


            Application.DoEvents();
            Application.DoEvents();

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

            NavitarCtrl = new ControllerLegacy(ZF_ComPortStr);
            NavitarCtrl.Connect();


            if (NavitarCtrl.ProductID > 0)
            {
                ZoomIn_Max = NavitarCtrl.Read(REG_SETUP_LIMIT_1);
                Focus_Max = NavitarCtrl.Read(REG_SETUP_LIMIT_2);
                ZoomIn_Min = 0;
                Focus_Min = 0;

                ZoomFactNum = NavitarCtrl.Read(REG_USER_CURRENT_1);
                FocusFactNum = NavitarCtrl.Read(REG_USER_CURRENT_2);


                CurZoomFactNum = ShotData[FULLSHOTTYPE].ZoomNum;
                CurFocusFactNum = ShotData[FULLSHOTTYPE].FocusNum;

                for (int i = 0; i < 8;i++ )
                {
                    if(ZoomFocusRunFunc()==true)
                    {
                        break;
                    }
                    DelayWaitRun(5);
                }
                DelayWaitRun(10);

            }
            else
            {
                ClearRecoViewObject();
                CameraPlayerStop();
                CameraOneClose();
                return false;
            }

            // 동작 프로세스

            int Cur_Temp_Zoom_Status, Cur_Temp_Focus_Status;
            int ZeroLoopCountChkMax = 3;// 10;
            int ZeroLoopCountInitChkMax = 9;// 10;
            int ZeroLoopCountChkMax_Shot = 5;
            int DelayTimerLoopTime = 1;


            double IndexRoAngle = 0;
            //double TmpMoveXAxis = 0;
            //double TmpMoveYAxis = 0;
            //double TmpMoveZAxis = 0;
            double Detail_Move_Pos_X, Detail_Move_Pos_Y, Detail_Move_Pos_Z;
            SFPoint GetDetailSelObj;

            //double ObjectPlWeight, ObjPLAddMove_X;


            LpWk_LoopCount = 0;
            LpWk_LoopCountMax = 1;
            LpWk_Zoom = ZoomFactNum;// Int32.Parse(textBox1.Text);
            LpWk_Focus = FocusFactNum;// Int32.Parse(textBox2.Text);

            LpWk_RunTimerCount = 0;
            LpWk_RunFlag = true;
            LpWk_RUN_STATUS = WORKSTART;


            if (LpWk_RunFlag == true)
            {
                LpWk_RUN_STATUS = RECOWORKSTART;
                while(LpWk_RUN_STATUS == RECOWORKSTART)
                {
                    LpWk_RUN_STATUS = MOTIONMOVE;
                    LpWk_ZeroCount = 0;
                    //label5.Text = "RECOWORKSTART";
                    DelayWaitRun(DelayTimerLoopTime);
                    MultiMotion.IndexRPosClear();
                    MultiMotion.SetSpeed(MultiMotion.KSM_SPEED_FAST);
                    if(LocalTestProcFlag == false)
                    {
                        CameraPlayerStop();
                        CameraOneClose();
                        return false;
                    }
                }

                while(LpWk_RUN_STATUS == MOTIONMOVE)
                {
                    if (LocalTestProcFlag == false)
                    {
                        CameraPlayerStop();
                        CameraOneClose();
                        return false;
                    }
                    ZoomFocusRunFunc();
                    //label5.Text = "MOTION이동";
                    if (LpWk_ZeroCount == 0)
                    {
                        LpWk_ZeroCount = 1;
                        MultiMotion.GetCurrentPos();

                        if (BackAlignProcFlag == true)
                        {
                            MultiMotion.GetCurrentPos();
                            SetMoveValueAxis = RecoSetData.Motion_Move_Z;
                            TmpMoveAxis_Index = MultiMotion.BACK_CAM_Z;
                            TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];

                            /*
                            if (RecoSetData.Motion_Move_Z > 80)
                            {
                                MessageBox.Show("Z축 위치가 너무 낮습니다. 테스트를 종료합니다.");
                                return false;
                            }
                            */

                            RunMotionAxis_Back_Z(RecoSetData.Motion_Move_Z);

                        }
                        else if (TopAlignProcFlag == true)
                        {

                            MultiMotion.GetCurrentPos();
                            TmpMoveAxis_Index = MultiMotion.INDEX_MOVE_M;
                            TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];

                            if (TmpMoveValueAxis > 10.0)
                            {
                                RunMotionAxis_Z(1.0, true);
                                RunMotionAxis_MR(0.0, true);
                            }

                            SetMoveValueAxis = RecoSetData.Motion_Move_Z;
                            TmpMoveAxis_Index = MultiMotion.CAM_UNIT_Z;

                            TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];

                            if (RecoSetData.Motion_Move_Z > 80)
                            {
                                MessageBox.Show("Z축 위치가 너무 낮습니다. 테스트를 종료합니다.");
                                CameraPlayerStop();
                                CameraOneClose();
                                return false;
                            }

                            RunMotionAxis_Z(RecoSetData.Motion_Move_Z, true);
                            RunMotionAxis_X(RecoSetData.Motion_Move_X, true);
                            RunMotionAxis_Y(RecoSetData.Motion_Move_Y, true);
                        }
                        else if (MoveBackAlignProcFlag == true)
                        {
                            if (RecoSetData.Motion_Move_MR < 800)
                            {
                                MessageBox.Show("이동축의 위치가 후방카메라와 겹칩니다.");
                                CameraPlayerStop();
                                CameraOneClose();
                                return false;
                            }

                            MultiMotion.GetCurrentPos();
                            TmpMoveAxis_Index = MultiMotion.INDEX_MOVE_M;
                            TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];

                            if (TmpMoveValueAxis < 800.0)
                            {
                                RunMotionAxis_Z(1.0, true);
                            }

                            //MOTION이동부분
                            //=============================================
                            RunMotionAxis_MR(RecoSetData.Motion_Move_MR, true);
                            RunMotionAxis_X(RecoSetData.Motion_Move_X, true);
                            RunMotionAxis_Y(RecoSetData.Motion_Move_Y, true);
                            RunMotionAxis_Z(RecoSetData.Motion_Move_Z, true);

                            //MultiMotion.RotateAxis(0, 10.0);
                            //=============================================

                        }
                        else
                        {
                            MultiMotion.GetCurrentPos();
                            TmpMoveAxis_Index = MultiMotion.INDEX_MOVE_M;
                            TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];

                            if (TmpMoveValueAxis > 10.0)
                            {
                                RunMotionAxis_Z(1.0, true);
                                RunMotionAxis_MR(1.0, true);
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
                            //RunMotionAxis_X(RecoSetData.Motion_Move_X, true);
                            RunMotionAxis_X(ObjPLAddMove_X, true);
                            RunMotionAxis_Y(RecoSetData.Motion_Move_Y, true);
                            RunMotionAxis_Z(RecoSetData.Motion_Move_Z, true);

                            //MultiMotion.RotateAxis(0, 10.0);
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
                    DelayWaitRun(DelayTimerLoopTime);
                }

                while(LpWk_RUN_STATUS == GOFULLSHOT)
                {
                    if (LocalTestProcFlag == false)
                    {
                        CameraPlayerStop();
                        CameraOneClose();
                        return false;
                    }
                    ZoomFocusRunFunc();

                    //label5.Text = "FULLSHOT보기";
                    if (LpWk_ZeroCount == 0)
                    {
                        StartCalcFlag = true;

                        //Base.X = AutoScaleBox.Image.Width / 2;
                        //Base.Y = AutoScaleBox.Image.Height / 2;

                        BaseDefault.X = AutoScaleBox.Image.Width / 2;
                        BaseDefault.Y = AutoScaleBox.Image.Height / 2;
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


                        DrawDXFImageCalc(AutoScaleBox.Image.Width, AutoScaleBox.Image.Height, false);


                        CurZoomFactNum = ShotData[FULLSHOTTYPE].ZoomNum;
                        CurFocusFactNum = ShotData[FULLSHOTTYPE].FocusNum;
                        LpWk_ZeroCount = 1;
                    }
                    if (LpWk_ZeroCount > ZeroLoopCountInitChkMax)
                    {
                        LpWk_RUN_STATUS = FULLRECOPROC;
                        LpWk_ZeroCount = 0;
                    }
                    else
                    {
                        if (NavitarCtrl.ProductID > 0)
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
                    DelayWaitRun(DelayTimerLoopTime);
                }

                while(LpWk_RUN_STATUS == FULLRECOPROC)
                {
                    if (LocalTestProcFlag == false)
                    {
                        CameraPlayerStop();
                        CameraOneClose();
                        return false;
                    }
                    //label5.Text = "인식";
                    if (LpWk_ZeroCount == 0)
                    {
                        /*
                        //picRecoImg.Image = RealScaleBox.Image;
                        //RecoBlankImg = RealScaleBox.Image;
                        RecoWorkImg = RecoBlankImg;
                        AutoScaleBox.Image.Save("001_1.jpg");
                        if (RecoWorkImg != null)
                        {
                            RecoCls = new clsRecoProcess(ref RecoWorkImg);
                            RecoCls.FindEdge(0);
                            RecoCls.CutOffFindEdge();
                            RecoCls.FindObject();
                            RecoCls.FindCircleArc();
                            RecoCls.FindObjectCenterAngle();
                            picRecoImg.Image = RecoCls.GetArrObjectImage();
                        }
                        */

                        RecoWorkImg = RecoBlankImg;
                        //RealScaleBox.Image.Save("001_1.jpg");

                        if (BackAlignProcFlag == true)
                        {
                            if (RecoWorkImg != null)
                            {
                                bool LineCheckFlag;
                                LineCheckFlag = false;
                                if (RecoSetData.DXF_RecoType == RECO_TYPE_HOLE)
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
                                    if (RecoCls.FindAngleWorkOKFlag == false)
                                    {
                                        SaveErrorAlignImg();
                                    }
                                }
                                else if (RecoSetData.DXF_RecoType == RECO_TYPE_HOLE_ONELINE)
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
                                else if (RecoSetData.DXF_RecoType == RECO_TYPE_LINE)
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
                                    //if (LineCheckFlag == true)
                                    {
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
                                    if (RecoCls.FindAngleWorkOKFlag == false)
                                    {
                                        SaveErrorAlignImg();
                                    }
                                }



                                IndexFindRoAngle = RecoCls.GetFullAngle;
                                IndexFindRoAngle = 0 - IndexFindRoAngle;
                                picRecoImg.Image = RecoCls.GetArrObjectImage();
                            }
                        }
                        else if (TopAlignProcFlag == true)
                        {
                            if (RecoWorkImg != null)
                            {
                                RecoCls = new clsRecoProcess(ref RecoWorkImg, CamOpticalGap_X[CamOpticalGapIndex], CamOpticalGap_Y[CamOpticalGapIndex]);
                                RecoCls.FindEdge2(1);
                                RecoCls.CutOffFindEdge2();
                                RecoCls.FindObject();
                                RecoCls.FindCircleObjSizeArc();
                                RecoCls.FindCircleDetailShot();

                                IndexFindRoAngle = RecoCls.FindTopCenterAngle(RecoSetData.Object_Pin_Height + RecoSetData.Object_Diameter, RecoSetData.Object_Pin_Diameter * 1.36);//18 + 6.35, 8 * 2);
                                picRecoImg.Image = RecoCls.GetArrObjectImage();
                            }
                        }
                        else
                        {

                            if (RecoWorkImg != null)
                            {
                                bool LineCheckFlag;
                                LineCheckFlag = false;
                                if (RecoSetData.DXF_RecoType == RECO_TYPE_HOLE)
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
                                    if (RecoCls.FindAngleWorkOKFlag == false)
                                    {
                                        SaveErrorAlignImg();
                                    }
                                }
                                else if (RecoSetData.DXF_RecoType == RECO_TYPE_HOLE_ONELINE)
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
                                else if (RecoSetData.DXF_RecoType == RECO_TYPE_LINE)
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
                                    //if (LineCheckFlag == true)
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
                                    if (RecoCls.FindAngleWorkOKFlag == false)
                                    {
                                        SaveErrorAlignImg();
                                    }
                                }

                                
                                IndexFindRoAngle = RecoCls.GetFullAngle;
                                picRecoImg.Image = RecoCls.GetArrObjectImage();
                            }
                        }

                        AutoScaleBox.Visible = false;
                        picRecoImg.Visible = true;
                        LpWk_ZeroCount = 1;
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
                    DelayWaitRun(DelayTimerLoopTime);
                }

                while(LpWk_RUN_STATUS == INDEXROTATE_FULL)
                {
                    if (LocalTestProcFlag == false)
                    {
                        CameraPlayerStop();
                        CameraOneClose();
                        return false;
                    }
                    //label5.Text = "INDEX회전";
                    if (LpWk_ZeroCount == 0)
                    {
                        MultiMotion.GetCurrentPos();
                        IndexFindRoAngle = IndexRotationHalf(IndexFindRoAngle);
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
                        MultiMotion.MoveAxis(RotateIndex_Index, IndexRoAngle, false);

                        //MultiMotion.MoveAxis(0, 10.0, true);  // 완료 후 리턴

                        //MultiMotion.MoveAxis(0, -10.0, true); // 완료 후 리턴

                        //MultiMotion.RotateAxis(0, 10.0);
                        //=============================================
                        AutoScaleBox.Visible = true;
                        picRecoImg.Visible = false;
                        while (Math.Abs(MultiMotion.AxisValue[RotateIndex_Index] - IndexRoAngle) > 1.0)
                        {
                            if (LocalTestProcFlag == false)
                            {
                                CameraPlayerStop();
                                CameraOneClose();
                                return false;
                            }
                            MultiMotion.GetCurrentPos();
                            DelayWaitRun(5);
                        }
                        DelayWaitRun(5);

                        LpWk_ZeroCount = 1;

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
                    DelayWaitRun(DelayTimerLoopTime);
                }

                while(LpWk_RUN_STATUS == FULLRECOPROC2)
                {
                    if (LocalTestProcFlag == false)
                    {
                        CameraPlayerStop();
                        CameraOneClose();
                        return false;
                    }
                    //label5.Text = "인식2";
                    if (LpWk_ZeroCount == 0)
                    {
                        /*
                        //picRecoImg.Image = RealScaleBox.Image;
                        //RecoBlankImg = RealScaleBox.Image;
                        RecoWorkImg = RecoBlankImg;
                        //AutoScaleBox.Image.Save("001_2.jpg");
                        if (RecoWorkImg != null)
                        {
                            RecoCls = new clsRecoProcess(ref RecoWorkImg);
                            RecoCls.FindEdge(0);
                            RecoCls.CutOffFindEdge();
                            RecoCls.FindObject();
                            RecoCls.FindCircleArc();
                            RecoCls.FindObjectCenterAngle();
                            picRecoImg.Image = RecoCls.GetArrObjectImage();
                        }
                        */
                        RecoWorkImg = RecoBlankImg;
                        //RealScaleBox.Image.Save("001_2.jpg");
                        if (BackAlignProcFlag == true)
                        {
                            if (RecoWorkImg != null)
                            {
                                bool LineCheckFlag;
                                LineCheckFlag = false;
                                if (RecoSetData.DXF_RecoType == RECO_TYPE_HOLE)
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
                                    RecoCls.FindObjectCenterAngle5();
                                    if (RecoCls.FindAngleWorkOKFlag == false)
                                    {
                                        SaveErrorAlignImg();
                                    }
                                }
                                else if (RecoSetData.DXF_RecoType == RECO_TYPE_HOLE_ONELINE)
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
                                    if (RecoCls.FindAngleWorkOKFlag == false)
                                    {
                                        SaveErrorAlignImg();
                                    }
                                }
                                else if (RecoSetData.DXF_RecoType == RECO_TYPE_LINE)
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
                                    if (RecoCls.FindAngleWorkOKFlag == false)
                                    {
                                        SaveErrorAlignImg();
                                    }
                                }

                                
                                IndexFindRoAngle = RecoCls.GetFullAngle;
                                IndexFindRoAngle = 0 - IndexFindRoAngle;
                                picRecoImg.Image = RecoCls.GetArrObjectImage();
                            }
                        }
                        else if (TopAlignProcFlag == true)
                        {
                            if (RecoWorkImg != null)
                            {
                                RecoCls = new clsRecoProcess(ref RecoWorkImg, CamOpticalGap_X[CamOpticalGapIndex], CamOpticalGap_Y[CamOpticalGapIndex]);
                                RecoCls.FindEdge2(1);
                                RecoCls.CutOffFindEdge2();
                                RecoCls.FindObject();
                                RecoCls.FindCircleObjSizeArc();
                                RecoCls.FindCircleDetailShot();

                                IndexFindRoAngle = RecoCls.FindTopCenterAngle(RecoSetData.Object_Pin_Height + RecoSetData.Object_Diameter, RecoSetData.Object_Pin_Diameter * 1.36);//18 + 6.35, 8 * 2);
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
                                if (RecoSetData.DXF_RecoType == RECO_TYPE_HOLE)
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
                                    RecoCls.FindObjectCenterAngle5();
                                    if (RecoCls.FindAngleWorkOKFlag == false)
                                    {
                                        SaveErrorAlignImg();
                                    }
                                }
                                else if (RecoSetData.DXF_RecoType == RECO_TYPE_HOLE_ONELINE)
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
                                    if (RecoCls.FindAngleWorkOKFlag == false)
                                    {
                                        SaveErrorAlignImg();
                                    }
                                }
                                else if (RecoSetData.DXF_RecoType == RECO_TYPE_LINE)
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
                                    if (RecoCls.FindAngleWorkOKFlag == false)
                                    {
                                        SaveErrorAlignImg();
                                    }
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
                            }
                        }

                        AutoScaleBox.Visible = false;
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
                    DelayWaitRun(DelayTimerLoopTime);
                }

                while(LpWk_RUN_STATUS == INDEXROTATE_FULL2)
                {
                    if (LocalTestProcFlag == false)
                    {
                        CameraPlayerStop();
                        CameraOneClose();
                        return false;
                    }
                    //label5.Text = "INDEX회전2";
                    if (LpWk_ZeroCount == 0)
                    {
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
                        MultiMotion.MoveAxis(RotateIndex_Index, IndexRoAngle, false);

                        //MultiMotion.MoveAxis(0, 10.0, true);  // 완료 후 리턴

                        //MultiMotion.MoveAxis(0, -10.0, true); // 완료 후 리턴

                        //MultiMotion.RotateAxis(0, 10.0);
                        //=============================================
                        AutoScaleBox.Visible = true;
                        picRecoImg.Visible = false;
                        while (Math.Abs(MultiMotion.AxisValue[RotateIndex_Index] - IndexRoAngle) > 1.0)
                        {
                            if (LocalTestProcFlag == false)
                            {
                                CameraPlayerStop();
                                CameraOneClose();
                                return false;
                            }
                            MultiMotion.GetCurrentPos();
                            DelayWaitRun(5);
                        }
                        DelayWaitRun(5);


                        LpWk_ZeroCount = 1;

                    }
                    if (LpWk_ZeroCount > ZeroLoopCountChkMax)
                    {
                        picRecoImg.Visible = false;
                        AutoScaleBox.Visible = true;
                        AutoScaleBox.Refresh();
                        LpWk_RUN_STATUS = DETAILMOTIONMOVE;
                        LpWk_ZeroCount = 0;
                    }
                    else
                    {
                        LpWk_ZeroCount++;
                    }
                    DelayWaitRun(DelayTimerLoopTime);
                }

                while(LpWk_RUN_STATUS == DETAILMOTIONMOVE)
                {
                    if (LocalTestProcFlag == false)
                    {
                        CameraPlayerStop();
                        CameraOneClose();
                        return false;
                    }
                    ZoomFocusRunFunc();

                    if (TopAlignProcFlag == true)
                    {
                        if (RecoSetData.Object_TopHoleType == 0)
                        {
                            SetValueLamp(_WorkFuncInfo.Lamp_Index, 0);

                            /*
                            if (XCCam.ImageStart())
                            {
                                if (!XCCam.ImageStop())
                                {
                                    //MessageBox.Show("Image Stop Error");
                                }
                                XCCam.SetImageCallBack();
                                if (!XCCam.ResourceRelease())
                                {
                                    MessageBox.Show("Resource Release Error");
                                }
                                DispExec.Stop();
                                Frame.Stop();
                            }
                            */
                            CameraPlayerStop();
                            CameraOneClose();

                            AutoScaleBox.Visible = false;
                            picRecoImg.Visible = false;

                            return ResSucessFlag;
                        }
                    }

                    if (RecoSetData.DXF_RecoType == RECO_TYPE_LINE)
                    {
                        SetValueLamp(_WorkFuncInfo.Lamp_Index, 0);

                        /*
                        if (XCCam.ImageStart())
                        {
                            if (!XCCam.ImageStop())
                            {
                                //MessageBox.Show("Image Stop Error");
                            }
                            XCCam.SetImageCallBack();
                            if (!XCCam.ResourceRelease())
                            {
                                MessageBox.Show("Resource Release Error");
                            }
                            DispExec.Stop();
                            Frame.Stop();
                        }
                        */
                        CameraPlayerStop();
                        CameraOneClose();

                        AutoScaleBox.Visible = false;
                        picRecoImg.Visible = false;

                        return ResSucessFlag;
                    }

                    //label5.Text = "DETAIL_MOTION이동";
                    if (LpWk_ZeroCount == 0)
                    {

                        if (BackAlignProcFlag == true)
                        {
                            GetDetailSelObj = GetSelectDXFData();
                            Detail_Move_Pos_X = 0;
                            //Detail_Move_Pos_Y = 0 - GetDetailSelObj.Y;
                            Detail_Move_Pos_Z = GetDetailSelObj.Z;

                            MultiMotion.GetCurrentPos();

                            TmpMoveAxis_Index = MultiMotion.BACK_CAM_Z;
                            TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
                            SetMoveValueAxis = TmpMoveValueAxis - Detail_Move_Pos_Z;
                            RunMotionAxis_Back_Z(SetMoveValueAxis);
                        }
                        else if (TopAlignProcFlag == true)
                        {
                            Detail_Move_Pos_X = RecoSetData.Object_Hole_Distance;

                            MultiMotion.GetCurrentPos();
                            TmpMoveAxis_Index = MultiMotion.CAM_UNIT_X;
                            TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
                            SetMoveValueAxis = TmpMoveValueAxis - Detail_Move_Pos_X;
                            RunMotionAxis_X(SetMoveValueAxis, true);
                        }
                        else
                        {
                            GetDetailSelObj = GetSelectDXFData();
                            Detail_Move_Pos_X = 0;
                            Detail_Move_Pos_Y = 0 - GetDetailSelObj.Y;
                            Detail_Move_Pos_Z = GetDetailSelObj.Z;


                            if (ReAlignIndex == RE_ALIGN_TRANSLATION)
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
                            RunMotionAxis_Y(SetMoveValueAxis, false);


                            TmpMoveAxis_Index = MultiMotion.CAM_UNIT_Z;
                            TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
                            SetMoveValueAxis = TmpMoveValueAxis - Detail_Move_Pos_Z;
                            RunMotionAxis_Z(SetMoveValueAxis, true);
                        }



                        LpWk_ZeroCount = 1;

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
                    DelayWaitRun(DelayTimerLoopTime);
                }

                while(LpWk_RUN_STATUS == GODETAILSHOT)
                {
                    if (LocalTestProcFlag == false)
                    {
                        CameraPlayerStop();
                        CameraOneClose();
                        return false;
                    }
                    ZoomFocusRunFunc();

                    //label5.Text = "DETAILSHOT보기";
                    if (LpWk_ZeroCount == 0)
                    {
                        //SelectDXFReScale(AutoScaleBox.Image.Width, AutoScaleBox.Image.Height);

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
                        if (NavitarCtrl.ProductID > 0)
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
                    DelayWaitRun(DelayTimerLoopTime);
                }

                while(LpWk_RUN_STATUS == DETAILRECOPROC)
                {
                    if (LocalTestProcFlag == false)
                    {
                        CameraPlayerStop();
                        CameraOneClose();
                        return false;
                    }
                    //label5.Text = "세부인식";
                    if (LpWk_ZeroCount == 0)
                    {
                        /*
                        //picRecoImg.Image = RealScaleBox.Image;
                        //RecoBlankImg = RealScaleBox.Image;
                        RecoWorkImg = RecoBlankImg;
                        //AutoScaleBox.Image.Save("001_3.jpg");
                        if (RecoWorkImg != null)
                        {
                            RecoCls = new clsRecoProcess(ref RecoWorkImg);
                            RecoCls.FindEdge(0);
                            RecoCls.CutOffFindEdge();
                            RecoCls.FindObject();
                            RecoCls.FindCircleArc();
                            RecoCls.FindObjectCenterAngle();
                            picRecoImg.Image = RecoCls.GetArrObjectImage();
                        }

                        AutoScaleBox.Visible = false;
                        picRecoImg.Visible = true;
                        */
                        RecoWorkImg = RecoBlankImg;
                        //RealScaleBox.Image.Save("001_3.jpg");
                        if (BackAlignProcFlag == true)
                        {
                            if (RecoWorkImg != null)
                            {
                                DXFLayerInfo FindDXFSelObj;
                                FindDXFSelObj = FindSelectDXFObjList();
                                if (FindDXFSelObj.ExistLyFlag == true)
                                {
                                    RecoCls = new clsRecoProcess(ref RecoWorkImg, CamOpticalGap_X[CamOpticalGapIndex], CamOpticalGap_Y[CamOpticalGapIndex]);
                                    //RecoCls.FindEdge2(1);
                                    RecoCls.FindEdge_BackShadow(1);
                                    RecoCls.CutOffFindEdge2();
                                    RecoCls.FindObject();
                                    RecoCls.FindCircleObjSizeArc();
                                    RecoCls.FindCircleDetailShot();
                                    IndexFindRoAngle = RecoCls.FindDetailCenterAngle(FindDXFSelObj.Obj_X1, FindDXFSelObj.Obj_Y1, FindDXFSelObj.Obj_Dia);
                                    IndexFindRoAngle = 0 - IndexFindRoAngle;
                                    picRecoImg.Image = RecoCls.GetArrDetailObjectImage(IndexFindRoAngle);
                                }
                            }
                        }
                        else if (TopAlignProcFlag == true)
                        {
                            if (RecoWorkImg != null)
                            {
                                RecoCls = new clsRecoProcess(ref RecoWorkImg, CamOpticalGap_X[CamOpticalGapIndex], CamOpticalGap_Y[CamOpticalGapIndex]);
                                int LeftRightType = RecoCls.FindCenterLR(0);
                                bool DirectionCorrectFlag = false;

                                if (RecoSetData.Object_TopHoleType == 1)
                                {
                                    if (LeftRightType == -1)
                                    {
                                        MessageBox.Show("방향이 맞습니다!");
                                        DirectionCorrectFlag = true;
                                    }
                                    else
                                    {
                                        MessageBox.Show("방향이 틀립니다!");
                                        DirectionCorrectFlag = false;
                                    }
                                }
                                else if (RecoSetData.Object_TopHoleType == 2)
                                {
                                    if (LeftRightType == 1)
                                    {
                                        MessageBox.Show("방향이 맞습니다!");
                                        DirectionCorrectFlag = true;
                                    }
                                    else
                                    {
                                        MessageBox.Show("방향이 틀립니다!");
                                        DirectionCorrectFlag = false;
                                    }
                                }
                                else
                                {
                                    DirectionCorrectFlag = false;
                                }
                                
                                ResSucessFlag = DirectionCorrectFlag;

                                SetValueLamp(_WorkFuncInfo.Lamp_Index, 0);
                                /*
                                if (XCCam.ImageStart())
                                {
                                    if (!XCCam.ImageStop())
                                    {
                                        //MessageBox.Show("Image Stop Error");
                                    }
                                    XCCam.SetImageCallBack();
                                    if (!XCCam.ResourceRelease())
                                    {
                                        MessageBox.Show("Resource Release Error");
                                    }
                                    DispExec.Stop();
                                    Frame.Stop();
                                }
                                */
                                CameraPlayerStop();
                                CameraOneClose();

                                AutoScaleBox.Visible = false;
                                picRecoImg.Visible = false;

                                return ResSucessFlag;
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
                        }

                        AutoScaleBox.Visible = false;
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
                    DelayWaitRun(DelayTimerLoopTime);
                }

                while(LpWk_RUN_STATUS == INDEXROTATE_DETAIL)
                {
                    if (LocalTestProcFlag == false)
                    {
                        CameraPlayerStop();
                        CameraOneClose();
                        return false;
                    }
                    //label5.Text = "INDEX회전";
                    if (LpWk_ZeroCount == 0)
                    {
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

                        SetAddDisplayAngle(DIS_TYPE_DETAILSHOT, IndexFindRoAngle);
                        //INDEX회전부분
                        //=============================================
                        MultiMotion.MoveAxis(RotateIndex_Index, IndexRoAngle, true);

                        //MultiMotion.MoveAxis(0, 10.0, true);  // 완료 후 리턴

                        //MultiMotion.MoveAxis(0, -10.0, true); // 완료 후 리턴

                        //MultiMotion.RotateAxis(0, 10.0);
                        //=============================================
                        
                        AutoScaleBox.Visible = true;
                        picRecoImg.Visible = false;
                        /*
                        MultiMotion.GetCurrentPos();
                        while (Math.Abs(MultiMotion.AxisValue[RotateIndex_Index] - IndexRoAngle) > 0.1)
                        {
                            if(LocalTestProcFlag == false)
                            {
                                CameraPlayerStop();
                                CameraOneClose();
                                return false;
                            }
                            MultiMotion.GetCurrentPos();
                            DelayWaitRun(5);
                        }
                        */
                        DelayWaitRun(10);

                        LpWk_ZeroCount = 1;


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
                    DelayWaitRun(DelayTimerLoopTime);
                }

                while(LpWk_RUN_STATUS == DETAILRECOPROC2)
                {
                    if (LocalTestProcFlag == false)
                    {
                        CameraPlayerStop();
                        CameraOneClose();
                        return false;
                    }
                    //label5.Text = "세부인식2";
                    if (LpWk_ZeroCount == 0)
                    {
                        /*
                        //picRecoImg.Image = RealScaleBox.Image;
                        //RecoBlankImg = RealScaleBox.Image;
                        RecoWorkImg = RecoBlankImg;
                        //AutoScaleBox.Image.Save("001_4.jpg");
                        if (RecoWorkImg != null)
                        {
                            RecoCls = new clsRecoProcess(ref RecoWorkImg);
                            RecoCls.FindEdge(0);
                            RecoCls.CutOffFindEdge();
                            RecoCls.FindObject();
                            RecoCls.FindCircleArc();
                            RecoCls.FindObjectCenterAngle();
                            picRecoImg.Image = RecoCls.GetArrObjectImage();
                        }
                        */
                        //picRecoImg.Image = RealScaleBox.Image;
                        //RecoBlankImg = RealScaleBox.Image;
                        RecoWorkImg = RecoBlankImg;
                        //RealScaleBox.Image.Save("001_4.jpg");
                        if (BackAlignProcFlag == true)
                        {
                            if (RecoWorkImg != null)
                            {
                                DXFLayerInfo FindDXFSelObj;
                                FindDXFSelObj = FindSelectDXFObjList();
                                if (FindDXFSelObj.ExistLyFlag == true)
                                {
                                    RecoCls = new clsRecoProcess(ref RecoWorkImg, CamOpticalGap_X[CamOpticalGapIndex], CamOpticalGap_Y[CamOpticalGapIndex]);
                                    //RecoCls.FindEdge2(1);
                                    RecoCls.FindEdge_BackShadow(1);
                                    RecoCls.CutOffFindEdge2();
                                    RecoCls.FindObject();
                                    RecoCls.FindCircleObjSizeArc();
                                    RecoCls.FindCircleDetailShot();
                                    IndexFindRoAngle = RecoCls.FindDetailCenterAngle(FindDXFSelObj.Obj_X1, FindDXFSelObj.Obj_Y1, FindDXFSelObj.Obj_Dia);
                                    IndexFindRoAngle = 0 - IndexFindRoAngle;
                                    picRecoImg.Image = RecoCls.GetArrDetailObjectImage(IndexFindRoAngle);
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
                            }
                        }


                        AutoScaleBox.Visible = false;
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
                    DelayWaitRun(DelayTimerLoopTime);
                }

                while(LpWk_RUN_STATUS == INDEXROTATE_DETAIL2)
                {
                    if (LocalTestProcFlag == false)
                    {
                        CameraPlayerStop();
                        CameraOneClose();
                        return false;
                    }
                    //label5.Text = "INDEX회전2";
                    if (LpWk_ZeroCount == 0)
                    {
                        double Add_Index_M_Gap;
                        Add_Index_M_Gap = 0.0547;
                        IndexFindRoAngle = IndexRotationHalf(IndexFindRoAngle);
                        MultiMotion.GetCurrentPos();
                        IndexRoAngle = MultiMotion.AxisValue[RotateIndex_Index];
                        if (RotateIndex_Index == MultiMotion.INDEX_MOVE_R)
                        {
                            if (MoveBackAlignProcFlag == true)
                            {
                                IndexRoAngle = IndexRoAngle - IndexFindRoAngle;// +Add_Index_M_Gap;
                            }
                            else
                            {
                                IndexRoAngle = IndexRoAngle + IndexFindRoAngle;// +Add_Index_M_Gap;
                            }
                        }
                        else
                        {
                            IndexRoAngle = IndexRoAngle - IndexFindRoAngle;
                        }

                        SetAddDisplayAngle(DIS_TYPE_DETAILSHOT, IndexFindRoAngle);
                        //INDEX회전부분
                        //=============================================
                        MultiMotion.MoveAxis(RotateIndex_Index, IndexRoAngle, false);

                        //MultiMotion.MoveAxis(0, 10.0, true);  // 완료 후 리턴

                        //MultiMotion.MoveAxis(0, -10.0, true); // 완료 후 리턴

                        //MultiMotion.RotateAxis(0, 10.0);
                        //=============================================
                        
                        AutoScaleBox.Visible = true;
                        picRecoImg.Visible = false;
                        /*
                        MultiMotion.GetCurrentPos();
                        while (Math.Abs(MultiMotion.AxisValue[RotateIndex_Index] - IndexRoAngle) > 0.1)
                        {
                            if(LocalTestProcFlag == false)
                            {
                                CameraPlayerStop();
                                CameraOneClose();
                                return false;
                            }
                            MultiMotion.GetCurrentPos();
                            DelayWaitRun(5);
                        }
                        */
                        DelayWaitRun(10);

                        LpWk_ZeroCount = 1;

                    }
                    if (LpWk_ZeroCount > ZeroLoopCountChkMax)
                    {
                        picRecoImg.Visible = false;
                        AutoScaleBox.Visible = true;
                        AutoScaleBox.Refresh();
                        LpWk_RUN_STATUS = INDEXROTATE_GAPADD;
                        LpWk_ZeroCount = 0;
                    }
                    else
                    {

                        LpWk_ZeroCount++;
                    }
                    DelayWaitRun(DelayTimerLoopTime);
                }


                while (LpWk_RUN_STATUS == INDEXROTATE_GAPADD)
                {
                    if (LocalTestProcFlag == false)
                    {
                        CameraPlayerStop();
                        CameraOneClose();
                        return false;
                    }
                    //label5.Text = "INDEX보정회전";
                    if (LpWk_ZeroCount == 0)
                    {
                        double DisGapAddAngle = 0.0;
                        double Add_Index_M_Gap;
                        Add_Index_M_Gap = 0.0547;
                        IndexFindRoAngle = IndexRotationHalf(IndexFindRoAngle);
                        MultiMotion.GetCurrentPos();
                        IndexRoAngle = MultiMotion.AxisValue[RotateIndex_Index];
                        if (RecoSetData.RecoCamIndex == 0)
                        {
                            //IndexRoAngle = IndexRoAngle + IndexGapAddRoAngle;
                            IndexRoAngle = IndexRoAngle + RecoSetData.Move_R_FIX_Angle_Gap;
                            DisGapAddAngle = RecoSetData.Move_R_FIX_Angle_Gap;
                            if (ReAlignIndex == RE_ALIGN_ANGLE_DIVIDE)
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
                            //IndexRoAngle = IndexRoAngle + IndexGapAddRoAngle;
                            IndexRoAngle = IndexRoAngle + RecoSetData.Move_R_MOVE_Angle_Gap;
                            DisGapAddAngle = RecoSetData.Move_R_MOVE_Angle_Gap;
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
                        SetAddDisplayAngle(DIS_TYPE_ADDANGLE, DisGapAddAngle);
                        //INDEX회전부분
                        //=============================================
                        MultiMotion.MoveAxis(RotateIndex_Index, IndexRoAngle, false);

                        //MultiMotion.MoveAxis(0, 10.0, true);  // 완료 후 리턴

                        //MultiMotion.MoveAxis(0, -10.0, true); // 완료 후 리턴

                        //MultiMotion.RotateAxis(0, 10.0);
                        //=============================================

                        AutoScaleBox.Visible = true;
                        picRecoImg.Visible = false;
                        /*
                        MultiMotion.GetCurrentPos();
                        while (Math.Abs(MultiMotion.AxisValue[RotateIndex_Index] - IndexRoAngle) > 0.1)
                        {
                            if(LocalTestProcFlag == false)
                            {
                                CameraPlayerStop();
                                CameraOneClose();
                                return false;
                            }
                            MultiMotion.GetCurrentPos();
                            DelayWaitRun(5);
                        }
                        */
                        DelayWaitRun(10);

                        LpWk_ZeroCount = 1;

                    }
                    if (LpWk_ZeroCount > ZeroLoopCountChkMax)
                    {
                        picRecoImg.Visible = false;
                        AutoScaleBox.Visible = true;
                        AutoScaleBox.Refresh();
                        LpWk_RUN_STATUS = RECOWORKEND;
                        LpWk_ZeroCount = 0;
                    }
                    else
                    {

                        LpWk_ZeroCount++;
                    }
                    DelayWaitRun(DelayTimerLoopTime);
                }


                while (LpWk_RUN_STATUS == RECOWORKEND)
                {
                    if (LocalTestProcFlag == false)
                    {
                        CameraPlayerStop();
                        CameraOneClose();
                        return false;
                    }
                    //label5.Text = "테스트끝";
                    if (LpWk_ZeroCount == ZeroLoopCountChkMax_Shot)
                    {
                        //RealScaleBox.Image.Save("shot" + LpWk_LoopCount.ToString() + ".jpg");
                        LpWk_LoopCount++;
                    }
                    if (LpWk_ZeroCount > ZeroLoopCountChkMax)
                    {
                        LpWk_RUN_STATUS = RECOWORKSTART;
                        LpWk_ZeroCount = 0;
                    }
                    else
                    {
                        LpWk_ZeroCount++;
                    }
                    DelayWaitRun(DelayTimerLoopTime);
                }

                if (LpWk_LoopCount >= LpWk_LoopCountMax)
                {
                    //label5.Text = "";
                    //LpWk_RunFlag = false;
                    //timWork.Enabled = false;
                   // btLoopStart.Enabled = true;
                    //btLoopStop.Enabled = false;
                    //MessageBox.Show("테스트종료!");
                }
            }
            //LpWk_RunTimerCount++;

            SetValueLamp(_WorkFuncInfo.Lamp_Index, 0);

            /*
            if (XCCam.ImageStart())
            {
                if (!XCCam.ImageStop())
                {
                    //MessageBox.Show("Image Stop Error");
                }
                XCCam.SetImageCallBack();
                if (!XCCam.ResourceRelease())
                {
                    MessageBox.Show("Resource Release Error");
                }
                DispExec.Stop();
                Frame.Stop();
            }
            */
            CameraPlayerStop();
            CameraOneClose();



            AutoScaleBox.Visible = false;
            picRecoImg.Visible = false;

            ResSucessFlag = true;

            return ResSucessFlag;

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

        private bool CalibrationRun()
        {
            WorkFuncInfo _WorkFuncInfo;
            bool ResSucessFlag;
            string LoadRecoCfgFileStr;
            string LoadMainFolder;
            bool Disp_Chcek;
            string ZF_ComPortStr;
            string CfgSaveFolderPath;
            string CfgSaveHeadFileName;
            bool TopAlignProcFlag = false;

            CalibrationRuninngFlag = true;
            AutoScaleBox.Visible = true;
            picRecoImg.Visible = false;

            PortConnect();

            LoadCalibPosSetFiles(SetCalibPosDataFileName);

            //_WorkFuncInfo = DataManager.TestProcList[CurTestProcIndex];
            _WorkFuncInfo.WFRecoDatHeadFileName = "";
            _WorkFuncInfo.Lamp_Index = 0;
            _WorkFuncInfo.WFLampValue = 500;
            _WorkFuncInfo.Rotation_Index = 0;



            picRecoImg.Top = AutoScaleBox.Top;
            picRecoImg.Left = AutoScaleBox.Left;
            picRecoImg.Width = AutoScaleBox.Width;
            picRecoImg.Height = AutoScaleBox.Height;


            CfgSaveFolderPath = ConfigManager.GetModelFilePath + DataManager.GetModelSelectFileNameFolder();
            CfgSaveHeadFileName = _WorkFuncInfo.WFRecoDatHeadFileName;

            int idx;

            ResSucessFlag = false;

            //LoadMainFolder = ConfigManager.GetModelFilePath + DataManager.GetModelSelectFileNameFolder() + @"\";
            LoadMainFolder = CfgSaveFolderPath;

            LoadRecoCfgFileStr = CfgSaveFolderPath + @"\" + CfgSaveHeadFileName + @".dat";

            /*
            if (File.Exists(LoadRecoCfgFileStr) == false)
            {
                ClearRecoViewObject();
                CameraPlayerStop();
                CameraOneClose();
                return false;
            }
            */

            //LoadCfgFiles(LoadRecoCfgFileStr);
            RecoSetData.RecoDXFFileName = "";
            RecoSetData.RecoCapFileName = "";
            RecoSetData.RecoCamIndex = 0;
            RecoSetData.RecoCamID = "";
            RecoSetData.ZoomFocus_PortNo = 0;
            RecoSetData.FullShot_ZoomNum = 11762;
            RecoSetData.FullShot_FocusNum = 21422;
            RecoSetData.DetailShot_ZoomNum = 11762;
            RecoSetData.DetailShot_FocusNum = 21422;
            RecoSetData.Rotation_Index = 0;
            RecoSetData.Rotation_R = 0;
            RecoSetData.Motion_Index = 0;
            RecoSetData.Motion_Move_X = 859.9980;
            RecoSetData.Motion_Move_Y = 109.138;// 111.1156;// 107.8558;
            RecoSetData.Motion_Move_Z = 363.0792;// 364.5068;
            RecoSetData.Lamp_Index = 0;
            RecoSetData.Lamp_PortNo = 0;
            RecoSetData.Lamp_SetChannel = 0;
            RecoSetData.Lamp_SetValue = _WorkFuncInfo.WFLampValue;

            Calibration_Origin.ddp_X = RecoSetData.Motion_Move_X;
            Calibration_Origin.ddp_Y = RecoSetData.Motion_Move_Y;
            Calibration_Origin.ddp_Z = RecoSetData.Motion_Move_Z;

            idx = FindSelectUIDList(DataManager.CameraSettingInfoList[RecoSetData.RecoCamIndex].IP);// RecoSetData.RecoCamIndex;
            Disp_Chcek = false;

            if (CamTotalNum > idx)
            {
            }
            else
            {
                ClearRecoViewObject();
                return false;
            }

            if (CameraOneOpen(CUID[idx]) == true)
            {
            }
            else
            {
                ClearRecoViewObject();
                return false;
            }

            if (RecoSetData.RecoCamIndex == 3)
            {
                TopAlignProcFlag = true;
            }
            else
            {
                TopAlignProcFlag = false;
            }

            CamOpticalGapIndex = RecoSetData.RecoCamIndex;

            ScreenFlipFlag = GetScreenFlip(RecoSetData.RecoCamIndex);

            Dislay_FPS = 10;//30;//10-100;

            //XCCam.SetValue("width", "640");
            //XCCam.SetValue("height", "480");

            if (!XCCam.ResourceAlloc())
            {
                //MessageBox.Show("Resource Alloc Error");
                ClearRecoViewObject();
                return false;
            }

            if (!XCCam.GetImageInfo(out ImageInfo))
            {
                XCCam.ResourceRelease();
                //MessageBox.Show("Get ImageInfo Error");
                ClearRecoViewObject();
                return false;
            }

            if (Disp_Chcek)
            {
                //PanelPicture.Visible = true;
                //AutoScaleBox.Visible = false;
            }
            else
            {
                //PanelPicture.Visible = true;
                //AutoScaleBox.Visible = false;

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
                //MessageBox.Show("Image Start Error");
                XCCam.SetImageCallBack();
                DispExec.Stop();
                Frame.Stop();
                XCCam.ResourceRelease();
                ClearRecoViewObject();
                return false;
            }

            Disp_Flag = false;

            //DXF도면 로드
            string LoadDXFLocalFileStr;
            /*
            if (TopAlignProcFlag == false)
            {
                if (CfgSaveFolderPath.Length > 0)
                {
                    StartCalcFlag = true;
                    FCADImage = new CADImage();
                    Base.X = 1;
                    Base.Y = 1;
                    FScale = 1.0f;
                    //timer1.Interval = 600;
                    //timer1.Enabled = true;
                    LoadDXFLocalFileStr = CfgSaveFolderPath + @"\" + RecoSetData.RecoDXFFileName;
                    if (File.Exists(LoadDXFLocalFileStr) == true)
                    {
                        FCADImage.LoadFromFile(LoadDXFLocalFileStr);
                    }
                    else
                    {
                        ClearRecoViewObject();
                        CameraPlayerStop();
                        CameraOneClose();
                        return false;
                    }
                }
            }
            */

            SetValueLamp(_WorkFuncInfo.Lamp_Index, _WorkFuncInfo.WFLampValue);


            //Navitar초기화

            ZF_ComPortStr = "COM" + DataManager.CameraSettingInfoList[RecoSetData.RecoCamIndex].ZoomFocusPort.ToString();

            ShotData[FULLSHOTTYPE].ZoomNum = RecoSetData.FullShot_ZoomNum;
            ShotData[FULLSHOTTYPE].FocusNum = RecoSetData.FullShot_FocusNum;
            ShotData[DETAILSHOTTYPE].ZoomNum = RecoSetData.DetailShot_ZoomNum;
            ShotData[DETAILSHOTTYPE].FocusNum = RecoSetData.DetailShot_FocusNum;


            Application.DoEvents();
            Application.DoEvents();

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

            NavitarCtrl = new ControllerLegacy(ZF_ComPortStr);
            NavitarCtrl.Connect();


            if (NavitarCtrl.ProductID > 0)
            {
                ZoomIn_Max = NavitarCtrl.Read(REG_SETUP_LIMIT_1);
                Focus_Max = NavitarCtrl.Read(REG_SETUP_LIMIT_2);
                ZoomIn_Min = 0;
                Focus_Min = 0;

                ZoomFactNum = NavitarCtrl.Read(REG_USER_CURRENT_1);
                FocusFactNum = NavitarCtrl.Read(REG_USER_CURRENT_2);

                for (int i = 0; i < 10; i++)
                {
                    ZoomFocusRunFunc();
                    DelayWaitRun(5);
                }
                DelayWaitRun(10);

            }
            else
            {
                ClearRecoViewObject();
                CameraPlayerStop();
                CameraOneClose();
                return false;
            }

            // 동작 프로세스

            int Cur_Temp_Zoom_Status, Cur_Temp_Focus_Status;
            int ZeroLoopCountChkMax = 10;
            int ZeroLoopCountChkMax_Shot = 5;
            int DelayTimerLoopTime = 1;

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


            if (_WorkFuncInfo.Rotation_Index == 0)
            {
                RotateIndex_Index = MultiMotion.INDEX_FIX_R;
            }
            else
            {
                RotateIndex_Index = MultiMotion.INDEX_MOVE_R;
            }


            LpWk_LoopCount = 0;
            LpWk_LoopCountMax = 1;
            LpWk_Zoom = ZoomFactNum;// Int32.Parse(textBox1.Text);
            LpWk_Focus = FocusFactNum;// Int32.Parse(textBox2.Text);

            LpWk_RunTimerCount = 0;
            LpWk_RunFlag = true;
            LpWk_RUN_STATUS = WORKSTART;


            if (LpWk_RunFlag == true)
            {
                LpWk_RUN_STATUS = RECOWORKSTART;
                while (LpWk_RUN_STATUS == RECOWORKSTART)
                {
                    if (CalibrationRuninngFlag == false)
                    {
                        CameraPlayerStop();
                        CameraOneClose();
                        return false;
                    }
                    LpWk_RUN_STATUS = MOTIONMOVE;
                    LpWk_ZeroCount = 0;
                    //label5.Text = "RECOWORKSTART";
                    DelayWaitRun(DelayTimerLoopTime);
                    MultiMotion.IndexRPosClear();
                    MultiMotion.SetSpeed(MultiMotion.KSM_SPEED_FAST);
                }

                while (LpWk_RUN_STATUS == MOTIONMOVE)
                {
                    if (CalibrationRuninngFlag == false)
                    {
                        CameraPlayerStop();
                        CameraOneClose();
                        return false;
                    }
                    ZoomFocusRunFunc();
                    //label5.Text = "MOTION이동";
                    if (LpWk_ZeroCount == 0)
                    {
                        LpWk_ZeroCount = 1;
                        MultiMotion.GetCurrentPos();

                        //HOME이동부분
                        //=============================================
                        MultiMotion.HomeMove(MultiMotion.CAM_UNIT_Z, true);
                        MultiMotion.HomeMove(MultiMotion.CAM_UNIT_Y, true);
                        MultiMotion.HomeMove(MultiMotion.CAM_UNIT_X, true);

                        //MOTION이동부분
                        //=============================================
                        RunMotionAxis_X(RecoSetData.Motion_Move_X, true);
                        RunMotionAxis_Y(RecoSetData.Motion_Move_Y, true);
                        RunMotionAxis_Z(RecoSetData.Motion_Move_Z, true);

                        //MultiMotion.RotateAxis(0, 10.0);
                        //=============================================

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
                    DelayWaitRun(DelayTimerLoopTime);
                }

                while (LpWk_RUN_STATUS == GOFULLSHOT)
                {
                    if (CalibrationRuninngFlag == false)
                    {
                        CameraPlayerStop();
                        CameraOneClose();
                        return false;
                    }
                    ZoomFocusRunFunc();

                    //label5.Text = "FULLSHOT보기";
                    if (LpWk_ZeroCount == 0)
                    {
                        if (CalibrationRuninngFlag == false)
                        {
                            CameraPlayerStop();
                            CameraOneClose();
                            return false;
                        }

                        StartCalcFlag = true;

                        //Base.X = AutoScaleBox.Image.Width / 2;
                        //Base.Y = AutoScaleBox.Image.Height / 2;

                        BaseDefault.X = AutoScaleBox.Image.Width / 2;
                        BaseDefault.Y = AutoScaleBox.Image.Height / 2;
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


                        DrawDXFImageCalc(AutoScaleBox.Image.Width, AutoScaleBox.Image.Height, false);


                        CurZoomFactNum = ShotData[FULLSHOTTYPE].ZoomNum;
                        CurFocusFactNum = ShotData[FULLSHOTTYPE].FocusNum;
                        LpWk_ZeroCount = 1;
                    }
                    if (LpWk_ZeroCount > ZeroLoopCountChkMax)
                    {
                        LpWk_RUN_STATUS = CALIB_RECOPROCESS;
                        LpWk_ZeroCount = 0;
                    }
                    else
                    {
                        if (CalibrationRuninngFlag == false)
                        {
                            CameraPlayerStop();
                            CameraOneClose();
                            return false;
                        }
                        if (NavitarCtrl.ProductID > 0)
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
                    DelayWaitRun(DelayTimerLoopTime);
                }

                for (int Calib_n = 0; Calib_n < Calib_Run_Count;Calib_n++ )
                {
                    if (CalibrationRuninngFlag == false)
                    {
                        CameraPlayerStop();
                        CameraOneClose();
                        return false;
                    }
                    while (LpWk_RUN_STATUS == CALIB_RECOPROCESS)
                    {
                        if (CalibrationRuninngFlag == false)
                        {
                            CameraPlayerStop();
                            CameraOneClose();
                            return false;
                        }
                        //label5.Text = "인식";
                        if (LpWk_ZeroCount == 0)
                        {
                            RecoWorkImg = RecoBlankImg;
                            //RealScaleBox.Image.Save("001_1.jpg");

                            if (RecoWorkImg != null)
                            {
                                RecoCls = new clsRecoProcess(ref RecoWorkImg, CamOpticalGap_X[CamOpticalGapIndex], CamOpticalGap_Y[CamOpticalGapIndex]);
                                RecoCls.FindEdge2(1);
                                RecoCls.CutOffFindEdge2();
                                RecoCls.FindObject();
                                RecoCls.FindCircleObjSizeArc();
                                DoubleDataPosInfo FindCalbCenterPos = RecoCls.FindCalibrationCenter();
                                CameraMoveXYPos = RecoCls.FindCalibrationCenterAlign(10);
                                picRecoImg.Image = RecoCls.GetArrCalibrationCenterObjectImage(CameraMoveXYPos);
                            }
                            AutoScaleBox.Visible = false;
                            picRecoImg.Visible = true;
                            LpWk_ZeroCount = 1;
                        }
                        if (LpWk_ZeroCount > ZeroLoopCountChkMax)
                        {
                            LpWk_RUN_STATUS = CALIB_MOV_POSITION;
                            LpWk_ZeroCount = 0;
                        }
                        else
                        {
                            LpWk_ZeroCount++;
                        }
                        DelayWaitRun(DelayTimerLoopTime);
                    }

                    DelayWaitRun(10);

                    while (LpWk_RUN_STATUS == CALIB_MOV_POSITION)
                    {
                        if (CalibrationRuninngFlag == false)
                        {
                            CameraPlayerStop();
                            CameraOneClose();
                            return false;
                        }
                        if (LpWk_ZeroCount == 0)
                        {
                            AutoScaleBox.Visible = true;
                            picRecoImg.Visible = false;
                            if (CameraMoveXYPos.ddp_ExistFlag == true)
                            {
                                MultiMotion.GetCurrentPos();
                                TmpMoveAxis_Index = MultiMotion.CAM_UNIT_X;
                                TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
                                Calibration_Current.ddp_X = TmpMoveValueAxis;

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
                                RunMotionAxis_Y(SetMoveValueAxis, true);
                                Calibration_Current.ddp_Y = SetMoveValueAxis;


                                TmpMoveAxis_Index = MultiMotion.CAM_UNIT_Z;
                                TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
                                SetMoveValueAxis = TmpMoveValueAxis - CameraMoveXYPos.ddp_Y;
                                RunMotionAxis_Z(SetMoveValueAxis, true);
                                Calibration_Current.ddp_Z = SetMoveValueAxis;

                            }

                            LpWk_ZeroCount = 1;

                        }
                        if (LpWk_ZeroCount > ZeroLoopCountChkMax)
                        {
                            LpWk_RUN_STATUS = CALIB_RECOPROCESS;
                            LpWk_ZeroCount = 0;
                        }
                        else
                        {
                            LpWk_ZeroCount++;

                        }
                        DelayWaitRun(DelayTimerLoopTime);
                    }
                    DelayWaitRun(10);

                }

                LpWk_RUN_STATUS = RECOWORKEND;

                AutoScaleBox.Visible = true;
                picRecoImg.Visible = false;

                while (LpWk_RUN_STATUS == RECOWORKEND)
                {
                    if (CalibrationRuninngFlag == false)
                    {
                        CameraPlayerStop();
                        CameraOneClose();
                        return false;
                    }
                    //label5.Text = "테스트끝";
                    if (LpWk_ZeroCount == ZeroLoopCountChkMax_Shot)
                    {
                        //RealScaleBox.Image.Save("shot" + LpWk_LoopCount.ToString() + ".jpg");
                        LpWk_LoopCount++;
                    }
                    if (LpWk_ZeroCount > ZeroLoopCountChkMax)
                    {
                        LpWk_RUN_STATUS = RECOWORKSTART;
                        LpWk_ZeroCount = 0;
                    }
                    else
                    {
                        LpWk_ZeroCount++;
                    }
                    DelayWaitRun(DelayTimerLoopTime);
                }

                if (LpWk_LoopCount >= LpWk_LoopCountMax)
                {
                    //label5.Text = "";
                    //LpWk_RunFlag = false;
                    //timWork.Enabled = false;
                    // btLoopStart.Enabled = true;
                    //btLoopStop.Enabled = false;
                    //MessageBox.Show("테스트종료!");
                }
            }
            //LpWk_RunTimerCount++;

            SetValueLamp(_WorkFuncInfo.Lamp_Index, 0);

            /*
            if (XCCam.ImageStart())
            {
                if (!XCCam.ImageStop())
                {
                    //MessageBox.Show("Image Stop Error");
                }
                XCCam.SetImageCallBack();
                if (!XCCam.ResourceRelease())
                {
                    MessageBox.Show("Resource Release Error");
                }
                DispExec.Stop();
                Frame.Stop();
            }
            */
            CameraPlayerStop();

            CameraOneClose();
            PortDisconnect();

            DelayWaitRun(5);

            MultiMotion.GetCurrentPos();
            TmpMoveAxis_Index = MultiMotion.CAM_UNIT_X;
            TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
            Calibration_Current.ddp_X = TmpMoveValueAxis;

            TmpMoveAxis_Index = MultiMotion.CAM_UNIT_Y;
            TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
            Calibration_Current.ddp_Y = TmpMoveValueAxis;

            TmpMoveAxis_Index = MultiMotion.CAM_UNIT_Z;
            TmpMoveValueAxis = MultiMotion.AxisValue[TmpMoveAxis_Index];
            Calibration_Current.ddp_Z = TmpMoveValueAxis;

            SaveCalibPosSetFiles(SetCalibPosDataFileName);

            AutoScaleBox.Visible = false;
            picRecoImg.Visible = false;

            ResSucessFlag = true;

            CalibrationRuninngFlag = false;

            return ResSucessFlag;

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
                if (LocalTestProcFlag == false)
                {
                    return;
                }
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

        /*
        private void InitVisionLibrary()
        {
            

            // 1. Form_생성자
            // --------------------------------------------------
            CamTotalNum = 0;
            XCCAM.SetStructVersion(XCCamDotNet.Constants.LIBRARY_STRUCT_VERSION);
            ParamCB = GCHandle.Alloc(this);
            XCCAM.SetCallBack(GCHandle.ToIntPtr(ParamCB), SystemCB);
            SetBounds(10, 10, 0, 0, BoundsSpecified.X | BoundsSpecified.Y);

            ZoomFactNum = 0;
            FocusFactNum = 0;
            CurZoomFactNum = 0;
            CurFocusFactNum = 0;

            DXFForm_Width = 640;
            DXFForm_Height = 480;



            // 2. Form_Load ...
            // --------------------------------------------------
            
            UInt64[] WorkUID = new UInt64[0];


            GetProcRunFlag = false;

            CameraList_Relist(ref WorkUID);

            CUID = (UInt64[])WorkUID.Clone();
            //Array.Resize(ref CView, WorkUID.Length);
            CamTotalNum = WorkUID.Length;
            

            Pic_Width = AutoScaleBox.Width;
            Pic_Height = AutoScaleBox.Height;

            canvas = new Bitmap(Pic_Width, Pic_Height);

            

            // 홍동성 시리얼 통신 설정은 다른 곳에 있어야 ...
            NavitarCtrl = new ControllerLegacy("COM1");
            NavitarCtrl.Connect();
        }

        private void ExitVisionLibrary()
        {

        }

        private static void ImageCallback(XCCAM XCCam, IntPtr pInBuf, UInt32 Length, UInt32 iWidth, UInt32 iHeight, XCCAM_IMAGEDATAINFO Info, IntPtr Context)
        {
            GCHandle param = GCHandle.FromIntPtr(Context);
            MainFrame VRef = (MainFrame)param.Target;
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

        private static void SystemCallback(STATUS_SYSTEMCODE SystemStatus, IntPtr Context)
        {
            GCHandle param = GCHandle.FromIntPtr(Context);
            MainFrame CameraListRef = (MainFrame)param.Target;

            switch (SystemStatus)
            {
                case STATUS_SYSTEMCODE.STATUSXCCAM_BUSRESET: // Processing of bus reset
                    if (!CameraListRef.BusResetWorker.IsBusy)
                    {
                        CameraListRef.BusResetWorker.RunWorkerAsync();
                    }
                    break;

                case STATUS_SYSTEMCODE.STATUSXCCAM_POWERUP: // Processing of PowerUP
                    break;
            }
        }

        private void CameraList_Relist(ref UInt64[] WorkUID)
        {
            // 홍동성 => 카메라 UID 추출하는 함수 ...
            
            XCCAM_CAMERAINFO[] CameraInfo;
            int idx;
            String str;


            XCCAM.GetList(out CameraInfo);
            Array.Resize(ref WorkUID, CameraInfo.Length);

            //UIDList.Items.Clear();

            if (CameraInfo.Length != 0)
            {
                for (idx = 0; idx < CameraInfo.Length; idx++)
                {
                    WorkUID[idx] = CameraInfo[idx].UID;
                    str = String.Format("0x{0:X}", WorkUID[idx]);
                    //UIDList.Items.Add(str);
                }
            }
            else
            {
                //UIDList.Items.Add("Not found Camera");
                //UIDList.Enabled = false;
                //CameraOpen.Enabled = false;
            }            
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

        private void BStart_Click()
        {
            Dislay_FPS = 10;//30;//10-100;

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

            //Disp_Flag = false;
        }

        private void BStop_Click()
        {
            if (!XCCam.ImageStop())
                MessageBox.Show("Image Stop Error");

            XCCam.SetImageCallBack();

            if (!XCCam.ResourceRelease())
                MessageBox.Show("Resource Release Error");

            DispExec.Stop();
            Frame.Stop();

        }

        private void DisplayUpdate_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            RGBImage = new Bitmap((int)ImageInfo.Width, (int)ImageInfo.Height, (int)ImageInfo.Width * 4, PixelFormat.Format32bppRgb, RGBData);

            

            SetDisplayImageAuto();
        }

        delegate void SetDisplayImageAutoCallback();

        private void SetDisplayImageAuto()
        {
            if (AutoScaleBox.InvokeRequired)
            {
                SetDisplayImageAutoCallback d = new SetDisplayImageAutoCallback(SetDisplayImageAuto);

                Invoke(d, new object[] { });
            }
            else
            {
                AutoScaleBox.Image = RGBImage;

            }

        }
        */


#endregion Vision Logic ...


#region Monitoring Camera Logic ...

        private VimbaHelper m_VimbaHelper = null;

        private bool m_Acquiring = false;

        private const int m_MonCamListMAX = 10;
        private string[] m_MonCamList = new string[m_MonCamListMAX];


        private void InitMonitorCam()
        {
            try
            {
                // Start up Vimba SDK
                VimbaHelper vimbaHelper = new VimbaHelper();
                vimbaHelper.Startup(this.OnCameraListChanged);
                m_VimbaHelper = vimbaHelper;

                //Text = String.Format("{0} (Vimba Version V{1})", Text, m_VimbaHelper.GetVersion());
                try
                {
                    UpdateCameraList();
                    UpdateControls();
                }
                catch (Exception exception)
                {
                    //textBox1.Text += "\n" + "Could not update camera list. Reason: " + exception.Message;
                }
            }
            catch (Exception exception)
            {
                //textBox1.Text += "\n" + "Could not startup Vimba API. Reason: " + exception.Message;
            }

            for(int i=0;i<m_MonCamListMAX;i++)
            {
                m_MonCamList[i] = "";
            }

        }

        private void CloseMonitorCam()
        {
            if (null != m_VimbaHelper)
            {
                try
                {
                    try
                    {
                        // Shutdown Vimba SDK when application exits
                        m_VimbaHelper.Shutdown();
                    }
                    finally
                    {
                        m_VimbaHelper = null;
                    }
                }
                catch (Exception exception)
                {
                    //textBox1.Text += "\n" + "Could not shutdown Vimba API. Reason: " + exception.Message;
                }
            }
        }

        /// <summary>
        /// Update the camera List in the UI
        /// </summary>
        private void UpdateCameraList()
        {
            int mon_count;
            // Remember the old selection (if there was any)y
            //CameraInfo oldSelectedItem = m_CameraList.SelectedItem as CameraInfo;
            //m_CameraList.Items.Clear();



            List<CameraInfo> cameras = m_VimbaHelper.CameraList;

            CameraInfo newSelectedItem = null;
            mon_count=0;
            foreach (CameraInfo cameraInfo in cameras)
            {
                m_MonCamList[mon_count] = cameraInfo.ID;
                mon_count++;
                /*
                m_CameraList.Items.Add(cameraInfo);

                if (null == newSelectedItem)
                {
                    // At least select the first camera
                    newSelectedItem = cameraInfo;
                }
                else if (null != oldSelectedItem)
                {
                    // If the previous selected camera is still available
                    // then prefer this camera.
                    if (string.Compare(newSelectedItem.ID, cameraInfo.ID, StringComparison.Ordinal) == 0)
                    {
                        newSelectedItem = cameraInfo;
                    }
                }
                */
            }

            /*
            // If available select a camera and adjust the status of the "Start acquisition" button
            if (null != newSelectedItem)
            {
                m_CameraList.SelectedItem = newSelectedItem;
                //m_AcquireButton.Enabled = true;
            }
            else
            {
                //m_AcquireButton.Enabled = false;
            }
            */
        }

        /// <summary>
        /// Handles the CameraListChanged event
        /// </summary>
        /// <param name="sender">The Sender</param>
        /// <param name="args">The EventArgs</param>
        private void OnCameraListChanged(object sender, EventArgs args)
        {
            // Start an asynchronous invoke in case this method was not
            // called by the GUI thread.
            if (InvokeRequired == true)
            {
                BeginInvoke(new CameraListChangedHandler(this.OnCameraListChanged), sender, args);
                return;
            }

            if (null != m_VimbaHelper)
            {
                try
                {
                    UpdateCameraList();

                    //textBox1.Text += "\n" + "Camera list updated.";
                }
                catch (Exception exception)
                {
                    //textBox1.Text += "\n" + "Could not update camera list. Reason: " + exception.Message;
                }
            }
        }

        /// <summary>
        /// Handles the FrameReceived event
        /// </summary>
        /// <param name="sender">The Sender</param>
        /// <param name="args">The FrameEventArgs</param>
        private void OnFrameReceived(object sender, FrameEventArgs args)
        {
            /*
            // Start an async invoke in case this method was not
            // called by the GUI thread.
            if (InvokeRequired == true)
            {
                BeginInvoke(new FrameReceivedHandler(this.OnFrameReceived), sender, args);
                return;
            }

            if (true == m_Acquiring)
            {
                // Display image
                Image image = args.Image;
                if (null != image)
                {
                    AutoScaleBox.Image = image;
                    //textBox1.Text += "\n" + "Capture Image!";
                }
                else
                {
                    //textBox1.Text += "\n" + "An acquisition error occurred. Reason: " + args.Exception.Message;

                    try
                    {
                        try
                        {
                            // Start asynchronous image acquisition (grab) in selected camera
                            m_VimbaHelper.StopContinuousImageAcquisition();
                        }
                        finally
                        {
                            m_Acquiring = false;
                            UpdateControls();
                            m_CameraList.Enabled = true;
                        }

                        //textBox1.Text += "\n" + "Asynchronous image acquisition stopped.";
                    }
                    catch (Exception exception)
                    {
                        //textBox1.Text += "\n" + "Error while stopping asynchronous image acquisition. Reason: " + exception.Message;
                    }
                }
            }
            */
        }

        /// <summary>
        /// Updates the state of the Acquisition and Trigger controls
        /// </summary>
        private void UpdateControls()
        {
            /*
            if (true == m_Acquiring)
            {
                //m_AcquireButton.Text = "Stop image acquisition";
                //m_AcquireButton.Enabled = true;
                //textBox1.Text += "\n" + "Start image acquisition1.";
            }
            else
            {
                //m_AcquireButton.Text = "Start image acquisition";
                //textBox1.Text += "\n" + "Start image acquisition2.";

                CameraInfo cameraInfo = m_CameraList.SelectedItem as CameraInfo;
                if (null != cameraInfo)
                {
                    // Enable button if a camera is selected
                    //m_AcquireButton.Enabled = true;
                }
                else
                {
                    // Disable button if no camera is selected
                    //m_AcquireButton.Enabled = false;
                }
            }

            if (m_VimbaHelper.IsTriggerAvailable)
            {
                if (false == m_Acquiring)
                {
                    //m_SoftwareTriggerCheckbox.Enabled = m_AcquireButton.Enabled;
                    //m_SoftwareTriggerButton.Enabled = false;
                }
                else
                {
                    //m_SoftwareTriggerCheckbox.Enabled = false;
                    //m_SoftwareTriggerButton.Enabled = m_SoftwareTriggerCheckbox.Checked;
                }
            }
            else
            {
                //m_SoftwareTriggerCheckbox.Checked = false;
                //m_SoftwareTriggerCheckbox.Enabled = false;
            }
            */
        }

#endregion Monitoring Camera Logic ...


        public MainFrame()
        {
            InitializeComponent();

            //인식용 줌카메라 초기화.
            InitCamSetting();
        }

        private void MainFrame_Load(object sender, EventArgs e)
        {
            this.Left = 0;
            this.Top = 0;


            Initialize();

            InitMotionController();
           
            MultiMotion.MainLightOnOff(1);  // 형광등 코드 ...

            InitializingCam();              // 카메라 초기화

            InitMonitorCam();               // 모니터링 카메라 초기화.

            //InitVisionLibrary();
        }

        private void MainFrame_FormClosing(object sender, FormClosingEventArgs e)
        {
            //ExitVisionLibrary();
            CalibrationRuninngFlag = false;
            //모니터링 카메라 닫기.
            CloseMonitorCam();

            ExitMotionController();
        }

        private void Initialize()
        {
            // 1. 환경 설정 정보 읽기
            // --------------------------------------------------
            DeviceManager.Read();            


            // 2. 설정 파일 로드(DIO, Motion, Camera, Lighting, 용접 프로그램)
            // ----------
            string filename = ConfigManager.GetDataFilePath + "setting_dio.dat";
            DataManager.LoadDIOSettingFiles(filename);

            filename = ConfigManager.GetDataFilePath + "setting_motion.dat";
            DataManager.LoadMotionSettingFiles(filename);

            filename = ConfigManager.GetDataFilePath + "setting_camera.dat";
            DataManager.LoadCameraSettingFiles(filename);

            filename = ConfigManager.GetDataFilePath + "setting_lighting.dat";
            DataManager.LoadLightingSettingFiles(filename);


            // 3. 모델 파일 로드
            // --------------------------------------------------
            ModelLoad_Display(); // DataManager.LoadModelListFiles(ConfigManager.GetModelListPath);


            // CamAnalyzer 초기화 부분
            // --------------------------------------------------
            ModelNameStr = "";
            NGLoopCountNum = 0;


            // 컬럼명과 컬럼 사이즈 지정
            // --------------------------------------------------
            lstViewTestList.Columns.Add("작업명", 300, HorizontalAlignment.Center);
            //lstViewTestList.Columns.Add("상태", 100, HorizontalAlignment.Center);
            lstViewTestList.Columns.Add("결과", 100, HorizontalAlignment.Center);

            lstViewTestList.EndUpdate();

            lstViewResList.Columns.Add("일련 번호", 200, HorizontalAlignment.Center);
            lstViewResList.Columns.Add("작업 시간", 100, HorizontalAlignment.Center);
            lstViewResList.Columns.Add("NG/OK", 100, HorizontalAlignment.Center);

            lstViewResList.EndUpdate();

            //배열초기화
            DXFObjList = new DXFLayerInfo[DXFOBJLIST_MAX];


            ShowReadyImage();
        }


#region TestProc(개별) ...

        public void AssembleProc()
        {
            for (int i = 0; i < 1000; i++)
            {
                CurTestProcIndex = i;

                if (DataManager.TestProcList[CurTestProcIndex].TestProcExistFlag == false)
                {
                    CurTestProcIndex = -1;

                    LocalTestProcFlag = false;

                }

                if (LocalTestProcFlag == false)
                {
                    // 홍동성 => 통신 포트 해제하기                    

                    return;
                }

                // 개별 작업 실행 ...
                // ----------
                if (AssembleOneProc() == false)
                {
                    AssembleOk = false;
                }
                // ----------
            }
        }

        public bool AssembleOneProc()
        {



            // ----------
            bool bReturn = false;

            UpdateAssembleProcStatus(); // 상태 업데이트 함수 ...            

            DataManager.TestProcList[CurTestProcIndex].T_TestResultSts = ING_RESULT;

            SelectReDrawTestProcList(CurTestProcIndex);

            lstViewTestList.Refresh();
            // ----------


            switch (DataManager.TestProcList[CurTestProcIndex].WFType)
            {       

                case 0: // INDEX(고정축) 회전(R) 기능
                case 1: // INDEX(이동축) 회전(R) 기능
                    {
                        bReturn = AssembleProcRotate();
                    }
                    break;

                case 3: // 원점 복귀 기능
                    {
                        bReturn = AssembleProcSingleAxisMove();
                    }
                    break;

                case 4: // INDEX(이동축) X축 압축 이동 기능
                    {
                        bReturn = AssembleProcGantry();
                    }
                    break;

                case 5: // 전면(A) 카메라 이동(XYZ) 기능
                case 6: // 전면(B) 카메라 이동(XYZ) 기능                
                case 7: // 상방 카메라 이동(XYZ) 기능
                    {
                        bReturn = AssembleProcCamUnitMove();
                    }
                    break;

                case 8: // 후방 카메라 이동(Z) 기능
                    {
                        bReturn = AssembleProcBackCam();
                    }
                    break;

                case 9: // 카메라 조정(Zoom, Focus) 기능
                    {

                    }
                    break;

                case 10: // 조명 조정(밝기) 기능
                    {
                        bReturn = AssembleProcLighting();
                    }
                    break;

                case 11: // 영상 인식 INDEX 위치 맞춤 기능
                    {
                        MultiMotion.MainLightOnOff(0);



                        // 인식 진행 전에 축을 푼다.
                        // ----------
                        if (MultiMotion.GantryAxisEnable(0, false) == MultiMotion.KSM_OK)
                        {

                        }
                        // ----------



                        MultiMotion.SetSpeed(MultiMotion.KSM_SPEED_FAST);


                        Status_pictureBox.Visible = false;

                        bReturn = RecoProcessRun();

                        Status_pictureBox.Visible = true;

                        MultiMotion.MainLightOnOff(1);

                        MultiMotion.SetSpeed(MultiMotion.KSM_SPEED_SLOW);
                    }
                    break;

                case 19: // @"고정축 INDEX 전방, 영상 인식 기능", // 19 => 20160816
                    {
                        DeviceManager.PlaySoundM4A("고정축전방모드입니다.m4a");

                        MultiMotion.MainLightOnOff(0);


                        // 인식 진행 전에 축을 푼다.
                        // ----------
                        if (MultiMotion.GantryAxisEnable(0, false) == MultiMotion.KSM_OK)
                        {

                        }
                        // ----------
 


                        MultiMotion.SetSpeed(MultiMotion.KSM_SPEED_FAST);


                        Status_pictureBox.Visible = false;

                        bReturn = RecoProcessRun();

                        Status_pictureBox.Visible = true;

                        MultiMotion.MainLightOnOff(1);

                        MultiMotion.SetSpeed(MultiMotion.KSM_SPEED_SLOW);
                    }
                    break;

                case 20: // @"고정축 INDEX 후방, 영상 인식 기능", 
                    {
                        DeviceManager.PlaySoundM4A("고정축후방모드입니다.m4a");
                        


                        MultiMotion.MainLightOnOff(0);


                        // 인식 진행 전에 축을 푼다.
                        // ----------
                        if (MultiMotion.GantryAxisEnable(0, false) == MultiMotion.KSM_OK)
                        {

                        }
                        // ----------
 


                        MultiMotion.SetSpeed(MultiMotion.KSM_SPEED_FAST);


                        Status_pictureBox.Visible = false;

                        bReturn = RecoProcessRun();

                        Status_pictureBox.Visible = true;

                        MultiMotion.MainLightOnOff(1);

                        MultiMotion.SetSpeed(MultiMotion.KSM_SPEED_SLOW);
                    }
                    break;

                case 21: // @"이동축 INDEX 전방, 영상 인식 기능", 
                    {
                        DeviceManager.PlaySoundM4A("이동축전방모드입니다.m4a");



                        MultiMotion.MainLightOnOff(0);


                        // 인식 진행 전에 축을 푼다.
                        // ----------
                        if (MultiMotion.GantryAxisEnable(0, false) == MultiMotion.KSM_OK)
                        {

                        }
                        // ----------
 


                        MultiMotion.SetSpeed(MultiMotion.KSM_SPEED_FAST);


                        Status_pictureBox.Visible = false;

                        bReturn = RecoProcessRun();

                        Status_pictureBox.Visible = true;

                        MultiMotion.MainLightOnOff(1);

                        MultiMotion.SetSpeed(MultiMotion.KSM_SPEED_SLOW);
                    }
                    break;

                case 22: // @"이동축 INDEX 후방, 영상 인식 기능", 
                    {
                        MultiMotion.MainLightOnOff(0);


                        DeviceManager.PlaySoundM4A("이동축후방모드입니다.m4a");

                        // 인식 진행 전에 축을 푼다.
                        // ----------
                        if (MultiMotion.GantryAxisEnable(0, false) == MultiMotion.KSM_OK)
                        {

                        }
                        // ----------
  


                        MultiMotion.SetSpeed(MultiMotion.KSM_SPEED_FAST);


                        Status_pictureBox.Visible = false;

                        bReturn = RecoProcessRun();

                        Status_pictureBox.Visible = true;

                        MultiMotion.MainLightOnOff(1);

                        MultiMotion.SetSpeed(MultiMotion.KSM_SPEED_SLOW);
                    }
                    break;

                case 23: // @"이동축 INDEX 상방, 영상 인식 기능"
                    {
                        MultiMotion.MainLightOnOff(0);

                        DeviceManager.PlaySoundM4A("이동축상방모드입니다.m4a");

                        // 인식 진행 전에 축을 푼다.
                        // ----------
                        if (MultiMotion.GantryAxisEnable(0, false) == MultiMotion.KSM_OK)
                        {

                        }
                        // ----------
   


                        MultiMotion.SetSpeed(MultiMotion.KSM_SPEED_FAST);


                        Status_pictureBox.Visible = false;

                        bReturn = RecoProcessRun();

                        Status_pictureBox.Visible = true;

                        MultiMotion.MainLightOnOff(1);

                        MultiMotion.SetSpeed(MultiMotion.KSM_SPEED_SLOW);
                    }
                    break;
                

                case 12: // V블럭 위치 이동(Z) 기능
                    {
                        bReturn = AssembleProcVBlock();
                    }
                    break;

                case 13: // 용접 로봇 동작 기능
                    {

                    }
                    break;

                case 14: // 용접 기능
                    {
                        bReturn = AssembleProcWelding();
                    }
                    break;

                case 15: // Delay 기능
                    {
                        bReturn = AssembleProcDelay();
                    }
                    break;

                case 16: // Digital I/O 동작 기능
                    {
                        bReturn = AssembleProcDIO();
                    }
                    break;

                case 17: // 롤링 카메라 모니터링 기능
                    {
                        //return AssembleProcDIO();
                    }
                    break;

                case 18: // INDEX 양쪽 동기 회전(R) 기능
                    {
                        bReturn = AssembleProcIndexGantry();
                    }
                    break;

                case 2: // INDEX(고정축) Rolling 기능
                    {
                        bReturn = AssembleProcRolling();
                    }
                    break;

                default :
                    break;
            }


            // 상태 이미지 업데이트
            // ----------
            if (bReturn)
            {
                UpdateNgOkImage(1); // ng, ok, ing
            }
            else
            {
                UpdateNgOkImage(0); // ng, ok, ing
            }
            // ----------


            return bReturn;
        }

        public bool AssembleProcDelay()
        {            
            CommonUtility.WaitTime(DataManager.TestProcList[CurTestProcIndex].WFDelayTime * 1000, false);

            return true;
        }

        public bool AssembleProcDIO()
        {
            short bitno = (short)DataManager.TestProcList[CurTestProcIndex].WFDIOPortNum;
            short onoff = 0;            
            
            if (DataManager.TestProcList[CurTestProcIndex].WFDioOnOff == true)
            {
                onoff = 1;
            }

            NMC2.nmc_SetDIOOutputBit(DeviceManager.g_ndevIdA_4, bitno, onoff);

            return true;
        }

        public bool AssembleProcLighting()
        {
            int channel = DataManager.TestProcList[CurTestProcIndex].WFLampChannel;
            int value = DataManager.TestProcList[CurTestProcIndex].WFLampValue;

            LampComm.SetLamp(channel, value);

            return true;
        }

        public bool AssembleProcBackCam()
        {
            double dZValue = DataManager.TestProcList[CurTestProcIndex].WFMoveZ;

            MultiMotion.MoveAxis(MultiMotion.BACK_CAM_Z, dZValue, true);

            CommonUtility.WaitTime(DataManager.TestProcList[CurTestProcIndex].WFDelayTime * 1000, false);

            return true;
        }

        public bool AssembleProcVBlock()
        {
            double dZValue = DataManager.TestProcList[CurTestProcIndex].WFMoveZ;

            MultiMotion.MoveAxis(MultiMotion.VBLOCK_Z, dZValue, true);

            return true;
        }

        private bool AssembleProcRotate()
        {
            int Index = -1;

            double dRotateValue = DataManager.TestProcList[CurTestProcIndex].WFRotationAngle;


            switch (DataManager.TestProcList[CurTestProcIndex].WFType)
            {
                case 0:
                    MultiMotion.MoveAxis(MultiMotion.INDEX_FIX_R, dRotateValue, true);
                    break;
                case 1:
                    MultiMotion.MoveAxis(MultiMotion.INDEX_MOVE_R, dRotateValue, true);
                    break;
            }

            return true;
        }

        private bool AssembleProcCamUnitMove()
        {
            double dXPos = DataManager.TestProcList[CurTestProcIndex].WFMoveX;
            double dYPos = DataManager.TestProcList[CurTestProcIndex].WFMoveY;
            double dZPos = DataManager.TestProcList[CurTestProcIndex].WFMoveZ;

            /*
            MultiMotion.MoveAxis(MultiMotion.CAM_UNIT_Z, dZPos);
            MultiMotion.MoveAxis(MultiMotion.CAM_UNIT_X, dXPos);
            MultiMotion.MoveAxis(MultiMotion.CAM_UNIT_Y, dYPos);            
            */

            return true;
        }

        private bool AssembleProcGantry()
        {
            double dXPos = DataManager.TestProcList[CurTestProcIndex].WFMoveX;

            MultiMotion.GantryAxis(MultiMotion.INDEX_MOVE_M, MultiMotion.INDEX_MOVE_S, dXPos, true);

            return true;
        }

        private bool AssembleProcIndexGantry()
        {
            double dRotateValue = DataManager.TestProcList[CurTestProcIndex].WFRotationAngle;

            if (MultiMotion.GantryAxisEnable(0, true) == MultiMotion.KSM_OK)
            {
                MultiMotion.IndexGantryAxis(MultiMotion.INDEX_FIX_R, MultiMotion.INDEX_MOVE_R, dRotateValue, true);
            }

            return true;
        }
        
        public bool AssembleProcSingleAxisMove()
        {
            double dPosition = DataManager.TestProcList[CurTestProcIndex].WFMoveX;

            bool bReturnHome = false;                
            if (DataManager.TestProcList[CurTestProcIndex].ReturnHome == 1)
                bReturnHome = true;


            bool bAxisEndWait = false;
            if (DataManager.TestProcList[CurTestProcIndex].AxisEndWait == 0)
                bAxisEndWait = true;


            int SelectedAxis = DataManager.TestProcList[CurTestProcIndex].SelectedAxis;



            // 속도 제어 ...
            // ----------
            if (DataManager.TestProcList[CurTestProcIndex].AxisSpeed > 0)
            {
                MultiMotion.SetSpeed(DataManager.TestProcList[CurTestProcIndex].AxisSpeed);
            }
            else
            {
                MultiMotion.SetSpeed(MultiMotion.KSM_SPEED_SLOW);
            }
            // ----------    
            


            switch (SelectedAxis)
            {
                case 0: //INDEX(고정축) 회전(R) 기능
                    {
                        if (bAxisEndWait == true)
                            MultiMotion.MoveAxis(MultiMotion.INDEX_FIX_R, dPosition, true);
                        else
                            MultiMotion.MoveAxis(MultiMotion.INDEX_FIX_R, dPosition, false);


                        if (bReturnHome == true)
                            MultiMotion.HomeMove(MultiMotion.INDEX_FIX_R, true);
                    }
                    break;
                case 1: //INDEX(이동축) 회전(R) 기능
                    {
                        if (bAxisEndWait == true)
                            MultiMotion.MoveAxis(MultiMotion.INDEX_MOVE_R, dPosition, true);
                        else
                            MultiMotion.MoveAxis(MultiMotion.INDEX_MOVE_R, dPosition, false);

                        if (bReturnHome == true)
                            MultiMotion.HomeMove(MultiMotion.INDEX_MOVE_R, true);
                    }
                    break;


                case 2: //INDEX(고정축) Rolling(1) 기능
                    {
                        if (bAxisEndWait == true)
                            MultiMotion.MoveAxis(MultiMotion.ROLLING_FIX_1, dPosition, true);
                        else
                            MultiMotion.MoveAxis(MultiMotion.ROLLING_FIX_1, dPosition, false);

                        if (bReturnHome == true)
                            MultiMotion.HomeMove(MultiMotion.ROLLING_FIX_1, true);
                    }
                    break;
                case 3: //INDEX(고정축) Rolling(2) 기능
                    {
                        if (bAxisEndWait == true)
                            MultiMotion.MoveAxis(MultiMotion.ROLLING_FIX_2, dPosition, true);
                        else
                            MultiMotion.MoveAxis(MultiMotion.ROLLING_FIX_2, dPosition, false);

                        if (bReturnHome == true)
                            MultiMotion.HomeMove(MultiMotion.ROLLING_FIX_2, true);
                    }
                    break;
                case 4: //INDEX(이동축) Rolling(1) 기능
                    {
                        if (bAxisEndWait == true)
                            MultiMotion.MoveAxis(MultiMotion.ROLLING_MOVE_1, dPosition, true);
                        else
                            MultiMotion.MoveAxis(MultiMotion.ROLLING_MOVE_1, dPosition, false);

                        if (bReturnHome == true)
                            MultiMotion.HomeMove(MultiMotion.ROLLING_MOVE_1, true);
                    }
                    break;
                case 5: //INDEX(이동축) Rolling(2) 기능
                    {
                        if (bAxisEndWait == true)
                            MultiMotion.MoveAxis(MultiMotion.ROLLING_MOVE_2, dPosition, true);
                        else
                            MultiMotion.MoveAxis(MultiMotion.ROLLING_MOVE_2, dPosition, false);

                        if (bReturnHome == true)
                            MultiMotion.HomeMove(MultiMotion.ROLLING_MOVE_2, true);
                    }
                    break;
                case 8: //카메라 유닛 X축 이동 기능
                    {
                        if (bAxisEndWait == true)
                            MultiMotion.MoveAxis(MultiMotion.CAM_UNIT_X, dPosition, true);
                        else
                            MultiMotion.MoveAxis(MultiMotion.CAM_UNIT_X, dPosition, false);

                        if (bReturnHome == true)
                            MultiMotion.HomeMove(MultiMotion.CAM_UNIT_X, true);
                    }
                    break;
                case 9: //카메라 유닛 Y축 이동 기능
                    {
                        if (bAxisEndWait == true)
                            MultiMotion.MoveAxis(MultiMotion.CAM_UNIT_Y, dPosition, true);
                        else
                            MultiMotion.MoveAxis(MultiMotion.CAM_UNIT_Y, dPosition, false);

                        if (bReturnHome == true) 
                            MultiMotion.HomeMove(MultiMotion.CAM_UNIT_Y, true);
                    }
                    break;
                case 10://카메라 유닛 Z축 이동 기능
                    {
                        if (bAxisEndWait == true)
                            MultiMotion.MoveAxis(MultiMotion.CAM_UNIT_Z, dPosition, true);
                        else
                            MultiMotion.MoveAxis(MultiMotion.CAM_UNIT_Z, dPosition, false);

                        if (bReturnHome == true)
                            MultiMotion.HomeMove(MultiMotion.CAM_UNIT_Z, true);
                    }
                    break;
                case 11://후면 카메라 Z축 이동 기능
                    {
                        if (bAxisEndWait == true)
                            MultiMotion.MoveAxis(MultiMotion.BACK_CAM_Z, dPosition, true);
                        else
                            MultiMotion.MoveAxis(MultiMotion.BACK_CAM_Z, dPosition, false);

                        if (bReturnHome == true)
                            MultiMotion.HomeMove(MultiMotion.BACK_CAM_Z, true);
                    }
                    break;
                case 12://V블럭 위치 이동(Z) 기능
                    {
                        if (bAxisEndWait == true)
                            MultiMotion.MoveAxis(MultiMotion.VBLOCK_Z, dPosition, true);
                        else
                            MultiMotion.MoveAxis(MultiMotion.VBLOCK_Z, dPosition, false);

                        if (bReturnHome == true)
                            MultiMotion.HomeMove(MultiMotion.VBLOCK_Z, true);
                    }
                    break;
                case 6: //INDEX(이동축) X축 갠트리(M) 제어
                    if (MultiMotion.GantryAxisEnable(1, true) == MultiMotion.KSM_OK)
                    {
                        if (bAxisEndWait == true)
                            MultiMotion.GantryAxis(MultiMotion.INDEX_MOVE_M, MultiMotion.INDEX_MOVE_S, dPosition, true);
                        else
                            MultiMotion.GantryAxis(MultiMotion.INDEX_MOVE_M, MultiMotion.INDEX_MOVE_S, dPosition, false);
                    }

                    if (bReturnHome == true)
                        MultiMotion.HomeMove(MultiMotion.INDEX_MOVE_M, true);

                    break;
                case 7: //INDEX(이동축) X축 갠트리(S) 제어
                    MessageBox.Show("개별 축으로 제어 할 수 없는 축입니다.");
                    break;

                default:
                    break;
            }


            // 기본 속도로 변경 ...
            // ----------
            MultiMotion.SetSpeed(MultiMotion.KSM_SPEED_SLOW);
            // ----------


            return true;
        }

        public bool AssembleProcRolling()
        {
            DeviceManager.PlaySoundM4A("캡슐을투입하여주십시오.m4a");


            frmFuncRollingUI dlg = new frmFuncRollingUI();

            dlg._WorkFuncInfo = DataManager.TestProcList[CurTestProcIndex];

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                DataManager.TestProcList[CurTestProcIndex] = dlg._WorkFuncInfo;                

                DataManager.SaveWorkListFiles(DataManager.SaveTestWorkListPath);

                DataManager.SaveCurrentModel();

                return true;
            }
            else
            {
                return false;
            }

            DeviceManager.PlaySoundM4A("롤링이끝났습니다.m4a");            

            return true;
        }

        public bool AssembleProcWelding()
        {

            frmFuncWelding dlg = new frmFuncWelding();

            dlg._WorkFuncInfo = DataManager.TestProcList[CurTestProcIndex];

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                DataManager.TestProcList[CurTestProcIndex] = dlg._WorkFuncInfo;

                DataManager.SaveWorkListFiles(DataManager.SaveTestWorkListPath);

                return true;
            }
            else
            {
                return false;
            }

            
            //DeviceManager.PlaySoundM4A("오빠용접끝났어.m4a");
            


            return true;
        }


#endregion TestProc(개별) ...


#region Button

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (LocalTestProcFlag == true)
            {
                MessageBox.Show("작업이 진행중입니다. 작업을 중지하시고 종료하시기 바랍니다.");
                return;
            }

            if (MessageBox.Show("프로그램을 종료하시겠습니까?", "종료여부", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            LocalTestProcFlag = true;  // 홍동성 테스트 종료 ...

            Application.DoEvents();
            Application.DoEvents();
            Application.DoEvents();
            Application.DoEvents();
            Application.DoEvents();
            Application.DoEvents();
            Application.DoEvents();
            Application.DoEvents();

            Application.Exit();
        }

        private void btnTestStart_Click(object sender, EventArgs e)
        {
            InitTestStart(); 

            PortConnect();


            // 1. 홍동성 => 사용할 지 여부는 알 수 없다.
            /*
            if (txtSerialNumber.Text.Length == 0)
            {
                MessageBox.Show("시리얼 번호 입력창이 비어있습니다. 시리얼 번호를 입력하셔야 합니다.");

                txtSerialNumber.Focus();

                return;
            }
            */


            


            //CommonUtility.WaitTime(2000, false);

            //PlayModeSound();



            if (lstViewTestList.SelectedItems.Count > 0)
            {
                frmSelectRun frmselectrun = new frmSelectRun();

                DialogResult dialogresult = frmselectrun.ShowDialog();

                if (dialogresult == DialogResult.OK)
                {
                    // 선택 항목 확인 및 Clear
                    int SelTestNumIndex = lstViewTestList.SelectedIndices[0];
                    lstViewTestList.SelectedItems.Clear();
                    lstViewTestList.Refresh();

                    // 초기화
                    OneTestDataProcClear(SelTestNumIndex);

                    // 실행 작업 위치 설정
                    CurTestProcIndex = SelTestNumIndex;

                    // ----------
                    TestRunCountTime = GetCurrentSecond();

                    /*
                    TestTimeThreadFlag = true;
                    TestTimeThread = new Thread(new ThreadStart(ThreadTestTime));
                    TestTimeThread.Priority = ThreadPriority.Highest;
                    TestTimeThread.Start();
                    */

                    UpdateNgOkImage(2);         // 상태 이미지 업데이트(ng, ok, ing)
                    // ----------

                    LocalTestProcFlag = true;

                    // 작업 실행
                    AssembleOneProc();

                    LocalTestProcFlag = false; 
                    
                    lstViewTestList.SelectedItems.Clear();
                    lstViewTestList.Refresh();

                    // 홍동성 => 통신 포트 닫기
                    PortDisconnect();

                    TestTimeThreadFlag = false;

                }
                else if (dialogresult == DialogResult.Retry)
                {
                    DeviceManager.PlaySoundM4A("검사를시작합니다.m4a");


                    // 선택 항목 확인 및 Clear
                    int SelTestNumIndex = lstViewTestList.SelectedIndices[0];

                    TestDataProcClear();

                    LocalTestProcFlag = true;

                    CurTestProcIndex = SelTestNumIndex;

                    // ----------
                    TestRunCountTime = GetCurrentSecond();
                    /*                    
                    TestTimeThreadFlag = true;
                    TestTimeThread = new Thread(new ThreadStart(ThreadTestTime));
                    TestTimeThread.Start();
                    */

                    UpdateNgOkImage(2);         // 상태 이미지 업데이트(ng, ok, ing)
                    // ----------


                    timeTestProc.Enabled = true;

                    

                }
                else
                {
                    lstViewTestList.SelectedItems.Clear();
                    lstViewTestList.Refresh();
                }


            }
            else
            {
                if (MessageBox.Show("작업을 시작하시겠습니까?", "작업 시작", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DeviceManager.PlaySoundM4A("검사를시작합니다.m4a");


                    TestDataProcClear();

                    LocalTestProcFlag = true;

                    CurTestProcIndex = -1;

                    // ----------
                    TestRunCountTime = GetCurrentSecond();
                    /*                    
                    TestTimeThreadFlag = true;
                    TestTimeThread = new Thread(new ThreadStart(ThreadTestTime));
                    TestTimeThread.Start();
                    */

                    UpdateNgOkImage(2);         // 상태 이미지 업데이트(ng, ok, ing)
                    // ----------

                    timeTestProc.Enabled = true;                    
                }
            }            
        }

        private static void PlayModeSound()
        {
            for (int i = 0; i < DataManager.TESTPROCMAX; i++)
            {
                if (DataManager.TestProcList[i].TestProcExistFlag == true)
                {
                    switch (DataManager.TestProcList[i].WFType)
                    {
                        case 19: // @"고정축 INDEX 전방, 영상 인식 기능"
                            {

                            }
                            break;

                        case 20: // @"고정축 INDEX 후방, 영상 인식 기능", 
                            {

                            }
                            break;

                        case 21: // @"이동축 INDEX 전방, 영상 인식 기능", 
                            {

                            }
                            break;

                        case 22: // @"이동축 INDEX 후방, 영상 인식 기능", 
                            {

                            }
                            break;

                        case 23: // @"이동축 INDEX 상방, 영상 인식 기능"
                            {

                            }
                            break;
                    }
                }
            }



            //DeviceManager.PlaySoundM4A("고정축상방_이동축전방.m4a");
            //DeviceManager.PlaySoundM4A("고정축상방_이동축후방.m4a");
            DeviceManager.PlaySoundM4A("고정축전방_이동축전방.m4a");
            //DeviceManager.PlaySoundM4A("고정축전방_이동축후방.m4a");
            //DeviceManager.PlaySoundM4A("고정축후방_이동축전방.m4a");
            //DeviceManager.PlaySoundM4A("고정축후방_이동축후방.m4a");
        }

        private void btnTestEnd_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("작업을 종료하시겠습니까?", "작업 종료", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                CurTestProcIndex = -1;
                CalibrationRuninngFlag = false;
                LocalTestProcFlag = false;
                TestTimeThreadFlag = false;

                SelectReDrawTestProcList(-1);
            }
        }

        private void InitTestStart()
        {
            AssembleProgressBar.Value = 0;

            AssembleOk = true;
            SerialNumber = String.Format("0001{0:yyMMddhhmmss}", DateTime.Now);
            txtSerialNumber.Text = SerialNumber;

            TestCurrentResData.Total_TestCount++;


            // ----------
            ShowReadyImage();
            // ----------
        }

        private void ShowReadyImage()
        {
            Status_pictureBox.Visible = true;

            if (File.Exists("C:\\KSM\\Images\\ready.bmp") == true)
            {
                Status_pictureBox.Image = System.Drawing.Image.FromFile("C:\\KSM\\Images\\ready.bmp");
            }

            if (File.Exists("C:\\KSM\\Images\\bottom_05_완료.png") == true)
            {
                AssembleSteppictureBox.Image = System.Drawing.Image.FromFile("C:\\KSM\\Images\\bottom_05_완료.png");
            }
        }
        
        private void btCountReset_Click(object sender, EventArgs e)
        {
            TestCurrentResClear();
        }

        private void btnEditor_Click(object sender, EventArgs e)
        {            
            if (LocalTestProcFlag == true)
            {
                MessageBox.Show("작업이 진행중입니다. 작업을 중지하시고 편집하시기 바랍니다.");
             
                return;
            }

            /*
            if (PassWdOKFlag == false)
            {
                frmPassWord frmPassWdDlg;

                frmPassWdDlg = new frmPassWord();
                if (frmPassWdDlg.ShowDialog() == DialogResult.OK)
                {
                }
                else
                {
                    return;
                }
                PassWdOKFlag = true;
            }
            */

            frmEditor frmDlg = new frmEditor();

            if (frmDlg.ShowDialog() == DialogResult.OK)
            {
                
            }

            ModelLoad_Display();
        }

        private void btnSetting_Click(object sender, EventArgs e)
        {
            frmDeviceSetting dlg = new frmDeviceSetting();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
            }
        }

        private void btnModelOpen_Click(object sender, EventArgs e)
        {
            frmModelList frmDlg;
            frmDlg = new frmModelList();
            if (frmDlg.ShowDialog() == DialogResult.OK)
            {
                
            }

            DeviceManager.Write_SelectedModelInfo();

            ModelLoad_Display();
        }

#endregion Button
        
#region Model


        private void ModelLoad_Display()
        {
            DataManager.LoadModelListFiles_NET();


            DataManager.SelectedModelByName(DataManager.SelectedModelName);


            // 20160830 => 추가 및 주석
            {
                txtModelName.Text = DataManager.SelectedModelName;
                
                SelectModelDatIndex = DataManager.SelectModelIndex;
                if (SelectModelDatIndex > -1)
                {
                    DataManager.SaveTestWorkListPath =  ConfigManager.GetModelFilePath + DataManager.GetModelSelectFileName();

                    //txtModelName.Text = DataManager.GetModelSelectName();

                    // 현재 모델 정보 저장 ... - 20160816 ...
                    // ----------
                    DataManager.SelectedModel = DataManager.ModelDatList[SelectModelDatIndex];
                    // ----------
                }
                else
                {
                    DataManager.SaveTestWorkListPath = "";
                }
                
            }
            
            DataManager.TestProcClear();
            DataManager.LoadWorkListFiles(DataManager.SaveTestWorkListPath); // 3 작업 리스트 파일 로드
            ReDrawTestProcList();


            if (SelectModelDatIndex > -1)
            {
                //string strFileName = ConfigManager.GetDataFilePath + "Images\\" + DataManager.ModelDatList[SelectModelDatIndex].ImageFileName;

                string strFileName = ConfigManager.GetModelFilePath + DataManager.SelectedModel.ModelName + "\\" + DataManager.ModelDatList[SelectModelDatIndex].ImageFileName;

                if (File.Exists(strFileName) == true)
                {
                    ModelPictureBox.Image = System.Drawing.Image.FromFile(strFileName);
                }
            }
        }

        private void ReDrawTestProcList()
        {
            SelectReDrawTestProcList(-1);
        }

        private void SelectReDrawTestProcList(int SelVisibleIndex)
        {
            lstViewTestList.Items.Clear();

            for (int i = 0; i < DataManager.TESTPROCMAX; i++)
            {
                if (DataManager.TestProcList[i].TestProcExistFlag == true)
                {
                    ListViewItem lstViewTestItem = new ListViewItem(DataManager.TestProcList[i].TestProcName);

                    if (SelVisibleIndex > -1 && SelVisibleIndex == i)
                    {
                        lstViewTestItem.ForeColor = Color.Yellow;
                        lstViewTestItem.BackColor = Color.Gray;
                    }
                    else
                    {
                        lstViewTestItem.ForeColor = Color.Black;
                    }

                    lstViewTestItem.SubItems.Add(GetProcResStatusName(DataManager.TestProcList[i].T_TestResultSts));
                    
                    lstViewTestList.Items.Add(lstViewTestItem);

                }
            }
            lstViewTestList.EndUpdate();
            if (SelVisibleIndex > -1)
            {
                lstViewTestList.Items[SelVisibleIndex].Selected = true;
                lstViewTestList.EnsureVisible(SelVisibleIndex);
            }
        }

        private string GetProcResStatusName(int GetProcResStsIndex)
        {
            string ResProcStsNameStr = "";

            for (int i = 0; i < ProcResStatusIndexList.Count; i++)
            {
                if ((int)ProcResStatusIndexList[i] == GetProcResStsIndex)
                {
                    ResProcStsNameStr = ProcResStatusInfoList[i].ToString();
                    break;
                }
            }
            return ResProcStsNameStr;
        }

#endregion Model


#region 시나리오 Test

        private void TestDataProcClear()
        {
            for (int i = 0; i < TESTPROCMAX; i++)
            {
                OneTestDataProcClear(i);
            }
        }

        private void OneTestDataProcClear(int ClearIndex)
        {
            //DataManager.TestProcList[ClearIndex].P_ProcessStartSec = 0;   // 홍동성 => 코드 막음.
            //DataManager.TestProcList[ClearIndex].TestProcStatus = WAIT_PROCSTATUS;
            DataManager.TestProcList[ClearIndex].T_TestResultSts = NONE_RESULT;
        }

        private int GetCurrentSecond()
        {
            int ResBackTimeSec;
            DateTime dt = DateTime.Now;
            string ResHourStr = dt.ToString("hh");
            string ResMinStr = dt.ToString("mm");
            string ResSecStr = dt.ToString("ss");
            ResBackTimeSec = Convert.ToInt32(ResHourStr) * 3600 + Convert.ToInt32(ResMinStr) * 60 + Convert.ToInt32(ResSecStr);
            return ResBackTimeSec;
        }

        private void DisplayTestTime()
        {
            int TestGapTime;
            int Time_Hour, Time_Min, Time_Sec;
            string PreTimeHour, PreTimeMin, PreTimeSec;
            string DisplayTimeStr;
            TestGapTime = GetCurrentSecond() - TestRunCountTime;
            Time_Hour = TestGapTime / 3600;
            TestGapTime = TestGapTime % 3600;
            Time_Min = TestGapTime / 60;
            Time_Sec = TestGapTime % 60;
            PreTimeHour = "";
            PreTimeMin = "";
            PreTimeSec = "";
            if (Time_Hour < 10)
            {
                PreTimeHour = "0";
            }
            if (Time_Min < 10)
            {
                PreTimeMin = "0";
            }
            if (Time_Sec < 10)
            {
                PreTimeSec = "0";
            }

            DisplayTimeStr = PreTimeHour + Time_Hour.ToString() + ":" + PreTimeMin + Time_Min.ToString() + ":" + PreTimeSec + Time_Sec.ToString();

            
            // ----------
            //txtTestTime.Text = DisplayTimeStr;  // 홍동성 추가 코드 ...
            SetText(DisplayTimeStr);
            // ----------            
        }

        private void SetText(string text)
        {
            if (this.InvokeRequired)
            {
                SetTextCallBack d = new SetTextCallBack(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.txtTestTime.Text = text;

                //this.txtTestTime.Refresh();
            }
        }

#endregion 시나리오 Test


#region Timer

        private void timeTestProc_Tick(object sender, EventArgs e)
        {
            timeTestProc.Enabled = false;

            DisplayTestTime();

            if (LocalTestProcFlag == true)
            {
                if (CurTestProcIndex < 0)
                {
                    CurTestProcIndex = 0;
                }

                // ----------
                if (DataManager.TestProcList[CurTestProcIndex].TestProcExistFlag == true)
                {
                    if (AssembleOneProc() == true)
                    {
                        DataManager.TestProcList[CurTestProcIndex].T_TestResultSts = OK_RESULT;
                    }                        
                    else
                    {
                        if (CurTestProcIndex != -1)
                        {
                            DataManager.TestProcList[CurTestProcIndex].T_TestResultSts = NG_RESULT;
                        }
                        
                    }
                        

                    //ReDrawTestProcList();
                    if (CurTestProcIndex != -1)
                    {
                        SelectReDrawTestProcList(CurTestProcIndex);
                        CurTestProcIndex++;
                    }
                }
                else
                {
                    MessageBox.Show("작업이 종료되었습니다.");

                    LocalTestProcFlag = false;
                    TestTimeThreadFlag = false;

                    CurTestProcIndex = -1;
                    

                    SelectReDrawTestProcList(-1);

                    
                    WorkCompleteReport(); // 2016-05-27 : 작업 완료 시점.

                    PortDisconnect(); // 2016-06-15 : .
                }
                // ----------
            }

            if (LocalTestProcFlag == true)
            {
                timeTestProc.Enabled = true;
            }
        }

#endregion Timer


#region 모션 컨트롤러 ...

        private void InitMotionController()
        {
            MultiMotion.UpdateAxisInfo();

            frmReturnHome frm_return_home = new frmReturnHome();

            frm_return_home.Show();

            Application.DoEvents();

            if (MultiMotion.Initialize() != MultiMotion.KSM_OK)
            {
                
            }            

            frm_return_home.Hide();            
        }

        private void ExitMotionController()
        {
            if (MultiMotion.Exit() != MultiMotion.KSM_OK)
            {

            }
        }

#endregion 모션 컨트롤러 ...


#region 상태 업데이트 ...

        public void UpdateAssembleProcStatus()
        {
            // 1. 진행 ProgressBar 업데이트
            // ----------
            int StepCount = GetAssembleStepCount();

            int CurStepPos = CurTestProcIndex + 1;

            AssembleProgressBar.Value = CurStepPos * 100 / StepCount;


            // 2. 상태 이미지 업데이트
            // ----------
            //UpdateNgOkImage(2);


            // 3. 작업 상태 위치 표시 ...
            // ----------
            DisplayAssembleStepImage();
        }

        private int GetAssembleStepCount()
        {
            int StepCount = 0;

            for (int i = 0; i < 1000; i++)
            {
                if (DataManager.TestProcList[i].TestProcExistFlag == false)
                {
                    return StepCount;
                }

                StepCount++;
            }

            return StepCount;
        }

        public void UpdateNgOkImage(int status)
        {
            string strFileName = "C:\\KSM\\Images\\";

            switch (status)
            {
            case 0: strFileName += "assemble_ng.bmp";  break;
            case 1: strFileName += "assemble_ok.bmp"; break;
            case 2: strFileName += "assemble_ing.bmp"; break;
            }

            if (File.Exists(strFileName) == true)
            {
                NgOkPictureBox.Image = System.Drawing.Image.FromFile(strFileName);
            }
        }

        public void WorkCompleteReport()
        {
            // ...
            // ----------
            if (AssembleOk == true)
            {
                UpdateNgOkImage(1);

                TestCurrentResData.OK_TestCount++;
                TestCurrentResData.NG_TestCount = TestCurrentResData.Total_TestCount - TestCurrentResData.OK_TestCount;
            }
            else
            {
                UpdateNgOkImage(0);

                TestCurrentResData.NG_TestCount++;
                TestCurrentResData.OK_TestCount = TestCurrentResData.Total_TestCount - TestCurrentResData.NG_TestCount;
            }


            // 메인화면 텍스트 업데이트 ...
            // ----------
            TestCurrentResDisplay();


            // 결과 리스트 박스에 추가 ...
            // ----------
            AddResultOKNGList();



            // LOG 파일 생성하기 ...
            // ----------
        }


        // 기존에 작성되어 있던 함수
        private void TestCurrentResClear()
        {
            TestCurrentResData.Total_TestCount = 0;
            TestCurrentResData.OK_TestCount = 0;
            TestCurrentResData.NG_TestCount = 0;
            TestCurrentResDisplay();            
        }

        private void TestCurrentResDisplay()
        {
            // 홍동성 20160616
            
            txtTotalCount.Text = TestCurrentResData.Total_TestCount.ToString();
            txtOKCount.Text = TestCurrentResData.OK_TestCount.ToString();
            txtNGCount.Text = TestCurrentResData.NG_TestCount.ToString();
            
        }

        private void AddResultOKNGList()
        {
            bool AddResOKNGFlag = false;
            string SerialNumStr;
            DateTime currTime = DateTime.Now;
            //AddResOKNGFlag = TestProcOKNG();
            SerialNumStr = txtSerialNumber.Text;

            ListViewItem lstViewTestItem = new ListViewItem(SerialNumStr);

            lstViewTestItem.SubItems.Add(currTime.ToString("yy-MM-dd HH:mm:ss"));
            if (AddResOKNGFlag == true)
            {
                lstViewTestItem.SubItems.Add("OK");
            }
            else
            {
                lstViewTestItem.SubItems.Add("NG");
            }
            lstViewResList.Items.Add(lstViewTestItem);

            lstViewResList.EndUpdate();
        }

        private void DisplayAssembleStepImage()
        {
            string strFileName = "";
            string strFileName_Status = "";

            switch (DataManager.TestProcList[CurTestProcIndex].WFType)
            {
                case 0: // INDEX(고정축) 회전(R) 기능
                case 1: // INDEX(이동축) 회전(R) 기능
                case 3: // 개별 축 이동
                case 4: // INDEX(이동축) 갠트리 X축 이동 기능
                case 5: // 전면(A) 카메라 이동(XYZ) 기능
                case 6: // 전면(B) 카메라 이동(XYZ) 기능
                case 7: // 상방 카메라 이동(XYZ) 기능
                case 12:  // V블럭 위치 이동(Z) 기능
                    strFileName_Status = "C:\\KSM\\Images\\moving.bmp";
                    break;

                case 11: // 영상 인식 INDEX 위치 맞춤 기능
                case 19: // @"고정축 INDEX 전방, 영상 인식 기능", // 19 => 20160816
                case 20: // @"고정축 INDEX 후방, 영상 인식 기능", 
                case 21: // @"이동축 INDEX 전방, 영상 인식 기능", 
                case 22: // @"이동축 INDEX 후방, 영상 인식 기능", 
                case 23: // @"이동축 INDEX 상방, 영상 인식 기능"
                    {
                        strFileName = "C:\\KSM\\Images\\bottom_02_플랜지_측정.png";
                        strFileName_Status = "C:\\KSM\\Images\\testing.bmp";
                    }
                    break;

                case 14: // 용접 기능
                    {
                        strFileName = "C:\\KSM\\Images\\bottom_04_용접.png";
                        strFileName_Status = "C:\\KSM\\Images\\testing.bmp";
                    }
                    break;
                case 2: // Rolling 기능
                    strFileName = "C:\\KSM\\Images\\bottom_03_인덱스_진입.png";
                    strFileName_Status = "C:\\KSM\\Images\\testing.bmp";
                    break;
                default:
                    break;
            }


            if (File.Exists(strFileName) == true && strFileName != "")
            {
                AssembleSteppictureBox.Image = System.Drawing.Image.FromFile(strFileName);

                AssembleSteppictureBox.Refresh();
            }



            if (File.Exists(strFileName_Status) == true && strFileName_Status != "")
            {
                Status_pictureBox.Image = System.Drawing.Image.FromFile(strFileName_Status);

                Status_pictureBox.Refresh();
            }
        }


#endregion 상태 업데이트 ...



#region Test ...

        private void ThreadTestTime()
        {
            //try
            //{
            while (TestTimeThreadFlag)
            {                
                DisplayTestTime();

                if (LocalTestProcFlag == false)
                {
                    break;
                }

                System.Threading.Thread.Sleep(500);

                Application.DoEvents();
            }

            DisplayTestTime();
            //}
            //catch { }
        }


        private void Test()
        {
            return;

            MultiMotion.Swing(true);

            //MultiMotion.UpdateAxisInfo();

            return;

            

            // 모터 전원 공급 ...

            MultiMotion.SystemInit();

            //MultiMotion.SystemOff();

            return;

            MultiMotion.GetDIOStatus();     // DIO 접점 읽기 ...

            return;

            MultiMotion.StopAll();          // 비상 정지 ...

            return;

            MultiMotion.GetCurrentPos();

            return;

            this.Location = new Point(50, 50);


            return;


            // 홍동성 => double형 변환 test
            /*            
            double number = 23.01;
            string strText = number.ToString("0000.0000");
            //char[] delimiter = strText.ToCharArray();
            number = Convert.ToDouble(strText);
            */

            // 시리얼 통신 test
            /*
            List<int> rets = SerialPortManager.SendData(0xB1, 0x0, 0x0);
            if (rets.Count >= 6)
                _100exist = rets[2].Equals(1);
            */

            // 홍동성 - 카메라 UID를 0번째로 설정하여 test
            // --------------------------------------------------
            CurCamUID = 0;

            // 카메라 여는 함수 ...
            if (CameraOneOpen(CUID[CurCamUID]) == true)
            {
                // ...
            }
            else
            {
                MessageBox.Show("카메라를 정상적으로 열지 못하였습니다.");

                return;
            }
            // --------------------------------------------------


            //BStart_Click();


            return;

            // 프로세스 test
            // ----------
            LocalTestProcFlag = true;

            AssembleProc();
            // ----------


            // 파일 포맷 Test ...
            // --------------------------------------------------
            //frmDIOSetting frmDlg = new frmDIOSetting();
            //frmMotionSetting frmDlg = new frmMotionSetting();
            //frmLightingSetting frmDlg = new frmLightingSetting();
            //frmCameraSetting frmDlg = new frmCameraSetting();

            //frmDlg.ShowDialog();
            // --------------------------------------------------

            /*
            frmFuncDIO frmDlg2 = new frmFuncDIO();

            frmDlg2.ShowDialog();
            */

            // 홍동성 - 시나리오 편집기 코드 test
            // ----------
            /*
            frmEditor frmDlg = new frmEditor();

            if (frmDlg.ShowDialog() == DialogResult.Cancel)
            {
                ModelLoad_Display();
            }
            */
            // ----------

            // 설정
            // ----------
            /*
            frmDeviceSetting dlg = new frmDeviceSetting();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
            }
            */
            // ----------


        }

        private void lstViewTestList_Leave(object sender, EventArgs e)
        {
            /*
            return;

            lstViewTestList.SelectedItems.Clear();
            lstViewTestList.Refresh();
            */
        }


#endregion Test ...

        private void timLoopCam_Tick(object sender, EventArgs e)
        {

        }

        private void timTestRunCheck_Tick(object sender, EventArgs e)
        {

        }

        private void timCommPort_Tick(object sender, EventArgs e)
        {

        }

        private void timDelayWait_Tick(object sender, EventArgs e)
        {

        }

        private void btnCalibration_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Calibration을 시작합니다.");
            Status_pictureBox.Visible = false;
            MultiMotion.MainLightOnOff(0);
            MultiMotion.SetSpeed(MultiMotion.KSM_SPEED_FAST);
            CalibrationRun();
            MultiMotion.SetSpeed(MultiMotion.KSM_SPEED_SLOW);
            MultiMotion.MainLightOnOff(1);
            Status_pictureBox.Visible = true;
            CalibrationRunFlag = true;
            MessageBox.Show("Calibration완료!");
        }

        

        private void btnTest_Click(object sender, EventArgs e)
        {
            DeviceManager.PlaySoundM4A("검사를시작합니다.m4a");
            
        }

        private void btnJog_Click(object sender, EventArgs e)
        {
            timerAlarm.Enabled = false;

            frmJogAxis frm_jog_axis = new frmJogAxis();

            frm_jog_axis.ShowDialog();

            timerAlarm.Enabled = true;
        }

        private void timerAlarm_Tick(object sender, EventArgs e)
        {
            MultiMotion.CheckDefense();


            // 알람 체크 ...
            // ----------
            if (MultiMotion.AlarmCheck() != MultiMotion.KSM_OK)
            {
                MultiMotion.StopAll();

                timerAlarm.Enabled = false;

                frmJogAxis frm_jog_axis = new frmJogAxis();

                frm_jog_axis.ShowDialog();

                timerAlarm.Enabled = true;
            }
            // ----------


            // 재기동
            // ----------
            if (MultiMotion.bRestart == true)
            {
                MultiMotion.bRestart = false;


                //lstViewTestList.selecte

                //CurTestProcIndex

                //btnTestStart.PerformClick();
                //btnTestStart.cl
            }
            // ----------
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
            if (DisplayTypeIndex == DIS_TYPE_FULLSHOT)
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

        private void SaveErrorAlignImg()
        {
            string SaveErrPath;
            if (SAVE_ERROR_ALIGN_FLAG==true)
            {
                if (RecoWorkImg != null)
                {
                    SaveErrorAlignPath = System.Environment.CurrentDirectory + @"\SaveErrAlign\" + GetFullYearMonthDayPath();
                    CreateMakeFolderFunc(SaveErrorAlignPath);
                    SaveErrPath = SaveErrorAlignPath + SaveErrorAlignIndex.ToString() + @".jpg";
                    RecoWorkImg.Save(SaveErrPath);
                    SaveErrorAlignIndex++;
                }
            }
        }

        private string GetFullYearMonthDayPath()
        {
            string FullYMDPath;
            DateTime dt = DateTime.Now;
            FullYMDPath = dt.Year.ToString() + "_" + dt.Month.ToString() + "_" + dt.Day.ToString() + "_" + dt.Hour.ToString() + "_" + dt.Minute.ToString() + "_" + dt.Second.ToString();
            return FullYMDPath;
        }

        private void CreateMakeFolderFunc(string CrMakeFolderStr)
        {
            DirectoryInfo ChkDirFld = new DirectoryInfo(CrMakeFolderStr);
            if (ChkDirFld.Exists == false)
            {
                ChkDirFld.Create();
            }
        }

    }
}
