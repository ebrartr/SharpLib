using Microsoft.AspNetCore.Http;
using SharpLib.Model.Common;
using SharpLib.Model.Upload;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SharpLib.Concrete
{
    public class CommonFileUpload
    { 
      /// <summary>
      /// Cheks if posted files are valid and returns valid and not valid file Lists
      /// </summary>
      /// <param name="init"></param>
      /// <returns>There is a few scenario : 
      /// <para>1-) ProcessStatus will return false only these case : when posted file count is not valid</para>
      /// <para>2-) ProcessStatus will true only this case : when posted file count is valid</para>
      /// <para></para>
      /// <para>But when ProcessStatus is true and you want to check if all files are valid you must control the All Files Are Valid property in the Response (usage : Response.AllFilesAreValid) in the method result</para></returns>
        public static ResponseModel<FileValidateResultVM> ValidateFiles(FileUploadVM init)
        {
            if (init.UploadedFiles == null || init.UploadedFiles.Count() == 0)
                return new ResponseModel<FileValidateResultVM> { ProcessStatus = false, Message = init.NoFileSelectedMessage };

            if (init.UploadedFiles.Count() > init.MaxFileCount)
                return new ResponseModel<FileValidateResultVM> { ProcessStatus = false, Message = $"{init.MaxFileCountOverflowMessage} (max:{init.MaxFileCount})" };

            var tempResult = new FileValidateResultVM { ValidFileList = new List<ValidFileVM>(), NotValidFileList = new List<NotValidFileVM>() };

            foreach (IFormFile postedFile in init.UploadedFiles)
            {
                var validCheck = ValidateFile(postedFile, init);

                if (validCheck.ProcessStatus)
                {
                    if (!string.IsNullOrEmpty(init.FileNamePrefix))
                    {
                        validCheck.Result.FileName = $"{init.FileNamePrefix} {validCheck.Result.FileName}";
                    }

                    tempResult.ValidFileList.Add(validCheck.Result);
                }
                else
                {
                    tempResult.NotValidFileList.Add(new NotValidFileVM { OriginalFileName = postedFile.FileName, ValidationMessage = validCheck.Message });
                }
            }

            if (tempResult.ValidFileList.Count() == init.UploadedFiles.Count() && !tempResult.NotValidFileList.Any())// All files are valid and there is no invalid file
                tempResult.AllFilesAreValid = true;

            var result = new ResponseModel<FileValidateResultVM> { ProcessStatus = true, Result = tempResult };

            return result;
        }

        /// <summary>
        /// Controls file validation with file size, extention properties
        /// </summary>
        /// <param name="postedFile"></param>
        /// <param name="init"></param>
        /// <returns>If file is valid then returns ProcessStatus true and returns with ValidFileVM
        /// <para>If file is not valid ProcessStatus returns false with the custom message</para>
        /// </returns>
        private static ResponseModel<ValidFileVM> ValidateFile(IFormFile postedFile, FileUploadVM init)
        {
            if (postedFile == null || postedFile.Length == 0)
                return new ResponseModel<ValidFileVM> { ProcessStatus = false, Message = init.NoFileSelectedMessage };

            if (postedFile.Length > init.MaxFileSize)
                return new ResponseModel<ValidFileVM> { ProcessStatus = false, Message = init.FileSizeOverflowMesaage };

            var fileExtension = Path.GetExtension(postedFile.FileName).ToLower();

            if (init.AllowedExtensionList != null && !init.AllowedExtensionList.Contains(fileExtension))
                return new ResponseModel<ValidFileVM> { ProcessStatus = false, Message = init.InvalidExtensionMessage };

            byte[] tempBuffer = new byte[postedFile.Length];
            postedFile.OpenReadStream().Read(tempBuffer, 0, tempBuffer.Length);
            postedFile.OpenReadStream().Close();

            return new ResponseModel<ValidFileVM>
            {
                ProcessStatus = true,
                Result = new ValidFileVM
                {
                    FileName = postedFile.FileName,
                    FileBytes = tempBuffer,
                    FileSize = postedFile.Length
                }
            };
        }

        /// <summary>
        /// Save files to DestinationPath
        /// </summary>
        /// <param name="validFileList"></param>
        /// <param name="init"></param>
        /// <returns>Saved and not saved file Lists.
        /// <para>Warning : Not saved file Lists contains exceptions info</para>
        /// </returns>
        public static FileSaveResultVM SaveFiles(List<ValidFileVM> validFileList, FileUploadVM init)
        {
            var result = new FileSaveResultVM { SavedFileList = new List<SavedFileVM>(), NotSavedFileList = new List<NotSavedFileVM>() };

            foreach (var validFile in validFileList)
            {
                if (!string.IsNullOrEmpty(validFile.FileName))
                {
                    var saveResult = SaveFile(validFile, init.DestinationPath);

                    if (saveResult.ProcessStatus)
                    {
                        result.SavedFileList.Add(saveResult.Result);
                    }
                    else
                    {
                        result.NotSavedFileList.Add(new NotSavedFileVM { FileOriginalName = validFile.FileName, Message = saveResult.Message });
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Try file to save, if operation fire exception returns exception info and ProcessStatus false
        /// </summary>
        /// <param name="validFile"></param>
        /// <param name="destinationPath"></param>
        /// <returns></returns>
        private static ResponseModel<SavedFileVM> SaveFile(ValidFileVM validFile, string destinationPath)
        {
            try
            {
                if (validFile.FileBytes == null)
                    return new ResponseModel<SavedFileVM> { ProcessStatus = false, Message = "No Bytes found" };

                var fullPath = $"{destinationPath}\\{validFile.FileName}";

                Directory.CreateDirectory(destinationPath);
                File.WriteAllBytes(fullPath, validFile.FileBytes);

                var savedFile = new SavedFileVM { DirectoryPath = fullPath, FileName = validFile.FileName, FileSize = validFile.FileSize };

                return new ResponseModel<SavedFileVM> { ProcessStatus = true, Result = savedFile, Message = "File is sucessfully saved" };
            }
            catch (Exception e)
            {
                return new ResponseModel<SavedFileVM> { ProcessStatus = false, Message = $"Message : {e.Message}" };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath">directory for file includes file name</param>
        /// <returns></returns>
        public ResponseModel<string> DeleteFile(string fileDirectory, string customErrorMessage)
        {
            try
            {
                return null;
            }
            catch (Exception e)
            {
                return new ResponseModel<string> { ProcessStatus = false, Message = $"{(string.IsNullOrEmpty(customErrorMessage) ? "" : customErrorMessage)}Exception : {e.Message}" };
            }
        }
    }
}
