using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoAssembler.Data
{
    public struct WorkFuncInfo
    {
        public bool WFRunFlag;
        public int WFType;
        public string WFName;
        

        // 모션
        // ----------
        public int WFMotionChannel;
        

        // DIO
        // ----------
        public int WFDIOPortNum;


        // 조명
        // ----------
        public int WFLampChannel;
        public int WFLampValue;         // 2016-05-26 - WFField_2 변수를 리팩토링 해서 사용 => 파일 포맷 변경 없음.
        

        // 회전
        // ----------
        public double WFRotationAngle;
        public double WFRotationCount;  // WFRotationCount => Ball Screw Lead : 20160517


        // 상태 이미지
        // ----------
        public string StepImageFileName;


        // 이동
        // ----------
        public double WFMoveX;
        public double WFMoveY;
        public double WFMoveZ;

        public double WFMoveX2;
        public double WFMoveY2;
        public double WFMoveZ2;
      
        public double WFMoveX3;
        public double WFMoveY3;
        public double WFMoveZ3;
        

        // ----------
        public bool WFDioOnOff;
        public double WFLampNum;


        // 홍동성 추가 ... => 프로그램 실행시의 상태값 ...
        // --------------------------------------------------
        public bool TestProcExistFlag;
        public string TestProcName;
        public int TestProcIndex;

        public int TestProcStatus;
        public int T_TestResultSts;
        // --------------------------------------------------


        // 20160429
        // ----------
        public int WFDelayTime;


        // 20160622 => 인식용 Data파일 세팅
        // ----------
        public string WFRecoDatHeadFileName;


        // 20160622 => 인식용 대화상자 설정 정보
        // ----------
        public int Rotation_Index;
        public int Lamp_Index;
        public int Motion_Index;
        public int RecoCamIndex;


        // 속도 배속 ... => 기본 속도가 너무 낮아서 추가함.(홍동성 - 20160714)        
        // ----------
        public int Velocity_Multiple;
        // ----------

        public double dWFRollingValue;


        // Rolling UI
        public double dMetalThick1;
        public double dMetalThick2;
        public double dFLValue;
        public double dSLValue;
        public double dCapsule;
        public double dWRValue;

        public double dRollingOffset;
        


        public double dRolling70;
        public double dRolling80;



        // 용접용 변수
        // ----------
        public int AxisSpeed;
        public double dSpotLaserOutput;
        public double dWeldSolidRate;



        // double 여유 필드
        // ----------
        public double dIndexRotateValue;
        public double dVBlockFL_Limit;
        public double dVBlockFL_Offset;


        public double sWFField5;
        public double sWFField6;
        public double sWFField7;


        // 개별 축 이동
        // ----------        
        public int ReturnHome;
        public int SelectedAxis;
        public int AxisEndWait;

    }
}
