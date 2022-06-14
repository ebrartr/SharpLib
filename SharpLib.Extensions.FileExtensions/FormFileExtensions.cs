using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SharpLib.Extensions.File
{
    public static class FormFileExtensions
    {
        /// <summary>
        /// returns only file name without file extenison
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        public static string FileName(this IFormFile formFile)
        {
            var temp = formFile.FileName;

            return temp.Replace(Path.GetExtension(temp),"");
        }
    }
}
