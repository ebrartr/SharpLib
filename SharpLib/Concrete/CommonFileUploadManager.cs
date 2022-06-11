using Microsoft.AspNetCore.Http;
using SharpLib.Model.Common;
using SharpLib.Model.Upload;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SharpLib.Extensions.DataValidation;

namespace SharpLib.Concrete
{
    public class CommonFileUploadManager
    { 
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

            if (!(tempResult.ValidFileList.Count() == init.UploadedFiles.Count() && !tempResult.NotValidFileList.Any()))
                return new ResponseModel<FileValidateResultVM> { ProcessStatus = false, Message = $"{init.SomeFilesNotValidMessage} {string.Join(", ", tempResult.NotValidFileList.Select(x => $"(File Name:{ x.OriginalFileName }, Message : { x.ValidationMessage})"))}",Result=tempResult };

            return new ResponseModel<FileValidateResultVM> { ProcessStatus = true,Result=tempResult }; ;
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
            if (postedFile == null)
                return new ResponseModel<ValidFileVM> { ProcessStatus = false, Message = init.NoFileSelectedMessage };

            if (postedFile.Length == 0)
                return new ResponseModel<ValidFileVM> { ProcessStatus = false, Message = init.EmtyContentMessage };

            if (postedFile.Length > init.MaxFileSize)
                return new ResponseModel<ValidFileVM> { ProcessStatus = false, Message = init.FileSizeOverflowMesaage };

            init.AllowedExtensionList = init.AllowedExtensionList.IfNullSetEmpty();

            if (init.AllowedExtensionList.Any())
            {
                // if allowed extension set

                var fileExtension = Path.GetExtension(postedFile.FileName).ToLower();

                if (!init.AllowedExtensionList.Any(x => x == fileExtension))
                    return new ResponseModel<ValidFileVM> { ProcessStatus = false, Message = init.InvalidExtensionMessage };
            }

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

            var result = new FileSaveResultVM { SavedFileList = new List<ServerFileVM>(), NotSavedFileList = new List<NotSavedFileVM>() };

            if (string.IsNullOrWhiteSpace(init.DestinationPath))
            {
                var tempList = validFileList.Select(x => new NotSavedFileVM { FileOriginalName = x.FileName, Message = init.DestinationNotSetMessage });

                return result;
            }

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
        private static ResponseModel<ServerFileVM> SaveFile(ValidFileVM validFile, string destinationPath)
        {
            try
            {
                if (validFile.FileBytes == null)
                    return new ResponseModel<ServerFileVM> { ProcessStatus = false, Message = "No Bytes found" };

                var fullPath = $"{destinationPath}\\{validFile.FileName}";

                Directory.CreateDirectory(destinationPath);
                File.WriteAllBytes(fullPath, validFile.FileBytes);

                var savedFile = new ServerFileVM { DirectoryPath = fullPath, FileName = validFile.FileName, FileSize = validFile.FileSize };

                return new ResponseModel<ServerFileVM> { ProcessStatus = true, Result = savedFile, Message = "File is sucessfully saved" };
            }
            catch (Exception e)
            {
                return new ResponseModel<ServerFileVM> { ProcessStatus = false, Message = $"Message : {e.Message}" };
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
