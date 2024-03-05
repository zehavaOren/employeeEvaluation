using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Entity
{
    public partial class School
    {
        public int SchoolCode { get; set; }
        public string SchoolDescription { get; set; }
    }
}
