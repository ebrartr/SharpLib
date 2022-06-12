using ExcelDataReader;
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
        /// <para>Warning ! Only string or int properties converts to real type. If property is not string or int, it converts it to string by default.</para>
        /// <para>The conversion to string process controls CustomStringFormat atttribute for string property and converts excell value to string with this attribute info.</para>
        /// <para>You can convert this default value to datetime or etc. in your custom codes.</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="file"></param>
        /// <returns></returns>
        public static List<T> ToObjectList<T>(this ServerFileVM file)
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
        private static void ConvertToString<T>(object excellValue,PropertyInfo prop,T obj,ColumnInfo objPropInExcellColumn,IExcelDataReader reader)
        {
            if (excellValue != null)
            {

                var customFormat = prop.DataFormat();

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
