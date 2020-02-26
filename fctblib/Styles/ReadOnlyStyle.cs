//
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
using fctblib.Helpers.TextRange;

namespace fctblib.Styles
{
    public sealed class ReadOnlyStyle : Style
    {
        public ReadOnlyStyle()
        {
            IsExportable = false;
        }

        public override void Draw(Graphics gr, Point position, Range range)
        {
            /* Empty */
        }
    }
}
