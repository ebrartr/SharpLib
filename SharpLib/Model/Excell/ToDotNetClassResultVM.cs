using System;
using System.Collections.Generic;
using System.Text;

namespace SharpLib.Model.Excell
{
    public class ToDotNetClassResultVM<T>
    {
        public List<T> ResultList { get; set; }

        public List<string> MessageList { get; set; }
    }
}
