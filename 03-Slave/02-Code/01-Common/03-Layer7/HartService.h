/*
 *          File: CHartService.h (CService)
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

#ifndef __hartservice_h__
#define __hartservice_h__

class CFrame;
class CService
{
public:
    // Enum classes
    enum class EN_Status : TY_Byte
    {
        IDLE = 0,
        REQUESTED = 1,
        BUSY = 2,
        XMT_RESPONSE = 3,
        XMT_ERR = 4,
        WAITING = 5,
        KILL = 6
    };

    enum class EN_SubStat : TY_Byte
    {
        IDLE = 0,
        RECEIVING = 1,
        SENDING = 2
    };

    enum class EN_Type : TY_Byte
    {
        SEND_BURST = 0,
        RECEIVE_SEND = 1
    };

    // Public data
    // From the protcol
    CFrame           Request;
    EN_Master        Master;
    // To the protocol
    CFrame           Response;
    // There  is only one burst frame for all services
    static CFrame    Burst;
    EN_SRV_Result    CompletionCode;
    // Shared
    EN_Owner         Owner;
    EN_Status        Status;
    TY_Word          Command;
    TY_Byte          ErrRespCode;
    // There is only one active service
    static CService* ActiveService;

    // Construction/Destruction
    CService();

    static void  ReleaseActiveService();

    // Operation
    static void            PassToUser(CFrame* work_frame);
    static void        PassToProtocol();
    // Handle responses
    static void      MakeErrorResponse(TY_Byte err_);
    static void           MakeResponse();
    static void        PrepareResponse();
    static void       FinalizeResponse();
    static void           SendHartIpMessage();
    static void        SendRespErrCode(TY_Byte error_);
    static void   SetConfigChangedFlag();
    static void ClearConfigChangedFlag(EN_Master master_);

private:
    /* Data */
    EN_SubStat     m_sub_status;
    EN_Type        m_type;
    TY_DWord       m_start_time;
    TY_DWord       m_duration;
};

#endif // __hartservice_h__