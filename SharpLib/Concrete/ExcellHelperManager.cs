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
       
        private ResponseModel<ToDotNetClassResultVM<T>> ReturnIfNotConvert<T>(ToDotNetClassInitVM init, ServerFileVM tempFile, ResponseModel<List<T>> convertResult)
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

        private ResponseModel<List<T>> ConvertExcellToGenericTypeList<T>(ServerFileVM excellFile, ToDotNetClassInitVM init)
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
