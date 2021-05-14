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
    public partial class frmLightingSetting : Form
    {
        public LightingSettingInfo SelectedLightingSettingInfo;

        private string _FileName = ConfigManager.GetDataFilePath + "setting_lighting.dat";

        public frmLightingSetting()
        {
            InitializeComponent();
        }

        private void frmLightingSetting_Load(object sender, EventArgs e)
        {
            Initialize();
        }

        public void Initialize()
        {
            // 초기화 ...
            lstLightingList.Columns.Add("조명 이름", 219, HorizontalAlignment.Center);


            // 조명 정보 추가
            /*
            foreach (var item in DataManager.AxisWorkList)
            {
                //cboAxisFunc.Items.Add(item);
            }
            */


            // 업데이트 ...
            ReDrawList();


            // 첫 항목 선택 ...
            //lstLightingList.
        }

        private void ReDrawList()
        {
            lstLightingList.Items.Clear();

            for (int i = 0; i < 16; i++)
            {
                ListViewItem lstViewTestItem = new ListViewItem(DataManager.LightingSettingInfoList[i].Name);


                // 홍동성 ...
                // ----------
                //lstViewTestItem.Tag = DataManager.LightingSettingInfoList[i];

                //lstViewTestItem.SubItems.Add(DataManager.DIOSettingInfoList[i].Name);
                // ----------


                lstLightingList.Items.Add(lstViewTestItem);
            }

            lstLightingList.EndUpdate();
        }


#region Button ...

        private void btnOk_Click(object sender, EventArgs e)
        {
            DataManager.SaveLightingSettingFiles(_FileName);         // 저장 ...
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DataManager.LoadLightingSettingFiles(_FileName);    // 원상 복구 ...
        }

#endregion Button ...


#region Event ...

        private void lstLightingList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstLightingList.SelectedItems.Count == 1)
            {
                int index = lstLightingList.SelectedIndices[0];

                txtName.Text                = DataManager.LightingSettingInfoList[index].Name;
                txtMaxValue.Text            = DataManager.LightingSettingInfoList[index].dMaxValue.ToString();
                txtMinValue.Text            = DataManager.LightingSettingInfoList[index].dMinValue.ToString();
                cboChannel.SelectedIndex    = (DataManager.LightingSettingInfoList[index].Channel - 1);
            }
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (lstLightingList.SelectedItems.Count == 1)
            {
                int index = lstLightingList.SelectedIndices[0];

                // 홍동성
                // ----------
                DataManager.LightingSettingInfoList[index].Name = txtName.Text;

                lstLightingList.SelectedItems[0].Text = txtName.Text;
                // ----------
            }
        }

        // Sub ...

        private void txtMaxValue_TextChanged(object sender, EventArgs e)
        {
            if (lstLightingList.SelectedItems.Count == 1)
            {
                int index = lstLightingList.SelectedIndices[0];

                DataManager.LightingSettingInfoList[index].dMaxValue = Convert.ToDouble(txtMaxValue.Text);
            }
        }

        private void txtMinValue_TextChanged(object sender, EventArgs e)
        {
            if (lstLightingList.SelectedItems.Count == 1)
            {
                int index = lstLightingList.SelectedIndices[0];

                DataManager.LightingSettingInfoList[index].dMinValue = Convert.ToDouble(txtMinValue.Text);
            }
        }


        private void cboChannel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstLightingList.SelectedItems.Count == 1)
            {
                int index = lstLightingList.SelectedIndices[0];

                DataManager.LightingSettingInfoList[index].Channel = cboChannel.SelectedIndex + 1;
            }
        
        }

#endregion Event ...


    }
}
