using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using AutoAssembler.Data;
using AutoAssembler.Drivers;
using AutoAssembler.VisionLibrary;
using AutoAssembler.Utilities;


namespace AutoAssembler
{
    public partial class frmMonitorCam : Form
    {
        public WorkFuncInfo _WorkFuncInfo;  // 홍동성 => 시나리오 정보 저장

        private const int LAMP_INC_GAP = 30;
        clsLamp LampComm = new clsLamp();

        
        public int CamIndex = -1;
        public string CamIndexID = "";

        /// <summary>
        /// The VimbaHelper (see VimbaHelper Class)
        /// </summary>
        private VimbaHelper m_VimbaHelper = null;

        /// <summary>
        /// Flag to indicate if the camera is acquiring images
        /// </summary>
        private bool m_Acquiring = false;

        public frmMonitorCam()
        {
            InitializeComponent();
        }

        private void frmMonitorCam_Load(object sender, EventArgs e)
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
                    textBox1.Text += "\n" + "Could not update camera list. Reason: " + exception.Message;
                }
            }
            catch (Exception exception)
            {
                textBox1.Text += "\n" + "Could not startup Vimba API. Reason: " + exception.Message;
            }


            LampComm.Open(DeviceManager.LightingComPort);

            // 조명
            // ----------
            for (int i = 0; i < 4; i++)
            {
                cboLampList.Items.Add(DataManager.LightingSettingInfoList[i].Name);
            }



            // 동작 기능 선택
            // ----------
            cboLampList.SelectedIndex = _WorkFuncInfo.Lamp_Index;

        }

        /// <summary>
        /// Update the camera List in the UI
        /// </summary>
        private void UpdateCameraList()
        {
            // Remember the old selection (if there was any)y
            CameraInfo oldSelectedItem = m_CameraList.SelectedItem as CameraInfo;
            m_CameraList.Items.Clear();

            List<CameraInfo> cameras = m_VimbaHelper.CameraList;

            CameraInfo newSelectedItem = null;
            foreach (CameraInfo cameraInfo in cameras)
            {
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
            }

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

                    textBox1.Text += "\n" + "Camera list updated.";
                }
                catch (Exception exception)
                {
                    textBox1.Text += "\n" + "Could not update camera list. Reason: " + exception.Message;
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
                    m_PictureBox.Image = image;
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

                        textBox1.Text += "\n" + "Asynchronous image acquisition stopped.";
                    }
                    catch (Exception exception)
                    {
                        textBox1.Text += "\n" + "Error while stopping asynchronous image acquisition. Reason: " + exception.Message;
                    }
                }
            }
        }

        /// <summary>
        /// Updates the state of the Acquisition and Trigger controls
        /// </summary>
        private void UpdateControls()
        {
            if (true == m_Acquiring)
            {
                //m_AcquireButton.Text = "Stop image acquisition";
                //m_AcquireButton.Enabled = true;
                textBox1.Text += "\n" + "Start image acquisition1.";
            }
            else
            {
                //m_AcquireButton.Text = "Start image acquisition";
                textBox1.Text += "\n" + "Start image acquisition2.";

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
        }


        private void m_CameraList_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Close the camera if it was opened
            m_VimbaHelper.CloseCamera();

            // Determine selected camera
            CameraInfo selectedItem = m_CameraList.SelectedItem as CameraInfo;
            if (null == selectedItem)
            {
                textBox1.Text += "\n" + "No camera selected.";
                throw new NullReferenceException("No camera selected.");
            }
            else
            {
                textBox1.Text += "\n" + "camera selected OK.";

            }

            // Open selected camera
            m_VimbaHelper.OpenCamera(selectedItem.ID);

            UpdateControls();

            // In case that the check box is still checked, enable the software trigger
            if (m_VimbaHelper.IsTriggerAvailable)
            {
                //m_VimbaHelper.EnableSoftwareTrigger(m_SoftwareTriggerCheckbox.Checked);
            }
        }

        private void btCamStart_Click(object sender, EventArgs e)
        {
            if (false == m_Acquiring)
            {
                try
                {
                    // Determine selected camera
                    CameraInfo selectedItem = m_CameraList.SelectedItem as CameraInfo;
                    if (null == selectedItem)
                    {
                        throw new NullReferenceException("No camera selected.");
                    }

                    // Open the camera if it was not opened before
                    m_VimbaHelper.OpenCamera(selectedItem.ID);

                    // Start asynchronous image acquisition (grab) in selected camera
                    m_VimbaHelper.StartContinuousImageAcquisition(this.OnFrameReceived);

                    m_Acquiring = true;
                    UpdateControls();

                    // Disable the camera list to inhibit changing the camera
                    m_CameraList.Enabled = false;

                    textBox1.Text += "\n" + "Asynchronous image acquisition started.";
                }
                catch (Exception exception)
                {
                    textBox1.Text += "\n" + "Could not start asynchronous image acquisition. Reason: " + exception.Message;
                }
            }
            else
            {
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
                    }

                    textBox1.Text += "\n" + "Asynchronous image acquisition stopped.";
                }
                catch (Exception exception)
                {
                    textBox1.Text += "\n" + "Error while stopping asynchronous image acquisition. Reason: " + exception.Message;
                }

                // Re-enable the camera list
                m_CameraList.Enabled = true;
            }
        }

        private void btCamStop_Click(object sender, EventArgs e)
        {

        }

        private void btSave_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Dispose();
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Dispose();
        }

        private void frmMonitorCam_FormClosing(object sender, FormClosingEventArgs e)
        {
            for (int i = 0; i < 8; i++)
            {
                LampComm.OFFLamp(i);
            }


            LampComm.Close();
            LampComm = null;


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
                    textBox1.Text += "\n" + "Could not shutdown Vimba API. Reason: " + exception.Message;
                }
            }
        }

        private void m_CameraList_Click(object sender, EventArgs e)
        {
            // Close the camera if it was opened
            m_VimbaHelper.CloseCamera();

            // Determine selected camera
            CameraInfo selectedItem = m_CameraList.SelectedItem as CameraInfo;
            if (null == selectedItem)
            {
                //throw new NullReferenceException("No camera selected.");
                return;
            }

            // Open selected camera
            m_VimbaHelper.OpenCamera(selectedItem.ID);

            UpdateControls();

            // In case that the check box is still checked, enable the software trigger
            if (m_VimbaHelper.IsTriggerAvailable)
            {
                //m_VimbaHelper.EnableSoftwareTrigger(m_SoftwareTriggerCheckbox.Checked);
            }
        }

        private void m_PictureBox_Paint(object sender, PaintEventArgs e)
        {
            VimbaHelper.ImageInUse = true;
        }

        private void SetValueLamp()
        {
            if (cboLampList.SelectedIndex > -1)
            {
                int SetChnlNum = DataManager.LightingSettingInfoList[cboLampList.SelectedIndex].Channel - 1;
                int SetValueNum = int.Parse(txtLampValue.Text);
                LampComm.SetLamp(SetChnlNum, SetValueNum);
            }
        }

        private void btnJogLampMinus_Click(object sender, EventArgs e)
        {
            int SetValueNum = int.Parse(txtLampValue.Text);
            SetValueNum = SetValueNum - LAMP_INC_GAP;
            if (SetValueNum < 0)
            {
                SetValueNum = 0;
            }
            txtLampValue.Text = SetValueNum.ToString();
            SetValueLamp();

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

        private void txtLampValue_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

    }
}
