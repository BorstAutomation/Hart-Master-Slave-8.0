/*
 *          File: Monitor.h (CMonitor)
 *                The same applies to the Monitor function as to the MacPort.
 *                At this point only the interface can be defined.
 *                The implementation takes place in the specific part.
 *
 *        Author: Walter Borst
 *
 *        E-Mail: info@borst-automation.de
 *          Home: https://www.borst-automation.de
 *
 * No Warranties: https://www.borst-automation.com/legal/warranty-disclaimer
 *
 * Copyright 2006-2025 Walter Borst, Cuxhaven, Germany
 */

// Once
#ifndef __monitor_h__
#define __monitor_h__

// Data used for the monitor function
#pragma pack(push, 1)
typedef struct ty_mon_frame
{
    TY_DWord      StartTime;
    TY_DWord        EndTime;
    TY_Word             Len;
    EN_Bool  IsFrameStarted;
    // .0: Gap TO, .1: Client Tx, .2: Slave Tx 
    TY_Byte          Detail;
    EN_Bool    IsValidFrame;
    EN_Bool  IsReceiveReady;
    TY_Byte     BytesOfData[MON_MAX_FRAME_DATA_SIZE];
}
TY_MonFrame;
#pragma pack(pop)


class CMonitor
{
public:
    class CDetail
    {
    public:
        static const TY_Byte    GAP_TO = 0x01;
        static const TY_Byte CLIENT_TX = 0x02;
    };

    // Support of external functions
    static void          Init();
    static void     Terminate();
    static void         Start();
    static void          Stop();
    static EN_Bool    GetData(TY_MonFrame* mon_frame_);
    static EN_Bit   GetStatus();

    // Operation
    static EN_Bool            IsActive();
    static void           StartReceive(TY_DWord start_time_);
    static void          StartTransmit(TY_DWord start_time_);
    static void              StoreData(TY_Byte* data_, TY_Word len_);
    static void             RemoveData(TY_Word len_);
    static void            EndTransmit(TY_DWord end_time_);
    static void       EndRcvValidFrame(TY_DWord last_rcv_evt_time_);
    static void            EndRcvGapTO(TY_DWord last_rcv_evt_time_);
    static TY_DWord       GetStartTime();
    static TY_Word          GetDataLen();
    static void           AbortReceive();
    static TY_Word   GetPayloadData(TY_Byte* data_);
    static void      SetAdditionalData(TY_Byte* data_, TY_Word data_len_);

    // Management
    static void     ResetReceive();

    /* Data */
    static EN_Bool     m_is_monitor_active;
    static TY_Word     m_wr_idx;
    static TY_Word     m_rd_idx;
    static TY_MonFrame m_mon_frames[MON_MAX_NUM_FRAMES];
    static TY_Byte     m_additional_data[MAX_IP_TXRX_SIZE];
    static TY_Word     m_additional_data_len;
};
#endif // __monitor_h__