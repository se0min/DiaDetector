using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AutoAssembler.Data
{
    public partial class frmNewModel : Form
    {
        public string ModelNameStr;


        public frmNewModel()
        {
            InitializeComponent();
        }

        private void frmNewModel_Load(object sender, EventArgs e)
        {
            Initialize();
        }

        private void Initialize()
        {
            txtModelName.Focus();
            
            ModelNameStr = "";
        }


#region Button

        private void btnNewModel_Click(object sender, EventArgs e)
        {
            if (txtModelName.Text.Length == 0)
            {
                MessageBox.Show("모델명을 입력하세요.");

                txtModelName.Focus();

                return;
            }

            ModelNameStr = txtModelName.Text;

            this.DialogResult = DialogResult.OK;
            this.Dispose();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Dispose();
        }

#endregion Button

    }
}
