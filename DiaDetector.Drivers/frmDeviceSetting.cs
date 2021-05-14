using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiaDetector.Drivers
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
            DeviceManagerS.Read();

            txtPAIX_Name.Text   = DeviceManagerS.PAIX_Model;
            txtPAIX_IP.Text     = DeviceManagerS.PAIX_IP; ;
            txtPAIX_Port.Text   = DeviceManagerS.PAIX_Port;

            txtPAIX_Name2.Text = DeviceManagerS.PAIX_Model2;
            txtPAIX_IP2.Text = DeviceManagerS.PAIX_IP2; ;
            txtPAIX_Port2.Text = DeviceManagerS.PAIX_Port2;
            // 통신 포트 ...
            // ----------
            comLightingPort.SelectedIndex = DeviceManagerS.LightingComPort - 1;
            cboWeldingPort.SelectedIndex = DeviceManagerS.WeldingComPort - 1;
            cboTiltingPort.SelectedIndex = DeviceManagerS.TilTingComPort - 1;
            cboSelectedAxis.SelectedIndex = DeviceManagerS.TilTingAxis - 1;





            // 20160803 업데이트 ...
            // ----------            
            //cboSelectedAxis.SelectedIndex = DeviceManager.LightingComPort - 1;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            DeviceManagerS.PAIX_Model    = txtPAIX_Name.Text;
            DeviceManagerS.PAIX_IP       = txtPAIX_IP.Text;
            DeviceManagerS.PAIX_Port     = txtPAIX_Port.Text;

            DeviceManagerS.PAIX_Model2 = txtPAIX_Name2.Text;
            DeviceManagerS.PAIX_IP2 = txtPAIX_IP2.Text;
            DeviceManagerS.PAIX_Port2 = txtPAIX_Port2.Text;

            DeviceManagerS.LightingComPort = comLightingPort.SelectedIndex + 1;

            // 20160803 업데이트 ...
            // ----------
            DeviceManagerS.WeldingComPort    = cboWeldingPort.SelectedIndex + 1;
            DeviceManagerS.TilTingComPort    = cboTiltingPort.SelectedIndex + 1;
            DeviceManagerS.TilTingAxis       = cboSelectedAxis.SelectedIndex + 1;

            // ----------            

            DeviceManagerS.Write();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {


        }

        private void groupBox8_Enter(object sender, EventArgs e)
        {

        }


    }
}
