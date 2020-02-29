/* CodePad
 * Written by Jason James Newland
 * Copyright (C) 2020
 * Provided AS-IS with no warranty expressed or implied
 */
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using CodePad.Forms;
using corelib.Helpers;

namespace CodePad.Helpers
{
    public static class Tabs
    {
        /* This just splits up the code from the main form */
        public static Form MainForm { get; set; }

        public static event Action<object, EventArgs> OnTextBoxTextChanged;

        public static void CreateTab(FileInfo fileInfo)
        {
            var f = CreateTab(Path.GetFileName(fileInfo.FullName));
            /* Now, load the file into the text box */
            f.LoadDocumentText(fileInfo);
        }

        public static FrmDocument CreateTab(string name = @"Untitled.txt")
        {
            return NewDocumentWindow(name);
        }

        private static FrmDocument NewDocumentWindow(string name)
        {
            var index = SettingsManager.ApplicationSettings.ApplicationWindow.Theme;
            var f = new FrmDocument
                        {
                            Text = name,
                            MdiParent = MainForm,
                            Box =
                                {
                                    BackColor = index == 1 ? Color.DimGray : Color.White,
                                    ForeColor = index == 1 ? Color.FloralWhite : Color.Black
                                }
                        };
            f.BackColor = f.Box.BackColor;
            f.Show();
            f.Box.TextChangedDelayed += TextBoxTextChanged;
            return f;
        }

        public static FrmDocument GetActiveDocument()
        {
            var f = (FrmDocument)MainForm.ActiveMdiChild;
            return f;
        }

        private static void TextBoxTextChanged(object sender, EventArgs e)
        {
            if (OnTextBoxTextChanged != null)
            {
                OnTextBoxTextChanged(sender, e);
            }
        }
    }
}
