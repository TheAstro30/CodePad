/* CodePad
 * Written by Jason James Newland
 * Copyright (C) 2020
 * Provided AS-IS with no warranty expressed or implied
 */
using System.Reflection;
using CodePad.Controls;

namespace CodePad.Forms
{
    public partial class FrmAbout : FormEx
    {
        public FrmAbout()
        {
            InitializeComponent();
            pnlIcon.BackgroundImage = Properties.Resources.about.ToBitmap();

            var version = Assembly.GetExecutingAssembly().GetName().Version;
            lblVersion.Text = string.Format("Version: {0}.{1}.{2} (build: {3})", version.Major, version.Minor,
                                 version.MinorRevision, version.Build);
        }
    }
}
