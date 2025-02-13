/*
 *          File: StatusControl.cs (CDevStatus)
 *                This is a set of functions keeping track of
 *                configuration changes with the master test
 *                framework.
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

namespace BaTestHart
{
    internal static class CDevStatus
    {
        internal static bool CfgChanged = true;

        #region Internal Data
        internal static bool ConfigInProgress = false;
        internal static bool ApplyingSettings = false;

        internal static class CMonitor
        {
            internal static bool CfgChanged = true;
            internal static byte ActiveComport = 0;
        }

        internal static class CGeneral
        {
            internal static bool CfgChanged = true;
            internal static byte ActiveComport = 0;
        }

        internal static class CFramesDisplay
        {
            internal static bool bCfgChanged = false;
        }

        internal static class CColors
        {
            internal static bool CfgChanged = false;
        }

        internal static class CTopMost
        {
            internal static bool CfgChanged = false;
        }
        #endregion Internal Data

        #region Internal Properties
        internal static bool AnyCfgChanged
        {
            get
            {
                if ((CMonitor.CfgChanged) ||
                     (CGeneral.CfgChanged) ||
                     (CFramesDisplay.bCfgChanged) ||
                     (CColors.CfgChanged) ||
                     (CTopMost.CfgChanged) ||
                     (CfgChanged)
                  )
                {
                    return true;
                }
                return false;
            }
        }

        internal static bool HandleEvents
        {
            get
            {
                if (ConfigInProgress || ApplyingSettings)
                {
                    return false;
                }
                return true;
            }
        }
        #endregion

        #region Internal Methods
        internal static void Init()
        {
            ConfigInProgress = false;
            CMonitor.CfgChanged = true;
            CMonitor.ActiveComport = 0;
            CGeneral.ActiveComport = 0;
            CGeneral.CfgChanged = true;
            CFramesDisplay.bCfgChanged = true;
            CColors.CfgChanged = true;
            CTopMost.CfgChanged = true;
            ConfigInProgress = false;
            ApplyingSettings = false;
            CfgChanged = false;
        }

        internal static void ClearChangeFlags()
        {
            CMonitor.CfgChanged = false;
            CGeneral.CfgChanged = false;
            CFramesDisplay.bCfgChanged = false;
            CColors.CfgChanged = false;
            CTopMost.CfgChanged = false;
            CfgChanged = false;
        }
        #endregion Internal Methods
    }
}
