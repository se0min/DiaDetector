/**
  **************************************************************************************************
  * @file       NMF.cs
  * @author     PAIX Lab.
  * @version
  * @date       2020-01-16
  * @brief      Network Multi Function ��ġ �������� �Դϴ�.
  * @note       C#
  * @copyright  2019. PAIX Co., Ltd. All rights reserved.
  * @see        http://www.paix.co.kr/
  **************************************************************************************************
  * @note       2019-06-26  Initial release
  *             2019-07-29  �ּ� ���� ����
  *             2019-09-09  ��� �߰� ����
  *             2019-11-27  ��ȯ�� �߰� �� ��� ����
  *             2020-01-16  �Լ��� �� �ּ� ����
  **************************************************************************************************
  */

using System.Runtime.InteropServices;     // DLL support

namespace PAIX_NMF_DEV
{
    public class NMF
    {
        /**
          * @brief  NMF ���ǰ�
          */
        public const short  MAX_DEV         = 255;  /*!< ��Ʈ��ũ �ִ� ��ġ ���� */
        public const short  MAX_BRD         = 8;    /*!< �ִ� ���� ���� */
        public const short  EXT_IN_PINS     = 8;    /*!< External �Է� �� ���� */
        public const short  BRD_VS_DIO_PINS = 16;   /*!< DIO������ �� ���� */
        public const short  BRD_VS_AO_PINS  = 8;    /*!< AO������ �� ���� */
        public const short  BRD_VS_AI_PINS  = 8;    /*!< AI������ �� ���� */
        public const short  BRD_VS_AO_GROUP = 2;    /*!< AO������ �׷� ���� */
        public const short  MAX_DIO_PINS    = (MAX_BRD*BRD_VS_DIO_PINS);    /*!< �ִ� DIO �ɼ� */
        public const short  MAX_AIO_PINS    = (MAX_BRD*BRD_VS_AI_PINS );    /*!< �ִ� AIO �ɼ� */

        public const int    AI_MAX_SRATE            = (100000);     /*!< �ִ� ���ø� ���ļ�(100KHz) */
        public const int    AI_MEM_BLK_VS_DATAS     = 2048;         /*!< AI �޸� ���� ���� 2048��(1���� 4Byte) */
        public const int    AI_MEM_BLOCKS           = 64;           /*!< AI �޸� �� �� ���� 65�� */
        public const int    AI_MEM_MAX_DATAS        = (AI_MEM_BLK_VS_DATAS*(AI_MEM_BLOCKS+1));      /*!< AI �޸� �ִ� ������ ���� 133120=2048*65(1���� ä�� �޸�) */

        public const int    COUNTER_MTIME_MAX_DATAS = 1024;        /*!< Count �ð����� ������ �ִ� ���� */

        /**************************************************************************************************/
        /**
          * @brief  �Լ� ��ȯ��
          */
        public enum Enmf_FUNC_RESULT : short
        {
            nmf_R_OK                        =  0,       /*!< ���� */
            nmf_R_NOT_CONNECT               = -1,       /*!< ��ġ�� ������ ������ ��� */
            nmf_R_SOCKET_ERR                = -2,       /*!< ���� ���� */
            nmf_R_UNKOWN                    = -3,       /*!< �������� �ʴ� �Լ� ȣ�� */
            nmf_R_INVALID                   = -4,       /*!< �Լ� ���ڰ��� �����߻� */
            nmf_R_SYNTAX_ERR                = -5,       /*!< �Լ� ȣ�� ��, ���� ���� �߻� */
            nmf_R_CONN_ERR                  = -6,       /*!< ����Ǿ� ���� ���� */
            nmf_R_INVALID_NMF               = -7,       /*!< ��ȿ���� ���� NMF��ġ��ȣ */
            nmf_R_FUNC_NOT_ENOUGH           = -8,       /*!< �Լ��� ������ ���̸�ŭ ����/�����Ͽ��� �ϳ� ������ ���(��:���� ����� ������ ���� �����Ͱ� ���ϴ� ���̺��� ������ ���) */
            nmf_R_NOT_RESPONSE              = -9,       /*!< �Լ� ȣ�� ��, ������ ���� ��� */
            nmf_R_CMDNO_ERR                 = -10,      /*!< �Լ� ȣ�� ��, �ĺ��ڵ忡 ���� �߻� */
            nmf_R_NOT_EXISTS                = -11,      /*!< ��Ʈ��ũ ��ġ�� �ĺ����� �ʴ� ���, ��ȭ���̳� ���� ���¸� Ȯ�� */
            nmf_R_ICMP_LOAD_FAIL            = -12,      /*!< ICMP.DLL �ε� ����, nmf_PingCheck ȣ��� �߻�. PC�� DLL������ Ȯ�� */
            nmf_R_FILE_LOAD_FAIL            = -13,      /*!< F/W file �ε� ���� */
            nmf_R_SYNCHRONIZE               = -14,      /*!< ����ȭ ����(���ؽ�,���������� ������) */
            nmf_R_SOCKET_TX_TMO             = -15,      /*!< ���� ������ ������ Time-out */
            nmf_R_SOCKET_RX_TMO             = -16,      /*!< ���� ������ ������ Time-out */
            nmf_R_BUF_OVERFLOW              = -17,      /*!< ���� Bufferũ�⸦ �ʰ�(ex.���Ź��� �ʰ�) */
            nmf_R_MCU_FAIL                  = -18,      /*!< MCU���ۻ��� ���� */
            nmf_R_FUNC_FAIL                 = -19,      /*!< �Լ����࿡ ������ ��� */
            nmf_R_HW_PART_NONE              = -20,      /*!< �����Ϸ��� �ϴ� ����� H/W��ġ�� ������ ���°�� ��)CPU�𵨿��� Count ��� ���� */

            nmf_R_BRD_INVALID               = -100,     /*!< �Լ����� ������ ���尡 ���ų� ������ �ʰ��� ��� ��)DO���尡 2���ε� 3�� ���忡 1Pin�� ON��� �ϴ޽� */
            nmf_R_BRD_MISMATCH_DO           = -101,     /*!< �Լ����� ������ ���尡 ���ų� DO�� �ƴ� ��� ��)DI, AO, AI ���忡 DO����� ���޵� */
            nmf_R_BRD_MISMATCH_DI           = -102,     /*!< �Լ����� ������ ���尡 ���ų� DI�� �ƴ� ��� ��)AO, AI, DO ���忡 DI����� ���޵� */
            nmf_R_BRD_MISMATCH_AI           = -103,     /*!< �Լ����� ������ ���尡 ���ų� AI�� �ƴ� ��� ��)DO, DI, AO ���忡 AI����� ���޵� */
            nmf_R_BRD_MISMATCH_AO           = -104,     /*!< �Լ����� ������ ���尡 ���ų� AO�� �ƴ� ��� ��)DO, DI, AI ���忡 AO����� ���޵� */
            nmf_R_BRD_INVALID_PIN           = -105,     /*!< �ش� ������ Pin ������ �ʰ� */

            nmf_R_AI_SRATE_ERR              = -200,     /*!< AI ���ø� ���� ��, ���ø� �ӵ� ���� ���� �ʰ� (0~ 100000) */
            nmf_R_AI_INVALID_RANGE          = -201,     /*!< AI �Է� ���� Range ���� ���� �ʰ� (0 ~ 1) */
            nmf_R_AI_MEM_BLOCK_RANGE_ERR    = -202,     /*!< AI �ܺ� �޸� ���� �� �ִ� Block �ʰ� �ܺ� �޸� ���� */

            nmf_R_AO_RANGE_OVER             = -300,     /*!< AO���� ��� ���� Range�� �Ѿ ��� ��) ��5V Range�� 6V�� ���� �� ���) */

            nmf_R_ARG_RNG_OVER              = -1001,    /*!< ������ �Լ��� ���ڰ� ���� ������ �ʰ��� ���
                                            ~ -1500,         ������ ������� �����ڵ尡 ������. ��) 3��° ���� ���� �� (-1003) */
            nmf_R_ARG_ERR                   = -1501,    /*!< ������ �Լ��� ���� ������ �ִ� ���
                                            ~ -1999,         ������ ������� �����ڵ尡 ������. ��) 3��° ���� ���� �� (-1503) */

            // ��⿡���� �����Ǵ� �����ڵ�=================================================================================
            nmf_R_FW_ARG_RNG_OVER           = -20001,   /*!< ������ �Լ��� �����ϱ����� ���۵Ǵ� �������ݿ� ���ڰ� ���� ������ �ʰ��� ���
                                            ~ -20500,        ������ ������� �����ڵ尡 ������. ��) 3��° ���� ���� �� (-20003) */
            nmf_R_FW_ARG_ERR                = -20501,   /*!< ������ �Լ��� �����ϱ����� ���۵Ǵ� �������ݿ� ���� ������ �ִ� ���
                                            ~ -20999,        ������ ������� �����ڵ尡 ������. ��) 3��° ���� ���� �� (-20503) */
        };
        //------------------------------------------------------------------------------

        /**
          * @brief  ���� ������ ��ġ(NMF)���� ����
          */
        [StructLayout(LayoutKind.Sequential, Pack=1)]
        public struct TNMF_COMPO
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=4)]
            public short[]  nIP;                /*!< IP�ּ� */
            public short    nModelPart;         /*!< �� Part ���� (0=NMF, 1=UDIO) �����Ǵ� ��(NMF),��(UDIO)�� ���� */
            public short    nCntDIBrd;          /*!< DI ���� ����(0~7) */
            public short    nCntDOBrd;          /*!< DO ���� ����(0~7) */
            public short    nCntAIBrd;          /*!< AI ���� ����(0~7) */
            public short    nCntAOBrd;          /*!< AO ���� ����(0~7) */
            public short    nCntAIOBrd;         /*!< AIO ���� ����(0~7) */
            public short    nTotalCntBrd;       /*!< ����� ��ü ���� ���� (0~7) */
            public short    nReserved;          /*!< ���� */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=MAX_BRD)]
            public short[]  nType;              /*!< ���� ���� (0=����, 1=D0, 2=DI, 3=AI, 4=AO, 5=AIO) */
        };

        /**
          * @brief  DIO���庰 ���� ����
          */
        [StructLayout(LayoutKind.Sequential, Pack=1)]
        public struct TNMF_BRD_DIO_STS
        {
            public short    nID;                /*!< ���� ID (0~7) -1�� ��� ������ ����Ÿ�� �Ǵ� ���� ���� */
            public short    nReserved;          /*!< ���� */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=BRD_VS_DIO_PINS)]
            public short[]  nPinStatus;         /*!< DI : 0=�Է�OFF, 1=�Է�ON
                                                     DO : 0=���OFF, 1=���ON */
        };

        /**
          * @brief  AI ���ø� ���� ����
          */
        [StructLayout(LayoutKind.Sequential, Pack=1)]
        public struct TNMF_BRD_AI_SMPL
        {
            public short    nID;                /*!< ���� ID (0~7) -1�� ��� ������ ����Ÿ�� �Ǵ� ���� ���� */
            public short    nReserved;          /*!< ���� */
            public short    nStatus;            /*!< ���ø� ���� (0=�̽���, 1=Ʈ���� �Է� �����, 2=���� ��, 3=Read�� ���ø� ��� ��, 4=����� ����, 5=�޸� Full ����) */
            public short    nTrgType;           /*!< ���ø� Ʈ���� Ÿ�� (0=���� ����, 1=Trg Rising, 2=Trg Falling, 3=Trg Both) */
            public short    nExtPin;            /*!< ���ø��� Ʈ���Ÿ� ����ϴ� ��� External Pin ��ȣ (0 ~ 3) */
            public short    nRange;             /*!< AI ������ �Է� ���� (0=��5V, 1=��10V) */
            public int      lRate;              /*!< AD ���ø� �ֱ� (0 ~ 100000Hz) */
            public short    nMemFullOvW;        /*!< �޸� Overwrite����(0=Ovewrite����(���ø� ����), 1=Overwrite(���ø� ��������) */
            public short    nMemFullFlag;       /*!< �޸� Full Flag (0=Full X, 1=Full O) */
            public int      lMemCntData;        /*!< �����ͷ� ä���� �ִ� �޸� ���� (�ִ� 133120��) */
            public int      lMemCntEmpty;       /*!< ��� �ִ� �޸� ���� (�ִ� 133120��) */
            public int      lRespSize;          /*!< ���� ������ ���� (�ִ� 133120��) */
        };

        /**
          * @brief  AI���� ���� ����
          */
        [StructLayout(LayoutKind.Sequential, Pack=1)]
        public struct TNMF_BRD_AI_STS
        {
            public short    nID;                /*!< ���� ID (0~7) -1�� ��� ������ ����Ÿ�� �Ǵ� ���� ���� */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=BRD_VS_AI_PINS)]
            public short[]  nStatus;            /*!< ���ø� ���� (0=�̽���, 1=Ʈ���� �Է� �����, 2=���� ��, 3=Read�� ���ø� ��� ��, 4=����� ����, 5=�޸� Full ����) */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=BRD_VS_AI_PINS)]
            public short[]  nTrgType;           /*!< ���ø� Ʈ���� Ÿ�� (0=���� ����, 1=Trg Rising, 2=Trg Falling, 3=Trg Both) */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=BRD_VS_AI_PINS)]
            public short[]  nExtPin;            /*!< ���ø��� Ʈ���Ÿ� ����ϴ� ��� External Pin ��ȣ (0 ~ 3) */

            public short    nRange;             /*!< AI ������ �Է� ���� (0=��5V, 1=��10V) */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=BRD_VS_AI_PINS)]
            public int[]    lRate;              /*!< AD ���ø� �ֱ� (0 ~ 100000Hz) */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=BRD_VS_AI_PINS)]
            public short[]  nMemFullOvW;        /*!< �޸� Overwrite����(0=Ovewrite����(���ø� ����), 1=Overwrite(���ø� ��������) */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=BRD_VS_AI_PINS)]
            public short[]  nMemFullFlag;       /*!< �޸� Full Flag (0=Full X, 1=Full O) */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=BRD_VS_AI_PINS)]
            public short[]  nMemExists;         /*!< �޸��� ���� (0=������ ����, 1=������ ����) */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=BRD_VS_AI_PINS)]
            public int[]    lADValue;           /*!< ������ �Է� ������ ���� Pin�� �Էµ� ������ ������ �� (-32768 ~ 32767) */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=BRD_VS_AI_PINS)]
            public double[] dVolt;              /*!< Pin�� �Էµ� ���� �� */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=BRD_VS_AI_PINS)]
            public double[] dCurrent;           /*!< Pin�� �Էµ� ���� �� */
        };

        /**
          * @brief  AO���� ���� ����
          */
        [StructLayout(LayoutKind.Sequential, Pack=1)]
        public struct TNMF_BRD_AO_STS
        {
            short   nID;                        /*!< ���� ID (0~7) -1�� ��� �ٸ� ����Ÿ�� �Ǵ� ���� ���� */
            public short    nReserved;          /*!< ���� */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=BRD_VS_AO_PINS)]
            public short[]  nRange;             /*!< AO ������ �� Pin�� ��¹���
                                                    (������ : 0=0~5V, 1=0~10V, 2=0~10.86V, 3=��5V, 4=��10V, 5=��10.86V) */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=BRD_VS_AO_PINS)]
            public int[]    lDAValue;           /*!< ��������(nRange)�� ���� �ֱ� ��� ������ ������ ��
                                                     (0,1,2 = 0~65535, 3,4,5 = -32768 ~ 32767 ) */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=BRD_VS_AO_PINS)]
            public double[] dVolt;              /*!< ������ ���а� */
        };

        /**
          * @brief  External Pin ����
          */
        [StructLayout(LayoutKind.Sequential, Pack=1)]
        public struct TNMF_EXT_STS
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=EXT_IN_PINS)]
            public short[]  nStatus;            /*!< External Pin�� �Է� ���� */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=EXT_IN_PINS)]
            public short[]  nCountEnable;       /*!< External Pin�� Count ��� ��� ���� */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=EXT_IN_PINS)]
            public uint[]   dwCountValue;       /*!< External Pin�� Count ��� ����, Count(Ƚ��) */
            public ulong    ddwCountTime;       /*!< External Pin�� Count ���۽�, �ð�(us) */
        };

        /**
          * @brief  External Pin Count ���� ����
          */
        [StructLayout(LayoutKind.Sequential, Pack=1)]
        public struct TNMF_EXTC_INFO
        {
            public ulong    ddwCountTime;       /*!< Count ���۽ð�(us) */
            public short    nReserved0;         /*!< ����0 */
            public short    nReserved1;         /*!< ����1 */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=EXT_IN_PINS)]
            public short[]  nStatus;            /*!< Count�� �ش� External Pin �Է»��� */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=EXT_IN_PINS)]
            public short[]  nEdge;              /*!< ���� ��ȣ Edge(0=Rising, 1=Falling, 2=Both) */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=EXT_IN_PINS)]
            public short[]  nMTimeMode;         /*!< �ð� ���� ����(0=���� ����, 1=����) */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=EXT_IN_PINS)]
            public short[]  nEnable;            /*!< Count ��� ���� */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=EXT_IN_PINS)]
            public short[]  nDO_ID;             /*!< ���� DO���� ID */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=EXT_IN_PINS)]
            public short[]  nDO_PinNo;          /*!< ���� DO������ Pin No */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=EXT_IN_PINS)]
            public short[]  nDO_PinStatus;      /*!< ���� DO������ Pin ���� */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=EXT_IN_PINS)]
            public uint[]   dwSetCount;         /*!< ���� Count �� (DO ���迡 ����) */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=EXT_IN_PINS)]
            public uint[]   dwCurCount;         /*!< ���� Count �� */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=EXT_IN_PINS)]
            public short[]  nInterval;          /*!< ��������(us) */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=EXT_IN_PINS)]
            public uint[]   dwMTime;            /*!< �����ð� */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=EXT_IN_PINS)]
            public short[]  nEReserved0;        /*!< Ext Pin �� ����0 */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=EXT_IN_PINS)]
            public short[]  nEReserved1;        /*!< Ext Pin �� ����1 */
        };

        /**
          * @brief  Count �ð����� ������ ���� ����
          */
        [StructLayout(LayoutKind.Sequential, Pack=1)]
        public struct TNMF_CMT_STS
        {
            public short    nStatus;            /*!< Count�� �ش� External Pin �Է»��� */
            public short    nEdge;              /*!< ���� ��ȣ Edge(0=Rising, 1=Falling, 2=Both) */
            public short    nMTimeMode;         /*!< �ð� ���� ����(0=���� ����, 1=����) */
            public short    nEnable;            /*!< Count ��� ���� */
            public uint     dwValue;            /*!< Count(Ƚ��) */
            public short    nInterval;          /*!< ��������(us) */
            public short    nMemCntData;        /*!< �����ͷ� ä���� �ִ� �޸� ���� (�ִ� 1024��) */
            public short    nMemCntEmpty;       /*!< ��� �ִ� �޸� ���� (�ִ� 1024��) */
            public short    nRespSize;          /*!< ���� ������ ���� (�ִ� 1024��) */
            public short    nReserved0;         /*!< ����0 */
            public short    nReserved1;         /*!< ����1 */
        };

        /**
          * @brief  ���带 �������� �� ��ü ������ ���� (�迭 ����: �ִ� ���� ��)
          */
        [StructLayout(LayoutKind.Sequential, Pack=1)]
        public struct TNMF_ALL_STS
        {
            public short                nCntDIBrd;      /*!< DI ���� ���� */
            public short                nCntDOBrd;      /*!< DO ���� ���� */
            public short                nCntAIBrd;      /*!< AI ���� ���� */
            public short                nCntAOBrd;      /*!< AO ���� ���� */
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType=UnmanagedType.Struct, SizeConst=MAX_BRD)]
            public TNMF_BRD_DIO_STS[]   tDI;            /*!< DI ������ (�迭�� Index�� ID�� ��Ÿ��) */
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType=UnmanagedType.Struct, SizeConst=MAX_BRD)]
            public TNMF_BRD_DIO_STS[]   tDO;            /*!< DO ������ (�迭�� Index�� ID�� ��Ÿ��) */
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType=UnmanagedType.Struct, SizeConst=MAX_BRD)]
            public TNMF_BRD_AI_STS[]    tAI;            /*!< AI ������ (�迭�� Index�� ID�� ��Ÿ��) */
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType=UnmanagedType.Struct, SizeConst=MAX_BRD)]
            public TNMF_BRD_AO_STS[]    tAO;            /*!< AO ������ (�迭�� Index�� ID�� ��Ÿ��) */
            public TNMF_EXT_STS         tExtSts;        /*!< External Pin ���� */
        };
        /**************************************************************************************************/

        /**
          * @brief      ��ġ�� ��Ʈ��ũ ������¸� Ȯ���մϴ�. (Ping Check)
          * @param[in]  nNmfNo          ��ġ��ȣ
          * @param[in]  nIpAddr0        IP Address Field 0
          * @param[in]  nIpAddr1        IP Address Field 1
          * @param[in]  nIpAddr2        IP Address Field 2
          * @param[in]  lWaitTime       ������ð�(ms)
          * @return     Enmf_FUNC_RESULT
          */
        [DllImport("NMF.dll")]
        public static extern short nmf_PingCheck(short nNmfNo, short nIpAddr0, short nIpAddr1, short nIpAddr2, int lWaitTime);

        /**
          * @brief      ��ġ�� ������ �����մϴ�.
          * @param[in]  nNmfNo          ��ġ��ȣ(���͸� ����ġ ��ȣ���� IP��ȣ)
          * @param[in]  nIpAddr0        IP Address Field 0
          * @param[in]  nIpAddr1        IP Address Field 1
          * @param[in]  nIpAddr2        IP Address Field 2
          * @return     Enmf_FUNC_RESULT
          * @see        nmf_Disconnect
          */
        [DllImport("NMF.dll")]
        public static extern short nmf_Connect(short nNmfNo, short nIpAddr0, short nIpAddr1, short nIpAddr2);

        /**
          * @brief      ��ġ�� ������ �����մϴ�.
          * @param[in]  nNmfNo          ��ġ��ȣ
          * @see        nmf_Connect
          */
        [DllImport("NMF.dll")]
        public static extern void nmf_Disconnect(short nNmfNo);

        /**
          * @brief      ��ġ���� ����, ��Ʈ��ũ ������ �ۼ����� ���ѽð��� �����մϴ�.(ms����)
          * @param[in]  nNmfNo          ��ġ��ȣ
          * @param[in]  nTMO_Conn       ���� ���ѽð�(ms)
          * @param[in]  nTMO_Tx         �۽� ���ѽð�(ms)
          * @param[in]  nTMO_Rx         ���� ���ѽð�(ms)
          * @return     Enmf_FUNC_RESULT
          * @see        nmf_TMOGet
          */
        [DllImport("NMF.dll")]
        public static extern short nmf_TMOSet(short nNmfNo, short nTMO_Conn, short nTMO_Tx, short nTMO_Rx);

        /**
          * @brief      ������ ���ѽð��� Ȯ���մϴ�.(ms����)
          * @param[in]  nNmfNo          ��ġ��ȣ
          * @param[out] pnRetTMO_Conn   ���� ���ѽð� ��ȯ ������(ms)
          * @param[out] pnRetTMO_Tx     �۽� ���ѽð� ��ȯ ������(ms)
          * @param[out] pnRetTMO_Rx     ���� ���ѽð� ��ȯ ������(ms)
          * @return     Enmf_FUNC_RESULT
          * @see        nmf_TMOSet
          */
        [DllImport("NMF.dll")]
        public static extern short nmf_TMOGet(short nNmfNo, out short pnRetTMO_Conn, out short pnRetTMO_Tx, out short pnRetTMO_Rx);

        /**
          * @brief      ��ġ�� ��Ź���� �����մϴ�.
          * @param[in]  nNmfNo          ��ġ��ȣ
          * @param[in]  nMethod         ��Ź��(0=TCP, 1=UDP)
          */
        [DllImport("NMF.dll")]
        public static extern short nmf_ProtocolMethodSet(short nNmfNo, short nMethod);

        /**
          * @brief      ��ġ�� ��Ź���� Ȯ���մϴ�.
          * @param[in]  nNmfNo          ��ġ��ȣ
          * @return     ��Ź��(0=TCP, 1=UDP)
          */
        [DllImport("NMF.dll")]
        public static extern short nmf_ProtocolMethodGet(short nNmfNo);

        /**
          * @brief      [ DI���� ]�� �����Ͽ� ��üPin�� �Է»��¸� Ȯ���մϴ�.
          * @param[in]  nNmfNo          ��ġ��ȣ
          * @param[in]  pnInStatus      �Է�Pin�� ����(0=Off, 1=On) �迭 ������(�迭 ���� 128�� ����)
          * @return     Enmf_FUNC_RESULT
          * @warning    DI���� ������ ��ü �Է�Pin������ Ȯ���Ͻʽÿ�.
          */
        [DllImport("NMF.dll")]
        public static extern short nmf_DIGet(short nNmfNo, short[] pnInStatus);

        /**
          * @brief      [ DO���� ]�� �����Ͽ� ��üPin�� ��»��¸� Ȯ���մϴ�.
          * @param[in]  nNmfNo          ��ġ��ȣ
          * @param[in]  pnOutStatus     ���Pin�� ����(0=Off, 1=On) �迭 ������(�迭 ���� 128�� ����)
          * @return     Enmf_FUNC_RESULT
          * @warning    DO���� ������ ��ü ���Pin������ Ȯ���Ͻʽÿ�.
          */
        [DllImport("NMF.dll")]
        public static extern short nmf_DOGet(short nNmfNo, short[] pnOutStatus);

        /**
          * @brief      [ DO���� ]�� �����Ͽ� ��üPin�� ��»��¸� �����մϴ�.
          * @param[in]  nNmfNo          ��ġ��ȣ
          * @param[out] pnOutStatus     ���Pin�� ����(0=Off, 1=On) �迭 ������(�迭 ���� 128�� ����)
          * @return     Enmf_FUNC_RESULT
          * @warning    DO���� ������ ��ü ���Pin������ Ȯ���Ͻʽÿ�.
          */
        [DllImport("NMF.dll")]
        public static extern short nmf_DOSet(short nNmfNo, short[] pnOutStatus);

        /**
          * @brief      [ DO���� ]�� �����Ͽ� ��ü Pin�� �ϳ��� ��»��¸� �����մϴ�.
          * @param[in]  nNmfNo          ��ġ��ȣ
          * @param[in]  nPinNo          Pin ��ȣ(0~127)
          * @param[in]  nOutStatus      ���pin ���� (0=Off, 1=On)
          * @return     Enmf_FUNC_RESULT
          * @warning    DO���� ������ ��ü ���Pin������ Ȯ���Ͻʽÿ�.
          */
        [DllImport("NMF.dll")]
        public static extern short nmf_DOSetPin(short nNmfNo, short nPinNo, short nOutStatus);

        /**
          * @brief      [ DO���� ]�� �����Ͽ� ���Pin�� �������� �����ϰ� ��»��¸� �����մϴ�.
          * @param[in]  nNmfNo          ��ġ��ȣ
          * @param[in]  nPinCount       ��� Pin ����(Max. 128)
          * @param[in]  pnPinNo         Pin ��ȣ(0~127) �迭 ������
          * @param[in]  pnStatus        ��»���(0=Off, 1=On) �迭 ������
          * @return     Enmf_FUNC_RESULT
          * @warning    DO���� ������ ��ü ���Pin������ Ȯ���Ͻʽÿ�.
          */
        [DllImport("NMF.dll")]
        public static extern short nmf_DOSetPins(short nNmfNo, short nPinCount, short[] pnPinNo, short[] pnStatus);

        /**
          * @brief      [ DO���� ]�� �����Ͽ� ��ü Pin�� �ϳ��� ��»��¸� ������ŵ�ϴ�.
          * @param[in]  nNmfNo          ��ġ��ȣ
          * @param[in]  nPinNo          Pin ��ȣ(0~127)
          * @return     Enmf_FUNC_RESULT
          * @warning    DO���� ������ ��ü ���Pin������ Ȯ���Ͻʽÿ�.
          */
        [DllImport("NMF.dll")]
        public static extern short nmf_DOSetTogPin(short nNmfNo, short nPinNo);

        /**
          * @brief      [ DO���� ]�� �����Ͽ� ���Pin�� �������� �����ϰ� ��»��¸� ������ŵ�ϴ�.
          * @param[in]  nNmfNo          ��ġ��ȣ
          * @param[in]  nPinCount       Pin ����(Max. 128)
          * @param[in]  pnPinNo         Pin ��ȣ(0~127) �迭 ������
          * @return     Enmf_FUNC_RESULT
          * @warning    DO���� ������ ��ü ���Pin������ Ȯ���Ͻʽÿ�.
          */
        [DllImport("NMF.dll")]
        public static extern short nmf_DOSetTogPins(short nNmfNo, short nPinCount, short[] pnPinNo);

        /**
          * @brief      [ DO���� ]�� �����Ͽ� ���� ���Pin�� �����ð� ���ѱ���� �����ϰ� ��� On/Off�� �մϴ�.
          * @param[in]  nNmfNo          ��ġ��ȣ
          * @param[in]  nPinNo          Pin ��ȣ(0~127)
          * @param[in]  nOutStatus      ������ ��°� (0=Off, 1=On)
          * @param[in]  lTime           ������ �������ѽð�(ms)
          * @return     Enmf_FUNC_RESULT
          * @warning    DO���� ������ ��ü ���Pin������ Ȯ���Ͻʽÿ�.
          */
        [DllImport("NMF.dll")]
        public static extern short nmf_DOSetLimitTime(short nNmfNo, short nPinNo, short nOutStatus, int lTime);

        /**
          * @brief      [ DO���� ]�� �����Ͽ� ���� ��� Pin�� ������ �����ð� ���ѱ���� Ȯ���մϴ�.
          * @param[in]  nNmfNo          ��ġ��ȣ
          * @param[in]  nPinNo          Pin ��ȣ(0~127)
          * @param[out] pnSet           ���� ���¸� �о�� ������ ���� (0=�����ȵ�, 1=On, 2=Off)
          * @param[out] pnStatus        ���� ���ѽð��� ���¸� �о�� ������ ����
          *                             (0=�����ȵ�, 1=���ѽð� ������, 2=���� �ð� ����)
          * @param[out] pnOutStatus     ���� Pin ��»��¸� �о�� ������ ���� (0=Off, 1=On)
          * @param[out] plRemainTime    ���� �ð��� �о�� ������ ����(ms)
          * @return     Enmf_FUNC_RESULT
          * @warning    DO���� ������ ��ü ���Pin������ Ȯ���Ͻʽÿ�.
          */
        [DllImport("NMF.dll")]
        public static extern short nmf_DOGetLimitTime(short nNmfNo, short nPinNo, out short pnSet, out short pnStatus, out short pnOutStatus, out int plRemainTime);

        /**
          * @brief      [ DO���� ]�� �����ϰ� ����Pin�� ��»��¸� �����մϴ�.
          * @param[in]  nNmfNo          ��ġ ��ȣ
          * @param[in]  nID             ���� ID(0 ~ 7)
          * @param[in]  nPinNo          Pin ��ȣ(0~ 15)
          * @param[in]  nOutStatus      ��»���(0=Off, 1=On)
          * @return     Enmf_FUNC_RESULT
          * @warning    ���� ID�� Pin��ȣ�� Ȯ���Ͻʽÿ�.
          */
        [DllImport("NMF.dll")]
        public static extern short nmf_DOBrdSetPin(short nNmfNo, short nID, short nPinNo, short nOutStatus);

        /**
          * @brief      [ DO���� ]�� �����ϰ� ����Pin�� ��»��¸� �����մϴ�.
          * @param[in]  nNmfNo          ��ġ��ȣ
          * @param[in]  nID             ���� ID(0 ~ 7)
          * @param[in]  nPinCount       ��� Pin ����(Max. 16)
          * @param[in]  pnPinNo         ��� Pin ��ȣ(0 ~ 15) �迭 ������
          * @param[in]  pnOutStatus     ��»��� (0=Off, 1=On) �迭 ������
          * @return     Enmf_FUNC_RESULT
          * @warning    ���� ID�� Pin�� ���� �� ��ȣ�� Ȯ���Ͻʽÿ�.
          */
        [DllImport("NMF.dll")]
        public static extern short nmf_DOBrdSetPins(short nNmfNo, short nID, short nPinCount, short[] pnPinNo, short[] pnOutStatus);

        /**
          * @brief      [ DO���� ]�� ������ �����Ͽ�, ����Pin�� ��»��¸� �����մϴ�.
          * @param[in]  nNmfNo          ��ġ��ȣ
          * @param[in]  nBrdCount       ���� ����(Max. 8Pin)
          * @param[in]  pnID            ���� ID(0 ~ 7) �迭 ������
          * @param[in]  plOnPins        ���庰 ��� On Mask (16Bit 0 ~ 0xFFFF) �迭 ������
          * @param[in]  plOffPins       ���庰 ��� Off Mask (16Bit 0 ~ 0xFFFF) �迭 ������
          * @return     Enmf_FUNC_RESULT
          * @warning    ���� ������ ID, Pin Mask�� Ȯ���Ͻʽÿ�.
          */
        [DllImport("NMF.dll")]
        public static extern short nmf_DOmBrdSetPins(short nNmfNo, short nBrdCount, short[] pnID, int[] plOnPins, int[] plOffPins);

        /**
          * @brief      [ DO���� ]�� �����ϰ� ����Pin�� ��»��¸� ������ŵ�ϴ�.
          * @param[in]  nNmfNo          ��ġ ��ȣ
          * @param[in]  nID             ���� ID(0 ~ 7)
          * @param[in]  nPinNo          Pin ��ȣ(0~ 15)
          * @return     Enmf_FUNC_RESULT
          * @warning    ���� ID�� Pin��ȣ�� Ȯ���Ͻʽÿ�.
          */
        [DllImport("NMF.dll")]
        public static extern short nmf_DOBrdSetTogPin(short nNmfNo, short nID, short nPinNo);

        /**
          * @brief      [ DO���� ]�� �����ϰ� ����Pin�� ��»��¸� ������ŵ�ϴ�.
          * @param[in]  nNmfNo          ��ġ��ȣ
          * @param[in]  nID             ���� ID(0 ~ 7)
          * @param[in]  nPinCount       ��� Pin ����(Max. 16)
          * @param[in]  pnPinNo         ������ Pin ��ȣ(0 ~ 15) �迭 ������
          * @return     Enmf_FUNC_RESULT
          * @warning    ���� ������ Pin�� ������ Ȯ���Ͻʽÿ�.
          */
        [DllImport("NMF.dll")]
        public static extern short nmf_DOBrdSetTogPins(short nNmfNo, short nID, short nPinCount, short[] pnPinNo);

        /**
          * @brief      [ DO���� ]�� ������ �����Ͽ�, ����Pin�� ��»��¸� ������ŵ�ϴ�.
          * @param[in]  nNmfNo          ��ġ��ȣ
          * @param[in]  nBrdCount       ���� ����(Max. 8)
          * @param[in]  pnID            ���� ID(0 ~ 7) �迭 ������
          * @param[in]  plPins          ���庰 ���� Mask (16Bit 0 ~ 0xFFFF) �迭 ������
          * @return     Enmf_FUNC_RESULT
          * @warning    ���� ������ ID, Pin Mask�� Ȯ���Ͻʽÿ�.
          */
        [DllImport("NMF.dll")]
        public static extern short nmf_DOmBrdSetTogPins(short nNmfNo, short nBrdCount, short[] pnID, int[] plPins);

        /**
          * @brief      [ AO���� ]�� �����Ͽ� ����Pin�� Analog ����� �����մϴ�.
          * @param[in]  nNmfNo          ��ġ��ȣ
          * @param[in]  nPinNo          ������ Pin ��ȣ(0~63)
          * @param[in]  nRange          ���� ���� (0=0~5V, 1=0~10V, 2=0~10.86V, 3=��5V, 4=��10V, 5=��10.86V)
          * @param[in]  dValue          ��� ���а�
          * @return     Enmf_FUNC_RESULT
          * @warning    AO���� ������ ��ü Analog ���Pin������ Ȯ���Ͻʽÿ�.
          */
        [DllImport("NMF.dll")]
        public static extern short nmf_AOSetPin(short nNmfNo, short nPinNo, short nRange, double dValue);

        /**
          * @brief      [ AO���� ]�� �����Ͽ� ����Pin�� Analog ����� �����մϴ�.
          * @param[in]  nNmfNo          ��ġ��ȣ
          * @param[in]  nPinCount       ���� Pin ����(Max. ��ü AO Pin ����)
          * @param[in]  pnPinNo         ������ Pin ��ȣ short �� �迭 ������ (��ü AO Pin ���� ����)
          * @param[in]  pnRange         ���� ���� �迭 ������(0=0~5V, 1=0~10V, 2=0~10.86V, 3=��5V, 4=��10V, 5=��10.86V)
          * @param[in]  pdValue         ��� ���а� �迭 ������
          * @return     Enmf_FUNC_RESULT
          * @warning    AO���� ������ ��ü Analog ���Pin������ Ȯ���Ͻʽÿ�.
          */
        [DllImport("NMF.dll")]
        public static extern short nmf_AOSetPins(short nNmfNo, short nPinCount, short[] pnPinNo, short[] pnRange, double[] pdValue);

        /**
          * @brief      [ AO���� ]�� �����Ͽ� ����Pin�� Analog ����� �����մϴ�.
          * @param[in]  nNmfNo          ��ġ��ȣ
          * @param[in]  nID             ���� ID(0 ~ 7)
          * @param[in]  nPinNo          Pin ��ȣ(0 ~ 7)
          * @param[in]  nRange          ���� ����(0=0~5V, 1=0~10V, 2=0~10.86V, 3=��5V, 4=��10V, 5=��10.86V)
          * @param[in]  dValue          ��� ���а�
          * @return     Enmf_FUNC_RESULT
          * @warning    ���� ID�� Pin��ȣ�� Ȯ���Ͻʽÿ�.
          */
        [DllImport("NMF.dll")]
        public static extern short nmf_AOBrdSetPin(short nNmfNo, short nID, short nPinNo, short nRange, double dValue);

        /**
          * @brief      [ AO���� ]�� �����Ͽ� ����Pin�� Analog ����� �����մϴ�.
          * @param[in]  nNmfNo          ��ġ��ȣ
          * @param[in]  nID             ���� ID(0 ~ 7)
          * @param[in]  nPinCount       ���� Pin ����(Max. 8Pin)
          * @param[in]  pnPinNo         Pin ��ȣ(0 ~ 7) �迭 ������
          * @param[in]  pnRange         ���� ���� �迭 ������(0=0~5V, 1=0~10V, 2=0~10.86V, 3=��5V, 4=��10V, 5=��10.86V)
          * @param[in]  pdValue         ��� ���а� �迭 ������
          * @return     Enmf_FUNC_RESULT
          * @warning    ���� ID�� Pin�� ���� �� ��ȣ�� Ȯ���Ͻʽÿ�.
          */
        [DllImport("NMF.dll")]
        public static extern short nmf_AOBrdSetPins(short nNmfNo, short nID, short nPinCount, short[] pnPinNo, short[] pnRange, double[] pdValue);

        /**
          * @brief      [ AO���� ]�� ������ �����Ͽ� ����Pin�� Analog ����� �����մϴ�.
          * @param[in]  nNmfNo          ��ġ��ȣ
          * @param[in]  nPinCount       ���� Pin ����(Max. 8)
          * @param[in]  pnID            ���� ID(0 ~ 7) �迭 ������
          * @param[in]  pnPinNo         ���庰 Pin ��ȣ(0 ~ 7) �迭 ������
          * @param[in]  pnRange         ���� ���� �迭 ������(0=0~5V, 1=0~10V, 2=0~10.86V, 3=��5V, 4=��10V, 5=��10.86V)
          * @param[in]  pdValue         ��� ���а� �迭 ������
          * @return     Enmf_FUNC_RESULT
          * @warning    ���� ������ Pin�� ������ Ȯ���Ͻʽÿ�.
          */
        [DllImport("NMF.dll")]
        public static extern short nmf_AOmBrdSetPins(short nNmfNo, short nPinCount, short[] pnID, short[] pnPinNo, short[] pnRange, double[] pdValue);

        /**
          * @brief      [Ext. Count] �ܺ� �Է� �ɿ� Count ����� �����ϰ�, �����մϴ�.
          * @param[in]  nNmfNo          ��ġ��ȣ
          * @param[in]  nEPinNo         ��ȣ�� ���� Ext Pin ��ȣ(0 ~ 7)
          ' @param[in]  nMTimeMode      �ð� ������� ���࿩��(0=�������, 1=������)
          * @param[in]  nEdge           ��ȣ Edge(0=Rising, 1=Falling, 2=Both)
          * @param[in]  nDO_ID          ���� DO ���� ID(0 ~ 7)
          * @param[in]  nDO_PinNo       ���� DO ���� ��ȣ(0 ~ 15)
          * @param[in]  nDO_On          ���� DO ���� ��� ����(0=Off, 1=On)
          * @param[in]  dwCount         Count�� ����(1 ~ 4,294,967,295(32Bit)), 0�� Count ������
          * @return     Enmf_FUNC_RESULT
          * @warning    DO���� �������� �Ŵ����� �����Ͽ� �ֽʽÿ�.
          */
        [DllImport("NMF.dll")]
        public static extern short nmf_ExtCountStart(short nNmfNo, short nEPinNo, short nMTimeMode, short nEdge, short nDO_ID, short nDO_PinNo, short nDO_On, uint dwCount);

        /**
          * @brief      [Ext. Count] �ܺ� �Է� ���� Count ��� ������ Ȯ���մϴ�.
          * @param[in]  nNmfNo          ��ġ��ȣ
          * @param[out] ptExtCInfo      ��ü Ext Pin�� Count ���� ����ü ������
          * @return     Enmf_FUNC_RESULT
          */
        [DllImport("NMF.dll")]
        public static extern short nmf_ExtCountGetInfo(short nNmfNo, out TNMF_EXTC_INFO ptExtCInfo);

        /**
          * @brief      [Ext. Count] Count ����� �ð���������� ��� ������ �����͸� Ȯ���մϴ�.
          * @param[in]  nNmfNo          ��ġ��ȣ
          * @param[in]  nEPinNo         �о�� Ext Pin ��ȣ(0 ~ 7)
          * @param[out] ptCMT           Count ���� Ȯ���� TNMF_CMT_STS ����ü ������
          * @param[in]  nBufNum         ����(pdwRetBuf)�� ����(�ִ� COUNTER_MTIME_MAX_DATAS=1024��)
          * @param[out] pdwRetBuf       ������ �ð��� �о�� �迭 ������(us)
          * @return     Enmf_FUNC_RESULT
          */
        [DllImport("NMF.dll")]
        public static extern short nmf_ExtCountGetTime(short nNmfNo, short nEPinNo, out TNMF_CMT_STS ptCMT, short nBufNum, out uint pdwRetBuf);

        /**
          * @brief      [Ext. Count] �ܺ� �Է� ���� Count ����� �����մϴ�.
          * @param[in]  nNmfNo          ��ġ��ȣ
          * @param[in]  nEPinNo         ������ Ext Pin ��ȣ(0 ~ 7)
          * @return     Enmf_FUNC_RESULT
          */
        [DllImport("NMF.dll")]
        public static extern short nmf_ExtCountStop(short nNmfNo, short nEPinNo);

        /**
          * @brief      [Ext. Count] �ܺ� �Է� �ɿ� ���� ���� �����մϴ�. (��� �ɸ� �ش�)
          * @param[in]  nNmfNo          ��ġ��ȣ
          * @param[in]  nWriteFlash     ���� ���� �� ����Ǵ� ���Ͱ� ���� ����(Flash�� ���� ����)
          * @param[in]  pnFilter        ���� Index�� �迭 ������ (��� Ext Pin 3~7)
          * @return     Enmf_FUNC_RESULT
          */
        [DllImport("NMF.dll")]
        public static extern short nmf_ExtCountFilterSet(short nNmfNo, short nWriteFlash, short[] pnFilter);

        /**
          * @brief      [Ext. Count] �ܺ� �Է� �ɿ� ������ ���� ���� Ȯ���մϴ�.(��� �ɸ� �ش�)
          * @param[in]  nNmfNo          ��ġ��ȣ
          * @param[out] pnRetFilter     ���� Index�� �迭(��� Ext Pin 3~7)�� �迭 ������
          * @return     Enmf_FUNC_RESULT
          */
        [DllImport("NMF.dll")]
        public static extern short nmf_ExtCountFilterGet(short nNmfNo, short[] pnRetFilter);

        /**
          * @brief      [ AI���� ]�� �����Ͽ� �Է� Range�� �����մϴ�.
          * @param[in]  nNmfNo          ��ġ��ȣ
          * @param[in]  nID             ���� ID(0 ~ 7)
          * @param[in]  nRange          AI�� �Է� ���� (0=��5V, 1=��10V)
          * @return     Enmf_FUNC_RESULT
          * @warning    ���� �� ���ø� �����͵� �ش� ������ ������ �ʱ�ȭ �˴ϴ�.
          */
        [DllImport("NMF.dll")]
        public static extern short nmf_AIBrdSetRange(short nNmfNo, short nID, short nRange);

        /**
          * @brief      [ AI���� ]�� �����Ͽ� ���� Analog �Է�Pin�� ���ø� ����� �����մϴ�.
          * @param[in]  nNmfNo          ��ġ��ȣ
          * @param[in]  nPinNo          ������ Pin ��ȣ (��ü AI Pin ���� ����)
          * @param[in]  lRate           AD ���ø� �ֱ�(0 ~ 100000)
          * @param[in]  nTrgType        ���� Ʈ���� ���(0=���� ����(���), 1=Trg Rising, 2=Trg Falling, 3=Trg Both)
          * @param[in]  nExtPin         ���� Ʈ���ſ� ���� External Pin ��ȣ (0 ~ 3)
          * @param[in]  nMemFullOvW     �޸� Full�� Overwrite���� (0= ���� ������ ���ø� �����ͺ��� Overwrite(���ø� ���� ����),
          *                                                          1= Overwrite����(���ø� ����))
          * @return     Enmf_FUNC_RESULT
          * @warning    ���� �� �ش� Pin�� �޸𸮵��� �ʱ�ȭ �˴ϴ�.
          */
        [DllImport("NMF.dll")]
        public static extern short nmf_AISmplStartPin(short nNmfNo, short nPinNo, int lRate, short nTrgType, short nExtPin, short nMemFullOvW);

        /**
          * @brief      [ AI���� ]�� �����Ͽ� ���� Analog �Է�Pin�� ���ø� ����� �����մϴ�.
          * @param[in]  nNmfNo          ��ġ��ȣ
          * @param[in]  nPinCount       ������ Pin ����(��ü AI Pin ���� ����)
          * @param[in]  pnPinNo         AI �Է� Pin ��ȣ �迭 ������ (��ü AI Pin ���� ����)
          * @param[in]  plRate          AD ���ø� �ֱ�(0~100000) �迭 ������
          * @param[in]  pnTrgType       ���� Ʈ���� ���(0=���� ����, 1=Trg Rising, 2=Trg Falling, 3=Trg Both) �迭 ������
          * @param[in]  pnExtPin        ���� Ʈ���ſ� ���� External Pin ��ȣ (0 ~ 3) �迭 ������
          * @param[in]  pnMemFullOvW    �޸� Full�� Overwrite���� �迭 ������ (0= ���� ������ ���ø� �����ͺ��� Overwrite(���ø� ���� ����),
          *                                                                      1= Overwrite����(���ø� ����))
          * @return     Enmf_FUNC_RESULT
          * @warning    AI���� ������ ��ü Analog �Է�Pin������ Ȯ���Ͻʽÿ�.
          */
        [DllImport("NMF.dll")]
        public static extern short nmf_AISmplStartPins(short nNmfNo, short nPinCount, short[] pnPinNo, int[] plRate, short[] pnTrgType, short[] pnExtPin, short[] pnMemFullOvW);

        /**
          * @brief      [ AI���� ]�� �����Ͽ� ���� Analog �Է�Pin�� ���ø� ����� �����մϴ�.
          * @param[in]  nNmfNo          ��ġ��ȣ
          * @param[in]  nID             ���� ID
          * @param[in]  nPinNo          AI �Է� Pin ��ȣ(0 ~ 7)
          * @param[in]  lRate           AD ���ø� �ֱ�(0 ~ 100000)
          * @param[in]  nTrgType        ���� Ʈ���� ���(0=���� ����, 1=Trg Rising, 2=Trg Falling, 3=Trg Both)
          * @param[in]  nExtPin         ���� Ʈ���ſ� ���� External Pin ��ȣ (0 ~ 3)
          * @param[in]  nMemFullOvW     �޸� Full�� Overwrite���� (0= ���� ������ ���ø� �����ͺ��� Overwrite(���ø� ���� ����),
          *                                                          1= Overwrite����(���ø� ����))
          * @return     Enmf_FUNC_RESULT
          * @warning    AI���� ID�� Pin��ȣ�� Ȯ���Ͻʽÿ�.
          */
        [DllImport("NMF.dll")]
        public static extern short nmf_AIBrdSmplStartPin(short nNmfNo, short nID, short nPinNo, int lRate, short nTrgType, short nExtPin, short nMemFullOvW);

        /**
          * @brief      [ AI���� ]�� �����Ͽ� ���� Analog �Է�Pin�� ���ø� ����� �����մϴ�.
          * @param[in]  nNmfNo          ��ġ��ȣ
          * @param[in]  pnID            ���� ID(0 ~ 7)
          * @param[in]  nPinCount       ������ Pin ���� (Max. 8Pin)
          * @param[in]  pnPinNo         AI �Է� Pin ��ȣ(0 ~ 7) �迭 ������
          * @param[in]  plRate          AD ���ø� �ֱ�(0~100000) �迭 ������
          * @param[in]  pnTrgType       ���� Ʈ���� ���(0=���� ����, 1=Trg Rising, 2=Trg Falling, 3=Trg Both) �迭 ������
          * @param[in]  pnExtPin        ���� Ʈ���ſ� ���� External Pin ��ȣ (0 ~ 3) �迭 ������
          * @param[in]  pnMemFullOvW    �޸� Full�� Overwrite���� �迭 ������ (0= ���� ������ ���ø� �����ͺ��� Overwrite(���ø� ���� ����),
          *                                                                      1= Overwrite����(���ø� ����))
          * @return     Enmf_FUNC_RESULT
          * @warning    ���� �� �ش� Pin�� �޸𸮵��� �ʱ�ȭ �˴ϴ�.
          */
        [DllImport("NMF.dll")]
        public static extern short nmf_AIBrdSmplStartPins(short nNmfNo, short nID, short nPinCount, short[] pnPinNo, int[] plRate, short[] pnTrgType, short[] pnExtPin, short[] pnMemFullOvW);

        /**
          * @brief      [ AI���� ]�� ������ �����Ͽ� ���� Analog �Է�Pin�� ���ø� ����� �����մϴ�.
          * @param[in]  nNmfNo          ��ġ��ȣ
          * @param[in]  nPinCount       ������ Pin ����(Max. ��ü AI Pin ����)
          * @param[in]  pnID            ���� ID(0 ~ 7) �迭 ������
          * @param[in]  pnPinNo         ���庰 AI �Է� Pin ��ȣ(0 ~ 7) �迭 ������
          * @param[in]  plRate          ���庰 AD ���ø� �ֱ�(0~100000) �迭 ������
          * @param[in]  pnTrgType       ���� Ʈ���� ���(0=���� ����, 1=Trg Rising, 2=Trg Falling 3=Trg Both) �迭 ������
          * @param[in]  pnExtPin        ���� Ʈ���ſ� ���� External Pin ��ȣ (0 ~ 3) �迭 ������
          * @param[in]  pnMemFullOvW    �޸� Full�� Overwrite���� �迭 ������ (0= ���� ������ ���ø� �����ͺ��� Overwrite(���ø� ���� ����),
          *                                                                      1= Overwrite����(���ø� ����))
          * @return     Enmf_FUNC_RESULT
          * @warning    ���� �� �ش� Pin�� �޸𸮵��� �ʱ�ȭ �˴ϴ�.
          */
        [DllImport("NMF.dll")]
        public static extern short nmf_AImBrdSmplStartPins(short nNmfNo, short nPinCount, short[] pnID, short[] pnPinNo, int[] plRate, short[] pnTrgType, short[] pnExtPin, short[] pnMemFullOvW);

        /**
          * @brief      [ AI���� ]�� �����Ͽ� ���� Analog �Է�Pin�� �Է»��¸� �����մϴ�.
          * @param[in]  nNmfNo          ��ġ��ȣ
          * @param[in]  nPinNo          AI �Է� Pin ��ȣ(��ü AI Pin ���� ����)
          * @return     Enmf_FUNC_RESULT
          * @warning    AI���� ������ ��ü Analog �Է�Pin������ Ȯ���Ͻʽÿ�.
          */
        [DllImport("NMF.dll")]
        public static extern short nmf_AISmplStopPin(short nNmfNo, short nPinNo);

        /**
          * @brief      [ AI���� ]�� �����Ͽ� ���� Analog �Է�Pin�� �Է»��¸� �����մϴ�.
          * @param[in]  nNmfNo          ��ġ��ȣ
          * @param[in]  nPinCount       ������ Pin ����(Max. ��ü AI Pin ����)
          * @param[in]  pnPinNo         AI �Է� Pin ��ȣ �迭 ������ (��ü AI Pin ���� ����)
          * @return     Enmf_FUNC_RESULT
          * @warning    AI���� ������ ��ü Analog �Է�Pin������ Ȯ���Ͻʽÿ�.
          */
        [DllImport("NMF.dll")]
        public static extern short nmf_AISmplStopPins(short nNmfNo, short nPinCount, short[] pnPinNo);

        /**
          * @brief      [ AI���� ]�� �����Ͽ� ���� Analog �Է�Pin�� �Է»��¸� �����մϴ�.
          * @param[in]  nNmfNo          ��ġ��ȣ
          * @param[in]  nID             ����ID
          * @param[in]  nPinNo          AI �Է� Pin ��ȣ(0 ~ 7)
          * @return     Enmf_FUNC_RESULT
          */
        [DllImport("NMF.dll")]
        public static extern short nmf_AIBrdSmplStopPin(short nNmfNo, short nID, short nPinNo);

        /**
          * @brief      [ AI���� ]�� �����Ͽ� ���� Analog �Է�Pin�� �Է»��¸� �����մϴ�.
          * @param[in]  nNmfNo          ��ġ��ȣ
          * @param[in]  nID             ���� ID(0 ~ 7)
          * @param[in]  nPinCount       ������ Pin ����(Max. 8Pin)
          * @param[in]  pnPinNo         AI �Է� Pin ��ȣ(0 ~ 7) �迭 ������
          * @return     Enmf_FUNC_RESULT
          */
        [DllImport("NMF.dll")]
        public static extern short nmf_AIBrdSmplStopPins(short nNmfNo, short nID, short nPinCount, short[] pnPinNo);

        /**
          * @brief      [ AI���� ]�� ������ �����Ͽ� ���� Analog �Է�Pin�� �Է»��¸� �����մϴ�.
          * @param[in]  nNmfNo          ��ġ��ȣ
          * @param[in]  nPinCount       ������ Pin ����(Max. ��ü AI Pin ����)
          * @param[in]  pnID            ���� ID(0 ~ 7) �迭 ������
          * @param[in]  pnPinNo         AI �Է� Pin ��ȣ(0 ~ 7) �迭 ������
          * @return     Enmf_FUNC_RESULT
          * @warning    ���� �� �ش� Pin�� �޸𸮵��� �ʱ�ȭ �˴ϴ�.
          */
        [DllImport("NMF.dll")]
        public static extern short nmf_AImBrdSmplStopPins(short nNmfNo, short nPinCount, short[] pnID, short[] pnPinNo);

        /**
          * @brief      [ AI���� ]�� �����Ͽ� ���� Analog �Է�Pin�� Sampling Rate ���� Ȯ���մϴ�.
          * @param[in]  nNmfNo          ��ġ��ȣ
          * @param[in]  nPinNo          AI �Է� Pin ��ȣ(��ü AI Pin ���� ����)
          * @param[out] plRetSmplRate   �ش� AI���� Pin�� ���� Sampling Rate�� �о�� ������ ����
          * @return     Enmf_FUNC_RESULT
          * @warning    AI���� ������ ��ü Analog �Է�Pin������ Ȯ���Ͻʽÿ�.
          */
        [DllImport("NMF.dll")]
        public static extern short nmf_AISmplRateGetPin(short nNmfNo, short nPinNo, out int plRetSmplRate);

        /**
          * @brief      [ AI���� ]�� �����Ͽ� ���� Analog �Է�Pin�� Sampling Rate ���� Ȯ���մϴ�.
          * @param[in]  nNmfNo          ��ġ��ȣ
          * @param[in]  nPinCount       Ȯ���� Pin ����(Max. ��ü AI Pin ����)
          * @param[in]  pnPinNo         AI �Է� Pin ��ȣ �迭 ������ (��ü AI Pin ���� ����)
          * @param[out] plRetSmplRate   �ش� AI���� Pin�� ���� Sampling Rate�� �о�� �迭 ������ (Pin Count)
          * @return     Enmf_FUNC_RESULT
          * @warning    AI���� ������ ��ü Analog �Է�Pin������ Ȯ���Ͻʽÿ�.
          */
        [DllImport("NMF.dll")]
        public static extern short nmf_AISmplRateGetPins(short nNmfNo, short nPinCount, short[] pnPinNo, int[] plRetSmplRate);

        /**
          * @brief      [ AI���� ]�� �����Ͽ� ���� Analog �Է�Pin�� Sampling Rate ���� Ȯ���մϴ�.
          * @param[in]  nNmfNo          ��ġ��ȣ
          * @param[in]  nID             ���� ID(0 ~ 7)
          * @param[in]  nPinNo          AI �Է� Pin ��ȣ(0 ~ 7)
          * @param[out] plRetSmplRate   �ش� AI���� Pin�� ���� Sampling Rate�� �о�� ������ ����
          * @return     Enmf_FUNC_RESULT
          */
        [DllImport("NMF.dll")]
        public static extern short nmf_AIBrdSmplRateGetPin(short nNmfNo, short nID, short nPinNo, out int plRetSmplRate);

        /**
          * @brief      [ AI���� ]�� �����Ͽ� ���� Analog �Է�Pin�� Sampling Rate ���� Ȯ���մϴ�.
          * @param[in]  nNmfNo          ��ġ��ȣ
          * @param[in]  nID             ���� ID(0 ~ 7)
          * @param[in]  nPinCount       ������ Pin ����(Max. 8)
          * @param[in]  pnPinNo         AI �Է� Pin ��ȣ(0 ~ 7) �迭 ������ (Size:nPinCount)
          * @param[out] plRetSmplRate   �ش� AI���� Pin�� ���� Sampling Rate�� �о�� �迭 ������ (Size:nPinCount)
          * @return     Enmf_FUNC_RESULT
          */
        [DllImport("NMF.dll")]
        public static extern short nmf_AIBrdSmplRateGetPins(short nNmfNo, short nID, short nPinCount, short[] pnPinNo, int[] plRetSmplRate);

        /**
          * @brief      [ AI���� ]�� ������ �����Ͽ� ���� Analog �Է�Pin�� Sampling Rate ���� Ȯ���մϴ�.
          * @param[in]  nNmfNo          ��ġ��ȣ
          * @param[in]  nPinCount       ������ Pin ���� (Max. ��ü AI Pin ����)
          * @param[in]  pnID            ���� ID(0 ~ 7) �迭 ������
          * @param[in]  pnPinNo         ���庰 AI �Է� Pin ��ȣ(0 ~ 7) �迭 ������ (Size:nPinCount)
          * @param[out] plRetSmplRate   �ش� AI���� Pin�� ���� Sampling Rate�� �о�� �迭 ������ (Size:nPinCount)
          * @return     Enmf_FUNC_RESULT
          */
        [DllImport("NMF.dll")]
        public static extern short nmf_AImBrdSmplRateGetPins(short nNmfNo, short nPinCount, short[] pnID, short[] pnPinNo, int[] plRetSmplRate);

        /**
          * @brief      [ AI���� ]�� �����Ͽ� ���� Analog �Է�Pin�� Ext. Memory Size�� �����մϴ�.
          * @param[in]  nNmfNo          ��ġ��ȣ
          * @param[in]  nPinCount       ������ Pin ���� (Max. ��ü AI Pin ����)
          * @param[in]  pnPinNo         �ش� AI������ Pin ��ȣ(0 ~ 7) �迭 ������(��ü AI Pin ���� ����)
          * @param[in]  pnMemBCount     Pin�� �Ҵ�Ǵ� �ܺ� Memory Block ������ (0~64 Block) �迭 ������
          *                             1Block = 2048��(4KByte), 0 ���� �� �ܺ� Memory ��� ����
          * @return     Enmf_FUNC_RESULT
          * @warning    �ټ��� Pin�鿡 �����ϴ� �޸��� ũ��� ������ �޸� 64���� �ʰ��� �� �����ϴ�.
          */
        [DllImport("NMF.dll")]
        public static extern short nmf_AISmplExMemSet(short nNmfNo, short nPinCount, short[] pnPinNo, short[] pnMemBCount);

        /**
          * @brief      [ AI���� ]�� ������ �����Ͽ� ���� Analog �Է�Pin�� Ext. Memory Size�� �����մϴ�.
          * @param[in]  nNmfNo          ��ġ��ȣ
          * @param[in]  nPinCount       ������ Pin ���� (Max. ��ü AI Pin ����)
          * @param[in]  pnID            ���� ID(0 ~ 7) �迭 ������
          * @param[in]  pnPinNo         �ش� AI������ Pin ��ȣ(0 ~ 7) �迭 ������ (Size:nPinCount)
          * @param[in]  pnMemBCount     Pin�� �Ҵ�Ǵ� �ܺ� Memory Block ������ (0~64 Block) �迭 ������ (Size:nPinCount)
          *                             1Block = 2048��(4KByte), 0 ���� �� �ܺ� Memory ��� ����
          * @return     Enmf_FUNC_RESULT
          * @warning    �ټ��� Pin�鿡 �����ϴ� �޸��� ũ��� ������ �޸� 64���� �ʰ��� �� �����ϴ�.
          */
        [DllImport("NMF.dll")]
        public static extern short nmf_AIBrdSmplExMemSet(short nNmfNo, short nPinCount, short[] pnID, short[] pnPinNo, short[] pnMemBCount);

        /**
          * @brief      [ AI���� ]�� ������ �����Ͽ� ���� Analog �Է�Pin�� Ext. Memory Size�� �������¸� Ȯ���մϴ�.
          * @param[in]  nNmfNo          ��ġ��ȣ
          * @param[out] pnRetPinCount   �ܺ� Memory�� �Ҵ�� Pin ���� (Max. ��ü AI Pin ����)
          * @param[out] pnRetID         ���� ID(0 ~ 7) �迭 ������
          * @param[out] pnRetPinNo      �ش� AI������ Pin ��ȣ(0 ~ 7) �迭 ������
          * @param[out] pnRetMemBCount  Pin�� �Ҵ�Ǵ� �ܺ� Memory Block ������ (0~64 Block) �迭 ������
          *                             1Block = 2048��(4KByte), 0 ���� �� �ܺ� Memory ��� ����
          * @param[out] pnRetMemBFill   �Ҵ�� Memory Block �� Data�� ä���� ������ (0~64 Block) �迭 ������
          * @return     Enmf_FUNC_RESULT
          */
        [DllImport("NMF.dll")]
        public static extern short nmf_AIBrdSmplExMemGet(short nNmfNo, out short pnRetPinCount, short[] pnRetID, short[] pnRetPinNo, short[] pnRetMemBCount, short[] pnRetMemBFill);

        /**
          * @brief      [ AI���� ]�� �����Ͽ� �Էµ� Sampling Data�� Ȯ���մϴ�.
          * @param[in]  nNmfNo          ��ġ��ȣ
          * @param[in]  nID             ���� ID(0 ~ 7)
          * @param[in]  nPinNo          AI �Է� Pin ��ȣ(0 ~ 7)
          * @param[in]  nType           AI �Է� Ÿ�� (0=���а�, 1=AD��, 2=������)
          * @param[out] ptAISmpl        AI ���ø� ���� ����ü ������
          * @param[in]  lBufNum         �о�� ������ ���� ���� (0�� ��� �޸𸮿� �ִ� �����͸� ���)
          * @param[out] pdCalcValue     AI ���ø� ������ (Max. AI_MEM_DATAS(133120))
          * @return     Enmf_FUNC_RESULT
          */
        [DllImport("NMF.dll")]
        public static extern short nmf_AIBrdSmplValueGetPin(short nNmfNo, short nID, short nPinNo, short nType, out TNMF_BRD_AI_SMPL ptAISmpl, int lBufNum, double[] pdCalcValue);

        /**
          * @brief      NMF�� ��� ������ Ȯ���մϴ�.
          * @param[in]  nNmfNo          ��ġ��ȣ
          * @param[out] ptRetAllSts     NMF�� ������ ��� ����
          * @return     Enmf_FUNC_RESULT
          */
        [DllImport("NMF.dll")]
        public static extern short nmf_GetBrdAllStatus(short nNmfNo, out TNMF_ALL_STS ptRetAllSts);

        /**
          * @brief      ��ġ�� ����(Composition) ������ Ȯ���մϴ�.
          * @param[in]  nNmfNo          ��ġ��ȣ
          * @param[out] ptCompo         ���� ������ �о�� TNMF_COMPO ����ü ������
          * @return     Enmf_FUNC_RESULT
          */
        [DllImport("NMF.dll")]
        public static extern short nmf_GetCompo(short nNmfNo, out TNMF_COMPO ptCompo);

        /**
          * @brief      ����� ��� ��ġ�� ����(Composition) ������ Ȯ���մϴ�.
          * @param[in]  pnIp            �˻��� IP �뿪
          * @param[in]  nListCount      ��û ����
          * @param[out] ptCompo         ���� ������ �о�� TNMF_COMPO ����ü ������
          * @return     ���:����, ����:Enmf_FUNC_RESULT
          */
        [DllImport("NMF.dll")]
        public static extern short nmf_GetCompoList(short[] pnIp, short nListCount, TNMF_COMPO[] ptCompo);

        /**
          * @brief      ��Ʈ ������ �����մϴ�. (1000���� ������ Port�� ���ϴ� ��Ʈ�� ����)
          * @param[in]  nNmfNo          ��ġ��ȣ
          * @param[in]  lPortNum        ������ ��Ʈ��ȣ
          * @return     Enmf_FUNC_RESULT
          * @warning    ���� ��Ʈ��ũ���� ����ϴ� ���, ��Ʈ������ �ʿ����� �ʽ��ϴ�.
          *             ��ġ�� ��Ʈ ��ȣ�� ����Ǵ� ���� �ƴմϴ�.
          */
        [DllImport("NMF.dll")]
        public static extern short nmf_NetPortNumSet(short nNmfNo, int lPortNum);

        /**
          * @brief      ��ġ ��� ���� �ø��� ��� ����� Ȱ��ȭ�մϴ�.
          * @param[in]  nNmfNo          ��ġ��ȣ
          * @param[in]  nMode           ��� Ȱ��ȭ(0=��Ȱ��ȭ, 1=Ȱ��ȭ)
          * @return     Enmf_FUNC_RESULT
          */
        [DllImport("NMF.dll")]
        public static extern short nmf_SerialSetBypass(short nNmfNo, short nMode);

        /**
          * @brief      �ø��� ��� ȯ���� �����մϴ�.
          * @param[in]  nNmfNo          ��ġ��ȣ
          * @param[in]  nBaud           ��żӵ�(0=9600, 1=19200, 2=38400 bps)
          * @param[in]  nData           ������ ��Ʈ �� (������ 0~7 = 1~8 bit)
          * @param[in]  nStop           ���� ��Ʈ �� (0 = 1, 1 = 2)
          * @param[in]  nParity         Parity ��Ʈ (0=None, 1=Odd, 2=Even)
          * @return     Enmf_FUNC_RESULT
          */
        [DllImport("NMF.dll")]
        public static extern short nmf_SerialSetCfg(short nNmfNo, short nBaud, short nData, short nStop, short nParity);

        /**
          * @brief      ��ġ�� �����͸� �����մϴ�.
          * @param[in]  nNmfNo          ��ġ��ȣ
          * @param[in]  nLen            ���� �������� ����Ʈ ��
          * @param[in]  szData          ������ ���� �迭 ������
          * @return     Enmf_FUNC_RESULT
          */
        [DllImport("NMF.dll")]
        public static extern short nmf_SerialWrite(short nNmfNo, short nLen, byte[] szData);

        /**
          * @brief      ��ġ�� ������ �����͸� Ȯ���մϴ�.
          * @param[in]  nNmfNo          ��ġ��ȣ
          * @param[out] pnReadLen       ���� �������� ����Ʈ ��(�ִ� 384bytes)
          * @param[out] szpRetData      ������ ������ ���� �迭 ������
          * @return     Enmf_FUNC_RESULT
          */
        [DllImport("NMF.dll")]
        public static extern short nmf_SerialRead(short nNmfNo, out short pnReadLen, byte[] szpRetData);

    };
};
//------------------------------------------------------------------------------

//DESCRIPTION  'NMF Windows Dynamic Link Library'     -- *def file* description ....