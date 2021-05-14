using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using AutoAssembler.Data;
using AutoAssembler.Utilities;

namespace AutoAssembler.Drivers
{
    public class MultiMotion
    {
        // 용접 프로그램 관련 좌표
        // --------------------------------------------------
        public static double dIndex_XPos = 1507.6 + 0.2;
        public static double dIndex_XOffset = 0.0;

        public static double dWeldStartBaseX = 268.075 - 8.9;
        public static double dWeldStartBaseZ = 337.75560 - 47.64;


        // 0~7번 축
        // --------------------------------------------------
        public static PaixMotion PaixMotionA;
        public static NMC2.NMCAXESEXPR NmcDataA;
        public static short g_nDevopenA;


        // 8~13번 축
        // --------------------------------------------------
        public static PaixMotion PaixMotionB;
        public static NMC2.NMCAXESEXPR NmcDataB;
        public static short g_nDevopenB;


        // DIO
        // --------------------------------------------------
        public static PaixMotion PaixMotionC;
        public static NMC2.NMCAXESEXPR NmcDataC;
        public static short g_nDevopenC;

        
        // 컨트롤러 및 축 지정
        // --------------------------------------------------
        private static short ControllerId = -1;
        public static PaixMotion CurPaixMotion = null;

        private static short AxisNumber = -1;
        private static short AxisNumber2 = -1;

        public static double[] AxisValue = new double[16];
        public static double[] AxisCmdValue = new double[16];
        public static short[] AlarmValue = new short[16];
        public static bool[] ReadyHome = new bool[16];      
        


        // 좌표
        // --------------------------------------------------
        // 본 프로그램 좌표        
        // ----------
        // Z축 330.220 + 45
        public static double dXPos_Vision_Unit = 254.5 + 20; // 20은 안전 수치
        public static double dXPos_Camera_Unit = 1450.0;


        // Rolling, 용접용 변수
        // --------------------------------------------------
        public static double dRolling70Value = 0.0;
        public static double dRolling80Value = 0.0;
        public static double dRolling100Value = 0.0;


        // 속도 배속 ... 
        // => 기본 속도가 너무 낮아서 추가함.(홍동성 - 20160714)        
        // => 저장은 WorkFuncInfo 클래스에서 함.
        
        public static int Velocity_Multiple = 1;        
        // --------------------------------------------------


        public static short[] OutStatus = new short[64];
        public static short[] InStatus = new short[64];

        public static short ErrorStatus = -1;
        private static bool bGetPosReady = false;


        // 개별 축 정보
        // --------------------------------------------------
        public static short CAM_UNIT_X      = -1;   // 카메라 유닛 X
        public static short CAM_UNIT_Y      = -1;   // 카메라 유닛 Y
        public static short CAM_UNIT_Z      = -1;   // 카메라 유닛 Z

        public static short BACK_CAM_Z      = -1;   // 백 캠 유닛 Z
        public static short VBLOCK_Z        = -1;   // V블럭 Z

        public static short ROLLING_FIX_1   = -1;   // 고정축 롤링 1
        public static short ROLLING_FIX_2   = -1;   // 고정축 롤링 2
        public static short ROLLING_MOVE_1  = -1;   // 이동축 롤링 1
        public static short ROLLING_MOVE_2  = -1;   // 이동축 롤링 2

        public static short INDEX_MOVE_M    = -1;   // INDEX 주행 M
        public static short INDEX_MOVE_S    = -1;   // INDEX 주행 S

        public static short INDEX_FIX_R     = -1;   // 고정축 INDEX 주행 R
        public static short INDEX_MOVE_R    = -1;   // 이동축 INDEX 주행 R


        // 모든 함수의 리턴값 
        // ----------
        public const short KSM_CONTI_BUF_FULL = -15;
        public const short KSM_CONTI_BUF_EMPTY = -14;
        public const short KSM_INTERPOLATION = -13;
        public const short KSM_FILE_LOAD_FAIL = -12;
        public const short KSM_ICMP_LOAD_FAIL = -11;
        public const short KSM_NOT_EXISTS = -10;
        public const short KSM_CMDNO_ERROR = -9;
        public const short KSM_NOTRESPONSE = -8;
        public const short KSM_BUSY = -7;
        public const short KSM_COMMERR = -6;
        public const short KSM_SYNTAXERR = -5;        
        public const short KSM_UNKOWN = -3;
        public const short KSM_SOCKINITERR = -2;
        // ----------
        public const short KSM_INVALID = -4;
        public const short KSM_NOTCONNECT = -1;
        public const short KSM_OK = 0;
        public const short KSM_ALARM = 1;
        // ----------        
        public const short KSM_HOME_FAIL = 3;


        // 모션 컨트롤러 전원 OFF 대응 변수 ...
        // ----------
        public static bool bPingCheckedA = false;
        public static bool bPingCheckedB = false;
        public static bool bPingCheckedC = false;
        public static bool bSystemReady = false;
        

        // 속도 관련 ...
        // ----------
        public const int KSM_SPEED_SSLOW = 0;
        public const int KSM_SPEED_SLOW = 1;
        public const int KSM_SPEED_MIDIUM = 2;
        public const int KSM_SPEED_FAST = 3;

        public static double dSpeedFactor = 1.0;
        public static int nSpeedMode = 0;


        // 좌표 관련 ...
        // ----------
        public static bool bUpdatedSecondPoint = false;


        public static bool bRestart = false;
        public static bool bEAutoStop = false;
        


#region 장비 고정치 ...

        // 이 장비 원점 고정을 위해 홈 찾는 속도는 고정되어야 하므로 생긴 변수와 함수
        public static MotionSettingInfo[] MotionSettingInfoList = new MotionSettingInfo[16];

        
        public static void InitMotionSettingInfo()
        {
            for (int i = 0; i < 16; i++)
            {
                MultiMotion.MotionSettingInfoList[i].Name = "";
                MultiMotion.MotionSettingInfoList[i].MotionType = 0;
                MultiMotion.MotionSettingInfoList[i].MaxValue = 0.0;
                MultiMotion.MotionSettingInfoList[i].MinValue = 0.0;


                // PAIX SDK 참고하여 추가한 변수들 ...
                // --------------------------------------------------
                MultiMotion.MotionSettingInfoList[i].Velocity_Start = 0.0;
                MultiMotion.MotionSettingInfoList[i].Velocity_Acc = 0.0;
                MultiMotion.MotionSettingInfoList[i].Velocity_Dec = 0.0;
                MultiMotion.MotionSettingInfoList[i].Velocity_Max = 0.0;

                MultiMotion.MotionSettingInfoList[i].Logic_Emergency = 0;
                MultiMotion.MotionSettingInfoList[i].Logic_UnitPerPulse = 0.0;
                MultiMotion.MotionSettingInfoList[i].Logic_Enc = 0;
                MultiMotion.MotionSettingInfoList[i].Logic_EncZ = 0;
                MultiMotion.MotionSettingInfoList[i].Logic_Enc_Input = 0;
                MultiMotion.MotionSettingInfoList[i].Logic_Near = 0;
                MultiMotion.MotionSettingInfoList[i].Logic_Limit_Minus = 0;
                MultiMotion.MotionSettingInfoList[i].Logic_Limit_Plus = 0;
                MultiMotion.MotionSettingInfoList[i].Logic_Alarm = 0;
                MultiMotion.MotionSettingInfoList[i].Logic_HomeMode = 0;
                MultiMotion.MotionSettingInfoList[i].Logic_PulseMode = 0;


                // 원점 관련 변수 추가 ... => 20160707
                // --------------------------------------------------
                MultiMotion.MotionSettingInfoList[i].Velocity_Home_1 = 0.0;
                MultiMotion.MotionSettingInfoList[i].Velocity_Home_2 = 0.0;
                MultiMotion.MotionSettingInfoList[i].Velocity_Home_3 = 0.0;
                MultiMotion.MotionSettingInfoList[i].Velocity_Home_Offset = 0.0;
                MultiMotion.MotionSettingInfoList[i].Home_Offset = 0.0;
            }


            



            // 8-0
            // --------------------------------------------------
            // 아래 코든는 원점 속도 고정을 위해 이 장비에만 적용된 고정치임.
            MultiMotion.MotionSettingInfoList[0].Velocity_Start = 1.0;
            MultiMotion.MotionSettingInfoList[0].Velocity_Acc = 100.0;
            MultiMotion.MotionSettingInfoList[0].Velocity_Dec = 100.0;
            MultiMotion.MotionSettingInfoList[0].Velocity_Max = 5.0;

            // 8-1
            MultiMotion.MotionSettingInfoList[1].Velocity_Start = 1.0;
            MultiMotion.MotionSettingInfoList[1].Velocity_Acc = 100.0;
            MultiMotion.MotionSettingInfoList[1].Velocity_Dec = 100.0;
            MultiMotion.MotionSettingInfoList[1].Velocity_Max = 5.0;

            // 8-2
            MultiMotion.MotionSettingInfoList[2].Velocity_Start = 1.0;
            MultiMotion.MotionSettingInfoList[2].Velocity_Acc = 100.0;
            MultiMotion.MotionSettingInfoList[2].Velocity_Dec = 100.0;
            MultiMotion.MotionSettingInfoList[2].Velocity_Max = 5.0;

            // 8-3
            MultiMotion.MotionSettingInfoList[3].Velocity_Start = 1.0;
            MultiMotion.MotionSettingInfoList[3].Velocity_Acc = 100.0;
            MultiMotion.MotionSettingInfoList[3].Velocity_Dec = 100.0;
            MultiMotion.MotionSettingInfoList[3].Velocity_Max = 5.0;

            // 8-4
            MultiMotion.MotionSettingInfoList[4].Velocity_Start = 1.0;
            MultiMotion.MotionSettingInfoList[4].Velocity_Acc = 100.0;
            MultiMotion.MotionSettingInfoList[4].Velocity_Dec = 100.0;
            MultiMotion.MotionSettingInfoList[4].Velocity_Max = 5.0;

            // 8-5
            MultiMotion.MotionSettingInfoList[5].Velocity_Start = 1.0;
            MultiMotion.MotionSettingInfoList[5].Velocity_Acc = 100.0;
            MultiMotion.MotionSettingInfoList[5].Velocity_Dec = 100.0;
            MultiMotion.MotionSettingInfoList[5].Velocity_Max = 5.0;

            // 8-6
            MultiMotion.MotionSettingInfoList[6].Velocity_Start = 1.0;
            MultiMotion.MotionSettingInfoList[6].Velocity_Acc = 100.0;
            MultiMotion.MotionSettingInfoList[6].Velocity_Dec = 100.0;
            MultiMotion.MotionSettingInfoList[6].Velocity_Max = 10.0;



            // 6-0
            // --------------------------------------------------
            MultiMotion.MotionSettingInfoList[8].Velocity_Start = 1.0;
            MultiMotion.MotionSettingInfoList[8].Velocity_Acc = 20.0;
            MultiMotion.MotionSettingInfoList[8].Velocity_Dec = 20.0;
            MultiMotion.MotionSettingInfoList[8].Velocity_Max = 20.0;

            // 6-1
            MultiMotion.MotionSettingInfoList[9].Velocity_Start = 1.0;
            MultiMotion.MotionSettingInfoList[9].Velocity_Acc = 20.0;
            MultiMotion.MotionSettingInfoList[9].Velocity_Dec = 20.0;
            MultiMotion.MotionSettingInfoList[9].Velocity_Max = 20.0;

            // 6-2
            MultiMotion.MotionSettingInfoList[10].Velocity_Start = 1.0;
            MultiMotion.MotionSettingInfoList[10].Velocity_Acc = 40.0;
            MultiMotion.MotionSettingInfoList[10].Velocity_Dec = 40.0;
            MultiMotion.MotionSettingInfoList[10].Velocity_Max = 40.0;

            // 6-3
            MultiMotion.MotionSettingInfoList[11].Velocity_Start = 3.0;
            MultiMotion.MotionSettingInfoList[11].Velocity_Acc = 300.0;
            MultiMotion.MotionSettingInfoList[11].Velocity_Dec = 300.0;
            MultiMotion.MotionSettingInfoList[11].Velocity_Max = 15.0;

            // 6-4
            MultiMotion.MotionSettingInfoList[12].Velocity_Start = 1.0;
            MultiMotion.MotionSettingInfoList[12].Velocity_Acc = 20.0;
            MultiMotion.MotionSettingInfoList[12].Velocity_Dec = 20.0;
            MultiMotion.MotionSettingInfoList[12].Velocity_Max = 20.0;

            // 6-5
            MultiMotion.MotionSettingInfoList[13].Velocity_Start = 1.0;
            MultiMotion.MotionSettingInfoList[13].Velocity_Acc = 20.0;
            MultiMotion.MotionSettingInfoList[13].Velocity_Dec = 20.0;
            MultiMotion.MotionSettingInfoList[13].Velocity_Max = 20.0;

        }


#endregion 장비 고정치 ...


#region Initialize ...


        // 동시 2축을 지정 할 수 있다.
        public static void autoSetAxisNumberAndControllerId(int AxisIndex, int AxisIndex2)
        {
            // 컨트롤러 및 1번 축 ...
            // --------------------------------------------------
            if (AxisIndex > -1 && AxisIndex < 8)        // 모션 컨트롤러 A
            {
                MultiMotion.ControllerId = 11;

                MultiMotion.AxisNumber = (short)AxisIndex;

                MultiMotion.CurPaixMotion = MultiMotion.PaixMotionA;
            }
            else if (AxisIndex > 7 && AxisIndex < 14)   // 모션 컨트롤러 B
            {
                MultiMotion.ControllerId = 12;

                MultiMotion.AxisNumber = (short)(AxisIndex - 8);

                MultiMotion.CurPaixMotion = MultiMotion.PaixMotionB;
            }
            else // -1을 지정하여 DIO 보드를 지정하게 할 수 있다.
            {
                MultiMotion.ControllerId = 13;

                MultiMotion.CurPaixMotion = MultiMotion.PaixMotionC;
            }


            // 동시에 2개 축을 제어하는 경우 ...
            // --------------------------------------------------
            if (AxisIndex2 > -1)
            {
                if (AxisIndex > -1 && AxisIndex < 8)        // 모션 컨트롤러 A
                {
                    MultiMotion.AxisNumber2 = (short)AxisIndex2;
                }
                else                                        // 모션 컨트롤러 B
                {
                    MultiMotion.AxisNumber2 = (short)(AxisIndex2 - 8);
                }
            }
        }

        public static short Initialize()
        {
            short nRet = KSM_OK;


            MultiMotion.UpdateAxisInfo();


            // 1. 모션 컨트롤러 A 초기화 ...
            // --------------------------------------------------
            PaixMotionA = new PaixMotion();
            NmcDataA = new NMC2.NMCAXESEXPR();
            g_nDevopenA = 0;

            nRet = NMC2.nmc_SetIPAddress(DeviceManager.g_ndevIdA_4, DeviceManager.g_ndevIdA_1, DeviceManager.g_ndevIdA_2, DeviceManager.g_ndevIdA_3);
            /*            
            if (nRet != KSM_OK)
                return nRet;
            */

            bool bOpen = PaixMotionA.Open(DeviceManager.g_ndevIdA_4);
            if (bOpen)
                g_nDevopenA = 1;
            else
                return KSM_NOTCONNECT;
            

            // 2. 모션 컨트롤러 B 초기화 ...
            // --------------------------------------------------
            PaixMotionB = new PaixMotion();
            NmcDataB = new NMC2.NMCAXESEXPR();
            g_nDevopenB = 0;

            nRet = NMC2.nmc_SetIPAddress(DeviceManager.g_ndevIdB_4, DeviceManager.g_ndevIdB_1, DeviceManager.g_ndevIdB_2, DeviceManager.g_ndevIdB_3);
            /*            
            if (nRet != KSM_OK)
                return nRet;
            */

            bOpen = PaixMotionB.Open(DeviceManager.g_ndevIdB_4);
            if (bOpen)
                g_nDevopenB = 1;
            else
                return KSM_NOTCONNECT;
            

            // 3. 모션 컨트롤러 C 초기화 ...
            // --------------------------------------------------
            PaixMotionC = new PaixMotion();
            NmcDataC = new NMC2.NMCAXESEXPR();
            g_nDevopenC = 0;

            nRet = NMC2.nmc_SetIPAddress(DeviceManager.g_ndevIdC_4, DeviceManager.g_ndevIdC_1, DeviceManager.g_ndevIdC_2, DeviceManager.g_ndevIdC_3);
            /*            
            if (nRet != KSM_OK)
                return nRet;
            */

            bOpen = PaixMotionC.Open(DeviceManager.g_ndevIdC_4);
            if (bOpen)
                g_nDevopenC = 1;
            else
                return KSM_NOTCONNECT;


            // 4. 마그네틱 ON, Servo ON, 전원 공급 ...
            // ----------
            nRet = MultiMotion.SystemInit();
            if (nRet != MultiMotion.KSM_OK)
                return nRet;

            bSystemReady = true;
            

            return KSM_OK;
        }

        public static short Exit()
        {
            if (MultiMotion.g_nDevopenA == 1)
            {
                PaixMotionA.Close();
            }            

            if (MultiMotion.g_nDevopenB == 1)
            {
                PaixMotionB.Close();
            }

            if (MultiMotion.g_nDevopenC == 1)
            {
                PaixMotionC.Close();
            } 

            return KSM_OK;
        }

        public static short SystemInit()
        {
            short nRet = -1;


            // 1. 마그네틱 ON
            // ----------
            for (short i = 41; i < 56; i++)
            {
                nRet = NMC2.nmc_SetDIOOutputBit(DeviceManager.g_ndevIdC_4, i, 1);

                CommonUtility.WaitTime(50, true);

                if (nRet != 0)
                {
                    return nRet;
                }
            }

            // 4. Alarm Reset
            // ----------
            
            for (short i = 0; i < 8; i++)
            {
                NMC2.nmc_SetAlarmResetOn(DeviceManager.g_ndevIdA_4, i, 1);

                CommonUtility.WaitTime(50, true);
            }

            for (short i = 0; i < 6; i++)
            {
                NMC2.nmc_SetAlarmResetOn(DeviceManager.g_ndevIdB_4, i, 1);

                CommonUtility.WaitTime(50, true);
            }

            

            for (short i = 0; i < 8; i++)
            {
                NMC2.nmc_SetAlarmResetOn(DeviceManager.g_ndevIdA_4, i, 0);

                CommonUtility.WaitTime(50, true);
            }

            for (short i = 0; i < 6; i++)
            {
                NMC2.nmc_SetAlarmResetOn(DeviceManager.g_ndevIdB_4, i, 0);

                CommonUtility.WaitTime(50, true);
            }
            

            // 홈 리턴 ...
            // ----------
            MultiMotion.InitMotionSettingInfo();

            MultiMotion.HomeReturn();
            // ----------


            // 0. 전원 및 원점 체크 ...
            // ----------
            nRet = MultiMotion.RiskCheck();
            if (nRet != KSM_OK)
                return nRet;
            // ----------


            return KSM_OK;
        }

        private static short HomeReturn()
        {
            //return KSM_OK;


            short nRet = -1;


            // 축 초기화 ..
            // --------------------------------------------------            

            MultiMotion.UpdateAxisInfo();

            




            // 카메라 유닛 원점 복귀 ...
            nRet = MultiMotion.HomeMove(MultiMotion.CAM_UNIT_Z, true);
            MultiMotion.ReadyHome[MultiMotion.CAM_UNIT_Z] = true;
            Application.DoEvents();

            

            nRet = MultiMotion.HomeMove(MultiMotion.CAM_UNIT_Y, true);
            MultiMotion.ReadyHome[MultiMotion.CAM_UNIT_Y] = true;
            Application.DoEvents();

            nRet = MultiMotion.HomeMove(MultiMotion.CAM_UNIT_X, true);
            MultiMotion.ReadyHome[MultiMotion.CAM_UNIT_X] = true;
            Application.DoEvents();


            // 후방 카메라 원점 복귀 ...
            MultiMotion.HomeMove(MultiMotion.BACK_CAM_Z, true);
            MultiMotion.ReadyHome[MultiMotion.BACK_CAM_Z] = true;
            Application.DoEvents();


            // INDEX X축 원점 복귀 ...
            nRet = MultiMotion.GantryAxisEnable(1, false);
            if (nRet != KSM_OK)
            {
            }

            nRet = MultiMotion.GantryAxisEnable(1, true);
            if (nRet == KSM_OK)
            {
                nRet = MultiMotion.HomeMove(MultiMotion.INDEX_MOVE_M, true);
                MultiMotion.ReadyHome[MultiMotion.INDEX_MOVE_M] = true;
            }
            Application.DoEvents();


            // V블럭 원점 복귀 ...
            MultiMotion.HomeMove(MultiMotion.VBLOCK_Z, true);
            MultiMotion.ReadyHome[MultiMotion.VBLOCK_Z] = true;
            Application.DoEvents();




            // 롤링 유닛 원점 복귀 ...
            // --------------------------------------------------

            // 3. Software Limit ...
            // ----------

            nRet = NMC2.nmc_SetSWLimitLogic(DeviceManager.g_ndevIdA_4, MultiMotion.ROLLING_FIX_1, 1,
                DataManager.MotionSettingInfoList[MultiMotion.ROLLING_FIX_1].MinValue,
                DataManager.MotionSettingInfoList[MultiMotion.ROLLING_FIX_1].MaxValue);

            nRet = NMC2.nmc_SetSWLimitLogic(DeviceManager.g_ndevIdA_4, MultiMotion.ROLLING_FIX_2, 1,
                DataManager.MotionSettingInfoList[MultiMotion.ROLLING_FIX_2].MinValue,
                DataManager.MotionSettingInfoList[MultiMotion.ROLLING_FIX_2].MaxValue);

            nRet = NMC2.nmc_SetSWLimitLogic(DeviceManager.g_ndevIdA_4, MultiMotion.ROLLING_MOVE_1, 1,
                DataManager.MotionSettingInfoList[MultiMotion.ROLLING_MOVE_1].MinValue,
                DataManager.MotionSettingInfoList[MultiMotion.ROLLING_MOVE_1].MaxValue);

            nRet = NMC2.nmc_SetSWLimitLogic(DeviceManager.g_ndevIdA_4, MultiMotion.ROLLING_MOVE_2, 1,
                DataManager.MotionSettingInfoList[MultiMotion.ROLLING_MOVE_2].MinValue,
                DataManager.MotionSettingInfoList[MultiMotion.ROLLING_MOVE_2].MaxValue);



            MultiMotion.HomeMove(MultiMotion.ROLLING_FIX_1, true);
            MultiMotion.ReadyHome[MultiMotion.ROLLING_FIX_1] = true;
            Application.DoEvents();

            MultiMotion.HomeMove(MultiMotion.ROLLING_FIX_2, true);
            MultiMotion.ReadyHome[MultiMotion.ROLLING_FIX_2] = true;
            Application.DoEvents();

            MultiMotion.HomeMove(MultiMotion.ROLLING_MOVE_1, true);
            MultiMotion.ReadyHome[MultiMotion.ROLLING_MOVE_1] = true;
            Application.DoEvents();

            MultiMotion.HomeMove(MultiMotion.ROLLING_MOVE_2, true);
            MultiMotion.ReadyHome[MultiMotion.ROLLING_MOVE_2] = true;
            Application.DoEvents();

            /*
            if (MultiMotion.GantryAxisEnable(0, false))
            {
                MultiMotion.HomeMove(MultiMotion.INDEX_MOVE_R, true);
                MultiMotion.HomeMove(MultiMotion.INDEX_FIX_R, true);
            }
            */
            // --------------------------------------------------




            return nRet;
        }

        public static short getAxisIndex(short AxisType)
        {
            short AxisIndex = -1;

            for (short i = 0; i < 16; i++)
            {
                if (DataManager.MotionSettingInfoList[i].MotionType == AxisType)
                {
                    AxisIndex = i;
                }
            }

            return AxisIndex;
        }

        public static void UpdateAxisInfo()
        {
            MultiMotion.CAM_UNIT_X      = MultiMotion.getAxisIndex(8);
            MultiMotion.CAM_UNIT_Y      = MultiMotion.getAxisIndex(9);
            MultiMotion.CAM_UNIT_Z      = MultiMotion.getAxisIndex(10);

            MultiMotion.BACK_CAM_Z      = MultiMotion.getAxisIndex(11);
            MultiMotion.VBLOCK_Z        = MultiMotion.getAxisIndex(12);

            MultiMotion.ROLLING_FIX_1   = MultiMotion.getAxisIndex(2);
            MultiMotion.ROLLING_FIX_2   = MultiMotion.getAxisIndex(3);
            MultiMotion.ROLLING_MOVE_1  = MultiMotion.getAxisIndex(4);
            MultiMotion.ROLLING_MOVE_2  = MultiMotion.getAxisIndex(5);

            MultiMotion.INDEX_MOVE_M    = MultiMotion.getAxisIndex(6);
            MultiMotion.INDEX_MOVE_S    = MultiMotion.getAxisIndex(7);

            MultiMotion.INDEX_FIX_R     = MultiMotion.getAxisIndex(0);
            MultiMotion.INDEX_MOVE_R    = MultiMotion.getAxisIndex(1);
        }

        public static short JogStop(short AxisIndex)
        {
            short nRet = -1;


            // 1. 축 및 컨트롤러 Id 자동 지정 ...
            autoSetAxisNumberAndControllerId(AxisIndex, -1);

            if (MultiMotion.CurPaixMotion == null)
                return KSM_NOTCONNECT;


            // 0. 전원 및 원점 체크 ...
            // ----------
            nRet = MultiMotion.RiskCheck();
            if (nRet != KSM_OK)
                return nRet;
            // ----------


            // 2. 조그 동작 ...
            if (MultiMotion.CurPaixMotion.Stop(MultiMotion.AxisNumber) != true)
                return KSM_INVALID;

            return KSM_OK;
        }

        public static short StopAll()
        {
            short nRet = -1;

            for (short i = 0; i < 8; i++)
            {
                nRet = NMC2.nmc_SuddenStop(DeviceManager.g_ndevIdA_4, i);
            }

            for (short k = 0; k < 6; k++)
            {
                nRet = NMC2.nmc_SuddenStop(DeviceManager.g_ndevIdB_4, k);
            }

            return nRet;
        }

        public static short GantryAxisEnable(short nGroup, bool bEnable)
        {
            short nRet = -1;

            bool bOk = false;

            short[] nGantryEnable = new short[2];
            short[] nGantryMain = new short[2];
            short[] nGantrySub = new short[2];

            if (bEnable)
            {
                switch (nGroup)
                {
                    case 0:     // 0번 그룹 인덱스 동시 회전
                        {
                            nRet = NMC2.nmc_SetGantryAxis(DeviceManager.g_ndevIdB_4, 0, 0, 1);
                            if (nRet != KSM_OK)
                                return nRet;

                            nRet = NMC2.nmc_SetGantryEnable(DeviceManager.g_ndevIdB_4, 0, 1);
                            if (nRet != KSM_OK)
                                return nRet;

                            nRet = NMC2.nmc_GetGantryInfo(DeviceManager.g_ndevIdB_4, out nGantryEnable[0], out nGantryMain[0], out nGantrySub[0]);
                            if (nRet != KSM_OK)
                                return nRet;

                            // 6-0, 6-1번 축이 갠트리임.
                            // ----------
                            bOk = (nGantryEnable[0] == 0x01 && nGantryMain[0] == 0x00 && nGantrySub[0] == 0x01);
                            if (bOk == false)
                                return KSM_INVALID;
                            // ----------
                        }
                        break;
                    case 1:     // 1번 그룹 INDEX X축 이동
                        {

                            nRet = NMC2.nmc_SetGantryAxis(DeviceManager.g_ndevIdB_4, 1, 4, 5);
                            if (nRet != KSM_OK)
                                return nRet;

                            nRet = NMC2.nmc_SetGantryEnable(DeviceManager.g_ndevIdB_4, 1, 1);
                            if (nRet != KSM_OK)
                                return nRet;

                            nRet = NMC2.nmc_GetGantryInfo(DeviceManager.g_ndevIdB_4, out nGantryEnable[0], out nGantryMain[0], out nGantrySub[0]);
                            if (nRet != KSM_OK)
                                return nRet;


                            // 6-4, 6-5번 축이 갠트리임.
                            // ----------
                            bOk = (nGantryEnable[1] == 0x01 && nGantryMain[1] == 0x04 && nGantrySub[1] == 0x05);
                            if (bOk == false)
                                return KSM_INVALID;
                            // ----------

                        }
                        break;
                }
            }
            else
            {
                switch (nGroup)
                {
                    case 0: // 0. INDEX는 개별로 동작해야 할 경우도 있음.
                        {
                            nRet = NMC2.nmc_SetGantryAxis(DeviceManager.g_ndevIdB_4, 0, 0, 1);
                            if (nRet != KSM_OK)
                                return nRet;

                            nRet = NMC2.nmc_SetGantryEnable(DeviceManager.g_ndevIdB_4, 0, 0);
                            if (nRet != KSM_OK)
                                return nRet;

                            nRet = NMC2.nmc_GetGantryInfo(DeviceManager.g_ndevIdB_4, out nGantryEnable[0], out nGantryMain[0], out nGantrySub[0]);
                            if (nRet != KSM_OK)
                                return nRet;

                            bOk = (nGantryEnable[0] == 0x00); // 6-0, 6-1번 축이 갠트리임.
                            if (bOk == false)
                                return KSM_INVALID;

                        }
                        break;
                    case 1: // 1. 주행 INDEX X축 이동은 갠트리를 비활성화 할 이유가 없음.                        
                        {
                            nRet = NMC2.nmc_SetGantryAxis(DeviceManager.g_ndevIdB_4, 1, 4, 5);
                            if (nRet != KSM_OK)
                                return nRet;

                            nRet = NMC2.nmc_SetGantryEnable(DeviceManager.g_ndevIdB_4, 1, 0);
                            if (nRet != KSM_OK)
                                return nRet;

                            nRet = NMC2.nmc_GetGantryInfo(DeviceManager.g_ndevIdB_4, out nGantryEnable[0], out nGantryMain[0], out nGantrySub[0]);
                            if (nRet != KSM_OK)
                                return nRet;

                            bOk = (nGantryEnable[1] == 0x00); // 6-4, 6-5번 축이 갠트리임.
                            if (bOk == false)
                                return KSM_INVALID;
                        }
                        break;
                }
            }

            return KSM_OK;
        }

        public static void CalcRollingData()
        {
            double d70Rate = (100.0 - DataManager.SelectedModel.dRolling70Rate) / 100.0;
            double d80Rate = (100.0 - DataManager.SelectedModel.dRolling80Rate) / 100.0;

            MultiMotion.dRolling70Value = (DataManager.SelectedModel.dFLValue - (DataManager.SelectedModel.dSLValue + DataManager.SelectedModel.dWRValue * 2.0)) * d70Rate 
                + (DataManager.SelectedModel.dSLValue + DataManager.SelectedModel.dWRValue * 2.0) + (DataManager.SelectedModel.dMetalThick1 + DataManager.SelectedModel.dMetalThick2 - 4.5);

            MultiMotion.dRolling80Value = (DataManager.SelectedModel.dFLValue - (DataManager.SelectedModel.dSLValue + DataManager.SelectedModel.dWRValue * 2.0)) * d80Rate 
                + (DataManager.SelectedModel.dSLValue + DataManager.SelectedModel.dWRValue * 2.0) + (DataManager.SelectedModel.dMetalThick1 + DataManager.SelectedModel.dMetalThick2 - 4.5);

            MultiMotion.dRolling100Value = DataManager.SelectedModel.dFLValue 
                + (DataManager.SelectedModel.dMetalThick1 + DataManager.SelectedModel.dMetalThick2 - 4.5);


            /*
            MultiMotion.dRolling70Value = (DataManager.SelectedModel.dFLValue - (DataManager.SelectedModel.dSLValue + DataManager.SelectedModel.dWRValue * 2.0)) * 
                DataManager.SelectedModel.dRolling70Rate + 
                (DataManager.SelectedModel.dSLValue + DataManager.SelectedModel.dWRValue * 2.0);

            MultiMotion.dRolling80Value = (DataManager.SelectedModel.dFLValue - (DataManager.SelectedModel.dSLValue + DataManager.SelectedModel.dWRValue * 2.0)) * 
                DataManager.SelectedModel.dRolling80Rate + 
                (DataManager.SelectedModel.dSLValue + DataManager.SelectedModel.dWRValue * 2.0);

            MultiMotion.dRolling100Value = DataManager.SelectedModel.dFLValue + (DataManager.SelectedModel.dMetalThick1 + DataManager.SelectedModel.dMetalThick2 - 4.0);
            // 식 = (dSLValue + dWRValue*2)
            // 70%L = (FL - 식)*0.3 + 식;
            */
        }

        public static short IndexRPosClear()
        {
            short nRet = -1;


            // 1. 축 및 컨트롤러 Id 자동 지정 ...
            autoSetAxisNumberAndControllerId(MultiMotion.INDEX_FIX_R, MultiMotion.INDEX_MOVE_R);

            // 2. 축 설정 확인
            if (MultiMotion.CurPaixMotion == null)
                return KSM_NOTCONNECT;


            // 0. 전원 및 원점 체크 ...
            // ----------
            nRet = MultiMotion.RiskCheck();
            if (nRet != KSM_OK)
                return nRet;
            // ----------


            nRet = NMC2.nmc_SetDccOn(MultiMotion.ControllerId, MultiMotion.AxisNumber, 1);
            if (nRet != KSM_OK)
                return nRet;

            nRet = NMC2.nmc_SetDccOn(MultiMotion.ControllerId, MultiMotion.AxisNumber2, 1);
            if (nRet != KSM_OK)
                return nRet;


            System.Threading.Thread.Sleep(100);


            nRet = NMC2.nmc_SetCmdEncPos(MultiMotion.ControllerId, MultiMotion.AxisNumber, 0.0);
            if (nRet != KSM_OK)
                return nRet;

            nRet = NMC2.nmc_SetCmdEncPos(MultiMotion.ControllerId, MultiMotion.AxisNumber2, 0.0);
            if (nRet != KSM_OK)
                return nRet;


            System.Threading.Thread.Sleep(100);


            nRet = NMC2.nmc_SetDccOn(MultiMotion.ControllerId, MultiMotion.AxisNumber, 0);
            if (nRet != KSM_OK)
                return nRet;

            nRet = NMC2.nmc_SetDccOn(MultiMotion.ControllerId, MultiMotion.AxisNumber2, 0);
            if (nRet != KSM_OK)
                return nRet;

            return KSM_OK;
        }

        public static void GetCurrentPos()
        {
            // 5. 정확한 수치값을 얻기 위해 UnitPerPulse를 지정 ...
            // ----------
            if (MultiMotion.bGetPosReady == false)
            {
                for (short i = 0; i < 8; i++)
                {
                    MultiMotion.PaixMotionA.SetUnitPulse(i, DataManager.MotionSettingInfoList[i].Logic_UnitPerPulse);
                }

                for (short i = 0; i < 6; i++)
                {
                    MultiMotion.PaixMotionB.SetUnitPulse(i, DataManager.MotionSettingInfoList[i + 8].Logic_UnitPerPulse);
                }

                bGetPosReady = true;
            }


            if (PaixMotionA.GetNmcStatus(ref NmcDataA))
            {
                for (int i = 0; i < 8; i++)
                {                    
                    MultiMotion.AxisValue[i]    = NmcDataA.dEnc[i];
                    MultiMotion.AxisCmdValue[i] = NmcDataA.dCmd[i];
                    MultiMotion.AlarmValue[i]   = NmcDataA.nAlarm[i];
                }
            }


            if (PaixMotionB.GetNmcStatus(ref NmcDataB))
            {
                for (int i = 0; i < 6; i++)
                {
                    MultiMotion.AxisValue[i + 8]    = NmcDataB.dEnc[i];
                    MultiMotion.AxisCmdValue[i + 8] = NmcDataB.dCmd[i];
                    MultiMotion.AlarmValue[i + 8]   = NmcDataB.nAlarm[i];
                }
            }
        }

        public static void SetSpeed(int SpeedMode)
        {
            MultiMotion.nSpeedMode = SpeedMode;

            switch (SpeedMode)
            {
                case KSM_SPEED_SSLOW:
                    MultiMotion.dSpeedFactor = 0.3;
                    break;
                case KSM_SPEED_SLOW:
                    MultiMotion.dSpeedFactor = 1.0;
                    break;
                case KSM_SPEED_MIDIUM:
                    MultiMotion.dSpeedFactor = 3.0;
                    break;
                case KSM_SPEED_FAST:
                    MultiMotion.dSpeedFactor = 5.0;
                    break;
            }
        }

        public static bool checkAxisEnd(int AxisIndex)
        {
            short nRet = -1;


            // 1. 축 및 컨트롤러 Id 자동 지정 ...
            autoSetAxisNumberAndControllerId(AxisIndex, -1);


            if (MultiMotion.CurPaixMotion == null)
                return false;


            nRet = NMC2.nmc_GetAxesExpress(MultiMotion.ControllerId, out NmcDataA);
            if (nRet != KSM_OK)
                return false;


            if (NmcDataA.nBusy[MultiMotion.AxisNumber] == 0)
                return true;


            return false;
        }


#endregion Initialize ...


#region 기본 축 이동 명령어 ...


        public static short IndexGantryAxis(int MAxis, int SAxis, double AxisValue, bool bWait)
        {
            short nRet = -1;


            // 0. 전원 및 원점 체크 ...
            // ----------
            nRet = MultiMotion.RiskCheck();
            if (nRet != KSM_OK)
                return nRet;


            // 값 보정
            // ----------
            MultiMotion.GetCurrentPos();

            double dRealValue = MultiMotion.AxisValue[MAxis];
            double dCmdValue = MultiMotion.AxisCmdValue[MAxis];
            double dGapValue = AxisValue - dRealValue;
            double dMoveValue = dCmdValue + dGapValue;
            //double dGapValue = dCmdValue - dRealValue;
            //double dMoveValue = AxisValue - dGapValue;
            // ----------



            // 1. 축 및 컨트롤러 Id 자동 지정 ...
            // ----------
            autoSetAxisNumberAndControllerId(MAxis, SAxis);

            if (MultiMotion.CurPaixMotion == null) {
                return KSM_NOT_EXISTS;
            }


            // 2. UnitPerPulse ...
            // ----------
            MultiMotion.CurPaixMotion.SetUnitPulse(MultiMotion.AxisNumber,
                DataManager.MotionSettingInfoList[MAxis].Logic_UnitPerPulse);      // 메인축

            MultiMotion.CurPaixMotion.SetUnitPulse(MultiMotion.AxisNumber2,
                DataManager.MotionSettingInfoList[SAxis].Logic_UnitPerPulse);      // 서브축


            // 3. Software Limit ...
            // ----------
            // 걸 필요 없다.
            nRet = NMC2.nmc_SetSWLimitLogic(MultiMotion.ControllerId, MultiMotion.AxisNumber, 0,
                DataManager.MotionSettingInfoList[MAxis].MinValue,
                DataManager.MotionSettingInfoList[MAxis].MaxValue);


            // 4. 속도 설정 ...
            // ----------
            // S자 속도 지정
            nRet = NMC2.nmc_SetSCurveSpeed(MultiMotion.ControllerId, MultiMotion.AxisNumber,
                DataManager.MotionSettingInfoList[MAxis].Velocity_Start * MultiMotion.dSpeedFactor,
                DataManager.MotionSettingInfoList[MAxis].Velocity_Acc * MultiMotion.dSpeedFactor,
                DataManager.MotionSettingInfoList[MAxis].Velocity_Dec * MultiMotion.dSpeedFactor,
                DataManager.MotionSettingInfoList[MAxis].Velocity_Max * MultiMotion.dSpeedFactor);

            nRet = NMC2.nmc_SetSCurveSpeed(MultiMotion.ControllerId, MultiMotion.AxisNumber2,
                DataManager.MotionSettingInfoList[MAxis].Velocity_Start * MultiMotion.dSpeedFactor,
                DataManager.MotionSettingInfoList[MAxis].Velocity_Acc * MultiMotion.dSpeedFactor,
                DataManager.MotionSettingInfoList[MAxis].Velocity_Dec * MultiMotion.dSpeedFactor,
                DataManager.MotionSettingInfoList[MAxis].Velocity_Max * MultiMotion.dSpeedFactor);


            // 7. 축 이동 ...
            // ----------

            NMC2.NMCAXESEXPR NmcAxesExpr;

            nRet = NMC2.nmc_GetAxesExpress(MultiMotion.ControllerId, out NmcAxesExpr);

            MultiMotion.CurPaixMotion.AbsMove(MultiMotion.AxisNumber, dMoveValue);




            if (bWait == false)
            {
                return KSM_OK;
            }






            // 메인축 이동 종료 체크
            // --------------------------------------------------
            short nBusyStatus;
            bool bMovDone = false;
            while (!bMovDone)
            {
                nRet = NMC2.nmc_GetBusyStatus(MultiMotion.ControllerId, MultiMotion.AxisNumber, out nBusyStatus);
                if (nRet != KSM_OK) {
                    return nRet;
                }                

                bMovDone = (nBusyStatus == 0);

                System.Threading.Thread.Sleep(100);
                //System.Windows.Forms.Application.DoEvents();
            }


            NMC2.nmc_GetAxesExpress(MultiMotion.ControllerId, out NmcAxesExpr);


            // 8. 갠드리 구동 비활성화
            // --------------------------------------------------
            if (NMC2.nmc_SetGantryEnable(MultiMotion.ControllerId, 0, 0) != 0)
            {
                return KSM_INVALID;
            }
            
            return KSM_OK;
        }

        public static short GantryAxis(int MAxis, int SAxis, double AxisValue, bool bWait)
        {
            short nRet = -1;


            // 1. 축 및 컨트롤러 Id 자동 지정 ...
            // ----------
            autoSetAxisNumberAndControllerId(MAxis, SAxis);

            if (MultiMotion.CurPaixMotion == null)
                return KSM_NOTCONNECT;


            // 0. 전원 및 원점 체크 ...
            // ----------
            nRet = MultiMotion.RiskCheck();
            if (nRet != KSM_OK)
                return nRet;
            // ----------



            // 값 보정
            // ----------
            MultiMotion.GetCurrentPos();

            double dRealValue = MultiMotion.AxisValue[MAxis];
            double dCmdValue = MultiMotion.AxisCmdValue[MAxis];
            double dGapValue = AxisValue - dRealValue;
            double dMoveValue = dCmdValue + dGapValue;

            //double dGapValue = dCmdValue - dRealValue;
            //double dMoveValue = AxisValue - dGapValue;
            // ----------



            // 2. UnitPerPulse ...
            // ----------
            MultiMotion.CurPaixMotion.SetUnitPulse(MultiMotion.AxisNumber,
                DataManager.MotionSettingInfoList[MAxis].Logic_UnitPerPulse);      // 메인축

            MultiMotion.CurPaixMotion.SetUnitPulse(MultiMotion.AxisNumber2,
                DataManager.MotionSettingInfoList[SAxis].Logic_UnitPerPulse);      // 서브축


            // 3. Software Limit ...
            // ----------
            // 걸 필요 없다.


            // S자 속도 지정
            nRet = NMC2.nmc_SetSCurveSpeed(MultiMotion.ControllerId, MultiMotion.AxisNumber,
                DataManager.MotionSettingInfoList[MAxis].Velocity_Start * MultiMotion.dSpeedFactor,
                DataManager.MotionSettingInfoList[MAxis].Velocity_Acc * MultiMotion.dSpeedFactor,
                DataManager.MotionSettingInfoList[MAxis].Velocity_Dec * MultiMotion.dSpeedFactor,
                DataManager.MotionSettingInfoList[MAxis].Velocity_Max * MultiMotion.dSpeedFactor);


            // 5. 갠드리 구동 설정 및 활성화 ...
            // --------------------------------------------------
            if (MultiMotion.GantryAxisEnable(1, true) != KSM_OK)
            {
                return KSM_INVALID;
            }


            // 7. 축 이동 ...
            // ----------
            NMC2.NMCAXESEXPR NmcAxesExpr;

            NMC2.nmc_GetAxesExpress(MultiMotion.ControllerId, out NmcAxesExpr);

            // 메인축 위치 0, 서브 위치 0
            // 메인축 => 300위치로 상대이동
            // 메인축과 서브축 동시에 구동을 합니다.
            //nRet = NMC2


            if (MultiMotion.CurPaixMotion.AbsMove(MultiMotion.AxisNumber, dMoveValue) != true)
            {
                return KSM_INVALID;
            }


            if (bWait == false)
            {
                return KSM_OK;
            }


            // 메인축 이동 종료 체크
            // --------------------------------------------------
            short nBusyStatus;
            bool bMovDone = false;
            while (!bMovDone)
            {
                nRet = NMC2.nmc_GetBusyStatus(MultiMotion.ControllerId, MultiMotion.AxisNumber, out nBusyStatus);

                bMovDone = (nBusyStatus == 0);

                System.Threading.Thread.Sleep(100);
            }

            return KSM_OK;
        }

        public static short JogMove(short AxisIndex, short nDir)
        {
            short nRet = -1;

            // 0. 전원 및 원점 체크 ...
            // ----------
            nRet = MultiMotion.RiskCheck();
            if (nRet != KSM_OK)
                return nRet;
            // ----------


            // 1. 축 및 컨트롤러 Id 자동 지정 ...
            // --------------------------------------------------
            autoSetAxisNumberAndControllerId(AxisIndex, -1);

            if (MultiMotion.CurPaixMotion == null)
                return KSM_NOTCONNECT;

            MultiMotion.CurPaixMotion.SetUnitPulse(MultiMotion.AxisNumber, DataManager.MotionSettingInfoList[AxisIndex].Logic_UnitPerPulse);

            // 3. Software Limit ...
            // ----------
            nRet = NMC2.nmc_SetSWLimitLogic(MultiMotion.ControllerId, MultiMotion.AxisNumber, 1,
                DataManager.MotionSettingInfoList[AxisIndex].MinValue,
                DataManager.MotionSettingInfoList[AxisIndex].MaxValue);


            // S자 속도 지정
            /*
            nRet = NMC2.nmc_SetSCurveSpeed(MultiMotion.ControllerId, MultiMotion.AxisNumber,
                DataManager.MotionSettingInfoList[AxisIndex].Velocity_Start * MultiMotion.dSpeedFactor,
                DataManager.MotionSettingInfoList[AxisIndex].Velocity_Acc * MultiMotion.dSpeedFactor,
                DataManager.MotionSettingInfoList[AxisIndex].Velocity_Dec * MultiMotion.dSpeedFactor,
                DataManager.MotionSettingInfoList[AxisIndex].Velocity_Max * MultiMotion.dSpeedFactor);
            */

            
            // 사다리꼴 속도 지정
            nRet = NMC2.nmc_SetSpeed(MultiMotion.ControllerId, MultiMotion.AxisNumber,
                DataManager.MotionSettingInfoList[AxisIndex].Velocity_Start * MultiMotion.dSpeedFactor,
                DataManager.MotionSettingInfoList[AxisIndex].Velocity_Acc * MultiMotion.dSpeedFactor,
                DataManager.MotionSettingInfoList[AxisIndex].Velocity_Dec * MultiMotion.dSpeedFactor,
                DataManager.MotionSettingInfoList[AxisIndex].Velocity_Max * MultiMotion.dSpeedFactor);
            

            // 3. 조그 동작 ...
            // --------------------------------------------------
            if (MultiMotion.CurPaixMotion.JogMove(MultiMotion.AxisNumber, nDir) != true)
            {
                return KSM_INVALID;
            }            

            return KSM_OK;
        }

        public static short StepMove(short AxisIndex, short nDir, bool bWait)
        {
            short nRet = -1;


            // 0. 전원 및 원점 체크 ...
            // ----------
            nRet = MultiMotion.RiskCheck();
            if (nRet != KSM_OK)
                return nRet;



            // 값 보정
            // ----------
            
            MultiMotion.GetCurrentPos();


            double dRealValue = MultiMotion.AxisValue[AxisIndex];
            double dMoveValue = 0.0;
            double dGapValue = 0.0;

            switch (MultiMotion.nSpeedMode)
            {
                case KSM_SPEED_SSLOW:
                    dGapValue = 0.3;
                    break;
                case KSM_SPEED_SLOW:
                    dGapValue = 1.0;
                    break;
                case KSM_SPEED_MIDIUM:
                    dGapValue = 3.0;
                    break;
                case KSM_SPEED_FAST:
                    dGapValue = 5.0;
                    break;
            }


            if (nDir == 1)
                dMoveValue = dRealValue - dGapValue;
            else
                dMoveValue = dRealValue + dGapValue;
            // ----------




            // 1. 축 및 컨트롤러 Id 자동 지정 ...
            // ----------
            autoSetAxisNumberAndControllerId(AxisIndex, -1);

            if (MultiMotion.CurPaixMotion == null)
                return KSM_NOTCONNECT;

            MultiMotion.CurPaixMotion.SetUnitPulse(MultiMotion.AxisNumber, DataManager.MotionSettingInfoList[AxisIndex].Logic_UnitPerPulse);


            // 3. Software Limit ...
            // ----------
            nRet = NMC2.nmc_SetSWLimitLogic(MultiMotion.ControllerId, MultiMotion.AxisNumber, 1,
                DataManager.MotionSettingInfoList[AxisIndex].MinValue,
                DataManager.MotionSettingInfoList[AxisIndex].MaxValue);


            // S자 속도 지정
            nRet = NMC2.nmc_SetSCurveSpeed(MultiMotion.ControllerId, MultiMotion.AxisNumber,
                DataManager.MotionSettingInfoList[AxisIndex].Velocity_Start * MultiMotion.dSpeedFactor,
                DataManager.MotionSettingInfoList[AxisIndex].Velocity_Acc * MultiMotion.dSpeedFactor,
                DataManager.MotionSettingInfoList[AxisIndex].Velocity_Dec * MultiMotion.dSpeedFactor,
                DataManager.MotionSettingInfoList[AxisIndex].Velocity_Max * MultiMotion.dSpeedFactor);

            if (MultiMotion.CurPaixMotion.AbsMove(MultiMotion.AxisNumber, dMoveValue) == false)
            {
                return KSM_INVALID;
            }


            if (bWait == false)
            {
                return KSM_OK;
            }


            while (true)
            {
                nRet = NMC2.nmc_GetAxesExpress(MultiMotion.ControllerId, out NmcDataA);
                if (nRet != KSM_OK)
                    return nRet;

                if (NmcDataA.nBusy[MultiMotion.AxisNumber] == 0) // if (NmcDataA.nBusy[0] == 0) => 원래 코드
                {
                    break;
                }

                System.Threading.Thread.Sleep(100);
            }


            return KSM_OK;
        }

        public static short MoveAxis(int AxisIndex, double AxisValue, bool bWait)
        {
            short nRet = -1;


            // 0. 전원 및 원점 체크 ...
            // ----------
            nRet = MultiMotion.RiskCheck();
            if (nRet != KSM_OK)
                return nRet;
            // ----------


            // 값 보정
            // ----------
            MultiMotion.GetCurrentPos();

            double dRealValue = MultiMotion.AxisValue[AxisIndex];
            double dCmdValue = MultiMotion.AxisCmdValue[AxisIndex];
            double dGapValue = AxisValue - dRealValue;
            double dMoveValue = dCmdValue + dGapValue;
            //double dGapValue = dCmdValue - dRealValue;
            //double dMoveValue = AxisValue - dGapValue;
            // ----------


            // 1. 축 및 컨트롤러 Id 자동 지정 ...
            autoSetAxisNumberAndControllerId(AxisIndex, -1);


            if (MultiMotion.CurPaixMotion == null)
                return KSM_NOTCONNECT;


            MultiMotion.CurPaixMotion.SetUnitPulse(MultiMotion.AxisNumber, DataManager.MotionSettingInfoList[AxisIndex].Logic_UnitPerPulse);


            // 3. Software Limit ...
            // ----------
            nRet = NMC2.nmc_SetSWLimitLogic(MultiMotion.ControllerId, MultiMotion.AxisNumber, 1,
                DataManager.MotionSettingInfoList[AxisIndex].MinValue,
                DataManager.MotionSettingInfoList[AxisIndex].MaxValue);

            if (nRet != KSM_OK)
                return nRet;

            // S자 속도 지정
            nRet = NMC2.nmc_SetSCurveSpeed(MultiMotion.ControllerId, MultiMotion.AxisNumber,
                DataManager.MotionSettingInfoList[AxisIndex].Velocity_Start * MultiMotion.dSpeedFactor,
                DataManager.MotionSettingInfoList[AxisIndex].Velocity_Acc * MultiMotion.dSpeedFactor,
                DataManager.MotionSettingInfoList[AxisIndex].Velocity_Dec * MultiMotion.dSpeedFactor,
                DataManager.MotionSettingInfoList[AxisIndex].Velocity_Max * MultiMotion.dSpeedFactor);


            if (MultiMotion.CurPaixMotion.AbsMove(MultiMotion.AxisNumber, dMoveValue) == false)
            {
                return KSM_INVALID;
            }
            //MultiMotion.CurPaixMotion.RelMove(MultiMotion.AxisNumber, AxisValue);
            // --------------------------------------------------


            if (bWait == false)
            {
                // 사용자 조작에 의한 입력일 경우 비상 정지 할 수 있어야 함.

                return KSM_OK;
            }


            while (true)
            {
                nRet = NMC2.nmc_GetAxesExpress(MultiMotion.ControllerId, out NmcDataA);
                if (nRet != KSM_OK)
                    return nRet;

                if (NmcDataA.nBusy[MultiMotion.AxisNumber] == 0) // if (NmcDataA.nBusy[0] == 0) => 원래 코드
                {
                    break;
                }

                System.Threading.Thread.Sleep(100);
                //System.Windows.Forms.Application.DoEvents();
            }


            return KSM_OK;
        }

        public static short HomeMove(int AxisIndex, bool bWait)
        {
            short nRet = -1;

            NMC2.NMCHOMEFLAG HomeFlag;


            // 1. 축 및 컨트롤러 Id 자동 지정 ...
            autoSetAxisNumberAndControllerId(AxisIndex, -1);

            // 2. 축 설정 확인
            if (MultiMotion.CurPaixMotion == null)
                return KSM_NOTCONNECT;


            // 0. 전원 및 원점 체크 ...
            // ----------
            /*
            nRet = MultiMotion.RiskCheck();
            if (nRet != KSM_OK)
                return nRet;
            */
            // ----------

            
            /*
            // 2. 이미 원점 위치에 있다면, true를 return하고 끝낸다.
            NMC2.nmc_GetHomeStatus(MultiMotion.ControllerId, out HomeFlag);
            //if (HomeFlag.nSrchFlag[0] == 0 && HomeFlag.nStatusFlag[0] == 0) // => 0이 인덱스 인가
            if (HomeFlag.nSrchFlag[MultiMotion.AxisNumber] == 0 && HomeFlag.nStatusFlag[MultiMotion.AxisNumber] == 0)
            {
                return true;
            }
            */


            MultiMotion.CurPaixMotion.SetUnitPulse(MultiMotion.AxisNumber, DataManager.MotionSettingInfoList[AxisIndex].Logic_UnitPerPulse);


            //bool FixHomeVelocity = false;
            bool FixHomeVelocity = true;

            if (FixHomeVelocity == true)
            {
                // S자 속도 지정(원점이동의 가감속만 지정됨)
                nRet = NMC2.nmc_SetSCurveSpeed(MultiMotion.ControllerId, MultiMotion.AxisNumber,
                    MultiMotion.MotionSettingInfoList[AxisIndex].Velocity_Start,
                    MultiMotion.MotionSettingInfoList[AxisIndex].Velocity_Acc,
                    MultiMotion.MotionSettingInfoList[AxisIndex].Velocity_Dec,
                    MultiMotion.MotionSettingInfoList[AxisIndex].Velocity_Max);


                // 가속 : 1000이 지정됨(원점이동의 가속도로 지정됨)
                nRet = NMC2.nmc_SetAccSpeed(MultiMotion.ControllerId, MultiMotion.AxisNumber,
                    MultiMotion.MotionSettingInfoList[AxisIndex].Velocity_Acc);

                // 감속 : 1000이 지정됨(원점이동의 감속도로 지정됨)
                nRet = NMC2.nmc_SetDecSpeed(MultiMotion.ControllerId, MultiMotion.AxisNumber,
                    MultiMotion.MotionSettingInfoList[AxisIndex].Velocity_Dec);

            }
            else
            {
                // S자 속도 지정(원점이동의 가감속만 지정됨)
                nRet = NMC2.nmc_SetSCurveSpeed(MultiMotion.ControllerId, MultiMotion.AxisNumber,
                    DataManager.MotionSettingInfoList[AxisIndex].Velocity_Start,
                    DataManager.MotionSettingInfoList[AxisIndex].Velocity_Acc,
                    DataManager.MotionSettingInfoList[AxisIndex].Velocity_Dec,
                    DataManager.MotionSettingInfoList[AxisIndex].Velocity_Max);


                // 가속 : 1000이 지정됨(원점이동의 가속도로 지정됨)
                nRet = NMC2.nmc_SetAccSpeed(MultiMotion.ControllerId, MultiMotion.AxisNumber,
                    DataManager.MotionSettingInfoList[AxisIndex].Velocity_Acc);

                // 감속 : 1000이 지정됨(원점이동의 감속도로 지정됨)
                nRet = NMC2.nmc_SetDecSpeed(MultiMotion.ControllerId, MultiMotion.AxisNumber,
                    DataManager.MotionSettingInfoList[AxisIndex].Velocity_Dec);
            }


            // 0번 축의 1차 속도 2000, 2차 1500, 3차 1000으로 설정
            //nRet = NMC2.nmc_SetHomeSpeed(MultiMotion.ControllerId, MultiMotion.AxisNumber, 3, 2, 1);
            nRet = NMC2.nmc_SetHomeSpeedEx(MultiMotion.ControllerId, MultiMotion.AxisNumber,
                DataManager.MotionSettingInfoList[AxisIndex].Velocity_Home_1,
                DataManager.MotionSettingInfoList[AxisIndex].Velocity_Home_2,
                DataManager.MotionSettingInfoList[AxisIndex].Velocity_Home_3,
                DataManager.MotionSettingInfoList[AxisIndex].Velocity_Home_Offset); // 원점 Offset 속도까지 지정하려면 이 함수를 사용해야
            //nRet = NMC2.nmc_SetHomeSpeed(MultiMotion.ControllerId, MultiMotion.AxisNumber, 10, 5, 2);


            // ==================================================
            // 1. 0번 축의 원점 이동 모드 : +Near
            // 2. 원점 검색후(3차이동 까지 완료) CW회전으로 Z상 검출
            // 3. 지령/엔코더 위치 0으로 초기화
            // 4. Offset 300 위치로 이동
            // 5. Offset 이동 후 지령/엔코더 위치 0으로 초기화
            //nRet = NMC2.nmc_HomeMove(MultiMotion.ControllerId, MultiMotion.AxisNumber, 0x80 | 3, 0x0f, 1, 0); // 0x0f는 원점 찾은 후 0으로 무조건, 그 뒤는 원점 Offset 이동 거리


            // 원점 이동
            // ----------
            nRet = NMC2.nmc_HomeMove(MultiMotion.ControllerId,
                MultiMotion.AxisNumber,
                0x80 | 2,
                0,
                DataManager.MotionSettingInfoList[AxisIndex].Home_Offset,
                0); // 한태희 과정 => 0x80 | 2


            if (nRet != KSM_OK) {
                return nRet;
            }                


            if (bWait == false)
            {
                return KSM_OK;    // 사용자 조작에 의한 입력일 경우 비상 정지 할 수 있어야 함.
            }


            // 원점이동이 완료 될때 까지 대기
            // ----------
            while (true)
            {
                nRet = NMC2.nmc_GetHomeStatus(MultiMotion.ControllerId, out HomeFlag);
                if (nRet != KSM_OK)
                {
                    //return nRet;
                }

                if (HomeFlag.nSrchFlag[MultiMotion.AxisNumber] == 0 && HomeFlag.nStatusFlag[MultiMotion.AxisNumber] == 0)
                {
                    // 위치 초기화
                    // --------------------------------------------------
                    nRet = NMC2.nmc_SetDccOn(MultiMotion.ControllerId, MultiMotion.AxisNumber, 1);

                    System.Threading.Thread.Sleep(100);

                    nRet = NMC2.nmc_SetCmdEncPos(MultiMotion.ControllerId, MultiMotion.AxisNumber, 0.0);

                    System.Threading.Thread.Sleep(100);

                    nRet = NMC2.nmc_SetDccOn(MultiMotion.ControllerId, MultiMotion.AxisNumber, 0);
                    // --------------------------------------------------

                    break;
                }

                //System.Windows.Forms.Application.DoEvents();
            }

            return KSM_OK;
        }

        public static short MoveRolling(double AxisValue, bool bWait)
        {
            short nRet = -1;


            // 1. 축 및 컨트롤러 Id 자동 지정 ...
            // --------------------------------------------------
            autoSetAxisNumberAndControllerId(MultiMotion.ROLLING_FIX_1, -1);

            if (MultiMotion.CurPaixMotion == null)
                return KSM_NOTCONNECT;


            // 0. 전원 및 원점 체크 ...
            // ----------
            nRet = MultiMotion.RiskCheck();
            if (nRet != KSM_OK)
                return nRet;
            // ----------



            // 2. UnitPerPulse ...
            // ----------
            MultiMotion.PaixMotionA.SetUnitPulse(MultiMotion.ROLLING_FIX_1,
                DataManager.MotionSettingInfoList[MultiMotion.ROLLING_FIX_1].Logic_UnitPerPulse);

            MultiMotion.PaixMotionA.SetUnitPulse(MultiMotion.ROLLING_FIX_2,
                DataManager.MotionSettingInfoList[MultiMotion.ROLLING_FIX_2].Logic_UnitPerPulse);

            MultiMotion.PaixMotionA.SetUnitPulse(MultiMotion.ROLLING_MOVE_1,
                DataManager.MotionSettingInfoList[MultiMotion.ROLLING_MOVE_1].Logic_UnitPerPulse);

            MultiMotion.PaixMotionA.SetUnitPulse(MultiMotion.ROLLING_MOVE_2,
                DataManager.MotionSettingInfoList[MultiMotion.ROLLING_MOVE_2].Logic_UnitPerPulse);


            // 3. Software Limit ...
            // ----------
            nRet = NMC2.nmc_SetSWLimitLogic(MultiMotion.ControllerId, MultiMotion.ROLLING_FIX_1, 1,
                DataManager.MotionSettingInfoList[MultiMotion.ROLLING_FIX_1].MinValue,
                DataManager.MotionSettingInfoList[MultiMotion.ROLLING_FIX_1].MaxValue);

            nRet = NMC2.nmc_SetSWLimitLogic(MultiMotion.ControllerId, MultiMotion.ROLLING_FIX_2, 1,
                DataManager.MotionSettingInfoList[MultiMotion.ROLLING_FIX_2].MinValue,
                DataManager.MotionSettingInfoList[MultiMotion.ROLLING_FIX_2].MaxValue);

            nRet = NMC2.nmc_SetSWLimitLogic(MultiMotion.ControllerId, MultiMotion.ROLLING_MOVE_1, 1,
                DataManager.MotionSettingInfoList[MultiMotion.ROLLING_MOVE_1].MinValue,
                DataManager.MotionSettingInfoList[MultiMotion.ROLLING_MOVE_1].MaxValue);

            nRet = NMC2.nmc_SetSWLimitLogic(MultiMotion.ControllerId, MultiMotion.ROLLING_MOVE_2, 1,
                DataManager.MotionSettingInfoList[MultiMotion.ROLLING_MOVE_2].MinValue,
                DataManager.MotionSettingInfoList[MultiMotion.ROLLING_MOVE_2].MaxValue);


            // 4. 속도 설정 ...
            // ----------
            nRet = NMC2.nmc_SetSCurveSpeed(MultiMotion.ControllerId, MultiMotion.ROLLING_FIX_1,
                DataManager.MotionSettingInfoList[MultiMotion.ROLLING_FIX_1].Velocity_Start * MultiMotion.dSpeedFactor,
                DataManager.MotionSettingInfoList[MultiMotion.ROLLING_FIX_1].Velocity_Acc * MultiMotion.dSpeedFactor,
                DataManager.MotionSettingInfoList[MultiMotion.ROLLING_FIX_1].Velocity_Dec * MultiMotion.dSpeedFactor,
                DataManager.MotionSettingInfoList[MultiMotion.ROLLING_FIX_1].Velocity_Max * MultiMotion.dSpeedFactor);

            nRet = NMC2.nmc_SetSCurveSpeed(MultiMotion.ControllerId, MultiMotion.ROLLING_FIX_2,
                DataManager.MotionSettingInfoList[MultiMotion.ROLLING_FIX_2].Velocity_Start * MultiMotion.dSpeedFactor,
                DataManager.MotionSettingInfoList[MultiMotion.ROLLING_FIX_2].Velocity_Acc * MultiMotion.dSpeedFactor,
                DataManager.MotionSettingInfoList[MultiMotion.ROLLING_FIX_2].Velocity_Dec * MultiMotion.dSpeedFactor,
                DataManager.MotionSettingInfoList[MultiMotion.ROLLING_FIX_2].Velocity_Max * MultiMotion.dSpeedFactor);

            nRet = NMC2.nmc_SetSCurveSpeed(MultiMotion.ControllerId, MultiMotion.ROLLING_MOVE_1,
                DataManager.MotionSettingInfoList[MultiMotion.ROLLING_MOVE_1].Velocity_Start * MultiMotion.dSpeedFactor,
                DataManager.MotionSettingInfoList[MultiMotion.ROLLING_MOVE_1].Velocity_Acc * MultiMotion.dSpeedFactor,
                DataManager.MotionSettingInfoList[MultiMotion.ROLLING_MOVE_1].Velocity_Dec * MultiMotion.dSpeedFactor,
                DataManager.MotionSettingInfoList[MultiMotion.ROLLING_MOVE_1].Velocity_Max * MultiMotion.dSpeedFactor);

            nRet = NMC2.nmc_SetSCurveSpeed(MultiMotion.ControllerId, MultiMotion.ROLLING_MOVE_2,
                DataManager.MotionSettingInfoList[MultiMotion.ROLLING_MOVE_2].Velocity_Start * MultiMotion.dSpeedFactor,
                DataManager.MotionSettingInfoList[MultiMotion.ROLLING_MOVE_2].Velocity_Acc * MultiMotion.dSpeedFactor,
                DataManager.MotionSettingInfoList[MultiMotion.ROLLING_MOVE_2].Velocity_Dec * MultiMotion.dSpeedFactor,
                DataManager.MotionSettingInfoList[MultiMotion.ROLLING_MOVE_2].Velocity_Max * MultiMotion.dSpeedFactor);





            // 5. 이동 ...
            // ----------

            // 값 보정
            // --------------------------------------------------
            MultiMotion.GetCurrentPos();

            // 고정축 A
            // ----------
            double dRealValue = MultiMotion.AxisValue[MultiMotion.ROLLING_FIX_1];
            double dCmdValue = MultiMotion.AxisCmdValue[MultiMotion.ROLLING_FIX_1];
            double dGapValue = dCmdValue - dRealValue;
            double dMoveValue = AxisValue - dGapValue;

            MultiMotion.PaixMotionA.AbsMove(MultiMotion.ROLLING_FIX_1, dMoveValue);


            // 고정축 B
            // ----------
            dRealValue = MultiMotion.AxisValue[MultiMotion.ROLLING_FIX_2];
            dCmdValue = MultiMotion.AxisCmdValue[MultiMotion.ROLLING_FIX_2];
            dGapValue = dCmdValue - dRealValue;
            dMoveValue = AxisValue - dGapValue;

            MultiMotion.PaixMotionA.AbsMove(MultiMotion.ROLLING_FIX_2, dMoveValue);


            // 이동축 A
            // ----------
            dRealValue = MultiMotion.AxisValue[MultiMotion.ROLLING_MOVE_1];
            dCmdValue = MultiMotion.AxisCmdValue[MultiMotion.ROLLING_MOVE_1];
            dGapValue = dCmdValue - dRealValue;
            dMoveValue = AxisValue - dGapValue;

            MultiMotion.PaixMotionA.AbsMove(MultiMotion.ROLLING_MOVE_1, dMoveValue);


            // 이동축 B
            // ----------
            dRealValue = MultiMotion.AxisValue[MultiMotion.ROLLING_MOVE_2];
            dCmdValue = MultiMotion.AxisCmdValue[MultiMotion.ROLLING_MOVE_2];
            dGapValue = dCmdValue - dRealValue;
            dMoveValue = AxisValue - dGapValue;

            MultiMotion.PaixMotionA.AbsMove(MultiMotion.ROLLING_MOVE_2, dMoveValue);


            if (bWait == false)
                return KSM_OK;


            // 6. 동작 완료 확인 ... => 마지막 대상만 확인
            // --------------------------------------------------
            while (true)
            {
                nRet = NMC2.nmc_GetAxesExpress(MultiMotion.ControllerId, out NmcDataA);

                //if (NmcDataA.nBusy[0] == 0) => 원래 코드
                if (NmcDataA.nBusy[MultiMotion.ROLLING_MOVE_2] == 0)
                {
                    break;
                }

                //System.Threading.Thread.Sleep(100);
                //System.Windows.Forms.Application.DoEvents();
            }

            return KSM_OK;
        }


#endregion 기본 축 이동 명령어 ...


#region 접점 제어 ...


        public static short GetDIOStatus()
        {
            short nRet = NMC2.nmc_GetDIOInput(DeviceManager.g_ndevIdC_4, out MultiMotion.InStatus[0]);
            if (nRet != 0)
            {
                return nRet;
            }

            nRet = NMC2.nmc_GetDIOOutput(DeviceManager.g_ndevIdC_4, out MultiMotion.OutStatus[0]);
            if (nRet != 0)
            {
                return nRet;
            }

            return nRet;
        }

        public static short MainLightOnOff(short OnOff)
        {
            short nRet = KSM_OK;

            nRet = NMC2.nmc_SetDIOOutputBit(DeviceManager.g_ndevIdC_4, 31, OnOff);
            if (nRet != KSM_OK)
                return nRet;

            return nRet;
        }

        public static short Swing(bool bSwing)
        {
            short nRet = KSM_OK;

            if (bSwing)
            {
                nRet = NMC2.nmc_SetDIOOutputBit(DeviceManager.g_ndevIdC_4, 11, 1);
                if (nRet != KSM_OK)
                    return nRet;

                nRet = NMC2.nmc_SetDIOOutputBit(DeviceManager.g_ndevIdC_4, 21, 1);
                if (nRet != KSM_OK)
                    return nRet;

                nRet = NMC2.nmc_SetDIOOutputBit(DeviceManager.g_ndevIdC_4, 12, 0);
                if (nRet != KSM_OK)
                    return nRet;

                nRet = NMC2.nmc_SetDIOOutputBit(DeviceManager.g_ndevIdC_4, 22, 0);
                if (nRet != KSM_OK)
                    return nRet;
            }
            else
            {
                nRet = NMC2.nmc_SetDIOOutputBit(DeviceManager.g_ndevIdC_4, 11, 0);
                if (nRet != KSM_OK)
                    return nRet;

                nRet = NMC2.nmc_SetDIOOutputBit(DeviceManager.g_ndevIdC_4, 21, 0);
                if (nRet != KSM_OK)
                    return nRet;

                nRet = NMC2.nmc_SetDIOOutputBit(DeviceManager.g_ndevIdC_4, 12, 1);
                if (nRet != KSM_OK)
                    return nRet;

                nRet = NMC2.nmc_SetDIOOutputBit(DeviceManager.g_ndevIdC_4, 22, 1);
                if (nRet != KSM_OK)
                    return nRet;
            }

            return nRet;
        }


#endregion 접점 제어 ...


#region 방어 코드 ...


        public static short CheckDefense()
        {
            

            short nRet = KSM_OK;
                
            nRet = GetDIOStatus();
            if (nRet != MultiMotion.KSM_OK)
            {
                return nRet;
            }


            MultiMotion.GetCurrentPos();

            
            // --------------------------------------------------
            // 비상 정지 버튼 
            if (MultiMotion.InStatus[45] == 0)
            {
                MultiMotion.StopAll();                

                Application.Exit();                

                return KSM_INVALID;
            }

            // 노즐 컨텍 인터락
            if (MultiMotion.InStatus[46] == 1)
            {
                MultiMotion.StopAll();

                return KSM_INVALID;
            }
            // --------------------------------------------------



            // 운전 준비
            if (MultiMotion.InStatus[41] == 1)
            {
                //MultiMotion.JogStop(MultiMotion.CAM_UNIT_Z);

                //return KSM_INVALID;
            }

            // 재가동
            if (MultiMotion.InStatus[42] == 1)
            {
                //MultiMotion.JogStop(MultiMotion.CAM_UNIT_Z);

                //return KSM_INVALID;

                MultiMotion.bRestart = true;
            }


            // 운전 정지
            if (MultiMotion.InStatus[43] == 1)
            {
                MultiMotion.StopAll();

                MultiMotion.bEAutoStop = true;

                return KSM_INVALID;
            }

            // 비상 해제
            if (MultiMotion.InStatus[44] == 1)
            {
                MultiMotion.AlarmReset();

                return KSM_INVALID;
            }


            
           
            
            // 31 : 비전 충동 감지 하(B접점) => 센서 인식 범위가 있는 것 같다. => 내려 갈 수 있는데도 신호가 온다.
            if (MultiMotion.InStatus[31] == 0)
            {
                //MultiMotion.StopAll();

                //return KSM_INVALID;
            }


            // 32 : 비전 충동 감지 우(B접점)
            if (MultiMotion.InStatus[32] == 0)
            {
                //MultiMotion.StopAll();

                //return KSM_INVALID;
            }

            // 33 : 비전 충동 감지 좌(B접점)
            if (MultiMotion.InStatus[33] == 0)
            {
                double dRealValue = MultiMotion.AxisValue[MultiMotion.CAM_UNIT_X];

                if (dRealValue > 300.0)
                {
                    //MultiMotion.StopAll();

                    //return KSM_INVALID;
                }                
            }
            
            // 34 : 레이저 충동 감지 (B접점)
            if (MultiMotion.InStatus[34] == 0)
            {
                //MultiMotion.StopAll();                

                //return KSM_INVALID;
            }
            

            /*
            // INDEX, CAM 유닛 거리값에 의한 정지 조건 검증 ...
            nRet = checkDefense_CamUnit();
            if (nRet != KSM_OK)
            {
                //MultiMotion.StopAll();
                MultiMotion.JogStop(MultiMotion.CAM_UNIT_Y);
                MultiMotion.JogStop(MultiMotion.CAM_UNIT_Z);

                return nRet;
            }
            */

            // 41 : 운전준비
            // 42 : 재가동
            // 43 : 운전 정지
            // 44 : 비상 해제
            // 45 : 비상 정지 버튼(B접점)


            
            // --------------------------------------------------
            if (MultiMotion.AxisValue[MultiMotion.CAM_UNIT_Z] > 30.0)
            {
                if (MultiMotion.AxisValue[MultiMotion.CAM_UNIT_X] < 10.0 && MultiMotion.AxisValue[MultiMotion.INDEX_MOVE_M] < 800.0)
                {
                    MultiMotion.StopAll();

                    //MultiMotion.JogStop();

                    return KSM_INVALID;
                }

                if (MultiMotion.AxisValue[MultiMotion.CAM_UNIT_X] > 10.0 && MultiMotion.AxisValue[MultiMotion.INDEX_MOVE_M] > 10.0)
                {
                    MultiMotion.StopAll();

                    return KSM_INVALID;
                }
            }




            /*
            if (MultiMotion.AxisValue[MultiMotion.INDEX_MOVE_M] > 15.0 && MultiMotion.AxisValue[MultiMotion.CAM_UNIT_Z] > 15.0)
            {
                MultiMotion.StopAll();

                return KSM_INVALID;
            }
            */

            return nRet;

            // --------------------------------------------------
            // 41 : 운전준비
            // 42 : 재가동
            // 43 : 운전 정지
            // 44 : 비상 해제
            // 45 : 비상 정지 버튼(B접점)

            // ----------
            // 11 : 좌감지
            // 12 : 우감지
            // 21 : 좌감지
            // 22 : 우감지

            // ----------
            // 13 : 스톱커 상
            // 14 : 스톱커 하

            // ----------
            // 31 : 비전 충동 감지 하(B접점)
            // 32 : 비전 충동 감지 우(B접점)
            // 33 : 비전 충동 감지 좌(B접점)

            // 34 : 레이저 충동 감지 하(B접점)

            // 10 : 공기압 이상(B접점)
            // --------------------------------------------------

            return nRet;
        }

        public static short checkDefense_CamUnit()
        {
            // --------------------------------------------------
            // 1. 비전 X축 800mmm 이상 간 경우에만 Z축을 내릴 수 있다.
            // 2. 비전 X축 1300mmm 이상 간 경우에는 Z축을 내릴 수 없다.
            // 3. 비전 X축이 1300mm 간 상태에서 500 + a 정도의 거리차이가 INDEX 고정축과 있다.


            // 2. 카메라 유닛 현재값 - 700 + 630(가능 이동 거리) - 주행축 Index X 값 > 0보다 커야 함. => 1호기 조건
            double SumValue = 0.0;
            bool XDistance = (MultiMotion.AxisValue[MultiMotion.CAM_UNIT_X] > 800.0 && MultiMotion.AxisValue[MultiMotion.CAM_UNIT_X] < 1300.0);

            if (XDistance == false)
            {
                return KSM_INVALID;
            }

            /*
            else
            {
                SumValue = 900.0 - MultiMotion.AxisValue[MultiMotion.CAM_UNIT_X] + 450.0 - MultiMotion.AxisValue[MultiMotion.INDEX_MOVE_M];
            }

            // 2. 위 조건에서 최대 내릴 수 있는 카메라 유닛 Z값은 200mm
            double CAM_Z_Limit = 200.0;

            if (MultiMotion.AxisValue[MultiMotion.CAM_UNIT_X] < 900.0)
            {
                return false;
            }

            if (SumValue < 0.0)
            {
                return false;
            }
            */

            return KSM_OK;

            // --------------------------------------------------
        }

        public static short RiskCheck()
        {
            short nRet = -1;
            int AxisIndex = -1;


            frmPowerDown frm_power_down = new frmPowerDown();

           
            // 1. Ping 체크 ...
            // --------------------------------------------------
            MultiMotion.bPingCheckedA = false;
            MultiMotion.bPingCheckedB = false;
            MultiMotion.bPingCheckedC = false;

            for (int i = 0; i < 3; i++)
            {
                // 모션 컨트롤러 A
                // ----------
                nRet = NMC2.nmc_PingCheck(DeviceManager.g_ndevIdA_4, 250);
                if (nRet == 0) {
                    MultiMotion.bPingCheckedA = true;
                }

                // 모션 컨트롤러 B
                // ----------
                nRet = NMC2.nmc_PingCheck(DeviceManager.g_ndevIdB_4, 250);
                if (nRet == 0)
                    MultiMotion.bPingCheckedB = true;


                // 모션 컨트롤러 C
                // ----------
                nRet = NMC2.nmc_PingCheck(DeviceManager.g_ndevIdC_4, 250);
                if (nRet == 0)
                    MultiMotion.bPingCheckedC = true;
            }

            if (MultiMotion.bPingCheckedA == false || MultiMotion.bPingCheckedB == false || MultiMotion.bPingCheckedC == false)
            {   
                frm_power_down.ShowDialog();

                return KSM_NOTCONNECT;
            }
            

            // 3. 원점 수행 체크 ...
            // --------------------------------------------------            
            NMC2.NMCHOMEFLAG HomeFlagA;
            NMC2.NMCHOMEFLAG HomeFlagB;            

            nRet = NMC2.nmc_GetHomeStatus(DeviceManager.g_ndevIdA_4, out HomeFlagA);
            if (nRet == 0)
            {
                if (HomeFlagA.nStatusFlag[MultiMotion.VBLOCK_Z] == 3)
                {
                    frm_power_down.ShowDialog();

                    return KSM_HOME_FAIL;
                }
                    

                if (HomeFlagA.nStatusFlag[MultiMotion.CAM_UNIT_Y] == 3)
                {
                    frm_power_down.ShowDialog();

                    return KSM_HOME_FAIL;
                }                    

                if (HomeFlagA.nStatusFlag[MultiMotion.BACK_CAM_Z] == 3)
                {
                    frm_power_down.ShowDialog();

                    return KSM_HOME_FAIL;
                }
                    

                if (HomeFlagA.nStatusFlag[MultiMotion.ROLLING_FIX_1] == 3)
                {
                    frm_power_down.ShowDialog();

                    return KSM_HOME_FAIL;
                }
                    

                if (HomeFlagA.nStatusFlag[MultiMotion.ROLLING_FIX_2] == 3)
                {
                    frm_power_down.ShowDialog();

                    return KSM_HOME_FAIL;
                }
                    

                if (HomeFlagA.nStatusFlag[MultiMotion.ROLLING_MOVE_1] == 3)
                {
                    frm_power_down.ShowDialog();

                    return KSM_HOME_FAIL;
                }
                    

                if (HomeFlagA.nStatusFlag[MultiMotion.ROLLING_MOVE_2] == 3)
                {
                    frm_power_down.ShowDialog();

                    return KSM_HOME_FAIL;
                }
                    
            }
            else
            {
                frm_power_down.ShowDialog();

                return nRet;
            }

            if (NMC2.nmc_GetHomeStatus(DeviceManager.g_ndevIdB_4, out HomeFlagB) == 0)
            {
                AxisIndex = MultiMotion.CAM_UNIT_X - 8;
                if (HomeFlagA.nStatusFlag[AxisIndex] == 3)
                {
                    frm_power_down.ShowDialog();

                    return KSM_HOME_FAIL;
                }
                    

                AxisIndex = MultiMotion.CAM_UNIT_Z - 8;
                if (HomeFlagA.nStatusFlag[AxisIndex] == 3)
                {
                    frm_power_down.ShowDialog();

                    return KSM_HOME_FAIL;
                }
                    

                AxisIndex = MultiMotion.INDEX_MOVE_M - 8;
                if (HomeFlagA.nStatusFlag[AxisIndex] == 3)
                {
                    frm_power_down.ShowDialog();

                    return KSM_HOME_FAIL;
                }
                    

                /*
                AxisIndex = MultiMotion.INDEX_MOVE_S - 8;
                if (HomeFlagA.nStatusFlag[AxisIndex] == 3)
                    return KSM_HOME_FAIL;
                
                AxisIndex = MultiMotion.INDEX_FIX_R - 8;
                if (HomeFlagA.nStatusFlag[AxisIndex] == 3)
                    return KSM_HOME_FAIL;

                AxisIndex = MultiMotion.INDEX_MOVE_R - 8;
                if (HomeFlagA.nStatusFlag[AxisIndex] == 3)
                    return KSM_HOME_FAIL;
                */
            }
            else
            {
                frm_power_down.ShowDialog();

                return nRet;
            }

            return KSM_OK;
        }

        public static short AlarmCheck()
        {
            // 시스템이 준비 되었는지 ...
            if (bSystemReady != true)
            {
                return KSM_OK;
            }



            MultiMotion.GetCurrentPos();


            bool bAlarm = false;


            for (int i = 0; i < 7; i++)
            {
                if (MultiMotion.AlarmValue[i] == 1)
                {
                    bAlarm = true;

                    break;
                }
            }


            for (int i = 0; i < 6; i++)
            {
                if (MultiMotion.AlarmValue[i + 8] == 1)
                {
                    bAlarm = true;

                    break;
                }
            }


            if (bAlarm == true)
            {
                return KSM_ALARM;
            }


            return KSM_OK;
        }

        public static short AlarmReset()
        {
            short nRet = -1;

            for (short i = 0; i < 8; i++)
            {
                NMC2.nmc_SetAlarmResetOn(DeviceManager.g_ndevIdA_4, i, 1);

                CommonUtility.WaitTime(50, true);
            }

            for (short i = 0; i < 6; i++)
            {
                NMC2.nmc_SetAlarmResetOn(DeviceManager.g_ndevIdB_4, i, 1);

                CommonUtility.WaitTime(50, true);
            }

            //CommonUtility.WaitTime(100, false);

            for (short i = 0; i < 8; i++)
            {
                NMC2.nmc_SetAlarmResetOn(DeviceManager.g_ndevIdA_4, i, 0);

                CommonUtility.WaitTime(50, true);
            }

            for (short i = 0; i < 6; i++)
            {
                NMC2.nmc_SetAlarmResetOn(DeviceManager.g_ndevIdB_4, i, 0);

                CommonUtility.WaitTime(50, true);
            }    

            return KSM_OK;
        }


#endregion 방어 코드 ...


    }
}
