using SharpLib.Model.Common;
using SharpLib.Model.Excell;
using SharpLib.Model.Upload;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpLib.Concrete
{
    public class ExcellHelperManager : CommonFileUploadManager
    {
        /// <summary>
        /// Converts posted file to Generic Type Dot Net Class List. Its converts only one file in one post.
        /// <para>If convertion is not sucessfully complete it tries delete saved file even if can be saved</para>
        /// </summary>
        /// <typeparam name="T">ToDotNetClassResultVM which stores the result and messages for the operation</typeparam>
        /// <param name="init"></param>
        /// <returns>
        /// <para></para>
        /// Tehere is a few scenario :
        /// <para>1-)</para>
        /// </returns>
        public ResponseModel<ToDotNetClassResultVM<T>> ToDotNetClass<T>(ToDotNetClassInitVM init)
        {
            var validAndSaveResult = ControlValidationSaveAndGetSavedFile(init);

            if (!validAndSaveResult.ProcessStatus)
                return new ResponseModel<ToDotNetClassResultVM<T>> { ProcessStatus = false, Message = validAndSaveResult.Message };

            var tempFile = validAndSaveResult.Result;

            //... buradan devam edielcek, aşağıdaki metottda genric olarak excelli okuyup class liste çevir...

            var convertResult = ConvertExcellToGenericTypeList<T>(tempFile, init);

            if (!convertResult.ProcessStatus)
                return ReturnIfNotConvert<T>(init, tempFile, convertResult);


            var tempResult = new ToDotNetClassResultVM<T>
            {
                ResultList = convertResult.Result,
                MessageList = new List<string>()
            };

            if (init.DeleteAfterSave)
            {
                var deleteResult = DeleteFile(tempFile.DirectoryPath, init.DeleteAfterSaveErrorMessage);

                if (!deleteResult.ProcessStatus)
                    tempResult.MessageList.Add(deleteResult.Message);
            }

            return new ResponseModel<ToDotNetClassResultVM<T>> { ProcessStatus = true, Result = tempResult };
        }

        private ResponseModel<ToDotNetClassResultVM<T>> ReturnIfNotConvert<T>(ToDotNetClassInitVM init, SavedFileVM tempFile, ResponseModel<List<T>> convertResult)
        {
            var tempMessage = $"{init.CanNotConvertMessage}.{Environment.NewLine}{Environment.NewLine}({convertResult.Message})";

            var deleteResult = DeleteFile(tempFile.DirectoryPath, init.DeleteAfterSaveErrorMessage);

            if (!deleteResult.ProcessStatus)
                tempMessage = $"{tempMessage}{Environment.NewLine} {Environment.NewLine} {deleteResult.Message}";

            return new ResponseModel<ToDotNetClassResultVM<T>>
            {
                ProcessStatus = false,
                Message = tempMessage
            };
        }

        /// <summary>
        /// Controls validations , tries saving than returns the saved file with SavedFileVM type
        /// </summary>
        /// <param name="init"></param>
        /// <returns></returns>
        private ResponseModel<SavedFileVM> ControlValidationSaveAndGetSavedFile(ToDotNetClassInitVM init)
        {
            var validCheck = ValidateFiles(init.UploadInit);

            if (!validCheck.ProcessStatus)
                return new ResponseModel<SavedFileVM> { ProcessStatus = false, Message = validCheck.Message };

            var savedResult = SaveFiles(validCheck.Result.ValidFileList, init.UploadInit);

            //if one file is validate and there was not sucseffully file save, means that : the posted file did not save correctly
            if (!savedResult.SavedFileList.Any())
            {
                var tempMessage = init.FileDidNotSaveMessage;

                if (validCheck.Result.NotValidFileList.Any())
                    tempMessage = $"{tempMessage}{Environment.NewLine}{string.Join($"{Environment.NewLine}*", validCheck.Result.NotValidFileList.Select(x => $"File Name : {x.OriginalFileName}, Message : {x.ValidationMessage}"))}";

                return new ResponseModel<SavedFileVM> { ProcessStatus = false, Message = tempMessage };
            }

            return new ResponseModel<SavedFileVM> { ProcessStatus = true, Result = savedResult.SavedFileList.Single() };
        }

        private ResponseModel<List<T>> ConvertExcellToGenericTypeList<T>(SavedFileVM excellFile, ToDotNetClassInitVM init)
        {
            try
            {
                var tempList = new List<T>();

                return new ResponseModel<List<T>> { ProcessStatus = true, Result = tempList };
            }
            catch (Exception e)
            {
                return new ResponseModel<List<T>> { ProcessStatus = false, Message = $"Exception Message :{e.Message}" };
            }
        }
    }
}
