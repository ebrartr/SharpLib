using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SharpLib.Model.Template
{
    public class TestModel1VM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        [DisplayName("Birt Date")]
        public DateTime BirthDate { get; set; }
    }
}
