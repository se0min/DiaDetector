using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

//////////

using AutoAssembler.Data;
using AutoAssembler.Drivers;

namespace AutoAssembler
{
    public partial class frmFuncLamp : Form
    {
        public WorkFuncInfo _WorkFuncInfo;
        public bool _bMinusJog = false;


        // 조명 컨트롤 클래스(강성호)
        // ----------
        clsLamp LampComm = new clsLamp();
        // ----------


        public frmFuncLamp()
        {
            InitializeComponent();
        }

        private void frmFuncLamp_Load(object sender, EventArgs e)
        {
            Initialize();
        }

        private void frmFuncLamp_FormClosing(object sender, FormClosingEventArgs e)
        {
            //LampComm.ONLamp(SetChannelIndex);

            for (int i = 0; i < 8; i++)
            {
                LampComm.OFFLamp(i); 
            }
                

            LampComm.Close();
            LampComm = null;
        }

        public void Initialize()
        {
            // 1. Lighting 기능
            // ----------
            for (int i = 0; i < 10; i++)
            {
                cboLightingName.Items.Add(DataManager.LightingSettingInfoList[i].Name);
            }

            cboLightingName.SelectedIndex = _WorkFuncInfo.WFLampChannel;


            // 2. 설정값
            // ----------
            this.txtLightingValue.Text = _WorkFuncInfo.WFLampValue.ToString();



            // 3. 강성호
            // ----------
            LampComm.Open(DeviceManager.LightingComPort);

        }


#region Button ...

        private void btnOk_Click(object sender, EventArgs e)
        {
            _WorkFuncInfo.WFLampChannel = cboLightingName.SelectedIndex;
            _WorkFuncInfo.WFLampValue = int.Parse(txtLightingValue.Text);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            int GetLampValue = int.Parse(txtLightingValue.Text);
            int SetChannelIndex = cboLightingName.SelectedIndex;

            // dshong 조명 test

            LampComm.SetLamp(SetChannelIndex, GetLampValue);

        }

        private void btnLightingHome_Click(object sender, EventArgs e)
        {
            txtLightingValue.Text = "0";
        }

#endregion Button ...


#region JOG ...

        private void timerLighting_Tick(object sender, EventArgs e)
        {
            int GetLampValue = Convert.ToInt32(txtLightingValue.Text);

            if (_bMinusJog == true) // 마이너스
            {
                GetLampValue -= 10;
                if (GetLampValue < 0)
                {
                    GetLampValue = 0;
                }                
            }
            else
            {
                GetLampValue += 10;
                if (GetLampValue > 1023)
                {
                    GetLampValue = 1023;
                }                
            }

            txtLightingValue.Text = GetLampValue.ToString();
        }

        private void btnLightingMinus_MouseDown(object sender, MouseEventArgs e)
        {
            _bMinusJog = true;

            timerLighting.Enabled = true;
        }

        private void btnLightingMinus_MouseUp(object sender, MouseEventArgs e)
        {
            _bMinusJog = true;

            timerLighting.Enabled = false;
        }

        private void btnLightingPlus_MouseDown(object sender, MouseEventArgs e)
        {
            _bMinusJog = false;

            timerLighting.Enabled = true;
        }

        private void btnLightingPlus_MouseUp(object sender, MouseEventArgs e)
        {
            _bMinusJog = false;

            timerLighting.Enabled = false;
        }

#endregion JOG ...

    }
}
