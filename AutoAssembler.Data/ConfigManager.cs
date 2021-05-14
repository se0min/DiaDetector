using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//////////

using System.Collections;
using System.IO;
using System.Xml;

namespace AutoAssembler.Data
{
    public static class ConfigManager
    {
        private static XmlDocument xmlDocument;

        private static string _startUPPath = @"c:\KSM\AutoAssembler";   // 프로그램 시작 위치
        public static string StartUPPath
        {
            get { return ConfigManager._startUPPath; }
            set { ConfigManager._startUPPath = value; }
        }

        private static string DataFilePath = @"c:\KSM\Data\";        // 데이터 파일 경로
        public static string GetDataFilePath
        {
            get { return DataFilePath; }
        }

        public static string SetXmlDataFilePath
        {
            set { DataFilePath = value; }
        }

        private static string XmlFilePath = @"c:\KSM\Data\config.xml";  // 장비설정 파일 경로
        public static string GetXmlFilePath
        {
            get { return XmlFilePath; }
        }

        public static string SetXmlFilePath
        {
            set { XmlFilePath = value; }
        }



#region 모델

        private static string ModelFilePath = @"c:\KSM\Data\Model\";

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


        public static string GetVanName
        {
            get
            {
                xmlDocument = new XmlDocument();
                xmlDocument.Load(XmlFilePath);

                string bRet = string.Empty;
                if (xmlDocument != null)
                {
                    XmlNodeList vanName = xmlDocument.GetElementsByTagName("vanName");
                    bRet = vanName[0].InnerText;
                }

                return bRet;
            }
        }

        public static string SetVanName
        {
            set
            {
                xmlDocument = new XmlDocument();
                xmlDocument.Load(XmlFilePath);

                if (xmlDocument != null)
                {
                    XmlNodeList vanName = xmlDocument.GetElementsByTagName("vanName");
                    vanName[0].InnerText = value;
                    xmlDocument.Save(XmlFilePath);
                }
            }
        }
    }
}

