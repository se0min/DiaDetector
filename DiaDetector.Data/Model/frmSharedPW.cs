using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiaDetector.Data
{
    public partial class frmSharedPW : Form
    {
        public frmSharedPW()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DataManager.SET_NET_USERID = txtId.Text;
            DataManager.SET_NET_PWD = txtPw.Text;
        }

        private void frmSharedPW_Load(object sender, EventArgs e)
        {
            txtId.Text = DataManager.SET_NET_USERID;
        }
    }
}
