using System.Drawing;
using System.Windows.Forms;

namespace tslib.Control
{
    internal class FaTabStripCloseButton
    {
        private readonly ToolStripProfessionalRenderer _renderer;

        public bool IsMouseOver { get; set; }

        public Rectangle Bounds { get; set; }

        internal FaTabStripCloseButton(ToolStripProfessionalRenderer renderer)
        {
            Bounds = Rectangle.Empty;
            _renderer = renderer;
        }

        public void DrawCross(Graphics g)
        {
            if (IsMouseOver)
            {
                var fill = _renderer.ColorTable.ButtonSelectedHighlight;
                using (var b = new SolidBrush(fill))
                {
                    g.FillRectangle(b, Bounds);
                }
                var borderRect = Bounds;
                borderRect.Width--;
                borderRect.Height--;
                g.DrawRectangle(SystemPens.Highlight, borderRect);
            }
            using (var pen = new Pen(Color.Black, 1.6f))
            {
                g.DrawLine(pen, Bounds.Left + 3, Bounds.Top + 3, Bounds.Right - 5, Bounds.Bottom - 4);
                g.DrawLine(pen, Bounds.Right - 5, Bounds.Top + 3, Bounds.Left + 3, Bounds.Bottom - 4);
            }
        }
    }
}
