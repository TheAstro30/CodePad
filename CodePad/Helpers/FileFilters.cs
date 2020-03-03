/* CodePad
 * Written by Jason James Newland
 * Copyright (C) 2020
 * Provided AS-IS with no warranty expressed or implied
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace CodePad.Helpers
{
    public static class FileFilters
    {
        /* Simple class to "build" the filter list required by open/save file dialogs */
        public class Filter
        {
            public string Extension { get; set; }
            public string Description { get; set; }
        }

        private static readonly List<Filter> Associations = new List<Filter>
                                                                {
                                                                    new Filter
                                                                        {
                                                                            Extension = "*.txt",
                                                                            Description = "Text file (*.txt)"
                                                                        },
                                                                    new Filter
                                                                        {
                                                                            Extension = "*.rtf",
                                                                            Description = "Rich Text Format (*.rtf)"
                                                                        },
                                                                    new Filter
                                                                        {
                                                                            Extension = "*.htm;*.html",
                                                                            Description = "HTML file (*.htm,*.html)"
                                                                        },
                                                                    new Filter
                                                                        {
                                                                            Extension = "*.css",
                                                                            Description =
                                                                                "Cascading Style Sheet (*.css)"
                                                                        },
                                                                    new Filter
                                                                        {
                                                                            Extension = "*.cs",
                                                                            Description = "CSharp file (*.cs)"
                                                                        },
                                                                    new Filter
                                                                        {
                                                                            Extension = "*.vb",
                                                                            Description = "Visual Basic file (*.vb)"
                                                                        },
                                                                    new Filter
                                                                        {
                                                                            Extension = "*.php",
                                                                            Description = "PHP file (*.php)"
                                                                        }
                                                                };

        public static string GetFilters()
        {
            var all = new StringBuilder("All files|");
            var sep = new StringBuilder();
            foreach (var a in Associations)
            {
                all.Append(string.Format("{0};", a.Extension));
                sep.Append(string.Format("|{0}|{1}", a.Description, a.Extension));
            }
            return string.Format("{0}{1}", all, sep);
        }

        public static int GetExtensionIndex(string extension)
        {
            var count = 2;
            foreach (var e in Associations)
            {
                if (e.Extension.Equals(extension, StringComparison.InvariantCultureIgnoreCase))
                {
                    return count;
                }
                count++;
            }
            return 1;
        }
    }
}
