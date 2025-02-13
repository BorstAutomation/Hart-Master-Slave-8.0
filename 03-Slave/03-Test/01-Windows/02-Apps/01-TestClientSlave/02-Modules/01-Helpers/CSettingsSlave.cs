/*
 *          File: Settings.cs (CSettings)
 *                A .NET component is used to store and read the user 
 *                settings. Nevertheless, the individual settings must 
 *                be assigned to specific functionalities.
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

#region Namespaces
using BaTestHart.Properties;
#endregion Namespaces

namespace BaTestHart
{
    internal static partial class CSettings
    {
        #region Internal Data
        internal static byte NumReqPreambles = 5;
        internal static byte NumRspPreambles = 5;
        #endregion Internal Data  

        #region Internal Methods
        internal static void FetchSpecific()
        {
            // Hart
            NumReqPreambles = Settings.Default.NumReqPreambles;
        }
        internal static void PutSpecific()
        {
        }
        #endregion Public Methods
    }
}

