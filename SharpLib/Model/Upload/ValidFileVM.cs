using System;
using System.Collections.Generic;
using System.Text;

namespace SharpLib.Model.Upload
{
    public class ValidFileVM
    {
        public string FileName { get; set; }

        public byte[] FileBytes { get; set; }

        /// <summary>
        /// File size in bytes
        /// </summary>
        public long FileSize { get; set; }
    }
}
