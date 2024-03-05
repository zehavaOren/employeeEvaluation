using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Entity
{
    public partial class EmployeeEvaluatioStatus
    {
        public string EmployeeId { get; set; }
        public int StatusCode { get; set; }

    }
}
