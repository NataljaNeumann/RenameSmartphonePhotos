/*  RenameSmartphonePhotos tries to provide means in ordering your family photos based
    on the date they have been taken

    Copyright (C) 2024  NataljaNeumann@gmx.de

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

namespace RenameSmartphonePhotos
{
    partial class RenameFilesForm
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RenameFilesForm));
            this.m_tbxFolder = new System.Windows.Forms.TextBox();
            this.mbnChooseFolder = new System.Windows.Forms.Button();
            this.m_lblFolder = new System.Windows.Forms.Label();
            this.m_btnRename = new System.Windows.Forms.Button();
            this.m_dlgFolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.m_lblAbout = new System.Windows.Forms.LinkLabel();
            this.m_lblShowLicence = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // m_tbxFolder
            // 
            resources.ApplyResources(this.m_tbxFolder, "m_tbxFolder");
            this.m_tbxFolder.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.m_tbxFolder.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystem;
            this.m_tbxFolder.Name = "m_tbxFolder";
            this.m_tbxFolder.TextChanged += new System.EventHandler(this.OnFolderTextChanged);
            // 
            // mbnChooseFolder
            // 
            resources.ApplyResources(this.mbnChooseFolder, "mbnChooseFolder");
            this.mbnChooseFolder.Name = "mbnChooseFolder";
            this.mbnChooseFolder.UseVisualStyleBackColor = true;
            this.mbnChooseFolder.Click += new System.EventHandler(this.OnChooseFolderClick);
            // 
            // m_lblFolder
            // 
            resources.ApplyResources(this.m_lblFolder, "m_lblFolder");
            this.m_lblFolder.Name = "m_lblFolder";
            // 
            // m_btnRename
            // 
            resources.ApplyResources(this.m_btnRename, "m_btnRename");
            this.m_btnRename.Name = "m_btnRename";
            this.m_btnRename.UseVisualStyleBackColor = true;
            this.m_btnRename.Click += new System.EventHandler(this.OnRenameButtonClick);
            // 
            // m_lblAbout
            // 
            resources.ApplyResources(this.m_lblAbout, "m_lblAbout");
            this.m_lblAbout.Name = "m_lblAbout";
            this.m_lblAbout.TabStop = true;
            this.m_lblAbout.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnAboutLinkClicked);
            // 
            // m_lblShowLicence
            // 
            resources.ApplyResources(this.m_lblShowLicence, "m_lblShowLicence");
            this.m_lblShowLicence.Name = "m_lblShowLicence";
            this.m_lblShowLicence.TabStop = true;
            this.m_lblShowLicence.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.m_lblShowLicence_LinkClicked);
            // 
            // RenameFilesForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_lblShowLicence);
            this.Controls.Add(this.m_lblAbout);
            this.Controls.Add(this.m_btnRename);
            this.Controls.Add(this.m_lblFolder);
            this.Controls.Add(this.mbnChooseFolder);
            this.Controls.Add(this.m_tbxFolder);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "RenameFilesForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox m_tbxFolder;
        private System.Windows.Forms.Button mbnChooseFolder;
        private System.Windows.Forms.Label m_lblFolder;
        private System.Windows.Forms.Button m_btnRename;
        private System.Windows.Forms.FolderBrowserDialog m_dlgFolderBrowserDialog;
        private System.Windows.Forms.LinkLabel m_lblAbout;
        private System.Windows.Forms.LinkLabel m_lblShowLicence;
    }
}

