/*
 *          File: FrmAbout.cs (FrmAbout)
 *                The form provides information about the current implementation. 
 *                Here you can also access the documentation.
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
using Microsoft.Extensions.FileProviders.Composite;
using System.Diagnostics;
#endregion Namespaces

namespace BaTestHart
{
    internal partial class FrmAbout : Form
    {

        internal int ForceLeft = 0;
        internal int ForceTop = 0;


        internal FrmAbout()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        private void FrmAbout_Load(object sender, EventArgs e)
        {
            this.Top = (int)ForceTop;
            this.Left = (int)ForceLeft;
        }

        private void ButExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ButDataSheet_Click(object sender, EventArgs e)
        {
            ProcessStartInfo info = new ProcessStartInfo();
            info.UseShellExecute = true;
            info.FileName = Path.GetFullPath(Directory.GetCurrentDirectory() + "\\..\\..\\..\\01-Docu\\DataSheet-Hart-Master-C++-8.0.pdf");

            Process.Start(info);
        }

        private void ButVersionInfo_Click(object sender, EventArgs e)
        {
            ProcessStartInfo info = new ProcessStartInfo();
            info.UseShellExecute = true;
            info.FileName = Path.GetFullPath(Directory.GetCurrentDirectory() + "\\..\\..\\..\\01-Docu\\ReleaseNotes.md");

            Process.Start(info);
        }

        private void ButHartCoding_Click(object sender, EventArgs e)
        {
            ProcessStartInfo info = new ProcessStartInfo();
            info.UseShellExecute = true;
            info.FileName = Path.GetFullPath(Directory.GetCurrentDirectory() + "\\..\\..\\..\\01-Docu\\Hart-At-a-Glance.pdf");

            Process.Start(info);
        }
    }
}
