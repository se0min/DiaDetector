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

namespace AutoAssembler
{
    public partial class frmFuncDIO : Form
    {
        public WorkFuncInfo _WorkFuncInfo;

        public frmFuncDIO()
        {
            InitializeComponent();
        }

        private void frmFuncDIO_Load(object sender, EventArgs e)
        {
            Initialize();
        }

        public void Initialize()
        {
            // 1. Digital I/O 동작 기능
            // ----------
            for (int i = 64; i < 128; i++)
            {
                cboDIOFunc.Items.Add(DataManager.DIOSettingInfoList[i].Name);
            }

            cboDIOFunc.SelectedIndex = _WorkFuncInfo.WFDIOPortNum;


            // 2. ON/OFF
            // ----------
            if (_WorkFuncInfo.WFDioOnOff)
                cboOnOff.SelectedIndex = 1;
            else
                cboOnOff.SelectedIndex = 0;
        }


#region Button ...

        private void cboDIOFunc_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (cboDIOFunc.SelectedIndex > -1)
            {
                _WorkFuncInfo.WFDIOPortNum = cboDIOFunc.SelectedIndex;
            }
        }

        private void cboOnOff_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboOnOff.SelectedIndex == 0)
                _WorkFuncInfo.WFDioOnOff = false;
            else
                _WorkFuncInfo.WFDioOnOff = true;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }

#endregion Button ...

    }
}
