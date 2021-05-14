using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using DiaDetector.Drivers;
namespace DiaDetector
{
    class clsLamp
    {
        MainFrame frmmain;
        private SerialPort serialPort = new SerialPort();
        private SerialPort serialPort2 = new SerialPort();
        private SerialPort serialPort3 = new SerialPort();
        private const int CHANNELMAX = 20;
        private int[] CurLamp = new int[CHANNELMAX];
        private string SendCommDataStr = "";
        private const int BaseChannelAddNum = 48;//48이 1번채널

        public bool Open(string Com)
        {
            try
            {
                if (MultiMotion.MotionCheck == 1)
                { 
                serialPort.PortName = Com;
                serialPort.BaudRate = 115200;
                serialPort.DataBits = 8;
                serialPort.Parity = System.IO.Ports.Parity.None;
                serialPort.StopBits = System.IO.Ports.StopBits.One;
                serialPort.Open();
                serialPort.DataReceived += new SerialDataReceivedEventHandler(serialPort_DataReceived);
                }
            }
            catch (Exception ex)
            {
                return false;

                //Debug.WriteLine(ex.Message);
            }
            

            return true;
        }
        public bool Open2(string Com)
        {
            try
            {
                if (MultiMotion.MotionCheck == 1)
                {
                    serialPort2.PortName = Com;
                    serialPort2.BaudRate = 115200;
                    serialPort2.DataBits = 8;
                    serialPort2.Parity = System.IO.Ports.Parity.None;
                    serialPort2.StopBits = System.IO.Ports.StopBits.One;
                    serialPort2.Open();
                    serialPort2.DataReceived += new SerialDataReceivedEventHandler(serialPort_DataReceived2);
                }
            }
            catch (Exception ex)
            {
                return false;

                //Debug.WriteLine(ex.Message);
            }

            return true;
        }

        public bool Open3(string Com)
        {
            try
            {
                if (MultiMotion.MotionCheck == 2)
                {
                    serialPort3.PortName = Com;
                    serialPort3.BaudRate = 9600;
                    serialPort3.DataBits = 8;
                    serialPort3.Parity = System.IO.Ports.Parity.None;
                    serialPort3.StopBits = System.IO.Ports.StopBits.One;
                    serialPort3.Open();
                    serialPort3.DataReceived += serialPort3_DataReceived;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

     

        public bool CommSts()
        {
            bool CommConnFlag;
            if (serialPort.IsOpen)
            {
                CommConnFlag = true;
            }
            else
            {
                CommConnFlag = false;
            }
            return CommConnFlag;
        }
        public bool CommSts2()
        {
            bool CommConnFlag;
            if (serialPort2.IsOpen)
            {
                CommConnFlag = true;
            }
            else
            {
                CommConnFlag = false;
            }
            return CommConnFlag;
        }


        public void Close()
        {
            if (serialPort.IsOpen)
            {
                serialPort.Close();
            }
            if (serialPort2.IsOpen)
            {
                serialPort2.Close();
            }
        }

        public string GetSendCommString()
        {
            return SendCommDataStr;
        }

        public int GetLamp(int ChannelNumber)
        {
            return CurLamp[ChannelNumber];
        }

        public void SetLamp(int ChannelNumber, int LampValue)
        {
            int ChannelFinal;
            int SendLampValue;
            ChannelFinal = BaseChannelAddNum + ChannelNumber;
            if (LampValue < 0)
            {
                SendLampValue = 0;
            }
            else if (LampValue > 1023)
            {
                SendLampValue = 1023;
            }
            else
            {
                SendLampValue = LampValue;
            }
            SendCommandSetValue(ChannelFinal, SendLampValue);
        }

        public void ONLamp(int ChannelNumber)
        {
            int ChannelFinal;
            ChannelFinal = BaseChannelAddNum + ChannelNumber;
            SendCommandONOFF(ChannelFinal, true);
        }

        public void OFFLamp(int ChannelNumber)
        {
            int ChannelFinal;
            ChannelFinal = BaseChannelAddNum + ChannelNumber;
            SendCommandONOFF(ChannelFinal, false);
        }

        private void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            byte[] rsv = new byte[64];
            int cnt = serialPort.Read(rsv, 0, 64);
            string msg = Encoding.ASCII.GetString(rsv, 0, cnt);
        }


        private void serialPort_DataReceived2(object sender, SerialDataReceivedEventArgs e)
        {
            byte[] rsv = new byte[64];
            int cnt = serialPort.Read(rsv, 0, 64);
            string msg = Encoding.ASCII.GetString(rsv, 0, cnt);
        }
        void serialPort3_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            byte[] rsv = new byte[64];
            int cnt = serialPort.Read(rsv, 0, 64);
            string msg = Encoding.ASCII.GetString(rsv, 0, cnt);
        }

        public void SendCommandSetValue(int ChannelNumber, int LampValue) //조명 컨트롤러2개
        {
            if (serialPort.IsOpen || serialPort2.IsOpen)
            {
                string ValueMsg;
                ValueMsg = string.Format("{0:00}", LampValue);

                byte[] valueB = Encoding.ASCII.GetBytes(ValueMsg);
                byte[] cmd = new byte[8];
                cmd[0] = 0xF1;
                cmd[1] = 0xF7;
                cmd[2] = 0x80;
                cmd[3] = 0x00;
                cmd[4] = 0x01;
                cmd[5] = (byte)LampValue;

                byte XORRes = cmd[0];
                for (int i = 0; i < 6;i++ )
                {
                    if(i == 0)
                    {
                        XORRes = cmd[0];
                    }
                    else
                    {
                        XORRes = (byte)(XORRes ^ cmd[i]);
                    }
                    
                }
                cmd[6] = XORRes;
       
            

            //    cmd[1] = (byte)ChannelNumber;

                //for (int i = 0; i < 5; i++)
                //{
                //    cmd[i+1] = valueB[i];
                //}
                //cmd[7] = 3;
                //Debug.WriteLine(strCmd);
                //byte[] cmd = Encoding.ASCII.GetBytes(strCmd);
                serialPort.Write(cmd, 0, cmd.Length);
                serialPort2.Write(cmd, 0, cmd.Length);
                SendCommDataStr = "";
                for (int i = 0; i < 8; i++)
                {
                    SendCommDataStr += cmd[i].ToString("X") + " ";
                }
                valueB = null;
                cmd = null;
                ValueMsg = null;
            }
        }

        public void SendCommandSetValue1(int ChannelNumber, int LampValue)  //조명 컨트롤러1개
        {
            if (serialPort3.IsOpen)
            {
                string ValueMsg;
                ValueMsg = string.Format(ChannelNumber + "w{0:0000}", LampValue);

                byte[] valueB = Encoding.ASCII.GetBytes(ValueMsg);
                byte[] cmd = new byte[8];
                cmd[0] = 0x02;
                //cmd[1] = (byte)ChannelNumber;
                for (int i = 0; i < 6; i++)
                {
                    cmd[i + 1] = valueB[i];
                }
                cmd[7] = 0x03;
                //Debug.WriteLine(strCmd);
                //byte[] cmd = Encoding.ASCII.GetBytes(strCmd);
                serialPort3.Write(cmd, 0, cmd.Length);
                SendCommDataStr = "";
                for (int i = 0; i < 8; i++)
                {
                    SendCommDataStr += cmd[i].ToString("X") + " ";
                }
                valueB = null;
                cmd = null;
                ValueMsg = null;
            }
        }


        private void SendCommandONOFF(int ChannelNumber, bool OnOffFlag)
        {
            string ValueMsg;
            if (serialPort.IsOpen)
            {
                if (OnOffFlag == true)
                {
                    ValueMsg = "o";
                }
                else
                {
                    ValueMsg = "f";
                }
                byte[] valueB = Encoding.ASCII.GetBytes(ValueMsg);
                byte[] cmd = new byte[4];

                cmd[0] = 2;
                cmd[1] = (byte)ChannelNumber;
                cmd[2] = valueB[0];
                cmd[3] = 3;

                serialPort.Write(cmd, 0, cmd.Length);
                SendCommDataStr = "";
                for (int i = 0; i < 4; i++)
                {
                    SendCommDataStr += cmd[i].ToString("X") + " ";
                }

                valueB = null;
                cmd = null;
                ValueMsg = null;
            }
        }
    }
}
