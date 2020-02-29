/* CodePad
 * Written by Jason James Newland
 * Copyright (C) 2020
 * Provided AS-IS with no warranty expressed or implied
 */
using System;
using System.Windows.Forms;
using CodePad.Forms;
using corelib.Classes.Theme;
using corelib.Helpers;

namespace CodePad.Helpers
{
    public static class Menus
    {
        /* This just splits up the code from the main form */
        public static void MenuClickCallback(object sender, EventArgs e)
        {
            var item = (ToolStripMenuItem)sender;
            var doc = Tabs.GetActiveDocument();
            if (item == null)
            {
                return;
            }
            var id = string.IsNullOrEmpty(item.Text) ? item.Tag.ToString() : item.Text.ToUpper();
            switch (id)
            {
                case "NEW":
                    Tabs.CreateTab();
                    break;

                case "OPEN":
                    FileOperations.OpenFile();
                    break;

                case "SAVE":
                    FileOperations.SaveFile(doc);
                    break;

                case "SAVE AS...":
                    FileOperations.SaveFileAs(doc);
                    break;

                case "SAVE ALL":
                    FileOperations.SaveAllFiles();
                    break;

                case "CLOSE":
                    if (doc != null)
                    {
                        doc.Close();
                    }
                    break;

                case "PRINT":
                    if (doc != null)
                    {
                        doc.Box.Print();
                    }
                    break;

                case "EXIT":
                    Tabs.MainForm.Close();
                    break;

                case "ABOUT":
                    using (var d = new FrmAbout())
                    {
                        d.ShowDialog(Tabs.MainForm);
                    }
                    break;
            }
        }

        public static void MenuThemeClickCallback(object sender, EventArgs e)
        {
            var item = (ToolStripMenuItem)sender;
            if (item == null)
            {
                return;
            }
            int index;
            if (!Int32.TryParse(item.Tag.ToString(), out index))
            {
                return;
            }
            ThemeManager.SetTheme(index);
            SettingsManager.ApplicationSettings.ApplicationWindow.Theme = index;
            /* Uncheck any currently checked items */
            var parent = (ToolStripMenuItem)item.OwnerItem;
            foreach (ToolStripMenuItem subItem in parent.DropDownItems)
            {
                subItem.Checked = false;
            }
            /* Check selected preset */
            item.Checked = true;
        }
    }
}
