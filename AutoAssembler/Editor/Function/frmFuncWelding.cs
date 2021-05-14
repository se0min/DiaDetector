using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

//////////

using AutoAssembler.Data;
using AutoAssembler.Drivers;
using AutoAssembler.Utilities;

namespace AutoAssembler
{
    public partial class frmFuncWelding : Form
    {
        public WorkFuncInfo _WorkFuncInfo;

        private double dFirstPos_X = 0.0;
        private double dFirstPos_Y = 0.0;

        private double dDisValue = 0.0;
        private double dSecondPos_X = 0.0;

        private int laser_unit_status = -1;

        private bool bFLMove = false;
        private bool bHomeReturn = false;

        public frmFuncWelding()
        {
            InitializeComponent();
        }



        private void Initialize()
        {
            // --------------------------------------------------

            txtMetalThick1.Text = DataManager.SelectedModel.dMetalThick1.ToString();
            txtMetalThick2.Text = DataManager.SelectedModel.dMetalThick2.ToString();
            txtCapsulePi.Text = DataManager.SelectedModel.dCapsulePie.ToString();

            MultiMotion.CalcRollingData();
            // --------------------------------------------------


            // 속도 설정 ...
            // --------------------------------------------------
            AxisSpeed = _WorkFuncInfo.AxisSpeed;

            SelectSpeed(AxisSpeed);

            MultiMotion.SetSpeed(AxisSpeed);
            // --------------------------------------------------


            this.serialPort_PCToPC.PortName = "COM" + DeviceManager.WeldingComPort.ToString();
        }

        private void timerAxis_Tick(object sender, EventArgs e)
        {
            MultiMotion.CheckDefense();

            UpdatePos();
        }

        private void UpdatePos()
        {
            MultiMotion.GetCurrentPos();

            // --------------------------------------------------
            double dCalcValue = MultiMotion.dIndex_XPos - MultiMotion.AxisValue[MultiMotion.INDEX_MOVE_M] 
                - (DataManager.SelectedModel.dMetalThick1 + DataManager.SelectedModel.dMetalThick2 - 4.5);

            double dResult = 100.0 - ((dCalcValue - (DataManager.SelectedModel.dSLValue + DataManager.SelectedModel.dWRValue * 2))
                / ((DataManager.SelectedModel.dFLValue - (DataManager.SelectedModel.dSLValue + DataManager.SelectedModel.dWRValue * 2)) * 0.01));            


            txtWeldSolidRate.Text = dResult.ToString();

            txtWeldSolidValue.Text = (MultiMotion.dIndex_XPos - MultiMotion.AxisValue[MultiMotion.INDEX_MOVE_M]).ToString("##.00");
            // --------------------------------------------------
        }

        private void frmFuncWelding_Load(object sender, EventArgs e)
        {
            Initialize();            
            
            // ----------
            timerAxis.Enabled = true;
            timerParsePacket.Enabled = true;
            //timerCommand.Enabled = true;
            // ----------

            if (!serialPort_PCToPC.IsOpen)
            {
                serialPort_PCToPC.Open();
            }

        }

        private void frmFuncWelding_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (serialPort_PCToPC.IsOpen)
            {
                serialPort_PCToPC.Close();
            }

            timerAxis.Enabled = false;

            MultiMotion.StopAll();

            MultiMotion.SetSpeed(MultiMotion.KSM_SPEED_SLOW); // 기본 속도로 변경 ...
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            _WorkFuncInfo.dWeldSolidRate = double.Parse(txtWeldSolidRate.Text);

            if (SelectedRB != null)
            {
                _WorkFuncInfo.AxisSpeed = Convert.ToInt32(SelectedRB.Tag);
            }

            if (bHomeReturn == false)
            {
                if (MessageBox.Show("홈 복귀를 하지 않고, 롤링 화면을 나가겠습니까?", "종료 여부", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    this.DialogResult = DialogResult.OK;
                }
            }
            else
            {
                this.DialogResult = DialogResult.OK;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (bHomeReturn == false)
            {
                MessageBox.Show("화면을 나가기 전에 홈 복귀를 먼저 실행해 주세요.");

                return;
            }

            this.DialogResult = DialogResult.Cancel;
        }

#region Welding ...

        private void btnUnswing_Click(object sender, EventArgs e)
        {
            MultiMotion.Swing(false);
        }

        private void btnRollingDown_Click(object sender, EventArgs e)
        {
            RollingDown();
        }

        private void RollingDown()
        {
            //MultiMotion.StopAll();

            MultiMotion.JogStop(MultiMotion.ROLLING_FIX_1);
            MultiMotion.JogStop(MultiMotion.ROLLING_FIX_2);
            MultiMotion.JogStop(MultiMotion.ROLLING_MOVE_1);
            MultiMotion.JogStop(MultiMotion.ROLLING_MOVE_2);


            MultiMotion.SetSpeed(MultiMotion.KSM_SPEED_MIDIUM);

            MultiMotion.MoveRolling(1.0, false);

            MultiMotion.SetSpeed(this.AxisSpeed);

            /*
            MultiMotion.MoveRolling(1.0, true);

            MultiMotion.HomeMove(MultiMotion.ROLLING_FIX_1, true);  // 고정축 롤링 1
            MultiMotion.HomeMove(MultiMotion.ROLLING_FIX_2, true);  // 고정축 롤링 2
            MultiMotion.HomeMove(MultiMotion.ROLLING_MOVE_1, true); // 이동축 롤링 1
            MultiMotion.HomeMove(MultiMotion.ROLLING_MOVE_2, true); // 이동축 롤링 2
             
            MessageBox.Show("Rolling Down이 완료되었습니다.");
            */
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            MultiMotion.StopAll();
        }

        private void btnWelding_Click(object sender, EventArgs e)
        {
            if (DataManager.SelectedModel.dMetalThick1 < 1.0 || DataManager.SelectedModel.dCapsulePie < 1.0)
            {
                MessageBox.Show("메탈1 두께와 캡슐 파이를 알 수 없습니다. 먼저, 롤링 UI 화면을 실행해 주세요.");

                return;
            }
            

            if (CalcPosition2() == false)
            {
                return;
            }

            List<Byte> rets = SendData(0x00, dFirstPos_X, dFirstPos_Y, dSecondPos_X, 30.0, 3);

            MessageBox.Show("초기 좌표를 전송했습니다.");
            
            return;

            /*
            // 좌표값
            double testX = MultiMotion.dWeldStartBaseX + 6.9;
            double testY = MultiMotion.dWeldStartBaseY - 50.0;
            double dDis = 0.0;

            List<Byte> rets = SendData(0x01, testX, testY, dDis, 80.0, 3);
            */            
        }


#endregion Welding ...


#region Button ...




        private void btnFLMove_Click(object sender, EventArgs e)
        {
            //DeviceManager.PlaySoundM4A("용접이끝났으니제품을꺼내주세요.m4a");

            if (MultiMotion.dRolling100Value > 30.0)
            {
                double dMoveDis = MultiMotion.dIndex_XPos - MultiMotion.dRolling100Value;

                MultiMotion.SetSpeed(MultiMotion.KSM_SPEED_MIDIUM);

                MultiMotion.GantryAxis(MultiMotion.INDEX_MOVE_M, MultiMotion.INDEX_MOVE_S, dMoveDis, false);

                MultiMotion.SetSpeed(this.AxisSpeed);                

                bFLMove = true;

                laser_unit_status = 16;
            }
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            if (bFLMove == false)
            {
                MessageBox.Show("F-L 위치로 먼저 이동해 주세요.");

                return;
            }





            if (MultiMotion.GantryAxisEnable(0, false) == MultiMotion.KSM_OK)
            {
                MultiMotion.SetSpeed(MultiMotion.KSM_SPEED_FAST);

                // ----------
                MultiMotion.HomeMove(MultiMotion.INDEX_FIX_R, false);

                MultiMotion.HomeMove(MultiMotion.INDEX_MOVE_R, false);
                // ----------

                MultiMotion.SetSpeed(this.AxisSpeed);



                bHomeReturn = true;
            }




            if (MultiMotion.GantryAxisEnable(1, true) == MultiMotion.KSM_OK)
            {
                MultiMotion.SetSpeed(MultiMotion.KSM_SPEED_FAST);




                MultiMotion.MoveAxis(MultiMotion.INDEX_MOVE_M, 1.0, true);                

                MultiMotion.HomeMove(MultiMotion.INDEX_MOVE_M, true);

                MultiMotion.SetSpeed(this.AxisSpeed);

                

                bHomeReturn = true;
            }

            //MessageBox.Show("INDEX Home 복귀가 완료되었습니다.");
            DeviceManager.PlaySoundM4A("작업이 완료 되었습니다.m4a");
        }

        private void btnMJog_Index_MouseDown(object sender, MouseEventArgs e)
        {
            if (MultiMotion.GantryAxisEnable(0, true) == MultiMotion.KSM_OK)
            {
                //MultiMotion.StepMove(MultiMotion.INDEX_FIX_R, 1, false);
                MultiMotion.JogMove(MultiMotion.INDEX_FIX_R, 1);
            }
        }

        private void btnMJog_Index_MouseUp(object sender, MouseEventArgs e)
        {
            if (MultiMotion.GantryAxisEnable(0, true) == MultiMotion.KSM_OK)
            {
                MultiMotion.JogStop(MultiMotion.INDEX_FIX_R);
            }
        }

        private void btnPJog_Index_MouseDown(object sender, MouseEventArgs e)
        {
            if (MultiMotion.GantryAxisEnable(0, true) == MultiMotion.KSM_OK)
            {
                //MultiMotion.StepMove(MultiMotion.INDEX_FIX_R, 0, false);
                MultiMotion.JogMove(MultiMotion.INDEX_FIX_R, 0);
            }
        }

        private void btnPJog_Index_MouseUp(object sender, MouseEventArgs e)
        {
            if (MultiMotion.GantryAxisEnable(0, true) == MultiMotion.KSM_OK)
            {
                MultiMotion.JogStop(MultiMotion.INDEX_FIX_R);
            }
        }

#endregion Button ...





#region 속도 ...

        public RadioButton SelectedRB;
        public int AxisSpeed = 1;

        public void SelectSpeed(int speed)
        {
            radioBtnSSlow.Checked = false;
            radioBtnSlow.Checked = false;
            //radioBtnMidium.Checked = false;
            //radioBtnFast.Checked = false;


            // ----------
            switch (speed)
            {
                case MultiMotion.KSM_SPEED_SSLOW:
                    SelectedRB = radioBtnSSlow;
                    break;
                case MultiMotion.KSM_SPEED_SLOW:
                    SelectedRB = radioBtnSlow;
                    break;
                case MultiMotion.KSM_SPEED_MIDIUM:
                    break;
                case MultiMotion.KSM_SPEED_FAST:
                    break;
            }
            // ----------


            SelectedRB.Checked = true;
        }

        private void radioBtn_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;

            if (rb == null)
            {
                return;
            }

            if (rb.Checked)
            {
                SelectedRB = rb;

                AxisSpeed = Convert.ToInt32(SelectedRB.Tag);

                MultiMotion.SetSpeed(AxisSpeed);
            }
        }

#endregion 속도 ...


#region 자동 EF 좌표 전송 ...

        bool bReadyCmdCheck = false;
        int ReadyCmdCount = 0;

        private void btn70JogMinus_MouseDown(object sender, MouseEventArgs e)
        {
            if (checkLaserUnitPos() == false)
            {
                MultiMotion.StopAll();

                MessageBox.Show("레이저 유닛 위치를 확인해 주세요.");

                return;
            }


            if (MultiMotion.GantryAxisEnable(1, true) == MultiMotion.KSM_OK)
            {
                MultiMotion.StepMove(MultiMotion.INDEX_MOVE_M, 1, false);

                // 좌표 중복 전송 방지 ...
                // ----------
                timerCommand.Enabled = false;
                ReadyCmdCount = 0;
                // ----------
            }
        }

        private void btn70JogMinus_MouseUp(object sender, MouseEventArgs e)
        {
            if (MultiMotion.GantryAxisEnable(1, true) == MultiMotion.KSM_OK)
            {
                //MultiMotion.JogStop(MultiMotion.INDEX_MOVE_M);

                // 좌표 중복 전송 방지 ...
                // ----------
                timerCommand.Enabled = true;
                bReadyCmdCheck = false;
                ReadyCmdCount = 0;
                // ----------
            }

            // 방어 코드 => 명령어 전송 전에 모든 축 정지
            // --------------------------------------------------
            MultiMotion.StopAll();
            // --------------------------------------------------
        }

        private void btn70JogPlus_MouseDown(object sender, MouseEventArgs e)
        {
            if (checkLaserUnitPos() == false)
            {
                MultiMotion.StopAll();

                MessageBox.Show("레이저 유닛 위치를 확인해 주세요.");

                return;
            }

            if (MultiMotion.GantryAxisEnable(1, true) == MultiMotion.KSM_OK)
            {
                MultiMotion.StepMove(MultiMotion.INDEX_MOVE_M, 0, false);

                // 좌표 중복 전송 방지 ...
                // ----------
                timerCommand.Enabled = false;

                ReadyCmdCount = 0;
                // ----------
            }
        }

        private void btn70JogPlus_MouseUp(object sender, MouseEventArgs e)
        {
            if (MultiMotion.GantryAxisEnable(1, true) == MultiMotion.KSM_OK)
            {
                //MultiMotion.JogStop(MultiMotion.INDEX_MOVE_M);

                // 좌표 중복 전송 방지 ...
                // ----------
                timerCommand.Enabled = true;

                bReadyCmdCheck = false;

                ReadyCmdCount = 0;
                // ----------
            }

            // 방어 코드 => 명령어 전송 전에 모든 축 정지
            // --------------------------------------------------
            //MultiMotion.StopAll();
            // --------------------------------------------------
        }

        private void timerCommand_Tick(object sender, EventArgs e)
        {
            if (ReadyCmdCount > 10)
            {
                bReadyCmdCheck = true;

                timerCommand.Enabled = false;

                ReadyCmdCount = 0;
            }
            else
            {
                ReadyCmdCount++;
            }

            if (bReadyCmdCheck == true)
            {
                timerCommand.Enabled = false;

                bReadyCmdCheck = false;

                ReadyCmdCount = 0;


                // 방어 코드 => 명령어 전송 전에 모든 축 정지
                // --------------------------------------------------
                //MultiMotion.StopAll();
                // --------------------------------------------------


                this.SendAxis();
            }            
        }

        private void SendAxis()
        {
            // --------------------------------------------------
            if (DataManager.SelectedModel.dMetalThick1 < 1.0 || DataManager.SelectedModel.dCapsulePie < 1.0)
            {
                MessageBox.Show("메탈1 두께와 캡슐 파이를 알 수 없습니다. 먼저, 롤링 UI 화면을 실행해 주세요.");

                return;
            }


            if (CalcPosition2() == false)
            {
                return;
            }

            List<Byte> rets = SendData(0x01, dFirstPos_X, this.dFirstPos_Y, dSecondPos_X, 30.0, 3);

            MessageBox.Show("업데이트 된 좌표를 전송했습니다.");
            // --------------------------------------------------
        }

        private void timerParsePacket_Tick(object sender, EventArgs e)
        {
            this.ParsePacket();
        }

        private bool checkLaserUnitPos()
        {
            switch (laser_unit_status)
            {
                case -1:    // 용접 명령어 전송 후
                    break;
                case 16:    // 교체 위치
                    break;
                case 17:     // 가접 1 위치
                case 19:     // 용접 1 위치
                    break;
                case 18:     // 가접 2 위치
                case 20:     // 용접 2 위치
                    return false;
                    break;
                case 32:    // 좌표 동기화
                    break;
                default:
                    return false;
                    break;
            }

            return true;
        }

        private bool CalcPosition()
        {
            // 폐기
            return false;

            // 1. 첫 번째 위치 : 고정축 메탈 두깨
            // 2. 두 번째 위치 : X좌표 + 롤링 위치 - 이동축 메탈 두께

            double dTempValue = 0.0;

            this.dFirstPos_X = MultiMotion.dWeldStartBaseX + DataManager.SelectedModel.dMetalThick1;
            this.dFirstPos_Y = MultiMotion.dWeldStartBaseZ + (DataManager.SelectedModel.dCapsulePie / 2);

            if (double.TryParse(txtWeldSolidValue.Text, out dTempValue) == true)
            {
                this.dDisValue = dTempValue - DataManager.SelectedModel.dMetalThick1 - DataManager.SelectedModel.dMetalThick2 + 4.4 + MultiMotion.dIndex_XOffset;
            }
            else
            {
                return false;
            }

            return true;
        }

        private bool CalcPosition2()
        {
            /*
            20160811 측정치 => X축 교차 지점
            
            IX = 1298.74 + M
            LX = 461.713 - M
            
            1494.58
            266.629
             
            1502.2
            259.009
            */

            double dTempValue = 0.0;

            this.dFirstPos_X = MultiMotion.dWeldStartBaseX + DataManager.SelectedModel.dMetalThick1;
            this.dFirstPos_Y = MultiMotion.dWeldStartBaseZ - (DataManager.SelectedModel.dCapsulePie / 2);

            //LX = 1298.74 + 461.713 - IX
            //this.dSecondPos_X = 1761.554 - MultiMotion.AxisValue[MultiMotion.INDEX_MOVE_M];



            this.dSecondPos_X = 1761.554 - MultiMotion.AxisValue[MultiMotion.INDEX_MOVE_M]
                - DataManager.SelectedModel.dMetalThick2 + MultiMotion.dIndex_XOffset;

            // 이전 소스
            /*
            this.dSecondPos_X = 1503.057 - MultiMotion.AxisValue[MultiMotion.INDEX_MOVE_M] +
                MultiMotion.dWeldStartBaseX + MultiMotion.dIndex_XOffset;
            */

            return true;
        }


#endregion 자동 EF 좌표 전송 ...


        private void Test()
        {
            // 용접 프로그램 내용
            // => dWeldStartBaseX = 264.3770 - 6.9;     // 감마 프로그램 쪽에 있어야 함.
            // => dWeldStartBaseY = 330.2082 - 47.64;   // 감마 프로그램 쪽에 있어야 함.

            /*if (MultiMotion.CheckDefense() == false) {
            }*/

            /*
            if (MultiMotion.checkAxisEnd(MultiMotion.INDEX_MOVE_M) == true && bCheckStart == true)
            {
                bCheckStart = false;
            }
            */
        }

        private void frmFuncWelding_Shown(object sender, EventArgs e)
        {            
            //if (MultiMotion.bUpdatedSecondPoint == true)
            {
                // --------------------------------------------------
                if (DataManager.SelectedModel.dMetalThick1 < 1.0 || DataManager.SelectedModel.dCapsulePie < 1.0)
                {
                    MessageBox.Show("메탈1 두께와 캡슐 파이를 알 수 없습니다. 먼저, 롤링 UI 화면을 실행해 주세요.");

                    return;
                }


                if (CalcPosition2() == false)
                {
                    return;
                }

                //List<Byte> rets = SendData(0x04, 0.0, this.dFirstPos_Y, dSecondPos_X, 30.0, 3);
                List<Byte> rets = SendData(0x04, this.dFirstPos_X, this.dFirstPos_Y, dSecondPos_X, 30.0, 3);

                
                // --------------------------------------------------

            }            
        }



        private void btnAuto_Click(object sender, EventArgs e)
        {
            //List<Byte> rets = SendData(0x05, dFirstPos_X, 0.0, dSecondPos_X, 30.0, 3);
        }

        private void btnVBlockDown_Click(object sender, EventArgs e)
        {
            //MultiMotion.StopAll();
            MultiMotion.JogStop(MultiMotion.VBLOCK_Z);

            MultiMotion.SetSpeed(MultiMotion.KSM_SPEED_SLOW);

            MultiMotion.MoveAxis(MultiMotion.VBLOCK_Z, 1.0, false);

            MultiMotion.SetSpeed(this.AxisSpeed);

        }


        #region 감마 통신 ...

        List<Byte> _rcvData = new List<Byte>();
        bool _rcvDone = false;
        bool isDataReceivedProc = false;

        public List<Byte> SendData(byte cmd, double dXValue, double dYValue, double dDis, double dLaserOutput, int SpotCount)
        {
            // 0x00 : 고정축 Spot 용접
            // 0x01 : 이동축 Spot 용접
            // 0x02 : 고정축 EF 용접
            // 0x03 : 이동축 EF 용접

            try
            {
                if (serialPort_PCToPC.IsOpen == false)
                {
                    return new List<Byte>();
                }


                _rcvDone = false;
                _rcvData.Clear();


                // 프로토콜 => 0x00(X축), 0x01(Y축), 0x02(Z축), 0x03(R축), 0x10(기타 명령어 시작) ...
                // --------------------------------------------------                

                byte[] sendBuf = new byte[48];
                sendBuf[0] = 0x02;
                sendBuf[1] = cmd;

                // X좌표값
                // ----------
                char[] data_array = dXValue.ToString("00000.0000").ToCharArray();
                for (int i = 2; i < 12; i++)
                {
                    sendBuf[i] = Convert.ToByte(data_array[i - 2]);
                }

                // Y좌표값
                // ----------
                data_array = dYValue.ToString("00000.0000").ToCharArray();
                for (int i = 12; i < 22; i++)
                {
                    sendBuf[i] = Convert.ToByte(data_array[i - 12]);
                }

                // 거리값
                // ----------
                data_array = dDis.ToString("00000.0000").ToCharArray();
                for (int i = 22; i < 32; i++)
                {
                    sendBuf[i] = Convert.ToByte(data_array[i - 22]);
                }

                // Laser 출력값
                // ----------
                data_array = dLaserOutput.ToString("00000.0000").ToCharArray();
                for (int i = 32; i < 42; i++)
                {
                    sendBuf[i] = Convert.ToByte(data_array[i - 32]);
                }

                // SPOT 개수
                // ----------
                data_array = SpotCount.ToString("0000").ToCharArray();
                for (int i = 42; i < 46; i++)
                {
                    sendBuf[i] = Convert.ToByte(data_array[i - 42]);
                }

                // 체크썸 계산
                // ----------
                byte Sum = 0;

                for (int k = 0; k < 46; k++)
                {
                    Sum += sendBuf[k];
                }

                byte chkCode = Convert.ToByte(Sum & (byte)0xff);

                sendBuf[46] = chkCode;  // 체크썸
                sendBuf[47] = 0x03;
                // --------------------------------------------------


                serialPort_PCToPC.Write(sendBuf, 0, sendBuf.Length);


                DateTime startDate = DateTime.Now;
                while (_rcvDone == false)
                {
                    System.Windows.Forms.Application.DoEvents();
                    TimeSpan ts = DateTime.Now - startDate;
                    if (ts.TotalMilliseconds > 1000)
                    {
                        break;
                    }
                    System.Threading.Thread.Sleep(10);
                }

                return _rcvData;
            }
            catch (Exception ee)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
                return new List<Byte>();
            }

        }

        private void serialPort_PCToPC_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            try
            {
                if (isDataReceivedProc)
                    return;

                isDataReceivedProc = true;

                if (e.EventType == System.IO.Ports.SerialData.Chars)
                {
                    _rcvData.Clear();

                    List<List<Byte>> _recv_packets = new List<List<Byte>>();
                    DateTime startDate = DateTime.Now;
                    while (true)
                    {
                        Byte rd = Convert.ToByte(serialPort_PCToPC.ReadByte());
                        _rcvData.Add(rd);


                        TimeSpan sp = DateTime.Now - startDate;
                        if (sp.TotalMilliseconds > 3000)
                        {
                            //TraceManager.AddLog("#### COMMAND RECV TIME OUT 1000 msec !!!!!");
                            _rcvData.Clear();
                            break;
                        }

                        //수신된 데이터 체크
                        _recv_packets = GetSplitePacketData(_rcvData);
                        if (_recv_packets.Count > 0)
                        {
                            break;
                        }
                    }

                    if (_recv_packets.Count > 0)
                    {
                        _rcvData = _recv_packets[0];
                        _rcvDone = true;
                    }

                    serialPort_PCToPC.DiscardInBuffer();
                    _rcvDone = true;
                }

                isDataReceivedProc = false;
            }
            catch (Exception ee)
            {
                //TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
                isDataReceivedProc = false;
            }
        }

        private void ParsePacket()
        {
            // 0. 
            // ----------
            if (this._rcvDone == true)
            {
                _rcvDone = false;
            }
            else
            {
                return;
            }


            // 1. 체크썸 확인 ...
            // ----------
            if (_rcvData.Count == 24)
            {
                byte Sum = 0;

                for (int k = 0; k < 22; k++)
                {
                    Sum += _rcvData[k];
                }

                byte chkCode = Convert.ToByte(Sum & (byte)0xff);

                if (chkCode == _rcvData[22])
                {

                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }


            // 레이저 유닛 위치 정보
            // --------------------------------------------------
            laser_unit_status = Convert.ToInt32(_rcvData[1]);

            switch (laser_unit_status)
            {
                case -1:    // 용접 명령어 전송 후
                case 16:    // 교체 위치
                case 17:     // 가접 1 위치
                case 18:     // 용접 1 위치
                case 19:     // 가접 2 위치
                case 20:     // 용접 2 위치
                    return;
                    break;
                case 32:    // 좌표 동기화
                    break;
                case 48:    // 롤링 다운
                    {
                        RollingDown();

                        return;
                    }
                    break;
            }
            // --------------------------------------------------





            // 2. 정보 추출 ...
            // ----------
            string strXValue = "";
            string strYValue = "";


            for (int i = 2; i < 12; i++)
                strXValue += (char)_rcvData[i];

            for (int i = 12; i < 22; i++)
                strYValue += (char)_rcvData[i];


            // 3. 정보 추출
            // ----------
            double dTempValue = 0.0;
            double dStart_X = 0.0;
            double dStart_Z = 0.0;

            if (double.TryParse(strXValue, out dTempValue) == true)
                dStart_X = dTempValue;
            else
                return;

            if (double.TryParse(strYValue, out dTempValue) == true)
                dStart_Z = dTempValue;
            else
                return;


            // 좌표 동기화 ...
            // --------------------------------------------------
            if (laser_unit_status == 32)
            {
                MultiMotion.dWeldStartBaseX = dStart_X - DataManager.SelectedModel.dMetalThick1;
                MultiMotion.dWeldStartBaseZ = dStart_Z + DataManager.SelectedModel.dCapsulePie / 2.0; // 좌표 동기화시에 Z값을 건드리는 것은 위험하다

                DeviceManager.Write();

                MessageBox.Show("감마 프로그램에서 시작 좌표가 업데이트 되었습니다.");
            }
            // --------------------------------------------------



            // 5. 
            // ----------
            _rcvData.Clear();
        }

        List<List<Byte>> GetSplitePacketData(List<Byte> _recvData)
        {
            try
            {
                List<List<Byte>> __ret_packs = new List<List<Byte>>();

                while (_recvData.Count > 0)
                {
                    if (_recvData.Count < 24) //최소한
                    {
                        break;
                    }

                    //STX나 EXT가 존재하지 않으면 ERROR
                    if ((!_recvData.Contains(0x02)) || (!_recvData.Contains(0x03)))
                    {
                        break;
                    }
                    //프로토콜 내에 ETX가 존재할때의 처리
                    //시작이 STX가 아닐떄의 처리

                    int idxS = _recvData.IndexOf(0x02); //가장 가까운 STX를 구함
                    _recvData.RemoveRange(0, idxS); //STX이전의 데이터는 삭제한다..
                    idxS = 0; //0로 만든다

                    int idxE = _rcvData.IndexOf(0x03);

                    //int LENGTH = _recvData[idxS + 1]; //LENGTH가
                    //if (_recvData.Count < (LENGTH + 2))
                    //{
                    //   break;
                    //}
                    //int idxE = idxS + 1 + LENGTH + 1; //STX + LENGTH + ETX까지의 갯수
                    int LENGTH = (idxE - idxS) + 1;
                    List<Byte> _packet = _recvData.GetRange(idxS, LENGTH);

                    _recvData.RemoveRange(idxS, LENGTH); //읽어온 패킷은 삭제한다.

                    __ret_packs.Add(_packet);
                }

                return __ret_packs;
            }
            catch (Exception ex)
            {
                return null; // 프로토콜 에러
            }
        }


        #endregion 감마 통신 ...

        private void btn5StepMove_Click(object sender, EventArgs e)
        {
            if (checkLaserUnitPos() == false)
            {
                //MultiMotion.StopAll();

                if (MultiMotion.GantryAxisEnable(1, true) == MultiMotion.KSM_OK)
                {
                    MultiMotion.JogStop(MultiMotion.INDEX_MOVE_M);
                }



                MessageBox.Show("레이저 유닛 위치를 확인해 주세요.");

                return;
            }

            


            if (MultiMotion.GantryAxisEnable(1, true) == MultiMotion.KSM_OK)
            {

                MultiMotion.SetSpeed(MultiMotion.KSM_SPEED_FAST);

                MultiMotion.StepMove(MultiMotion.INDEX_MOVE_M, 1, true);
                MultiMotion.StepMove(MultiMotion.INDEX_MOVE_M, 1, false);

                MultiMotion.SetSpeed(this.AxisSpeed);


            }
        }

        private void btnPJog_Index_Click(object sender, EventArgs e)
        {

        }

    }
}

