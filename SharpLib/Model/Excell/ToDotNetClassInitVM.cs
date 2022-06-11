using SharpLib.Model.Upload;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpLib.Model.Excell
{
    public class ToDotNetClassInitVM
    {
        public FileUploadVM UploadInit { get; set; }


        private string _FileDidNotSaveMessage { get; set; }
        /// <summary>
        /// The custom message when Posted file was not save sucessfully
        /// </summary>
        public string FileDidNotSaveMessage
        {
            get
            {
                return string.IsNullOrWhiteSpace(_FileDidNotSaveMessage) ? "File could not be saved!" : _FileDidNotSaveMessage;
            }
            set
            {
                _FileDidNotSaveMessage = value;
            }
        }

        /// <summary>
        /// if true, the saved filse will be delete after save
        /// </summary>
        public bool DeleteAfterSave { get; set; }

        /// <summary>
        /// when after save delete opration not correctly complete
        /// </summary>
        public string DeleteAfterSaveErrorMessage { get; set; }


        private string _CanNotConvertMessage { get; set; }
        public string CanNotConvertMessage
        {
            get
            {

                return string.IsNullOrWhiteSpace(_CanNotConvertMessage) ? "File did not select!" : _CanNotConvertMessage;
            }

            set
            {
                _CanNotConvertMessage = value;
            }
        }
    }
}
