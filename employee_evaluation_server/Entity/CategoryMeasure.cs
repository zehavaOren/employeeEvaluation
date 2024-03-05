using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Entity
{
    public partial class CategoryMeasure
    {
        public int CategoryCode { get; set; }
        public string CategoryDescription { get; set; }

    }
}
