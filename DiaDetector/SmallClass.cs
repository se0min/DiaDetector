using DiaDetector.Drivers;
using PAIX_NMF_DEV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiaDetector
{
    class SmallClass
    {
        public static double shuttleMove1 = ClassType.sinmoveShuttle1;
        public static double shuttleMove2 = ClassType.sinmoveShuttle2;

        public static short SetPin(short nNmfNo, short nPinNo, short nOutStatus)
        {
            //SubForm.SmallLog.Instance.LogReport(string.Format("핀번호={0} 상태값={1}", nPinNo, nOutStatus));
            return NMC2.nmc_SetDIOOutputBit(nNmfNo, nPinNo, nOutStatus);
        }

        /*
         IN (NMC2)
         * 0    시작 버튼
         * 1    정지 버튼
         * 2    리셋 버튼
         * 3    에어리어 센서1
         * 4    에어리어 센서2
         * 5    에어리어 센서3
         * 6    에어리어 센서4
         * 7    X
         * 8    리프트 실린더A 업 센서
         * 9    리프트 실린더A 다운 센서
         * 10   리프트 실린더B 업 센서
         * 11   리프트 실린더B 다운 센서
         * 12   리프트 실린더C 업 센서
         * 13   리프트 실린더C 다운 센서
         * 14   X
         * 15   X
         * 16   셔틀 클램프1
         * 17   셔틀 클램프1
         * 18   셔틀 클램프1
         * 19   셔틀 클램프1
         * 20   셔틀 클램프2
         * 21   셔틀 클램프2
         * 22   셔틀 클램프2
         * 23   셔틀 클램프2
         * 24   베큠 센서A
         * 25   베큠 센서B
         * 26   하프 센서1
         * 27   풀 센서1
         * 28   하프 센서2
         * 29   풀 센서2
         * 30   리프트 센서1
         * 31   리프트 센서2
         
         IN (NMF) - 3호기 추가 설치
         * 0    베큠 센서C
         * 1    베큠 센서D
         * 2    X


         OUT
         * 0    시작 버튼 램프
         * 1    정지 버튼 램프
         * 2    리셋 버튼 램프
         * 3    X
         * 4    베큠 솔레노이드A
         * 5    베큠 솔레노이드B
         * 6    베큠 솔레노이드C
         * 7    베큠 솔레노이드D
         * 8    리프트 업/다운 솔레노이드
         * 9    X
         * 10   X
         * 11   X
         * 12   리프트C 흡입
         * 13   리프트D 흡입
         * 14   리프트C 퍼지
         * 15   리프트D 퍼지
         * 16   셔틀1 클로즈
         * 17   셔틀1 오픈
         * 18   X
         * 19   X
         * 20   셔틀2 클로즈
         * 21   셔틀2 오픈
         * 22   X
         * 23   X
         * 24   리프트A 흡입
         * 25   리프트B 흡입
         * 26   리프트A 퍼지
         * 27   리프트B 퍼지
         * 28   적색 램프 (0=off,1=on)
         * 29   황색 램프 (0=off,1=on)
         * 30   녹색 램프 (0=off,1=on)
         * 31   알람 부저 (0=off,1=on)
         */
    }
}
