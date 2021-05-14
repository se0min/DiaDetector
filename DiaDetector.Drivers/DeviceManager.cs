using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//////////

using System.Collections;
using System.IO;
using System.Xml;

//////////

using DiaDetector.Data;
using MediaPlayer;

namespace DiaDetector.Drivers
{
    public class DeviceManagerS
    {

#region PAIX 1 ...

        // PAIX 장치 환경 설정 정보
        private static string _PAIX_Model = string.Empty;
        private static string _PAIX_IP = string.Empty;
        private static string _PAIX_Port = string.Empty;

        private static string _PAIX_Model2 = string.Empty;
        private static string _PAIX_IP2 = string.Empty;
        private static string _PAIX_Port2 = string.Empty;

        public static string PAIX_Model
        {
            get { return _PAIX_Model; }
            set { _PAIX_Model = value; }
        }
        public static string PAIX_Model2
        {
            get { return _PAIX_Model2; }
            set { _PAIX_Model2 = value; }
        }

        // PAIX IPAddress 나머지 정보들 => nmc_SetIPAddress에서 사용해야 함.
        // --------------------------------------------------
        public static short g_ndevIdA_1;
        public static short g_ndevIdA_2;
        public static short g_ndevIdA_3;
        public static short g_ndevIdA_4;


        public static short g_ndevIdC_11;
        public static short g_ndevIdC_22;
        public static short g_ndevIdC_33;
        public static short g_ndevIdC_44;
        public static short g_nDevopenC;

        public static string PAIX_IP
        {
            get { return _PAIX_IP; }
            set
            {
                _PAIX_IP = value;
                
                // 여기서 IPAddress 쪼개서 g_ndevId, g_ndevId_1, g_ndevId_2, g_ndevId_3에 채워주어야 함.
                // ----------
                string ipaddress = _PAIX_IP;

                string[] split = ipaddress.Split(new Char[] { ' ', ',', '.', ':', '\t' });

                g_ndevIdA_1 = Int16.Parse(split[0].ToString());
                g_ndevIdA_2 = Int16.Parse(split[1].ToString());
                g_ndevIdA_3 = Int16.Parse(split[2].ToString());
                g_ndevIdA_4 = Int16.Parse(split[3].ToString());
                // ----------                
            }  
        }

        public static string PAIX_IP2
        {
            get { return _PAIX_IP2; }
            set
            {
                _PAIX_IP2 = value;

                // 여기서 IPAddress 쪼개서 g_ndevId, g_ndevId_1, g_ndevId_2, g_ndevId_3에 채워주어야 함.
                // ----------
                string ipaddress = _PAIX_IP2;

                string[] split = ipaddress.Split(new Char[] { ' ', ',', '.', ':', '\t' });

                g_ndevIdC_11 = Int16.Parse(split[0].ToString());
                g_ndevIdC_22 = Int16.Parse(split[1].ToString());
                g_ndevIdC_33 = Int16.Parse(split[2].ToString());
                g_ndevIdC_44 = Int16.Parse(split[3].ToString());
                // ----------                
            }
        }

        public static string PAIX_Port
        {
            get { return _PAIX_Port; }
            set { _PAIX_Port = value; }
        }
        public static string PAIX_Port2
        {
            get { return _PAIX_Port2; }
            set { _PAIX_Port2 = value; }
        }
#endregion PAIX ...




#region 시리얼 포트 ...
        
        public static int LightingComPort = 1;
        public static int WeldingComPort = 1;
        public static int TilTingComPort = 1;
        public static int TilTingAxis = 1;

#endregion 시리얼 포트 ...


        // 장비 설정값 가져오기
        public static void Read()
        {
            if (File.Exists(ConfigManager.GetXmlFilePath))
            {
                XmlDocument xmlDocument = new XmlDocument();

                xmlDocument.Load(ConfigManager.GetXmlFilePath);


                // 모션 컨트롤러 2
                // ----------
                XmlElement pSelected = (XmlElement)xmlDocument.DocumentElement.SelectSingleNode("PAIX1");

                DeviceManagerS.PAIX_Model = pSelected.GetAttribute("MODEL");
                DeviceManagerS.PAIX_IP = pSelected.GetAttribute("IP");
                DeviceManagerS.PAIX_Port = pSelected.GetAttribute("PORT");

                // 모션 컨트롤러 2
                // ----------
                XmlElement pSelected2 = (XmlElement)xmlDocument.DocumentElement.SelectSingleNode("PAIX2");

                DeviceManagerS.PAIX_Model2 = pSelected2.GetAttribute("MODEL");
                DeviceManagerS.PAIX_IP2 = pSelected2.GetAttribute("IP");
                DeviceManagerS.PAIX_Port2 = pSelected2.GetAttribute("PORT");


                // 기타
                // ----------
                pSelected = (XmlElement)xmlDocument.DocumentElement.SelectSingleNode("WELDING");
                DeviceManagerS.WeldingComPort = int.Parse(pSelected.GetAttribute("COMPORT"));

                pSelected = (XmlElement)xmlDocument.DocumentElement.SelectSingleNode("LIGHTING");
                DeviceManagerS.LightingComPort = int.Parse(pSelected.GetAttribute("COMPORT"));


                // ----------
                pSelected = (XmlElement)xmlDocument.DocumentElement.SelectSingleNode("SelectedModelInfo");   // 현재 모델 정보 ...
                DataManager.SelectedModelName = pSelected.GetAttribute("ModelName");


                pSelected = (XmlElement)xmlDocument.DocumentElement.SelectSingleNode("MOTIONPOS");   // 현재 모델 정보 ...
                double dtemp = double.Parse(pSelected.GetAttribute("P90"));

                if (dtemp < DataManager.bsinmove94 - DataManager.dgab || dtemp > DataManager.bsinmove94 + DataManager.dgab)
                {
                    DataManager.sinmove94 = DataManager.bsinmove94;
                }
                else
                {
                    DataManager.sinmove94 = dtemp;
                }


                dtemp = double.Parse(pSelected.GetAttribute("P00"));
                if (dtemp < DataManager.bsinmove4 - DataManager.dgab || dtemp > DataManager.bsinmove4 + DataManager.dgab)
                {
                    DataManager.sinmove4 = DataManager.bsinmove4;
                }
                else
                {
                    DataManager.sinmove4 = dtemp;
                }

                dtemp = double.Parse(pSelected.GetAttribute("P45"));
                if (dtemp < DataManager.bsinmove49 - DataManager.dgab || dtemp > DataManager.bsinmove49 + DataManager.dgab)
                {
                    DataManager.sinmove49 = DataManager.bsinmove49;
                }
                else
                {
                    DataManager.sinmove49 = dtemp;
                }
            }
        }

        // 장비 설정값 저장하기
        public static void Write()
        {
            if (File.Exists(ConfigManager.GetXmlFilePath))
            {
                XmlDocument xmlDocument = new XmlDocument();

                xmlDocument.Load(ConfigManager.GetXmlFilePath);


                // 모션 컨트롤러 1
                // ----------
                XmlElement pSelected = (XmlElement)xmlDocument.DocumentElement.SelectSingleNode("PAIX1");

                pSelected.SetAttribute("MODEL", DeviceManagerS.PAIX_Model);
                pSelected.SetAttribute("IP", DeviceManagerS.PAIX_IP);
                pSelected.SetAttribute("PORT", DeviceManagerS.PAIX_Port);

                XmlElement pSelected2 = (XmlElement)xmlDocument.DocumentElement.SelectSingleNode("PAIX2");

                pSelected2.SetAttribute("MODEL", DeviceManagerS.PAIX_Model2);
                pSelected2.SetAttribute("IP", DeviceManagerS.PAIX_IP2);
                pSelected2.SetAttribute("PORT", DeviceManagerS.PAIX_Port2);

                // 기타
                // ----------
                pSelected = (XmlElement)xmlDocument.DocumentElement.SelectSingleNode("WELDING");    // 감마 프로그램 인터페이스 포트 ...
                pSelected.SetAttribute("COMPORT", DeviceManagerS.WeldingComPort.ToString());

                pSelected = (XmlElement)xmlDocument.DocumentElement.SelectSingleNode("LIGHTING");   // 조명 포트 ...
                pSelected.SetAttribute("COMPORT", DeviceManagerS.LightingComPort.ToString());




                xmlDocument.Save(ConfigManager.GetXmlFilePath);
            }
        }

        public static void Write_SelectedModelInfo()
        {
            if (File.Exists(ConfigManager.GetXmlFilePath))
            {
                XmlDocument xmlDocument = new XmlDocument();

                xmlDocument.Load(ConfigManager.GetXmlFilePath);

                XmlElement pSelected = (XmlElement)xmlDocument.DocumentElement.SelectSingleNode("SelectedModelInfo");   // 현재 모델 정보 ...
                pSelected.SetAttribute("ModelName", DataManager.SelectedModelName);

                xmlDocument.Save(ConfigManager.GetXmlFilePath);
            }
        }


        public static void Write_MotionPos()
        {
            if (File.Exists(ConfigManager.GetXmlFilePath))
            {
                XmlDocument xmlDocument = new XmlDocument();

                xmlDocument.Load(ConfigManager.GetXmlFilePath);

                XmlElement pSelected = (XmlElement)xmlDocument.DocumentElement.SelectSingleNode("MOTIONPOS");   // 현재 모델 정보 ...
                pSelected.SetAttribute("P90", DataManager.sinmove94.ToString());
                pSelected.SetAttribute("P00", DataManager.sinmove4.ToString());
                pSelected.SetAttribute("P45", DataManager.sinmove49.ToString());

                xmlDocument.Save(ConfigManager.GetXmlFilePath);
            }
        }

#region 사운드 ...

        public static MediaPlayer.MediaPlayerClass _player = new MediaPlayer.MediaPlayerClass();

        public static void PlaySoundM4A(string filename)
        {
            string PathName_Sound = "C:\\KSM\\DiaDetector\\Sound\\" + filename;

            _player.FileName = PathName_Sound;
            _player.Play();
        }

#endregion
    }
}

