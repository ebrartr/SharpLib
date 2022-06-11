using System;
using System.Collections.Generic;
using System.Text;

namespace SharpLib.Model.Upload
{
    public class FileSaveResultVM
    {
        public List<ServerFileVM> SavedFileList { get; set; }

        public List<NotSavedFileVM> NotSavedFileList { get; set; }
    }
}
