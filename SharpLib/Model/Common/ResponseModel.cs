using System;
using System.Collections.Generic;
using System.Text;

namespace SharpLib.Model.Common
{
    public class ResponseModel<T>
    {
        public bool ProcessStatus { get; set; }
        public T Result { get; set; }
        public string Message { get; set; }
    }
}
