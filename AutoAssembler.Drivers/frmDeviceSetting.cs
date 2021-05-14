using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AutoAssembler.Drivers
{
    public partial class frmDeviceSetting : Form
    {
        public frmDeviceSetting()
        {
            InitializeComponent();
        }

        private void frmDeviceSetting_Load(object sender, EventArgs e)
        {
            Initialize();
        }

        public void Initialize()
        {
            DeviceManager.Read();

            txtPAIX_Name.Text   = DeviceManager.PAIX_Model;
            txtPAIX_IP.Text     = DeviceManager.PAIX_IP; ;
            txtPAIX_Port.Text   = DeviceManager.PAIX_Port;

            txtPAIX2_Name.Text   = DeviceManager.PAIX2_Model;
            txtPAIX2_IP.Text     = DeviceManager.PAIX2_IP; ;
            txtPAIX2_Port.Text   = DeviceManager.PAIX2_Port;

            txtPAIX3_Name.Text   = DeviceManager.PAIX3_Model;
            txtPAIX3_IP.Text     = DeviceManager.PAIX3_IP; ;
            txtPAIX3_Port.Text   = DeviceManager.PAIX3_Port;
            
            // 통신 포트 ...
            // ----------
            comLightingPort.SelectedIndex = DeviceManager.LightingComPort - 1;
            cboWeldingPort.SelectedIndex = DeviceManager.WeldingComPort - 1;
            cboTiltingPort.SelectedIndex = DeviceManager.TilTingComPort - 1;
            cboSelectedAxis.SelectedIndex = DeviceManager.TilTingAxis - 1;


            // 좌표 설정 ...
            // ----------
            txtLaserXHome.Text = MultiMotion.dWeldStartBaseX.ToString();
            txtLaserZHome.Text = MultiMotion.dWeldStartBaseZ.ToString();
            txtRollingXHome.Text = MultiMotion.dIndex_XPos.ToString();
            txtRollingXOffset.Text = MultiMotion.dIndex_XOffset.ToString();




            // 20160803 업데이트 ...
            // ----------            
            //cboSelectedAxis.SelectedIndex = DeviceManager.LightingComPort - 1;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            DeviceManager.PAIX_Model    = txtPAIX_Name.Text;
            DeviceManager.PAIX_IP       = txtPAIX_IP.Text;
            DeviceManager.PAIX_Port     = txtPAIX_Port.Text;

            DeviceManager.PAIX2_Model   = txtPAIX2_Name.Text;
            DeviceManager.PAIX2_IP      = txtPAIX2_IP.Text;
            DeviceManager.PAIX2_Port    = txtPAIX2_Port.Text;

            DeviceManager.PAIX3_Model   = txtPAIX3_Name.Text;
            DeviceManager.PAIX3_IP      = txtPAIX3_IP.Text;
            DeviceManager.PAIX3_Port    = txtPAIX3_Port.Text;

            DeviceManager.LightingComPort = comLightingPort.SelectedIndex + 1;

            // 20160803 업데이트 ...
            // ----------
            DeviceManager.WeldingComPort    = cboWeldingPort.SelectedIndex + 1;
            DeviceManager.TilTingComPort    = cboTiltingPort.SelectedIndex + 1;
            DeviceManager.TilTingAxis       = cboSelectedAxis.SelectedIndex + 1;

            MultiMotion.dWeldStartBaseX     = double.Parse(txtLaserXHome.Text);
            MultiMotion.dWeldStartBaseZ     = double.Parse(txtLaserZHome.Text);
            MultiMotion.dIndex_XPos         = double.Parse(txtRollingXHome.Text);
            MultiMotion.dIndex_XOffset      = double.Parse(txtRollingXOffset.Text);
            // ----------            

            DeviceManager.Write();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {


        }

        private void groupBox8_Enter(object sender, EventArgs e)
        {

        }


    }
}
