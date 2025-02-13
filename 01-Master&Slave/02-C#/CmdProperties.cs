/*
 *          File: CmdProperties.cs (CCmdProps)
 *                The code provides texts for various elements of a 
 *                command response.
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

namespace BaTestHart.CmdProperties
{
    internal static class CCmdProps
  {
    internal static string GetResponseText(byte cmd_, byte rsp_)
    {
      switch (rsp_)
      {
        case 0:
          return string.Empty;
        case 2:
          return "Invalid Selection";
        case 3:
          return "Passed Parameter Too Large";
        case 4:
          return "Passed Parameter Too Small";
        case 5:
          return "Too Few Data Bytes Received";
        case 6:
          return "Device Specific Command Error";
        case 7:
          return "In Write Protect Mode";
        case 8:
          return "Update Failure";
        case 16:
          return "Access Restricted";
        case 17:
          return "Invalid Device Variable Index";
        case 18:
          return "Invalid Units Code";
        case 19:
          return "Device Variable Index Not Allowed";
        case 20:
          return "Invalid Extended Command Number";
        case 21:
          return "Invalid I/O Card Number";
        case 22:
          return "Invalid Channel Number";
        case 23:
          return "Sub-device Response Too Long";
        case 28:
          return "Invalid Range Units Code";
        case 30:
          return "Command Response Truncated";
        case 32:
          return "Device Busy";
        case 33:
          return "Delayed Response Initiated";
        case 34:
          return "Delayed Response Running";
        case 35:
          return "Delayed Response Dead";
        case 36:
          return "Delayed Response Conflict";
        case 60:
          return "Payload Too Long";
        case 61:
          return "No Buffers Available";
        case 62:
          return "No Alarm/Event Buffers Available";
        case 63:
          return "Priority Too Low";
        case 64:
          return "Command Not Implemented";
      }

      if (rsp_ > 127)
      {
        return "Communication Error: " + (rsp_ - 127).ToString("0");
      }

            return cmd_ switch
            {
                9 => GetRspCmd09(rsp_),
                18 => GetRspCmd18(rsp_),
                35 => GetRspCmd35(rsp_),
                37 => GetRspCmd37(rsp_),
                _ => "Response Code: " + rsp_.ToString("0"),
            };
        }

        internal static string GetStatusText(byte byStatus)
        {
            string s = string.Empty;

            if ((byStatus & 0x80) != 0)
            {
                s = CFrameHelper.AppendTextWithComma(s, "Malfunction");
            }
            if ((byStatus & 0x40) != 0)
            {
                s = CFrameHelper.AppendTextWithComma(s, "Cfg Changed");
            }
            if ((byStatus & 0x20) != 0)
            {
                s = CFrameHelper.AppendTextWithComma(s, "Cold Start");
            }
            if ((byStatus & 0x10) != 0)
            {
                s = CFrameHelper.AppendTextWithComma(s, "More Status");
            }
            if ((byStatus & 0x08) != 0)
            {
                s = CFrameHelper.AppendTextWithComma(s, "Fix Curr");
            }
            if ((byStatus & 0x04) != 0)
            {
                s = CFrameHelper.AppendTextWithComma(s, "Sat Curr");
            }
            if ((byStatus & 0x02) != 0)
            {
                s = CFrameHelper.AppendTextWithComma(s, "Var Limits");
            }
            if ((byStatus & 0x01) != 0)
            {
                s = CFrameHelper.AppendTextWithComma(s, "Prim Var Limits");
            }

            return s;
        }

        private static string GetRspCmd09(byte byRsp)
    {
            return byRsp switch
            {
                14 => "Dynimac Variables Returned for Device Variables",
                _ => "Response Code: " + byRsp.ToString("0"),
            };
        }

    private static string GetRspCmd18(byte byRsp)
    {
            return byRsp switch
            {
                9 => "Invalid Date Code Detected",
                _ => "Response Code: " + byRsp.ToString("0"),
            };
        }

    private static string GetRspCmd35(byte byRsp)
    {
            return byRsp switch
            {
                9 => "Lower Range Value Too High",
                10 => "Lower Range Value Too Low",
                11 => "Upper Range Value Too High",
                12 => "Upper Range Value Too Low",
                13 => "Upper and Lower Range Out Of Limits",
                14 => "Span Too Small",
                29 => "Invalid Span",
                _ => "Response Code: " + byRsp.ToString("0"),
            };
        }

        private static string GetRspCmd37(byte byRsp) => byRsp switch
        {
            9 => "Applied Process Too High",
            10 => "Applied Process Too Low",
            14 => "New Lower Range Value Pushed",
            29 => "Invalid Span",
            _ => "Response Code: " + byRsp.ToString("0"),
        };
    }
}
