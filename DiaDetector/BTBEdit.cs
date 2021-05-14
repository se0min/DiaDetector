using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NeptuneClassLibWrap;
using System.Threading;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using DiaDetector.Data;
using DiaDetector.Drivers;
namespace DiaDetector
{
    public partial class BTBEdit : Form
    {
        public BTBEdit()
        {
            InitializeComponent();
            InitializeComponents1(); ;
            timerThreads();
        }


        public void InitializeComponents1()
        {
            CheckForIllegalCrossThreadCalls = false;
            InitFormCtrl1();
            Neptune2 = new NeptuneClassLibCLR();
            Neptune2.InitLibrary();
            UpdateCameraList1();//캠정보 업데이트
            camlist1();
            CamLive1();  //캠정보저장 후 불러오기
            textPiename1.Text = ClassType.Pie.ToString();
            textPiename2.Text = ClassType.Pie.ToString();
        }
        #region 전역 변수
        private string[] comboBoxCamerastring1 = new string[ClassType.MAX_CAM];
        private System.Windows.Forms.PictureBox[] pictureBoxDisplay1 = new System.Windows.Forms.PictureBox[ClassType.MAX_CAM];
        private NeptuneClassLibCLR Neptune2 = null;
        private NEPTUNE_CAM_INFO[] m_CamInfo1 = null;
        private CameraInstance[] Cam1 = new CameraInstance[ClassType.MAX_CAM] { null, null };
        private DisplayImage[] m_Display1 = new DisplayImage[ClassType.MAX_CAM] { null, null };
        private bool[] m_bPlay1 = new bool[ClassType.MAX_CAM] { false, false };
        private bool[] m_bGrab1 = new bool[ClassType.MAX_CAM] { false, false };
        private Thread[] m_Thread1 = new Thread[ClassType.MAX_CAM] { null, null };
        private uint m_nGrabCount1 = 0;
        private bool m_bPlay = false;
        System.IO.FileStream stream1;  //카메라 1번
        System.IO.FileStream stream2;  //카메라 1번
        #endregion 전역 변수
        MainFrame mf = new MainFrame();






        //CamLive
        public void CamLive1()
        {
            try
            {
                comboBoxGrabMode.SelectedIndex = 0;
                for (int i = 0; i < ClassType.MAX_CAM; i++)
                {
                    if (Cam1[i] != null)
                    {
                        if (Cam1[i].AcquisitionStart((ENeptuneGrabMode)comboBoxGrabMode.SelectedIndex) != ENeptuneError.NEPTUNE_ERR_Success)
                        {
                            string strMsg = "Cam" + i.ToString() + " Acquisition start error!";
                            MessageBox.Show(strMsg);
                            continue;
                        }
                        m_bPlay1[i] = true;
                        if (m_Thread1[i] == null)
                        {
                            m_Thread1[i] = new Thread(new ParameterizedThreadStart(AcquisitionThread1));
                            m_Thread1[i].Start(i);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        FrameDataPtr data11 = new FrameDataPtr();
        FrameDataPtr data111 = new FrameDataPtr();


        private void AcquisitionThread1(object obj)
        {
            int nIdx = (int)obj;


            try
            {

                while (m_bPlay1[nIdx])
                {
                    ENeptuneError eErr = Cam1[0].WaitEventDataStream(ref data11, 10000);
                    if (eErr != ENeptuneError.NEPTUNE_ERR_Success)
                        continue;
                    m_Display1[1].DrawRawImage(data11);
                    Cam1[1].QueueBufferDataStream(data11.GetBufferIndex());
                    ENeptuneError eErr1 = Cam1[1].WaitEventDataStream(ref data111, 10000);
                    if (eErr1 != ENeptuneError.NEPTUNE_ERR_Success)
                        continue;
                    m_Display1[0].DrawRawImage(data111);
                    Cam1[0].QueueBufferDataStream(data111.GetBufferIndex());
                }


            }
            catch (Exception ex)
            {

            }
        }


        private void UpdateCameraList1()
        {
            uint nCam = DeviceManager.Instance.GetTotalCamera();

            m_CamInfo1 = new NEPTUNE_CAM_INFO[nCam];
            DeviceManager.Instance.GetCameraList(m_CamInfo1, nCam);
            //for (int i = 0; i < nCam; i++)
            //{
            //    for (int j = 0; j < ClassType.MAX_CAM; j++)
            //        comboBoxCamerastring[j] = m_CamInfo[i].strVendor + " : " + m_CamInfo[i].strModel + " S/N: " + m_CamInfo[i].strSerial;
            //}
        }


        private void InitFormCtrl1()
        {
            pictureBoxDisplay1[0] = pictureBox1;
            pictureBoxDisplay1[1] = pictureBox2;
        }

        public void camlist1()  //콤보박스 강제로 불러와서 그값으로 실행시키기
        {
            {
                NeptuneDevice iDev = DeviceManager.Instance.GetDeviceFromSerial(m_CamInfo1[1].strSerial);
                NeptuneDevice iDev1 = DeviceManager.Instance.GetDeviceFromSerial(m_CamInfo1[0].strSerial);
                try
                {
                    Cam1[0] = new CameraInstance(iDev, ENeptuneDevAccess.NEPTUNE_DEV_ACCESS_EXCLUSIVE);
                    Cam1[1] = new CameraInstance(iDev1, ENeptuneDevAccess.NEPTUNE_DEV_ACCESS_EXCLUSIVE);
                }
                catch (Exception exp)
                {

                }
                m_Display1[1] = new DisplayImage(pictureBoxDisplay1[1].Handle);
                m_Display1[0] = new DisplayImage(pictureBoxDisplay1[0].Handle);
            }
        }

        public void LiveExit1()
        {

            for (int i = 0; i < ClassType.MAX_CAM; i++)
            {
                if (Cam1[i] != null)
                {
                    m_bPlay1[i] = false;

                    if (comboBoxGrabMode.SelectedIndex == (int)ENeptuneGrabMode.NEPTUNE_GRAB_CONTINUOUS)
                    {
                        m_Thread1[i].Join();
                        m_Thread1[i] = null;
                    }

                    Cam1[i].AcquisitionStop();

                    //buttonPlay.Enabled = true;
                    //buttonStop.Enabled = false;
                    //buttonGrab.Enabled = false;
                    comboBoxGrabMode.Enabled = true;
                    m_nGrabCount1 = 0;

                    pictureBoxDisplay1[i].Image = null;
                }
            }
        }
        public void eurosys11(byte[] raw)
        {
            try
            {

                Bitmap res = new Bitmap(1288, 964, PixelFormat.Format8bppIndexed);


                BitmapData LoadBitmap = res.LockBits(new Rectangle(0, 0, res.Width, res.Height), ImageLockMode.ReadWrite, res.PixelFormat);
                IntPtr ptr = LoadBitmap.Scan0;
                Marshal.Copy(raw, 0, ptr, res.Width * res.Height);

                ColorPalette cp = res.Palette;
                for (int i = 0; i < 256; i++)
                {
                    cp.Entries[i] = Color.FromArgb(i, i, i);
                }
                res.Palette = cp;
                res.UnlockBits(LoadBitmap);
                pictureBox1.Image = res;
            }
            catch (Exception ex)
            {
            }
        }
        public void close1()
        {
            try
            {
                for (int i = 0; i < ClassType.MAX_CAM; i++)
                {
                    if (m_bPlay1[i])
                    {
                        m_bPlay1[i] = false;
                        if (comboBoxGrabMode.SelectedIndex == (int)ENeptuneGrabMode.NEPTUNE_GRAB_CONTINUOUS)
                            m_Thread1[i].Join();
                        Cam1[i].AcquisitionStop();
                    }

                    if (Cam1[i] != null)
                        Cam1[i].CameraClose();
                }
            }
            catch(Exception ex)
                {
                }

        }
        private void button1_Click(object sender, EventArgs e)
        {
           // this.DialogResult = DialogResult.OK;
           //// ClassType.sss = false;
           // this.Close();
           // ff.camload();
           // Neptune2.UninitLibrary();
        
        }
        MainFrame ff = new MainFrame();
        private void BTBEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            //ff.camload();     
            Neptune2.UninitLibrary();
            close1();
           // ClassType.sss = false;
            //ff = new MainFrame();
            //if (ff.ShowDialog() == DialogResult.OK)
            //{
            //}
            //else
            //{
            //}
        }




        private void btSave1_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("저장하시겠습니까?", "값 저장", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                DataManager.SelectedModel.Camera1Moving = double.Parse(Camera1TapJogResult.Text);
                DataManager.SaveModelListFiles();
            }
        }

        private void btClose1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
           // ff.camload();
            Neptune2.UninitLibrary();
            close1();
        }

        private void btSave2_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("저장하시겠습니까?", "값 저장", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                DataManager.SelectedModel.Camera2Moving = double.Parse(Camera2TapJogResult.Text);
                DataManager.SaveModelListFiles();
            }
        }

        private void btClose2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
           // ff.camload();
            Neptune2.UninitLibrary();
            close1();

        }


        //카메라1 탭
        private void Camera1TapJogL_MouseDown(object sender, MouseEventArgs e)
        {
            MultiMotion.JogMove(MultiMotion.Camera1Adjust, 1);
        }

        private void Camera1TapJogL_MouseUp(object sender, MouseEventArgs e)
        {
            MultiMotion.JogStop(MultiMotion.Camera1Adjust);
        }

        private void Camera1TapJogR_MouseDown(object sender, MouseEventArgs e)
        {
            MultiMotion.JogMove(MultiMotion.Camera1Adjust, 0);
        }

        private void Camera1TapJogR_MouseUp(object sender, MouseEventArgs e)
        {
            MultiMotion.JogStop(MultiMotion.Camera1Adjust);
        }

        private void Camera1TapJogHome_Click(object sender, EventArgs e)
        {     
                MultiMotion.HomeMove(MultiMotion.Camera1Adjust, false);
        }

        private void Camera1TapJogResultMoveClick_Click(object sender, EventArgs e)
        {
            MultiMotion.MoveAxis(MultiMotion.Camera1Adjust,double.Parse( Camera1TapJogResultMove.Text), false, MultiMotion.KSM_SPEED_10); //리프트 정면 이동

        }





        //카메라2 탭
        private void Camera2TapJogL_MouseDown(object sender, MouseEventArgs e)
        {
            MultiMotion.JogMove(MultiMotion.Camera2Adjust, 1);
        }

        private void Camera2TapJogL_MouseUp(object sender, MouseEventArgs e)
        {
            MultiMotion.JogStop(MultiMotion.Camera2Adjust);
        }

        private void Camera2TapJogR_MouseDown(object sender, MouseEventArgs e)
        {
            MultiMotion.JogMove(MultiMotion.Camera2Adjust, 0);
        }

        private void Camera2TapJogR_MouseUp(object sender, MouseEventArgs e)
        {
            MultiMotion.JogStop(MultiMotion.Camera2Adjust);
        }

        private void Camera2TapJogHome_Click(object sender, EventArgs e)
        {
            MultiMotion.HomeMove(MultiMotion.Camera2Adjust, false);
        }

        private void Camera2TapJogResultMoveClick_Click(object sender, EventArgs e)
        {
            MultiMotion.MoveAxis(MultiMotion.Camera2Adjust, double.Parse(Camera2TapJogResultMove.Text), false, MultiMotion.KSM_SPEED_10); //리프트 정면 이동

        }



        private System.Windows.Forms.Timer timerThread = new System.Windows.Forms.Timer();

        public void timerThreads()
        {
            timerThread.Interval = 100;
            timerThread.Tick += timerThread_Tick;
            timerThread.Enabled = true;
        }
        MotionValue MotionValueResult;
        void timerThread_Tick(object sender, EventArgs e)
        {

            MultiMotion.GetCurrentPos();
            MotionValueResult.Cam1Motor = MultiMotion.AxisValue[MultiMotion.Camera1Adjust];
            MotionValueResult.Cam2Motor = MultiMotion.AxisValue[MultiMotion.Camera2Adjust];
            MotionValueResult.LiftMotorLeft = MultiMotion.AxisValue[MultiMotion.Lift1Motor];
            MotionValueResult.LiftMotorRight = MultiMotion.AxisValue[MultiMotion.Lift2Motor];
            MotionValueResult.ShuttleMotorLeft = MultiMotion.AxisValue[MultiMotion.Shuttle1Motor];
            MotionValueResult.ShuttleMotorRight = MultiMotion.AxisValue[MultiMotion.Shuttle2Motor];
            MotionValueResult.RotationMotor = MultiMotion.AxisValue[MultiMotion.RotationMotor];

            Camera1TapJogResult.Text = MotionValueResult.Cam1Motor.ToString("0.####");
            Camera2TapJogResult.Text = MotionValueResult.Cam2Motor.ToString("0.####");

        }
     




       
    }
}
