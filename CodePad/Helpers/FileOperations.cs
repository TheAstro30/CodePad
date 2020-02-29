/* CodePad
 * Written by Jason James Newland
 * Copyright (C) 2020
 * Provided AS-IS with no warranty expressed or implied
 */
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CodePad.Forms;

namespace CodePad.Helpers
{
    public static class FileOperations
    {
        /* This just splits up the code from the main form */
        public static void OpenFile()
        {
            using (var ofd = new OpenFileDialog{Title = @"Select a file to open"})
            {
                ofd.Filter = FileFilters.GetFilters();
                if (ofd.ShowDialog(Tabs.MainForm) == DialogResult.Cancel)
                {
                    return;
                }
                var info = new FileInfo(ofd.FileName);
                Tabs.CreateTab(info);
            }
        }

        public static void SaveFile(FrmDocument f)
        {
            if (f == null)
            {
                return;
            }
            if (f.Text == @"Untitled.txt")
            {
                SaveFileAs(f);
                return;
            }
            f.SaveDocumentText(f.CurrentFileInfo);
        }

        public static void SaveFileAs(FrmDocument f)
        {
            if (f == null)
            {
                return;
            }
            using (var sfd = new SaveFileDialog{Title = string.Format(@"Select the filename to save {0} as...",f.Text)})
            {
                sfd.Filter = FileFilters.GetFilters();
                sfd.FileName = Path.GetFileNameWithoutExtension(f.Text);
                sfd.FilterIndex = FileFilters.GetExtensionIndex(string.Format("*{0}", Path.GetExtension(f.Text)));
                if (sfd.ShowDialog(Tabs.MainForm) == DialogResult.Cancel)
                {
                    return;
                }
                var info = new FileInfo(sfd.FileName);
                f.CurrentFileInfo = info;
                f.SaveDocumentText(info);                
            }
        }

        public static void SaveAllFiles()
        {
            foreach (var d in from FrmDocument d in Tabs.MainForm.MdiChildren where d != null && d.ContentsChanged select d)
            {
                SaveFile(d);
            }
        }
    }
}
