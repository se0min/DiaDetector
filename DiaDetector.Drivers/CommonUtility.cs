using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// ----------

using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;


namespace DiaDetector.Drivers
{
    internal struct LASTINPUTINFO
    {
        public uint cbSize;
        public uint dwTime;
    }

    public class CommonUtility
    {
        [DllImport("User32.dll")]
        public static extern bool LockWorkStation();

        [DllImport("User32.dll")]
        private static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

        [DllImport("Kernel32.dll")]
        private static extern uint GetLastError();

        [DllImport("User32.dll")]
        public static extern bool ShowCursor(bool show);

        [DllImport("kernel32", SetLastError = true)]
        private static extern bool GetSystemTime(out SYSTEMTIME systemTime);
        [DllImport("kernel32", SetLastError = true)]
        private static extern bool SetSystemTime(ref SYSTEMTIME systemTime);

        private struct SYSTEMTIME
        {
            public ushort wYear;
            public ushort wMonth;
            public ushort wDayOfWeek;
            public ushort wDay;
            public ushort wHour;
            public ushort wMinute;
            public ushort wSecond;
            public ushort wMilliseconds;
        }

        public static void SetTime(DateTime setDT)
        {
            SYSTEMTIME st = new SYSTEMTIME();

            DateTime utc = setDT.ToUniversalTime();
            st.wYear = (ushort)utc.Year;
            st.wMonth = (ushort)utc.Month;
            st.wDayOfWeek = (ushort)utc.DayOfWeek;
            st.wDay = (ushort)utc.Day;
            st.wHour = (ushort)utc.Hour;
            st.wMinute = (ushort)utc.Minute;
            st.wSecond = (ushort)utc.Second;
            st.wMilliseconds = (ushort)utc.Millisecond;

            SetSystemTime(ref st);
        }


        static string startupFolder = Application.StartupPath;
        /// <summary>
        /// ex) c:\\startup  뒤쪽에 \\를 붙이지 말것
        /// default = Application.Startup
        /// </summary>
        public static string StartupFolder
        {
            get { return CommonUtility.startupFolder; }
            set { CommonUtility.startupFolder = value; }
        }

        public CommonUtility()
        {
            startupFolder = Application.StartupPath;
        }

        public static uint GetIdleTime()
        {
            try
            {
                LASTINPUTINFO lastInPut = new LASTINPUTINFO();
                lastInPut.cbSize = (uint)Marshal.SizeOf(lastInPut);
                GetLastInputInfo(ref lastInPut);

                uint idletime = ((uint)Environment.TickCount - lastInPut.dwTime);

                return idletime;// -mBaseIdelTime;
            }
            catch (Exception ee)
            {
                //TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
                return 0;
            }
        }

        public static void WaitTime(int delay, bool doevent)
        {
            try
            {
                DateTime start = DateTime.Now;
                while (true)
                {
                    if (doevent) System.Windows.Forms.Application.DoEvents();
                    TimeSpan sp = DateTime.Now - start;
                    if (sp.TotalMilliseconds > delay) break;
                }
            }
            catch (Exception ex)
            {
                //TraceManager.AddLog(string.Format("{0}r\n{1}", ex.StackTrace, ex.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("{0}r\n{1}", ex.StackTrace, ex.Message));
            }
        }

        public static bool IsNumeric(string num)
        {
            try
            {
                int tmp = Convert.ToInt32(num);
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("{0}r\n{1}", ex.StackTrace, ex.Message));
                return false;
            }
        }
    }
}
