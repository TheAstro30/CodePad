/* CodePad
 * Written by Jason James Newland
 * Copyright (C) 2020
 * Provided AS-IS with no warranty expressed or implied
 */
using System;
using System.Windows.Forms;
using CodePad.Forms;

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
            switch (item.Text.ToUpper())
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
    }
}
