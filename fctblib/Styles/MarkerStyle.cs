﻿//
//  THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
//  KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
//  IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR
//  PURPOSE.
//
//  License: GNU Lesser General Public License (LGPLv3)
//
//  Email: pavel_torgashov@ukr.net
//
//  Copyright (C) Pavel Torgashov, 2011-2016.
using System.Drawing;
using fctblib.Export;
using fctblib.Helpers.TextRange;

namespace fctblib.Styles
{
    public sealed class MarkerStyle : Style
    {
        public MarkerStyle(Brush backgroundBrush)
        {
            BackgroundBrush = backgroundBrush;
            IsExportable = true;
        }

        public Brush BackgroundBrush { get; set; }

        public override void Draw(Graphics gr, Point position, Range range)
        {
            /* Draw background */
            if (BackgroundBrush == null)
            {
                return;
            }
            var rect = new Rectangle(position.X, position.Y,
                                     (range.End.Char - range.Start.Char)*range.TextBox.CharWidth, range.TextBox.CharHeight);
            if (rect.Width == 0)
            {
                return;
            }
            gr.FillRectangle(BackgroundBrush, rect);
        }

        public override string GetCss()
        {
            var result = "";
            if (BackgroundBrush is SolidBrush)
            {
                var s = ExportToHtml.GetColorAsString((BackgroundBrush as SolidBrush).Color);
                if (s != "")
                {
                    result += "background-color:" + s + ";";
                }
            }
            return result;
        }
    }
}