using System;
using System.Collections.Generic;
using System.Text;

namespace SharpLib.Model.Upload
{
    public class FileValidateResultVM
    {
        public List<ValidFileVM> ValidFileList { get; set; }

        public List<NotValidFileVM> NotValidFileList { get; set; }

    }
}
