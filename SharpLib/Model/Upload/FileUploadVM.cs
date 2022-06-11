using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpLib.Model.Upload
{
    public class FileUploadVM
    {
        /// <summary>
        /// The files where are selected from view
        /// </summary>
        public IFormFileCollection UploadedFiles { get; set; }

        /// <summary>
        /// The custom message which no files are selected on view
        /// </summary>
        public string NoFileSelectedMessage { get; set; }

        /// <summary>
        /// the custom message which on exception throw
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// .xls, .xlsx, .pdf
        /// </summary>
        public List<string> AllowedExtensionList { get; set; }

        /// <summary>
        /// Max filse size in bytes
        /// </summary>
        public long MaxFileSize { get; set; }


        private string _FileSizeOverflowMesaage;

        /// <summary>
        /// the custom message when uploaded file content length is bigger then MaxContentLength
        /// </summary>
        public string FileSizeOverflowMesaage
        {
            get { return $"{_FileSizeOverflowMesaage} (max: {MaxFileSize})"; }
            set
            {
                _FileSizeOverflowMesaage = value;
            }
        }

        /// <summary>
        /// the custom message when uploded file extension is exsits in AllowedExtensionList
        /// </summary>
        public string InvalidExtensionMessage { get; set; }

        /// <summary>
        /// if this property is not null or empty, the prefix will add before the name of original file name. You can set there time, guid etc.
        /// </summary>
        public string FileNamePrefix { get; set; }

        /// <summary>
        /// path where the posted file will be stored
        /// </summary>
        public string DestinationPath { get; set; }

        private int _MaxFileCount { get; set; }

        /// <summary>
        /// the count of allowed file count on single upload operation. Default 1 file.
        /// </summary>
        public int MaxFileCount
        {
            get { return _MaxFileCount == 0 ? 1 : _MaxFileCount; }
            set { _MaxFileCount = value; }
        }

        /// <summary>
        /// the custom message when uploaded file count is bigger then MaxFileCount 
        /// </summary>
        public string MaxFileCountOverflowMessage { get; set; }
    }
}
