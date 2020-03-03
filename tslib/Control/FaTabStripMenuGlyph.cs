using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace tslib.Control
{
    internal class FaTabStripMenuGlyph
    {
        private readonly ToolStripProfessionalRenderer _renderer;

        public bool IsMouseOver { get; set; }

        public Rectangle Bounds { get; set; }

        internal FaTabStripMenuGlyph(ToolStripProfessionalRenderer renderer)
        {
            Bounds = Rectangle.Empty;
            _renderer = renderer;
        }

        public void DrawGlyph(Graphics g)
        {
            if (IsMouseOver)
            {
                var fill = _renderer.ColorTable.ButtonSelectedHighlight; //Color.FromArgb(35, SystemColors.Highlight);
                using (var b = new SolidBrush(fill))
                {
                    g.FillRectangle(b, Bounds);
                }
                var borderRect = Bounds;
                borderRect.Width--;
                borderRect.Height--;
                g.DrawRectangle(SystemPens.Highlight, borderRect);
            }
            var bak = g.SmoothingMode;
            g.SmoothingMode = SmoothingMode.Default;
            using (var pen = new Pen(Color.Black))
            {
                pen.Width = 2;
                g.DrawLine(pen, new Point(Bounds.Left + (Bounds.Width/3) - 2, Bounds.Height/2 - 1),
                           new Point(Bounds.Right - (Bounds.Width/3), Bounds.Height/2 - 1));
            }
            g.FillPolygon(Brushes.Black, new[]
                                             {
                                                 new Point(Bounds.Left + (Bounds.Width/3) - 2, Bounds.Height/2 + 2),
                                                 new Point(Bounds.Right - (Bounds.Width/3), Bounds.Height/2 + 2),
                                                 new Point(Bounds.Left + Bounds.Width/2 - 1, Bounds.Bottom - 4)
                                             });
            g.SmoothingMode = bak;
        }
    }
}
