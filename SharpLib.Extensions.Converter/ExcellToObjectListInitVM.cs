using SharpLib.Model.Upload;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpLib.Extensions.Converter
{
    public class ExcellToObjectListInitVM
    {
        public bool DeleteAfterSave { get; set; }

        private string _DeleteErrorMessage { get; set; }

        public string DeleteErrorMessage
        {
            get
            {
                return string.IsNullOrWhiteSpace(_DeleteErrorMessage) ? "An unexpected error occurred while deleting the file" : _DeleteErrorMessage;
            }
            set { _DeleteErrorMessage = value; }
        }

        public FileUploadVM FileUploadOpitons { get; set; }
    }
}
