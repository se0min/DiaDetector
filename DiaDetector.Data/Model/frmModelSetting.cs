using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

//using DXFImportReco;

namespace DiaDetector.Data
{


    public partial class frmModelSetting : Form
    {
        public bool bModelNameChange = false;



        public frmModelSetting()
        {
            InitializeComponent();
        }

        private void frmModelSetting_Load(object sender, EventArgs e)
        {
            Initialize();
        }

        private void Initialize()
        {
            ld();
            combo();
        }


#region Button

        private void BtnModelSave_Click(object sender, EventArgs e)
        {
            SaveData();

            this.DialogResult = DialogResult.OK;
        }

        private void BtnModelDelete_Click(object sender, EventArgs e)
        {
            // ...
        }

        private void BtnLoadImage_Click(object sender, EventArgs e)
        {
            string NewFileName = string.Empty;
            string filters = "*.gif,*.jpg,*.png,*.bmp";

            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "All Images (" + filters + ")|" + filters.Replace(",", ";") + "";
            openFileDialog1.Multiselect = false;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string FullPathName = openFileDialog1.FileName;
                string FileName = openFileDialog1.SafeFileName;
                //string PathName = FullPathName.Substring(0, (FullPathName.Length - FullPathName.Length));

                if (File.Exists(openFileDialog1.FileName))
                {
                    // 년월일시분초
                    //string strText = String.Format("{0:yyMMddhhmmss}", DateTime.Now);

                    NewFileName = ConfigManager.GetModelFilePath + DataManager.SelectedModel.ModelName + "\\" + FileName;

                    try
                    {
                        File.Copy(FullPathName, NewFileName);

                        // Path 정보는 저장할 필요가 없다.
                        DataManager.SelectedModel.ImageFileName = FileName;

                        ModelPictureBox.Image = System.Drawing.Image.FromFile(NewFileName);
                    }
                    catch (Exception)
                    {                        
                        //throw;

                        MessageBox.Show("이미지 불러오기에 실패했습니다.");
                    }
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;

            this.Dispose();
        }

#endregion Button


        private void SaveData()
        {
            double dTempValue = 0.0;

            int TempValue = 0;


            // 폴더명이 변경 되었으면 ...
            // --------------------------------------------------


            try
            {
                if (txtModelName.Text != DataManager.SelectedModel.ModelName)
                {

                    string sourcePath = ConfigManager.GetModelFilePath + DataManager.SelectedModel.ModelName;
                    string destinationPath = ConfigManager.GetModelFilePath + txtModelName.Text;

                    System.IO.Directory.Move(sourcePath, destinationPath);


                    // ----------
                    DataManager.SelectedModel.ModelName = txtModelName.Text;

                    DataManager.SelectedModel.ndblOutDiameter = Convert.ToDouble(txtFie.Text);
                    DataManager.SelectedModel.Camera1Moving = Convert.ToDouble(CameraMoving1.Text);
                    DataManager.SelectedModel.Camera2Moving = Convert.ToDouble(CameraMoving2.Text);
                    DataManager.SelectedModel.txtSTValueA = Convert.ToDouble(txtSTValueA.Text);
                    DataManager.SelectedModel.txtSTValueB = Convert.ToDouble(txtSTValueB.Text);

                    DataManager.SelectedModel.OutPie = Convert.ToDouble(txtWRValue.Text);
                    DataManager.SelectedModel.InPie = Convert.ToDouble(txtCapsule.Text);
                    DataManager.SelectedModel.NamePie = Convert.ToDouble(txtRolling70Rate.Text);

                    DataManager.SelectedModel.OutPie = Convert.ToDouble(txtCapsule.Text);
                    DataManager.SelectedModel.NamePie1 = comboBox1.Text;
                    DataManager.SelectedModel.sField01 = comboBox2.Text;
                    DataManager.ModelDatList[DataManager.SelectModelIndex].ModelName = txtModelName.Text;
                    // ----------


                    bModelNameChange = true;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("모델 업데이트에 실패했습니다. 다시 시도해 주세요.");

                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;

                return;
            }
            

            // --------------------------------------------------

           


            // 기본 정보 ...
            // --------------------------------------------------
            if (txtModelName.Text.Length == 0)
            {
                MessageBox.Show("모델 이름을 입력하세요.");

                txtModelName.Focus();

                return;
            }

            // 모델 이름
            DataManager.SelectedModel.ModelName = txtModelName.Text;
            DataManager.SelectedModel.ndblOutDiameter = Convert.ToDouble(txtFie.Text);
            DataManager.SelectedModel.Camera1Moving = Convert.ToDouble(CameraMoving1.Text);
            DataManager.SelectedModel.Camera2Moving = Convert.ToDouble(CameraMoving2.Text);
            DataManager.SelectedModel.txtSTValueA = Convert.ToDouble(txtSTValueA.Text);
            DataManager.SelectedModel.txtSTValueB = Convert.ToDouble(txtSTValueB.Text);
            DataManager.SelectedModel.OutPie = Convert.ToDouble(txtCapsule.Text);
            DataManager.SelectedModel.InPie = Convert.ToDouble(txtWRValue.Text);
            DataManager.SelectedModel.NamePie1 = txtRolling70Rate.Text;
            // 자유장
            if (double.TryParse(this.txtFie.Text, out dTempValue))
            {
                DataManager.SelectedModel.dFLValue = dTempValue;
            }


            DataManager.SaveModelListFiles();
            // --------------------------------------------------
        }


        private void btLoadDXF_F_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "CAD Files|*.dxf";
            openFileDialog1.Title = "Select a CAD File";

            if (openFileDialog1.ShowDialog(this) != DialogResult.OK) return;
            if (openFileDialog1.FileName != null)
            {
                //LoadDXFFilesA = openFileDialog1.FileName;
                //LoadDXFDataA(LoadDXFFilesA);
            }
        }
        
        public void ld()
        {
            txtModelName.Text = DataManager.SelectedModel.ModelName ;
            txtFie.Text = DataManager.SelectedModel.ndblOutDiameter.ToString();
            CameraMoving1.Text = DataManager.SelectedModel.Camera1Moving.ToString();
            CameraMoving2.Text = DataManager.SelectedModel.Camera2Moving.ToString();
            txtSTValueA.Text = DataManager.SelectedModel.txtSTValueA.ToString();
            txtSTValueB.Text = DataManager.SelectedModel.txtSTValueB.ToString();
            txtCapsule.Text = DataManager.SelectedModel.OutPie.ToString();
            txtWRValue.Text = DataManager.SelectedModel.InPie.ToString();
            txtRolling70Rate.Text = DataManager.SelectedModel.NamePie1.ToString();

        }

        public void combo()
        {
            comboBox1.Items.Add("          A");
            comboBox1.Items.Add("          B");
            comboBox1.Items.Add("          C");
            comboBox2.Items.Add("         NONE");
            comboBox2.Items.Add("     Twist Type A");
            comboBox2.Items.Add("     Twist Type B");  
        }


    }
}
