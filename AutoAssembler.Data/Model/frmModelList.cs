using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

//////////

// 0826
using System.Collections;
using System.Xml;

using Microsoft.VisualBasic.FileIO;

namespace AutoAssembler.Data
{
    public partial class frmModelList : Form
    {
        public int SelectModelIndex;

        public frmModelList()
        {
            InitializeComponent();
        }



#region 모드 ...

        private bool iDriveMode = false;
        private bool bTestMode = false;

        public void set_i_DriveMode()
        {
            iDriveMode = true;

            ConfigManager.SetXmlDataFilePath = DataManager.SET_NET_FOLDER_I + "Data\\";
            ConfigManager.SetModelFilePath = DataManager.SET_NET_FOLDER_I + "Data\\Model\\";

            DataManager.Connect_I_Drive();            
        }

        public void setTestMode()
        {
            bTestMode = true;
        }

#endregion 모드 ...



        // 0826
        private void frmModelList_Load(object sender, EventArgs e)
        {
            ReadXML();

            Initialize();
        }

        // 0826
        public void ReadXML()
        {
            if (File.Exists(ConfigManager.GetXmlFilePath))
            {
                XmlDocument xmlDocument = new XmlDocument();

                xmlDocument.Load(ConfigManager.GetXmlFilePath);


                // 모션 컨트롤러 2
                // ----------
                XmlElement pSelected = (XmlElement)xmlDocument.DocumentElement.SelectSingleNode("SharedFolder");

                //DataManager.SET_NET_USERID = pSelected.GetAttribute("ID");
                //DataManager.SET_NET_PWD = pSelected.GetAttribute("PASSWORD");
            }
        }

        // 0826
        public void WriteXML()
        {
            if (File.Exists(ConfigManager.GetXmlFilePath))
            {
                XmlDocument xmlDocument = new XmlDocument();

                xmlDocument.Load(ConfigManager.GetXmlFilePath);


                // 모션 컨트롤러 1
                // ----------
                XmlElement pSelected = (XmlElement)xmlDocument.DocumentElement.SelectSingleNode("SharedFolder");

                pSelected.SetAttribute("ID", DataManager.SET_NET_USERID);
                pSelected.SetAttribute("PASSWORD", DataManager.SET_NET_PWD);

                xmlDocument.Save(ConfigManager.GetXmlFilePath);
            }
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Dispose();
        }

        // 0826
        private void Initialize()
        {
            lvModelList.Columns.Add("모델명", 340, HorizontalAlignment.Center);
            JDirList.Columns.Add("모델명", 200, HorizontalAlignment.Center);

            RefreshModelList();
        }

        private void RefreshModelList()
        {
            lvModelList.Items.Clear();

            SelectModelIndex = -1;

            //lvModelList.Columns.Add("모델명", 340, HorizontalAlignment.Center);
            //JDirList.Columns.Add("모델명", 200, HorizontalAlignment.Center);


            DataManager.LoadModelListFiles_NET();

            DataManager.SelectedModelByName(DataManager.SelectedModelName);

            int SelectModelDatIndex = DataManager.GetModelSelectIndex();
            if (SelectModelDatIndex > -1)
            {
                DataManager.SaveTestWorkListPath = ConfigManager.GetModelFilePath + DataManager.GetModelSelectFileName();

                DataManager.SelectedModel = DataManager.ModelDatList[SelectModelDatIndex];
            }
            else
            {
                DataManager.SaveTestWorkListPath = "";
            }


            ModelReDrawList();
        }

        private void ModelReDrawList()
        {
            RefreshCDrv();


            RefreshJDrv();
        }

        private void RefreshCDrv()
        {
            lvModelList.Items.Clear();

            for (int i = 0; i < DataManager.MODELLISTMAX; i++)
            {
                if (DataManager.ModelDatList[i].ModelExistFlag == true)
                {
                    ListViewItem lstViewTestItem = new ListViewItem(DataManager.ModelDatList[i].ModelName);

                    lvModelList.Items.Add(lstViewTestItem);
                }
            }

            lvModelList.EndUpdate();
        }

        private void RefreshJDrv()
        {
            /*
            if (this.bTestMode == true)
                return;
            */

            JDirList.Items.Clear();

            string PathName = DataManager.SET_NET_FOLDER_J + "Data\\Model\\";

            string[] dirs = Directory.GetDirectories(PathName);
            string strName = "";
            foreach (string dir in dirs)
            {
                strName = DataManager.getModelNameByFolder(dir);

                ListViewItem lstViewTestItem = new ListViewItem(strName);

                JDirList.Items.Add(lstViewTestItem);
            }

            JDirList.EndUpdate();
        }


#region Button

        private void btnEditModel_Click(object sender, EventArgs e)
        {
            if (lvModelList.SelectedItems.Count > 0)
            {
                if (MessageBox.Show("선택한 모델을 편집 하시겠습니까?", "모델 편집",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    SelectModelIndex = lvModelList.SelectedIndices[0];
                    
                    
                    DataManager.ModelChangeSelect(SelectModelIndex);
                    

                    DataManager.SelectedModelName = DataManager.SelectedModel.ModelName; // 20160830 => 추가
                    DataManager.SelectedModel = DataManager.ModelDatList[SelectModelIndex];

                    //DataManager.SaveModelListFiles(ConfigManager.GetModelListPath);// 20160830 => 주석


                    // ---------------------------------------------
                    frmModelSetting frmDlg = new frmModelSetting();
                    if (frmDlg.ShowDialog() == DialogResult.OK)
                    {
                        //DataManager.SaveCurrentModel();// 20160830 => 주석                        

                        if (frmDlg.bModelNameChange == true)
                        {
                            RefreshModelList();
                        }
                        else
                        {
                            ModelReDrawList();
                        }                        
                    }
                    // ---------------------------------------------

                    //this.DialogResult = DialogResult.OK;
                    //this.Dispose();
                }
            }


            return;

            
            //if (lvModelList.SelectedItems.Count > 0)
            {
                //int SelTestNumIndex = lvModelList.SelectedIndices[0];
                
                frmModelSetting frmDlg = new frmModelSetting();

                /*
                frmDlg.ModelNameStr = DataManager.ModelDatList[SelTestNumIndex].ModelName;
                frmDlg.ModelImageFileName = DataManager.ModelDatList[SelTestNumIndex].ImageFileName;
                */

                if (frmDlg.ShowDialog() == DialogResult.OK)
                {

                    /*
                    // --------------------------------------------------
                    DataManager.ModelDatList[SelTestNumIndex].ModelName = frmDlg.ModelNameStr;
                    DataManager.ModelDatList[SelTestNumIndex].ImageFileName = frmDlg.ModelImageFileName;
                    // --------------------------------------------------
                    */

                    DataManager.SaveCurrentModel();

                    ModelReDrawList();
                }
            }
            /*
            else
            {
                MessageBox.Show("선택한 모델이 없습니다.");
            }
            */
        }

        // 20160830
        private void btnNewModel_Click(object sender, EventArgs e)
        {
            bool bCreated = false;

            string findModelName = "";

            if (MessageBox.Show("신규 모델을 생성하시겠습니까?", "신규 모델 등록",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                frmNewModel frmDlg = new frmNewModel();
                if (frmDlg.ShowDialog() == DialogResult.OK)
                {
                    string sourcePath = ConfigManager.GetModelFilePath + DataManager.getDefaultModel().ModelName;

                    string destinationPath = ConfigManager.GetModelFilePath + frmDlg.ModelNameStr;

                    FileSystem.CopyDirectory(sourcePath, destinationPath, UIOption.AllDialogs); //using Microsoft.VisualBasic.FileIO;

                    DataManager.LoadModelListFiles_NET();

                    RefreshCDrv();

                    findModelName = frmDlg.ModelNameStr;

                    bCreated = true;
                }
            }


            // --------------------------------------------------
            if (bCreated == true)
            {
                EditByModelName(findModelName);

                /*
                for (int i = 0; i < lvModelList.Items.Count; i++)
			    {
			        if (lvModelList.Items[i].Text == findModelName)
                    {
                        SelectModelIndex = i;


                        DataManager.ModelChangeSelect(SelectModelIndex);
                        DataManager.SelectedModelName = DataManager.SelectedModel.ModelName; // 20160830 => 추가
                        DataManager.SelectedModel = DataManager.ModelDatList[SelectModelIndex];



                        frmModelSetting frmDlg = new frmModelSetting();
                        if (frmDlg.ShowDialog() == DialogResult.OK)
                        {
                            if (frmDlg.bModelNameChange == true)
                            {
                                RefreshModelList();
                            }
                            else
                            {
                                ModelReDrawList();
                            }                            
                        }

                        break;
                    }
			    }
                */
            }
            // --------------------------------------------------

        }

        private void btnSelectModel_Click(object sender, EventArgs e)
        {
            if (lvModelList.SelectedItems.Count > 0)
            {
                if (MessageBox.Show("선택한 모델로 변경하시겠습니까?", "모델 변경", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    SelectModelIndex = lvModelList.SelectedIndices[0];

                    DataManager.ModelChangeSelect(SelectModelIndex);

                    DataManager.SelectedModel = DataManager.ModelDatList[SelectModelIndex];
                    DataManager.SelectedModelName = DataManager.SelectedModel.ModelName;

                    DataManager.SaveModelListFiles();
                    
                    this.DialogResult = DialogResult.OK;
                    this.Dispose();
                }
            }
            else
            {
                MessageBox.Show("선택한 모델이 없습니다.");
            }
        }

        // 0826
        private void btnModelCopy_Click(object sender, EventArgs e)
        {
            string strModelName = "";

            if (lvModelList.SelectedItems.Count > 0)
            {
                if (MessageBox.Show("선택한 모델을 복사하시겠습니까?", "모델 복사",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    SelectModelIndex = lvModelList.SelectedIndices[0];

                    strModelName = DataManager.ModelCopySelect(SelectModelIndex);

                    ModelReDrawList();

                    // --------------------------------------------------
                    if (strModelName != "")
                    {
                        EditByModelName(strModelName);
                    }
                    // --------------------------------------------------
                }
            }
            else
            {
                MessageBox.Show("선택한 모델이 없습니다.");
            }
        }

        private void EditByModelName(string strModelName)
        {
            for (int i = 0; i < lvModelList.Items.Count; i++)
            {
                if (lvModelList.Items[i].Text == strModelName)
                {
                    SelectModelIndex = i;

                    DataManager.ModelChangeSelect(SelectModelIndex);
                    DataManager.SelectedModelName = DataManager.SelectedModel.ModelName; // 20160830 => 추가
                    DataManager.SelectedModel = DataManager.ModelDatList[SelectModelIndex];

                    frmModelSetting frmDlg = new frmModelSetting();
                    if (frmDlg.ShowDialog() == DialogResult.OK)
                    {
                        if (frmDlg.bModelNameChange == true)
                        {
                            RefreshModelList();
                        }
                        else
                        {
                            ModelReDrawList();

                        }

                    }


                    break;
                }
            }
        }

#endregion Button

        // 0826 ...
        private void btnUpload_Click(object sender, EventArgs e)
        {
            if (lvModelList.SelectedItems.Count > 0)
            {
                string strModelName = DataManager.ModelDatList[lvModelList.SelectedIndices[0]].ModelName;

                string sourceFolder = ConfigManager.GetModelFilePath + strModelName;
                string targetFolder = DataManager.SET_NET_FOLDER_J + "Data\\Model\\" + strModelName;


                DataManager.Connect_J_Drive();

                DataManager.Upload(sourceFolder, targetFolder);

                RefreshJDrv();

            }
            else
            {
                MessageBox.Show("선택한 모델이 없습니다.");
            }
        }

        // 0826 ...
        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (JDirList.SelectedItems.Count > 0)
            {
                string strModelName = JDirList.SelectedItems[0].Text;



                string targetFolder = ConfigManager.GetModelFilePath + strModelName;
                string sourceFolder = DataManager.SET_NET_FOLDER_J + "Data\\Model\\" + strModelName;

                DataManager.Connect_J_Drive();

                DataManager.Download(sourceFolder, targetFolder);

                RefreshModelList();
            }
            else
            {
                MessageBox.Show("선택한 모델이 없습니다.");
            }
        }

        private void btnSharedFolder_Click(object sender, EventArgs e)
        {
            bool bCheckedPassword = false;

            frmPassWord frmPassWdDlg;

            frmPassWdDlg = new frmPassWord();
            if (frmPassWdDlg.ShowDialog() == DialogResult.OK)
            {
                bCheckedPassword = true;
            }


            // ----------
            if (bCheckedPassword == true)
            {
                frmSharedPW dlg = new frmSharedPW();

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    WriteXML();
                }
            }

        }

        private void frmModelList_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DataManager.bConnectedNetworkDrive_J == true)
            {
                DataManager.ConnectNetworkDrive(DataManager.SET_NET_DRIVE_J, DataManager.SET_NET_FOLDER_J, DataManager.SET_NET_USERID, DataManager.SET_NET_PWD);

                DataManager.bConnectedNetworkDrive_J = false;
            }

            if (DataManager.bConnectedNetworkDrive_I == true)
            {
                DataManager.ConnectNetworkDrive(DataManager.SET_NET_DRIVE_I, DataManager.SET_NET_FOLDER_I, DataManager.SET_NET_USERID, DataManager.SET_NET_PWD);

                DataManager.bConnectedNetworkDrive_I = false;
            }
        }

        private void btnDeleteModel_Click(object sender, EventArgs e)
        {
            if (lvModelList.SelectedItems.Count > 0)
            {
                string strModelName = DataManager.ModelDatList[lvModelList.SelectedIndices[0]].ModelName;

                string sourceFolder = ConfigManager.GetModelFilePath + strModelName;

                System.IO.Directory.Delete(sourceFolder, true);

                RefreshModelList();
            }
            else
            {
                MessageBox.Show("선택한 모델이 없습니다.");
            }
        }

        private void btnDeleteFolder_Click(object sender, EventArgs e)
        {
            if (JDirList.SelectedItems.Count > 0)
            {
                string strModelName = JDirList.SelectedItems[0].Text;
                
                string sourceFolder = DataManager.SET_NET_FOLDER_J + "Data\\Model\\" + strModelName;

                System.IO.Directory.Delete(sourceFolder, true);

                RefreshJDrv();
            }
            else
            {
                MessageBox.Show("선택한 모델이 없습니다.");
            }
        }


#region 도면 파일 ...

        private void btnDownloadDXF_Click(object sender, EventArgs e)
        {
            if (this.iDriveMode == true)
            {
                MessageBox.Show("사무실에서는 사용할 수 없는 기능입니다.");

                return;
            }


            string sourceFolder = DataManager.SET_NET_FOLDER_J + "Data\\DXF\\";

            string targetFolder = ConfigManager.GetDataFilePath + "DXF\\";

            DataManager.Connect_J_Drive();


            DataManager.DownloadDXF(sourceFolder, targetFolder);

            MessageBox.Show("DXF 파일 다운로드가 완료되었습니다.");
        }

        private void btnUploadDXF_Click(object sender, EventArgs e)
        {
            if (this.iDriveMode == false)
            {
                MessageBox.Show("작업 현장에서는 사용할 수 없는 기능입니다.");

                return;
            }

            string sourceFolder = ConfigManager.GetDataFilePath + "DXF\\";

            string targetFolder = DataManager.SET_NET_FOLDER_J + "Data\\DXF\\";

            DataManager.Connect_J_Drive();

            DataManager.UploadDXF(sourceFolder, targetFolder);

            MessageBox.Show("DXF 파일 업로드가 완료되었습니다.");
        }

#endregion 도면 파일 ...

    }
}
