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
            /* Enumerate recently opened file(s) list and reopen documentsv */
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

        /* Private methods */
        private void CreateTab(FileInfo fileInfo)
        {
            CreateTab(Path.GetFileName(fileInfo.FullName));
        }

        private void CreateTab(string name = @"Untitled.txt")
        {
            NewDocumentWindow(name);
        }

        private void NewDocumentWindow(string name)
        {
            var f = new FrmDocument
                        {
                            Text = name,
                            MdiParent = this
                        };
            f.Show();
        }
    }
}
