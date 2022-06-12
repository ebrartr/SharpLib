using Microsoft.AspNetCore.Mvc;
using SharpLib.Extensions.Converter;
using SharpLib.Model.Common;
using SharpLib.Model.Template;
using SharpLib.Model.Upload;
using System;
using System.Collections.Generic;

namespace WebClient.Controllers
{
    public class ExcellController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ConvertExcellToVMList()
        {

            #region Usage Example 1

            try
            {

                //var options = new ExcellToObjectListInitVM
                //{
                //    DeleteAfterSave = true,
                //    FileUploadOpitons = new FileUploadVM
                //    {
                //        FileNamePrefix = string.Format("{0:yyyy.MM.dd_HH.mm.ss}", DateTime.Now),
                //        //DestinationPath = @"D:\TempUpload"
                //    }

                //};

                var result = Request.Form.Files.ToObjectList<TestModel1VM>();

                return View(new ResponseModel<List<TestModel1VM>> { ProcessStatus = true, Result = result });
            }
            catch (Exception e)
            {
                return View(new ResponseModel<List<TestModel1VM>> { ProcessStatus = false, Message = e.Message });
            }

            #endregion


            #region Usage Example 2

            //var options = new FileUploadVM
            //{
            //    FileNamePrefix = string.Format("{0:yyyy.MM.dd_HH.mm.ss}", DateTime.Now),
            //    DestinationPath = @"D:\TempUpload",
            //    MaxFileCount=1,
            //    AllowedExtensionList = new List<string> { ".xls",".xlsx"}
            //};

            //var saveResult = Request.Form.Files.Save(options);

            //if (!saveResult.ProcessStatus)
            //    return View(new ResponseModel<List<TestModel1VM>> { ProcessStatus = false, Message = saveResult.Message });

            //var tempList = saveResult.Result.Single().ToObjectList<TestModel1VM>();

            //return View(new ResponseModel<List<TestModel1VM>> { ProcessStatus = true, Result = tempList });

            #endregion

        }
    }
}
