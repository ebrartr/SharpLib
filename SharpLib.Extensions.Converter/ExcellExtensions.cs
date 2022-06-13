using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using SharpLib.Concrete;
using SharpLib.Extensions.File;
using SharpLib.Extensions.Reflection;
using SharpLib.Model.Upload;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SharpLib.Extensions.Converter
{
    public static class ExcellExtensions
    {
        /// <summary>
        /// Converts excell file to list of T generic Type. It matches columns with property names or DisplayName attribute of property.
        /// <para>Warning ! Only string or int properties converts with real type. If property is not string or int, it converts it to string by default.</para>
        /// <para>You can convert this default value to datetime or etc. in your custom codes.</para>
        /// <para>The conversion to string process controls CustomStringFormat atttribute for string property and converts excell value to string with this attribute info.</para>
        /// <para>CustomStringFormat attribute ise not set, Excell value canverts with default .ToString() method</para>
        /// <para>Note that : if you did not use custom string format, result of this method may look different from excel. For example : if excell value (1.1.1990) is datetime but you didn't use custom format may be look 1990.1.1 00.00.00 ...</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="file"></param>
        /// <returns></returns>
        private static List<T> ToObjectList<T>(this ServerFileVM file)
        {
            var objList = new List<T>();
            T obj = default(T);

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = System.IO.File.Open(file.DirectoryPath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var columnInfoList = new List<ColumnInfo>();

                    while (reader.Read()) //Each row of the file
                    {
                        for (int cellIndex = 0; cellIndex < reader.FieldCount; cellIndex++)// all cells in row
                        {
                            columnInfoList.Add(new ColumnInfo { Index = cellIndex, Title = (string)reader.GetValue(cellIndex) });
                        }

                        break;
                    }

                    if (!columnInfoList.Any(x => !string.IsNullOrWhiteSpace(x.Title)))
                        throw new Exception("No columns found!");

                    while (reader.Read())
                    {

                        obj = Activator.CreateInstance<T>();

                        foreach (PropertyInfo prop in obj.GetType().GetProperties())
                        {
                            var objPropInExcellColumn = columnInfoList.Where(x => x.Title == prop.Name).SingleOrDefault();// 1. find with prop name

                            if (objPropInExcellColumn == null)
                            {//find column with diplayname attr of object property

                                var displayName = prop.DisplayName();

                                if (!string.IsNullOrWhiteSpace(displayName))
                                    objPropInExcellColumn = columnInfoList.Where(x => x.Title == displayName).SingleOrDefault();
                            }

                            if (objPropInExcellColumn != null)// if there ise matched column with porpert name or disoplayname attribute, it is corrrect column to read
                            {
                                var excellValue = reader.GetValue(objPropInExcellColumn.Index);

                                if (prop.PropertyType == typeof(string))
                                {
                                    ConvertToString<T>(excellValue, prop, obj, objPropInExcellColumn, reader);
                                }
                                else if (prop.PropertyType == typeof(int))
                                {
                                    var dotNetValue = Convert.ToInt32(excellValue);

                                    prop.SetValue(obj, dotNetValue, null);
                                }
                                else if (prop.PropertyType == typeof(int?))
                                {
                                    if (excellValue != null)
                                    {
                                        var dotnetValue = Convert.ToInt32(excellValue);

                                        prop.SetValue(obj, dotnetValue, null);
                                    }
                                }
                                else
                                {
                                    if (excellValue != null)
                                        prop.SetValue(obj, excellValue.ToString(), null);
                                }
                            }
                        }

                        objList.Add(obj);

                    }
                }
            }

            return objList;
        }

        /// <summary>
        /// Allow only excell files and max 1 file can be upload. (your choice of file type and maximum number of files will be ignored)
        /// <para>Converts excell file to list of T generic Type. It matches columns with property names or DisplayName attribute of property.</para>
        /// <para>Warning ! Only string or int properties converts to real type. If property is not string or int, it converts it to string by default.</para>
        /// <para>The conversion to string process controls CustomStringFormat atttribute for string property and converts excell value to string with this attribute info.</para>
        /// <para>You can convert this default value to datetime or etc. in your custom codes.</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="formfileCollection"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static List<T> ToObjectList<T>(this IFormFileCollection formfileCollection,ExcellToObjectListInitVM options = null)
        {
            if(options == null)
            {
                options = new ExcellToObjectListInitVM { DeleteAfterSave = true};
            }

            if(options.FileUploadOpitons == null)
                options.FileUploadOpitons = new FileUploadVM();

            options.FileUploadOpitons.AllowedExtensionList = new List<string> { ".xls", ".xlsx" };
            options.FileUploadOpitons.MaxFileCount = 1;

            var saveResult = formfileCollection.Save(options.FileUploadOpitons);

            if (!saveResult.ProcessStatus)
                throw new Exception(saveResult.Message);

            var savedFile = saveResult.Result.Single();

            var tempList = savedFile.ToObjectList<T>();

            if(options.DeleteAfterSave)
                savedFile.Delete();

            return tempList;
        }
        private static void ConvertToString<T>(object excellValue,PropertyInfo prop,T obj,ColumnInfo objPropInExcellColumn,IExcelDataReader reader)
        {
            if (excellValue != null)
            {

                var customFormat = prop.CustomStringFormat();

                if (customFormat == null)
                {
                    prop.SetValue(obj, excellValue.ToString(), null);
                }
                else
                {

                    //00
                    if (string.IsNullOrWhiteSpace(customFormat.Format) && customFormat.CultureInfo == null)
                        throw new Exception("Invalid data formatting");


                    if (reader.GetFieldType(objPropInExcellColumn.Index) != typeof(DateTime))
                        throw new Exception("Invalid data type on excell. There is no matched type by default.");

                    var tempDateValueınExcell = (DateTime)excellValue;
                    string dotNetValue = null;

                    //01
                    if (string.IsNullOrWhiteSpace(customFormat.Format) && customFormat.CultureInfo != null)
                        dotNetValue = tempDateValueınExcell.ToString(customFormat.CultureInfo);


                    //10
                    if (!string.IsNullOrWhiteSpace(customFormat.Format) && customFormat.CultureInfo == null)
                        dotNetValue = tempDateValueınExcell.ToString(customFormat.Format);

                    //11
                    if (!string.IsNullOrWhiteSpace(customFormat.Format) && customFormat.CultureInfo != null)
                        dotNetValue = tempDateValueınExcell.ToString(customFormat.Format, customFormat.CultureInfo);

                    prop.SetValue(obj, dotNetValue, null);

                }
            }
        }
    }
}
