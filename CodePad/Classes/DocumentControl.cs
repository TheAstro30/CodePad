/* CodePad
 * Written by Jason James Newland
 * Copyright (C) 2020
 * Provided AS-IS with no warranty expressed or implied
 */
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using CodePad.Helpers;
using fctblib;
using fctblib.TextBoxEventArgs;
using tslib.Control;

namespace CodePad.Classes
{
    public class DocumentControl
    {
        private readonly FaTabStrip _tabStrip;

        public DocumentControl(FaTabStrip tabStrip)
        {
            _tabStrip = tabStrip;
        }

        /* Menu callback */
        public void FileOperation(ToolStripMenuItem item, EventHandler<TextChangedEventArgs> textChanged)
        {
            var doc = _tabStrip.SelectedItem;            
            var id = string.IsNullOrEmpty(item.Text) ? item.Tag.ToString() : item.Text.ToUpper();
            switch (id)
            {
                case "NEW":
                    CreateTab(textChanged);
                    break;

                case "OPEN":
                    OpenFile(textChanged);
                    break;

                case "SAVE":
                    SaveFile(doc);
                    break;

                case "SAVE AS...":
                    var fileName = SaveFileAs(doc);
                    if (string.IsNullOrEmpty(fileName))
                    {
                        return;
                    }
                    SaveFile(doc);
                    break;

                case "SAVE ALL":
                    SaveAllFiles();
                    break;

                case "CLOSE":
                    if (doc != null)
                    {
                        /* Check that the file doesn't need saving */
                        _tabStrip.RemoveTab(doc);
                    }
                    break;

                case "PRINT":
                    if (doc != null)
                    {
                        ((FastColoredTextBox)doc.Tag).Print();
                    }
                    break;                
            }
        }

        /* Public methods */
        public void CreateTab(EventHandler<TextChangedEventArgs> textChanged, FileSystemInfo fileInfo = null)
        {
            var index = SettingsManager.ApplicationSettings.ApplicationWindow.Theme;
            var tb = new FastColoredTextBox
                         {
                             Dock = DockStyle.Fill,
                             LeftPadding = 5,
                             BorderStyle = BorderStyle.None,
                             DelayedTextChangedInterval = 500,
                             BackColor = index == 1 ? Color.Black : Color.White,
                             ForeColor = index == 1 ? Color.Silver : Color.Black,
                             Zoom = SettingsManager.ApplicationSettings.EditorWindow.Zoom
                         };
            tb.TextChangedDelayed += textChanged;
            tb.ZoomChanged += OnZoomChanged;
            var fileName = fileInfo != null ? fileInfo.FullName : null;
            var caption = !string.IsNullOrEmpty(fileName) ? Path.GetFileName(fileName) : "Untitled.txt";
            var tab = new FaTabStripItem(caption, tb) {Tag = fileName};
            if (fileInfo != null)
            {
                tb.OpenFile(fileInfo.FullName);
                tb.Language = LanguageControl.GetSyntaxHighLightAuto(Path.GetExtension(fileInfo.FullName));
                tb.OnSyntaxHighlight(new TextChangedEventArgs(tb.Range));
            }
            _tabStrip.AddTab(tab);
            _tabStrip.SelectedItem = tab;
            tb.Focus();
        }

        public void DestroyTab(FastColoredTextBox tb)
        {
            tb.ZoomChanged -= OnZoomChanged;
        }

        public bool SaveFile(FaTabStripItem f)
        {
            if (f == null)
            {
                return false;
            }
            var tb = (FastColoredTextBox)f.Controls[0];
            return SaveFile(f, tb);
        }

        public bool SaveFile(FaTabStripItem f, FastColoredTextBox tb)
        {
            string fileName;
            if (f.Tag == null)
            {
                fileName = SaveFileAs(f);
                if (string.IsNullOrEmpty(fileName))
                {
                    return false;
                }
            }
            else
            {
                fileName = f.Tag.ToString();
            }
            try
            {
                File.WriteAllText(fileName, tb.Text);
                tb.IsChanged = false;
                f.Title = Path.GetFileName(fileName);
            }
            catch (Exception ex)
            {
                if (MessageBox.Show(ex.Message, @"Error Saving File", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Retry)
                {
                    SaveFile(f);
                }
                return false;
            }
            return true;
        }

        /* Private methods */
        private void OpenFile(EventHandler<TextChangedEventArgs> textChanged)
        {
            using (var ofd = new OpenFileDialog { Title = @"Select a file to open" })
            {
                ofd.Filter = FileFilters.GetFilters();
                if (ofd.ShowDialog(_tabStrip.Parent) == DialogResult.Cancel)
                {
                    return;
                }
                var info = new FileInfo(ofd.FileName);
                CreateTab(textChanged, info);
            }
        }
        
        private string SaveFileAs(FaTabStripItem f)
        {
            using (var sfd = new SaveFileDialog { Title = string.Format(@"Select the filename to save {0} as...", f.Title) })
            {
                sfd.Filter = FileFilters.GetFilters();
                sfd.FileName = Path.GetFileNameWithoutExtension(f.Title);
                sfd.FilterIndex = FileFilters.GetExtensionIndex(string.Format("*{0}", Path.GetExtension(f.Title)));
                if (sfd.ShowDialog(_tabStrip.Parent) == DialogResult.Cancel)
                {
                    return string.Empty;
                }
                var fileName = sfd.FileName;
                f.Tag = fileName;
                System.Diagnostics.Debug.Print(fileName);
                return fileName;
            }
        }

        private void SaveAllFiles()
        {
            foreach (var tab in _tabStrip.Items)
            {
                SaveFile((FaTabStripItem)tab);
            }
        }

        private static void OnZoomChanged(object sender, EventArgs e)
        {
            var tb = (FastColoredTextBox)sender;
            if (tb == null)
            {
                return;
            }
            SettingsManager.ApplicationSettings.EditorWindow.Zoom = tb.Zoom;
        }
    }
}
