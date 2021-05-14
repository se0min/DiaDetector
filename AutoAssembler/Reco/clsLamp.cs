using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

namespace AutoAssembler
{
    class clsLamp
    {
        private SerialPort serialPort = new SerialPort();
        private const int CHANNELMAX = 20;
        private int[] CurLamp = new int[CHANNELMAX];
        private string SendCommDataStr = "";
        private const int BaseChannelAddNum = 48;//48이 1번채널

        public bool Open(int port)
        {
            try
            {
                serialPort.PortName = ("COM" + port.ToString());
                serialPort.BaudRate = 9600;
                serialPort.DataBits = 8;
                serialPort.Parity = System.IO.Ports.Parity.None;
                serialPort.StopBits = System.IO.Ports.StopBits.One;
                serialPort.Open();
                serialPort.DataReceived += new SerialDataReceivedEventHandler(serialPort_DataReceived);
            }
            catch (Exception ex)
            {
                return false;

                //Debug.WriteLine(ex.Message);
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

        public void Close()
        {
            if (serialPort.IsOpen)
            {
                serialPort.Close();
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

        private void SendCommandSetValue(int ChannelNumber, int LampValue)
        {
            if (serialPort.IsOpen)
            {
                string ValueMsg;
                ValueMsg = string.Format("w{0:0000}", LampValue);

                byte[] valueB = Encoding.ASCII.GetBytes(ValueMsg);
                byte[] cmd = new byte[8];
                cmd[0] = 2;
                cmd[1] = (byte)ChannelNumber;
                for (int i = 0; i < 5; i++)
                {
                    cmd[i+2] = valueB[i];
                }
                cmd[7] = 3;
                //Debug.WriteLine(strCmd);
                //byte[] cmd = Encoding.ASCII.GetBytes(strCmd);
                serialPort.Write(cmd, 0, cmd.Length);
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
