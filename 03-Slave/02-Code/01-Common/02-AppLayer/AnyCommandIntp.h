/*
 *          File: AnyCommandIntp.h  (CAnyCommandIntp)
 *                Any command interpreter for
 *                common practice and user commands
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

#ifndef __any_command_intp_h__
#define __any_command_intp_h__

class CAnyCommandIntp
{
public:
    static TY_Word Execute();
private:
    static void VerifyDeviceStatus();

    class Command054
    {
    public:
        static EN_Error      FromRequest(TY_Byte* payload_data_, TY_Byte payload_size_);
        static void          ToResponse();
    private:
        static EN_DevVarCode m_code;
        static void          InsertDevVar(EN_DevVarCode code_, TY_Byte* buffer_);
    };

    class Command049
    {
    public:
        static EN_Error      FromRequest(TY_Byte* payload_data_, TY_Byte payload_size_);
        static void          ToResponse();
    };

    class Command035
    {
    public:
        static EN_Error      FromRequest(TY_Byte* payload_data_, TY_Byte payload_size_);
        static void          ToResponse();
    };

    class Command034
    {
    public:
        static EN_Error      FromRequest(TY_Byte* payload_data_, TY_Byte payload_size_);
        static void          ToResponse();
    };

    class Command033
    {
    public:
        static EN_Error      FromRequest(TY_Byte* payload_data_, TY_Byte payload_size_);
        static void          ToResponse();
    private:
        static TY_Byte m_num_slots;
        static TY_Byte m_slots[4];
        static void InsertDevVar(EN_DevVarCode code_, TY_Byte* buffer_);
    };

    class Command108
    {
    public:
        static EN_Error FromRequest(TY_Byte* payload_data_, TY_Byte payload_size_);
        static void      ToResponse();
    };

    class Command109
    {
    public:
        static EN_Error FromRequest(TY_Byte* payload_data_, TY_Byte payload_size_);
        static void      ToResponse();
    };

    class Command512
    {
    public:
        static void          ToResponse();
    };

    class Command513
    {
    public:
        static EN_Error      FromRequest(TY_Byte* payload_data_, TY_Byte payload_size_);
        static void          ToResponse();
    };
};

#endif // __any_command_intp_h__