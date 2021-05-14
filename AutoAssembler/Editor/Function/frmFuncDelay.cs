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
    public partial class frmFuncDelay : Form
    {
        public WorkFuncInfo _WorkFuncInfo;

        public frmFuncDelay()
        {
            InitializeComponent();
        }
        
        private void frmFuncDelay_Load(object sender, EventArgs e)
        {
            Initialize();
        }

        public void Initialize()
        {
            // 시간 지정 변수가 없음.
            txtSecond.Text = _WorkFuncInfo.WFDelayTime.ToString();
        }

        private void txtSecond_TextChanged(object sender, EventArgs e)
        {
            _WorkFuncInfo.WFDelayTime = Convert.ToInt32(txtSecond.Text);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            // 저장하기 ...
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }

        private void frmFuncDelay_FormClosing(object sender, FormClosingEventArgs e)
        {
            //MultiMotion.StopAll();
        }
    }
}
