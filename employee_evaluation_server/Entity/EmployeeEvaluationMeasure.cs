using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Entity
{
    public partial class EmployeeEvaluationMeasure
    {
        public string EmployeeId { get; set; }
        public int EvaluationYear { get; set; }
        public int GradeCode { get; set; }
        public int MeasureGradeCode { get; set; }

    }
}
