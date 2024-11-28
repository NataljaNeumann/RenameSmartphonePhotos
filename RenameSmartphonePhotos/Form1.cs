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
    public partial class RenameFilesForm : Form
    {
        public RenameFilesForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void buttonChooseFolder_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBoxFolder.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"(.*)(20\d\d)(\d\d)(\d\d)[\s_-](.*)");
            System.Text.RegularExpressions.Regex regex2 = new System.Text.RegularExpressions.Regex(@"\d\d\d\d-\d\d-\d\d\D(.*)");
            System.Text.RegularExpressions.Regex regex3 = new System.Text.RegularExpressions.Regex(@"^(20\d\d)(\d\d)(\d\d)(\d\d\d\d\d\d)(\s?\(\d+\))?[\.](.*)");
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(textBoxFolder.Text);
            foreach (System.IO.FileInfo fi in di.GetFiles())
            {
                if (fi.Name.ToLower().Contains(".modd") || fi.Name.ToUpper().Contains("THUMBS.DB"))
                    continue;
                if (regex.IsMatch(fi.Name))
                {
                    //string strNewName = System.IO.Path.Combine(di.FullName, regex.Replace(fi.Name, "$2-$3-$4 $1$5"));
                    string strNewName = System.IO.Path.Combine(di.FullName, regex.Replace(fi.Name, "$2-$3-$4 $1$5"));

                    System.IO.FileInfo fi2 = new System.IO.FileInfo(fi.FullName + ".modd");
                    string newModd = strNewName + ".modd";
                    System.IO.FileInfo fi3 = new System.IO.FileInfo(System.IO.Path.Combine(System.IO.Path.Combine(fi.DirectoryName, "RestoreInfo"), fi.Name + ".chk"));
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
                        string newName3 = System.IO.Path.Combine(System.IO.Path.Combine(di.FullName, "RestoreInfo"), strNewName.Substring(strNewName.LastIndexOf('\\') + 1) + ".chk");
                        fi3.MoveTo(newName3);
                    }
                }
                else
                    if (regex3.IsMatch(fi.Name))
                    {
                        string strNewName = System.IO.Path.Combine(di.FullName, regex3.Replace(fi.Name, "$1-$2-$3 $4$5.$6"));
                        //string strNewName = System.IO.Path.Combine(di.FullName, regex3.Replace(fi.Name, "$2-$3-$4 $5.$6"));

                        System.IO.FileInfo fi2 = new System.IO.FileInfo(fi.FullName + ".modd");
                        string newModd = strNewName + ".modd";
                        System.IO.FileInfo fi3 = new System.IO.FileInfo(System.IO.Path.Combine(System.IO.Path.Combine(fi.DirectoryName, "RestoreInfo"), fi.Name + ".chk"));
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
                            string newName3 = System.IO.Path.Combine(System.IO.Path.Combine(di.FullName, "RestoreInfo"), strNewName.Substring(strNewName.LastIndexOf('\\') + 1) + ".chk");
                            fi3.MoveTo(newName3);
                        }

                    }
                    else
                        if (!regex2.IsMatch(fi.Name))
                        {
                            System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex(":");
                            string strNewName = null;
                            try {
                                using (System.IO.FileStream fs = new System.IO.FileStream(fi.FullName, System.IO.FileMode.Open, System.IO.FileAccess.Read))
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
                                                        string.Format("{0:D4}-{1:D2}-{2:D2} {3}", t.Year, t.Month, t.Day, fi.Name.Replace("DSC_", "").Replace("DSC ", "").Trim()));
                                                }
                                                else
                                                {
                                                    strNewName = System.IO.Path.Combine(di.FullName,
                                                        string.Format("{0:D4}-{1:D2}-{2:D2} {3}", fi.LastWriteTime.Year, fi.LastWriteTime.Month, fi.LastWriteTime.Day, fi.Name));
                                                }
                                            }
                                            else
                                            {
                                                strNewName = System.IO.Path.Combine(di.FullName,
                                                    string.Format("{0:D4}-{1:D2}-{2:D2} {3}", fi.LastWriteTime.Year, fi.LastWriteTime.Month, fi.LastWriteTime.Day, fi.Name));
                                            }
                                        }
                                        catch (ArgumentException)
                                        {
                                            strNewName = System.IO.Path.Combine(di.FullName,
                                                string.Format("{0:D4}-{1:D2}-{2:D2} {3}", fi.LastWriteTime.Year, fi.LastWriteTime.Month, fi.LastWriteTime.Day, fi.Name));
                                        }
                                    }
                            }
                            catch (ArgumentException)
                            {
                                strNewName = System.IO.Path.Combine(di.FullName,
                                    string.Format("{0:D4}-{1:D2}-{2:D2} {3}", fi.LastWriteTime.Year, fi.LastWriteTime.Month, fi.LastWriteTime.Day, fi.Name));
                            }


                            if (strNewName!=null)
                            {
                                System.IO.FileInfo fi2 = new System.IO.FileInfo(fi.FullName + ".modd");
                                string newModd = strNewName + ".modd";
                                System.IO.FileInfo fi3 = new System.IO.FileInfo(System.IO.Path.Combine(System.IO.Path.Combine(fi.DirectoryName, "RestoreInfo"), fi.Name + ".chk"));
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
                                    string newName3 = System.IO.Path.Combine(System.IO.Path.Combine(di.FullName, "RestoreInfo"), strNewName.Substring(strNewName.LastIndexOf('\\') + 1)+".chk");
                                    fi3.MoveTo(newName3);
                                }
                            }
                        }
            }
        }
    }
}
