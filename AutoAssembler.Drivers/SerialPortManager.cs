using System;
using System.Collections.Generic;
using System.IO.Ports;
using AutoAssembler.Utilities;

namespace AutoAssembler.Drivers
{
    class SerialPortManager
    {
        public const byte STX = 0xE0;
        public const byte ETX = 0xF0;
        private static SerialPort _serialPort = null;

        public static bool Initialize()
        {
            try
            {
                //_serialPort = new SerialPort(string.Format("COM{0}", DataManager._portInfo._locker_port), 57600, Parity.None, 8, StopBits.One);
                _serialPort = new SerialPort(_serialPort.PortName, 57600, Parity.None, 8, StopBits.One);
                _serialPort.ReceivedBytesThreshold = 1;
                _serialPort.DataReceived += new SerialDataReceivedEventHandler(_serialPort_DataReceived);
                return true;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
                return false;
            }
        }

        public static void SetComPort(int port)
        {
            try
            {
                _serialPort.PortName = string.Format("COM{0}", port);
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
            }

        }

        public static bool PortOpen()
        {
            try
            {
                /*
                if (!DataManager._portInfo._locker_debug)
                    if (!_serialPort.IsOpen)
                        _serialPort.Open();
                */
                return true;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
                return false;
            }
        }

        public static bool IsPortOpen
        {
            get
            {
                //if (DataManager._portInfo._locker_debug) return true;

                return _serialPort.IsOpen;
            }
        }

        public static void PortClose()
        {
            try
            {
                if (_serialPort.IsOpen)
                    _serialPort.Close();
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
            }
        }

        static List<int> _rcvData = new List<int>();
        static bool snddta = false;
        public static List<int> SendData(byte cmd, byte data1, byte data2)
        {
            try
            {
                TraceManager.AddLog(string.Format("함: [{0}] - SendData: [{1:X}]", Convert.ToInt32(data1), Convert.ToInt32(cmd)));

                if (_serialPort.IsOpen == false)
                    return new List<int>();

                _rcvDone = false;
                _rcvData.Clear();
                snddta = true;

                byte chkCode = Convert.ToByte(((byte)0xE0 + cmd + data1 + data2) & (byte)0xff);
                byte[] sendBuf = new byte[6];
                sendBuf[0] = 0xE0;
                sendBuf[1] = cmd;
                sendBuf[2] = data1;
                sendBuf[3] = data2;
                sendBuf[4] = chkCode; //checksum
                sendBuf[5] = 0xF0;
                _serialPort.Write(sendBuf, 0, sendBuf.Length);

                WriteLog(">>", sendBuf);

                DateTime startDate = DateTime.Now;
                while (_rcvDone == false)
                {
                    System.Windows.Forms.Application.DoEvents();
                    TimeSpan ts = DateTime.Now - startDate;
                    if (ts.TotalMilliseconds > 1000)
                    {
                        TraceManager.AddLog("#### WAIT  TIME OUT 1000 msec !!!!!");

                        break;
                    }
                    System.Threading.Thread.Sleep(10);
                }

                return _rcvData;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
                return new List<int>();
            }
        }

        static void WriteLog(string prifix, byte[] byData)
        {
            string log = prifix;
            //Console.Write(prifix);
            for (int i = 0; i < byData.Length; i++)
            {
                //Console.Write(string.Format("{0:X2}", byData[i]));
                log += string.Format("{0:X2}", byData[i]);
                if (i != (byData.Length - 1)) log += ",";// Console.Write(",");
            }
            log += "";
            //Console.WriteLine("");
            TraceManager.AddLog(log);
        }

        static void WriteLog(string prifix, int[] byInt)
        {
            List<byte> bydata = new List<byte>();
            for (int i = 0; i < byInt.Length; i++)
                bydata.Add((byte)byInt[i]);
            WriteLog(prifix, bydata.ToArray());
        }

        public delegate void DataReceivedHandler(List<int> rcvData);
        public static event DataReceivedHandler OnDataReceived;

        //데이터수신
        static bool _rcvDone = false;
        static bool isDataReceivedProc = false;
        static void _serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                if (isDataReceivedProc) return;
                isDataReceivedProc = true;

                if (e.EventType == SerialData.Chars)
                {
                    _rcvData.Clear();
                    //bool start = false;

                    List<List<int>> _recv_packets = new List<List<int>>();
                    DateTime startDate = DateTime.Now;
                    while (true)
                    {
                        int rd = _serialPort.ReadByte();
                        _rcvData.Add(rd);

                        //if (rd.Equals(0xE0)) start = true;
                        //else if (rd.Equals(0xF0))
                        //{
                        //    _rcvData.Add(rd);
                        //    break;
                        //}

                        //if (start) _rcvData.Add(rd);

                        TimeSpan sp = DateTime.Now - startDate;
                        if (sp.TotalMilliseconds > 1000)
                        {
                            TraceManager.AddLog("#### COMMAND RECV TIME OUT 1000 msec !!!!!");
                            _rcvData.Clear();
                            break;
                        }

                        //수신된 데이터 체크
                        _recv_packets = GetSplitePacketData(_rcvData);
                        if (_recv_packets.Count > 0)
                            break;
                    }

                    //  C1 -> C2          EVENT
                    //STX c d1 d2 c1 EXT STX c d1 de c1 ETX
                    foreach (List<int> packet in _recv_packets)
                    {
                        WriteLog("<<", packet.ToArray());
                    }

                    if (_recv_packets.Count > 0)
                    {
                        List<int> event_packet = GetEventData(_recv_packets);
                        if (event_packet != null)
                        {

                            if (OnDataReceived != null) OnDataReceived(event_packet);
                            _recv_packets.Remove(event_packet);
                        }
                    }

                    if (_recv_packets.Count > 0)
                    {
                        _rcvData = _recv_packets[0];
                        _rcvDone = true;
                    }

                    //_rcvData.Clear();
                    //수신된 데이터가 2개일수있다.
                    //전송하지 않고 수신된 데이터는 EVENT이다.
                    if (!snddta)
                    {
                        if (OnDataReceived != null)
                            OnDataReceived(_rcvData);
                    }

                    snddta = false;
                    _serialPort.DiscardInBuffer();
                    _rcvDone = true;
                }

                isDataReceivedProc = false;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
                isDataReceivedProc = false;
            }

        }

        /// <summary>
        /// Event데이터추출
        /// </summary>
        /// <param name="recv_packets"></param>
        /// <returns></returns>
        static List<int> GetEventData(List<List<int>> recv_packets)
        {
            foreach (List<int> packet in recv_packets)
            {
                byte command = (byte)packet[1];
                if (command == 0xD2) //관리자버튼
                {
                    return packet;
                }
            }
            return null;
        }

        static List<List<int>> GetSplitePacketData(List<int> _recvData)
        {
            try
            {
                List<List<int>> __ret_packs = new List<List<int>>();


                while (_recvData.Count > 0)
                {
                    //BYTE#1 
                    //최소한의 길이 
                    //1+1+1+2+2+1+STX+ETX

                    //STX
                    //command
                    //data1
                    //data2
                    //check
                    //ETX

                    if (_recvData.Count < 6) //최소한
                    {
                        break;
                    }

                    //STX나 EXT가 존재하지 않으면 ERROR
                    if ((!_recvData.Contains(STX)) || (!_recvData.Contains(ETX)))
                    {
                        break;
                    }
                    //프로토콜 내에 ETX가 존재할때의 처리
                    //시작이 STX가 아닐떄의 처리

                    int idxS = _recvData.IndexOf(STX); //가장 가까운 STX를 구함
                    _recvData.RemoveRange(0, idxS); //STX이전의 데이터는 삭제한다..
                    idxS = 0; //0로 만든다

                    int idxE = _rcvData.IndexOf(ETX);


                    //int LENGTH = _recvData[idxS + 1]; //LENGTH가
                    //if (_recvData.Count < (LENGTH + 2))
                    //{
                    //   break;
                    //}
                    //int idxE = idxS + 1 + LENGTH + 1; //STX + LENGTH + ETX까지의 갯수
                    int LENGTH = (idxE - idxS) + 1;
                    List<int> _packet = _recvData.GetRange(idxS, LENGTH);

                    _recvData.RemoveRange(idxS, LENGTH); //읽어온 패킷은 삭제한다.

                    __ret_packs.Add(_packet);

                }


                //STX (1byte)
                //LENGTH (1byte)
                //ProtocolID (1byte)
                //Source (2byte)
                //Dest (2byte)
                //DATA-FTN (1byte)

                //F-DATA (0byte)

                //LRC(1byte)

                //ETX (1byte)


                return __ret_packs;
            }
            catch (Exception ex)
            {

                //프로토콜 에러
                return null;
            }
        }
    }
}
