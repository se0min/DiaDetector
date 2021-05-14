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
    public partial class frmDIOSetting : Form
    {
        public DIOSettingInfo SelectedDIOSettingInfo;

        public frmDIOSetting()
        {
            InitializeComponent();
        }

        private void frmDIOSetting_Load(object sender, EventArgs e)
        {
            Initialize();
        }

        public void Initialize()
        {
            // 초기화 ...
            lstOutputView.Columns.Add("접점 번호", 120, HorizontalAlignment.Center);
            lstOutputView.Columns.Add("이름", 306, HorizontalAlignment.Center);

            lstInputView.Columns.Add("접점 번호", 120, HorizontalAlignment.Center);
            lstInputView.Columns.Add("이름", 306, HorizontalAlignment.Center);

            // 업데이트 ...
            InputListReDrawList();
            OutputListReDrawList();
        }

        private void InputListReDrawList()
        {            
            lstInputView.Items.Clear();

            for (int i = 0; i < 32; i++)
            {
                ListViewItem lstViewTestItem = new ListViewItem(i.ToString());

                lstViewTestItem.SubItems.Add(DataManager.DIOSettingInfoList[i].Name);

                lstInputView.Items.Add(lstViewTestItem);
            }

            lstInputView.EndUpdate();
        }

        private void OutputListReDrawList()
        {
            lstOutputView.Items.Clear();

            for (int i = 32; i < 64; i++)
            {
                ListViewItem lstViewTestItem = new ListViewItem((i-32).ToString());

                lstViewTestItem.SubItems.Add(DataManager.DIOSettingInfoList[i].Name);

                lstOutputView.Items.Add(lstViewTestItem);
            }

            lstOutputView.EndUpdate();
        }


#region Data 관리 ...

        private void lstInputView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstInputView.SelectedItems.Count == 1)
            {
                txtInputName.Text = lstInputView.SelectedItems[0].SubItems[1].Text;
            }
        }

        private void txtInputName_TextChanged(object sender, EventArgs e)
        {
            if (lstInputView.SelectedItems.Count == 1)
            {
                int index = lstInputView.SelectedIndices[0];

                DataManager.DIOSettingInfoList[index].Name = txtInputName.Text;

                lstInputView.SelectedItems[0].SubItems[1].Text = DataManager.DIOSettingInfoList[index].Name;
            }
        }

        private void lstOutputView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstOutputView.SelectedItems.Count == 1)
            {
                txtOutputName.Text = lstOutputView.SelectedItems[0].SubItems[1].Text;
            }
        }

        private void txtOutputName_TextChanged(object sender, EventArgs e)
        {
            if (lstOutputView.SelectedItems.Count == 1)
            {
                int index = lstOutputView.SelectedIndices[0] + 32;


                DataManager.DIOSettingInfoList[index].Name = txtOutputName.Text;

                lstOutputView.SelectedItems[0].SubItems[1].Text = DataManager.DIOSettingInfoList[index].Name;
            }
        }

#endregion Data 관리 ...



#region Button ...

        private void btnOk_Click(object sender, EventArgs e)
        {
            string filename = ConfigManager.GetDataFilePath + "setting_dio.dat";

            DataManager.SaveDIOSettingFiles(filename);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }

        private void btnOnTest_Click(object sender, EventArgs e)
        {

        }

        private void btnOffTest_Click(object sender, EventArgs e)
        {

        }

#endregion Button ...




    }
}
