using DiaDetector.Data;
using DiaDetector.Drivers;
using ImageUtilmode;
using NeptuneClassLibWrap;
using PAIX_NMF_DEV;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
// ----------
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ZedGraph;
namespace DiaDetector
{
    public partial class MainFrame : Form
    {
        public static bool testMod = false;

        public MainFrame()
        {
            InitializeComponent();
            InitializeComponents();
            AUTO.Image = Properties.Resources.자동회색;
            MANUAL.Image = Properties.Resources.수동녹색;
            LampOnoff();
            LampSet();
        }
        private void MainFrame_Load(object sender, EventArgs e)
        {
            Initialize();

            if (testMod == false)
            {
                camload();
                TimerSub.Enabled = true;
                TimerSub3.Enabled = true;
            }
            else
            {
                MessageBox.Show("테스트 모드입니다");
            }


            DateTimeCheck();
            ClassType.JobNoDateWhite();
            ClassType.JobNoCreate();
            SmallClass.SetPin(MultiMotion.DPSB, 29, 1);  //황색 램프

        }
        private void MainFrame_FormClosing(object sender, FormClosingEventArgs e)
        {
            ClassType.Run_Flag = false;
            ShuttleOpen();
            LampSetoff();
            ExitMotionController();
            System.Diagnostics.Process.GetCurrentProcess().Kill();

        }
        public void Initialize()
        {
            // 1. 환경 설정 정보 읽기
            // --------------------------------------------------
            DeviceManagerS.Read();
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
            //   ModelLoad_Display(); // DataManager.LoadModelListFiles(ConfigManager.GetModelListPath);
            MultiMotion.UpdateAxisInfo();

            InitMotionController();

            TimerMainTick();
            TimerSubTick();
            TimerSub2Tick();
            TimerSub3Tick();
            TimerSub4Tick();
            TimerSub5Tick();
            DPBSCheck();
            ClassType.JobNoDateWhite();
            Cam1PictureBox.Visible = true;
            Cam2PictureBox.Visible = true;
            Cam1PictureBoxResultOKNG.Visible = true;
            Cam2PictureBoxResultOKNG.Visible = true;
            Cam2PictureBoxResult.Visible = false;
            Cam1PictureBoxResult.Visible = false;
            Cam1PictureBoxResultOKNG.Image = Properties.Resources.준비중;
            Cam2PictureBoxResultOKNG.Image = Properties.Resources.준비중;
            checkBox1.Checked = true;


            datetime = DateTime.Now.ToString("yyyy-MM-dd");

        }
        private void ModelLoad_Display()
        {
            DataManager.LoadModelListFiles_NET();
            DataManager.SelectedModelByName(DataManager.SelectedModelName);
            {
                txtModelName.Text = DataManager.SelectedModelName;
                ModelName.Text = DataManager.SelectedModelName;
                StageNameA.Text = DataManager.SelectedModel.txtSTValueA.ToString();
                StageNameB.Text = DataManager.SelectedModel.txtSTValueB.ToString();
                label3.Text = DataManager.SelectedModel.ndblOutDiameter.ToString();
                label7.Text = DataManager.SelectedModel.NamePie1.ToString();
                label8.Text = DataManager.SelectedModel.InPie.ToString();
                label9.Text = DataManager.SelectedModel.OutPie.ToString();


                SelectModelDatIndex = DataManager.SelectModelIndex;
                if (SelectModelDatIndex > -1)
                {
                    // ----------
                    DataManager.SelectedModel = DataManager.ModelDatList[SelectModelDatIndex];
                    // ----------
                }
            }
        }



        #region 번수들...

        private static DateTime mSTimer = new DateTime();  //초단위 스타트
        private static DateTime mETimer = new DateTime();  //초단위 종료
        public int SelectModelDatIndex;
        ClassType classtype;
        ModelListInfo Modellist;
        MotionValue MotionValueResult;
        #endregion 번수들...

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
        public void DPBSCheck()  //모션컨트롤러 장비번호 체크
        {
            ClassType.DPSB = MultiMotion.DPSB;
        }

        #endregion 모션 컨트롤러 ...   

        #region 조명 ...
        clsLamp lamp = new clsLamp();
        public void LampOnoff()
        {
            lamp = new clsLamp();
            if (MultiMotion.MotionCheck == 2)
            {
                lamp.Open3("COM5");
            }
            else
            {
                MessageBox.Show("조명 통신 오류입니다");
            }

        }

        public void LampSet()
        {
            if (MultiMotion.MotionCheck == 2)
            {
                for (int i = 0; i < 5; i++)
                {
                    lamp.SendCommandSetValue1(i, ClassType.Led1);
                }
            }
        }
        public void LampSetoff()
        {
            if (MultiMotion.MotionCheck == 2)
            {
                for (int i = 0; i < 5; i++)
                {
                    lamp.SendCommandSetValue1(i, ClassType.Led2);
                }
            }

        }
        #endregion 조명 ...

        #region 인식 ...

        private string[] comboBoxCamerastring = new string[ClassType.MAX_CAM];
        private System.Windows.Forms.PictureBox[] pictureBoxDisplay = new System.Windows.Forms.PictureBox[ClassType.MAX_CAM];
        private NeptuneClassLibCLR Neptune1 = null;
        private NEPTUNE_CAM_INFO[] m_CamInfo = null;
        private CameraInstance[] Cam = new CameraInstance[ClassType.MAX_CAM] { null, null };
        private DisplayImage[] m_Display = new DisplayImage[ClassType.MAX_CAM] { null, null };
        private bool[] m_bPlay = new bool[ClassType.MAX_CAM] { false, false };
        private bool[] m_bGrab = new bool[ClassType.MAX_CAM] { false, false };
        private FrameDataPtr[] GETDATA = new FrameDataPtr[ClassType.MAX_CAM] { null, null };

        private Thread[] m_Thread = new Thread[ClassType.MAX_CAM] { null, null };
        private uint m_nGrabCount = 0;
        System.IO.FileStream stream1;  //카메라 1번
        System.IO.FileStream stream2;  //카메라 1번


        public void InitializeComponents()
        {
            CheckForIllegalCrossThreadCalls = false;
        }

        public void CamLive()
        {
            try
            {
                comboBoxGrabMode.SelectedIndex = 0;
                for (int i = 0; i < ClassType.MAX_CAM; i++)
                {
                    if (Cam[i] != null)
                    {
                        if (Cam[i].AcquisitionStart((ENeptuneGrabMode)comboBoxGrabMode.SelectedIndex) != ENeptuneError.NEPTUNE_ERR_Success)
                        {
                            string strMsg = "Cam" + i.ToString() + " Acquisition start error!";
                            MessageBox.Show(strMsg);
                            continue;
                        }
                        m_bPlay[i] = true;
                        if (m_Thread[i] == null)
                        {
                            m_Thread[i] = new Thread(new ParameterizedThreadStart(AcquisitionThread));
                            m_Thread[i].Start(i);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        FrameDataPtr data2 = new FrameDataPtr();
        FrameDataPtr data1 = new FrameDataPtr();

        private void AcquisitionThread(object obj)
        {
            int nIdx = (int)obj;


            try
            {

                while (m_bPlay[nIdx])
                {
                    int aee = nIdx;
                    if (aee == 1)
                    {
                        ENeptuneError eErr2 = Cam[1].WaitEventDataStream(ref data2, 10000);
                        if (eErr2 != ENeptuneError.NEPTUNE_ERR_Success)
                            continue;
                        m_Display[1].DrawRawImage(data2);
                        Cam[1].QueueBufferDataStream(data2.GetBufferIndex());
                    }
                    if (aee == 0)
                    {
                        ENeptuneError eErr1 = Cam[0].WaitEventDataStream(ref data1, 10000);
                        if (eErr1 != ENeptuneError.NEPTUNE_ERR_Success)
                            continue;
                        m_Display[0].DrawRawImage(data1);
                        Cam[0].QueueBufferDataStream(data1.GetBufferIndex());
                    }

                    if (a == 1)
                    {
                        a = 0;
                        byte[] buffer = new byte[data1.GetBufferSize()];
                        Marshal.Copy(data1.GetBufferPtr(), buffer, 0, buffer.Length);
                        byte[] buffer2 = new byte[data2.GetBufferSize()];
                        Marshal.Copy(data2.GetBufferPtr(), buffer2, 0, buffer2.Length);
                        EmguDate1(buffer, buffer2);
                        data1.Dispose();
                        data2.Dispose();
                        Thread.Sleep(30);
                    }
                    if (b == 1)
                    {

                    }
                }


            }
            catch (Exception ex)
            {

            }
        }

        private void UpdateCameraList()
        {
            try
            {
                uint nCam = DeviceManager.Instance.GetTotalCamera();

                m_CamInfo = new NEPTUNE_CAM_INFO[nCam];
                DeviceManager.Instance.GetCameraList(m_CamInfo, nCam);
            }
            catch (Exception ex)
            {
            }
        }

        private void InitFormCtrl()
        {
            pictureBoxDisplay[0] = Cam1PictureBox;
            pictureBoxDisplay[1] = Cam2PictureBox;
        }

        public void camlist()  //콤보박스 강제로 불러와서 그값으로 실행시키기
        {
            {
                NeptuneDevice iDev = DeviceManager.Instance.GetDeviceFromSerial(m_CamInfo[1].strSerial);
                NeptuneDevice iDev1 = DeviceManager.Instance.GetDeviceFromSerial(m_CamInfo[0].strSerial);
                try
                {
                    Cam[0] = new CameraInstance(iDev, ENeptuneDevAccess.NEPTUNE_DEV_ACCESS_EXCLUSIVE);
                    Cam[1] = new CameraInstance(iDev1, ENeptuneDevAccess.NEPTUNE_DEV_ACCESS_EXCLUSIVE);
                }
                catch (Exception exp)
                {

                }
                m_Display[1] = new DisplayImage(pictureBoxDisplay[1].Handle);
                m_Display[0] = new DisplayImage(pictureBoxDisplay[0].Handle);
            }
        }

        int b;
        int a;

        private void Redraw1(Graphics g)
        {

        }

        private void Redraw2(Graphics g)
        {

        }

        public double[] Camera1Size;
        public double[] Camera2Size;
        public double[] Camera3Size;
        public double[] Camera4Size;
        public double[] Camera5Size;
        public double[] Camera6Size;
        public double[] Camera7Size;
        public double[] Camera8Size;
        public double[] Camera9Size;
        public double[] Camera10Size;
        public double[] Camera11Size;
        public double[] Camera12Size;
        public double[] Camera13Size;
        public double[] Camera14Size;
        public double[] Camera15Size;
        public double[] Camera16Size;
        public double[] Camera17Size;
        public double[] Camera18Size;
        public double[] Camera19Size;
        public double[] Camera20Size;

        LineItem curve1;
        LineItem curve2;
        PointPairList list1;
        PointPairList list2;
        GraphPane Zd_Pen1;
        GraphPane Zd_Pen2;

        //유로시스
        double cam1test2 = 0;
        double cam2test2 = 0;

        int Cam1test;
        int Cam2test;


        public double Camera1first = 0.0; //카몌라1 결과 처음
        public double Camera1last = 0.0; //카메라1 결과 마지막
        public double Camera1resultPlus = 0.0; //카메라1 결과 처음-마지막  값
        public double Camera2first = 0.0; //카몌라2 결과 처음
        public double Camera2last = 0.0; //카메라2 결과 마지막
        public double Camera2resultPlus = 0.0; //카메라2 결과 처음-마지막  값
        public double Camera3first = 0.0; //카몌라1 결과 처음
        public double Camera3last = 0.0; //카메라1 결과 마지막
        public double Camera3resultPlus = 0.0; //카메라1 결과 처음-마지막  값
        public double Camera4first = 0.0; //카몌라2 결과 처음
        public double Camera4last = 0.0; //카메라2 결과 마지막
        public double Camera4resultPlus = 0.0; //카메라2 결과 처음-마지막  값
        public double Camera5first = 0.0; //카몌라1 결과 처음
        public double Camera5last = 0.0; //카메라1 결과 마지막
        public double Camera5resultPlus = 0.0; //카메라1 결과 처음-마지막  값
        public string Camera5resultPlusString = ""; //카메라1 결과 처음-마지막  값
        public double Camera6first = 0.0; //카몌라2 결과 처음
        public double Camera6last = 0.0; //카메라2 결과 마지막
        public double Camera6resultPlus = 0.0; //카메라2 결과 처음-마지막  값
        public string Camera6resultPlusString = ""; //카메라1 결과 처음-마지막  값
        public double Camera7first = 0.0; //카몌라1 결과 처음
        public double Camera7last = 0.0; //카메라1 결과 마지막
        public double Camera7resultPlus = 0.0; //카메라1 결과 처음-마지막  값
        public string Camera7resultPlusString = ""; //카메라1 결과 처음-마지막  값
        public double Camera8first = 0.0; //카몌라2 결과 처음
        public double Camera8last = 0.0; //카메라2 결과 마지막
        public double Camera8resultPlus = 0.0; //카메라2 결과 처음-마지막  값
        public string Camera8resultPlusString = ""; //카메라1 결과 처음-마지막  값


        int max1;
        int min1;
        int maxmin1;
        int max3;
        int min3;
        int maxmin3;
        int max5;
        int min5;
        int maxmin5;
        int max7;
        int min7;
        int maxmin7;
        int max9;
        int min9;
        int maxmin9;
        int max11;
        int min11;
        int maxmin11;
        int max13;
        int min13;
        int maxmin13;
        int max15;
        int min15;
        int maxmin15;
        int max17;
        int min17;
        int maxmin17;
        int max19;
        int min19;
        int maxmin19;
        int Final1Max;
        int[] Final1;

        int max2;
        int min2;
        int maxmin2;
        int max4;
        int min4;
        int maxmin4;
        int max6;
        int min6;
        int maxmin6;
        int max8;
        int min8;
        int maxmin8;
        int max10;
        int min10;
        int maxmin10;
        int max12;
        int min12;
        int maxmin12;
        int max14;
        int min14;
        int maxmin14;
        int max16;
        int min16;
        int maxmin16;
        int max18;
        int min18;
        int maxmin18;
        int max20;
        int min20;
        int maxmin20;

        int Final2Max;
        int[] Final2;
        Bitmap res1;
        Bitmap res2;

        public void EmguDate1(byte[] bytes, byte[] bytes2)
        {
            try
            {

                res1 = new Bitmap(1288, 964, PixelFormat.Format8bppIndexed);
                BitmapData LoadBitmap = res1.LockBits(new Rectangle(0, 0, res1.Width, res1.Height), ImageLockMode.ReadWrite, res1.PixelFormat);
                IntPtr ptr = LoadBitmap.Scan0;
                Marshal.Copy(bytes, 0, ptr, res1.Width * res1.Height);

                ColorPalette cp = res1.Palette;
                for (int i = 0; i < 256; i++)
                {
                    cp.Entries[i] = Color.FromArgb(i, i, i);
                }
                res1.Palette = cp;
                res1.UnlockBits(LoadBitmap);
                PixelCounter.CountPixelbyPointer(res1.To32BppBitmap()); //Pointer를 이용한 코드

                res2 = new Bitmap(1288, 964, PixelFormat.Format8bppIndexed);
                BitmapData LoadBitmap2 = res2.LockBits(new Rectangle(0, 0, res2.Width, res2.Height), ImageLockMode.ReadWrite, res2.PixelFormat);
                IntPtr ptr2 = LoadBitmap2.Scan0;
                Marshal.Copy(bytes2, 0, ptr2, res2.Width * res2.Height);

                ColorPalette cp2 = res2.Palette;
                for (int i = 0; i < 256; i++)
                {
                    cp2.Entries[i] = Color.FromArgb(i, i, i);
                }
                res2.Palette = cp2;
                res2.UnlockBits(LoadBitmap2);
                PixelCounter.CountPixelbyPointer2(res2.To32BppBitmap2()); //Pointer를 이용한 코드
                CAM1RESULTVALUE1();
            }
            catch (Exception EX)
            {
                CAM1RESULTVALUE1();
            }

        }

        public void CAM1RESULTVALUE1() //카메라1
        {
            try
            {

                maxmin3 = 0;
                maxmin5 = 0;
                maxmin7 = 0;
                maxmin9 = 0;
                maxmin11 = 0;
                maxmin13 = 0;
                maxmin15 = 0;

                if (PixelCounter.HongPoint1.Count != 0)
                {
                    max1 = PixelCounter.HongPoint1[0];
                    min1 = PixelCounter.HongPoint1[0];
                    for (int i = 0; i < PixelCounter.HongPoint1.Count; i++)
                    {
                        if (PixelCounter.HongPoint1[i] > max1)
                            max1 = PixelCounter.HongPoint1[i];
                        if (PixelCounter.HongPoint1[i] < min1)
                            min1 = PixelCounter.HongPoint1[i];
                    }
                    maxmin1 = max1 - min1;
                }

                if (PixelCounter.HongPoint2.Count != 0)
                {
                    max3 = PixelCounter.HongPoint2[0];
                    min3 = PixelCounter.HongPoint2[0];
                    for (int i = 0; i < PixelCounter.HongPoint2.Count; i++)
                    {
                        if (PixelCounter.HongPoint2[i] > max3)
                            max3 = PixelCounter.HongPoint2[i];
                        if (PixelCounter.HongPoint2[i] < min3)
                            min3 = PixelCounter.HongPoint2[i];
                    }
                    maxmin3 = max3 - min3;
                }

                if (PixelCounter.HongPoint3.Count != 0)
                {
                    max5 = PixelCounter.HongPoint3[0];
                    min5 = PixelCounter.HongPoint3[0];
                    for (int i = 0; i < PixelCounter.HongPoint3.Count; i++)
                    {
                        if (PixelCounter.HongPoint3[i] > max5)
                            max5 = PixelCounter.HongPoint3[i];
                        if (PixelCounter.HongPoint3[i] < min5)
                            min5 = PixelCounter.HongPoint3[i];
                    }
                    maxmin5 = max5 - min5;
                }

                if (PixelCounter.HongPoint4.Count != 0)
                {
                    max7 = PixelCounter.HongPoint4[0];
                    min7 = PixelCounter.HongPoint4[0];
                    for (int i = 0; i < PixelCounter.HongPoint4.Count; i++)
                    {
                        if (PixelCounter.HongPoint4[i] > max7)
                            max7 = PixelCounter.HongPoint4[i];
                        if (PixelCounter.HongPoint4[i] < min7)
                            min7 = PixelCounter.HongPoint4[i];
                    }
                    maxmin7 = max7 - min7;
                }

                if (PixelCounter.HongPoint5.Count != 0)
                {
                    max9 = PixelCounter.HongPoint5[0];
                    min9 = PixelCounter.HongPoint5[0];
                    for (int i = 0; i < PixelCounter.HongPoint5.Count; i++)
                    {
                        if (PixelCounter.HongPoint5[i] > max9)
                            max9 = PixelCounter.HongPoint5[i];
                        if (PixelCounter.HongPoint5[i] < min9)
                            min9 = PixelCounter.HongPoint5[i];
                    }
                    maxmin9 = max9 - min9;
                }

                if (PixelCounter.HongPoint6.Count != 0)
                {
                    max11 = PixelCounter.HongPoint6[0];
                    min11 = PixelCounter.HongPoint6[0];
                    for (int i = 0; i < PixelCounter.HongPoint6.Count; i++)
                    {
                        if (PixelCounter.HongPoint6[i] > max11)
                            max11 = PixelCounter.HongPoint6[i];
                        if (PixelCounter.HongPoint6[i] < min11)
                            min11 = PixelCounter.HongPoint6[i];
                    }
                    maxmin11 = max11 - min11;
                }

                if (PixelCounter.HongPoint7.Count != 0)
                {
                    max13 = PixelCounter.HongPoint7[0];
                    min13 = PixelCounter.HongPoint7[0];
                    for (int i = 0; i < PixelCounter.HongPoint7.Count; i++)
                    {
                        if (PixelCounter.HongPoint7[i] > max13)
                            max13 = PixelCounter.HongPoint7[i];
                        if (PixelCounter.HongPoint7[i] < min13)
                            min13 = PixelCounter.HongPoint7[i];
                    }
                    maxmin13 = max13 - min13;
                }

                if (PixelCounter.HongPoint8.Count != 0)
                {
                    max15 = PixelCounter.HongPoint8[0];
                    min15 = PixelCounter.HongPoint8[0];
                    for (int i = 0; i < PixelCounter.HongPoint8.Count; i++)
                    {
                        if (PixelCounter.HongPoint8[i] > max15)
                            max15 = PixelCounter.HongPoint8[i];
                        if (PixelCounter.HongPoint8[i] < min15)
                            min15 = PixelCounter.HongPoint8[i];
                    }
                    maxmin15 = max15 - min15;
                }

                Final1 = new int[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
                Final1Max = maxmin1;
                Final1 = new int[8] { maxmin1, maxmin3, maxmin5, maxmin7, maxmin9, maxmin11, maxmin13, maxmin15 };
                for (int i = 0; i < Final1.Length; i++)
                {
                    if (Final1[i] > Final1Max)
                        Final1Max = Final1[i];
                }

                if (ClassType.Pie <= 0.005)
                {

                    if (Final1Max <= 26 && Final1Max != 0)
                    {
                        if (ClassType.CameraMode == false)
                        {
                            ClassType.Camera1result = true;
                            Cam1PictureBoxResultOKNG.Image = Properties.Resources.정상;
                        }
                        else
                        {
                            ClassType.Os1State = ClassType.Os1OK;
                        }
                    }
                    else
                    {
                        if (ClassType.CameraMode == false)
                        {
                            ClassType.Camera1result = false;
                            Cam1PictureBoxResultOKNG.Image = Properties.Resources.불량;
                        }
                        else
                        {
                            ClassType.Os1State = ClassType.Os1NG;
                        }

                    }
                    CAM1RESULTVALUE2();
                }
                else if (ClassType.Pie >= 0.006 && ClassType.Pie <= 0.007)
                {

                    if (Final1Max <= 32 && Final1Max != 0)
                    {
                        if (ClassType.CameraMode == false)
                        {
                            ClassType.Camera1result = true;
                            Cam1PictureBoxResultOKNG.Image = Properties.Resources.정상;
                        }
                        else
                        {
                            ClassType.Os1State = ClassType.Os1OK;

                        }

                    }
                    else
                    {
                        if (ClassType.CameraMode == false)
                        {
                            ClassType.Camera1result = false;
                            Cam1PictureBoxResultOKNG.Image = Properties.Resources.불량;
                        }
                        else
                        {
                            ClassType.Os1State = ClassType.Os1NG;
                        }

                    }
                    CAM1RESULTVALUE2();
                }
                else
                {
                    if (Final1Max <= 42 && Final1Max != 0)
                    {
                        if (ClassType.CameraMode == false)
                        {
                            ClassType.Camera1result = true;
                            Cam1PictureBoxResultOKNG.Image = Properties.Resources.정상;
                        }
                        else
                        {
                            ClassType.Os1State = ClassType.Os1OK;
                        }
                    }
                    else
                    {
                        if (ClassType.CameraMode == false)
                        {
                            ClassType.Camera1result = false;
                            Cam1PictureBoxResultOKNG.Image = Properties.Resources.불량;
                        }
                        else
                        {
                            ClassType.Os1State = ClassType.Os1NG;
                        }
                    }
                    CAM1RESULTVALUE2();
                }
            }
            catch (Exception ex)
            {
                if (ClassType.CameraMode == false)
                {
                    ClassType.Camera1result = false;
                    Cam1PictureBoxResultOKNG.Image = Properties.Resources.불량;
                }
                else
                {
                    CAM1RESULTVALUE2();
                    ClassType.Os1State = ClassType.Os1NG;
                    Cam1PictureBoxResultOKNG.Image = Properties.Resources.불량;
                }
            }

        }
        public void CAM1RESULTVALUE2() //카메라1
        {
            try
            {
                maxmin2 = 0;
                maxmin4 = 0;
                maxmin6 = 0;
                maxmin8 = 0;
                maxmin10 = 0;
                maxmin12 = 0;
                maxmin14 = 0;
                maxmin16 = 0;


                if (PixelCounter.HongPoint9.Count != 0)
                {
                    max2 = PixelCounter.HongPoint9[0];
                    min2 = PixelCounter.HongPoint9[0];
                    for (int i = 0; i < PixelCounter.HongPoint9.Count; i++)
                    {
                        if (PixelCounter.HongPoint9[i] > max2)
                            max2 = PixelCounter.HongPoint9[i];
                        if (PixelCounter.HongPoint9[i] < min2)
                            min2 = PixelCounter.HongPoint9[i];
                    }
                    maxmin2 = max2 - min2;
                }


                if (PixelCounter.HongPoint10.Count != 0)
                {
                    max4 = PixelCounter.HongPoint10[0];
                    min4 = PixelCounter.HongPoint10[0];
                    for (int i = 0; i < PixelCounter.HongPoint10.Count; i++)
                    {
                        if (PixelCounter.HongPoint10[i] > max4)
                            max4 = PixelCounter.HongPoint10[i];
                        if (PixelCounter.HongPoint10[i] < min4)
                            min4 = PixelCounter.HongPoint10[i];
                    }
                    maxmin4 = max4 - min4;
                }

                if (PixelCounter.HongPoint11.Count != 0)
                {
                    max6 = PixelCounter.HongPoint11[0];
                    min6 = PixelCounter.HongPoint11[0];
                    for (int i = 0; i < PixelCounter.HongPoint11.Count; i++)
                    {
                        if (PixelCounter.HongPoint11[i] > max6)
                            max6 = PixelCounter.HongPoint11[i];
                        if (PixelCounter.HongPoint11[i] < min6)
                            min6 = PixelCounter.HongPoint11[i];
                    }
                    maxmin6 = max6 - min6;
                }

                if (PixelCounter.HongPoint12.Count != 0)
                {
                    max8 = PixelCounter.HongPoint12[0];
                    min8 = PixelCounter.HongPoint12[0];
                    for (int i = 0; i < PixelCounter.HongPoint12.Count; i++)
                    {
                        if (PixelCounter.HongPoint12[i] > max8)
                            max8 = PixelCounter.HongPoint12[i];
                        if (PixelCounter.HongPoint12[i] < min8)
                            min8 = PixelCounter.HongPoint12[i];
                    }
                    maxmin8 = max8 - min8;
                }


                if (PixelCounter.HongPoint13.Count != 0)
                {
                    max10 = PixelCounter.HongPoint13[0];
                    min10 = PixelCounter.HongPoint13[0];
                    for (int i = 0; i < PixelCounter.HongPoint13.Count; i++)
                    {
                        if (PixelCounter.HongPoint13[i] > max10)
                            max10 = PixelCounter.HongPoint13[i];
                        if (PixelCounter.HongPoint13[i] < min10)
                            min10 = PixelCounter.HongPoint13[i];
                    }
                    maxmin10 = max10 - min10;
                }

                if (PixelCounter.HongPoint14.Count != 0)
                {
                    max12 = PixelCounter.HongPoint14[0];
                    min12 = PixelCounter.HongPoint14[0];
                    for (int i = 0; i < PixelCounter.HongPoint14.Count; i++)
                    {
                        if (PixelCounter.HongPoint14[i] > max12)
                            max12 = PixelCounter.HongPoint14[i];
                        if (PixelCounter.HongPoint14[i] < min12)
                            min12 = PixelCounter.HongPoint14[i];
                    }
                    maxmin12 = max12 - min12;
                }


                if (PixelCounter.HongPoint15.Count != 0)
                {
                    max14 = PixelCounter.HongPoint15[0];
                    min14 = PixelCounter.HongPoint15[0];
                    for (int i = 0; i < PixelCounter.HongPoint15.Count; i++)
                    {
                        if (PixelCounter.HongPoint15[i] > max14)
                            max14 = PixelCounter.HongPoint15[i];
                        if (PixelCounter.HongPoint15[i] < min14)
                            min14 = PixelCounter.HongPoint15[i];
                    }
                    maxmin14 = max14 - min14;
                }

                if (PixelCounter.HongPoint16.Count != 0)
                {
                    max16 = PixelCounter.HongPoint16[0];
                    min16 = PixelCounter.HongPoint16[0];
                    for (int i = 0; i < PixelCounter.HongPoint16.Count; i++)
                    {
                        if (PixelCounter.HongPoint16[i] > max16)
                            max16 = PixelCounter.HongPoint16[i];
                        if (PixelCounter.HongPoint16[i] < min16)
                            min16 = PixelCounter.HongPoint16[i];
                    }
                    maxmin16 = max16 - min16;
                }

                Final2 = new int[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
                Final2Max = maxmin2;
                Final2 = new int[8] { maxmin2, maxmin4, maxmin6, maxmin8, maxmin10, maxmin12, maxmin14, maxmin16 };
                for (int i = 0; i < Final2.Length; i++)
                {
                    if (Final2[i] > Final2Max)
                        Final2Max = Final2[i];
                }
                if (ClassType.Pie <= 0.005)
                {
                    if (Final2Max <= 26 && Final2Max != 0)
                    {
                        if (ClassType.CameraMode == false)
                        {
                            ClassType.Camera2result = true;
                            Cam2PictureBoxResultOKNG.Image = Properties.Resources.정상;
                        }
                        else
                        {
                            ClassType.Os2State = ClassType.Os2OK;
                            ClassType.JobResultState = ClassType.WORK_END;

                        }
                    }
                    else
                    {
                        if (ClassType.CameraMode == false)
                        {
                            //  ClassType.DateTimeWhite("2번 카메라" + "    " + "[" + ClassType.DateSec + "]" + "            결과 값:" + "NG", ClassType.Result);
                            ClassType.Camera2result = false;
                            Cam2PictureBoxResultOKNG.Image = Properties.Resources.불량;
                        }
                        else
                        {
                            ClassType.Os2State = ClassType.Os2NG;
                            ClassType.JobResultState = ClassType.WORK_END;
                        }
                    }
                }
                //20차이상
                else if (ClassType.Pie >= 0.006 && ClassType.Pie <= 0.007)
                {
                    if (Final2Max <= 32 && Final2Max != 0)
                    {
                        if (ClassType.CameraMode == false)
                        {
                            ClassType.Camera2result = true;
                            Cam2PictureBoxResultOKNG.Image = Properties.Resources.정상;

                        }
                        else
                        {
                            ClassType.Os2State = ClassType.Os2OK;
                            ClassType.JobResultState = ClassType.WORK_END;
                        }

                    }
                    else
                    {
                        if (ClassType.CameraMode == false)
                        {
                            //  ClassType.DateTimeWhite("2번 카메라" + "    " + "[" + ClassType.DateSec + "]" + "            결과 값:" + "NG", ClassType.Result);
                            ClassType.Camera2result = false;
                            Cam2PictureBoxResultOKNG.Image = Properties.Resources.불량;
                        }
                        else
                        {
                            ClassType.Os2State = ClassType.Os2NG;
                            ClassType.JobResultState = ClassType.WORK_END;

                        }
                    }
                }
                else
                {
                    if (Final2Max <= 42 && Final2Max != 0)
                    {
                        if (ClassType.CameraMode == false)
                        {
                            ClassType.Camera2result = true;
                            Cam2PictureBoxResultOKNG.Image = Properties.Resources.정상;

                        }
                        else
                        {
                            ClassType.Os2State = ClassType.Os2OK;
                            ClassType.JobResultState = ClassType.WORK_END;
                        }

                    }
                    else
                    {
                        if (ClassType.CameraMode == false)
                        {
                            //  ClassType.DateTimeWhite("2번 카메라" + "    " + "[" + ClassType.DateSec + "]" + "            결과 값:" + "NG", ClassType.Result);
                            ClassType.Camera2result = false;
                            Cam2PictureBoxResultOKNG.Image = Properties.Resources.불량;
                        }
                        else
                        {
                            ClassType.Os2State = ClassType.Os2NG;
                            ClassType.JobResultState = ClassType.WORK_END;

                        }
                    }
                }
            }
            catch (Exception EX)
            {
                if (ClassType.CameraMode == false)
                {
                    ClassType.Camera2result = false;
                    Cam2PictureBoxResultOKNG.Image = Properties.Resources.불량;
                }
                else
                {
                    ClassType.Os2State = ClassType.Os2NG;
                    Cam2PictureBoxResultOKNG.Image = Properties.Resources.불량;
                    ClassType.JobResultState = ClassType.WORK_END;
                }
            }
        }

        public double[] doublem;

        public void LiveExit()
        {
            try
            {
                for (int i = 0; i < ClassType.MAX_CAM; i++)
                {
                    if (Cam[i] != null)
                    {
                        m_bPlay[i] = false;

                        if (comboBoxGrabMode.SelectedIndex == (int)ENeptuneGrabMode.NEPTUNE_GRAB_CONTINUOUS)
                        {
                            m_Thread[i].Join();
                            m_Thread[i] = null;
                        }

                        Cam[i].AcquisitionStop();

                        comboBoxGrabMode.Enabled = true;
                        m_nGrabCount = 0;

                        pictureBoxDisplay[i].Image = null;
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }


        public void close2()
        {
            for (int i = 0; i < ClassType.MAX_CAM; i++)
            {
                if (m_bPlay[i])
                {
                    m_bPlay[i] = false;
                    if (comboBoxGrabMode.SelectedIndex == (int)ENeptuneGrabMode.NEPTUNE_GRAB_CONTINUOUS)
                        m_Thread[i].Join();
                    Cam[i].AcquisitionStop();
                }

                if (Cam[i] != null)
                    Cam[i].CameraClose();
            }
        }



        public void camload()
        {
            InitFormCtrl();
            Neptune1 = new NeptuneClassLibCLR();
            Neptune1.InitLibrary();
            UpdateCameraList();//캠정보 업데이트
            camlist();
            CamLive();  //캠정보저장 후 불러오기
        }



        public void calibration()
        {
            close2();
            Neptune1.UninitLibrary();
            DialogResult dr = new DialogResult();
            BTBEdit BTBEdits = new BTBEdit();
            dr = BTBEdits.ShowDialog();
            if (dr == DialogResult.OK)
            {
                camload();
                LiveExit();
                CamLive();
            }
        }



        #endregion 인식 ...

        #region 모델 ...
        string mdata;
        frmModelList frmDlg = new frmModelList();
        ArrayList arrys = new ArrayList();
        ArrayList arrystime = new ArrayList();
        private void btnModelOpen_Click(object sender, EventArgs e)
        {

            try
            {
                datetime = DateTime.Now.ToString("yyyy-MM-dd");

                updateplus();
                if (ClassType.Run_Flag == true)
                {
                    MessageBox.Show(ClassType.TestWaitMsg);
                    return;
                }
                frmDlg = new frmModelList();
                if (frmDlg.ShowDialog() == DialogResult.OK)
                {
                    arrys.Clear();
                    mdata = frmDlg.SelectModelIndextxt;
                    updatenull();
                    ModelLoad_Display();
                    Fastime();
                }
                DeviceManagerS.Write_SelectedModelInfo();
            }
            catch (Exception ex)
            {

            }
        }

        public void Modif()
        {
            LBLMakeOut.Text = ClassType.TotalTime / 3600 + "시간:" + ClassType.TotalTime % 3600 / 60 + "분" + ClassType.TotalTime % 3600 % 60 + "초";
            label5.Text = ClassType.Totalok.ToString();
            label6.Text = ClassType.Totalng.ToString();

            label4.Text = (ClassType.Totalok + ClassType.Totalng).ToString();
        }
        public void Fastime()
        {
            arrys.Clear();
            string sql11 = "SELECT Numbers, Numbersng , times FROM Table1 WHERE Production like '" + datetime + "' AND  Model = '" + DataManager.SelectedModelName + "'";
            DataSet ds = Microsoft_OleDb.Microsoft_OleDb.GetDataTime(sql11);
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                arrys.Add((string)row["times"].ToString());
                arrys.Add((string)row["Numbers"].ToString());
                arrys.Add((string)row["Numbersng"].ToString());
            }

            if (arrys.Count < 3) return;

            int Laert = int.Parse(arrys[0].ToString());
            int Laertok = int.Parse(arrys[1].ToString());
            int Laertng = int.Parse(arrys[2].ToString());
            LBLMakeOut.Text = Laert / 3600 + "시간:" + Laert % 3600 / 60 + "분" + Laert % 3600 % 60 + "초";
            label5.Text = Laertok.ToString();
            label6.Text = Laertng.ToString();
            ClassType.Totalok = Laertok;
            ClassType.Totalng = Laertng;
            ClassType.TotalTime = Laert;
            label4.Text = (Laertok + Laertng).ToString();
        }
        #endregion 모델 ...

        #region 폼이동 ...
        private Point mousePoint;

        private void MainFrame_MouseDown(object sender, MouseEventArgs e)
        {
            mousePoint = new Point(e.X, e.Y);
        }

        private void MainFrame_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                Location = new Point(this.Left - (mousePoint.X - e.X),
                    this.Top - (mousePoint.Y - e.Y));
            }
        }
        #endregion 폼이동..

        #region 버튼 이벤트 ...

        private void btnMotionSetting_Click(object sender, EventArgs e)
        {
            if (ClassType.Run_Flag == true)
            {
                MessageBox.Show(ClassType.TestWaitMsg);
                return;
            }
            frmMotionSetting frmDlg = new frmMotionSetting();

            frmDlg.ShowDialog();
        }

        private void btnDIOSetting_Click(object sender, EventArgs e)
        {
            if (ClassType.Run_Flag == true)
            {
                MessageBox.Show(ClassType.TestWaitMsg);
                return;
            }
            frmDIOSetting frmDlg = new frmDIOSetting();

            frmDlg.ShowDialog();
        }

        private void btnJog_Click(object sender, EventArgs e)
        {
            if (ClassType.Run_Flag == true)
            {
                MessageBox.Show(ClassType.TestWaitMsg);
                return;
            }
            if (ClassType.AutoManual == true)
            {
                MessageBox.Show(ClassType.TestWaitMsg4);
                return;
            };
            if (ClassType.Run_Flag1 == true)
            {
                MessageBox.Show(ClassType.TestWaitMsg);
                return;
            };
            frmJog frm_jog = new frmJog();

            frm_jog.ShowDialog();
        }

        private void btnSetting_Click(object sender, EventArgs e)
        {
            if (ClassType.Run_Flag == true)
            {
                MessageBox.Show(ClassType.TestWaitMsg);
                return;
            }
            frmDeviceSetting dlg = new frmDeviceSetting();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
            }
        }

        private void btnRecoStart_Click(object sender, EventArgs e)
        {
            try
            {

                if (ClassType.Run_Flag == true)
                {
                    MessageBox.Show(ClassType.TestWaitMsg);
                    return;
                }
                if (ClassType.Run_Flag1 == true)
                {
                    MessageBox.Show(ClassType.TestWaitMsg);
                    return;
                }
                if (ClassType.AutoManual == false)
                {
                    MessageBox.Show(ClassType.TestWaitMsg2);
                    return;
                }
                if (ClassType.ShuttleReady == false)
                {
                    MessageBox.Show(ClassType.TestWaitMsg6);
                    return;
                }
                if (MultiMotion.InStatus[30] == 1 && MultiMotion.InStatus[31] == 1)
                {
                    MessageBox.Show(ClassType.TestWaitMsg7);
                    return;
                }

                if (txtModelName.Text == "")
                {
                    MessageBox.Show(ClassType.TestWaitMsg8);
                    return;
                }

                btnRecoStart.Image = Properties.Resources.시작녹색;
                btnRecoStop.Image = Properties.Resources.정지회색;
                TimerSub5.Enabled = true; //토탈 데이터 저장
                if (ClassType.RestartCheck == false)
                {
                    if (MultiMotion.InStatus[26] == 0 || MultiMotion.InStatus[28] == 0)
                    {
                        if (MultiMotion.InStatus[18] == 1 && MultiMotion.InStatus[19] == 1 && MultiMotion.InStatus[22] == 1 && MultiMotion.InStatus[23] == 1) //셔틀클램프가 오픈일때

                        {
                            ClassType.JobResult = ClassType.JobShuttle1;
                            ClassType.JobResultState = ClassType.WORK_START;
                            TimerMain.Enabled = true;
                            ClassType.Run_Flag = true;

                        }
                    }
                    else
                    {
                        ClassType.RestartCheck = true;
                        ClassType.JobData = System.IO.File.ReadAllLines(ClassType._startUPPath + ClassType.DateDay + ClassType._startUPPath2 + ClassType.JobDate, System.Text.Encoding.Default);
                        ClassType.Run_Flag = true;
                        ClassType.JobResult = ClassType.JobStart;
                        ClassType.JobResultState = ClassType.WORK_START;
                        ClassType.JobLiftResult = ClassType.JobLiftStart;
                        ClassType.JobLiftResultState = ClassType.LiftWORK_START;
                        TimerMain.Enabled = true;
                        SmallClass.SetPin(MultiMotion.DPSB, 17, 0);  //셔틀1 오픈
                        SmallClass.SetPin(MultiMotion.DPSB, 21, 0);  //셔틀2 오픈      
                        SmallClass.SetPin(MultiMotion.DPSB, 16, 1);  //셔틀1 클로즈
                        SmallClass.SetPin(MultiMotion.DPSB, 20, 1);  //셔틀2 클로즈   

                    }
                }
                else
                {
                    if (MultiMotion.InStatus[26] == 0 || MultiMotion.InStatus[28] == 0)
                    {
                        if (MultiMotion.InStatus[27] == 0 || MultiMotion.InStatus[29] == 0) //가이드 센서 상위 감지되면 클램프 오프
                        {
                            if (MultiMotion.InStatus[18] == 1 && MultiMotion.InStatus[19] == 1 && MultiMotion.InStatus[22] == 1 && MultiMotion.InStatus[23] == 1) //셔틀클램프가 오픈일때
                            {
                                ClassType.JobResult = ClassType.JobShuttle1;
                                ClassType.JobResultState = ClassType.WORK_START;
                                TimerMain.Enabled = true;
                            }
                        }
                        else if ((Math.Abs(ClassType.sineps45) <= ClassType.eps))
                        {
                            if (ClassType.Restart == false)  //0작업
                            {
                                ClassType.JobData = System.IO.File.ReadAllLines(ClassType._startUPPath + ClassType.DateDay + ClassType._startUPPath2 + ClassType.JobDate, System.Text.Encoding.Default);
                                SmallClass.SetPin(MultiMotion.DPSB, 8, 0);  //리프트 업  
                                ClassType.Run_Flag = true;
                                ClassType.JobResult = ClassType.JobRotation0;
                                ClassType.JobResultState = ClassType.WORK_START;
                                TimerMain.Enabled = true;
                                SmallClass.SetPin(MultiMotion.DPSB, 17, 0);  //셔틀1 오픈
                                SmallClass.SetPin(MultiMotion.DPSB, 21, 0);  //셔틀2 오픈      
                                SmallClass.SetPin(MultiMotion.DPSB, 16, 1);  //셔틀1 클로즈
                                SmallClass.SetPin(MultiMotion.DPSB, 20, 1);  //셔틀2 클로즈    

                            }
                            else if (ClassType.Restart == true) //90도작업
                            {
                                ClassType.JobData = System.IO.File.ReadAllLines(ClassType._startUPPath + ClassType.DateDay + ClassType._startUPPath2 + ClassType.JobDate, System.Text.Encoding.Default);
                                SmallClass.SetPin(MultiMotion.DPSB, 8, 0);  //리프트 업  
                                ClassType.Run_Flag = true;
                                ClassType.JobResult = ClassType.JobRotation90;
                                ClassType.JobResultState = ClassType.WORK_START;
                                TimerMain.Enabled = true;
                                SmallClass.SetPin(MultiMotion.DPSB, 17, 0);  //셔틀1 오픈
                                SmallClass.SetPin(MultiMotion.DPSB, 21, 0);  //셔틀2 오픈      
                                SmallClass.SetPin(MultiMotion.DPSB, 16, 1);  //셔틀1 클로즈
                                SmallClass.SetPin(MultiMotion.DPSB, 20, 1);  //셔틀2 클로즈    
                            }
                        }
                    }
                    else
                    {
                        if ((Math.Abs(ClassType.sineps45) <= ClassType.eps))
                        {
                            if (ClassType.Restart == false)  //0작업
                            {
                                ClassType.JobData = System.IO.File.ReadAllLines(ClassType._startUPPath + ClassType.DateDay + ClassType._startUPPath2 + ClassType.JobDate, System.Text.Encoding.Default);
                                SmallClass.SetPin(MultiMotion.DPSB, 8, 0);  //리프트 업  
                                ClassType.Run_Flag = true;
                                ClassType.JobResult = ClassType.JobRotation0;
                                ClassType.JobResultState = ClassType.WORK_START;
                                TimerMain.Enabled = true;
                                SmallClass.SetPin(MultiMotion.DPSB, 17, 0);  //셔틀1 오픈
                                SmallClass.SetPin(MultiMotion.DPSB, 21, 0);  //셔틀2 오픈      
                                SmallClass.SetPin(MultiMotion.DPSB, 16, 1);  //셔틀1 클로즈
                                SmallClass.SetPin(MultiMotion.DPSB, 20, 1);  //셔틀2 클로즈    
                            }
                            else if (ClassType.Restart == true) //90도작업
                            {
                                ClassType.JobData = System.IO.File.ReadAllLines(ClassType._startUPPath + ClassType.DateDay + ClassType._startUPPath2 + ClassType.JobDate, System.Text.Encoding.Default);
                                SmallClass.SetPin(MultiMotion.DPSB, 8, 0);  //리프트 업  
                                ClassType.Run_Flag = true;
                                ClassType.JobResult = ClassType.JobRotation90;
                                ClassType.JobResultState = ClassType.WORK_START;
                                TimerMain.Enabled = true;
                                SmallClass.SetPin(MultiMotion.DPSB, 17, 0);  //셔틀1 오픈
                                SmallClass.SetPin(MultiMotion.DPSB, 21, 0);  //셔틀2 오픈      
                                SmallClass.SetPin(MultiMotion.DPSB, 16, 1);  //셔틀1 클로즈
                                SmallClass.SetPin(MultiMotion.DPSB, 20, 1);  //셔틀2 클로즈    
                            }
                        }
                    }

                }

            }
            catch (Exception ex)
            {

            }
        }

        private void btnRecoReStart_Click(object sender, EventArgs e) //재시작
        {
            if (ClassType.Run_Flag == true)
            {
                MessageBox.Show(ClassType.TestWaitMsg);
                return;
            }
            if (ClassType.Run_Flag1 == true)
            {
                MessageBox.Show(ClassType.TestWaitMsg);
                return;
            }
            if (ClassType.AutoManual == false)
            {
                MessageBox.Show(ClassType.TestWaitMsg2);
                return;
            }

            if (ClassType.RestartCheck == false)
            {
                if (MultiMotion.InStatus[26] == 0 || MultiMotion.InStatus[28] == 0)
                {
                    if (MultiMotion.InStatus[18] == 1 && MultiMotion.InStatus[19] == 1 && MultiMotion.InStatus[22] == 1 && MultiMotion.InStatus[23] == 1) //셔틀클램프가 오픈일때

                    {
                        ClassType.JobResult = ClassType.JobShuttle1;
                        ClassType.JobResultState = ClassType.WORK_START;
                        TimerMain.Enabled = true;
                        ClassType.Run_Flag = true;
                    }
                }
                else
                {
                    ClassType.RestartCheck = true;
                    ClassType.JobData = System.IO.File.ReadAllLines(ClassType._startUPPath + ClassType.DateDay + ClassType._startUPPath2 + ClassType.JobDate, System.Text.Encoding.Default);
                    ClassType.Run_Flag = true;
                    ClassType.JobResult = ClassType.JobStart;
                    ClassType.JobResultState = ClassType.WORK_START;
                    ClassType.JobLiftResult = ClassType.JobLiftStart;
                    ClassType.JobLiftResultState = ClassType.LiftWORK_START;
                    TimerMain.Enabled = true;
                    SmallClass.SetPin(MultiMotion.DPSB, 17, 0);  //셔틀1 오픈
                    SmallClass.SetPin(MultiMotion.DPSB, 21, 0);  //셔틀2 오픈      
                    SmallClass.SetPin(MultiMotion.DPSB, 16, 1);  //셔틀1 클로즈
                    SmallClass.SetPin(MultiMotion.DPSB, 20, 1);  //셔틀2 클로즈    
                }
            }
            else
            {
                if (MultiMotion.InStatus[26] == 0 || MultiMotion.InStatus[28] == 0)
                {
                    if (MultiMotion.InStatus[27] == 0 || MultiMotion.InStatus[29] == 0) //가이드 센서 상위 감지되면 클램프 오프
                    {
                        if (MultiMotion.InStatus[18] == 1 && MultiMotion.InStatus[19] == 1 && MultiMotion.InStatus[22] == 1 && MultiMotion.InStatus[23] == 1) //셔틀클램프가 오픈일때
                        {
                            ClassType.JobResult = ClassType.JobShuttle1;
                            ClassType.JobResultState = ClassType.WORK_START;
                            TimerMain.Enabled = true;
                        }
                    }
                    else if ((Math.Abs(ClassType.sineps45) <= ClassType.eps))
                    {
                        if (ClassType.Restart == false)  //0작업
                        {
                            ClassType.JobData = System.IO.File.ReadAllLines(ClassType._startUPPath + ClassType.DateDay + ClassType._startUPPath2 + ClassType.JobDate, System.Text.Encoding.Default);
                            SmallClass.SetPin(MultiMotion.DPSB, 8, 0);  //리프트 업  
                            ClassType.Run_Flag = true;
                            ClassType.JobResult = ClassType.JobRotation0;
                            ClassType.JobResultState = ClassType.WORK_START;
                            TimerMain.Enabled = true;
                            SmallClass.SetPin(MultiMotion.DPSB, 17, 0);  //셔틀1 오픈
                            SmallClass.SetPin(MultiMotion.DPSB, 21, 0);  //셔틀2 오픈      
                            SmallClass.SetPin(MultiMotion.DPSB, 16, 1);  //셔틀1 클로즈
                            SmallClass.SetPin(MultiMotion.DPSB, 20, 1);  //셔틀2 클로즈    
                        }
                        else if (ClassType.Restart == true) //90도작업
                        {
                            ClassType.JobData = System.IO.File.ReadAllLines(ClassType._startUPPath + ClassType.DateDay + ClassType._startUPPath2 + ClassType.JobDate, System.Text.Encoding.Default);
                            SmallClass.SetPin(MultiMotion.DPSB, 8, 0);  //리프트 업  
                            ClassType.Run_Flag = true;
                            ClassType.JobResult = ClassType.JobRotation90;
                            ClassType.JobResultState = ClassType.WORK_START;
                            TimerMain.Enabled = true;
                            SmallClass.SetPin(MultiMotion.DPSB, 17, 0);  //셔틀1 오픈
                            SmallClass.SetPin(MultiMotion.DPSB, 21, 0);  //셔틀2 오픈      
                            SmallClass.SetPin(MultiMotion.DPSB, 16, 1);  //셔틀1 클로즈
                            SmallClass.SetPin(MultiMotion.DPSB, 20, 1);  //셔틀2 클로즈    
                        }
                    }
                }
                else
                {
                    if ((Math.Abs(ClassType.sineps45) <= ClassType.eps))
                    {
                        if (ClassType.Restart == false)  //0작업
                        {
                            ClassType.JobData = System.IO.File.ReadAllLines(ClassType._startUPPath + ClassType.DateDay + ClassType._startUPPath2 + ClassType.JobDate, System.Text.Encoding.Default);
                            SmallClass.SetPin(MultiMotion.DPSB, 8, 0);  //리프트 업  
                            ClassType.Run_Flag = true;
                            ClassType.JobResult = ClassType.JobRotation0;
                            ClassType.JobResultState = ClassType.WORK_START;
                            TimerMain.Enabled = true;
                            SmallClass.SetPin(MultiMotion.DPSB, 17, 0);  //셔틀1 오픈
                            SmallClass.SetPin(MultiMotion.DPSB, 21, 0);  //셔틀2 오픈      
                            SmallClass.SetPin(MultiMotion.DPSB, 16, 1);  //셔틀1 클로즈
                            SmallClass.SetPin(MultiMotion.DPSB, 20, 1);  //셔틀2 클로즈    
                        }
                        else if (ClassType.Restart == true) //90도작업
                        {
                            ClassType.JobData = System.IO.File.ReadAllLines(ClassType._startUPPath + ClassType.DateDay + ClassType._startUPPath2 + ClassType.JobDate, System.Text.Encoding.Default);
                            SmallClass.SetPin(MultiMotion.DPSB, 8, 0);  //리프트 업  
                            ClassType.Run_Flag = true;
                            ClassType.JobResult = ClassType.JobRotation90;
                            ClassType.JobResultState = ClassType.WORK_START;
                            TimerMain.Enabled = true;
                            SmallClass.SetPin(MultiMotion.DPSB, 17, 0);  //셔틀1 오픈
                            SmallClass.SetPin(MultiMotion.DPSB, 21, 0);  //셔틀2 오픈      
                            SmallClass.SetPin(MultiMotion.DPSB, 16, 1);  //셔틀1 클로즈
                            SmallClass.SetPin(MultiMotion.DPSB, 20, 1);  //셔틀2 클로즈    
                        }
                    }
                }


            }
        }

        private void btnRecoStop_Click(object sender, EventArgs e)
        {
            if (ClassType.Run_Flag == true)
            {
                if (MessageBox.Show("검사를 종료하시겠습니까?", "DP검사종료여부", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    btnRecoStart.Image = Properties.Resources.시작회색;
                    btnRecoStop.Image = Properties.Resources.정지녹색;
                    ClassType.Run_Flag = false;
                    ClassType.RestartCheck = true;
                    TimerSub5.Enabled = false;

                }
            }
            else
            {
                MessageBox.Show("검사를 시작하지 않았습니다.");
            }
        }

        private void GuideStop_Click(object sender, EventArgs e) //가이드 스톱
        {

            if (ClassType.Run_Flag1 == true)
            {
                if (MessageBox.Show("검사를 종료하시겠습니까?", "DP검사종료여부", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    ClassType.Run_Flag1 = false;
                    btnRecoStart.Image = Properties.Resources.시작회색;
                    btnRecoStop.Image = Properties.Resources.정지녹색;
                }
            }
            else
            {
                MessageBox.Show("검사를 시작하지 않았습니다.");
            }
        }

        private void EXIT_Click(object sender, EventArgs e)
        {
            updateplus();
            if (DialogResult.Yes == MessageBox.Show("정말 종료하시겠습니까?", "프로그램 종료", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                Application.Exit();
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            else
            {
            }
        }

        private void btnEditor_Click(object sender, EventArgs e)
        {
            try
            {
                if (ClassType.AutoManual == false)
                {
                    if (ClassType.Run_Flag == true)
                    {
                        MessageBox.Show(ClassType.TestWaitMsg);
                        return;
                    }
                    calibration();
                }
                else
                {
                    MessageBox.Show("MANUAL 모드를 선택해주세요");
                }

                if (ClassType.AutoManual == true)
                {
                    MessageBox.Show(ClassType.TestWaitMsg4);
                    return;
                };
            }
            catch (Exception ex)
            {
            }
        }

        private void MANUAL_Click(object sender, EventArgs e)
        {
            //Paix_NMF_Controler.SetPin(MultiMotion.DPSB, 16, 0);  //셔틀1 클로즈
            //Paix_NMF_Controler.SetPin(MultiMotion.DPSB, 17, 1);  //셔틀1 오픈
            //Paix_NMF_Controler.SetPin(MultiMotion.DPSB, 20, 0);  //셔플2 클로즈
            //Paix_NMF_Controler.SetPin(MultiMotion.DPSB, 21, 1);  //셔틀2 오픈
            try
            {
                if (ClassType.Run_Flag == true)
                {
                    MessageBox.Show(ClassType.TestWaitMsg);
                    return;
                }
                if (ClassType.Run_Flag1 == true)
                {
                    MessageBox.Show(ClassType.TestWaitMsg);
                    return;
                };
                TimerSub4.Enabled = false;
                TimerSub5.Enabled = false;
                ClassType.AutoManual = false;
                SmallClass.SetPin(MultiMotion.DPSB, 28, 0);  //레드 램프
                SmallClass.SetPin(MultiMotion.DPSB, 29, 1);  //황색 램프
                SmallClass.SetPin(MultiMotion.DPSB, 30, 0);  //녹색 램프
                SmallClass.SetPin(MultiMotion.DPSB, 31, 0);  //알람 부저    

                if (ClassType.AutoManual == false)
                {
                    btnRecoStart.Image = Properties.Resources.시작회색;
                    btnRecoStop.Image = Properties.Resources.정지회색;
                    AUTO.Image = Properties.Resources.자동회색;
                    MANUAL.Image = Properties.Resources.수동녹색;
                }
                else
                {
                    AUTO.Image = Properties.Resources.자동녹색;
                    btnRecoStop.Image = Properties.Resources.정지녹색;
                    MANUAL.Image = Properties.Resources.수동회색;
                }

                ClassType.AirUp = ClassType.AirUp1;// //에어 업
                ClassType.AirUpState = ClassType.AirUpWORK_START;
                TimerSub2.Enabled = true;

            }
            catch (Exception ex)
            {
            }
        }

        private void AUTO_Click(object sender, EventArgs e)
        {
            if (ClassType.Run_Flag == true)
            {
                MessageBox.Show(ClassType.TestWaitMsg);
                return;
            }
            if (ClassType.Run_Flag1 == true)
            {
                MessageBox.Show(ClassType.TestWaitMsg2);
                return;
            }

            TimerSub4.Enabled = true;
            TimerSub2.Enabled = false;
            ClassType.AutoManual = true;
            if (ClassType.AutoManual == true)
            {
                AUTO.Image = Properties.Resources.자동녹색;
                btnRecoStop.Image = Properties.Resources.정지녹색;
                MANUAL.Image = Properties.Resources.수동회색;
            }
            else
            {
                AUTO.Image = Properties.Resources.자동회색;
                MANUAL.Image = Properties.Resources.수동녹색;
            }

        }


        private void GuideIn_Click(object sender, EventArgs e)  //가이드 인
        {
            if (ClassType.Run_Flag1 == true)
            {
                MessageBox.Show(ClassType.TestWaitMsg3);
                return;
            }
            if (ClassType.AutoManual == true)
            {
                MessageBox.Show(ClassType.TestWaitMsg4);
                return;
            }
            ClassType.Counting = 0;
            ClassType.JobGuide = ClassType.JobGuideInStart;
            ClassType.JobGuideState = ClassType.GuideWORK_START;
            ClassType.ShuttleSensor = false; //셔틀 하프센서 감지여부
            ClassType.RestartCheck = false;
            TimerSub2.Enabled = true;
            GuidMove();

        }

        private void GuideOut_Click(object sender, EventArgs e) //가이드 아웃
        {
            if (ClassType.Run_Flag1 == true)
            {
                MessageBox.Show(ClassType.TestWaitMsg);
                return;
            }
            if (ClassType.AutoManual == true)
            {
                MessageBox.Show(ClassType.TestWaitMsg4);
                return;
            }
            ClassType.Counting = 0;
            ClassType.JobGuide = ClassType.JobGuideOutStart;
            ClassType.JobGuideState = ClassType.GuideWORK_START;
            ClassType.ShuttleSensor = false; //셔틀 하프센서 감지여부
            ClassType.RestartCheck = false;
            TimerSub2.Enabled = true;
            GuidMove();
        }

        private void GuideMove_Click(object sender, EventArgs e) //가이드 단계이동
        {
            if (ClassType.Run_Flag1 == true)
            {
                MessageBox.Show(ClassType.TestWaitMsg);
                return;
            };
            if (ClassType.AutoManual == true)
            {
                MessageBox.Show(ClassType.TestWaitMsg4);
                return;
            }
            ClassType.Run_Flag1 = true;
            ClassType.Counting = 0;
            ClassType.JobGuide = ClassType.GuideStepStart;
            ClassType.JobGuideState = ClassType.GuideWORK_START;
            ClassType.ShuttleSensor = false; //셔틀 하프센서 감지여부
            ClassType.RestartCheck = false;
            TimerSub2.Enabled = true;
            GuidMove();
        }
        private void GuideReady_Click(object sender, EventArgs e) //가이드 대기 위치
        {
            if (ClassType.Run_Flag1 == true)
            {
                MessageBox.Show(ClassType.TestWaitMsg);
                return;
            };
            if (ClassType.AutoManual == true)
            {
                MessageBox.Show(ClassType.TestWaitMsg4);
                return;
            };
            ClassType.Run_Flag1 = true;
            ClassType.JobGuide = ClassType.GuideReadyStart;
            ClassType.JobGuideState = ClassType.GuideWORK_START;
            TimerSub2.Enabled = true;
            ClassType.ShuttleSensor = false; //셔틀 하프센서 감지여부
            ClassType.RestartCheck = false;
            ClassType.Counting = 0;
            GuidMove();
        }

        private void LiftFrontIn_Click(object sender, EventArgs e) //리프트1번 원점
        {
            if (ClassType.AutoManual == false)
            {
                MultiMotion.MoveAxis(MultiMotion.Lift1Motor, ClassType.sinmove0, false);
            }
        }

        private void LiftFrontStepMove_Click(object sender, EventArgs e) //리프트1번 스텝무브
        {
            if (ClassType.AutoManual == false)
            {

            }
        }

        private void LiftRearIn_Click(object sender, EventArgs e) //리프트2번 원점
        {
            if (ClassType.AutoManual == false)
            {
                MultiMotion.MoveAxis(MultiMotion.Lift2Motor, ClassType.sinmove0, false);
            }

        }

        private void LiftRearStepMove_Click(object sender, EventArgs e) //리프트2번 스텝무브
        {
            if (ClassType.AutoManual == false)
            {

            }
        }

        private void LiftFrontStepMove_MouseDown(object sender, MouseEventArgs e) //리프트1번 조그 다운 이동
        {
            if (ClassType.AutoManual == false)
            {
                MultiMotion.JogMove(MultiMotion.Lift1Motor, ClassType.sinmoves1);
            }
        }

        private void LiftFrontStepMove_MouseUp(object sender, MouseEventArgs e) //리프트1번 조그 다운 이동금지
        {
            if (ClassType.AutoManual == false)
            {
                MultiMotion.JogStop(MultiMotion.Lift1Motor);
            }
        }

        private void LiftRearStepMove_MouseDown(object sender, MouseEventArgs e) //리프트2번 조그 다운 이동
        {
            if (ClassType.AutoManual == false)
            {
                MultiMotion.JogMove(MultiMotion.Lift2Motor, ClassType.sinmoves1);
            }
        }

        private void LiftRearStepMove_MouseUp(object sender, MouseEventArgs e) //리프트2번 조그 다운 이동금지
        {
            if (ClassType.AutoManual == false)
            {
                MultiMotion.JogStop(MultiMotion.Lift2Motor);
            }
        }

        private void LiftFrontMoveUp_MouseDown(object sender, MouseEventArgs e) //리프트1번 조그 업 이동
        {
            if (ClassType.AutoManual == false)
            {
                MultiMotion.JogMove(MultiMotion.Lift1Motor, ClassType.sinmoves0);
            }
        }

        private void LiftFrontMoveUp_MouseUp(object sender, MouseEventArgs e) //리프트1번 조그 업 이동금지
        {
            if (ClassType.AutoManual == false)
            {
                MultiMotion.JogStop(MultiMotion.Lift1Motor);
            }
        }

        private void LiftRearMoveUp_MouseDown(object sender, MouseEventArgs e) //리프트2번 조그 업 이동
        {
            if (ClassType.AutoManual == false)
            {
                MultiMotion.JogMove(MultiMotion.Lift2Motor, ClassType.sinmoves0);
            }
        }

        private void LiftRearMoveUp_MouseUp(object sender, MouseEventArgs e) //리프트2번 조그 업 이동금지
        {
            if (ClassType.AutoManual == false)
            {
                MultiMotion.JogStop(MultiMotion.Lift2Motor);
            }
        }

        private void Counting_Click(object sender, EventArgs e) //카운팅 확인
        {
            if (ClassType.AutoManual == false)
            {
                if (MessageBox.Show("선택한 값으로 변경하시겠습니까?", "값 변경",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (ClassType.CountingCheck % 2 == 0)
                    {
                        ClassType.CountingCheck = Int32.Parse(Countintxt.Text);
                    }
                    else
                    {
                        MessageBox.Show("짝수 값만 입력가능합니다");
                    }
                }
            }
            else
            {
                MessageBox.Show("MANUAL 모드에서만 변경 가능합니다");
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)  //체크박스 10개
        {
            checkBox2.Checked = false;
            checkBox3.Checked = false;
            ClassType.CountingCheck = 10;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e) //체크박스 20개
        {
            checkBox1.Checked = false;
            checkBox3.Checked = false;
            ClassType.CountingCheck = 20;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e) //체크박스 30개
        {
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            ClassType.CountingCheck = 30;
        }

        private void LiftAStep_Click(object sender, EventArgs e)//리프트1 스텝 이동
        {
            if (ClassType.AutoManual == true)
            {
                MessageBox.Show(ClassType.TestWaitMsg4);
                return;
            }
            if (ClassType.Run_Flag1 == true)
            {
                MessageBox.Show(ClassType.TestWaitMsg);
                return;
            };
            if (ClassType.AutoManual == false)
            {
                MultiMotion.MoveAxis(MultiMotion.Lift1Motor, MotionValueResult.LiftMotorLeft + 50, false, MultiMotion.KSM_SPEED_20);
            }
            if (ClassType.AutoManual == false)
            {
                MultiMotion.MoveAxis(MultiMotion.Lift2Motor, MotionValueResult.LiftMotorRight + 50, false, MultiMotion.KSM_SPEED_20);
            }
        }

        private void LiftBStep_Click(object sender, EventArgs e)//리프트1 스텝 이동
        {
            if (ClassType.AutoManual == true)
            {
                MessageBox.Show(ClassType.TestWaitMsg4);
                return;
            }
            if (ClassType.AutoManual == false)
            {
                MultiMotion.MoveAxis(MultiMotion.Lift1Motor, MotionValueResult.LiftMotorLeft - 50, false, MultiMotion.KSM_SPEED_20);
            }
            if (ClassType.AutoManual == false)
            {
                MultiMotion.MoveAxis(MultiMotion.Lift2Motor, MotionValueResult.LiftMotorRight - 50, false, MultiMotion.KSM_SPEED_20);
            }
        }

        private void CameraSetingBtn_Click(object sender, EventArgs e)
        {
            if (ClassType.Run_Flag1 == true)
            {
                MessageBox.Show(ClassType.TestWaitMsg);
                return;
            };
            if (ClassType.AutoManual == true)
            {
                MessageBox.Show(ClassType.TestWaitMsg4);
                return;
            };
            contextMenuStrip2.Show(this, Control.MousePosition);
        }

        private void 메인화면ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cam2PictureBoxResult.Visible = false;
            Cam1PictureBoxResult.Visible = false;
            Cam1PictureBox.Visible = true;
            Cam2PictureBox.Visible = true;
            Cam1PictureBoxResultOKNG.Visible = true;
            Cam2PictureBoxResultOKNG.Visible = true;

        }

        private void 카메라FD결과ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cam1PictureBox.Visible = false;
            Cam2PictureBox.Visible = false;
            Cam1PictureBoxResultOKNG.Visible = false;
            Cam2PictureBoxResultOKNG.Visible = false;
            Cam2PictureBoxResult.Visible = false;
            Cam1PictureBoxResult.Visible = true;
        }

        private void 카메라MD결과ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cam1PictureBox.Visible = false;
            Cam2PictureBox.Visible = false;
            Cam1PictureBoxResultOKNG.Visible = false;
            Cam2PictureBoxResultOKNG.Visible = false;
            Cam1PictureBoxResult.Visible = false;
            Cam2PictureBoxResult.Visible = true;
        }

        private void 카메라A테스트ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (ClassType.AutoManual == true)
                {
                    MessageBox.Show(ClassType.TestWaitMsg4);
                    return;
                };
                ClassType.CameraMode = false;
                a = 1;
            }
            catch (Exception ex)
            {
            }
        }

        private void 카메라B테스트ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (ClassType.AutoManual == true)
                {
                    MessageBox.Show(ClassType.TestWaitMsg4);
                    return;
                };
                ClassType.CameraMode = false;
                b = 1;
            }
            catch (Exception ex)
            {
            }
        }

        private void 카메라A초점ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ClassType.Run_Flag1 == true)
            {
                MessageBox.Show(ClassType.TestWaitMsg);
                return;
            };
            if (ClassType.AutoManual == true)
            {
                MessageBox.Show(ClassType.TestWaitMsg4);
                return;
            };
            ClassType.JobGuide = ClassType.CameraAStart;
            ClassType.JobGuideState = ClassType.GuideWORK_START;
            ClassType.Run_Flag1 = true;
            TimerSub2.Enabled = true;
            GuidMove();
        }

        private void 카메라B초점ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ClassType.Run_Flag1 == true)
            {
                MessageBox.Show(ClassType.TestWaitMsg);
                return;
            };
            if (ClassType.AutoManual == true)
            {
                MessageBox.Show(ClassType.TestWaitMsg4);
                return;
            };
            ClassType.JobGuide = ClassType.CameraBStart;
            ClassType.JobGuideState = ClassType.GuideWORK_START;
            ClassType.Run_Flag1 = true;
            TimerSub2.Enabled = true;
            GuidMove();
        }

        private void 카운트초기화ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ClassType.Run_Flag == true)
            {
                MessageBox.Show(ClassType.TestWaitMsg);
                return;
            }
            if (DialogResult.Yes == MessageBox.Show("정말 초기화하시겠습니까?", "카운트 초기화", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                ClassType.Totalok = 0;
                ClassType.Totalng = 0;
                ClassType.Total = 0;

                label5.Text = ClassType.Totalok.ToString();
                label6.Text = ClassType.Totalng.ToString();
                label4.Text = (ClassType.Totalok + ClassType.Totalng).ToString();

                datetime = DateTime.Now.ToString("yyyy-MM-dd");
            }

        }

        private void 생산시간초기화ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ClassType.Run_Flag == true)
            {
                MessageBox.Show(ClassType.TestWaitMsg);
                return;
            }
            if (DialogResult.Yes == MessageBox.Show("정말 초기화하시겠습니까?", "카운트 초기화", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                ClassType.MakeStartTime1 = 0;
            }
        }

        private void 작업결과기록ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ClassType.Run_Flag == true)
            {
                MessageBox.Show(ClassType.TestWaitMsg);
                return;
            }
            // ClassType.DateTimeWhite("시간:" + ClassType.DateSec + "    " + "모델명:" + txtModelName.Text + "    " + "제품 합계:" + label5.Text + "개" + "    " + "총제품생산시간:" + LBLMakeStart.Text + "    " + "제품생산시간:" + LBLMakeOut.Text, ClassType.JobResultstate);
            BTB_Db BTB_Db = new BTB_Db();

            BTB_Db.ShowDialog();

        }

        private void button11_Click(object sender, EventArgs e) //클램프1
        {
            SmallClass.SetPin(MultiMotion.DPSB, 16, 0);  //셔틀1 클로즈
            SmallClass.SetPin(MultiMotion.DPSB, 17, 1);  //셔틀1 오픈
            SmallClass.SetPin(MultiMotion.DPSB, 20, 0);  //셔플2 클로즈
            SmallClass.SetPin(MultiMotion.DPSB, 21, 1);  //셔틀2 오픈
        }

        private void button10_Click(object sender, EventArgs e)//클램프2
        {
            SmallClass.SetPin(MultiMotion.DPSB, 17, 0);  //셔틀1 오픈
            SmallClass.SetPin(MultiMotion.DPSB, 16, 1);  //셔틀1 클로즈
            SmallClass.SetPin(MultiMotion.DPSB, 21, 0);  //셔틀2 오픈
            SmallClass.SetPin(MultiMotion.DPSB, 20, 1);  //셔플2 클로즈
        }

        private void button9_Click(object sender, EventArgs e)
        {
            SmallClass.SetPin(MultiMotion.DPSB, 26, 1); //리프트A-B 흡입 시작

        }



        private void OtherBtn_Click(object sender, EventArgs e)
        {
            if (ClassType.Run_Flag1 == true)
            {
                MessageBox.Show(ClassType.TestWaitMsg);
                return;
            };
            if (ClassType.AutoManual == true)
            {
                MessageBox.Show(ClassType.TestWaitMsg4);
                return;
            };
            contextMenuStrip1.Show(this, Control.MousePosition);
        }
        private void contextMenuStrip1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                contextMenuStrip1.Show(this, Control.MousePosition); // Or any other overload
            }
        }

        bool tsm1 = true;

        private void 번흡입ToolStripMenuItem_Click(object sender, EventArgs e)//1번흡입
        {
            //Paix_NMF_Controler.SetPin(MultiMotion.DPSB, 24, 0); //리프트A-B 흡입 시작
            //Paix_NMF_Controler.SetPin(MultiMotion.DPSB, 12, 0); //리프트A-B 흡입 시작
            //Paix_NMF_Controler.SetPin(MultiMotion.DPSB, 25, 1); //리프트C-D 흡입 종료 
            //Paix_NMF_Controler.SetPin(MultiMotion.DPSB, 13, 1); //리프트C-D 흡입 종료 

            //Paix_NMF_Controler.SetPin(MultiMotion.DPSB, 24, 0); //리프트A ON      
            //Paix_NMF_Controler.SetPin(MultiMotion.DPSB, 12, 0); //리프트C ON
            //Paix_NMF_Controler.SetPin(MultiMotion.DPSB, 26, 0); //리프트A ON 흡입      
            //Paix_NMF_Controler.SetPin(MultiMotion.DPSB, 14, 0); //리프트C ON 흡입

            //Paix_NMF_Controler.SetPin(MultiMotion.DPSB, 25, 1); //리프트B OFF
            //Paix_NMF_Controler.SetPin(MultiMotion.DPSB, 13, 1); //리프트D OFF
            //Paix_NMF_Controler.SetPin(MultiMotion.DPSB, 27, 1); //리프트B OFF 흡입
            //Paix_NMF_Controler.SetPin(MultiMotion.DPSB, 15, 1); //리프트D OFF 흡입
            //Paix_NMF_Controler.SetPin(MultiMotion.DPSB, 27, 0); //리프트B OFF 흡입
            //Paix_NMF_Controler.SetPin(MultiMotion.DPSB, 15, 0); //리프트D OFF 흡입

            if (tsm1)
            {
                SmallClass.SetPin(MultiMotion.DPSB, 24, 0); //베큠A off   (0=off)
                SmallClass.SetPin(MultiMotion.DPSB, 12, 0); //베큠C off   (0=off)
                SmallClass.SetPin(MultiMotion.DPSB, 4, 1); //베큠A on     (1=on)
                SmallClass.SetPin(MultiMotion.DPSB, 6, 1); //베큠C on     (1=on)


                SmallClass.SetPin(MultiMotion.DPSB, 5, 0); //베큠B on     (0=off)
                SmallClass.SetPin(MultiMotion.DPSB, 7, 0); //베큠D on     (0=off)
                SmallClass.SetPin(MultiMotion.DPSB, 25, 1); //베큠B off   (1=on)
                SmallClass.SetPin(MultiMotion.DPSB, 13, 1); //베큠D off   (1=on)
                SmallClass.SetPin(MultiMotion.DPSB, 27, 1); //베큠B 퍼지  (1=on)
                SmallClass.SetPin(MultiMotion.DPSB, 15, 1); //베큠D 퍼지  (1=on)
                SmallClass.SetPin(MultiMotion.DPSB, 27, 0); //베큠B 퍼지  (0=off)
                SmallClass.SetPin(MultiMotion.DPSB, 15, 0); //베큠D 퍼지  (0=off)
            }
            else
            {
                SmallClass.SetPin(MultiMotion.DPSB, 4, 0); //베큠A on     (0=off)
                SmallClass.SetPin(MultiMotion.DPSB, 6, 0); //베큠C on     (0=off)
                SmallClass.SetPin(MultiMotion.DPSB, 24, 1); //베큠A off   (1=on)
                SmallClass.SetPin(MultiMotion.DPSB, 12, 1); //베큠C off   (1=on)
                SmallClass.SetPin(MultiMotion.DPSB, 26, 1); //베큠A 퍼지  (1=on)
                SmallClass.SetPin(MultiMotion.DPSB, 14, 1); //베큠C 퍼지  (1=on)
                SmallClass.SetPin(MultiMotion.DPSB, 26, 0); //베큠A 퍼지  (0=off)
                SmallClass.SetPin(MultiMotion.DPSB, 14, 0); //베큠C 퍼지  (0=off)
            }

            tsm1 = !tsm1;
        }

        bool tsm2 = true;

        private void 번흡입ToolStripMenuItem1_Click(object sender, EventArgs e)//2번흡입
        {
            //Paix_NMF_Controler.SetPin(MultiMotion.DPSB, 24, 1); //리프트A-B 흡입 종료 
            //Paix_NMF_Controler.SetPin(MultiMotion.DPSB, 12, 1); //리프트A-B 흡입 종료 
            //Paix_NMF_Controler.SetPin(MultiMotion.DPSB, 25, 0); //리프트C-D 흡입 시작 
            //Paix_NMF_Controler.SetPin(MultiMotion.DPSB, 13, 0); //리프트C-D 흡입 시작 
            //Paix_NMF_Controler.SetPin(MultiMotion.DPSB, 24, 1); //리프트A OFF      
            //Paix_NMF_Controler.SetPin(MultiMotion.DPSB, 12, 1); //리프트C OFF
            //Paix_NMF_Controler.SetPin(MultiMotion.DPSB, 26, 1); //리프트A OFF 흡입      
            //Paix_NMF_Controler.SetPin(MultiMotion.DPSB, 14, 1); //리프트C OFF 흡입

            //Paix_NMF_Controler.SetPin(MultiMotion.DPSB, 25, 0); //리프트B ON
            //Paix_NMF_Controler.SetPin(MultiMotion.DPSB, 13, 0); //리프트D ON
            //Paix_NMF_Controler.SetPin(MultiMotion.DPSB, 27, 0); //리프트B ON 흡입
            //Paix_NMF_Controler.SetPin(MultiMotion.DPSB, 15, 0); //리프트D ON 흡입
            //Paix_NMF_Controler.SetPin(MultiMotion.DPSB, 26, 0); //리프트A OFF 흡입      
            //Paix_NMF_Controler.SetPin(MultiMotion.DPSB, 14, 0); //리프트C OFF 흡입

            if (tsm2)
            {
                SmallClass.SetPin(MultiMotion.DPSB, 25, 0); //베큠B off   (0=off)
                SmallClass.SetPin(MultiMotion.DPSB, 13, 0); //베큠D off   (0=off)
                SmallClass.SetPin(MultiMotion.DPSB, 5, 1); //베큠B on     (1=on)
                SmallClass.SetPin(MultiMotion.DPSB, 7, 1); //베큠D on     (1=on)


                SmallClass.SetPin(MultiMotion.DPSB, 4, 0); //베큠A on     (0=off)
                SmallClass.SetPin(MultiMotion.DPSB, 6, 0); //베큠C on     (0=off)
                SmallClass.SetPin(MultiMotion.DPSB, 24, 1); //베큠A off   (1=on)
                SmallClass.SetPin(MultiMotion.DPSB, 12, 1); //베큠C off   (1=on)
                SmallClass.SetPin(MultiMotion.DPSB, 26, 1); //베큠A 퍼지  (1=on)
                SmallClass.SetPin(MultiMotion.DPSB, 14, 1); //베큠C 퍼지  (1=on)
                SmallClass.SetPin(MultiMotion.DPSB, 26, 0); //베큠A 퍼지  (0=off)
                SmallClass.SetPin(MultiMotion.DPSB, 14, 0); //베큠C 퍼지  (0=off)
            }
            else
            {
                SmallClass.SetPin(MultiMotion.DPSB, 5, 0); //베큠B on     (0=off)
                SmallClass.SetPin(MultiMotion.DPSB, 7, 0); //베큠D on     (0=off)
                SmallClass.SetPin(MultiMotion.DPSB, 25, 1); //베큠B off   (1=on)
                SmallClass.SetPin(MultiMotion.DPSB, 13, 1); //베큠D off   (1=on)
                SmallClass.SetPin(MultiMotion.DPSB, 27, 1); //베큠B 퍼지  (1=on)
                SmallClass.SetPin(MultiMotion.DPSB, 15, 1); //베큠D 퍼지  (1=on)
                SmallClass.SetPin(MultiMotion.DPSB, 27, 0); //베큠B 퍼지  (0=off)
                SmallClass.SetPin(MultiMotion.DPSB, 15, 0); //베큠D 퍼지  (0=off)
            }

            tsm2 = !tsm2;
        }

        private void 업ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SmallClass.SetPin(MultiMotion.DPSB, 8, 0);  //리프트 실린더 업

        }

        private void 다운ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SmallClass.SetPin(MultiMotion.DPSB, 8, 1);  //리프트 실린더 다운

        }

        private void 도ToolStripMenuItem_Click(object sender, EventArgs e) //0도
        {
            if (MultiMotion.InStatus[8] == 0 && MultiMotion.InStatus[10] == 0)
            {
                MessageBox.Show(ClassType.TestWaitMsg9);
                return;
            }
            SmallClass.SetPin(MultiMotion.DPSB, 8, 0);
            if (MultiMotion.InStatus[8] == 1 && MultiMotion.InStatus[10] == 1) //리프트 실린더 센서가 업일때
            {
                MultiMotion.MoveAxis(MultiMotion.RotationMotor, DataManager.sinmove4, false, MultiMotion.KSM_SPEED_10); //회전축을 4도 이동
                ClassType.JobResultState = ClassType.WORK_END;
            }
        }

        private void 도ToolStripMenuItem1_Click(object sender, EventArgs e)//90도
        {
            if (MultiMotion.InStatus[8] == 0 && MultiMotion.InStatus[10] == 0)
            {
                MessageBox.Show(ClassType.TestWaitMsg9);
                return;
            }
            SmallClass.SetPin(MultiMotion.DPSB, 8, 0);
            if (MultiMotion.InStatus[8] == 1 && MultiMotion.InStatus[10] == 1) //리프트 실린더 센서가 업일때
            {
                MultiMotion.MoveAxis(MultiMotion.RotationMotor, DataManager.sinmove94, false, MultiMotion.KSM_SPEED_10); //회전축을 94도 이동
                ClassType.JobResultState = ClassType.WORK_END;
            }
        }

        private void contextMenuStrip2_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                contextMenuStrip2.Show(this, Control.MousePosition); // Or any other overload
            }

        }

        #endregion 버튼 이벤트..

        #region 속성 이벤트 ...



        private void Cam2PictureBoxResult_Paint(object sender, PaintEventArgs e)
        {
            Redraw2(e.Graphics);
        }

        private void Cam1PictureBoxResult_Paint(object sender, PaintEventArgs e)
        {
            Redraw1(e.Graphics);
        }


        private void Countintxt_KeyUp(object sender, KeyEventArgs e) //카운팅 키업
        {
            try
            {
                Encoding enc = Encoding.GetEncoding(0);      //문자 입력 제한
                                                             // ClassType.CountingCheck = enc.GetByteCount(Countintxt.Text);  //숫자 카운트
                if ((ClassType.CountingCheck - 1) % 2 == 0)
                {
                    Countintxt.SelectionStart = Countintxt.Text.Length;   //마지막줄 유지
                }
                else
                {
                    e.SuppressKeyPress = true;
                    Countintxt.SelectionStart = Countintxt.Text.Length;                          //마지막줄 유지
                }
            }
            catch (Exception ex)
            {
            }
        }

        #endregion 속성 이벤트..

        #region 함수
        public void GuidMove()
        {
            TimerSub2.Enabled = false;
            if (ClassType.Run_Flag == true)
            {
                MessageBox.Show("메뉴얼 모드입니다");
                return;
            }
            if (ClassType.AutoManual == false)
            {

                if (ClassType.JobGuide == ClassType.JobGuideInStart) // //가이드 인 
                {
                    if (ClassType.JobGuideState == ClassType.GuideWORK_START)
                    {
                        ClassType.Run_Flag1 = true;
                        SmallClass.SetPin(MultiMotion.DPSB, 16, 0);  //셔틀1 클로즈
                        SmallClass.SetPin(MultiMotion.DPSB, 20, 0);  //셔틀2 클로즈  
                        SmallClass.SetPin(MultiMotion.DPSB, 17, 1);  //셔틀1 오픈
                        SmallClass.SetPin(MultiMotion.DPSB, 21, 1);  //셔틀2 오픈 
                                                                            //20170612 수정    if ( MultiMotion.InStatus[17] == 0 && MultiMotion.InStatus[19] == 0  && MultiMotion.InStatus[21] == 0 && MultiMotion.InStatus[23] == 0 )
                        if (MultiMotion.InStatus[16] == 0 && MultiMotion.InStatus[17] == 0 && MultiMotion.InStatus[20] == 0 && MultiMotion.InStatus[21] == 0)
                        {
                            MultiMotion.MoveAxis(MultiMotion.Shuttle1Motor, 0, false, MultiMotion.KSM_SPEED_50);
                            MultiMotion.MoveAxis(MultiMotion.Shuttle2Motor, 0, false, MultiMotion.KSM_SPEED_50);
                            ClassType.JobGuideState = ClassType.GuideWORK_WORKING;
                        }
                    }
                    else if (ClassType.JobGuideState == ClassType.GuideWORK_WORKING)
                    {
                        ClassType.JobGuideState = ClassType.GuideWORK_END;
                    }
                    else if (ClassType.JobGuideState == ClassType.GuideWORK_END)
                    {
                        if (Math.Abs(ClassType.guideeps0) <= ClassType.eps)  //가이드가 0도에 도착했을때
                        {
                            MultiMotion.HomeMove(MultiMotion.Shuttle1Motor, true);
                            MultiMotion.HomeMove(MultiMotion.Shuttle2Motor, true);
                            ClassType.JobGuide = ClassType.JobGuideFinal;
                            ClassType.Run_Flag1 = false;
                            TimerSub2.Enabled = false;
                            return;
                        }
                    }
                }

                else if (ClassType.JobGuide == ClassType.JobGuideOutStart) // //가이드 아웃
                {
                    if (ClassType.JobGuideState == ClassType.GuideWORK_START)
                    {
                        ClassType.Run_Flag1 = true;
                        SmallClass.SetPin(MultiMotion.DPSB, 16, 0);  //셔틀1 클로즈
                        SmallClass.SetPin(MultiMotion.DPSB, 20, 0);  //셔틀2 클로즈  
                        SmallClass.SetPin(MultiMotion.DPSB, 17, 1);  //셔틀1 오픈
                        SmallClass.SetPin(MultiMotion.DPSB, 21, 1);  //셔틀2 오픈 

                        if (MultiMotion.InStatus[16] == 0 && MultiMotion.InStatus[17] == 0 && MultiMotion.InStatus[20] == 0 && MultiMotion.InStatus[21] == 0)

                        {
                            MultiMotion.MoveAxis(MultiMotion.Shuttle1Motor, 1410, false, MultiMotion.KSM_SPEED_50);
                            MultiMotion.MoveAxis(MultiMotion.Shuttle2Motor, 1410, false, MultiMotion.KSM_SPEED_50);
                            ClassType.JobGuideState = ClassType.GuideWORK_WORKING;
                        }
                    }
                    else if (ClassType.JobGuideState == ClassType.GuideWORK_WORKING)
                    {
                        ClassType.JobGuideState = ClassType.GuideWORK_END;
                    }
                    else if (ClassType.JobGuideState == ClassType.GuideWORK_END)
                    {
                        if (Math.Abs(MotionValueResult.ShuttleMotorLeft) >= ClassType.sinmoveShuttle1end)  //가이드가 끝점에 도착했을때
                        {
                            ClassType.JobGuide = ClassType.JobGuideFinal;
                            ClassType.Run_Flag1 = false;
                            TimerSub2.Enabled = false;
                            return;
                        }
                    }
                }


                else if (ClassType.JobGuide == ClassType.GuideReadyStart) // //가이드 대기 위치
                {
                    if (ClassType.JobGuideState == ClassType.GuideWORK_START)
                    {
                        ClassType.Run_Flag1 = true;
                        ClassType.JobGuideState = ClassType.GuideWORK_WORKING;
                        SmallClass.SetPin(MultiMotion.DPSB, 16, 0);  //셔틀1 클로즈
                        SmallClass.SetPin(MultiMotion.DPSB, 20, 0);  //셔틀2 클로즈  
                        SmallClass.SetPin(MultiMotion.DPSB, 17, 1);  //셔틀1 오픈
                        SmallClass.SetPin(MultiMotion.DPSB, 21, 1);  //셔틀2 오픈 
                    }
                    else if (ClassType.JobGuideState == ClassType.GuideWORK_WORKING)
                    {
                        if (MultiMotion.InStatus[18] == 1 && MultiMotion.InStatus[19] == 1 && MultiMotion.InStatus[22] == 1 && MultiMotion.InStatus[23] == 1) //셔틀클램프가 오픈일때
                        {
                            ClassType.JobGuideState = ClassType.GuideWORK_WORKING2;
                            MultiMotion.MoveAxis(MultiMotion.Shuttle1Motor, ClassType.guide1, false, MultiMotion.KSM_SPEED_20);
                            MultiMotion.MoveAxis(MultiMotion.Shuttle2Motor, ClassType.guide2, false, MultiMotion.KSM_SPEED_20);
                        }
                    }
                    else if (ClassType.JobGuideState == ClassType.GuideWORK_WORKING2)
                    {
                        if (Math.Abs(ClassType.guideeps1A) <= ClassType.eps && Math.Abs(ClassType.guideeps1B) <= ClassType.eps)
                        {
                            ClassType.JobGuideState = ClassType.GuideWORK_WORKING3;
                            ClassType.ShuttleReady = true; //셔틀 준비완료되면
                        }

                    }
                    else if (ClassType.JobGuideState == ClassType.GuideWORK_WORKING3)//센서하프 감지시 스텝이동으로 점프
                    {
                        if (MultiMotion.InStatus[26] == 0 || MultiMotion.InStatus[28] == 0) //가이드 센서 하프감지시
                        {
                            ClassType.JobGuide = ClassType.GuideStepStart;
                            ClassType.JobGuideState = ClassType.GuideWORK_START;
                        }
                        else
                        {
                            ClassType.JobGuideState = ClassType.GuideWORK_END;
                            SmallClass.SetPin(MultiMotion.DPSB, 17, 0);  //셔틀1 오픈
                            SmallClass.SetPin(MultiMotion.DPSB, 21, 0);  //셔틀2 오픈 
                            SmallClass.SetPin(MultiMotion.DPSB, 16, 1);  //셔틀1 클로즈
                            SmallClass.SetPin(MultiMotion.DPSB, 20, 1);  //셔틀2 클로즈
                        }
                    }
                    else if (ClassType.JobGuideState == ClassType.GuideWORK_END)
                    {
                        ClassType.JobGuide = ClassType.JobGuideFinal;
                        ClassType.Run_Flag1 = false;
                        TimerSub2.Enabled = false;
                        return;
                    }
                }

                else if (ClassType.JobGuide == ClassType.GuideStepStart) // //가이드 스텝이동
                {
                    if (ClassType.JobGuideState == ClassType.GuideWORK_START)
                    {
                        ClassType.Run_Flag1 = true;
                        SmallClass.SetPin(MultiMotion.DPSB, 16, 0);  //셔틀1 클로즈
                        SmallClass.SetPin(MultiMotion.DPSB, 20, 0);  //셔틀2 클로즈  
                        SmallClass.SetPin(MultiMotion.DPSB, 17, 1);  //셔틀1 오픈
                        SmallClass.SetPin(MultiMotion.DPSB, 21, 1);  //셔틀2 오픈 

                        if (MultiMotion.InStatus[18] == 1 && MultiMotion.InStatus[19] == 1 && MultiMotion.InStatus[22] == 1 && MultiMotion.InStatus[23] == 1) //셔틀클램프가 오픈일때

                        {
                            ClassType.sinepsShuttleA1Result = MotionValueResult.ShuttleMotorLeft + ClassType.sinmoveShuttle1;
                            ClassType.sinepsShuttleA2Result = MotionValueResult.ShuttleMotorRight + ClassType.sinmoveShuttle2;
                            MultiMotion.MoveAxis(MultiMotion.Shuttle1Motor, MotionValueResult.ShuttleMotorLeft + ClassType.sinmoveShuttle1, false, MultiMotion.KSM_SPEED_20); //셔틀1이동
                            MultiMotion.MoveAxis(MultiMotion.Shuttle2Motor, MotionValueResult.ShuttleMotorRight + ClassType.sinmoveShuttle2, false, MultiMotion.KSM_SPEED_20); //셔틀2이동

                            ClassType.JobGuideState = ClassType.GuideWORK_WORKING;
                        }
                    }
                    else if (ClassType.JobGuideState == ClassType.GuideWORK_WORKING)
                    {
                        if (MotionValueResult.ShuttleMotorLeft >= ClassType.sinmoveShuttle1end)
                        {
                            ClassType.ShuttleReady = true; //셔틀 준비완료되면
                            ClassType.Run_Flag1 = false;
                            TimerSub2.Enabled = false;
                            //Paix_NMF_Controler.SetPin(MultiMotion.DPSB, 28, 1);  //레드 램프
                            //Paix_NMF_Controler.SetPin(MultiMotion.DPSB, 29, 0);  //녹색 램프
                            //Paix_NMF_Controler.SetPin(MultiMotion.DPSB, 30, 0);  //황색 램프

                            //Paix_NMF_Controler.SetPin(MultiMotion.DPSB, 31, 1);  //알람 부저   
                            //MessageBox.Show("작업이 종료되었습니다");
                            return;
                        }
                        if (Math.Abs(ClassType.sinepsShuttleA1Eps) <= ClassType.eps && Math.Abs(ClassType.sinepsShuttleA2Eps) <= ClassType.eps)
                        {
                            if (MultiMotion.InStatus[26] == 0 || MultiMotion.InStatus[28] == 0) //가이드 센서 하프감지시
                            {
                                ClassType.JobGuideState = ClassType.GuideWORK_START;
                            }
                            else
                            {
                                ClassType.JobGuideState = ClassType.GuideWORK_END;
                            }
                        }
                    }
                    else if (ClassType.JobGuideState == ClassType.GuideWORK_END)
                    {
                        if (Math.Abs(ClassType.sinepsShuttleA1Eps) <= ClassType.eps && Math.Abs(ClassType.sinepsShuttleA2Eps) <= ClassType.eps)
                        {
                            SmallClass.SetPin(MultiMotion.DPSB, 17, 0);  //셔틀1 오픈
                            SmallClass.SetPin(MultiMotion.DPSB, 21, 0);  //셔틀2 오픈 
                            SmallClass.SetPin(MultiMotion.DPSB, 16, 1);  //셔틀1 클로즈
                            SmallClass.SetPin(MultiMotion.DPSB, 20, 1);  //셔틀2 클로즈  
                            ClassType.JobGuide = ClassType.JobGuideFinal;
                            ClassType.ShuttleReady = true; //셔틀 준비완료되면
                            ClassType.Run_Flag1 = false;
                            TimerSub2.Enabled = false;
                            //   return;
                        }
                    }
                }


                else if (ClassType.JobGuide == ClassType.CameraAStart) // //카메라 초점A
                {
                    if (ClassType.JobGuideState == ClassType.GuideWORK_START)
                    {
                        ClassType.Run_Flag1 = true;
                        SmallClass.SetPin(MultiMotion.DPSB, 8, 0);  //리프트 실린더 업
                        ClassType.JobGuideState = ClassType.GuideWORK_WORKING;
                    }
                    else if (ClassType.JobGuideState == ClassType.GuideWORK_WORKING)
                    {
                        if (MultiMotion.InStatus[8] == 1 && MultiMotion.InStatus[10] == 1) //리프트 실린더 센서가 업일때
                        {
                            MultiMotion.MoveAxis(MultiMotion.RotationMotor, DataManager.sinmove4, false, MultiMotion.KSM_SPEED_10); //회전축을 4도 이동
                            ClassType.JobGuideState = ClassType.GuideWORK_WORKING2;
                            SmallClass.SetPin(MultiMotion.DPSB, 24, 0); //리프트A-B 흡입 시작
                            SmallClass.SetPin(MultiMotion.DPSB, 12, 0); //리프트A-B 흡입 시작
                        }
                    }
                    else if (ClassType.JobGuideState == ClassType.GuideWORK_WORKING2)
                    {
                        if (Math.Abs(ClassType.sineps0) <= ClassType.eps)  //절대값 비교 4.5도에 도착했을때
                        {
                            ClassType.JobGuideState = ClassType.GuideWORK_WORKING3;
                            SmallClass.SetPin(MultiMotion.DPSB, 8, 1);  //리프트 실린더 다운
                        }
                    }
                    else if (ClassType.JobGuideState == ClassType.GuideWORK_WORKING3)
                    {
                        ClassType.JobGuideState = ClassType.GuideWORK_END;
                    }
                    else if (ClassType.JobGuideState == ClassType.GuideWORK_END)
                    {
                        SmallClass.SetPin(MultiMotion.DPSB, 8, 0);  //리프트 실린더 업
                        ClassType.JobGuide = ClassType.JobGuideFinal;
                        ClassType.Run_Flag1 = false;
                        TimerSub2.Enabled = false;
                        return;
                    }
                }
                else if (ClassType.JobGuide == ClassType.CameraBStart) // //카메라 초점B
                {
                    if (ClassType.JobGuideState == ClassType.GuideWORK_START)
                    {
                        ClassType.Run_Flag1 = true;
                        SmallClass.SetPin(MultiMotion.DPSB, 8, 0);  //리프트 실린더 업
                        ClassType.JobGuideState = ClassType.GuideWORK_WORKING;

                    }
                    else if (ClassType.JobGuideState == ClassType.GuideWORK_WORKING)
                    {
                        if (MultiMotion.InStatus[8] == 1 && MultiMotion.InStatus[10] == 1) //리프트 실린더 센서가 업일때
                        {
                            MultiMotion.MoveAxis(MultiMotion.RotationMotor, DataManager.sinmove94, false, MultiMotion.KSM_SPEED_10); //회전축을 4도 이동
                            ClassType.JobGuideState = ClassType.GuideWORK_WORKING2;
                            SmallClass.SetPin(MultiMotion.DPSB, 25, 0); //리프트C-D 흡입 시작 
                            SmallClass.SetPin(MultiMotion.DPSB, 13, 0); //리프트C-D 흡입 시작 
                        }
                    }
                    else if (ClassType.JobGuideState == ClassType.GuideWORK_WORKING2)
                    {
                        if (Math.Abs(ClassType.sineps90) <= ClassType.eps)  //절대값 비교 4.5도에 도착했을때
                        {
                            ClassType.JobGuideState = ClassType.GuideWORK_WORKING3;
                            SmallClass.SetPin(MultiMotion.DPSB, 8, 1);  //리프트 실린더 다운
                        }
                    }
                    else if (ClassType.JobGuideState == ClassType.GuideWORK_WORKING3)
                    {
                        ClassType.JobGuideState = ClassType.GuideWORK_END;

                    }
                    else if (ClassType.JobGuideState == ClassType.GuideWORK_END)
                    {
                        SmallClass.SetPin(MultiMotion.DPSB, 8, 0);  //리프트 실린더 업

                        ClassType.JobGuide = ClassType.JobGuideFinal;
                        ClassType.Run_Flag1 = false;
                        TimerSub2.Enabled = false;
                        return;
                    }
                }


            }
            TimerSub2.Enabled = true;
        }

        public void DateTimeCheck()  //날짜 체크
        {
            try
            {
                ClassType.DateDay = DateTime.Now.ToString("yyyy-MM-dd");
                ClassType.DateDayS = DateTime.Now.ToString("yyyyMMdd");
                ClassType.DateHHmm = DateTime.Now.ToString("HHmm");
                ClassType.DateSec = DateTime.Now.ToString("yyyy년 MM월 dd일 HH시 mm분 ss초 ff");
                ClassType.Forder = ClassType._startUPPath + ClassType.DateDay + ClassType._startUPPath2;
                Jobno.Text = ClassType.DateDayS + ClassType.JobData[0];
                //  label2.Text = ClassType.DateHHmm.ToString();
                dir();
                if (ClassType.DateHHmm == "0000")
                {
                    ClassType.JobNoDateWhite("00000");
                }
                if (ClassType.DateDay != ClassType.DirecName)
                {
                    ClassType.JobNoDateWhite("00000");
                }
            }
            catch (Exception ex)
            {
            }
        }
        public void dir() //최하위 디렉토리 불러오기
        {
            DirectoryInfo Info = new DirectoryInfo(@"C:\KSM\DiaDetector\Log");
            if (Info.Exists)
            {
                DirectoryInfo[] CInfo = Info.GetDirectories();
                foreach (DirectoryInfo info in CInfo)
                {
                    ClassType.DirecName = info.Name;
                }
            }
        }
        public void jovno()  //작업번호 하나씩 추가하기
        {
            string aa = ClassType.JobData[0];
            ClassType.JobNoDateWhite((Int16.Parse(aa) + 1).ToString("00000"));
            //    label1.Text = ClassType.JobData[0];
        }

        public void MotionChek() //실시간 모션 값 체크
        {
            MultiMotion.GetCurrentPos();
            MotionValueResult.Cam1Motor = MultiMotion.AxisValue[MultiMotion.Camera1Adjust];
            MotionValueResult.Cam2Motor = MultiMotion.AxisValue[MultiMotion.Camera2Adjust];
            MotionValueResult.LiftMotorLeft = MultiMotion.AxisValue[MultiMotion.Lift1Motor];
            MotionValueResult.LiftMotorRight = MultiMotion.AxisValue[MultiMotion.Lift2Motor];
            MotionValueResult.ShuttleMotorLeft = MultiMotion.AxisValue[MultiMotion.Shuttle1Motor];
            MotionValueResult.ShuttleMotorRight = MultiMotion.AxisValue[MultiMotion.Shuttle2Motor];
            MotionValueResult.RotationMotor = MultiMotion.AxisValue[MultiMotion.RotationMotor];

            ClassType.sineps0 = (MotionValueResult.RotationMotor - DataManager.sinmove4);
            ClassType.sineps45 = (MotionValueResult.RotationMotor - DataManager.sinmove49);
            ClassType.sinepsstan = (MotionValueResult.RotationMotor - ClassType.standby);
            ClassType.sineps90 = (MotionValueResult.RotationMotor - DataManager.sinmove94);
            ClassType.guideeps0 = (MotionValueResult.ShuttleMotorLeft - ClassType.guide0);
            ClassType.guideeps100 = (MotionValueResult.ShuttleMotorLeft - ClassType.sinmoveShuttle1end);
            ClassType.guideeps200 = (MotionValueResult.ShuttleMotorRight - ClassType.sinmoveShuttle2end);
            ClassType.guideeps1A = (MotionValueResult.ShuttleMotorLeft - ClassType.guide1);
            ClassType.guideeps1B = (MotionValueResult.ShuttleMotorRight - ClassType.guide2);
            ClassType.sinepsShuttleA1Eps = (MotionValueResult.ShuttleMotorLeft - ClassType.sinepsShuttleA1Result);
            ClassType.sinepsShuttleA2Eps = (MotionValueResult.ShuttleMotorRight - ClassType.sinepsShuttleA2Result);
            ClassType.sinepsShuttleA1Limitvalue = (MotionValueResult.ShuttleMotorLeft - ClassType.sinepsShuttleA1LimitEps);
            ClassType.sinepsShuttleA2Limitvalue = (MotionValueResult.ShuttleMotorRight - ClassType.sinepsShuttleA2LimitEps);
            ClassType.Pie = DataManager.SelectedModel.ndblOutDiameter;
        }
        public void ServoStopInfoCheck()  //서보 정지원인 체크
        {
            for (int i = 0; i < 8; i++)
            {
                if (MultiMotion.ServoStopInfo[i] != 0 && MultiMotion.ServoStopInfo[i] != 2 && MultiMotion.ServoStopInfo[i] != 3)
                {
                    switch (MultiMotion.ServoStopInfo[i])
                    {
                        case 0:
                            ClassType.DateTimeWhite(i + "번 Servo" + "    " + "[" + ClassType.DateSec + "]" + "            에러원인:" + "정상종료", ClassType.Servo);
                            break;
                        case 1:
                            ClassType.DateTimeWhite(i + "번 Servo" + "    " + "[" + ClassType.DateSec + "]" + "            에러원인:" + "EMG ON", ClassType.Servo);
                            break;
                        case 2:
                            ClassType.DateTimeWhite(i + "번 Servo" + "    " + "[" + ClassType.DateSec + "]" + "            에러원인:" + "-Limit On", ClassType.Servo);
                            break;
                        case 3:
                            ClassType.DateTimeWhite(i + "번 Servo" + "    " + "[" + ClassType.DateSec + "]" + "            에러원인:" + "+Limit On", ClassType.Servo);
                            break;
                        case 4:
                            ClassType.DateTimeWhite(i + "번 Servo" + "    " + "[" + ClassType.DateSec + "]" + "            에러원인:" + "Alarm On", ClassType.Servo);
                            break;
                        case 5:
                            ClassType.DateTimeWhite(i + "번 Servo" + "    " + "[" + ClassType.DateSec + "]" + "            에러원인:" + "Near On", ClassType.Servo);
                            break;
                        case 6:
                            ClassType.DateTimeWhite(i + "번 Servo" + "    " + "[" + ClassType.DateSec + "]" + "            에러원인:" + "Encoder Z상 oN", ClassType.Servo);
                            break;
                    }

                }

            }
        }
        public void Accode() //보호코드
        {
            if (MultiMotion.InStatus[9] == 1 || MultiMotion.InStatus[11] == 1)
            {
                ClassType.LiftCheck = true;
            }
            else
            {
                ClassType.LiftCheck = false;
            }

            if (MultiMotion.InStatus[16] == 1 || MultiMotion.InStatus[17] == 1 || MultiMotion.InStatus[20] == 1 || MultiMotion.InStatus[21] == 1)
            {
                ClassType.ShuttleCheck = true;
            }
            else
            {
                ClassType.ShuttleCheck = false;
            }
        }

        public void ModelSelect()  //모델 선택했는지 안했는지 확인하는 함수
        {
            if (frmDlg.MODELCHANGE == 1)
            {

                CameraMove(true);
                frmDlg.MODELCHANGE = 0;
            }
            else
            {

            }
        }

        public void CameraMove(bool ModelSelect)
        {
            MultiMotion.MoveAxis(MultiMotion.Camera1Adjust, DataManager.SelectedModel.Camera1Moving, false, MultiMotion.KSM_SPEED_10); //리프트 정면 이동
            MultiMotion.MoveAxis(MultiMotion.Camera2Adjust, DataManager.SelectedModel.Camera2Moving, false, MultiMotion.KSM_SPEED_10); //리프트 정면 이동

            //카메라 이동
        }

        public void LiftUp()
        {
            TimerSub4.Enabled = false;
            if (ClassType.AutoManual == true)
            {
                if (MultiMotion.InStatus[3] == 0 || MultiMotion.InStatus[4] == 0 || MultiMotion.InStatus[5] == 0 || MultiMotion.InStatus[6] == 0 && ClassType.Run_Flag == true)
                {
                    SmallClass.SetPin(MultiMotion.DPSB, 28, 1);  //레드 램프
                    SmallClass.SetPin(MultiMotion.DPSB, 29, 0);  //녹색 램프
                    SmallClass.SetPin(MultiMotion.DPSB, 30, 0);  //황색 램프
                    SmallClass.SetPin(MultiMotion.DPSB, 31, 1);  //알람 부저   
                    ClassType.Run_Flag = false;
                    MultiMotion.StopAll();
                }

                if (MultiMotion.InStatus[30] == 1)
                {
                    if (MotionValueResult.LiftMotorRight < ClassType.LiftLimit1)
                    {
                        MultiMotion.MoveAxis(MultiMotion.Lift1Motor, MotionValueResult.LiftMotorLeft + ClassType.LiftLimitUp, false, MultiMotion.KSM_SPEED_30); //리프트 정면 이동
                        ClassType.LiftModeLimit1 = false;
                    }
                    if (MotionValueResult.LiftMotorRight >= ClassType.LiftLimit1)
                    {
                        ClassType.LiftModeLimit1 = true;
                    }
                }

                if (MultiMotion.InStatus[31] == 1)
                {
                    if (MotionValueResult.LiftMotorRight < ClassType.LiftLimit2)
                    {
                        MultiMotion.MoveAxis(MultiMotion.Lift2Motor, MotionValueResult.LiftMotorRight + ClassType.LiftLimitUp, false, MultiMotion.KSM_SPEED_30); //리프트 후면 이동
                        ClassType.LiftModeLimit2 = false;
                    }
                    if (MotionValueResult.LiftMotorRight >= ClassType.LiftLimit2)
                    {
                        ClassType.LiftModeLimit2 = true;
                    }
                }
            }

            TimerSub4.Enabled = true;
        }

        public void ShuttleOpen()
        {
            SmallClass.SetPin(MultiMotion.DPSB, 16, 0);  //셔틀1 클로즈
            SmallClass.SetPin(MultiMotion.DPSB, 17, 1);  //셔틀1 오픈
            SmallClass.SetPin(MultiMotion.DPSB, 20, 0);  //셔플2 클로즈
            SmallClass.SetPin(MultiMotion.DPSB, 21, 1);  //셔틀2 오픈
            SmallClass.SetPin(MultiMotion.DPSB, 24, 1);  //바큠 흡입 A-B 오픈
            SmallClass.SetPin(MultiMotion.DPSB, 12, 1);  //바큠 흡입 C-D 오픈
            SmallClass.SetPin(MultiMotion.DPSB, 13, 1);  //바큠 흡입 A-B 오픈
            SmallClass.SetPin(MultiMotion.DPSB, 25, 1);  //바큠 흡입 C-D 오픈
            SmallClass.SetPin(MultiMotion.DPSB, 26, 0);  //바큠 불기 A-B 오픈
            SmallClass.SetPin(MultiMotion.DPSB, 27, 0);  //바큠 불기 C-D 오픈
            SmallClass.SetPin(MultiMotion.DPSB, 14, 0);  //바큠 불기 A-B 오픈
            SmallClass.SetPin(MultiMotion.DPSB, 15, 0);  //바큠 불기 C-D 오픈
        }

        public static void StartTimer()  //초단위 스타트
        {

            mSTimer = DateTime.Now;
        }
        public static string StopTimer() //초단위 엔드
        {
            mETimer = DateTime.Now;
            double gapTime = (mETimer - mSTimer).Ticks / 10000;
            string dispTime = gapTime.ToString("000") + " m/s ";
            return dispTime;
        }
        public void Pictureboxsize()//픽쳐박스 사이즈 조절
        {
            Cam1PictureBoxResult.Location = new Point(Cam1PictureBoxResult.Location.X, Cam1PictureBoxResult.Location.Y - 200);
            Cam1PictureBoxResult.Size = new Size(Cam1PictureBoxResult.Size.Width, Cam1PictureBoxResult.Size.Height + 200);
            Cam2PictureBoxResult.Location = new Point(Cam2PictureBoxResult.Location.X, Cam2PictureBoxResult.Location.Y - 200);
            Cam2PictureBoxResult.Size = new Size(Cam2PictureBoxResult.Size.Width, Cam2PictureBoxResult.Size.Height + 200);
        }

        frmModelList molist = new frmModelList();
        //생산시간 + 종료
        public void Maketime()
        {
            try
            {
                if (ClassType.Run_Flag == true)
                {
  
                    ClassType.TotalTime++;
                    Modif();

                }
                else
                {

                }
            }
            catch (Exception ex)
            {
            }

        }

        #endregion 함수

        #region 작업시퀀스...
        public void sin()
        {
            try
            {
                TimerMain.Enabled = false;
                if (ClassType.Run_Flag == false)
                {
                    return;
                }
                if (ClassType.AutoManual == true)
                {
                    if (ClassType.JobResult == ClassType.JobStart) // //작업시작  
                    {
                        ClassType.CameraMode = true;
                        if (ClassType.JobResultState == ClassType.WORK_START)
                        {
                            ClassType.Counting--;

                            SmallClass.SetPin(MultiMotion.DPSB, 8, 0);  //리프트 실린더 업


                            //Paix_NMF_Controler.SetPin(MultiMotion.DPSB, 26, 0); //리프트A-B 불기 오프
                            //Paix_NMF_Controler.SetPin(MultiMotion.DPSB, 27, 0); //리프트C-D 불기 오프
                            //Paix_NMF_Controler.SetPin(MultiMotion.DPSB, 14, 0); //리프트A-B 불기 오프
                            //Paix_NMF_Controler.SetPin(MultiMotion.DPSB, 15, 0); //리프트C-D 불기 오프
                            ClassType.JobResultState = ClassType.WORK_WORKING;

                        }
                        else if (ClassType.JobResultState == ClassType.WORK_WORKING)
                        {
                            ClassType.JobResultState = ClassType.WORK_END;

                        }
                        else if (ClassType.JobResultState == ClassType.WORK_END)
                        {

                            ClassType.JobResult = ClassType.JobRotation0;
                            ClassType.JobResultState = ClassType.WORK_START;

                        }
                    }
                    else if (ClassType.JobResult == ClassType.JobRotation0) //첫번째 4도 회전축이동
                    {

                        ClassType.CameraMode = true;
                        SmallClass.SetPin(MultiMotion.DPSB, 17, 0);  //셔틀1 오픈
                        SmallClass.SetPin(MultiMotion.DPSB, 21, 0);  //셔틀2 오픈      
                        SmallClass.SetPin(MultiMotion.DPSB, 16, 1);  //셔틀1 클로즈
                        SmallClass.SetPin(MultiMotion.DPSB, 20, 1);  //셔틀2 클로즈  

                        SmallClass.SetPin(MultiMotion.DPSB, 28, 0);  //레드 램프
                        SmallClass.SetPin(MultiMotion.DPSB, 29, 0);  //황색 램프
                        SmallClass.SetPin(MultiMotion.DPSB, 30, 1);  //녹색 램프
                        if (ClassType.JobResultState == ClassType.WORK_START)
                        {
                            if (MotionValueResult.ShuttleMotorLeft >= ClassType.sinmoveShuttle1end)
                            {
                                ClassType.Run_Flag = false;
                                ClassType.RestartCheck = false;
                                ClassType.ShuttleReady = false; //셔틀 준비완료되면
                                SmallClass.SetPin(MultiMotion.DPSB, 28, 1);  //레드 램프
                                SmallClass.SetPin(MultiMotion.DPSB, 29, 0);  //녹색 램프
                                SmallClass.SetPin(MultiMotion.DPSB, 30, 0);  //황색 램프

                                SmallClass.SetPin(MultiMotion.DPSB, 31, 1);  //알람 부저   
                                ClassType.Run_Flag = false;

                                MessageBox.Show("작업이 종료되었습니다");
                                return;
                            }
                            else
                            {
                                ClassType.JobResultState = ClassType.WORK_WORKING3;
                            }
                        }
                        else if (ClassType.JobResultState == ClassType.WORK_WORKING)
                        {
                            ClassType.JobResultState = ClassType.WORK_WORKING2;
                        }
                        else if (ClassType.JobResultState == ClassType.WORK_WORKING2)
                        {
                            ClassType.JobResultState = ClassType.WORK_WORKING3;
                        }

                        else if (ClassType.JobResultState == ClassType.WORK_WORKING3)
                        {


                            if (MultiMotion.InStatus[8] == 1 && MultiMotion.InStatus[10] == 1) //리프트 실린더 센서가 업일때
                            {
                                MultiMotion.MoveAxis(MultiMotion.RotationMotor, DataManager.sinmove4, false, MultiMotion.KSM_SPEED_100); //회전축을 4도 이동
                                ClassType.JobResultState = ClassType.WORK_END;
                            }
                            else
                            {

                            }

                        }
                        else if (ClassType.JobResultState == ClassType.WORK_END)
                        {
                            if (Math.Abs(ClassType.sineps0) <= ClassType.eps)  //절대값 비교 4.5도에 도착했을때
                            {
                                Tacttime.Text = StopTimer().Substring(0, 1) + "." + StopTimer().Substring(1, 1) + "초";
                                StartTimer();
                                ClassType.JobResult = ClassType.JobLiftDown;
                                ClassType.JobResultState = ClassType.WORK_START;

                                //2017.06.14 수정
                                //Paix_NMF_Controler.SetPin(MultiMotion.DPSB, 17, 0);  //셔틀1 오픈
                                //Paix_NMF_Controler.SetPin(MultiMotion.DPSB, 21, 0);  //셔틀2 오픈      
                                //Paix_NMF_Controler.SetPin(MultiMotion.DPSB, 16, 1);  //셔틀1 클로즈
                                //Paix_NMF_Controler.SetPin(MultiMotion.DPSB, 20, 1);  //셔틀2 클로즈  

                            }
                        }
                    }

                    else if (ClassType.JobResult == ClassType.JobLiftDown) //실린더 다운 동시에 흡입
                    {
                        if (ClassType.JobResultState == ClassType.WORK_START)
                        {
                            //  Paix_NMF_Controler.SetPin(MultiMotion.DPSB, 24, 0); //리프트A-B 흡입 시작  
                            //  Paix_NMF_Controler.SetPin(MultiMotion.DPSB, 12, 0); //리프트A-B 흡입 시작  
                            ClassType.JobResultState = ClassType.WORK_END;

                        }
                        else if (ClassType.JobResultState == ClassType.WORK_WORKING)
                        {
                            //  ClassType.JobResultState = ClassType.WORK_END;
                        }
                        else if (ClassType.JobResultState == ClassType.WORK_END) //리프트 실린더 다운
                        {

                            if (ClassType.LiftModeLimit1 == true || ClassType.LiftModeLimit2 == true)
                            {
                                SmallClass.SetPin(MultiMotion.DPSB, 28, 1);  //레드 램프
                                SmallClass.SetPin(MultiMotion.DPSB, 29, 0);  //녹색 램프
                                SmallClass.SetPin(MultiMotion.DPSB, 30, 0);  //황색 램프

                                SmallClass.SetPin(MultiMotion.DPSB, 31, 1);  //알람 부저  
                                ClassType.Run_Flag = false;

                                MessageBox.Show("DP가 부족 합니다. \r\n 작업이 종료 되었습니다");

                                return;
                            }
                            else
                            {
                                SmallClass.SetPin(MultiMotion.DPSB, 8, 1);  //리프트 다운  
                                ClassType.JobResult = ClassType.JobUP1;
                                ClassType.JobResultState = ClassType.WORK_START;
                            }

                        }
                    }

                    else if (ClassType.JobResult == ClassType.JobUP1) //0도 실린더 업
                    {
                        if (ClassType.JobResultState == ClassType.WORK_START)
                        {
                            if (MultiMotion.InStatus[9] == 1 && MultiMotion.InStatus[11] == 1) //리프트 실린더 센서가 다운일때
                            {
                                SmallClass.SetPin(MultiMotion.DPSB, 24, 0); //리프트A ON      
                                SmallClass.SetPin(MultiMotion.DPSB, 12, 0); //리프트C ON
                                SmallClass.SetPin(MultiMotion.DPSB, 26, 0); //리프트A ON 흡입      
                                SmallClass.SetPin(MultiMotion.DPSB, 14, 0); //리프트C ON 흡입

                                SmallClass.SetPin(MultiMotion.DPSB, 25, 1); //리프트B OFF
                                SmallClass.SetPin(MultiMotion.DPSB, 13, 1); //리프트D OFF
                                SmallClass.SetPin(MultiMotion.DPSB, 27, 1); //리프트B OFF 흡입
                                SmallClass.SetPin(MultiMotion.DPSB, 15, 1); //리프트D OFF 흡입

                                ClassType.JobResultState = ClassType.WORK_WORKING2;

                            }
                        }
                        else if (ClassType.JobResultState == ClassType.WORK_WORKING)
                        {

                        }
                        else if (ClassType.JobResultState == ClassType.WORK_WORKING2)
                        {
                            SmallClass.SetPin(MultiMotion.DPSB, 27, 0); //리프트B OFF 흡입
                            SmallClass.SetPin(MultiMotion.DPSB, 15, 0); //리프트D OFF 흡입
                            ClassType.JobResultState = ClassType.WORK_WORKING3;
                        }
                        else if (ClassType.JobResultState == ClassType.WORK_WORKING3)
                        {
                            ClassType.JobResultState = ClassType.WORK_END;
                        }
                        else if (ClassType.JobResultState == ClassType.WORK_END)
                        {
                            if (MultiMotion.InStatus[9] == 1 && MultiMotion.InStatus[11] == 1) //리프트 실린더 센서가 다운일때
                            {

                                SmallClass.SetPin(MultiMotion.DPSB, 8, 0);  //리프트 실린더 업
                                ClassType.Counting++;
                                ClassType.SQLP = true;
                                ClassType.Totalok++;
                                Countinglbl.Text = (ClassType.Counting).ToString("00000");
                                ClassType.JobResult = ClassType.JobLiftUp;
                                ClassType.JobResultState = ClassType.WORK_START;
                                ClassType.Restart = false;
                                //  updateplus();
                            }

                        }
                    }

                    else if (ClassType.JobResult == ClassType.JobLiftUp)  //카메라 A-B 스샷 시작
                    {
                        if (ClassType.JobResultState == ClassType.WORK_START)
                        {
                            if (ClassType.Totalok == Int32.Parse(txtTotal.Text.Trim()))
                            {
                                ClassType.Run_Flag = false;
                                ClassType.RestartCheck = true;
                                MessageBox.Show("작업이 종료되었습니다 풀 : " + ClassType.Totalok);
                                return;
                            }
                            else
                            {
                                ClassType.JobResultState = ClassType.WORK_WORKING;
                            }
                        }
                        else if (ClassType.JobResultState == ClassType.WORK_WORKING)
                        {
                            if (MultiMotion.InStatus[16] == 1 && MultiMotion.InStatus[17] == 1 && MultiMotion.InStatus[20] == 1 && MultiMotion.InStatus[21] == 1) //셔틀클램프가 닫혀있을때
                            {
                                if (ClassType.ShuttleSensor == true)
                                {
                                    if (MultiMotion.InStatus[27] == 0 || MultiMotion.InStatus[29] == 0) //가이드 센서 상위 감지되면 클램프 오프
                                    {
                                        SmallClass.SetPin(MultiMotion.DPSB, 16, 0);  //셔틀1 클로즈
                                        SmallClass.SetPin(MultiMotion.DPSB, 20, 0);  //셔틀2 클로즈  
                                        SmallClass.SetPin(MultiMotion.DPSB, 17, 1);  //셔틀1 오픈
                                        SmallClass.SetPin(MultiMotion.DPSB, 21, 1);  //셔틀2 오픈  
                                        ClassType.JobResult = ClassType.JobShuttle1;
                                        ClassType.JobResultState = ClassType.WORK_START;
                                    }
                                    else
                                    {
                                        //Paix_NMF_Controler.SetPin(MultiMotion.DPSB, 27, 0); //리프트C-D 불기 종료 
                                        //Paix_NMF_Controler.SetPin(MultiMotion.DPSB, 25, 0); //리프트C-D 불기 종료 
                                        //Paix_NMF_Controler.SetPin(MultiMotion.DPSB, 13, 0); //리프트C-D 불기 종료 
                                        //Paix_NMF_Controler.SetPin(MultiMotion.DPSB, 15, 0); //리프트C-D 불기 종료 
                                        ClassType.JobResultState = ClassType.WORK_START;
                                        ClassType.JobResult = ClassType.JobCamera1stResult;
                                    }
                                }

                                else
                                {
                                    //Paix_NMF_Controler.SetPin(MultiMotion.DPSB, 27, 0); //리프트C-D 불기 종료 
                                    //Paix_NMF_Controler.SetPin(MultiMotion.DPSB, 25, 0); //리프트C-D 불기 종료 
                                    //Paix_NMF_Controler.SetPin(MultiMotion.DPSB, 13, 0); //리프트C-D 불기 종료 
                                    //Paix_NMF_Controler.SetPin(MultiMotion.DPSB, 15, 0); //리프트C-D 불기 종료 
                                    ClassType.JobResultState = ClassType.WORK_START;
                                    ClassType.JobResult = ClassType.JobCamera1stResult;
                                }
                            }
                            else if (ClassType.JobResultState == ClassType.WORK_END)
                            {
                                //ClassType.JobResult = ClassType.JobCamera1stResult;
                                //ClassType.JobResultState = ClassType.WORK_START;
                            }
                        }
                    }

                    else if (ClassType.JobResult == ClassType.JobCamera1stResult) ///카메라 A-B 스샷
                    {
                        if (ClassType.JobResultState == ClassType.WORK_START)
                        {
                            //a = 1;//카메라1 A-B 스샷
                            //b = 1; //카메라2 A-B 스샷
                            if (MultiMotion.InStatus[8] == 1 && MultiMotion.InStatus[10] == 1) //리프트 실린더 센서가 업일때
                            {
                                //ClassType.JobResultState = ClassType.WORK_END;
                                ClassType.JobResult = ClassType.JobNGA2;
                                ClassType.JobResultState = ClassType.WORK_START;
                            }
                        }
                        else if (ClassType.JobResultState == ClassType.WORK_WORKING)
                        {
                            //  ClassType.JobResultState = ClassType.WORK_END;
                        }
                        else if (ClassType.JobResultState == ClassType.WORK_WORKING2)
                        {
                            //  ClassType.JobResultState = ClassType.WORK_END;
                        }
                        else if (ClassType.JobResultState == ClassType.WORK_END)
                        {

                        }
                    }

                    else if (ClassType.JobResult == ClassType.JobNGA2) ///카메라 A-B 스샷2
                    {
                        if (ClassType.JobResultState == ClassType.WORK_START)
                        {
                            //a = 1;//카메라1 A-B 스샷
                            //b = 1; //카메라2 A-B 스샷
                            //  ClassType.JobResultState = ClassType.WORK_END;
                            SmallClass.SetPin(MultiMotion.DPSB, 17, 0);  //셔틀1 오픈
                            SmallClass.SetPin(MultiMotion.DPSB, 21, 0);  //셔틀2 오픈      
                            SmallClass.SetPin(MultiMotion.DPSB, 16, 1);  //셔틀1 클로즈
                            SmallClass.SetPin(MultiMotion.DPSB, 20, 1);  //셔틀2 클로즈   
                            ClassType.JobResult = ClassType.JobNGA3;
                            ClassType.JobResultState = ClassType.WORK_START;
                        }
                        else if (ClassType.JobResultState == ClassType.WORK_WORKING)
                        {

                        }
                        else if (ClassType.JobResultState == ClassType.WORK_WORKING2)
                        {

                        }
                        else if (ClassType.JobResultState == ClassType.WORK_END)
                        {
                            ClassType.JobResult = ClassType.JobNGA3;
                            ClassType.JobResultState = ClassType.WORK_START;
                        }
                    }

                    else if (ClassType.JobResult == ClassType.JobNGA3) ///카메라 A-B 스샷3
                    {
                        if (ClassType.JobResultState == ClassType.WORK_START)
                        {
                            ClassType.JobResultState = ClassType.WORK_WORKING;
                        }
                        else if (ClassType.JobResultState == ClassType.WORK_WORKING)
                        {
                            MultiMotion.GetDIOStatus();
                            if (MultiMotion.InStatus[24] == 0 || MultiMotion.InStatusNMF[0] == 0) //공압에 이상시
                            {
                                if (MultiMotion.ServoStopInfo[2] == 3 || MultiMotion.ServoStopInfo[3] == 3)
                                {
                                    SmallClass.SetPin(MultiMotion.DPSB, 28, 1);  //레드 램프
                                    SmallClass.SetPin(MultiMotion.DPSB, 29, 0);  //녹색 램프
                                    SmallClass.SetPin(MultiMotion.DPSB, 30, 0);  //황색 램프

                                    SmallClass.SetPin(MultiMotion.DPSB, 31, 1);  //알람 부저    
                                    ClassType.Run_Flag = false;
                                    MessageBox.Show("작업이 종료되었습니다");
                                    return;
                                }
                                else
                                {
                                    ClassType.Airsensor++;
                                    if (ClassType.Airsensor < 2)
                                    {
                                        ClassType.JobResult = ClassType.JobNG;
                                        ClassType.JobResultState = ClassType.WORK_START;
                                        Cam1PictureBoxResultOKNG.Image = Properties.Resources.불량;
                                        Cam2PictureBoxResultOKNG.Image = Properties.Resources.불량;
                                    }
                                    else
                                    {
                                        SmallClass.SetPin(MultiMotion.DPSB, 28, 1);  //레드 램프
                                        SmallClass.SetPin(MultiMotion.DPSB, 29, 0);  //녹색 램프
                                        SmallClass.SetPin(MultiMotion.DPSB, 30, 0);  //황색 램프

                                        SmallClass.SetPin(MultiMotion.DPSB, 31, 1);  //알람 부저    
                                        ClassType.Run_Flag = false;
                                        MessageBox.Show("압력센서 이상");
                                        return;
                                    }
                                }
                            }
                            else
                            {
                                ClassType.JobResultState = ClassType.WORK_WORKING2;
                            }
                        }
                        else if (ClassType.JobResultState == ClassType.WORK_WORKING2)
                        {
                            ClassType.Airsensor = 0;
                            if (MultiMotion.InStatus[8] == 1 && MultiMotion.InStatus[10] == 1) //리프트 실린더 센서가 업일때
                            {
                                a = 1; //카메라2 A-B 스샷
                                b = 1;//카메라1 A-B 스샷
                                ClassType.JobResultState = ClassType.WORK_WORKING3;
                            }
                        }
                        else if (ClassType.JobResultState == ClassType.WORK_WORKING3)
                        {

                        }
                        else if (ClassType.JobResultState == ClassType.WORK_END)
                        {

                            if (ClassType.Os1State == ClassType.Os1OK && ClassType.Os2State == ClassType.Os2OK) // 둘다 정상제품이면
                            {

                                ClassType.Os1State = ClassType.Os1END;
                                ClassType.Os2State = ClassType.Os2END;
                                jovno();

                                ClassType.DateTimeWhite(ClassType.JobState + ClassType.DateDayS + ClassType.JobData[0] + ClassType.Jump + ClassType.CameraLog1 + ClassType.Jump + ClassType.Lgr + ClassType.DateSec + ClassType.Rgr + ClassType.Jump + ClassType.ResultTest + ClassType.OK + ClassType.Jump + ClassType.CameraValus +
                                ClassType.Jump + maxmin1 + ClassType.Jump + maxmin3 + ClassType.Jump + maxmin5 + ClassType.Jump + maxmin7 + ClassType.Jump + maxmin9 + ClassType.Jump + maxmin11 + ClassType.Jump + maxmin13 + ClassType.Jump + maxmin15 + ClassType.Jump + Final1Max, ClassType.Result);

                                ClassType.DateTimeWhite(ClassType.JobState + ClassType.DateDayS + ClassType.JobData[0] + ClassType.Jump + ClassType.CameraLog2 + ClassType.Jump + ClassType.Lgr + ClassType.DateSec + ClassType.Rgr + ClassType.Jump + ClassType.ResultTest + ClassType.OK + ClassType.Jump + ClassType.CameraValus +
                                ClassType.Jump + maxmin2 + ClassType.Jump + maxmin4 + ClassType.Jump + maxmin6 + ClassType.Jump + maxmin8 + ClassType.Jump + maxmin10 + ClassType.Jump + maxmin12 + ClassType.Jump + maxmin14 + ClassType.Jump + maxmin16 + ClassType.Jump + Final2Max, ClassType.Result);

                                Cam1PictureBoxResultOKNG.Image = Properties.Resources.정상;
                                Cam2PictureBoxResultOKNG.Image = Properties.Resources.정상;
                                res1.Dispose();
                                res2.Dispose();
                                ClassType.JobResult = ClassType.JobRotation90;
                                ClassType.JobResultState = ClassType.WORK_START;

                            }



                            else if (ClassType.Os1State == ClassType.Os1NG || ClassType.Os2State == ClassType.Os2NG) //둘중 하나라도 불량인 경우
                            {
                                jovno();
                                if (ClassType.Os1State == ClassType.Os1NG && ClassType.Os2State == ClassType.Os2NG)
                                {
                                    ClassType.Os1State = ClassType.Os1END;
                                    ClassType.Os2State = ClassType.Os2END;

                                    ClassType.DateTimeWhite(ClassType.JobState + ClassType.DateDayS + ClassType.JobData[0] + ClassType.Jump + ClassType.CameraLog1 + ClassType.Jump + ClassType.Lgr + ClassType.DateSec + ClassType.Rgr + ClassType.Jump + ClassType.ResultTest + ClassType.NG + ClassType.Jump + ClassType.CameraValus +
                                    ClassType.Jump + maxmin1 + ClassType.Jump + maxmin3 + ClassType.Jump + maxmin5 + ClassType.Jump + maxmin7 + ClassType.Jump + maxmin9 + ClassType.Jump + maxmin11 + ClassType.Jump + maxmin13 + ClassType.Jump + maxmin15 + ClassType.Jump + Final1Max, ClassType.Result);

                                    ClassType.DateTimeWhite(ClassType.JobState + ClassType.DateDayS + ClassType.JobData[0] + ClassType.Jump + ClassType.CameraLog2 + ClassType.Jump + ClassType.Lgr + ClassType.DateSec + ClassType.Rgr + ClassType.Jump + ClassType.ResultTest + ClassType.NG + ClassType.Jump + ClassType.CameraValus +
                                    ClassType.Jump + maxmin2 + ClassType.Jump + maxmin4 + ClassType.Jump + maxmin6 + ClassType.Jump + maxmin8 + ClassType.Jump + maxmin10 + ClassType.Jump + maxmin12 + ClassType.Jump + maxmin14 + ClassType.Jump + maxmin16 + ClassType.Jump + Final2Max, ClassType.Result);
                                    Cam1PictureBoxResultOKNG.Image = Properties.Resources.불량;
                                    Cam2PictureBoxResultOKNG.Image = Properties.Resources.불량;
                                    res1.Dispose();
                                    res2.Dispose();
                                    ClassType.JobResult = ClassType.JobNG;
                                    ClassType.JobResultState = ClassType.WORK_START;
                                }
                                else
                                {
                                    if (ClassType.Os1State == ClassType.Os1NG)
                                    {
                                        ClassType.Os1State = ClassType.Os1END;
                                        ClassType.Os2State = ClassType.Os2END;

                                        ClassType.DateTimeWhite(ClassType.JobState + ClassType.DateDayS + ClassType.JobData[0] + ClassType.Jump + ClassType.CameraLog1 + ClassType.Jump + ClassType.Lgr + ClassType.DateSec + ClassType.Rgr + ClassType.Jump + ClassType.ResultTest + ClassType.NG + ClassType.Jump + ClassType.CameraValus +
                                        ClassType.Jump + maxmin1 + ClassType.Jump + maxmin3 + ClassType.Jump + maxmin5 + ClassType.Jump + maxmin7 + ClassType.Jump + maxmin9 + ClassType.Jump + maxmin11 + ClassType.Jump + maxmin13 + ClassType.Jump + maxmin15 + ClassType.Jump + Final1Max, ClassType.Result);

                                        ClassType.DateTimeWhite(ClassType.JobState + ClassType.DateDayS + ClassType.JobData[0] + ClassType.Jump + ClassType.CameraLog2 + ClassType.Jump + ClassType.Lgr + ClassType.DateSec + ClassType.Rgr + ClassType.Jump + ClassType.ResultTest + ClassType.OK + ClassType.Jump + ClassType.CameraValus +
                                        ClassType.Jump + maxmin2 + ClassType.Jump + maxmin4 + ClassType.Jump + maxmin6 + ClassType.Jump + maxmin8 + ClassType.Jump + maxmin10 + ClassType.Jump + maxmin12 + ClassType.Jump + maxmin14 + ClassType.Jump + maxmin16 + ClassType.Jump + Final2Max, ClassType.Result);

                                        res1.Dispose();
                                        Cam1PictureBoxResultOKNG.Image = Properties.Resources.불량;
                                        ClassType.JobResult = ClassType.JobNG;
                                        ClassType.JobResultState = ClassType.WORK_START;
                                    }
                                    else if (ClassType.Os2State == ClassType.Os2NG)
                                    {
                                        ClassType.Os1State = ClassType.Os1END;
                                        ClassType.Os2State = ClassType.Os2END;

                                        ClassType.DateTimeWhite(ClassType.JobState + ClassType.DateDayS + ClassType.JobData[0] + ClassType.Jump + ClassType.CameraLog1 + ClassType.Jump + ClassType.Lgr + ClassType.DateSec + ClassType.Rgr + ClassType.Jump + ClassType.ResultTest + ClassType.OK + ClassType.Jump + ClassType.CameraValus +
                                        ClassType.Jump + maxmin1 + ClassType.Jump + maxmin3 + ClassType.Jump + maxmin5 + ClassType.Jump + maxmin7 + ClassType.Jump + maxmin9 + ClassType.Jump + maxmin11 + ClassType.Jump + maxmin13 + ClassType.Jump + maxmin15 + ClassType.Jump + Final1Max, ClassType.Result);

                                        ClassType.DateTimeWhite(ClassType.JobState + ClassType.DateDayS + ClassType.JobData[0] + ClassType.Jump + ClassType.CameraLog2 + ClassType.Jump + ClassType.Lgr + ClassType.DateSec + ClassType.Rgr + ClassType.Jump + ClassType.ResultTest + ClassType.NG + ClassType.Jump + ClassType.CameraValus +
                                        ClassType.Jump + maxmin2 + ClassType.Jump + maxmin4 + ClassType.Jump + maxmin6 + ClassType.Jump + maxmin8 + ClassType.Jump + maxmin10 + ClassType.Jump + maxmin12 + ClassType.Jump + maxmin14 + ClassType.Jump + maxmin16 + ClassType.Jump + Final2Max, ClassType.Result);
                                        res2.Dispose();
                                        Cam2PictureBoxResultOKNG.Image = Properties.Resources.불량;
                                        ClassType.JobResult = ClassType.JobNG;
                                        ClassType.JobResultState = ClassType.WORK_START;
                                    }
                                }
                            }
                        }
                    }


                    else if (ClassType.JobResult == ClassType.JobRotation90) //회전축을 94도 이동
                    {
                        ClassType.CameraMode = true;
                        if (ClassType.JobResultState == ClassType.WORK_START)
                        {
                            ClassType.JobResultState = ClassType.WORK_WORKING2;
                        }
                        else if (ClassType.JobResultState == ClassType.WORK_WORKING)
                        {
                            ClassType.JobResultState = ClassType.WORK_WORKING2;
                        }
                        else if (ClassType.JobResultState == ClassType.WORK_WORKING2)
                        {

                            if (MultiMotion.InStatus[8] == 1 && MultiMotion.InStatus[10] == 1) //리프트 실린더 센서가 업일때
                            {
                                MultiMotion.MoveAxis(MultiMotion.RotationMotor, DataManager.sinmove94, false, MultiMotion.KSM_SPEED_100); //회전축을 94도 이동
                                ClassType.JobResultState = ClassType.WORK_END;

                            }
                        }
                        else if (ClassType.JobResultState == ClassType.WORK_END)
                        {
                            if (Math.Abs(ClassType.sineps90) <= ClassType.eps)  //절대값 비교 90도에 도착했을때
                            {
                                Tacttime.Text = StopTimer().Substring(0, 1) + "." + StopTimer().Substring(1, 1) + "초";
                                StartTimer();
                                ClassType.JobResult = ClassType.JobLiftDown2;
                                ClassType.JobResultState = ClassType.WORK_START;
                                SmallClass.SetPin(MultiMotion.DPSB, 17, 0);  //셔틀1 오픈
                                SmallClass.SetPin(MultiMotion.DPSB, 21, 0);  //셔틀2 오픈      
                                SmallClass.SetPin(MultiMotion.DPSB, 16, 1);  //셔틀1 클로즈
                                SmallClass.SetPin(MultiMotion.DPSB, 20, 1);  //셔틀2 클로즈    

                            }
                        }
                    }

                    else if (ClassType.JobResult == ClassType.JobLiftDown2) //실린더 다운 동시에 흡입
                    {
                        if (ClassType.JobResultState == ClassType.WORK_START)
                        {
                            //Paix_NMF_Controler.SetPin(MultiMotion.DPSB, 25, 0); //리프트C-D 흡입 시작
                            //Paix_NMF_Controler.SetPin(MultiMotion.DPSB, 13, 0); //리프트C-D 흡입 시작
                            ClassType.JobResultState = ClassType.WORK_END;

                        }
                        else if (ClassType.JobResultState == ClassType.WORK_WORKING)
                        {

                        }
                        else if (ClassType.JobResultState == ClassType.WORK_END)
                        {
                            if (ClassType.LiftModeLimit1 == true || ClassType.LiftModeLimit2 == true)
                            {
                                MessageBox.Show("DP 부족 합니다. \r\n 작업이 종료 되었습니다");
                                SmallClass.SetPin(MultiMotion.DPSB, 28, 1);  //레드 램프
                                SmallClass.SetPin(MultiMotion.DPSB, 29, 0);  //녹색 램프
                                SmallClass.SetPin(MultiMotion.DPSB, 30, 0);  //황색 램프

                                SmallClass.SetPin(MultiMotion.DPSB, 31, 1);  //알람 부저   
                                ClassType.Run_Flag = false;
                                return;
                            }
                            else
                            {
                                SmallClass.SetPin(MultiMotion.DPSB, 8, 1);  //리프트 실린더 다운
                                ClassType.JobResult = ClassType.JobUP2;
                                ClassType.JobResultState = ClassType.WORK_START;
                            }
                        }
                    }
                    else if (ClassType.JobResult == ClassType.JobUP2) //0도 실린더 업
                    {
                        if (ClassType.JobResultState == ClassType.WORK_START)
                        {
                            if (MultiMotion.InStatus[9] == 1 && MultiMotion.InStatus[11] == 1) //리프트 실린더 센서가 다운일때
                            {
                                SmallClass.SetPin(MultiMotion.DPSB, 24, 1); //리프트A OFF      
                                SmallClass.SetPin(MultiMotion.DPSB, 12, 1); //리프트C OFF
                                SmallClass.SetPin(MultiMotion.DPSB, 26, 1); //리프트A OFF 흡입      
                                SmallClass.SetPin(MultiMotion.DPSB, 14, 1); //리프트C OFF 흡입

                                SmallClass.SetPin(MultiMotion.DPSB, 25, 0); //리프트B ON
                                SmallClass.SetPin(MultiMotion.DPSB, 13, 0); //리프트D ON
                                SmallClass.SetPin(MultiMotion.DPSB, 27, 0); //리프트B ON 흡입
                                SmallClass.SetPin(MultiMotion.DPSB, 15, 0); //리프트D ON 흡입
                                ClassType.JobResultState = ClassType.WORK_WORKING2;

                            }
                        }
                        else if (ClassType.JobResultState == ClassType.WORK_WORKING)
                        {

                        }
                        else if (ClassType.JobResultState == ClassType.WORK_WORKING2)
                        {
                            SmallClass.SetPin(MultiMotion.DPSB, 26, 0); //리프트A OFF 흡입      
                            SmallClass.SetPin(MultiMotion.DPSB, 14, 0); //리프트C OFF 흡입
                            ClassType.JobResultState = ClassType.WORK_WORKING3;
                        }
                        else if (ClassType.JobResultState == ClassType.WORK_WORKING3)
                        {
                            ClassType.JobResultState = ClassType.WORK_END;
                        }
                        else if (ClassType.JobResultState == ClassType.WORK_END)
                        {
                            if (MultiMotion.InStatus[9] == 1 && MultiMotion.InStatus[11] == 1) //리프트 실린더 센서가 다운일때
                            {

                                SmallClass.SetPin(MultiMotion.DPSB, 8, 0);  //리프트 실린더 업
                                ClassType.Counting++;
                                ClassType.SQLP = true;
                                ClassType.Totalok++;
                                Countinglbl.Text = (ClassType.Counting).ToString("00000");
                                ClassType.JobResult = ClassType.JobLiftUp2;
                                ClassType.JobResultState = ClassType.WORK_START;
                                ClassType.Restart = true;
                            }

                        }
                    }
                    else if (ClassType.JobResult == ClassType.JobLiftUp2) //90도 실린더 업
                    {
                        if (ClassType.JobResultState == ClassType.WORK_START)
                        {
                            if (ClassType.Totalok == Int32.Parse(txtTotal.Text.Trim()))
                            {
                                ClassType.Run_Flag = false;
                                ClassType.RestartCheck = true;
                                MessageBox.Show("작업이 종료되었습니다 풀 : " + ClassType.Totalok);
                                return;
                            }
                            else
                            {

                                ClassType.Counting1++;
                                if ((ClassType.Counting1 * 2) % ClassType.CountingCheck == 0)
                                {
                                    if ((MultiMotion.InStatus[26] == 1 || MultiMotion.InStatus[28] == 1) && ClassType.ShuttleSensor == false)
                                    {
                                        SmallClass.SetPin(MultiMotion.DPSB, 16, 0);  //셔틀1 클로즈
                                        SmallClass.SetPin(MultiMotion.DPSB, 20, 0);  //셔틀2 클로즈  
                                        SmallClass.SetPin(MultiMotion.DPSB, 17, 1);  //셔틀1 오픈
                                        SmallClass.SetPin(MultiMotion.DPSB, 21, 1);  //셔틀2 오픈
                                        ClassType.JobResultState = ClassType.WORK_WORKING;
                                    }
                                    else
                                    {
                                        ClassType.JobResultState = ClassType.WORK_WORKING;
                                        //  ClassType.ShuttleSensor = true; //하프센서 감지되면
                                    }
                                }
                                else
                                {
                                    if (MultiMotion.InStatus[26] == 0 || MultiMotion.InStatus[28] == 0)
                                    {
                                        ClassType.JobResultState = ClassType.WORK_WORKING;
                                        ClassType.ShuttleSensor = true; //하프센서 감지되면
                                    }
                                    else
                                    {
                                        ClassType.JobResultState = ClassType.WORK_WORKING;
                                    }
                                }
                            }


                        }
                        else if (ClassType.JobResultState == ClassType.WORK_WORKING)
                        {
                            // Paix_NMF_Controler.SetPin(MultiMotion.DPSB, 26, 0); //리프트A-B 불기 쫑료     
                            // Paix_NMF_Controler.SetPin(MultiMotion.DPSB, 14, 0); //리프트A-B 불기 쫑료    
                            ClassType.JobResult = ClassType.JobCamera2stResult;
                            ClassType.JobResultState = ClassType.WORK_START;
                        }
                        else if (ClassType.JobResultState == ClassType.WORK_END)
                        {

                        }
                    }

                    else if (ClassType.JobResult == ClassType.JobCamera2stResult)///카메라 C-D 스샷
                    {
                        if (ClassType.JobResultState == ClassType.WORK_START)
                        {
                            //a = 1;//카메라1 C-D 스샷
                            //b = 1; //카메라2 C-D 스샷
                            if (MultiMotion.InStatus[8] == 1 && MultiMotion.InStatus[10] == 1) //리프트 실린더 센서가 업일때
                            {
                                ClassType.JobResult = ClassType.JobNGb3;
                                ClassType.JobResultState = ClassType.WORK_START;

                            }
                        }
                        else if (ClassType.JobResultState == ClassType.WORK_WORKING)
                        {

                        }
                        else if (ClassType.JobResultState == ClassType.WORK_WORKING2)
                        {

                        }
                        else if (ClassType.JobResultState == ClassType.WORK_END)
                        {

                        }
                    }

                    else if (ClassType.JobResult == ClassType.JobNGb2)///카메라 C-D 스샷2
                    {
                        if (ClassType.JobResultState == ClassType.WORK_START)
                        {

                            /*  2017.06.14 수정
                            Paix_NMF_Controler.SetPin(MultiMotion.DPSB, 17, 0);  //셔틀1 오픈
                            Paix_NMF_Controler.SetPin(MultiMotion.DPSB, 21, 0);  //셔틀2 오픈      
                            Paix_NMF_Controler.SetPin(MultiMotion.DPSB, 16, 1);  //셔틀1 클로즈
                            Paix_NMF_Controler.SetPin(MultiMotion.DPSB, 20, 1);  //셔틀2 클로즈   
                             
                             */
                            //a = 1;//카메라1 C-D 스샷
                            //b = 1; //카메라2 C-D 스샷                    
                            ClassType.JobResult = ClassType.JobNGb3;
                            ClassType.JobResultState = ClassType.WORK_START;
                        }
                        else if (ClassType.JobResultState == ClassType.WORK_WORKING)
                        {

                        }
                        else if (ClassType.JobResultState == ClassType.WORK_WORKING2)
                        {

                        }
                        else if (ClassType.JobResultState == ClassType.WORK_END)
                        {

                        }
                    }
                    else if (ClassType.JobResult == ClassType.JobNGb3)///카메라 C-D 스샷3
                    {
                        if (ClassType.JobResultState == ClassType.WORK_START)
                        {

                            ClassType.JobResultState = ClassType.WORK_WORKING;
                        }
                        else if (ClassType.JobResultState == ClassType.WORK_WORKING)
                        {
                            MultiMotion.GetDIOStatus();
                            if (MultiMotion.InStatus[25] == 0 || MultiMotion.InStatusNMF[1] == 0) //공압에 이상시
                            {
                                if (MultiMotion.ServoStopInfo[2] == 3 || MultiMotion.ServoStopInfo[3] == 3)
                                {
                                    SmallClass.SetPin(MultiMotion.DPSB, 28, 1);  //레드 램프
                                    SmallClass.SetPin(MultiMotion.DPSB, 29, 0);  //녹색 램프
                                    SmallClass.SetPin(MultiMotion.DPSB, 30, 0);  //황색 램프

                                    SmallClass.SetPin(MultiMotion.DPSB, 31, 1);  //알람 부저 
                                    ClassType.Run_Flag = false;
                                    MessageBox.Show("작업이 종료되었습니다");
                                    return;
                                }
                                else
                                {
                                    //압력 센서 관련
                                    ClassType.Airsensor++;
                                    if (ClassType.Airsensor < 2)
                                    {
                                        ClassType.JobResult = ClassType.JobNGSub;
                                        ClassType.JobResultState = ClassType.WORK_START;
                                        Cam1PictureBoxResultOKNG.Image = Properties.Resources.불량;
                                        Cam2PictureBoxResultOKNG.Image = Properties.Resources.불량;
                                    }
                                    else
                                    {
                                        SmallClass.SetPin(MultiMotion.DPSB, 28, 1);  //레드 램프
                                        SmallClass.SetPin(MultiMotion.DPSB, 29, 0);  //녹색 램프
                                        SmallClass.SetPin(MultiMotion.DPSB, 30, 0);  //황색 램프

                                        SmallClass.SetPin(MultiMotion.DPSB, 31, 1);  //알람 부저  
                                        ClassType.Run_Flag = false;
                                        MessageBox.Show("압력센서 이상");
                                        return;
                                    }
                                }
                            }
                            else
                            {
                                ClassType.JobResultState = ClassType.WORK_WORKING2;
                            }
                        }
                        else if (ClassType.JobResultState == ClassType.WORK_WORKING2)
                        {
                            ClassType.Airsensor = 0;
                            if (MultiMotion.InStatus[8] == 1 && MultiMotion.InStatus[10] == 1) //리프트 실린더 센서가 업일때
                            {
                                a = 1; //카메라2 C-D 스샷
                                b = 1;//카메라1 C-D 스샷
                                ClassType.JobResultState = ClassType.WORK_WORKING3;
                            }


                        }
                        else if (ClassType.JobResultState == ClassType.WORK_WORKING3)
                        {
                            // a = 1; //카메라2 C-D 스샷
                            // b = 1;//카메라1 C-D 스샷
                        }
                        else if (ClassType.JobResultState == ClassType.WORK_END)
                        {
                            if (ClassType.Os1State == ClassType.Os1OK && ClassType.Os2State == ClassType.Os2OK) // 둘다 정상제품이면
                            {

                                ClassType.Os1State = ClassType.Os1END;
                                ClassType.Os2State = ClassType.Os2END;
                                jovno();

                                ClassType.JobResultState = ClassType.WORK_START;
                                ClassType.JobResult = ClassType.JobRotation0;

                                ClassType.DateTimeWhite(ClassType.JobState + ClassType.DateDayS + ClassType.JobData[0] + ClassType.Jump + ClassType.CameraLog1 + ClassType.Jump + ClassType.Lgr + ClassType.DateSec + ClassType.Rgr + ClassType.Jump + ClassType.ResultTest + ClassType.OK + ClassType.Jump + ClassType.CameraValus +
                                ClassType.Jump + maxmin1 + ClassType.Jump + maxmin3 + ClassType.Jump + maxmin5 + ClassType.Jump + maxmin7 + ClassType.Jump + maxmin9 + ClassType.Jump + maxmin11 + ClassType.Jump + maxmin13 + ClassType.Jump + maxmin15 + ClassType.Jump + Final1Max, ClassType.Result);

                                ClassType.DateTimeWhite(ClassType.JobState + ClassType.DateDayS + ClassType.JobData[0] + ClassType.Jump + ClassType.CameraLog2 + ClassType.Jump + ClassType.Lgr + ClassType.DateSec + ClassType.Rgr + ClassType.Jump + ClassType.ResultTest + ClassType.OK + ClassType.Jump + ClassType.CameraValus +
                                ClassType.Jump + maxmin2 + ClassType.Jump + maxmin4 + ClassType.Jump + maxmin6 + ClassType.Jump + maxmin8 + ClassType.Jump + maxmin10 + ClassType.Jump + maxmin12 + ClassType.Jump + maxmin14 + ClassType.Jump + maxmin16 + ClassType.Jump + Final2Max, ClassType.Result);

                                Cam1PictureBoxResultOKNG.Image = Properties.Resources.정상;
                                Cam2PictureBoxResultOKNG.Image = Properties.Resources.정상;
                                res1.Dispose();
                                res2.Dispose();
                            }
                            else if (ClassType.Os1State == ClassType.Os1NG || ClassType.Os2State == ClassType.Os2NG) //둘중 하나라도 불량인 경우
                            {
                                jovno();
                                if (ClassType.Os1State == ClassType.Os1NG && ClassType.Os2State == ClassType.Os2NG)
                                {
                                    ClassType.Os1State = ClassType.Os1END;
                                    ClassType.Os2State = ClassType.Os2END;

                                    ClassType.DateTimeWhite(ClassType.JobState + ClassType.DateDayS + ClassType.JobData[0] + ClassType.Jump + ClassType.CameraLog1 + ClassType.Jump + ClassType.Lgr + ClassType.DateSec + ClassType.Rgr + ClassType.Jump + ClassType.ResultTest + ClassType.NG + ClassType.Jump + ClassType.CameraValus +
                                    ClassType.Jump + maxmin1 + ClassType.Jump + maxmin3 + ClassType.Jump + maxmin5 + ClassType.Jump + maxmin7 + ClassType.Jump + maxmin9 + ClassType.Jump + maxmin11 + ClassType.Jump + maxmin13 + ClassType.Jump + maxmin15 + ClassType.Jump + Final1Max, ClassType.Result);

                                    ClassType.DateTimeWhite(ClassType.JobState + ClassType.DateDayS + ClassType.JobData[0] + ClassType.Jump + ClassType.CameraLog2 + ClassType.Jump + ClassType.Lgr + ClassType.DateSec + ClassType.Rgr + ClassType.Jump + ClassType.ResultTest + ClassType.NG + ClassType.Jump + ClassType.CameraValus +
                                    ClassType.Jump + maxmin2 + ClassType.Jump + maxmin4 + ClassType.Jump + maxmin6 + ClassType.Jump + maxmin8 + ClassType.Jump + maxmin10 + ClassType.Jump + maxmin12 + ClassType.Jump + maxmin14 + ClassType.Jump + maxmin16 + ClassType.Jump + Final2Max, ClassType.Result);

                                    Cam1PictureBoxResultOKNG.Image = Properties.Resources.불량;
                                    Cam2PictureBoxResultOKNG.Image = Properties.Resources.불량;
                                    res1.Dispose();
                                    res2.Dispose();
                                    ClassType.JobResult = ClassType.JobNGSub;
                                    ClassType.JobResultState = ClassType.WORK_START;
                                }
                                else
                                {
                                    if (ClassType.Os1State == ClassType.Os1NG)
                                    {
                                        ClassType.Os1State = ClassType.Os1END;
                                        ClassType.Os2State = ClassType.Os2END;

                                        ClassType.DateTimeWhite(ClassType.JobState + ClassType.DateDayS + ClassType.JobData[0] + ClassType.Jump + ClassType.CameraLog1 + ClassType.Jump + ClassType.Lgr + ClassType.DateSec + ClassType.Rgr + ClassType.Jump + ClassType.ResultTest + ClassType.NG + ClassType.Jump + ClassType.CameraValus +
                                        ClassType.Jump + maxmin1 + ClassType.Jump + maxmin3 + ClassType.Jump + maxmin5 + ClassType.Jump + maxmin7 + ClassType.Jump + maxmin9 + ClassType.Jump + maxmin11 + ClassType.Jump + maxmin13 + ClassType.Jump + maxmin15 + ClassType.Jump + Final1Max, ClassType.Result);

                                        ClassType.DateTimeWhite(ClassType.JobState + ClassType.DateDayS + ClassType.JobData[0] + ClassType.Jump + ClassType.CameraLog2 + ClassType.Jump + ClassType.Lgr + ClassType.DateSec + ClassType.Rgr + ClassType.Jump + ClassType.ResultTest + ClassType.OK + ClassType.Jump + ClassType.CameraValus +
                                        ClassType.Jump + maxmin2 + ClassType.Jump + maxmin4 + ClassType.Jump + maxmin6 + ClassType.Jump + maxmin8 + ClassType.Jump + maxmin10 + ClassType.Jump + maxmin12 + ClassType.Jump + maxmin14 + ClassType.Jump + maxmin16 + ClassType.Jump + Final2Max, ClassType.Result);

                                        Cam1PictureBoxResultOKNG.Image = Properties.Resources.불량;
                                        ClassType.JobResult = ClassType.JobNGSub;
                                        ClassType.JobResultState = ClassType.WORK_START;
                                        res1.Dispose();
                                    }
                                    if (ClassType.Os2State == ClassType.Os2NG)
                                    {
                                        ClassType.Os1State = ClassType.Os1END;
                                        ClassType.Os2State = ClassType.Os2END;

                                        ClassType.DateTimeWhite(ClassType.JobState + ClassType.DateDayS + ClassType.JobData[0] + ClassType.Jump + ClassType.CameraLog1 + ClassType.Jump + ClassType.Lgr + ClassType.DateSec + ClassType.Rgr + ClassType.Jump + ClassType.ResultTest + ClassType.OK + ClassType.Jump + ClassType.CameraValus +
                                        ClassType.Jump + maxmin1 + ClassType.Jump + maxmin3 + ClassType.Jump + maxmin5 + ClassType.Jump + maxmin7 + ClassType.Jump + maxmin9 + ClassType.Jump + maxmin11 + ClassType.Jump + maxmin13 + ClassType.Jump + maxmin15 + ClassType.Jump + Final1Max, ClassType.Result);

                                        ClassType.DateTimeWhite(ClassType.JobState + ClassType.DateDayS + ClassType.JobData[0] + ClassType.Jump + ClassType.CameraLog2 + ClassType.Jump + ClassType.Lgr + ClassType.DateSec + ClassType.Rgr + ClassType.Jump + ClassType.ResultTest + ClassType.NG + ClassType.Jump + ClassType.CameraValus +
                                        ClassType.Jump + maxmin2 + ClassType.Jump + maxmin4 + ClassType.Jump + maxmin6 + ClassType.Jump + maxmin8 + ClassType.Jump + maxmin10 + ClassType.Jump + maxmin12 + ClassType.Jump + maxmin14 + ClassType.Jump + maxmin16 + ClassType.Jump + Final2Max, ClassType.Result);

                                        Cam2PictureBoxResultOKNG.Image = Properties.Resources.불량;
                                        ClassType.JobResult = ClassType.JobNGSub;
                                        ClassType.JobResultState = ClassType.WORK_START;
                                        res2.Dispose();
                                    }
                                }
                            }
                        }
                    }

                    else if (ClassType.JobResult == ClassType.JobNG)  //NG일경우
                    {
                        if (ClassType.JobResultState == ClassType.WORK_START)
                        {
                            ClassType.JobResultState = ClassType.WORK_WORKING;
                        }
                        else if (ClassType.JobResultState == ClassType.WORK_WORKING)
                        {
                            MultiMotion.MoveAxis(MultiMotion.RotationMotor, DataManager.sinmove49, false, MultiMotion.KSM_SPEED_100); //회전축을 49도 이동
                            ClassType.JobResultState = ClassType.WORK_END;

                        }
                        else if (ClassType.JobResultState == ClassType.WORK_END)
                        {
                            if (Math.Abs(ClassType.sineps45) <= ClassType.eps)  //절대값 비교 49도에 도착했을때
                            {
                                SmallClass.SetPin(MultiMotion.DPSB, 8, 1);  //리프트 실린더 다운
                                ClassType.JobResult = ClassType.JobNG2;
                                ClassType.JobResultState = ClassType.WORK_START;
                            }
                        }
                    }

                    else if (ClassType.JobResult == ClassType.JobNG2)
                    {
                        if (ClassType.JobResultState == ClassType.WORK_START)
                        {
                            if (MultiMotion.InStatus[9] == 1 && MultiMotion.InStatus[11] == 1) //리프트 실린더 센서가 다운일때
                            {
                                SmallClass.SetPin(MultiMotion.DPSB, 24, 1); //리프트A OFF      
                                SmallClass.SetPin(MultiMotion.DPSB, 12, 1); //리프트C OFF
                                SmallClass.SetPin(MultiMotion.DPSB, 26, 1); //리프트A OFF 흡입      
                                SmallClass.SetPin(MultiMotion.DPSB, 14, 1); //리프트C OFF 흡입

                                SmallClass.SetPin(MultiMotion.DPSB, 25, 0); //리프트B ON
                                SmallClass.SetPin(MultiMotion.DPSB, 13, 0); //리프트D ON
                                SmallClass.SetPin(MultiMotion.DPSB, 27, 0); //리프트B ON 흡입
                                SmallClass.SetPin(MultiMotion.DPSB, 15, 0); //리프트D ON 흡입
                                ClassType.JobResultState = ClassType.WORK_WORKING;
                            }
                        }
                        else if (ClassType.JobResultState == ClassType.WORK_WORKING)
                        {
                            SmallClass.SetPin(MultiMotion.DPSB, 26, 0); //리프트A OFF 흡입      
                            SmallClass.SetPin(MultiMotion.DPSB, 14, 0); //리프트C OFF 흡입
                            ClassType.JobResultState = ClassType.WORK_WORKING3;
                        }
                        else if (ClassType.JobResultState == ClassType.WORK_WORKING2)
                        {
                            ClassType.JobResultState = ClassType.WORK_WORKING3;
                        }

                        else if (ClassType.JobResultState == ClassType.WORK_WORKING3)
                        {

                            SmallClass.SetPin(MultiMotion.DPSB, 8, 0);  //리프트 실린더 업
                            ClassType.Counting = ClassType.Counting - 1;
                            ClassType.Totalng++;
                            ClassType.Totalok--;

                            ClassType.SQLM = true;
                            ClassType.JobResultState = ClassType.WORK_WORKING4;
                        }
                        else if (ClassType.JobResultState == ClassType.WORK_WORKING4)
                        {
                            ClassType.JobResultState = ClassType.WORK_END;

                        }

                        else if (ClassType.JobResultState == ClassType.WORK_END)
                        {
                            SmallClass.SetPin(MultiMotion.DPSB, 17, 0);  //셔틀1 오픈
                            SmallClass.SetPin(MultiMotion.DPSB, 21, 0);  //셔틀2 오픈      
                            SmallClass.SetPin(MultiMotion.DPSB, 16, 1);  //셔틀1 클로즈
                            SmallClass.SetPin(MultiMotion.DPSB, 20, 1);  //셔틀2 클로즈   
                            ClassType.JobResult = ClassType.JobRotation0;
                            ClassType.JobResultState = ClassType.WORK_START;
                        }
                    }




                    else if (ClassType.JobResult == ClassType.JobNGSub)  //NG2일경우 45도이동
                    {
                        if (ClassType.JobResultState == ClassType.WORK_START)
                        {
                            ClassType.JobResultState = ClassType.WORK_WORKING;
                        }
                        else if (ClassType.JobResultState == ClassType.WORK_WORKING)
                        {
                            MultiMotion.MoveAxis(MultiMotion.RotationMotor, DataManager.sinmove49, false, MultiMotion.KSM_SPEED_100); //회전축을 49도 이동
                            ClassType.JobResultState = ClassType.WORK_END;

                        }
                        else if (ClassType.JobResultState == ClassType.WORK_END)
                        {
                            if (Math.Abs(ClassType.sineps45) <= ClassType.eps)  //절대값 비교 49도에 도착했을때
                            {
                                ClassType.JobResult = ClassType.JobNGSub2;
                                ClassType.JobResultState = ClassType.WORK_START;
                                SmallClass.SetPin(MultiMotion.DPSB, 8, 1);  //리프트 실린더 다운
                            }
                        }
                    }

                    else if (ClassType.JobResult == ClassType.JobNGSub2)
                    {
                        if (ClassType.JobResultState == ClassType.WORK_START)
                        {
                            if (MultiMotion.InStatus[9] == 1 && MultiMotion.InStatus[11] == 1) //리프트 실린더 센서가 다운일때
                            {
                                SmallClass.SetPin(MultiMotion.DPSB, 24, 0); //리프트A ON      
                                SmallClass.SetPin(MultiMotion.DPSB, 12, 0); //리프트C ON
                                SmallClass.SetPin(MultiMotion.DPSB, 26, 0); //리프트A ON 흡입      
                                SmallClass.SetPin(MultiMotion.DPSB, 14, 0); //리프트C ON 흡입

                                SmallClass.SetPin(MultiMotion.DPSB, 25, 1); //리프트B OFF
                                SmallClass.SetPin(MultiMotion.DPSB, 13, 1); //리프트D OFF
                                SmallClass.SetPin(MultiMotion.DPSB, 27, 1); //리프트B OFF 흡입
                                SmallClass.SetPin(MultiMotion.DPSB, 15, 1); //리프트D OFF 흡입  
                                ClassType.JobResultState = ClassType.WORK_WORKING;
                            }
                        }
                        else if (ClassType.JobResultState == ClassType.WORK_WORKING)
                        {
                            SmallClass.SetPin(MultiMotion.DPSB, 27, 0); //리프트B OFF 흡입
                            SmallClass.SetPin(MultiMotion.DPSB, 15, 0); //리프트D OFF 흡입  
                            ClassType.JobResultState = ClassType.WORK_WORKING3;
                        }
                        else if (ClassType.JobResultState == ClassType.WORK_WORKING2)
                        {
                            ClassType.JobResultState = ClassType.WORK_WORKING3;
                        }

                        else if (ClassType.JobResultState == ClassType.WORK_WORKING3)
                        {
                            ClassType.JobResultState = ClassType.WORK_WORKING4;

                            SmallClass.SetPin(MultiMotion.DPSB, 8, 0);  //리프트 실린더 업
                            ClassType.Counting = ClassType.Counting - 1;
                            ClassType.SQLM = true;
                            ClassType.Totalng++;
                            ClassType.Totalok--;
                        }
                        else if (ClassType.JobResultState == ClassType.WORK_WORKING4)
                        {
                            ClassType.JobResultState = ClassType.WORK_END;

                        }
                        else if (ClassType.JobResultState == ClassType.WORK_END)
                        {
                            ClassType.JobResult = ClassType.JobRotation90;
                            ClassType.JobResultState = ClassType.WORK_START;
                        }
                    }

                    //셔틀 관련
                    else if (ClassType.JobResult == ClassType.JobShuttle1) //셔틀 센서가 감지됐을때
                    {
                        if (ClassType.JobResultState == ClassType.WORK_START)
                        {
                            SmallClass.SetPin(MultiMotion.DPSB, 16, 0);  //셔틀1 클로즈
                            SmallClass.SetPin(MultiMotion.DPSB, 20, 0);  //셔틀2 클로즈  
                            SmallClass.SetPin(MultiMotion.DPSB, 17, 1);  //셔틀1 오픈
                            SmallClass.SetPin(MultiMotion.DPSB, 21, 1);  //셔틀2 오픈 
                            ClassType.JobResultState = ClassType.WORK_WORKING;
                        }
                        else if (ClassType.JobResultState == ClassType.WORK_WORKING)
                        {
                            ClassType.JobResultState = ClassType.WORK_END;

                        }
                        else if (ClassType.JobResultState == ClassType.WORK_END)
                        {
                            if (MultiMotion.InStatus[18] == 1 && MultiMotion.InStatus[19] == 1 && MultiMotion.InStatus[22] == 1 && MultiMotion.InStatus[23] == 1) //셔틀클램프가 오픈일때

                            {
                                ClassType.sinepsShuttleA1Result = MotionValueResult.ShuttleMotorLeft + ClassType.sinmoveShuttle1;
                                ClassType.sinepsShuttleA2Result = MotionValueResult.ShuttleMotorRight + ClassType.sinmoveShuttle2;
                                MultiMotion.MoveAxis(MultiMotion.Shuttle1Motor, MotionValueResult.ShuttleMotorLeft + ClassType.sinmoveShuttle1, false, MultiMotion.KSM_SPEED_20); //셔틀1이동
                                MultiMotion.MoveAxis(MultiMotion.Shuttle2Motor, MotionValueResult.ShuttleMotorRight + ClassType.sinmoveShuttle2, false, MultiMotion.KSM_SPEED_20); //셔틀2이동
                                ClassType.JobResult = ClassType.JobShuttle2;
                                ClassType.JobResultState = ClassType.WORK_START;
                                ClassType.Counting = 1;
                            }
                        }
                    }

                    else if (ClassType.JobResult == ClassType.JobShuttle2) //셔틀1
                    {
                        if (ClassType.JobResultState == ClassType.WORK_START)
                        {
                            ClassType.JobResultState = ClassType.WORK_WORKING;
                        }
                        else if (ClassType.JobResultState == ClassType.WORK_WORKING)
                        {
                            if (Math.Abs(ClassType.sinepsShuttleA1Eps) <= ClassType.eps && Math.Abs(ClassType.sinepsShuttleA2Eps) <= ClassType.eps)
                            {
                                if (MultiMotion.InStatus[26] == 0 || MultiMotion.InStatus[28] == 0) //가이드 센서 하프감지시
                                {
                                    SmallClass.SetPin(MultiMotion.DPSB, 16, 0);  //셔틀1 클로즈
                                    SmallClass.SetPin(MultiMotion.DPSB, 20, 0);  //셔틀2 클로즈  
                                    SmallClass.SetPin(MultiMotion.DPSB, 17, 1);  //셔틀1 오픈
                                    SmallClass.SetPin(MultiMotion.DPSB, 21, 1);  //셔틀2 오픈 
                                    ClassType.JobResult = ClassType.JobShuttle1;
                                    ClassType.JobResultState = ClassType.WORK_START;
                                }
                                else
                                {
                                    SmallClass.SetPin(MultiMotion.DPSB, 17, 0);  //셔틀1 오픈
                                    SmallClass.SetPin(MultiMotion.DPSB, 21, 0);  //셔틀2 오픈 
                                    SmallClass.SetPin(MultiMotion.DPSB, 16, 1);  //셔틀1 클로즈
                                    SmallClass.SetPin(MultiMotion.DPSB, 20, 1);  //셔틀2 클로즈  
                                    ClassType.JobResultState = ClassType.WORK_END;
                                }
                            }
                            else if (Math.Abs(ClassType.sinepsShuttleA1Limitvalue) <= ClassType.eps2 && Math.Abs(ClassType.sinepsShuttleA2Limitvalue) <= ClassType.eps2)
                            {
                                ClassType.Run_Flag = false;
                                ClassType.RestartCheck = false;
                                ClassType.ShuttleReady = false; //셔틀 준비완료되면
                                SmallClass.SetPin(MultiMotion.DPSB, 28, 1);  //레드 램프
                                SmallClass.SetPin(MultiMotion.DPSB, 29, 0);  //녹색 램프
                                SmallClass.SetPin(MultiMotion.DPSB, 30, 0);  //황색 램프

                                SmallClass.SetPin(MultiMotion.DPSB, 31, 1);  //알람 부저   
                                MessageBox.Show("작업이 종료되었습니다");
                                return;
                            }
                        }
                        else if (ClassType.JobResultState == ClassType.WORK_END)
                        {
                            ClassType.ShuttleSensor = false; //셔틀 하프센서 감지여부
                            ClassType.JobResult = ClassType.JobRotation0;
                            ClassType.JobResultState = ClassType.WORK_START;
                            ClassType.Counting = 1;
                        }
                    }

                    else if (ClassType.JobResult == ClassType.JobShuttle3) //셔틀2
                    {
                        if (ClassType.JobResultState == ClassType.WORK_START)
                        {
                            if (MultiMotion.InStatus[18] == 1 && MultiMotion.InStatus[19] == 1 && MultiMotion.InStatus[22] == 1 && MultiMotion.InStatus[23] == 1) //셔틀클램프가 오픈일때

                            {
                                SmallClass.SetPin(MultiMotion.DPSB, 17, 0);  //셔틀1 오픈
                                SmallClass.SetPin(MultiMotion.DPSB, 21, 0);  //셔틀2 오픈      
                                SmallClass.SetPin(MultiMotion.DPSB, 16, 1);  //셔틀1 클로즈
                                SmallClass.SetPin(MultiMotion.DPSB, 20, 1);  //셔틀2 클로즈   
                            }
                        }
                        else if (ClassType.JobResultState == ClassType.WORK_WORKING)
                        {
                            ClassType.JobResultState = ClassType.WORK_END;

                        }
                        else if (ClassType.JobResultState == ClassType.WORK_END)
                        {
                            ClassType.Counting--;

                            ClassType.JobResult = ClassType.JobRotation0;
                            ClassType.JobResultState = ClassType.WORK_START;
                        }
                    }
                }
                TimerMain.Enabled = true;
            }
            catch (Exception ex)
            {
            }
        }


        public void ManualMode()  // 매뉴얼 선택시
        {
            TimerSub2.Enabled = false;
            if (ClassType.AutoManual == false)
            {

                if (ClassType.AirUp == ClassType.AirUp1) // //에어 업
                {
                    SmallClass.SetPin(MultiMotion.DPSB, 8, 0);
                    if (ClassType.AirUpState == ClassType.AirUpWORK_START)
                    {
                        if (ClassType.LiftCheck == false)
                        {
                            if (MultiMotion.InStatus[8] == 1 && MultiMotion.InStatus[10] == 1) //리프트 실린더 센서가 업일때
                            {
                                MultiMotion.MoveAxis(MultiMotion.RotationMotor, DataManager.sinmove49, false);
                                ClassType.AirUpState = ClassType.AirUpWORK_WORKING;
                            }
                        }
                    }
                    else if (ClassType.AirUpState == ClassType.AirUpWORK_WORKING)
                    {
                        if (Math.Abs(ClassType.sineps45) <= ClassType.eps)  //절대값 비교 49도에 도착했을때
                        {
                            SmallClass.SetPin(MultiMotion.DPSB, 24, 1);  //바큠 흡입 A-B 오픈
                            SmallClass.SetPin(MultiMotion.DPSB, 25, 1);  //바큠 흡입 C-D 오픈
                            SmallClass.SetPin(MultiMotion.DPSB, 12, 1);  //바큠 흡입 A-B 오픈
                            SmallClass.SetPin(MultiMotion.DPSB, 13, 1);  //바큠 흡입 C-D 오픈
                            SmallClass.SetPin(MultiMotion.DPSB, 26, 1);  //바큠 불기 A-B 오픈
                            SmallClass.SetPin(MultiMotion.DPSB, 27, 1);  //바큠 불기 C-D 오픈    
                            SmallClass.SetPin(MultiMotion.DPSB, 14, 1);  //바큠 불기 A-B 오픈
                            SmallClass.SetPin(MultiMotion.DPSB, 15, 1);  //바큠 불기 C-D 오픈  
                            ClassType.AirUpState = ClassType.AirUpWORK_END;
                        }

                    }
                    else if (ClassType.AirUpState == ClassType.AirUpWORK_END)
                    {
                        SmallClass.SetPin(MultiMotion.DPSB, 26, 0);  //바큠 불기 A-B 오픈
                        SmallClass.SetPin(MultiMotion.DPSB, 27, 0);  //바큠 불기 C-D 오픈  
                        SmallClass.SetPin(MultiMotion.DPSB, 14, 0);  //바큠 불기 A-B 오픈
                        SmallClass.SetPin(MultiMotion.DPSB, 15, 0);  //바큠 불기 C-D 오픈  
                        ClassType.AirUp = ClassType.AirUp2;
                        ClassType.AirUpState = ClassType.AirUpWORK_START;
                        ClassType.Run_Flag1 = false;
                        return;

                    }
                }
            }
            TimerSub2.Enabled = true;
        }

        //2017.05.18 추가
        string[] aaa;
        bool stic = false;
        List<string> listA1 = new List<string>();
        string datetime;
        string datetimesc;
        public void datatime()
        {

        }

        public void updateplus()
        {
            try
            {
                string sql11 = "SELECT Model FROM Table1 WHERE Production like '" + datetime + "' ";
                DataSet ds = Microsoft_OleDb.Microsoft_OleDb.GetDataReadFind(sql11);
                aaa = new string[ds.Tables[0].Rows.Count];

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    listA1.Add(row["Model"].ToString());
                }
                for (int i = 0; i < listA1.Count; i++)
                {
                    if (txtModelName.Text == listA1[i].ToString())
                    {
                        stic = true;
                        break;
                    }
                }

                if (stic == true)
                {
                    listA1.Clear();
                    string sql = "UPDATE Table1 SET Numbers =  '" + ClassType.Totalok.ToString() + "' , Numbersng =  '" + ClassType.Totalng.ToString() + "' ,times =  '" + ClassType.TotalTime.ToString() + "'  WHERE Production = '" + datetime + "'   AND Model ='" + txtModelName.Text + "'";

                    DataSet ds1 = Microsoft_OleDb.Microsoft_OleDb.GetDataNumWhite(sql);
                    stic = false;
                }
                else
                {
                    listA1.Clear();
                    string sql = "INSERT INTO Table1(Production,Model,Numbers, Numbersng ,times) Values( '" + datetime + "' , '" + txtModelName.Text + "','0','0','0')";
                    DataSet ds1 = Microsoft_OleDb.Microsoft_OleDb.GetDataNumWhite(sql);

                    ClassType.Totalok = 0;
                    ClassType.Totalng = 0;
                    ClassType.Total = 0;
                }
            }
            catch (Exception EX)
            {

            }

        }

        public void updateminus()
        {
            try
            {
                string sql11 = "SELECT Model FROM Table1 WHERE Production like '" + datetime + "' ";
                DataSet ds = Microsoft_OleDb.Microsoft_OleDb.GetDataReadFind(sql11);
                aaa = new string[ds.Tables[0].Rows.Count];

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    listA1.Add(row["Model"].ToString());
                }
                for (int i = 0; i < listA1.Count; i++)
                {
                    if (txtModelName.Text == listA1[i].ToString())
                    {
                        stic = true;
                        break;
                    }
                }

                if (stic == true)
                {
                    listA1.Clear();
                    string sql = "UPDATE Table1 SET Numbers= Numbers -1 , Numbersng= Numbersng +1 WHERE Production = '" + datetime + "'   AND Model ='" + txtModelName.Text + "'";
                    DataSet ds1 = Microsoft_OleDb.Microsoft_OleDb.GetDataNumWhite(sql);
                    stic = false;
                }
                else
                {
                    listA1.Clear();
                    string sql = "INSERT INTO Table1(Production,Model,Numbers, Numbersng ,times) Values( '" + datetime + "' , '" + txtModelName.Text + "','0','0','0')";
                    DataSet ds1 = Microsoft_OleDb.Microsoft_OleDb.GetDataNumWhite(sql);

                    ClassType.Totalok = 0;
                    ClassType.Totalng = 0;
                    ClassType.Total = 0;
                }
            }
            catch (Exception EX)
            {

            }
        }

        public void updatenull()
        {
            try
            {
                ModelLoad_Display();
                string sql11 = "SELECT Model FROM Table1 WHERE Production like '" + datetime + "' ";
                DataSet ds = Microsoft_OleDb.Microsoft_OleDb.GetDataReadFind(sql11);
                aaa = new string[ds.Tables[0].Rows.Count];

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    listA1.Add(row["Model"].ToString());
                }
                for (int i = 0; i < listA1.Count; i++)
                {
                    if (DataManager.SelectedModelName == listA1[i].ToString())
                    {
                        stic = true;
                        break;
                    }
                }

                if (stic == true)
                {
                    listA1.Clear();

                    stic = false;
                }
                else
                {
                    listA1.Clear();
                    string sql = "INSERT INTO Table1(Production,Model,Numbers, Numbersng ,times) Values( '" + datetime + "' , '" + txtModelName.Text + "','0','0','0')";
                    DataSet ds1 = Microsoft_OleDb.Microsoft_OleDb.GetDataNumWhite(sql);

                    ClassType.Totalok = 0;
                    ClassType.Totalng = 0;
                    ClassType.Total = 0;
                }
            }
            catch (Exception EX)
            {

            }
        }

        #endregion 작업시퀀스 ...

        #region 타이머...

        private System.Windows.Forms.Timer TimerSub = new System.Windows.Forms.Timer();
        private System.Windows.Forms.Timer TimerMain = new System.Windows.Forms.Timer();
        private System.Windows.Forms.Timer TimerSub2 = new System.Windows.Forms.Timer();
        private System.Windows.Forms.Timer TimerSub3 = new System.Windows.Forms.Timer();
        private System.Windows.Forms.Timer TimerSub4 = new System.Windows.Forms.Timer();
        private System.Windows.Forms.Timer TimerSub5 = new System.Windows.Forms.Timer();
        public void TimerMainTick()
        {
            TimerMain.Interval = 60;
            TimerMain.Tick += TimerMain_Tick;
        }
        public void TimerSubTick()
        {
            TimerSub.Interval = 50;
            TimerSub.Tick += TimerSub_Tick;
        }
        public void TimerSub2Tick()
        {
            TimerSub2.Interval = 500;
            TimerSub2.Tick += TimerSub2_Tick;
        }
        public void TimerSub3Tick()
        {
            TimerSub3.Interval = 1000;
            TimerSub3.Tick += TimerSub3_Tick;
        }
        public void TimerSub4Tick()
        {
            TimerSub4.Interval = 10;
            TimerSub4.Tick += TimerSub4_Tick;
        }
        public void TimerSub5Tick()
        {
            TimerSub5.Interval = 5000;
            TimerSub5.Tick += TimerSub5_Tick;
        }

        void TimerMain_Tick(object sender, EventArgs e)
        {
            TimerMainJob();
        }
        void TimerSub_Tick(object sender, EventArgs e)
        {
            TimerSubJob();
            DateTimeCheck(); //날짜 체크
        }
        void TimerSub2_Tick(object sender, EventArgs e)
        {
            TimerSubJob2();
        }
        void TimerSub3_Tick(object sender, EventArgs e)
        {
            TimerSubJob3();
        }
        void TimerSub4_Tick(object sender, EventArgs e)
        {
            TimerSubJob4();
        }
        void TimerSub5_Tick(object sender, EventArgs e)
        {
            TimerSubJob5();
        }

        public void TimerMainJob()
        {
            sin(); //시퀀스제어

        }
        public void TimerSubJob()
        {
            MultiMotion.GetDIOStatus(); //DIO 체크
            ServoStopInfoCheck(); //서보정지원인 체크
            MotionChek();  //실시간 모션 값 체크
            Accode();
            ModelSelect();//모델 선택 확인

        }
        public void TimerSubJob2()
        {
            GuidMove();
            ManualMode();

        }
        public void TimerSubJob3()
        {
            datatime();
            Maketime();
        }
        public void TimerSubJob4()
        {
            LiftUp();
        }
        public void TimerSubJob5()
        {
            if (ClassType.Run_Flag == true)
            {
                updateplus();
            }
        }
        #endregion 타이머 ...

        private void DB_Click(object sender, EventArgs e)
        {
            if (ClassType.Run_Flag == true)
            {
                MessageBox.Show(ClassType.TestWaitMsg);
                return;
            }
            // ClassType.DateTimeWhite("시간:" + ClassType.DateSec + "    " + "모델명:" + txtModelName.Text + "    " + "제품 합계:" + label5.Text + "개" + "    " + "총제품생산시간:" + LBLMakeStart.Text + "    " + "제품생산시간:" + LBLMakeOut.Text, ClassType.JobResultstate);
            BTB_Db BTB_Db = new BTB_Db();
            updateplus();
            BTB_Db.ShowDialog();
        }

        private void txtTotal_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void txtTotal_Leave(object sender, EventArgs e)
        {
            if (ClassType.Run_Flag == true)
            {
                MessageBox.Show(ClassType.TestWaitMsg);
                return;
            }
            int ipno = Convert.ToInt32(txtTotal.Text);
            if (ipno > 99999)
            {
                MessageBox.Show("입력값을 초과하였습니다");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ClassType.Run_Flag = true;


        }

        private void button2_Click(object sender, EventArgs e)
        {

            ClassType.Run_Flag = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ClassType.SQLP = true;
        }

        private void LiftReady_Click(object sender, EventArgs e)
        {
            if (MultiMotion.MotionCheck == 2)
            {
                for (int i = 0; i < 5; i++)
                {
                    lamp.SendCommandSetValue1(i, Int32.Parse(textBox1.Text));
                }
            }
        }

        bool stb = true;

        private void button4_Click(object sender, EventArgs e)
        {
            if (stb)
            {
                SmallClass.SetPin(MultiMotion.DPSB, 17, 0);  //셔틀1 오픈   (0=off)
                SmallClass.SetPin(MultiMotion.DPSB, 21, 0);  //셔틀2 오픈   (0=off)
                SmallClass.SetPin(MultiMotion.DPSB, 16, 1);  //셔틀1 클로즈 (1=on)
                SmallClass.SetPin(MultiMotion.DPSB, 20, 1);  //셔틀2 클로즈 (1=on)
            }
            else
            {
                SmallClass.SetPin(MultiMotion.DPSB, 16, 0);  //셔틀1 클로즈  (0=off)
                SmallClass.SetPin(MultiMotion.DPSB, 20, 0);  //셔틀2 클로즈  (0=off)
                SmallClass.SetPin(MultiMotion.DPSB, 17, 1);  //셔틀1 오픈 on (1=on)
                SmallClass.SetPin(MultiMotion.DPSB, 21, 1);  //셔틀2 오픈 on (1=on)
            }

            stb = !stb;
        }
    }
}
