/* CodePad
 * Written by Jason James Newland
 * Copyright (C) 2020
 * Provided AS-IS with no warranty expressed or implied
 */
using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using corelib.Helpers;

namespace CodePad.Forms
{
    public sealed partial class FrmEditor : Form
    {
        /* This shit code will eventually be refactored into something more modular */
        private readonly bool _initialize;

        public FrmEditor()
        {
            _initialize = true;
            InitializeComponent();
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            Text = string.Format("CodePad - v{0}.{1}.{2} (build: {3}) ©2020 KangaSoft Software", version.Major, version.Minor,
                                 version.MinorRevision, version.Build);
            /* Load application settings */
            SettingsManager.Load();
            /* Set window size, location and window state */
            ClientSize = SettingsManager.ApplicationSettings.ApplicationWindow.Size;
            Location = SettingsManager.ApplicationSettings.ApplicationWindow.Location;
            WindowState = SettingsManager.ApplicationSettings.ApplicationWindow.Maximized
                              ? FormWindowState.Maximized
                              : FormWindowState.Normal;
            _initialize = false;
        }

        protected override void OnLoad(EventArgs e)
        {
            /* Enumerate recently opened file(s) list and reopen documents */
            CreateTab();
            base.OnLoad(e);
        }

        protected override void OnResizeEnd(EventArgs e)
        {
            if (_initialize || !Visible || WindowState == FormWindowState.Maximized)
            {
                return;
            }
            /* Update window size settings */
            SettingsManager.ApplicationSettings.ApplicationWindow.Size = ClientSize;
            base.OnResizeEnd(e);
        }

        protected override void OnMove(EventArgs e)
        {
            if (_initialize || !Visible)
            {
                return;
            }
            /* Update window location, size and maximized state */            
            SettingsManager.ApplicationSettings.ApplicationWindow.Maximized = WindowState == FormWindowState.Maximized;
            if (WindowState != FormWindowState.Maximized)
            {
                SettingsManager.ApplicationSettings.ApplicationWindow.Size = ClientSize;
                SettingsManager.ApplicationSettings.ApplicationWindow.Location = Location;
            }
            base.OnMove(e);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            /* Check all open documents prior to closing to see if we need to save any */
            /* Save application settings */
            SettingsManager.Save();
            base.OnFormClosing(e);
        }

        private void itmNew_Click(object sender, EventArgs e)
        {
            //temporary code
            CreateTab();
        }

        private void itmOpen_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            SaveFile(GetActiveDocument());
        }

        /* File operations */
        private void OpenFile()
        {
            using (var ofd = new OpenFileDialog{Title = @"Select a file to open"})
            {
                if (ofd.ShowDialog(this) == DialogResult.Cancel)
                {
                    return;
                }
                var info = new FileInfo(ofd.FileName);
                CreateTab(info);
            }
        }

        private void SaveFile(FrmDocument f)
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

        private void SaveFileAs(FrmDocument f)
        {
            if (f == null)
            {
                return;
            }
            using (var sfd = new SaveFileDialog{Title = @"Select the file to save"})
            {
                if (sfd.ShowDialog(this) == DialogResult.Cancel)
                {
                    return;
                }
                var info = new FileInfo(sfd.FileName);
                f.CurrentFileInfo = info;
                f.SaveDocumentText(info);
            }
        }

        /* Private methods */
        private void CreateTab(FileInfo fileInfo)
        {
            var f = CreateTab(Path.GetFileName(fileInfo.FullName));
            /* Now, load the file into the text box */
            f.LoadDocumentText(fileInfo);
        }

        private FrmDocument CreateTab(string name = @"Untitled.txt")
        {
            return NewDocumentWindow(name);
        }

        private FrmDocument NewDocumentWindow(string name)
        {
            var f = new FrmDocument
                        {
                            Text = name,
                            MdiParent = this
                        };
            f.Show();
            f.Box.TextChangedDelayed += TextBoxTextChanged;
            return f;
        }

        private FrmDocument GetActiveDocument()
        {
            var f = (FrmDocument)ActiveMdiChild;
            return f;
        }

        /* Callbacks */
        private void TextBoxTextChanged(object sender, EventArgs e)
        {
            /* We use this event to update the status bar */
        }

        private void MenuClickCallback(object sender, EventArgs e)
        {
            
        }
    }
}
