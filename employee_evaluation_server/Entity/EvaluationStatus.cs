using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Entity
{
    public partial class EvaluationStatus
    {
        public int StatusCode { get; set; }
        public string StatusDescription { get; set; }

    }
}
