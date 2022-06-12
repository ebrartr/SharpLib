using SharbLib.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SharpLib.Extensions.Reflection
{
    public static class PropertyInfoExtensions
    {
        /// <summary>
        /// Returns porperty's DiplayName attr value, if no attr found returns null
        /// </summary>
        /// <param name="propInfo"></param>
        /// <returns></returns>
        public static string DisplayName(this PropertyInfo propInfo)
        {
            string result = null;

            DisplayNameAttribute tempAttribute = propInfo.GetCustomAttributes(typeof(DisplayNameAttribute), true).Cast<DisplayNameAttribute>().SingleOrDefault();

            if (tempAttribute != null)
            {
                result = tempAttribute.DisplayName;
            }

            return result;
        }

        public static CustomDataFormat DataFormat(this PropertyInfo propInfo)
        {

            CustomStringFormatAttribute tempAttribute = propInfo.GetCustomAttributes(typeof(CustomStringFormatAttribute), true).Cast<CustomStringFormatAttribute>().SingleOrDefault();


            if (tempAttribute != null)
            {
                return new CustomDataFormat { CultureInfo = tempAttribute.CultureInfo, Format = tempAttribute.Format };
            }

            //foreach (var attr in propInfo.GetCustomAttributes(false))
            //{
            //    CustomStringFormatAttribute customAttr = (CustomStringFormatAttribute)attr;

            //    return new CustomDataFormat { CultureInfo = customAttr.CultureInfo, Format = customAttr.Format };
            //}

            return null;
        }
    }
}
