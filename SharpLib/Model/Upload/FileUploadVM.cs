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

        private string _NoFileSelectedMessage { get; set; }

        /// <summary>
        /// The custom message which no files are selected on view
        /// </summary>
        public string NoFileSelectedMessage
        {
            get { return string.IsNullOrWhiteSpace(_NoFileSelectedMessage) ? "No file selected" : _NoFileSelectedMessage; }
            set { _NoFileSelectedMessage = value; }
        }

        private string _ErrorMessage { get; set; }

        /// <summary>
        /// the custom message which on exception throw
        /// </summary>
        public string ErrorMessage
        {
            get { return string.IsNullOrWhiteSpace(_ErrorMessage) ? "An unexpected error has occurred!" : _ErrorMessage; }
            set
            {
                _ErrorMessage = value;
            }
        }

        /// <summary>
        /// .xls, .xlsx, .pdf, if not set allow all types
        /// </summary>
        public List<string> AllowedExtensionList { get; set; }

        private long _MaxFileSize { get; set; }

        /// <summary>
        /// Max filse size in bytes. Defoult is 1GB
        /// </summary>
        public long MaxFileSize
        {
            get { return _MaxFileSize == 0 ? 1024 * 1024 * 1024 /*1GB*/: _MaxFileSize; }
            set { _MaxFileSize = value; }
        }

        private string _FileSizeOverflowMesaage;

        /// <summary>
        /// the custom message when uploaded file content length is bigger then MaxContentLength
        /// </summary>
        public string FileSizeOverflowMesaage
        {
            get { return $"{(string.IsNullOrWhiteSpace(_FileSizeOverflowMesaage) ? "The file size is larger than the specified size!" : _FileSizeOverflowMesaage)} (max: {MaxFileSize})"; }
            set
            {
                _FileSizeOverflowMesaage = value;
            }
        }

        private string _InvalidExtensionMessage { get; set; }

        /// <summary>
        /// the custom message when uploded file extension is exsits in AllowedExtensionList
        /// </summary>
        public string InvalidExtensionMessage
        {
            get { return string.IsNullOrWhiteSpace(_InvalidExtensionMessage) ? "Invalid file extension!" : _InvalidExtensionMessage; }
            set { _InvalidExtensionMessage = value; }
        }

        /// <summary>
        /// if this property is not null or empty, the prefix will add before the name of original file name. You can set there time, guid etc.
        /// </summary>
        public string FileNamePrefix { get; set; }

        private string _DestinationPath { get; set; }

        /// <summary>
        /// path where the posted file will be store,if not set , files will save under C:\SharpLib\UploadedFiles by default
        /// </summary>
        public string DestinationPath
        {
            get { return string.IsNullOrWhiteSpace(_DestinationPath) ? @"C:\SharpLib\UploadedFiles" : _DestinationPath; }
            set { _DestinationPath = value; }
        }

        private int _MaxFileCount { get; set; }

        /// <summary>
        /// the count of allowed file count on single upload operation. Default 1 file.
        /// </summary>
        public int MaxFileCount
        {
            get { return _MaxFileCount == 0 ? 1 : _MaxFileCount; }
            set { _MaxFileCount = value; }
        }

        private string _MaxFileCountOverflowMessage { get; set; }

        /// <summary>
        /// the custom message when uploaded file count is bigger then MaxFileCount 
        /// </summary>
        public string MaxFileCountOverflowMessage
        {
            get { return string.IsNullOrWhiteSpace(_MaxFileCountOverflowMessage) ? $"Up to {MaxFileCount} files can be selected !" : _MaxFileCountOverflowMessage; }
            set { _MaxFileCountOverflowMessage = value; }
        }

        private string _FileDidNotSaveMessage { get; set; }
        /// <summary>
        /// The custom message when Posted file was not save sucessfully
        /// </summary>
        public string FileDidNotSaveMessage
        {
            get
            {
                return string.IsNullOrWhiteSpace(_FileDidNotSaveMessage) ? "Files could not be saved!" : _FileDidNotSaveMessage;
            }
            set
            {
                _FileDidNotSaveMessage = value;
            }
        }

        private string _EmtyContentMessage { get; set; }
        public string EmtyContentMessage
        {
            get { return string.IsNullOrWhiteSpace(_EmtyContentMessage) ? "File content is emty!" : _EmtyContentMessage; }
            set { _EmtyContentMessage = value; }
        }

        private string _SomeFilesNotValidMessage { get; set; }

        public string SomeFilesNotValidMessage
        {
            get { return string.IsNullOrWhiteSpace(_SomeFilesNotValidMessage) ? "Some files are not valid!" : _SomeFilesNotValidMessage; }
            set { _SomeFilesNotValidMessage = value; }
        }
    }
}
