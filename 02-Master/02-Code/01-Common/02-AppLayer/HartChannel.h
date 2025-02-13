/*
 *          File: CHartChannel.h (CChannel)
 *                The channel manages a communication interface and the
 *                associated propperties. The channel also uses services
 *                to conduct Hart commands.
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

class CService;
class CChannel
{
    // Since only one channel is initially supported in this
    // version of the implementation, the channel class is
    // purely static.
public:


    static EN_Bool                   Open(TY_Word port_number_, EN_CommType type_);
    static void                     Close();
    static EN_Bool                 IsOpen();
    static void                      Init();
    // Service handling
    static SRV_Handle       GetNewService();
    static CService*        GetServicePtr(SRV_Handle handle_);
    static void            ReleaseService(SRV_Handle handle_);
    static EN_Bool         IsValidService(SRV_Handle handle_);
    static EN_Bool     IsServiceCompleted(SRV_Handle handle_);
    static void           SetServiceOwner(SRV_Handle handle_, EN_Owner owner_);
    static EN_Owner       GetServiceOwner(SRV_Handle handle_);
    static void               FreeService(SRV_Handle handle_);
    static SRV_Handle GetRequestedService();

public:
    class CProtocol
    {
    public:
        static const TY_Byte     NONE = 0;
        static const TY_Byte   MASTER = 1;
        static const TY_Byte    SLAVE = 2;
    };
private:
    static EN_Bool        m_is_open;
    static EN_CommType    m_comm_type;
    static TY_Word        m_port_number;
    static TY_Byte        m_protocol;
    static COSAL::CTask   m_CTask_cyclic_50ms;
    static TY_Word        m_num_active_services;
    static CService       m_CService_pool[MAX_NUM_SERVICES];
    static TY_DWord       m_baudrate;
    static TY_Byte        m_num_preambles;
    static TY_Byte        m_num_retries;
    static EN_Bool        m_retry_if_busy;
    static EN_Master      m_master_type;
    static TY_Byte        m_addressing_mode;
    static EN_Bool        m_hart_enabled;
    static TY_Byte        m_hart_version;
    static EN_Master      m_local_master;
    static TY_Word        m_next_srv_search_idx;
public:
    // HartIp globals
    static TY_Byte        HartIpHostName[MAX_STRING_LEN];
    static TY_Byte        HartIpAddress[MAX_STRING_LEN];
    static TY_Byte        HartIpPort[MAX_STRING_LEN];
    static EN_Bool        HartIpUseAddress;
    static EN_Bool        HartIpEnabled;
    static EN_Bool        HartIpSendKeepAlive;
    /* Get Configuration */
    static TY_DWord            GetBaudrate(void);
    static TY_Byte         GetNumPreambles(void);
    static TY_Byte           GetNumRetries(void);
    static EN_Bool          GetRetryIfBusy(void);
    static EN_Master         GetMasterType(void);
    static TY_Byte       GetAddressingMode(void);
    static EN_Bool          GetHartEnabled(void);
    static TY_Byte          GetHartVersion(void);
    static EN_Bool         IsPrimaryMaster(void);
    static void        FetchHartIpHostName(TY_Byte* hart_ip_host_name_);
    static void         FetchHartIpAddress(TY_Byte* hart_ip_address_);
    static void         FetchHartIpPort(TY_Byte* hart_ip_address_);
    static TY_Word           GetHartIpPort(void);
    static EN_Bool     GetHartIpUseAddress(void);
    static EN_CommType         GetCommType(void);
    static TY_Word         GetHartIpStatus(void);

    /* Set Configuration */
    static void                SetBaudrate(TY_DWord baudrate_);
    static void            SetNumPreambles(TY_Byte num_preambles_);
    static void              SetNumRetries(TY_Byte num_retries_);
    static void             SetRetryIfBusy(EN_Bool retry_if_busy_);
    static void              SetMasterType(EN_Master master_type_);
    static void          SetAddressingMode(TY_Byte addressing_mode_);
    static void             SetHartEnabled(EN_Bool enabled_);
    static void             SetHartVersion(TY_Byte hart_version_);
    static void           FireServiceEvent(TY_Byte event_, SRV_Handle handle_, TY_DWord data_);
    static void          SetHartIpHostName(TY_Byte* hart_ip_host_name_);
    static void           SetHartIpAddress(TY_Byte* hart_ip_address_);
    static void              SetHartIpPort(TY_Byte* hart_ip_port_);
    static void        SetHartIpUseAddress(EN_Bool hart_ip_use_address_);

    /* Cyclic Data Handling */
    static void              BurstIndicate(CFrame* frame_);

    class CServiceEvent
    {
    public:
        static const TY_Word             NONE = 0;
        static const TY_Word     CONFIRMATION = 1;
        static const TY_Word BURST_INDICATION = 2;
        static const TY_Word          REQUEST = 3;
    };
};
