/* CodePad
 * Written by Jason James Newland
 * Copyright (C) 2020
 * Provided AS-IS with no warranty expressed or implied
 */
using System;
using System.IO;
using System.Windows.Forms;
using corelib.Helpers;
using fctblib;

namespace CodePad.Forms
{
    public sealed class FrmDocument : Form
    {
        private bool _documentLoaded;

        public FileInfo CurrentFileInfo { get; set; }

        public bool ContentsChanged { get; set; }

        public FastColoredTextBox Box { get; set; }

        public FrmDocument()
        {
            ShowInTaskbar = false;
            ShowIcon = false;
            /* Initialize new FCTB and set basic settings of document window */
            Box = new FastColoredTextBox
                      {
                          Dock = DockStyle.Fill,
                          Zoom = SettingsManager.ApplicationSettings.EditorWindow.Zoom
                      };
            Controls.Add(Box);

            BackColor = Box.BackColor;

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
            if (e.CloseReason == CloseReason.MdiFormClosing || !ContentsChanged)
            {
                /* Do not trigger this multiple times on application exit - or if text hasn't changed */
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

        public void LoadDocumentText(FileInfo fileInfo)
        {
            _documentLoaded = true;
            CurrentFileInfo = fileInfo;
            using (var fs = new FileStream(fileInfo.FullName, FileMode.Open))
            {
                using (var sr = new StreamReader(fs))
                {
                    Box.Text = sr.ReadToEnd();
                }
            }
        }

        public void SaveDocumentText(FileInfo fileInfo)
        {
            using (var fs = new FileStream(fileInfo.FullName, FileMode.Create))
            {
                using (var sw = new StreamWriter(fs))
                {
                    sw.Write(Box.Text);
                    sw.Flush();
                    fs.Flush();
                }                
            }
            ContentsChanged = false;
            /* Make sure we update the title text of this window */
            Text = Path.GetFileName(fileInfo.FullName);
        }

        /* Callbacks */
        private void TextChangedDelayed(object sender, EventArgs e)
        {            
            if (_documentLoaded)
            {
                _documentLoaded = false;
                return;                
            }
            ContentsChanged = true;
        }

        private void ZoomChanged(object sender, EventArgs e)
        {
            SettingsManager.ApplicationSettings.EditorWindow.Zoom = Box.Zoom;
        }
    }
}
