using System;
using System.Collections.Generic;
using System.Text;

namespace SharpLib.Model.Upload
{
    public class SavedFileVM : ValidFileVM
    {
        /// <summary>
        /// File directory with new path include file name
        /// </summary>
        public string DirectoryPath { get; set; }
    }
}
