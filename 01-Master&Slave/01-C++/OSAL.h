/*
 *          File: OSAL.h (COSAL)
 *                The Operating System Abstraction Layer is the top header.
 *                This is where the central connection to the respective
 *                hardware or software platform takes place.
 *                The header OSAL.h can only exist once, while a special
 *                implementation (OSAL.cpp) exists for each specific
 *                hardware or software.
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
#ifndef __osal_h__
#define __osal_h__

#include "WbHart_Typedefs.h"
#include "WbHartUser.h"

// Further typedefs
typedef char          TY_Char;
typedef TY_Word       TY_Len;
typedef unsigned long TY_ULong;
typedef void*         PTR_Handle;

// Further enum classes
enum class EN_Owner : TY_Byte
{
    USER = 0,
    PROTOCOL = 1
};

#ifdef NULL
#undef NULL
#endif
#ifndef __cplusplus
#define NULL ((void*) 0)
#else
#define NULL 0
#endif

typedef struct st_RcvByte
{
    TY_Byte   Data;
    TY_Byte   Error;
    TY_DWord  Time;
}
ST_RcvByte;

class COSAL
{
public:
    // Initialization/Termination
    static void                   Init(void);
    static void              Terminate(void);
    // Operation
    static void                   Wait(TY_DWord u32_Time);
    static void                   Lock(void);
    static void                 Unlock(void);
    static bool     IsInvalidIntHandle(WRD_Handle handle_);
    static void           CopyRcvBytes(ST_RcvByte* dst_, ST_RcvByte* src_, TY_Word len_);
    static void        ExtractRcvBytes(TY_Byte* dst_, ST_RcvByte* src_, TY_Word len_);
    static void       ExtractRcvErrors(TY_Byte* dst_, ST_RcvByte* src_, TY_Word len_);

    // Nested Classes
    class CLock
    {
    private:
        PTR_Handle m_lock_semaphore;
    public:
        // Construction/Deconstruction
        CLock(void);
        ~CLock(void);
        // Operation
        void Lock(void);
        void Unlock(void);
    };

    class CTimer
    {
    public:
        void             InitNoneStatic();
        void                      Start(TY_DWord u32_ms);
        void                    Restart();
        void                   Continue(TY_DWord u32_ms);
        void                       Stop();
        EN_Bool               IsExpired();
        EN_Bool                IsActive();
        static void                Init();
        static TY_DWord         GetTime();
        static TY_DWord        GetDelay(TY_Word num_bytes_, TY_DWord baudrate_);
        static TY_DWord   GetTxDuration(TY_Word num_bytes_, TY_DWord baudrate_);
        static TY_DWord     GetByteTime(TY_DWord u32_BitRate);
        static void          UpdateTime(TY_Word time_ms_);
        static void                Wait(TY_DWord ms_);
        static void         BeginPeriod(TY_DWord ms_);
        static void           EndPeriod(TY_DWord ms_);
    private:
        EN_Bool         m_locked;
        EN_Bool         m_active;
        TY_DWord        m_start_time;
        TY_DWord        m_time_limit;
        TY_DWord        m_last_time_limit;
        static TY_DWord s_time;
    };

    class CTask
    {
    public:
                                CTask();
        virtual EN_Error        Start(void (*handler_)(TY_Word time_));
        void                    Terminate();
        virtual EN_Bool         IsTerminated();
    protected:
        void    (*m_handler)(TY_Word time_ms_);
        TY_Byte   m_type;
        TY_Word   m_cycle;
        TY_Byte   m_priority;
        EN_Bool   m_terminate;
        EN_Bool   m_terminated;
    };

    class CRcvErr
    {
    public:
        static const TY_Byte ERR_None = 0;
        static const TY_Byte ERR_Frame = 1;
        static const TY_Byte ERR_Parity = 2;
        static const TY_Byte ERR_Over = 4;
        static const TY_Byte ERR_CheckSum = 8;
    };

    class CMem
    {
    public:
        static void       Copy(TY_Byte* dst_, const TY_Byte* src_, TY_DWord len_);
        static void        Set(TY_Byte* dst_, TY_Byte val_, TY_DWord len_);
        static EN_Bool IsEqual(TY_Byte* mem1_, TY_Byte* mem2_, TY_DWord len_);
    };

    class CString
    {
    public:
        static void        Copy(TY_Char* psz_Dst, TY_Char* psz_Src);
        static TY_Word   GetLen(TY_Char* psz_Str);
    };

    class CBitRates
    {
    public:
        class CIndex
        {
        public:
            static const TY_Byte IDX_1200 = 0;
            static const TY_Byte IDX_2400 = 1;
            static const TY_Byte IDX_4800 = 2;
            static const TY_Byte IDX_9600 = 3;
            static const TY_Byte IDX_19200 = 4;
            static const TY_Byte IDX_38400 = 5;
            static const TY_Byte IDX_57600 = 6;
        };

        class CBR_Win
        {
        public:
            static const TY_DWord BR_1200 = 1200;
            static const TY_DWord BR_2400 = 2400;
            static const TY_DWord BR_4800 = 4800;
            static const TY_DWord BR_9600 = 9600;
            static const TY_DWord BR_19200 = 19200;
            static const TY_DWord BR_38400 = 38400;
            static const TY_DWord BR_57600 = 57600;
        };

        class CByteTime
        {
        public:
            static const TY_DWord BT_1200 = 9;
            static const TY_DWord BT_2400 = 5;
            static const TY_DWord BT_4800 = 2;
            static const TY_DWord BT_9600 = 1;
            static const TY_DWord BT_Default = 1;
        };
    };

private:
    COSAL::CLock* mpc_Lock;
};

#endif // __osal_h__