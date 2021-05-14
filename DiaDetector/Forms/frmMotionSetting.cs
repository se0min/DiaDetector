using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

//////////

using DiaDetector.Data;

namespace DiaDetector
{
    public partial class frmMotionSetting : Form
    {
        private string _FileName = ConfigManager.GetDataFilePath + "setting_motion.dat";

        public frmMotionSetting()
        {
            InitializeComponent();
        }

        private void frmControlSetting_Load(object sender, EventArgs e)
        {
            Initialize();
        }

        public void Initialize()
        {
            // 초기화 ...
            lstMotionList.Columns.Add("제어 축", 80, HorizontalAlignment.Center);
            lstMotionList.Columns.Add("제어 축 명칭", 236, HorizontalAlignment.Center);

            // 제어 축 정보 추가
            foreach (var item in DataManager.AxisWorkList)
            {
                cboAxisFunc.Items.Add(item);
            }

            // 업데이트 ...
            ReDrawList();


            // 첫 항목 선택 ...
            //lstMotionList.
        }

        private void ReDrawList()
        {
            lstMotionList.Items.Clear();

            for (int i = 0; i < 8; i++)
            {
                //ListViewItem lstViewTestItem = new ListViewItem(DataManager.MotionSettingInfoList[i].Name);
                ListViewItem lstViewTestItem = new ListViewItem(i.ToString());

                // 홍동성 ...
                // ----------
                lstViewTestItem.SubItems.Add(DataManager.MotionSettingInfoList[i].Name);
                // ----------


                lstMotionList.Items.Add(lstViewTestItem);
            }

            lstMotionList.EndUpdate();
        }


#region Event ...

        private void lstMotionList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstMotionList.SelectedItems.Count == 1)
            {
                int index = lstMotionList.SelectedIndices[0];
                
                // 1.
                // ----------
                txtName.Text                = DataManager.MotionSettingInfoList[index].Name;
                cboAxisFunc.SelectedIndex   = DataManager.MotionSettingInfoList[index].MotionType;
                txtMaxValue.Text            = DataManager.MotionSettingInfoList[index].MaxValue.ToString();
                txtMinValue.Text            = DataManager.MotionSettingInfoList[index].MinValue.ToString();


                // 2. 
                // ----------
                txtVelocityStart.Text       = DataManager.MotionSettingInfoList[index].Velocity_Start.ToString();
                txtVelocityMax.Text         = DataManager.MotionSettingInfoList[index].Velocity_Max.ToString();
                txtVelocityAcc.Text         = DataManager.MotionSettingInfoList[index].Velocity_Acc.ToString();
                txtVelocityDec.Text         = DataManager.MotionSettingInfoList[index].Velocity_Dec.ToString();


                // 3. PAIX SDK
                // ----------
                cboEmergency.SelectedIndex  = DataManager.MotionSettingInfoList[index].Logic_Emergency;
                cboEnc.SelectedIndex        = DataManager.MotionSettingInfoList[index].Logic_Enc;
                cboEncZ.SelectedIndex       = DataManager.MotionSettingInfoList[index].Logic_EncZ;
                cboEncInput.SelectedIndex   = DataManager.MotionSettingInfoList[index].Logic_Enc_Input;
                cboAlarm.SelectedIndex      = DataManager.MotionSettingInfoList[index].Logic_Alarm;

                cboHomeMode.SelectedIndex   = DataManager.MotionSettingInfoList[index].Logic_HomeMode;
                cboPulseMode.SelectedIndex  = DataManager.MotionSettingInfoList[index].Logic_PulseMode;
                cboNear.SelectedIndex       = DataManager.MotionSettingInfoList[index].Logic_Near;
                cboLimitMinus.SelectedIndex = DataManager.MotionSettingInfoList[index].Logic_Limit_Minus;
                cboLimitPlus.SelectedIndex  = DataManager.MotionSettingInfoList[index].Logic_Limit_Plus;

                txtUnitPerPulse.Text        = DataManager.MotionSettingInfoList[index].Logic_UnitPerPulse.ToString();


                // 4. 
                // ----------
                txtBallScrewLead.Text       = DataManager.MotionSettingInfoList[index].BallScrew_Lead.ToString();


                // 5. 원점 속도 ...
                // ----------
                txtVelocityHome1.Text       = DataManager.MotionSettingInfoList[index].Velocity_Home_1.ToString();
                txtVelocityHome2.Text       = DataManager.MotionSettingInfoList[index].Velocity_Home_2.ToString();
                txtVelocityHome3.Text       = DataManager.MotionSettingInfoList[index].Velocity_Home_3.ToString();
                txtVelocityHomeOffset.Text  = DataManager.MotionSettingInfoList[index].Velocity_Home_Offset.ToString();
                txtHomeOffset.Text          = DataManager.MotionSettingInfoList[index].Home_Offset.ToString();


                // 6. 원점 가감속 ...
                // ----------
                txtVelocityStart_Home.Text = DataManager.MotionSettingInfoList[index].Velocity_Start_Home.ToString();
                txtVelocityMax_Home.Text = DataManager.MotionSettingInfoList[index].Velocity_Max_Home.ToString();
                txtVelocityAcc_Home.Text = DataManager.MotionSettingInfoList[index].Velocity_Acc_Home.ToString();
                txtVelocityDec_Home.Text = DataManager.MotionSettingInfoList[index].Velocity_Dec_Home.ToString();


            }
        }

        private void cboAxisFunc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstMotionList.SelectedItems.Count == 1)
            {
                int index = lstMotionList.SelectedIndices[0];

                DataManager.MotionSettingInfoList[index].MotionType = cboAxisFunc.SelectedIndex;
            }
        }

#endregion Event ...


#region Button ...

        private void btnOk_Click(object sender, EventArgs e)
        {
            DataManager.SaveMotionSettingFiles(_FileName);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DataManager.LoadMotionSettingFiles(_FileName);
        }

#endregion Button ...


#region ComboBox ...

        private void cboEmergency_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstMotionList.SelectedItems.Count == 1)
            {
                int index = lstMotionList.SelectedIndices[0];

                DataManager.MotionSettingInfoList[index].Logic_Emergency = cboEmergency.SelectedIndex;
            }
        }

        private void cboEnc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstMotionList.SelectedItems.Count == 1)
            {
                int index = lstMotionList.SelectedIndices[0];

                DataManager.MotionSettingInfoList[index].Logic_Enc = cboEnc.SelectedIndex;
            }
        }

        private void cboEncZ_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstMotionList.SelectedItems.Count == 1)
            {
                int index = lstMotionList.SelectedIndices[0];

                DataManager.MotionSettingInfoList[index].Logic_EncZ = cboEncZ.SelectedIndex;
            }
        }

        private void cboEncInput_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstMotionList.SelectedItems.Count == 1)
            {
                int index = lstMotionList.SelectedIndices[0];

                DataManager.MotionSettingInfoList[index].Logic_Enc_Input = cboEncInput.SelectedIndex;
            }
        }

        private void cboAlarm_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstMotionList.SelectedItems.Count == 1)
            {
                int index = lstMotionList.SelectedIndices[0];

                DataManager.MotionSettingInfoList[index].Logic_Alarm = cboAlarm.SelectedIndex;
            }
        }

        private void cboHomeMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstMotionList.SelectedItems.Count == 1)
            {
                int index = lstMotionList.SelectedIndices[0];

                DataManager.MotionSettingInfoList[index].Logic_HomeMode = cboHomeMode.SelectedIndex;
            }
        }

        private void cboPulseMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstMotionList.SelectedItems.Count == 1)
            {
                int index = lstMotionList.SelectedIndices[0];

                DataManager.MotionSettingInfoList[index].Logic_PulseMode = cboPulseMode.SelectedIndex;
            }
        }

        private void cboNear_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstMotionList.SelectedItems.Count == 1)
            {
                int index = lstMotionList.SelectedIndices[0];

                DataManager.MotionSettingInfoList[index].Logic_Near = cboNear.SelectedIndex;
            }
        }

        private void cboLimitMinus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstMotionList.SelectedItems.Count == 1)
            {
                int index = lstMotionList.SelectedIndices[0];

                DataManager.MotionSettingInfoList[index].Logic_Limit_Minus = cboLimitMinus.SelectedIndex;
            }
        }

        private void cboLimitPlus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstMotionList.SelectedItems.Count == 1)
            {
                int index = lstMotionList.SelectedIndices[0];

                DataManager.MotionSettingInfoList[index].Logic_Limit_Plus = cboLimitPlus.SelectedIndex;
            }
        }

#endregion ComboBox ...


#region Text ...

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (lstMotionList.SelectedItems.Count == 1)
            {
                int index = lstMotionList.SelectedIndices[0];

                DataManager.MotionSettingInfoList[index].Name = txtName.Text;

                lstMotionList.SelectedItems[0].SubItems[1].Text = txtName.Text;
            }
        }

        private void txtVelocityAcc_TextChanged(object sender, EventArgs e)
        {
            if (lstMotionList.SelectedItems.Count == 1)
            {
                int index = lstMotionList.SelectedIndices[0];

                double dTempValue = 0.0;

                if (double.TryParse(this.txtVelocityAcc.Text, out dTempValue))
                {
                    DataManager.MotionSettingInfoList[index].Velocity_Acc = dTempValue;
                }
                else
                {
                    this.txtVelocityAcc.Text = "0.0";
                }
            }
        }

        private void txtVelocityDec_TextChanged(object sender, EventArgs e)
        {
            if (lstMotionList.SelectedItems.Count == 1)
            {
                int index = lstMotionList.SelectedIndices[0];

                double dTempValue = 0.0;

                if (double.TryParse(this.txtVelocityDec.Text, out dTempValue))
                {
                    DataManager.MotionSettingInfoList[index].Velocity_Dec = dTempValue;
                }
                else
                {
                    this.txtVelocityDec.Text = "0.0";
                }
            }

        }

        private void txtVelocityMax_TextChanged(object sender, EventArgs e)
        {
            if (lstMotionList.SelectedItems.Count == 1)
            {
                int index = lstMotionList.SelectedIndices[0];

                double dTempValue = 0.0;

                if (double.TryParse(this.txtVelocityMax.Text, out dTempValue))
                {
                    DataManager.MotionSettingInfoList[index].Velocity_Max = dTempValue;
                }
                else
                {
                    this.txtVelocityMax.Text = "0.0";
                }
            }
        }

        private void txtUnitPerPulse_TextChanged(object sender, EventArgs e)
        {
            if (lstMotionList.SelectedItems.Count == 1)
            {
                int index = lstMotionList.SelectedIndices[0];

                double dTempValue = 0.0;

                if (double.TryParse(this.txtUnitPerPulse.Text, out dTempValue))
                {
                    DataManager.MotionSettingInfoList[index].Logic_UnitPerPulse = dTempValue;
                }
                else
                {
                    this.txtUnitPerPulse.Text = "0.0";
                }
            }
        }

        private void txtMaxValue_TextChanged(object sender, EventArgs e)
        {
            if (lstMotionList.SelectedItems.Count == 1)
            {
                int index = lstMotionList.SelectedIndices[0];

                double dTempValue = 0.0;

                if (double.TryParse(this.txtMaxValue.Text, out dTempValue))
                {
                    DataManager.MotionSettingInfoList[index].MaxValue = dTempValue;
                }
                else
                {
                    this.txtMaxValue.Text = "0.0";
                }
            }
        }

        private void txtMinValue_TextChanged(object sender, EventArgs e)
        {
            if (lstMotionList.SelectedItems.Count == 1)
            {
                int index = lstMotionList.SelectedIndices[0];

                double dTempValue = 0.0;

                if (double.TryParse(this.txtMinValue.Text, out dTempValue))
                {
                    DataManager.MotionSettingInfoList[index].MinValue = dTempValue;
                }
                else
                {
                    this.txtMinValue.Text = "0.0";
                }
            }
        }

        private void txtBallScrewLead_TextChanged(object sender, EventArgs e)
        {
            if (lstMotionList.SelectedItems.Count == 1)
            {
                int index = lstMotionList.SelectedIndices[0];

                double dTempValue = 0.0;

                if (double.TryParse(this.txtBallScrewLead.Text, out dTempValue))
                {
                    DataManager.MotionSettingInfoList[index].BallScrew_Lead = dTempValue;
                }
                else
                {
                    this.txtBallScrewLead.Text = "0.0";
                }
            }

        }

        private void txtVelocityHome1_TextChanged(object sender, EventArgs e)
        {
            if (lstMotionList.SelectedItems.Count == 1)
            {
                int index = lstMotionList.SelectedIndices[0];

                double dTempValue = 0.0;

                if (double.TryParse(this.txtVelocityHome1.Text, out dTempValue))
                {
                    DataManager.MotionSettingInfoList[index].Velocity_Home_1 = dTempValue;
                }
                else
                {
                    this.txtVelocityHome1.Text = "0.0";
                }
            }
        }

        private void txtVelocityHome2_TextChanged(object sender, EventArgs e)
        {
            if (lstMotionList.SelectedItems.Count == 1)
            {
                int index = lstMotionList.SelectedIndices[0];

                double dTempValue = 0.0;

                if (double.TryParse(this.txtVelocityHome2.Text, out dTempValue))
                {
                    DataManager.MotionSettingInfoList[index].Velocity_Home_2 = dTempValue;
                }
                else
                {
                    this.txtVelocityHome2.Text = "0.0";
                }
            }
        }

        private void txtVelocityHome3_TextChanged(object sender, EventArgs e)
        {
            if (lstMotionList.SelectedItems.Count == 1)
            {
                int index = lstMotionList.SelectedIndices[0];

                double dTempValue = 0.0;

                if (double.TryParse(this.txtVelocityHome3.Text, out dTempValue))
                {
                    DataManager.MotionSettingInfoList[index].Velocity_Home_3 = dTempValue;
                }
                else
                {
                    this.txtVelocityHome3.Text = "0.0";
                }
            }
        }

        private void txtVelocityHomeOffset_TextChanged(object sender, EventArgs e)
        {
            if (lstMotionList.SelectedItems.Count == 1)
            {
                int index = lstMotionList.SelectedIndices[0];

                double dTempValue = 0.0;

                if (double.TryParse(this.txtVelocityHomeOffset.Text, out dTempValue))
                {
                    DataManager.MotionSettingInfoList[index].Velocity_Home_Offset = dTempValue;
                }
                else
                {
                    this.txtVelocityHomeOffset.Text = "0.0";
                }
            }

        }

        private void txtHomeOffset_TextChanged(object sender, EventArgs e)
        {

            if (lstMotionList.SelectedItems.Count == 1)
            {
                int index = lstMotionList.SelectedIndices[0];

                double dTempValue = 0.0;

                if (double.TryParse(this.txtHomeOffset.Text, out dTempValue))
                {
                    DataManager.MotionSettingInfoList[index].Home_Offset = dTempValue;
                }
                else
                {
                    this.txtHomeOffset.Text = "0.0";
                }
            }
        }
        private void txtVelocityStart_TextChanged(object sender, EventArgs e)
        {
            if (lstMotionList.SelectedItems.Count == 1)
            {
                int index = lstMotionList.SelectedIndices[0];

                double dTempValue = 0.0;

                if (double.TryParse(this.txtVelocityStart.Text, out dTempValue))
                {
                    DataManager.MotionSettingInfoList[index].Velocity_Start = dTempValue;
                }
                else
                {
                    this.txtVelocityStart.Text = "0.0";
                }
            }
        }

#endregion Text ...

        private void txtVelocityStart_Home_TextChanged(object sender, EventArgs e)
        {
            if (lstMotionList.SelectedItems.Count == 1)
            {
                int index = lstMotionList.SelectedIndices[0];

                double dTempValue = 0.0;

                if (double.TryParse(this.txtVelocityStart_Home.Text, out dTempValue))
                {
                    DataManager.MotionSettingInfoList[index].Velocity_Start_Home = dTempValue;
                }
                else
                {
                    this.txtVelocityStart_Home.Text = "0.0";
                }
            }
        }

        private void txtVelocityAcc_Home_TextChanged(object sender, EventArgs e)
        {
            if (lstMotionList.SelectedItems.Count == 1)
            {
                int index = lstMotionList.SelectedIndices[0];

                double dTempValue = 0.0;

                if (double.TryParse(this.txtVelocityAcc_Home.Text, out dTempValue))
                {
                    DataManager.MotionSettingInfoList[index].Velocity_Acc_Home = dTempValue;
                }
                else
                {
                    this.txtVelocityAcc_Home.Text = "0.0";
                }
            }
        }

        private void txtVelocityDec_Home_TextChanged(object sender, EventArgs e)
        {
            if (lstMotionList.SelectedItems.Count == 1)
            {
                int index = lstMotionList.SelectedIndices[0];

                double dTempValue = 0.0;

                if (double.TryParse(this.txtVelocityDec_Home.Text, out dTempValue))
                {
                    DataManager.MotionSettingInfoList[index].Velocity_Dec_Home = dTempValue;
                }
                else
                {
                    this.txtVelocityDec_Home.Text = "0.0";
                }
            }
        }

        private void txtVelocityMax_Home_TextChanged(object sender, EventArgs e)
        {
            if (lstMotionList.SelectedItems.Count == 1)
            {
                int index = lstMotionList.SelectedIndices[0];

                double dTempValue = 0.0;

                if (double.TryParse(this.txtVelocityMax_Home.Text, out dTempValue))
                {
                    DataManager.MotionSettingInfoList[index].Velocity_Max_Home = dTempValue;
                }
                else
                {
                    this.txtVelocityMax_Home.Text = "0.0";
                }
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }




    }
}
