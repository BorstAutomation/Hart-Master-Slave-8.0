/*
 *          File: CHartService.cpp (CService)
 *                In simple terms, a service executes a Hart command by
 *                receiving a request from Layer2 of the Hart protocol.
 *                In doing so, it returns a handle to the caller, with
 *                which the calling program can check the status.
 *                The slave implementation is using only two services.
 *                One for the primary master and one for the secondary.
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

#include "WbHartSlave.h"
#include "HartService.h"
#include "HartChannel.h"

// Public data
CService* CService::ActiveService;
CFrame    CService::Burst;

// Construktor
CService::CService()
{
    Master = EN_Master::PRIMARY;
    CompletionCode = EN_SRV_Result::EMPTY;
    
    Owner = EN_Owner::PROTOCOL;
    Status = EN_Status::IDLE;
    Command = 0xffff;
    ErrRespCode = 0;

    // private data
    m_sub_status = EN_SubStat::IDLE;
    m_type = EN_Type::RECEIVE_SEND;
    m_start_time = 0;
    m_duration = 0;
}

void CService::PassToUser(CFrame* work_frame_)
{
    CService* service_ref;
    CFrame*   work_frame_ref = work_frame_;

    if (work_frame_ref->GetRemoteMaster() == EN_Bool::TRUE8)
    {
        service_ref = &CChannel::PrimMasterService;
        service_ref->Master = EN_Master::PRIMARY;
    }
    else
    {
        service_ref = &CChannel::ScndMasterService;
        service_ref->Master = EN_Master::SECONDARY;
    }

    service_ref->Command = work_frame_ref->Command;
    // Copy frame to service
    service_ref->Request = *work_frame_ref;
    // Handle cmd 31 (extended command)
    if (service_ref->Command == 31)
    {
        // Extended command expected in payload
        if (work_frame_ref->PayloadCount >= 2)
        {
            // Cut out the extended command
            service_ref->Command = CCoding::PickWord(0, work_frame_ref->PayloadData, EN_Endian::MSB_First);
            service_ref->Request.PayloadSize = work_frame_ref->PayloadCount - 2;
            service_ref->Request.PayloadCount = service_ref->Request.PayloadSize;
            if (service_ref->Request.PayloadSize > 0)
            {
                COSAL::CMem::Copy(service_ref->Request.PayloadData, &work_frame_ref->PayloadData[2], service_ref->Request.PayloadSize);
            }
        }
        else
        {
            // Invalid payload count (size), set to 0
            service_ref->Request.PayloadSize = 0;
            service_ref->Request.PayloadCount = 0;
        }
    }
    else
    {
        // Copy payload data if any
        if (work_frame_ref->PayloadCount > 0)
        {
            COSAL::CMem::Copy(service_ref->Request.PayloadData, work_frame_ref->PayloadData, work_frame_ref->PayloadCount);
            service_ref->Request.PayloadSize = work_frame_ref->PayloadCount;
        }
    }
     
    // Reset the work frame
    work_frame_ref->Init();
    // Configure
    service_ref->Status = CService::EN_Status::REQUESTED;
    service_ref->Owner = EN_Owner::USER;
    CService::ActiveService = service_ref;
}

void CService::PassToProtocol()
{
    if (ActiveService->Command > 255)
    {
        TY_Byte* data = ActiveService->Response.PayloadData;

        // Insert the extended command into the response
        if (ActiveService->Response.PayloadSize > 0)
        {
            // Shift payload data by 2 positions
            // Begin copy at the end of the data
            for (TY_Word i = ActiveService->Response.PayloadSize + 1; i > 1; i--)
            {
                data[i] = data[i - 2];
            }
        }

        // Insert the extended command at the gap
        CCoding::PutWord(ActiveService->Command, 0, &data[0], EN_Endian::MSB_First);
        // New size of the response
        ActiveService->Response.PayloadSize += 2;

    }

    ActiveService->Owner = EN_Owner::PROTOCOL;
}

void CService::ReleaseActiveService()
{
    ActiveService->Status = EN_Status::IDLE;
    ActiveService = NULL;
}

// Response handling
void CService::MakeErrorResponse(TY_Byte err_)
{
    PrepareResponse();
    if (ActiveService->Command > 255)
    {
        // Command was inserted
        ActiveService->Response.PayloadSize = 2;
    }
    else
    {
        ActiveService->Response.PayloadSize = 0;
    }

    ActiveService->Response.CmdRespCode = err_;
    FinalizeResponse();
}

void CService::MakeResponse()
{
    PrepareResponse();
    FinalizeResponse();
}

void CService::PrepareResponse()
{
    // Do not copy payload because
    // this was already set by the 
    // command interpreter
    ActiveService->Response.DoNotCopyPayload = EN_Bool::TRUE8;
    // Copy request to response
    ActiveService->Response = ActiveService->Request;
    // Set master
    if (ActiveService->Response.GetRemoteMaster() == EN_Bool::TRUE8)
    {
        ActiveService->Response.Host = EN_Master::PRIMARY;
    }
    else
    {
        ActiveService->Response.Host = EN_Master::SECONDARY;
    }

    // Addressing type
    ActiveService->Response.AddrMode = ActiveService->Request.AddrMode;
    // Num preambles
    ActiveService->Response.NumPreambles = CHartData::CStat.NumResponsePreambles;
    // Command
    ActiveService->Response.Command = ActiveService->Request.Command;
    // Set Type
    ActiveService->Response.Type = CFrame::EN_Type::RESPONSE;
}

void CService::FinalizeResponse()
{
    // Encode response frame to tx_buffer
    ActiveService->Response.Encode();
}

void CService::SendHartIpMessage()
{
    ActiveService->Status = CService::EN_Status::XMT_RESPONSE;
    CService::PassToProtocol();
}

void CService::SendRespErrCode(TY_Byte error_)
{
    ActiveService->ErrRespCode = error_;
    ActiveService->Status = CService::EN_Status::XMT_ERR;
    ActiveService->Response.PayloadSize = 0;
    CService::PassToProtocol();
}

// Status info
void CService::SetConfigChangedFlag()
{
    // Is always signalled to both masters
    CHartData::CDyn.DeviceStatusPrimary |= CHart::CDevStatus::CFG_CHANGED;
    CHartData::CDyn.DeviceStatusSecondary |= CHart::CDevStatus::CFG_CHANGED;
}

void CService::ClearConfigChangedFlag(EN_Master master_)
{
    if (master_ == EN_Master::PRIMARY)
    {
        CHartData::CDyn.DeviceStatusPrimary &= (TY_Byte)~CHart::CDevStatus::CFG_CHANGED;
    }
    else
    {
        CHartData::CDyn.DeviceStatusSecondary &= (TY_Byte)~CHart::CDevStatus::CFG_CHANGED;
    }
}
