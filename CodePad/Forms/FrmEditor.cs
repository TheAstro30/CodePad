/* CodePad
 * Written by Jason James Newland
 * Copyright (C) 2020
 * Provided AS-IS with no warranty expressed or implied
 */
using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using CodePad.Classes.Theme;
using CodePad.Helpers;
using fctblib;
using tslib.Control;

namespace CodePad.Forms
{
    public sealed partial class FrmEditor : Form
    {
        /* Your mom wrote this code */
        private readonly bool _initialize;

        private readonly MenuStrip _menu;

        private readonly ToolStripMenuItem _mnuFile;
        private readonly ToolStripMenuItem _mnuView;
        private readonly ToolStripMenuItem _mnuHelp;

        private readonly ToolStrip _toolBar;

        private readonly ToolStripMenuItem _btnNew;
        private readonly ToolStripMenuItem _btnOpen;
        private readonly ToolStripMenuItem _btnSave;
        private readonly ToolStripMenuItem _btnSaveAll;
        private readonly ToolStripMenuItem _btnPrint;

        private readonly FaTabStrip _tsFiles;

        private readonly StatusStrip _statusBar;

        public FrmEditor()
        {
            _initialize = true;
            InitializeComponent();
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            Text = string.Format("CodePad - v{0}.{1}.{2} (build: {3}) ©2020 KangaSoft Software", version.Major,
                                 version.Minor,
                                 version.MinorRevision, version.Build);
            /* Load application settings */
            SettingsManager.Load();
            /* Set window size, location and window state */
            ClientSize = SettingsManager.ApplicationSettings.ApplicationWindow.Size;
            Location = SettingsManager.ApplicationSettings.ApplicationWindow.Location;
            WindowState = SettingsManager.ApplicationSettings.ApplicationWindow.Maximized
                              ? FormWindowState.Maximized
                              : FormWindowState.Normal;
            /* Set menu and toolbar */
            _menu = new MenuStrip
                        {
                            CanOverflow = false,
                            GripStyle = ToolStripGripStyle.Visible,
                            Stretch = true,
                            Padding = new Padding(2, 2, 0, 2)
                        };

            /* File */
            _mnuFile = new ToolStripMenuItem {AutoSize = true, Text = @"&File"};
            _mnuFile.DropDownItems.AddRange(
                new ToolStripItem[]
                    {
                        new ToolStripMenuItem("New", Properties.Resources.fileNew.ToBitmap(), MenuClickCallback, Keys.Control | Keys.N),
                        new ToolStripSeparator(),
                        new ToolStripMenuItem("Open", Properties.Resources.fileOpen.ToBitmap(), MenuClickCallback, Keys.Control | Keys.O),
                        new ToolStripMenuItem("Save", Properties.Resources.fileSave.ToBitmap(), MenuClickCallback, Keys.Control | Keys.S),
                        new ToolStripMenuItem("Save As...", null, MenuClickCallback, Keys.Control | Keys.Shift | Keys.S)
                        ,
                        new ToolStripMenuItem("Save All", Properties.Resources.fileSaveAll.ToBitmap(), MenuClickCallback),
                        new ToolStripSeparator(),
                        new ToolStripMenuItem("Close", null, MenuClickCallback, Keys.Control | Keys.F4),
                        new ToolStripSeparator(),
                        new ToolStripMenuItem("Print", Properties.Resources.filePrint.ToBitmap(), MenuClickCallback, Keys.Control | Keys.P),
                        new ToolStripSeparator(),
                        new ToolStripMenuItem("Exit", null, MenuClickCallback, Keys.Alt | Keys.F4)
                    });
            /* View */
            _mnuView = new ToolStripMenuItem {AutoSize = true, Text = @"&View"};
            var theme = new ToolStripMenuItem {Text = @"Theme"};
            var index = 0;
            var themeIndex = SettingsManager.ApplicationSettings.ApplicationWindow.Theme;
            foreach (var preset in ThemeManager.Presets)
            {
                var d = new ToolStripMenuItem(preset.Name) {Tag = index};
                d.Click += MenuThemeClickCallback;
                if (index == themeIndex)
                {
                    d.Checked = true;
                }
                theme.DropDownItems.Add(d);
                index++;
            }
            _mnuView.DropDownItems.AddRange(new ToolStripItem[] {theme});
            /* Help */
            _mnuHelp = new ToolStripMenuItem {AutoSize = true, Text = @"&Help"};
            _mnuHelp.DropDownItems.AddRange(new ToolStripItem[]
                                                {
                                                    new ToolStripMenuItem("About", null, MenuClickCallback, Keys.None)
                                                });

            _menu.Items.AddRange(new ToolStripItem[] {_mnuFile, _mnuView, _mnuHelp});

            _toolBar = new ToolStrip
                           {
                               Stretch = true,
                               AutoSize = false,
                               ImageScalingSize = new Size(16, 16),
                               Padding = new Padding(3, 0, 0, 1),
                               Size = new Size(704, 25)
                           };

            _btnNew = new ToolStripMenuItem
                          {
                              Image = Properties.Resources.fileNew.ToBitmap(),
                              ImageScaling = ToolStripItemImageScaling.None,
                              Size = new Size(16, 16),
                              ToolTipText = @"New document",
                              Tag = "NEW"
                          };
            _btnNew.Click += MenuClickCallback;

            _btnOpen = new ToolStripMenuItem
                           {
                               Image = Properties.Resources.fileOpen.ToBitmap(),
                               ImageScaling = ToolStripItemImageScaling.None,
                               Size = new Size(16, 16),
                               ToolTipText = @"Open",
                               Tag = "OPEN"
                           };
            _btnOpen.Click += MenuClickCallback;

            _btnSave = new ToolStripMenuItem
                           {
                               Image = Properties.Resources.fileSave.ToBitmap(),
                               ImageScaling = ToolStripItemImageScaling.None,
                               Size = new Size(16, 16),
                               ToolTipText = @"Save",
                               Tag = "SAVE"
                           };
            _btnSave.Click += MenuClickCallback;

            _btnSaveAll = new ToolStripMenuItem
                              {
                                  Image = Properties.Resources.fileSaveAll.ToBitmap(),
                                  ImageScaling = ToolStripItemImageScaling.None,
                                  Size = new Size(16, 16),
                                  ToolTipText = @"Save All",
                                  Tag = "SAVE ALL"
                              };
            _btnSaveAll.Click += MenuClickCallback;

            _btnPrint = new ToolStripMenuItem
                            {
                                Image = Properties.Resources.filePrint.ToBitmap(),
                                ImageScaling = ToolStripItemImageScaling.None,
                                Size = new Size(16, 16),
                                ToolTipText = @"Print",
                                Tag = "PRINT"
                            };
            _btnPrint.Click += MenuClickCallback;

            _toolBar.Items.AddRange(new ToolStripItem[]
                                        {
                                            _btnNew,
                                            new ToolStripSeparator(),
                                            _btnOpen,
                                            _btnSave,
                                            _btnSaveAll,
                                            new ToolStripSeparator(),
                                            _btnPrint
                                        });

            _tsFiles = new FaTabStrip
                          {
                              Font =
                                  new Font("Segoe UI", 9F, FontStyle.Regular,
                                           GraphicsUnit.Point, 0),
                              Location = new Point(136, 70),
                              Dock = DockStyle.Fill,
                              Size = new Size(350, 200),
                              TabIndex = 10
                          };

            _statusBar = new StatusStrip
                             {
                                 Location = new Point(0, 451),
                                 Padding = new Padding(1, 0, 16, 0),
                                 RenderMode = ToolStripRenderMode.Professional,
                                 Size = new Size(822, 22),
                                 Dock = DockStyle.Bottom
                             };

            Controls.AddRange(new Control[] {_tsFiles, _statusBar, _toolBar, _menu});
            /* Set renderers for toolbars */            
            _statusBar.RenderMode = ToolStripRenderMode.ManagerRenderMode;

            ThemeManager.ThemeChanged += ThemeChanged;
            ToolStripManager.Renderer = ThemeManager.Renderer;
            ThemeManager.SetTheme(SettingsManager.ApplicationSettings.ApplicationWindow.Theme);
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

        /* Private methods */
        private void CreateTab(FileSystemInfo fileInfo = null)
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
            tb.TextChangedDelayed += OnTextBoxTextChanged;
            tb.ZoomChanged += OnZoomChanged;
            var fileName = fileInfo != null ? fileInfo.FullName : null;
            var caption = !string.IsNullOrEmpty(fileName) ? Path.GetFileName(fileName) : "Untitled.txt";
            var tab = new FaTabStripItem(caption, tb) {Tag = fileName};
            if (fileInfo != null)
            {
                tb.OpenFile(fileInfo.FullName);
            }
            _tsFiles.AddTab(tab);
            _tsFiles.SelectedItem = tab;
            tb.Focus();
        }

        private void OpenFile()
        {
            using (var ofd = new OpenFileDialog { Title = @"Select a file to open" })
            {
                ofd.Filter = FileFilters.GetFilters();
                if (ofd.ShowDialog(this) == DialogResult.Cancel)
                {
                    return;
                }
                var info = new FileInfo(ofd.FileName);
                CreateTab(info);
            }
        }

        private void SaveFile(FaTabStripItem f)
        {
            if (f == null)
            {
                return;
            }
            var tb = (FastColoredTextBox) f.Controls[0];
            string fileName;
            if (f.Tag == null)
            {
                fileName = SaveFileAs(f);
                if (string.IsNullOrEmpty(fileName))
                {
                    return;
                }
            }
            else
            {
                fileName = f.Tag.ToString();
            }
            try
            {
                System.Diagnostics.Debug.Print("Ne file " + fileName);
                File.WriteAllText(fileName, tb.Text);
                tb.IsChanged = false;
                f.Title = Path.GetFileName(fileName);
            }
            catch (Exception ex)
            {
                if (MessageBox.Show(ex.Message, @"Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Retry)
                {
                    SaveFile(f);
                }
            }
        }

        private string SaveFileAs(FaTabStripItem f)
        {
            using (var sfd = new SaveFileDialog { Title = string.Format(@"Select the filename to save {0} as...", f.Title) })
            {
                sfd.Filter = FileFilters.GetFilters();
                sfd.FileName = Path.GetFileNameWithoutExtension(f.Title);
                sfd.FilterIndex = FileFilters.GetExtensionIndex(string.Format("*{0}", Path.GetExtension(f.Title)));
                if (sfd.ShowDialog(this) == DialogResult.Cancel)
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
            foreach (var tab in _tsFiles.Items)
            {
                SaveFile((FaTabStripItem) tab);
            }
        }

        /* Callbacks */
        private void MenuClickCallback(object sender, EventArgs e)
        {
            var item = (ToolStripMenuItem)sender;
            var doc = _tsFiles.SelectedItem;
            if (item == null)
            {
                return;
            }
            var id = string.IsNullOrEmpty(item.Text) ? item.Tag.ToString() : item.Text.ToUpper();
            switch (id)
            {
                case "NEW":
                    CreateTab();
                    break;

                case "OPEN":
                    OpenFile();
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
                        _tsFiles.RemoveTab(doc);
                    }
                    break;

                case "PRINT":
                    if (doc != null)
                    {
                        ((FastColoredTextBox)doc.Tag).Print();
                    }
                    break;

                case "EXIT":
                    Close();
                    break;

                case "ABOUT":
                    using (var d = new FrmAbout())
                    {
                        d.ShowDialog(this);
                    }
                    break;
            }
        }

        private static void MenuThemeClickCallback(object sender, EventArgs e)
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

        private void ThemeChanged(int index)
        {
            foreach (FaTabStripItem tab in _tsFiles.Items)
            {
                var tb = (FastColoredTextBox) tab.Controls[0];
                tb.BackColor = index == 1 ? Color.Black : Color.White;
                tb.ForeColor = index == 1 ? Color.Silver : Color.Black;
            }
        }

        private void OnTextBoxTextChanged(object sender, EventArgs e)
        {
            /* We use this event to update the status bar */
            System.Diagnostics.Debug.Print("Firing text change: " + DateTime.Now);
        }

        private static void OnZoomChanged(object sender, EventArgs e)
        {
            var tb = (FastColoredTextBox) sender;
            if (tb == null)
            {
                return;
            }
            SettingsManager.ApplicationSettings.EditorWindow.Zoom = tb.Zoom;
        }
    }
}
