using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Entity
{
    public partial class Job
    {
        public int JobCode { get; set; }
        public string JobDescription { get; set; }

    }
}
