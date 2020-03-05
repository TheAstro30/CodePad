/* CodePad
 * Written by Jason James Newland
 * Copyright (C) 2020
 * Provided AS-IS with no warranty expressed or implied
 */
using System.IO;
using System.Windows.Forms;
using fctblib;
using fctblib.Highlight;
using fctblib.TextBoxEventArgs;
using tslib.Control;

namespace CodePad.Helpers
{
    public static class LanguageControl
    {
        public static Language GetSyntaxHighLightAuto(string ext)
        {
            /* Get the language automatically by file extension */
            if (!SettingsManager.ApplicationSettings.EditorWindow.AutoHighLight)
            {
                return SettingsManager.ApplicationSettings.EditorWindow.SyntaxLanguage;
            }
            if (!string.IsNullOrEmpty(ext))
            {
                switch (ext.ToUpper())
                {
                    case ".CS":
                        return Language.CSharp;

                    case ".HTM":
                    case ".HTML":
                        return Language.Html;

                    case ".VB":
                    case ".VBA":
                        return Language.Vb;

                    case ".XML":
                        return Language.Xml;

                    case ".JS":
                        return Language.Js;

                    case ".PHP":
                        return Language.Php;

                    case ".SQL":
                        return Language.Sql;
                }
            }
            return Language.Custom; /* No syntax highlighting essentially */
        }

        /* Syntax menu callback (main form) */
        public static void Syntax(object sender, FaTabStrip e)
        {
            var item = (ToolStripMenuItem) sender;
            var tab = e.SelectedItem;
            var doc = (FastColoredTextBox) tab.Controls[0];
            if (item == null)
            {
                return;
            }
            /* Set this to false first */
            SettingsManager.ApplicationSettings.EditorWindow.AutoHighLight = false;
            /* Get file extension */
            var ext = tab.Tag != null ? Path.GetExtension(tab.Tag.ToString()) : null;
            switch (item.Text.ToUpper())
            {
                case "AUTO":
                    doc.Language = GetSyntaxHighLightAuto(ext);
                    SettingsManager.ApplicationSettings.EditorWindow.AutoHighLight = true;
                    break;

                case "XML":
                    doc.Language = Language.Xml;
                    break;

                case "HTML":
                    doc.Language = Language.Html;
                    break;

                case "PHP":
                    doc.Language = Language.Php;
                    break;

                case "SQL":
                    doc.Language = Language.Sql;
                    break;

                case "JAVA SCRIPT":
                    doc.Language = Language.Js;
                    break;

                case "CSHARP (C#)":
                    doc.Language = Language.CSharp;
                    break;

                case "VISUAL BASIC (VB)":
                    doc.Language = Language.Vb;
                    break;
            }
            /* Force update */
            doc.OnSyntaxHighlight(new TextChangedEventArgs(doc.Range));            
            /* Check current selection */
            item.Checked = true;
            SettingsManager.ApplicationSettings.EditorWindow.SyntaxLanguage = doc.Language;
        }
    }
}
