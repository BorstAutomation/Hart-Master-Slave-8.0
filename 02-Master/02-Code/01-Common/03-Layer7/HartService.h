/*
 *          File: CHartService.h (CService)
 *                In simple terms, a service executes a Hart command by
 *                passing a request to Layer2 of the Hart protocol.
 *                In doing so, it returns a handle to the caller, with
 *                which the calling program can check the status.
 *                A service is only considered completed when the caller
 *                has fetched the data (e.g. FetchConfirmation).
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
        WAITING = 3,
        KILL = 4
    };

    enum class EN_SubStat : TY_Byte
    {
        IDLE = 0,
        SENDING = 1,
        RECEIVING = 2
    };

    enum class EN_Type : TY_Byte
    {
        HART = 0,
        SEND_BURST = 1,
        SEND_ONLY = 2,
        SEND_RECEIVE = 3
    };

    enum class EN_Mode : TY_Byte
    {
        NORMAL = 0,
        SEND_BURST = 1
    };

    /* Construction/Destruction */
    void                       Init();
    void                    Release();
    EN_Bool                IsActive();
    void                      Clear();
    /* Operation */
    SRV_Handle            GetHandle();
    void                  SetHandle(SRV_Handle service_);
    EN_Status             GetStatus();
    void                  SetStatus(EN_Status status_);
    EN_SubStat         GetSubStatus();
    void               SetSubStatus(EN_SubStat status_);
    EN_Type                 GetType();
    void                    SetType(EN_Type type_);
    EN_Mode                 GetMode();
    void                    SetMode(EN_Mode mode_);
    EN_Owner               GetOwner();
    void                   SetOwner(EN_Owner owner_);
    TY_Byte*              GetTxData(TY_Word* len_);
    void          SetCompletionCode(EN_SRV_Result code_);
    EN_SRV_Result GetCompletionCode();
    void                     Launch();
    void               SetLastEvent(TY_Byte event_);
    TY_Byte            GetLastEvent();
    // Set Request Data
    void                 SetCommand(TY_Byte command_);
    void                SetAddrMode(TY_Byte addr_mode_);
    void             SetRetryIfBusy(EN_Bool retry_if_busy_);
    void              SetNumRetries(TY_Byte num_retries_);
    void            SetNumPreambles(TY_Byte num_preambles_);
    void                    SetData(TY_Byte* data_, TY_Byte len_);
    void                SetUniqueID(TY_Byte* bytes_of_unique_id_);
    void               SetShortAddr(TY_Byte short_addr_);
    void                SetShortTag(TY_Byte* short_tag_);
    void                 SetLongTag(TY_Byte* long_tag_);
    void               SetInvMaster(EN_Bool inv_master_);
    // Get Response Data
    TY_Byte              Command();
    void                SetResponse(CFrame* frame_);
    // -------------------------------------
    EN_Bool        IsRetryPermitted();
    EN_Bool            IsInProgress();
    EN_Bool             RetryIfBusy();
    EN_Bool                  Failed();
    void            ClearRetryCount();
    void              IncRetryCount();
    TY_Byte              GetRespLen();
    TY_Byte            GetRespCode1();
    TY_Byte            GetRespCode2();
    TY_Byte         GetRespDataByte(TY_Byte idx_);
    EN_Bool    GetDeviceInBurstMode();
    TY_Byte          GetUsedRetries();
    TY_Word             GetDuration();
    TY_Byte             GetRespData(TY_Byte* pu8_Data);
    TY_Byte              GetRespCmd();
protected:
private:
    /* Data */
    EN_Status      Status;
    EN_SubStat     m_sub_status;
    EN_Type        m_type;
    EN_Mode        m_mode;
    SRV_Handle     m_handle;
    TY_DWord       m_app_key;
    EN_Bool        m_retry_if_busy;
    TY_Byte        m_num_retries;
    EN_SRV_Result  m_completion_code;
    TY_DWord       m_start_time;
    TY_DWord       m_duration;
    TY_Byte        m_retry_count;
    TY_Byte        m_req_cmd;
    CFrame         m_request;
    CFrame         m_response;
    EN_Bool        m_is_active;
    EN_Owner       m_owner;
    TY_Byte        m_last_event;
};

#endif // __hartservice_h__