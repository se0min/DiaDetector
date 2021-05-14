using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoAssembler.Data
{
    public struct MotionSettingInfo
    {
        public string Name;
        public int MotionType; // 축에 제어 타입을 할당한다. => 제어 목록 인덱스 값을 할당한다. (홍동성)
        public double MaxValue;
        public double MinValue;

        // PAIX SDK 참고하여 추가한 변수들 ...
        // --------------------------------------------------
        public double Velocity_Start;
        public double Velocity_Acc;
        public double Velocity_Dec;
        public double Velocity_Max;

        public int Logic_Emergency;
        public double Logic_UnitPerPulse;
        public int Logic_Enc;
        public int Logic_EncZ;
        public int Logic_Enc_Input;
        public int Logic_Near;
        public int Logic_Limit_Minus;
        public int Logic_Limit_Plus;
        public int Logic_Alarm;
        public int Logic_HomeMode;
        public int Logic_PulseMode;

        // ----------
        public double BallScrew_Lead;               // 볼 스크류 리드 => Rotate Factor로 변경? => 감속비까지 적용해야 할 수 있음
        public double BallScrew_Diameter;           // 볼 스크류 지름
        public double Servo_Amplifier_Resolution;   // 서보 앰프 해상도

        // 원점 및 원점 복귀 속도 ...
        // ----------
        public double Velocity_Home_1;
        public double Velocity_Home_2;
        public double Velocity_Home_3;
        public double Velocity_Home_Offset;
        public double Home_Offset;

    }
}
