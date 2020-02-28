/* CodePad
 * Written by Jason James Newland
 * Copyright (C) 2020
 * Provided AS-IS with no warranty expressed or implied
 */
using System;
using System.Reflection;
using System.Windows.Forms;
using CodePad.Helpers;
using corelib.Helpers;

namespace CodePad.Forms
{
    public sealed partial class FrmEditor : Form
    {
        /* This shit code will eventually be refactored into something more modular */
        private readonly bool _initialize;

        private readonly ToolStripMenuItem _mnuFile;
        private readonly ToolStripMenuItem _mnuHelp;

        public FrmEditor()
        {
            _initialize = true;
            InitializeComponent();
            /* Set this as it's important */
            Tabs.MainForm = this;
            Tabs.OnTextBoxTextChanged += TextBoxTextChanged;
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
            /* Set up menus */
            _mnuFile = new ToolStripMenuItem {AutoSize = true, Text = @"&File"};
            _mnuFile.DropDownItems.AddRange(
                new ToolStripItem[]
                    {
                        new ToolStripMenuItem("New", null, Menus.MenuClickCallback, Keys.Control | Keys.N),
                        new ToolStripSeparator(), 
                        new ToolStripMenuItem("Open", null, Menus.MenuClickCallback, Keys.Control | Keys.O),
                        new ToolStripMenuItem("Save", null, Menus.MenuClickCallback, Keys.Control | Keys.S),
                        new ToolStripMenuItem("Save as...", null, Menus.MenuClickCallback, Keys.Control | Keys.Shift | Keys.S),
                        new ToolStripSeparator(),
                        new ToolStripMenuItem("Close", null, Menus.MenuClickCallback, Keys.Control | Keys.F4),
                        new ToolStripSeparator(),
                        new ToolStripMenuItem("Print", null, Menus.MenuClickCallback, Keys.Control | Keys.P),
                        new ToolStripSeparator(),
                        new ToolStripMenuItem("Exit", null, Menus.MenuClickCallback, Keys.Alt | Keys.F4)
                    });
            _mnuHelp = new ToolStripMenuItem {AutoSize = true, Text = @"&Help"};
            _mnuHelp.DropDownItems.AddRange(new ToolStripItem[]
                                                {
                                                    new ToolStripMenuItem("About", null, Menus.MenuClickCallback, Keys.None) 
                                                });
            mnuMain.Items.AddRange(new ToolStripItem[] {_mnuFile, _mnuHelp});
            _initialize = false;
        }

        protected override void OnLoad(EventArgs e)
        {
            /* Enumerate recently opened file(s) list and reopen documents */
            Tabs.CreateTab();
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

        /* Callbacks */
        private void TextBoxTextChanged(object sender, EventArgs e)
        {
            /* We use this event to update the status bar */
            System.Diagnostics.Debug.Print("Firing text change: " + DateTime.Now);
        }
    }
}
