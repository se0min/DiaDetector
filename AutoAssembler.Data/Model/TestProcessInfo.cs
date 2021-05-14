using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoAssembler.Data
{
    public struct TotalTestResultInfo
    {
        public int Total_TestCount;
        public int OK_TestCount;
        public int NG_TestCount;
    }

    public struct TestResultInfo
    {
        public bool TestProcExistFlag;
        public string TestSerialNum;
        public int Res_OKNGType;
    }
}
