using System;
using System.Collections.Generic;
using System.Text;

namespace SharpLib.Model.Upload
{
    /// <summary>
    /// The file which is store in application server folders. It can be a pre uploaded file or etc.
    /// </summary>
    public class ServerFileVM : ValidFileVM
    {
        /// <summary>
        /// File directory with new path include file name
        /// </summary>
        public string DirectoryPath { get; set; }
    }
}
