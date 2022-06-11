using Microsoft.AspNetCore.Mvc;
using SharpLib.Concrete;
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
            var init = new ToDotNetClassInitVM
            {
                UploadInit = new FileUploadVM
                {
                    AllowedExtensionList = new List<string> { ".xls", ".xlsx" },
                    FileNamePrefix = string.Format("{0:yyyy.MM.dd_HH.mm.ss}", DateTime.Now),
                    DestinationPath = @"D:\TempUpload",
                    UploadedFiles = Request.Form.Files,
                }
            };

            var result = _excellHelper.ToDotNetClass<TestModel1VM>(init);

            return View(result);
        }
    }
}
