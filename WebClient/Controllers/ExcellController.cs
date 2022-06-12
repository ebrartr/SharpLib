using Microsoft.AspNetCore.Mvc;
using SharpLib.Concrete;
using SharpLib.Extensions.Converter;
using SharpLib.Extensions.File;
using SharpLib.Model.Common;
using SharpLib.Model.Excell;
using SharpLib.Model.Template;
using SharpLib.Model.Upload;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebClient.Controllers
{
    public class ExcellController : Controller
    {
        private static ExcellHelperManager _excellHelper;

        public ExcellController()
        {
            _excellHelper = new ExcellHelperManager();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ConvertExcellToVMList()
        {
            var options = new FileUploadVM
            {
                FileNamePrefix = string.Format("{0:yyyy.MM.dd_HH.mm.ss}", DateTime.Now),
                DestinationPath = @"D:\TempUpload",
                MaxFileCount=1,
                AllowedExtensionList = new List<string> { ".xls",".xlsx"}
            };

            var saveResult = Request.Form.Files.Save(options);

            if (!saveResult.ProcessStatus)
                return View(new ResponseModel<List<TestModel1VM>> { ProcessStatus = false, Message = saveResult.Message });

            var tempList = saveResult.Result.Single().ToObjectList<TestModel1VM>();

            return View(new ResponseModel<List<TestModel1VM>> { ProcessStatus = true,Result = tempList});
        }
    }
}
