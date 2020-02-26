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
using System.Text.RegularExpressions;

namespace fctblib.Highlight.Descriptor
{
    public class FoldingDesc
    {
        public string StartMarkerRegex { get; set; }
        public string FinishMarkerRegex { get; set; }

        public RegexOptions Options = RegexOptions.None;
    }
}
