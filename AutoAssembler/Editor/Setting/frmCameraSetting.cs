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

//////////

using AutoAssembler.Data;

namespace AutoAssembler
{
    public partial class frmCameraSetting : Form
    {
        private string _FileName = ConfigManager.GetDataFilePath + "setting_camera.dat";

#region SonyCamSetting

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
        private Boolean Scale_Flag, Disp_Flag;
        private bool DispSize_Checked = false;

        private static bool GetProcRunFlag;
        private Image RecoBlankImg;
        private static int Pic_Width, Pic_Height;

        private static RGBDatInfo[,] RGBDXFData;
        private static bool RGBListFlag;
        private static bool RGBListActionFlag;

        private static void SystemCallback(STATUS_SYSTEMCODE SystemStatus, IntPtr Context)
        {
            GCHandle param = GCHandle.FromIntPtr(Context);
            frmCameraSetting CameraListRef = (frmCameraSetting)param.Target;

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
            frmCameraSetting VRef = (frmCameraSetting)param.Target;
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
            
#endregion

        public frmCameraSetting()
        {
            InitializeComponent();
            InitCamSetting();
        }

        private void frmCamSetting_Load(object sender, EventArgs e)
        {
            Initialize();
        }

        public void Initialize()
        {
            // 초기화 ...
            lstCameraList.Columns.Add("카메라 이름", 219, HorizontalAlignment.Center);
            //lstOutputView.Columns.Add("이름", 199, HorizontalAlignment.Center);

            // 업데이트 ...
            ReDrawList();


            // 첫 항목 선택 ...
            //lstCameraList.

            InitializingCam();
        }

        private void ReDrawList()
        {
            lstCameraList.Items.Clear();

            for (int i = 0; i < 16; i++)
            {
                ListViewItem lstViewTestItem = new ListViewItem(DataManager.CameraSettingInfoList[i].Name);


                // 홍동성 ...
                // ----------
                //lstViewTestItem.Tag = DataManager.LightingSettingInfoList[i];

                //lstViewTestItem.SubItems.Add(DataManager.DIOSettingInfoList[i].Name);
                // ----------


                lstCameraList.Items.Add(lstViewTestItem);
            }

            lstCameraList.EndUpdate();
        }

#region Button ...

        private void btnOk_Click(object sender, EventArgs e)
        {
            DataManager.SaveCameraSettingFiles(_FileName);         // 저장 ...
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DataManager.LoadCameraSettingFiles(_FileName);    // 원상 복구 ...
        }

        private void btnStart_Click(object sender, EventArgs e)
        {

        }

        private void btnEnd_Click(object sender, EventArgs e)
        {

        }


#endregion Button ...


#region Event ...

        private void lstCameraList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstCameraList.SelectedItems.Count == 1)
            {
                int index = lstCameraList.SelectedIndices[0];

                txtName.Text                    = DataManager.CameraSettingInfoList[index].Name;
                txtIP.Text                      = DataManager.CameraSettingInfoList[index].IP;
                cboCamScreenSize.SelectedIndex  = DataManager.CameraSettingInfoList[index].ScreenSize;
                txtFrameRate.Text               = DataManager.CameraSettingInfoList[index].FrameRate.ToString();
                cboZoomPortNum.SelectedIndex    = DataManager.CameraSettingInfoList[index].ZoomFocusPort - 1;
            }
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (lstCameraList.SelectedItems.Count == 1)
            {
                int index = lstCameraList.SelectedIndices[0];

                DataManager.CameraSettingInfoList[index].Name = txtName.Text;

                lstCameraList.SelectedItems[0].Text = txtName.Text;
            }
        }

        private void txtIP_TextChanged(object sender, EventArgs e)
        {
            if (lstCameraList.SelectedItems.Count == 1)
            {
                int index = lstCameraList.SelectedIndices[0];

                DataManager.CameraSettingInfoList[index].IP = txtIP.Text;
            }
        }

        private void txtFrameRate_TextChanged(object sender, EventArgs e)
        {
            if (lstCameraList.SelectedItems.Count == 1)
            {
                int index = lstCameraList.SelectedIndices[0];

                DataManager.CameraSettingInfoList[index].FrameRate = Convert.ToDouble(this.txtFrameRate.Text);
            }
        }

        private void cboCamScreenSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstCameraList.SelectedItems.Count == 1)
            {
                int index = lstCameraList.SelectedIndices[0];

                DataManager.CameraSettingInfoList[index].ScreenSize = cboCamScreenSize.SelectedIndex;
            }
        }

        private void cboZoomPortNum_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstCameraList.SelectedItems.Count == 1)
            {
                int index = lstCameraList.SelectedIndices[0];

                DataManager.CameraSettingInfoList[index].ZoomFocusPort = cboZoomPortNum.SelectedIndex + 1;
            }
        }


#endregion Event ...

        private void UIDList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idx;

            idx = UIDList.SelectedIndex;
            if (CamTotalNum > idx)
            {
                //CameraOpen.Enabled = true;
                //CameraClose.Enabled = false;
                txtIP.Text = UIDList.Items[UIDList.SelectedIndex].ToString();
            }
            else
            {
                //CameraOpen.Enabled = false;
                //CameraClose.Enabled = true;
            }
        }


    }
}
