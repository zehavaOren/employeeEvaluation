using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Entity
{
    public partial class MeasureGrade
    {
        public int GradeCode { get; set; }
        public string GradeDescription { get; set; }

    }
}
