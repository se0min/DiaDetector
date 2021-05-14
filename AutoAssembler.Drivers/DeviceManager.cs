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

using AutoAssembler.Data;
using MediaPlayer;

namespace AutoAssembler.Drivers
{
    public class DeviceManager
    {

#region PAIX 1 ...

        // PAIX 장치 환경 설정 정보
        private static string _PAIX_Model = string.Empty;
        private static string _PAIX_IP = string.Empty;
        private static string _PAIX_Port = string.Empty;

        public static string PAIX_Model
        {
            get { return _PAIX_Model; }
            set { _PAIX_Model = value; }
        }

        // 홍동성 20160331
        // PAIX IPAddress 나머지 정보들 => nmc_SetIPAddress에서 사용해야 함.
        // --------------------------------------------------
        public static short g_ndevIdA_1;
        public static short g_ndevIdA_2;
        public static short g_ndevIdA_3;
        public static short g_ndevIdA_4;

        public static short g_nDevopen;

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

        public static string PAIX_Port
        {
            get { return _PAIX_Port; }
            set { _PAIX_Port = value; }
        }

#endregion PAIX ...


#region PAIX 2 ...

        // PAIX 장치 환경 설정 정보
        private static string _PAIX2_Model = string.Empty;
        private static string _PAIX2_IP = string.Empty;
        private static string _PAIX2_Port = string.Empty;

        public static string PAIX2_Model
        {
            get { return _PAIX2_Model; }
            set { _PAIX2_Model = value; }
        }

        // ----------
        public static short g_ndevIdB_1;
        public static short g_ndevIdB_2;
        public static short g_ndevIdB_3;
        public static short g_ndevIdB_4;

        //public static short g_nDevopen;

        public static string PAIX2_IP
        {
            get { return _PAIX2_IP; }
            set
            {
                _PAIX2_IP = value;
                
                // 여기서 IPAddress 쪼개서 g_ndevId, g_ndevId_1, g_ndevId_2, g_ndevId_3에 채워주어야 함.
                // ----------
                string ipaddress = _PAIX2_IP;

                string[] split = ipaddress.Split(new Char[] { ' ', ',', '.', ':', '\t' });

                g_ndevIdB_1 = Int16.Parse(split[0].ToString());
                g_ndevIdB_2 = Int16.Parse(split[1].ToString());
                g_ndevIdB_3 = Int16.Parse(split[2].ToString());
                g_ndevIdB_4 = Int16.Parse(split[3].ToString());
                // ----------                
            }
        }

        public static string PAIX2_Port
        {
            get { return _PAIX2_Port; }
            set { _PAIX2_Port = value; }
        }

#endregion PAIX 2 ...


#region PAIX 3 ...

        // PAIX 장치 환경 설정 정보
        private static string _PAIX3_Model = string.Empty;
        private static string _PAIX3_IP = string.Empty;
        private static string _PAIX3_Port = string.Empty;

        public static string PAIX3_Model
        {
            get { return _PAIX3_Model; }
            set { _PAIX3_Model = value; }
        }

        // ----------
        public static short g_ndevIdC_1;
        public static short g_ndevIdC_2;
        public static short g_ndevIdC_3;
        public static short g_ndevIdC_4;

        //public static short g_nDevopen;

        public static string PAIX3_IP
        {
            get { return _PAIX3_IP; }
            set
            {
                _PAIX3_IP = value;

                // 여기서 IPAddress 쪼개서 g_ndevId, g_ndevId_1, g_ndevId_2, g_ndevId_3에 채워주어야 함.
                // ----------
                string ipaddress = _PAIX3_IP;

                string[] split = ipaddress.Split(new Char[] { ' ', ',', '.', ':', '\t' });

                g_ndevIdC_1 = Int16.Parse(split[0].ToString());
                g_ndevIdC_2 = Int16.Parse(split[1].ToString());
                g_ndevIdC_3 = Int16.Parse(split[2].ToString());
                g_ndevIdC_4 = Int16.Parse(split[3].ToString());
                // ----------                
            }
        }

        public static string PAIX3_Port
        {
            get { return _PAIX3_Port; }
            set { _PAIX3_Port = value; }
        }

#endregion PAIX 3 ...


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

                DeviceManager.PAIX_Model = pSelected.GetAttribute("MODEL");
                DeviceManager.PAIX_IP = pSelected.GetAttribute("IP");
                DeviceManager.PAIX_Port = pSelected.GetAttribute("PORT");


                // 모션 컨트롤러 2
                // ----------
                pSelected = (XmlElement)xmlDocument.DocumentElement.SelectSingleNode("PAIX2");

                DeviceManager.PAIX2_Model = pSelected.GetAttribute("MODEL");
                DeviceManager.PAIX2_IP = pSelected.GetAttribute("IP");
                DeviceManager.PAIX2_Port = pSelected.GetAttribute("PORT");

                // 모션 컨트롤러 3
                // ----------
                pSelected = (XmlElement)xmlDocument.DocumentElement.SelectSingleNode("PAIX3");

                DeviceManager.PAIX3_Model = pSelected.GetAttribute("MODEL");
                DeviceManager.PAIX3_IP = pSelected.GetAttribute("IP");
                DeviceManager.PAIX3_Port = pSelected.GetAttribute("PORT");


                // 기타
                // ----------
                pSelected = (XmlElement)xmlDocument.DocumentElement.SelectSingleNode("WELDING");
                DeviceManager.WeldingComPort = int.Parse(pSelected.GetAttribute("COMPORT"));

                pSelected = (XmlElement)xmlDocument.DocumentElement.SelectSingleNode("LIGHTING");
                DeviceManager.LightingComPort = int.Parse(pSelected.GetAttribute("COMPORT"));

                pSelected = (XmlElement)xmlDocument.DocumentElement.SelectSingleNode("TILTING");   // 틸팅 설정 ...
                DeviceManager.TilTingComPort = int.Parse(pSelected.GetAttribute("COMPORT"));
                DeviceManager.TilTingAxis = int.Parse(pSelected.GetAttribute("AXIS"));

                pSelected = (XmlElement)xmlDocument.DocumentElement.SelectSingleNode("LASER");   // 레이저 설정 ...
                MultiMotion.dWeldStartBaseX = double.Parse(pSelected.GetAttribute("StartX"));
                MultiMotion.dWeldStartBaseZ = double.Parse(pSelected.GetAttribute("StartZ"));

                pSelected = (XmlElement)xmlDocument.DocumentElement.SelectSingleNode("ROLLING");   // 롤링 설정 ...
                MultiMotion.dIndex_XPos = double.Parse(pSelected.GetAttribute("XLen"));
                MultiMotion.dIndex_XOffset = double.Parse(pSelected.GetAttribute("XOffset"));



                pSelected = (XmlElement)xmlDocument.DocumentElement.SelectSingleNode("SelectedModelInfo");   // 현재 모델 정보 ...
                DataManager.SelectedModelName = pSelected.GetAttribute("ModelName");

                                


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

                pSelected.SetAttribute("MODEL", DeviceManager.PAIX_Model);
                pSelected.SetAttribute("IP", DeviceManager.PAIX_IP);
                pSelected.SetAttribute("PORT", DeviceManager.PAIX_Port);


                // 모션 컨트롤러 2
                // ----------
                pSelected = (XmlElement)xmlDocument.DocumentElement.SelectSingleNode("PAIX2");

                pSelected.SetAttribute("MODEL", DeviceManager.PAIX2_Model);
                pSelected.SetAttribute("IP", DeviceManager.PAIX2_IP);
                pSelected.SetAttribute("PORT", DeviceManager.PAIX2_Port);


                // 모션 컨트롤러 3
                // ----------
                pSelected = (XmlElement)xmlDocument.DocumentElement.SelectSingleNode("PAIX3");

                pSelected.SetAttribute("MODEL", DeviceManager.PAIX3_Model);
                pSelected.SetAttribute("IP", DeviceManager.PAIX3_IP);
                pSelected.SetAttribute("PORT", DeviceManager.PAIX3_Port);


                // 기타
                // ----------
                pSelected = (XmlElement)xmlDocument.DocumentElement.SelectSingleNode("WELDING");    // 감마 프로그램 인터페이스 포트 ...
                pSelected.SetAttribute("COMPORT", DeviceManager.WeldingComPort.ToString());

                pSelected = (XmlElement)xmlDocument.DocumentElement.SelectSingleNode("LIGHTING");   // 조명 포트 ...
                pSelected.SetAttribute("COMPORT", DeviceManager.LightingComPort.ToString());


                // 틸팅 설정 ...
                // ----------
                pSelected = (XmlElement)xmlDocument.DocumentElement.SelectSingleNode("TILTING");
                pSelected.SetAttribute("COMPORT", DeviceManager.TilTingComPort.ToString());
                pSelected.SetAttribute("AXIS", DeviceManager.TilTingAxis.ToString());

                // 레이저 설정 ...
                // ----------
                pSelected = (XmlElement)xmlDocument.DocumentElement.SelectSingleNode("LASER");
                pSelected.SetAttribute("StartX", MultiMotion.dWeldStartBaseX.ToString());
                pSelected.SetAttribute("StartZ", MultiMotion.dWeldStartBaseZ.ToString());

                // 롤링 설정 ...
                // ----------
                pSelected = (XmlElement)xmlDocument.DocumentElement.SelectSingleNode("ROLLING");
                pSelected.SetAttribute("XLen", MultiMotion.dIndex_XPos.ToString());
                pSelected.SetAttribute("XOffset", MultiMotion.dIndex_XOffset.ToString());


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

#region 사운드 ...

        public static MediaPlayer.MediaPlayerClass _player = new MediaPlayer.MediaPlayerClass();

        public static void PlaySoundM4A(string filename)
        {
            string PathName_Sound = "C:\\KSM\\Sound\\" + filename;

            _player.FileName = PathName_Sound;
            _player.Play();


            /*
            PlaySoundM4A("고정축상방_이동축전방.m4a");
            PlaySoundM4A("고정축상방_이동축후방.m4a");
            PlaySoundM4A("고정축전방_이동축전방.m4a");
            PlaySoundM4A("고정축전방_이동축후방.m4a");
            PlaySoundM4A("고정축후방_이동축전방.m4a");
            PlaySoundM4A("고정축후방_이동축후방.m4a");
            PlaySoundM4A("검사를시작합니다.m4a");
            PlaySoundM4A("롤링이끝났습니다.m4a");
            PlaySoundM4A("오빠용접끝났어.m4a");
            PlaySoundM4A("용접이끝났습니다.m4a");
            PlaySoundM4A("용접이끝났으니제품을꺼내주세요 (2).m4a");
            PlaySoundM4A("용접이끝났으니제품을꺼내주세요(3).m4a");
            PlaySoundM4A("용접이끝났으니제품을꺼내주세요.m4a");
            PlaySoundM4A("캡슐을투입하여주십시오.m4a");
            */
        }

#endregion
    }
}

