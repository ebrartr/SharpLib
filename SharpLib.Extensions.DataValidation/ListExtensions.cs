using System;
using System.Collections.Generic;
using System.Text;

namespace SharpLib.Extensions.DataValidation
{
    public static class ListExtensions
    {
        /// <summary>
        /// If the Listis null retuns it with emty list => new List<T>()
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<T> IfNullSetEmpty<T>(this List<T> list)
        {
            if (list == null)
                list = new List<T>();

            return list;
        }
    }
}
