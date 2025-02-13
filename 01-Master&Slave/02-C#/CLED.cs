/*
 *          File: CLED.cs (CLED)
 *                The module provides the code that is needed to realize 
 *                the graphical representation of an LED.
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

namespace SimpleLED
{
    #region Namespaces
    using System.Windows.Forms;
    #endregion Namespaces

    internal class CLED
    {
        #region Fields
        #region Private Constants
        private const int M_BLINK_STATE_OFF = 0;
        private const int M_BLINK_STATE_ON = 1;
        private const int M_BLINK_DELAY_TICS = 5;
        #endregion Private Constants

        #region Private Data
        private PictureBox mo_picture_box = new PictureBox();
        private ImageList? mo_image_list = null;
        private Timer mo_cycle_timer;
        private int m_delay_tics = M_BLINK_DELAY_TICS;
        private int m_toggle_counter = M_BLINK_STATE_OFF;
        private bool m_enabled = false;
        private bool m_blink_enabled = false;
        private int m_active_led = CLedColor.GREY;
        private int m_visible_led_idx = -1;
        #endregion Private Data
        #endregion Fields

        #region Constructor
        internal CLED(ref PictureBox pic_led_, ref ImageList imgl_leds_, ref Timer tim_cycle_)
        {
            this.mo_picture_box = pic_led_;
            this.mo_image_list = imgl_leds_;
            this.mo_cycle_timer = tim_cycle_;
            this.SetVisibleLed(CLedColor.GREY);
        }
        #endregion

        #region Internal Properties
        internal bool Enabled
        {
            get
            {
                return this.m_enabled;
            }

            set
            {
                if (value == false)
                {
                    this.SetBlinkTimerOff();
                    this.SetVisibleLed(CLedColor.GREY);
                }
                else
                {
                    this.SetVisibleLed(this.m_active_led);
                    if (this.m_blink_enabled)
                    {
                        this.SetBlinkTimerOn();
                    }

                    this.m_enabled = value;
                }
            }
        }
        internal bool BlinkEnabled
        {
            get
            {
                return this.m_blink_enabled;
            }

            set
            {
                this.SetVisibleLed(this.m_active_led);
                this.m_blink_enabled = value;
                if (value == true)
                {
                    this.SetBlinkTimerOn();
                }
                else
                {
                    this.SetBlinkTimerOff();
                }
            }
        }
        internal int ActiveLedColor
        {
            get
            {
                return this.m_active_led;
            }

            set
            {
                this.m_active_led = value;
                this.SetVisibleLed(this.m_active_led);
                if (this.m_blink_enabled)
                {
                    this.SetBlinkTimerDouble();
                }
            }
        }
        #endregion Public Properties

        #region Internal Methods
        internal void Update()
        {
            if (this.m_enabled == false)
            {
                this.SetBlinkTimerOff();
            }
            else
            {
                if (this.m_blink_enabled == false)
                {
                    this.SetBlinkTimerOff();
                }
                else
                {
                    // Blinking is m_enabled
                    if (this.m_delay_tics > 0)
                    {
                        this.m_delay_tics--;
                    }

                    if (this.m_delay_tics == 0)
                    {
                        this.m_delay_tics = M_BLINK_DELAY_TICS;
                        if (this.m_active_led == CLedColor.GREEN)
                        {
                            if (this.m_toggle_counter == M_BLINK_STATE_ON)
                            {
                                this.m_toggle_counter = M_BLINK_STATE_OFF;
                                this.SetVisibleLed(CLedColor.GREY);
                            }
                            else
                            {
                                this.m_toggle_counter = M_BLINK_STATE_ON;
                                this.SetVisibleLed(CLedColor.GREEN);
                            }
                        }
                        else
                        {
                            this.SetVisibleLed(this.m_active_led);
                        }
                    }
                }
            }
        }
        #endregion

        #region Private Methods
        private void SetVisibleLed(int index_of_led_)
        {
            if (index_of_led_ != this.m_visible_led_idx)
            {
                this.m_visible_led_idx = index_of_led_;
                if (mo_image_list != null)
                {
                    if (mo_image_list.Images != null)
                    {
                        this.mo_picture_box.Image = mo_image_list.Images[index_of_led_];
                        this.mo_picture_box.Refresh();
                    }
                }
            }
        }

        /// <summary>
        /// Disable blink timer
        /// </summary>
        private void SetBlinkTimerOff()
        {
            if (mo_cycle_timer.Enabled)
            {
                mo_cycle_timer.Enabled = false;
            }

            this.m_delay_tics = M_BLINK_DELAY_TICS;
            this.m_toggle_counter = M_BLINK_STATE_ON;
        }

        /// <summary>
        /// Enable blink timer
        /// </summary>
        private void SetBlinkTimerOn()
        {
            if (!this.mo_cycle_timer.Enabled)
            {
                this.mo_cycle_timer.Enabled = true;
            }

            this.m_delay_tics = M_BLINK_DELAY_TICS;
            this.m_toggle_counter = M_BLINK_STATE_ON;
        }

        /// <summary>
        /// Enable blink timer for slow period
        /// </summary>
        private void SetBlinkTimerDouble()
        {
            if (!this.mo_cycle_timer.Enabled)
            {
                this.mo_cycle_timer.Enabled = true;
            }

            this.m_delay_tics = 2 * M_BLINK_DELAY_TICS;
            this.m_toggle_counter = M_BLINK_STATE_ON;
        }
        #endregion Private Methods

        #region Nested Classes
        internal static class CLedColor
        {
            #region Internal Constants
            internal const int GREY = 0;
            internal const int YELLOW = 1;
            internal const int GREEN = 2;
            internal const int RED = 3;
            #endregion Internal Constants
        }
        #endregion Nested Classes
    }
}
