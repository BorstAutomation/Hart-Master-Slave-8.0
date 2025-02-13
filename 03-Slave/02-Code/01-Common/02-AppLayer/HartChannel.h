/*
 *          File: HartChannel.h (CChannel)
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

#ifndef __chart_channel_h__
#define __chart_channel_h__

class CService;
class CChannel
{
    // Since only one channel is initially supported in this
    // version of the implementation, the channel class is
    // purely static.
public:
    // Data
    static EN_Bool  HartEnabledChanged;
    static EN_Bool  BurstModeChanged;
    static CService PrimMasterService; // SRV_Handle == 0
    static CService ScndMasterService; // SRV_handle == 1

    // Methods
    static EN_Bool                   Open(TY_Word port_number_, EN_CommType type_);
    static void                     Close();
    static void                      Init();

    // Test Information
    static TY_Word GetHartIpStatus();

    // Service handling
    static SRV_Handle  WasCommandReceived();

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

public:
    // Get Configuration
    static TY_DWord            GetBaudrate(void);
    static EN_Bool           IsHartEnabled(void);
    static EN_Bool             IsBurstMode(void);
    static EN_CommType         GetCommType(void);

    class CServiceEvent
    {
    public:
        static const TY_Word             NONE = 0;
        static const TY_Word     CONFIRMATION = 1;
        static const TY_Word BURST_INDICATION = 2;
        static const TY_Word          REQUEST = 3;
    };
};

#endif // __chart_channel_h__
