using Microsoft.AspNetCore.Http;
using SharpLib.Concrete;
using SharpLib.Model.Common;
using SharpLib.Model.Upload;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpLib.Extensions.File
{
    public static class FileUploadExtensions
    {
        /// <summary>
        /// Controls validations , tries saving than returns the saved file with SavedFileVM type.
        /// </summary>
        /// <param name="fomrCollection"></param>
        /// <param name="fileUploadInit"></param>
        /// <returns></returns>
        public static ResponseModel<List<ServerFileVM>> Save(this IFormFileCollection formCollection, FileUploadVM options = null)
        {
            if (options == null)
                options = new FileUploadVM();

            options.UploadedFiles = formCollection;

            var validCheck = CommonFileUploadManager.ValidateFiles(options);

            if (!validCheck.ProcessStatus)
                return new ResponseModel<List<ServerFileVM>> { ProcessStatus = false, Message = validCheck.Message };

            var savedResult = CommonFileUploadManager.SaveFiles(validCheck.Result.ValidFileList, options);

            if (savedResult.SavedFileList.Any())
                return new ResponseModel<List<ServerFileVM>> { ProcessStatus = true, Result = savedResult.SavedFileList };

            return new ResponseModel<List<ServerFileVM>> { ProcessStatus = false, Message = options.FileDidNotSaveMessage };
        }
    }
}
