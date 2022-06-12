using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace SharbLib.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CustomStringFormatAttribute : Attribute
    {
        public CultureInfo CultureInfo { get; set; }

        public string Format { get; set; }
    }
}