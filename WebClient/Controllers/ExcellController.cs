using Microsoft.AspNetCore.Mvc;
using SharpLib.Concrete;
using SharpLib.Extensions.File;
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
                MaxFileCount=3
            };

            var result = Request.Form.Files.Save(options);

            return View(result);
        }
    }
}
