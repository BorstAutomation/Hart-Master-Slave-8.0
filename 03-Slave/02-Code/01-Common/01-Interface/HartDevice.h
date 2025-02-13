/*
 *          File: HartDevice.h (CDevice)
 *                Module used for some settings in the scope of the device.
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
#ifndef __device_h__
#define __device_h__

class CDevice
{
public:
    // Initialization
    static void            Init();
    static void UpdateTimeStamp();
};

#endif // __device_h__