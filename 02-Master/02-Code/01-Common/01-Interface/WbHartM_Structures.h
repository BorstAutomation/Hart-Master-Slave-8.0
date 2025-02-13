/*
 *          File: WbHartM_Structures.h
 *                This file contains structures which are accessed 
 *                at the outer interface as well as in some modules
 *                in the master kernel.
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
#ifndef __wbhartm_structures_h__
#define __wbhartm_structures_h__

#include "WbHart_Typedefs.h"
#include "WbHartUser.h"

// Structure Definitions

// Data for the driver configuration
#pragma pack(push, 1)
typedef struct ty_configuration
{
    // Bitrate as defined in Windows, default: 1200
    TY_Int32          BaudRate;

    // Number of preambles used for a request (0..22), default: 5
    TY_Byte       NumPreambles;

    // Number of retries if device response is erroneous, default: 2
    TY_Byte         NumRetries;

    // FALSE8: Do not retry if device is responding with busy code
    //  TRUE8: Retry the command if device is responding with busy code.
    //         The number of retries is reflected in the confirmation as <c>ucUsedRetries</c>
    EN_Bool        RetryIfBusy;

    // 0: Secondary master, 1: Primary master, Default: 0
    EN_Master       MasterType;

    // Addressing mode, 2: Use short address, other: Use long address
    TY_Byte     AddressingMode;

    // Hart IP
    TY_Byte     HartIpHostName[MAX_STRING_LEN];
    TY_Byte      HartIpAddress[MAX_STRING_LEN];
    TY_Byte         HartIpPort[MAX_STRING_LEN];
    EN_Bool   HartIpUseAddress;
    EN_Bool  HartIpDataChanged;
    EN_Bool  HartIpSendKeepAlive;
}
TY_Configuration;
#pragma pack(pop)

// Data assoziated with the connection
#pragma pack(push, 1)
typedef struct ty_connection
{
    // Manufacturer id as defined by the Hart Communication Foundation
    TY_Byte                   ManuId;

    // Vendor's device id
    TY_Byte                    DevId;

    // Number of preambles to be sent with a response
    TY_Byte             NumPreambles;

    // Command set revision number as defined by Hart, 5 or 6 or 7
    TY_Byte                CmdRevNum;

    // Device specific revision code
    TY_Byte              SpecRevCode;

    // Software revision code
    TY_Byte                    SwRev;

    // Hardware revision code
    TY_Byte                    HwRev;

    // Hart flags
    TY_Byte                HartFlags;

    // Service completion code
    // 0: Not active, 1: No dev response, 2: Some comm err, 3: Invalid handle
    // 4: Service in progress, 5: Successfully completed, 6: Out of resource,
    // 7: Reserved for cmd 31
    TY_Byte            SrvResultCode;

    // Command specific response code
    TY_Byte                RespCode1;

    // Device status
    TY_Byte                RespCode2;

    // Number of retries to complete service
    TY_Byte              UsedRetries;

    // Device in burst mode indication, 1: Device in burst mode
    TY_Byte        DeviceInBurstMode;

    // Extended device status
    TY_Byte             ExtDevStatus;

    // Configuration changed counter
    TY_Word               CfgChCount;

    // Minimum number of preambles for the receiving a request
    TY_Byte            MinNumPreambs;

    // Maximum number of device variables
    TY_Byte                MaxNumDVs;

    // Extended manufacturer ID
    TY_Word                ExtManuID;

    // Extended label distributor code
    TY_Word             ExtLabDistID;

    // Device profile
    TY_Byte              EDevProfile;

    // The field is not used.
    TY_Byte                 Reserved;

    // Array for the unique identifier
    TY_Byte       BytesOfUniqueID[5];
}
TY_Connection;
#pragma pack(pop)

// Data from the response to a command
#pragma pack(push, 1)
typedef struct ty_confirmation
{
    // Received command
    TY_Byte                 Cmd;

    // Command specific response code (response byte 1)
    TY_Byte           RespCode1;

    // Device status (response byte 2)
    TY_Byte           RespCode2;

    // Service completion code
    // 0: Not active, 1: No dev response, 2: Some comm err, 3: Invalid handle
    // 4: Service in progress, 5: Successfully completed, 6: Out of resource,
    // 7: Reserved for cmd 31
    EN_SRV_Result SrvResultCode;

    // Number of retries to complete service
    TY_Byte         UsedRetries;

    // Device in burst mode indication, 1: Device in burst mode
    EN_Bool   DeviceInBurstMode;

    // Duration of the service conduction in milliseconds
    TY_Word         SrvDuration;

    // The 16 bit command code (if any)
    TY_Word          ExtCommand;

    // This field is not used.
    TY_Byte            Reserved;

    // Length of payload data
    TY_Byte             DataLen;

    // Byte array for the payload data
    TY_Byte         BytesOfData[MAX_PAYLOAD_SIZE];
}
TY_Confirmation;
#pragma pack(pop)

#endif // __wbhartm_structures_h__