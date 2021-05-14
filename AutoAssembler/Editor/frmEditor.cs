using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

//////////

using System.IO;
using System.Collections;
using System.Threading;
using System.Net;

//////////

using AutoAssembler.Data;
//using AutoAssembler.Drivers;

namespace AutoAssembler
{
    public partial class frmEditor : Form
    {
        private WorkFuncInfo _SelectedWorkFuncInfo;
        bool bCheckedPassword = false;


#region 강성호 ...

        public const int SETTYPE_NONE = 700;
        public const int SETTYPE_FIX_FRONT = 710;
        public const int SETTYPE_FIX_BACK = 720;
        public const int SETTYPE_MOVE_FRONT = 730;
        public const int SETTYPE_MOVE_BACK = 740;
        public const int SETTYPE_MOVE_TOP = 750;

        public string BaseCurPath;
        public string BaseCurSubPath;


#endregion 강성호 ...


        public frmEditor()
        {
            InitializeComponent();
        }

        private void frmEditor_Load(object sender, EventArgs e)
        {
            Initialize();

            bCheckedPassword = false;


            // 강성홍 ...
            // ----------
            BaseCurPath = @"C:\_Testbed\SaveResult\";            
            BaseCurSubPath = BaseCurPath + "2016_06_07_10_10_10" + @"\";
        }


        public void Initialize()
        {
            // 작업 목록 초기화
            // --------------------------------------------------
            lstWorkList.Columns.Add("작업 목록", 314, HorizontalAlignment.Center);

            for (int i = 0; i < DataManager.WorkNameList.Length; i++)
            {
                ListViewItem lstViewTestItem = new ListViewItem(DataManager.WorkNameList[i]);

                lstWorkList.Items.Add(lstViewTestItem);
            }

            lstWorkList.EndUpdate();

            lstAddList.Columns.Add("등록된 작업 목록", 270, HorizontalAlignment.Center);
            lstAddList.Columns.Add("편집 유무", 100, HorizontalAlignment.Center);

            lstAddList.EndUpdate();

            ReDrawTestProcList(-1);   // 홍동성 test
        }

#region Button

        private void btnAddWork_Click(object sender, EventArgs e)
        {
            if (lstWorkList.SelectedItems.Count != 1)
                return;


            // --------------------------------------------------

            if (bCheckedPassword == false)
            {
                switch (lstWorkList.SelectedIndices[0])
                {
                    case 19:
                    case 20:
                    case 21:
                    case 22:
                    case 23:
                        break;
                    default:
                        {
                            frmPassWord frmPassWdDlg;

                            frmPassWdDlg = new frmPassWord();
                            if (frmPassWdDlg.ShowDialog() == DialogResult.OK)
                            {
                                bCheckedPassword = true;
                            }
                            else
                            {
                                return;
                            }
                        }
                        break;
                }
            }

            // --------------------------------------------------


            string strWorkName = DataManager.WorkNameList[lstWorkList.SelectedIndices[0]];

            DataManager.AddTestProcList(strWorkName, (int)lstWorkList.SelectedIndices[0]);
            
            ReDrawTestProcList(-1);
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            frmPassWord frmPassWdDlg;


            if (bCheckedPassword == false)
            {
                frmPassWdDlg = new frmPassWord();
                if (frmPassWdDlg.ShowDialog() == DialogResult.OK)
                {
                    bCheckedPassword = true;
                }
                else
                {
                    return;
                }
            }

            int tempSelIndex = DataManager.MoveUpTestProcList((int)lstAddList.SelectedIndices[0]);

            ReDrawTestProcList(tempSelIndex);
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            frmPassWord frmPassWdDlg;

            if (bCheckedPassword == false)
            {
                frmPassWdDlg = new frmPassWord();
                if (frmPassWdDlg.ShowDialog() == DialogResult.OK)
                {
                    bCheckedPassword = true;
                }
                else
                {
                    return;
                }
            }

            int tempSelIndex = DataManager.MoveDownTestProcList((int)lstAddList.SelectedIndices[0]);

            ReDrawTestProcList(tempSelIndex);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            frmPassWord frmPassWdDlg;

            if (bCheckedPassword == false)
            {


                frmPassWdDlg = new frmPassWord();
                if (frmPassWdDlg.ShowDialog() == DialogResult.OK)
                {
                    bCheckedPassword = true;
                }
                else
                {
                    return;
                }
            }

            DataManager.DelTestProcList((int)lstAddList.SelectedIndices[0]);
            

            ReDrawTestProcList(-1);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            // 위쪽 4개의 추가/삭제/위로/아래로 함수에 저장 로직이 포함되어 있다. => 주석 처리 후 아래 코드로 변경 ...
            DataManager.SaveWorkListFiles(DataManager.SaveTestWorkListPath);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // MainFrame 쪽에 코드가 있다.
            ReDrawTestProcList(-1);
        }

        private void lstAddList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            EditStep();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            EditStep();
        }

        private void EditStep()
        {
            if (lstAddList.SelectedItems.Count != 1)
            {
                return;

            }

            if (_SelectedWorkFuncInfo.TestProcExistFlag == false)
            {
                return;
            }

            switch (DataManager.TestProcList[lstAddList.SelectedIndices[0]].WFType)
            {
                case 0: // INDEX(고정축) 회전(R) 기능
                case 1: // INDEX(이동축) 회전(R) 기능
                    {
                        frmFuncIndex dlg = new frmFuncIndex();

                        dlg._WorkFuncInfo = DataManager.TestProcList[lstAddList.SelectedIndices[0]];

                        if (dlg.ShowDialog() == DialogResult.OK)
                        {
                            DataManager.TestProcList[lstAddList.SelectedIndices[0]] = dlg._WorkFuncInfo;

                            ReDrawTestProcList(-1);
                        }
                    }
                    break;

                case 2: // INDEX(고정축) Rolling 기능                
                    {
                        frmFuncRollingUI dlg = new frmFuncRollingUI();

                        dlg._WorkFuncInfo = DataManager.TestProcList[lstAddList.SelectedIndices[0]];

                        if (dlg.ShowDialog() == DialogResult.OK)
                        {
                            DataManager.TestProcList[lstAddList.SelectedIndices[0]] = dlg._WorkFuncInfo;

                            ReDrawTestProcList(-1);

                            DataManager.SaveCurrentModel();
                        }
                    }
                    break;

                case 3: // 개별 축 이동
                    {
                        frmFuncAxisMove dlg = new frmFuncAxisMove();

                        dlg._WorkFuncInfo = DataManager.TestProcList[lstAddList.SelectedIndices[0]];

                        if (dlg.ShowDialog() == DialogResult.OK)
                        {
                            DataManager.TestProcList[lstAddList.SelectedIndices[0]] = dlg._WorkFuncInfo;

                            ReDrawTestProcList(-1);
                        }
                    }
                    break;

                case 8: // 후방 카메라 이동(Z) 기능
                    {
                        frmFuncBackCam dlg = new frmFuncBackCam();

                        dlg._WorkFuncInfo = DataManager.TestProcList[lstAddList.SelectedIndices[0]];

                        if (dlg.ShowDialog() == DialogResult.OK)
                        {
                            DataManager.TestProcList[lstAddList.SelectedIndices[0]] = dlg._WorkFuncInfo;

                            ReDrawTestProcList(-1);
                        }
                    }
                    break;

                case 12:
                    {
                        frmFuncVBlock dlg = new frmFuncVBlock();

                        dlg._WorkFuncInfo = DataManager.TestProcList[lstAddList.SelectedIndices[0]];

                        if (dlg.ShowDialog() == DialogResult.OK)
                        {
                            DataManager.TestProcList[lstAddList.SelectedIndices[0]] = dlg._WorkFuncInfo;

                            ReDrawTestProcList(-1);
                        }
                    }
                    break;

                case 4: // INDEX(이동축) X축 압축 이동 기능
                    {
                        frmFuncGantry dlg = new frmFuncGantry();

                        dlg._WorkFuncInfo = DataManager.TestProcList[lstAddList.SelectedIndices[0]];

                        if (dlg.ShowDialog() == DialogResult.OK)
                        {
                            DataManager.TestProcList[lstAddList.SelectedIndices[0]] = dlg._WorkFuncInfo;

                            ReDrawTestProcList(-1);
                        }                        
                    }
                    break;


                case 5: // 전면(A) 카메라 이동(XYZ) 기능
                case 6: // 전면(B) 카메라 이동(XYZ) 기능                
                case 7: // 상방 카메라 이동(XYZ) 기능
                    {
                        frmFuncCamUnitMove dlg = new frmFuncCamUnitMove();

                        dlg._WorkFuncInfo = DataManager.TestProcList[lstAddList.SelectedIndices[0]];

                        if (dlg.ShowDialog() == DialogResult.OK)
                        {
                            DataManager.TestProcList[lstAddList.SelectedIndices[0]] = dlg._WorkFuncInfo;

                            ReDrawTestProcList(-1);
                        }
                    }
                    break;                   


                case 9: // 카메라 조정(Zoom, Focus) 기능
                    {
                        MessageBox.Show("카메라 조정(Zoom, Focus) 기능");
                    }
                    break;

                case 10: // 조명 조정(밝기) 기능
                    {
                        frmFuncLamp dlg = new frmFuncLamp();

                        dlg._WorkFuncInfo = DataManager.TestProcList[lstAddList.SelectedIndices[0]];

                        if (dlg.ShowDialog() == DialogResult.OK)
                        {
                            DataManager.TestProcList[lstAddList.SelectedIndices[0]] = dlg._WorkFuncInfo;

                            ReDrawTestProcList(-1);
                        }
                    }
                    break;

                case 11: // 영상 인식 INDEX 위치 맞춤 기능
                    {
                        frmReco dlg = new frmReco();
                        //frmCalibration dlg = new frmCalibration();
                        
                        dlg._WorkFuncInfo = DataManager.TestProcList[lstAddList.SelectedIndices[0]];
                        dlg.SetTypeIndex = SETTYPE_NONE;

                        // 강성호 ...
                        // ----------
                        dlg.CfgSaveFolderPath = ConfigManager.GetModelFilePath + DataManager.GetModelSelectFileNameFolder();
                        dlg.CfgSaveHeadFileName = DataManager.TestProcList[lstAddList.SelectedIndices[0]].WFRecoDatHeadFileName;

                        /*
                        dlg.RecoSetData.RecoCamIndex = 0;
                        dlg.RecoSetData.Lamp_Index = 0;
                        dlg.RecoSetData.Rotation_Index = 0;
                        dlg.RecoSetData.Motion_Index = 0;
                        */
                        // ----------


                        if (dlg.ShowDialog() == DialogResult.OK)
                        {
                            DataManager.TestProcList[lstAddList.SelectedIndices[0]] = dlg._WorkFuncInfo;
                            if (DataManager.TestProcList[lstAddList.SelectedIndices[0]].WFRecoDatHeadFileName == null)
                            {
                                DataManager.TestProcList[lstAddList.SelectedIndices[0]].WFRecoDatHeadFileName = dlg.CfgSaveHeadFileName;
                            }
                            else
                            {
                                if (DataManager.TestProcList[lstAddList.SelectedIndices[0]].WFRecoDatHeadFileName.Length == 0)
                                {
                                    DataManager.TestProcList[lstAddList.SelectedIndices[0]].WFRecoDatHeadFileName = dlg.CfgSaveHeadFileName;
                                }
                            }

                            ReDrawTestProcList(-1);
                        }
                    }
                    break;

                case 19: // @"고정축 INDEX 전방, 영상 인식 기능", // 19 => 20160816
                    {
                        frmReco dlg = new frmReco();
                        //frmCalibration dlg = new frmCalibration();
                        
                        dlg._WorkFuncInfo = DataManager.TestProcList[lstAddList.SelectedIndices[0]];
                        dlg.SetTypeIndex = SETTYPE_FIX_FRONT;

                        // 강성호 ...
                        // ----------
                        dlg.CfgSaveFolderPath = ConfigManager.GetModelFilePath + DataManager.GetModelSelectFileNameFolder();
                        dlg.CfgSaveHeadFileName = DataManager.TestProcList[lstAddList.SelectedIndices[0]].WFRecoDatHeadFileName;

                        /*
                        dlg.RecoSetData.RecoCamIndex = 0;
                        dlg.RecoSetData.Lamp_Index = 0;
                        dlg.RecoSetData.Rotation_Index = 0;
                        dlg.RecoSetData.Motion_Index = 0;
                        */
                        // ----------


                        if (dlg.ShowDialog() == DialogResult.OK)
                        {
                            DataManager.TestProcList[lstAddList.SelectedIndices[0]] = dlg._WorkFuncInfo;
                            if (DataManager.TestProcList[lstAddList.SelectedIndices[0]].WFRecoDatHeadFileName == null)
                            {
                                DataManager.TestProcList[lstAddList.SelectedIndices[0]].WFRecoDatHeadFileName = dlg.CfgSaveHeadFileName;
                            }
                            else
                            {
                                if (DataManager.TestProcList[lstAddList.SelectedIndices[0]].WFRecoDatHeadFileName.Length == 0)
                                {
                                    DataManager.TestProcList[lstAddList.SelectedIndices[0]].WFRecoDatHeadFileName = dlg.CfgSaveHeadFileName;
                                }
                            }

                            ReDrawTestProcList(-1);
                        }
                    }
                    break;

                case 20: // @"고정축 INDEX 후방, 영상 인식 기능", 
                    {
                        frmReco dlg = new frmReco();
                        //frmCalibration dlg = new frmCalibration();
                        
                        dlg._WorkFuncInfo = DataManager.TestProcList[lstAddList.SelectedIndices[0]];
                        dlg.SetTypeIndex = SETTYPE_FIX_BACK;

                        // 강성호 ...
                        // ----------
                        dlg.CfgSaveFolderPath = ConfigManager.GetModelFilePath + DataManager.GetModelSelectFileNameFolder();
                        dlg.CfgSaveHeadFileName = DataManager.TestProcList[lstAddList.SelectedIndices[0]].WFRecoDatHeadFileName;

                        /*
                        dlg.RecoSetData.RecoCamIndex = 0;
                        dlg.RecoSetData.Lamp_Index = 0;
                        dlg.RecoSetData.Rotation_Index = 0;
                        dlg.RecoSetData.Motion_Index = 0;
                        */
                        // ----------


                        if (dlg.ShowDialog() == DialogResult.OK)
                        {
                            DataManager.TestProcList[lstAddList.SelectedIndices[0]] = dlg._WorkFuncInfo;
                            if (DataManager.TestProcList[lstAddList.SelectedIndices[0]].WFRecoDatHeadFileName == null)
                            {
                                DataManager.TestProcList[lstAddList.SelectedIndices[0]].WFRecoDatHeadFileName = dlg.CfgSaveHeadFileName;
                            }
                            else
                            {
                                if (DataManager.TestProcList[lstAddList.SelectedIndices[0]].WFRecoDatHeadFileName.Length == 0)
                                {
                                    DataManager.TestProcList[lstAddList.SelectedIndices[0]].WFRecoDatHeadFileName = dlg.CfgSaveHeadFileName;
                                }
                            }

                            ReDrawTestProcList(-1);
                        }
                    }
                    break;

                case 21: // @"이동축 INDEX 전방, 영상 인식 기능", 
                    {
                        frmReco dlg = new frmReco();
                        //frmCalibration dlg = new frmCalibration();
                        
                        dlg._WorkFuncInfo = DataManager.TestProcList[lstAddList.SelectedIndices[0]];
                        dlg.SetTypeIndex = SETTYPE_MOVE_FRONT;

                        // 강성호 ...
                        // ----------
                        dlg.CfgSaveFolderPath = ConfigManager.GetModelFilePath + DataManager.GetModelSelectFileNameFolder();
                        dlg.CfgSaveHeadFileName = DataManager.TestProcList[lstAddList.SelectedIndices[0]].WFRecoDatHeadFileName;

                        /*
                        dlg.RecoSetData.RecoCamIndex = 0;
                        dlg.RecoSetData.Lamp_Index = 0;
                        dlg.RecoSetData.Rotation_Index = 0;
                        dlg.RecoSetData.Motion_Index = 0;
                        */
                        // ----------


                        if (dlg.ShowDialog() == DialogResult.OK)
                        {
                            DataManager.TestProcList[lstAddList.SelectedIndices[0]] = dlg._WorkFuncInfo;
                            if (DataManager.TestProcList[lstAddList.SelectedIndices[0]].WFRecoDatHeadFileName == null)
                            {
                                DataManager.TestProcList[lstAddList.SelectedIndices[0]].WFRecoDatHeadFileName = dlg.CfgSaveHeadFileName;
                            }
                            else
                            {
                                if (DataManager.TestProcList[lstAddList.SelectedIndices[0]].WFRecoDatHeadFileName.Length == 0)
                                {
                                    DataManager.TestProcList[lstAddList.SelectedIndices[0]].WFRecoDatHeadFileName = dlg.CfgSaveHeadFileName;
                                }
                            }

                            ReDrawTestProcList(-1);
                        }
                    }
                    break;

                case 22: // @"이동축 INDEX 후방, 영상 인식 기능", 
                    {
                        frmReco dlg = new frmReco();
                        //frmCalibration dlg = new frmCalibration();
                        
                        dlg._WorkFuncInfo = DataManager.TestProcList[lstAddList.SelectedIndices[0]];
                        dlg.SetTypeIndex = SETTYPE_MOVE_BACK;

                        // 강성호 ...
                        // ----------
                        dlg.CfgSaveFolderPath = ConfigManager.GetModelFilePath + DataManager.GetModelSelectFileNameFolder();
                        dlg.CfgSaveHeadFileName = DataManager.TestProcList[lstAddList.SelectedIndices[0]].WFRecoDatHeadFileName;

                        /*
                        dlg.RecoSetData.RecoCamIndex = 0;
                        dlg.RecoSetData.Lamp_Index = 0;
                        dlg.RecoSetData.Rotation_Index = 0;
                        dlg.RecoSetData.Motion_Index = 0;
                        */
                        // ----------


                        if (dlg.ShowDialog() == DialogResult.OK)
                        {
                            DataManager.TestProcList[lstAddList.SelectedIndices[0]] = dlg._WorkFuncInfo;
                            if (DataManager.TestProcList[lstAddList.SelectedIndices[0]].WFRecoDatHeadFileName == null)
                            {
                                DataManager.TestProcList[lstAddList.SelectedIndices[0]].WFRecoDatHeadFileName = dlg.CfgSaveHeadFileName;
                            }
                            else
                            {
                                if (DataManager.TestProcList[lstAddList.SelectedIndices[0]].WFRecoDatHeadFileName.Length == 0)
                                {
                                    DataManager.TestProcList[lstAddList.SelectedIndices[0]].WFRecoDatHeadFileName = dlg.CfgSaveHeadFileName;
                                }
                            }

                            ReDrawTestProcList(-1);
                        }
                    }
                    break;

                case 23: // @"이동축 INDEX 상방, 영상 인식 기능"
                    {
                        frmReco dlg = new frmReco();
                        //frmCalibration dlg = new frmCalibration();
                        
                        dlg._WorkFuncInfo = DataManager.TestProcList[lstAddList.SelectedIndices[0]];
                        dlg.SetTypeIndex = SETTYPE_MOVE_TOP;

                        // 강성호 ...
                        // ----------
                        dlg.CfgSaveFolderPath = ConfigManager.GetModelFilePath + DataManager.GetModelSelectFileNameFolder();
                        dlg.CfgSaveHeadFileName = DataManager.TestProcList[lstAddList.SelectedIndices[0]].WFRecoDatHeadFileName;

                        /*
                        dlg.RecoSetData.RecoCamIndex = 0;
                        dlg.RecoSetData.Lamp_Index = 0;
                        dlg.RecoSetData.Rotation_Index = 0;
                        dlg.RecoSetData.Motion_Index = 0;
                        */
                        // ----------


                        if (dlg.ShowDialog() == DialogResult.OK)
                        {
                            DataManager.TestProcList[lstAddList.SelectedIndices[0]] = dlg._WorkFuncInfo;
                            if (DataManager.TestProcList[lstAddList.SelectedIndices[0]].WFRecoDatHeadFileName == null)
                            {
                                DataManager.TestProcList[lstAddList.SelectedIndices[0]].WFRecoDatHeadFileName = dlg.CfgSaveHeadFileName;
                            }
                            else
                            {
                                if (DataManager.TestProcList[lstAddList.SelectedIndices[0]].WFRecoDatHeadFileName.Length == 0)
                                {
                                    DataManager.TestProcList[lstAddList.SelectedIndices[0]].WFRecoDatHeadFileName = dlg.CfgSaveHeadFileName;
                                }
                            }

                            ReDrawTestProcList(-1);
                        }
                    }
                    break;


                case 13: // 용접 로봇 동작 기능
                    {                        
                        frmEFControl dlg = new frmEFControl();

                        dlg._WorkFuncInfo = DataManager.TestProcList[lstAddList.SelectedIndices[0]];


                        // 강성호 ...
                        // ----------
                        dlg.LoadDXFFilesA = "";
                        dlg.LoadDXFFilesB = "";
                        // ----------


                        if (dlg.ShowDialog() == DialogResult.OK)
                        {
                            DataManager.TestProcList[lstAddList.SelectedIndices[0]] = dlg._WorkFuncInfo;

                            ReDrawTestProcList(-1);
                        }
                        
                    }
                    break;

                case 14: // 용접 기능
                    {
                        frmFuncWelding dlg = new frmFuncWelding();

                        dlg._WorkFuncInfo = DataManager.TestProcList[lstAddList.SelectedIndices[0]];

                        if (dlg.ShowDialog() == DialogResult.OK)
                        {
                            DataManager.TestProcList[lstAddList.SelectedIndices[0]] = dlg._WorkFuncInfo;

                            ReDrawTestProcList(-1);
                        }
                    }
                    break;

                case 15: // Delay 기능
                    {
                        frmFuncDelay dlg = new frmFuncDelay();

                        dlg._WorkFuncInfo = DataManager.TestProcList[lstAddList.SelectedIndices[0]];

                        if (dlg.ShowDialog() == DialogResult.OK)
                        {
                            DataManager.TestProcList[lstAddList.SelectedIndices[0]] = dlg._WorkFuncInfo;

                            ReDrawTestProcList(-1);
                        }
                    }
                    break;

                case 16: // Digital I/O 동작 기능
                    {
                        frmFuncDIO dlg = new frmFuncDIO();

                        dlg._WorkFuncInfo = DataManager.TestProcList[lstAddList.SelectedIndices[0]];

                        if (dlg.ShowDialog() == DialogResult.OK)
                        {
                            DataManager.TestProcList[lstAddList.SelectedIndices[0]] = dlg._WorkFuncInfo;

                            ReDrawTestProcList(-1);
                        }
                    }
                    break;

                case 17: // 롤링 카메라 모니터링 기능
                    {
                        frmMonitorCam dlg = new frmMonitorCam();

                        dlg._WorkFuncInfo = DataManager.TestProcList[lstAddList.SelectedIndices[0]];

                        // 강성호 ...
                        // ----------
                        dlg.CamIndex = 1;
                        dlg.CamIndexID = "";
                        // ----------

                        if (dlg.ShowDialog() == DialogResult.OK)
                        {
                            DataManager.TestProcList[lstAddList.SelectedIndices[0]] = dlg._WorkFuncInfo;

                            ReDrawTestProcList(-1);
                        }                        
                    }
                    break;

                case 18: // INDEX 양쪽 동기 회전(R) 기능
                    {
                        frmFuncIndex dlg = new frmFuncIndex();

                        dlg._WorkFuncInfo = DataManager.TestProcList[lstAddList.SelectedIndices[0]];

                        if (dlg.ShowDialog() == DialogResult.OK)
                        {
                            DataManager.TestProcList[lstAddList.SelectedIndices[0]] = dlg._WorkFuncInfo;

                            ReDrawTestProcList(-1);
                        }
                    }
                    break;


                default:
                    break;
            }
        }

        private void lstAddList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstAddList.SelectedItems.Count == 1)
            {
                _SelectedWorkFuncInfo = DataManager.TestProcList[lstAddList.SelectedIndices[0]];
            }
        }

        private void ReDrawTestProcList(int SelDrawIndex)
        {
            lstAddList.Items.Clear();
            for (int i = 0; i < DataManager.TESTPROCMAX; i++)
            {
                if (DataManager.TestProcList[i].TestProcExistFlag == true)
                {
                    ListViewItem lstViewTestItem = new ListViewItem(DataManager.TestProcList[i].TestProcName);

                    //lstViewTestItem.SubItems.Add("홍동성");

                    /*
                    if (TestProcList[i].TestSkipProcessType == 1)
                    {
                        lstViewTestItem.SubItems.Add("SKIP");
                    }
                    else
                    {
                        lstViewTestItem.SubItems.Add(GetProcStatusName(TestProcList[i].TestProcStatus));
                    }
                    */

                    lstAddList.Items.Add(lstViewTestItem);
                }

                if (DataManager.TestProcList[i].TestProcExistFlag == false)
                {
                    break;
                }
            }
            lstAddList.EndUpdate();
            if(SelDrawIndex>-1)
            {
                lstAddList.Items[SelDrawIndex].Selected = true;
            }
        }




#endregion


#region 환경 설정 ...

        private void btnCameraSetting_Click(object sender, EventArgs e)
        {
            frmCameraSetting frmDlg = new frmCameraSetting();

            frmDlg.ShowDialog();
        }

        private void btnLightingSetting_Click(object sender, EventArgs e)
        {
            frmLightingSetting frmDlg = new frmLightingSetting();

            frmDlg.ShowDialog();
        }

        private void btnMotionSetting_Click(object sender, EventArgs e)
        {
            frmMotionSetting frmDlg = new frmMotionSetting();

            frmDlg.ShowDialog();
        }

        private void btnDIOSetting_Click(object sender, EventArgs e)
        {
            frmDIOSetting frmDlg = new frmDIOSetting();

            frmDlg.ShowDialog();
        }

#endregion 환경 설정 ...

        private void btnRecognizeSet_Click(object sender, EventArgs e)
        {
            // 11, 19, 20, 21, 22, 23

            /*
            고정축 전방, 이동축 전방
            고정축 전방, 이동축 후방
            고정축 전방, 이동축 상방
            고정축 후방, 이동축 전방
            고정축 후방, 이동축 후방
            고정축 후방, 이동축 상방
            */


            frmRecognizeSet frm_recognize_set = new frmRecognizeSet();

            string strWorkName = "";

            if (frm_recognize_set.ShowDialog() == DialogResult.OK)
            {
                int Index = DataManager.FindIndex_RecoItem();

                string First_Name = "";
                int First_Type = 0;
                int First_Index = Index;

                string Second_Name = "";
                int Second_Type = 0;
                int Second_Index = Index + 1;





                /*
                        case 11:
                        case 19:
                        case 20:
                        case 21:
                        case 22:
                        case 23:
                */




                switch (frm_recognize_set.SelectedButton)
                {
                    case -1:
                        {
                            MessageBox.Show("취소 되었습니다.");

                            return;
                        }
                        break;

                    case 0: // 고정축 전방, 이동축 전방
                        {
                            First_Type = 19;
                            First_Name = DataManager.WorkNameList[First_Type];

                            Second_Type = 21;
                            Second_Name = DataManager.WorkNameList[Second_Type];
                        }
                        break;
                    case 1: // 고정축 전방, 이동축 후방
                        {
                            // 고정축
                            First_Type = 19;
                            First_Name = DataManager.WorkNameList[First_Type];

                            Second_Type = 22;
                            Second_Name = DataManager.WorkNameList[Second_Type];
                        }
                        break;
                    case 2: // 고정축 전방, 이동축 상방
                        {
                            // 고정축
                            First_Type = 19;
                            First_Name = DataManager.WorkNameList[First_Type];

                            Second_Type = 23;
                            Second_Name = DataManager.WorkNameList[Second_Type];
                        }
                        break;
                    case 3: // 고정축 후방, 이동축 전방
                        {
                            // 고정축
                            First_Type = 20;
                            First_Name = DataManager.WorkNameList[First_Type];

                            Second_Type = 21;
                            Second_Name = DataManager.WorkNameList[Second_Type];
                        }
                        break;
                    case 4: // 고정축 후방, 이동축 후방
                        {
                            // 고정축
                            First_Type = 20;
                            First_Name = DataManager.WorkNameList[First_Type];

                            Second_Type = 22;
                            Second_Name = DataManager.WorkNameList[Second_Type];
                        }
                        break;
                    case 5: // 고정축 후방, 이동축 상방
                        {
                            // 고정축
                            First_Type = 20;
                            First_Name = DataManager.WorkNameList[First_Type];

                            Second_Type = 23;
                            Second_Name = DataManager.WorkNameList[Second_Type];
                        }
                        break;
                }


                // --------------------------------------------------


                if (Index == -1)
                {
                    DataManager.AddTestProcList(First_Name, First_Type);

                    DataManager.AddTestProcList(Second_Name, Second_Type);
                }
                else
                {
                    DataManager.ReplaceTestProcList(First_Name, First_Type, First_Index);

                    DataManager.ReplaceTestProcList(Second_Name, Second_Type, Second_Index);
                }


            }




            // 이동축

            ReDrawTestProcList(-1);




        }




    }
}
