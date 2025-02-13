/*
 *          File: WbHartUser.h
 *                Limits applied by the user of the hart master
 *                software
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
#ifndef __wbhartuser_h__
#define __wbhartuser_h__

#include "WbHart_Typedefs.h"

// Buffer sizes
static const TY_Byte MAX_STRING_LEN = 64;
static const TY_Byte MAX_PAYLOAD_SIZE = 64;
static const TY_Byte MAX_TXRX_SIZE = 128;
static const TY_Word MAX_IP_TXRX_SIZE = 512;
static const TY_Word HART_IP_HEADER_LEN = 8;
static const TY_Byte MAX_CYCLIC_DATA_SIZE = 128;

// Numbers of objects
static const TY_Byte MAX_NUM_SERVICES = 10;
static const TY_Byte MAX_NUM_PAYLOAD_BUFFERS = 20;
static const TY_Byte MAX_NUM_TXRX_BUFFERS = 20;
static const TY_Byte MAX_NUM_CYCLIC_BUFFERS = 10;

// Identifier
static const TY_Byte MAX_COMPORT_ID = 254;

// Monitor
static const TY_Word MON_MAX_FRAME_DATA_SIZE = 256;
static const TY_Word MON_MAX_NUM_FRAMES = 1000;

#endif // __wbhartuser_h__
