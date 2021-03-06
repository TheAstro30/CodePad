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
using System;
using System.Collections.Generic;
using fctblib.Helpers.Lines;

namespace fctblib.TextBoxEventArgs
{
    public class WordWrapNeededEventArgs : EventArgs
    {
        public List<int> CutOffPositions { get; private set; }
        public bool ImeAllowed { get; private set; }
        public Line Line { get; private set; }

        public WordWrapNeededEventArgs(List<int> cutOffPositions, bool imeAllowed, Line line)
        {
            CutOffPositions = cutOffPositions;
            ImeAllowed = imeAllowed;
            Line = line;
        }
    }
}
