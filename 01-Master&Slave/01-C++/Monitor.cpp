/*
 *          File: Monitor.cpp (CMonitor)
 *                On the one hand, there are methods that are mapped to the
 *                interface of the Windows DLL. In addition, there are a
 *                number of functions that are included with the kernel
 *                functions. Since this module is so small overall, the
 *                methods were not implemented in two different files.
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
 
#include "OSAL.h"
#include "WbHartUser.h"
#include "Monitor.h"

// Data

EN_Bool                             CMonitor::m_is_monitor_active;
TY_Word                             CMonitor::m_wr_idx;
TY_Word                             CMonitor::m_rd_idx;
TY_MonFrame                         CMonitor::m_mon_frames[MON_MAX_NUM_FRAMES];
TY_Byte                             CMonitor::m_additional_data[MAX_IP_TXRX_SIZE];
TY_Word                             CMonitor::m_additional_data_len = 0;

// Methods

void CMonitor::Init()
{
    ResetReceive();
}

void CMonitor::Terminate()
{
}

void CMonitor::Start()
{
    m_is_monitor_active = EN_Bool::TRUE8;
}

void CMonitor::Stop()
{
    m_is_monitor_active = EN_Bool::FALSE8;
}

EN_Bool CMonitor::GetData(TY_MonFrame* mon_frame_)
{
    TY_MonFrame* active_frame = &m_mon_frames[m_rd_idx];

    if (m_rd_idx != m_wr_idx)
    {
        if (active_frame->IsReceiveReady == EN_Bool::TRUE8)
        {
            *mon_frame_ = *active_frame;
            COSAL::CMem::Set((TY_Byte*)active_frame, 0, sizeof(TY_MonFrame));
            m_rd_idx += 1;
            if (m_rd_idx > MON_MAX_NUM_FRAMES)
            {
                m_rd_idx = 0;
            }
            return EN_Bool::TRUE8;
        }
    }

    return EN_Bool::FALSE8;
}

EN_Bit CMonitor::GetStatus()
{
    if (m_is_monitor_active == EN_Bool::TRUE8)
    {
        return EN_Bit::SET8;
    }

    return EN_Bit::CLEAR8;
}

EN_Bool CMonitor::IsActive()
{
    if (m_is_monitor_active == EN_Bool::TRUE8)
    {
        return EN_Bool::TRUE8;
    }

    return EN_Bool::FALSE8;
}

void CMonitor::StartReceive(TY_DWord start_time_)
{
        TY_MonFrame* active_frame = &m_mon_frames[m_wr_idx];

        COSAL::CMem::Set((TY_Byte*)active_frame, 0, sizeof(TY_MonFrame));
        active_frame->StartTime = start_time_;
        active_frame->IsFrameStarted = EN_Bool::TRUE8;
}

void CMonitor::StartTransmit(TY_DWord start_time_)
{
    TY_MonFrame* active_frame = &m_mon_frames[m_wr_idx];

    COSAL::CMem::Set((TY_Byte*)active_frame, 0, sizeof(TY_MonFrame));
    active_frame->StartTime = start_time_;
    active_frame->IsFrameStarted = EN_Bool::TRUE8;
    active_frame->Detail |= CDetail::CLIENT_TX;
}

void CMonitor::StoreData(TY_Byte* pu8_Data, TY_Word len_)
{
    if (len_ > 0)
    {
        TY_MonFrame* active_frame = &m_mon_frames[m_wr_idx];

        if (active_frame->IsFrameStarted == EN_Bool::TRUE8)
        {
            if ((active_frame->Len + len_) <= MON_MAX_FRAME_DATA_SIZE)
            {
                COSAL::CMem::Copy(&active_frame->BytesOfData[active_frame->Len], pu8_Data, len_);
                active_frame->Len += len_;
            }
        }
    }
}

void CMonitor::RemoveData(TY_Word len_)
{
    if (len_ > 0)
    {
        TY_MonFrame* active_frame = &m_mon_frames[m_wr_idx];

        if (active_frame->IsFrameStarted == EN_Bool::TRUE8)
        {
            if (active_frame->Len >= len_)
            {
                active_frame->Len -= len_;
            }
        }
    }
}

void CMonitor::AbortReceive()
{
    TY_MonFrame* active_frame = &m_mon_frames[m_wr_idx];

    active_frame->StartTime = 0;
    active_frame->EndTime = 0;
    active_frame->Len = 0;
    active_frame->IsFrameStarted = EN_Bool::FALSE8;
    active_frame->Detail = 0;
    active_frame->IsValidFrame = EN_Bool::FALSE8;
    active_frame->IsReceiveReady = EN_Bool::FALSE8;
}

void CMonitor::EndTransmit(TY_DWord EndTime)
{
    TY_MonFrame* active_frame = &m_mon_frames[m_wr_idx];

    if (active_frame->IsFrameStarted == EN_Bool::TRUE8)
    {
        active_frame->EndTime = EndTime;
        m_wr_idx += 1;
        if (m_wr_idx >= MON_MAX_NUM_FRAMES)
        {
            m_wr_idx = 0;
        }
        active_frame->IsValidFrame = EN_Bool::TRUE8;
        active_frame->IsReceiveReady = EN_Bool::TRUE8;
    }
}

void CMonitor::EndRcvValidFrame(TY_DWord u32_LastRcvEvtTime)
{
    TY_MonFrame* active_frame = &m_mon_frames[m_wr_idx];

    if (active_frame->IsFrameStarted == EN_Bool::TRUE8)
    {
        active_frame->EndTime = u32_LastRcvEvtTime;
        m_wr_idx += 1;
        if (m_wr_idx >= MON_MAX_NUM_FRAMES)
        {
            m_wr_idx = 0;
        }
        active_frame->IsValidFrame = EN_Bool::TRUE8;
        active_frame->IsReceiveReady = EN_Bool::TRUE8;
    }
}

void CMonitor::EndRcvGapTO(TY_DWord last_rcv_evt_time_)
{
    TY_MonFrame* active_frame = &m_mon_frames[m_wr_idx];

    if (active_frame->IsFrameStarted == EN_Bool::TRUE8)
    {
        if ((active_frame->IsValidFrame == EN_Bool::TRUE8) || (active_frame->IsReceiveReady == EN_Bool::TRUE8))
        {
            // GAP Failed
            return;
        }

        active_frame->EndTime = last_rcv_evt_time_;
        m_wr_idx += 1;
        if (m_wr_idx >= MON_MAX_NUM_FRAMES)
        {
            m_wr_idx = 0;
        }
        active_frame->Detail |= CDetail::GAP_TO;
        active_frame->IsReceiveReady = EN_Bool::TRUE8;
    }
}

TY_DWord CMonitor::GetStartTime()
{
    TY_MonFrame* active_frame = &m_mon_frames[m_wr_idx];

    if (active_frame->IsFrameStarted == EN_Bool::TRUE8)
    {
        return active_frame->StartTime;
    }

    return 0;
}

TY_Word CMonitor::GetDataLen()
{
    TY_MonFrame* active_frame = &m_mon_frames[m_wr_idx];

    if (active_frame->IsFrameStarted == EN_Bool::TRUE8)
    {
        return active_frame->Len;
    }

    return 0;
}

void CMonitor::ResetReceive()
{
    m_rd_idx = 0;
    m_wr_idx = 0;
    COSAL::CMem::Set((TY_Byte*)&m_mon_frames[0], 0, sizeof(m_mon_frames));
}

TY_Word CMonitor::GetPayloadData(TY_Byte* data_)
{
    COSAL::CMem::Copy(data_, m_additional_data, m_additional_data_len);
    return m_additional_data_len;
}

void CMonitor::SetAdditionalData(TY_Byte* data_, TY_Word data_len_)
{
    COSAL::CMem::Copy(m_additional_data, data_, data_len_);
    m_additional_data_len = data_len_;
}
