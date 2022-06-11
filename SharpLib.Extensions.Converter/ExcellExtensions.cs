using ExcelDataReader;
using SharpLib.Model.Upload;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SharpLib.Extensions.Converter
{
    public static class ExcellExtensions
    {
        /// <summary>
        /// Converts excell file to list of T generic Type. It matches columns with property names or DisplayName attribute of property.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="file"></param>
        /// <returns></returns>
        public static List<T> ToObjectList<T>(this ServerFileVM file)
        {

            var result = new List<T>();


            // For .net core, the next line requires the NuGet package, 
            // System.Text.Encoding.CodePages
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = System.IO.File.Open(file.DirectoryPath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {

                    var cellInfoList = new List<CellInfo>();

                    bool firstRow = true;

                    while (reader.Read()) //Each row of the file
                    {

                        if (firstRow)
                        {
                            for (int cellIndex = 0; cellIndex < reader.FieldCount; cellIndex++)
                            {
                                cellInfoList.Add(new CellInfo { Index = cellIndex, Title = (string)reader.GetValue(cellIndex) });
                            }
                        }

                        firstRow = false;
                    }
                }
            }

            return null;
        }
    }
}
