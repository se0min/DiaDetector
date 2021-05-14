﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiaDetector.Data
{
    public struct ModelListInfo
    {
        public bool     ModelExistFlag;
        public string   ModelName;
        public string   ModelFileName_delete;
        public int      SelectStatus;
        public int      NGLoopCount;
        public string   ImageFileName;

        // ----------

        public int nAlignSetIndex;
        public int nField01;
        public int nField02;
        public int nField03;
        public int nField04;
        public int nField05;
        public int nField06;
        public int nField07;
        public int nField08;
        public int nField09;

        public double ndblOutDiameter;
        public double Camera1Moving;
        public double Camera2Moving;
        public double txtSTValueA;
        public double txtSTValueB;
        public double OutPie;
        public double InPie;
        public double NamePie;
        public double nField18;
        public double nField19;

        public double dFLValue;
        public double dMetalThick1;
        public double dMetalThick2;
        public double dSLValue;
        public double dWRValue;
        public double dCapsulePie;
        public double dVBlockFL_Offset_Value;
        public double dVBlockFL_Limit_Value;
        public double dRolling70Rate;
        public double dRolling80Rate;

        public double dRollingOffset;
        public double dRotateCount;
        public double dField12;
        public double dField13;
        public double dField14;
        public double dField15;
        public double dField16;
        public double dField17;
        public double dField18;
        public double dField19;

        public string NamePie1;
        public string sField01;
        public string sField02;
        public string sField03;
        public string sField04;
        public string sField05;
        public string sField06;
        public string sField07;
        public string sField08;
        public string sField09;

        public string sField10;
        public string sField11;
        public string sField12;
        public string sField13;
        public string sField14;
        public string sField15;
        public string sField16;
        public string sField17;
        public string sField18;
        public string sField19;
    }
}
