/*
 *          File: UniCommandIntp.h (CUniCommandIntp)
 *                Universal commands interpreter
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

#ifndef __uni_command_intp_h__
#define __uni_command_intp_h__

class CUniCommandIntp
{
public:
    static EN_Bool Execute();
    static EN_Error PrecheckCommand(TY_Byte min_payload_, TY_Byte payload_size_);
private:
    class Command000
    {
    public:
        static void ToResponse();
    };

    class Command001
    {
    public:
        static void ToResponse();
    };

    class Command002
    {
    public:
        static void ToResponse();
    };

    class Command003
    {
    public:
        static void ToResponse();
    };

    class Command006
    {
    public:
        static EN_Error FromRequest(TY_Byte * payload_data_, TY_Byte payload_size_);
        static void      ToResponse();
    };

    class Command007
    {
    public:
        static void      ToResponse();
    };

    class Command008
    {
    public:
        static void      ToResponse();
    };

    class Command009
    {
    public:
        static EN_Error FromRequest(TY_Byte* payload_data_, TY_Byte payload_size_);
        static void      ToResponse();
    private:
        static TY_Byte m_num_slots;
        static TY_Byte m_slots[8];
        static TY_Byte InsertDevVar(EN_DevVarCode code_, TY_Byte* buffer_);
    };

    class Command011
    {
    public:
        static void      ToResponse();
    };

    class Command012
    {
    public:
        static void ToResponse();
    };

    class Command013
    {
    public:
        static void ToResponse();
    };

    class Command014
    {
    public:
        static void ToResponse();
    };

    class Command015
    {
    public:
        static void ToResponse();
    };

    class Command016
    {
    public:
        static void ToResponse();
    };

    class Command017
    {
    public:
        static EN_Error FromRequest(TY_Byte* payload_data_, TY_Byte payload_size_);
        static void      ToResponse();
    };

    class Command018
    {
    public:
        static EN_Error FromRequest(TY_Byte* payload_data_, TY_Byte payload_size_);
        static void      ToResponse();
    };

    class Command019
    {
    public:
        static EN_Error FromRequest(TY_Byte* payload_data_, TY_Byte payload_size_);
        static void      ToResponse();
    };

    class Command020
    {
    public:
        static void ToResponse();
    };

    class Command021
    {
    public:
        static void      ToResponse();
    };

    class Command022
    {
    public:
        static EN_Error FromRequest(TY_Byte* payload_data_, TY_Byte payload_size_);
        static void      ToResponse();
    };

    class Command038
    {
    public:
        static EN_Error FromRequest(TY_Byte* payload_data_, TY_Byte payload_size_);
        static void      ToResponse();
    };

    class Command048
    {
    public:
        static EN_Error FromRequest(TY_Byte* payload_data_, TY_Byte payload_size_);
        static void      ToResponse();
    };

    static void SendUniqueIdResponse();
    static void SendTagDescrDateResponse();
};
#endif // __uni_command_intp_h__

