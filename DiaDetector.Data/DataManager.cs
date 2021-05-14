using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;
using System.Xml;
using System.Windows.Forms;
using System.Runtime.InteropServices;

// ----------

using Microsoft.VisualBasic.FileIO; 


namespace DiaDetector.Data
{
    public class DataManager
    {

        private static char[] SaveDiv = new char[] { '|' };
        private static char[] SaveDataDiv = new char[] { '#' };


        public static string[] AxisWorkList = { 
            //@"Cam Up/Down Motor",
            //@"Cam Rotating Motor",  
            //@"Guide Adjust Motor(L)", 
            //@"Guide Adjust Motor(R)",            
            //@"Cam Up/Down Motor1", 
            //@"Cam Up/Down Motor2", 
            //@"Cam Up/Down Motor3", 
            //@"미정의 2" 
             @"CameraFD",
            @"CameraMD",  
            @"LiftL", 
            @"LiftR",            
            @"SuttleL", 
            @"SuttleR", 
            @"Rotation", 
            @"미정의 2" 

            };
        
#region 1. 모델 ...


        public static ModelListInfo SelectedModel;
        public static string SelectedModelName = "default";

        public static double sinmove4 = -9.7848; // 시작위치
        public static double sinmove49 = 34.884; // NG위치       
        public static double sinmove94 = 80.6526; //90도위치

        public const double bsinmove4 = -9.7848; // 시작위치
        public const double bsinmove49 = 34.884; // NG위치       
        public const double bsinmove94 = 80.6526; //90도위치

        public static double dgab = 1;

        public static void SaveCurrentModel()
        {
            DataManager.ModelDatList[DataManager.GetModelSelectIndex()] = SelectedModel;

            DataManager.SaveModelListFiles();
        }

        public static int MODELLISTMAX = 1000;

        public static ModelListInfo[] ModelDatList = new ModelListInfo[MODELLISTMAX];
        public static int SelectModelIndex = -1;





        // 20160830
        public static void SelectedModelByName(string strName)
        {
            for (int i = 0; i < MODELLISTMAX; i++)
            {
                if (DataManager.ModelDatList[i].ModelExistFlag == true)
                {
                    DataManager.ModelDatList[i].SelectStatus = 0;
                }
            }

            for (int k = 0; k < MODELLISTMAX; k++)
            {
                if (DataManager.ModelDatList[k].ModelName == strName)
                {
                    DataManager.SelectedModel = DataManager.ModelDatList[k];
                    DataManager.SelectModelIndex = k;
                    DataManager.ModelDatList[k].SelectStatus = 1;
                    

                    break;
                }
            }
        }

        // 0826
        public static ModelListInfo getDefaultModel()
        {

            for (int i = 0; i < MODELLISTMAX; i++)
            {
                if (DataManager.ModelDatList[i].ModelName == "default")
                {
                    return DataManager.ModelDatList[i];
                }

            }

            MessageBox.Show("default 모델 정보를 찾을 수 없습니다. 대상 경로를 확인해 주세요.");

            Application.Exit();

            return DataManager.ModelDatList[0];
        }

        public static void ModelListClear()
        {
            for (int i = 0; i < MODELLISTMAX; i++)
            {
                DataManager.ModelDatList[i].ModelExistFlag = false;
                DataManager.ModelDatList[i].ModelName = "";
                DataManager.ModelDatList[i].ModelFileName_delete = "";
                DataManager.ModelDatList[i].ImageFileName = "";
                DataManager.ModelDatList[i].NGLoopCount = 0;
                DataManager.ModelDatList[i].SelectStatus = 0;


                // ----------
                DataManager.ModelDatList[i].ModelFileName_delete = "";
                DataManager.ModelDatList[i].ImageFileName = "";

                DataManager.ModelDatList[i].NamePie1 = "";
                DataManager.ModelDatList[i].sField01 = "";
                DataManager.ModelDatList[i].sField02 = "";
                DataManager.ModelDatList[i].sField03 = "";
                DataManager.ModelDatList[i].sField04 = "";
                DataManager.ModelDatList[i].sField05 = "";
                DataManager.ModelDatList[i].sField06 = "";
                DataManager.ModelDatList[i].sField07 = "";
                DataManager.ModelDatList[i].sField08 = "";
                DataManager.ModelDatList[i].sField09 = "";

                DataManager.ModelDatList[i].sField10 = "";
                DataManager.ModelDatList[i].sField11 = "";
                DataManager.ModelDatList[i].sField12 = "";
                DataManager.ModelDatList[i].sField13 = "";
                DataManager.ModelDatList[i].sField14 = "";
                DataManager.ModelDatList[i].sField15 = "";
                DataManager.ModelDatList[i].sField16 = "";
                DataManager.ModelDatList[i].sField17 = "";
                DataManager.ModelDatList[i].sField18 = "";
                DataManager.ModelDatList[i].sField19 = "";
                // ----------
            }
        }

        public static string getModelNameByFolder(string PathName)
        {
            string strPathName = "";

            int index = PathName.LastIndexOf("\\") + 1;
            int length = PathName.Length - index;

            strPathName = PathName.Substring(index, length);

            return strPathName;
        }

        public static void LoadModelListFiles_NET()
        {
            try
            {
                DataManager.ModelListClear();

                string[] dirs = Directory.GetDirectories(ConfigManager.GetModelFilePath);
                string PathName = "";

                int i = 0;
                
                
                foreach (string dir in dirs)
                {
                    string m_GetConfigDataStr = "";
                    PathName = dir + "\\" + "modelinfo.dat";


                    //MessageBox.Show(PathName);


                    try
                    {
                        StreamReader sr = new StreamReader(PathName, Encoding.Default);
                        m_GetConfigDataStr = sr.ReadToEnd();
                        sr.Close();
                    }
                    catch (Exception)
                    {
                        continue;
                        //throw;
                    }


                    string[] m_DtlWordStr;

                    m_DtlWordStr = m_GetConfigDataStr.Split(SaveDataDiv);
                    if (m_DtlWordStr.Length > 0)

                    // 홍동성 - 파일 포맷 관련 - 모델 리스트
                    // --------------------------------------------------


                    // 20160420 ...
                    // ---------- 
                    DataManager.ModelDatList[i].ModelExistFlag = true;
                    //DataManager.ModelDatList[i].ModelName = m_DtlWordStr[0];
                    DataManager.ModelDatList[i].ModelName = DataManager.getModelNameByFolder(dir);    // 모델 이름 => 경로가 안 맞는 문제 해결하기 위해
                    DataManager.ModelDatList[i].ModelFileName_delete = m_DtlWordStr[1];
                    DataManager.ModelDatList[i].ImageFileName = m_DtlWordStr[2];
                    //DataManager.ModelDatList[i].SelectStatus = Convert.ToInt32(m_DtlWordStr[3]);


                    // 20160815 ...
                    // ---------- 

                    // int ...

                    DataManager.ModelDatList[i].nAlignSetIndex = Convert.ToInt32(m_DtlWordStr[4]);
                    DataManager.ModelDatList[i].nField01 = Convert.ToInt32(m_DtlWordStr[5]);
                    DataManager.ModelDatList[i].nField02 = Convert.ToInt32(m_DtlWordStr[6]);
                    DataManager.ModelDatList[i].nField03 = Convert.ToInt32(m_DtlWordStr[7]);
                    DataManager.ModelDatList[i].nField04 = Convert.ToInt32(m_DtlWordStr[8]);

                    DataManager.ModelDatList[i].nField05 = Convert.ToInt32(m_DtlWordStr[9]);
                    DataManager.ModelDatList[i].nField06 = Convert.ToInt32(m_DtlWordStr[10]);
                    DataManager.ModelDatList[i].nField07 = Convert.ToInt32(m_DtlWordStr[11]);
                    DataManager.ModelDatList[i].nField08 = Convert.ToInt32(m_DtlWordStr[12]);
                    DataManager.ModelDatList[i].nField09 = Convert.ToInt32(m_DtlWordStr[13]);

                    // double ...

                    DataManager.ModelDatList[i].ndblOutDiameter = Convert.ToDouble(m_DtlWordStr[14]);
                    DataManager.ModelDatList[i].Camera1Moving = Convert.ToDouble(m_DtlWordStr[15]);
                    DataManager.ModelDatList[i].Camera2Moving = Convert.ToDouble(m_DtlWordStr[16]);
                    DataManager.ModelDatList[i].txtSTValueA = Convert.ToDouble(m_DtlWordStr[17]);
                    DataManager.ModelDatList[i].txtSTValueB = Convert.ToDouble(m_DtlWordStr[18]);

                    DataManager.ModelDatList[i].OutPie = Convert.ToDouble(m_DtlWordStr[19]);
                    DataManager.ModelDatList[i].InPie = Convert.ToDouble(m_DtlWordStr[20]);
                    DataManager.ModelDatList[i].NamePie = Convert.ToDouble(m_DtlWordStr[21]);
                    DataManager.ModelDatList[i].nField18 = Convert.ToDouble(m_DtlWordStr[22]);
                    DataManager.ModelDatList[i].nField19 = Convert.ToDouble(m_DtlWordStr[23]);

                    DataManager.ModelDatList[i].dFLValue = Convert.ToDouble(m_DtlWordStr[24]);
                    DataManager.ModelDatList[i].dMetalThick1 = Convert.ToDouble(m_DtlWordStr[25]);
                    DataManager.ModelDatList[i].dMetalThick2 = Convert.ToDouble(m_DtlWordStr[26]);
                    DataManager.ModelDatList[i].dSLValue = Convert.ToDouble(m_DtlWordStr[27]);
                    DataManager.ModelDatList[i].dWRValue = Convert.ToDouble(m_DtlWordStr[28]);

                    DataManager.ModelDatList[i].dCapsulePie = Convert.ToDouble(m_DtlWordStr[29]);
                    DataManager.ModelDatList[i].dVBlockFL_Offset_Value = Convert.ToDouble(m_DtlWordStr[30]);
                    DataManager.ModelDatList[i].dVBlockFL_Limit_Value = Convert.ToDouble(m_DtlWordStr[31]);
                    DataManager.ModelDatList[i].dRolling70Rate = Convert.ToDouble(m_DtlWordStr[32]);
                    DataManager.ModelDatList[i].dRolling80Rate = Convert.ToDouble(m_DtlWordStr[33]);

                    DataManager.ModelDatList[i].dRollingOffset = Convert.ToDouble(m_DtlWordStr[34]);
                    DataManager.ModelDatList[i].dRotateCount = Convert.ToDouble(m_DtlWordStr[35]);
                    DataManager.ModelDatList[i].dField12 = Convert.ToDouble(m_DtlWordStr[36]);
                    DataManager.ModelDatList[i].dField13 = Convert.ToDouble(m_DtlWordStr[37]);
                    DataManager.ModelDatList[i].dField14 = Convert.ToDouble(m_DtlWordStr[38]);

                    DataManager.ModelDatList[i].dField15 = Convert.ToDouble(m_DtlWordStr[39]);
                    DataManager.ModelDatList[i].dField16 = Convert.ToDouble(m_DtlWordStr[40]);
                    DataManager.ModelDatList[i].dField17 = Convert.ToDouble(m_DtlWordStr[41]);
                    DataManager.ModelDatList[i].dField18 = Convert.ToDouble(m_DtlWordStr[42]);
                    DataManager.ModelDatList[i].dField19 = Convert.ToDouble(m_DtlWordStr[43]);

                    // string ...

                    DataManager.ModelDatList[i].NamePie1 = m_DtlWordStr[44];
                    DataManager.ModelDatList[i].sField01 = m_DtlWordStr[45];
                    DataManager.ModelDatList[i].sField02 = m_DtlWordStr[46];
                    DataManager.ModelDatList[i].sField03 = m_DtlWordStr[47];
                    DataManager.ModelDatList[i].sField04 = m_DtlWordStr[48];

                    DataManager.ModelDatList[i].sField05 = m_DtlWordStr[49];
                    DataManager.ModelDatList[i].sField06 = m_DtlWordStr[50];
                    DataManager.ModelDatList[i].sField07 = m_DtlWordStr[51];
                    DataManager.ModelDatList[i].sField08 = m_DtlWordStr[52];
                    DataManager.ModelDatList[i].sField09 = m_DtlWordStr[53];

                    DataManager.ModelDatList[i].sField10 = m_DtlWordStr[54];
                    DataManager.ModelDatList[i].sField11 = m_DtlWordStr[55];
                    DataManager.ModelDatList[i].sField12 = m_DtlWordStr[56];
                    DataManager.ModelDatList[i].sField13 = m_DtlWordStr[57];
                    DataManager.ModelDatList[i].sField14 = m_DtlWordStr[58];

                    DataManager.ModelDatList[i].sField15 = m_DtlWordStr[59];
                    DataManager.ModelDatList[i].sField16 = m_DtlWordStr[60];
                    DataManager.ModelDatList[i].sField17 = m_DtlWordStr[61];
                    DataManager.ModelDatList[i].sField18 = m_DtlWordStr[62];
                    DataManager.ModelDatList[i].sField19 = m_DtlWordStr[63];

                    i++;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
        }


        public static void SaveModelListFiles()
        {
            string WritePresetDataStr;
            string PathName = "";
            

            for (int i = 0; i < MODELLISTMAX; i++)
            {
                if (DataManager.ModelDatList[i].ModelExistFlag == true && 
                    DataManager.SelectedModel.ModelName == DataManager.ModelDatList[i].ModelName)
                {
                    DataManager.ModelDatList[i] = DataManager.SelectedModel;

                    try
                    {
                        PathName = ConfigManager.GetModelFilePath + DataManager.ModelDatList[i].ModelName + "\\" + "modelinfo.dat";

                        StreamWriter sw = new StreamWriter(PathName, false, Encoding.Default);

                        WritePresetDataStr = "";


                        // ...
                        // ----------
                        WritePresetDataStr = WritePresetDataStr +
                            DataManager.ModelDatList[i].ModelName + "#" +
                            DataManager.ModelDatList[i].ModelFileName_delete + "#" +
                            DataManager.ModelDatList[i].ImageFileName + "#" +
                            DataManager.ModelDatList[i].SelectStatus + "#" +

                        // ...
                        // ---------- 
                        DataManager.ModelDatList[i].nAlignSetIndex.ToString() + "#" +
                        DataManager.ModelDatList[i].nField01.ToString() + "#" +
                        DataManager.ModelDatList[i].nField02.ToString() + "#" +
                        DataManager.ModelDatList[i].nField03.ToString() + "#" +
                        DataManager.ModelDatList[i].nField04.ToString() + "#" +

                        DataManager.ModelDatList[i].nField05.ToString() + "#" +
                        DataManager.ModelDatList[i].nField06.ToString() + "#" +
                        DataManager.ModelDatList[i].nField07.ToString() + "#" +
                        DataManager.ModelDatList[i].nField08.ToString() + "#" +
                        DataManager.ModelDatList[i].nField09.ToString() + "#" +

                        DataManager.ModelDatList[i].ndblOutDiameter.ToString() + "#" +
                        DataManager.ModelDatList[i].Camera1Moving.ToString() + "#" +
                        DataManager.ModelDatList[i].Camera2Moving.ToString() + "#" +
                        DataManager.ModelDatList[i].txtSTValueA.ToString() + "#" +
                        DataManager.ModelDatList[i].txtSTValueB.ToString() + "#" +

                        DataManager.ModelDatList[i].OutPie.ToString() + "#" +
                        DataManager.ModelDatList[i].InPie.ToString() + "#" +
                        DataManager.ModelDatList[i].NamePie.ToString() + "#" +
                        DataManager.ModelDatList[i].nField18.ToString() + "#" +
                        DataManager.ModelDatList[i].nField19.ToString() + "#" +

                        // ...

                        DataManager.ModelDatList[i].dFLValue.ToString() + "#" +
                        DataManager.ModelDatList[i].dMetalThick1.ToString() + "#" +
                        DataManager.ModelDatList[i].dMetalThick2.ToString() + "#" +
                        DataManager.ModelDatList[i].dSLValue.ToString() + "#" +
                        DataManager.ModelDatList[i].dWRValue.ToString() + "#" +

                        DataManager.ModelDatList[i].dCapsulePie.ToString() + "#" +
                        DataManager.ModelDatList[i].dVBlockFL_Offset_Value.ToString() + "#" +
                        DataManager.ModelDatList[i].dVBlockFL_Limit_Value.ToString() + "#" +
                        DataManager.ModelDatList[i].dRolling70Rate.ToString() + "#" +
                        DataManager.ModelDatList[i].dRolling80Rate.ToString() + "#" +

                        DataManager.ModelDatList[i].dRollingOffset.ToString() + "#" +
                        DataManager.ModelDatList[i].dRotateCount.ToString() + "#" +
                        DataManager.ModelDatList[i].dField12.ToString() + "#" +
                        DataManager.ModelDatList[i].dField13.ToString() + "#" +
                        DataManager.ModelDatList[i].dField14.ToString() + "#" +

                        DataManager.ModelDatList[i].dField15.ToString() + "#" +
                        DataManager.ModelDatList[i].dField16.ToString() + "#" +
                        DataManager.ModelDatList[i].dField17.ToString() + "#" +
                        DataManager.ModelDatList[i].dField18.ToString() + "#" +
                        DataManager.ModelDatList[i].dField19.ToString() + "#" +

                        // ...

                        DataManager.ModelDatList[i].NamePie1 + "#" +
                        DataManager.ModelDatList[i].sField01 + "#" +
                        DataManager.ModelDatList[i].sField02 + "#" +
                        DataManager.ModelDatList[i].sField03 + "#" +
                        DataManager.ModelDatList[i].sField04 + "#" +

                        DataManager.ModelDatList[i].sField05 + "#" +
                        DataManager.ModelDatList[i].sField06 + "#" +
                        DataManager.ModelDatList[i].sField07 + "#" +
                        DataManager.ModelDatList[i].sField08 + "#" +
                        DataManager.ModelDatList[i].sField09 + "#" +

                        DataManager.ModelDatList[i].sField10 + "#" +
                        DataManager.ModelDatList[i].sField11 + "#" +
                        DataManager.ModelDatList[i].sField12 + "#" +
                        DataManager.ModelDatList[i].sField13 + "#" +
                        DataManager.ModelDatList[i].sField14 + "#" +

                        DataManager.ModelDatList[i].sField15 + "#" +
                        DataManager.ModelDatList[i].sField16 + "#" +
                        DataManager.ModelDatList[i].sField17 + "#" +
                        DataManager.ModelDatList[i].sField18 + "#" +
                        DataManager.ModelDatList[i].sField19.ToString();

                        sw.Write(WritePresetDataStr);

                        sw.Close();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("모델 저장에 실패했습니다.");
                        //throw;
                    }

                }
            }

            
        }


        public static int GetModelSelectIndex()
        {
            int ResSelIndex;
            ResSelIndex = -1;
            for (int i = 0; i < MODELLISTMAX; i++)
            {
                if (DataManager.ModelDatList[i].ModelExistFlag == true)
                {
                    if (DataManager.ModelDatList[i].SelectStatus == 1)
                    {
                        ResSelIndex = i;
                        break;
                    }
                }
            }
            return ResSelIndex;
        }


        public static string GetModelSelectFileNameFolder()
        {
            string ResModelFileName;
            ResModelFileName = "";

            for (int i = 0; i < MODELLISTMAX; i++)
            {
                if (DataManager.ModelDatList[i].ModelExistFlag == true)
                {
                    if (DataManager.ModelDatList[i].SelectStatus == 1)
                    {
                        ResModelFileName = DataManager.ModelDatList[i].ModelName;

                        break;
                    }
                }
            }
            return ResModelFileName;
        }

        public static string GetModelSelectName()
        {
            string ResModelFileName;
            ResModelFileName = "";
            for (int i = 0; i < MODELLISTMAX; i++)
            {
                if (DataManager.ModelDatList[i].ModelExistFlag == true)
                {
                    if (DataManager.ModelDatList[i].SelectStatus == 1)
                    {
                        ResModelFileName = DataManager.ModelDatList[i].ModelName;
                        break;
                    }
                }
            }
            return ResModelFileName;
        }

        public static void ModelChangeSelect(int ChangeSelIndex)
        {
            for (int i = 0; i < MODELLISTMAX; i++)
            {
                if (DataManager.ModelDatList[i].ModelExistFlag == true)
                {
                    DataManager.ModelDatList[i].SelectStatus = 0;
                }
            }

            DataManager.ModelDatList[ChangeSelIndex].SelectStatus = 1;

            DataManager.SelectModelIndex = ChangeSelIndex;
        }

        public static void AddDataModelList(string ModelNameStr, int NGLoopCount)
        {
            string strFolderName = "";

            for (int i = 0; i < MODELLISTMAX; i++)
            {
                if (DataManager.ModelDatList[i].ModelExistFlag == false)
                {
                    try
                    {
                        strFolderName = ConfigManager.GetModelFilePath + ModelNameStr;
                        
                        DataManager.CreateMakeFolderFunc(strFolderName);

                        //DataManager.SaveEmptyTestListFiles(ModelNameStr); // 빈 파일 추가 => ModelData_XXXX.dat

                        string sourcePath = ConfigManager.GetModelFilePath + DataManager.getDefaultModel().ModelName;

                        string destinationPath = ConfigManager.GetModelFilePath + ModelNameStr;

                        FileSystem.CopyDirectory(sourcePath, destinationPath, UIOption.AllDialogs); //using Microsoft.VisualBasic.FileIO;

                    }
                    catch (Exception)
                    {
                        MessageBox.Show("신규 모델 생성에 실패하셨습니다.");

                        return;
                    }

                    DataManager.ModelDatList[i].ModelExistFlag = true;
                    DataManager.ModelDatList[i].ModelName = ModelNameStr;


                    break;
                }
            }

            DataManager.SaveModelListFiles();
        }

        private static void CreateMakeFolderFunc(string CrMakeFolderStr)
        {
            DirectoryInfo ChkDirFld = new DirectoryInfo(CrMakeFolderStr);
            if (ChkDirFld.Exists == false)
            {
                ChkDirFld.Create();
            }
        }

        public static string ModelCopySelect(int CopySelIndex)
        {   
            string strReturnModelName = "";

            string FileName = string.Empty;
            string NewFileName = string.Empty;
            string OnlyNewFileName = string.Empty;
            string NewFolderName = string.Empty;


            for (int i = 0; i < MODELLISTMAX; i++)
            {
                if (DataManager.ModelDatList[i].ModelExistFlag == false)
                {
                    string sourcePath = ConfigManager.GetModelFilePath + DataManager.ModelDatList[CopySelIndex].ModelName;
                    string destinationPath = ConfigManager.GetModelFilePath + DataManager.ModelDatList[CopySelIndex].ModelName + "_COPY";


                    try
                    {
                        CreateMakeFolderFunc(destinationPath);

                        FileSystem.CopyDirectory(sourcePath, destinationPath, UIOption.AllDialogs); //using Microsoft.VisualBasic.FileIO;
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("모델 복사에 실패했습니다.");

                        return strReturnModelName;
                    }

                    // --------------------------------------------------
                    DataManager.ModelDatList[i] = DataManager.ModelDatList[CopySelIndex];

                    DataManager.ModelDatList[i].ModelName += "_COPY";

                    strReturnModelName = DataManager.ModelDatList[i].ModelName;
                    // --------------------------------------------------

                    break;
                }
            }

            DataManager.SaveModelListFiles();

            return strReturnModelName;            
        }

#endregion 1. 모델 ...


#region 3. DIO 설정 ...

        public static DIOSettingInfo[] DIOSettingInfoList = new DIOSettingInfo[64];

        public static void ClearDIOSettingInfo()
        {
            for (int i=0; i<64; i++)
            {
                DataManager.DIOSettingInfoList[i].Name = "";
                DataManager.DIOSettingInfoList[i].Field01 = "";
            }            
        }

        public static void LoadDIOSettingFiles(string filename)
        {
            if (System.IO.File.Exists(filename))
            {
                int k = 0;
                string[] m_WordStr;


                // 추가 코드
                ClearDIOSettingInfo();

                StreamReader sr = new StreamReader(filename, Encoding.Default);
                string m_GetConfigDataStr = sr.ReadToEnd();
                sr.Close();

                m_WordStr = m_GetConfigDataStr.Split(SaveDiv);

                //if (m_WordStr.Length > 0)
                if (m_GetConfigDataStr.Length > 0)
                {
                    for (int i = 0; i < m_WordStr.Length; i++)
                    {
                        string[] m_DtlWordStr;

                        m_DtlWordStr = m_WordStr[i].Split(SaveDataDiv);


                        // 파일 포맷 관련 - 작업 리스트
                        // --------------------------------------------------

                        // 20160425 ...
                        // ----------
                        DataManager.DIOSettingInfoList[i].Name = m_DtlWordStr[0];
                        DataManager.DIOSettingInfoList[i].Field01 = m_DtlWordStr[1];

                        // 코드 작업
                        //DIOSettingInfoList



                        k++;
                    }
                }
            }

        }

        public static void SaveDIOSettingFiles(string filename)
        {
            bool FirstFlag = true;

            string WritePresetDataStr = "";

            StreamWriter sw = new StreamWriter(filename, false, Encoding.Default);

            for (int i = 0; i < 64; i++)
            {
                if (FirstFlag == true)
                {
                    FirstFlag = false;

                    WritePresetDataStr = "";
                }
                else
                {
                    WritePresetDataStr = "|";
                }


                // 파일 포맷 관련 - 작업 리스트
                // --------------------------------------------------

                // 20160425 ...
                // ----------
                WritePresetDataStr = WritePresetDataStr +
                    DataManager.DIOSettingInfoList[i].Name + "#" +
                    DataManager.DIOSettingInfoList[i].Field01;

                sw.Write(WritePresetDataStr);
            }

            sw.Close();
        }
        
#endregion 3. DIO 설정 ...


#region 4. 모션 ...

        public static MotionSettingInfo[] MotionSettingInfoList = new MotionSettingInfo[16];

        public static void ClearMotionSettingInfo()
        {
            for (int i = 0; i < 8; i++)
            {
                DataManager.MotionSettingInfoList[i].Name = "";
                DataManager.MotionSettingInfoList[i].MotionType = 0;
                DataManager.MotionSettingInfoList[i].MaxValue = 0.0;
                DataManager.MotionSettingInfoList[i].MinValue = 0.0;


                // PAIX SDK 참고하여 추가한 변수들 ...
                // --------------------------------------------------
                DataManager.MotionSettingInfoList[i].Velocity_Start = 0.0;
                DataManager.MotionSettingInfoList[i].Velocity_Acc = 0.0;
                DataManager.MotionSettingInfoList[i].Velocity_Dec = 0.0;
                DataManager.MotionSettingInfoList[i].Velocity_Max = 0.0;

                DataManager.MotionSettingInfoList[i].Logic_Emergency = 0;
                DataManager.MotionSettingInfoList[i].Logic_UnitPerPulse = 0.0;
                DataManager.MotionSettingInfoList[i].Logic_Enc = 0;
                DataManager.MotionSettingInfoList[i].Logic_EncZ = 0;
                DataManager.MotionSettingInfoList[i].Logic_Enc_Input = 0;
                DataManager.MotionSettingInfoList[i].Logic_Near = 0;
                DataManager.MotionSettingInfoList[i].Logic_Limit_Minus = 0;
                DataManager.MotionSettingInfoList[i].Logic_Limit_Plus = 0;
                DataManager.MotionSettingInfoList[i].Logic_Alarm = 0;
                DataManager.MotionSettingInfoList[i].Logic_HomeMode = 0;
                DataManager.MotionSettingInfoList[i].Logic_PulseMode = 0;


                // 원점 관련 변수 추가 ... => 20160707
                // --------------------------------------------------
                DataManager.MotionSettingInfoList[i].Velocity_Home_1 = 0.0;
                DataManager.MotionSettingInfoList[i].Velocity_Home_2 = 0.0;
                DataManager.MotionSettingInfoList[i].Velocity_Home_3 = 0.0;
                DataManager.MotionSettingInfoList[i].Velocity_Home_Offset = 0.0;
                DataManager.MotionSettingInfoList[i].Home_Offset = 0.0;


                // 원점 관련 변수 추가 ... => 20161010
                // --------------------------------------------------
                DataManager.MotionSettingInfoList[i].Velocity_Start_Home = 0.0;
                DataManager.MotionSettingInfoList[i].Velocity_Acc_Home = 0.0;
                DataManager.MotionSettingInfoList[i].Velocity_Dec_Home = 0.0;
                DataManager.MotionSettingInfoList[i].Velocity_Max_Home = 0.0;
            }
        }

        public static void LoadMotionSettingFiles(string filename)
        {            
            //ClearMotionSettingInfo(); // 홍동성 Test 코드 

            //return;

            if (System.IO.File.Exists(filename))
            {
                int k = 0;
                string[] m_WordStr;


                // 홍동성 추가 코드
                ClearCameraSettingInfo();

                StreamReader sr = new StreamReader(filename, Encoding.Default);
                string m_GetConfigDataStr = sr.ReadToEnd();
                sr.Close();

                m_WordStr = m_GetConfigDataStr.Split(SaveDiv);

                //if (m_WordStr.Length > 0)
                if (m_GetConfigDataStr.Length > 0) // 홍동성 - 위 코드보다 이게 더 정확하다.
                {
                    for (int i = 0; i < m_WordStr.Length; i++)
                    {
                        string[] m_DtlWordStr;

                        m_DtlWordStr = m_WordStr[i].Split(SaveDataDiv);


                        // 파일 포맷 관련
                        // --------------------------------------------------

                        // 20160426 ...
                        // ----------
                        DataManager.MotionSettingInfoList[i].Name               = m_DtlWordStr[0];
                        DataManager.MotionSettingInfoList[i].MotionType         = Convert.ToInt32(m_DtlWordStr[1]);
                        DataManager.MotionSettingInfoList[i].MaxValue           = Convert.ToDouble(m_DtlWordStr[2]);
                        DataManager.MotionSettingInfoList[i].MinValue           = Convert.ToDouble(m_DtlWordStr[3]);


                        // PAIX SDK 참고하여 추가한 변수들 ...
                        // --------------------------------------------------
                        DataManager.MotionSettingInfoList[i].Velocity_Start     = Convert.ToDouble(m_DtlWordStr[4]);
                        DataManager.MotionSettingInfoList[i].Velocity_Acc       = Convert.ToDouble(m_DtlWordStr[5]);
                        DataManager.MotionSettingInfoList[i].Velocity_Dec       = Convert.ToDouble(m_DtlWordStr[6]);
                        DataManager.MotionSettingInfoList[i].Velocity_Max       = Convert.ToDouble(m_DtlWordStr[7]);

                        DataManager.MotionSettingInfoList[i].Logic_Emergency    = Convert.ToInt32(m_DtlWordStr[8]);
                        DataManager.MotionSettingInfoList[i].Logic_UnitPerPulse = Convert.ToDouble(m_DtlWordStr[9]);
                        DataManager.MotionSettingInfoList[i].Logic_Enc          = Convert.ToInt32(m_DtlWordStr[10]);
                        DataManager.MotionSettingInfoList[i].Logic_EncZ         = Convert.ToInt32(m_DtlWordStr[11]);
                        DataManager.MotionSettingInfoList[i].Logic_Enc_Input    = Convert.ToInt32(m_DtlWordStr[12]);
                        DataManager.MotionSettingInfoList[i].Logic_Near         = Convert.ToInt32(m_DtlWordStr[13]);
                        DataManager.MotionSettingInfoList[i].Logic_Limit_Minus  = Convert.ToInt32(m_DtlWordStr[14]);
                        DataManager.MotionSettingInfoList[i].Logic_Limit_Plus   = Convert.ToInt32(m_DtlWordStr[15]);
                        DataManager.MotionSettingInfoList[i].Logic_Alarm        = Convert.ToInt32(m_DtlWordStr[16]);
                        DataManager.MotionSettingInfoList[i].Logic_HomeMode     = Convert.ToInt32(m_DtlWordStr[17]);
                        DataManager.MotionSettingInfoList[i].Logic_PulseMode    = Convert.ToInt32(m_DtlWordStr[18]);

                        DataManager.MotionSettingInfoList[i].BallScrew_Lead             = Convert.ToDouble(m_DtlWordStr[19]);
                        DataManager.MotionSettingInfoList[i].BallScrew_Diameter         = Convert.ToDouble(m_DtlWordStr[20]);
                        DataManager.MotionSettingInfoList[i].Servo_Amplifier_Resolution = Convert.ToDouble(m_DtlWordStr[21]);


                        // 원점 관련 ...
                        // ----------
                        DataManager.MotionSettingInfoList[i].Velocity_Home_1    = Convert.ToDouble(m_DtlWordStr[22]);
                        DataManager.MotionSettingInfoList[i].Velocity_Home_2    = Convert.ToDouble(m_DtlWordStr[23]);
                        DataManager.MotionSettingInfoList[i].Velocity_Home_3    = Convert.ToDouble(m_DtlWordStr[24]);
                        DataManager.MotionSettingInfoList[i].Velocity_Home_Offset       = Convert.ToDouble(m_DtlWordStr[25]);
                        DataManager.MotionSettingInfoList[i].Home_Offset        = Convert.ToDouble(m_DtlWordStr[26]);


                        // 원점 관련 - 20161010 ...
                        // ----------
                        DataManager.MotionSettingInfoList[i].Velocity_Start_Home = Convert.ToDouble(m_DtlWordStr[27]);
                        DataManager.MotionSettingInfoList[i].Velocity_Acc_Home = Convert.ToDouble(m_DtlWordStr[28]);
                        DataManager.MotionSettingInfoList[i].Velocity_Dec_Home = Convert.ToDouble(m_DtlWordStr[29]);
                        DataManager.MotionSettingInfoList[i].Velocity_Max_Home = Convert.ToDouble(m_DtlWordStr[30]);



                        k++;
                    }
                }
            }

        }

        public static void SaveMotionSettingFiles(string filename)
        {
            bool FirstFlag = true;

            string WritePresetDataStr = "";

            StreamWriter sw = new StreamWriter(filename, false, Encoding.Default);

            for (int i = 0; i < 8; i++)
            {
                if (FirstFlag == true)
                {
                    FirstFlag = false;

                    WritePresetDataStr = "";
                }
                else
                {
                    WritePresetDataStr = "|";
                }


                // 파일 포맷 관련
                // --------------------------------------------------
                WritePresetDataStr = WritePresetDataStr +
                    DataManager.MotionSettingInfoList[i].Name + "#" +
                    DataManager.MotionSettingInfoList[i].MotionType.ToString() + "#" +
                    DataManager.MotionSettingInfoList[i].MaxValue.ToString() + "#" +
                    DataManager.MotionSettingInfoList[i].MinValue.ToString() + "#" +
                    DataManager.MotionSettingInfoList[i].Velocity_Start.ToString() + "#" +
                    DataManager.MotionSettingInfoList[i].Velocity_Acc.ToString() + "#" +
                    DataManager.MotionSettingInfoList[i].Velocity_Dec.ToString() + "#" +
                    DataManager.MotionSettingInfoList[i].Velocity_Max.ToString() + "#" +
                    DataManager.MotionSettingInfoList[i].Logic_Emergency.ToString() + "#" +
                    DataManager.MotionSettingInfoList[i].Logic_UnitPerPulse.ToString() + "#" +
                    DataManager.MotionSettingInfoList[i].Logic_Enc.ToString() + "#" +
                    DataManager.MotionSettingInfoList[i].Logic_EncZ.ToString() + "#" +
                    DataManager.MotionSettingInfoList[i].Logic_Enc_Input.ToString() + "#" +
                    DataManager.MotionSettingInfoList[i].Logic_Near.ToString() + "#" +
                    DataManager.MotionSettingInfoList[i].Logic_Limit_Minus.ToString() + "#" +
                    DataManager.MotionSettingInfoList[i].Logic_Limit_Plus.ToString() + "#" +
                    DataManager.MotionSettingInfoList[i].Logic_Alarm.ToString() + "#" +
                    DataManager.MotionSettingInfoList[i].Logic_HomeMode.ToString() + "#" +
                    DataManager.MotionSettingInfoList[i].Logic_PulseMode.ToString() + "#" +
                    DataManager.MotionSettingInfoList[i].BallScrew_Lead.ToString() + "#" +
                    DataManager.MotionSettingInfoList[i].BallScrew_Diameter.ToString() + "#" +
                    DataManager.MotionSettingInfoList[i].Servo_Amplifier_Resolution.ToString() + "#" +
                    DataManager.MotionSettingInfoList[i].Velocity_Home_1.ToString() + "#" +
                    DataManager.MotionSettingInfoList[i].Velocity_Home_2.ToString() + "#" +
                    DataManager.MotionSettingInfoList[i].Velocity_Home_3.ToString() + "#" +
                    DataManager.MotionSettingInfoList[i].Velocity_Home_Offset.ToString() + "#" +
                    DataManager.MotionSettingInfoList[i].Home_Offset.ToString() + "#" +
                    DataManager.MotionSettingInfoList[i].Velocity_Start_Home.ToString() + "#" +
                    DataManager.MotionSettingInfoList[i].Velocity_Acc_Home.ToString() + "#" +
                    DataManager.MotionSettingInfoList[i].Velocity_Dec_Home.ToString() + "#" +
                    DataManager.MotionSettingInfoList[i].Velocity_Max_Home.ToString();

                sw.Write(WritePresetDataStr);
            }

            sw.Close();
        }

#endregion 4. 모션 ...


#region 5. 조명 ...

        public static LightingSettingInfo[] LightingSettingInfoList = new LightingSettingInfo[16];

        public static void ClearLightingSettingInfo()
        {
            for (int i = 0; i < 16; i++)
            {
                DataManager.LightingSettingInfoList[i].Name = "";
                DataManager.LightingSettingInfoList[i].dMaxValue = 100.0;
                DataManager.LightingSettingInfoList[i].dMinValue = 50.0;
                DataManager.LightingSettingInfoList[i].dCurValue = 0.0;
                DataManager.LightingSettingInfoList[i].Channel = 1;
            }
        }

        public static void LoadLightingSettingFiles(string filename)
        {
            // 홍동성 Test 코드 
            //////////
            
            //ClearLightingSettingInfo();

            //return;
            //////////



            if (System.IO.File.Exists(filename))
            {
                int k = 0;
                string[] m_WordStr;


                // 홍동성 추가 코드
                ClearLightingSettingInfo();

                StreamReader sr = new StreamReader(filename, Encoding.Default);
                string m_GetConfigDataStr = sr.ReadToEnd();
                sr.Close();

                m_WordStr = m_GetConfigDataStr.Split(SaveDiv);

                //if (m_WordStr.Length > 0)
                if (m_GetConfigDataStr.Length > 0) // 홍동성 - 위 코드보다 이게 더 정확하다.
                {
                    for (int i = 0; i < m_WordStr.Length; i++)
                    {
                        string[] m_DtlWordStr;

                        m_DtlWordStr = m_WordStr[i].Split(SaveDataDiv);


                        // 홍동성 - 파일 포맷 관련 - 작업 리스트
                        // --------------------------------------------------

                        // 20160426 ...
                        // ----------
                        DataManager.LightingSettingInfoList[i].Name = m_DtlWordStr[0];
                        DataManager.LightingSettingInfoList[i].dMaxValue = Convert.ToDouble(m_DtlWordStr[1]);
                        DataManager.LightingSettingInfoList[i].dMinValue = Convert.ToDouble(m_DtlWordStr[2]);
                        DataManager.LightingSettingInfoList[i].Channel = Convert.ToInt32(m_DtlWordStr[3]);


                        k++;
                    }
                }
            }

        }

        public static void SaveLightingSettingFiles(string filename)
        {
            bool FirstFlag = true;

            string WritePresetDataStr = "";

            StreamWriter sw = new StreamWriter(filename, false, Encoding.Default);

            for (int i = 0; i < 16; i++)
            {
                if (FirstFlag == true)
                {
                    FirstFlag = false;

                    WritePresetDataStr = "";
                }
                else
                {
                    WritePresetDataStr = "|";
                }


                // 홍동성 - 파일 포맷 관련 - 작업 리스트
                // --------------------------------------------------

                // 20160426 ...
                // ----------
                WritePresetDataStr = WritePresetDataStr +
                    DataManager.LightingSettingInfoList[i].Name + "#" +
                    DataManager.LightingSettingInfoList[i].dMaxValue.ToString() + "#" +
                    DataManager.LightingSettingInfoList[i].dMinValue.ToString() + "#" +
                    DataManager.LightingSettingInfoList[i].Channel.ToString();

                sw.Write(WritePresetDataStr);
            }

            sw.Close();
        }

#endregion 5. 조명 ...


#region 6. 카메라 ...

        public static CameraSettingInfo[] CameraSettingInfoList = new CameraSettingInfo[16];

        public static void ClearCameraSettingInfo()
        {
            for (int i = 0; i < 16; i++)
            {                
                DataManager.CameraSettingInfoList[i].Name = "";
                DataManager.CameraSettingInfoList[i].IP = "";
                DataManager.CameraSettingInfoList[i].ScreenSize = 0;
                DataManager.CameraSettingInfoList[i].FrameRate = 0.0;
                DataManager.CameraSettingInfoList[i].ZoomFocusPort = 1;
                DataManager.CameraSettingInfoList[i].FocusPort = 1;
            }
        }

        public static void LoadCameraSettingFiles(string filename)
        {
            // 홍동성 Test 코드 
            //////////

            //ClearCameraSettingInfo();

            //return;
            //////////



            if (System.IO.File.Exists(filename))
            {
                int k = 0;
                string[] m_WordStr;


                // 홍동성 추가 코드
                ClearCameraSettingInfo();

                StreamReader sr = new StreamReader(filename, Encoding.Default);
                string m_GetConfigDataStr = sr.ReadToEnd();
                sr.Close();

                m_WordStr = m_GetConfigDataStr.Split(SaveDiv);

                //if (m_WordStr.Length > 0)
                if (m_GetConfigDataStr.Length > 0) // 홍동성 - 위 코드보다 이게 더 정확하다.
                {
                    for (int i = 0; i < m_WordStr.Length; i++)
                    {
                        string[] m_DtlWordStr;

                        m_DtlWordStr = m_WordStr[i].Split(SaveDataDiv);


                        // 파일 포맷 관련
                        // --------------------------------------------------

                        // 20160426 ...
                        DataManager.CameraSettingInfoList[i].Name = m_DtlWordStr[0];
                        DataManager.CameraSettingInfoList[i].IP = m_DtlWordStr[1];
                        DataManager.CameraSettingInfoList[i].ScreenSize = Convert.ToInt32(m_DtlWordStr[2]);
                        DataManager.CameraSettingInfoList[i].FrameRate = Convert.ToDouble(m_DtlWordStr[3]);
                        DataManager.CameraSettingInfoList[i].ZoomFocusPort = Convert.ToInt32(m_DtlWordStr[4]);
                        DataManager.CameraSettingInfoList[i].FocusPort = Convert.ToInt32(m_DtlWordStr[5]);
                        

                        k++;
                    }
                }
            }

        }

        public static void SaveCameraSettingFiles(string filename)
        {
            bool FirstFlag = true;

            string WritePresetDataStr = "";

            StreamWriter sw = new StreamWriter(filename, false, Encoding.Default);

            for (int i = 0; i < 16; i++)
            {
                if (FirstFlag == true)
                {
                    FirstFlag = false;

                    WritePresetDataStr = "";
                }
                else
                {
                    WritePresetDataStr = "|";
                }


                // 파일 포맷 관련
                // --------------------------------------------------

                // 20160426 ...
                // ----------
                WritePresetDataStr = WritePresetDataStr +
                    DataManager.CameraSettingInfoList[i].Name + "#" +
                    DataManager.CameraSettingInfoList[i].IP + "#" +
                    DataManager.CameraSettingInfoList[i].ScreenSize.ToString() + "#" +
                    DataManager.CameraSettingInfoList[i].FrameRate.ToString() + "#" +
                    DataManager.CameraSettingInfoList[i].ZoomFocusPort.ToString() + "#" +
                    DataManager.CameraSettingInfoList[i].FocusPort.ToString();

                sw.Write(WritePresetDataStr);
            }

            sw.Close();
        }

#endregion 6. 카메라 ...


#region 원격 편집 지원 ...


        public static bool bConnectedNetworkDrive_J = false;    // 공유 폴더
        public static bool bConnectedNetworkDrive_I = false;    // 작업 폴더

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]

        public struct NetworkResource
        {
            public uint Scope;
            public uint Type;
            public uint DisplayType;
            public uint Usage;
            public string LocalName;
            public string RemoteName;
            public string Comment;
            public string Provider;
        }

        [DllImport("mpr.dll", CharSet = CharSet.Auto)]
        public static extern int WNetUseConnection
        (
            IntPtr ownerWindowHandle,
            [MarshalAs(UnmanagedType.Struct)] ref NetworkResource networkResource,
            string password,
            string userID,
            uint flag,
            StringBuilder accessNameStringBuilder,
            ref int bufferSize,
            out uint result
        );

        public static int ConnectNetworkDrive(string networkDrive, string shareFolder, string userID, string password)
        {
            NetworkResource networkResource = new NetworkResource();

            networkResource.Type = 1;
            networkResource.LocalName = networkDrive;
            networkResource.RemoteName = shareFolder;
            networkResource.Provider = null;

            uint flag = 0u;
            int bufferSize = 64;
            StringBuilder stringBuilder = new StringBuilder(bufferSize);
            uint result = 0u;

            return WNetUseConnection
            (
                IntPtr.Zero,
                ref networkResource,
                password,
                userID,
                flag,
                stringBuilder,
                ref bufferSize,
                out result
            );
        }

        [DllImport("mpr.dll", EntryPoint = "WNetCancelConnection2", CharSet = CharSet.Auto)]
        public static extern int WNetCancelConnection2A(string localName, int flag, int force);

        public static void DisconnectNetworkDrive(string networkDrive)
        {
            WNetCancelConnection2A(networkDrive, 1, 0);
        }


        // 실제 경로
        
        // ----------
        public static string SET_NET_DRIVE_J = @"j:";
        public static string SET_NET_FOLDER_J = @"\\vcms.ksm.co.kr\파일송수신\_공용작업실\JigFree\";
        public static string SET_NET_USERID = @"hjinkim";
        public static string SET_NET_PWD = @"ksm0512//";

        public static string SET_NET_DRIVE_I = @"i:";
        public static string SET_NET_FOLDER_I = @"\\ksm-er\생산부\Semi생산\Semi생산기술\Bellows\JIG_FREE\";
        // ----------
        


        // Test 경로        
        // ----------
        /*
        public static string SET_NET_DRIVE_J = @"j:";
        public static string SET_NET_FOLDER_J = @"D:\_NetFolderTest\j\";
        public static string SET_NET_USERID = @"hjinkim";
        public static string SET_NET_PWD = @"ksm0512//";

        public static string SET_NET_DRIVE_I = @"i:";
        public static string SET_NET_FOLDER_I = @"D:\_NetFolderTest\i\";
        */
        // ----------


        public static void Upload(string sourcePath, string targetPath)
        {
            


            try
            {
                CreateMakeFolderFunc(targetPath);

                FileSystem.CopyDirectory(sourcePath, targetPath, UIOption.AllDialogs); //using Microsoft.VisualBasic.FileIO;
            }
            catch (Exception)
            {
                MessageBox.Show("모델 복사에 실패했습니다.");

                return;
            }

            // 예제 ...
            // --------------------------------------------------
            /* if (File.Exists(@"\\vcms.ksm.co.kr\파일송수신\_공용작업실\JigFree\1.dxf")) {
                File.Copy(@"\\vcms.ksm.co.kr\파일송수신\_공용작업실\JigFree\1.dxf", @"\\vcms.ksm.co.kr\파일송수신\_공용작업실\JigFree\2.dxf");
            } */
            // --------------------------------------------------



            //DataManager.DisconnectNetworkDrive(SET_NET_DRIVE);



            MessageBox.Show("업로드가 완료되었습니다.");
        }

        public static void Connect_J_Drive()
        {
            if (bConnectedNetworkDrive_J == false)
            {
                DataManager.ConnectNetworkDrive(SET_NET_DRIVE_J, SET_NET_FOLDER_J, SET_NET_USERID, SET_NET_PWD);

                bConnectedNetworkDrive_J = true;
            }
        }

        public static void Connect_I_Drive()
        {
            if (bConnectedNetworkDrive_I == false)
            {
                DataManager.ConnectNetworkDrive(SET_NET_DRIVE_I, SET_NET_FOLDER_I, SET_NET_USERID, SET_NET_PWD);

                bConnectedNetworkDrive_I = true;
            }
        }

        public static void Download(string sourcePath, string targetPath)
        {
            

            try
            {
                CreateMakeFolderFunc(targetPath);

                FileSystem.CopyDirectory(sourcePath, targetPath, UIOption.AllDialogs); //using Microsoft.VisualBasic.FileIO;
            }
            catch (Exception)
            {
                MessageBox.Show("모델 복사에 실패했습니다.");

                return;
            }


            //DataManager.DisconnectNetworkDrive(SET_NET_DRIVE);

            
            MessageBox.Show("다운로드가 완료되었습니다.");
        }

	    public static void DownloadDXF(string sourcePath, string targetPath)
	    {
            DataManager.CopyFolderDXF(sourcePath, targetPath);
	    }

        public static void UploadDXF(string sourcePath, string targetPath)
        {
            DataManager.CopyFolderDXF(sourcePath, targetPath);
        }

        public static void CopyFolderDXF(string sourceFolder, string destFolder)
        {
            if (!Directory.Exists(destFolder))
                Directory.CreateDirectory(destFolder);

            string[] files = Directory.GetFiles(sourceFolder);
            string[] folders = Directory.GetDirectories(sourceFolder);


        
            foreach (string file in files)
            {
                string name = Path.GetFileName(file);
                string dest = Path.Combine(destFolder, name);



                // 파일이 없으면 복사 ...
                // --------------------------------------------------
                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(destFolder);
                System.IO.FileInfo[] fi = di.GetFiles(name);
                if (fi.Length == 0)
                {
                    File.Copy(file, dest);
                }
                // --------------------------------------------------


                
            }

            foreach (string folder in folders)
            {
                string name = Path.GetFileName(folder);
                string dest = Path.Combine(destFolder, name);
                CopyFolderDXF(folder, dest);
            }
        
        }

#endregion 원격 편집 지원 ...

    }
}
