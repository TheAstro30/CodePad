/* CodePad
 * Written by Jason James Newland
 * Copyright (C) 2020
 * Provided AS-IS with no warranty expressed or implied
 */
using System;
using System.IO;
using CodePad.Classes;
using CodePad.Utils.Serialization;

namespace CodePad.Helpers
{
    public sealed class SettingsManager
    {
        private static readonly string File =
            string.Format("{0}\\KangaSoft\\CodePad\\settings.xml",
                          Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));

        public static Settings ApplicationSettings = new Settings();

        static SettingsManager()
        {
            /* Check application path exists */
            var path = Path.GetDirectoryName(File);
            if (path != null && !Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public static void Load()
        {
            if (!XmlSerialize<Settings>.Load(File, ref ApplicationSettings))
            {
                ApplicationSettings = new Settings();
            }
        }

        public static void Save()
        {
            XmlSerialize<Settings>.Save(File, ApplicationSettings);
        }
    }
}
