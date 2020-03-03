using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace tslib.BaseClasses
{
    [ToolboxItem(false)]
    public class BaseStyledPanel : ContainerControl
    {
        private static readonly ToolStripProfessionalRenderer Renderer;

        public event EventHandler ThemeChanged;

        static BaseStyledPanel()
        {
            Renderer = new ToolStripProfessionalRenderer();
        }

        public BaseStyledPanel()
        {
            /* Set painting style for better performance. */
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.UserPaint, true);
        }

        protected override void OnSystemColorsChanged(EventArgs e)
        {
            base.OnSystemColorsChanged(e);
            UpdateRenderer();
            Invalidate();
        }

        protected virtual void OnThemeChanged(EventArgs e)
        {
            if (ThemeChanged != null)
            {
                ThemeChanged(this, e);
            }
        }

        private void UpdateRenderer()
        {
            Renderer.ColorTable.UseSystemColors = !UseThemes;
        }

        [Browsable(false)]
        public ToolStripProfessionalRenderer ToolStripRenderer
        {
            get
            {
                return Renderer;
            }
        }

        [DefaultValue(true)]
        [Browsable(false)]
        public bool UseThemes
        {
            get
            {
                return VisualStyleRenderer.IsSupported && VisualStyleInformation.IsSupportedByOS && Application.RenderWithVisualStyles;
            }
        }
    }
}
