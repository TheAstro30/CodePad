/* CodePad
 * Written by Jason James Newland
 * Copyright (C) 2020
 * Provided AS-IS with no warranty expressed or implied
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml.Serialization;
using CodePad.Utils;
using fctblib.Highlight;

namespace CodePad.Classes
{
    [Serializable, XmlRoot("settings")]
    public class Settings
    {
        /* Class that stores all the settings of CodePad in an XML file */
        public class Window
        {
            [XmlAttribute("size")]
            public string SizeString
            {
                get { return XmlFormatting.WriteSizeFormat(Size); }
                set { Size = XmlFormatting.ParseSizeFormat(value); }
            }

            [XmlIgnore]
            public Size Size { get; set; }

            [XmlAttribute("location")]
            public string LocationString
            {
                get { return XmlFormatting.WritePointFormat(Location); }
                set { Location = XmlFormatting.ParsePointFormat(value); }
            }

            [XmlIgnore]
            public Point Location { get; set; }

            [XmlAttribute("maximized")]
            public bool Maximized { get; set; }

            [XmlAttribute("theme")]
            public int Theme { get; set; }
        }

        public class Editor
        {
            [XmlAttribute("zoom")]
            public int Zoom { get; set; }

            [XmlAttribute("autoHighlight")]
            public bool AutoHighLight { get; set; }

            [XmlAttribute("syntaxLanguage")]
            public Language SyntaxLanguage { get; set; }
        }

        public class FileHistory
        {
            [XmlAttribute("fileName")]
            public string FileName { get; set; }
        }

        [XmlElement("applicationWindow")]
        public Window ApplicationWindow = new Window();

        [XmlElement("editor")]
        public Editor EditorWindow = new Editor();

        [XmlElement("recentFiles")]
        public List<FileHistory> RecentFiles = new List<FileHistory>(); 

        [XmlElement("recentOpenFiles")]
        public List<FileHistory> RecentOpenFiles = new List<FileHistory>(); 

        public Settings()
        {
            ApplicationWindow.Size = new Size(721, 449);
            ApplicationWindow.Location = new Point(150, 50);

            EditorWindow.Zoom = 100;
            EditorWindow.AutoHighLight = true;
            EditorWindow.SyntaxLanguage = Language.Custom;
        }
    }
}
