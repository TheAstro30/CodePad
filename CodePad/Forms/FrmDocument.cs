/* CodePad
 * Written by Jason James Newland
 * Copyright (C) 2020
 * Provided AS-IS with no warranty expressed or implied
 */
using System;
using System.Windows.Forms;
using corelib.Helpers;
using fctblib;

namespace CodePad.Forms
{
    public sealed class FrmDocument : Form
    {
        public bool ContentsChanged { get; set; }

        public FastColoredTextBox Box { get; set; }

        public FrmDocument()
        {
            ShowInTaskbar = false;
            ShowIcon = false;

            Box = new FastColoredTextBox
                      {
                          Dock = DockStyle.Fill,
                          Zoom = SettingsManager.ApplicationSettings.EditorWindow.Zoom
                      };
            Controls.Add(Box);

            Box.TextChangedDelayed += TextChangedDelayed;
            Box.ZoomChanged += ZoomChanged;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (MdiParent == null)
            {
                return;
            }
            WindowState = FormWindowState.Normal;
            Dock = DockStyle.Fill;
            FormBorderStyle = FormBorderStyle.None;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.MdiFormClosing)
            {
                /* Do not trigger this multiple times on application exit */
                return;
            }
            /* Check current document doesn't need saving prior to closing this window/tab */
            if (MessageBox.Show(this, @"Close me?", @"Close this document?", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
            {
                e.Cancel = true;
                return;
            }
            base.OnFormClosing(e);
        }

        /* Callbacks */
        private void TextChangedDelayed(object sender, EventArgs e)
        {            
            ContentsChanged = true;
        }

        private void ZoomChanged(object sender, EventArgs e)
        {
            SettingsManager.ApplicationSettings.EditorWindow.Zoom = Box.Zoom;
        }
    }
}
