using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Entity
{
    public partial class EmployeeData
    {
        public string EmployeeId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string JobName { get; set; }
        public string SchoolName { get; set; }
        public string SuperiorId { get; set; }
        public string? SupervisorName { get; set; }
        public int? CurrentYear { get; set; }
        public int? WeightedMeasureGrade { get; set; }
        public int? EvaluationStatusCode { get; set; }
        public string? StatusDescription { get; set; }    
    }
}
