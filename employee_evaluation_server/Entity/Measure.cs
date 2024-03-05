using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Entity
{
    public partial class Measure
    {
        public int MeasureCode { get; set; }
        public string MeasureName { get; set; }
        public int MeasureWeight { get; set; }
        public int CategoryCode { get; set; }
    }
}
