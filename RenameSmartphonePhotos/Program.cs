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
using System.Linq;
using System.Windows.Forms;
using System.Globalization;
using System.IO;

namespace RenameSmartphonePhotos
{
    //*******************************************************************************************************
    /// <summary>
    /// Main class of the application
    /// </summary>
    //*******************************************************************************************************
    static class Program
    {
        //===================================================================================================
        /// <summary>
        /// Indicates that the application runs in a predominantly islamic country;
        /// </summary>
        public static bool IslamicCountry; 

        //===================================================================================================
        /// <summary>
        /// Main entry point of the application
        /// </summary>
        //===================================================================================================
        [STAThread]
        static void Main()
        {
#if DEBUG
            //set test culture
            //string strSetCulture =
            // "af-ZA";
            // "ar-SA";
            // "az-AZ";
            // "be-BY";
            // "bg-BG";
            // "bs-Latn-BA";
            // "cs-CZ";
            // "da-DK";
            // "de-DE";
            // "el-GR";
            // "es-ES";
            // "et-EE";
            // "fa-IR";
            // "fi-FI";
            // "fr-FR";
            // "he-IL";
            // "hi-IN";
            // "hu-HU";
            // "hy-AM";
            // "id-ID";
            // "is-IS";
            // "it-IT";
            // "ja-JP";
            // "ka-GE";
            // "kk-KZ";
            // "km-KH";
            // "ko-KR";
            // "ky-KG";
            // "lt-LT";
            // "lv-LV";
            // "mk-MK";
            // "mn-MN";
            // "ms-MY";
            // "nl-NL";
            // "no-NO";
            // "pa-Arab-PK";
            // "pa-IN";
            // "pl-PL";
            // "ps-AF";
            // "pt-PT";
            // "en-US";
            // "ro-RO";
            // "ru-RU";
            // "sa-IN";
            // "sk-SK";
            // "sl-SL";
            // "sr-Latn-RS"; // TODO: need a fix
            // "sv-SE";
            // "tg-Cyrl-TJ";
            // "th-TH";
            // "tr-TR";
            // "uk-UA";
            // "uz-Latn-UZ";
            // "vi-VN";
            // "zh-TW";
            // "zh-CN";
            //System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo(strSetCulture);
            //System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo(strSetCulture);
#endif

            var astrIslamicCountries = new[]
            {
                "AF", "DZ", "BH", "BD", "BJ", "BN", "BF", "CM", "TD", "KM",
                "DJ", "EG", "GM", "GN", "ID", "IR", "IQ", "JO", "KZ", "KW",
                "KG", "LB", "LY", "MY", "MV", "ML", "MR", "MA", "MZ", "NE",
                "NG", "OM", "PK", "PS", "QA", "SA", "SN", "SO", "SS", "SD",
                "SY", "TJ", "TG", "TN", "TR", "TM", "AE", "UZ", "EH", "YE"
            };


            string cultureName = System.Threading.Thread.CurrentThread.CurrentUICulture.Name;
            IslamicCountry = Array.Exists(astrIslamicCountries, code => cultureName.Contains(code));

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
