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

            public Filter()
            {
                /* Default empty constructor */
            }

            public Filter(string ext, string desc)
            {
                Extension = ext;
                Description = desc;
            }
        }

        private static readonly List<Filter> Associations = new List<Filter>
                                                                {
                                                                    new Filter("*.txt", "Text file"),
                                                                    new Filter("*.rtf", "Rich Text Format file (*.rtf)"),
                                                                    new Filter("*.xml", "XML file (*.xml)"),
                                                                    new Filter("*.htm", "Hyper Text Mark-Up file (*.htm)"),
                                                                    new Filter("*.html", "Hyper Text Mark-Up Language file (*.html)"),
                                                                    new Filter("*.css", "Cascading Style Sheet file (*.css"),
                                                                    new Filter("*.php", "PHP file (*.php)"),
                                                                    new Filter("*.sql", "SQL file (*.sql)"),
                                                                    new Filter("*.js", "Java Script file (*.js)"),
                                                                    new Filter("*.cs", "CSharp file (*.cs)"),
                                                                    new Filter("*.vb", "Visual Basic file (*.vb)"),
                                                                    new Filter("*.vba", "Visual Basic Script file"),
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
