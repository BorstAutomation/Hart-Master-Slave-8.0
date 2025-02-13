/*
 *          File: HartData.h (CHartData)
 *                Data defined for the Hart commands
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

#ifndef __hart_data_h__
#define __hart_data_h__

class CHartData
{
public:
    static TY_ConstDataHart CConst;
    static TY_DynDataHart   CDyn;
    static TY_StatDataHart  CStat;
    static const TY_Byte    NOT_A_NUMBER[4];

    static void SetDefaults();
};

#endif // __hart_data_h__

