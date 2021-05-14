using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

//using Automation.BDaq;

using DiaDetector.Data;
using PAIX_NMF_DEV;

namespace DiaDetector.Drivers
{
    public class MultiMotion
    {
        // 0~7번 축
        // --------------------------------------------------
        public static PaixMotion PaixMotionA;
        public static NMC2.NMCAXESEXPR NmcDataA;
        public static short g_nDevopenA;

        // DIO
        // --------------------------------------------------
        public static PaixMotion PaixMotionC;
        public static NMC2.NMCAXESEXPR NmcDataC;
        public static short g_nDevopenC;
        
        // 컨트롤러 및 축 지정
        // --------------------------------------------------
        public static PaixMotion CurPaixMotion = null;    

        public static double[] AxisValue = new double[8];
        public static double[] AxisCmdValue = new double[8];
        public static short[] AlarmValue = new short[8];
        public static bool[] ReadyHome = new bool[8];      
        


        // 속도 배속 ... 
        // => 기본 속도가 너무 낮아서 추가함.(홍동성 - 20160714)        
        // => 저장은 WorkFuncInfo 클래스에서 함.
        
        public static int Velocity_Multiple = 1;        
        // --------------------------------------------------

        public static short ErrorStatus = -1;
        private static bool bGetPosReady = false;



        // DP
        // --------------------------------------------------
        public static short Camera1Adjust = -1;
        public static short Camera2Adjust = -1;
        public static short Lift1Motor = -1;
        public static short Lift2Motor = -1;
        public static short Shuttle1Motor = -1;
        public static short Shuttle2Motor = -1;
        public static short RotationMotor = -1;




       


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
        public static bool bSystemReady = false;
        

        // 속도 관련 ...
        // ----------
        public const int KSM_SPEED_SSLOW = 0;
        public const int KSM_SPEED_SLOW = 1;
        public const int KSM_SPEED_MIDIUM = 2;
        public const int KSM_SPEED_FAST = 3;

        public const int KSM_SPEED_05 = 4;
        public const int KSM_SPEED_10 = 5;
        public const int KSM_SPEED_20 = 6;
        public const int KSM_SPEED_30 = 7;
        public const int KSM_SPEED_40 = 8;
        public const int KSM_SPEED_50 = 9;
        public const int KSM_SPEED_60 = 10;
        public const int KSM_SPEED_70 = 11;
        public const int KSM_SPEED_80 = 12;
        public const int KSM_SPEED_90 = 13;
        public const int KSM_SPEED_100 = 14;

        public static double dSpeedFactor = 1.0;
        public static double dSpeedFactor2 = 0.1;
        public static double dSpeedFactor1 = 1.0;
        public static int nSpeedMode = 0;
        public static short DPSB;
        public    int ButtonOT = 111;
        public   int AreaS = 112;
        public   int None = 112;
        public  bool bEAutoStop = false;

        public static int bus;



        // 이 장비 원점 고정을 위해 홈 찾는 속도는 고정되어야 하므로 생긴 변수와 함수
        public static MotionSettingInfo[] MotionSettingInfoList = new MotionSettingInfo[8];






        
        public static void InitMotionSettingInfo()
        {
           
            for (int i = 0; i < 8; i++)
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


                // 원점 관련 ...
                // ----------
                MultiMotion.MotionSettingInfoList[i].Velocity_Home_1 = 0.0;
                MultiMotion.MotionSettingInfoList[i].Velocity_Home_2 = 0.0;
                MultiMotion.MotionSettingInfoList[i].Velocity_Home_3 = 0.0;
                MultiMotion.MotionSettingInfoList[i].Velocity_Home_Offset = 0.0;
                MultiMotion.MotionSettingInfoList[i].Home_Offset = 0.0;

                MultiMotion.MotionSettingInfoList[i].Velocity_Start_Home = 0.0;
                MultiMotion.MotionSettingInfoList[i].Velocity_Acc_Home = 0.0;
                MultiMotion.MotionSettingInfoList[i].Velocity_Dec_Home = 0.0;
                MultiMotion.MotionSettingInfoList[i].Velocity_Max_Home = 0.0;

            }

        }
        //static string[] ModelData1;
        //public static void concheck()
        //{
        //    try
        //    {
        //        string[] splitdata = new string[100];
        //        ModelData1 = System.IO.File.ReadAllLines("C:\\KSM\\DiaDetector\\Data\\Model\\conn.txt", System.Text.Encoding.Default);
        //        foreach (var re in ModelData1)
        //        {
        //            splitdata = re.Split('#');
        //        }
        //        //  CamMotions.GML = Convert.ToDouble(splitdata[14]) ;
        //        //  CamMotions.Type = splitdata[44];  
        //        MotionCheck = Convert.ToInt32(splitdata[0]);
              
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

#region Initialize ...
      // static string[] ModelData;
        public static int MotionCheck =2;
        public static void DPBSCheck()
        {
            if (MotionCheck == 1)
            {
                MultiMotion.DPSB = DeviceManagerS.g_ndevIdC_44;
            }
            else
            {
                MultiMotion.DPSB = DeviceManagerS.g_ndevIdA_4;
            }
        }

        public static short Initialize()
        {

         //   concheck();
            DPBSCheck();
            short nRet = KSM_OK;
          
            MultiMotion.UpdateAxisInfo();
          

            // 1. 모션 컨트롤러 A 초기화 ...
            // --------------------------------------------------
            PaixMotionA = new PaixMotion();
            NmcDataA = new NMC2.NMCAXESEXPR();
            g_nDevopenA = 0;

            nRet = NMC2.nmc_SetIPAddress(DeviceManagerS.g_ndevIdA_4, DeviceManagerS.g_ndevIdA_1, DeviceManagerS.g_ndevIdA_2, DeviceManagerS.g_ndevIdA_3);
            /*            
            if (nRet != KSM_OK)
                return nRet;
            */

            bool bOpen = PaixMotionA.Open(DeviceManagerS.g_ndevIdA_4);
            if (bOpen)
                g_nDevopenA = 1;
            else
                return KSM_NOTCONNECT;


            MultiMotion.CurPaixMotion = PaixMotionA;

            NMF.nmf_Connect(DeviceManagerS.g_ndevIdC_44, DeviceManagerS.g_ndevIdC_11, DeviceManagerS.g_ndevIdC_22, DeviceManagerS.g_ndevIdC_33);

            if (MotionCheck == 1)
            {
                // 3. 모션 컨트롤러 C 초기화 ...
                // --------------------------------------------------
                PaixMotionC = new PaixMotion();
                NmcDataC = new NMC2.NMCAXESEXPR();
                g_nDevopenC = 0;

                nRet = NMC2.nmc_SetIPAddress(DeviceManagerS.g_ndevIdC_44, DeviceManagerS.g_ndevIdC_11, DeviceManagerS.g_ndevIdC_22, DeviceManagerS.g_ndevIdC_33);
                /*            
                if (nRet != KSM_OK)
                    return nRet;
                */

                bOpen = PaixMotionC.Open(DeviceManagerS.g_ndevIdC_44);
                if (bOpen)
                    g_nDevopenC = 1;
                else
                    return KSM_NOTCONNECT;

            }


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


            return KSM_OK;
        }

        public static short SystemInit()
        {
            short nRet = -1;


            // 1. 마그네틱 ON
            // ----------
            //for (short i = 11; i < 26; i++)
            //{
            //    nRet = NMC2.nmc_SetMDIOOutputBit(DeviceManager.g_ndevIdA_4, i, 1);

            //    //MultiMotion.Write_Output(i, true);

            //    CommonUtility.WaitTime(50, true);

            //    if (nRet != 0)
            //    {
            //        //return nRet;
            //    }
            //}

            // 서보ON
            for (short i = 6; i < 7; i++)
            {
                NMC2.nmc_SetServoOn(DeviceManagerS.g_ndevIdA_4, i, 1);

                CommonUtility.WaitTime(50, true);
            }

            //서보 READY
            for (short i = 6; i < 7; i++)
            
            {
                NMC2.nmc_SetSReadyLogic(DeviceManagerS.g_ndevIdA_4, i, 1);
                CommonUtility.WaitTime(50, true);
            }

            // 4. Alarm Reset  //2016.12.30 수정
            // ----------
            
            //for (short i = 0; i < 7; i++)
            //{
            //    NMC2.nmc_SetAlarmResetOn(DeviceManager.g_ndevIdA_4, i, 1);

            //    CommonUtility.WaitTime(50, true);
            //}

            

            for (short i = 0; i < 7; i++)
            {
                NMC2.nmc_SetAlarmResetOn(DeviceManagerS.g_ndevIdA_4, i, 0);

                CommonUtility.WaitTime(50, true);
            }

            

            // 홈 리턴 ...
            // ----------
            MultiMotion.InitMotionSettingInfo();

            MultiMotion.HomeReturn();
            // ----------
            return KSM_OK;

            // 0. 전원 및 원점 체크 ...
            // ----------
            nRet = MultiMotion.RiskCheck();
            if (nRet != KSM_OK)
                return nRet;
            // ----------


            return KSM_OK;
        }

        public static short HomeReturn()
        {

            MultiMotion.GetDIOStatus();
            short nRet = -1;
            NMC2.nmc_SetDIOOutputBit(MultiMotion.DPSB, 16, 0);  //셔틀1 클로즈
            NMC2.nmc_SetDIOOutputBit(MultiMotion.DPSB, 17, 1);  //셔틀1 오픈
            NMC2.nmc_SetDIOOutputBit(MultiMotion.DPSB, 20, 0);  //셔플2 클로즈
            NMC2.nmc_SetDIOOutputBit(MultiMotion.DPSB, 21, 1);  //셔틀2 오픈
            NMC2.nmc_SetDIOOutputBit(MultiMotion.DPSB, 24, 1);  //바큠 흡입 A-B 오픈
            NMC2.nmc_SetDIOOutputBit(MultiMotion.DPSB, 25, 1);  //바큠 흡입 C-D 오픈
            NMC2.nmc_SetDIOOutputBit(MultiMotion.DPSB, 26, 0);  //바큠 불기 A-B 오픈
            NMC2.nmc_SetDIOOutputBit(MultiMotion.DPSB, 27, 0);  //바큠 불기 C-D 오픈
            NMC2.nmc_SetDIOOutputBit(MultiMotion.DPSB, 8, 0);  //리프트 실린더 업

            // return KSM_OK;
            // 축 초기화 ..
            // --------------------------------------------------            
            if (MultiMotion.InStatus[8] == 1 && MultiMotion.InStatus[10] == 1 ) //리프트 실린더 센서가 업일때
            {
                MultiMotion.UpdateAxisInfo();

                //nRet = MultiMotion.HomeMove(MultiMotion.RotationMotor, false);

                //Application.DoEvents();

                //nRet = MultiMotion.HomeMove(MultiMotion.Camera2Adjust, false);

                //Application.DoEvents();

                //nRet = MultiMotion.HomeMove(MultiMotion.Lift1Motor, false);

                //Application.DoEvents();

                //nRet = MultiMotion.HomeMove(MultiMotion.Lift2Motor, false);

                //Application.DoEvents();

                //nRet = MultiMotion.HomeMove(MultiMotion.Camera1Adjust, false);

                //Application.DoEvents();


                //nRet = MultiMotion.HomeMove(MultiMotion.Shuttle1Motor, false);

                //Application.DoEvents();

                //nRet = MultiMotion.HomeMove(MultiMotion.Shuttle2Motor, false);

                //Application.DoEvents();






                nRet = MultiMotion.HomeMove(MultiMotion.RotationMotor, true);
                MultiMotion.ReadyHome[MultiMotion.RotationMotor] = true;
                Application.DoEvents();

                nRet = MultiMotion.HomeMove(MultiMotion.Camera2Adjust, true);
                MultiMotion.ReadyHome[MultiMotion.Camera2Adjust] = true;
                Application.DoEvents();

                nRet = MultiMotion.HomeMove(MultiMotion.Lift1Motor, true);
                MultiMotion.ReadyHome[MultiMotion.Lift1Motor] = true;
                Application.DoEvents();

                nRet = MultiMotion.HomeMove(MultiMotion.Lift2Motor, true);
                MultiMotion.ReadyHome[MultiMotion.Lift2Motor] = true;
                Application.DoEvents();

                nRet = MultiMotion.HomeMove(MultiMotion.Camera1Adjust, true);
                MultiMotion.ReadyHome[MultiMotion.Camera1Adjust] = true;
                Application.DoEvents();


                nRet = MultiMotion.HomeMove(MultiMotion.Shuttle1Motor, true);
                MultiMotion.ReadyHome[MultiMotion.Shuttle1Motor] = true;
                Application.DoEvents();

                nRet = MultiMotion.HomeMove(MultiMotion.Shuttle2Motor, true);
                MultiMotion.ReadyHome[MultiMotion.Shuttle2Motor] = true;
                Application.DoEvents();

                /*

                // 카메라 유닛 원점 복귀 ...
                nRet = MultiMotion.HomeMove(MultiMotion.CAM_AXIS_A, true);
                MultiMotion.ReadyHome[MultiMotion.CAM_AXIS_A] = true;
                Application.DoEvents();


            
                nRet = MultiMotion.HomeMove(MultiMotion.CAM_AXIS_B, true);
                MultiMotion.ReadyHome[MultiMotion.CAM_AXIS_B] = true;
                Application.DoEvents();

                nRet = MultiMotion.HomeMove(MultiMotion.CAM_AXIS_C, true);
                MultiMotion.ReadyHome[MultiMotion.CAM_AXIS_C] = true;
                Application.DoEvents();


                // 후방 카메라 원점 복귀 ...
                MultiMotion.HomeMove(MultiMotion.CAM_UNIT_Z, true);
                MultiMotion.ReadyHome[MultiMotion.CAM_UNIT_Z] = true;
                Application.DoEvents();



                // V블럭 원점 복귀 ...
                MultiMotion.HomeMove(MultiMotion.CAM_AXIS_ROTATE, true);
                MultiMotion.ReadyHome[MultiMotion.CAM_AXIS_ROTATE] = true;
                Application.DoEvents();


                MultiMotion.HomeMove(MultiMotion.INDEX_IN_OUT, true);
                MultiMotion.ReadyHome[MultiMotion.INDEX_IN_OUT] = true;
                Application.DoEvents();


                */
            }
            else
            {
                if (DialogResult.Yes == MessageBox.Show("공압이상 발생?", "프로그램 종료", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    Application.Exit();
                    System.Diagnostics.Process.GetCurrentProcess().Kill();

                }
                else
                {
                    Application.Exit();
                    System.Diagnostics.Process.GetCurrentProcess().Kill();

                }
            }

            return nRet;
        }

        public static short getAxisIndex(short AxisType)
        {
            short AxisIndex = -1;

            for (short i = 0; i < 8; i++)
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

            MultiMotion.Camera1Adjust = MultiMotion.getAxisIndex(0);
            MultiMotion.Camera2Adjust = MultiMotion.getAxisIndex(1);
            MultiMotion.Lift1Motor = MultiMotion.getAxisIndex(2);
            MultiMotion.Lift2Motor = MultiMotion.getAxisIndex(3);
            MultiMotion.Shuttle1Motor = MultiMotion.getAxisIndex(4);
            MultiMotion.Shuttle2Motor = MultiMotion.getAxisIndex(5);
            MultiMotion.RotationMotor = MultiMotion.getAxisIndex(6);
        }

        public static short JogStop(short AxisIndex)
        {
            short nRet = -1;


            // 0. 전원 및 원점 체크 ...
            // ----------
            nRet = MultiMotion.RiskCheck();
            if (nRet != KSM_OK)
                return nRet;
            // ----------


            // 2. 조그 동작 ...
            if (MultiMotion.CurPaixMotion.Stop(AxisIndex) != true)
                return KSM_INVALID;

            return KSM_OK;
        }

        public static short StopAll()
        {
            short nRet = -1;

            for (short i = 0; i < 8; i++)
            {
                nRet = NMC2.nmc_SuddenStop(DeviceManagerS.g_ndevIdA_4, i);
            }


            return nRet;
        }

        public static short IndexRPosClear()
        {
            short nRet = -1;


            // 0. 전원 및 원점 체크 ...
            // ----------
            nRet = MultiMotion.RiskCheck();
            if (nRet != KSM_OK)
                return nRet;
            // ----------


            nRet = NMC2.nmc_SetDccOn(DeviceManagerS.g_ndevIdA_4, MultiMotion.Camera2Adjust, 1);
            if (nRet != KSM_OK)
                return nRet;


            System.Threading.Thread.Sleep(100);


            nRet = NMC2.nmc_SetCmdEncPos(DeviceManagerS.g_ndevIdA_4, MultiMotion.Camera2Adjust, 0.0);
            if (nRet != KSM_OK)
                return nRet;
            

            System.Threading.Thread.Sleep(100);


            nRet = NMC2.nmc_SetDccOn(DeviceManagerS.g_ndevIdA_4, MultiMotion.Camera2Adjust, 0);
            if (nRet != KSM_OK)
                return nRet;


            return KSM_OK;
        }

        public static void GetCurrentPos()  //현재위치,실제위치,알람
        {
            // 5. 정확한 수치값을 얻기 위해 UnitPerPulse를 지정 ...
            // ----------
            if (MultiMotion.bGetPosReady == false)
            {
                for (short i = 0; i < 8; i++)
                {
                    MultiMotion.PaixMotionA.SetUnitPulse(i, DataManager.MotionSettingInfoList[i].Logic_UnitPerPulse);
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

        }
        public static void Speed1(int SpeedMode)
        {
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
        public static void SetSpeed(short AxisIndex ,int SpeedMode)
        {
            MultiMotion.nSpeedMode = SpeedMode;
            if (MultiMotion.RotationMotor == AxisIndex)
            {
                Speed1(SpeedMode);
            }
            else if (MultiMotion.Camera2Adjust == AxisIndex)
            {
                Speed1(SpeedMode);
            }
            else if (MultiMotion.Lift1Motor == AxisIndex)
            {
                Speed1(SpeedMode);
            }
            else if (MultiMotion.Lift2Motor == AxisIndex)
            {
                Speed1(SpeedMode);
            }
            else if (MultiMotion.Camera1Adjust == AxisIndex)
            {
                Speed1(SpeedMode);
            }
            else if (MultiMotion.Shuttle1Motor == AxisIndex)
            {
                Speed1(SpeedMode);
            }
            else
            {
                Speed1(SpeedMode);
            }
        
        }

        public static short GantryAxisEnable(short nGroup, bool bEnable) //갠트리
        {

            // 사용예
            
            //if (MultiMotion.GantryAxisEnable(1, true) == MultiMotion.KSM_OK)
            //{
            //    MultiMotion.HomeMove(MultiMotion.CAM_AXIS_A, true);

            //    MessageBox.Show("원점 이동이 완료되었습니다.");
            //}
            



            short nRet = -1;

            bool bOk = false;

            short[] nGantryEnable = new short[2];
            short[] nGantryMain = new short[2];
            short[] nGantrySub = new short[2];

            if (bEnable)
            {
               // nRet = NMC2.nmc_SetGantryAxis(DeviceManager.g_ndevIdA_4, nGroup, 0, 1);
                nRet = NMC2.nmc_SetGantryAxis(DeviceManagerS.g_ndevIdA_4, nGroup, 2, 3);
                if (nRet != KSM_OK)
                    return nRet;

                nRet = NMC2.nmc_SetGantryEnable(DeviceManagerS.g_ndevIdA_4, nGroup, 1);
                if (nRet != KSM_OK)
                    return nRet;

                nRet = NMC2.nmc_GetGantryInfo(DeviceManagerS.g_ndevIdA_4, out nGantryEnable[0], out nGantryMain[0], out nGantrySub[0]);
                if (nRet != KSM_OK)
                    return nRet;

                bOk = (nGantryEnable[0] == 0x01 && nGantryMain[0] == 0x00 && nGantrySub[0] == 0x01);
                if (bOk == false)
                    return KSM_INVALID;
            }
            else
            {
                nRet = NMC2.nmc_SetGantryAxis(DeviceManagerS.g_ndevIdA_4, nGroup, 2, 3);
                if (nRet != KSM_OK)
                    return nRet;

                nRet = NMC2.nmc_SetGantryEnable(DeviceManagerS.g_ndevIdA_4, nGroup, 0);
                if (nRet != KSM_OK)
                    return nRet;

                nRet = NMC2.nmc_GetGantryInfo(DeviceManagerS.g_ndevIdA_4, out nGantryEnable[0], out nGantryMain[0], out nGantrySub[0]);
                if (nRet != KSM_OK)
                    return nRet;

                bOk = (nGantryEnable[0] == 0x00);
                if (bOk == false)
                    return KSM_INVALID;
            }

            return KSM_OK;
        }


      

#endregion Initialize ...


#region 기본 축 이동 명령어 ...


        public static short JogMove(short AxisIndex, short nDir)
        {
            short nRet = -1;

            // 0. 전원 및 원점 체크 ...
            // ----------
            nRet = MultiMotion.RiskCheck();
            if (nRet != KSM_OK)
                return nRet;
            // ----------


            MultiMotion.CurPaixMotion.SetUnitPulse(AxisIndex, DataManager.MotionSettingInfoList[AxisIndex].Logic_UnitPerPulse);

            // 3. Software Limit ...
            // ----------
            nRet = NMC2.nmc_SetSWLimitLogic(DeviceManagerS.g_ndevIdA_4, AxisIndex, 1,
                DataManager.MotionSettingInfoList[AxisIndex].MinValue,
                DataManager.MotionSettingInfoList[AxisIndex].MaxValue);

            if (AxisIndex == MultiMotion.RotationMotor)
            {
                nRet = NMC2.nmc_SetSpeed(DeviceManagerS.g_ndevIdA_4, AxisIndex,
                  DataManager.MotionSettingInfoList[AxisIndex].Velocity_Start * MultiMotion.dSpeedFactor2,
                  DataManager.MotionSettingInfoList[AxisIndex].Velocity_Acc * MultiMotion.dSpeedFactor2,
                  DataManager.MotionSettingInfoList[AxisIndex].Velocity_Dec * MultiMotion.dSpeedFactor2,
                  DataManager.MotionSettingInfoList[AxisIndex].Velocity_Max * MultiMotion.dSpeedFactor2);
            }
            else
            {

                // 사다리꼴 속도 지정
                nRet = NMC2.nmc_SetSpeed(DeviceManagerS.g_ndevIdA_4, AxisIndex,
                    DataManager.MotionSettingInfoList[AxisIndex].Velocity_Start * MultiMotion.dSpeedFactor,
                    DataManager.MotionSettingInfoList[AxisIndex].Velocity_Acc * MultiMotion.dSpeedFactor,
                    DataManager.MotionSettingInfoList[AxisIndex].Velocity_Dec * MultiMotion.dSpeedFactor,
                    DataManager.MotionSettingInfoList[AxisIndex].Velocity_Max * MultiMotion.dSpeedFactor);
            }

            // 3. 조그 동작 ...
            // --------------------------------------------------
            if (MultiMotion.CurPaixMotion.JogMove(AxisIndex, nDir) != true)
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


            if (MultiMotion.CurPaixMotion == null)
                return KSM_NOTCONNECT;
            MultiMotion.CurPaixMotion.SetUnitPulse(AxisIndex, DataManager.MotionSettingInfoList[AxisIndex].Logic_UnitPerPulse);


            // 3. Software Limit ...
            // ----------
            nRet = NMC2.nmc_SetSWLimitLogic(DeviceManagerS.g_ndevIdA_4, AxisIndex, 1,
                DataManager.MotionSettingInfoList[AxisIndex].MinValue,
                DataManager.MotionSettingInfoList[AxisIndex].MaxValue);


            // S자 속도 지정
            nRet = NMC2.nmc_SetSCurveSpeed(DeviceManagerS.g_ndevIdA_4, AxisIndex,
                DataManager.MotionSettingInfoList[AxisIndex].Velocity_Start * MultiMotion.dSpeedFactor,
                DataManager.MotionSettingInfoList[AxisIndex].Velocity_Acc * MultiMotion.dSpeedFactor,
                DataManager.MotionSettingInfoList[AxisIndex].Velocity_Dec * MultiMotion.dSpeedFactor,
                DataManager.MotionSettingInfoList[AxisIndex].Velocity_Max * MultiMotion.dSpeedFactor);

            if (MultiMotion.CurPaixMotion.AbsMove(AxisIndex, dMoveValue) == false)
            {
                return KSM_INVALID;
            }


            if (bWait == false)
            {
                return KSM_OK;
            }


            while (true)
            {
                nRet = NMC2.nmc_GetAxesExpress(DeviceManagerS.g_ndevIdA_4, out NmcDataA);
                if (nRet != KSM_OK)
                    return nRet;

                if (NmcDataA.nBusy[AxisIndex] == 0) // if (NmcDataA.nBusy[0] == 0) => 원래 코드
                {
                    break;
                }

                System.Threading.Thread.Sleep(100);
            }


            return KSM_OK;
        }

        public static short MoveAxis(int AxisIndex, double AxisValue, bool bWait, int SpeedMode = -1)
        {
            short nRet = -1;
            double dSpeedFactor11 = 0.0;;
            switch (SpeedMode)
            {
                case -1:
                    dSpeedFactor11 = dSpeedFactor;
                    break;
                case KSM_SPEED_SSLOW:
                    dSpeedFactor11 = 0.3;
                    break;
                case KSM_SPEED_SLOW:
                    dSpeedFactor11 = 1.0;
                    break;
                case KSM_SPEED_MIDIUM:
                    dSpeedFactor11 = 3.0;
                    break;
                case KSM_SPEED_FAST:
                    dSpeedFactor11 = 5.0;
                    break;

                case KSM_SPEED_05:
                    dSpeedFactor11 = 0.5;
                    break;
                case KSM_SPEED_10:
                    dSpeedFactor11 = 1.0;
                    break;
                case KSM_SPEED_20:
                    dSpeedFactor11 = 2.0;
                    break;
                case KSM_SPEED_30:
                    dSpeedFactor11 = 3.0;
                    break;
                case KSM_SPEED_40:
                    dSpeedFactor11 = 4.0;
                    break;
                case KSM_SPEED_50:
                   dSpeedFactor11 = 5.0;
                    break;
                case KSM_SPEED_60:
                    dSpeedFactor11 = 6.0;
                    break;
                case KSM_SPEED_70:
                    dSpeedFactor11 = 7.0;
                    break;
                case KSM_SPEED_80:
                    dSpeedFactor11 = 8.0;
                    break;
                case KSM_SPEED_90:
                    dSpeedFactor11 = 9.0;
                    break;
                case KSM_SPEED_100:
                    dSpeedFactor11 = 10.0;
                    break;

            }

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
            


            MultiMotion.CurPaixMotion.SetUnitPulse((short)AxisIndex, DataManager.MotionSettingInfoList[AxisIndex].Logic_UnitPerPulse);


            // 3. Software Limit ...
            // ----------
            nRet = NMC2.nmc_SetSWLimitLogic(DeviceManagerS.g_ndevIdA_4, (short)AxisIndex, 1,
                DataManager.MotionSettingInfoList[AxisIndex].MinValue,
                DataManager.MotionSettingInfoList[AxisIndex].MaxValue);

            if (nRet != KSM_OK)
                return nRet;

            // S자 속도 지정
            //if (AxisIndex == 0 || AxisIndex ==2 || AxisIndex == 3 || AxisIndex == 4 || AxisIndex == 5 ||  AxisIndex == 6  )
            //{
            //nRet = NMC2.nmc_SetSCurveSpeed(DeviceManagerS.g_ndevIdA_4, (short)AxisIndex,
            //    DataManager.MotionSettingInfoList[AxisIndex].Velocity_Start * dSpeedFactor11,
            //    DataManager.MotionSettingInfoList[AxisIndex].Velocity_Acc * dSpeedFactor11,
            //    DataManager.MotionSettingInfoList[AxisIndex].Velocity_Dec * dSpeedFactor11,
            //    DataManager.MotionSettingInfoList[AxisIndex].Velocity_Max * dSpeedFactor11);
            nRet = NMC2.nmc_SetSpeed(DeviceManagerS.g_ndevIdA_4, (short)AxisIndex,
               DataManager.MotionSettingInfoList[AxisIndex].Velocity_Start * dSpeedFactor11,
               DataManager.MotionSettingInfoList[AxisIndex].Velocity_Acc * dSpeedFactor11,
               DataManager.MotionSettingInfoList[AxisIndex].Velocity_Dec * dSpeedFactor11,
               DataManager.MotionSettingInfoList[AxisIndex].Velocity_Max * dSpeedFactor11);

            //}
            //if (AxisIndex == 1)
            //{
            //    nRet = NMC2.nmc_SetSCurveSpeed(DeviceManager.g_ndevIdA_4, (short)AxisIndex,
            //  DataManager.MotionSettingInfoList[AxisIndex].Velocity_Start * MultiMotion.dSpeedFactor1,
            //  DataManager.MotionSettingInfoList[AxisIndex].Velocity_Acc * MultiMotion.dSpeedFactor1,
            //  DataManager.MotionSettingInfoList[AxisIndex].Velocity_Dec * MultiMotion.dSpeedFactor1,
            //  DataManager.MotionSettingInfoList[AxisIndex].Velocity_Max * MultiMotion.dSpeedFactor1);
            //}

            if (MultiMotion.CurPaixMotion.AbsMove((short)AxisIndex, dMoveValue) == false)
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
                nRet = NMC2.nmc_GetAxesExpress(DeviceManagerS.g_ndevIdA_4, out NmcDataA);
                if (nRet != KSM_OK)
                    return nRet;

                if (NmcDataA.nBusy[AxisIndex] == 0) // if (NmcDataA.nBusy[0] == 0) => 원래 코드
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




            
            //// 2. 이미 원점 위치에 있다면, true를 return하고 끝낸다.
            //NMC2.nmc_GetHomeStatus(DeviceManager.g_ndevIdA_4, out HomeFlag);
            ////if (HomeFlag.nSrchFlag[0] == 0 && HomeFlag.nStatusFlag[0] == 0) // => 0이 인덱스 인가
            //if (HomeFlag.nSrchFlag[MultiMotion.AxisNumber] == 0 && HomeFlag.nStatusFlag[MultiMotion.AxisNumber] == 0)
            //{
            //    return true;
            //}
            


            MultiMotion.CurPaixMotion.SetUnitPulse((short)AxisIndex, DataManager.MotionSettingInfoList[AxisIndex].Logic_UnitPerPulse);


            // S자 속도 지정(원점이동의 가감속만 지정됨)
            nRet = NMC2.nmc_SetSCurveSpeed(DeviceManagerS.g_ndevIdA_4, (short)AxisIndex,
                DataManager.MotionSettingInfoList[AxisIndex].Velocity_Start,
                DataManager.MotionSettingInfoList[AxisIndex].Velocity_Acc,
                DataManager.MotionSettingInfoList[AxisIndex].Velocity_Dec,
                DataManager.MotionSettingInfoList[AxisIndex].Velocity_Max);


            // 가속 : 1000이 지정됨(원점이동의 가속도로 지정됨)
            nRet = NMC2.nmc_SetAccSpeed(DeviceManagerS.g_ndevIdA_4, (short)AxisIndex,
                DataManager.MotionSettingInfoList[AxisIndex].Velocity_Acc);

            // 감속 : 1000이 지정됨(원점이동의 감속도로 지정됨)
            nRet = NMC2.nmc_SetDecSpeed(DeviceManagerS.g_ndevIdA_4, (short)AxisIndex,
                DataManager.MotionSettingInfoList[AxisIndex].Velocity_Dec);


            // 0번 축의 1차 속도 2000, 2차 1500, 3차 1000으로 설정
            //nRet = NMC2.nmc_SetHomeSpeed(DeviceManager.g_ndevIdA_4, MultiMotion.AxisNumber, 3, 2, 1);
            nRet = NMC2.nmc_SetHomeSpeedEx(DeviceManagerS.g_ndevIdA_4, (short)AxisIndex,
                DataManager.MotionSettingInfoList[AxisIndex].Velocity_Home_1,
                DataManager.MotionSettingInfoList[AxisIndex].Velocity_Home_2,
                DataManager.MotionSettingInfoList[AxisIndex].Velocity_Home_3,
                DataManager.MotionSettingInfoList[AxisIndex].Velocity_Home_Offset); // 원점 Offset 속도까지 지정하려면 이 함수를 사용해야
            //nRet = NMC2.nmc_SetHomeSpeed(DeviceManager.g_ndevIdA_4, MultiMotion.AxisNumber, 10, 5, 2);


            // ==================================================
            // 1. 0번 축의 원점 이동 모드 : +Near
            // 2. 원점 검색후(3차이동 까지 완료) CW회전으로 Z상 검출
            // 3. 지령/엔코더 위치 0으로 초기화
            // 4. Offset 300 위치로 이동
            // 5. Offset 이동 후 지령/엔코더 위치 0으로 초기화
            //nRet = NMC2.nmc_HomeMove(DeviceManager.g_ndevIdA_4, MultiMotion.AxisNumber, 0x80 | 3, 0x0f, 1, 0); // 0x0f는 원점 찾은 후 0으로 무조건, 그 뒤는 원점 Offset 이동 거리


            // 원점 이동
            // ----------
            //if (AxisIndex == MultiMotion.CAM_AXIS_ROTATE)
            //{
                nRet = NMC2.nmc_HomeMove(DeviceManagerS.g_ndevIdA_4,
                    (short)AxisIndex,
                    0x80 | 1,
                    0,
                    DataManager.MotionSettingInfoList[AxisIndex].Home_Offset,
                    0); // 한태희 과정 => 0x80 | 2

            //}
            //else
            //{
            //    nRet = NMC2.nmc_HomeMove(DeviceManagerS.g_ndevIdA_4,
            //      (short)AxisIndex,
            //      0x80 | 2,
            //      0,
            //      DataManager.MotionSettingInfoList[AxisIndex].Home_Offset,
            //      0); // 한태희 과정 => 0x80 | 2
            //}
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
                nRet = NMC2.nmc_GetHomeStatus(DeviceManagerS.g_ndevIdA_4, out HomeFlag);
                if (nRet != KSM_OK)
                {
                    //return nRet;
                }

                if (HomeFlag.nSrchFlag[AxisIndex] == 0 && HomeFlag.nStatusFlag[AxisIndex] == 0)
                {
                    // 위치 초기화
                    // --------------------------------------------------
                    nRet = NMC2.nmc_SetDccOn(DeviceManagerS.g_ndevIdA_4, (short)AxisIndex, 1);

                    System.Threading.Thread.Sleep(100);

                    nRet = NMC2.nmc_SetCmdEncPos(DeviceManagerS.g_ndevIdA_4, (short)AxisIndex, 0.0);

                    System.Threading.Thread.Sleep(100);

                    nRet = NMC2.nmc_SetDccOn(DeviceManagerS.g_ndevIdA_4, (short)AxisIndex, 0);
                    // --------------------------------------------------

                    break;
                }

                //System.Windows.Forms.Application.DoEvents();
            }
            
            return KSM_OK;
        }


#endregion 기본 축 이동 명령어 ...





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
            /*
            if (MultiMotion.InStatus[45] == 0)
            {
                MultiMotion.StopAll();                

                Application.Exit();                

                return KSM_INVALID;
            }
            */

            return nRet;


            
        }


        public static short RiskCheck()
        {
            short nRet = -1;
            int AxisIndex = -1;


            frmPowerDown frm_power_down = new frmPowerDown();

           
            // 1. Ping 체크 ...
            // --------------------------------------------------
            MultiMotion.bPingCheckedA = false;


            for (int i = 0; i < 3; i++)
            {
                // 모션 컨트롤러 A
                // ----------
                nRet = NMC2.nmc_PingCheck(DeviceManagerS.g_ndevIdA_4, 500);
                if (nRet == 0) {
                    MultiMotion.bPingCheckedA = true;
                }

            }

            if (MultiMotion.bPingCheckedA == false)
            {   
                frm_power_down.ShowDialog();

                return KSM_NOTCONNECT;
            }





            // 3. 원점 수행 이력 체크 ...
            // --------------------------------------------------            
            NMC2.NMCHOMEFLAG HomeFlagA;
            
            nRet = NMC2.nmc_GetHomeStatus(DeviceManagerS.g_ndevIdA_4, out HomeFlagA);
            if (nRet == 0)
            {
                if (HomeFlagA.nStatusFlag[MultiMotion.Camera1Adjust] == 3)
                {
                    frm_power_down.ShowDialog();

                    return KSM_HOME_FAIL;
                }


                if (HomeFlagA.nStatusFlag[MultiMotion.Shuttle1Motor] == 3)
                {
                    frm_power_down.ShowDialog();

                    return KSM_HOME_FAIL;
                }

                //if (HomeFlagA.nStatusFlag[MultiMotion.CAM_AXIS_C] == 3)
                //{
                //    frm_power_down.ShowDialog();

                //    return KSM_HOME_FAIL;
                //}
                    

                if (HomeFlagA.nStatusFlag[MultiMotion.RotationMotor] == 3)
                {
                    frm_power_down.ShowDialog();

                    return KSM_HOME_FAIL;
                }
                    

                if (HomeFlagA.nStatusFlag[MultiMotion.Camera2Adjust] == 3)
                {
                    frm_power_down.ShowDialog();

                    return KSM_HOME_FAIL;
                }
                    

                if (HomeFlagA.nStatusFlag[MultiMotion.Lift2Motor] == 3)
                {
                    frm_power_down.ShowDialog();

                    return KSM_HOME_FAIL;
                }

                if (HomeFlagA.nStatusFlag[MultiMotion.Lift1Motor] == 3)
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
                NMC2.nmc_SetAlarmResetOn(DeviceManagerS.g_ndevIdA_4, i, 1);

                CommonUtility.WaitTime(50, true);
            }

            CommonUtility.WaitTime(100, false);

            for (short i = 0; i < 8; i++)
            {
                NMC2.nmc_SetAlarmResetOn(DeviceManagerS.g_ndevIdA_4, i, 0);

                CommonUtility.WaitTime(50, true);
            }

            return KSM_OK;
        }


#endregion 방어 코드 ...



#region 접점 제어 ...


        //public static Automation.BDaq.InstantDiCtrl instantDiCtrl = null;
        //public static Automation.BDaq.InstantDoCtrl instantDoCtrl = null;

        public static short[] OutStatus = new short[64];
        public static short[] InStatus = new short[64];
        public static short[] InStatusNMF = new short[128];
        public static short[] ServoStopInfo = new short[8];
        public static short GetDIOStatus()
        {

            //short nRet = NMF.nmf_DIGet(DeviceManager.g_ndevIdC_44, out MultiMotion.InStatus[0]);


            if (MotionCheck == 2)
            {
                short nRet = NMF.nmf_DIGet(DeviceManagerS.g_ndevIdC_44, InStatusNMF);
                if (nRet != 0)
                {
                    return nRet;
                }

                nRet = NMC2.nmc_GetDIOInput(DeviceManagerS.g_ndevIdA_4, out MultiMotion.InStatus[0]);
                if (nRet != 0)
                {
                    return nRet;
                }

                // nRet = NMF.nmf_DOGet(DeviceManager.g_ndevIdC_44, out MultiMotion.OutStatus[0]);
                nRet = NMC2.nmc_GetDIOOutput(DeviceManagerS.g_ndevIdA_4, out MultiMotion.OutStatus[0]);

                if (nRet != 0)
                {
                    return nRet;
                }
                nRet = NMC2.nmc_GetStopInfo(DeviceManagerS.g_ndevIdA_4, out MultiMotion.ServoStopInfo[0]);


            }

            if (MotionCheck == 1)
            {
                NMF.nmf_DIGet(DeviceManagerS.g_ndevIdC_44, InStatusNMF);
                short nRet = NMC2.nmc_GetDIOInput(DeviceManagerS.g_ndevIdC_44, out MultiMotion.InStatus[0]);
                if (nRet != 0)
                {
                    return nRet;
                }

                // nRet = NMF.nmf_DOGet(DeviceManager.g_ndevIdC_44, out MultiMotion.OutStatus[0]);
                nRet = NMC2.nmc_GetDIOOutput(DeviceManagerS.g_ndevIdC_44, out MultiMotion.OutStatus[0]);

                if (nRet != 0)
                {
                    return nRet;
                }
            }
            return KSM_OK ;

            /*
            // 1. Output Status ...
            // ----------
            for (int i = 0; i < 4; i++)
            {
                string portDataHex = "";
                byte portData = 0;
                ErrorCode err = MultiMotion.instantDoCtrl.Read(i, out portData);
                if (err != ErrorCode.Success)
                {
                    //오류 처리
                    return KSM_INVALID;
                }

                portDataHex = portData.ToString("X2");
                for (int bit = 0; bit < 8; bit++)
                {
                    int isBitOn = ((portData >> bit) & 0x1);
                    int bitindex = (i * 8) + bit;
                    int bitNo = bitindex;


                    MultiMotion.OutStatus[bitindex] = isBitOn;
                }
            }


            // 2. Input Status ...
            // ----------
            for (int i = 0; i < 4; i++)
            {                
                byte portData = 0;
                ErrorCode err = instantDiCtrl.Read(i, out portData);
                if (err != ErrorCode.Success)
                {
                    //오류 처리
                    return KSM_INVALID;
                }

                
                for (int bit = 0; bit < 8; bit++)
                {
                    int isBitOn = ((portData >> bit) & 0x1);
                    int bitindex = (i * 8) + bit;
                    int bitNo = bitindex;

                    MultiMotion.InStatus[bitindex] = isBitOn;
                }
            }

            */

        }
        /*
        public static bool Write_Output(int ioNo, bool data)
        {
            bool ret = false;
            if (ioNo < 0)
                return ret;


            int PortNo = 0;
            int BitNo = ioNo;
            if (ioNo >= 0 && ioNo < 8)
            {
                PortNo = 0;
            }
            else if (ioNo >= 8 && ioNo < 16)
            {
                PortNo = 1;
                BitNo = ioNo - 8;
            }
            else if (ioNo >= 16 && ioNo < 24)
            {
                PortNo = 2;
                BitNo = ioNo - 16;
            }
            else
            {
                PortNo = 3;
                BitNo = ioNo - 24;
            }

            byte portData = 0;
            ErrorCode err = instantDoCtrl.Read(PortNo, out portData);
            if (err != ErrorCode.Success)
            {
                //오류 처리

            }
            else
            {
                byte newport = portData;
                if (data)
                {
                    newport = (byte)(portData | (byte)(0x01 << BitNo));
                }
                else
                {
                    newport = (byte)(portData & ~(byte)(0x01 << BitNo));
                }

                ErrorCode errwrite = instantDoCtrl.Write(PortNo, newport);
                if (errwrite != ErrorCode.Success)
                {
                    //오류 처리

                }
                else
                {
                    ret = true;
                }
            }


            return ret;
        }
        */

      
#endregion 접점 제어 ...
    }
   
}
