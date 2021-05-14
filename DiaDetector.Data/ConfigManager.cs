using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// ----------

using System.Collections;
using System.IO;
using System.Xml;

namespace DiaDetector.Data
{
    public static class ConfigManager
    {
        private static XmlDocument xmlDocument;

        private static string _startUPPath = @"c:\KSM\DiaDetector";                 // 프로그램 시작 위치
        public static string StartUPPath
        {
            get { return ConfigManager._startUPPath; }
            set { ConfigManager._startUPPath = value; }
        }

        private static string DataFilePath = @"c:\KSM\DiaDetector\Data\";           // 데이터 파일 경로
        public static string GetDataFilePath
        {
            get { return DataFilePath; }
        }

        public static string SetXmlDataFilePath
        {
            set { DataFilePath = value; }
        }

        private static string XmlFilePath = @"c:\KSM\DiaDetector\Data\config.xml";  // 장비설정 파일 경로
        public static string GetXmlFilePath
        {
            get { return XmlFilePath; }
        }

        public static string SetXmlFilePath
        {
            set { XmlFilePath = value; }
        }


#region 모델

        private static string ModelFilePath = @"c:\KSM\DiaDetector\Data\Model\";

        public static string GetModelFilePath
        {
            get { return ModelFilePath; }
        }

        public static string SetModelFilePath
        {
            set { ModelFilePath = value; }
        }


#endregion 모델


#region 장치 환경 설정

        private static string _PAIX_IPAddress = string.Empty;   // PAIX 모션 컨트롤러 IP 주소
        public static string PAIX_IPAddress
        {
            get { return _PAIX_IPAddress; }
            set { _PAIX_IPAddress = value; }
        }

#endregion 장치 환경 설정


    }
}
