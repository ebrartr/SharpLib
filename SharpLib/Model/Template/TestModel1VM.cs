using SharbLib.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SharpLib.Model.Template
{
    public class TestModel1VM
    {
        public int? Id { get; set; }

        public int Age { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }

        [DisplayName("Birth Date")]
        [CustomStringFormat(Format = "dd.MM.yyyy")]
        public string BirthDate { get; set; }

    }
}