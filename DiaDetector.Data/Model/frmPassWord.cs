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
    public partial class frmPassWord : Form
    {

        // 에디터 진입 전에 비밀번호를 물어 본다.


        public frmPassWord()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txtInputPassWd.Text.Length > 0)
            {
                if (txtInputPassWd.Text == "123456")
                {
                    this.DialogResult = DialogResult.OK;
                    this.Dispose();
                }
                else
                {
                    MessageBox.Show("비밀번호가 다릅니다.");
                    this.DialogResult = DialogResult.Cancel;
                    this.Dispose();
                }
            }
            else
            {
                MessageBox.Show("입력한 정보가 없습니다.");
            }
        }

        private void txtInputPassWd_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (int)Keys.Enter)
            {
                btnOK_Click(this, new EventArgs());
            }
        }
    }
}
