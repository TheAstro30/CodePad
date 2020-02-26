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
using System;
using fctblib.Helpers.Hints;

namespace fctblib.TextBoxEventArgs
{
    public class HintClickEventArgs : EventArgs
    {
        public Hint Hint { get; private set; }

        public HintClickEventArgs(Hint hint)
        {
            Hint = hint;
        }        
    }
}
