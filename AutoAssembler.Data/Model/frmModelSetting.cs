using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using DXFImportReco;

namespace AutoAssembler.Data
{

    struct DoubleDataPosInfo
    {
        public bool ddp_ExistFlag;
        public double ddp_X;
        public double ddp_Y;
    }


    public struct DXFDataInfo
    {
        public CADImage FCADImage;// = new CADImage();
        public float FScale;
        public Point Base;
        public Point BaseDefault;
        public float FS_W_Base, FS_H_Base, FS_W, FS_H;
        public bool StartCalcFlag;

    }

    public struct RecoStructInfo2
    {
        public bool RecoExistTypeFlag;
        public string RecoDXFFileName;
        public string RecoCapFileName;
        public int FullShot_ZoomNum;
        public int FullShot_FocusNum;
        public int DetailShot_ZoomNum;
        public int DetailShot_FocusNum;

        // ----------

        public int ZoomFocus_PortNo;

        public int RecoCamIndex;
        public string RecoCamID;

        public int Rotation_Index;
        public double Rotation_R;

        public int Motion_Index;
        public double Motion_Move_X;
        public double Motion_Move_Y;
        public double Motion_Move_Z;

        public int Lamp_Index;
        public int Lamp_PortNo;
        public int Lamp_SetChannel;
        public int Lamp_SetValue;

        public double Move_R_FIX_Angle_Gap;
        public double Move_R_MOVE_Angle_Gap;
        public double Move_R_Move_X_Gap;
        public double Move_R_Move_Y_Gap;
        public double Move_R_Move_Z_Gap;

        public double Object_Weight;
        public double Object_Diameter;
        public double Object_Hole_Distance;
        public double Object_Pin_Height;
        public double Object_Pin_Diameter;

        public double Back_R_MOVE_Angle_Gap;
        public double Top_R_MOVE_Angle_Gap;

        public int Object_TopHoleType;
        public int DXF_RecoType;
        public int Object_Param3;
        public int Object_Param4;
        public int Object_Param5;

        public double Object_TopDirHoleDistance;
        public double Motion_Move_MR;
        public double Object_ShaftLength;
        public double Object_Param9;
        public double Object_Param10;
        public double Object_Param11;
        public double Object_Param12;
    }
    public struct ShotDetailInfo2
    {
        public int ShotType;
        public double ShotGapDistance;
        public int ZoomNum;
        public int FocusNum;
    }


    public partial class frmModelSetting : Form
    {
        public bool bModelNameChange = false;


        // 인식 정의 ...
        // --------------------------------------------------
        private static char[] SaveDiv = new char[] { '|' };

        public const int ALIGN_SET_NONE = 0;
        public const int ALIGN_SET_FIX_F_MOVE_F = 1;
        public const int ALIGN_SET_FIX_F_MOVE_B = 2;
        public const int ALIGN_SET_FIX_F_MOVE_T = 3;
        public const int ALIGN_SET_FIX_B_MOVE_F = 4;
        public const int ALIGN_SET_FIX_B_MOVE_B = 5;
        public const int ALIGN_SET_FIX_B_MOVE_T = 6;
        private int CurSelectTypeIndex = ALIGN_SET_NONE;

        public const int SETTYPE_NONE = 700;
        public const int SETTYPE_FIX_FRONT = 710;
        public const int SETTYPE_FIX_BACK = 720;
        public const int SETTYPE_MOVE_FRONT = 730;
        public const int SETTYPE_MOVE_BACK = 740;
        public const int SETTYPE_MOVE_TOP = 750;

        public const string SetCalibrationDataFileName = "CalibValue.dat";

        public string LoadDXFFilesA = "";
        public string LoadDXFFilesB = "";
        public string PreLoadDXFFilesA = "";
        public string PreLoadDXFFilesB = "";
        private DXFDataInfo DXFDatA;
        private DXFDataInfo DXFDatB;
        private Bitmap BlankA;
        private Bitmap BlankB;

        float CalcReScalePer = 92.0f;//77.0f;

        public RecoStructInfo2 RecoSetData;

        private int Model_Reco_Index;
        private int Model_Fix_Index;
        private int Model_Fix_Type;
        private int Model_Move_Index;
        private int Model_Move_Type;

        DoubleDataPosInfo RealCalibrationData_A;
        DoubleDataPosInfo RealCalibrationData_B;
        DoubleDataPosInfo RealCalibrationData_C;
        DoubleDataPosInfo RealCalibrationData_D;
        DoubleDataPosInfo RealCalibrationData_E;
        DoubleDataPosInfo RealCalibrationData_F;
        DoubleDataPosInfo RealCalibrationData_G;
        DoubleDataPosInfo RealCalibrationData_H;

        private ShotDetailInfo2[] DxfShotDataList_A = new ShotDetailInfo2[19];
        private ShotDetailInfo2[] DxfShotDataList_B = new ShotDetailInfo2[19];
        private ShotDetailInfo2[] DxfShotDataList_C = new ShotDetailInfo2[19];
        private ShotDetailInfo2[] DxfShotDataList_D = new ShotDetailInfo2[19];
        private ShotDetailInfo2[] DxfShotDataList_E = new ShotDetailInfo2[19];


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
            txtModelName.Text = DataManager.SelectedModel.ModelName;

            string strFileName = ConfigManager.GetModelFilePath + DataManager.SelectedModel.ModelName + "\\" + DataManager.SelectedModel.ImageFileName;

            if (File.Exists(strFileName) == true)
            {
                ModelPictureBox.Image = System.Drawing.Image.FromFile(strFileName);
            }


            // 롤링 정보 ...
            // --------------------------------------------------
            txtFLValue.Text = DataManager.SelectedModel.dFLValue.ToString("##.00");           // 자유장
            txtMetalThick1.Text = DataManager.SelectedModel.dMetalThick1.ToString("##.00");   // 메탈 1
            txtMetalThick2.Text = DataManager.SelectedModel.dMetalThick2.ToString("##.00");   // 메탈 2
            txtSLValue.Text = DataManager.SelectedModel.dSLValue.ToString("##.00");           // 밀착장
            txtWRValue.Text = DataManager.SelectedModel.dWRValue.ToString("##.00");   // WR
            txtCapsule.Text = DataManager.SelectedModel.dCapsulePie.ToString("##.00");    // 캡슐 파이
            txtRolling70Rate.Text = DataManager.SelectedModel.dRolling70Rate.ToString("##.00");
            txtRolling80Rate.Text = DataManager.SelectedModel.dRolling80Rate.ToString("##.00");
            txtRollingOffset.Text = DataManager.SelectedModel.dRollingOffset.ToString("##.00");
            txtRotateCount.Text = DataManager.SelectedModel.dRotateCount.ToString("##.00");
            txtVBlockFL_Limit_Value.Text = DataManager.SelectedModel.dVBlockFL_Limit_Value.ToString("##.00");
            txtVBlockFL_Offset_Value.Text = DataManager.SelectedModel.dVBlockFL_Offset_Value.ToString("##.00");


            // 인식 정보 ...
            // --------------------------------------------------
            cboSetList.Items.Clear();
            cboSetList.Items.Add("(선택하지 않음)");
            cboSetList.Items.Add("고정축(A)-전방(F), 이동축(B)-전방(F)");
            cboSetList.Items.Add("고정축(A)-전방(F), 이동축(B)-후방(B)");
            cboSetList.Items.Add("고정축(A)-전방(F), 이동축(B)-상방(T)");
            cboSetList.Items.Add("고정축(A)-후방(B), 이동축(B)-전방(F)");
            cboSetList.Items.Add("고정축(A)-후방(B), 이동축(B)-후방(B)");
            cboSetList.Items.Add("고정축(A)-후방(B), 이동축(B)-상방(T)");

            SetDXFShotDataInit();

            LoadCalibSetFiles(SetCalibrationDataFileName);

            if(DataManager.SelectedModel.nAlignSetIndex > -1)
            {
                CurSelectTypeIndex = DataManager.SelectedModel.nAlignSetIndex;
                cboSetList.SelectedIndex = DataManager.SelectedModel.nAlignSetIndex;
            }

            BlankA = DrawFilledRectangle(picDXFIndex_F.Width, picDXFIndex_F.Height);
            BlankB = DrawFilledRectangle(picDXFIndex_M.Width, picDXFIndex_M.Height);

            TopHoleTypeList();
            DXFTypeList();

            // 시나리오 파일 열기 ...
            // --------------------------------------------------
            string DataFilePath = ConfigManager.GetModelFilePath + DataManager.SelectedModel.ModelName + "\\" + "ModelListData.dat";

            DataManager.TestProcClear();
            DataManager.LoadWorkListFiles(DataFilePath); // 3 작업 리스트 파일 로드            
            // --------------------------------------------------

            Model_Reco_Index = -1;
            Model_Fix_Index = -1;
            Model_Fix_Type = SETTYPE_NONE;
            Model_Move_Index = -1;
            Model_Move_Type = SETTYPE_NONE;
            for (int i = 0; i < 1000; i++)
            {
                if (DataManager.TestProcList[i].TestProcExistFlag == true)
                {
                    switch (DataManager.TestProcList[i].WFType)
                    {
                        case 11: // 영상 인식 INDEX 위치 맞춤 기능
                            if (Model_Reco_Index == -1)
                            {
                                Model_Reco_Index = i;
                            }
                            break;
                        case 19: // @"고정축 INDEX 전방, 영상 인식 기능"
                            if (Model_Fix_Index == -1)
                            {
                                Model_Fix_Index = i;
                                Model_Fix_Type = SETTYPE_FIX_FRONT;
                            }
                            break;
                        case 20: // @"고정축 INDEX 후방, 영상 인식 기능"
                            if (Model_Fix_Index == -1)
                            {
                                Model_Fix_Index = i;
                                Model_Fix_Type = SETTYPE_FIX_BACK;
                            }
                            break;
                        case 21: // @"이동축 INDEX 전방, 영상 인식 기능"
                            if (Model_Move_Index == -1)
                            {
                                Model_Move_Index = i;
                                Model_Move_Type = SETTYPE_MOVE_FRONT;
                            }
                            break;
                        case 22: // @"이동축 INDEX 후방, 영상 인식 기능"
                            if (Model_Move_Index == -1)
                            {
                                Model_Move_Index = i;
                                Model_Move_Type = SETTYPE_MOVE_BACK;
                            }
                            break;
                        case 23: // @"이동축 INDEX 상방, 영상 인식 기능"
                            if (Model_Move_Index == -1)
                            {
                                Model_Move_Index = i;
                                Model_Move_Type = SETTYPE_MOVE_TOP;
                            }
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    break;
                }
            }

            //if(false)
            if (Model_Fix_Index > -1 && Model_Move_Index > -1)
            {
                if(Model_Fix_Type == SETTYPE_FIX_FRONT && Model_Move_Type == SETTYPE_MOVE_FRONT)
                {
                    CurSelectTypeIndex = 1;
                    cboSetList.SelectedIndex = CurSelectTypeIndex;
                }
                else if (Model_Fix_Type == SETTYPE_FIX_FRONT && Model_Move_Type == SETTYPE_MOVE_BACK)
                {
                    CurSelectTypeIndex = 2;
                    cboSetList.SelectedIndex = CurSelectTypeIndex;
                }
                else if (Model_Fix_Type == SETTYPE_FIX_FRONT && Model_Move_Type == SETTYPE_MOVE_TOP)
                {
                    CurSelectTypeIndex = 3;
                    cboSetList.SelectedIndex = CurSelectTypeIndex;
                }
                else if (Model_Fix_Type == SETTYPE_FIX_BACK && Model_Move_Type == SETTYPE_MOVE_FRONT)
                {
                    CurSelectTypeIndex = 4;
                    cboSetList.SelectedIndex = CurSelectTypeIndex;
                }
                else if (Model_Fix_Type == SETTYPE_FIX_BACK && Model_Move_Type == SETTYPE_MOVE_BACK)
                {
                    CurSelectTypeIndex = 5;
                    cboSetList.SelectedIndex = CurSelectTypeIndex;
                }
                else if (Model_Fix_Type == SETTYPE_FIX_BACK && Model_Move_Type == SETTYPE_MOVE_TOP)
                {
                    CurSelectTypeIndex = 6;
                    cboSetList.SelectedIndex = CurSelectTypeIndex;
                }
            }
            string WFReco_FixName;
            string WFReco_MoveName;
            if (DataManager.TestProcList[Model_Fix_Index].WFRecoDatHeadFileName == null)
            {
                WFReco_FixName = "1";
                DataManager.TestProcList[Model_Fix_Index].WFRecoDatHeadFileName = WFReco_FixName;
            }
            else if (DataManager.TestProcList[Model_Fix_Index].WFRecoDatHeadFileName == "")
            {
                WFReco_FixName = "1";
                DataManager.TestProcList[Model_Fix_Index].WFRecoDatHeadFileName = WFReco_FixName;
            }
            else
            {
                WFReco_FixName = DataManager.TestProcList[Model_Fix_Index].WFRecoDatHeadFileName;
            }

            if (DataManager.TestProcList[Model_Move_Index].WFRecoDatHeadFileName == null)
            {
                WFReco_MoveName = "2";
                DataManager.TestProcList[Model_Move_Index].WFRecoDatHeadFileName = WFReco_MoveName;
            }
            else if (DataManager.TestProcList[Model_Move_Index].WFRecoDatHeadFileName == "")
            {
                WFReco_MoveName = "2";
                DataManager.TestProcList[Model_Move_Index].WFRecoDatHeadFileName = WFReco_MoveName;
            }
            else
            {
                WFReco_MoveName = DataManager.TestProcList[Model_Move_Index].WFRecoDatHeadFileName;
            }

            string CfgSaveFolderPath_FileName;
            string CfgSaveDXFPath_FileName;
            if(Model_Fix_Index>-1)
            {
                CfgSaveFolderPath_FileName = ConfigManager.GetModelFilePath + DataManager.SelectedModel.ModelName + "\\" + WFReco_FixName + ".dat";
                LoadCfgFiles(CfgSaveFolderPath_FileName);
                PreLoadDXFFilesA = RecoSetData.RecoDXFFileName;
                //CfgSaveDXFPath_FileName = ConfigManager.GetModelFilePath + DataManager.SelectedModel.ModelName + "\\" + WFReco_FixName + ".dxf";
                CfgSaveDXFPath_FileName = ConfigManager.GetModelFilePath + DataManager.SelectedModel.ModelName + "\\" + PreLoadDXFFilesA;
                DisplayRecoData(1);
                LoadDXFDataA(CfgSaveDXFPath_FileName);
            }
            if (Model_Move_Index > -1)
            {
                CfgSaveFolderPath_FileName = ConfigManager.GetModelFilePath + DataManager.SelectedModel.ModelName + "\\" + WFReco_MoveName + ".dat";
                LoadCfgFiles(CfgSaveFolderPath_FileName);
                PreLoadDXFFilesB = RecoSetData.RecoDXFFileName;
                CfgSaveDXFPath_FileName = ConfigManager.GetModelFilePath + DataManager.SelectedModel.ModelName + "\\" + PreLoadDXFFilesB;
                DisplayRecoData(2);
                LoadDXFDataB(CfgSaveDXFPath_FileName);
            }
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

            // 자유장
            if (double.TryParse(this.txtFLValue.Text, out dTempValue))
            {
                DataManager.SelectedModel.dFLValue = dTempValue;
            }

            // 메탈 1
            if (double.TryParse(this.txtMetalThick1.Text, out dTempValue))
            {
                DataManager.SelectedModel.dMetalThick1 = dTempValue;
            }

            // 메탈 2
            if (double.TryParse(this.txtMetalThick2.Text, out dTempValue))
            {
                DataManager.SelectedModel.dMetalThick2 = dTempValue;
            }

            // 밀착장
            if (double.TryParse(this.txtSLValue.Text, out dTempValue))
            {
                DataManager.SelectedModel.dSLValue = dTempValue;
            }

            // WR
            if (double.TryParse(this.txtWRValue.Text, out dTempValue))
            {
                DataManager.SelectedModel.dWRValue = dTempValue;
            }

            // 캡슐 파이
            if (double.TryParse(this.txtCapsule.Text, out dTempValue))
            {
                DataManager.SelectedModel.dCapsulePie = dTempValue;
            }

            if (double.TryParse(this.txtRolling70Rate.Text, out dTempValue))
            {
                DataManager.SelectedModel.dRolling70Rate = dTempValue;
            }

            if (double.TryParse(this.txtRolling80Rate.Text, out dTempValue))
            {
                DataManager.SelectedModel.dRolling80Rate = dTempValue;
            }

            if (double.TryParse(this.txtRollingOffset.Text, out dTempValue))
            {
                DataManager.SelectedModel.dRollingOffset = dTempValue;
            }

            if (double.TryParse(this.txtRotateCount.Text, out dTempValue))
            {
                DataManager.SelectedModel.dRotateCount = dTempValue;
            }

            if (double.TryParse(this.txtVBlockFL_Limit_Value.Text, out dTempValue))
            {
                DataManager.SelectedModel.dVBlockFL_Limit_Value = dTempValue;
            }

            if (double.TryParse(this.txtVBlockFL_Offset_Value.Text, out dTempValue))
            {
                DataManager.SelectedModel.dVBlockFL_Offset_Value = dTempValue;
            }



            // 인식 정보 ...
            // --------------------------------------------------

            string DXFRealFileName;
            DataManager.SelectedModel.nAlignSetIndex = cboSetList.SelectedIndex;
            if(cboSetList.SelectedIndex > 0)
            {
                string RecoFixFileName, RecoMoveFileName, RecoFixDXF_FileName, RecoMoveDXF_FileName;

                if (Model_Fix_Type>-1)
                {
                    if (LoadDXFFilesA.Length > 0)
                    {
                        if (File.Exists(LoadDXFFilesA) == true)
                        {
                            DXFRealFileName = GetOutOnlyFileName(LoadDXFFilesA);
                        }
                        else
                        {
                            DXFRealFileName = PreLoadDXFFilesA;
                        }
                    }
                    else
                    {
                        DXFRealFileName = PreLoadDXFFilesA;
                    }
                    RecoFixFileName = ConfigManager.GetModelFilePath + DataManager.SelectedModel.ModelName + "\\" + DataManager.TestProcList[Model_Fix_Index].WFRecoDatHeadFileName + ".dat";
                    RecoFixDXF_FileName = ConfigManager.GetModelFilePath + DataManager.SelectedModel.ModelName + "\\" + DXFRealFileName;
                    SaveRecoData(RecoFixFileName, Model_Fix_Type, DXFRealFileName, ref DataManager.TestProcList[Model_Fix_Index]);
                    if (LoadDXFFilesA.Length>0)
                    {
                        if (File.Exists(LoadDXFFilesA) == true)
                        {
                            CopyDXFFiles(LoadDXFFilesA, RecoFixDXF_FileName);
                        }
                    }
                }
                if (Model_Move_Type > -1)
                {
                    if (LoadDXFFilesB.Length > 0)
                    {
                        if (File.Exists(LoadDXFFilesB) == true)
                        {
                            DXFRealFileName = GetOutOnlyFileName(LoadDXFFilesB);
                        }
                        else
                        {
                            DXFRealFileName = PreLoadDXFFilesB;
                        }
                    }
                    else
                    {
                        DXFRealFileName = PreLoadDXFFilesB;
                    }
                    RecoMoveFileName = ConfigManager.GetModelFilePath + DataManager.SelectedModel.ModelName + "\\" + DataManager.TestProcList[Model_Move_Index].WFRecoDatHeadFileName + ".dat";
                    RecoMoveDXF_FileName = ConfigManager.GetModelFilePath + DataManager.SelectedModel.ModelName + "\\" + DXFRealFileName;
                    SaveRecoData(RecoMoveFileName, Model_Move_Type, DXFRealFileName, ref DataManager.TestProcList[Model_Move_Index]);
                    if (LoadDXFFilesB.Length>0)
                    {
                        if (File.Exists(LoadDXFFilesB) == true)
                        {
                            CopyDXFFiles(LoadDXFFilesB, RecoMoveDXF_FileName);
                        }
                    }
                }


                /*
                if (cboSetList.SelectedIndex == ALIGN_SET_FIX_F_MOVE_F)
                {
                    WorkFuncInfo _WorkFuncInfoFix;
                    WorkFuncInfo _WorkFuncInfoMove;
                    RecoFixFileName = "";
                    RecoMoveFileName = "";
                    SaveRecoData(RecoFixFileName, SETTYPE_FIX_FRONT, ref _WorkFuncInfoFix);
                    SaveRecoData(RecoMoveFileName, SETTYPE_MOVE_FRONT, ref _WorkFuncInfoMove);
                }
                else if (cboSetList.SelectedIndex == ALIGN_SET_FIX_F_MOVE_B)
                {
                    WorkFuncInfo _WorkFuncInfoFix;
                    WorkFuncInfo _WorkFuncInfoMove;
                    RecoFixFileName = "";
                    RecoMoveFileName = "";
                    SaveRecoData(RecoFixFileName, SETTYPE_FIX_FRONT, ref _WorkFuncInfoFix);
                    SaveRecoData(RecoMoveFileName, SETTYPE_MOVE_BACK, ref _WorkFuncInfoMove);
                }
                else if (cboSetList.SelectedIndex == ALIGN_SET_FIX_F_MOVE_T)
                {
                    WorkFuncInfo _WorkFuncInfoFix;
                    WorkFuncInfo _WorkFuncInfoMove;
                    RecoFixFileName = "";
                    RecoMoveFileName = "";
                    SaveRecoData(RecoFixFileName, SETTYPE_FIX_FRONT, ref _WorkFuncInfoFix);
                    SaveRecoData(RecoMoveFileName, SETTYPE_MOVE_TOP, ref _WorkFuncInfoMove);
                }
                else if (cboSetList.SelectedIndex == ALIGN_SET_FIX_B_MOVE_F)
                {
                    WorkFuncInfo _WorkFuncInfoFix;
                    WorkFuncInfo _WorkFuncInfoMove;
                    RecoFixFileName = "";
                    RecoMoveFileName = "";
                    SaveRecoData(RecoFixFileName, SETTYPE_FIX_BACK, ref _WorkFuncInfoFix);
                    SaveRecoData(RecoMoveFileName, SETTYPE_MOVE_FRONT, ref _WorkFuncInfoMove);
                }
                else if (cboSetList.SelectedIndex == ALIGN_SET_FIX_B_MOVE_B)
                {
                    WorkFuncInfo _WorkFuncInfoFix;
                    WorkFuncInfo _WorkFuncInfoMove;
                    RecoFixFileName = "";
                    RecoMoveFileName = "";
                    SaveRecoData(RecoFixFileName, SETTYPE_FIX_BACK, ref _WorkFuncInfoFix);
                    SaveRecoData(RecoMoveFileName, SETTYPE_MOVE_BACK, ref _WorkFuncInfoMove);
                }
                else if (cboSetList.SelectedIndex == ALIGN_SET_FIX_B_MOVE_T)
                {
                    WorkFuncInfo _WorkFuncInfoFix;
                    WorkFuncInfo _WorkFuncInfoMove;
                    RecoFixFileName = "";
                    RecoMoveFileName = "";
                    SaveRecoData(RecoFixFileName, SETTYPE_FIX_BACK, ref _WorkFuncInfoFix);
                    SaveRecoData(RecoMoveFileName, SETTYPE_MOVE_TOP, ref _WorkFuncInfoMove);
                }
                */

                
                string DataFilePath = ConfigManager.GetModelFilePath + DataManager.SelectedModel.ModelName + "\\" + "ModelListData.dat";

                DataManager.SaveWorkListFiles(DataFilePath);
            }

            DataManager.SaveModelListFiles();
            // --------------------------------------------------
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void cboSetList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int CmpAlignCurSetIndex;
            bool RunSetInitFlag = false;
            if (cboSetList.SelectedIndex > 0)
            {
                CmpAlignCurSetIndex = ALIGN_SET_FIX_F_MOVE_F;
                if (cboSetList.SelectedIndex == CmpAlignCurSetIndex)
                {
                    RunSetInitFlag = false;
                    if (CurSelectTypeIndex == ALIGN_SET_NONE)
                    {
                        RunSetInitFlag = true;
                    }
                    if (CurSelectTypeIndex != CmpAlignCurSetIndex)
                    {
                        if (MessageBox.Show("변경하실 경우 기존 Align자료가 초기화됩니다. 변경하시겠습니까?", "Align Set 변경", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            RunSetInitFlag = true;
                        }
                        else
                        {
                            RunSetInitFlag = false;
                        }
                    }
                    if(RunSetInitFlag == true)
                    {
                        ClearAlignData(CmpAlignCurSetIndex);
                        MessageBox.Show("AlignSet변경완료!");
                        CurSelectTypeIndex = CmpAlignCurSetIndex;

                        // 홍동성 ...
                        // ----------
                        UpdateRecoType(cboSetList.SelectedIndex);
                        // ----------

                        return;
                    }
                }

                CmpAlignCurSetIndex = ALIGN_SET_FIX_F_MOVE_B;
                if (cboSetList.SelectedIndex == CmpAlignCurSetIndex)
                {
                    RunSetInitFlag = false;
                    if (CurSelectTypeIndex == ALIGN_SET_NONE)
                    {
                        RunSetInitFlag = true;
                    }
                    if (CurSelectTypeIndex != CmpAlignCurSetIndex)
                    {
                        if (MessageBox.Show("변경하실 경우 기존 Align자료가 초기화됩니다. 변경하시겠습니까?", "Align Set 변경", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            RunSetInitFlag = true;
                        }
                        else
                        {
                            RunSetInitFlag = false;
                        }
                    }
                    if (RunSetInitFlag == true)
                    {
                        ClearAlignData(CmpAlignCurSetIndex);
                        MessageBox.Show("AlignSet변경완료!");
                        CurSelectTypeIndex = CmpAlignCurSetIndex;

                        // 홍동성 ...
                        // ----------
                        UpdateRecoType(cboSetList.SelectedIndex);
                        // ----------

                        return;
                    }
                }

                CmpAlignCurSetIndex = ALIGN_SET_FIX_F_MOVE_T;
                if (cboSetList.SelectedIndex == CmpAlignCurSetIndex)
                {
                    RunSetInitFlag = false;
                    if (CurSelectTypeIndex == ALIGN_SET_NONE)
                    {
                        RunSetInitFlag = true;
                    }
                    if (CurSelectTypeIndex != CmpAlignCurSetIndex)
                    {
                        if (MessageBox.Show("변경하실 경우 기존 Align자료가 초기화됩니다. 변경하시겠습니까?", "Align Set 변경", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            RunSetInitFlag = true;
                        }
                        else
                        {
                            RunSetInitFlag = false;
                        }
                    }
                    if (RunSetInitFlag == true)
                    {
                        ClearAlignData(CmpAlignCurSetIndex);
                        MessageBox.Show("AlignSet변경완료!");
                        CurSelectTypeIndex = CmpAlignCurSetIndex;

                        // 홍동성 ...
                        // ----------
                        UpdateRecoType(cboSetList.SelectedIndex);
                        // ----------

                        return;
                    }
                }

                CmpAlignCurSetIndex = ALIGN_SET_FIX_B_MOVE_F;
                if (cboSetList.SelectedIndex == CmpAlignCurSetIndex)
                {
                    RunSetInitFlag = false;
                    if (CurSelectTypeIndex == ALIGN_SET_NONE)
                    {
                        RunSetInitFlag = true;
                    }
                    if (CurSelectTypeIndex != CmpAlignCurSetIndex)
                    {
                        if (MessageBox.Show("변경하실 경우 기존 Align자료가 초기화됩니다. 변경하시겠습니까?", "Align Set 변경", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            RunSetInitFlag = true;
                        }
                        else
                        {
                            RunSetInitFlag = false;
                        }
                    }
                    if (RunSetInitFlag == true)
                    {
                        ClearAlignData(CmpAlignCurSetIndex);
                        MessageBox.Show("AlignSet변경완료!");
                        CurSelectTypeIndex = CmpAlignCurSetIndex;

                        // 홍동성 ...
                        // ----------
                        UpdateRecoType(cboSetList.SelectedIndex);
                        // ----------

                        return;
                    }
                }

                CmpAlignCurSetIndex = ALIGN_SET_FIX_B_MOVE_B;
                if (cboSetList.SelectedIndex == CmpAlignCurSetIndex)
                {
                    RunSetInitFlag = false;
                    if (CurSelectTypeIndex == ALIGN_SET_NONE)
                    {
                        RunSetInitFlag = true;
                    }
                    if (CurSelectTypeIndex != CmpAlignCurSetIndex)
                    {
                        if (MessageBox.Show("변경하실 경우 기존 Align자료가 초기화됩니다. 변경하시겠습니까?", "Align Set 변경", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            RunSetInitFlag = true;
                        }
                        else
                        {
                            RunSetInitFlag = false;
                        }
                    }
                    if (RunSetInitFlag == true)
                    {
                        ClearAlignData(CmpAlignCurSetIndex);
                        MessageBox.Show("AlignSet변경완료!");
                        CurSelectTypeIndex = CmpAlignCurSetIndex;

                        // 홍동성 ...
                        // ----------
                        UpdateRecoType(cboSetList.SelectedIndex);
                        // ----------

                        return;
                    }
                }

                CmpAlignCurSetIndex = ALIGN_SET_FIX_B_MOVE_T;
                if (cboSetList.SelectedIndex == CmpAlignCurSetIndex)
                {
                    RunSetInitFlag = false;
                    if (CurSelectTypeIndex == ALIGN_SET_NONE)
                    {
                        RunSetInitFlag = true;
                    }
                    if (CurSelectTypeIndex != CmpAlignCurSetIndex)
                    {
                        if (MessageBox.Show("변경하실 경우 기존 Align자료가 초기화됩니다. 변경하시겠습니까?", "Align Set 변경", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            RunSetInitFlag = true;
                        }
                        else
                        {
                            RunSetInitFlag = false;
                        }
                    }
                    if (RunSetInitFlag == true)
                    {
                        ClearAlignData(CmpAlignCurSetIndex);
                        MessageBox.Show("AlignSet변경완료!");
                        CurSelectTypeIndex = CmpAlignCurSetIndex;

                        // 홍동성 ...
                        // ----------
                        UpdateRecoType(cboSetList.SelectedIndex);
                        // ----------

                        return;
                    }
                }
            }

            // 홍동성
            // ----------
            if (RunSetInitFlag == true)
            {
                //UpdateRecoType(cboSetList.SelectedIndex);
            }
            // ----------
            
        }

        private void UpdateRecoType(int type)
        {
            if (type == -1)
                return;

            int Index = DataManager.FindIndex_RecoItem();

            string First_Name = "";
            int First_Type = 0;
            int First_Index = Index;

            string Second_Name = "";
            int Second_Type = 0;
            int Second_Index = Index + 1;

            string strWorkName = "";

            switch (type)
            {

                case 1: // 고정축 전방, 이동축 전방
                    {
                        First_Type = 19;
                        First_Name = DataManager.WorkNameList[First_Type];

                        Second_Type = 21;
                        Second_Name = DataManager.WorkNameList[Second_Type];
                    }
                    break;
                case 2: // 고정축 전방, 이동축 후방
                    {
                        // 고정축
                        First_Type = 19;
                        First_Name = DataManager.WorkNameList[First_Type];

                        Second_Type = 22;
                        Second_Name = DataManager.WorkNameList[Second_Type];
                    }
                    break;
                case 3: // 고정축 전방, 이동축 상방
                    {
                        // 고정축
                        First_Type = 19;
                        First_Name = DataManager.WorkNameList[First_Type];

                        Second_Type = 23;
                        Second_Name = DataManager.WorkNameList[Second_Type];
                    }
                    break;
                case 4: // 고정축 후방, 이동축 전방
                    {
                        // 고정축
                        First_Type = 20;
                        First_Name = DataManager.WorkNameList[First_Type];

                        Second_Type = 21;
                        Second_Name = DataManager.WorkNameList[Second_Type];
                    }
                    break;
                case 5: // 고정축 후방, 이동축 후방
                    {
                        // 고정축
                        First_Type = 20;
                        First_Name = DataManager.WorkNameList[First_Type];

                        Second_Type = 22;
                        Second_Name = DataManager.WorkNameList[Second_Type];
                    }
                    break;
                case 6: // 고정축 후방, 이동축 상방
                    {
                        // 고정축
                        First_Type = 20;
                        First_Name = DataManager.WorkNameList[First_Type];

                        Second_Type = 23;
                        Second_Name = DataManager.WorkNameList[Second_Type];
                    }
                    break;
            }


            if (Index == -1)
            {
             
            }
            else
            {
                DataManager.ReplaceTestProcList(First_Name, First_Type, First_Index);

                DataManager.ReplaceTestProcList(Second_Name, Second_Type, Second_Index);
            }
        }

        private void ClearAlignData(int ChangeSetDatIndex)
        {
            if (ChangeSetDatIndex == ALIGN_SET_FIX_F_MOVE_F)
            {
                tbFixIndex.Text = "고정축(A)-전방(F)";
                tbMoveIndex.Text = "이동축(B)-전방(F)";
            }
            else if (ChangeSetDatIndex == ALIGN_SET_FIX_F_MOVE_B)
            {
                tbFixIndex.Text = "고정축(A)-전방(F)";
                tbMoveIndex.Text = "이동축(B)-후방(B)";
            }
            else if (ChangeSetDatIndex == ALIGN_SET_FIX_F_MOVE_T)
            {
                tbFixIndex.Text = "고정축(A)-전방(F)";
                tbMoveIndex.Text = "이동축(B)-상방(T)";
            }
            else if (ChangeSetDatIndex == ALIGN_SET_FIX_B_MOVE_F)
            {
                tbFixIndex.Text = "고정축(A)-후방(B)";
                tbMoveIndex.Text = "이동축(B)-전방(F)";
            }
            else if (ChangeSetDatIndex == ALIGN_SET_FIX_B_MOVE_B)
            {
                tbFixIndex.Text = "고정축(A)-후방(B)";
                tbMoveIndex.Text = "이동축(B)-후방(B)";
            }
            else if (ChangeSetDatIndex == ALIGN_SET_FIX_B_MOVE_T)
            {
                tbFixIndex.Text = "고정축(A)-후방(B)";
                tbMoveIndex.Text = "이동축(B)-상방(T)";
            }
            LoadDXFFilesA = "";
            LoadDXFFilesB = "";
        }

        private void btLoadDXF_F_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "CAD Files|*.dxf";
            openFileDialog1.Title = "Select a CAD File";

            if (openFileDialog1.ShowDialog(this) != DialogResult.OK) return;
            if (openFileDialog1.FileName != null)
            {
                LoadDXFFilesA = openFileDialog1.FileName;
                LoadDXFDataA(LoadDXFFilesA);
            }
        }

        private void LoadDXFDataA(string LoadDXFFilePath)
        {
            if (File.Exists(LoadDXFFilePath) == false)
            {
                return;
            }
            DXFDatA.StartCalcFlag = true;
            if (DXFDatA.FCADImage != null)
            {
                DXFDatA.FCADImage = null;
            }
            DXFDatA.FCADImage = new CADImage();
            DXFDatA.Base.X = picDXFIndex_F.Width / 2;
            DXFDatA.Base.Y = picDXFIndex_F.Width / 2;

            BlankA = DrawFilledRectangle(picDXFIndex_F.Width, picDXFIndex_F.Height);

            //FCADImage.Base.Y = Bottom - 400;
            //FCADImage.Base.X = 100;
            DXFDatA.FScale = 1.0f;
            DXFDatA.FCADImage.LoadFromFile(LoadDXFFilePath);
            //DrawCADImage();
            Bitmap tmp = BlankA;
            //FCADImage.FScale = 4.0f;
            DrawDXFImageCalc(tmp.Width, tmp.Height, true, ref DXFDatA);
            DrawDXFImage2((Bitmap)tmp, ref DXFDatA);
            picDXFIndex_F.Image = (Image)tmp.Clone();
            //EF_DXFDataInfo GetEFDXFData = GetEF_DXFData(ref DXFDatA);
            //DisplayGetEFData(GetEFDXFData);
            ShotDetailInfo2 TestZoom = GetDXFShotZoomFocus(0, DXFDatA.FS_H);

            txtZoom_F.Text = TestZoom.ZoomNum.ToString();
            txtFocus_F.Text = TestZoom.FocusNum.ToString();
            txtDtZoom_F.Text = "11755";
            txtDtFocus_F.Text = "19584";

        }

        private Bitmap DrawFilledRectangle(int x, int y)
        {
            Bitmap bmp = new Bitmap(x, y);
            using (Graphics graph = Graphics.FromImage(bmp))
            {
                Rectangle ImageSize = new Rectangle(0, 0, x, y);
                graph.FillRectangle(Brushes.Black, ImageSize);
            }
            return bmp;
        }


        public void DrawDXFImage2(Bitmap bmp, ref DXFDataInfo DrawDXFDat)
        {
            string GetEntityName;
            DXFLine dxLine;
            DXFCircle dxCircle;
            DXFArc dxArc;
            SFPoint P1, P2;
            float rd1, rd2;
            float MinX, MinY, MaxX, MaxY;
            int Dr_Cen_X, Dr_Cen_Y;

            MinX = 99999.0f;
            MinY = 99999.0f;
            MaxX = -99999.0f;
            MaxY = -99999.0f;
            Dr_Cen_X = bmp.Width;
            Dr_Cen_Y = bmp.Height / 2;
            if (DrawDXFDat.FCADImage == null)
                return;
            if (DrawDXFDat.FCADImage.FEntities == null)
                return;

            Pen blackPen = new Pen(Color.LightGreen, 2);

            Pen SelectPen = new Pen(Color.Red, 2);

            using (var graphics = Graphics.FromImage(bmp))
            {
                for (int i = 0; i < DrawDXFDat.FCADImage.FEntities.Entities.Count; i++)
                {
                    GetEntityName = DrawDXFDat.FCADImage.FEntities.Entities[i].ToString();
                    if (GetEntityName == "DXFImportReco.DXFLine")
                    {

                        dxLine = (DXFLine)DrawDXFDat.FCADImage.FEntities.Entities[i];

                        blackPen.Color = dxLine.FColor;

                        P1 = GetPoint(dxLine.Point1, ref DrawDXFDat);
                        P2 = GetPoint(dxLine.Point2, ref DrawDXFDat);

                        graphics.DrawLine(blackPen, P1.X, P1.Y, P2.X, P2.Y);

                    }
                    else if (GetEntityName == "DXFImportReco.DXFCircle")
                    {
                        dxCircle = (DXFCircle)DrawDXFDat.FCADImage.FEntities.Entities[i];

                        if (dxCircle.FColor == Color.Red)
                        {
                            blackPen.Color = dxCircle.FColor;
                        }
                        else
                        {
                            blackPen.Color = Color.GreenYellow;
                        }

                        rd1 = dxCircle.radius;
                        P1 = GetPoint(dxCircle.Point1, ref DrawDXFDat);
                        rd1 = rd1 * DrawDXFDat.FScale;
                        P1.X = P1.X - rd1;
                        P1.Y = P1.Y - rd1;

                        if (dxCircle.SelectObjFlag == true)
                        {
                            graphics.DrawEllipse(SelectPen, P1.X, P1.Y, rd1 * 2, rd1 * 2);
                        }
                        else
                        {
                            graphics.DrawEllipse(blackPen, P1.X, P1.Y, rd1 * 2, rd1 * 2);
                        }
                    }
                    else if (GetEntityName == "DXFImportReco.DXFArc")
                    {

                        dxArc = (DXFArc)DrawDXFDat.FCADImage.FEntities.Entities[i];

                        blackPen.Color = dxArc.FColor;

                        if (dxArc.pt1.X == 0)
                        {
                            rd1 = dxArc.radius; //Math.Abs(dxArc.radius * dxArc.ratio);
                            rd2 = dxArc.radius;
                        }
                        else
                        {
                            rd1 = dxArc.radius;
                            rd2 = dxArc.radius; //Math.Abs(dxArc.radius * ratio);
                        }

                        rd1 = dxArc.radius;
                        P1 = GetPoint(dxArc.Point1, ref DrawDXFDat);
                        rd1 = rd1 * DrawDXFDat.FScale;
                        rd2 = rd2 * DrawDXFDat.FScale;
                        P1.X = P1.X - rd1;
                        P1.Y = P1.Y - rd1;
                        float sA = -dxArc.startAngle, eA = -dxArc.endAngle;
                        if (dxArc.endAngle < dxArc.startAngle) sA = Conversion_Angle(sA);
                        eA -= sA;

                        if (eA == 0)
                        {
                            graphics.DrawEllipse(blackPen, P1.X, P1.Y, rd1 * 2, rd2 * 2);
                        }
                        else
                        {
                            graphics.DrawArc(blackPen, P1.X, P1.Y, rd1 * 2, rd2 * 2, sA, eA);//sA, eA);
                        }
                    }
                    else
                    {
                        //OutTxtStr += "\r\n" + GetEntityName;
                    }
                }
            }
        }

        public SFPoint GetPoint(SFPoint Point, ref DXFDataInfo DrawDXFDat)
        {
            SFPoint P;
            P.X = DrawDXFDat.Base.X + DrawDXFDat.FScale * (Point.X - DrawDXFDat.FS_W / 2 - DrawDXFDat.FS_W_Base);// * FParams.Scale.X);
            P.Y = DrawDXFDat.Base.Y - DrawDXFDat.FScale * (Point.Y - DrawDXFDat.FS_H / 2 - DrawDXFDat.FS_H_Base);// * FParams.Scale.Y);
            P.Z = Point.Z * DrawDXFDat.FScale;
            return P;
        }

        public void DrawDXFImageCalc(int DrImgW, int DrImgH, bool ChangeSelFlag, ref DXFDataInfo DrawDXFDat)
        {
            string GetEntityName;
            DXFLine dxLine;
            DXFCircle dxCircle;
            DXFArc dxArc;
            SFPoint P1, P2;
            float rd1, rd2;
            float MinX, MinY, MaxX, MaxY;

            MinX = 99999.0f;
            MinY = 99999.0f;
            MaxX = -99999.0f;
            MaxY = -99999.0f;
            if (DrawDXFDat.FCADImage == null)
                return;
            if (DrawDXFDat.FCADImage.FEntities == null)
                return;

            //Bitmap tmp = new Bitmap(bmp, bmp.Width, bmp.Height);

            for (int i = 0; i < DrawDXFDat.FCADImage.FEntities.Entities.Count; i++)
            {
                GetEntityName = DrawDXFDat.FCADImage.FEntities.Entities[i].ToString();
                if (GetEntityName == "DXFImportReco.DXFLine")
                {
                    /*
                    dxLine = (DXFLine)FCADImage.FEntities.Entities[i];

                    blackPen.Color = dxLine.FColor;

                    P1 = GetPoint(dxLine.Point1);
                    P2 = GetPoint(dxLine.Point2);

                    graphics.DrawLine(blackPen, P1.X, P1.Y, P2.X, P2.Y);
                    if (MinX > P1.X)
                    {
                        MinX = P1.X;
                    }
                    if (MaxX < P1.X)
                    {
                        MaxX = P1.X;
                    }
                    if (MinY > P1.Y)
                    {
                        MinY = P1.Y;
                    }
                    if (MaxY < P1.Y)
                    {
                        MaxY = P1.Y;
                    }

                    if (MinX > P2.X)
                    {
                        MinX = P2.X;
                    }
                    if (MaxX < P2.X)
                    {
                        MaxX = P2.X;
                    }
                    if (MinY > P2.Y)
                    {
                        MinY = P2.Y;
                    }
                    if (MaxY < P2.Y)
                    {
                        MaxY = P2.Y;
                    }
                    */
                }
                else if (GetEntityName == "DXFImportReco.DXFCircle")
                {
                    dxCircle = (DXFCircle)DrawDXFDat.FCADImage.FEntities.Entities[i];

                    if (DrawDXFDat.StartCalcFlag == true && ChangeSelFlag == true)
                    {
                        dxCircle.SelectObjFlag = false;
                    }

                    rd1 = dxCircle.radius;
                    P1 = dxCircle.Point1;
                    rd1 = rd1;// *FScale;

                    if (MinX > P1.X - rd1)
                    {
                        MinX = P1.X - rd1;
                    }
                    if (MaxX < P1.X + rd1)
                    {
                        MaxX = P1.X + rd1;
                    }
                    if (MinY > P1.Y - rd1)
                    {
                        MinY = P1.Y - rd1;
                    }
                    if (MaxY < P1.Y + rd1)
                    {
                        MaxY = P1.Y + rd1;
                    }


                }
                else if (GetEntityName == "DXFImportReco.DXFArc")
                {
                    dxArc = (DXFArc)DrawDXFDat.FCADImage.FEntities.Entities[i];

                    if (dxArc.pt1.X == 0)
                    {
                        rd1 = dxArc.radius; //Math.Abs(dxArc.radius * dxArc.ratio);
                        rd2 = dxArc.radius;
                    }
                    else
                    {
                        rd1 = dxArc.radius;
                        rd2 = dxArc.radius; //Math.Abs(dxArc.radius * ratio);
                    }

                    rd1 = dxArc.radius;
                    P1 = dxArc.Point1;

                    float sA = -dxArc.startAngle, eA = -dxArc.endAngle;
                    if (dxArc.endAngle < dxArc.startAngle) sA = Conversion_Angle(sA);
                    eA -= sA;

                    if (eA == 0)
                    {
                        if (MinX > P1.X - rd1)
                        {
                            MinX = P1.X - rd1;
                        }
                        if (MaxX < P1.X + rd1)
                        {
                            MaxX = P1.X + rd1;
                        }
                        if (MinY > P1.Y - rd1)
                        {
                            MinY = P1.Y - rd1;
                        }
                        if (MaxY < P1.Y + rd1)
                        {
                            MaxY = P1.Y + rd1;
                        }

                    }
                    else
                    {
                        if (MinX > P1.X - rd1)
                        {
                            MinX = P1.X - rd1;
                        }
                        if (MaxX < P1.X + rd1)
                        {
                            MaxX = P1.X + rd1;
                        }
                        if (MinY > P1.Y - rd1)
                        {
                            MinY = P1.Y - rd1;
                        }
                        if (MaxY < P1.Y + rd1)
                        {
                            MaxY = P1.Y + rd1;
                        }
                    }
                    
                }
                else
                {
                    //OutTxtStr += "\r\n" + GetEntityName;
                }
            }
            if (DrawDXFDat.StartCalcFlag == true)
            {
                DrawDXFDat.FS_W = MaxX - MinX;
                DrawDXFDat.FS_H = MaxY - MinY;
                DrawDXFDat.FS_W_Base = MinX;
                DrawDXFDat.FS_H_Base = MinY;

                float W_FScale, H_FScale;
                W_FScale = ((float)DrImgW * CalcReScalePer / 100.0f) / DrawDXFDat.FS_W;
                H_FScale = ((float)DrImgH * CalcReScalePer / 100.0f) / DrawDXFDat.FS_H;
                if (W_FScale < H_FScale)
                {
                    DrawDXFDat.FScale = W_FScale;
                }
                else
                {
                    DrawDXFDat.FScale = H_FScale;
                }

                //Base.X = DrImgW / 2;
                //Base.Y = DrImgH / 2;
                DrawDXFDat.BaseDefault.X = DrImgW / 2;
                DrawDXFDat.BaseDefault.Y = DrImgH / 2;
                DrawDXFDat.Base.X = DrawDXFDat.BaseDefault.X;
                DrawDXFDat.Base.Y = DrawDXFDat.BaseDefault.Y;


                DrawDXFDat.StartCalcFlag = false;
            }
            //bmp = tmp;
        }

        public float Conversion_Angle(float Val)
        {
            while (Val < 0) Val = Val + 360;
            return Val;
        }

        private void btLoadDXF_M_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "CAD Files|*.dxf";
            openFileDialog1.Title = "Select a CAD File";

            if (openFileDialog1.ShowDialog(this) != DialogResult.OK) return;
            if (openFileDialog1.FileName != null)
            {
                LoadDXFFilesB = openFileDialog1.FileName;
                LoadDXFDataB(LoadDXFFilesB);
            }
        }

        private void LoadDXFDataB(string LoadDXFFilePath)
        {
            if(File.Exists(LoadDXFFilePath) == false)
            {
                return;
            }
            DXFDatB.StartCalcFlag = true;
            if (DXFDatB.FCADImage != null)
            {
                DXFDatB.FCADImage = null;
            }
            DXFDatB.FCADImage = new CADImage();
            DXFDatB.Base.X = picDXFIndex_M.Width / 2;
            DXFDatB.Base.Y = picDXFIndex_M.Width / 2;

            BlankB = DrawFilledRectangle(picDXFIndex_M.Width, picDXFIndex_M.Height);

            //FCADImage.Base.Y = Bottom - 400;
            //FCADImage.Base.X = 100;
            DXFDatB.FScale = 1.0f;
            DXFDatB.FCADImage.LoadFromFile(LoadDXFFilePath);
            //DrawCADImage();
            Bitmap tmp = BlankB;
            //FCADImage.FScale = 4.0f;
            DrawDXFImageCalc(tmp.Width, tmp.Height, true, ref DXFDatB);
            DrawDXFImage2((Bitmap)tmp, ref DXFDatB);
            picDXFIndex_M.Image = (Image)tmp.Clone();
            //EF_DXFDataInfo GetEFDXFData = GetEF_DXFData(ref DXFDatA);
            //DisplayGetEFData(GetEFDXFData);
            ShotDetailInfo2 TestZoom = GetDXFShotZoomFocus(1, DXFDatB.FS_H);

            txtZoom_M.Text = TestZoom.ZoomNum.ToString();
            txtFocus_M.Text = TestZoom.FocusNum.ToString();
            txtDtZoom_M.Text = "11716";
            txtDtFocus_M.Text = "19081";

        }

        private void SaveRecoData(string SaveRecoFileNameStr, int SetDataTypeIndex, string SaveDXFFileName, ref WorkFuncInfo _WorkFuncInfo)
        {
            int ZoomCamListIndex = 0;

            if (SetDataTypeIndex == SETTYPE_FIX_FRONT)
            {
                ZoomCamListIndex = 0;
                _WorkFuncInfo.Rotation_Index = 0;
                _WorkFuncInfo.Motion_Index = 0;
                _WorkFuncInfo.Lamp_Index = 0;
            }
            else if (SetDataTypeIndex == SETTYPE_FIX_BACK)
            {
                ZoomCamListIndex = 2;
                _WorkFuncInfo.Rotation_Index = 0;
                _WorkFuncInfo.Motion_Index = 1;
                _WorkFuncInfo.Lamp_Index = 3;
            }
            else if (SetDataTypeIndex == SETTYPE_MOVE_FRONT)
            {
                ZoomCamListIndex = 1;
                _WorkFuncInfo.Rotation_Index = 1;
                _WorkFuncInfo.Motion_Index = 0;
                _WorkFuncInfo.Lamp_Index = 1;
            }
            else if (SetDataTypeIndex == SETTYPE_MOVE_BACK)
            {
                ZoomCamListIndex = 0;
                _WorkFuncInfo.Rotation_Index = 1;
                _WorkFuncInfo.Motion_Index = 0;
                _WorkFuncInfo.Lamp_Index = 4;
            }
            else if (SetDataTypeIndex == SETTYPE_MOVE_TOP)
            {
                ZoomCamListIndex = 3;
                _WorkFuncInfo.Rotation_Index = 1;
                _WorkFuncInfo.Motion_Index = 0;
                _WorkFuncInfo.Lamp_Index = 2;
            }

            if (SetDataTypeIndex == SETTYPE_FIX_FRONT || SetDataTypeIndex == SETTYPE_FIX_BACK)
            {
                RecoSetData.RecoDXFFileName = SaveDXFFileName;
                RecoSetData.RecoCamIndex = ZoomCamListIndex;
                RecoSetData.FullShot_ZoomNum = Convert.ToInt32(txtZoom_F.Text);
                RecoSetData.FullShot_FocusNum = Convert.ToInt32(txtFocus_F.Text);
                RecoSetData.DetailShot_ZoomNum = Convert.ToInt32(txtDtZoom_F.Text);
                RecoSetData.DetailShot_FocusNum = Convert.ToInt32(txtDtFocus_F.Text);

                RecoSetData.Motion_Move_X = double.Parse(txtXPos_F.Text);
                RecoSetData.Motion_Move_Y = double.Parse(txtYPos_F.Text);
                RecoSetData.Motion_Move_Z = double.Parse(txtZPos_F.Text);
                RecoSetData.Motion_Move_MR = double.Parse(txtMoveIndex_F.Text);

                RecoSetData.Move_R_FIX_Angle_Gap = Convert.ToDouble(txtFixAngleAddGap_F.Text);
                RecoSetData.Move_R_MOVE_Angle_Gap = Convert.ToDouble(txtMoveAngleAddGap_F.Text);
                RecoSetData.Back_R_MOVE_Angle_Gap = Convert.ToDouble(txtBackAngleAddGap_F.Text);
                RecoSetData.Top_R_MOVE_Angle_Gap = Convert.ToDouble(txtTopAngleAddGap_F.Text);
                RecoSetData.Object_Weight = Convert.ToDouble(txtPlangeThickness_F.Text);

                RecoSetData.Object_Hole_Distance = Convert.ToDouble(txtPinHoleDistance_F.Text);
                RecoSetData.Object_Diameter = Convert.ToDouble(txtPinHoleDiameter_F.Text);
                RecoSetData.Object_Pin_Diameter = Convert.ToDouble(txtCalcPinDiameter_F.Text);
                RecoSetData.Object_Pin_Height = Convert.ToDouble(txtCalcPinHeight_F.Text);

                RecoSetData.Object_TopHoleType = cboTopHolePos_F.SelectedIndex;

                RecoSetData.Object_TopDirHoleDistance = Convert.ToDouble(txtTopDirHoleDistance_F.Text);
                RecoSetData.Object_ShaftLength = Convert.ToDouble(txtShaftLength_F.Text);

                RecoSetData.DXF_RecoType = cboDXFType_F.SelectedIndex;
            }
            else if (SetDataTypeIndex == SETTYPE_MOVE_FRONT || SetDataTypeIndex == SETTYPE_MOVE_BACK || SetDataTypeIndex == SETTYPE_MOVE_TOP)
            {
                RecoSetData.RecoDXFFileName = SaveDXFFileName;
                RecoSetData.RecoCamIndex = ZoomCamListIndex;
                RecoSetData.FullShot_ZoomNum = Convert.ToInt32(txtZoom_M.Text);
                RecoSetData.FullShot_FocusNum = Convert.ToInt32(txtFocus_M.Text);
                RecoSetData.DetailShot_ZoomNum = Convert.ToInt32(txtDtZoom_M.Text);
                RecoSetData.DetailShot_FocusNum = Convert.ToInt32(txtDtFocus_M.Text);

                RecoSetData.Motion_Move_X = double.Parse(txtXPos_M.Text);
                RecoSetData.Motion_Move_Y = double.Parse(txtYPos_M.Text);
                RecoSetData.Motion_Move_Z = double.Parse(txtZPos_M.Text);
                RecoSetData.Motion_Move_MR = double.Parse(txtMoveIndex_M.Text);

                RecoSetData.Move_R_FIX_Angle_Gap = Convert.ToDouble(txtFixAngleAddGap_M.Text);
                RecoSetData.Move_R_MOVE_Angle_Gap = Convert.ToDouble(txtMoveAngleAddGap_M.Text);
                RecoSetData.Back_R_MOVE_Angle_Gap = Convert.ToDouble(txtBackAngleAddGap_M.Text);
                RecoSetData.Top_R_MOVE_Angle_Gap = Convert.ToDouble(txtTopAngleAddGap_M.Text);
                RecoSetData.Object_Weight = Convert.ToDouble(txtPlangeThickness_M.Text);

                RecoSetData.Object_Hole_Distance = Convert.ToDouble(txtPinHoleDistance_M.Text);
                RecoSetData.Object_Diameter = Convert.ToDouble(txtPinHoleDiameter_M.Text);
                RecoSetData.Object_Pin_Diameter = Convert.ToDouble(txtCalcPinDiameter_M.Text);
                RecoSetData.Object_Pin_Height = Convert.ToDouble(txtCalcPinHeight_M.Text);

                RecoSetData.Object_TopHoleType = cboTopHolePos_M.SelectedIndex;

                RecoSetData.Object_TopDirHoleDistance = Convert.ToDouble(txtTopDirHoleDistance_M.Text);
                RecoSetData.Object_ShaftLength = Convert.ToDouble(txtShaftLength_M.Text);

                RecoSetData.DXF_RecoType = cboDXFType_M.SelectedIndex;
            }

            SaveCfgFiles(SaveRecoFileNameStr);
        }


        private void SaveCfgFiles(string SaveCfgFilePath)
        {
            string WritePresetDataStr;

            StreamWriter sw = new StreamWriter(SaveCfgFilePath, false, Encoding.Default);

            WritePresetDataStr = RecoSetData.RecoDXFFileName + "|" + RecoSetData.RecoCapFileName + "|" + RecoSetData.RecoCamIndex.ToString();
            WritePresetDataStr = WritePresetDataStr + "|" + RecoSetData.RecoCamID + "|" + RecoSetData.ZoomFocus_PortNo.ToString() + "|" + RecoSetData.FullShot_ZoomNum.ToString();
            //WritePresetDataStr = RecoSetData.RecoDXFFileName + "|" + RecoSetData.RecoCapFileName + "|" + RecoSetData.RecoCamIndex.ToString();
            //WritePresetDataStr = WritePresetDataStr + "|" + RecoSetData.RecoCamID + "|" + RecoSetData.ZoomFocus_PortNo.ToString() + "|" + RecoSetData.FullShot_ZoomNum.ToString();
            WritePresetDataStr = WritePresetDataStr + "|" + RecoSetData.FullShot_FocusNum.ToString() + "|" + RecoSetData.DetailShot_ZoomNum.ToString() + "|" + RecoSetData.DetailShot_FocusNum.ToString();
            WritePresetDataStr = WritePresetDataStr + "|" + RecoSetData.Rotation_Index.ToString() + "|" + RecoSetData.Rotation_R.ToString() + "|" + RecoSetData.Motion_Index.ToString() + "|" + RecoSetData.Motion_Move_X.ToString();
            WritePresetDataStr = WritePresetDataStr + "|" + RecoSetData.Motion_Move_Y.ToString() + "|" + RecoSetData.Motion_Move_Z.ToString() + "|" + RecoSetData.Lamp_Index.ToString() + "|" + RecoSetData.Lamp_PortNo.ToString();
            WritePresetDataStr = WritePresetDataStr + "|" + RecoSetData.Lamp_SetChannel.ToString() + "|" + RecoSetData.Lamp_SetValue.ToString();
            WritePresetDataStr = WritePresetDataStr + "|" + RecoSetData.Move_R_FIX_Angle_Gap.ToString() + "|" + RecoSetData.Move_R_MOVE_Angle_Gap.ToString();
            WritePresetDataStr = WritePresetDataStr + "|" + RecoSetData.Move_R_Move_X_Gap.ToString() + "|" + RecoSetData.Move_R_Move_Y_Gap.ToString() + "|" + RecoSetData.Move_R_Move_Z_Gap.ToString();
            WritePresetDataStr = WritePresetDataStr + "|" + RecoSetData.Object_Weight.ToString() + "|" + RecoSetData.Object_Diameter.ToString() + "|" + RecoSetData.Object_Pin_Height.ToString() + "|" + RecoSetData.Object_Pin_Diameter.ToString();
            WritePresetDataStr = WritePresetDataStr + "|" + RecoSetData.Back_R_MOVE_Angle_Gap.ToString() + "|" + RecoSetData.Top_R_MOVE_Angle_Gap.ToString() + "|" + RecoSetData.Object_Hole_Distance.ToString();

            WritePresetDataStr = WritePresetDataStr + "|" + RecoSetData.Object_TopHoleType.ToString() + "|" + RecoSetData.DXF_RecoType.ToString() + "|" + RecoSetData.Object_Param3.ToString() + "|" + RecoSetData.Object_Param4.ToString() + "|" + RecoSetData.Object_Param5.ToString();
            WritePresetDataStr = WritePresetDataStr + "|" + RecoSetData.Object_TopDirHoleDistance.ToString() + "|" + RecoSetData.Motion_Move_MR.ToString() + "|" + RecoSetData.Object_ShaftLength.ToString() + "|" + RecoSetData.Object_Param9.ToString() + "|" + RecoSetData.Object_Param10.ToString() + "|" + RecoSetData.Object_Param11.ToString() + "|" + RecoSetData.Object_Param12.ToString();
            sw.Write(WritePresetDataStr);

            sw.Close();

        }


        private void LoadCfgFiles(string LoadCfgFilePath)
        {
            if (System.IO.File.Exists(LoadCfgFilePath))
            {
                string[] m_WordStr;

                StreamReader sr = new StreamReader(LoadCfgFilePath, Encoding.Default);
                string m_GetConfigDataStr = sr.ReadToEnd();
                sr.Close();

                RecoSetData.Move_R_FIX_Angle_Gap = 0.0;
                RecoSetData.Move_R_MOVE_Angle_Gap = 0.0;
                RecoSetData.Move_R_Move_X_Gap = 0.0;
                RecoSetData.Move_R_Move_Y_Gap = 0.0;
                RecoSetData.Move_R_Move_Z_Gap = 0.0;
                RecoSetData.Object_Weight = 0.0;
                RecoSetData.Object_Diameter = 0.0;
                RecoSetData.Object_Pin_Height = 45.0;
                RecoSetData.Object_Pin_Diameter = 16.0;
                RecoSetData.Back_R_MOVE_Angle_Gap = 0.0;
                RecoSetData.Top_R_MOVE_Angle_Gap = 0.0;
                RecoSetData.Object_Hole_Distance = 0.0;

                m_WordStr = m_GetConfigDataStr.Split(SaveDiv);
                if (m_WordStr.Length > 0)
                {
                    RecoSetData.RecoDXFFileName = m_WordStr[0];
                    RecoSetData.RecoCapFileName = m_WordStr[1];
                    RecoSetData.RecoCamIndex = Convert.ToInt32(m_WordStr[2]);
                    RecoSetData.RecoCamID = m_WordStr[3];
                    RecoSetData.ZoomFocus_PortNo = Convert.ToInt32(m_WordStr[4]);
                    RecoSetData.FullShot_ZoomNum = Convert.ToInt32(m_WordStr[5]);
                    RecoSetData.FullShot_FocusNum = Convert.ToInt32(m_WordStr[6]);
                    RecoSetData.DetailShot_ZoomNum = Convert.ToInt32(m_WordStr[7]);
                    RecoSetData.DetailShot_FocusNum = Convert.ToInt32(m_WordStr[8]);
                    RecoSetData.Rotation_Index = Convert.ToInt32(m_WordStr[9]);
                    RecoSetData.Rotation_R = Convert.ToDouble(m_WordStr[10]);
                    RecoSetData.Motion_Index = Convert.ToInt32(m_WordStr[11]);
                    RecoSetData.Motion_Move_X = Convert.ToDouble(m_WordStr[12]);
                    RecoSetData.Motion_Move_Y = Convert.ToDouble(m_WordStr[13]);
                    RecoSetData.Motion_Move_Z = Convert.ToDouble(m_WordStr[14]);
                    RecoSetData.Lamp_Index = Convert.ToInt32(m_WordStr[15]);
                    RecoSetData.Lamp_PortNo = Convert.ToInt32(m_WordStr[16]);
                    RecoSetData.Lamp_SetChannel = Convert.ToInt32(m_WordStr[17]);
                    RecoSetData.Lamp_SetValue = Convert.ToInt32(m_WordStr[18]);
                    if (m_WordStr.Length > 19)
                    {
                        RecoSetData.Move_R_FIX_Angle_Gap = Convert.ToDouble(m_WordStr[19]);
                        RecoSetData.Move_R_MOVE_Angle_Gap = Convert.ToDouble(m_WordStr[20]);
                        RecoSetData.Move_R_Move_X_Gap = Convert.ToDouble(m_WordStr[21]);
                        RecoSetData.Move_R_Move_Y_Gap = Convert.ToDouble(m_WordStr[22]);
                        RecoSetData.Move_R_Move_Z_Gap = Convert.ToDouble(m_WordStr[23]);
                        RecoSetData.Object_Weight = Convert.ToDouble(m_WordStr[24]);
                        RecoSetData.Object_Diameter = Convert.ToDouble(m_WordStr[25]);
                        RecoSetData.Object_Pin_Height = Convert.ToDouble(m_WordStr[26]);
                        RecoSetData.Object_Pin_Diameter = Convert.ToDouble(m_WordStr[27]);
                        RecoSetData.Back_R_MOVE_Angle_Gap = Convert.ToDouble(m_WordStr[28]);
                        RecoSetData.Top_R_MOVE_Angle_Gap = Convert.ToDouble(m_WordStr[29]);
                        RecoSetData.Object_Hole_Distance = Convert.ToDouble(m_WordStr[30]);
                        if (m_WordStr.Length > 31)
                        {
                            RecoSetData.Object_TopHoleType = Convert.ToInt32(m_WordStr[31]);
                            RecoSetData.DXF_RecoType = Convert.ToInt32(m_WordStr[32]);
                            RecoSetData.Object_Param3 = Convert.ToInt32(m_WordStr[33]);
                            RecoSetData.Object_Param4 = Convert.ToInt32(m_WordStr[34]);
                            RecoSetData.Object_Param5 = Convert.ToInt32(m_WordStr[35]);

                            RecoSetData.Object_TopDirHoleDistance = Convert.ToDouble(m_WordStr[36]);
                            RecoSetData.Motion_Move_MR = Convert.ToDouble(m_WordStr[37]);
                            RecoSetData.Object_ShaftLength = Convert.ToDouble(m_WordStr[38]);
                            RecoSetData.Object_Param9 = Convert.ToDouble(m_WordStr[39]);
                            RecoSetData.Object_Param10 = Convert.ToDouble(m_WordStr[40]);
                            RecoSetData.Object_Param11 = Convert.ToDouble(m_WordStr[41]);
                            RecoSetData.Object_Param12 = Convert.ToDouble(m_WordStr[42]);
                        }
                    }

                }
            }
        }

        private void DisplayRecoData(int Fix_Move_Type)
        {
            if (Fix_Move_Type == 1)
            {
                txtZoom_F.Text = RecoSetData.FullShot_ZoomNum.ToString();
                txtFocus_F.Text = RecoSetData.FullShot_FocusNum.ToString();
                txtDtZoom_F.Text = RecoSetData.DetailShot_ZoomNum.ToString();
                txtDtFocus_F.Text = RecoSetData.DetailShot_FocusNum.ToString();

                txtXPos_F.Text = RecoSetData.Motion_Move_X.ToString("#.0000");
                txtYPos_F.Text = RecoSetData.Motion_Move_Y.ToString("#.0000");
                txtZPos_F.Text = RecoSetData.Motion_Move_Z.ToString("#.0000");
                txtMoveIndex_F.Text = RecoSetData.Motion_Move_MR.ToString("#.0000");

                txtFixAngleAddGap_F.Text = RecoSetData.Move_R_FIX_Angle_Gap.ToString("#.0000");
                txtMoveAngleAddGap_F.Text = RecoSetData.Move_R_MOVE_Angle_Gap.ToString("#.0000");
                txtBackAngleAddGap_F.Text = RecoSetData.Back_R_MOVE_Angle_Gap.ToString("#.0000");
                txtTopAngleAddGap_F.Text = RecoSetData.Top_R_MOVE_Angle_Gap.ToString("#.0000");
                txtPlangeThickness_F.Text = RecoSetData.Object_Weight.ToString("#.0000");

                txtPinHoleDistance_F.Text = RecoSetData.Object_Hole_Distance.ToString("#.0000");
                txtPinHoleDiameter_F.Text = RecoSetData.Object_Diameter.ToString("#.0000");
                txtCalcPinDiameter_F.Text = RecoSetData.Object_Pin_Diameter.ToString("#.0000");
                txtCalcPinHeight_F.Text = RecoSetData.Object_Pin_Height.ToString("#.0000");

                if (RecoSetData.Object_TopHoleType > -1)
                {
                    cboTopHolePos_F.SelectedIndex = RecoSetData.Object_TopHoleType;
                }

                txtTopDirHoleDistance_F.Text = RecoSetData.Object_TopDirHoleDistance.ToString("#.0000");
                txtShaftLength_F.Text = RecoSetData.Object_ShaftLength.ToString("#.0000");

                if (RecoSetData.DXF_RecoType > -1)
                {
                    cboDXFType_F.SelectedIndex = RecoSetData.DXF_RecoType;
                }
            }
            else if (Fix_Move_Type == 2)
            {
                txtZoom_M.Text = RecoSetData.FullShot_ZoomNum.ToString();
                txtFocus_M.Text = RecoSetData.FullShot_FocusNum.ToString();
                txtDtZoom_M.Text = RecoSetData.DetailShot_ZoomNum.ToString();
                txtDtFocus_M.Text = RecoSetData.DetailShot_FocusNum.ToString();

                txtXPos_M.Text = RecoSetData.Motion_Move_X.ToString("#.0000");
                txtYPos_M.Text = RecoSetData.Motion_Move_Y.ToString("#.0000");
                txtZPos_M.Text = RecoSetData.Motion_Move_Z.ToString("#.0000");
                txtMoveIndex_M.Text = RecoSetData.Motion_Move_MR.ToString("#.0000");

                txtFixAngleAddGap_M.Text = RecoSetData.Move_R_FIX_Angle_Gap.ToString("#.0000");
                txtMoveAngleAddGap_M.Text = RecoSetData.Move_R_MOVE_Angle_Gap.ToString("#.0000");
                txtBackAngleAddGap_M.Text = RecoSetData.Back_R_MOVE_Angle_Gap.ToString("#.0000");
                txtTopAngleAddGap_M.Text = RecoSetData.Top_R_MOVE_Angle_Gap.ToString("#.0000");
                txtPlangeThickness_M.Text = RecoSetData.Object_Weight.ToString("#.0000");

                txtPinHoleDistance_M.Text = RecoSetData.Object_Hole_Distance.ToString("#.0000");
                txtPinHoleDiameter_M.Text = RecoSetData.Object_Diameter.ToString("#.0000");
                txtCalcPinDiameter_M.Text = RecoSetData.Object_Pin_Diameter.ToString("#.0000");
                txtCalcPinHeight_M.Text = RecoSetData.Object_Pin_Height.ToString("#.0000");

                if (RecoSetData.Object_TopHoleType > -1)
                {
                    cboTopHolePos_M.SelectedIndex = RecoSetData.Object_TopHoleType;
                }

                txtTopDirHoleDistance_M.Text = RecoSetData.Object_TopDirHoleDistance.ToString("#.0000");
                txtShaftLength_M.Text = RecoSetData.Object_ShaftLength.ToString("#.0000");

                if (RecoSetData.DXF_RecoType > -1)
                {
                    cboDXFType_M.SelectedIndex = RecoSetData.DXF_RecoType;
                }
            }
        }

        private void TopHoleTypeList()
        {
            cboTopHolePos_F.Items.Clear();
            cboTopHolePos_F.Items.Add("NONE");
            cboTopHolePos_F.Items.Add("LEFT");
            cboTopHolePos_F.Items.Add("RIGHT");

            cboTopHolePos_M.Items.Clear();
            cboTopHolePos_M.Items.Add("NONE");
            cboTopHolePos_M.Items.Add("LEFT");
            cboTopHolePos_M.Items.Add("RIGHT");
        }

        private void DXFTypeList()
        {
            cboDXFType_F.Items.Clear();
            cboDXFType_F.Items.Add("NONE");
            cboDXFType_F.Items.Add("홀 Type");
            cboDXFType_F.Items.Add("홀 & 단일직선 Type");
            cboDXFType_F.Items.Add("직선 Type");
            cboDXFType_F.SelectedIndex = 1;

            cboDXFType_M.Items.Clear();
            cboDXFType_M.Items.Add("NONE");
            cboDXFType_M.Items.Add("홀 Type");
            cboDXFType_M.Items.Add("홀 & 단일직선 Type");
            cboDXFType_M.Items.Add("직선 Type");
            cboDXFType_M.SelectedIndex = 1;
        }

        private void CopyDXFFiles(string SrcDxfFilePath, string TrgDxfFilePath)
        {
            if (File.Exists(SrcDxfFilePath) == true)
            {
                if (File.Exists(TrgDxfFilePath) == true)
                {
                    File.Delete(TrgDxfFilePath);
                }
                File.Copy(SrcDxfFilePath, TrgDxfFilePath);
            }
        }

        private void SetRecoObjLock(int GetSetDataIndex)
        {
            if(GetSetDataIndex == SETTYPE_FIX_FRONT)
            {
                txtZoom_F.Enabled = false;
                txtFocus_F.Enabled = false;

                txtXPos_F.Enabled = false;
                txtYPos_F.Enabled = false;
                txtZPos_F.Enabled = false;
                txtMoveIndex_F.Enabled = false;

                txtFixAngleAddGap_F.Enabled = false;
                txtMoveAngleAddGap_F.Enabled = false;
                txtBackAngleAddGap_F.Enabled = false;
                txtTopAngleAddGap_F.Enabled = false;
                txtPlangeThickness_F.Enabled = false;
                txtPinHoleDistance_F.Enabled = false;
                txtPinHoleDiameter_F.Enabled = false;
                txtCalcPinDiameter_F.Enabled = false;
                txtCalcPinHeight_F.Enabled = false;
                cboTopHolePos_F.Enabled = false;
                txtTopDirHoleDistance_F.Enabled = false;
                txtShaftLength_F.Enabled = false;
            }
        }


        private void LoadCalibSetFiles(string SaveCalibFilePath)
        {
            if (System.IO.File.Exists(SaveCalibFilePath))
            {
                string[] m_WordStr;

                StreamReader sr = new StreamReader(SaveCalibFilePath, Encoding.Default);
                string m_GetConfigDataStr = sr.ReadToEnd();
                sr.Close();

                m_WordStr = m_GetConfigDataStr.Split(SaveDiv);
                if (m_WordStr.Length > 0)
                {
                    RealCalibrationData_A.ddp_X = Convert.ToDouble(m_WordStr[0]);
                    RealCalibrationData_A.ddp_Y = Convert.ToDouble(m_WordStr[1]);
                    RealCalibrationData_B.ddp_X = Convert.ToDouble(m_WordStr[2]);
                    RealCalibrationData_B.ddp_Y = Convert.ToDouble(m_WordStr[3]);
                    RealCalibrationData_C.ddp_X = Convert.ToDouble(m_WordStr[4]);
                    RealCalibrationData_C.ddp_Y = Convert.ToDouble(m_WordStr[5]);
                    RealCalibrationData_D.ddp_X = Convert.ToDouble(m_WordStr[6]);
                    RealCalibrationData_D.ddp_Y = Convert.ToDouble(m_WordStr[7]);
                    if (m_WordStr.Length > 8)
                    {
                        RealCalibrationData_E.ddp_X = Convert.ToDouble(m_WordStr[8]);
                        RealCalibrationData_E.ddp_Y = Convert.ToDouble(m_WordStr[9]);
                        RealCalibrationData_F.ddp_X = Convert.ToDouble(m_WordStr[10]);
                        RealCalibrationData_F.ddp_Y = Convert.ToDouble(m_WordStr[11]);
                        RealCalibrationData_G.ddp_X = Convert.ToDouble(m_WordStr[12]);
                        RealCalibrationData_G.ddp_Y = Convert.ToDouble(m_WordStr[13]);
                        RealCalibrationData_H.ddp_X = Convert.ToDouble(m_WordStr[14]);
                        RealCalibrationData_H.ddp_Y = Convert.ToDouble(m_WordStr[15]);
                    }
                }
            }
        }


        private ShotDetailInfo2 GetDXFShotZoomFocus(int GetCamShotIndex, double DXFObjLength)
        {
            ShotDetailInfo2 ResDXFZoomFocus;
            int CmpObjLength;
            int CmpObjStartNum, CmpObjOtherNum;

            ResDXFZoomFocus.ShotType = 0;
            ResDXFZoomFocus.ShotGapDistance = 0;
            ResDXFZoomFocus.ZoomNum = 0;
            ResDXFZoomFocus.FocusNum = 0;

            CmpObjLength = (int)(DXFObjLength * 10.0);

            CmpObjStartNum = CmpObjLength / 100;
            CmpObjOtherNum = CmpObjLength % 100;

            if (GetCamShotIndex == 0)
            {
                ResDXFZoomFocus.ShotType = DxfShotDataList_A[CmpObjStartNum].ShotType;
                ResDXFZoomFocus.ShotGapDistance = DxfShotDataList_A[CmpObjStartNum].ShotGapDistance;
                ResDXFZoomFocus.ZoomNum = DxfShotDataList_A[CmpObjStartNum].ZoomNum - CmpObjOtherNum * (DxfShotDataList_A[CmpObjStartNum].ZoomNum - DxfShotDataList_A[CmpObjStartNum + 1].ZoomNum) / 100;
                ResDXFZoomFocus.FocusNum = DxfShotDataList_A[CmpObjStartNum].FocusNum;
            }
            else if (GetCamShotIndex == 1)
            {
                ResDXFZoomFocus.ShotType = DxfShotDataList_B[CmpObjStartNum].ShotType;
                ResDXFZoomFocus.ShotGapDistance = DxfShotDataList_B[CmpObjStartNum].ShotGapDistance;
                ResDXFZoomFocus.ZoomNum = DxfShotDataList_B[CmpObjStartNum].ZoomNum - CmpObjOtherNum * (DxfShotDataList_B[CmpObjStartNum].ZoomNum - DxfShotDataList_B[CmpObjStartNum + 1].ZoomNum) / 100;
                ResDXFZoomFocus.FocusNum = DxfShotDataList_B[CmpObjStartNum].FocusNum;
            }


            return ResDXFZoomFocus;
        }


        private void SetDXFShotDataInit()
        {
            int i;
            int i_gap1, i_gap2, i_gap;

            //==================================
            // A
            //==================================

            int i_gap_Center;

            for (int j = 0; j < 19; j++)
            {
                i_gap_Center = (130 - 40) / 2 + 40;
                i_gap1 = (i_gap_Center - j * 10);
                i_gap2 = i_gap_Center - 40;
                if (i_gap1 < 0)
                {
                    i_gap1 = 0 - i_gap1;
                }
                i_gap = i_gap1 - i_gap2;
                DxfShotDataList_A[j].ZoomNum = 10939 - (j * 10 - 40) * (10939 - 4907) / (130 - 40);// +i_gap * 328 / 10;
                DxfShotDataList_A[j].FocusNum = 19584;
            }

            /*
            i = 0;
            DxfShotDataList_A[i].ShotType = 0;
            DxfShotDataList_A[i].ShotGapDistance = 0.0;
            DxfShotDataList_A[i].ZoomNum = 0;
            DxfShotDataList_A[i].FocusNum = 19678;
            i = 1;
            DxfShotDataList_A[i].ShotType = 0;
            DxfShotDataList_A[i].ShotGapDistance = 0.0;
            DxfShotDataList_A[i].ZoomNum = 0;
            DxfShotDataList_A[i].FocusNum = 19678;
            i = 2;
            DxfShotDataList_A[i].ShotType = 0;
            DxfShotDataList_A[i].ShotGapDistance = 0.0;
            DxfShotDataList_A[i].ZoomNum = 0;
            DxfShotDataList_A[i].FocusNum = 19678;
            i = 3;
            DxfShotDataList_A[i].ShotType = 0;
            DxfShotDataList_A[i].ShotGapDistance = 0.0;
            DxfShotDataList_A[i].ZoomNum = 0;
            DxfShotDataList_A[i].FocusNum = 19678;
            */
            i = 4;
            DxfShotDataList_A[i].ShotType = 0;
            DxfShotDataList_A[i].ShotGapDistance = 0.0;
            DxfShotDataList_A[i].ZoomNum = 10939;
            //DxfShotDataList_A[i].FocusNum = 19678;
            i = 5;
            DxfShotDataList_A[i].ShotType = 0;
            DxfShotDataList_A[i].ShotGapDistance = 0.0;
            DxfShotDataList_A[i].ZoomNum = 9797;
            //DxfShotDataList_A[i].FocusNum = 19678;
            i = 6;
            DxfShotDataList_A[i].ShotType = 0;
            DxfShotDataList_A[i].ShotGapDistance = 0.0;
            DxfShotDataList_A[i].ZoomNum = 8958;
            //DxfShotDataList_A[i].FocusNum = 19678;
            i = 7;
            DxfShotDataList_A[i].ShotType = 0;
            DxfShotDataList_A[i].ShotGapDistance = 0.0;
            DxfShotDataList_A[i].ZoomNum = 8208;
            //DxfShotDataList_A[i].FocusNum = 19678;
            i = 8;
            DxfShotDataList_A[i].ShotType = 0;
            DxfShotDataList_A[i].ShotGapDistance = 0.0;
            DxfShotDataList_A[i].ZoomNum = 7602;
            //DxfShotDataList_A[i].FocusNum = 19678;
            i = 9;
            DxfShotDataList_A[i].ShotType = 0;
            DxfShotDataList_A[i].ShotGapDistance = 0.0;
            DxfShotDataList_A[i].ZoomNum = 7013;
            //DxfShotDataList_A[i].FocusNum = 19678;
            i = 10;
            DxfShotDataList_A[i].ShotType = 0;
            DxfShotDataList_A[i].ShotGapDistance = 0.0;
            DxfShotDataList_A[i].ZoomNum = 6460;
            //DxfShotDataList_A[i].FocusNum = 19678;
            i = 11;
            DxfShotDataList_A[i].ShotType = 0;
            DxfShotDataList_A[i].ShotGapDistance = 0.0;
            DxfShotDataList_A[i].ZoomNum = 5924;
            //DxfShotDataList_A[i].FocusNum = 19678;
            i = 12;
            DxfShotDataList_A[i].ShotType = 0;
            DxfShotDataList_A[i].ShotGapDistance = 0.0;
            DxfShotDataList_A[i].ZoomNum = 5425;
            //DxfShotDataList_A[i].FocusNum = 19678;
            i = 13;
            DxfShotDataList_A[i].ShotType = 0;
            DxfShotDataList_A[i].ShotGapDistance = 0.0;
            DxfShotDataList_A[i].ZoomNum = 4907;
            //DxfShotDataList_A[i].FocusNum = 19678;
            i = 14;
            DxfShotDataList_A[i].ShotType = 0;
            DxfShotDataList_A[i].ShotGapDistance = 0.0;
            DxfShotDataList_A[i].ZoomNum = 4386;
            //DxfShotDataList_A[i].FocusNum = 19678;
            i = 15;
            DxfShotDataList_A[i].ShotType = 0;
            DxfShotDataList_A[i].ShotGapDistance = 0.0;
            DxfShotDataList_A[i].ZoomNum = 3852;
            //DxfShotDataList_A[i].FocusNum = 19678;
            i = 16;
            DxfShotDataList_A[i].ShotType = 0;
            DxfShotDataList_A[i].ShotGapDistance = 0.0;
            //DxfShotDataList_A[i].ZoomNum = 0;
            //DxfShotDataList_A[i].FocusNum = 19678;
            i = 17;
            DxfShotDataList_A[i].ShotType = 0;
            DxfShotDataList_A[i].ShotGapDistance = 0.0;
            //DxfShotDataList_A[i].ZoomNum = 0;
            //DxfShotDataList_A[i].FocusNum = 19678;
            i = 18;
            DxfShotDataList_A[i].ShotType = 0;
            DxfShotDataList_A[i].ShotGapDistance = 0.0;
            //DxfShotDataList_A[i].ZoomNum = 0;
            //DxfShotDataList_A[i].FocusNum = 19678;


            //==================================
            // B
            //==================================

            for (int j = 0; j < 19; j++)
            {
                DxfShotDataList_B[j].ZoomNum = 10403 - (j * 10 - 40) * (10403 - 3692) / (130 - 40);
                DxfShotDataList_B[j].FocusNum = 19081;
            }
            /*
            i = 0;
            DxfShotDataList_B[i].ShotType = 0;
            DxfShotDataList_B[i].ShotGapDistance = 0.0;
            DxfShotDataList_B[i].ZoomNum = 0;
            DxfShotDataList_B[i].FocusNum = 0;
            i = 1;
            DxfShotDataList_B[i].ShotType = 0;
            DxfShotDataList_B[i].ShotGapDistance = 0.0;
            DxfShotDataList_B[i].ZoomNum = 0;
            DxfShotDataList_B[i].FocusNum = 0;
            i = 2;
            DxfShotDataList_B[i].ShotType = 0;
            DxfShotDataList_B[i].ShotGapDistance = 0.0;
            DxfShotDataList_B[i].ZoomNum = 0;
            DxfShotDataList_B[i].FocusNum = 0;
            i = 3;
            DxfShotDataList_B[i].ShotType = 0;
            DxfShotDataList_B[i].ShotGapDistance = 0.0;
            DxfShotDataList_B[i].ZoomNum = 0;
            DxfShotDataList_B[i].FocusNum = 0;
            */
            i = 4;
            DxfShotDataList_B[i].ShotType = 0;
            DxfShotDataList_B[i].ShotGapDistance = 0.0;
            DxfShotDataList_B[i].ZoomNum = 10405;
            DxfShotDataList_B[i].FocusNum = 18995;
            i = 5;
            DxfShotDataList_B[i].ShotType = 0;
            DxfShotDataList_B[i].ShotGapDistance = 0.0;
            DxfShotDataList_B[i].ZoomNum = 9322;
            DxfShotDataList_B[i].FocusNum = 18995;
            i = 6;
            DxfShotDataList_B[i].ShotType = 0;
            DxfShotDataList_B[i].ShotGapDistance = 0.0;
            DxfShotDataList_B[i].ZoomNum = 8452;
            DxfShotDataList_B[i].FocusNum = 18995;
            i = 7;
            DxfShotDataList_B[i].ShotType = 0;
            DxfShotDataList_B[i].ShotGapDistance = 0.0;
            DxfShotDataList_B[i].ZoomNum = 7706;
            DxfShotDataList_B[i].FocusNum = 18995;
            i = 8;
            DxfShotDataList_B[i].ShotType = 0;
            DxfShotDataList_B[i].ShotGapDistance = 0.0;
            DxfShotDataList_B[i].ZoomNum = 7049;
            DxfShotDataList_B[i].FocusNum = 18995;
            i = 9;
            DxfShotDataList_B[i].ShotType = 0;
            DxfShotDataList_B[i].ShotGapDistance = 0.0;
            DxfShotDataList_B[i].ZoomNum = 6446;
            DxfShotDataList_B[i].FocusNum = 18995;
            i = 10;
            DxfShotDataList_B[i].ShotType = 0;
            DxfShotDataList_B[i].ShotGapDistance = 0.0;
            DxfShotDataList_B[i].ZoomNum = 5860;
            DxfShotDataList_B[i].FocusNum = 18995;
            i = 11;
            DxfShotDataList_B[i].ShotType = 0;
            DxfShotDataList_B[i].ShotGapDistance = 0.0;
            DxfShotDataList_B[i].ZoomNum = 5292;
            DxfShotDataList_B[i].FocusNum = 18995;
            i = 12;
            DxfShotDataList_B[i].ShotType = 0;
            DxfShotDataList_B[i].ShotGapDistance = 0.0;
            DxfShotDataList_B[i].ZoomNum = 4741;
            DxfShotDataList_B[i].FocusNum = 18995;
            i = 13;
            DxfShotDataList_B[i].ShotType = 0;
            DxfShotDataList_B[i].ShotGapDistance = 0.0;
            DxfShotDataList_B[i].ZoomNum = 4191;
            DxfShotDataList_B[i].FocusNum = 18995;

            /*
            i = 14;
            DxfShotDataList_B[i].ShotType = 0;
            DxfShotDataList_B[i].ShotGapDistance = 0.0;
            DxfShotDataList_B[i].ZoomNum = 0;
            DxfShotDataList_B[i].FocusNum = 0;
            i = 15;
            DxfShotDataList_B[i].ShotType = 0;
            DxfShotDataList_B[i].ShotGapDistance = 0.0;
            DxfShotDataList_B[i].ZoomNum = 0;
            DxfShotDataList_B[i].FocusNum = 0;
            i = 16;
            DxfShotDataList_B[i].ShotType = 0;
            DxfShotDataList_B[i].ShotGapDistance = 0.0;
            DxfShotDataList_B[i].ZoomNum = 0;
            DxfShotDataList_B[i].FocusNum = 0;
            i = 17;
            DxfShotDataList_B[i].ShotType = 0;
            DxfShotDataList_B[i].ShotGapDistance = 0.0;
            DxfShotDataList_B[i].ZoomNum = 0;
            DxfShotDataList_B[i].FocusNum = 0;
            i = 18;
            DxfShotDataList_B[i].ShotType = 0;
            DxfShotDataList_B[i].ShotGapDistance = 0.0;
            DxfShotDataList_B[i].ZoomNum = 0;
            DxfShotDataList_B[i].FocusNum = 0;
            */

            //==================================
            // C
            //==================================
            i = 0;
            DxfShotDataList_C[i].ShotType = 0;
            DxfShotDataList_C[i].ShotGapDistance = 0.0;
            DxfShotDataList_C[i].ZoomNum = 0;
            DxfShotDataList_C[i].FocusNum = 0;
            i = 1;
            DxfShotDataList_C[i].ShotType = 0;
            DxfShotDataList_C[i].ShotGapDistance = 0.0;
            DxfShotDataList_C[i].ZoomNum = 0;
            DxfShotDataList_C[i].FocusNum = 0;
            i = 2;
            DxfShotDataList_C[i].ShotType = 0;
            DxfShotDataList_C[i].ShotGapDistance = 0.0;
            DxfShotDataList_C[i].ZoomNum = 0;
            DxfShotDataList_C[i].FocusNum = 0;
            i = 3;
            DxfShotDataList_C[i].ShotType = 0;
            DxfShotDataList_C[i].ShotGapDistance = 0.0;
            DxfShotDataList_C[i].ZoomNum = 0;
            DxfShotDataList_C[i].FocusNum = 0;
            i = 4;
            DxfShotDataList_C[i].ShotType = 0;
            DxfShotDataList_C[i].ShotGapDistance = 0.0;
            DxfShotDataList_C[i].ZoomNum = 0;
            DxfShotDataList_C[i].FocusNum = 0;
            i = 5;
            DxfShotDataList_C[i].ShotType = 0;
            DxfShotDataList_C[i].ShotGapDistance = 0.0;
            DxfShotDataList_C[i].ZoomNum = 0;
            DxfShotDataList_C[i].FocusNum = 0;
            i = 6;
            DxfShotDataList_C[i].ShotType = 0;
            DxfShotDataList_C[i].ShotGapDistance = 0.0;
            DxfShotDataList_C[i].ZoomNum = 0;
            DxfShotDataList_C[i].FocusNum = 0;
            i = 7;
            DxfShotDataList_C[i].ShotType = 0;
            DxfShotDataList_C[i].ShotGapDistance = 0.0;
            DxfShotDataList_C[i].ZoomNum = 0;
            DxfShotDataList_C[i].FocusNum = 0;
            i = 8;
            DxfShotDataList_C[i].ShotType = 0;
            DxfShotDataList_C[i].ShotGapDistance = 0.0;
            DxfShotDataList_C[i].ZoomNum = 0;
            DxfShotDataList_C[i].FocusNum = 0;
            i = 9;
            DxfShotDataList_C[i].ShotType = 0;
            DxfShotDataList_C[i].ShotGapDistance = 0.0;
            DxfShotDataList_C[i].ZoomNum = 0;
            DxfShotDataList_C[i].FocusNum = 0;
            i = 10;
            DxfShotDataList_C[i].ShotType = 0;
            DxfShotDataList_C[i].ShotGapDistance = 0.0;
            DxfShotDataList_C[i].ZoomNum = 0;
            DxfShotDataList_C[i].FocusNum = 0;
            i = 11;
            DxfShotDataList_C[i].ShotType = 0;
            DxfShotDataList_C[i].ShotGapDistance = 0.0;
            DxfShotDataList_C[i].ZoomNum = 0;
            DxfShotDataList_C[i].FocusNum = 0;
            i = 12;
            DxfShotDataList_C[i].ShotType = 0;
            DxfShotDataList_C[i].ShotGapDistance = 0.0;
            DxfShotDataList_C[i].ZoomNum = 0;
            DxfShotDataList_C[i].FocusNum = 0;
            i = 13;
            DxfShotDataList_C[i].ShotType = 0;
            DxfShotDataList_C[i].ShotGapDistance = 0.0;
            DxfShotDataList_C[i].ZoomNum = 0;
            DxfShotDataList_C[i].FocusNum = 0;
            i = 14;
            DxfShotDataList_C[i].ShotType = 0;
            DxfShotDataList_C[i].ShotGapDistance = 0.0;
            DxfShotDataList_C[i].ZoomNum = 0;
            DxfShotDataList_C[i].FocusNum = 0;
            i = 15;
            DxfShotDataList_C[i].ShotType = 0;
            DxfShotDataList_C[i].ShotGapDistance = 0.0;
            DxfShotDataList_C[i].ZoomNum = 0;
            DxfShotDataList_C[i].FocusNum = 0;
            i = 16;
            DxfShotDataList_C[i].ShotType = 0;
            DxfShotDataList_C[i].ShotGapDistance = 0.0;
            DxfShotDataList_C[i].ZoomNum = 0;
            DxfShotDataList_C[i].FocusNum = 0;
            i = 17;
            DxfShotDataList_C[i].ShotType = 0;
            DxfShotDataList_C[i].ShotGapDistance = 0.0;
            DxfShotDataList_C[i].ZoomNum = 0;
            DxfShotDataList_C[i].FocusNum = 0;
            i = 18;
            DxfShotDataList_C[i].ShotType = 0;
            DxfShotDataList_C[i].ShotGapDistance = 0.0;
            DxfShotDataList_C[i].ZoomNum = 0;
            DxfShotDataList_C[i].FocusNum = 0;


            //==================================
            // D
            //==================================
            i = 0;
            DxfShotDataList_D[i].ShotType = 0;
            DxfShotDataList_D[i].ShotGapDistance = 0.0;
            DxfShotDataList_D[i].ZoomNum = 0;
            DxfShotDataList_D[i].FocusNum = 0;
            i = 1;
            DxfShotDataList_D[i].ShotType = 0;
            DxfShotDataList_D[i].ShotGapDistance = 0.0;
            DxfShotDataList_D[i].ZoomNum = 0;
            DxfShotDataList_D[i].FocusNum = 0;
            i = 2;
            DxfShotDataList_D[i].ShotType = 0;
            DxfShotDataList_D[i].ShotGapDistance = 0.0;
            DxfShotDataList_D[i].ZoomNum = 0;
            DxfShotDataList_D[i].FocusNum = 0;
            i = 3;
            DxfShotDataList_D[i].ShotType = 0;
            DxfShotDataList_D[i].ShotGapDistance = 0.0;
            DxfShotDataList_D[i].ZoomNum = 0;
            DxfShotDataList_D[i].FocusNum = 0;
            i = 4;
            DxfShotDataList_D[i].ShotType = 0;
            DxfShotDataList_D[i].ShotGapDistance = 0.0;
            DxfShotDataList_D[i].ZoomNum = 0;
            DxfShotDataList_D[i].FocusNum = 0;
            i = 5;
            DxfShotDataList_D[i].ShotType = 0;
            DxfShotDataList_D[i].ShotGapDistance = 0.0;
            DxfShotDataList_D[i].ZoomNum = 0;
            DxfShotDataList_D[i].FocusNum = 0;
            i = 6;
            DxfShotDataList_D[i].ShotType = 0;
            DxfShotDataList_D[i].ShotGapDistance = 0.0;
            DxfShotDataList_D[i].ZoomNum = 0;
            DxfShotDataList_D[i].FocusNum = 0;
            i = 7;
            DxfShotDataList_D[i].ShotType = 0;
            DxfShotDataList_D[i].ShotGapDistance = 0.0;
            DxfShotDataList_D[i].ZoomNum = 0;
            DxfShotDataList_D[i].FocusNum = 0;
            i = 8;
            DxfShotDataList_D[i].ShotType = 0;
            DxfShotDataList_D[i].ShotGapDistance = 0.0;
            DxfShotDataList_D[i].ZoomNum = 0;
            DxfShotDataList_D[i].FocusNum = 0;
            i = 9;
            DxfShotDataList_D[i].ShotType = 0;
            DxfShotDataList_D[i].ShotGapDistance = 0.0;
            DxfShotDataList_D[i].ZoomNum = 0;
            DxfShotDataList_D[i].FocusNum = 0;
            i = 10;
            DxfShotDataList_D[i].ShotType = 0;
            DxfShotDataList_D[i].ShotGapDistance = 0.0;
            DxfShotDataList_D[i].ZoomNum = 0;
            DxfShotDataList_D[i].FocusNum = 0;
            i = 11;
            DxfShotDataList_D[i].ShotType = 0;
            DxfShotDataList_D[i].ShotGapDistance = 0.0;
            DxfShotDataList_D[i].ZoomNum = 0;
            DxfShotDataList_D[i].FocusNum = 0;
            i = 12;
            DxfShotDataList_D[i].ShotType = 0;
            DxfShotDataList_D[i].ShotGapDistance = 0.0;
            DxfShotDataList_D[i].ZoomNum = 0;
            DxfShotDataList_D[i].FocusNum = 0;
            i = 13;
            DxfShotDataList_D[i].ShotType = 0;
            DxfShotDataList_D[i].ShotGapDistance = 0.0;
            DxfShotDataList_D[i].ZoomNum = 0;
            DxfShotDataList_D[i].FocusNum = 0;
            i = 14;
            DxfShotDataList_D[i].ShotType = 0;
            DxfShotDataList_D[i].ShotGapDistance = 0.0;
            DxfShotDataList_D[i].ZoomNum = 0;
            DxfShotDataList_D[i].FocusNum = 0;
            i = 15;
            DxfShotDataList_D[i].ShotType = 0;
            DxfShotDataList_D[i].ShotGapDistance = 0.0;
            DxfShotDataList_D[i].ZoomNum = 0;
            DxfShotDataList_D[i].FocusNum = 0;
            i = 16;
            DxfShotDataList_D[i].ShotType = 0;
            DxfShotDataList_D[i].ShotGapDistance = 0.0;
            DxfShotDataList_D[i].ZoomNum = 0;
            DxfShotDataList_D[i].FocusNum = 0;
            i = 17;
            DxfShotDataList_D[i].ShotType = 0;
            DxfShotDataList_D[i].ShotGapDistance = 0.0;
            DxfShotDataList_D[i].ZoomNum = 0;
            DxfShotDataList_D[i].FocusNum = 0;
            i = 18;
            DxfShotDataList_D[i].ShotType = 0;
            DxfShotDataList_D[i].ShotGapDistance = 0.0;
            DxfShotDataList_D[i].ZoomNum = 0;
            DxfShotDataList_D[i].FocusNum = 0;

            //==================================
            // E
            //==================================
            i = 0;
            DxfShotDataList_E[i].ShotType = 0;
            DxfShotDataList_E[i].ShotGapDistance = 0.0;
            DxfShotDataList_E[i].ZoomNum = 0;
            DxfShotDataList_E[i].FocusNum = 0;
            i = 1;
            DxfShotDataList_E[i].ShotType = 0;
            DxfShotDataList_E[i].ShotGapDistance = 0.0;
            DxfShotDataList_E[i].ZoomNum = 0;
            DxfShotDataList_E[i].FocusNum = 0;
            i = 2;
            DxfShotDataList_E[i].ShotType = 0;
            DxfShotDataList_E[i].ShotGapDistance = 0.0;
            DxfShotDataList_E[i].ZoomNum = 0;
            DxfShotDataList_E[i].FocusNum = 0;
            i = 3;
            DxfShotDataList_E[i].ShotType = 0;
            DxfShotDataList_E[i].ShotGapDistance = 0.0;
            DxfShotDataList_E[i].ZoomNum = 0;
            DxfShotDataList_E[i].FocusNum = 0;
            i = 4;
            DxfShotDataList_E[i].ShotType = 0;
            DxfShotDataList_E[i].ShotGapDistance = 0.0;
            DxfShotDataList_E[i].ZoomNum = 0;
            DxfShotDataList_E[i].FocusNum = 0;
            i = 5;
            DxfShotDataList_E[i].ShotType = 0;
            DxfShotDataList_E[i].ShotGapDistance = 0.0;
            DxfShotDataList_E[i].ZoomNum = 0;
            DxfShotDataList_E[i].FocusNum = 0;
            i = 6;
            DxfShotDataList_E[i].ShotType = 0;
            DxfShotDataList_E[i].ShotGapDistance = 0.0;
            DxfShotDataList_E[i].ZoomNum = 0;
            DxfShotDataList_E[i].FocusNum = 0;
            i = 7;
            DxfShotDataList_E[i].ShotType = 0;
            DxfShotDataList_E[i].ShotGapDistance = 0.0;
            DxfShotDataList_E[i].ZoomNum = 0;
            DxfShotDataList_E[i].FocusNum = 0;
            i = 8;
            DxfShotDataList_E[i].ShotType = 0;
            DxfShotDataList_E[i].ShotGapDistance = 0.0;
            DxfShotDataList_E[i].ZoomNum = 0;
            DxfShotDataList_E[i].FocusNum = 0;
            i = 9;
            DxfShotDataList_E[i].ShotType = 0;
            DxfShotDataList_E[i].ShotGapDistance = 0.0;
            DxfShotDataList_E[i].ZoomNum = 0;
            DxfShotDataList_E[i].FocusNum = 0;
            i = 10;
            DxfShotDataList_E[i].ShotType = 0;
            DxfShotDataList_E[i].ShotGapDistance = 0.0;
            DxfShotDataList_E[i].ZoomNum = 0;
            DxfShotDataList_E[i].FocusNum = 0;
            i = 11;
            DxfShotDataList_E[i].ShotType = 0;
            DxfShotDataList_E[i].ShotGapDistance = 0.0;
            DxfShotDataList_E[i].ZoomNum = 0;
            DxfShotDataList_E[i].FocusNum = 0;
            i = 12;
            DxfShotDataList_E[i].ShotType = 0;
            DxfShotDataList_E[i].ShotGapDistance = 0.0;
            DxfShotDataList_E[i].ZoomNum = 0;
            DxfShotDataList_E[i].FocusNum = 0;
            i = 13;
            DxfShotDataList_E[i].ShotType = 0;
            DxfShotDataList_E[i].ShotGapDistance = 0.0;
            DxfShotDataList_E[i].ZoomNum = 0;
            DxfShotDataList_E[i].FocusNum = 0;
            i = 14;
            DxfShotDataList_E[i].ShotType = 0;
            DxfShotDataList_E[i].ShotGapDistance = 0.0;
            DxfShotDataList_E[i].ZoomNum = 0;
            DxfShotDataList_E[i].FocusNum = 0;
            i = 15;
            DxfShotDataList_E[i].ShotType = 0;
            DxfShotDataList_E[i].ShotGapDistance = 0.0;
            DxfShotDataList_E[i].ZoomNum = 0;
            DxfShotDataList_E[i].FocusNum = 0;
            i = 16;
            DxfShotDataList_E[i].ShotType = 0;
            DxfShotDataList_E[i].ShotGapDistance = 0.0;
            DxfShotDataList_E[i].ZoomNum = 0;
            DxfShotDataList_E[i].FocusNum = 0;
            i = 17;
            DxfShotDataList_E[i].ShotType = 0;
            DxfShotDataList_E[i].ShotGapDistance = 0.0;
            DxfShotDataList_E[i].ZoomNum = 0;
            DxfShotDataList_E[i].FocusNum = 0;
            i = 18;
            DxfShotDataList_E[i].ShotType = 0;
            DxfShotDataList_E[i].ShotGapDistance = 0.0;
            DxfShotDataList_E[i].ZoomNum = 0;
            DxfShotDataList_E[i].FocusNum = 0;

        }


        private string GetOutOnlyFileName(string FindFullFilePath)
        {
            string OutOnlyFileNameStr;
            OutOnlyFileNameStr = "";
            OutOnlyFileNameStr = FindFullFilePath.Substring(FindFullFilePath.LastIndexOf(@"\") + 1);
            return OutOnlyFileNameStr;
        }

    }
}
