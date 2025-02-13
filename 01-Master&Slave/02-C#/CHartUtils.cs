/*
 *          File: CHartUtils.cs (CHartUtils)
 *                There are a few small functions here that read texts 
 *                from numerical information in the hard protocol that 
 *                indicate what the codes mean. An example of this is 
 *                the engineering unit
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

namespace BaTestHart.HartUtils
{
    #region Namespaces
    using System.Windows.Forms;
    #endregion

    // Hart communication functionality of common usage
    internal static class CHartUtils
    {
        #region Fields
        private static int m_last_error = CErrCode.NONE;
        private static Form? m_parent_form;
        #endregion Fields

        #region Public Properties
        // Gets the m_text for the last error code
        internal static string LastErrorText
        {
            get
            {
                return m_last_error switch
                {
                    CErrCode.NONE => "NONE",
                    CErrCode.COM_PORT_NOT_VALID => "Com Port not Accesseable",
                    CErrCode.CONNECTION_FAILED => "Establishing Connection Failed",
                    CErrCode.COMMUNICATION_ERROR => "Hart Communication Error",
                    CErrCode.SERVICE_REJECTED => "Service Rejected by the Hart Device Software",
                    CErrCode.TOO_FEW_DATA_BYTES => "Too few Data Bytes Received",
                    _ => "Unknown Error Code...",
                };
            }
        }

        // Gets or sets the numeric value of the last error
        internal static int LastError
        {
            get
            {
                return m_last_error;
            }

            set
            {
                m_last_error = value;
            }
        }

        // Gets a reference to the parent form
        internal static Form? Parent
        {
            get
            {
                return m_parent_form;
            }
        }
        #endregion

        #region Enumerations
        internal enum EN_DevVarClass : byte
        {
            // Parts of Hart table 21
            NONE = 0,
            TEMPERATURE = 64,
            PRESSURE = 65,
            VOLUMEFLOW = 66,
            VELOCITY = 67,
            VOLUME = 68,
            LENGTH = 69,
            MASS = 71,
            MASS_FLOW = 72,
            DENSITY = 73,
            CURRENT = 84
        }

        internal enum EN_ExtDevStatusBits : byte
        {
            // Parts of Hart table 21
            NONE = 0,
            MAINTENANCE = 0x01,
            DEV_VAR_ALERT = 0x02,
            POWER_FAILURE = 0x04,
            FAILURE = 0x08,
            OUT_OF_SPEC = 0x10,
            FUNCTION_CHECK = 0x20
        }
        #endregion Enumerations

        #region Public Methods
        // Initialize parent form reference
        internal static void Init(Form frm_)
        {
            // Insert arrays into structures
            m_parent_form = frm_;
        }
        #endregion Public Methods

        #region Nested Classes
        #region Static Classes
        // Error codes associated with the CHartUtils class
        internal static class CErrCode
        {
            #region Internal Constants
            internal const int NONE = 0;
            internal const int COM_PORT_NOT_VALID = 1;
            internal const int CONNECTION_FAILED = 2;
            internal const int COMMUNICATION_ERROR = 3;
            internal const int SERVICE_REJECTED = 4;
            internal const int TOO_FEW_DATA_BYTES = 5;
            #endregion

            #region Public Methods
            // Gets the m_text for the last error code
            internal static string ErrorText(int err_)
            {
                return err_ switch
                {
                    CErrCode.NONE => "NONE",
                    CErrCode.COM_PORT_NOT_VALID => "Com Port not Accesseable",
                    CErrCode.CONNECTION_FAILED => "Establishing Connection Failed",
                    CErrCode.COMMUNICATION_ERROR => "Hart Communication Error",
                    CErrCode.SERVICE_REJECTED => "Service Rejected by the HartDeviceDLL",
                    CErrCode.TOO_FEW_DATA_BYTES => "Too few Data Bytes Received",
                    _ => "Unknown Error Code...",
                };
            }

            #endregion Public Methods
        }

        // Hart unit conversions and handling
        internal static class CHartUnit
        {
            // Get a m_text string for the units code
            internal static string GetText(byte units_code_)
            {
                return units_code_ switch
                {
                    0 => "-/-",
                    1 => "InH2O",
                    2 => "InHg",
                    3 => "FtH2O",
                    4 => "mmH2O",
                    5 => "mmHg",
                    6 => "psi",
                    7 => "bar",
                    8 => "mbar",
                    9 => "g_SqCm",
                    10 => "kg_SqCm",
                    11 => "PA",
                    12 => "kPA",
                    13 => "torr",
                    14 => "ATM",
                    15 => "ft³/minute",
                    16 => "USgal/minute",
                    17 => "l/minute",
                    18 => "ImpGal/minute",
                    19 => "m³/hour",
                    20 => "ft/s",
                    21 => "m/s",
                    22 => "USgal/s",
                    23 => "MilUSgal/day",
                    24 => "l/s",
                    25 => "MilL/day",
                    26 => "ft³/s",
                    27 => "ft³/day",
                    28 => "m³/s",
                    29 => "m³/day",
                    30 => "ImpGal/h",
                    31 => "ImpGal/day",
                    32 => "°C",
                    33 => "°F",
                    34 => "Rk",
                    35 => "K",
                    36 => "mV",
                    37 => "Ohm",
                    38 => "Hz",
                    39 => "mA",
                    40 => "gal",
                    41 => "liter",
                    42 => "impGal",
                    43 => "m³",
                    44 => "ft",
                    45 => "meter",
                    46 => "Barrels",
                    47 => "in",
                    48 => "cm",
                    49 => "mm",
                    50 => "min",
                    51 => "sec",
                    52 => "hr",
                    53 => "day",
                    54 => "centi_st.",
                    55 => "cpoise",
                    56 => "uMhol",
                    57 => "%",
                    58 => "V",
                    59 => "pH",
                    60 => "grams",
                    61 => "kg",
                    62 => "MetTon",
                    63 => "lbs",
                    64 => "ShTon",
                    65 => "LTon",
                    70 => "g/s",
                    71 => "g/minute",
                    72 => "g/h",
                    73 => "kg/s",
                    74 => "kg/m",
                    75 => "kg/h",
                    76 => "kg/day",
                    77 => "MetTons/m",
                    78 => "MetTons/h",
                    79 => "MetTons/day",
                    80 => "lbs/s",
                    81 => "lbs/m",
                    82 => "lbs/h",
                    83 => "lbs/day",
                    84 => "ShTons/m",
                    85 => "ShTons/h",
                    86 => "ShTons/day",
                    87 => "LTons/h",
                    88 => "LTons/day",
                    90 => "SGU",
                    91 => "g/cm³",
                    92 => "kg/m³",
                    93 => "pounds/UsGal",
                    94 => "pounds/ft³",
                    95 => "g/ml",
                    96 => "kg/l",
                    97 => "g/l",
                    98 => "lb/In³",
                    99 => "ShTons/Yd³",
                    100 => "degTwad",
                    101 => "degBrix",
                    102 => "degBaum/hv",
                    103 => "degBaum/lt",
                    104 => "degAPI",
                    105 => "Percent_sol_wt",
                    106 => "Percent_sol_vol",
                    107 => "degBall",
                    108 => "proof_vol",
                    109 => "proof_mass",
                    110 => "bush",
                    111 => "Yd³",
                    112 => "Ft³",
                    113 => "In³",
                    120 => "m/h",
                    130 => "Ft³/h",
                    131 => "m³/m",
                    132 => "barrels/s",
                    133 => "barrels/m",
                    134 => "barrels/h",
                    135 => "barrels/day",
                    136 => "USgal/h",
                    137 => "ImpGal/s",
                    138 => "l/h",
                    139 => "%/StmQual",
                    151 => "Ftin16",
                    152 => "Ft³/lb",
                    153 => "pico_farads",
                    156 => "dB",
                    160 => "Percent_plato",
                    161 => "%LEL",
                    162 => "mega calorie",
                    163 => "kOhm",
                    164 => "MJ",
                    165 => "Blu",
                    166 => "Nm³",
                    167 => "Nl",
                    168 => "SCF",
                    169 => "ppb",
                    170 => "ppth",
                    171 => "ft/s²",
                    172 => "m/s²",
                    173 => "bbl-beer/d",
                    174 => "fl oz (UK)/d",
                    175 => "fl oz/d",
                    176 => "ml/d",
                    177 => "l/d",
                    178 => "hl/d",
                    179 => "mHg",
                    180 => "Mpsi",
                    181 => "Nm³/d",
                    182 => "Nm³/min",
                    183 => "Nm³/s",
                    184 => "SCF/d",
                    185 => "SCF/h",
                    186 => "SCF/s",
                    187 => "Sm³/d",
                    188 => "Sm³/h",
                    189 => "Sm³/min",
                    190 => "Sm³/s",
                    192 => "bbl-beer (UK)/s",
                    235 => "gallons/day",
                    236 => "hl",
                    237 => "mega_pascals",
                    238 => "in_H2O_4_degrees_C",
                    239 => "mm_H2O_4_degrees_C",
                    240 => "?",
                    250 => "not used",
                    251 => "none",
                    253 => "-/-",
                    _ => units_code_.ToString("000"),
                };
            }

            // Initialize a combo box with unit items
            internal static void InitCombo(ComboBox cmb_, int index_)
            {
                cmb_.Items.Clear();

                for (int e = 0; e < 256; e++)
                {
                    cmb_.Items.Add(GetText((byte)e));
                }

                cmb_.SelectedIndex = index_;
                cmb_.BackColor = SystemColors.Window;
            }
        }
        #endregion Static Classes
        #endregion Nested Classes
    }
}