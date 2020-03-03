/* CodePad
 * Written by Jason James Newland
 * Copyright (C) 2020
 * Provided AS-IS with no warranty expressed or implied
 */
using System;
using System.Collections.Generic;
using CodePad.Classes.Theme.ColorTables;
using CodePad.Classes.Theme.Presets;

namespace CodePad.Classes.Theme
{
    public static class ThemeManager
    {
        /* A class that doesn't really do much except keeping the renderer and the preset list in one place */
        public static BaseRenderer Renderer;

        public static List<PresetColorTable> Presets;

        public static event Action<int> ThemeChanged; 

        static ThemeManager()
        {
            Renderer = new BaseRenderer {RoundedEdges = true};

            Presets = new List<PresetColorTable>
                          {
                              new SystemPreset(),
                              new DarkPreset(),
                              new OfficeClassicPreset()
                          };            
        }

        public static void SetTheme(int index)
        {
            Renderer.ColorTable.InitFrom(Presets[index], true);
            Renderer.RefreshToolStrips();

            if (ThemeChanged != null)
            {
                ThemeChanged(index);
            }
        }        
    }
}
