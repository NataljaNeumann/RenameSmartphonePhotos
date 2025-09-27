/*  RenameSmartphonePhotos tries to provide means in ordering your family photos based
    on the date they have been taken

    Copyright (C) 2024-2025 NataljaNeumann@gmx.de

    This program is free software; you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation; either version 2 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License along
    with this program; if not, write to the Free Software Foundation, Inc.,
    51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA.
*/


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace RenameSmartphonePhotos
{
    //*******************************************************************************************************
    /// <summary>
    /// Main form of the application
    /// </summary>
    //*******************************************************************************************************
    public partial class Form1 : Form
    {
        //===================================================================================================
        /// <summary>
        /// File name with prefix, e.g. Screenshot20010101_124951.jpg
        /// </summary>
        static System.Text.RegularExpressions.Regex s_oRegex1 =
            new System.Text.RegularExpressions.Regex(@"(.*)(20\d\d)(\d\d)(\d\d)[\s_-](.*)");

        //===================================================================================================
        /// <summary>
        /// File name already in target format, e.g 2001-01-01 124951.jpg
        /// </summary>
        static System.Text.RegularExpressions.Regex s_oRegex2 =
            new System.Text.RegularExpressions.Regex(@"\d\d\d\d-\d\d-\d\d\D(.*)");

        //===================================================================================================
        /// <summary>
        /// File name with all data concatenadted, e.g. 20010101124951.jpg
        /// </summary>
        System.Text.RegularExpressions.Regex s_oRegex3 =
            new System.Text.RegularExpressions.Regex(@"^(20\d\d)(\d\d)(\d\d)(\d\d\d\d\d\d)(\s?\(\d+\))?[\.](.*)");


        //===================================================================================================
        /// <summary>
        /// Constructs a new form
        /// </summary>
        //===================================================================================================
        public Form1()
        {
            InitializeComponent();

            System.ComponentModel.ComponentResourceManager res = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            res.ApplyResources(this, "$this");

            ReadyToUseImageInjection("RenameSmartphonePhotosHeader.dat");
        }


        //===================================================================================================
        /// <summary>
        /// This is executed when user clicks the button for choosing the folder
        /// </summary>
        /// <param name="oSender">Sender object</param>
        /// <param name="oEventArgs">Event args</param>
        //===================================================================================================
        private void OnChooseFolderClick(
            object oSender, 
            EventArgs oEventArgs
            )
        {
            if (m_dlgFolderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                m_tbxFolder.Text = m_dlgFolderBrowserDialog.SelectedPath;
            }
        }

        //===================================================================================================
        /// <summary>
        /// Renames a complement file, that belongs to original image file
        /// </summary>
        /// <param name="strOriginalJpeg">Original file path</param>
        /// <param name="strComplementExt">The extention of complement, e.g. raw file</param>
        //===================================================================================================
        void RenameComplementFile(
            string strOriginalJpeg, 
            string strOriginalJpegNewName, 
            string strComplementExt
            )
        {
            if (strOriginalJpeg.EndsWith(".JPG"))
            {
                string strComplementPath = strOriginalJpeg.Replace(".JPG", strComplementExt);
                string strNewName = strOriginalJpegNewName.Replace(".JPG", strComplementExt);

                FileInfo fi = new FileInfo(strComplementPath);

                if (fi.Exists)
                {
                    fi.MoveTo(strNewName);

                    System.IO.FileInfo fi3 = new System.IO.FileInfo(
                        System.IO.Path.Combine(System.IO.Path.Combine(
                        fi.DirectoryName, "RestoreInfo"), fi.Name + ".chk"));

                    if (fi3.Exists)
                    {
                        // Files, created by SaveMyFiles and SyncFolders
                        string newName3 = System.IO.Path.Combine(
                            System.IO.Path.Combine(fi.Directory.FullName, "RestoreInfo"),
                            strNewName.Substring(strNewName.LastIndexOf('\\') + 1) + ".chk");
                        fi3.MoveTo(newName3);
                    }
                }
            }
        }

        //===================================================================================================
        /// <summary>
        /// This is executed when user clicks the rename button
        /// </summary>
        /// <param name="oSender">Sender object</param>
        /// <param name="oEventArgs">Event args</param>
        //===================================================================================================
        private void OnRenameButtonClick(
            object oSender, 
            EventArgs oEventArgs
            )
        {
            try
            {
                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(m_tbxFolder.Text);

                foreach (string strPattern in new string [] {"*.JPG","*.*" })
                foreach (System.IO.FileInfo fi in di.GetFiles(strPattern))
                {
                    string strReducedName = fi.Name;
                    if (strReducedName.StartsWith("DSC "))
                        strReducedName = strReducedName.Substring(4);
                    if (strReducedName.StartsWith("DSCF"))
                        strReducedName = strReducedName.Substring(4);
                    if (strReducedName.StartsWith("DSC"))
                        strReducedName = strReducedName.Substring(3);
                    if (strReducedName.StartsWith("_DSF"))
                        strReducedName = strReducedName.Substring(4);
                    if (strReducedName.StartsWith("DSF"))
                        strReducedName = strReducedName.Substring(3);
                    if (strReducedName.StartsWith("IMG_"))
                        strReducedName = strReducedName.Substring(4);
                    if (strReducedName.StartsWith("_MG_"))
                        strReducedName = strReducedName.Substring(4);
                    strReducedName = strReducedName.Trim();

                    try
                    {
                        if (fi.Name.ToLower().Contains(".modd") || fi.Name.ToUpper().Contains("THUMBS.DB"))
                            continue;

                        if (s_oRegex1.IsMatch(fi.Name))
                        {
                            // date with prefix matched - move prefix to the time
                            string strNewName = System.IO.Path.Combine(di.FullName, 
                                s_oRegex1.Replace(fi.Name, "$2-$3-$4 $1$5"));

                            System.IO.FileInfo fi2 = new System.IO.FileInfo(fi.FullName + ".modd");
                            string newModd = strNewName + ".modd";

                            System.IO.FileInfo fi3 = new System.IO.FileInfo(
                                System.IO.Path.Combine(System.IO.Path.Combine(
                                fi.DirectoryName, "RestoreInfo"), fi.Name + ".chk"));

                            if (!fi2.Exists && fi2.Name.ToLower().Contains(".m2ts"))
                            {
                                fi2 = new System.IO.FileInfo(fi.FullName.Replace(".m2ts", ".modd"));
                                newModd = strNewName.Replace(".m2ts", ".modd");
                            }
                            if (!fi2.Exists && fi2.Name.ToLower().Contains(".mpg"))
                            {
                                fi2 = new System.IO.FileInfo(fi.FullName.Replace(".mpg", ".modd"));
                                newModd = strNewName.Replace(".mpg", ".modd");
                            }

                            fi.MoveTo(strNewName);

                            if (fi2.Exists)
                                fi2.MoveTo(newModd);

                            if (fi3.Exists)
                            {
                                // Files, created by SaveMyFiles and SyncFolders
                                string newName3 = System.IO.Path.Combine(
                                    System.IO.Path.Combine(di.FullName, "RestoreInfo"),
                                    strNewName.Substring(strNewName.LastIndexOf('\\') + 1) + ".chk");
                                fi3.MoveTo(newName3);
                            }
                        }
                        else
                        {
                            if (s_oRegex3.IsMatch(fi.Name))
                            {
                                // date and time, all concatenated together - split the information and make it human readable
                                string strNewName = System.IO.Path.Combine(
                                    di.FullName, s_oRegex3.Replace(fi.Name, "$1-$2-$3 $4$5.$6"));

                                System.IO.FileInfo fi2 = new System.IO.FileInfo(fi.FullName + ".modd");
                                string newModd = strNewName + ".modd";

                                System.IO.FileInfo fi3 = new System.IO.FileInfo(
                                    System.IO.Path.Combine(System.IO.Path.Combine(
                                    fi.DirectoryName, "RestoreInfo"), fi.Name + ".chk"));

                                if (!fi2.Exists && fi2.Name.ToLower().Contains(".m2ts"))
                                {
                                    fi2 = new System.IO.FileInfo(fi.FullName.Replace(".m2ts", ".modd"));
                                    newModd = strNewName.Replace(".m2ts", ".modd");
                                }

                                if (!fi2.Exists && fi2.Name.ToLower().Contains(".mpg"))
                                {
                                    fi2 = new System.IO.FileInfo(fi.FullName.Replace(".mpg", ".modd"));
                                    newModd = strNewName.Replace(".mpg", ".modd");
                                }

                                fi.MoveTo(strNewName);

                                if (fi2.Exists)
                                    fi2.MoveTo(newModd);

                                if (fi3.Exists)
                                {
                                    // Files, created by SaveMyFiles and SyncFolders
                                    string newName3 = System.IO.Path.Combine(
                                        System.IO.Path.Combine(di.FullName, "RestoreInfo"),
                                        strNewName.Substring(strNewName.LastIndexOf('\\') + 1) + ".chk");

                                    fi3.MoveTo(newName3);
                                }

                            }
                            else
                            {
                                // maybe it is already correctly formatted?
                                if (!s_oRegex2.IsMatch(fi.Name))
                                {
                                    // if not - try to get metadata
                                    System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex(":");
                                    string strNewName = null;
                                    try
                                    {
                                        using (System.IO.FileStream fs = new System.IO.FileStream(
                                            fi.FullName, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                                        using (Image myImage = Image.FromStream(fs, false, false))
                                        {
                                            try
                                            {
                                                System.Drawing.Imaging.PropertyItem propItem = myImage.GetPropertyItem(36867);
                                                if (propItem != null && propItem.Value != null)
                                                {
                                                    string dateTaken = r.Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);
                                                    DateTime t;
                                                    if (DateTime.TryParse(dateTaken, out t))
                                                    {
                                                        strNewName = System.IO.Path.Combine(di.FullName,
                                                            string.Format("{0:D4}-{1:D2}-{2:D2} {3}",
                                                            t.Year, t.Month, t.Day,
                                                            strReducedName));
                                                    }
                                                    else
                                                    {
                                                        // if we couldn't get any meaningful date default to file modification date
                                                        strNewName = System.IO.Path.Combine(di.FullName,
                                                            string.Format("{0:D4}-{1:D2}-{2:D2} {3}",
                                                            fi.LastWriteTime.Year, fi.LastWriteTime.Month,
                                                            fi.LastWriteTime.Day, strReducedName));
                                                    }
                                                }
                                                else
                                                {

                                                    // if we couldn't get any meaningful date default to file modification date
                                                    strNewName = System.IO.Path.Combine(di.FullName,
                                                        string.Format("{0:D4}-{1:D2}-{2:D2} {3}",
                                                        fi.LastWriteTime.Year, fi.LastWriteTime.Month,
                                                        fi.LastWriteTime.Day, strReducedName));
                                                }
                                            }
                                            catch (ArgumentException)
                                            {
                                                // if we couldn't get any meaningful date default to file modification date
                                                strNewName = System.IO.Path.Combine(di.FullName,
                                                    string.Format("{0:D4}-{1:D2}-{2:D2} {3}",
                                                    fi.LastWriteTime.Year, fi.LastWriteTime.Month,
                                                    fi.LastWriteTime.Day, strReducedName));
                                            }
                                        }
                                    }
                                    catch (ArgumentException)
                                    {
                                        // if we couldn't get any meaningful date default to file modification date
                                        strNewName = System.IO.Path.Combine(di.FullName,
                                            string.Format("{0:D4}-{1:D2}-{2:D2} {3}",
                                            fi.LastWriteTime.Year, fi.LastWriteTime.Month,
                                            fi.LastWriteTime.Day, strReducedName));
                                    }


                                    if (strNewName != null)
                                    {
                                        string strOriginalName = fi.FullName;

                                        System.IO.FileInfo fi2 = new System.IO.FileInfo(fi.FullName + ".modd");
                                        string newModd = strNewName + ".modd";


                                        // Files, created by SaveMyFiles and SyncFolders
                                        System.IO.FileInfo fi3 = new System.IO.FileInfo(
                                            System.IO.Path.Combine(System.IO.Path.Combine(
                                            fi.DirectoryName, "RestoreInfo"), fi.Name + ".chk"));

                                        if (!fi2.Exists && fi2.Name.ToLower().Contains(".m2ts"))
                                        {
                                            fi2 = new System.IO.FileInfo(fi.FullName.Replace(".m2ts", ".modd"));
                                            newModd = strNewName.Replace(".m2ts", ".modd");
                                        }

                                        if (!fi2.Exists && fi2.Name.ToLower().Contains(".mpg"))
                                        {
                                            fi2 = new System.IO.FileInfo(fi.FullName.Replace(".mpg", ".modd"));
                                            newModd = strNewName.Replace(".mpg", ".modd");
                                        }

                                        fi.MoveTo(strNewName);

                                        if (fi2.Exists)
                                            fi2.MoveTo(newModd);

                                        if (fi3.Exists)
                                        {
                                            // Files, created by SaveMyFiles and SyncFolders
                                            string newName3 = System.IO.Path.Combine(
                                                System.IO.Path.Combine(di.FullName, "RestoreInfo"),
                                                strNewName.Substring(strNewName.LastIndexOf('\\') + 1) + ".chk");

                                            fi3.MoveTo(newName3);
                                        }

                                        RenameComplementFile(strOriginalName, strNewName, ".RAF");
                                        RenameComplementFile(strOriginalName, strNewName, ".CR2");
                                        RenameComplementFile(strOriginalName, strNewName, ".MOV");
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception oEx)
                    {
                        // if something unexpected happens with a file - show to user
                        System.Windows.Forms.MessageBox.Show(this, oEx.Message, this.Text, 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception oEx)
            {
                // if something unexpected happens - show to user
                System.Windows.Forms.MessageBox.Show(this, oEx.Message, this.Text,
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //===================================================================================================
        /// <summary>
        /// Shows licence
        /// </summary>
        /// <param name="oSender">Sender object</param>
        /// <param name="oArgs">Event args</param>
        //===================================================================================================
        private void m_lblShowLicence_LinkClicked(
            object oSender, 
            LinkLabelLinkClickedEventArgs oArgs
            )
        {
            string strUrl = "https://www.gnu.org/licenses/gpl-2.0.html";
            try
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    Process.Start(new ProcessStartInfo(strUrl) { UseShellExecute = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", strUrl);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", strUrl);
                }
            }
            catch (Exception oEx)
            {
                MessageBox.Show("Could not open browser: " + oEx.Message);
            }
        }


        //===================================================================================================
        /// <summary>
        /// This is executed when folder text changes
        /// </summary>
        /// <param name="oSender">Sender object</param>
        /// <param name="oArgs">Even args</param>
        //===================================================================================================
        private void OnFolderTextChanged(
            object oSender, 
            EventArgs oArgs
            )
        {
            try
            {
                m_btnRename.Enabled = System.IO.Directory.Exists(m_tbxFolder.Text);
            }
            catch
            {
                m_btnRename.Enabled = false;
            }
        }

        //===================================================================================================
        /// <summary>
        /// This is executed when user clicks on the "About" link
        /// </summary>
        /// <param name="oSender">Sender object</param>
        /// <param name="oEventArgs">Event args</param>
        //===================================================================================================
        private void OnAboutLinkClicked(
            object oSender, 
            LinkLabelLinkClickedEventArgs oEventArgs
            )
        {
            using (AboutForm dlgAbout = new AboutForm())
            {
                dlgAbout.ShowDialog(this);
            }
        }

        //===================================================================================================
        /// <summary>
        /// This is executed when user requests help
        /// </summary>
        /// <param name="oSender">Sender object</param>
        /// <param name="oEventArgs">Event args</param>
        //===================================================================================================
        private void OnHelpRequested(
            object oSender, HelpEventArgs  oEventArgs)
        {
            System.Diagnostics.Process.Start(System.IO.Path.Combine(Application.StartupPath, "Readme.html"));
        }



        #region image injection part
        //===================================================================================================
        /// <summary>
        /// Picture box control
        /// </summary>
        private PictureBox m_ctlPictureBox;
        //===================================================================================================
        /// <summary>
        /// Image
        /// </summary>
        private Image m_oLoadedImage;
        //===================================================================================================
        /// <summary>
        /// A dictionary with positions of other elements
        /// </summary>
        private Dictionary<Control, int> m_oOriginalPositions;

        //===================================================================================================
        /// <summary>
        /// Loads an image from application startup path and shows it at the top of the window
        /// </summary>
        /// <param name="strName">Name of the image, without directory specifications</param>
        //===================================================================================================
        private void ReadyToUseImageInjection(string strImageName)
        {

            string strImagePath = System.IO.Path.Combine(Application.StartupPath, strImageName);
            if (System.IO.File.Exists(strImagePath))
            {
                m_oOriginalPositions = new Dictionary<Control, int>();
                foreach (Control ctl in Controls)
                {
                    m_oOriginalPositions[ctl] = ctl.Top;
                }

                m_ctlPictureBox = new PictureBox();
                m_ctlPictureBox.Location = this.ClientRectangle.Location;
                m_ctlPictureBox.Size = new Size(0,0);
                Controls.Add(m_ctlPictureBox);

                LoadAndResizeImage(strImagePath);

                this.Resize += new EventHandler(ResizeImageAlongWithForm);
            }
        }



        //===================================================================================================
        /// <summary>
        /// Resizes image along with the form
        /// </summary>
        /// <param name="oSender">Sender object</param>
        /// <param name="oEventArgs">Event args</param>
        //===================================================================================================
        private void ResizeImageAlongWithForm(object oSender, EventArgs oEventArgs)
        {
            ResizeImageAndShiftElements();
        }



        //===================================================================================================
        /// <summary>
        /// Loads an image and resizes it to the width of client area
        /// </summary>
        /// <param name="strImagePath">Path to header image file</param>
        //===================================================================================================
        private void LoadAndResizeImage(string strImagePath)
        {

            try
            {
                using (FileStream oStream = new FileStream(strImagePath, FileMode.Open, FileAccess.Read))
                {
                    byte[] aImageBytes;

                    if (Program.IslamicCountry)
                    {
                        oStream.Seek(1, SeekOrigin.Begin);
                        aImageBytes = new byte[259106];
                        oStream.Read(aImageBytes, 0, 259106);
                    }
                    else
                    {
                        oStream.Seek(259107, SeekOrigin.Begin);
                        int nRemainingBytes = (int)(oStream.Length - 259107);
                        aImageBytes = new byte[nRemainingBytes];
                        oStream.Read(aImageBytes, 0, nRemainingBytes);
                    }

                    using (MemoryStream oMemoryStream = new MemoryStream(aImageBytes))
                    {
                        m_oLoadedImage = Image.FromStream(oMemoryStream);
                    }
                }


            }
            catch (Exception)
            {
                // ignore
            }

            ResizeImageAndShiftElements();
        }

        //===================================================================================================
        /// <summary>
        /// Resizes image and shifts other elements
        /// </summary>
        //===================================================================================================
        private void ResizeImageAndShiftElements()
        {
            if (m_oLoadedImage != null)
            {
                if (WindowState != FormWindowState.Minimized)
                {
                    float fAspectRatio = (float)m_oLoadedImage.Width / (float)m_oLoadedImage.Height;

                    int nNewWidth = this.ClientSize.Width;
                    if (nNewWidth != 0)
                    {
                        int nNewHeight = (int)(nNewWidth / fAspectRatio);

                        int nHeightChange = nNewHeight - m_ctlPictureBox.Height;

                        this.m_ctlPictureBox.Image = new Bitmap(m_oLoadedImage, nNewWidth, nNewHeight);
                        this.m_ctlPictureBox.Size = new Size(nNewWidth, nNewHeight);

                        ShiftOtherElementsUpOrDown(nHeightChange);
                        this.Height += nHeightChange;
                    }
                }
            }
        }

        //===================================================================================================
        /// <summary>
        /// Shifts elements, apart from the image box up or down
        /// </summary>
        /// <param name="nHeightChange">Change in the heights, compared to previous position</param>
        //===================================================================================================
        private void ShiftOtherElementsUpOrDown(int nHeightChange)
        {
            foreach (Control ctl in m_oOriginalPositions.Keys)
            {
                if ((ctl.Anchor & AnchorStyles.Bottom) == AnchorStyles.None)
                    ctl.Top += nHeightChange;
            }
        }
        #endregion
    }
}
