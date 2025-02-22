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

namespace RenameSmartphonePhotos
{
    //*******************************************************************************************************
    /// <summary>
    /// Main form of the application
    /// </summary>
    //*******************************************************************************************************
    public partial class RenameFilesForm : Form
    {
        //===================================================================================================
        /// <summary>
        /// Constructs a new form
        /// </summary>
        //===================================================================================================
        public RenameFilesForm()
        {
            InitializeComponent();
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
            System.Text.RegularExpressions.Regex oRegex1 =
                new System.Text.RegularExpressions.Regex(@"(.*)(20\d\d)(\d\d)(\d\d)[\s_-](.*)");
            System.Text.RegularExpressions.Regex oRegex2 = 
                new System.Text.RegularExpressions.Regex(@"\d\d\d\d-\d\d-\d\d\D(.*)");
            System.Text.RegularExpressions.Regex oRegex3 =
                new System.Text.RegularExpressions.Regex(@"^(20\d\d)(\d\d)(\d\d)(\d\d\d\d\d\d)(\s?\(\d+\))?[\.](.*)");

            try
            {
                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(m_tbxFolder.Text);

                foreach (System.IO.FileInfo fi in di.GetFiles())
                {
                    try
                    {
                        if (fi.Name.ToLower().Contains(".modd") || fi.Name.ToUpper().Contains("THUMBS.DB"))
                            continue;

                        if (oRegex1.IsMatch(fi.Name))
                        {
                            //string strNewName = System.IO.Path.Combine(di.FullName, regex.Replace(fi.Name, "$2-$3-$4 $1$5"));
                            string strNewName = System.IO.Path.Combine(di.FullName, oRegex1.Replace(fi.Name, "$2-$3-$4 $1$5"));

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
                            if (oRegex3.IsMatch(fi.Name))
                            {
                                string strNewName = System.IO.Path.Combine(
                                    di.FullName, oRegex3.Replace(fi.Name, "$1-$2-$3 $4$5.$6"));
                                //string strNewName = System.IO.Path.Combine(
                                //    di.FullName, regex3.Replace(fi.Name, "$2-$3-$4 $5.$6"));

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
                                if (!oRegex2.IsMatch(fi.Name))
                                {
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
                                                            fi.Name.Replace("DSC_", "").Replace("DSC ", "").Trim()));
                                                    }
                                                    else
                                                    {
                                                        strNewName = System.IO.Path.Combine(di.FullName,
                                                            string.Format("{0:D4}-{1:D2}-{2:D2} {3}",
                                                            fi.LastWriteTime.Year, fi.LastWriteTime.Month,
                                                            fi.LastWriteTime.Day, fi.Name));
                                                    }
                                                }
                                                else
                                                {
                                                    strNewName = System.IO.Path.Combine(di.FullName,
                                                        string.Format("{0:D4}-{1:D2}-{2:D2} {3}",
                                                        fi.LastWriteTime.Year, fi.LastWriteTime.Month,
                                                        fi.LastWriteTime.Day, fi.Name));
                                                }
                                            }
                                            catch (ArgumentException)
                                            {
                                                strNewName = System.IO.Path.Combine(di.FullName,
                                                    string.Format("{0:D4}-{1:D2}-{2:D2} {3}",
                                                    fi.LastWriteTime.Year, fi.LastWriteTime.Month,
                                                    fi.LastWriteTime.Day, fi.Name));
                                            }
                                        }
                                    }
                                    catch (ArgumentException)
                                    {
                                        strNewName = System.IO.Path.Combine(di.FullName,
                                            string.Format("{0:D4}-{1:D2}-{2:D2} {3}",
                                            fi.LastWriteTime.Year, fi.LastWriteTime.Month,
                                            fi.LastWriteTime.Day, fi.Name));
                                    }


                                    if (strNewName != null)
                                    {
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
                                }
                            }
                        }
                    }
                    catch (Exception oEx)
                    {
                        System.Windows.Forms.MessageBox.Show(this, oEx.Message, this.Text, 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception oEx)
            {
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
            System.Diagnostics.Process.Start("https://www.gnu.org/licenses/gpl-2.0.html");
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
    }
}
