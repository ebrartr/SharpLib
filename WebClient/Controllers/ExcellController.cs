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
        private static ExcellHelper _excellHelper;

        public ExcellController()
        {
            _excellHelper = new ExcellHelper();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ConvertExcellToVMList()
        {
            var init = new ToDotNetClassInitVM
            {
                FileDidNotSaveMessage = "File could not be saved!",
                UploadInit = new FileUploadVM
                {
                    AllowedExtensionList = new List<string> { ".xls", ".xlsx" },
                    MaxFileSize = 1024 * 1024 * 1024,// 1 GB
                    FileSizeOverflowMesaage = "The file size is larger than the specified size!",
                    ErrorMessage = "An unexpected error has occurred!",
                    InvalidExtensionMessage = "Invalid file extension!",
                    NoFileSelectedMessage = "No file found!",
                    FileNamePrefix = string.Format("{0:yyyy.MM.dd_HH.mm.ss}", DateTime.Now),
                    DestinationPath = @"D:\TempUpload",
                    UploadedFiles = Request.Form.Files
                }
            };

            var result = _excellHelper.ToDotNetClass<TestModel1VM>(init);

            return View(result);
        }
    }
}
