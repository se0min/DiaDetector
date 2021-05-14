using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiaDetector.Data
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
        public static ArrayList Modelnamefind = new ArrayList();
        private void btnNewModel_Click(object sender, EventArgs e)
        {
            Modelnamefind.Clear();
            for (int i = 0; i < DataManager.MODELLISTMAX; i++)
            {
                Modelnamefind.Add(DataManager.ModelDatList[i].ModelName);
             
            }
            if (txtModelName.Text.Length == 0)
            {
                MessageBox.Show("모델명을 입력하세요.");

                txtModelName.Focus();

                return;
            }
            for (int i = 0; i < Modelnamefind.Count; i++)
            {
                if (txtModelName.Text == Modelnamefind[i].ToString())
                {

                    MessageBox.Show("동일 모델이 존재합니다.");

                    txtModelName.Focus();

                    return;
                }
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
