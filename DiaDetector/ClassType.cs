using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DiaDetector.Drivers;
using System.IO;

namespace DiaDetector
{
    class ClassType
    {
        public const int MAX_CAM = 2;   //카메라의 수
        public const int SM = 10;  //모션컨트롤러 1개
        public const int BM = 11;  //모션컨트롤러 2개
        public static double Counting = 0;  //카운터
        public static int CountingCheck = 0;  //카운터누적
        public static double Counting1 = 0;  //카운터
        public static int Total = 0;  //카운터누적
        public static int Totalok = 0;  //카운터누적
        public static int Totalng = 0;
        public static int TotalTime = 0;
        public const int JobStart = 110;
        public const int JobRotation45 = 111;
        public const int JobRotation0 = 112;
        public const int JobRotation90 = 113;
        public const int JobLiftDown = 114;
        public const int JobLiftUp = 115;
        public const int JobCamera1stResult = 116;
        public const int JobLiftDown2 = 117;
        public const int JobLiftUp2 = 118;
        public const int JobCamera2stResult = 119;
        public const int JobRotation0_2 = 120;
        public const int JobNG = 121;
        public const int JobNG2 = 122;
        public const int JobShuttle1 = 123;
        public const int JobShuttle2 = 124;
        public const int JobShuttle3 = 125;
        public const int WORK_START = 201;
        public const int WORK_WORKING = 202;
        public const int WORK_WORKING2 = 203;
        public const int WORK_WORKING3 = 204;
        public const int WORK_WORKING4 = 2044;
        public const int WORK_END = 205;
        public const int JobNGSub = 131;
        public const int JobNGSub2 = 132;
        public const int JobUP1 = 133;
        public const int JobUP2 = 134;
        public const int JobNGA2 = 135;
        public const int JobNGb2 = 136;
        public const int JobNGA3 = 137;
        public const int JobNGb3 = 138;
        public const int WORK_STARTNSHOT = 190;
        public const int WORK_WORKINGSHOT = 191;
        public const int WORK_WORKING2SHOT = 192;
        public const int WORK_WORKING3SHOT = 193;
        public const int WORK_ENDSHOT = 194;
        public const int WORK_ENDNGESHOT = 194;
        //영상 OK NG 관련
        public static int Os1Result = 0;
        public static int Os2Result = 0;
        public static int Os1State = 148;
        public static int Os2State = 149;
        public const int Os1OK = 150;
        public const int Os1NG = 151;
        public const int Os2OK = 152;
        public const int Os2NG = 153;
        public const int Os1END = 154;
        public const int Os2END = 155;

        public const int JobLiftD1 = 140;
        public const int JobLiftD2 = 141;
        public const int JobLiftD3 = 142;
        public const int JobLiftD4 = 143;


        public const int JobLiftStart = 301;
        public const int JobLiftStartUp1 = 302;
        public const int JobLiftStartUp2 = 303;
        public const int JobLiftLiftDown = 304;
        public const int LiftWORK_START = 310;
        public const int LiftWORK_WORKING = 311;
        public const int LiftWORK_END = 312;

        //압력센서

        public static int Airsensor = 0;



        public static int Led1 = 1023;  //조명컨트롤러 1개값
        public static int Led2 = 0;  //조명컨트롤러 2개 값
        public static int JobDateNo = 0;  //잡넘버
        public static int JobResult = 100;
        public static int JobResultState = 200;
        public static int JobLiftResult = 300;
        public static int JobLiftResultState = 400;
        public static int MakeStartTime = 0;
        public static int MakeStartTime1 = 0;
        public static int MakeOutTime = 0;


        //가이드 Move 관련
        public static int JobGuide = 500;
        public static int JobGuideState = 600;
        public static int JobGuideFinal = 5050;
        public const int JobGuideInStart = 501;
        public const int JobGuideInEnd = 502;
        public const int JobGuideOutStart = 503;
        public const int JobGuideOuEnd = 504;
        public const int JobGuideMoveStart = 505;
        public const int JobGuideMoveEnd = 506;
        public const int GuideWORK_START = 601;
        public const int GuideWORK_WORKING = 602;
        public const int GuideWORK_WORKING2 = 603;
        public const int GuideWORK_WORKING3 = 604;
        public const int GuideWORK_END = 605;
        public const int GuideReadyStart = 606;
        public const int GuideStepStart = 607;
        public const int CameraAStart = 610; //카메라A초점
        public const int CameraBStart = 611; //카메라B초점
        //가이드 AirUp 관련
        public static int AirUp = 800;
        public static int AirUpState = 900;
        public static int AirUp1 = 5000;
        public const int AirUp2 = 5001;
        public const int AirUp3 = 5002;
        public const int AirUpWORK_START = 801;
        public const int AirUpWORK_WORKING = 802;
        public const int AirUpWORK_WORKING2 = 803;
        public const int AirUpWORK_WORKING3 = 804;
        public const int AirUpWORK_END = 805;
 
        //카메라 그랩 관련
        public const int Camera1ShotOK = 700;
        public const int Camera2ShotOK = 800;
        public const int Camera1ShotNG = 5050;
        public const int Camera2ShotNG = 5001;
 
     


        public const double eps = 0.1;  //절대값
        public const double eps2 = 1;  //절대값
        public const double guide0 = 0; // 가이드 원점
        public const double guide1 = 13.38; //가이드 1단계
        public const double guide2 = 18.49; //가이드 1단계
        public const double guide970 = 1410; // 가이드 끝점
        public const double sinmove0 = 0; // 시작위치
        public const double sinmove1 = 1; // 시작위치

        public const double sinmove4 = 1.7694; // 시작위치
        public const double sinmove49 = 46.5714; // NG위치
        public const double standby = 65.000; // 대기위치
        public const double sinmove94 = 91.5732; //90도위치
        public const double sinmoveLift1 = 4.0; //90도위치
        public const double sinmoveLift2 = 4.0; //90도위치
        public const double sinmoveShuttle1 = 180; //셔틀1 이동위치
        public const double sinmoveShuttle2 = 180; //셔틀2 이동위치
        public const double sinmoveShuttle1end = 1409;  //셔틀1 종료지점
        public const double sinmoveShuttle2end = 1409;  //셔틀2 종료지점
        public const double LiftLimit1 = 649.65; //리프트1 리미트 위치
        public const double LiftLimit2 = 649.95;//리프트2 리미트 위치
        public const double LiftLimitUp= 0.3;//리프트2 리미트 위치
        public const double LiftLimitUp1 = 1;//리프트2 리미트 위치
        public static double guideeps0 = 0; // 가이드 원점
        public static double guideeps1A = 0; // 가이드 준비 단계
        public static double guideeps1B = 0; // 가이드 준비 단계
        public static double guideeps50= 0; // 가이드 단계
        public static double guideeps100 = 0; // 가이드 끝점1번셔틀
        public static double guideeps200 = 0;// 가이드 끝점2번셔틀
        public static double sineps0 = 00.0; //0도 절대값 비교
        public static double sineps45 = 00.00; //45도 절대값 비교
        public static double sineps90 = 00.00; //90도 절대값 비교
        public static double sinepsstan = 00.00; //대기 절대값 비교
      


        public static double sinepsShuttleA1Result = 0.0; //셔틀1 단계별 값 저장
        public static double sinepsShuttleA2Result = 0.0; //셔틀2 단계별 값 저장
        public static double sinepsShuttleA1Eps = 0.0; //셔틀1 단계별 절대값 비교
        public static double sinepsShuttleA2Eps = 0.0; //셔틀2 단계별 절대값 비교
        public static double sinepsShuttleA1LimitEps = 1418.309; //셔틀1 리미트 플러스
        public static double sinepsShuttleA2LimitEps = 1419.1225; //셔틀2 리미트 플러스
        public static double sinepsShuttleA1Limitvalue = 0; //셔틀1 리미트 절대값
        public static double sinepsShuttleA2Limitvalue = 0; //셔틀2 리미트 절대값
        public static double sinepsShuttleA1 = 0.0; //90도 절대값 비교
        public static double sinepsShuttleA2 = 0.0; //90도 절대값 비교
        public static double sinepsShuttleA3 = 0.0; //90도 절대값 비교
        public static double sinepsShuttleA4 = 0.0; //90도 절대값 비교
        public static double sinepsShuttleA5 = 0.0; //90도 절대값 비교
        public static double sinepsShuttleA6 = 0.0; //90도 절대값 비교
        public static double sinepsShuttleB1 = 0.0; //90도 절대값 비교
        public static double sinepsShuttleB2 = 0.0; //90도 절대값 비교
        public static double sinepsShuttleB3 = 0.0; //90도 절대값 비교
        public static double sinepsShuttleB4 = 0.0; //90도 절대값 비교
        public static double sinepsShuttleB5 = 0.0; //90도 절대값 비교
        public static double sinepsShuttleB6 = 0.0; //90도 절대값 비교

        public static double Pie = 0.0; //90도 절대값 비교

        public const short sinmoves0 = 0; // 조그용 0
        public const short sinmoves1 = 1; // 조그용 1

        public static bool Stop = true;  //정지
        public static bool ModelSelect = false;
        public static bool AutoManual = false;  //TRUE 오토-FALSE 수동
        public static bool Run_Flag = false; // TRUE 실행중-FALSE 정지중
        public static bool Run_Flag1 = false; // GuideMoveTRUE 실행중-FALSE 정지중
        public static bool LiftCheck = false;  //TRUE 내려감 -FALSE 올라감
        public static bool ShuttleCheck = false;  //TRUE 내려감 -FALSE 올라감
        public static bool Camera1result = false; //카몌라 1번 결과
        public static bool Camera2result = false; //카메라 2번 결과
        public static bool CameraMode = false; //카메라 인식 자동 수동 확인  //true 자동  false 수동
        public static bool CameraJobBack = false;
        public static bool LiftModeLimit1 = false; //리프트1 리미트 체크  //true 리미트온  false 리미트 오프
        public static bool LiftModeLimit2 = false; //리프트1 리미트 체크   //true 리미트온  false 리미트 오프
        public static bool ShuttleReady = false; //리프트1 리미트 체크   //true 리미트온  false 리미트 오프
        public static bool SQLP = false; //SQL +
        public static bool SQLM = false; //SQL -


        //재시작 관련
        public static bool Restart; // 리스타트 순서 정하기  //true 0도작업  false 90도작업
        public static bool RestartCheck; // 리스타트 정하기  //true 재시작 작업가능  false 재시작 작업 불가능

        public static bool ShuttleSensor = false;  //정지

        public const string TestWaitMsg = "검사중입니다.\n검사종료후 사용하시기 바랍니다.";
        public const string TestWaitMsg2 = "MANUAL 모드입니다..\nAUTO 모드를 선택 후 사용하시기 바랍니다.";
        public const string TestWaitMsg3 = "가이드 이동중입니다.\n종료후 사용하시기 바랍니다.";
        public const string TestWaitMsg4 = "AUTO 모드입니다..\nMANUAL 모드를 선택 후 사용하시기 바랍니다.";
        public const string TestWaitMsg5 = "재시작 작업 가능한 상태가 아닙니다.";
        public const string TestWaitMsg6 = "셔틀 이동을 완료해주세요";
        public const string TestWaitMsg7 = "리프트 이동중입니다";
        public const string TestWaitMsg8 = "모델을 선택해주세요";
        public const string TestWaitMsg9 = "실린더가 다운 상태입니다.";
        public static short DPSB;  //모션컨트롤러 번호
        public static string DateDay = "";   //현재날짜
        public static string DateDayS = "";   //현재날짜
        public static string DateHHmm = "";   //현재날짜 비교해서 0시되면 작업번호 리셋
        public static string DateSec = "";   //현재날짜

        public static string _startUPPath = @"c:\KSM\DiaDetector\Log\";  //로그파일 경로
       // C:\KSM\Source\DiaDetector\DiaDetector\Resources

        public static string _startUPPath2 = "\\";
        public static string CameraF = "불량이미지";
        public static string Result = "\\검사결과.txt"; //검사결과
        public static string Servo = "\\서보알람.txt";  //서보알람
        public static string Jobstate = "\\시퀀스확인.txt";  //서보알람
        public static string JobResultstate = "\\작업결과.txt";  //서보알람
        public static string CameraLog1 = "1번Camera";
        public static string CameraLog2 = "2번Camera";
        public static string Bmp = ".Bmp";
        public static string Jump = "  ";
        public static string Lgr = "[";
        public static string Rgr = "]";
        public static string OK = "OK";
        public static string NG = "NG";
        public static string JobState = "작업번호:";
        public static string ResultTest = "결과 값:";
        public static string JobDate = "\\작업번호.txt";  //서보알람
        public const string JobDateNone = "";  //잡넘버
        public static string[] JobData;  //작업번호 불러와서 들어가는곳
        public static string JobDateString = "\\작업번호.txt";  //서보알람
        public static string Forder = "";
        public static string DirecName = "";
        public static string CameraValus = "두께 값:";
        public static string PointValus = "포인트:";



        public static void JobNoCreate()
        {
            DirectoryInfo di11 = new DirectoryInfo(_startUPPath + DateDay + _startUPPath2);
            DirectoryInfo di1 = new DirectoryInfo(_startUPPath + DateDay + _startUPPath2 + CameraF); 
            if (di11.Exists == false)
            {
                di11.Create();
                JobNoDateWhite("00000");
            }
            if (di1.Exists == false)
            {
                di1.Create();
            }
        }
        public static void DateTimeWhite( string white ,string Type)
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(_startUPPath + DateDay);
                DirectoryInfo di1 = new DirectoryInfo(_startUPPath + DateDay + _startUPPath2 + CameraF); 

                if (di.Exists == false)
                {
                    di.Create();
                }
                if (di1.Exists == false)
                {
                    di1.Create();
                }
                if (Type == Result)
                {
                    FileStream fs = new FileStream(_startUPPath + DateDay + _startUPPath2 + Result, FileMode.Append);

                    StreamWriter sw = new StreamWriter(fs, Encoding.Default);
                    string rowValueString = white;

                    sw.WriteLine(rowValueString);
                    sw.Close();
                    fs.Close();
                }
                if (Type == Servo)
                {
                    FileStream fs = new FileStream(_startUPPath + DateDay + _startUPPath2 + Servo, FileMode.Append);

                    StreamWriter sw = new StreamWriter(fs, Encoding.Default);
                    string rowValueString = white;

                    sw.WriteLine(rowValueString);
                    sw.Close();
                    fs.Close();
                }
                if(Type ==JobResultstate)
                {
                    FileStream fs = new FileStream(_startUPPath + DateDay + _startUPPath2 + JobResultstate, FileMode.Append);

                    StreamWriter sw = new StreamWriter(fs, Encoding.Default);
                    string rowValueString = white;

                    sw.WriteLine(rowValueString);
                    sw.Close();
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
            }
        }
        public static void JobNoDateWhite(string SpeedMode = JobDateNone)
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(_startUPPath + DateDay);
                if (di.Exists == false)
                {
                    di.Create();
                    JobNoDateWhite("00000");

                }
                if (SpeedMode != JobDateNone)
                {
                    FileStream fs = new FileStream(_startUPPath + DateDay + _startUPPath2 + JobDate, FileMode.OpenOrCreate);

                    StreamWriter sw = new StreamWriter(fs, Encoding.Default);
                    string rowValueString = SpeedMode;

                    sw.WriteLine(rowValueString);
                    sw.Close();
                    fs.Close();
                }
                JobData = System.IO.File.ReadAllLines(_startUPPath + DateDay + _startUPPath2 + JobDate, System.Text.Encoding.Default);
            }

            catch (Exception ex)
            {
            }
        }


      
    }
    public  struct MotionValue
    {
        public double Cam1Motor;
        public double Cam2Motor;
        public double LiftMotorLeft;
        public double LiftMotorRight;
        public double ShuttleMotorLeft;
        public double ShuttleMotorRight;
        public double RotationMotor;
    }
}
