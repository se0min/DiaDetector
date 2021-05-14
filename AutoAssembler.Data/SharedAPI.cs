using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace AutoAssembler.Data
{
    public class SharedAPI
    {
        // 구조체 선언
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct NETRESOURCE
        {
            public uint dwScope;
            public uint dwType;
            public uint dwDisplayType;
            public uint dwUsage;
            public string lpLocalName;
            public string lpRemoteName;
            public string lpComment;
            public string lpProvider;
        }

        /*
        // API 함수 선언
        [DllImport("mpr.dll", CharSet = CharSet.Auto)]
        public static extern int WNetUseConnection(
                    IntPtr hwndOwner,
                    [MarshalAs(UnmanagedType.Struct)] ref NETRESOURCE lpNetResource,
                    string lpPassword,
                    string lpUserID,
                    uint dwFlags,
                    StringBuilder lpAccessName,
                    ref int lpBufferSize,
                    out uint lpResult);

        // API 함수 선언 (공유해제)
        [DllImport("mpr.dll", EntryPoint = "WNetCancelConnection2", CharSet = CharSet.Auto)]
        public static extern int WNetCancelConnection2A(string lpName, int dwFlags, int fForce);
        */

        // API 함수 선언
        [DllImport("mpr.dll", CharSet = CharSet.Ansi)]
        public static extern int WNetUseConnection(
                    IntPtr hwndOwner,
                    [MarshalAs(UnmanagedType.Struct)] ref NETRESOURCE lpNetResource,
                    string lpPassword,
                    string lpUserID,
                    uint dwFlags,
                    StringBuilder lpAccessName,
                    ref int lpBufferSize,
                    out uint lpResult);

        // API 함수 선언 (공유해제)
        [DllImport("mpr.dll", EntryPoint = "WNetCancelConnection2", CharSet = CharSet.Ansi)]
        public static extern int WNetCancelConnection2A(string lpName, int dwFlags, int fForce);


        // 공유 연결
        public static int ConnectRemoteServer(string server)
        {
            int capacity = 128;
            uint resultFlags = 0;
            uint flags = 0;
            System.Text.StringBuilder sb = new System.Text.StringBuilder(capacity);

            NETRESOURCE ns = new NETRESOURCE();
            ns.dwType = 1;              // 공유 디스크
            ns.lpLocalName = null;   // 로컬 드라이브 지정하지 않음
            //ns.lpLocalName = @"K:\_공용작업실\JigFree";
            ns.lpRemoteName = server;
            ns.lpProvider = null;

            int result = 0;

            //@ksm.co.kr
            result = WNetUseConnection(IntPtr.Zero, ref ns, "ksm0512//", "hjinkim", flags,
                                        sb, ref capacity, out resultFlags);


            //if (server == @"\\10.144.70.120\d$")
            /*
            if (server == @"\\vcms.ksm.co.kr\_공용작업실\JigFree")
            {
                result = WNetUseConnection(IntPtr.Zero, ref ns, "ksm0512//", "hjinkim", flags,
                                            sb, ref capacity, out resultFlags);
            }
            else
            {

                result = WNetUseConnection(IntPtr.Zero, ref ns, "ksm0512//", "hjinkim", flags,
                                                sb, ref capacity, out resultFlags);
            }
            */


            return result;
        }

        // 공유 해제
        public static void CancelRemoteServer(string server)
        {
            WNetCancelConnection2A(server, 1, 0);
        }
    }
}
