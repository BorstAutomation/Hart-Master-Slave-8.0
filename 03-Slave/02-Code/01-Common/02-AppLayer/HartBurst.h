/*
 *          File: HartBurst.h (CBurst)
 *                Handling of the burst mode from the
 *                perspective of the application.
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
#ifndef __burst_h__
#define __burst_h__

class CBurst
{
public:
    // Initialization
    static void     Config(TY_Byte command);
    static void     Launch();
private:
    static TY_Byte InsertDevVar(EN_DevVarCode code_, TY_Byte* buffer_);
};

#endif // __burst_h__