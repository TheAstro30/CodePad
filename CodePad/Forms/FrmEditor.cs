/* CodePad
 * Written by Jason James Newland
 * Copyright (C) 2020
 * Provided AS-IS with no warranty expressed or implied
 */
using System;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using CodePad.Classes;
using CodePad.Classes.Theme;
using CodePad.Helpers;
using fctblib;
using fctblib.Highlight;
using tslib;
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
        private readonly ToolStripMenuItem _mnuSyntax;
        private readonly ToolStripMenuItem _mnuHelp;

        private readonly ToolStrip _toolBar;

        private readonly ToolStripMenuItem _btnNew;
        private readonly ToolStripMenuItem _btnOpen;
        private readonly ToolStripMenuItem _btnSave;
        private readonly ToolStripMenuItem _btnSaveAll;
        private readonly ToolStripMenuItem _btnPrint;

        private readonly FaTabStrip _tsFiles;

        private readonly StatusStrip _statusBar;

        private readonly DocumentControl _doc;

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
                        new ToolStripMenuItem("Save As...", Properties.Resources.fileSaveAs.ToBitmap(), MenuClickCallback, Keys.Control | Keys.Shift | Keys.S)
                        ,
                        new ToolStripMenuItem("Save All", Properties.Resources.fileSaveAll.ToBitmap(), MenuClickCallback),
                        new ToolStripSeparator(),
                        new ToolStripMenuItem("Close", Properties.Resources.fileClose.ToBitmap(), MenuClickCallback, Keys.Control | Keys.F4),
                        new ToolStripSeparator(),
                        new ToolStripMenuItem("Print", Properties.Resources.filePrint.ToBitmap(), MenuClickCallback, Keys.Control | Keys.P),
                        new ToolStripSeparator(),
                        new ToolStripMenuItem("Exit", Properties.Resources.fileExit.ToBitmap(), MenuClickCallback, Keys.Alt | Keys.F4)
                    });
            /* View */
            _mnuView = new ToolStripMenuItem {AutoSize = true, Text = @"&View"};
            var theme = new ToolStripMenuItem {Text = @"Theme", Image = Properties.Resources.viewTheme.ToBitmap()};
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
            /* Syntax */
            _mnuSyntax = new ToolStripMenuItem {AutoSize = true, Text = @"&Syntax"};
            _mnuSyntax.DropDownItems.AddRange(new ToolStripItem[]
                                                  {
                                                      new ToolStripMenuItem("Auto", null, MenuSyntaxClickCallback),
                                                      new ToolStripSeparator(),
                                                      new ToolStripMenuItem("CSharp (C#)", null, MenuSyntaxClickCallback),
                                                      new ToolStripMenuItem("Visual Basic (VB)", null, MenuSyntaxClickCallback),
                                                      new ToolStripMenuItem("HTML", null, MenuSyntaxClickCallback),
                                                      new ToolStripMenuItem("XML", null, MenuSyntaxClickCallback), 
                                                      new ToolStripMenuItem("SQL", null, MenuSyntaxClickCallback),
                                                      new ToolStripMenuItem("PHP", null, MenuSyntaxClickCallback),                                                      
                                                      new ToolStripMenuItem("Java Script", null, MenuSyntaxClickCallback)                                                      
                                                  });
            var lang = SettingsManager.ApplicationSettings.EditorWindow.SyntaxLanguage;
            if (SettingsManager.ApplicationSettings.EditorWindow.AutoHighLight)
            {
                ((ToolStripMenuItem) _mnuSyntax.DropDownItems[0]).Checked = true;
            }
            else if (lang != Language.Custom)
            {
                ((ToolStripMenuItem) _mnuSyntax.DropDownItems[(int) lang + 1]).Checked = true;
            }
            /* Help */
            _mnuHelp = new ToolStripMenuItem {AutoSize = true, Text = @"&Help"};
            _mnuHelp.DropDownItems.AddRange(new ToolStripItem[]
                                                {
                                                    new ToolStripMenuItem("About", null, MenuClickCallback, Keys.None)
                                                });

            _menu.Items.AddRange(new ToolStripItem[] {_mnuFile, _mnuView, _mnuSyntax, _mnuHelp});

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
            _tsFiles.TabStripItemSelectionChanged += OnTabStripSelectionChanged;
            _tsFiles.TabStripItemClosing += OnTabStripItemClosing;

            _doc = new DocumentControl(_tsFiles);

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
            _doc.CreateTab(OnTextBoxTextChanged);
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
            var items = _tsFiles.Items.Cast<FaTabStripItem>().ToList();
            foreach (var tab in items)
            {
                var args = new TabStripItemClosingEventArgs(tab);
                OnTabStripItemClosing(args);
                if (args.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
                _tsFiles.RemoveTab(tab);
            }
            /* Save application settings */
            SettingsManager.Save();
            base.OnFormClosing(e);
        }

        /* Callbacks */
        private void MenuClickCallback(object sender, EventArgs e)
        {            
            var item = (ToolStripMenuItem)sender;
            if (item == null)
            {
                return;
            }
            _doc.FileOperation(item, OnTextBoxTextChanged);
            var id = string.IsNullOrEmpty(item.Text) ? item.Tag.ToString() : item.Text.ToUpper();
            switch (id)
            {
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
            if (!int.TryParse(item.Tag.ToString(), out index))
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

        private void MenuSyntaxClickCallback(object sender, EventArgs e)
        {
            /* Unselect previously selected menu items */
            foreach (var item in _mnuSyntax.DropDownItems.Cast<ToolStripItem>().Where(item => item.GetType() == typeof(ToolStripMenuItem)))
            {
                ((ToolStripMenuItem)item).Checked = false;
            }
            LanguageControl.Syntax(sender, _tsFiles);
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

        private static void OnTabStripSelectionChanged(TabStripItemChangedEventArgs e)
        {
            var doc = (FastColoredTextBox) e.Item.Controls[0];
            doc.Focus();
        }

        private void OnTabStripItemClosing(TabStripItemClosingEventArgs e)
        {
            var doc = (FastColoredTextBox) e.Item.Controls[0];
            if (doc.IsChanged)
            {
                switch (MessageBox.Show(string.Format("Do you want save \"{0}\"?", e.Item.Title), @"Save File Before Closing", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information))
                {
                    case DialogResult.Yes:
                        if (!_doc.SaveFile(e.Item, doc))
                        {
                            e.Cancel = true;
                        }
                        break;

                    case DialogResult.Cancel:
                        e.Cancel = true;
                        break;
                }
            }
            doc.TextChangedDelayed -= OnTextBoxTextChanged;
            _doc.DestroyTab(doc);
        }

        private void OnTextBoxTextChanged(object sender, EventArgs e)
        {
            /* We use this event to update the status bar */
            System.Diagnostics.Debug.Print("Firing text change: " + DateTime.Now);
        }
    }
}
